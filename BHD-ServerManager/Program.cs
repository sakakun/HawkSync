using HawkSyncShared;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using BHD_ServerManager.API;

namespace BHD_ServerManager
{
	public static class Program
	{

		// Server Manager UI Object
		public static ServerManagerUI? ServerManagerUI { get; set; } 

		[STAThread]
		static void Main()
		{   
			ApplicationConfiguration.Initialize();
			// Encoding for legacy code pages
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			try { 
				// Initialize the Instance of the Application
				DatabaseManager.Initialize();
				
				// Migrate chat log timestamps from TEXT to INTEGER (one-time operation)
				DatabaseManager.MigrateChatLogsTimestamps();
				
				CommonCore.InitializeCore();
			
				ServerManagerUI = new ServerManagerUI();
				Application.Run(ServerManagerUI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fatal error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cleanup all core infrastructure
                APICore.Shutdown().Wait();
            }
		}
	}
}