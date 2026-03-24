using System;
using System.Collections.Generic;

namespace HawkSyncShared.DTOs.tabStats
{
    /// <summary>
    /// Legacy single-server web stats configuration settings.
    /// Kept for backward compatibility.
    /// </summary>
    public record WebStatsSettings(
        string ProfileID,
        bool Enabled,
        string ServerPath,
        bool Announcements,
        int ReportInterval,
        int UpdateInterval
    );

    /// <summary>
    /// Multi-server Babstats settings row.
    /// </summary>
    public record BabstatsServerSettings(
        int BabstatsServerID,
        string DisplayName,
        string ServerPath,
        string ProfileID,
        bool IsEnabled,
        bool EnableAnnouncements,
        int ReportIntervalSeconds,
        int UpdateIntervalSeconds,
        int SortOrder
    );

    public record WebStatsServersResponse(
        List<BabstatsServerSettings> Servers
    );

    public record SaveBabstatsServerRequest(
        BabstatsServerSettings Server
    );

    /// <summary>
    /// DTO for web stats validation request.
    /// </summary>
    public class WebStatsValidateRequest
    {
        public string ServerPath { get; set; } = string.Empty;
    }
}
