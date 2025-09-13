using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdSendConsoleCommand
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(string CommandLineText)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdSendConsoleCommand",
                CommandData = new
                {
                    Command = CommandLineText
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdSendConsoleCommand", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}