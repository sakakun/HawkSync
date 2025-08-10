using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdImportBanSettings")]
    public static class CmdImportBanSettings
    {
        private static banInstance banInstance => CommonCore.instanceBans!;

        public static CommandResponse ProcessCommand(object data)
        {
            bool jsonFile = false;
            string encodedData = string.Empty;
            bool clearBans = false;

            // Handle if data is a JsonElement (common with System.Text.Json)
            if (data is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("jsonFile", out var jsonFileProp))
                    jsonFile = jsonFileProp.GetBoolean();

                if (jsonElement.TryGetProperty("encodedData", out var encodedDataProp))
                    encodedData = encodedDataProp.GetString() ?? string.Empty;

                if (jsonElement.TryGetProperty("clearBans", out var clearBansProp))
                    clearBans = clearBansProp.GetBoolean();
            }
            // Handle if data is a Dictionary<string, object>
            else if (data is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue("jsonFile", out var jsonFileObj) && jsonFileObj is bool jf)
                    jsonFile = jf;

                if (dict.TryGetValue("encodedData", out var encodedDataObj) && encodedDataObj is string ed)
                    encodedData = ed;

                if (dict.TryGetValue("clearBans", out var clearBansObj) && clearBansObj is bool cb)
                    clearBans = cb;
            }

            string decodedData = string.Empty;
            bool success = false;

            // Decode the base64 encoded data
            byte[] dataBytes = Convert.FromBase64String(encodedData);
            decodedData = Encoding.GetEncoding("Windows-1252").GetString(dataBytes);

            List<BannedPlayerAddress> BackupBannedPlayerAddresses = new List<BannedPlayerAddress>();
            List<BannedPlayerNames> BackupBannedPlayerNames = new List<BannedPlayerNames>();

            BackupBannedPlayerAddresses = banInstance.BannedPlayerAddresses.ToList();
            BackupBannedPlayerNames = banInstance.BannedPlayerNames.ToList();
            
            try
            {
                // Clear Existing Bans
                if (clearBans)
                {
                    banInstance.BannedPlayerAddresses.Clear();
                    banInstance.BannedPlayerNames.Clear();
                }
                
                // Run the Import
                banInstanceManager.ImportSettingsFromBase64(encodedData, jsonFile ? ".json" : ".ini");

                // If we made it this far, the import was successful
                success = true;

            }
            catch (Exception ex)
            {
                AppDebug.Log("Error clearing bans", ex.Message);
                return new CommandResponse
                {
                    Success = success,
                    Message = "Import of Ban Settings failed. " + ex.Message,
                    ResponseData = success.ToString()
                };
            }

            return new CommandResponse
            {
                Success = success,
                Message = "Import of Ban Settings was successful.",
                ResponseData = success.ToString()
            };
        }

    }
}
