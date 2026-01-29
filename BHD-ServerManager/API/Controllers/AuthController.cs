using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.DTOs;

namespace BHD_ServerManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string JwtKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWT!";

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var (success, user, message) = adminInstanceManager.AuthenticateUser(
            request.Username, request.Password);

        if (!success || user == null)
        {
            return Ok(new LoginResponse
            {
                Success = false,
                Message = message
            });
        }

        var token = GenerateJwtToken(user);

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

    private string GenerateJwtToken(adminInstanceManager.UserDTO user)
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
}