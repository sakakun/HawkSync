using System.Text.Json.Serialization;

namespace BHD_ServerManager.Classes.Services.ProxyDetection
{
    // Root response object for proxycheck.io
    public class ProxyCheckResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("query_time")]
        public int QueryTime { get; set; }

        // IP data - use JsonExtensionData for dynamic IP key
        [JsonExtensionData]
        public Dictionary<string, System.Text.Json.JsonElement>? IpData { get; set; }
    }

    // IP-specific data
    public class ProxyCheckIpData
    {
        [JsonPropertyName("network")]
        public ProxyCheckNetworkData? Network { get; set; }

        [JsonPropertyName("location")]
        public ProxyCheckLocationData? Location { get; set; }

        [JsonPropertyName("detections")]
        public ProxyCheckDetectionData? Detections { get; set; }

        [JsonPropertyName("last_updated")]
        public string LastUpdated { get; set; } = string.Empty;
    }

    // Network information
    public class ProxyCheckNetworkData
    {
        [JsonPropertyName("asn")]
        public string Asn { get; set; } = string.Empty;

        [JsonPropertyName("range")]
        public string Range { get; set; } = string.Empty;

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        [JsonPropertyName("organisation")]
        public string Organisation { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    // Detection/Security data
    public class ProxyCheckDetectionData
    {
        [JsonPropertyName("proxy")]
        public bool Proxy { get; set; }

        [JsonPropertyName("vpn")]
        public bool Vpn { get; set; }

        [JsonPropertyName("tor")]
        public bool Tor { get; set; }

        [JsonPropertyName("compromised")]
        public bool Compromised { get; set; }

        [JsonPropertyName("scraper")]
        public bool Scraper { get; set; }

        [JsonPropertyName("hosting")]
        public bool Hosting { get; set; }

        [JsonPropertyName("anonymous")]
        public bool Anonymous { get; set; }

        [JsonPropertyName("risk")]
        public int Risk { get; set; }

        [JsonPropertyName("confidence")]
        public int Confidence { get; set; }

        [JsonPropertyName("first_seen")]
        public string? FirstSeen { get; set; }

        [JsonPropertyName("last_seen")]
        public string? LastSeen { get; set; }
    }

    // Geographic location data
    public class ProxyCheckLocationData
    {
        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; } = string.Empty;

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; } = string.Empty;

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; } = string.Empty;

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonPropertyName("region_name")]
        public string RegionName { get; set; } = string.Empty;

        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; } = string.Empty;

        [JsonPropertyName("city_name")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = string.Empty;
    }
}