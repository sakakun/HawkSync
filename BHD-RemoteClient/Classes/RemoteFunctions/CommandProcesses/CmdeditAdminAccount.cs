using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdeditAdminAccount
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(int userid, string UserName, string UserPass, int selectedRole)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdeditAdminAccount",
                CommandData = new
                {
                    UserID = userid,
                    UserName,
                    UserPass,
                    UserRole = selectedRole
                }
            };

            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdeditAdminAccount", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}