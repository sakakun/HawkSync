using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdAddBannedPlayer
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(string EncodedPlayerName,IPAddress ipAddress = null, int submask = 0)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdAddBannedPlayer",
                CommandData = new
                {
                    EncodedPlayerName = EncodedPlayerName,
                    IPAddress = ipAddress?.ToString() ?? string.Empty,
                    Submask = submask
                }
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdAddBannedPlayer", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {
                // ResponseData is a boolean indicating if the process is attached
                return response.Success;
            }

            return false;
        }
    }
}
