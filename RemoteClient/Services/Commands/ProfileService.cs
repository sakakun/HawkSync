using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabProfile;
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
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[ProfileService] ValidateProfileSettings → {(int)response.StatusCode}: {body}");
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
            System.Diagnostics.Debug.WriteLine($"[ProfileService] ValidateProfileSettings error: {ex}");
            return new ValidationResult
            {
                IsValid = false,
                Errors = new List<string> { "An unexpected error occurred. Please try again." }
            };
        }
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

