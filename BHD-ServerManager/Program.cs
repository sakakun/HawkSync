using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;

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
                CommonCore.Shutdown().Wait();
            }
		}
	}
}