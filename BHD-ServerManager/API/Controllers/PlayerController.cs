using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlayerController : ControllerBase
{

    [HttpPost("arm")]
    public ActionResult<CommandResult> ArmPlayer([FromBody] ArmPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.ArmPlayer(command.PlayerSlot, playerName);
        LogPlayerAction("ArmPlayer", playerName ?? "", result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
    [HttpPost("disarm")]
    public ActionResult<CommandResult> DisarmPlayer([FromBody] DisarmPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.DisarmPlayer(command.PlayerSlot, playerName);
        LogPlayerAction("DisarmPlayer", playerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
    
    [HttpPost("togglegodmode")]
    public ActionResult<CommandResult> ToggleGodMode([FromBody] GodModePlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        bool IsGod = CommonCore.instancePlayers!.PlayerList[command.PlayerSlot].IsGod;
        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.ToggleGodMode(command.PlayerSlot, playerName, !IsGod);
        LogPlayerAction("ToggleGodMode", playerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
    [HttpPost("switchteam")]
    public ActionResult<CommandResult> SwitchTeamPlayer([FromBody] SwitchTeamPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.SwitchPlayerTeam(command.PlayerSlot, playerName, command.TeamNum);
        LogPlayerAction("SwitchTeamPlayer", playerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("kick")]
    public ActionResult<CommandResult> KickPlayer([FromBody] KickPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();
        var PlayerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.KickPlayer(command.PlayerSlot, PlayerName);
        LogPlayerAction("KickPlayer", PlayerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("ban")]
    public async Task<ActionResult<CommandResult>> BanPlayer([FromBody] BanPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        string playerIP = command.PlayerIP;
        int playerSlot = command.PlayerSlot;
        bool banIP = command.BanIP;

        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            username = "Remote Admin";
        }

        OperationResult result;

        if (!banIP && (playerName == string.Empty || playerName == null))
        {
            result = playerInstanceManager.BanPlayerByName(playerName!, playerSlot, username!);
            LogPlayerAction("BanPlayerByName", playerName!, result.Success, result.Message);
            return Ok(new CommandResult
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        if (banIP && (playerName == string.Empty || playerName == null))
        {
            if (IPAddress.TryParse(playerIP, out IPAddress? ipAddress))
            {
                result = await playerInstanceManager.BanPlayerByIPAsync(ipAddress, playerName!, playerSlot, username!);
                LogPlayerAction("BanPlayerByIP", playerIP ?? "", result.Success, result.Message);
            }
            else
            {
                LogPlayerAction("BanPlayerByIP", playerIP, false, "Invalid IP address format.");
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Invalid IP address format."
                });
            }
            return Ok(new CommandResult
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        if(banIP && !(playerName == string.Empty || playerName == null))
        {
            if (IPAddress.TryParse(playerIP, out IPAddress? ipAddress))
            {
                result = await playerInstanceManager.BanPlayerByBothAsync(playerName, ipAddress, playerSlot, username!);
                LogPlayerAction("BanPlayerByBoth", $"{playerName} ({playerIP})", result.Success, result.Message);
            }
            else
            {
                LogPlayerAction("BanPlayerByBoth", $"{playerName} ({playerIP})", false, "Invalid IP address format.");
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Invalid IP address format."
                });
            }
            return Ok(new CommandResult
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        LogPlayerAction("BanPlayer", playerName!, false, "Invalid ban parameters.");
        return Ok(new CommandResult
        {
            Success = false,
            Message = "Invalid ban parameters."
        });
    }

    [HttpPost("warn")]
    public ActionResult<CommandResult> WarnPlayer([FromBody] WarnPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.WarnPlayer(
            command.PlayerSlot,
            playerName,
            command.Message);
        LogPlayerAction("WarnPlayer", playerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("kill")]
    public ActionResult<CommandResult> KillPlayer([FromBody] KillPlayerCommand command)
    {
        if(!HasPermission("players")) return Forbid();

        string playerName = AppFunc.FB64(command.PlayerName);
        var result = playerInstanceManager.KillPlayer(command.PlayerSlot, playerName);
        LogPlayerAction("KillPlayer", playerName, result.Success, result.Message);
        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }

    private void LogPlayerAction(string actionType, string target, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null,
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.Player,
            actionType: actionType,
            description: $"{actionType} on {target}",
            targetType: "Player",
            targetId: null,
            targetName: target,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}