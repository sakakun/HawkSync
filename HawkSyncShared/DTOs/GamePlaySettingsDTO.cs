namespace HawkSyncShared.DTOs;

/// <summary>
/// Server gameplay options DTO
/// </summary>
public record ServerOptionsDTO(
    bool AutoBalance,
    bool ShowTracers,
    bool ShowClays,
    bool AutoRange,
    bool CustomSkins,
    bool DestroyBuildings,
    bool FatBullets,
    bool OneShotKills,
    bool AllowLeftLeaning
);

/// <summary>
/// Friendly fire settings DTO
/// </summary>
public record FriendlyFireSettingsDTO(
    bool Enabled,
    int MaxKills,
    bool WarnOnKill,
    bool ShowFriendlyTags
);

/// <summary>
/// Role restrictions DTO
/// </summary>
public record RoleRestrictionsDTO(
    bool CQB,
    bool Gunner,
    bool Sniper,
    bool Medic
);

/// <summary>
/// Weapon restrictions DTO
/// </summary>
public record WeaponRestrictionsDTO(
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
/// Request to save gameplay settings
/// </summary>
public class GamePlaySettingsRequest
{
    // Lobby Passwords
    public string BluePassword { get; set; } = string.Empty;
    public string RedPassword { get; set; } = string.Empty;

    // Win Conditions
    public int ScoreKOTH { get; set; }
    public int ScoreDM { get; set; }
    public int ScoreFB { get; set; }

    // Server Values
    public int TimeLimit { get; set; }
    public int LoopMaps { get; set; }
    public int StartDelay { get; set; }
    public int RespawnTime { get; set; }
    public int ScoreBoardDelay { get; set; }
    public int MaxSlots { get; set; }
    public int PSPTakeoverTimer { get; set; }
    public int FlagReturnTime { get; set; }
    public int MaxTeamLives { get; set; }

    // Grouped Settings
    public ServerOptionsDTO Options { get; set; } = new(false, false, false, false, false, false, false, false, false);
    public FriendlyFireSettingsDTO FriendlyFire { get; set; } = new(false, 0, false, false);
    public RoleRestrictionsDTO Roles { get; set; } = new(false, false, false, false);
    public WeaponRestrictionsDTO Weapons { get; set; } = new(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
}

/// <summary>
/// Gameplay settings data
/// </summary>
public class GamePlaySettingsData
{
    // Lobby Passwords
    public string BluePassword { get; set; } = string.Empty;
    public string RedPassword { get; set; } = string.Empty;

    // Win Conditions
    public int ScoreKOTH { get; set; }
    public int ScoreDM { get; set; }
    public int ScoreFB { get; set; }

    // Server Values
    public int TimeLimit { get; set; }
    public int LoopMaps { get; set; }
    public int StartDelay { get; set; }
    public int RespawnTime { get; set; }
    public int ScoreBoardDelay { get; set; }
    public int MaxSlots { get; set; }
    public int PSPTakeoverTimer { get; set; }
    public int FlagReturnTime { get; set; }
    public int MaxTeamLives { get; set; }

    // Grouped Settings
    public ServerOptionsDTO Options { get; set; } = new(false, false, false, false, false, false, false, false, false);
    public FriendlyFireSettingsDTO FriendlyFire { get; set; } = new(false, 0, false, false);
    public RoleRestrictionsDTO Roles { get; set; } = new(false, false, false, false);
    public WeaponRestrictionsDTO Weapons { get; set; } = new(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
}

/// <summary>
/// Response for gameplay settings request
/// </summary>
public class GamePlaySettingsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public GamePlaySettingsData? Settings { get; set; }
}

/// <summary>
/// Response for exporting gameplay settings
/// </summary>
public class GamePlaySettingsExportResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string JsonData { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}

/// <summary>
/// Request for importing gameplay settings
/// </summary>
public class GamePlaySettingsImportRequest
{
    public string JsonData { get; set; } = string.Empty;
}