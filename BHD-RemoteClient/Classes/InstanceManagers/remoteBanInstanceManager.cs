using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteBanInstanceManager : banInstanceInterface
    {
        public static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        public static banInstance instanceBans => CommonCore.instanceBans!;

        public void AddBannedPlayer(string playerName, IPAddress playerIP, int subnetMask) => CmdAddBannedPlayer.ProcessCommand(playerName, playerIP, subnetMask);

        public void LoadSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public bool RemoveBannedPlayerAddress(int recordId) => CmdRemoveBannedPlayerAddress.ProcessCommand(recordId);

        public bool RemoveBannedPlayerBoth(int recordId) => CmdRemoveBannedPlayerBoth.ProcessCommand(recordId);

        public bool RemoveBannedPlayerName(int recordId) => CmdRemoveBannedPlayerName.ProcessCommand(recordId);

        public void SaveSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }
        public void UpdateBannedTables()
        {
            if (!theInstance.banInstanceUpdated && 
               !(thisServer.dg_playerNames.Rows.Count != instanceBans.BannedPlayerNames.Count 
                 || thisServer.dg_IPAddresses.Rows.Count != instanceBans.BannedPlayerAddresses.Count))
            { return; }

            // Clear existing rows
            thisServer.dg_playerNames.Rows.Clear();
            thisServer.dg_IPAddresses.Rows.Clear();

            // Add banned player names
            foreach (var bannedName in instanceBans.BannedPlayerNames)
            {
                // Decode Base64 and interpret as Windows-1252
                byte[] decodedBytes = Convert.FromBase64String(bannedName.playerName);
                string decodedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(decodedBytes);

                // Add row and apply font
                int rowIndex = thisServer.dg_playerNames.Rows.Add(
                    bannedName.recordId,
                    decodedPlayerName
                );
                var row = thisServer.dg_playerNames.Rows[rowIndex];
                row.DefaultCellStyle.Font = thisServer.dg_playerNames.Font;
                // DataGridView uses GDI+ text rendering by default; no need to set UseCompatibleTextRendering
            }

            // Add banned player addresses
            foreach (var bannedAddress in instanceBans.BannedPlayerAddresses)
            {
                thisServer.dg_IPAddresses.Rows.Add(
                    bannedAddress.recordId,
                    $"{bannedAddress.playerIP}/{bannedAddress.subnetMask}"
                );
            }
        }

        public void ExportSettings()
        {
            // Export the current ban settings to a JSON file using the Functions SaveFileDialog to allow the user to choose file location.
            string exportPath = Functions.ShowFileDialog(true, "JSON Files (*.json)|*.json|All Files (*.*)|*.*", "Export Ban Settings", CommonCore.AppDataPath, "BanSettings.json");
            if (string.IsNullOrEmpty(exportPath))
            {
                AppDebug.Log("BanManagement", "Export cancelled by user.");
                return;
            }
            else
            {
                // Write the instanceBans data to the specified file.
                try
                {
                    var settings = new BanSettingsFile
                    {
                        BannedPlayerNames = instanceBans.BannedPlayerNames,
                        BannedPlayerAddresses = instanceBans.BannedPlayerAddresses,
                        _recordIdCounter = instanceBans._recordIdCounter
                    };
                    var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(exportPath, json);
                    AppDebug.Log("BanManagement", $"Ban settings exported successfully to {exportPath}");
                }
                catch (Exception ex)
                {
                    AppDebug.Log("BanManagement", $"Error exporting ban settings: {ex.Message}");
                }
            }
        }

        public void ImportSettings()
        {
            throw new NotImplementedException();
        }

        void banInstanceInterface.ImportSettingsFromBase64(string encodedData, string fileType)
        {
            throw new NotImplementedException();
        }
    }
}
