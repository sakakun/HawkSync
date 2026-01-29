using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.DTOs;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    /// <summary>
    /// Send a chat message to the game server
    /// </summary>
    /// <param name="command">Message and channel (0=Server, 1=Global, 2=Blue, 3=Red)</param>
    [HttpPost("send")]
    public ActionResult<CommandResult> SendMessage([FromBody] SendChatCommand command)
    {
        if (!HasPermission("chat"))
            return Forbid();

        // Validate channel
        if (command.Channel < 0 || command.Channel > 3)
        {
            return BadRequest(new CommandResult
            {
                Success = false,
                Message = "Invalid channel. Must be 0 (Server), 1 (Global), 2 (Blue), or 3 (Red)."
            });
        }

        // Send via manager (includes CanSendMessage validation)
        var result = chatInstanceManager.SendChatMessage(command.Message, command.Channel);

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