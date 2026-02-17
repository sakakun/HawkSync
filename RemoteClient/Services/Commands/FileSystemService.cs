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


    // ================================================================================
    // FILE MANAGER OPERATIONS
    // ================================================================================

    /// <summary>
    /// Get list of files from server's profileServerPath
    /// </summary>
    public async Task<FileListResponse?> GetFilesAsync()
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<FileListResponse>("/api/filesystem/files");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting files: {ex.Message}");
            return new FileListResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Upload a file to the server
    /// </summary>
    public async Task<FileOperationResponse?> UploadFileAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return new FileOperationResponse
                {
                    Success = false,
                    Message = "File not found"
                };
            }

            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);
            using var streamContent = new StreamContent(fileStream);
            
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, "file", Path.GetFileName(filePath));

            var response = await _apiClient._httpClient.PostAsync("/api/filesystem/upload", content);

            if (!response.IsSuccessStatusCode)
            {
                return new FileOperationResponse
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}"
                };
            }

            return await response.Content.ReadFromJsonAsync<FileOperationResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            return new FileOperationResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Download a file from the server
    /// </summary>
    public async Task<bool> DownloadFileAsync(string fileName, string destinationPath)
    {
        try
        {
            var response = await _apiClient._httpClient.GetAsync($"/api/filesystem/download/{Uri.EscapeDataString(fileName)}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error downloading file: HTTP {response.StatusCode}");
                return false;
            }

            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Delete files from the server
    /// </summary>
    public async Task<FileOperationResponse?> DeleteFilesAsync(List<string> fileNames)
    {
        try
        {
            var request = new FileDeleteRequest { FileNames = fileNames };
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/filesystem/delete", request);

            if (!response.IsSuccessStatusCode)
            {
                return new FileOperationResponse
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}"
                };
            }

            return await response.Content.ReadFromJsonAsync<FileOperationResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting files: {ex.Message}");
            return new FileOperationResponse
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

}

