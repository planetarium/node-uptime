using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodeUptime.Client;
using Xunit;
using Microsoft.Extensions.Options;

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
        [InlineData(Constants.Planets.ODIN)]
        [InlineData(Constants.Planets.HEIMDALL)]
        public async Task Block_Timestamp_Should_Be_Recent(string planet)
        {
            // Arrange
            var headlessUrl = Constants.JWT_HEADLESS_URLS[planet];
            var headlessOptions = _fixture.HeadlessOptions.Value;

            var client = new HeadlessGQLClient(
                new Uri(headlessUrl),
                headlessOptions.JwtIssuer,
                headlessOptions.JwtSecretKey
            );

            // Act
            var (response, _) = await client.GetBlockTimestampAsync();

            // The timestamp from the API is in ISO 8601 format
            var blockTimestamp = DateTime.Parse(response.ChainQuery.BlockQuery.Blocks[0].Timestamp);
            var currentTime = DateTime.UtcNow;

            // Calculate the difference in seconds
            var timeDifference = (currentTime - blockTimestamp).TotalSeconds;

            // Assert
            // Check if the block timestamp is less than 300 seconds (5 minutes) old
            Assert.True(timeDifference < 300,
                $"Block timestamp for {planet} is too old. Difference: {timeDifference} seconds.");

            // Output the actual difference for informational purposes
            Console.WriteLine($"Time difference for {planet}: {timeDifference} seconds");
        }
    }
}
