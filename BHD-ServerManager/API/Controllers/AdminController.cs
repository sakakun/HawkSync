using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    [HttpPost("create")]
    public ActionResult<AdminCommandResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = adminInstanceManager.CreateUser(request);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("update")]
    public ActionResult<AdminCommandResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var result = adminInstanceManager.UpdateUser(request);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("delete")]
    public ActionResult<AdminCommandResult> DeleteUser([FromBody] DeleteUserRequest request)
    {
        var result = adminInstanceManager.DeleteUser(request.UserID);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }
}