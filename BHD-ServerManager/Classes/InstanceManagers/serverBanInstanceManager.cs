using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverBanInstanceManager : banInstanceInterface
    {

        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance instanceBans => CommonCore.instanceBans!;

        // Function: LoadSettings, loads the ban settings from a JSON file. If it does not exist, it initializes empty lists and saves them.
        public void LoadSettings()
        {
            string BanSettingsPath = Path.Combine(CommonCore.AppDataPath, "BanSettings.json");
            if (!File.Exists(BanSettingsPath))
            {
                instanceBans.BannedPlayerNames = new List<BannedPlayerNames>();
                instanceBans.BannedPlayerAddresses = new List<BannedPlayerAddress>();
                instanceBans._recordIdCounter = 1;
                banInstanceManager.SaveSettings();
                return;
            }

            try
            {
                var json = File.ReadAllText(BanSettingsPath);
                var settings = JsonSerializer.Deserialize<BanSettingsFile>(json);
                instanceBans.BannedPlayerNames = settings?.BannedPlayerNames ?? new List<BannedPlayerNames>();
                instanceBans.BannedPlayerAddresses = settings?.BannedPlayerAddresses ?? new List<BannedPlayerAddress>();
                instanceBans._recordIdCounter = settings?._recordIdCounter ?? 1;
            }
            catch (JsonException ex)
            {
                AppDebug.Log("BanManagement", $"Error deserializing ban settings file: {ex.Message}");
            }
            catch (IOException ex)
            {
                AppDebug.Log("BanManagement", $"Error reading ban settings file: {ex.Message}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("BanManagement", $"Unexpected error loading ban settings: {ex.Message}");
            }
        }

        // Function: SaveSettings, saves the current ban settings to a JSON file.
        public void SaveSettings()
        {
            string BanSettingsPath = Path.Combine(CommonCore.AppDataPath, "BanSettings.json");
            var settings = new BanSettingsFile
            {
                BannedPlayerNames = instanceBans.BannedPlayerNames,
                BannedPlayerAddresses = instanceBans.BannedPlayerAddresses,
                _recordIdCounter = instanceBans._recordIdCounter
            };

            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(BanSettingsPath, json);
            }
            catch (IOException ex)
            {
                AppDebug.Log("BanManagement", $"Error writing ban settings file: {ex.Message}");
            }
        }

        // Function: AddBannedPlayer, adds a player to the ban list by name and/or IP address.
        public void AddBannedPlayer(string playerName, IPAddress playerIP = null!, int subnetMask = 32)
        {
            int recordId = instanceBans._recordIdCounter++;

            if (!string.IsNullOrWhiteSpace(playerName))
            {
                AppDebug.Log("BanManagement", $"Adding banned player: {playerName} (ID: {recordId})");
                var newBan = new BannedPlayerNames
                {
                    recordId = recordId,
                    playerName = playerName
                };
                instanceBans.BannedPlayerNames.Add(newBan);
            }
            if (playerIP != null)
            {
                AppDebug.Log("BanManagement", $"Adding banned player IP: {playerIP} (ID: {recordId}) with subnet mask {subnetMask}");
                var newBan = new BannedPlayerAddress
                {
                    recordId = recordId,
                    playerIP = playerIP,
                    subnetMask = subnetMask
                };
                instanceBans.BannedPlayerAddresses.Add(newBan);
            }
            SaveSettings();
        }

        // Function: RemoveBannedPlayerName, removes a banned player by their record ID from the name ban list.
        public bool RemoveBannedPlayerName(int recordId)
        {
            bool removed = instanceBans.BannedPlayerNames.RemoveAll(b => b.recordId == recordId) > 0;
            if (removed) SaveSettings();
            return removed;
        }

        // Function: RemoveBannedPlayerAddress, removes a banned player by their record ID from the address ban list.
        public bool RemoveBannedPlayerAddress(int recordId)
        {
            bool removed = instanceBans.BannedPlayerAddresses.RemoveAll(b => b.recordId == recordId) > 0;
            if (removed) SaveSettings();
            return removed;
        }

        // Function: RemoveBannedPlayerBoth, removes a banned player by their record ID from both the name and address ban lists.
        public bool RemoveBannedPlayerBoth(int recordId)
        {
            bool removedName = RemoveBannedPlayerName(recordId);
            bool removedAddress = RemoveBannedPlayerAddress(recordId);
            return removedName || removedAddress;
        }
        // Function: UpdateBannedTables, updates the DataGridViews in the ServerManager UI with the current banned players.
        public void UpdateBannedTables()
        {
            // Clear existing rows
            thisServer.dg_playerNames.Rows.Clear();
            thisServer.dg_IPAddresses.Rows.Clear();

            // Add banned player names
            foreach (var bannedName in instanceBans.BannedPlayerNames)
            {
                string decodedName = Encoding.UTF8.GetString(Convert.FromBase64String(bannedName.playerName));
                // Adjust the order and number of columns as needed
                thisServer.dg_playerNames.Rows.Add(
                    bannedName.recordId,
                    decodedName
                );
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

    }
}
