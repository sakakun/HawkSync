using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdSendConsoleMessage
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(string ChatMessage)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdSendConsoleMessage",
                CommandData = new
                {
                    Msg = ChatMessage
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdSendConsoleMessage", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}