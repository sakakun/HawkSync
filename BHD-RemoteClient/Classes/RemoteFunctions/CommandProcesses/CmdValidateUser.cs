using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdValidateUser
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool Login(string username, string password)
        {
            if (theRemoteClient._commStream == null)
                throw new InvalidOperationException("Communication stream is not initialized.");

            // Prepare the authentication packet
            var authPacket = new
            {
                Username = username,
                Password = password
            };

            // Create the command packet
            var packet = new CommandPacket
            {
                Command = "ValidateUser",
                CommandData = authPacket
            };

            // Use the new unified method for command/response
            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            // If successful, extract and store the AuthToken
            if (response != null && response.Success && response.ResponseData != null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(response.ResponseData.ToString()!);
                    if (doc.RootElement.TryGetProperty("Token", out var tokenElement))
                    {
                        AppDebug.Log("CmdValidateUser", "Received AuthToken: " + tokenElement.GetString());
                        theRemoteClient.AuthToken = tokenElement.GetString() ?? string.Empty;
                        if (!string.IsNullOrEmpty(theRemoteClient.AuthToken))
                        {
                            return true; // Only return true if token is present and not empty
                        }
                    }
                }
                catch
                {
                    // Handle parsing errors if needed
                }
            }

            // If we reach here, login failed or token is missing/invalid
            theRemoteClient.AuthToken = string.Empty;
            return false;
        }
    }
}