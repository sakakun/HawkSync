using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdUpdateNextMap")]
    public static class CmdUpdateNextMap
    {
        public static CommandResponse ProcessCommand(object data)
        {
            int mapIndex;

            if (data is int i)
            {
                mapIndex = i;
            }
            else if (data is string s && int.TryParse(s, out int parsed))
            {
                mapIndex = parsed;
            }
            else if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    mapIndex = jsonElement.GetInt32();
                }
                else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(jsonElement.GetString(), out int parsedJson))
                {
                    mapIndex = parsedJson;
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

            AppDebug.Log("CmdUpdateNextMap", "Next Map ID: " + mapIndex.ToString());
            GameManager.UpdateNextMap(mapIndex);

            return new CommandResponse
            {
                Success = true,
                Message = $"Updated Next Map to Index of: " + mapIndex.ToString(),
                ResponseData = true.ToString()
            };
        }

    }
}
