using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerPlayerManagement
    {
        // Global Variables
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance thisInstance => CommonCore.theInstance!;

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

            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
                // Tab Ticker Hooks
                thisServer.PlayersTab.tickerPlayerHook();                           // Ticker PlayerTab Hook

                // Update stats grids (these should be UI-thread safe)
                try
                {
                    StatFunctions.PopulatePlayerStatsGrid();
                    StatFunctions.PopulateWeaponStatsGrid();
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerPlayerManagement", $"Error updating stats grids: {ex.Message}");
                }
            });
        }
    }
}