using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            List<int> numbers = new List<int>() { 128, 64, 32, 16, 8, 4, 2, 1 };
            List<string> badMapsList = new List<string>();

            foreach (var file in d.GetFiles("*.bms"))
            {
                using (FileStream fsSourceDDS = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
                {
                    var map = new mapFileInfo();
                    string first_line = string.Empty;
                    try
                    {
                        first_line = File.ReadLines(Path.Combine(theInstance.profileServerPath!, file.Name), Encoding.Default).First().ToString();
                    }
                    catch (Exception e)
                    {
                        // MessageBox.Show("File Name: " + file.Name + "\n" + "Reason: " + e.Message, "Skipping infoCurrentMapName File");
                        AppDebug.Log("Skipping infoCurrentMapName File", e.Message );
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
                        map.MapName = first_line_list[1];
                    }
                    catch
                    {
                        badMapsList.Add(file.Name);
                        continue;
                    }
                    map.MapFile = file.Name;

                    fsSourceDDS.Seek(0x8A, SeekOrigin.Begin);
                    var attackdefend = binaryReader.ReadInt16();

                    if (attackdefend == 128)
                    {
                        map.gameTypeBits.Add(0);
                    }
                    else
                    {
                        fsSourceDDS.Seek(0x8B, SeekOrigin.Begin);
                        var maptype = binaryReader.ReadInt16();
                        CalculateGameTypeBits(numbers, Convert.ToInt32(maptype), map);
                    }
                    map.CustomMap = true;
                    instanceMaps.customMaps.Add(map);
                }
            }

            foreach (var item in instanceMaps.customMaps)
            {
                List<int> gametypes = new List<int>();
                foreach (var customMapBits in item.gameTypeBits)
                {
                    int gametypeId = -1;
                    foreach (var gametype in objectGameTypes.All)
                    {
                        if (customMapBits == gametype.Bitmap)
                        {
                            gametypeId = gametype.DatabaseId;
                            item.GameType = gametype.ShortName;
                            break;
                        }
                    }
                    if (gametypeId == -1)
                    {
                        MessageBox.Show("File Name: " + item.MapFile + "\n" + " with Bitmap: " + customMapBits + "\n" + "Reason: Could not find gametype for map.");
                        continue;
                    }
                    else
                    {
                        gametypes.Add(gametypeId);
                    }
                }
                item.GameTypes = gametypes;
                item.MapID = instanceMaps.availableMaps.Count;
                instanceMaps.availableMaps.Add(item);
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
        public void CalculateGameTypeBits(List<int> numbers, int target, mapFileInfo map)
        {
            void Recurse(List<int> nums, int tgt, List<int> partial)
            {
                int sum = partial.Sum();
                if (sum == tgt)
                {
                    map.gameTypeBits = new List<int>(partial);
                    return;
                }
                if (sum >= tgt)
                    return;

                for (int i = 0; i < nums.Count; i++)
                {
                    var remaining = nums.Skip(i + 1).ToList();
                    var nextPartial = new List<int>(partial) { nums[i] };
                    Recurse(remaining, tgt, nextPartial);
                }
            }
            Recurse(numbers, target, new List<int>());
        }

    }
}
