using System.Collections.Generic;

namespace NodeUptime.Options
{
    public class PagerDutyOption
    {
        public const string SectionName = "PagerDuty";

        public Dictionary<string, string> RoutingKeys { get; set; } =
            new Dictionary<string, string>();

        public bool Enabled { get; set; } = false;
    }
}
