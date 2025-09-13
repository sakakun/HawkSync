using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverConsoleInstanceManager : consoleInstanceInterface
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        void consoleInstanceInterface.updateConsoleWindow(string AuthToken)
        {
            throw new NotImplementedException();
        }
    }
}
