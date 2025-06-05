using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NodeUptime
{
    public static class Constants
    {
        public static readonly IReadOnlyDictionary<string, string> HEADLESS_URLS =
            new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    // { "OdinRpc1", "https://odin-rpc-1.nine-chronicles.com/graphql" },
                    { "OdinRpc2", "https://odin-rpc-2.nine-chronicles.com/graphql" },
                    { "OdinRpc3", "https://odin-rpc-3.nine-chronicles.com/graphql" },
                    { "HeimdallRpc1", "https://heimdall-rpc-1.nine-chronicles.com/graphql" },
                    { "HeimdallRpc2", "https://heimdall-rpc-2.nine-chronicles.com/graphql" },
                    { "OdinValidator5", "https://odin-validator-5.nine-chronicles.com/graphql" },
                    {
                        "HeimdallValidator1",
                        "https://heimdall-validator-1.nine-chronicles.com/graphql"
                    },
                }
            );
    }
}
