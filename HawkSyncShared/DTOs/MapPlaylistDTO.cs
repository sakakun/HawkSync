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