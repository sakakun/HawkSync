<<<<<<< HEAD:HawkSyncShared/Instances/mapInstance.cs
﻿using HawkSyncShared.ObjectClasses;
using HawkSyncShared.Instances;
using System.Collections.Generic;

namespace HawkSyncShared.Instances;
=======
﻿using BHD_ServerManager.Classes.ObjectClasses;
using System.Collections.Generic;

namespace BHD_ServerManager.Classes.Instances;
>>>>>>> ed4f84426d6449f979e54b6025b7ef785c7a674e:BHD-ServerManager/Classes/Instances/mapInstance.cs

/// <summary>
/// Contains all map-related data for map management and playlists
/// </summary>
public class mapInstance
{
    // ================================================================================
    // AVAILABLE MAPS (Source List)
    // ================================================================================
    
    /// <summary>
    /// All available default maps shipped with the game
    /// </summary>
    public List<mapFileInfo> DefaultMaps { get; set; } = new();
    
    /// <summary>
    /// All available custom maps found in the server directory
    /// </summary>
    public List<mapFileInfo> CustomMaps { get; set; } = new();
    
    /// <summary>
    /// Combined list of all available maps (for quick access)
    /// </summary>
    public List<mapFileInfo> AllAvailableMaps => 
        DefaultMaps.Concat(CustomMaps).OrderBy(m => m.MapName).ToList();
    
    // ================================================================================
    // PLAYLISTS (5 Playlists)
    // ================================================================================
    
    /// <summary>
    /// Map playlists storage
    /// Key: Playlist number (1-5)
    /// Value: List of maps in that playlist
    /// </summary>
    public Dictionary<int, List<mapFileInfo>> Playlists { get; set; } = new()
    {
        { 1, new List<mapFileInfo>() },
        { 2, new List<mapFileInfo>() },
        { 3, new List<mapFileInfo>() },
        { 4, new List<mapFileInfo>() },
        { 5, new List<mapFileInfo>() }
    };
    
    // ================================================================================
    // PLAYLIST STATE
    // ================================================================================
    
    /// <summary>
    /// Currently active playlist (the one running on the server)
    /// </summary>
    public int ActivePlaylist { get; set; } = 1;
    
    /// <summary>
    /// Currently selected playlist in the UI (for editing)
    /// </summary>
    public int SelectedPlaylist { get; set; } = 1;
    
    /// <summary>
    /// Flag indicating if the active playlist was changed
    /// </summary>
    public bool ActivePlaylistChanged { get; set; } = false;
    
    // ================================================================================
    // CURRENT MAP STATE
    // ================================================================================
    
    /// <summary>
    /// Current map name being played
    /// </summary>
    public string CurrentMapName { get; set; } = string.Empty;
    
    /// <summary>
    /// Current map file being played
    /// </summary>
    public string CurrentMapFile { get; set; } = string.Empty;
    
    /// <summary>
    /// Current game type (0=DM, 1=TDM, etc.)
    /// </summary>
    public int CurrentGameType { get; set; } = 0;

    /// <summary>
    /// Current index in the active playlist (0-based)
    /// </summary>
    public int ActualPlayingMapIndex { get; set; } = 0;  // The map that's actually playing right now (for UI)

    /// <summary>
    /// Current index in the active playlist (0-based)
    /// </summary>
    public int CurrentMapIndex { get; set; } = 0;
    
    /// <summary>
    /// Total maps in the active playlist
    /// </summary>
    public int TotalMapsInPlaylist => Playlists.ContainsKey(ActivePlaylist) 
        ? Playlists[ActivePlaylist].Count 
        : 0;
    
    // ================================================================================
    // NEXT MAP INFO
    // ================================================================================
    
    /// <summary>
    /// Next map to be played
    /// </summary>
    public string NextMapName { get; set; } = string.Empty;
    
    /// <summary>
    /// Next map's game type
    /// </summary>
    public int NextMapGameType { get; set; } = 0;
    
    /// <summary>
    /// Get the next map object from the active playlist
    /// </summary>
    public mapFileInfo? GetNextMap()
    {
        if (!Playlists.ContainsKey(ActivePlaylist)) 
            return null;
        
        var playlist = Playlists[ActivePlaylist];
        if (playlist.Count == 0) 
            return null;
        
        int nextIndex = (CurrentMapIndex + 1) % playlist.Count;
        return playlist[nextIndex];
    }
    
    /// <summary>
    /// Get the current map object from the active playlist
    /// </summary>
    public mapFileInfo? GetCurrentMap()
    {
        if (!Playlists.ContainsKey(ActivePlaylist)) 
            return null;
        
        var playlist = Playlists[ActivePlaylist];
        if (playlist.Count == 0 || CurrentMapIndex >= playlist.Count) 
            return null;
        
        return playlist[CurrentMapIndex];
    }
}