using ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabBans.Service;
using Microsoft.AspNetCore.Mvc;

namespace ServerManager.API.Controllers.Ban.ProxyChecking;

[ApiController]
[Route("api/ban/netlimiter")]
public class NetLimiterController : ControllerBase    
{
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    [HttpPost("save")]
    public ActionResult<CommandResult> SaveNetLimiterSettings([FromBody] NetLimiterSettingsRequest request)
    {
        if(!HasPermission("bans")) return Forbid();
        
        // Map DTO to internal model
        var netLimiterSettings = new NetLimiterSettings(
            Enabled: request.NetLimiterEnabled,
            Host: request.NetLimiterHost,
            Port: request.NetLimiterPort,
            Username: request.NetLimiterUsername,
            Password: request.NetLimiterPassword,
            FilterName: request.NetLimiterFilterName,
            EnableConLimit: request.NetLimiterEnableConLimit,
            ConThreshold: request.NetLimiterConThreshold
        );

        // Save to disk or database as needed
        var result = banInstanceManager.SaveNetLimiterSettings(netLimiterSettings);

        CommonCore.instanceBans!.ForceUIUpdates = true;

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpGet("filters")]
    public ActionResult<NetLimiterFiltersResponse> GetNetLimiterFilters()
    {
        if(!HasPermission("bans")) return Forbid();

        // You may need to adjust this to your actual manager/service logic
        bool success = true;
        string errorMessage = string.Empty;
        List<string> filters = Program.ServerManagerUI!.BanTab.comboBox_NetLimiterFilterName.Items.Cast<string>()
            .ToList();

        return Ok(new NetLimiterFiltersResponse
        {
            Success = success,
            Message = errorMessage,
            Filters = filters
        });
    }

}
