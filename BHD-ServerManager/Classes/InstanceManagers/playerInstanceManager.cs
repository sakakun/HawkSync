using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using HawkSyncShared.Instances;
using HawkSyncShared.DTOs.tabPlayers;
using BHD_ServerManager.Classes.Services.NetLimiter;
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

        // Track players who have been disarmed for weapon restrictions
        private static Dictionary<int, WeaponDisarmInfo> weaponDisarmedPlayers = new Dictionary<int, WeaponDisarmInfo>();
        private const int WEAPON_DISARM_DURATION_SECONDS = 5;

        /// <summary>
        /// Tracks disarm information for a player
        /// </summary>
        private class WeaponDisarmInfo
        {
            public DateTime DisarmTime { get; set; }
            public int RestrictedWeaponId { get; set; }
            public string RestrictedWeaponName { get; set; } = string.Empty;
            public bool MessageSent { get; set; }
        }

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
        // WEAPON RESTRICTION OPERATIONS
        // ================================================================================

        /// <summary>
        /// Check if a player's weapon is restricted based on player count threshold
        /// </summary>
        public static void CheckWeaponRestriction(PlayerObject playerInfo)
        {
            try
            {
                // Get current player count and threshold
                int currentPlayers = theInstance.gameInfoNumPlayers;
                int threshold = theInstance.gameFullWeaponThreshold;

                // Get weapon ID
                int weaponId = playerInfo.SelectedWeaponID;

                // Skip check for knife and medkit (always allowed)
                if (weaponId == (int)WeaponStack.WPN_KNIFE || weaponId == (int)WeaponStack.WPN_MEDPACK)
                {
                    return;
                }

                // Check if weapon is on the restricted list
                WeaponRestrictionResult restriction = CheckWeaponRestrictionStatus(weaponId);

                // LOGIC: Below threshold, check if weapon is allowed
                // Green (IsFullyAllowed) = Always allowed
                // Gold (IsLimitedAllowed) = Allowed below threshold
                // Gray (neither) = RESTRICTED
                bool isWeaponRestricted = !restriction.IsFullyAllowed && !restriction.IsLimitedAllowed;

                // Check if player is currently in disarm cycle
                if (weaponDisarmedPlayers.TryGetValue(playerInfo.PlayerSlot, out WeaponDisarmInfo? disarmInfo))
                {
                    // Player is in disarm cycle
                    TimeSpan timeSinceDisarm = DateTime.Now - disarmInfo.DisarmTime;

                    // Check if 5 seconds have passed
                    if (timeSinceDisarm.TotalSeconds >= WEAPON_DISARM_DURATION_SECONDS)
                    {
                        // 5 seconds passed - check current weapon
                        if (isWeaponRestricted && weaponId == disarmInfo.RestrictedWeaponId)
                        {
                            // Still has restricted weapon - disarm again for another 5 seconds
                            disarmInfo.DisarmTime = DateTime.Now;
                            disarmInfo.MessageSent = false;
                            
                            DisarmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                            
                            string message = $"{DateTime.Now:HH:mm:ss} - {playerInfo.PlayerName} still has {disarmInfo.RestrictedWeaponName} - disarmed again (requires {threshold}+ players)";
                            ServerMemory.WriteMemorySendChatMessage(1, message);
                            
                            AppDebug.Log("CheckWeaponRestriction", 
                                $"Re-disarmed {playerInfo.PlayerName} (slot {playerInfo.PlayerSlot}) - " +
                                $"still has {disarmInfo.RestrictedWeaponName} after 5 seconds");
                        }
                        else
                        {
                            // Player switched to allowed weapon - re-arm them
                            weaponDisarmedPlayers.Remove(playerInfo.PlayerSlot);
                            
                            ArmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                            
                            string rearmMessage = $"{DateTime.Now:HH:mm:ss} - {playerInfo.PlayerName} - Re-armed with allowed weapon";
                            ServerMemory.WriteMemorySendChatMessage(1, rearmMessage);
                            
                            AppDebug.Log("CheckWeaponRestriction", 
                                $"Re-armed {playerInfo.PlayerName} (slot {playerInfo.PlayerSlot}) - " +
                                $"switched from {disarmInfo.RestrictedWeaponName} to {restriction.WeaponName}");
                        }
                    }
                    // else: Still within 5-second disarm window, do nothing
                    return;
                }

                // Player NOT in disarm cycle - check if weapon is restricted
                if (isWeaponRestricted)
                {
                    // Weapon is RESTRICTED (Gray) - start disarm cycle
                    string weaponName = playerInfo.SelectedWeaponName ?? restriction.WeaponName ?? "Unknown";
                    
                    weaponDisarmedPlayers[playerInfo.PlayerSlot] = new WeaponDisarmInfo
                    {
                        DisarmTime = DateTime.Now,
                        RestrictedWeaponId = weaponId,
                        RestrictedWeaponName = weaponName,
                        MessageSent = false
                    };

                    // Disarm the player
                    var result = DisarmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);

                    if (result.Success)
                    {
                        string message = $"{DateTime.Now:HH:mm:ss} - {playerInfo.PlayerName} disarmed for 5s - {weaponName} requires {threshold}+ players (Current: {currentPlayers})";
                        ServerMemory.WriteMemorySendChatMessage(1, message);

                        AppDebug.Log("CheckWeaponRestriction", 
                            $"Disarmed {playerInfo.PlayerName} (slot {playerInfo.PlayerSlot}) for 5s - " +
                            $"Weapon {weaponName} (ID: {weaponId}) not allowed with {currentPlayers} players");
                    }
                }
                // else: Weapon is allowed (Green or Gold), do nothing
            }
            catch (Exception ex)
            {
                AppDebug.Log("CheckWeaponRestriction", $"Error checking weapon restriction: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if a weapon is fully allowed, limited, or restricted
        /// </summary>
        private static WeaponRestrictionResult CheckWeaponRestrictionStatus(int weaponId)
        {
            return weaponId switch
            {
                // Pistols
                (int)WeaponStack.WPN_colt45 => new WeaponRestrictionResult(
                    theInstance.weaponColt45, 
                    theInstance.limitedWeaponColt45, 
                    "Colt .45"),
                
                (int)WeaponStack.WPN_M9Beretta => new WeaponRestrictionResult(
                    theInstance.weaponM9Beretta, 
                    theInstance.limitedWeaponM9Beretta, 
                    "M9 Beretta"),

                // Shotgun
                (int)WeaponStack.WPN_RemmingtonSG => new WeaponRestrictionResult(
                    theInstance.weaponShotgun, 
                    theInstance.limitedWeaponShotgun, 
                    "Shotgun"),

                // CAR-15 variants
                (int)WeaponStack.WPN_CAR15_AUTO or 
                (int)WeaponStack.WPN_CAR15_SEMI => new WeaponRestrictionResult(
                    theInstance.weaponCar15, 
                    theInstance.limitedWeaponCar15, 
                    "CAR-15"),

                (int)WeaponStack.WPN_CAR15_203_AUTO or 
                (int)WeaponStack.WPN_CAR15_203_SEMI or 
                (int)WeaponStack.WPN_CAR15_203_203 => new WeaponRestrictionResult(
                    theInstance.weaponCar15203, 
                    theInstance.limitedWeaponCar15203, 
                    "CAR-15 M203"),

                // M16 variants
                (int)WeaponStack.WPN_M16_Burst or 
                (int)WeaponStack.WPN_M16_SEMI => new WeaponRestrictionResult(
                    theInstance.weaponM16, 
                    theInstance.limitedWeaponM16, 
                    "M16"),

                (int)WeaponStack.WPN_M16_203_Burst or 
                (int)WeaponStack.WPN_M16_203_SEMI or 
                (int)WeaponStack.WPN_M16_203_203 => new WeaponRestrictionResult(
                    theInstance.weaponM16203, 
                    theInstance.limitedWeaponM16203, 
                    "M16 M203"),

                // Sniper rifles
                (int)WeaponStack.WPN_M21 => new WeaponRestrictionResult(
                    theInstance.weaponM21, 
                    theInstance.limitedWeaponM21, 
                    "M21"),

                (int)WeaponStack.WPN_M24 => new WeaponRestrictionResult(
                    theInstance.weaponM24, 
                    theInstance.limitedWeaponM24, 
                    "M24"),

                (int)WeaponStack.WPN_MCRT_300_TACTICAL => new WeaponRestrictionResult(
                    theInstance.weaponMCRT300, 
                    theInstance.limitedWeaponMCRT300, 
                    "McMillan .300"),

                (int)WeaponStack.WPN_Barrett => new WeaponRestrictionResult(
                    theInstance.weaponBarrett, 
                    theInstance.limitedWeaponBarrett, 
                    "Barrett"),

                (int)WeaponStack.WPN_PSG1 => new WeaponRestrictionResult(
                    theInstance.weaponPSG1, 
                    theInstance.limitedWeaponPSG1, 
                    "PSG-1"),

                // Machine guns
                (int)WeaponStack.WPN_SAW => new WeaponRestrictionResult(
                    theInstance.weaponSAW, 
                    theInstance.limitedWeaponSAW, 
                    "SAW"),

                (int)WeaponStack.WPN_M60 => new WeaponRestrictionResult(
                    theInstance.weaponM60, 
                    theInstance.limitedWeaponM60, 
                    "M60"),

                (int)WeaponStack.WPN_M240 => new WeaponRestrictionResult(
                    theInstance.weaponM240, 
                    theInstance.limitedWeaponM240, 
                    "M240"),

                (int)WeaponStack.WPN_MP5 => new WeaponRestrictionResult(
                    theInstance.weaponMP5, 
                    theInstance.limitedWeaponMP5, 
                    "MP5"),

                // G3 variants
                (int)WeaponStack.WPN_G3_Auto or 
                (int)WeaponStack.WPN_G3_SEMI => new WeaponRestrictionResult(
                    theInstance.weaponG3, 
                    theInstance.limitedWeaponG3, 
                    "G3"),

                // G36 variants
                (int)WeaponStack.WPN_G36_AUTO or 
                (int)WeaponStack.WPN_G36_SEMI => new WeaponRestrictionResult(
                    theInstance.weaponG36, 
                    theInstance.limitedWeaponG36, 
                    "G36"),

                // Grenades and explosives
                (int)WeaponStack.WPN_XM84_STUN => new WeaponRestrictionResult(
                    theInstance.weaponFlashGrenade, 
                    theInstance.limitedWeaponFlashGrenade, 
                    "Flash Grenade"),

                (int)WeaponStack.WPN_M67_FRAG => new WeaponRestrictionResult(
                    theInstance.weaponFragGrenade, 
                    theInstance.limitedWeaponFragGrenade, 
                    "Frag Grenade"),

                (int)WeaponStack.WPN_AN_M8_SMOKE => new WeaponRestrictionResult(
                    theInstance.weaponSmokeGrenade, 
                    theInstance.limitedWeaponSmokeGrenade, 
                    "Smoke Grenade"),

                (int)WeaponStack.WPN_Satchel_CHARGE or 
                (int)WeaponStack.WPN_Radio_DETONATOR => new WeaponRestrictionResult(
                    theInstance.weaponSatchelCharges, 
                    theInstance.limitedWeaponSatchelCharges, 
                    "Satchel Charge"),

                (int)WeaponStack.WPN_Claymore => new WeaponRestrictionResult(
                    theInstance.weaponClaymore, 
                    theInstance.limitedWeaponClaymore, 
                    "Claymore"),

                (int)WeaponStack.WPN_AT4 => new WeaponRestrictionResult(
                    theInstance.weaponAT4, 
                    theInstance.limitedWeaponAT4, 
                    "AT4"),

                // Unknown or always-allowed weapons (knife, medkit, mounts)
                _ => new WeaponRestrictionResult(true, false, "Unknown")
            };
        }

        /// <summary>
        /// Clean up disconnected players from weapon restriction tracking
        /// </summary>
        public static void CleanupDisconnectedPlayers()
        {
            var disconnectedSlots = weaponDisarmedPlayers.Keys
                .Where(slot => !playerInstance.PlayerList.ContainsKey(slot))
                .ToList();

            foreach (var slot in disconnectedSlots)
            {
                weaponDisarmedPlayers.Remove(slot);
                AppDebug.Log("CheckWeaponRestriction", $"Cleaned up disconnected player from slot {slot}");
            }
        }

        /// <summary>
        /// Re-arm all players who were previously disarmed due to weapon restrictions
        /// </summary>
        public static void RearmAllDisarmedPlayers(int currentPlayers, int threshold)
        {
            if (weaponDisarmedPlayers.Count == 0)
                return;

            var slotsToRearm = weaponDisarmedPlayers.Keys.ToList();
            
            foreach (var slot in slotsToRearm)
            {
                if (playerInstance.PlayerList.TryGetValue(slot, out PlayerObject? player))
                {
                    ArmPlayer(slot, player.PlayerName);
                    
                    string rearmMessage = $"{DateTime.Now:HH:mm:ss} - {player.PlayerName} - Full weapons now available ({currentPlayers}/{threshold} players)";
                    ServerMemory.WriteMemorySendChatMessage(1, rearmMessage);
                    AppDebug.Log("CheckWeaponRestriction", rearmMessage);
                }
            }
            
            weaponDisarmedPlayers.Clear();
        }

        /// <summary>
        /// Result of weapon restriction check
        /// </summary>
        private class WeaponRestrictionResult
        {
            public bool IsFullyAllowed { get; }
            public bool IsLimitedAllowed { get; }
            public string WeaponName { get; }

            public WeaponRestrictionResult(bool isFullyAllowed, bool isLimitedAllowed, string weaponName)
            {
                IsFullyAllowed = isFullyAllowed;
                IsLimitedAllowed = isLimitedAllowed;
                WeaponName = weaponName;
            }
        }

        // ================================================================================
        // BAN OPERATIONS
        // ================================================================================

        /// <summary>
        /// Ban a player by name only
        /// </summary>
        public static OperationResult BanPlayerByName(string playerName, int playerSlot, string admin = "Console")
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
                    notes: $"Banned from Player Tab by {admin}",
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
        public static async Task<OperationResult> BanPlayerByIPAsync(IPAddress ipAddress, string playerName, int playerSlot, string admin = "Console")
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
                    notes: $"Banned from Player Tab by {admin}",
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
        public static async Task<OperationResult> BanPlayerByBothAsync(string playerName, IPAddress ipAddress, int playerSlot, string admin = "Console")
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
                    notes: $"Banned from Player Tab by {admin}"
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