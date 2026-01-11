using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Animation;
using BHD_ServerManager.Classes.InstanceInterfaces;

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
            theInstance.banInstanceUpdated = true;
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
            theInstance.banInstanceUpdated = true;
            SaveSettings();
        }

        // Function: RemoveBannedPlayerName, removes a banned player by their record ID from the name ban list.
        public bool RemoveBannedPlayerName(int recordId)
        {
            bool removed = instanceBans.BannedPlayerNames.RemoveAll(b => b.recordId == recordId) > 0;
            if (removed) SaveSettings();
            theInstance.banInstanceUpdated = true;
            return removed;
        }

        // Function: RemoveBannedPlayerAddress, removes a banned player by their record ID from the address ban list.
        public bool RemoveBannedPlayerAddress(int recordId)
        {
            bool removed = instanceBans.BannedPlayerAddresses.RemoveAll(b => b.recordId == recordId) > 0;
            if (removed) SaveSettings();
            theInstance.banInstanceUpdated = true;
            return removed;
        }

        // Function: RemoveBannedPlayerBoth, removes a banned player by their record ID from both the name and address ban lists.
        public bool RemoveBannedPlayerBoth(int recordId)
        {
            bool removedName = RemoveBannedPlayerName(recordId);
            bool removedAddress = RemoveBannedPlayerAddress(recordId);
            theInstance.banInstanceUpdated = true;
            return removedName || removedAddress;
        }
        // Function: UpdateBannedTables, updates the DataGridViews in the ServerManager UI with the current banned players.
        public void UpdateBannedTables()
        {
            if (!theInstance.banInstanceUpdated) { return; }

            theInstance.banInstanceUpdated = false;

            // Clear existing rows
            thisServer.BanTab.dg_playerNames.Rows.Clear();
            thisServer.BanTab.dg_IPAddresses.Rows.Clear();

            // Add banned player names
            foreach (var bannedName in instanceBans.BannedPlayerNames)
            {
                // Decode Base64 and interpret as Windows-1252
                byte[] decodedBytes = Convert.FromBase64String(bannedName.playerName);
                string decodedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(decodedBytes);

                // Add row and apply font
                int rowIndex = thisServer.BanTab.dg_playerNames.Rows.Add(
                    bannedName.recordId,
                    decodedPlayerName
                );
                var row = thisServer.BanTab.dg_playerNames.Rows[rowIndex];
                row.DefaultCellStyle.Font = thisServer.BanTab.dg_playerNames.Font;
                // DataGridView uses GDI+ text rendering by default; no need to set UseCompatibleTextRendering
            }

            // Add banned player addresses
            foreach (var bannedAddress in instanceBans.BannedPlayerAddresses)
            {
                thisServer.BanTab.dg_IPAddresses.Rows.Add(
                    bannedAddress.recordId,
                    $"{bannedAddress.playerIP}/{bannedAddress.subnetMask}"
                );
            }
        }

        public void ExportSettings()
        {
            SaveSettings();
            // Export the current ban settings to a JSON file using the Functions SaveFileDialog to allow the user to choose file location.
            string exportPath = Functions.ShowFileDialog(true, "JSON Files (*.json)|*.json|All Files (*.*)|*.*", "Export Ban Settings", CommonCore.AppDataPath, "BanSettings.json");
            if (string.IsNullOrEmpty(exportPath))
            {
                AppDebug.Log("BanManagement", "Export cancelled by user.");
                return;
            } else
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

        // Main entry: Import from file dialog (existing UI)
        public void ImportSettings()
        {
            string importPath = Functions.ShowFileDialog(
                false,
                "Ban Files (*.json;*.ini)|*.json;*.ini|JSON Files (*.json)|*.json|INI Files (*.ini)|*.ini",
                "Import Ban Settings",
                CommonCore.AppDataPath,
                "BanSettings.json"
            );
            if (string.IsNullOrEmpty(importPath))
            {
                AppDebug.Log("BanManagement", "Import cancelled by user.");
                return;
            }
            ImportSettingsFromFile(importPath);
        }

        // New entry: Import from remote (Base64-encoded string)
        public void ImportSettingsFromBase64(string base64Data, string fileType)
        {
            try
            {
                // Decode the base64 string to get the file content
                byte[] dataBytes = Convert.FromBase64String(base64Data);
                string fileContent = Encoding.GetEncoding("Windows-1252").GetString(dataBytes);

                if (fileType.Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    ImportSettingsFromString(fileContent, isJson: true);
                }
                else if (fileType.Equals(".ini", StringComparison.OrdinalIgnoreCase))
                {
                    ImportSettingsFromString(fileContent, isJson: false);
                }
                else
                {
                    AppDebug.Log("BanManagement", $"Unsupported file type: {fileType}");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("BanManagement", $"Error importing ban settings from remote: {ex.Message}");
            }
        }

        // Internal: Import from file path
        private void ImportSettingsFromFile(string importPath)
        {
            try
            {
                string ext = Path.GetExtension(importPath).ToLowerInvariant();
                if (ext == ".json")
                {
                    string json = File.ReadAllText(importPath, Encoding.GetEncoding("Windows-1252"));
                    ImportSettingsFromString(json, isJson: true);
                }
                else if (ext == ".ini")
                {
                    string ini = File.ReadAllText(importPath, Encoding.GetEncoding("Windows-1252"));
                    ImportSettingsFromString(ini, isJson: false);
                }
                else
                {
                    AppDebug.Log("BanManagement", $"Unsupported file type: {ext}");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("BanManagement", $"Error importing ban settings file: {ex.Message}");
            }
        }

        // Internal: Import from string content (used by both file and remote)
        private void ImportSettingsFromString(string content, bool isJson)
        {
            try
            {
                if (isJson)
                {
                    var settings = JsonSerializer.Deserialize<BanSettingsFile>(content);
                    if (settings != null)
                    {
                        foreach (var ban in settings.BannedPlayerNames)
                        {
                            if (!instanceBans.BannedPlayerNames.Any(b => b.playerName == ban.playerName))
                            {
                                AddBannedPlayer(ban.playerName, null!);
                            }
                        }
                        foreach (var ban in settings.BannedPlayerAddresses)
                        {
                            if (!instanceBans.BannedPlayerAddresses.Any(b =>
                                b.playerIP.Equals(ban.playerIP) && b.subnetMask == ban.subnetMask))
                            {
                                AddBannedPlayer("", ban.playerIP, ban.subnetMask);
                            }
                        }
                        theInstance.banInstanceUpdated = true;
                        UpdateBannedTables();
                        SaveSettings();
                        AppDebug.Log("BanManagement", $"Ban settings imported and merged successfully from string data");
                    }
                    else
                    {
                        AppDebug.Log("BanManagement", "Failed to deserialize ban settings.");
                    }
                }
                else // INI
                {
                    ImportIniBanString(content);
                    theInstance.banInstanceUpdated = true;
                    UpdateBannedTables();
                    SaveSettings();
                    AppDebug.Log("BanManagement", $"Legacy INI ban data imported and merged successfully from string data");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("BanManagement", $"Error importing ban settings from string: {ex.Message}");
            }
        }

        // Helper: Import INI from string content
        private void ImportIniBanString(string iniContent)
        {
            var lines = iniContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            string? currentSection = null;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line[1..^1];
                    continue;
                }

                int eqIdx = line.IndexOf('=');
                if (eqIdx < 0) continue;
                string key = line[..eqIdx].Trim();
                string value = line[(eqIdx + 1)..].Trim();

                if (string.IsNullOrEmpty(currentSection))
                    continue;

                if (currentSection == "Players")
                {
                    // Remove trailing [n] if present
                    string playerName = key;
                    int bracketIndex = playerName.IndexOf('[');
                    if (bracketIndex > 0)
                        playerName = playerName.Substring(0, bracketIndex);

                    playerName = playerName.Trim();

                    if (!string.IsNullOrEmpty(playerName))
                    {
                        string encodedName = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(playerName));
                        // Only add if not already present
                        if (!instanceBans.BannedPlayerNames.Any(b => b.playerName == encodedName))
                        {
                            int recordId = instanceBans._recordIdCounter++;
                            var newBan = new BannedPlayerNames
                            {
                                recordId = recordId,
                                playerName = encodedName
                            };
                            instanceBans.BannedPlayerNames.Add(newBan);
                        }
                    }
                }
                else if (currentSection == "IpAddresses")
                {
                    string ipPattern = key;
                    if (ipPattern.Contains('*'))
                    {
                        var octets = ipPattern.Split('.');
                        int subnetMask = 0;
                        string baseIp = "";

                        if (octets.Length == 4 && octets[3] == "*")
                        {
                            subnetMask = 24;
                            baseIp = $"{octets[0]}.{octets[1]}.{octets[2]}.0";
                        }
                        else if (octets.Length == 3 && octets[2] == "*")
                        {
                            subnetMask = 16;
                            baseIp = $"{octets[0]}.{octets[1]}.0.0";
                        }
                        else if (octets.Length == 2 && octets[1] == "*")
                        {
                            subnetMask = 8;
                            baseIp = $"{octets[0]}.0.0.0";
                        }
                        else
                        {
                            subnetMask = 32;
                            baseIp = ipPattern.Replace("*", "0");
                        }

                        if (IPAddress.TryParse(baseIp, out var ip))
                        {
                            if (!instanceBans.BannedPlayerAddresses.Any(b => b.playerIP.Equals(ip) && b.subnetMask == subnetMask))
                            {
                                int recordId = instanceBans._recordIdCounter++;
                                var newIpBan = new BannedPlayerAddress
                                {
                                    recordId = recordId,
                                    playerIP = ip,
                                    subnetMask = subnetMask
                                };
                                instanceBans.BannedPlayerAddresses.Add(newIpBan);
                            }
                        }
                    }
                    else if (IPAddress.TryParse(ipPattern, out var ip))
                    {
                        if (!instanceBans.BannedPlayerAddresses.Any(b => b.playerIP.Equals(ip) && b.subnetMask == 32))
                        {
                            int recordId = instanceBans._recordIdCounter++;
                            var newIpBan = new BannedPlayerAddress
                            {
                                recordId = recordId,
                                playerIP = ip,
                                subnetMask = 32
                            };
                            instanceBans.BannedPlayerAddresses.Add(newIpBan);
                        }
                    }
                }
                else if (currentSection != "" && currentSection != "Players" && currentSection != "IpAddresses")
                {
                    // Per-player section
                    string playerName = currentSection.Trim();
                    string encodedName = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(playerName));
                    int recordId;

                    // Check if player name already exists
                    var existingBan = instanceBans.BannedPlayerNames.FirstOrDefault(b => b.playerName == encodedName);
                    if (existingBan == null)
                    {
                        // Add player name and get new recordId
                        recordId = instanceBans._recordIdCounter++;
                        var newBan = new BannedPlayerNames
                        {
                            recordId = recordId,
                            playerName = encodedName
                        };
                        instanceBans.BannedPlayerNames.Add(newBan);
                    }
                    else
                    {
                        // Use existing recordId
                        recordId = existingBan.recordId;
                    }

                    // Add IP if not already present for this recordId
                    if (IPAddress.TryParse(value, out var ip))
                    {
                        bool ipExists = instanceBans.BannedPlayerAddresses.Any(b =>
                            b.playerIP.Equals(ip) && b.subnetMask == 32 && b.recordId == recordId);

                        if (!ipExists)
                        {
                            var newIpBan = new BannedPlayerAddress
                            {
                                recordId = recordId,
                                playerIP = ip,
                                subnetMask = 32
                            };
                            instanceBans.BannedPlayerAddresses.Add(newIpBan);
                        }
                    }
                }
            }
        }
    }
}
