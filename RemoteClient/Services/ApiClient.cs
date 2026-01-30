using System.Net.Http.Headers;
using System.Net.Http.Json;
using HawkSyncShared.DTOs;

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

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}