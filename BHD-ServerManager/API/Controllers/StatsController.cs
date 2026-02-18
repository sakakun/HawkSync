using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabBans;
using HawkSyncShared.DTOs.tabBans.Service;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.DTOs.tabStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BHD_ServerManager.API.Controllers.ProfileController;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    [HttpPost("save")]
    public ActionResult<CommandResult> SaveWebStatsSettings([FromBody] WebStatsSettings settings)
    {
        if(!HasPermission("stats")) return Forbid();

        if (settings == null)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = theInstanceManager.SaveWebStatsSettings(settings);

        LogStatsAction(
            "SaveWebStatsSettings",
            "Web stats settings updated",
            result.Success,
            result.Message
        );

        CommonCore.instanceStats!.ForceUIUpdate = true;
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
    [HttpPost("validate")]
    public async Task<ActionResult<CommandResult>> ValidateWebStatsConnection([FromBody] WebStatsValidateRequest req)
    {
        if(!HasPermission("stats")) return Forbid();

        if (req == null || string.IsNullOrWhiteSpace(req.ServerPath))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = await theInstanceManager.TestWebStatsConnectionAsync(req.ServerPath);

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    private void LogStatsAction(string actionType, string description, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.Stats,
            actionType: actionType,
            description: description,
            targetType: "Stats",
            targetId: null,
            targetName: null,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}
