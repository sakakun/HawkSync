using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdAddAutoMessage")]
    public static class CmdAddAutoMessage
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int TimerTigger = 0;
            string ChatMessage = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("TimerTigger", out var channelProp))
                    TimerTigger = channelProp.GetInt32();
                if (jsonElement.TryGetProperty("ChatMessage", out var msgProp))
                    ChatMessage = msgProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("MsgLocation", out var channelObj) && channelObj is int ch)
                    TimerTigger = ch;
                if (dict.TryGetValue("Msg", out var msgObj) && msgObj is string msg)
                    ChatMessage = msg;
            }
            // Optionally, handle anonymous object via reflection (less common, not recommended)
            AppDebug.Log("CmdSendChatMessage", $"Received data: {ChatMessage}-{TimerTigger}");

            chatInstanceManagers.AddAutoMessage(ChatMessage.Trim(), TimerTigger);

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = true.ToString()
            };
        }

    }
}
