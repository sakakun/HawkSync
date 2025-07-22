using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdRearmplayer
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(int playerSlot)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdRearmplayer",
                CommandData = new
                    {
                        playerSlot = playerSlot
                    }
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdRearmplayer", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {
                // ResponseData is a boolean indicating if the process is attached
                return response.Success;
            }

            return false;
        }
    }
}
