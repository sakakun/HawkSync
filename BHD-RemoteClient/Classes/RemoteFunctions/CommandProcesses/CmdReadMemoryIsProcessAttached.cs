using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdReadMemoryIsProcessAttached
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand()
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdReadMemoryIsProcessAttached",
                CommandData = string.Empty
            };

            try
            {
                var response = theRemoteClient.SendCommandAndGetResponse(packet);
                AppDebug.Log("CmdReadMemoryIsProcessAttached", JsonSerializer.Serialize(response));
                return response?.Success == true ? true : false;
            }
            catch (Exception ex)
            {
                AppDebug.Log("CmdReadMemoryIsProcessAttached", ex.Message);
                return false;
            }
        }
    }
}