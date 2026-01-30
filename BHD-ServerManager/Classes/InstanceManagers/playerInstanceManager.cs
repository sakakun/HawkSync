using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    /// <summary>
    /// Manager for player-related operations (kick, kill, ban, etc.)
    /// </summary>
    public static class playerInstanceManager
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;

        private static playerInstance playerInstance => CommonCore.instancePlayers!;

		// ================================================================================
		// PLAYER VALIDATION
		// ================================================================================

		/// <summary>
		/// Validate that server is online and player exists
		/// </summary>
		private static (bool isValid, string errorMessage) ValidatePlayerOperation(int playerSlot, string playerName)
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return (false, "Server is offline. Player operations are not available.");

            if (playerSlot < 1 || playerSlot > 50)
                return (false, $"Invalid player slot: {playerSlot}. Must be between 1 and 50.");

            if (string.IsNullOrWhiteSpace(playerName))
                return (false, "Player name cannot be empty.");

            if (!playerInstance.PlayerList.ContainsKey(playerSlot))
                return (false, $"Player slot {playerSlot} is empty.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Validate server is online (for operations that don't require a player)
        /// </summary>
        private static (bool isValid, string errorMessage) ValidateServerOnline()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return (false, "Server is offline.");

            return (true, string.Empty);
        }

        // ================================================================================
        // BASIC PLAYER ACTIONS
        // ================================================================================

        /// <summary>
        /// Arm a player with their default weapons
        /// </summary>
        public static OperationResult ArmPlayer(int playerSlot, string playerName)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                ServerMemory.WriteMemoryArmPlayer(playerSlot);

                AppDebug.Log("playerInstanceManager", $"Player {playerName} (slot {playerSlot}) has been armed");
                return new OperationResult(true, $"Player {playerName} has been armed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error arming player: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Disarm a player (remove all weapons)
        /// </summary>
        public static OperationResult DisarmPlayer(int playerSlot, string playerName)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                ServerMemory.WriteMemoryDisarmPlayer(playerSlot);

                AppDebug.Log("playerInstanceManager", $"Player {playerName} (slot {playerSlot}) has been disarmed");
                return new OperationResult(true, $"Player {playerName} has been disarmed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error disarming player: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Kill a player
        /// </summary>
        public static OperationResult KillPlayer(int playerSlot, string playerName)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                ServerMemory.WriteMemoryKillPlayer(playerSlot);

                AppDebug.Log("playerInstanceManager", $"Player {playerName} (slot {playerSlot}) has been killed");
                return new OperationResult(true, $"Player {playerName} has been killed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error killing player: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Kick a player from the server
        /// </summary>
        public static OperationResult KickPlayer(int playerSlot, string playerName)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");

                AppDebug.Log("playerInstanceManager", $"Player {playerName} (slot {playerSlot}) has been kicked");
                return new OperationResult(true, $"Player {playerName} has been kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error kicking player: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Send a warning message to a player
        /// </summary>
        public static OperationResult WarnPlayer(int playerSlot, string playerName, string warningMessage)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                if (string.IsNullOrWhiteSpace(warningMessage))
                    return new OperationResult(false, "Warning message cannot be empty.");

                string fullMessage = $"{playerName}, {warningMessage}";
                ServerMemory.WriteMemorySendChatMessage(1, fullMessage);

                AppDebug.Log("playerInstanceManager", $"Warning sent to player {playerName}: {warningMessage}");
                return new OperationResult(true, $"Warning sent to {playerName}.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error warning player: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Toggle god mode for a player
        /// </summary>
        public static OperationResult ToggleGodMode(int playerSlot, string playerName, bool enable)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                int health = enable ? 9999 : 100;
                ServerMemory.WriteMemoryTogglePlayerGodMode(playerSlot, health);

                AppDebug.Log("playerInstanceManager", $"God mode {(enable ? "enabled" : "disabled")} for player {playerName}");
                return new OperationResult(true, $"God mode {(enable ? "enabled" : "disabled")} for {playerName}.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error toggling god mode: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // TEAM MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Switch a player to the opposite team for the next map
        /// </summary>
        public static OperationResult SwitchPlayerTeam(int playerSlot, string playerName, int currentTeam)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                // Check if already queued for team switch
                var existing = playerInstance.PlayerChangeTeamList.FirstOrDefault(p => p.slotNum == playerSlot);
                
                if (existing != null)
                {
                    // Undo the team switch
                    playerInstance.PlayerChangeTeamList.Remove(existing);
                    AppDebug.Log("playerInstanceManager", $"Team switch undone for player {playerName}");
                    return new OperationResult(true, $"Team switch for {playerName} has been undone.");
                }

                // Determine new team
                int newTeam = currentTeam switch
                {
                    1 => 2, // Blue to Red
                    2 => 1, // Red to Blue
                    _ => currentTeam
                };

                if (newTeam == currentTeam)
                {
                    return new OperationResult(false, $"Player {playerName} is not on a valid team for switching.");
                }

                // Queue team switch for next map
                playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                {
                    slotNum = playerSlot,
                    Team = newTeam
                });

                string teamName = newTeam == 1 ? "Blue" : "Red";
                AppDebug.Log("playerInstanceManager", $"Player {playerName} queued for team switch to {teamName}");
                return new OperationResult(true, $"Player {playerName} will be switched to {teamName} team for the next map.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error switching player team: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // BAN OPERATIONS
        // ================================================================================

        /// <summary>
        /// Ban a player by name only
        /// </summary>
        public static OperationResult BanPlayerByName(string playerName, int playerSlot)
        {
            try
            {
                var (isValid, errorMessage) = ValidateServerOnline();
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                // Add ban via banInstanceManager (CORRECTED METHOD NAME)
                var banResult = banInstanceManager.AddBlacklistNameRecord(
                    playerName: playerName,
                    banDate: DateTime.Now,
                    expireDate: null,
                    recordType: banInstanceRecordType.Permanent,
                    notes: "Banned from PlayerCard context menu",
                    associatedIPID: 0
                );

                if (!banResult.Success)
                    return banResult;

                // Kick the player if they're online
                if (playerSlot > 0 && playerInstance.PlayerList.ContainsKey(playerSlot))
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");
                }

                AppDebug.Log("playerInstanceManager", $"Player {playerName} banned by name and kicked");
                return new OperationResult(true, $"Player {playerName} has been banned by name and kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error banning player by name: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Ban a player by IP address only (async for NetLimiter integration)
        /// </summary>
        public static async Task<OperationResult> BanPlayerByIPAsync(IPAddress ipAddress, string playerName, int playerSlot)
        {
            try
            {
                var (isValid, errorMessage) = ValidateServerOnline();
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                if (ipAddress == null)
                    return new OperationResult(false, "IP address cannot be null.");

                // Add ban via banInstanceManager (CORRECTED METHOD NAME)
                var banResult = banInstanceManager.AddBlacklistIPRecord(
                    ipAddress: ipAddress,
                    subnetMask: 32,
                    banDate: DateTime.Now,
                    expireDate: null,
                    recordType: banInstanceRecordType.Permanent,
                    notes: "Banned from PlayerCard context menu",
                    associatedNameID: 0
                );

                if (!banResult.Success)
                    return banResult;

                // Add to NetLimiter if enabled
                if (theInstance.netLimiterEnabled && !string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                {
                    try
                    {
                        await NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress.ToString(), 32);
                        AppDebug.Log("playerInstanceManager", $"Added IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'");
                    }
                    catch (Exception nlEx)
                    {
                        AppDebug.Log("playerInstanceManager", $"Warning: NetLimiter update failed: {nlEx.Message}");
                        // Continue anyway - ban was successful
                    }
                }

                // Kick the player if they're online
                if (playerSlot > 0 && playerInstance.PlayerList.ContainsKey(playerSlot))
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");
                }

                AppDebug.Log("playerInstanceManager", $"Player {playerName} ({ipAddress}) banned by IP and kicked");
                return new OperationResult(true, $"Player {playerName} has been banned by IP and kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error banning player by IP: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Ban a player by both name and IP address (async for NetLimiter integration)
        /// </summary>
        public static async Task<OperationResult> BanPlayerByBothAsync(string playerName, IPAddress ipAddress, int playerSlot)
        {
            try
            {
                var (isValid, errorMessage) = ValidateServerOnline();
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (ipAddress == null)
                    return new OperationResult(false, "IP address cannot be null.");

                // Add both bans via banInstanceManager (CORRECTED METHOD NAME)
                var banResult = banInstanceManager.AddBlacklistBothRecords(
                    playerName: playerName,
                    ipAddress: ipAddress,
                    subnetMask: 32,
                    banDate: DateTime.Now,
                    expireDate: null,
                    recordType: banInstanceRecordType.Permanent,
                    notes: "Banned from PlayerCard context menu"
                );

                if (!banResult.Success)
                    return new OperationResult(false, banResult.Message);

                // Add to NetLimiter if enabled
                if (theInstance.netLimiterEnabled && !string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                {
                    try
                    {
                        await NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress.ToString(), 32);
                        AppDebug.Log("playerInstanceManager", $"Added IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'");
                    }
                    catch (Exception nlEx)
                    {
                        AppDebug.Log("playerInstanceManager", $"Warning: NetLimiter update failed: {nlEx.Message}");
                        // Continue anyway - ban was successful
                    }
                }

                // Kick the player if they're online
                if (playerSlot > 0 && playerInstance.PlayerList.ContainsKey(playerSlot))
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");
                }

                AppDebug.Log("playerInstanceManager", $"Player {playerName} ({ipAddress}) banned by name and IP, then kicked");
                return new OperationResult(true, $"Player {playerName} has been banned by name and IP, then kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error banning player by both: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // BULK OPERATIONS
        // ================================================================================

        /// <summary>
        /// Kick all players from the server
        /// </summary>
        public static OperationResult KickAllPlayers()
        {
            try
            {
                var (isValid, errorMessage) = ValidateServerOnline();
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                int kickedCount = 0;
                foreach (var player in playerInstance.PlayerList.Values.ToList())
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {player.PlayerSlot}");
                    kickedCount++;
                }

                AppDebug.Log("playerInstanceManager", $"Kicked all players ({kickedCount} total)");
                return new OperationResult(true, $"Kicked {kickedCount} player(s) from the server.", kickedCount);
            }
            catch (Exception ex)
            {
                AppDebug.Log("playerInstanceManager", $"Error kicking all players: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }
    }
}