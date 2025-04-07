using NodeUptime.Client;

namespace NodeUptime.BlockTests
{
    public class BlockTimestampTests : IClassFixture<HeadlessOptionsFixture>
    {
        private readonly HeadlessOptionsFixture _fixture;

        public BlockTimestampTests(HeadlessOptionsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("OdinRpc1", "Odin")]
        [InlineData("HeimdallRpc1", "Heimdall")]
        [InlineData("OdinRpc2", "Odin")]
        [InlineData("HeimdallRpc2", "Heimdall")]
        [InlineData("OdinValidator5", "Odin")]
        [InlineData("HeimdallValidator1", "Heimdall")]
        [InlineData("OdinEksRpc1", "Odin")]
        [InlineData("HeimdallEksRpc1", "Heimdall")]
        public async Task Block_Timestamp_Should_Be_Recent(string headlessKey, string routingKey)
        {
            // Arrange
            var headlessUrl = Constants.HEADLESS_URLS[headlessKey];
            var headlessOptions = _fixture.HeadlessOptions.Value;

            var client = new HeadlessGQLClient(
                new Uri(headlessUrl),
                headlessOptions.JwtIssuer,
                headlessOptions.JwtSecretKey
            );

            try
            {
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

                        Assert.True(isValid, errorMessage);
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
                    Assert.True(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                await _fixture.PagerDutyService.SendAlertAsync(
                    headlessKey,
                    routingKey,
                    $"Error checking {headlessKey} block timestamp: {ex.Message}"
                );
                throw;
            }
        }
    }
}
