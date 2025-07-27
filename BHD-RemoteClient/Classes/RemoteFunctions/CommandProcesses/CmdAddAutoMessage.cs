using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdAddAutoMessage
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(string ChatMessage, int TimerTigger)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdAddAutoMessage",
                CommandData = new
                {
                    TimerTigger,
                    ChatMessage
                }
            };

            // Use the new unified method for command/response
            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdAddAutoMessage", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}