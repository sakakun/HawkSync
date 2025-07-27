using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdSwitchPlayerTeam
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static CommandResponse? ProcessCommand(int playerSlot)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdSwitchPlayerTeam",
                CommandData = new
                {
                    playerSlot
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdSwitchPlayerTeam", JsonSerializer.Serialize(response));

            return response;
        }
    }
}