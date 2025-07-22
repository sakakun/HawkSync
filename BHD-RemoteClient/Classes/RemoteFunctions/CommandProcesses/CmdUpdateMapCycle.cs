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
    public class CmdUpdateMapCycle
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand()
        {
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();

            // Serialize to JSON
            string json = JsonSerializer.Serialize(newMapPlaylist);

            // Encode JSON to Base64
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdUpdateMapCycle",
                CommandData = base64
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            // Optionally log or handle the response
            return response != null && response.Success;
        }
    }
}
