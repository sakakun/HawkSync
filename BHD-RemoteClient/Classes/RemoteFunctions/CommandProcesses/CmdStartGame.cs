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
                Command = "startGame",
                CommandData = string.Empty
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet, 1000);

            AppDebug.Log("startGame", JsonSerializer.Serialize(response));

            return response!.Success;
        }
    }
}