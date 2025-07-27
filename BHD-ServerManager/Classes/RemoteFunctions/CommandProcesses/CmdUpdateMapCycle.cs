using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdUpdateMapCycle")]
    public static class CmdUpdateMapCycle
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;

        // Processes the incoming update map cycle command
        public static CommandResponse ProcessCommand(object data)
        {
            string base64;
            if (data is string s)
            {
                base64 = s;
            }
            else if (data is JsonElement elem && elem.ValueKind == JsonValueKind.String)
            {
                base64 = elem.GetString()!;
            }
            else
            {
                // Handle error: unexpected type
                throw new InvalidOperationException("Expected a string or JsonElement containing a string.");
            }

            byte[] json = Convert.FromBase64String(base64);

            AppDebug.Log("CmdUpdateMapCycle", $"Received map playlist JSON: {json}");

            List<mapFileInfo>? newMapPlaylist = JsonSerializer.Deserialize<List<mapFileInfo>>(json);

            // Save and update map cycle
            instanceMaps.currentMapPlaylist = newMapPlaylist;
            mapInstanceManager.SaveCurrentMapPlaylist(newMapPlaylist, false);

            if (theInstance.instanceStatus == InstanceStatus.STARTDELAY || theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                ServerMemory.UpdateMapCycle1();
                ServerMemory.UpdateMapCycle2();
            }

            return new CommandResponse
            {
                Success = true,
                Message = "Map list updated.",
                ResponseData = true.ToString()
            };
        }

    }
}
