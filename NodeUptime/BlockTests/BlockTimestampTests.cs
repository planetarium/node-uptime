using System.Text.RegularExpressions;
using NodeUptime.Client;

namespace NodeUptime.BlockTests
{
    public class BlockTimestampTests : IClassFixture<HeadlessOptionsFixture>
    {
        private readonly HeadlessOptionsFixture _fixture;

        private readonly IDictionary<string, string[]> _planetRegistry;

        public BlockTimestampTests(HeadlessOptionsFixture fixture)
        {
            _fixture = fixture;
            _planetRegistry = new PlanetRegistryClient().Nodes;
            foreach (var planet in _planetRegistry)
            {
                if (Constants.VALIDATOR_URLS.ContainsKey(planet.Key))
                {
                    _planetRegistry[planet.Key] = _planetRegistry[planet.Key]
                        .Concat(Constants.VALIDATOR_URLS[planet.Key]).ToArray();
                }
            }
        }

        [Fact]
        public async Task Block_Timestamp_Should_Be_Recent()
        {
            var firstToUpper = (string str) => string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
            var headlessUrlToKey = (string url) => string.Concat(
                Regex.Match(url, @"https://(.*)\.nine-chronicles\.com/graphql")
                    .Groups[1].Value.Split('-')
                    .Select(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase));

            var planetNodePairs = _planetRegistry.SelectMany(planet => planet.Value.Select(url => (planet: planet.Key, url)));
            var tasks = planetNodePairs.Select(pair => TestNodeAsync(headlessUrlToKey(pair.url), firstToUpper(pair.planet), pair.url));
            var results = await Task.WhenAll(tasks);

            var failures = results.Where(r => !string.IsNullOrEmpty(r)).ToList();

            if (failures.Count > 0)
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        private async Task<string> TestNodeAsync(string headlessKey, string routingKey, string headlessUrl)
        {
            try
            {
                var headlessOptions = _fixture.HeadlessOptions.Value;

                var client = new HeadlessGQLClient(
                    new Uri(headlessUrl),
                    headlessOptions.JwtIssuer,
                    headlessOptions.JwtSecretKey
                );

                var (response, _) = await client.GetBlockTimestampAsync();

                if (
                    response?.ChainQuery?.BlockQuery?.Blocks != null
                    && response.ChainQuery.BlockQuery.Blocks.Count > 0
                    && !string.IsNullOrEmpty(response.ChainQuery.BlockQuery.Blocks[0].Timestamp)
                )
                {
                    var blockTimestamp = DateTime.Parse(
                        response.ChainQuery.BlockQuery.Blocks[0].Timestamp
                    ).ToUniversalTime();
                    var currentTime = DateTime.UtcNow;

                    var timeDifference = (currentTime - blockTimestamp).TotalSeconds;

                    var isValid = timeDifference < 300;

                    if (!isValid)
                    {
                        var errorMessage =
                            $"Block timestamp for {headlessKey} is too old. Difference: {timeDifference} seconds.";

                        await _fixture.PagerDutyService.SendAlertAsync(
                            headlessKey,
                            routingKey,
                            errorMessage
                        );

                        return errorMessage;
                    }
                    else
                    {
                        var resolveMessage =
                            $"Block timestamp for {headlessKey} is now valid. Difference: {timeDifference} seconds.";
                        await _fixture.PagerDutyService.ResolveAlertAsync(
                            headlessKey,
                            routingKey,
                            resolveMessage
                        );
                    }
                }
                else
                {
                    var errorMessage = $"No valid block data received for {headlessKey}";
                    await _fixture.PagerDutyService.SendAlertAsync(
                        headlessKey,
                        routingKey,
                        errorMessage
                    );
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                await _fixture.PagerDutyService.SendAlertAsync(
                    headlessKey,
                    routingKey,
                    $"Error checking {headlessKey} block timestamp: {ex.Message}"
                );
                return $"Error checking {headlessKey} block timestamp: {ex.Message}";
            }

            return string.Empty;
        }
    }
}
