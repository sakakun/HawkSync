using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdRemoveSlapMessage
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(int mapID)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdRemoveSlapMessage",
                CommandData = mapID.ToString(),
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdRemoveSlapMessage", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {

                // ResponseData is a boolean indicating if the process is attached
                return response.Success;
            }

            return false;
        }
    }
}
