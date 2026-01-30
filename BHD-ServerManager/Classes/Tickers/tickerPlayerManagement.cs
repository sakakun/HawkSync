using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System.Threading.Channels;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerPlayerManagement
    {
        // Global Variables
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

        // Track disarmed players and their re-arm time
        private static Dictionary<int, DateTime> disarmedPlayers = new Dictionary<int, DateTime>();

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

            if (thisInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                // Check for left leaning violations
                CheckLeftLeaningViolations();
            }

            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
                // Tab Ticker Hooks
                thisServer.PlayersTab.tickerPlayerHook();                           // Ticker PlayerTab Hook

                // Update stats grids (these should be UI-thread safe)
                try
                {
                    statsInstanceManager.PopulatePlayerStatsGrid();
                    statsInstanceManager.PopulateWeaponStatsGrid();
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerPlayerManagement", $"Error updating stats grids: {ex.Message}");
                }
            });
        }

        public static void CheckLeftLeaningViolations()
        {
            // Only check if left leaning is disabled
            if (thisInstance.gameAllowLeftLeaning)
            {
                return;
            }

            // Re-arm players whose disarm time has expired
            var playersToRearm = disarmedPlayers.Where(kvp => DateTime.Now >= kvp.Value).ToList();
            foreach (var player in playersToRearm)
            {
                try
                {
                    ServerMemory.WriteMemoryArmPlayer(player.Key);
                    disarmedPlayers.Remove(player.Key);
                    AppDebug.Log("LeftLeaningCheck", $"Re-armed player in slot {player.Key}");
                }
                catch (Exception ex)
                {
                    AppDebug.Log("LeftLeaningCheck", $"Error re-arming player {player.Key}: {ex.Message}");
                }
            }

            // Remove disconnected players from tracking
            var disconnectedPlayers = disarmedPlayers.Keys
                .Where(slot => !playerInstance.PlayerList.ContainsKey(slot))
                .ToList();
            
            foreach (var slot in disconnectedPlayers)
            {
                disarmedPlayers.Remove(slot);
            }

            // Check all active players for left leaning
            foreach (var playerKvp in playerInstance.PlayerList.ToList())
            {
                int playerSlot = playerKvp.Key;
                var playerInfo = playerKvp.Value;

                // Skip if already disarmed
                if (disarmedPlayers.ContainsKey(playerSlot))
                {
                    continue;
                }

                try
                {
                    int leanStatus = ServerMemory.ReadMemoryPlayerLeaningStatus(playerSlot);

                    // 2 = left leaning
                    if (leanStatus == 2)
                    {
                        // Disarm the player
                        ServerMemory.WriteMemoryDisarmPlayer(playerSlot);

                        // Track when to re-arm (10 seconds from now)
                        disarmedPlayers[playerSlot] = DateTime.Now.AddSeconds(10);

                        // Send chat message
                        string message = $"{DateTime.Now.ToString("HH:mm:ss")} - {playerInfo.PlayerName} has been disarmed for 10 seconds - Left leaning is not allowed!";

                        SendLongMessage(message, 59);

                        AppDebug.Log("LeftLeaningCheck", $"Disarmed {playerInfo.PlayerName} (slot {playerSlot}) for left leaning");
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("LeftLeaningCheck", $"Error checking lean status for player {playerSlot}: {ex.Message}");
                }
            }
        }
        private static void SendLongMessage(string message, int maxLength = 59)
        {
            if (message.Length <= maxLength)
            {
                ServerMemory.WriteMemorySendChatMessage(1, message);
                return;
            }

            for (int i = 0; i < message.Length; i += maxLength)
            {
                
                int remainingLength = message.Length - i;
                int chunkLength = Math.Min(maxLength, remainingLength);
        
                // Try to find a space to break at (word boundary)
                if (chunkLength == maxLength && i + maxLength < message.Length)
                {
                    int lastSpace = message.LastIndexOf(' ', i + maxLength, maxLength);
                    if (lastSpace > i)
                    {
                        chunkLength = lastSpace - i;
                    }
                }
        
                string chunk = message.Substring(i, chunkLength).Trim();
                AppDebug.Log("SendLongMessage", $"Message being sent: {chunk}");
                ServerMemory.WriteMemorySendChatMessage(1, chunk);
                Thread.Sleep(1000); // Delay between chunks
            }
        }

    }
}