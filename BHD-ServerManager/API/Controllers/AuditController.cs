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
        if (!HasPermission("audit"))
            return Forbid();

        try
        {
            // Default to last 24 hours if no dates specified
            var startDate = request.StartDate ?? DateTime.UtcNow.AddHours(-24);
            var endDate = request.EndDate ?? DateTime.UtcNow;

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

            return Ok(new AuditLogResponse
            {
                Logs = logs,
                TotalCount = totalCount,
                HasMore = (request.Offset + logs.Count) < totalCount
            });

        }
        catch (Exception ex)
        {
            // Log the exception server-side (keep existing logger)
            AppDebug.Log("Error retrieving audit logs", AppDebug.LogLevel.Error, ex);

            // Add a trace id to the response so clients can report it to support
            var traceId = HttpContext.TraceIdentifier;
            Response.Headers["X-Trace-Id"] = traceId;

            // Return a ProblemDetails (RFC 7807) with 500 status without leaking exception details
            return Problem(
                detail: "An unexpected error occurred while retrieving audit logs. Provide the X-Trace-Id header to support.",
                statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
            );
        }

    }

    /// <summary>
    /// Get available audit log categories for filter dropdown
    /// </summary>
    [HttpGet("categories")]
    public ActionResult<List<string>> GetCategories()
    {
        if (!HasPermission("audit"))
            return Forbid();

        try
        {
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
        catch (Exception ex)
        {
            // Log the exception server-side (keep existing logger)
            AppDebug.Log("Error retrieving audit categories", AppDebug.LogLevel.Error, ex);

            // Add a trace id to the response so clients can report it to support
            var traceId = HttpContext.TraceIdentifier;
            Response.Headers["X-Trace-Id"] = traceId;

            // Return a ProblemDetails (RFC 7807) with 500 status without leaking exception details
            return Problem(
                detail: "An unexpected error occurred while retrieving audit categories. Provide the X-Trace-Id header to support.",
                statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
            );
        }
    }

    /// <summary>
    /// Get available audit action types for filter dropdown
    /// </summary>
    [HttpGet("actiontypes")]
    public ActionResult<List<string>> GetActionTypes()
    {
        if (!HasPermission("audit"))
            return Forbid();

        try
        {
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
        catch (Exception ex)
        {
            // Log the exception server-side (keep existing logger)
            AppDebug.Log("Error retrieving audit action types", AppDebug.LogLevel.Error, ex);

            // Add a trace id to the response so clients can report it to support
            var traceId = HttpContext.TraceIdentifier;
            Response.Headers["X-Trace-Id"] = traceId;

            // Return a ProblemDetails (RFC 7807) with 500 status without leaking exception details
            return Problem(
                detail: "An unexpected error occurred while retrieving audit action types. Provide the X-Trace-Id header to support.",
                statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
            );
        }

    }

    /// <summary>
    /// Get audit log statistics for dashboard/summary
    /// </summary>
    [HttpGet("stats")]
    public ActionResult<AuditStatsDTO> GetStats([FromQuery] int hours = 24)
    {
        if (!HasPermission("audit"))
            return Forbid();

        try
        {
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
        catch (Exception ex)
        {
            // Log the exception server-side (keep existing logger)
            AppDebug.Log("Error retrieving audit stats", AppDebug.LogLevel.Error, ex);

            // Add a trace id to the response so clients can report it to support
            var traceId = HttpContext.TraceIdentifier;
            Response.Headers["X-Trace-Id"] = traceId;

            // Return a ProblemDetails (RFC 7807) with 500 status without leaking exception details
            return Problem(
                detail: "An unexpected error occurred while retrieving audit stats. Provide the X-Trace-Id header to support.",
                statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
            );
        }

    }

    /// <summary>
    /// Check if current user has specific permission
    /// </summary>
    private bool HasPermission(string permission)
    {
        var username = User.FindFirst("username")?.Value ?? "Unknown";
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        bool hasIt = permissions.Contains(permission);
        if (!hasIt)
        {
            AppDebug.Log($"User {username} does not have '{permissions}' permission - Forbidden", AppDebug.LogLevel.Warning);
        }
        return hasIt;
    }
}

