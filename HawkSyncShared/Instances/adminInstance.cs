using System.Collections.Generic;
using HawkSyncShared.DTOs;

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

public class AdminAuditLog
{
    public DateTime Timestamp { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}