using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public class CmdRemoveBannedPlayerName
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(int recordID)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdRemoveBannedPlayerName",
                CommandData = recordID.ToString()
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdRemoveBannedPlayerName", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}