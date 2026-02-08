using System;
using System.Collections.Generic;
using System.Text;

namespace HawkSyncShared.DTOs.tabBans.Service;

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

/// <summary>
/// DTO for NetLimiter filters response.
/// </summary>
public class NetLimiterFiltersResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string>? Filters { get; set; }
}