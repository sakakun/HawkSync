using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdTogglePlayerGodMode
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(int playerSlot, int health)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdTogglePlayerGodMode",
                CommandData = new
                {
                    playerSlot,
                    health
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdTogglePlayerGodMode", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}