using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabStats;
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
    /// Save all Babstats servers to the server.
    /// </summary>
    public async Task<CommandResult> SaveBabstatsServersAsync(BabstatsServerSettings server)
    {
        var request = new BabstatsServerRequest(server);
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/save", request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] SaveBabstatsServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] SaveBabstatsServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Add a Babstats server.
    /// </summary>
    public async Task<CommandResult> AddBabstatsServersAsync(BabstatsServerSettings server)
    {
        var request = new BabstatsServerRequest(server);
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/add", request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] AddBabstatsServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] AddBabstatsServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Remove a Babstats server.
    /// </summary>
    public async Task<CommandResult> RemoveBabstatsServersAsync(int serverID)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/remove", serverID);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] RemoveBabstatsServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] RemoveBabstatsServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Clear Babstats announcements.
    /// </summary>
    public async Task<CommandResult> ClearBabstatsAnnoucementsAsync()
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/clearAnnoucements", true);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] ClearBabstatsAnnoucements → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] ClearBabstatsAnnoucements error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Validate the web stats connection on the server.
    /// </summary>
    public async Task<CommandResult> ValidateWebStatsConnectionAsync(string serverPath)
    {
        var request = new { ServerPath = serverPath };
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/validate", request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] ValidateWebStatsConnection → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] ValidateWebStatsConnection error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Saves the specified lobby server settings asynchronously.
    /// </summary>
    public async Task<CommandResult> SaveLobbyServersAsync(LobbyServerSettings server)
    {
        var request = new LobbyServerRequest(server);
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/save", request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] SaveLobbyServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] SaveLobbyServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Asynchronously adds a new lobby server using the specified server settings.
    /// </summary>
    public async Task<CommandResult> AddLobbyServersAsync(LobbyServerSettings server)
    {
        var request = new LobbyServerRequest(server);
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/add", request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] AddLobbyServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] AddLobbyServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
        }
    }

    /// <summary>
    /// Removes a lobby server with the specified server identifier asynchronously.
    /// </summary>
    public async Task<CommandResult> RemoveLobbyServersAsync(int serverID)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/remove", serverID);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[StatService] RemoveLobbyServers → {(int)response.StatusCode}: {body}");
                return new CommandResult { Success = false, Message = GetFriendlyHttpError(response.StatusCode) };
            }
            var result = await response.Content.ReadFromJsonAsync<CommandResult>();
            return result ?? new CommandResult { Success = false, Message = "Empty response from server." };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[StatService] RemoveLobbyServers error: {ex}");
            return new CommandResult { Success = false, Message = "An unexpected error occurred. Please try again." };
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
