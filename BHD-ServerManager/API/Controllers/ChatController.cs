using BHD_ServerManager.Classes.InstanceManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.DTOs.tabPlayers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.Instances;

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

        string chatMessage = AppFunc.FB64(command.Message);

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

    [HttpGet("history/players")]
    public ActionResult<List<string>> GetDistinctPlayerNames([FromQuery] int limit = 500)
    {
        if (!HasPermission("chat")) return Forbid();
        var players = DatabaseManager.GetDistinctPlayerNames(limit);
        return Ok(players);
    }

    [HttpPost("history/search")]
    public ActionResult<ChatHistoryResponse> GetChatHistory([FromBody] ChatHistoryRequest request)
    {
        if (!HasPermission("chat")) return Forbid();

        var result = DatabaseManager.GetChatLogsFiltered(
            request.StartDate,
            request.EndDate,
            request.PlayerFilter,
            request.TypeFilter,
            request.TeamFilter,
            request.SearchText,
            request.Page,
            request.PageSize
        );

        return Ok(new ChatHistoryResponse
        {
            Logs = result.logs,
            TotalCount = result.totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
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

public class ChatHistoryRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PlayerFilter { get; set; }
    public int? TypeFilter { get; set; }
    public int? TeamFilter { get; set; }
    public string? SearchText { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}

public class ChatHistoryResponse
{
    public List<ChatLogObject> Logs { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}