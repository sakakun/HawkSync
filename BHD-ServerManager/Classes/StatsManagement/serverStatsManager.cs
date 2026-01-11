using BHD_ServerManager.Classes.StatsManagement;

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
