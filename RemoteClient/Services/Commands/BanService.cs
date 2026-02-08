using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.tabBans;
using HawkSyncShared.DTOs.tabBans.Service;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace RemoteClient.Services.Commands;
public class BanService
{
    private readonly ApiClient _apiClient;

    public BanService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    /////////
    /// Blacklist and Whitelist Management
    /////////

    public async Task<BanRecordSaveResult> SaveBlacklistRecordAsync(BanDTOs req)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/ban/save-blacklist", req);

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
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/ban/delete-blacklist", request);

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

    public async Task<BanRecordSaveResult> SaveWhitelistRecordAsync(BanDTOs req)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/ban/save-whitelist", req);

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
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/ban/delete-whitelist", request);

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
       
    /////////
    /// Proxy Checking Management
    /////////
        
    public async Task<CommandResult> SaveProxyCheckSettingsAsync(ProxyCheck settings)
    {
        // Change "/api/settings/proxycheck" to "/api/profile/proxycheck"
        return await _apiClient.SendCommandAsync("/api/ban/proxycheck/save", settings);
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
        return await _apiClient.SendCommandAsync("/api/ban/proxycheck/country/add", request);
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
        return await _apiClient.SendCommandAsync("/api/ban/proxycheck/country/remove", request);
    }

    public async Task<ProxyCheckTestResult?> ValidateProxyCheckServiceAsync(string apiKey, int serviceProvider, string ipAddress)
    {
        var request = new ProxyCheckTestRequest
        {
            ApiKey = apiKey,
            ServiceProvider = serviceProvider,
            IPAddress = ipAddress
        };

        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/ban/proxycheck/validate", request);
            if (!response.IsSuccessStatusCode)
                return new ProxyCheckTestResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" };

            return await response.Content.ReadFromJsonAsync<ProxyCheckTestResult>();
        }
        catch (Exception ex)
        {
            return new ProxyCheckTestResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    /////////
    /// NetLimiter Management
    /////////

    public async Task<CommandResult> SaveNetLimiterSettingsAsync(NetLimiterSettingsRequest settings)
    {
        // Adjust the endpoint to match your server route
        return await _apiClient.SendCommandAsync("/api/ban/netlimiter/save", settings);
    }

    /// <summary>
    /// Get available NetLimiter filters from the server.
    /// </summary>
    public async Task<(bool Success, List<string> Filters, string? ErrorMessage)> GetNetLimiterFiltersAsync()
    {
        try
        {
            var response = await _apiClient._httpClient.GetAsync("/api/ban/netlimiter/filters");
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

}

