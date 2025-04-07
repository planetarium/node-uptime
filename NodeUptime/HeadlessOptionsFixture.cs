using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NodeUptime.Options;
using NodeUptime.Services;

namespace NodeUptime
{
    public class HeadlessOptionsFixture
    {
        public IConfiguration Configuration { get; private set; }
        public IOptions<HeadlessOption> HeadlessOptions { get; private set; }
        public IOptions<PagerDutyOption> PagerDutyOptions { get; private set; }
        public PagerDutyService PagerDutyService { get; private set; }

        public HeadlessOptionsFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddEnvironmentVariables(prefix: "NodeUptime_")
                .Build();

            var headlessOptions = new HeadlessOption();
            Configuration.GetSection(HeadlessOption.SectionName).Bind(headlessOptions);
            HeadlessOptions = Microsoft.Extensions.Options.Options.Create(headlessOptions);

            var pagerDutyOptions = new PagerDutyOption();
            Configuration.GetSection(PagerDutyOption.SectionName).Bind(pagerDutyOptions);
            PagerDutyOptions = Microsoft.Extensions.Options.Options.Create(pagerDutyOptions);

            PagerDutyService = new PagerDutyService(PagerDutyOptions);
        }
    }
}
