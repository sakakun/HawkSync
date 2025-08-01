using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdeditAdminAccount")]
    public static class CmdeditAdminAccount
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int UserID = -1;
            int UserRole = -1;
            string UserName = string.Empty;
            string UserPass = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("UserID", out var object0))
                    UserID = object0.GetInt32();
                if (jsonElement.TryGetProperty("UserRole", out var object1))
                    UserRole = object1.GetInt32();
                if (jsonElement.TryGetProperty("UserName", out var object2))
                    UserName = object2.GetString() ?? string.Empty;
                if (jsonElement.TryGetProperty("UserPass", out var object3))
                    UserPass = object3.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("UserID", out var channelObj0) && channelObj0 is int ch0)
                    UserID = ch0;
                if (dict.TryGetValue("UserRole", out var channelObj1) && channelObj1 is int ch1)
                    UserRole = ch1;
                if (dict.TryGetValue("UserName", out var objUserName) && objUserName is string name)
                    UserName = name;
                if (dict.TryGetValue("UserPass", out var objUserPass) && objUserPass is string pass)
                    UserPass = pass;
            }

            if (adminInstanceManager.updateAdminAccount(UserID, UserName, UserPass, (AdminRoles)UserRole))
            {
                return new CommandResponse
                {
                    Success = true,
                    Message = $"User Updated.",
                    ResponseData = true.ToString()
                };
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Failed to update user.",
                ResponseData = false.ToString()
            };
        }

    }
}
