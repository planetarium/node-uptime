using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NodeUptime
{
    /// <summary>
    /// Constants used throughout the application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Planet names
        /// </summary>
        public static class Planets
        {
            public const string ODIN = "ODIN";
            public const string HEIMDALL = "HEIMDALL";
        }

        /// <summary>
        /// Dictionary of JWT Headless URLs mapped by planet name
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> JWT_HEADLESS_URLS = 
            new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { Planets.ODIN, "https://odin-jwt.nine-chronicles.com/graphql" },
                    { Planets.HEIMDALL, "https://heimdall-jwt.nine-chronicles.com/graphql" }
                });
    }
}
