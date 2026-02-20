using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;

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
        
        // Track if weapon config has been logged
        private static bool weaponConfigLogged = false;

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
                // Check for leaning violations (left and/or right)
                CheckLeaningViolations();
                
                // Check for weapon restrictions based on player count threshold
                CheckWeaponRestrictions();
                
                // Clean up disconnected players from weapon restriction tracking
                playerInstanceManager.CleanupDisconnectedPlayers();
            }

            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
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

        /// <summary>
        /// Check and enforce leaning restrictions (left and/or right)
        /// </summary>
        public static void CheckLeaningViolations()
        {
            // Check if either leaning check is enabled
            bool checkLeftLeaning = !thisInstance.gameAllowLeftLeaning;
            bool checkRightLeaning = !thisInstance.gameAllowRightLeaning;

            // Skip if both are allowed
            if (!checkLeftLeaning && !checkRightLeaning)
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
                    AppDebug.Log("LeaningCheck", $"Re-armed player in slot {player.Key}");
                }
                catch (Exception ex)
                {
                    AppDebug.Log("LeaningCheck", $"Error re-arming player {player.Key}: {ex.Message}");
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

            // Check all active players for leaning violations
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

                    // Lean status values:
                    // 0 = upright
                    // 2 = left leaning
                    // 4 = right leaning
                    
                    bool isLeftLeaning = leanStatus == 2;
                    bool isRightLeaning = leanStatus == 4;
                    
                    // Check for left leaning violation
                    if (checkLeftLeaning && isLeftLeaning)
                    {
                        // Disarm the player
                        ServerMemory.WriteMemoryDisarmPlayer(playerSlot);

                        // Track when to re-arm (10 seconds from now)
                        disarmedPlayers[playerSlot] = DateTime.Now.AddSeconds(10);

                        // Send chat message
                        string message = $"{DateTime.Now:HH:mm:ss} - {playerInfo.PlayerName} has been disarmed for 10 seconds - Left leaning is not allowed!";

                        SendLongMessage(message, 59);

                        AppDebug.Log("LeaningCheck", $"Disarmed {playerInfo.PlayerName} (slot {playerSlot}) for left leaning");
                    }
                    // Check for right leaning violation
                    else if (checkRightLeaning && isRightLeaning)
                    {
                        // Disarm the player
                        ServerMemory.WriteMemoryDisarmPlayer(playerSlot);

                        // Track when to re-arm (10 seconds from now)
                        disarmedPlayers[playerSlot] = DateTime.Now.AddSeconds(10);

                        // Send chat message
                        string message = $"{DateTime.Now:HH:mm:ss} - {playerInfo.PlayerName} has been disarmed for 10 seconds - Right leaning is not allowed!";

                        SendLongMessage(message, 59);

                        AppDebug.Log("LeaningCheck", $"Disarmed {playerInfo.PlayerName} (slot {playerSlot}) for right leaning");
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("LeaningCheck", $"Error checking lean status for player {playerSlot}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Check and enforce weapon restrictions based on player count threshold
        /// </summary>
        public static void CheckWeaponRestrictions()
        {
            // Log configuration once on first run
            if (!weaponConfigLogged)
            {
                playerInstanceManager.LogWeaponRestrictionConfig();
                weaponConfigLogged = true;
            }

            // Get current player count and threshold
            int currentPlayers = thisInstance.gameInfoNumPlayers;
            int threshold = thisInstance.gameFullWeaponThreshold;

            AppDebug.Log("WeaponRestrictionCheck", $"=== CheckWeaponRestrictions START === Players: {currentPlayers}, Threshold: {threshold}");

            // If player count is at or above threshold, all configured weapons are allowed
            if (currentPlayers >= threshold)
            {
                AppDebug.Log("WeaponRestrictionCheck", $"Player count ({currentPlayers}) >= threshold ({threshold}) - Full weapons enabled");
                
                // Re-arm any players who were previously disarmed
                playerInstanceManager.RearmAllDisarmedPlayers(currentPlayers, threshold);
                return; // Full weapons enabled
            }

            AppDebug.Log("WeaponRestrictionCheck", $"Player count ({currentPlayers}) < threshold ({threshold}) - Checking {playerInstance.PlayerList.Count} players for restricted weapons");

            // Below threshold - check each active player for restricted weapons
            int checkedCount = 0;
            foreach (var playerKvp in playerInstance.PlayerList.ToList())
            {
                int playerSlot = playerKvp.Key;
                var playerInfo = playerKvp.Value;

                try
                {
                    checkedCount++;
                    AppDebug.Log("WeaponRestrictionCheck", $"[{checkedCount}] Checking player: {playerInfo.PlayerName} (slot {playerSlot}), WeaponID: {playerInfo.SelectedWeaponID}, WeaponName: {playerInfo.SelectedWeaponName}");
                    
                    // Check this player's weapon restriction
                    playerInstanceManager.CheckWeaponRestriction(playerInfo);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("WeaponRestrictionCheck", $"ERROR checking weapons for player {playerSlot}: {ex.Message}");
                }
            }

            AppDebug.Log("WeaponRestrictionCheck", $"=== CheckWeaponRestrictions END === Checked {checkedCount} players");
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