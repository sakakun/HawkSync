using System;
using System.Collections.Generic;

namespace HawkSyncShared.DTOs.Audit;

/// <summary>
/// Audit log entry DTO
/// </summary>
public record AuditLogDTO
{
    public int LogID { get; init; }
    public DateTime Timestamp { get; init; }
    public int? UserID { get; init; }
    public string Username { get; init; } = string.Empty;
    public string ActionCategory { get; init; } = string.Empty;
    public string ActionType { get; init; } = string.Empty;
    public string ActionDescription { get; init; } = string.Empty;
    public string? TargetType { get; init; }
    public string? TargetID { get; init; }
    public string? TargetName { get; init; }
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    public string? IPAddress { get; init; }
    public bool Success { get; init; } = true;
    public string? ErrorMessage { get; init; }
    public string? Metadata { get; init; }
}

/// <summary>
/// Request for querying audit logs with filters
/// </summary>
public record AuditLogRequest
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? UsernameFilter { get; init; }
    public string? CategoryFilter { get; init; }
    public string? ActionTypeFilter { get; init; }
    public string? TargetFilter { get; init; }
    public bool? SuccessOnly { get; init; }
    public int Limit { get; init; } = 1000;
    public int Offset { get; init; } = 0;
}

/// <summary>
/// Response containing audit logs and pagination info
/// </summary>
public record AuditLogResponse
{
    public List<AuditLogDTO> Logs { get; init; } = new();
    public int TotalCount { get; init; }
    public bool HasMore { get; init; }
}

/// <summary>
/// Statistics summary for audit logs
/// </summary>
public record AuditStatsDTO
{
    public int TotalActions { get; init; }
    public int SuccessfulActions { get; init; }
    public int FailedActions { get; init; }
    public int UniqueUsers { get; init; }
    public Dictionary<string, int> ActionsByCategory { get; init; } = new();
    public Dictionary<string, int> ActionsByType { get; init; } = new();
    public List<UserActivityDTO> MostActiveUsers { get; init; } = new();
    public List<AuditLogDTO> RecentActions { get; init; } = new();
}

/// <summary>
/// User activity summary
/// </summary>
public record UserActivityDTO
{
    public string Username { get; init; } = string.Empty;
    public int ActionCount { get; init; }
}

/// <summary>
/// Constants for audit log categories
/// </summary>
public static class AuditCategory
{
    public const string Ban = "Ban";
    public const string Chat = "Chat";
    public const string Player = "Player";
    public const string Map = "Map";
    public const string Settings = "Settings";
    public const string User = "User";
    public const string System = "System";
    public const string Stats = "Stats";
    public const string Server = "Server";
}

/// <summary>
/// Constants for audit action types
/// </summary>
public static class AuditAction
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Execute = "Execute";
    public const string Login = "Login";
    public const string Logout = "Logout";
    public const string Connect = "Connect";
    public const string Disconnect = "Disconnect";
    public const string Start = "Start";
    public const string Stop = "Stop";
}

