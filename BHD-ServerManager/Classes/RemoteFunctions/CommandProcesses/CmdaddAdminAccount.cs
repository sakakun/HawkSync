using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdaddAdminAccount")]
    public static class CmdaddAdminAccount
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int UserRole = -1;
            string UserName = string.Empty;
            string UserPass = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
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
                if (dict.TryGetValue("UserRole", out var channelObj) && channelObj is int ch)
                    UserRole = ch;
                if (dict.TryGetValue("UserName", out var objUserName) && objUserName is string name)
                    UserName = name;
                if (dict.TryGetValue("UserPass", out var objUserPass) && objUserPass is string pass)
                    UserPass = pass;
            }

            if (adminInstanceManager.addAdminAccount(UserName, UserPass, (AdminRoles)UserRole))
            {
                return new CommandResponse
                {
                    Success = true,
                    Message = $"User Added, {UserName}.",
                    ResponseData = true.ToString()
                };
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Failed to add user.",
                ResponseData = false.ToString()
            };
        }

    }
}
