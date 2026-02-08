using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.SupportClasses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RemoteClient.Services.Commands;

public class MapServices
{
    private readonly ApiClient _apiClient;

    public MapServices(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // MAPS/PLAYLISTS
    // ================================================================================

    public async Task<List<MapDTO>?> GetAvailableMapsAsync()
    {
        return await _apiClient._httpClient.GetFromJsonAsync<List<MapDTO>>("/api/maps/list/available");
    }

    public async Task<AllPlaylistsDTO?> GetAllPlaylistsAsync()
    {
        return await _apiClient._httpClient.GetFromJsonAsync<AllPlaylistsDTO>("/api/maps/list/playlists");
    }

    public async Task<PlaylistDTO?> GetPlaylistAsync(int playlistId)
    {
        return await _apiClient._httpClient.GetFromJsonAsync<PlaylistDTO>($"/api/maps/list/playlist/{playlistId}");
    }

    public async Task<PlaylistCommandResult> SavePlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _apiClient._httpClient.PostAsJsonAsync("/api/maps/playlist/save", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> SetActivePlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _apiClient._httpClient.PostAsJsonAsync("/api/maps/playlist/set-active", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> ImportPlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _apiClient._httpClient.PostAsJsonAsync("/api/maps/playlist/import", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistDTO?> ExportPlaylistAsync(int playlistId)
    {
        return await _apiClient._httpClient.GetFromJsonAsync<PlaylistDTO>($"/api/maps/playlist/export/{playlistId}");
    }

    public async Task<PlaylistCommandResult> SkipMapAsync()
    {
        var response = await _apiClient._httpClient.PostAsync("/api/maps/server/skip-map", null);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> ScoreMapAsync()
    {
        var response = await _apiClient._httpClient.PostAsync("/api/maps/server/score-map", null);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> PlayNextMapAsync(int mapIndex)
    {
        var response = await _apiClient._httpClient.PostAsJsonAsync("/api/maps/server/play-next", mapIndex);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }


}
