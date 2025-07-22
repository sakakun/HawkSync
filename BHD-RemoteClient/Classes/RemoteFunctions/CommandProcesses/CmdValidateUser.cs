using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            // Send the packet
            theRemoteClient.SendCommandPacket(theRemoteClient._commStream, packet);

            // Receive the response
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream);

            // If successful, extract and store the AuthToken
            if (response != null && response.Success && response.ResponseData != null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(response.ResponseData.ToString()!);
                    if (doc.RootElement.TryGetProperty("Token", out var tokenElement))
                    {
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
