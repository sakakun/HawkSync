using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;
using System.Text.Json;

namespace BHD_ServerManager.Classes.Services.ProxyDetection
{
    public class ProxyCheckService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private const string API_BASE_URL = "https://proxycheck.io/v3";

        public string ApiKey { get; set; } = string.Empty;
        public int CacheDurationHours { get; set; } = 24;
        
        private readonly Dictionary<string, proxyRecord> _cache = new();

        /// <summary>
        /// Check IP address for VPN/proxy/threat information with caching
        /// </summary>
        public async Task<proxyRecord?> CheckIPAsync(string ipAddress)
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                AppDebug.Log("ProxyCheckService", "API key not configured");
                return null;
            }

            if (!IPAddress.TryParse(ipAddress, out var ip))
            {
                AppDebug.Log("ProxyCheckService", $"Invalid IP address: {ipAddress}");
                return null;
            }

            // Check cache first
            if (_cache.TryGetValue(ipAddress, out var cachedRecord))
            {
                if (DateTime.UtcNow < cachedRecord.CacheExpiry)
                {
                    AppDebug.Log("ProxyCheckService", $"Using cached result for {ipAddress}");
                    return cachedRecord;
                }
                _cache.Remove(ipAddress);
            }

            try
            {
                // Build API URL
                string url = $"{API_BASE_URL}/{ipAddress}?key={ApiKey}";

                AppDebug.Log("ProxyCheckService", $"Calling API for IP: {ipAddress}");

                // Make API request
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                AppDebug.Log("ProxyCheckService", $"API Response: {jsonString}");

                var apiResponse = JsonSerializer.Deserialize<ProxyCheckResponse>(jsonString);

                if (apiResponse == null || apiResponse.Status != "ok")
                {
                    AppDebug.Log("ProxyCheckService", $"API returned non-ok status: {apiResponse?.Status}");
                    return null;
                }

                // Extract IP data from the dynamic property
                if (apiResponse.IpData == null || !apiResponse.IpData.ContainsKey(ipAddress))
                {
                    AppDebug.Log("ProxyCheckService", "IP data not found in response");
                    return null;
                }

                var ipDataElement = apiResponse.IpData[ipAddress];
                var ipData = JsonSerializer.Deserialize<ProxyCheckIpData>(ipDataElement.GetRawText());

                if (ipData == null)
                {
                    AppDebug.Log("ProxyCheckService", "Failed to deserialize IP data");
                    return null;
                }

                // Map to proxyRecord
                var record = MapToProxyRecord(ipData, ip);

                // Cache the result
                _cache[ipAddress] = record;

                AppDebug.Log("ProxyCheckService", 
                    $"Check complete: {ipAddress} | VPN: {record.IsVpn} | Proxy: {record.IsProxy} | Tor: {record.IsTor} | Risk: {record.RiskScore}");

                return record;
            }
            catch (HttpRequestException ex)
            {
                AppDebug.Log("ProxyCheckService", $"HTTP error checking IP {ipAddress}: {ex.Message}");
                return null;
            }
            catch (TaskCanceledException)
            {
                AppDebug.Log("ProxyCheckService", $"API request timeout for IP {ipAddress}");
                return null;
            }
            catch (JsonException ex)
            {
                AppDebug.Log("ProxyCheckService", $"JSON parsing error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                AppDebug.Log("ProxyCheckService", $"Unexpected error checking IP {ipAddress}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Map API response to proxyRecord class
        /// </summary>
        private proxyRecord MapToProxyRecord(ProxyCheckIpData ipData, IPAddress ipAddress)
        {
            var detections = ipData.Detections;
            var location = ipData.Location;
            var network = ipData.Network;

            return new proxyRecord
            {
                RecordID = 0,
                IPAddress = ipAddress,
                
                // Security/Detection data
                IsVpn = detections?.Vpn ?? false,
                IsProxy = detections?.Proxy ?? false,
                IsTor = detections?.Tor ?? false,
                RiskScore = detections?.Risk ?? 0,
                Provider = network?.Provider ?? string.Empty,
                
                // Location data
                CountryCode = location?.CountryCode ?? string.Empty,
                City = location?.CityName ?? string.Empty,
                Region = location?.RegionName ?? string.Empty,
                
                // Cache management
                LastChecked = DateTime.UtcNow,
                CacheExpiry = DateTime.UtcNow.AddHours(CacheDurationHours)
            };
        }

        /// <summary>
        /// Clear in-memory cache
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
            AppDebug.Log("ProxyCheckService", "Cache cleared");
        }

        /// <summary>
        /// Remove expired entries from cache
        /// </summary>
        public void CleanupExpiredCache()
        {
            var now = DateTime.UtcNow;
            var expiredKeys = new List<string>();

            foreach (var kvp in _cache)
            {
                if (now >= kvp.Value.CacheExpiry)
                    expiredKeys.Add(kvp.Key);
            }

            foreach (var key in expiredKeys)
            {
                _cache.Remove(key);
            }

            if (expiredKeys.Count > 0)
                AppDebug.Log("ProxyCheckService", $"Removed {expiredKeys.Count} expired cache entries");
        }

        /// <summary>
        /// Test API connectivity
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                AppDebug.Log("ProxyCheckService", "Cannot test: API key not configured");
                return false;
            }

            try
            {
                // Test with a known public IP (Google DNS)
                var result = await CheckIPAsync("8.8.8.8").ConfigureAwait(false);
                return result != null;
            }
            catch (Exception ex)
            {
                AppDebug.Log("ProxyCheckService", $"Connection test failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get detailed threat information as string
        /// </summary>
        public static string GetThreatSummary(proxyRecord record)
        {
            var threats = new List<string>();

            if (record.IsVpn) threats.Add("VPN");
            if (record.IsProxy) threats.Add("Proxy");
            if (record.IsTor) threats.Add("Tor");
            if (record.RiskScore >= 75) threats.Add($"High Risk ({record.RiskScore})");
            else if (record.RiskScore >= 50) threats.Add($"Medium Risk ({record.RiskScore})");

            return threats.Count > 0 ? string.Join(", ", threats) : "Clean";
        }
    }
}