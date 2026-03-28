using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.SupportClasses;
using System.Net.Http.Headers;
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

    /// <summary>
    /// Save all Babstats servers to the server.
    /// </summary>
    public async Task<CommandResult> AddBabstatsServersAsync(BabstatsServerSettings server)
    {
        var request = new BabstatsServerRequest(server);

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/add", request);
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

    /// <summary>
    /// Save all Babstats servers to the server.
    /// </summary>
    public async Task<CommandResult> RemoveBabstatsServersAsync(int serverID)
    {

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/remove", serverID);
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

    /// <summary>
    /// Save all Babstats servers to the server.
    /// </summary>
    public async Task<CommandResult> ClearBabstatsAnnoucementsAsync()
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/babstats/servers/clearAnnoucements", true);
            
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

    /// <summary>
    /// Saves the specified lobby server settings asynchronously to the backend service.
    /// </summary>
    /// <remarks>If the backend service returns an error or an exception occurs during the request, the
    /// returned CommandResult will indicate failure and include an error message.</remarks>
    /// <param name="server">The lobby server settings to be saved. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a CommandResult indicating whether
    /// the save operation was successful and any related message.</returns>
    public async Task<CommandResult> SaveLobbyServersAsync(LobbyServerSettings server)
    {
        var request = new LobbyServerRequest(server);

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/save", request);
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

    /// <summary>
    /// Asynchronously adds a new lobby server using the specified server settings.
    /// </summary>
    /// <param name="server">The settings for the lobby server to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a CommandResult indicating whether
    /// the server was added successfully and any related message.</returns>
    public async Task<CommandResult> AddLobbyServersAsync(LobbyServerSettings server)
    {
        var request = new LobbyServerRequest(server);

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/add", request);
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

    /// <summary>
    /// Removes a lobby server with the specified server identifier asynchronously.
    /// </summary>
    /// <param name="serverID">The unique identifier of the lobby server to remove.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a CommandResult indicating whether
    /// the removal was successful and includes any relevant messages.</returns>
    public async Task<CommandResult> RemoveLobbyServersAsync(int serverID)
    {

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/stats/lobby/servers/remove", serverID);
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
