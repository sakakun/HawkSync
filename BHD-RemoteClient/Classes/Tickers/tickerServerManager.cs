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
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static statInstance instanceStats => CommonCore.instanceStats!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;
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

            InstanceStatus status = theInstance.instanceStatus;
            int maxSlots = theInstance.gameMaxSlots;
            List<playerObject> players = theInstance.playerList.Values.ToList();

            DateTime currentTime = DateTime.Now;
            if (!(DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) < 0))
                return;

            // UI updates that should always run
            SafeInvoke(thisServer, () =>
            {
                // --- UI Update Hooks ---
                thisServer.ProfileTab.tickerProfileTabHook();                                   // Toggle Profile Lock based on server status
                thisServer.PlayersTab.tickerPlayerHook();                                       // Ticker for Players
                thisServer.ChatTab.ChatTickerHook();
                thisServer.ServerTab.tickerServerHook();                                        // Toggle Server Lock based on server status
                thisServer.MapsTab.tickerMapsHook();                                            // Toggle Maps Lock based on server status
                thisServer.AdminTab.AdminsTickerHook();                                         // Update Admins Tab
            });


            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
                // Gather data off the UI thread
                bool isAttached = GameManager.ReadMemoryIsProcessAttached();

                // Server Status and Buttons
                thisServer.functionEvent_serverStatus();

                // Server Settings Refresh Tasks
                theInstanceManager.HighlightDifferences();

                // Chat Messages Refresh Tasks
                chatInstanceManagers.UpdateChatMessagesGrid();

                // Ban Management Refresh Tasks
                banInstanceManager.UpdateBannedTables();

                // Stats Refresh Tasks
                StatFunctions.PopulatePlayerStatsGrid();
                StatFunctions.PopulateWeaponStatsGrid();

            });

            theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
            theInstance.instanceLastUpdateTime = currentTime;
        }

    }
}