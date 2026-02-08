using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.tabBans;
using HawkSyncShared.DTOs.tabBans.Service;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.SupportClasses;
using RemoteClient.Services.Commands;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Windows.Forms;
using Windows.Gaming.Input;
using Windows.Services.Maps;
namespace RemoteClient.Services.Commands;

public class FileSystemService
{
    private readonly ApiClient _apiClient;

    public FileSystemService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // FILE SYSTEM BROWSING
    // ================================================================================

    /// <summary>
    /// Get list of drives on server
    /// </summary>
    public async Task<DirectoryListingResponse?> GetServerDrivesAsync()
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<DirectoryListingResponse>("/api/filesystem/drives");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting drives: {ex.Message}");
            return new DirectoryListingResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Get directory listing from server
    /// </summary>
    public async Task<DirectoryListingResponse?> GetDirectoryListingAsync(string? path, string? fileFilter = null)
    {
        try
        {
            var request = new DirectoryListingRequest
            {
                Path = path,
                FileFilter = fileFilter
            };

            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/filesystem/list", request);

            if (!response.IsSuccessStatusCode)
            {
                return new DirectoryListingResponse
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}"
                };
            }

            return await response.Content.ReadFromJsonAsync<DirectoryListingResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting directory listing: {ex.Message}");
            return new DirectoryListingResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Validate if a path exists on server
    /// </summary>
    public async Task<CommandResult> ValidateServerPathAsync(string path)
    {
        try
        {
            var request = new DirectoryListingRequest { Path = path };
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/filesystem/validate-path", request);

            if (!response.IsSuccessStatusCode)
            {
                return new CommandResult
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}"
                };
            }

            return await response.Content.ReadFromJsonAsync<CommandResult>() 
                ?? new CommandResult { Success = false, Message = "Empty response" };
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


}
