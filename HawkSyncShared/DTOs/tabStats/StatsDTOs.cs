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
    /// <summary>
    /// Represents a request containing server settings for a Babstats server operation.
    /// </summary>
    /// <param name="Server">The server settings to be used for the Babstats server request. Cannot be null.</param>
    public record BabstatsServerRequest(
        BabstatsServerSettings Server
    );

    /// <summary>
    /// Represents a request to configure or interact with a lobby server using the specified server settings.
    /// </summary>
    /// <param name="Server">The settings that define the configuration and behavior of the lobby server for this request. Cannot be null.</param>
    public record LobbyServerRequest(
        LobbyServerSettings Server
	);

    /// <summary>
    /// DTO for web stats validation request.
    /// </summary>
    public class WebStatsValidateRequest
    {
        public string ServerPath { get; set; } = string.Empty;
    }

    // Add alongside BabstatsServerSettings for lobby reporting servers
    public record LobbyServerSettings(
        int LobbyServerID,
        string SiteName,
        string ServerUri,
        int GamePort,
        string SecretKey,
        bool IsEnabled,
        int SortOrder
    );

}
