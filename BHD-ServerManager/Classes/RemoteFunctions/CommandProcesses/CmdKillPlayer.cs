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
    [CommandHandler("CmdKillPlayer")]
    public static class CmdKillPlayer
    {

        public static CommandResponse ProcessCommand(object data)
        {
            int playerSlot = -1;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("playerSlot", out var object1))
                    playerSlot = object1.GetInt32();

            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("playerSlot", out var channelObj) && channelObj is int ch)
                    playerSlot = ch;
            }

            GameManager.WriteMemoryKillPlayer(playerSlot);

            return new CommandResponse
            {
                Success = true,
                Message = $"Player with slot number ({playerSlot.ToString()}) was killed.",
                ResponseData = true.ToString()
            };
        }

    }
}
