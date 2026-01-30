using System.Collections.Generic;

namespace HawkSyncShared.DTOs;

/// <summary>
/// DTO for a single map in a playlist
/// </summary>
public class MapDTO
{
    public int MapID { get; set; }
    public string MapName { get; set; } = string.Empty;
    public string MapFile { get; set; } = string.Empty;
    public int ModType { get; set; }
    public int MapType { get; set; }

    // Add this constructor:
    public MapDTO(int mapID, string mapName, string mapFile, int mapType)
    {
        MapID = mapID;
        MapName = mapName;
        MapFile = mapFile;
        MapType = mapType;
    }

    // Optionally, add a full constructor if you want to set ModType too:
    public MapDTO(int mapID, string mapName, string mapFile, int modType, int mapType)
    {
        MapID = mapID;
        MapName = mapName;
        MapFile = mapFile;
        ModType = modType;
        MapType = mapType;
    }

    // Parameterless constructor for serialization
    public MapDTO() { }
}

/// <summary>
/// DTO for a playlist (list of maps)
/// </summary>
public class PlaylistDTO
{
    public int PlaylistID { get; set; }
    public List<MapDTO> Maps { get; set; } = new();
}

/// <summary>
/// DTO for all playlists
/// </summary>
public class AllPlaylistsDTO
{
    public Dictionary<int, List<MapDTO>> Playlists { get; set; } = new();
    public int ActivePlaylist { get; set; }
    public int SelectedPlaylist { get; set; }
}

/// <summary>
/// Command result for playlist/map actions
/// </summary>
public class PlaylistCommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public PlaylistDTO? Playlist { get; set; }
    public AllPlaylistsDTO? AllPlaylists { get; set; }
}