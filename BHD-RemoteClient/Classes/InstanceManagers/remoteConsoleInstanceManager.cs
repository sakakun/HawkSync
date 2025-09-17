using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteConsoleInstanceManager : consoleInstanceInterface
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static consoleInstance instanceConsole => CommonCore.instanceConsole!;

        public void updateConsoleWindow(string AuthToken)
        {
            instanceConsole.ClientConsole = instanceConsole.AdminConsoles[AuthToken];
            AppDebug.Log("updateConsoleWindow", "Client Side Console Updated");
        }

    }
    
}
