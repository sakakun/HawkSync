using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    /// <summary>
    /// Get audit logs with optional filtering
    /// </summary>
    [HttpPost("logs")]
    public ActionResult<AuditLogResponse> GetAuditLogs([FromBody] AuditLogRequest request)
    {
        AppDebug.Log("AuditController", "GetAuditLogs endpoint called");
        
        var username = User.FindFirst("username")?.Value ?? "Unknown";
        AppDebug.Log("AuditController", $"Request from user: {username}");
        
        if (!HasPermission("audit"))
        {
            AppDebug.Log("AuditController", $"User {username} does not have 'audit' permission - Forbidden");
            return Forbid();
        }

        AppDebug.Log("AuditController", "Permission check passed");

        // Default to last 24 hours if no dates specified
        var startDate = request.StartDate ?? DateTime.Now.AddHours(-24);
        var endDate = request.EndDate ?? DateTime.Now;

        AppDebug.Log("AuditController", $"Calling DatabaseManager.GetAuditLogs - Start: {startDate}, End: {endDate}, User: {request.UsernameFilter}, Category: {request.CategoryFilter}");

        var (logs, totalCount) = DatabaseManager.GetAuditLogs(
            startDate,
            endDate,
            request.UsernameFilter,
            request.CategoryFilter,
            request.ActionTypeFilter,
            request.TargetFilter,
            request.SuccessOnly,
            request.Limit,
            request.Offset
        );

        AppDebug.Log("AuditController", $"Returning {logs.Count} logs out of {totalCount} total");

        return Ok(new AuditLogResponse
        {
            Logs = logs,
            TotalCount = totalCount,
            HasMore = (request.Offset + logs.Count) < totalCount
        });
    }

    /// <summary>
    /// Get available audit log categories for filter dropdown
    /// </summary>
    [HttpGet("categories")]
    public ActionResult<List<string>> GetCategories()
    {
        if (!HasPermission("audit"))
            return Forbid();

        // Return both database categories and predefined constants
        var dbCategories = DatabaseManager.GetAuditCategories();
        var allCategories = new HashSet<string>(dbCategories)
        {
            AuditCategory.Ban,
            AuditCategory.Chat,
            AuditCategory.Player,
            AuditCategory.Map,
            AuditCategory.Settings,
            AuditCategory.User,
            AuditCategory.System,
            AuditCategory.Server
        };

        return Ok(allCategories.OrderBy(c => c).ToList());
    }

    /// <summary>
    /// Get available audit action types for filter dropdown
    /// </summary>
    [HttpGet("actiontypes")]
    public ActionResult<List<string>> GetActionTypes()
    {
        if (!HasPermission("audit"))
            return Forbid();

        // Return both database action types and predefined constants
        var dbActionTypes = DatabaseManager.GetAuditActionTypes();
        var allActionTypes = new HashSet<string>(dbActionTypes)
        {
            AuditAction.Create,
            AuditAction.Update,
            AuditAction.Delete,
            AuditAction.Execute,
            AuditAction.Login,
            AuditAction.Logout,
            AuditAction.Start,
            AuditAction.Stop
        };

        return Ok(allActionTypes.OrderBy(a => a).ToList());
    }

    /// <summary>
    /// Get audit log statistics for dashboard/summary
    /// </summary>
    [HttpGet("stats")]
    public ActionResult<AuditStatsDTO> GetStats([FromQuery] int hours = 24)
    {
        if (!HasPermission("audit"))
            return Forbid();

        var startDate = DateTime.Now.AddHours(-hours);
        var (logs, _) = DatabaseManager.GetAuditLogs(
            startDate: startDate,
            endDate: DateTime.Now,
            limit: 10000 // Get all for stats
        );

        var stats = new AuditStatsDTO
        {
            TotalActions = logs.Count,
            SuccessfulActions = logs.Count(l => l.Success),
            FailedActions = logs.Count(l => !l.Success),
            UniqueUsers = logs.Select(l => l.Username).Distinct().Count(),
            ActionsByCategory = logs.GroupBy(l => l.ActionCategory)
                                    .ToDictionary(g => g.Key, g => g.Count()),
            ActionsByType = logs.GroupBy(l => l.ActionType)
                                .ToDictionary(g => g.Key, g => g.Count()),
            MostActiveUsers = logs.GroupBy(l => l.Username)
                                  .OrderByDescending(g => g.Count())
                                  .Take(5)
                                  .Select(g => new UserActivityDTO 
                                  { 
                                      Username = g.Key, 
                                      ActionCount = g.Count() 
                                  })
                                  .ToList(),
            RecentActions = logs.OrderByDescending(l => l.Timestamp)
                                .Take(10)
                                .ToList()
        };

        return Ok(stats);
    }

    /// <summary>
    /// Check if current user has specific permission
    /// </summary>
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        AppDebug.Log("AuditController", $"HasPermission check for '{permission}' - User has permissions: [{string.Join(", ", permissions)}]");
        bool hasIt = permissions.Contains(permission);
        AppDebug.Log("AuditController", $"Permission '{permission}' check result: {hasIt}");
        return hasIt;
    }
}

