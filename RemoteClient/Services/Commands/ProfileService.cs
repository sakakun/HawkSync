using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.SupportClasses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RemoteClient.Services.Commands;

public class ProfileService
{
    private readonly ApiClient _apiClient;

    public ProfileService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // PROFILE SETTINGS COMMANDS
    // ================================================================================

    /// <summary>
    /// Get current profile settings from server
    /// </summary>
    public async Task<ProfileSettingsResponse?> GetProfileSettingsAsync()
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<ProfileSettingsResponse>("/api/profile/settings");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting profile settings: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Save profile settings to server
    /// </summary>
    public async Task<CommandResult> SaveProfileSettingsAsync(ProfileSettingsRequest settings)
    {
        return await _apiClient.SendCommandAsync("/api/profile/settings", settings);
    }

    /// <summary>
    /// Validate profile settings without saving
    /// </summary>
    public async Task<ValidationResult> ValidateProfileSettingsAsync(ProfileSettingsRequest settings)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/profile/validate", settings);
        
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


}

