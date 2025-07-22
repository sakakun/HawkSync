using BHD_SharedResources.Classes.InstanceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.StatsManagement
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
