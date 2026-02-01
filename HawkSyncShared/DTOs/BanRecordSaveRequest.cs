using HawkSyncShared.Instances;

namespace HawkSyncShared.DTOs;

public class BanRecordSaveRequest
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

public class ProxyCheckTestRequest
{
    public string ApiKey { get; set; } = string.Empty;
    public int ServiceProvider { get; set; }
    public string IPAddress { get; set; } = string.Empty;
}

public class ProxyCheckTestResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsVpn { get; set; }
    public bool IsProxy { get; set; }
    public bool IsTor { get; set; }
    public int RiskScore { get; set; }
    public string? CountryName { get; set; }
    public string? CountryCode { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Provider { get; set; }
}

public class NetLimiterSettingsRequest
{
    public bool NetLimiterEnabled { get; set; }
    public string NetLimiterHost { get; set; } = string.Empty;
    public int NetLimiterPort { get; set; }
    public string NetLimiterUsername { get; set; } = string.Empty;
    public string NetLimiterPassword { get; set; } = string.Empty;
    public string NetLimiterFilterName { get; set; } = string.Empty;
    public bool NetLimiterEnableConLimit { get; set; }
    public int NetLimiterConThreshold { get; set; }
}