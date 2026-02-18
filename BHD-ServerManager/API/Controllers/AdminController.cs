using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.DTOs.tabAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    [HttpPost("user/create")]
    public ActionResult<AdminCommandResult> CreateUser([FromBody] CreateUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();

        var result = adminInstanceManager.CreateUser(request);
        LogUserAdminAction("Create", request.Username, null, result.Success, result.Message);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;      

        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("user/update")]
    public ActionResult<AdminCommandResult> UpdateUser([FromBody] UpdateUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();
        var result = adminInstanceManager.UpdateUser(request);
        LogUserAdminAction("Update", request.Username, request.UserID, result.Success, result.Message);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
        return Ok(new AdminCommandResult
        {
            Success = result.Success,
            Message = result.Message
        });
    }

    [HttpPost("user/delete")]
    public ActionResult<AdminCommandResult> DeleteUser([FromBody] DeleteUserRequestDTO request)
    {
        if(!HasPermission("users")) return Forbid();
        var result = adminInstanceManager.DeleteUser(request.UserID);
        LogUserAdminAction("Delete", $"UserID:{request.UserID}", request.UserID, result.Success, result.Message);

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

    private void LogUserAdminAction(string actionType, string targetUsername, int? targetUserId, bool success, string message)
    {
        DatabaseManager.LogAuditAction(
            userId: null, // or get from User claims if available
            username: User.Identity?.Name ?? "Unknown",
            category: AuditCategory.User,
            actionType: actionType,
            description: $"{actionType} user '{targetUsername}'",
            targetType: "User",
            targetId: targetUserId?.ToString(),
            targetName: targetUsername,
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString(),
            success: success,
            errorMessage: success ? null : message
        );
    }

}