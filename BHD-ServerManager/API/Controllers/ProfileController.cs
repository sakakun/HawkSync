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
using System.Net;
using static BHD_ServerManager.Classes.InstanceManagers.theInstanceManager;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    /// <summary>
    /// Get current profile settings
    /// </summary>
    [HttpGet("settings")]
    public ActionResult<ProfileSettingsResponse> GetSettings()
    {
        if(!HasPermission("profile")) return Forbid();

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
        if(!HasPermission("profile")) return Forbid();

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

        LogProfileAction(
            "SaveSettings",
            "Profile settings updated",
            result.Success,
            result.Message
        );

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
        if(!HasPermission("profile")) return Forbid();

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
                serverUI.ProfileTab?.Profile_LoadSettings();
            });
        }
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    private void LogProfileAction(string actionType, string description, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.Settings,
            actionType: actionType,
            description: description,
            targetType: "Profile",
            targetId: null,
            targetName: null,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}