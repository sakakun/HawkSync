using BHD_ServerManager.Classes.GameManagement;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdSendConsoleMessage
    {

        public static CommandResponse ProcessCommand(object data)
        {
            string message = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("Msg", out var msgProp))
                    message = msgProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("Msg", out var msgObj) && msgObj is string msg)
                    message = msg;
            }

            GameManager.WriteMemorySendConsoleCommand(message);

            return new CommandResponse
            {
                Success = true,
                Message = $"The command writen to console.",
                ResponseData = true.ToString()
            };
        }

    }
}
