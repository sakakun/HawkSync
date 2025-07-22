using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdRemoveBannedPlayerName
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

            banInstanceManager.RemoveBannedPlayerName(recordID);

            return new CommandResponse
            {
                Success = true,
                Message = $"The command to start the server was received and run.",
                ResponseData = true.ToString()
            };
        }

    }
}
