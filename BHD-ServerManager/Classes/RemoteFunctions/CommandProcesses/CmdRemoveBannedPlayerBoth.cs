using BHD_SharedResources.Classes.InstanceManagers;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdRemoveBannedPlayerBoth")]
    public static class CmdRemoveBannedPlayerBoth
    {
        public static CommandResponse ProcessCommand(object data)
        {
            int recordID;

            if (data is int i)
            {
                recordID = i;
            }
            else if (data is string s && int.TryParse(s, out int parsed))
            {
                recordID = parsed;
            }
            else if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    recordID = jsonElement.GetInt32();
                }
                else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(jsonElement.GetString(), out int parsedJson))
                {
                    recordID = parsedJson;
                }
                else
                {
                    return new CommandResponse
                    {
                        Success = false,
                        Message = "Invalid data type for map index.",
                        ResponseData = null
                    };
                }
            }
            else
            {
                return new CommandResponse
                {
                    Success = false,
                    Message = "Invalid data type for map index.",
                    ResponseData = null
                };
            }

            banInstanceManager.RemoveBannedPlayerBoth(recordID);

            return new CommandResponse
            {
                Success = true,
                Message = $"Removed a banned player, (both ip/name).",
                ResponseData = true.ToString()
            };
        }

    }
}
