using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.SupportClasses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RemoteClient.Services.Commands;

public class GamePlayService
{
    private readonly ApiClient _apiClient;

    public GamePlayService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // GAMEPLAY SETTINGS COMMANDS
    // ================================================================================

    /// <summary>
    /// Get current gameplay settings from server
    /// </summary>
    public async Task<GamePlaySettingsResponse?> GetGamePlaySettingsAsync()
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<GamePlaySettingsResponse>("/api/gameplay/settings");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting gameplay settings: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Save gameplay settings to server
    /// </summary>
    public async Task<CommandResult> SaveGamePlaySettingsAsync(GamePlaySettingsRequest settings)
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/settings", settings);
    }

    /// <summary>
    /// Validate gameplay settings without saving
    /// </summary>
    public async Task<ValidationResult> ValidateGamePlaySettingsAsync(GamePlaySettingsRequest settings)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/gameplay/validate", settings);
        
            if (!response.IsSuccessStatusCode)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Errors = new List<string> { $"HTTP {response.StatusCode}" }
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ValidationResult>();
            return result ?? new ValidationResult { IsValid = false, Errors = new List<string> { "Empty response" } };
        }
        catch (Exception ex)
        {
            return new ValidationResult 
            { 
                IsValid = false, 
                Errors = new List<string> { ex.Message } 
            };
        }
    }

    /// <summary>
    /// Update running game server with current gameplay settings
    /// </summary>
    public async Task<CommandResult> UpdateGameServerAsync()
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/update-server", new { });
    }

    /// <summary>
    /// Lock the game lobby (prevent new players from joining)
    /// </summary>
    public async Task<CommandResult> LockLobbyAsync()
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/lock-lobby", new { });
    }

    /// <summary>
    /// Export (download) gameplay settings from server as JSON
    /// </summary>
    public async Task<GamePlaySettingsExportResponse?> ExportGamePlaySettingsAsync()
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<GamePlaySettingsExportResponse>("/api/gameplay/export");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting gameplay settings: {ex.Message}");
            return new GamePlaySettingsExportResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Import (upload) gameplay settings to server from JSON
    /// </summary>
    public async Task<CommandResult> ImportGamePlaySettingsAsync(string jsonData)
    {
        try
        {
            var request = new GamePlaySettingsImportRequest { JsonData = jsonData };
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/gameplay/import", request);

            if (!response.IsSuccessStatusCode)
            {
                return new CommandResult
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response" };
        }
        catch (Exception ex)
        {
            return new CommandResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
    /// <summary>
    /// Start the game server
    /// </summary>
    public async Task<CommandResult> StartServerAsync()
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/start-server", new { });
    }

    /// <summary>
    /// Stop the game server
    /// </summary>
    public async Task<CommandResult> StopServerAsync()
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/stop-server", new { });
    }


}
