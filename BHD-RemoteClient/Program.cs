using BHD_RemoteClient.Classes.GameManagement;
using BHD_RemoteClient.Classes.InstanceManagers;
using BHD_RemoteClient.Classes.RemoteFunctions;
using BHD_RemoteClient.Classes.StatManagement;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.StatsManagement;

namespace BHD_RemoteClient
{
    internal static class Program
    {
        public static ServerManager? ServerManagerUI { get; set; }
        public static RemoteClient? theRemoteClient { get; set; }

        [STAThread]
        static void Main()
        {
            // Initialize the Instance of the Application
            CommonCore.InitializeCore();

            // Setup the Instance Managers
            theInstanceManager.Implementation = new remoteTheInstanceManager();
            banInstanceManager.Implementation = new remoteBanInstanceManager();
            chatInstanceManagers.Implementation = new remoteChatInstanceManager();
            mapInstanceManager.Implementation = new remoteMapInstanceManager();
            adminInstanceManager.Implementation = new remoteAdminInstanceManager();

            StatsManager.Implementation = new remoteStatsManager();
            GameManager.Implementation = new remoteGameManager();

            // Ensure the correct namespace or class is used for ApplicationConfiguration
            Application.EnableVisualStyles(); // This is a common initialization call
            Application.SetCompatibleTextRenderingDefault(false); // Another common call    
            Application.Run(new LoginWindow());
        }

    }
}