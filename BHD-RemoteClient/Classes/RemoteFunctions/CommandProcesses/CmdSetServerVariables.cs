using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public class CmdSetServerVariables
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(theInstance newInstance)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdSetServerVariables",
                CommandData = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(newInstance)))
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdSetServerVariables", JsonSerializer.Serialize(response));

            try
            {

                if (response != null && response.Success && response.ResponseData != null)
                {
                    // ResponseData is a boolean indicating if the process is attached
                    return bool.TryParse(response.ResponseData.ToString(), out bool hasSetVariables) && hasSetVariables;
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
