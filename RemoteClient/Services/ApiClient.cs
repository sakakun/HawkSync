using System.Net;
using HawkSyncShared.DTOs.API;
using RemoteClient.Services.Commands;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RemoteClient.Services;

/// <summary>
/// REST API client for server communication
/// </summary>
public class ApiClient : IDisposable
{
    internal readonly HttpClient _httpClient;

    // public AuthenticationService Authentication { get; }
    public PlayerService Player { get; set; }
    public ChatService Chat { get; set; }
    public ProfileService Profile { get; set; }
    public GamePlayService GamePlay { get; set; }
    public FileSystemService FileSystem { get; }
    public MapServices Maps { get; set; }
    public AdminService Admin { get; set; }
    public BanService Ban { get; set; }
    public StatService Stats { get; set; }

    private string? _jwtToken;
   
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
        _httpClient = new HttpClient { BaseAddress = new Uri(serverUrl), Timeout = TimeSpan.FromSeconds(10) };

        // Authentication = new AuthenticationService(_httpClient);
        Player = new PlayerService(this);
        Chat = new ChatService(this);
        Profile = new ProfileService(this);
        GamePlay = new GamePlayService(this);
        FileSystem = new FileSystemService(this);
        Maps = new MapServices(this);
        Admin = new AdminService(this);
        Stats = new StatService(this);
        Ban = new BanService(this);

    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
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

            // Handle rate limit explicitly
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                int retryAfterSeconds = ParseRetryAfterSeconds(response) ?? 60;

                return new LoginResponse
                {
                    Success = false,
                    Message = $"Too many login attempts. Please wait {retryAfterSeconds} second(s) and try again.",
                    RetryAfterSeconds = retryAfterSeconds
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                // Avoid dumping raw server payload to users
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Login failed (HTTP {(int)response.StatusCode})."
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
        catch (TaskCanceledException)
        {
            return new LoginResponse { Success = false, Message = "Request timed out. Please try again." };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    private static int? ParseRetryAfterSeconds(HttpResponseMessage response)
    {
        var retryAfter = response.Headers.RetryAfter;
        if (retryAfter == null) return null;

        if (retryAfter.Delta.HasValue)
        {
            return Math.Max(1, (int)Math.Ceiling(retryAfter.Delta.Value.TotalSeconds));
        }

        if (retryAfter.Date.HasValue)
        {
            var seconds = (int)Math.Ceiling((retryAfter.Date.Value - DateTimeOffset.UtcNow).TotalSeconds);
            return Math.Max(1, seconds);
        }

        return null;
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
    // HELPER METHODS
    // ================================================================================

    internal async Task<CommandResult> SendCommandAsync<T>(string endpoint, T command)
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

}