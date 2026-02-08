using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabBans.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static BHD_ServerManager.API.Controllers.ProfileController;

namespace BHD_ServerManager.API.Controllers.Ban.ProxyChecking;

[ApiController]
[Route("api/ban/proxycheck")]
public class ProxyCheckController : ControllerBase
{
    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<ProxyCheckTestResult>> TestProxyCheck([FromBody] ProxyCheckTestRequest request)
    {
        if(!HasPermission("bans")) return Forbid();

        if (request == null || string.IsNullOrWhiteSpace(request.ApiKey) || string.IsNullOrWhiteSpace(request.IPAddress))
        {
            return BadRequest(new ProxyCheckTestResult { Success = false, ErrorMessage = "Invalid request." });
        }

        if (!IPAddress.TryParse(request.IPAddress, out var ip))
        {
            return BadRequest(new ProxyCheckTestResult { Success = false, ErrorMessage = "Invalid IP address." });
        }

        // Call your existing manager logic
        var (success, result, errorMessage) = await banInstanceManager.TestProxyService(
            request.ApiKey, request.ServiceProvider, ip);

        if (!success || result == null)
        {
            return Ok(new ProxyCheckTestResult
            {
                Success = false,
                ErrorMessage = errorMessage ?? "Unknown error"
            });
        }

        return Ok(new ProxyCheckTestResult
        {
            Success = true,
            IsVpn = result.IsVpn,
            IsProxy = result.IsProxy,
            IsTor = result.IsTor,
            RiskScore = result.RiskScore,
            CountryName = result.CountryName,
            CountryCode = result.CountryCode,
            Region = result.Region,
            City = result.City,
            Provider = result.Provider
        });
    }

    [HttpPost("save")]
    public ActionResult<CommandResult> SaveProxyCheckSettings([FromBody] ProxyCheck request)
    {
        if(!HasPermission("bans")) return Forbid();

        if (request == null)
        {
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });
        }

        // Map DTO to record
        var proxySettings = new ProxyCheckSettings(
            Enabled: request.ProxyCheckEnabled,
            ApiKey: request.ProxyAPIKey ?? string.Empty,
            CacheTime: request.ProxyCacheDays,
            ProxyAction: request.ProxyAction,
            VpnAction: request.VPNAction,
            TorAction: request.TORAction,
            GeoMode: request.GeoMode,
            ServiceProvider: request.ServiceProvider
        );

        // Save to disk or database
        var result = banInstanceManager.SaveProxyCheckSettings(proxySettings);

        CommonCore.instanceBans!.ForceUIUpdates = true;

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("country/add")]
    public ActionResult<CommandResult> AddBlockedCountry([FromBody] AddBlockedCountryRequest req)
    {
        if(!HasPermission("bans")) return Forbid();

        if (req == null || string.IsNullOrWhiteSpace(req.CountryCode) || string.IsNullOrWhiteSpace(req.CountryName))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = banInstanceManager.AddBlockedCountry(req.CountryCode, req.CountryName);
        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("country/remove")]
    public ActionResult<CommandResult> RemoveBlockedCountry([FromBody] RemoveBlockedCountryRequest req)
    {
        if(!HasPermission("bans")) return Forbid();

        if (req == null || req.RecordID <= 0)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = banInstanceManager.RemoveBlockedCountry(req.RecordID);
        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }
}

