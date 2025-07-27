using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdKillPlayer
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(int playerSlot)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdKillPlayer",
                CommandData = new
                {
                    playerSlot
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdKillPlayer", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}