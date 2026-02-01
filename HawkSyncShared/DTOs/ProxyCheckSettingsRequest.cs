namespace HawkSyncShared.DTOs;

public class ProxyCheckSettingsRequest
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