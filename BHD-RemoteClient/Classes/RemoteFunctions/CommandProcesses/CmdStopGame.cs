using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdStopGame
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand()
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "stopGame",
                CommandData = string.Empty
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet, 30000);

            AppDebug.Log("stopGame", JsonSerializer.Serialize(response));

            try
            {
                if (response != null && response.Success && response.ResponseData != null)
                {
                    // ResponseData is a boolean indicating if the process is attached
                    return bool.TryParse(response.ResponseData.ToString(), out bool stopGame) && stopGame;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("Error processing stopGame command", ex.Message);
                return false;
            }

            return false;
        }
    }
}