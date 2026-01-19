using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerPlayerManagement
    {
        // Global Variables
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
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