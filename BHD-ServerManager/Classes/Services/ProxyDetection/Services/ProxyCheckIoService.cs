using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.SupportClasses
{
    /// <summary>
    /// ProxyCheck.io API implementation.
    /// </summary>
    public class ProxyCheckIoService : IProxyCheckService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://proxycheck.io/v3/";

        public string ServiceName => "ProxyCheck.io";

        public ProxyCheckIoService(string apiKey)
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
                string url = $"{BaseUrl}{ipAddress}?key={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ProxyCheckIoResponse>(jsonString, new JsonSerializerOptions
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

                // Check status
                if (apiResponse.Status != "ok" && apiResponse.Status != "warning")
                {
                    return new ProxyCheckResult
                    {
                        Success = false,
                        ErrorMessage = apiResponse.Message ?? $"API returned status: {apiResponse.Status}"
                    };
                }

                // Get the IP data from the dynamic property
                var ipData = apiResponse.GetIPData(ipAddress.ToString());
                if (ipData == null)
                {
                    return new ProxyCheckResult
                    {
                        Success = false,
                        ErrorMessage = "No data found for IP address in response."
                    };
                }

                return new ProxyCheckResult
                {
                    Success = true,
                    IsProxy = ipData.Detections?.Proxy ?? false,
                    IsVpn = ipData.Detections?.Vpn ?? false,
                    IsTor = ipData.Detections?.Tor ?? false,
                    RiskScore = ipData.Detections?.Risk ?? 0,
                    Provider = ipData.Network?.Provider,
                    CountryCode = ipData.Location?.CountryCode,
                    CountryName = ipData.Location?.CountryName,
                    City = ipData.Location?.CityName,
                    Region = ipData.Location?.RegionName
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

        private class ProxyCheckIoResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("message")]
            public string? Message { get; set; }

            [JsonPropertyName("query_time")]
            public int? QueryTime { get; set; }

            [JsonExtensionData]
            public Dictionary<string, JsonElement>? AdditionalData { get; set; }

            public IPDataResponse? GetIPData(string ipAddress)
            {
                if (AdditionalData?.TryGetValue(ipAddress, out var element) == true)
                {
                    return JsonSerializer.Deserialize<IPDataResponse>(element.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                return null;
            }
        }

        private class IPDataResponse
        {
            [JsonPropertyName("network")]
            public NetworkInfo? Network { get; set; }

            [JsonPropertyName("location")]
            public LocationInfo? Location { get; set; }

            [JsonPropertyName("detections")]
            public DetectionsInfo? Detections { get; set; }

            [JsonPropertyName("last_updated")]
            public string? LastUpdated { get; set; }
        }

        private class NetworkInfo
        {
            [JsonPropertyName("asn")]
            public string? Asn { get; set; }

            [JsonPropertyName("provider")]
            public string? Provider { get; set; }

            [JsonPropertyName("organisation")]
            public string? Organisation { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }
        }

        private class LocationInfo
        {
            [JsonPropertyName("continent_name")]
            public string? ContinentName { get; set; }

            [JsonPropertyName("continent_code")]
            public string? ContinentCode { get; set; }

            [JsonPropertyName("country_name")]
            public string? CountryName { get; set; }

            [JsonPropertyName("country_code")]
            public string? CountryCode { get; set; }

            [JsonPropertyName("region_name")]
            public string? RegionName { get; set; }

            [JsonPropertyName("region_code")]
            public string? RegionCode { get; set; }

            [JsonPropertyName("city_name")]
            public string? CityName { get; set; }

            [JsonPropertyName("postal_code")]
            public string? PostalCode { get; set; }
        }

        private class DetectionsInfo
        {
            [JsonPropertyName("proxy")]
            public bool Proxy { get; set; }

            [JsonPropertyName("vpn")]
            public bool Vpn { get; set; }

            [JsonPropertyName("tor")]
            public bool Tor { get; set; }

            [JsonPropertyName("risk")]
            public int Risk { get; set; }

            [JsonPropertyName("confidence")]
            public int? Confidence { get; set; }

            [JsonPropertyName("anonymous")]
            public bool? Anonymous { get; set; }
        }

        #endregion
    }
}