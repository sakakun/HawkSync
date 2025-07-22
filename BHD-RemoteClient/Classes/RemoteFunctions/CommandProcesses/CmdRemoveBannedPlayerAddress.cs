using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
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
    public class CmdRemoveBannedPlayerAddress
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand(int recordID)
        {

            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdRemoveBannedPlayerAddress",
                CommandData = recordID.ToString()
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdRemoveBannedPlayerAddress", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {
                return response.Success;
            }

            return false;
        }
    }
}
