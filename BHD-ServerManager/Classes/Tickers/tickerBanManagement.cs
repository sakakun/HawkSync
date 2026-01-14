using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        // This method runs the ticker for ban management tasks.
        public static void runTicker()
        {
            // Always marshal to UI thread for UI updates
            SafeInvoke(thisServer, () =>
            {
                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // Only check and punt bans if server is ONLINE
                    if (thisInstance.instanceStatus == InstanceStatus.ONLINE)
                    {
						// Server Running - Check for Bans
					}
				}
                else
                {
                    AppDebug.Log("tickerBanManagement", "Server process is not attached. Ticker Skipping.");
                }

            });
        }

    }
}