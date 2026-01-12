using BHD_ServerManager.Classes.StatsManagement;

namespace BHD_ServerManager.Classes.StatsManagement
{
    public static class StatsManager
    {
        public static bool TestBabstatsConnection(string WebURL)
        {
            return StatFunctions.TestBabstatsConnectionAsync(WebURL).GetAwaiter().GetResult();
        }
    }
}
