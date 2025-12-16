using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NodeUptime
{
    public static class Constants
    {
        public static readonly IReadOnlyDictionary<string, string[]> VALIDATOR_URLS =
            new Dictionary<string, string[]>
            {
                { "odin", new[] { "https://odin-validator-5.nine-chronicles.com/graphql" } },
                { "heimdall", new[] { "https://heimdall-validator-1.nine-chronicles.com/graphql" } },
                { "thor", new[] { "https://thor-validator-1.nine-chronicles.com/graphql" } }
            };
    }
}
