using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.InstanceManagers;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmddeleteAdminAccount")]
    public static class CmddeleteAdminAccount
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int UserID = -1;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("UserID", out var object1))
                    UserID = object1.GetInt32();

            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("UserID", out var channelObj) && channelObj is int ch)
                    UserID = ch;

            }

            if (adminInstanceManager.removeAdminAccount(UserID))
            {
                return new CommandResponse
                {
                    Success = true,
                    Message = $"User Deleted",
                    ResponseData = true.ToString()
                };
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Failed to delete user.",
                ResponseData = false.ToString()
            };
        }

    }
}
