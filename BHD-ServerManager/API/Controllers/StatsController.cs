using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("servers/save")]
    public async Task<ActionResult<CommandResult>> SaveBabstatsServer([FromBody] SaveBabstatsServerRequest req) {

        if (!HasPermission("stats")) return Forbid();

        if (req == null || string.IsNullOrWhiteSpace(req.Server.ServerPath))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid path." });
    
        bool success = DatabaseManager.UpdateBabstatsServer(req.Server);

        CommonCore.instanceStats!.ForceUIUpdate = true;

		return Ok(new CommandResult
        {
            Success = success,
            Message = "Babstats server settings saved successfully."
		});

    }

    [HttpPost("servers/add")]
    public async Task<ActionResult<CommandResult>> AddBabstatsServer([FromBody] SaveBabstatsServerRequest req) {

        if (!HasPermission("stats")) return Forbid();

        if (req == null || string.IsNullOrWhiteSpace(req.Server.ServerPath))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid path." });
    
        string message = "Server Added Successfully.";
        bool success = true;

		try {
            DatabaseManager.AddBabstatsServer(req.Server);
        } catch (Exception ex) {
            success = false;
            message = ex.Message;
        }

		CommonCore.instanceStats!.ForceUIUpdate = true;

		return Ok(new CommandResult
        {
            Success = success,
            Message = message
		});

    }

    [HttpPost("servers/remove")]
    public async Task<ActionResult<CommandResult>> RemoveBabstatsServer([FromBody] int serverID) {

        if (!HasPermission("stats")) return Forbid();

        string message = "Server Removed Successfully.";
        bool success = true;

		try {
            success = DatabaseManager.RemoveBabstatsServer(serverID);
		} catch (Exception ex) {
            success = false;
            message = ex.Message;
        }

		CommonCore.instanceStats!.ForceUIUpdate = true;

		return Ok(new CommandResult
        {
            Success = success,
            Message = message
		});

    }

    [HttpPost("servers/clearAnnoucements")]
    public async Task<ActionResult<CommandResult>> ClearBabstatsAnnoucements([FromBody] bool req) {

        if (!HasPermission("stats")) return Forbid();

        DatabaseManager.DisableAllBabstatsAnnouncements();

		return Ok(new CommandResult
        {
            Success = true,
            Message = "Babstats server settings saved successfully."
		});

    }

	[HttpPost("validate")]
    public async Task<ActionResult<CommandResult>> ValidateWebStatsConnection([FromBody] WebStatsValidateRequest req)
    {
        if (!HasPermission("stats")) return Forbid();

        if (req == null || string.IsNullOrWhiteSpace(req.ServerPath))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = await theInstanceManager.TestWebStatsConnectionAsync(req.ServerPath);

        LogStatsAction(
            "ValidateWebStatsConnection",
            "Validated Web Stats Connection",
            result.Success,
            result.Message
        );

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
