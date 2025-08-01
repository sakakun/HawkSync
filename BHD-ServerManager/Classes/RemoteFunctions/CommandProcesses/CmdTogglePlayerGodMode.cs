using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.GameManagement;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdTogglePlayerGodMode")]
    public static class CmdTogglePlayerGodMode
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            int playerSlot = -1;
            int health = -1;
            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("playerSlot", out var object1))
                    playerSlot = object1.GetInt32();
                if (jsonElement.TryGetProperty("health", out var object2))
                    health = object1.GetInt32();

            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("playerSlot", out var channelObj) && channelObj is int ch)
                    playerSlot = ch;
                if (dict.TryGetValue("health", out var channelObj1) && channelObj1 is int ch1)
                    health = ch1;

            }

            GameManager.WriteMemoryTogglePlayerGodMode(playerSlot, health);

            return new CommandResponse
            {
                Success = true,
                Message = $"Has toggled god mode for player slot, " + playerSlot.ToString(),
                ResponseData = true.ToString()
            };
        }

    }
}
