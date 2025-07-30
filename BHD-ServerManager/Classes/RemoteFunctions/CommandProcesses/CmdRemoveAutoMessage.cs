using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdRemoveAutoMessage")]
    public static class CmdRemoveAutoMessage
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int messageIndex;

            if (data is int i)
            {
                messageIndex = i;
            }
            else if (data is string s && int.TryParse(s, out int parsed))
            {
                messageIndex = parsed;
            }
            else if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    messageIndex = jsonElement.GetInt32();
                }
                else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(jsonElement.GetString(), out int parsedJson))
                {
                    messageIndex = parsedJson;
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

            chatInstanceManagers.RemoveAutoMessage(messageIndex);
            thisServer.functionEvent_UpdateAutoMessages();

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = true.ToString()
            };
        }

    }
}
