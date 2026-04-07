using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ServerManager.Classes.InstanceManagers;
using ServerManager.Classes.SupportClasses;
using ServerManager.API.Services;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.RateLimiting;

// Token TTL — 8 hours is a reasonable balance between usability and exposure window.
// Reducing from the original 24h means a stolen token is usable for at most 8h.
// Explicit logout revokes the token immediately via the jti denylist.

namespace ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [EnableRateLimiting("LoginPolicy")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        var (success, user, message) = adminInstanceManager.AuthenticateUser(
            request.Username, request.Password);

        if (!success || user == null)
        {
            AppDebug.Log($"Login failed for {request.Username}: {message}", AppDebug.LogLevel.Warning);
            
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
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error writing failed login audit log", AppDebug.LogLevel.Error, ex);
            }

            return Ok(new LoginResponse
            {
                Success = false,
                Message = message
            });
        }

        var token = GenerateJwtToken(user);

        adminInstanceManager.TrackSession(user.Username);
        
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
        }
        catch (Exception ex)
        {
            AppDebug.Log($"Error writing successful login audit log", AppDebug.LogLevel.Error, ex);
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKeyProvider.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique token ID for revocation
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
            expires: DateTime.UtcNow.AddHours(8), // Reduced from 24h
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

        // Revoke the specific token so it cannot be reused before its natural expiry
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        if (!string.IsNullOrEmpty(jti) && long.TryParse(expClaim, out var expSeconds))
        {
            var utcExpiry = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            TokenDenylistService.Revoke(jti, utcExpiry);
            AppDebug.Log($"Token revoked for user '{username}' (jti: {jti})", AppDebug.LogLevel.Info);
        }

        return Ok(new { Success = true, Message = "Logged out successfully" });
    }
}