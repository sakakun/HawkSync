using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.SupportClasses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RemoteClient.Services.Commands;

public class StatService
{
    private readonly ApiClient _apiClient;

    public StatService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    /// <summary>
    /// Save web stats settings to the server.
    /// </summary>
    public async Task<CommandResult> SaveWebStatsSettingsAsync(WebStatsSettings settings)
    {
        return await _apiClient.SendCommandAsync("/api/stats/save", settings);
    }

    /// <summary>
    /// Validate the web stats connection on the server.
    /// </summary>
    public async Task<CommandResult> ValidateWebStatsConnectionAsync(string serverPath)
    {
        var request = new { ServerPath = serverPath };
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/validate", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new CommandResult
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}: {error}"
                };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response" };
        }
        catch (Exception ex)
        {
            return new CommandResult { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

}
