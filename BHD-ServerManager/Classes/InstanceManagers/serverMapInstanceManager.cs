using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System.Reflection;
using System.Text;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverMapInstanceManager : mapInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;

        public void ResetAvailableMaps()
        {
            instanceMaps.availableMaps.Clear();
            instanceMaps.customMaps.Clear();
            mapInstanceManager.LoadDefaultMaps();

            if (!theInstanceManager.ValidateGameServerPath()) return;
            mapInstanceManager.LoadCustomMaps();
        }
        public void LoadDefaultMaps()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "BHD_ServerManager.DataStores.defaultMapsBHD.csv";

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using StreamReader reader = new(stream, Encoding.UTF8);

            bool isFirstLine = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (isFirstLine)
                {
                    isFirstLine = false; // Skip header
                    continue;
                }
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length < 5) continue;

                mapFileInfo tempMapFile = new mapFileInfo
                {
                    MapID = int.Parse(parts[0]),
                    MapName = parts[1],
                    MapFile = parts[2],
                    ProfileServerType = int.Parse(parts[3]),
                    GameType = parts[4],
                    GameTypes = new List<int>()
                };
                foreach (var gameType in objectGameTypes.All)
                {
                    if (gameType.ShortName!.Equals(tempMapFile.GameType, StringComparison.OrdinalIgnoreCase))
                    {
                        tempMapFile.GameTypes.Add(gameType.Bitmap);
                    }
                }
                instanceMaps.availableMaps.Add(tempMapFile);
            }
        }
        public void LoadCustomMaps()
        {
            DirectoryInfo d = new DirectoryInfo(theInstance.profileServerPath!);
            List<string> badMapsList = new List<string>();

            foreach (var file in d.GetFiles("*.bms"))
            {
                using (FileStream fsSourceDDS = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
                {
                    string first_line = string.Empty;
                    string mapName = string.Empty;
                    try
                    {
                        first_line = File.ReadLines(Path.Combine(theInstance.profileServerPath!, file.Name), Encoding.Default).First().ToString();
                    }
                    catch (Exception e)
                    {
                        AppDebug.Log("Skipping infoCurrentMapName File", e.Message);
                        continue;
                    }

                    first_line = first_line.Replace("", "|").Replace("\0\0\0", "|");
                    string[] first_line_arr = first_line.Split("|".ToCharArray());
                    var first_line_list = new List<string>();
                    foreach (string f in first_line_arr)
                    {
                        string tmp = f.Trim().Replace("\0", "".ToString());
                        if (!string.IsNullOrEmpty(tmp))
                            first_line_list.Add(tmp);
                    }
                    try
                    {
                        mapName = first_line_list[1];
                    }
                    catch
                    {
                        badMapsList.Add(file.Name);
                        continue;
                    }
                    string mapFile = file.Name;

                    // Read BitmapBytes sum at 0x8A
                    fsSourceDDS.Seek(0x8A, SeekOrigin.Begin);
                    var bitmapBytesSum = binaryReader.ReadInt16();

                    // Read Bitmap sum at 0x8B
                    fsSourceDDS.Seek(0x8B, SeekOrigin.Begin);
                    var bitmapSum = binaryReader.ReadInt16();

                    // Get all possible BitmapBytes and Bitmap values (excluding 0)
                    var bitmapBytesNumbers = objectGameTypes.All.Select(gt => gt.BitmapBytes).Where(b => b > 0).OrderByDescending(b => b).ToList();
                    var bitmapNumbers = objectGameTypes.All.Select(gt => gt.Bitmap).Where(b => b > 0).OrderByDescending(b => b).ToList();

                    // Find all game types for this map
                    var gameTypeBits = new List<int>();
                    CalculateGameTypeBits(bitmapBytesNumbers, bitmapBytesSum, gameTypeBits);
                    if (gameTypeBits.Count == 0)
                    {
                        CalculateGameTypeBits(bitmapNumbers, bitmapSum, gameTypeBits);
                    }

                    // If still nothing, check for special case (Attack and Defend)
                    if (gameTypeBits.Count == 0 && (bitmapBytesSum == 128 || bitmapSum == 0))
                    {
                        gameTypeBits.Add(0); // Attack and Defend
                    }

                    // Map to game type info and add a separate entry for each supported game type
                    var gameTypes = new List<int>();
                    foreach (var bit in gameTypeBits)
                    {
                        var match = objectGameTypes.All.FirstOrDefault(gt => gt.Bitmap == bit || gt.BitmapBytes == bit);
                        if (match != null)
                        {
                            gameTypes.Add(match.DatabaseId);

                            var mapEntry = new mapFileInfo
                            {
                                MapID = instanceMaps.availableMaps.Count,
                                MapFile = mapFile,
                                MapName = mapName,
                                GameType = match.ShortName ?? string.Empty,
                                GameTypes = new List<int> { match.DatabaseId },
                                CustomMap = true,
                                ProfileServerType = 0, // Set as needed
                                gameTypeBits = new List<int> { bit }
                            };
                            instanceMaps.availableMaps.Add(mapEntry);
                            instanceMaps.customMaps.Add(mapEntry);
                        }
                        else
                        {
                            MessageBox.Show("File Name: " + mapFile + "\n" + " with Bitmap/BitmapBytes: " + bit + "\n" + "Reason: Could not find gametype for map.");
                        }
                    }
                }
            }

            if (badMapsList.Count > 0)
            {
                string badMaps = string.Join("\n", badMapsList);
                MessageBox.Show("Could not read map title from:\n" + badMaps + "\nThis could due to a non-converted, or a corrupted file.", "infoCurrentMapName List Error");
            }
        }
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
                savePath = Path.Combine(CommonCore.AppDataPath, "currentMapPlaylist.mpl");
            }

            try
            {
                var sb = new StringBuilder();
                foreach (var map in mapList)
                {
                    sb.AppendLine($"{map.MapFile},{map.MapName},{map.GameType}");
                }
                File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save map playlist:\n" + ex.Message, "Save Error");
            }
        }
        public List<mapFileInfo> BuildCurrentMapPlaylist()
        {
            DataGridView ogList = thisServer.dataGridView_currentMaps;

            var newPlaylist = new List<mapFileInfo>();


            foreach (DataGridViewRow row in ogList.Rows)
            {
                if (row.IsNewRow) continue;

                string? mapName = row.Cells["MapName"].Value?.ToString();
                string? gameType = row.Cells["GameType"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(mapName) || string.IsNullOrWhiteSpace(gameType))
                    continue;

                var map = instanceMaps.availableMaps
                    .FirstOrDefault(m => string.Equals(m.MapName, mapName, StringComparison.OrdinalIgnoreCase)
                                      && string.Equals(m.GameType, gameType, StringComparison.OrdinalIgnoreCase));

                if (map == null)
                    continue;

                newPlaylist.Add(map);
            }

            return newPlaylist;
        }
        public void CalculateGameTypeBits(List<int> numbers, int target, List<int> result)
        {
            void Recurse(List<int> nums, int tgt, List<int> partial)
            {
                int sum = partial.Sum();
                if (sum == tgt)
                {
                    result.AddRange(partial);
                    return;
                }
                if (sum >= tgt)
                    return;

                for (int i = 0; i < nums.Count; i++)
                {
                    var remaining = nums.Skip(i + 1).ToList();
                    var nextPartial = new List<int>(partial) { nums[i] };
                    Recurse(remaining, tgt, nextPartial);
                    if (result.Count > 0) return; // Only need the first valid combination
                }
            }
            result.Clear();
            Recurse(numbers, target, new List<int>());
        }

    }
}
