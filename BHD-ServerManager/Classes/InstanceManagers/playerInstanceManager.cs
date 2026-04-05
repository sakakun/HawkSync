using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using ServerManager.Classes.GameManagement;
using HawkSyncShared.Instances;
using HawkSyncShared.DTOs.tabPlayers;
using ServerManager.Classes.Services.NetLimiter;
using System.Net;

namespace ServerManager.Classes.InstanceManagers
{
    /// <summary>
    /// Manager for player-related operations (kick, kill, ban, etc.)
    /// </summary>
    public static class playerInstanceManager
    {
        private static theInstance theInstance => CommonCore.theInstance!;

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
            public bool IsRearmed { get; set; }
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

                return new OperationResult(true, $"Player {playerName} has been armed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error arming player", AppDebug.LogLevel.Error, ex);
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
                
                return new OperationResult(true, $"Player {playerName} has been disarmed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error disarming player",  AppDebug.LogLevel.Error, ex);
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

                return new OperationResult(true, $"Player {playerName} has been killed.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error killing player", AppDebug.LogLevel.Error, ex);
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

                return new OperationResult(true, $"Player {playerName} has been kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error kicking player", AppDebug.LogLevel.Error, ex);
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
                chatInstanceManager.SendChatMessage(fullMessage, 3);
                
                return new OperationResult(true, $"Warning sent to {playerName}.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error warning player", AppDebug.LogLevel.Error, ex);
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
                
