using BHD_ServerManager.API;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Tickers;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Storage;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    // ================================================================================
    // DTOs (Data Transfer Objects)
    // ================================================================================

    /// <summary>
    /// Command-line flags for server startup
    /// </summary>
    public record CommandLineFlags(
        bool Flag01, bool Flag02, bool Flag03, bool Flag04,
        bool Flag05, bool Flag06, bool Flag07, bool Flag08,
        bool Flag09, bool Flag10, bool Flag11, bool Flag12,
        bool Flag13, bool Flag14, bool Flag15, bool Flag16,
        bool Flag17, bool Flag18, bool Flag19, bool Flag20,
        bool Flag21
    );

    /// <summary>
    /// Profile settings for server configuration
    /// </summary>
    public record ProfileSettings(
        string ServerPath,
        string ModFileName,
        string HostName,
        string ServerName,
        string MOTD,
        string BindIP,
        int BindPort,
        string LobbyPassword,
        bool Dedicated,
        bool RequireNova,
        bool LocalPlay,
        string CountryCode,
        bool MinPingEnabled,
        bool MaxPingEnabled,
        int MinPingValue,
        int MaxPingValue,
        bool EnableRemote,
        int RemotePort,
        CommandLineFlags Attributes,
        bool EnableKickIdle,
        int PlayerIdleLimit
    );

    /// <summary>
    /// Server gameplay options
    /// </summary>
    public record ServerOptions(
        bool AutoBalance,
        bool ShowTracers,
        bool ShowClays,
        bool AutoRange,
        bool CustomSkins,
        bool DestroyBuildings,
        bool FatBullets,
        bool OneShotKills,
        bool AllowLeftLeaning,
        bool AllowRightLeaning,
        bool Enable4Teams
    );

    /// <summary>
    /// Friendly fire settings
    /// </summary>
    public record FriendlyFireSettings(
        bool Enabled,
        int MaxKills,
        bool WarnOnKill,
        bool ShowFriendlyTags
    );

    /// <summary>
    /// Role restrictions
    /// </summary>
    public record RoleRestrictions(
        bool CQB,
        bool Gunner,
        bool Sniper,
        bool Medic
    );

    /// <summary>
    /// Weapon restrictions
    /// </summary>
    public record WeaponEnablement(
        bool Colt45, bool M9Beretta,
        bool CAR15, bool CAR15203,
        bool M16, bool M16203,
        bool G3, bool G36,
        bool M60, bool M240,
        bool MP5, bool SAW,
        bool MCRT300, bool M21,
        bool M24, bool Barrett,
        bool PSG1, bool Shotgun,
        bool FragGrenade, bool SmokeGrenade,
        bool Satchel, bool AT4,
        bool FlashBang, bool Claymore
    );

    /// <summary>
    /// Limited weapon restrictions (allowed when below player threshold)
    /// </summary>
    public record RestrictedWeapons(
        bool Colt45, bool M9Beretta,
        bool CAR15, bool CAR15203,
        bool M16, bool M16203,
        bool G3, bool G36,
        bool M60, bool M240,
        bool MP5, bool SAW,
        bool MCRT300, bool M21,
        bool M24, bool Barrett,
        bool PSG1, bool Shotgun,
        bool FragGrenade, bool SmokeGrenade,
        bool Satchel, bool AT4,
        bool FlashBang, bool Claymore
    );

    /// <summary>
    /// Complete gameplay settings
    /// </summary>
    public record GamePlaySettings(
        // Lobby Passwords
        string BluePassword,
        string RedPassword,
        string YellowPassword,
        string VioletPassword,
        
        // Win Conditions
        int ScoreKOTH,
        int ScoreDM,
        int ScoreFB,
        
        // Server Values
        int TimeLimit,
        int LoopMaps,
        int StartDelay,
        int RespawnTime,
        int ScoreBoardDelay,
        int MaxSlots,
        int PSPTakeoverTimer,
        int FlagReturnTime,
        int FullWeaponThreshold,
        
        // Grouped Settings
        ServerOptions Options,
        FriendlyFireSettings FriendlyFire,
        RoleRestrictions Roles,
        WeaponEnablement Weapons,
        RestrictedWeapons RestrictedWeapons
    );

    // ================================================================================
    // The Instance Manager - Business Logic Layer
    // ================================================================================

    public static class theInstanceManager
    {
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;
        private static statInstance instanceStats => CommonCore.instanceStats!;

        // ================================================================================
        // PROFILE SETTINGS MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Load profile settings from database
        /// </summary>
        public static OperationResult LoadProfileSettings()
        {
            try
            {
                var flags = new CommandLineFlags(
                    Flag01: ServerSettings.Get("profileServerAttribute01", false),
                    Flag02: ServerSettings.Get("profileServerAttribute02", false),
                    Flag03: ServerSettings.Get("profileServerAttribute03", false),
                    Flag04: ServerSettings.Get("profileServerAttribute04", false),
                    Flag05: ServerSettings.Get("profileServerAttribute05", false),
                    Flag06: ServerSettings.Get("profileServerAttribute06", false),
                    Flag07: ServerSettings.Get("profileServerAttribute07", true),
                    Flag08: ServerSettings.Get("profileServerAttribute08", true),
                    Flag09: ServerSettings.Get("profileServerAttribute09", false),
                    Flag10: ServerSettings.Get("profileServerAttribute10", false),
                    Flag11: ServerSettings.Get("profileServerAttribute11", false),
                    Flag12: ServerSettings.Get("profileServerAttribute12", false),
                    Flag13: ServerSettings.Get("profileServerAttribute13", false),
                    Flag14: ServerSettings.Get("profileServerAttribute14", false),
                    Flag15: ServerSettings.Get("profileServerAttribute15", false),
                    Flag16: ServerSettings.Get("profileServerAttribute16", false),
                    Flag17: ServerSettings.Get("profileServerAttribute17", false),
                    Flag18: ServerSettings.Get("profileServerAttribute18", true),
                    Flag19: ServerSettings.Get("profileServerAttribute19", false),
                    Flag20: ServerSettings.Get("profileServerAttribute20", false),
                    Flag21: ServerSettings.Get("profileServerAttribute21", true)
                );

                var settings = new ProfileSettings(
                    ServerPath: ServerSettings.Get("profileServerPath", string.Empty),
                    ModFileName: ServerSettings.Get("profileModFileName", string.Empty),
                    HostName: ServerSettings.Get("gameHostName", string.Empty),
                    ServerName: ServerSettings.Get("gameServerName", string.Empty),
                    MOTD: ServerSettings.Get("gameMOTD", string.Empty),
                    BindIP: ServerSettings.Get("profileBindIP", "0.0.0.0"),
                    BindPort: ServerSettings.Get("profileBindPort", 17479),
                    LobbyPassword: ServerSettings.Get("gamePasswordLobby", string.Empty),
                    Dedicated: ServerSettings.Get("gameDedicated", true),
                    RequireNova: ServerSettings.Get("gameRequireNova", false),
                    LocalPlay: ServerSettings.Get("gameLocalPlay", false),
                    CountryCode: ServerSettings.Get("gameCountryCode", "US"),
                    MinPingEnabled: ServerSettings.Get("gameMinPing", false),
                    MaxPingEnabled: ServerSettings.Get("gameMaxPing", false),
                    MinPingValue: ServerSettings.Get("gameMinPingValue", 0),
                    MaxPingValue: ServerSettings.Get("gameMaxPingValue", 999),
                    EnableRemote: ServerSettings.Get("profileEnableRemote", false),
                    RemotePort: ServerSettings.Get("profileRemotePort", 9090),
                    Attributes: flags,
                    EnableKickIdle: ServerSettings.Get("gameEnableKickIdle", false),
                    PlayerIdleLimit: ServerSettings.Get("gameKickIdleTime", 120)
                );

                // Update instance
                ApplyProfileSettingsToInstance(settings);

                AppDebug.Log("theInstanceManager", "Profile settings loaded successfully");
                return new OperationResult(true, "Profile settings loaded successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error loading profile settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Save profile settings to database
        /// </summary>
        public static OperationResult SaveProfileSettings(ProfileSettings settings)
        {
            try
            {
                // Validate settings
                var (isValid, errors) = ValidateProfileSettings(settings);
                if (!isValid)
                {
                    return new OperationResult(false, $"Validation failed: {string.Join(", ", errors)}");
                }

                // Save to database
                ServerSettings.Set("profileServerPath", settings.ServerPath);
                ServerSettings.Set("profileModFileName", settings.ModFileName);
                ServerSettings.Set("gameHostName", settings.HostName);
                ServerSettings.Set("gameServerName", settings.ServerName);
                ServerSettings.Set("gameMOTD", settings.MOTD);
                ServerSettings.Set("profileBindIP", settings.BindIP);
                ServerSettings.Set("profileBindPort", settings.BindPort);
                ServerSettings.Set("gamePasswordLobby", settings.LobbyPassword);
                ServerSettings.Set("gameDedicated", settings.Dedicated);
                ServerSettings.Set("gameRequireNova", settings.RequireNova);
                ServerSettings.Set("gameLocalPlay", settings.LocalPlay);
                ServerSettings.Set("gameCountryCode", settings.CountryCode);
                ServerSettings.Set("gameMinPing", settings.MinPingEnabled);
                ServerSettings.Set("gameMaxPing", settings.MaxPingEnabled);
                ServerSettings.Set("gameMinPingValue", settings.MinPingValue);
                ServerSettings.Set("gameMaxPingValue", settings.MaxPingValue);
                ServerSettings.Set("profileEnableRemote", settings.EnableRemote);
                ServerSettings.Set("profileRemotePort", settings.RemotePort);
                // Save idle kick fields
                ServerSettings.Set("gameEnableKickIdle", settings.EnableKickIdle);
                ServerSettings.Set("gameKickIdleTime", settings.PlayerIdleLimit);

                // Save command-line flags
                ServerSettings.Set("profileServerAttribute01", settings.Attributes.Flag01);
                ServerSettings.Set("profileServerAttribute02", settings.Attributes.Flag02);
                ServerSettings.Set("profileServerAttribute03", settings.Attributes.Flag03);
                ServerSettings.Set("profileServerAttribute04", settings.Attributes.Flag04);
                ServerSettings.Set("profileServerAttribute05", settings.Attributes.Flag05);
                ServerSettings.Set("profileServerAttribute06", settings.Attributes.Flag06);
                ServerSettings.Set("profileServerAttribute07", settings.Attributes.Flag07);
                ServerSettings.Set("profileServerAttribute08", settings.Attributes.Flag08);
                ServerSettings.Set("profileServerAttribute09", settings.Attributes.Flag09);
                ServerSettings.Set("profileServerAttribute10", settings.Attributes.Flag10);
                ServerSettings.Set("profileServerAttribute11", settings.Attributes.Flag11);
                ServerSettings.Set("profileServerAttribute12", settings.Attributes.Flag12);
                ServerSettings.Set("profileServerAttribute13", settings.Attributes.Flag13);
                ServerSettings.Set("profileServerAttribute14", settings.Attributes.Flag14);
                ServerSettings.Set("profileServerAttribute15", settings.Attributes.Flag15);
                ServerSettings.Set("profileServerAttribute16", settings.Attributes.Flag16);
                ServerSettings.Set("profileServerAttribute17", settings.Attributes.Flag17);
                ServerSettings.Set("profileServerAttribute18", settings.Attributes.Flag18);
                ServerSettings.Set("profileServerAttribute19", settings.Attributes.Flag19);
                ServerSettings.Set("profileServerAttribute20", settings.Attributes.Flag20);
                ServerSettings.Set("profileServerAttribute21", settings.Attributes.Flag21);

                // Update instance
                ApplyProfileSettingsToInstance(settings);

                AppDebug.Log("theInstanceManager", "Profile settings saved successfully");
                return new OperationResult(true, "Profile settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error saving profile settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Validate profile settings
        /// </summary>
        public static (bool isValid, List<string> errors) ValidateProfileSettings(ProfileSettings settings)
        {
            var errors = new List<string>();

            // Validate server path
            if (string.IsNullOrWhiteSpace(settings.ServerPath))
                errors.Add("Server path cannot be empty.");
            else if (!Directory.Exists(settings.ServerPath))
                errors.Add($"Server path does not exist: {settings.ServerPath}");
            else if (!File.Exists(Path.Combine(settings.ServerPath, "dfbhd.exe")))
                errors.Add("Server path does not contain dfbhd.exe");

            // Validate host name
            if (string.IsNullOrWhiteSpace(settings.HostName))
                errors.Add("Host name cannot be empty.");
            else if (settings.HostName.Length > 50)
                errors.Add("Host name cannot exceed 50 characters.");

            // Validate server name
            if (string.IsNullOrWhiteSpace(settings.ServerName))
                errors.Add("Server name cannot be empty.");
            else if (settings.ServerName.Length > 100)
                errors.Add("Server name cannot exceed 100 characters.");

            // Validate MOTD
            if (settings.MOTD.Length > 200)
                errors.Add("MOTD cannot exceed 200 characters.");

            // Validate bind IP
            if (!ValidateBindIP(settings.BindIP, out string ipError))
                errors.Add(ipError);

            // Validate port
            if (!ValidatePort(settings.BindPort, out string portError))
                errors.Add(portError);

            // Validate remote port
            if (settings.EnableRemote && !ValidatePort(settings.RemotePort, out string remotePortError))
                errors.Add($"Remote port error: {remotePortError}");

            // Validate country code
            if (!ValidateCountryCode(settings.CountryCode, out string countryError))
                errors.Add(countryError);

            // Validate ping range
            if (!ValidatePingRange(settings.MinPingValue, settings.MaxPingValue, out string pingError))
                errors.Add(pingError);

            return (errors.Count == 0, errors);
        }

        /// <summary>
        /// Export profile settings to JSON file
        /// </summary>
        public static OperationResult ExportProfileSettings(string filePath)
        {
            try
            {
                var result = LoadProfileSettings();
                if (!result.Success)
                    return result;

                // Get current settings from instance
                var flags = new CommandLineFlags(
                    theInstance.profileServerAttribute01, theInstance.profileServerAttribute02,
                    theInstance.profileServerAttribute03, theInstance.profileServerAttribute04,
                    theInstance.profileServerAttribute05, theInstance.profileServerAttribute06,
                    theInstance.profileServerAttribute07, theInstance.profileServerAttribute08,
                    theInstance.profileServerAttribute09, theInstance.profileServerAttribute10,
                    theInstance.profileServerAttribute11, theInstance.profileServerAttribute12,
                    theInstance.profileServerAttribute13, theInstance.profileServerAttribute14,
                    theInstance.profileServerAttribute15, theInstance.profileServerAttribute16,
                    theInstance.profileServerAttribute17, theInstance.profileServerAttribute18,
                    theInstance.profileServerAttribute19, theInstance.profileServerAttribute20,
                    theInstance.profileServerAttribute21
                );

                var settings = new ProfileSettings(
                    theInstance.profileServerPath, theInstance.profileModFileName,
                    theInstance.gameHostName, theInstance.gameServerName, theInstance.gameMOTD,
                    theInstance.profileBindIP, theInstance.profileBindPort,
                    theInstance.gamePasswordLobby, theInstance.gameDedicated, theInstance.gameRequireNova,
                    theInstance.gameLocalPlay, theInstance.gameCountryCode, theInstance.gameMinPing, theInstance.gameMaxPing,
                    theInstance.gameMinPingValue, theInstance.gameMaxPingValue,
                    theInstance.profileEnableRemote, theInstance.profileRemotePort,
                    flags,
                    theInstance.gameEnableKickIdle, // EnableKickIdle
                    theInstance.gameKickIdleTime   // PlayerIdleLimit
                );

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(filePath, json);

                AppDebug.Log("theInstanceManager", $"Profile settings exported to {filePath}");
                return new OperationResult(true, $"Settings exported to {filePath}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error exporting profile settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Import profile settings from JSON file
        /// </summary>
        public static (bool success, ProfileSettings? settings, string errorMessage) ImportProfileSettings(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return (false, null, "File not found.");

                string json = File.ReadAllText(filePath);
                var settings = JsonSerializer.Deserialize<ProfileSettings>(json);

                if (settings == null)
                    return (false, null, "Failed to deserialize settings.");

                AppDebug.Log("theInstanceManager", $"Profile settings imported from {filePath}");
                return (true, settings, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error importing profile settings: {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        // ================================================================================
        // GAMEPLAY SETTINGS MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Load gameplay settings from database
        /// </summary>
        public static OperationResult LoadGamePlaySettings()
        {
            try
            {
            var options = new ServerOptions(
                AutoBalance: ServerSettings.Get("gameOptionAutoBalance", true),
                ShowTracers: ServerSettings.Get("gameOptionShowTracers", false),
                ShowClays: ServerSettings.Get("gameShowTeamClays", true),
                AutoRange: ServerSettings.Get("gameOptionAutoRange", false),
                CustomSkins: ServerSettings.Get("gameCustomSkins", true),
                DestroyBuildings: ServerSettings.Get("gameDestroyBuildings", true),
                FatBullets: ServerSettings.Get("gameFatBullets", false),
                OneShotKills: ServerSettings.Get("gameOneShotKills", false),
                AllowLeftLeaning: ServerSettings.Get("gameAllowLeftLeaning", true),
                AllowRightLeaning: ServerSettings.Get("gameAllowRightLeaning", true),
                Enable4Teams: ServerSettings.Get("gameEnableFourTeams", false)
            );

                var friendlyFire = new FriendlyFireSettings(
                    Enabled: ServerSettings.Get("gameOptionFF", true),
                    MaxKills: ServerSettings.Get("gameFriendlyFireKills", 10),
                    WarnOnKill: ServerSettings.Get("gameOptionFFWarn", false),
                    ShowFriendlyTags: ServerSettings.Get("gameOptionFriendlyTags", false)
                );

                var roles = new RoleRestrictions(
                    CQB: ServerSettings.Get("roleCQB", true),
                    Gunner: ServerSettings.Get("roleGunner", true),
                    Sniper: ServerSettings.Get("roleSniper", true),
                    Medic: ServerSettings.Get("roleMedic", true)
                );

                var weapons = new WeaponEnablement(
                    Colt45: ServerSettings.Get("weaponColt45", true),
                    M9Beretta: ServerSettings.Get("weaponM9Beretta", true),
                    CAR15: ServerSettings.Get("weaponCar15", true),
                    CAR15203: ServerSettings.Get("weaponCar15203", true),
                    M16: ServerSettings.Get("weaponM16", true),
                    M16203: ServerSettings.Get("weaponM16203", true),
                    G3: ServerSettings.Get("weaponG3", true),
                    G36: ServerSettings.Get("weaponG36", true),
                    M60: ServerSettings.Get("weaponM60", true),
                    M240: ServerSettings.Get("weaponM240", true),
                    MP5: ServerSettings.Get("weaponMP5", true),
                    SAW: ServerSettings.Get("weaponSAW", true),
                    MCRT300: ServerSettings.Get("weaponMCRT300", true),
                    M21: ServerSettings.Get("weaponM21", true),
                    M24: ServerSettings.Get("weaponM24", true),
                    Barrett: ServerSettings.Get("weaponBarrett", true),
                    PSG1: ServerSettings.Get("weaponPSG1", true),
                    Shotgun: ServerSettings.Get("weaponShotgun", true),
                    FragGrenade: ServerSettings.Get("weaponFragGrenade", true),
                    SmokeGrenade: ServerSettings.Get("weaponSmokeGrenade", true),
                    Satchel: ServerSettings.Get("weaponSatchelCharges", true),
                    AT4: ServerSettings.Get("weaponAT4", true),
                    FlashBang: ServerSettings.Get("weaponFlashGrenade", true),
                    Claymore: ServerSettings.Get("weaponClaymore", true)
                );

                var RestrictedWeapons = new RestrictedWeapons (
                    Colt45: ServerSettings.Get("restrictedWeaponColt45", false),
                    M9Beretta: ServerSettings.Get("restrictedWeaponM9Beretta", false),
                    CAR15: ServerSettings.Get("restrictedWeaponCar15", false),
                    CAR15203: ServerSettings.Get("restrictedWeaponCar15203", false),
                    M16: ServerSettings.Get("restrictedWeaponM16", false),
                    M16203: ServerSettings.Get("restrictedWeaponM16203", false),
                    G3: ServerSettings.Get("restrictedWeaponG3", false),
                    G36: ServerSettings.Get("restrictedWeaponG36", false),
                    M60: ServerSettings.Get("restrictedWeaponM60", false),
                    M240: ServerSettings.Get("restrictedWeaponM240", false),
                    MP5: ServerSettings.Get("restrictedWeaponMP5", false),
                    SAW: ServerSettings.Get("RestrictedWeaponsAW", false),
                    MCRT300: ServerSettings.Get("restrictedWeaponMCRT300", false),
                    M21: ServerSettings.Get("restrictedWeaponM21", false),
                    M24: ServerSettings.Get("restrictedWeaponM24", false),
                    Barrett: ServerSettings.Get("restrictedWeaponBarrett", false),
                    PSG1: ServerSettings.Get("restrictedWeaponPSG1", false),
                    Shotgun: ServerSettings.Get("RestrictedWeaponshotgun", false),
                    FragGrenade: ServerSettings.Get("restrictedWeaponFragGrenade", false),
                    SmokeGrenade: ServerSettings.Get("RestrictedWeaponsmokeGrenade", false),
                    Satchel: ServerSettings.Get("RestrictedWeaponsatchelCharges", false),
                    AT4: ServerSettings.Get("restrictedWeaponAT4", false),
                    FlashBang: ServerSettings.Get("restrictedWeaponFlashGrenade", false),
                    Claymore: ServerSettings.Get("restrictedWeaponClaymore", false)
                );

                var settings = new GamePlaySettings(
                    BluePassword: ServerSettings.Get("gamePasswordBlue", string.Empty),
                    RedPassword: ServerSettings.Get("gamePasswordRed", string.Empty),
                    YellowPassword: ServerSettings.Get("gamePasswordYellow", string.Empty),
                    VioletPassword: ServerSettings.Get("gamePasswordViolet", string.Empty),
                    ScoreKOTH: ServerSettings.Get("gameScoreZoneTime", 10),
                    ScoreDM: ServerSettings.Get("gameScoreKills", 200),
                    ScoreFB: ServerSettings.Get("gameScoreFlags", 10),
                    TimeLimit: ServerSettings.Get("gameTimeLimit", 22),
                    LoopMaps: ServerSettings.Get("gameLoopMaps", 0),
                    StartDelay: ServerSettings.Get("gameStartDelay", 1),
                    RespawnTime: ServerSettings.Get("gameRespawnTime", 20),
                    ScoreBoardDelay: ServerSettings.Get("gameScoreBoardDelay", 20),
                    MaxSlots: ServerSettings.Get("gameMaxSlots", 50),
                    PSPTakeoverTimer: ServerSettings.Get("gamePSPTOTimer", 20),
                    FlagReturnTime: ServerSettings.Get("gameFlagReturnTime", 210 ),
                    FullWeaponThreshold: ServerSettings.Get("gameFullWeaponThreshold", 10),
                    Options: options,
                    FriendlyFire: friendlyFire,
                    Roles: roles,
                    Weapons: weapons,
                    RestrictedWeapons: RestrictedWeapons
                );

                // Update instance
                ApplyGamePlaySettingsToInstance(settings);

                AppDebug.Log("theInstanceManager", "GamePlay settings loaded successfully");
                return new OperationResult(true, "GamePlay settings loaded successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error loading gameplay settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Save gameplay settings to database
        /// </summary>
        public static OperationResult SaveGamePlaySettings(GamePlaySettings settings)
        {
            try
            {
                // Validate settings
                var (isValid, errors) = ValidateGamePlaySettings(settings);
                if (!isValid)
                {
                    return new OperationResult(false, $"Validation failed: {string.Join(", ", errors)}");
                }

                // Save lobby passwords
                ServerSettings.Set("gamePasswordBlue", settings.BluePassword);
                ServerSettings.Set("gamePasswordRed", settings.RedPassword);
                ServerSettings.Set("gamePasswordYellow", settings.YellowPassword);
                ServerSettings.Set("gamePasswordViolet", settings.VioletPassword);

                // Save win conditions
                ServerSettings.Set("gameScoreZoneTime", settings.ScoreKOTH);
                ServerSettings.Set("gameScoreKills", settings.ScoreDM);
                ServerSettings.Set("gameScoreFlags", settings.ScoreFB);

                // Save server values
                ServerSettings.Set("gameTimeLimit", settings.TimeLimit);
                ServerSettings.Set("gameLoopMaps", settings.LoopMaps);
                ServerSettings.Set("gameStartDelay", settings.StartDelay);
                ServerSettings.Set("gameRespawnTime", settings.RespawnTime);
                ServerSettings.Set("gameScoreBoardDelay", settings.ScoreBoardDelay);
                ServerSettings.Set("gameMaxSlots", settings.MaxSlots);
                ServerSettings.Set("gamePSPTOTimer", settings.PSPTakeoverTimer);
                ServerSettings.Set("gameFlagReturnTime", settings.FlagReturnTime);
                ServerSettings.Set("gameFullWeaponThreshold", settings.FullWeaponThreshold);

            // Save server options
            ServerSettings.Set("gameOptionAutoBalance", settings.Options.AutoBalance);
            ServerSettings.Set("gameOptionShowTracers", settings.Options.ShowTracers);
            ServerSettings.Set("gameShowTeamClays", settings.Options.ShowClays);
            ServerSettings.Set("gameOptionAutoRange", settings.Options.AutoRange);
            ServerSettings.Set("gameCustomSkins", settings.Options.CustomSkins);
            ServerSettings.Set("gameDestroyBuildings", settings.Options.DestroyBuildings);
            ServerSettings.Set("gameFatBullets", settings.Options.FatBullets);
            ServerSettings.Set("gameOneShotKills", settings.Options.OneShotKills);
            ServerSettings.Set("gameAllowLeftLeaning", settings.Options.AllowLeftLeaning);
            ServerSettings.Set("gameAllowRightLeaning", settings.Options.AllowRightLeaning);
            ServerSettings.Set("gameEnableFourTeams", settings.Options.Enable4Teams);

                // Save friendly fire
                ServerSettings.Set("gameOptionFF", settings.FriendlyFire.Enabled);
                ServerSettings.Set("gameFriendlyFireKills", settings.FriendlyFire.MaxKills);
                ServerSettings.Set("gameOptionFFWarn", settings.FriendlyFire.WarnOnKill);
                ServerSettings.Set("gameOptionFriendlyTags", settings.FriendlyFire.ShowFriendlyTags);

                // Save role restrictions
                ServerSettings.Set("roleCQB", settings.Roles.CQB);
                ServerSettings.Set("roleGunner", settings.Roles.Gunner);
                ServerSettings.Set("roleSniper", settings.Roles.Sniper);
                ServerSettings.Set("roleMedic", settings.Roles.Medic);

                // Save weapon restrictions
                ServerSettings.Set("weaponColt45", settings.Weapons.Colt45);
                ServerSettings.Set("weaponM9Beretta", settings.Weapons.M9Beretta);
                ServerSettings.Set("weaponCar15", settings.Weapons.CAR15);
                ServerSettings.Set("weaponCar15203", settings.Weapons.CAR15203);
                ServerSettings.Set("weaponM16", settings.Weapons.M16);
                ServerSettings.Set("weaponM16203", settings.Weapons.M16203);
                ServerSettings.Set("weaponG3", settings.Weapons.G3);
                ServerSettings.Set("weaponG36", settings.Weapons.G36);
                ServerSettings.Set("weaponM60", settings.Weapons.M60);
                ServerSettings.Set("weaponM240", settings.Weapons.M240);
                ServerSettings.Set("weaponMP5", settings.Weapons.MP5);
                ServerSettings.Set("weaponSAW", settings.Weapons.SAW);
                ServerSettings.Set("weaponMCRT300", settings.Weapons.MCRT300);
                ServerSettings.Set("weaponM21", settings.Weapons.M21);
                ServerSettings.Set("weaponM24", settings.Weapons.M24);
                ServerSettings.Set("weaponBarrett", settings.Weapons.Barrett);
                ServerSettings.Set("weaponPSG1", settings.Weapons.PSG1);
                ServerSettings.Set("weaponShotgun", settings.Weapons.Shotgun);
                ServerSettings.Set("weaponFragGrenade", settings.Weapons.FragGrenade);
                ServerSettings.Set("weaponSmokeGrenade", settings.Weapons.SmokeGrenade);
                ServerSettings.Set("weaponSatchelCharges", settings.Weapons.Satchel);
                ServerSettings.Set("weaponAT4", settings.Weapons.AT4);
                ServerSettings.Set("weaponFlashGrenade", settings.Weapons.FlashBang);
                ServerSettings.Set("weaponClaymore", settings.Weapons.Claymore);

                // Save limited weapon restrictions
                ServerSettings.Set("restrictedWeaponColt45", settings.RestrictedWeapons.Colt45);
                ServerSettings.Set("restrictedWeaponM9Beretta", settings.RestrictedWeapons.M9Beretta);
                ServerSettings.Set("restrictedWeaponCar15", settings.RestrictedWeapons.CAR15);
                ServerSettings.Set("restrictedWeaponCar15203", settings.RestrictedWeapons.CAR15203);
                ServerSettings.Set("restrictedWeaponM16", settings.RestrictedWeapons.M16);
                ServerSettings.Set("restrictedWeaponM16203", settings.RestrictedWeapons.M16203);
                ServerSettings.Set("restrictedWeaponG3", settings.RestrictedWeapons.G3);
                ServerSettings.Set("restrictedWeaponG36", settings.RestrictedWeapons.G36);
                ServerSettings.Set("restrictedWeaponM60", settings.RestrictedWeapons.M60);
                ServerSettings.Set("restrictedWeaponM240", settings.RestrictedWeapons.M240);
                ServerSettings.Set("restrictedWeaponMP5", settings.RestrictedWeapons.MP5);
                ServerSettings.Set("RestrictedWeaponsAW", settings.RestrictedWeapons.SAW);
                ServerSettings.Set("restrictedWeaponMCRT300", settings.RestrictedWeapons.MCRT300);
                ServerSettings.Set("restrictedWeaponM21", settings.RestrictedWeapons.M21);
                ServerSettings.Set("restrictedWeaponM24", settings.RestrictedWeapons.M24);
                ServerSettings.Set("restrictedWeaponBarrett", settings.RestrictedWeapons.Barrett);
                ServerSettings.Set("restrictedWeaponPSG1", settings.RestrictedWeapons.PSG1);
                ServerSettings.Set("RestrictedWeaponshotgun", settings.RestrictedWeapons.Shotgun);
                ServerSettings.Set("restrictedWeaponFragGrenade", settings.RestrictedWeapons.FragGrenade);
                ServerSettings.Set("RestrictedWeaponsmokeGrenade", settings.RestrictedWeapons.SmokeGrenade);
                ServerSettings.Set("RestrictedWeaponsatchelCharges", settings.RestrictedWeapons.Satchel);
                ServerSettings.Set("restrictedWeaponAT4", settings.RestrictedWeapons.AT4);
                ServerSettings.Set("restrictedWeaponFlashGrenade", settings.RestrictedWeapons.FlashBang);
                ServerSettings.Set("restrictedWeaponClaymore", settings.RestrictedWeapons.Claymore);

                // Update instance
                ApplyGamePlaySettingsToInstance(settings);

                AppDebug.Log("theInstanceManager", "GamePlay settings saved successfully");
                return new OperationResult(true, "GamePlay settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error saving gameplay settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Validate gameplay settings
        /// </summary>
        public static (bool isValid, List<string> errors) ValidateGamePlaySettings(GamePlaySettings settings)
        {
            var errors = new List<string>();

            // Validate passwords
            if (settings.BluePassword.Length > 50)
                errors.Add("Blue team password cannot exceed 50 characters.");
            if (settings.RedPassword.Length > 50)
                errors.Add("Red team password cannot exceed 50 characters.");
            if (settings.YellowPassword.Length > 50)
                errors.Add("Yellow team password cannot exceed 50 characters.");
            if (settings.VioletPassword.Length > 50)
                errors.Add("Violet team password cannot exceed 50 characters.");

            // Validate win conditions
            if (settings.ScoreKOTH < 1 || settings.ScoreKOTH > 999)
                errors.Add("KOTH score must be between 1 and 999.");
            if (settings.ScoreDM < 1 || settings.ScoreDM > 9999)
                errors.Add("DM score must be between 1 and 9999.");
            if (settings.ScoreFB < 1 || settings.ScoreFB > 999)
                errors.Add("FB score must be between 1 and 999.");

            // Validate server values
            if (settings.TimeLimit < 1 || settings.TimeLimit > 999)
                errors.Add("Time limit must be between 1 and 999 minutes.");
            if (settings.LoopMaps < 0 || settings.LoopMaps > 2)
                errors.Add("Loop maps setting must be 0, 1, or 2.");
            if (settings.StartDelay < 0 || settings.StartDelay > 999)
                errors.Add("Start delay must be between 0 and 999 seconds.");
            if (settings.RespawnTime < 0 || settings.RespawnTime > 999)
                errors.Add("Respawn time must be between 0 and 999 seconds.");
            if (settings.MaxSlots < 1 || settings.MaxSlots > 100)
                errors.Add("Max slots must be between 1 and 80.");

            // Validate friendly fire
            if (settings.FriendlyFire.MaxKills < 0 || settings.FriendlyFire.MaxKills > 999)
                errors.Add("Max friendly fire kills must be between 0 and 999.");

            return (errors.Count == 0, errors);
        }

        /// <summary>
        /// Export gameplay settings to JSON file
        /// </summary>
        public static OperationResult ExportGamePlaySettings(string filePath)
        {
            try
            {
                var result = LoadGamePlaySettings();
                if (!result.Success)
                    return result;

            // Get current settings from instance
            var options = new ServerOptions(
                theInstance.gameOptionAutoBalance, theInstance.gameOptionShowTracers,
                theInstance.gameShowTeamClays, theInstance.gameOptionAutoRange,
                theInstance.gameCustomSkins, theInstance.gameDestroyBuildings,
                theInstance.gameFatBullets, theInstance.gameOneShotKills,
                theInstance.gameAllowLeftLeaning, theInstance.gameAllowRightLeaning,
                theInstance.gameEnableFourTeams
            );

                var friendlyFire = new FriendlyFireSettings(
                    theInstance.gameOptionFF, theInstance.gameFriendlyFireKills,
                    theInstance.gameOptionFFWarn, theInstance.gameOptionFriendlyTags
                );

                var roles = new RoleRestrictions(
                    theInstance.roleCQB, theInstance.roleGunner,
                    theInstance.roleSniper, theInstance.roleMedic
                );

                var weapons = new WeaponEnablement(
                    theInstance.weaponColt45, theInstance.weaponM9Beretta,
                    theInstance.weaponCar15, theInstance.weaponCar15203,
                    theInstance.weaponM16, theInstance.weaponM16203,
                    theInstance.weaponG3, theInstance.weaponG36,
                    theInstance.weaponM60, theInstance.weaponM240,
                    theInstance.weaponMP5, theInstance.weaponSAW,
                    theInstance.weaponMCRT300, theInstance.weaponM21,
                    theInstance.weaponM24, theInstance.weaponBarrett,
                    theInstance.weaponPSG1, theInstance.weaponShotgun,
                    theInstance.weaponFragGrenade, theInstance.weaponSmokeGrenade,
                    theInstance.weaponSatchelCharges, theInstance.weaponAT4,
                    theInstance.weaponFlashGrenade, theInstance.weaponClaymore
                );

                var RestrictedWeapons = new RestrictedWeapons (
                    theInstance.restrictedWeaponColt45, theInstance.restrictedWeaponM9Beretta,
                    theInstance.restrictedWeaponCar15, theInstance.restrictedWeaponCar15203,
                    theInstance.restrictedWeaponM16, theInstance.restrictedWeaponM16203,
                    theInstance.restrictedWeaponG3, theInstance.restrictedWeaponG36,
                    theInstance.restrictedWeaponM60, theInstance.restrictedWeaponM240,
                    theInstance.restrictedWeaponMP5, theInstance.restrictedWeaponSAW,
                    theInstance.restrictedWeaponMCRT300, theInstance.restrictedWeaponM21,
                    theInstance.restrictedWeaponM24, theInstance.restrictedWeaponBarrett,
                    theInstance.restrictedWeaponPSG1, theInstance.restrictedWeaponShotgun,
                    theInstance.restrictedWeaponFragGrenade, theInstance.restrictedWeaponSmokeGrenade,
                    theInstance.restrictedWeaponSatchelCharges, theInstance.restrictedWeaponAT4,
                    theInstance.restrictedWeaponFlashGrenade, theInstance.restrictedWeaponClaymore
                );

                var settings = new GamePlaySettings(
                    theInstance.gamePasswordBlue, theInstance.gamePasswordRed,
                    theInstance.gamePasswordYellow, theInstance.gamePasswordViolet,
                    theInstance.gameScoreZoneTime, theInstance.gameScoreKills, theInstance.gameScoreFlags,
                    theInstance.gameTimeLimit, theInstance.gameLoopMaps, theInstance.gameStartDelay,
                    theInstance.gameRespawnTime, theInstance.gameScoreBoardDelay, theInstance.gameMaxSlots,
                    theInstance.gamePSPTOTimer, theInstance.gameFlagReturnTime,
                    theInstance.gameFullWeaponThreshold,
                    options, friendlyFire, roles, weapons, RestrictedWeapons
                );

                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, jsonOptions);
                File.WriteAllText(filePath, json);

                AppDebug.Log("theInstanceManager", $"GamePlay settings exported to {filePath}");
                return new OperationResult(true, $"Settings exported to {filePath}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error exporting gameplay settings: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Import gameplay settings from JSON file
        /// </summary>
        public static (bool success, GamePlaySettings? settings, string errorMessage) ImportGamePlaySettings(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return (false, null, "File not found.");

                string json = File.ReadAllText(filePath);
                var settings = JsonSerializer.Deserialize<GamePlaySettings>(json);

                if (settings == null)
                    return (false, null, "Failed to deserialize settings.");

                AppDebug.Log("theInstanceManager", $"GamePlay settings imported from {filePath}");
                return (true, settings, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error importing gameplay settings: {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Enable all weapons
        /// </summary>
        public static OperationResult EnableAllWeapons()
        {
            try
            {
                var allEnabled = new WeaponEnablement(
                    true, true, true, true, true, true,
                    true, true, true, true, true, true,
                    true, true, true, true, true, true,
                    true, true, true, true, true, true
                );

                ApplyWeaponRestrictionsToInstance(allEnabled);

                AppDebug.Log("theInstanceManager", "All weapons enabled");
                return new OperationResult(true, "All weapons enabled successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error enabling all weapons: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Disable all weapons
        /// </summary>
        public static OperationResult DisableAllWeapons()
        {
            try
            {
                var allDisabled = new WeaponEnablement(
                    false, false, false, false, false, false,
                    false, false, false, false, false, false,
                    false, false, false, false, false, false,
                    false, false, false, false, false, false
                );

                ApplyWeaponRestrictionsToInstance(allDisabled);

                AppDebug.Log("theInstanceManager", "All weapons disabled");
                return new OperationResult(true, "All weapons disabled successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error disabling all weapons: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // VALIDATION HELPERS
        // ================================================================================

        private static bool ValidateServerPath(string path, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(path))
            {
                error = "Server path cannot be empty.";
                return false;
            }

            if (!Directory.Exists(path))
            {
                error = $"Server path does not exist: {path}";
                return false;
            }

            if (!File.Exists(Path.Combine(path, "dfbhd.exe")))
            {
                error = "Server path does not contain dfbhd.exe";
                return false;
            }

            return true;
        }

        private static bool ValidateBindIP(string ip, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(ip))
            {
                error = "Bind IP cannot be empty.";
                return false;
            }

            if (ip == "0.0.0.0")
                return true;

            if (!IPAddress.TryParse(ip, out IPAddress? address))
            {
                error = $"Invalid IP address format: {ip}";
                return false;
            }

            // Check if IP is available on this system
            var hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            if (!hostAddresses.Any(a => a.ToString() == ip))
            {
                error = $"IP address {ip} is not available on this system.";
                return false;
            }

            return true;
        }

        private static bool ValidatePort(int port, out string error)
        {
            error = string.Empty;

            if (port < 1 || port > 65535)
            {
                error = $"Port must be between 1 and 65535. Current: {port}";
                return false;
            }

            return true;
        }

        private static bool ValidateCountryCode(string code, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(code))
            {
                error = "Country code cannot be empty.";
                return false;
            }

            if (code.Length != 2)
            {
                error = $"Country code must be exactly 2 characters. Current: {code}";
                return false;
            }

            if (!code.All(char.IsLetter))
            {
                error = $"Country code must contain only letters. Current: {code}";
                return false;
            }

            return true;
        }

        private static bool ValidatePingRange(int min, int max, out string error)
        {
            error = string.Empty;

            if (min < 0 || min > 999)
            {
                error = $"Min ping must be between 0 and 999. Current: {min}";
                return false;
            }

            if (max < 0 || max > 999)
            {
                error = $"Max ping must be between 0 and 999. Current: {max}";
                return false;
            }

            if (min > max)
            {
                error = $"Min ping ({min}) cannot be greater than max ping ({max}).";
                return false;
            }

            return true;
        }

        // ================================================================================
        // INSTANCE APPLIERS
        // ================================================================================

        private static void ApplyProfileSettingsToInstance(ProfileSettings settings)
        {
            theInstance.profileServerPath = settings.ServerPath;
            theInstance.profileModFileName = settings.ModFileName;
            theInstance.gameHostName = settings.HostName;
            theInstance.gameServerName = settings.ServerName;
            theInstance.gameMOTD = settings.MOTD;
            theInstance.profileBindIP = settings.BindIP;
            theInstance.profileBindPort = settings.BindPort;
            theInstance.gamePasswordLobby = settings.LobbyPassword;
            theInstance.gameDedicated = settings.Dedicated;
            theInstance.gameLocalPlay = settings.LocalPlay;
            theInstance.gameRequireNova = settings.LocalPlay ? false : settings.RequireNova;
            theInstance.gameCountryCode = settings.CountryCode;
            theInstance.gameMinPing = settings.MinPingEnabled;
            theInstance.gameMaxPing = settings.MaxPingEnabled;
            theInstance.gameMinPingValue = settings.MinPingValue;
            theInstance.gameMaxPingValue = settings.MaxPingValue;
            theInstance.profileEnableRemote = settings.EnableRemote;
            theInstance.profileRemotePort = settings.RemotePort;
            // Map idle kick fields
            theInstance.gameEnableKickIdle = settings.EnableKickIdle;
            theInstance.gameKickIdleTime = settings.PlayerIdleLimit;

            theInstance.profileServerAttribute01 = settings.Attributes.Flag01;
            theInstance.profileServerAttribute02 = settings.Attributes.Flag02;
            theInstance.profileServerAttribute03 = settings.Attributes.Flag03;
            theInstance.profileServerAttribute04 = settings.Attributes.Flag04;
            theInstance.profileServerAttribute05 = settings.Attributes.Flag05;
            theInstance.profileServerAttribute06 = settings.Attributes.Flag06;
            theInstance.profileServerAttribute07 = settings.Attributes.Flag07;
            theInstance.profileServerAttribute08 = settings.Attributes.Flag08;
            theInstance.profileServerAttribute09 = settings.Attributes.Flag09;
            theInstance.profileServerAttribute10 = settings.Attributes.Flag10;
            theInstance.profileServerAttribute11 = settings.Attributes.Flag11;
            theInstance.profileServerAttribute12 = settings.Attributes.Flag12;
            theInstance.profileServerAttribute13 = settings.Attributes.Flag13;
            theInstance.profileServerAttribute14 = settings.Attributes.Flag14;
            theInstance.profileServerAttribute15 = settings.Attributes.Flag15;
            theInstance.profileServerAttribute16 = settings.Attributes.Flag16;
            theInstance.profileServerAttribute17 = settings.Attributes.Flag17;
            theInstance.profileServerAttribute18 = settings.Attributes.Flag18;
            theInstance.profileServerAttribute19 = settings.Attributes.Flag19;
            theInstance.profileServerAttribute20 = settings.Attributes.Flag20;
            theInstance.profileServerAttribute21 = settings.Attributes.Flag21;
        }

        private static void ApplyGamePlaySettingsToInstance(GamePlaySettings settings)
        {
            theInstance.gamePasswordBlue = settings.BluePassword;
            theInstance.gamePasswordRed = settings.RedPassword;
            theInstance.gamePasswordYellow = settings.YellowPassword;
            theInstance.gamePasswordViolet = settings.VioletPassword;
            theInstance.gameScoreZoneTime = settings.ScoreKOTH;
            theInstance.gameScoreKills = settings.ScoreDM;
            theInstance.gameScoreFlags = settings.ScoreFB;
            theInstance.gameTimeLimit = settings.TimeLimit;
            theInstance.gameLoopMaps = settings.LoopMaps;
            theInstance.gameStartDelay = settings.StartDelay;
            theInstance.gameRespawnTime = settings.RespawnTime;
            theInstance.gameScoreBoardDelay = settings.ScoreBoardDelay;
            theInstance.gameMaxSlots = settings.MaxSlots;
            theInstance.gamePSPTOTimer = settings.PSPTakeoverTimer;
            theInstance.gameFlagReturnTime = settings.FlagReturnTime;
            theInstance.gameFullWeaponThreshold = settings.FullWeaponThreshold;

        // Server options
        theInstance.gameOptionAutoBalance = settings.Options.AutoBalance;
        theInstance.gameOptionShowTracers = settings.Options.ShowTracers;
        theInstance.gameShowTeamClays = settings.Options.ShowClays;
        theInstance.gameOptionAutoRange = settings.Options.AutoRange;
        theInstance.gameCustomSkins = settings.Options.CustomSkins;
        theInstance.gameDestroyBuildings = settings.Options.DestroyBuildings;
        theInstance.gameFatBullets = settings.Options.FatBullets;
        theInstance.gameOneShotKills = settings.Options.OneShotKills;
        theInstance.gameAllowLeftLeaning = settings.Options.AllowLeftLeaning;
        theInstance.gameAllowRightLeaning = settings.Options.AllowRightLeaning;
        theInstance.gameEnableFourTeams = settings.Options.Enable4Teams;

            // Friendly fire
            theInstance.gameOptionFF = settings.FriendlyFire.Enabled;
            theInstance.gameFriendlyFireKills = settings.FriendlyFire.MaxKills;
            theInstance.gameOptionFFWarn = settings.FriendlyFire.WarnOnKill;
            theInstance.gameOptionFriendlyTags = settings.FriendlyFire.ShowFriendlyTags;

            // Roles
            theInstance.roleCQB = settings.Roles.CQB;
            theInstance.roleGunner = settings.Roles.Gunner;
            theInstance.roleSniper = settings.Roles.Sniper;
            theInstance.roleMedic = settings.Roles.Medic;

            // Weapons
            ApplyWeaponRestrictionsToInstance(settings.Weapons);
            ApplyrestrictedWeaponRestrictionsToInstance(settings.RestrictedWeapons);
        }

        private static void ApplyWeaponRestrictionsToInstance(WeaponEnablement weapons)
        {
            theInstance.weaponColt45 = weapons.Colt45;
            theInstance.weaponM9Beretta = weapons.M9Beretta;
            theInstance.weaponCar15 = weapons.CAR15;
            theInstance.weaponCar15203 = weapons.CAR15203;
            theInstance.weaponM16 = weapons.M16;
            theInstance.weaponM16203 = weapons.M16203;
            theInstance.weaponG3 = weapons.G3;
            theInstance.weaponG36 = weapons.G36;
            theInstance.weaponM60 = weapons.M60;
            theInstance.weaponM240 = weapons.M240;
            theInstance.weaponMP5 = weapons.MP5;
            theInstance.weaponSAW = weapons.SAW;
            theInstance.weaponMCRT300 = weapons.MCRT300;
            theInstance.weaponM21 = weapons.M21;
            theInstance.weaponM24 = weapons.M24;
            theInstance.weaponBarrett = weapons.Barrett;
            theInstance.weaponPSG1 = weapons.PSG1;
            theInstance.weaponShotgun = weapons.Shotgun;
            theInstance.weaponFragGrenade = weapons.FragGrenade;
            theInstance.weaponSmokeGrenade = weapons.SmokeGrenade;
            theInstance.weaponSatchelCharges = weapons.Satchel;
            theInstance.weaponAT4 = weapons.AT4;
            theInstance.weaponFlashGrenade = weapons.FlashBang;
            theInstance.weaponClaymore = weapons.Claymore;
        }

        private static void ApplyrestrictedWeaponRestrictionsToInstance(RestrictedWeapons RestrictedWeapons)
        {
            theInstance.restrictedWeaponColt45 = RestrictedWeapons.Colt45;
            theInstance.restrictedWeaponM9Beretta = RestrictedWeapons.M9Beretta;
            theInstance.restrictedWeaponCar15 = RestrictedWeapons.CAR15;
            theInstance.restrictedWeaponCar15203 = RestrictedWeapons.CAR15203;
            theInstance.restrictedWeaponM16 = RestrictedWeapons.M16;
            theInstance.restrictedWeaponM16203 = RestrictedWeapons.M16203;
            theInstance.restrictedWeaponG3 = RestrictedWeapons.G3;
            theInstance.restrictedWeaponG36 = RestrictedWeapons.G36;
            theInstance.restrictedWeaponM60 = RestrictedWeapons.M60;
            theInstance.restrictedWeaponM240 = RestrictedWeapons.M240;
            theInstance.restrictedWeaponMP5 = RestrictedWeapons.MP5;
            theInstance.restrictedWeaponSAW = RestrictedWeapons.SAW;
            theInstance.restrictedWeaponMCRT300 = RestrictedWeapons.MCRT300;
            theInstance.restrictedWeaponM21 = RestrictedWeapons.M21;
            theInstance.restrictedWeaponM24 = RestrictedWeapons.M24;
            theInstance.restrictedWeaponBarrett = RestrictedWeapons.Barrett;
            theInstance.restrictedWeaponPSG1 = RestrictedWeapons.PSG1;
            theInstance.restrictedWeaponShotgun = RestrictedWeapons.Shotgun;
            theInstance.restrictedWeaponFragGrenade = RestrictedWeapons.FragGrenade;
            theInstance.restrictedWeaponSmokeGrenade = RestrictedWeapons.SmokeGrenade;
            theInstance.restrictedWeaponSatchelCharges = RestrictedWeapons.Satchel;
            theInstance.restrictedWeaponAT4 = RestrictedWeapons.AT4;
            theInstance.restrictedWeaponFlashGrenade = RestrictedWeapons.FlashBang;
            theInstance.restrictedWeaponClaymore = RestrictedWeapons.Claymore;
        }

        // ================================================================================
        // EXISTING METHODS (PRESERVED)
        // ================================================================================

        public static bool ValidateGameServerPath()
        {
            if (string.IsNullOrWhiteSpace(theInstance.profileServerPath) || !Directory.Exists(theInstance.profileServerPath))
            {
                AppDebug.Log("theInstanceManager", "Profile server path is invalid or does not exist.");
                MessageBox.Show("The profile server path is invalid or does not exist. Please 'set' your server path and refresh your map list before starting the server.", "Invalid Profile Server Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static void UpdateGameServer()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return;
            
            // Update the DFV to match the current settings. On map change these should "apply".
            StartServer.createdfv();
            
            // Update Memory Locations
            ServerMemory.UpdateServerName();
            ServerMemory.UpdatePlayerHostName();
            ServerMemory.UpdateMOTD();
            ServerMemory.UpdateRequireNovaLogin();
            ServerMemory.UpdateTimeLimit();
            ServerMemory.UpdateLoopMaps();
            ServerMemory.UpdateStartDelay();
            ServerMemory.UpdateRespawnTime();
            ServerMemory.UpdateMaxSlots();
            ServerMemory.UpdateBluePassword();
            ServerMemory.UpdateRedPassword();
            ServerMemory.UpdateYellowPassword();
            ServerMemory.UpdateVioletPassword();
			ServerMemory.UpdateGamePlayOptions();
            ServerMemory.UpdatePSPTakeOverTime();
            ServerMemory.UpdateFlagReturnTime();
            ServerMemory.UpdateFriendlyFireKills();
            ServerMemory.UpdateMinPing();
            ServerMemory.UpdateMaxPing();
            ServerMemory.UpdateMinPingValue();
            ServerMemory.UpdateMaxPingValue();
            ServerMemory.UpdateAllowCustomSkins();
            ServerMemory.UpdateDestroyBuildings();
            ServerMemory.UpdateFatBullets();
            ServerMemory.UpdateOneShotKills();
            ServerMemory.UpdateWeaponRestrictions();
            ServerMemory.UpdateGameScores();

		}

        public static void InitializeTickers()
        {
            CommonCore.Ticker?.Start("ServerManager", 500, () => tickerServerManager.runTicker());
            CommonCore.Ticker?.Start("ChatManager", 100, () => tickerChatManagement.runTicker());
            CommonCore.Ticker?.Start("PlayerManager", 1000, () => tickerPlayerManagement.runTicker());
            CommonCore.Ticker?.Start("BanManager", 1000, () => tickerBanManagement.runTicker());
            CommonCore.Ticker?.Start("SessionCleanup", 60000, () => adminInstanceManager.CleanupStaleSessions(2));
        }

        public static void changeTeamGameMode(int currentMapType, int nextMapType, bool isCurrentMap4Team, bool isNextMap4Team)
        {
            bool isCurrentMapTeamMap = Functions.IsMapTeamBased(currentMapType);
            bool isNextMapTeamMap = Functions.IsMapTeamBased(nextMapType);

            AppDebug.Log("changeTeamGameMode", $"Transition: CurrentType={currentMapType} NextType={nextMapType} Current4Team={isCurrentMap4Team} Next4Team={isNextMap4Team}");

            // SCENARIO 1: Team-based → Non-team (2-team or 4-team → FFA/DM)
            if (isNextMapTeamMap == false && isCurrentMapTeamMap == true)
            {
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject playerObj = playerRecord.Value;
                    playerInstance.PlayerPreviousTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerTeam
                    });

                    // Assign all players to team 0 (Deathmatch/FFA)
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = (int)Teams.TEAM_GREEN // 0 = FFA
                    });
                }
                AppDebug.Log("changeTeamGameMode", "Assigned all players to FFA (Team 0)");
            }
            // SCENARIO 2: Non-team → Team-based (FFA/DM → 2-team or 4-team)
            else if (isNextMapTeamMap == true && isCurrentMapTeamMap == false)
            {
                // Restore players who were on teams before FFA
                foreach (playerTeamObject playerObj in playerInstance.PlayerPreviousTeamList)
                {
                    if (playerInstance.PlayerList.ContainsKey(playerObj.slotNum))
                    {
                        playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = playerObj.slotNum,
                            Team = playerObj.Team
                        });
                    }
                }

                // Balance new players who joined during FFA
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject player = playerRecord.Value;
                    bool found = playerInstance.PlayerPreviousTeamList.Any(p => p.slotNum == player.PlayerSlot);

                    if (!found)
                    {
                        int assignedTeam = BalanceNewPlayer(isNextMap4Team);
                        
                        playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = player.PlayerSlot,
                            Team = assignedTeam
                        });
                        playerInstance.PlayerPreviousTeamList.Add(new playerTeamObject
                        {
                            slotNum = player.PlayerSlot,
                            Team = assignedTeam
                        });

                        AppDebug.Log("changeTeamGameMode", $"Balanced new player slot {player.PlayerSlot} to team {assignedTeam}");
                    }
                }
                playerInstance.PlayerPreviousTeamList.Clear();
            }
            // SCENARIO 3: 2-team → 4-team
            else if (isCurrentMapTeamMap && isNextMapTeamMap && !isCurrentMap4Team && isNextMap4Team)
            {
                // Split existing 2 teams into 4 teams
                var blueTeamPlayers = playerInstance.PlayerList.Values.Where(p => p.PlayerTeam == (int)Teams.TEAM_BLUE).ToList();
                var redTeamPlayers = playerInstance.PlayerList.Values.Where(p => p.PlayerTeam == (int)Teams.TEAM_RED).ToList();

                // Split Blue team → Blue (1) and Yellow (3)
                for (int i = 0; i < blueTeamPlayers.Count; i++)
                {
                    int newTeam = (i % 2 == 0) ? (int)Teams.TEAM_BLUE : (int)Teams.TEAM_YELLOW;
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = blueTeamPlayers[i].PlayerSlot,
                        Team = newTeam
                    });
                }

                // Split Red team → Red (2) and Purple (4)
                for (int i = 0; i < redTeamPlayers.Count; i++)
                {
                    int newTeam = (i % 2 == 0) ? (int)Teams.TEAM_RED : (int)Teams.TEAM_PURPLE;
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = redTeamPlayers[i].PlayerSlot,
                        Team = newTeam
                    });
                }

                AppDebug.Log("changeTeamGameMode", $"Split 2 teams into 4: {blueTeamPlayers.Count} blue→blue/yellow, {redTeamPlayers.Count} red→red/purple");
            }
            // SCENARIO 4: 4-team → 2-team
            else if (isCurrentMapTeamMap && isNextMapTeamMap && isCurrentMap4Team && !isNextMap4Team)
            {
                // Merge 4 teams into 2 teams
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject player = playerRecord.Value;
                    int newTeam;

                    // Blue (1) and Yellow (3) → Blue (1)
                    // Red (2) and Purple (4) → Red (2)
                    if (player.PlayerTeam == (int)Teams.TEAM_BLUE || player.PlayerTeam == (int)Teams.TEAM_YELLOW)
                    {
                        newTeam = (int)Teams.TEAM_BLUE;
                    }
                    else // Red or Purple
                    {
                        newTeam = (int)Teams.TEAM_RED;
                    }

                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = player.PlayerSlot,
                        Team = newTeam
                    });
                }

                AppDebug.Log("changeTeamGameMode", "Merged 4 teams into 2 (Blue/Yellow→Blue, Red/Purple→Red)");
            }
            // SCENARIO 5: Same team structure (2→2 or 4→4) - no changes needed
            else if (isCurrentMapTeamMap && isNextMapTeamMap)
            {
                AppDebug.Log("changeTeamGameMode", $"Same team structure ({(isCurrentMap4Team ? "4" : "2")}→{(isNextMap4Team ? "4" : "2")}), no team changes");
            }
        }

        /// <summary>
        /// Balances a new player to the smallest team
        /// </summary>
        private static int BalanceNewPlayer(bool is4TeamMode)
        {
            if (is4TeamMode)
            {
                // Count players in each of the 4 teams (1, 2, 3, 4)
                int blueCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_BLUE);
                int redCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_RED);
                int yellowCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_YELLOW);
                int purpleCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_PURPLE);

                // Find minimum count
                int minCount = new[] { blueCount, redCount, yellowCount, purpleCount }.Min();

                // Assign to smallest team (with random tiebreaker)
                var smallestTeams = new List<int>();
                if (blueCount == minCount) smallestTeams.Add((int)Teams.TEAM_BLUE);
                if (redCount == minCount) smallestTeams.Add((int)Teams.TEAM_RED);
                if (yellowCount == minCount) smallestTeams.Add((int)Teams.TEAM_YELLOW);
                if (purpleCount == minCount) smallestTeams.Add((int)Teams.TEAM_PURPLE);

                Random rand = new Random();
                return smallestTeams[rand.Next(smallestTeams.Count)];
            }
            else
            {
                // 2-team balancing (Blue=1, Red=2)
                int blueteam = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_BLUE);
                int redteam = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_RED);

                if (blueteam > redteam)
                {
                    return (int)Teams.TEAM_RED;
                }
                else if (blueteam < redteam)
                {
                    return (int)Teams.TEAM_BLUE;
                }
                else
                {
                    Random rand = new Random();
                    return rand.Next(1, 3) == 1 ? (int)Teams.TEAM_BLUE : (int)Teams.TEAM_RED;
                }
            }
        }

        public static void GenerateMatchID()
        {
            int currentDate = int.Parse(DateTime.Now.ToString("yyMMdd"));
            int currentMatchID = theInstance.gameMatchID;

            if (currentMatchID == 0)
            {
                theInstance.gameMatchID = currentDate * 1000 + 1;
                AppDebug.Log("GenerateMatchID", $"Match ID: {theInstance.gameMatchID}");
                ServerSettings.Set("gameMatchID", theInstance.gameMatchID);
                return;
            }

            int lastDate = currentMatchID / 1000;
            int lastMatchNumber = currentMatchID % 1000;

            if (lastDate == currentDate)
            {
                theInstance.gameMatchID = currentDate * 1000 + (lastMatchNumber + 1);
            }
            else
            {
                theInstance.gameMatchID = currentDate * 1000 + 1;
            }
    
            AppDebug.Log("GenerateMatchID", $"Match ID: {theInstance.gameMatchID}");
            ServerSettings.Set("gameMatchID", theInstance.gameMatchID);
        }

        // ================================================================================
        // WEB STATS SETTINGS MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Test connection to Babstats server
        /// </summary>
        public static async Task<OperationResult> TestWebStatsConnectionAsync(string serverPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serverPath))
                    return new OperationResult(false, "Server path cannot be empty.");

                bool result = await statsInstanceManager.TestBabstatsConnectionAsync(serverPath);

                return result
                    ? new OperationResult(true, "Connection successful.")
                    : new OperationResult(false, "Connection failed. Please verify the server path.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", $"Error testing connection: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // REMOTE API MANAGEMENT
        // ================================================================================

        /// <summary>
        /// Check and start/stop the embedded API based on profile settings.
        /// Called from ticker to ensure API state matches configuration.
        /// </summary>
        public static void ManageEmbeddedApi()
        {
            try
            {
                bool shouldBeRunning = theInstance.profileEnableRemote;
                bool isCurrentlyRunning = APICore.IsApiRunning;
                int configuredPort = theInstance.profileRemotePort;

                // Case 1: Should be running but isn't
                if (shouldBeRunning && !isCurrentlyRunning)
                {
                    AppDebug.Log("theInstanceManager", 
                        $"Remote API enabled but not running. Starting on port {configuredPort}...");
            
                    try
                    {
                        APICore.StartApiHost(configuredPort);
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("theInstanceManager", 
                            $"Failed to start API host: {ex.Message}");
                    }
                }
                // Case 2: Should NOT be running but is
                else if (!shouldBeRunning && isCurrentlyRunning)
                {
                    AppDebug.Log("theInstanceManager", 
                        "Remote API disabled but currently running. Stopping...");
            
                    // Fire and forget - async shutdown
                    _ = APICore.StopApiHost();
                }
                // Case 3 & 4: Already in correct state (no action needed)
            }
            catch (Exception ex)
            {
                AppDebug.Log("theInstanceManager", 
                    $"Error managing embedded API: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the current status of the embedded API
        /// /// <returns>Tuple with (isEnabled, isRunning, port)</returns>
        public static (bool isEnabled, bool isRunning, int port) GetApiStatus()
        {
            return (
                isEnabled: theInstance.profileEnableRemote,
                isRunning: APICore.IsApiRunning,
                port: theInstance.profileRemotePort
            );
        }

        /// <summary>
        /// Restart the API host on a new port (useful when port changes)
        /// </summary>
        public static async Task RestartApiHost(int newPort)
        {
            AppDebug.Log("theInstanceManager", $"Restarting API host on new port {newPort}");
            await APICore.RestartApiHost(newPort);
        }

    }
}