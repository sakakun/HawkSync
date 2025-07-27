using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
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

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdAddSlapMessage")]
    public static class CmdAddSlapMessage
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int TimerTigger = 0;
            string ChatMessage = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("ChatMessage", out var msgProp))
                    ChatMessage = msgProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("Msg", out var msgObj) && msgObj is string msg)
                    ChatMessage = msg;
            }
            // Optionally, handle anonymous object via reflection (less common, not recommended)
            AppDebug.Log("CmdSendChatMessage", $"Received data: {ChatMessage}-{TimerTigger}");

            chatInstanceManagers.AddSlapMessage(ChatMessage.Trim());
            thisServer.functionEvent_UpdateSlapMessages();

            return new CommandResponse
            {
                Success = true,
                Message = $"The command to start the server was received and run.",
                ResponseData = true.ToString()
            };
        }

    }
}
