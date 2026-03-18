using HawkSyncShared.Instances;
using System.Collections.Generic;
using HawkSyncShared.DTOs.tabMaps;

namespace HawkSyncShared.Instances;

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
    public List<MapObject> DefaultMaps { get; set; } = new();
    
    /// <summary>
    /// All available custom maps found in the server directory
    /// </summary>
    public List<MapObject> CustomMaps { get; set; } = new();
    
    // ================================================================================
    // PLAYLISTS (5 Playlists)
    // ================================================================================
    
    /// <summary>
    /// Map playlists storage
    /// Key: Playlist number (1-5)
    /// Value: List of maps in that playlist
    /// </summary>
    public Dictionary<int, List<MapObject>> Playlists { get; set; } = new()
    {
        { 1, new List<MapObject>() },
        { 2, new List<MapObject>() },
        { 3, new List<MapObject>() },
        { 4, new List<MapObject>() },
        { 5, new List<MapObject>() }
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
    public int ActualPlayingMapIndex { get; set; } = 0;  
    // The map that's actually playing right now (for UI)

    /// <summary>
    /// Current index in the active playlist (0-based)
    /// This is used for determining the next map to be played based on the server's map cycle counter.
    /// </summary>
    public int CurrentMapIndex { get; set; } = 0;
    
    /// <summary>
    /// Is the current map a 4 Team Map?
    /// </summary>
    public bool IsCurrentMap4Team { get; set; } = false;

    // ================================================================================
    // NEXT MAP INFO
    // ================================================================================
    
    /// <summary>
    /// Is the next map a 4 Team Map?
    /// </summary>
    public bool IsNextMap4Team { get; set; } = false;

    /// <summary>
    /// Next map to be played
    /// </summary>
    public string NextMapName { get; set; } = string.Empty;
    
    /// <summary>
    /// Next map to be played
    /// </summary>
    public string NextMapFile { get; set; } = string.Empty;

    /// <summary>
    /// Next map's game type
    /// </summary>
    public int NextMapGameType { get; set; } = 0;
    
}