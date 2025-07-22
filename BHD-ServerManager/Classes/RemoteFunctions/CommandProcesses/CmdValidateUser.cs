using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public class CmdValidateUser
    {
        private static readonly adminInstance adminInstance = CommonCore.instanceAdmin!;

        // Now receives a strongly-typed AuthenticationPacket
        public static CommandResponse ProcessCommand(AuthenticationPacket authData)
        {
            string username = authData.Username ?? string.Empty;
            string password = authData.Password ?? string.Empty;

            var adminUser = adminInstance.Admins
                .FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            AppDebug.Log("ValidateUser", $"ValidateUser: Username={username}, Password={password}");
            AppDebug.Log("ValidateUser", $"ValidateUser: Found AdminUser={adminUser?.Username}");

            if (adminUser == null || !adminInstanceManager.ValidatePassword(password, adminUser!.Password))
            {
                AppDebug.Log("ValidateUser", "ValidateUser: Invalid username or password.");
                return new CommandResponse
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            string token = GenerateToken(username);

            var client = new AuthorizedClient
            {
                ClientId = adminUser.UserId,
                User = adminUser,
                AuthorizationToken = token,
                AuthorizationTime = DateTime.Now
            };

            RemoteServer.AddOrUpdateAuthorizedClient(client);

            return new CommandResponse
            {
                Success = true,
                Message = "User validated.",
                ResponseData = new { Token = token }
            };
        }

        private static string GenerateToken(string username)
        {
            using var sha256 = SHA256.Create();
            var raw = $"{username}:{Guid.NewGuid()}:{DateTime.UtcNow.Ticks}";
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(bytes);
        }
    }
}