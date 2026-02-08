namespace HawkSyncShared.DTOs.tabBans.Service;

public class ProxyCheck
{
    public bool ProxyCheckEnabled { get; set; }
    public string? ProxyAPIKey { get; set; }
    public int ProxyCacheDays { get; set; }
    public int ProxyAction { get; set; }
    public int VPNAction { get; set; }
    public int TORAction { get; set; }
    public int GeoMode { get; set; }
    public int ServiceProvider { get; set; }
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

/// <summary>
/// DTOs for add/remove country requests.
/// </summary>
public class AddBlockedCountryRequest
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
}
public class RemoveBlockedCountryRequest
{
    public int RecordID { get; set; }
}