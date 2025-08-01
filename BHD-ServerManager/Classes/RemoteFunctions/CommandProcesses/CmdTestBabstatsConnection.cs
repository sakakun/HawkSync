using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdTestBabstatsConnection")]
    public static class CmdTestBabstatsConnection
    {
        public static CommandResponse ProcessCommand(object data)
        {
            string webUrl = string.Empty;

            // Handle both string and JsonElement input
            string base64 = data switch
            {
                string s => s,
                System.Text.Json.JsonElement je when je.ValueKind == System.Text.Json.JsonValueKind.String => je.GetString()!,
                _ => throw new InvalidCastException("Input data is not a valid Base64 string.")
            };

            byte[] bytes = Convert.FromBase64String(base64);
            webUrl = Encoding.UTF8.GetString(bytes);

            AppDebug.Log("CmdTestBabstatsConnection", $"Testing connection to Babstats at {webUrl}");

            if (StatsManager.TestBabstatsConnection(webUrl))
            {
                AppDebug.Log("CmdTestBabstatsConnection", $"Testing Successful.");
                return new CommandResponse
                {
                    Success = true,
                    Message = $"Connection to Babstats at {webUrl} was successful.",
                    ResponseData = null
                };
            }
            else
            {
                AppDebug.Log("CmdTestBabstatsConnection", $"Testing Failed.");
                return new CommandResponse
                {
                    Success = false,
                    Message = string.Empty,
                    ResponseData = null
                };
            }

        }

    }
}
