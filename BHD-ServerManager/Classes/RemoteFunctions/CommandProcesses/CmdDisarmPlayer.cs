using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.GameManagement;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdDisarmplayer")]
    public static class CmdDisarmplayer
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
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

            GameManager.WriteMemoryDisarmPlayer(playerSlot);

            return new CommandResponse
            {
                Success = true,
                Message = $"Disarm player slot, {playerSlot.ToString()} sent.",
                ResponseData = true.ToString()
            };

        }

    }
}
