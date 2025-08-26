using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdStartGame
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand()
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdStartGame",
                CommandData = string.Empty
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet, 12000); // 12 seconds

            if (response == null)
            {
                AppDebug.Log("CmdStartGame", "No response received from server. Check server logs and network.");
                return false;
            }

            AppDebug.Log("CmdStartGame", JsonSerializer.Serialize(response));
            return response.Success;
        }
    }
}