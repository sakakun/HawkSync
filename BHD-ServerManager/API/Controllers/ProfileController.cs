using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static BHD_ServerManager.Classes.InstanceManagers.theInstanceManager;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    /// <summary>
    /// Get current profile settings
    /// </summary>
    [HttpGet("settings")]
    public ActionResult<ProfileSettingsResponse> GetSettings()
    {

        var result = theInstanceManager.LoadProfileSettings();
        
        if (!result.Success)
        {
            return Ok(new ProfileSettingsResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        // Map from theInstance to DTO
        var settings = MapToSettingsData();

        return Ok(new ProfileSettingsResponse
        {
            Success = true,
            Message = "Settings loaded successfully",
            Settings = settings
        });
    }

    /// <summary>
    /// Save profile settings
    /// </summary>
    [HttpPost("settings")]
    public ActionResult<CommandResult> SaveSettings([FromBody] ProfileSettingsRequest request)
    {
        

        // Convert DTO to ProfileSettings
        var settings = new ProfileSettings(
            ServerPath: request.ServerPath,
            ModFileName: request.ModFileName,
            HostName: request.HostName,
            ServerName: request.ServerName,
            MOTD: request.MOTD,
            BindIP: request.BindIP,
            BindPort: request.BindPort,
            LobbyPassword: request.LobbyPassword,
            Dedicated: request.Dedicated,
            RequireNova: request.RequireNova,
            CountryCode: request.CountryCode,
            MinPingEnabled: request.MinPingEnabled,
            MaxPingEnabled: request.MaxPingEnabled,
            MinPingValue: request.MinPingValue,
            MaxPingValue: request.MaxPingValue,
            EnableRemote: request.EnableRemote,
            RemotePort: request.RemotePort,
            Attributes: new CommandLineFlags(
                request.Attributes.Flag01, request.Attributes.Flag02,
                request.Attributes.Flag03, request.Attributes.Flag04,
                request.Attributes.Flag05, request.Attributes.Flag06,
                request.Attributes.Flag07, request.Attributes.Flag08,
                request.Attributes.Flag09, request.Attributes.Flag10,
                request.Attributes.Flag11, request.Attributes.Flag12,
                request.Attributes.Flag13, request.Attributes.Flag14,
                request.Attributes.Flag15, request.Attributes.Flag16,
                request.Attributes.Flag17, request.Attributes.Flag18,
                request.Attributes.Flag19, request.Attributes.Flag20,
                request.Attributes.Flag21
            )
        );

        // Save via manager
        var result = theInstanceManager.SaveProfileSettings(settings);

        if (result.Success)
        {
            // Trigger UI reload on server manager
            TriggerServerUIReload();
        }

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    /// <summary>
    /// Validate profile settings without saving
    /// </summary>
    [HttpPost("validate")]
    public ActionResult<ValidationResult> ValidateSettings([FromBody] ProfileSettingsRequest request)
    {

        // Convert to ProfileSettings for validation
        var settings = new ProfileSettings(
            request.ServerPath, request.ModFileName, request.HostName,
            request.ServerName, request.MOTD, request.BindIP, request.BindPort,
            request.LobbyPassword, request.Dedicated, request.RequireNova,
            request.CountryCode, request.MinPingEnabled, request.MaxPingEnabled,
            request.MinPingValue, request.MaxPingValue, request.EnableRemote,
            request.RemotePort,
            new CommandLineFlags(
                request.Attributes.Flag01, request.Attributes.Flag02,
                request.Attributes.Flag03, request.Attributes.Flag04,
                request.Attributes.Flag05, request.Attributes.Flag06,
                request.Attributes.Flag07, request.Attributes.Flag08,
                request.Attributes.Flag09, request.Attributes.Flag10,
                request.Attributes.Flag11, request.Attributes.Flag12,
                request.Attributes.Flag13, request.Attributes.Flag14,
                request.Attributes.Flag15, request.Attributes.Flag16,
                request.Attributes.Flag17, request.Attributes.Flag18,
                request.Attributes.Flag19, request.Attributes.Flag20,
                request.Attributes.Flag21
            )
        );

        var (isValid, errors) = theInstanceManager.ValidateProfileSettings(settings);

        return Ok(new ValidationResult
        {
            IsValid = isValid,
            Errors = errors
        });
    }

   [HttpPost("proxycheck")]
    public ActionResult<CommandResult> SaveProxyCheckSettings([FromBody] ProxyCheckSettingsRequest request)
    {
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

    // ================================================================================
    // HELPER METHODS
    // ================================================================================

    private ProfileSettingsData MapToSettingsData()
    {
        var instance = CommonCore.theInstance!;
        
        return new ProfileSettingsData
        {
            ServerPath = instance.profileServerPath,
            ModFileName = instance.profileModFileName,
            HostName = instance.gameHostName,
            ServerName = instance.gameServerName,
            MOTD = instance.gameMOTD,
            BindIP = instance.profileBindIP,
            BindPort = instance.profileBindPort,
            LobbyPassword = instance.gamePasswordLobby,
            Dedicated = instance.gameDedicated,
            RequireNova = instance.gameRequireNova,
            CountryCode = instance.gameCountryCode,
            MinPingEnabled = instance.gameMinPing,
            MaxPingEnabled = instance.gameMaxPing,
            MinPingValue = instance.gameMinPingValue,
            MaxPingValue = instance.gameMaxPingValue,
            EnableRemote = instance.profileEnableRemote,
            RemotePort = instance.profileRemotePort,
            Attributes = new CommandLineFlagsDTO
            {
                Flag01 = instance.profileServerAttribute01,
                Flag02 = instance.profileServerAttribute02,
                Flag03 = instance.profileServerAttribute03,
                Flag04 = instance.profileServerAttribute04,
                Flag05 = instance.profileServerAttribute05,
                Flag06 = instance.profileServerAttribute06,
                Flag07 = instance.profileServerAttribute07,
                Flag08 = instance.profileServerAttribute08,
                Flag09 = instance.profileServerAttribute09,
                Flag10 = instance.profileServerAttribute10,
                Flag11 = instance.profileServerAttribute11,
                Flag12 = instance.profileServerAttribute12,
                Flag13 = instance.profileServerAttribute13,
                Flag14 = instance.profileServerAttribute14,
                Flag15 = instance.profileServerAttribute15,
                Flag16 = instance.profileServerAttribute16,
                Flag17 = instance.profileServerAttribute17,
                Flag18 = instance.profileServerAttribute18,
                Flag19 = instance.profileServerAttribute19,
                Flag20 = instance.profileServerAttribute20,
                Flag21 = instance.profileServerAttribute21
            }
        };
    }

    private void TriggerServerUIReload()
    {
        // Trigger UI update on server manager via invoke
        var serverUI = Program.ServerManagerUI;
        if (serverUI != null)
        {
            serverUI.Invoke(() =>
            {
                serverUI.ProfileTab?.methodFunction_loadSettings();
            });
        }
    }

    [HttpPost("proxycheck/test")]
    public async Task<ActionResult<ProxyCheckTestResult>> TestProxyCheck([FromBody] ProxyCheckTestRequest request)
    {
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

    [HttpPost("netlimiter")]
    public ActionResult<CommandResult> SaveNetLimiterSettings([FromBody] NetLimiterSettingsRequest request)
    {
        if (request == null)
        {
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });
        }

        // Map DTO to internal model
        var netLimiterSettings = new NetLimiterSettings(
            Enabled: request.NetLimiterEnabled,
            Host: request.NetLimiterHost ?? string.Empty,
            Port: request.NetLimiterPort,
            Username: request.NetLimiterUsername ?? string.Empty,
            Password: request.NetLimiterPassword ?? string.Empty,
            FilterName: request.NetLimiterFilterName ?? string.Empty,
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

    [HttpGet("netlimiter/filters")]
    public ActionResult<NetLimiterFiltersResponse> GetNetLimiterFilters()
    {
        // You may need to adjust this to your actual manager/service logic
        bool success = true;
        string? errorMessage = string.Empty;
        List<string>? filters = Program.ServerManagerUI!.BanTab.comboBox_NetLimiterFilterName.Items.Cast<string>()
            .ToList() ?? new List<string>();

        return Ok(new NetLimiterFiltersResponse
        {
            Success = success,
            Message = errorMessage,
            Filters = filters ?? new List<string>()
        });
    }

    /// <summary>
    /// DTO for NetLimiter filters response.
    /// </summary>
    public class NetLimiterFiltersResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Filters { get; set; }
    }
    [HttpPost("proxycheck/add-country")]
    public ActionResult<CommandResult> AddBlockedCountry([FromBody] AddBlockedCountryRequest req)
    {
        if (req == null || string.IsNullOrWhiteSpace(req.CountryCode) || string.IsNullOrWhiteSpace(req.CountryName))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = banInstanceManager.AddBlockedCountry(req.CountryCode, req.CountryName);
        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("proxycheck/remove-country")]
    public ActionResult<CommandResult> RemoveBlockedCountry([FromBody] RemoveBlockedCountryRequest req)
    {
        if (req == null || req.RecordID <= 0)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = banInstanceManager.RemoveBlockedCountry(req.RecordID);
        CommonCore.instanceBans!.ForceUIUpdates = true;
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    /// <summary>
    /// DTOs for add/remove country requests.
    /// </summary>
    public class AddBlockedCountryRequest
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
    }
    public class RemoveBlockedCountryRequest
    {
        public int RecordID { get; set; }
    }

    [HttpPost("webstats")]
    public ActionResult<CommandResult> SaveWebStatsSettings([FromBody] WebStatsSettings settings)
    {
        if (settings == null)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = theInstanceManager.SaveWebStatsSettings(settings);
        CommonCore.instanceStats!.ForceUIUpdate = true;
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
    [HttpPost("webstats/validate")]
    public async Task<ActionResult<CommandResult>> ValidateWebStatsConnection([FromBody] WebStatsValidateRequest req)
    {
        if (req == null || string.IsNullOrWhiteSpace(req.ServerPath))
            return BadRequest(new CommandResult { Success = false, Message = "Invalid request." });

        var result = await theInstanceManager.TestWebStatsConnectionAsync(req.ServerPath);

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    /// <summary>
    /// DTO for web stats validation request.
    /// </summary>
    public class WebStatsValidateRequest
    {
        public string ServerPath { get; set; } = string.Empty;
    }
}