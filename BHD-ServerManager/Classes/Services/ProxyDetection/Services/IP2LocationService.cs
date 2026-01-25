using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.SupportClasses
{
    /// <summary>
    /// IP2Location.io API implementation.
    /// Requires Security Plan for proxy detection features.
    /// </summary>
    public class IP2LocationService : IProxyCheckService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.ip2location.io/";

        public string ServiceName => "IP2Location.io";

        public IP2LocationService(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be empty.", nameof(apiKey));

            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<ProxyCheckResult> CheckIPAsync(IPAddress ipAddress)
        {
            try
            {
                string url = $"{BaseUrl}?ip={ipAddress}&key={_apiKey}&format=json";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<IP2LocationResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse == null)
                {
                    return new ProxyCheckResult
                    {
                        Success = false,
                        ErrorMessage = "Failed to parse API response."
                    };
                }

                // Check for errors
                if (apiResponse.Error != null)
                {
                    return new ProxyCheckResult
                    {
                        Success = false,
                        ErrorMessage = $"API Error {apiResponse.Error.ErrorCode}: {apiResponse.Error.ErrorMessage}"
                    };
                }

                // Calculate risk score from fraud_score (0-99 in IP2Location)
                // Normalize to 0-100 scale to match proxyCheck.io
                int riskScore = apiResponse.FraudScore ?? 0;
                if (riskScore > 100)
                {
                    riskScore = 100;
                }

                // Determine provider from proxy data or ISP
                string? provider = null;
                if (apiResponse.Proxy?.Provider != null && apiResponse.Proxy.Provider != "-")
                {
                    provider = apiResponse.Proxy.Provider;
                }
                else if (apiResponse.Isp != null && apiResponse.Isp != "-")
                {
                    provider = apiResponse.Isp;
                }
                return new ProxyCheckResult
                {
                    Success = true,
                    IsProxy = (apiResponse.Proxy?.IsPublicProxy ?? false) || 
                              (apiResponse.Proxy?.IsWebProxy ?? false) ||
                              (apiResponse.Proxy?.IsResidentialProxy ?? false),
                    IsVpn = apiResponse.Proxy?.IsVpn ?? false,
                    IsTor = apiResponse.Proxy?.IsTor ?? false,
                    RiskScore = riskScore,
                    Provider = provider,
                    CountryCode = apiResponse.CountryCode,
                    CountryName = apiResponse.CountryName,
                    City = apiResponse.CityName,
                    Region = apiResponse.RegionName
                };
            }
            catch (HttpRequestException ex)
            {
                return new ProxyCheckResult
                {
                    Success = false,
                    ErrorMessage = $"HTTP request failed: {ex.Message}"
                };
            }
            catch (TaskCanceledException)
            {
                return new ProxyCheckResult
                {
                    Success = false,
                    ErrorMessage = "Request timed out."
                };
            }
            catch (Exception ex)
            {
                return new ProxyCheckResult
                {
                    Success = false,
                    ErrorMessage = $"Unexpected error: {ex.Message}"
                };
            }
        }

        #region API Response Models

        private class IP2LocationResponse
        {
            [JsonPropertyName("ip")]
            public string? Ip { get; set; }

            [JsonPropertyName("country_code")]
            public string? CountryCode { get; set; }

            [JsonPropertyName("country_name")]
            public string? CountryName { get; set; }

            [JsonPropertyName("region_name")]
            public string? RegionName { get; set; }

            [JsonPropertyName("district")]
            public string? District { get; set; }

            [JsonPropertyName("city_name")]
            public string? CityName { get; set; }

            [JsonPropertyName("latitude")]
            public double? Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public double? Longitude { get; set; }

            [JsonPropertyName("zip_code")]
            public string? ZipCode { get; set; }

            [JsonPropertyName("time_zone")]
            public string? TimeZone { get; set; }

            [JsonPropertyName("asn")]
            public string? Asn { get; set; }

            [JsonPropertyName("as")]
            public string? As { get; set; }

            [JsonPropertyName("isp")]
            public string? Isp { get; set; }

            [JsonPropertyName("domain")]
            public string? Domain { get; set; }

            [JsonPropertyName("usage_type")]
            public string? UsageType { get; set; }

            [JsonPropertyName("is_proxy")]
            public bool? IsProxy { get; set; }

            [JsonPropertyName("fraud_score")]
            public int? FraudScore { get; set; }

            [JsonPropertyName("proxy")]
            public ProxyInfo? Proxy { get; set; }

            [JsonPropertyName("error")]
            public ErrorInfo? Error { get; set; }
        }

        private class ProxyInfo
        {
            [JsonPropertyName("last_seen")]
            public int? LastSeen { get; set; }

            [JsonPropertyName("proxy_type")]
            public string? ProxyType { get; set; }

            [JsonPropertyName("threat")]
            public string? Threat { get; set; }

            [JsonPropertyName("provider")]
            public string? Provider { get; set; }

            [JsonPropertyName("is_vpn")]
            public bool? IsVpn { get; set; }

            [JsonPropertyName("is_tor")]
            public bool? IsTor { get; set; }

            [JsonPropertyName("is_data_center")]
            public bool? IsDataCenter { get; set; }

            [JsonPropertyName("is_public_proxy")]
            public bool? IsPublicProxy { get; set; }

            [JsonPropertyName("is_web_proxy")]
            public bool? IsWebProxy { get; set; }

            [JsonPropertyName("is_web_crawler")]
            public bool? IsWebCrawler { get; set; }

            [JsonPropertyName("is_residential_proxy")]
            public bool? IsResidentialProxy { get; set; }

            [JsonPropertyName("is_consumer_privacy_network")]
            public bool? IsConsumerPrivacyNetwork { get; set; }

            [JsonPropertyName("is_enterprise_private_network")]
            public bool? IsEnterprisePrivateNetwork { get; set; }

            [JsonPropertyName("is_spammer")]
            public bool? IsSpammer { get; set; }

            [JsonPropertyName("is_scanner")]
            public bool? IsScanner { get; set; }

            [JsonPropertyName("is_botnet")]
            public bool? IsBotnet { get; set; }

            [JsonPropertyName("is_bogon")]
            public bool? IsBogon { get; set; }
        }

        private class ErrorInfo
        {
            [JsonPropertyName("error_code")]
            public int ErrorCode { get; set; }

            [JsonPropertyName("error_message")]
            public string ErrorMessage { get; set; } = string.Empty;
        }

        #endregion
    }
}