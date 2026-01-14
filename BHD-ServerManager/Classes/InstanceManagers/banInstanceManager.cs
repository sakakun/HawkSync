using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Animation;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class banInstanceManager
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance instanceBans => CommonCore.instanceBans!;

        public static void LoadSettings()
        {

		}

	}
}
