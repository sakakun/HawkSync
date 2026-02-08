using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkSyncShared.DTOs.tabStats
{
    /// <summary>
    /// Web stats configuration settings
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
    /// DTO for web stats validation request.
    /// </summary>
    public class WebStatsValidateRequest
    {
        public string ServerPath { get; set; } = string.Empty;
    }
}
