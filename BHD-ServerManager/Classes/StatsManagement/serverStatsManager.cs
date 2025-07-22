using BHD_SharedResources.Classes.StatsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.StatsManagement
{
    public class serverStatsManager : StatsInterface
    {
        public bool TestBabstatsConnection(string WebURL)
        {
            return StatFunctions.TestBabstatsConnectionAsync(WebURL).GetAwaiter().GetResult();
        }
    }
}
