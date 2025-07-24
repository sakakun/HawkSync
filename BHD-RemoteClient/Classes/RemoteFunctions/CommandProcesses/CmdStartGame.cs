using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdStartGame
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand()
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "startGame",
                CommandData = string.Empty
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!, 30000);

            AppDebug.Log("startGame", JsonSerializer.Serialize(response));

            try
            {
                if (response != null && response.Success && response.ResponseData != null)
                {

                    // ResponseData is a boolean indicating if the process is attached
                    return bool.TryParse(response.ResponseData.ToString(), out bool startGame) && startGame;
                } 
            }
            catch (Exception ex)
            {
                AppDebug.Log("Error processing startGame command", ex.Message);

                return false;
            }

            return false;
        }
    }
}
