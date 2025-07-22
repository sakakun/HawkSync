using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
