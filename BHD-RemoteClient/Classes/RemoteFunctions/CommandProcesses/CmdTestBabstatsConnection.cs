using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{

    public static class CmdTestBabstatsConnection
    {

        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(string WebURL)
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdTestBabstatsConnection",
                CommandData = Convert.ToBase64String(Encoding.UTF8.GetBytes(WebURL))
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdTestBabstatsConnection", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {
                AppDebug.Log("CmdTestBabstatsConnection", $"Remote Testing Successful.");
                if (response.ResponseData != null)
                {
                    // If ResponseData is expected to be a boolean, parse it
                    return bool.TryParse(response.ResponseData.ToString(), out bool isSuccess) && isSuccess;
                }
                // If ResponseData is null, but Success is true, treat as success
                return true;
            }
            else
            {
                AppDebug.Log("CmdTestBabstatsConnection", $"Remote Testing Failed.");
                return false;
            }


        }
    }
}
