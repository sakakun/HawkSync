using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.SupportClasses;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string JwtKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWT!";

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        AppDebug.Log("AuthController", $"Login attempt from {ipAddress} for user: {request.Username}");
        
        var (success, user, message) = adminInstanceManager.AuthenticateUser(
            request.Username, request.Password);

        if (!success || user == null)
        {
            AppDebug.Log("AuthController", $"Login failed for {request.Username}: {message}");
            
            // Log failed login attempt
            try
            {
                DatabaseManager.LogAuditAction(
                    userId: null,
                    username: request.Username,
                    category: AuditCategory.System,
                    actionType: AuditAction.Login,
                    description: $"Failed login attempt: {message}",
                    targetType: "Authentication",
                    targetId: null,
                    targetName: request.Username,
                    ipAddress: ipAddress,
                    success: false,
                    errorMessage: message
                );
                AppDebug.Log("AuthController", "Failed login audit log written");
            }
            catch (Exception ex)
            {
                AppDebug.Log("AuthController", $"Error writing failed login audit log: {ex.Message}");
            }

            return Ok(new LoginResponse
            {
                Success = false,
                Message = message
            });
        }

        var token = GenerateJwtToken(user);

        adminInstanceManager.TrackSession(user.Username);

        AppDebug.Log("AuthController", $"Login successful for {request.Username}");
        
        // Log successful login
        bool auditLogSuccess = false;
        try
        {
            DatabaseManager.LogAuditAction(
                userId: user.UserID,
                username: user.Username,
                category: AuditCategory.System,
                actionType: AuditAction.Login,
                description: "User logged in successfully",
                targetType: "User",
                targetId: user.UserID.ToString(),
                targetName: user.Username,
                ipAddress: ipAddress,
                success: true
            );
            auditLogSuccess = true;
            AppDebug.Log("AuthController", "Successful login audit log written");
        }
        catch (Exception ex)
        {
            AppDebug.Log("AuthController", $"Error writing successful login audit log: {ex.Message}");
        }

        if (!auditLogSuccess)
        {
            AppDebug.Log("AuthController", "WARNING: Login was successful but audit log failed to write");
        }

        return Ok(new LoginResponse
        {
            Success = true,
            Token = token,
            Message = "Login successful",
            User = new UserInfoDTO
            {
                UserId = user.UserID,
                Username = user.Username,
                Permissions = user.Permissions
            }
        });
    }

    private string GenerateJwtToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("userId", user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("username", user.Username)
        };

        foreach (var permission in user.Permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var token = new JwtSecurityToken(
            issuer: "BHD.ServerManager",
            audience: "BHD.RemoteClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("logout")]
    [Authorize]
    public ActionResult Logout()
    {
        var username = User.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            adminInstanceManager.RemoveSession(username);
        }
        return Ok(new { Success = true, Message = "Logged out successfully" });
    }
}