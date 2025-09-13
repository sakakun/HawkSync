using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdSendConsoleCommand")]
    public static class CmdSendConsoleCommand
    {
        public static CommandResponse ProcessCommand(object data)
        {
            int channel = 0;
            string message = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("Command", out var msgProp))
                    message = msgProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("Command", out var msgObj) && msgObj is string msg)
                    message = msg;
            }
            // Optionally, handle anonymous object via reflection (less common, not recommended)
            AppDebug.Log("CmdSendConsoleCommand", $"Received data: {message}");

            //chatInstanceManagers.SendMessageToQueue(false, channel, message);
            // GameManager.WriteMemorySendChatMessage(channel, message);

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = true.ToString()
            };
        }

    }
}
