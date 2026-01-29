namespace HawkSyncShared.DTOs;

public record ServerSnapshot
{
    public ServerInstanceDTO Server { get; init; } = new();
    public PlayerInstanceDTO Players { get; init; } = new();
    public ChatInstanceDTO Chat { get; init; } = new();
    public BanInstanceDTO Bans { get; init; } = new();
    public MapInstanceDTO Maps { get; init; } = new();
    public DateTime SnapshotTime { get; init; } = DateTime.UtcNow;
}

public record ServerInstanceDTO
{
    public string Status { get; init; } = "OFFLINE";
    public string ServerName { get; init; } = string.Empty;
    public string MOTD { get; init; } = string.Empty;
    public string HostName { get; init; } = string.Empty;
    public int MaxSlots { get; init; }
    public int CurrentPlayers { get; init; }
    public int BlueScore { get; init; }
    public int RedScore { get; init; }
    public TimeSpan TimeRemaining { get; init; }
    public string CurrentMap { get; init; } = string.Empty;
    public int GameType { get; init; }
}

public record PlayerInstanceDTO
{
    public Dictionary<int, PlayerDTO> Players { get; init; } = new();
}

public record PlayerDTO
{
    public int Slot { get; init; }
    public string Name { get; init; } = string.Empty;
    public string IPAddress { get; init; } = string.Empty;
    public int Team { get; init; }
    public int Ping { get; init; }
    public int Kills { get; init; }
    public int Deaths { get; init; }
}

public record ChatInstanceDTO
{
    public List<ChatMessageDTO> RecentMessages { get; init; } = new();
    public List<SlapMessageDTO> SlapMessages { get; init; } = new();
}

public record ChatMessageDTO
{
    public DateTime Timestamp { get; init; }
    public string PlayerName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Team { get; init; } = string.Empty;
}

public record SlapMessageDTO(int Id, string Text);

public record BanInstanceDTO
{
    public List<BannedNameDTO> BannedNames { get; init; } = new();
    public List<BannedIPDTO> BannedIPs { get; init; } = new();
    public List<WhitelistedNameDTO> WhitelistedNames { get; init; } = new();
}

public record BannedNameDTO(int RecordID, string Name, DateTime BanDate, string RecordType);
public record BannedIPDTO(int RecordID, string IP, int Subnet, DateTime BanDate, string RecordType);
public record WhitelistedNameDTO(int RecordID, string Name);

public record MapInstanceDTO
{
    public Dictionary<int, List<MapDTO>> Playlists { get; init; } = new();
    public int ActivePlaylist { get; init; }
    public string CurrentMap { get; init; } = string.Empty;
    public int CurrentMapIndex { get; init; }
    public string NextMap { get; init; } = string.Empty;
}

public record MapDTO(int ID, string Name, string File, int GameType);