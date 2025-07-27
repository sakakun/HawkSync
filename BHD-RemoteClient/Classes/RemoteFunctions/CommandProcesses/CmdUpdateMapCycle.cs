using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public class CmdUpdateMapCycle
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand()
        {
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();

            // Serialize to JSON and encode to Base64
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(newMapPlaylist)));

            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdUpdateMapCycle",
                CommandData = base64
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            // Optionally log or handle the response
            return response != null && response.Success;
        }
    }
}