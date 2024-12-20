using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ServerManager.Classes.Objects;

namespace ServerManager.Classes.Enviroment
{
    class ServerMapPlaylists // Changed to PascalCase
    {
        public List<ObjectMap> CurrentMapPlaylist { get; set; }
        public Dictionary<string, List<ObjectMap>> SavedMapPlaylists { get; set; }
    }

    public class ServerMapManagement
    {
        // System Wide Vars
        [JsonIgnore]
        protected ServerEnvironment _env;
        [JsonIgnore]
        protected ServerInstance _instance;
        [JsonIgnore]
        private string _mapListPath;

        // Map Manager
        [JsonIgnore]
        public List<ObjectMap> DefaultMapList;
        [JsonIgnore]
        public List<ObjectMap> CustomMapList;

        public List<ObjectMap> CurrentMapPlaylist;
        public Dictionary<string, List<ObjectMap>> SavedMapPlaylists;

        public ServerMapManagement(ServerInstance instance)
        {
            // System Wide Vars
            _env = ServerEnvironment.Instance;
            _instance = instance;
            _mapListPath = Path.Combine(_instance.serverProfile.ProfilePath, "mapPlaylists.json");

            // Map Manager Related
            DefaultMapList = new List<ObjectMap>();
            CustomMapList = new List<ObjectMap>();

            LoadSettings();

            // Init Refresh of Maps
            refreshDefaultMapList();
            refreshCustomMapList();
        }

        public void refreshDefaultMapList()
        {
            int mod = _instance.serverSettings.CurrentSettings.GameMod;
            // Add Default BHD Maps to List - Regardless
            DefaultMapList = _env._defaultMaps[0];
            // Add Default BHD TS Maps to List
            if (_env._defaultMaps[mod] != null && _env._defaultMaps[mod].Count > 0)
            {
                DefaultMapList.AddRange(_env._defaultMaps[mod]);
            }
        }

        public void refreshCustomMapList()
        {
            DirectoryInfo d = new DirectoryInfo(_instance.serverProfile.ServerPath);
            List<ObjectMap> objectMaps = new List<ObjectMap>();
            List<int> numbers = new List<int>() { 128, 64, 32, 16, 8, 4, 2, 1 };

            foreach (var file in d.GetFiles("*.bms"))
            {
                using (FileStream fsSourceDDS = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
                {
                    var objectMap = new ObjectMap();
                    string firstLine = File.ReadLines(file.FullName, Encoding.Default).First().ToString();
                    firstLine = firstLine.Replace("", "|").Replace("\0\0\0", "|");
                    string[] firstLineArr = firstLine.Split("|".ToCharArray());
                    var firstLineList = new List<string>();

                    foreach (string f in firstLineArr)
                    {
                        string tmp = f.Trim().Replace("\0", string.Empty);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            firstLineList.Add(tmp);
                        }
                    }

                    try
                    {
                        objectMap.mission_name = firstLineList[1];
                    }
                    catch
                    {
                        continue; // Skip if map name is not valid
                    }

                    objectMap.mission_file = file.Name;
                    objectMap.CustomMap = true; // Assuming all processed maps are custom
                    objectMap.GameTypes = new List<int>();

                    fsSourceDDS.Seek(0x8A, SeekOrigin.Begin);
                    var attackDefend = binaryReader.ReadInt16();

                    if (attackDefend == 128)
                    {
                        // TODO: Confirm this... seems odd.
                        objectMap.gametype = "AD";
                        objectMap.GameTypes.Add(0);
                    }
                    else
                    {
                        fsSourceDDS.Seek(0x8B, SeekOrigin.Begin);
                        var mapType = binaryReader.ReadInt16();

                        // Merged sum_up logic
                        int target = Convert.ToInt32(mapType);
                        DefaultMaps.sum_up_recursive(numbers, target, new List<int>(), ref objectMap);
                        objectMap.CustomMap = true; // Set a default or derived game type
                    }

                    objectMaps.Add(objectMap);
                }
            }

            CustomMapList = objectMaps;
        }

        public void SaveSettings()
        {
            SavedMapPlaylists["Current"] = CurrentMapPlaylist; // Directly assign CurrentMapPlaylist
            var json = JsonConvert.SerializeObject(new ServerMapPlaylists { SavedMapPlaylists = this.SavedMapPlaylists });
            Directory.CreateDirectory(Path.GetDirectoryName(_mapListPath));
            File.WriteAllText(_mapListPath, json);
        }

        private void LoadSettings()
        {
            if (File.Exists(_mapListPath))
            {
                var json = File.ReadAllText(_mapListPath);
                ServerMapPlaylists mapData = JsonConvert.DeserializeObject<ServerMapPlaylists>(json);
                SavedMapPlaylists = mapData?.SavedMapPlaylists ?? new Dictionary<string, List<ObjectMap>>();
                CurrentMapPlaylist = SavedMapPlaylists["Current"]; // Directly assign from SavedMapPlaylists
            }
            else
            {
                CurrentMapPlaylist = new List<ObjectMap>();
                SavedMapPlaylists = new Dictionary<string, List<ObjectMap>> { { "Current", CurrentMapPlaylist } };
                SaveSettings();
            }
        }

    }
}