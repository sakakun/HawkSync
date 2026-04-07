using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.DTOs.tabProfile;
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
            System.Diagnostics.Debug.WriteLine($"[GamePlayService] GetGamePlaySettings error: {ex}");
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
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[GamePlayService] ValidateGamePlaySettings → {(int)response.StatusCode}: {body}");
                return new ValidationResult
                {
                    IsValid = false,
                    Errors = new List<string> { GetFriendlyHttpError(response.StatusCode) }
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ValidationResult>();
            return result ?? new ValidationResult { IsValid = false, Errors = new List<string> { "Empty response from server." } };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[GamePlayService] ValidateGamePlaySettings error: {ex}");
            return new ValidationResult
            {
                IsValid = false,
                Errors = new List<string> { "An unexpected error occurred. Please try again." }
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
            System.Diagnostics.Debug.WriteLine($"[GamePlayService] ExportGamePlaySettings error: {ex}");
            return new GamePlaySettingsExportResponse
            {
                Success = false,
                Message = "An unexpected error occurred. Please try again."
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
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[GamePlayService] ImportGamePlaySettings → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }

            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[GamePlayService] ImportGamePlaySettings error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
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

    /// <summary>
    /// Toggle match state (pause/resume match)
    /// </summary>
    public async Task<CommandResult> ToggleMatchStateAsync()
    {
        return await _apiClient.SendCommandAsync("/api/gameplay/toggle-match-state", new { });
    }

    private static string GetFriendlyHttpError(System.Net.HttpStatusCode code) => code switch
    {
        System.Net.HttpStatusCode.Unauthorized    => "Authentication required. Please log in again.",
        System.Net.HttpStatusCode.Forbidden       => "You do not have permission to perform this action.",
        System.Net.HttpStatusCode.NotFound        => "The requested resource was not found.",
        System.Net.HttpStatusCode.BadRequest      => "The request was invalid. Please check your input.",
        System.Net.HttpStatusCode.TooManyRequests => "Too many requests. Please wait and try again.",
        _ when (int)code >= 500                   => "A server error occurred. Please try again later.",
        _                                         => $"Request failed ({(int)code})."
    };

}
