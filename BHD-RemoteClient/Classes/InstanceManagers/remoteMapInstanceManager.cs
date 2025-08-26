using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;

namespace BHD_RemoteClient.Classes.InstanceManagers
{

    public class remoteMapInstanceManager : mapInstanceInterface
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        private static mapInstance instanceMaps = CommonCore.instanceMaps!;

        public List<mapFileInfo> BuildCurrentMapPlaylist()
        {
            DataGridView ogList = thisServer.MapsTab.dataGridView_currentMaps;

            var newPlaylist = new List<mapFileInfo>();


            foreach (DataGridViewRow row in ogList.Rows)
            {
                if (row.IsNewRow) continue;

                string? mapName = row.Cells["current_MapName"].Value?.ToString();
                string? gameType = row.Cells["current_MapType"].Value?.ToString();
                string? mapFile = row.Cells["current_MapFileName"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(mapName) || string.IsNullOrWhiteSpace(gameType))
                    continue;

                var map = instanceMaps.availableMaps
                    .FirstOrDefault(m => string.Equals(m.MapName, mapName, StringComparison.OrdinalIgnoreCase)
                                        && string.Equals(m.MapFile, mapFile, StringComparison.OrdinalIgnoreCase)
                                        && string.Equals(m.MapType, gameType, StringComparison.OrdinalIgnoreCase));

                if (map == null)
                    continue;

                newPlaylist.Add(map);
            }

            return newPlaylist;
        }

        void mapInstanceInterface.CalculateGameTypeBits(List<int> numbers, int target, List<int> result)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void mapInstanceInterface.LoadCustomMaps()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void mapInstanceInterface.LoadDefaultMaps()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void ResetAvailableMaps() => CmdResetAvailableMaps.ProcessCommand();

        public void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external)
        {
            string savePath = CommonCore.AppDataPath;

            if (external)
            {
                savePath = Functions.ShowFileDialog(true, "Map Playlist (*.mpl)|*.mpl|All files (*.*)|*.*", "Save Map Playlist", savePath, "MapPlaylist.mpl")!;
                if (savePath == null)
                {
                    return; // User cancelled
                }
            }
            else
            {
                return;  // No Remote Side
            }

            try
            {
                var sb = new StringBuilder();
                foreach (var map in mapList)
                {
                    sb.AppendLine($"{map.MapFile},{map.MapName},{map.MapType}");
                }
                File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save map playlist:\n" + ex.Message, "Save Error");
            }
        }

        public List<mapFileInfo> LoadCustomMapPlaylist(bool external)
        {
            string? startPath = CommonCore.AppDataPath;
            string[] lines;

            List<mapFileInfo> newMapPlaylist = new List<mapFileInfo>();

            if (external)
            {
                try
                {
                    lines = GetFileLinesFromDialog(false, "Map Playlist (*.mpl)|*.mpl|All files (*.*)|*.*", "Open Map Playlist File", startPath, "currentMapPlaylist.mpl")!;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open map playlist file:\n" + ex.Message, "Open Error");
                    return null!;
                }
            }
            else
            {
                // No Need On Remote Side Just Return Current Playlist as it is updated from server.
                newMapPlaylist = instanceMaps.currentMapPlaylist.ToList();
                return newMapPlaylist;
            }
           

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                string decodedMapFile = TryDecodeBase64(parts[0], Encoding.Default);
                string decodedMapName = TryDecodeBase64(parts[1], Encoding.GetEncoding("Windows-1252"));

                var mapItem = instanceMaps.availableMaps
                    .FirstOrDefault(m => string.Equals(m.MapFile, decodedMapFile, StringComparison.OrdinalIgnoreCase)
                                      && string.Equals(m.MapName, decodedMapName, StringComparison.OrdinalIgnoreCase)
                                      && string.Equals(m.MapType, parts[2], StringComparison.OrdinalIgnoreCase));

                if (mapItem != null)
                {
                    newMapPlaylist.Add(mapItem);
                }
                else
                {
                    MessageBox.Show($"Map file '{decodedMapFile}' does not exist in the server path.", "File Not Found");
                    AppDebug.Log("remoteMapInstanceManager", $"Map file '{decodedMapFile}' does not exist in the server path.");
                    continue; // Skip this map if the file doesn't exist
                }
            }

            return newMapPlaylist;
        }

        public string[]? GetFileLinesFromDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName)
        {
            string? filePath = Functions.ShowFileDialog(saveDialog, filter, title, initialDirectory, defaultFileName);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;
            return File.ReadAllLines(filePath, Encoding.Default);
        }

        // Helper: Try to decode Base64, fallback to plain text
        private static string TryDecodeBase64(string input, Encoding encoding)
        {
            input = input.Trim();
            if ((input.Length % 4 == 0) && System.Text.RegularExpressions.Regex.IsMatch(input, @"^[A-Za-z0-9\+/]*={0,2}$"))
            {
                try
                {
                    return encoding.GetString(Convert.FromBase64String(input));
                }
                catch
                {
                    // Not valid Base64, fall through
                }
            }
            return input;
        }
    }
}
