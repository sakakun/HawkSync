using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.StatsManagement;

namespace BHD_RemoteClient.Classes.StatManagement
{
    public class remoteStatsManager : StatsInterface
    {
        public bool TestBabstatsConnection(string WebURL)
        {
            return CmdTestBabstatsConnection.ProcessCommand(WebURL);
        }
    }
}
