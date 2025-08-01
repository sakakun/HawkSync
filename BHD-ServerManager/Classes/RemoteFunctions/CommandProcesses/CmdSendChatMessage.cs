using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdSendChatMessage")]
    public static class CmdSendChatMessage
    {
        public static CommandResponse ProcessCommand(object data)
        {
            int channel = 0;
            string message = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("MsgLocation", out var channelProp))
                    channel = channelProp.GetInt32();
                if (jsonElement.TryGetProperty("Msg", out var msgProp))
                    message = msgProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("MsgLocation", out var channelObj) && channelObj is int ch)
                    channel = ch;
                if (dict.TryGetValue("Msg", out var msgObj) && msgObj is string msg)
                    message = msg;
            }
            // Optionally, handle anonymous object via reflection (less common, not recommended)
            AppDebug.Log("CmdSendChatMessage", $"Received data: {channel}-{message}");

            GameManager.WriteMemorySendChatMessage(channel, message);

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = true.ToString()
            };
        }

    }
}
