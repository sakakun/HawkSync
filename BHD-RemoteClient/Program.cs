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

        // Server Manager UI Object
        // Remote Client Object
        public static ServerManager? ServerManagerUI { get; set; }
        public static RemoteClient? theRemoteClient { get; set; }

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