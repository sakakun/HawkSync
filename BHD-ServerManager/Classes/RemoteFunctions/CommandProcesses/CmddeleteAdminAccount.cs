using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Chat;
using Windows.Networking.Proximity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
