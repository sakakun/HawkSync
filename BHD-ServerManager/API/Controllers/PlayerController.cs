using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.DTOs;
using System.Net;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlayerController : ControllerBase
{
    [HttpPost("kick")]
    public ActionResult<CommandResult> KickPlayer([FromBody] KickPlayerCommand command)
    {
        if (!HasPermission("players"))
            return Forbid();

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
        if (!HasPermission("players"))
            return Forbid();

        var result = command.BanIP
            ? await playerInstanceManager.BanPlayerByBothAsync(
                command.PlayerName,
                IPAddress.Parse(command.PlayerIP),
                command.PlayerSlot)
            : playerInstanceManager.BanPlayerByName(command.PlayerName, command.PlayerSlot);

        return Ok(new CommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("warn")]
    public ActionResult<CommandResult> WarnPlayer([FromBody] WarnPlayerCommand command)
    {
        if (!HasPermission("players"))
            return Forbid();

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
        if (!HasPermission("players"))
            return Forbid();

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