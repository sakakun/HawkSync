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
    public static class CmdSwitchPlayerTeam
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static CommandResponse ProcessCommand(int playerSlot)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdSwitchPlayerTeam",
                CommandData = new
                    {
                        playerSlot = playerSlot
                    }
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdSwitchPlayerTeam", JsonSerializer.Serialize(response));

            return response!;

        }
    }
}
