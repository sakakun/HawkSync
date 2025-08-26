using BHD_RemoteClient.Classes.StatManagement;
using BHD_RemoteClient.Forms;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_RemoteClient.Classes.Tickers
{
    public class tickerServerManager
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        public static void runTicker()
        {
            DateTime currentTime = DateTime.Now;
            if (!(DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) < 0))
                return;

            // UI updates that should always run
            SafeInvoke(thisServer, () =>
            {
                // Server Status and Buttons
                thisServer.functionEvent_serverStatus();

                // --- UI Update Hooks ---
                thisServer.ProfileTab.tickerProfileTabHook();                                   // Toggle Profile Lock based on server status
                thisServer.ServerTab.tickerServerHook();                                        // Toggle Server Lock based on server status
                thisServer.MapsTab.tickerMapsHook();                                            // Toggle Maps Lock based on server status
                thisServer.PlayersTab.tickerPlayerHook();                                       // Ticker for Players
                thisServer.ChatTab.ChatTickerHook();
                thisServer.BanTab.BanTickerHook();                                             // Update Bans Tab
                thisServer.StatsTab.StatsTickerHook();                                         // Update Stats Tab
                thisServer.AdminTab.AdminsTickerHook();                                         // Update Admins Tab
            });

            theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
            theInstance.instanceLastUpdateTime = currentTime;
        }

    }
}