                return new OperationResult(true, $"God mode {(enable ? "enabled" : "disabled")} for {playerName}.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error toggling god mode", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // TEAM MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Switch a player to a specific team for the next map
        /// </summary>
        public static OperationResult SwitchPlayerTeam(int playerSlot, string playerName, int currentTeam, int targetTeam)
        {
            try
            {
                var (isValid, errorMessage) = ValidatePlayerOperation(playerSlot, playerName);
                if (!isValid)
                    return new OperationResult(false, errorMessage);

                // Validate target team
                if (targetTeam < 1 || targetTeam > 4)
                    return new OperationResult(false, $"Invalid target team: {targetTeam}. Must be 1-4.");

                if (currentTeam == targetTeam)
                    return new OperationResult(false, $"Player {playerName} is already on that team.");

                // Check if already queued for team switch
                var existing = playerInstance.PlayerChangeTeamList.FirstOrDefault(p => p.slotNum == playerSlot);
                
                if (existing != null)
                {
                    // Undo the team switch
                    playerInstance.PlayerChangeTeamList.Remove(existing);
                    return new OperationResult(true, $"Team switch for {playerName} has been undone.");
                }

                // Queue team switch for next map
                playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                {
                    slotNum = playerSlot,
                    Team = targetTeam
                });

                string teamName = targetTeam switch
                {
                    1 => "Blue",
                    2 => "Red",
                    3 => "Yellow",
                    4 => "Violet",
                    // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
                    _ => "Unknown"
                };

                return new OperationResult(true, $"Player {playerName} will be switched to {teamName} team for the next map.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error switching player team", AppDebug.LogLevel.Error, ex);
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

                // Check for knife and medkit (always allowed) - but may need to rearm if previously disarmed
                if (weaponId == (int)WeaponStack.WPN_KNIFE || weaponId == (int)WeaponStack.WPN_MEDPACK)
                {
                    // If player was disarmed, rearm them and remove from tracking
                    if (weaponDisarmedPlayers.TryGetValue(playerInfo.PlayerSlot, out _))
                    {
                        weaponDisarmedPlayers.Remove(playerInfo.PlayerSlot);
                        
                        ArmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                        
                        chatInstanceManager.SendChatMessage($"{playerInfo.PlayerName} re-armed.", 3);
                    }
                    
                    return;
                }

                // Check if weapon is on the restricted list
                WeaponRestrictionResult restriction = CheckWeaponRestrictionStatus(weaponId);

                // LOGIC: Determine if weapon should be restricted
                // Gold (IsFullyAllowed = true) = Always allowed, never disarm
                // Green (IsLimitedAllowed = true) = Allowed only when currentPlayers >= threshold
                // Gray (both false) = Always disabled, always disarm
                bool isWeaponRestricted;
                if (restriction.IsFullyAllowed)
                {
                    // Gold button - weapon always allowed regardless of player count
                    isWeaponRestricted = false;
                }
                else if (restriction.IsLimitedAllowed)
                {
                    // Green button - weapon restricted below threshold
                    isWeaponRestricted = (currentPlayers < threshold);
                }
                else
                {
                    // Gray button - weapon always disabled
                    isWeaponRestricted = true;
                }

                // Check if player is currently in disarm cycle
                if (weaponDisarmedPlayers.TryGetValue(playerInfo.PlayerSlot, out WeaponDisarmInfo? disarmInfo))
                {
                    // Player is in disarm cycle
                    TimeSpan timeSinceDisarm = DateTime.Now - disarmInfo.DisarmTime;

                    // Check if 5 seconds have passed
                    if (timeSinceDisarm.TotalSeconds >= WEAPON_DISARM_DURATION_SECONDS)
                    {
                        // 5 seconds passed - check current weapon
                        if (isWeaponRestricted)
                        {
                            // Player has a restricted weapon after disarm period
                            // Only send message if player was rearmed (state transition: allowed → restricted)
                            if (disarmInfo.IsRearmed)
                            {
                                // Player switched back to restricted weapon after being rearmed
                                disarmInfo.DisarmTime = DateTime.Now;
                                disarmInfo.RestrictedWeaponId = weaponId;
                                disarmInfo.RestrictedWeaponName = restriction.WeaponName;
                                disarmInfo.IsRearmed = false;
                                
                                DisarmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                                
                                // Send message
                                string message = $"{playerInfo.PlayerName} disarmed.";
                                string message2 = $"{playerInfo.PlayerName}, weapon requires {threshold}+ players.";
                                chatInstanceManager.SendChatMessage(message, 3);
                                chatInstanceManager.SendChatMessage(message2, 3);
                            }
                            else
                            {
                                // Still holding same restricted weapon OR switched to different restricted weapon
                                // Update tracking but don't send another message
                                disarmInfo.DisarmTime = DateTime.Now;
                                disarmInfo.RestrictedWeaponId = weaponId;
                                disarmInfo.RestrictedWeaponName = restriction.WeaponName;
                                
                                DisarmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                                
                            }
                        }
                        else
                        {
                            // Player has an allowed weapon
                            if (!disarmInfo.IsRearmed)
                            {
                                // Player just switched to allowed weapon - re-arm them
                                disarmInfo.IsRearmed = true;
                                
                                ArmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);
                                
                                chatInstanceManager.SendChatMessage($"{playerInfo.PlayerName} re-armed.", 3);
                                
                            }
                            else
                            {
                                // Player already rearmed and still has allowed weapon - remove from tracking
                                weaponDisarmedPlayers.Remove(playerInfo.PlayerSlot);
                            }
                        }
                    }
                    // else: Still within 5-second disarm window, do nothing
                    return;
                }

                // Player NOT in disarm cycle - check if weapon is restricted
                if (isWeaponRestricted)
                {
                    // Weapon is RESTRICTED (Gray) - start disarm cycle
                    string weaponName = playerInfo.SelectedWeaponName;
                    
                    weaponDisarmedPlayers[playerInfo.PlayerSlot] = new WeaponDisarmInfo
                    {
                        DisarmTime = DateTime.Now,
                        RestrictedWeaponId = weaponId,
                        RestrictedWeaponName = weaponName,
                        MessageSent = false,
                        IsRearmed = false
                    };

                    // Disarm the player
                    var result = DisarmPlayer(playerInfo.PlayerSlot, playerInfo.PlayerName);

                    if (result.Success)
                    {
                        // Send message
                        string message = $"{playerInfo.PlayerName} disarmed.";
                        string message2 = $"{playerInfo.PlayerName}, weapon requires {threshold}+ players.";
                        chatInstanceManager.SendChatMessage(message, 3);
                        chatInstanceManager.SendChatMessage(message2, 3);
						weaponDisarmedPlayers[playerInfo.PlayerSlot].MessageSent = true;

                    }
                    else
                    {
                        AppDebug.Log($"FAILED to disarm", AppDebug.LogLevel.Error, new Exception(result.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"EXCEPTION", AppDebug.LogLevel.Error, ex);
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
                    theInstance.restrictedWeaponColt45, 
                    theInstance.weaponColt45, 
                    "Colt .45"),
                
                (int)WeaponStack.WPN_M9Beretta => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM9Beretta, 
                    theInstance.weaponM9Beretta, 
                    "M9 Beretta"),

                // Shotgun
                (int)WeaponStack.WPN_RemmingtonSG => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponShotgun, 
                    theInstance.weaponShotgun, 
                    "Shotgun"),

                // CAR-15 variants
                (int)WeaponStack.WPN_CAR15_AUTO or 
                (int)WeaponStack.WPN_CAR15_SEMI => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponCar15, 
                    theInstance.weaponCar15, 
                    "CAR-15"),

                (int)WeaponStack.WPN_CAR15_203_AUTO or 
                (int)WeaponStack.WPN_CAR15_203_SEMI or 
                (int)WeaponStack.WPN_CAR15_203_203 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponCar15203, 
                    theInstance.weaponCar15203, 
                    "CAR-15 M203"),

