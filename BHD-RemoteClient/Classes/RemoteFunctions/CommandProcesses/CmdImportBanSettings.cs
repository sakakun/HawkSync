using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdImportBanSettings
    {
        private static RemoteClient theRemoteClient => Program.theRemoteClient!;

        /// <summary>
        /// Prompts for a ban file, encodes and sends it to the server for import.
        /// </summary>
        /// <param name="clearBans">If true, clears existing bans before import.</param>
        /// <returns>True if import was successful, false otherwise.</returns>
        public static bool ImportSettings(bool clearBans = false)
        {

            // Prompt user for file
            string importPath = Functions.ShowFileDialog(
                false,
                "Ban Files (*.json;*.ini)|*.json;*.ini|JSON Files (*.json)|*.json|INI Files (*.ini)|*.ini",
                "Import Ban Settings",
                CommonCore.AppDataPath,
                "BanSettings.json"
            );
            if (string.IsNullOrEmpty(importPath))
            {
                AppDebug.Log("CmdImportBanSettings", "Import cancelled by user.");
                return false;
            }

            try
            {
                // Read and encode file
                string fileContent = File.ReadAllText(importPath, Encoding.GetEncoding("Windows-1252"));
                string encodedData = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(fileContent));
                bool jsonFile = importPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase);

                // Build and send command packet
                var packet = new CommandPacket
                {
                    AuthToken = theRemoteClient.AuthToken,
                    Command = "CmdImportBanSettings",
                    CommandData = new
                    {
                        jsonFile,
                        encodedData,
                        clearBans
                    }
                };

                var response = theRemoteClient.SendCommandAndGetResponse(packet);

                AppDebug.Log("CmdImportBanSettings", JsonSerializer.Serialize(response));

                return response?.Success == true;
            }
            catch (Exception ex)
            {
                AppDebug.Log("CmdImportBanSettings", $"Error importing ban settings: {ex.Message}");
                return false;
            }
        }
    }
}