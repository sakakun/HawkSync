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

			// Encoding for legacy code pages
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

			// Initialize the Instance of the Application
			CommonCore.InitializeCore();
			DatabaseManager.Initialize();

			ApplicationConfiguration.Initialize();
			ServerManagerUI = new ServerManagerUI();

			Application.Run(ServerManagerUI);
		}
	}
}