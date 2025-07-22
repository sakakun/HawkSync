using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
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
    public static class CmdResetAvailableMaps
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;
        public static bool ProcessCommand()
        {
            var packet = new CommandPacket
            {
                AuthToken = Program.theRemoteClient!.AuthToken,
                Command = "CmdResetAvailableMaps",
                CommandData = string.Empty
            };

            theRemoteClient.SendCommandPacket(theRemoteClient._commStream!, packet);
            var response = theRemoteClient.ReceiveCommandResponse(theRemoteClient._commStream!);

            AppDebug.Log("CmdResetAvailableMaps", JsonSerializer.Serialize(response));

            if (response != null && response.Success)
            {
                thisServer.functionEvent_PopulateMapDataGrid(thisServer.dataGridView_availableMaps, instanceMaps.availableMaps, false);
                // ResponseData is a boolean indicating if the process is attached
                return bool.TryParse(response.Success.ToString(), out bool mapsReset) && mapsReset;
            }

            return false;
        }
    }
}
