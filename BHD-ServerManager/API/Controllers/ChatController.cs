using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HawkSyncShared.SupportClasses;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    [HttpPost("send")]
    public ActionResult<CommandResult> SendMessage([FromBody] SendChatCommand command)
    {
        if (!HasPermission("chat")) return Forbid();
       
        if (command.Channel < 0 || command.Channel > 3)
            return BadRequest(new CommandResult { Success = false, Message = "Invalid channel." });

        string chatMessage = Func.FB64(command.Message);

        var result = chatInstanceManager.SendChatMessage(chatMessage, command.Channel);
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("auto/add")]
    public ActionResult<CommandResult> AddAutoMessage([FromBody] AutoMessageRequest request)
    {
        if (!HasPermission("chat")) return Forbid();
        var result = chatInstanceManager.AddAutoMessage(request.Message!, request.Interval);
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("auto/remove")]
    public ActionResult<CommandResult> RemoveAutoMessage([FromBody] RemoveMessageRequest request)
    {
        if (!HasPermission("chat")) return Forbid();
        var result = chatInstanceManager.RemoveAutoMessage(int.Parse(request.Id!));
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("slap/add")]
    public ActionResult<CommandResult> AddSlapMessage([FromBody] SlapMessageRequest request)
    {
        if (!HasPermission("chat")) return Forbid();
        var result = chatInstanceManager.AddSlapMessage(request.Message!);
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    [HttpPost("slap/remove")]
    public ActionResult<CommandResult> RemoveSlapMessage([FromBody] RemoveMessageRequest request)
    {
        if (!HasPermission("chat")) return Forbid();
        var result = chatInstanceManager.RemoveSlapMessage(int.Parse(request.Id!));
        return Ok(new CommandResult { Success = result.Success, Message = result.Message });
    }

    private bool HasPermission(string permission)
    {
        var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
        return permissions.Contains(permission);
    }
}

public class AutoMessageRequest { public string? Message { get; set; } public int Interval { get; set; } }
public class SlapMessageRequest { public string? Message { get; set; } }
public class RemoveMessageRequest { public string? Id { get; set; } }