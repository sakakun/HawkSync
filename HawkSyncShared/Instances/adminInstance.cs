using HawkSyncShared.DTOs.tabAdmin;

namespace HawkSyncShared.Instances;

/// <summary>
/// Contains admin user management data
/// NOTE: This is NOT broadcast to remote clients for security
/// Remote clients should use REST API to query/manage users
/// </summary>
public class adminInstance
{
    // Cache of users (loaded from database)
    public List<UserDTO> Users { get; set; } = new();

    // Session tracking
    public Dictionary<string, DateTime> ActiveSessions { get; set; } = new();

    // Audit log
    public List<AdminAuditLog> AuditLog { get; set; } = new();
    public bool ForceUIUpdate = false;
}

public record AdminAuditLog
{
    public DateTime Timestamp;
    public required string Username;
    public required string Action;
    public required string Details;
}