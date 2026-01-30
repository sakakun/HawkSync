namespace HawkSyncShared.DTOs;

/// <summary>
/// Profile settings request for updates
/// </summary>
public record ProfileSettingsRequest
{
    public string ServerPath { get; init; } = string.Empty;
    public string ModFileName { get; init; } = string.Empty;
    public string HostName { get; init; } = string.Empty;
    public string ServerName { get; init; } = string.Empty;
    public string MOTD { get; init; } = string.Empty;
    public string BindIP { get; init; } = string.Empty;
    public int BindPort { get; init; }
    public string LobbyPassword { get; init; } = string.Empty;
    public bool Dedicated { get; init; }
    public bool RequireNova { get; init; }
    public string CountryCode { get; init; } = string.Empty;
    public bool MinPingEnabled { get; init; }
    public bool MaxPingEnabled { get; init; }
    public int MinPingValue { get; init; }
    public int MaxPingValue { get; init; }
    public bool EnableRemote { get; init; }
    public int RemotePort { get; init; }
    public CommandLineFlagsDTO Attributes { get; init; } = new();
}

/// <summary>
/// Profile settings response
/// </summary>
public record ProfileSettingsResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public ProfileSettingsData? Settings { get; init; }
}

public record ProfileSettingsData
{
    public string ServerPath { get; init; } = string.Empty;
    public string ModFileName { get; init; } = string.Empty;
    public string HostName { get; init; } = string.Empty;
    public string ServerName { get; init; } = string.Empty;
    public string MOTD { get; init; } = string.Empty;
    public string BindIP { get; init; } = string.Empty;
    public int BindPort { get; init; }
    public string LobbyPassword { get; init; } = string.Empty;
    public bool Dedicated { get; init; }
    public bool RequireNova { get; init; }
    public string CountryCode { get; init; } = string.Empty;
    public bool MinPingEnabled { get; init; }
    public bool MaxPingEnabled { get; init; }
    public int MinPingValue { get; init; }
    public int MaxPingValue { get; init; }
    public bool EnableRemote { get; init; }
    public int RemotePort { get; init; }
    public CommandLineFlagsDTO Attributes { get; init; } = new();
}

public record CommandLineFlagsDTO
{
    public bool Flag01 { get; init; }
    public bool Flag02 { get; init; }
    public bool Flag03 { get; init; }
    public bool Flag04 { get; init; }
    public bool Flag05 { get; init; }
    public bool Flag06 { get; init; }
    public bool Flag07 { get; init; }
    public bool Flag08 { get; init; }
    public bool Flag09 { get; init; }
    public bool Flag10 { get; init; }
    public bool Flag11 { get; init; }
    public bool Flag12 { get; init; }
    public bool Flag13 { get; init; }
    public bool Flag14 { get; init; }
    public bool Flag15 { get; init; }
    public bool Flag16 { get; init; }
    public bool Flag17 { get; init; }
    public bool Flag18 { get; init; }
    public bool Flag19 { get; init; }
    public bool Flag20 { get; init; }
    public bool Flag21 { get; init; }
}

public record ValidationResult
{
    public bool IsValid { get; init; }
    public List<string> Errors { get; init; } = new();
}