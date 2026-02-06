using HawkSyncShared.Instances;

namespace HawkSyncShared.DTOs.tabBans;

// DTO for import/export
public class BlacklistImportModel
{
    public List<banInstancePlayerName>? BannedPlayerNames { get; set; }
    public List<banInstancePlayerIP>? BannedPlayerIPs { get; set; }
}

public class BanDTOs
{
    public int? NameRecordID { get; set; }
    public int? IPRecordID { get; set; }
    public string? PlayerName { get; set; }
    public string? IPAddress { get; set; }
    public int? SubnetMask { get; set; }
    public DateTime BanDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public banInstanceRecordType RecordType { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsName { get; set; }
    public bool IsIP { get; set; }
    public bool IgnoreValidation { get; set; } = false;
}

public class BanRecordSaveResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? NameRecordID { get; set; }
    public int? IPRecordID { get; set; }
}

public enum RecordDeleteAction
{
    None,
    NameOnly,
    IPOnly,
    Both
}

public class DeleteBanRecordRequest
{
    public int RecordID { get; set; }
    public bool IsName { get; set; }
}