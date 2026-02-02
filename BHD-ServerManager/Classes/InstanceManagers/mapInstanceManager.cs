using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using HawkSyncShared.SupportClasses;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Windows.Gaming.Input;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    // ================================================================================
    // DTOs (Data Transfer Objects)
    // ================================================================================

    /// <summary>
    /// Result object for map scanning operations
    /// </summary>
    public record MapScanResult(
        bool Success,
        List<mapFileInfo> Maps,
        List<string> SkippedFiles,
        string ErrorMessage = ""
    );

    /// <summary>
    /// Result object for playlist operations
    /// </summary>
    public record PlaylistResult(
        bool Success,
        string Message = "",
        int PlaylistID = 0,
        int MapCount = 0,
        Exception? Exception = null
    );

    /// <summary>
    /// Available maps result
    /// </summary>
    public record AvailableMapsResult(
        List<mapFileInfo> DefaultMaps,
        List<mapFileInfo> CustomMaps,
        int TotalCount
    );

    // ================================================================================
    // Map Instance Manager - Business Logic Layer
    // ================================================================================

    public static class mapInstanceManager
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static mapInstance mapInstance => CommonCore.instanceMaps!;

        // ================================================================================
        // MAP LOADING & SCANNING
        // ================================================================================

        /// <summary>
        /// Load all available maps (default + custom)
        /// </summary>
        public static AvailableMapsResult LoadAvailableMaps()
        {
            try
            {
                mapInstance.DefaultMaps.Clear();
                mapInstance.CustomMaps.Clear();

                var defaultMaps = DatabaseManager.GetDefaultMaps();
                var customMapsResult = ScanCustomMaps(defaultMaps.Count);

                mapInstance.DefaultMaps.AddRange(defaultMaps);
                mapInstance.CustomMaps.AddRange(customMapsResult.Maps);

                var allMaps = new List<mapFileInfo>();
                allMaps.AddRange(defaultMaps.Where(m => m.ModType == 0));
                allMaps.AddRange(customMapsResult.Maps);

                AppDebug.Log("mapInstanceManager", $"Loaded {defaultMaps.Count} default maps, {customMapsResult.Maps.Count} custom maps");

                return new AvailableMapsResult(
                    DefaultMaps: defaultMaps.Where(m => m.ModType == 0).ToList(),
                    CustomMaps: customMapsResult.Maps,
                    TotalCount: allMaps.Count
                );
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error loading available maps: {ex.Message}");
                return new AvailableMapsResult(
                    DefaultMaps: new List<mapFileInfo>(),
                    CustomMaps: new List<mapFileInfo>(),
                    TotalCount: 0
                );
            }
        }

        /// <summary>
        /// Scan filesystem for custom .bms map files
        /// </summary>
        public static MapScanResult ScanCustomMaps(int DefaultCount)
        {
            var customMaps = new List<mapFileInfo>();
            var skippedFiles = new List<string>();

            try
            {
                string gamePath = theInstance.profileServerPath;

                if (string.IsNullOrEmpty(gamePath))
                {
                    return new MapScanResult(false, customMaps, skippedFiles, "Server game path not configured.");
                }

                if (!Directory.Exists(gamePath))
                {
                    return new MapScanResult(false, customMaps, skippedFiles, $"Server path does not exist: {gamePath}");
                }

                DirectoryInfo directory = new DirectoryInfo(gamePath);
                var bmsFiles = directory.GetFiles("*.bms");

                int customMapStartID = DefaultCount;

                foreach (var file in bmsFiles)
                {
                    var result = ParseMapFile(file.FullName, customMapStartID);

                    if (result.Success)
                    {
                        customMaps.AddRange(result.Maps);
                        customMapStartID += result.Maps.Count;
                    }
                    else
                    {
                        skippedFiles.Add(file.Name);
                        AppDebug.Log("mapInstanceManager", $"Skipped map file: {file.Name} - {result.ErrorMessage}");
                    }
                    
                }

                AppDebug.Log("mapInstanceManager", $"Scanned {bmsFiles.Length} files, found {customMaps.Count} maps, skipped {skippedFiles.Count}");

                return new MapScanResult(true, customMaps, skippedFiles);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error scanning custom maps: {ex.Message}");
                return new MapScanResult(false, customMaps, skippedFiles, ex.Message);
            }
        }

        /// <summary>
        /// Parse a .bms map file to extract map name and supported game types
        /// </summary>
        private static MapScanResult ParseMapFile(string filePath, int mapID)
        {
            var maps = new List<mapFileInfo>();

            try
            {
                string fileName = Path.GetFileName(filePath);

                using (FileStream fsSourceDDS = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
                {
                    string first_line = string.Empty;
                    string mapName = string.Empty;
            
                    try
                    {
                        first_line = File.ReadLines(filePath, Encoding.Default).First().ToString();
                    }
                    catch (Exception e)
                    {
                        return new MapScanResult(false, maps, new List<string>(), $"Cannot read file: {e.Message}");
                    }

                    // Parse map name - use the exact same logic as original
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
                        return new MapScanResult(false, maps, new List<string>(), "Cannot parse map name from file header");
                    }
            
                    string mapFile = fileName;

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
                    int currentMapID = mapID;
            
                    foreach (var bit in gameTypeBits)
                    {
                        var match = objectGameTypes.All.FirstOrDefault(gt => gt.Bitmap == bit || gt.BitmapBytes == bit);

                        if (match != null)
                        {
                            gameTypes.Add(match.DatabaseId);

                            var mapEntry = new mapFileInfo
                            {
                                MapID = currentMapID++,
                                MapFile = mapFile,
                                MapName = mapName,
                                MapType = match.DatabaseId,
                                ModType = 9,
                            };
                    
                            maps.Add(mapEntry);
                        }
                        else
                        {
                            AppDebug.Log("mapInstanceManager", 
                                $"Warning: File '{mapFile}' has Bitmap/BitmapBytes: {bit} but could not find matching game type");
                        }
                    }

                    if (maps.Count == 0)
                    {
                        return new MapScanResult(false, maps, new List<string>(), 
                            $"No valid game types found (BitmapBytes: {bitmapBytesSum}, Bitmap: {bitmapSum})");
                    }
                }

                return new MapScanResult(true, maps, new List<string>());
            }
            catch (Exception ex)
            {
                return new MapScanResult(false, maps, new List<string>(), $"Error parsing file: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculate which game type bits are set in the map file
        /// Uses recursive algorithm to find valid combinations
        /// </summary>
        private static void CalculateGameTypeBits(List<int> numbers, int target, List<int> result)
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
                    if (result.Count > 0) return; // Only need first valid combination
                }
            }

            result.Clear();
            Recurse(numbers, target, new List<int>());
        }

        // ================================================================================
        // PLAYLIST OPERATIONS
        // ================================================================================

        /// <summary>
        /// Load a playlist from database
        /// </summary>
        public static PlaylistResult LoadPlaylist(int playlistID)
        {
            try
            {
                // Validation
                if (playlistID < 1 || playlistID > 5)
                    return new PlaylistResult(false, "Invalid playlist ID. Must be between 1 and 5.");

                // Load from database
                var maps = DatabaseManager.GetPlaylistMaps(playlistID);

                // Update in-memory playlist
                mapInstance.Playlists[playlistID] = maps;

                AppDebug.Log("mapInstanceManager", $"Loaded playlist {playlistID} with {maps.Count} maps");

                return new PlaylistResult(true, "Playlist loaded successfully.", playlistID, maps.Count);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error loading playlist {playlistID}: {ex.Message}");
                return new PlaylistResult(false, $"Error: {ex.Message}", playlistID, 0, ex);
            }
        }

        /// <summary>
        /// Save a playlist to database
        /// </summary>
        public static PlaylistResult SavePlaylist(int playlistID, List<mapFileInfo> maps)
        {
            try
            {
                // Validation
                if (playlistID < 1 || playlistID > 5)
                    return new PlaylistResult(false, "Invalid playlist ID. Must be between 1 and 5.");

                if (maps == null || maps.Count == 0)
                    return new PlaylistResult(false, "Cannot save an empty playlist.");

                // Validate maps exist
                var availableMaps = LoadAvailableMaps();
                var availableMapLookup = new HashSet<(string MapFile, int MapType)>();

                foreach (var map in availableMaps.DefaultMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));
                foreach (var map in availableMaps.CustomMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));

                foreach (var map in maps)
                {
                    if (!availableMapLookup.Contains((map.MapFile, map.MapType)))
                    {
                        return new PlaylistResult(false, 
                            $"Map '{map.MapName}' ({map.MapFile}) is no longer available and cannot be saved.");
                    }
                }

                // Save to database
                DatabaseManager.SavePlaylist(playlistID, maps);

                // Update in-memory
                mapInstance.Playlists[playlistID] = maps;

                AppDebug.Log("mapInstanceManager", $"Saved playlist {playlistID} with {maps.Count} maps");

                return new PlaylistResult(true, $"Playlist {playlistID} saved successfully.", playlistID, maps.Count);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error saving playlist {playlistID}: {ex.Message}");
                return new PlaylistResult(false, $"Error: {ex.Message}", playlistID, 0, ex);
            }
        }

        /// <summary>
        /// Get maps in a playlist
        /// </summary>
        public static (bool success, List<mapFileInfo> maps, string errorMessage) GetPlaylistMaps(int playlistID)
        {
            try
            {
                if (playlistID < 0 || playlistID > 5)
                    return (false, new List<mapFileInfo>(), "Invalid playlist ID.");

                if (!mapInstance.Playlists.ContainsKey(playlistID))
                    return (false, new List<mapFileInfo>(), $"Playlist {playlistID} not initialized.");

                return (true, mapInstance.Playlists[playlistID], string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error getting playlist maps: {ex.Message}");
                return (false, new List<mapFileInfo>(), ex.Message);
            }
        }

        /// <summary>
        /// Set active playlist
        /// </summary>
        public static PlaylistResult SetActivePlaylist(int playlistID, bool updateServerMemory = true)
        {
            try
            {
                // Validation
                if (playlistID < 1 || playlistID > 5)
                    return new PlaylistResult(false, "Invalid playlist ID. Must be between 1 and 5.");

                var (success, maps, error) = GetPlaylistMaps(playlistID);
                if (!success || maps.Count == 0)
                    return new PlaylistResult(false, $"Playlist {playlistID} is empty or not loaded.");

                // Check server status
                if (theInstance.instanceStatus == InstanceStatus.SCORING)
                    return new PlaylistResult(false, "Cannot change playlist while server is in scoring phase.");

                // Backup current active playlist (for server memory update)
                if (mapInstance.ActivePlaylist != playlistID)
                {
                    mapInstance.Playlists[0] = mapInstance.Playlists[mapInstance.ActivePlaylist];
                }

                // Set as active
                mapInstance.ActivePlaylist = playlistID;

                // Save setting
                ServerSettings.Set("ActiveMapPlaylist", playlistID);

                // Update server memory if requested and server is running
                if (updateServerMemory && 
                    (theInstance.instanceStatus == InstanceStatus.ONLINE || 
                     theInstance.instanceStatus == InstanceStatus.STARTDELAY))
                {
                    ServerMemory.UpdateMapCycle1();
                    ServerMemory.UpdateMapCycle2();
                    ServerMemory.UpdateMapListCount();
                }

                AppDebug.Log("mapInstanceManager", $"Set playlist {playlistID} as active");

                return new PlaylistResult(true, $"Playlist {playlistID} is now active.", playlistID, maps.Count);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error setting active playlist: {ex.Message}");
                return new PlaylistResult(false, $"Error: {ex.Message}", playlistID, 0, ex);
            }
        }

        /// <summary>
        /// Randomize playlist order using Fisher-Yates shuffle
        /// </summary>
        public static PlaylistResult RandomizePlaylist(List<mapFileInfo> maps)
        {
            try
            {
                if (maps == null || maps.Count == 0)
                    return new PlaylistResult(false, "Cannot randomize an empty playlist.");

                var random = new Random();
                for (int i = maps.Count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    (maps[i], maps[j]) = (maps[j], maps[i]);
                }

                // Renumber MapIDs
                for (int i = 0; i < maps.Count; i++)
                {
                    maps[i].MapID = i + 1;
                }

                AppDebug.Log("mapInstanceManager", $"Randomized playlist with {maps.Count} maps");

                return new PlaylistResult(true, "Playlist randomized successfully.", 0, maps.Count);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error randomizing playlist: {ex.Message}");
                return new PlaylistResult(false, $"Error: {ex.Message}", 0, 0, ex);
            }
        }

        /// <summary>
        /// Validate playlist can be used (all maps still exist)
        /// </summary>
        public static (bool isValid, List<string> missingMaps) ValidatePlaylist(List<mapFileInfo> playlist)
        {
            var missingMaps = new List<string>();

            try
            {
                var availableMaps = LoadAvailableMaps();
                var availableMapLookup = new HashSet<(string MapFile, int MapType)>();

                foreach (var map in availableMaps.DefaultMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));
                foreach (var map in availableMaps.CustomMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));

                foreach (var map in playlist)
                {
                    if (!availableMapLookup.Contains((map.MapFile, map.MapType)))
                    {
                        missingMaps.Add($"{map.MapName} ({map.MapFile}) - Type: {objectGameTypes.GetShortName(map.MapType)}");
                    }
                }

                return (missingMaps.Count == 0, missingMaps);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error validating playlist: {ex.Message}");
                return (false, new List<string> { $"Validation error: {ex.Message}" });
            }
        }

        // ================================================================================
        // BACKUP & RESTORE
        // ================================================================================

        /// <summary>
        /// Export playlist to JSON file
        /// </summary>
        public static OperationResult ExportPlaylistToJson(int playlistID, string filePath)
        {
            try
            {
                var (success, maps, error) = GetPlaylistMaps(playlistID);
                if (!success)
                    return new OperationResult(false, error);

                if (maps.Count == 0)
                    return new OperationResult(false, "Cannot export an empty playlist.");

                string json = JsonSerializer.Serialize(maps, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                File.WriteAllText(filePath, json);

                AppDebug.Log("mapInstanceManager", $"Exported playlist {playlistID} to {filePath}");

                return new OperationResult(true, $"Playlist exported to {filePath}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error exporting playlist: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Import playlist from JSON file with validation
        /// </summary>
        public static (bool success, List<mapFileInfo> maps, int importedCount, int skippedCount, string errorMessage) ImportPlaylistFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return (false, new List<mapFileInfo>(), 0, 0, "File not found.");

                string json = File.ReadAllText(filePath);
                var importedMaps = JsonSerializer.Deserialize<List<mapFileInfo>>(json);

                if (importedMaps == null || importedMaps.Count == 0)
                    return (false, new List<mapFileInfo>(), 0, 0, "The backup file contains no maps.");

                // Build lookup of available maps
                var availableMaps = LoadAvailableMaps();
                var availableMapLookup = new HashSet<(string MapFile, int MapType)>();

                foreach (var map in availableMaps.DefaultMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));
                foreach (var map in availableMaps.CustomMaps)
                    availableMapLookup.Add((map.MapFile, map.MapType));

                // Filter to only available maps
                var validMaps = new List<mapFileInfo>();
                int skippedCount = 0;

                foreach (var map in importedMaps)
                {
                    if (availableMapLookup.Contains((map.MapFile, map.MapType)))
                    {
                        validMaps.Add(map);
                    }
                    else
                    {
                        skippedCount++;
                        AppDebug.Log("mapInstanceManager", $"Skipped unavailable map: {map.MapName} ({map.MapFile}) - Type: {map.MapType}");
                    }
                }

                // Renumber MapIDs
                for (int i = 0; i < validMaps.Count; i++)
                {
                    validMaps[i].MapID = i + 1;
                }

                AppDebug.Log("mapInstanceManager", $"Imported {validMaps.Count} maps, skipped {skippedCount}");

                return (true, validMaps, validMaps.Count, skippedCount, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error importing playlist: {ex.Message}");
                return (false, new List<mapFileInfo>(), 0, 0, ex.Message);
            }
        }

        // ================================================================================
        // SERVER COORDINATION
        // ================================================================================

        /// <summary>
        /// Update server map cycle (call after changing active playlist)
        /// </summary>
        public static OperationResult UpdateServerMapCycle()
        {
            try
            {
                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                    return new OperationResult(false, "Server is offline.");

                ServerMemory.UpdateMapCycle1();
                ServerMemory.UpdateMapCycle2();
                ServerMemory.UpdateMapListCount();

                AppDebug.Log("mapInstanceManager", "Updated server map cycle");

                return new OperationResult(true, "Server map cycle updated successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error updating server map cycle: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Set next map in rotation
        /// </summary>
        public static OperationResult SetNextMap(int mapIndex)
        {
            try
            {
                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                    return new OperationResult(false, "Server is offline.");

                var (success, maps, error) = GetPlaylistMaps(mapInstance.ActivePlaylist);
                if (!success)
                    return new OperationResult(false, error);

                if (mapIndex < 0 || mapIndex >= maps.Count)
                    return new OperationResult(false, $"Invalid map index. Must be between 0 and {maps.Count - 1}.");

                ServerMemory.UpdateNextMap(mapIndex);

                AppDebug.Log("mapInstanceManager", $"Set next map to index {mapIndex}: {maps[mapIndex].MapName}");

                return new OperationResult(true, $"Next map set to: {maps[mapIndex].MapName}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error setting next map: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Score current map (end match)
        /// </summary>
        public static OperationResult ScoreMap()
        {
            try
            {
                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                    return new OperationResult(false, "Server is offline.");

                ServerMemory.ScoreMap();

                AppDebug.Log("mapInstanceManager", "Scored current map");

                return new OperationResult(true, "Map scored successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error scoring map: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Skip current map
        /// </summary>
        public static OperationResult SkipMap()
        {
            try
            {
                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                    return new OperationResult(false, "Server is offline.");

                if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                    return new OperationResult(false, "Cannot skip while map is loading. Please try again in a moment.");

                ServerMemory.WriteMemorySendConsoleCommand("resetgames");

                AppDebug.Log("mapInstanceManager", "Skipped current map");

                return new OperationResult(true, "Map skipped successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error skipping map: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // INITIALIZATION
        // ================================================================================

        /// <summary>
        /// Initialize map playlists
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Initialize playlist dictionaries
                for (int i = 0; i <= 5; i++)
                {
                    mapInstance.Playlists[i] = new List<mapFileInfo>();
                }

                // Load active playlist setting
                mapInstance.ActivePlaylist = ServerSettings.Get("ActiveMapPlaylist", 1);
                mapInstance.SelectedPlaylist = mapInstance.ActivePlaylist;

                // Load all playlists
                for (int i = 1; i <= 5; i++)
                {
                    LoadPlaylist(i);
                }

                AppDebug.Log("mapInstanceManager", "Map playlists initialized");
            }
            catch (Exception ex)
            {
                AppDebug.Log("mapInstanceManager", $"Error initializing map playlists: {ex.Message}");
            }
        }
    }
}