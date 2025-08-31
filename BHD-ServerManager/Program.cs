using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.StatsManagement;
using System.Reflection;

namespace BHD_ServerManager
{
    public static class Program
    {

        // Server Manager UI Object
        public static ServerManager? ServerManagerUI { get; private set; }

        // Major Version Number
        public static string ApplicationVersion = "1.1.3";

        [STAThread]
        static void Main()
        {   

            // Encoding for legacy code pages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Initialize the Instance of the Application
            CommonCore.InitializeCore();

            // Setup the Instance Managers
            theInstanceManager.Implementation = new serverTheInstanceManager();
            banInstanceManager.Implementation = new serverBanInstanceManager();
            chatInstanceManagers.Implementation = new serverChatInterfaceManager();
            mapInstanceManager.Implementation = new serverMapInstanceManager();
            adminInstanceManager.Implementation = new serverAdminInstanceManager();

            StatsManager.Implementation = new serverStatsManager();
            GameManager.Implementation = new serverGameManager();

            ApplicationConfiguration.Initialize();
            ServerManagerUI = new ServerManager();

            Application.Run(ServerManagerUI);
        }
    }
}