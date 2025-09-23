using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdUpdateMapPlaylist
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(List<mapFileInfo> newMapList)
        {

            string jsonString = JsonSerializer.Serialize(newMapList);
            string base64String = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonString));

            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdUpdateMapPlaylist",
                CommandData = new
                {
                    newMapList = base64String
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdUpdateMapPlaylist", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}