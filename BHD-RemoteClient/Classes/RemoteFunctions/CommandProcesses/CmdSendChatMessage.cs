using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdSendChatMessage
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(int ChatMessageLocation, string ChatMessage)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdSendChatMessage",
                CommandData = new
                {
                    MsgLocation = ChatMessageLocation,
                    Msg = ChatMessage
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdSendChatMessage", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}