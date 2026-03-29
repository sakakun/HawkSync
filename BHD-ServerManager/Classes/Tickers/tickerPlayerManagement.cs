using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Windows.Storage;

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

        private static readonly Dictionary<int, PlayerIdleState> playerIdleStates = new();
        
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

				// Check for Idle players and kick if enabled
                CheckForIdlePlayers();

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

                        chatInstanceManager.SendChatMessage(message, 3);

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

                        chatInstanceManager.SendChatMessage(message, 3);

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

        private class PlayerIdleState
        {
            public float PosX, PosY, PosZ;
            public float FacingYaw, FacingPitch;
            public int ShotsFired;
            public int Health;
            public DateTime LastActive;
        }

        public static void CheckForIdlePlayers()
        {
            if (!thisInstance.gameEnableKickIdle)
                return;

            int idleLimit = thisInstance.gameKickIdleTime;
            if (idleLimit == 0)
                return;

            var now = DateTime.Now;
            var toKick = new List<int>();

            foreach (var playerKvp in playerInstance.PlayerList.ToList())
            {
                int playerSlot = playerKvp.Key;
                var playerInfo = playerKvp.Value;

                // Read current state
                float posX = playerInfo.PosX;
                float posY = playerInfo.PosY;
                float posZ = playerInfo.PosZ;
                float yaw = playerInfo.FacingYaw;
                float pitch = playerInfo.FacingPitch;
                int shots = playerInfo.stat_TotalShotsFired;
                int health = playerInfo.PlayerHealth;

                AppDebug.Log("IdleCheck", $"Checking player {playerInfo.PlayerName} (slot {playerSlot}): Pos({posX}, {posY}, {posZ}), Facing({yaw}, {pitch}), ShotsFired: {shots}, Health: {health}");

                if (!playerIdleStates.TryGetValue(playerSlot, out var state))
                {
                    // First time tracking this player
                    playerIdleStates[playerSlot] = new PlayerIdleState
                    {
                        PosX = posX,
                        PosY = posY,
                        PosZ = posZ,
                        FacingYaw = yaw,
                        FacingPitch = pitch,
                        ShotsFired = shots,
                        Health = health,
                        LastActive = now
                    };
                    continue;
                }

                // Use a threshold to ignore floating-point noise
                const float movementThreshold = 0.01f;
                bool moved = Math.Abs(state.PosX - posX) > movementThreshold ||
                             Math.Abs(state.PosY - posY) > movementThreshold ||
                             Math.Abs(state.PosZ - posZ) > movementThreshold;
                bool turned = Math.Abs(state.FacingYaw - yaw) > movementThreshold ||
                              Math.Abs(state.FacingPitch - pitch) > movementThreshold;

                // If any activity, update state
                if (moved || turned || state.ShotsFired != shots || (health > 0 && state.Health <= 0) || (health <= 0 && state.Health > 0))
                {
                    state.PosX = posX;
                    state.PosY = posY;
                    state.PosZ = posZ;
                    state.FacingYaw = yaw;
                    state.FacingPitch = pitch;
                    state.ShotsFired = shots;
                    state.Health = health;
                    state.LastActive = now;
                    continue;
                }

                // If health is 0 for the whole idle period, treat as idle
                bool deadTooLong = health <= 0 && state.Health <= 0 && (now - state.LastActive).TotalSeconds >= idleLimit;
                // If alive and idle
                bool aliveAndIdle = health > 0 && (now - state.LastActive).TotalSeconds >= idleLimit;

                if (deadTooLong || aliveAndIdle)
                {
                    toKick.Add(playerSlot);
                }
            }

            // Kick idle players
            foreach (var slot in toKick)
            {
                if (playerInstance.PlayerList.TryGetValue(slot, out var playerInfo))
                {
                    playerInstanceManager.KickPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
				}
                playerIdleStates.Remove(slot);
            }

            // Clean up tracking for disconnected players
            var disconnected = playerIdleStates.Keys.Except(playerInstance.PlayerList.Keys).ToList();
            foreach (var slot in disconnected)
                playerIdleStates.Remove(slot);
        }
    }

}