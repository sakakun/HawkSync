﻿namespace BHD_SharedResources.Classes.StatsManagement
{
    public static class StatsManager
    {
        public static StatsInterface Implementation { get; set; }

        public static bool TestBabstatsConnection(string WebURL)
        {
            return Implementation.TestBabstatsConnection(WebURL);
        }
    }
}
