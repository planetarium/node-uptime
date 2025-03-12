using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NodeUptime.Options;

namespace NodeUptime
{
    public class HeadlessOptionsFixture
    {
        public IConfiguration Configuration { get; private set; }
        public IOptions<HeadlessOption> HeadlessOptions { get; private set; }

        public HeadlessOptionsFixture()
        {
            // Load configuration
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddEnvironmentVariables(prefix: "NodeUptime_")
                .Build();

            // Get headless options
            var headlessOptions = new HeadlessOption();
            Configuration.GetSection(HeadlessOption.SectionName).Bind(headlessOptions);
            HeadlessOptions = Microsoft.Extensions.Options.Options.Create(headlessOptions);
        }
    }
} 