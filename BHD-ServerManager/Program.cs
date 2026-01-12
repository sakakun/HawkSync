using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using BHD_ServerManager.Classes.SupportClasses;

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
            DatabaseManager.Initialize();

			ApplicationConfiguration.Initialize();
            ServerManagerUI = new ServerManager();

            Application.Run(ServerManagerUI);
        }
    }
}