                // M16 variants
                (int)WeaponStack.WPN_M16_Burst or 
                (int)WeaponStack.WPN_M16_SEMI => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM16, 
                    theInstance.weaponM16, 
                    "M16"),

                (int)WeaponStack.WPN_M16_203_Burst or 
                (int)WeaponStack.WPN_M16_203_SEMI or 
                (int)WeaponStack.WPN_M16_203_203 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM16203, 
                    theInstance.weaponM16203, 
                    "M16 M203"),

                // Sniper rifles
                (int)WeaponStack.WPN_M21 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM21, 
                    theInstance.weaponM21, 
                    "M21"),

                (int)WeaponStack.WPN_M24 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM24, 
                    theInstance.weaponM24, 
                    "M24"),

                (int)WeaponStack.WPN_MCRT_300_TACTICAL => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponMCRT300, 
                    theInstance.weaponMCRT300, 
                    "McMillan .300"),

                (int)WeaponStack.WPN_Barrett => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponBarrett, 
                    theInstance.weaponBarrett, 
                    "Barrett"),

                (int)WeaponStack.WPN_PSG1 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponPSG1, 
                    theInstance.weaponPSG1, 
                    "PSG-1"),

                // Machine guns
                (int)WeaponStack.WPN_SAW => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponSAW, 
                    theInstance.weaponSAW, 
                    "SAW"),

                (int)WeaponStack.WPN_M60 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM60, 
                    theInstance.weaponM60, 
                    "M60"),

                (int)WeaponStack.WPN_M240 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponM240, 
                    theInstance.weaponM240, 
                    "M240"),

                (int)WeaponStack.WPN_MP5 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponMP5, 
                    theInstance.weaponMP5, 
                    "MP5"),

                // G3 variants
                (int)WeaponStack.WPN_G3_Auto or 
                (int)WeaponStack.WPN_G3_SEMI => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponG3, 
                    theInstance.weaponG3, 
                    "G3"),

                // G36 variants
                (int)WeaponStack.WPN_G36_AUTO or 
                (int)WeaponStack.WPN_G36_SEMI => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponG36, 
                    theInstance.weaponG36, 
                    "G36"),

                // Grenades and explosives
                (int)WeaponStack.WPN_XM84_STUN => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponFlashGrenade, 
                    theInstance.weaponFlashGrenade, 
                    "Flash Grenade"),

                (int)WeaponStack.WPN_M67_FRAG => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponFragGrenade, 
                    theInstance.weaponFragGrenade, 
                    "Frag Grenade"),

                (int)WeaponStack.WPN_AN_M8_SMOKE => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponSmokeGrenade, 
                    theInstance.weaponSmokeGrenade, 
                    "Smoke Grenade"),

                (int)WeaponStack.WPN_Satchel_CHARGE or 
                (int)WeaponStack.WPN_Radio_DETONATOR => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponSatchelCharges, 
                    theInstance.weaponSatchelCharges, 
                    "Satchel Charge"),

                (int)WeaponStack.WPN_Claymore => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponClaymore, 
                    theInstance.weaponClaymore, 
                    "Claymore"),

                (int)WeaponStack.WPN_AT4 => new WeaponRestrictionResult(
                    theInstance.restrictedWeaponAT4, 
                    theInstance.weaponAT4, 
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
                    
                    string rearmMessage = $"{player.PlayerName} re-armed. Full weapons available ({currentPlayers}/{threshold} players).";
                    chatInstanceManager.SendChatMessage(rearmMessage, 3);
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

                return new OperationResult(true, $"Player {playerName} has been banned by name and kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error banning player by name", AppDebug.LogLevel.Error, ex);
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
                    }
                    catch (Exception nlEx)
                    {
                        AppDebug.Log($"Warning: NetLimiter update failed", AppDebug.LogLevel.Error,  nlEx);
                    }
                }

                // Kick the player if they're online
                if (playerSlot > 0 && playerInstance.PlayerList.ContainsKey(playerSlot))
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");
                }
                
                return new OperationResult(true, $"Player {playerName} has been banned by IP and kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error banning player by IP", AppDebug.LogLevel.Error, ex);
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
                    }
                    catch (Exception nlEx)
                    {
                        AppDebug.Log($"Warning: NetLimiter update failed", AppDebug.LogLevel.Error, nlEx);
                        // Continue anyway - ban was successful
                    }
                }

                // Kick the player if they're online
                if (playerSlot > 0 && playerInstance.PlayerList.ContainsKey(playerSlot))
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerSlot}");
                }
                
                return new OperationResult(true, $"Player {playerName} has been banned by name and IP, then kicked from the server.");
            }
            catch (Exception ex)
            {
                AppDebug.Log( $"Error banning player by both", AppDebug.LogLevel.Error, ex);
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
                
                return new OperationResult(true, $"Kicked {kickedCount} player(s) from the server.", kickedCount);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error kicking all players", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }
    }
}