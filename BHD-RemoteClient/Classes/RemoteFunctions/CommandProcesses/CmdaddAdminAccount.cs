using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdaddAdminAccount
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        public static bool ProcessCommand(string UserName, string UserPass, int selectedRole)
        {
            var packet = new CommandPacket
            {
                AuthToken = theRemoteClient.AuthToken,
                Command = "CmdaddAdminAccount",
                CommandData = new
                {
                    UserName,
                    UserPass,
                    UserRole = selectedRole
                }
            };

            // Use the new unified method for command/response
            var response = theRemoteClient.SendCommandAndGetResponse(packet);

            AppDebug.Log("CmdaddAdminAccount", JsonSerializer.Serialize(response));

            return response?.Success == true;
        }
    }
}