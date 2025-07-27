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
                Command = "ReadMemoryIsProcessAttached",
                CommandData = string.Empty
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("ReadMemoryIsProcessAttached", JsonSerializer.Serialize(response));

            try
            {
                if (response != null && response.Success && response.ResponseData != null)
                {
                    // ResponseData is a boolean indicating if the process is attached
                    return bool.TryParse(response.ResponseData.ToString(), out bool isAttached) && isAttached;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("Error processing ReadMemoryIsProcessAttached command", ex.Message);
                return false;
            }

            return false;
        }
    }
}