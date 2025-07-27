using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Net;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdAddBannedPlayer
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(string EncodedPlayerName, IPAddress ipAddress = null!, int submask = 0)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdAddBannedPlayer",
                CommandData = new
                {
                    EncodedPlayerName,
                    IPAddress = ipAddress?.ToString() ?? string.Empty,
                    Submask = submask
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdAddBannedPlayer", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}