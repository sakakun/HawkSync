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
            string message = string.Empty;
            string AuthToken = string.Empty;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("Command", out var msgProp))
                    message = msgProp.GetString() ?? string.Empty;
                if (jsonElement.TryGetProperty("AuthToken", out var authProp))
                    AuthToken = authProp.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("Command", out var msgObj) && msgObj is string msg)
                    message = msg;
                if (dict.TryGetValue("Command", out var authObj) && authObj is string auth)
                    AuthToken = auth;
            }

            // Replace this line:
            // ConsoleCommandProcessor.ProcessCommand(AuthToken, message.Split(' '));
            
            AppDebug.Log("CmdSendConsoleCommand", $"Received data: {message} Token: '{AuthToken}'");
            
            // With the following code to create an instance of ConsoleCommandProcessor:
            var ConsoleCommandProcessor = new ConsoleCommandProcessor();
            return ConsoleCommandProcessor.ProcessCommand(AuthToken, message.Split(' '));

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = true.ToString()
            };
        }

    }
}
