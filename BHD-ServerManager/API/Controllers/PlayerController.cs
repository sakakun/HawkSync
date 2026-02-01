using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.DTOs;
using System.Net;
using HawkSyncShared;

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

        var result = playerInstanceManager.ArmPlayer(command.PlayerSlot, command.PlayerName);

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

        var result = playerInstanceManager.DisarmPlayer(command.PlayerSlot, command.PlayerName);

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

        var result = playerInstanceManager.ToggleGodMode(command.PlayerSlot, command.PlayerName, !IsGod);

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

        var result = playerInstanceManager.SwitchPlayerTeam(command.PlayerSlot, command.PlayerName, command.TeamNum);

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

        var result = playerInstanceManager.KickPlayer(command.PlayerSlot, command.PlayerName);

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

        string playerName = command.PlayerName;
        string playerIP = command.PlayerIP;
        int playerSlot = command.PlayerSlot;
        bool banIP = command.BanIP;

        OperationResult result;

        // Ban Name Only
        // if BanIP is false
        if (!banIP && (playerName == string.Empty || playerName == null))
        {
            result = playerInstanceManager.BanPlayerByName(playerName!, playerSlot);
            return Ok(new CommandResult
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        // Ban IP Only
        // if playerName is empty or null and BanIP is true
        if (banIP && (playerName == string.Empty || playerName == null))
        {
            if (IPAddress.TryParse(playerIP, out IPAddress? ipAddress))
            {
                result = await playerInstanceManager.BanPlayerByIPAsync(ipAddress, playerName!, playerSlot);
            }
            else
            {
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
        // Ban Both Name and IP
        // BanIP is true and playerName is not empty or null
        if(banIP && !(playerName == string.Empty || playerName == null))
        {
            if (IPAddress.TryParse(playerIP, out IPAddress? ipAddress))
            {
                result = await playerInstanceManager.BanPlayerByBothAsync(playerName, ipAddress, playerSlot);
            }
            else
            {
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

        var result = playerInstanceManager.WarnPlayer(
            command.PlayerSlot,
            command.PlayerName,
            command.Message);

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

        var result = playerInstanceManager.KillPlayer(command.PlayerSlot, command.PlayerName);

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

}