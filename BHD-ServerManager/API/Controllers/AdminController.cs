using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    [HttpPost("create")]
    public ActionResult<AdminCommandResult> CreateUser([FromBody] CreateUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();

        var result = adminInstanceManager.CreateUser(request);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("update")]
    public ActionResult<AdminCommandResult> UpdateUser([FromBody] UpdateUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();
        var result = adminInstanceManager.UpdateUser(request);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("delete")]
    public ActionResult<AdminCommandResult> DeleteUser([FromBody] DeleteUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();
        var result = adminInstanceManager.DeleteUser(request.UserID);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
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