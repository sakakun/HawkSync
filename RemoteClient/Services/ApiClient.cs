using HawkSyncShared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Channels;
using System.Windows.Forms;

namespace RemoteClient.Services;

/// <summary>
/// REST API client for server communication
/// </summary>
public class ApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private string? _jwtToken;

    public string ServerUrl { get; }
    
    public string? JwtToken
    {
        get => _jwtToken;
        set
        {
            _jwtToken = value;
            if (!string.IsNullOrEmpty(value))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", value);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }

    public ApiClient(string serverUrl)
    {
        ServerUrl = serverUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(serverUrl),
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    // ================================================================================
    // AUTHENTICATION
    // ================================================================================

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        try
        {
            var request = new LoginRequest(username, password);
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new LoginResponse
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}: {error}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                JwtToken = result.Token;
            }

            return result ?? new LoginResponse { Success = false, Message = "Empty response" };
        }
        catch (HttpRequestException ex)
        {
            return new LoginResponse { Success = false, Message = $"Connection failed: {ex.Message}" };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    // ================================================================================
    // SERVER STATE
    // ================================================================================

    public async Task<ServerSnapshot?> GetSnapshotAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ServerSnapshot>("/api/snapshot");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting snapshot: {ex.Message}");
            return null;
        }
    }

    // ================================================================================
    // PLAYER COMMANDS
    // ================================================================================

    public async Task<CommandResult> KickPlayerAsync(int playerSlot, string playerName)
    {
        var command = new KickPlayerCommand(playerSlot, playerName);
        return await SendCommandAsync("/api/player/kick", command);
    }

    public async Task<CommandResult> BanPlayerAsync(int playerSlot, string playerName, string playerIP, bool banIP)
    {
        var command = new BanPlayerCommand(playerSlot, playerName, playerIP, banIP);
        return await SendCommandAsync("/api/player/ban", command);
    }

    public async Task<CommandResult> WarnPlayerAsync(int playerSlot, string playerName, string message)
    {
        var command = new WarnPlayerCommand(playerSlot, playerName, message);
        return await SendCommandAsync("/api/player/warn", command);
    }

    public async Task<CommandResult> KillPlayerAsync(int playerSlot, string playerName)
    {
        var command = new KillPlayerCommand(playerSlot, playerName);
        return await SendCommandAsync("/api/player/kill", command);
    }

    // ================================================================================
    // CHAT COMMANDS
    // ================================================================================

    public async Task<CommandResult> SendChatAsync(string message, int channel = 1)
    {
        var command = new SendChatCommand(message, channel);
        return await SendCommandAsync("/api/chat/send", command);
    }

    // Example generic command sender for auto/slap messages
    public async Task<CommandResult> SendAutoMessageAsync(string message, int interval)
    {
        var command = new { Message = message, Interval = interval };
        return await SendCommandAsync("/api/chat/auto/add", command);
    }

    public async Task<CommandResult> RemoveAutoMessageAsync(string id)
    {
        var command = new { Id = id };
        return await SendCommandAsync("/api/chat/auto/remove", command);
    }

    public async Task<CommandResult> SendSlapMessageAsync(string message)
    {
        var command = new { Message = message };
        return await SendCommandAsync("/api/chat/slap/add", command);
    }

    public async Task<CommandResult> RemoveSlapMessageAsync(string id)
    {
        var command = new { Id = id };
        return await SendCommandAsync("/api/chat/slap/remove", command);
    }

    // ================================================================================
    // HELPER METHODS
    // ================================================================================

    private async Task<CommandResult> SendCommandAsync<T>(string endpoint, T command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, command);

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

    // Add this section after CHAT COMMANDS and before HELPER METHODS

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
            return await _httpClient.GetFromJsonAsync<ProfileSettingsResponse>("/api/profile/settings");
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
        return await SendCommandAsync("/api/profile/settings", settings);
    }

    /// <summary>
    /// Validate profile settings without saving
    /// </summary>
    public async Task<ValidationResult> ValidateProfileSettingsAsync(ProfileSettingsRequest settings)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/profile/validate", settings);
        
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
            return await _httpClient.GetFromJsonAsync<GamePlaySettingsResponse>("/api/gameplay/settings");
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
        return await SendCommandAsync("/api/gameplay/settings", settings);
    }

    /// <summary>
    /// Validate gameplay settings without saving
    /// </summary>
    public async Task<ValidationResult> ValidateGamePlaySettingsAsync(GamePlaySettingsRequest settings)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/gameplay/validate", settings);
        
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
        return await SendCommandAsync("/api/gameplay/update-server", new { });
    }

    /// <summary>
    /// Lock the game lobby (prevent new players from joining)
    /// </summary>
    public async Task<CommandResult> LockLobbyAsync()
    {
        return await SendCommandAsync("/api/gameplay/lock-lobby", new { });
    }

    /// <summary>
    /// Export (download) gameplay settings from server as JSON
    /// </summary>
    public async Task<GamePlaySettingsExportResponse?> ExportGamePlaySettingsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<GamePlaySettingsExportResponse>("/api/gameplay/export");
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
            var response = await _httpClient.PostAsJsonAsync("/api/gameplay/import", request);

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
        return await SendCommandAsync("/api/gameplay/start-server", new { });
    }

    /// <summary>
    /// Stop the game server
    /// </summary>
    public async Task<CommandResult> StopServerAsync()
    {
        return await SendCommandAsync("/api/gameplay/stop-server", new { });
    }
    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
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
            return await _httpClient.GetFromJsonAsync<DirectoryListingResponse>("/api/filesystem/drives");
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

            var response = await _httpClient.PostAsJsonAsync("/api/filesystem/list", request);

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
            var response = await _httpClient.PostAsJsonAsync("/api/filesystem/validate-path", request);

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
    // MAPS/PLAYLISTS
    // ================================================================================

    public async Task<List<MapDTO>?> GetAvailableMapsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<MapDTO>>("/api/mapplaylist/available-maps");
    }

    public async Task<AllPlaylistsDTO?> GetAllPlaylistsAsync()
    {
        return await _httpClient.GetFromJsonAsync<AllPlaylistsDTO>("/api/mapplaylist/playlists");
    }

    public async Task<PlaylistDTO?> GetPlaylistAsync(int playlistId)
    {
        return await _httpClient.GetFromJsonAsync<PlaylistDTO>($"/api/mapplaylist/playlist/{playlistId}");
    }

    public async Task<PlaylistCommandResult> SavePlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/mapplaylist/playlist/save", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> SetActivePlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/mapplaylist/playlist/set-active", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> ImportPlaylistAsync(PlaylistDTO playlist)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/mapplaylist/playlist/import", playlist);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistDTO?> ExportPlaylistAsync(int playlistId)
    {
        return await _httpClient.GetFromJsonAsync<PlaylistDTO>($"/api/mapplaylist/playlist/export/{playlistId}");
    }

    public async Task<PlaylistCommandResult> SkipMapAsync()
    {
        var response = await _httpClient.PostAsync("/api/mapplaylist/server/skip-map", null);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> ScoreMapAsync()
    {
        var response = await _httpClient.PostAsync("/api/mapplaylist/server/score-map", null);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<PlaylistCommandResult> PlayNextMapAsync(int mapIndex)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/mapplaylist/server/play-next", mapIndex);
        return await response.Content.ReadFromJsonAsync<PlaylistCommandResult>() ?? new PlaylistCommandResult { Success = false, Message = "No response" };
    }

    public async Task<BanRecordSaveResult> SaveBlacklistRecordAsync(BanRecordSaveRequest req)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/ban/save-blacklist", req);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BanRecordSaveResult
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}: {error}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<BanRecordSaveResult>();
            return result ?? new BanRecordSaveResult { Success = false, Message = "Empty response" };
        }
        catch (Exception ex)
        {
            return new BanRecordSaveResult { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<CommandResult> DeleteBlacklistRecordAsync(int recordId, bool isName)
    {
        try
        {
            var request = new
            {
                RecordID = recordId,
                IsName = isName
            };
            var response = await _httpClient.PostAsJsonAsync("/api/ban/delete-blacklist", request);

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

    public async Task<BanRecordSaveResult> SaveWhitelistRecordAsync(BanRecordSaveRequest req)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/ban/save-whitelist", req);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BanRecordSaveResult
                {
                    Success = false,
                    Message = $"HTTP {response.StatusCode}: {error}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<BanRecordSaveResult>();
            return result ?? new BanRecordSaveResult { Success = false, Message = "Empty response" };
        }
        catch (Exception ex)
        {
            return new BanRecordSaveResult { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<CommandResult> DeleteWhitelistRecordAsync(int recordId, bool isName)
    {
        try
        {
            var request = new
            {
                RecordID = recordId,
                IsName = isName
            };
            var response = await _httpClient.PostAsJsonAsync("/api/ban/delete-whitelist", request);

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

    public async Task<CommandResult> SaveProxyCheckSettingsAsync(ProxyCheckSettingsRequest settings)
    {
        // Change "/api/settings/proxycheck" to "/api/profile/proxycheck"
        return await SendCommandAsync("/api/profile/proxycheck", settings);
    }

    public async Task<ProxyCheckTestResult?> TestProxyCheckServiceAsync(string apiKey, int serviceProvider, string ipAddress)
    {
        var request = new ProxyCheckTestRequest
        {
            ApiKey = apiKey,
            ServiceProvider = serviceProvider,
            IPAddress = ipAddress
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/profile/proxycheck/test", request);
            if (!response.IsSuccessStatusCode)
                return new ProxyCheckTestResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" };

            return await response.Content.ReadFromJsonAsync<ProxyCheckTestResult>();
        }
        catch (Exception ex)
        {
            return new ProxyCheckTestResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<CommandResult> SaveNetLimiterSettingsAsync(NetLimiterSettingsRequest settings)
    {
        // Adjust the endpoint to match your server route
        return await SendCommandAsync("/api/profile/netlimiter", settings);
    }

    /// <summary>
    /// Get available NetLimiter filters from the server.
    /// </summary>
    public async Task<(bool Success, List<string> Filters, string? ErrorMessage)> GetNetLimiterFiltersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/profile/netlimiter/filters");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, new List<string>(), $"HTTP {response.StatusCode}: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<NetLimiterFiltersResponse>();
            if (result == null || !result.Success)
                return (false, new List<string>(), result?.Message ?? "Unknown error");

            return (true, result.Filters ?? new List<string>(), null);
        }
        catch (Exception ex)
        {
            return (false, new List<string>(), ex.Message);
        }
    }

    /// <summary>
    /// DTO for NetLimiter filters response.
    /// </summary>
    public class NetLimiterFiltersResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Filters { get; set; }
    }
    /// <summary>
    /// Add a blocked country to the server.
    /// </summary>
    public async Task<CommandResult> AddBlockedCountryAsync(string countryCode, string countryName)
    {
        var request = new
        {
            CountryCode = countryCode,
            CountryName = countryName
        };
        return await SendCommandAsync("/api/profile/proxycheck/add-country", request);
    }

    /// <summary>
    /// Remove a blocked country from the server.
    /// </summary>
    public async Task<CommandResult> RemoveBlockedCountryAsync(int recordId)
    {
        var request = new
        {
            RecordID = recordId
        };
        return await SendCommandAsync("/api/profile/proxycheck/remove-country", request);
    }

    /// <summary>
    /// Save web stats settings to the server.
    /// </summary>
    public async Task<CommandResult> SaveWebStatsSettingsAsync(WebStatsSettings settings)
    {
        return await SendCommandAsync("/api/profile/webstats", settings);
    }

    /// <summary>
    /// Validate the web stats connection on the server.
    /// </summary>
    public async Task<CommandResult> ValidateWebStatsConnectionAsync(string serverPath)
    {
        var request = new { ServerPath = serverPath };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/profile/webstats/validate", request);
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