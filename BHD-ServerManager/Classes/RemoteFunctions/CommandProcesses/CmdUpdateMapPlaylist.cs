using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdUpdateMapPlaylist")]
    public static class CmdUpdateMapPlaylist
    {

        private static mapInstance instanceMaps => CommonCore.instanceMaps!;

        public static CommandResponse ProcessCommand(object data)
        {

            string decodedJson = string.Empty;
            string encodedMapList = string.Empty;
            List<mapFileInfo> newPlaylist = new();

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("newMapList", out var playList))
                    encodedMapList = playList.GetString() ?? string.Empty;
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("Msg", out var playlistObj) && playlistObj is string playList)
                    encodedMapList = playList;
            }

            decodedJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedMapList));
            newPlaylist = System.Text.Json.JsonSerializer.Deserialize<List<mapFileInfo>>(decodedJson) ?? new List<mapFileInfo>();

            instanceMaps.currentMapPlaylist = newPlaylist;

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = null
            };
        }

    }
}
