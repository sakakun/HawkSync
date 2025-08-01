using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace BHD_SharedResources.Classes.Instances
{
    public class banInstance
    {
        public int _recordIdCounter { get; set; } = 1;
        public List<BannedPlayerNames> BannedPlayerNames { get; set; } = new List<BannedPlayerNames>();
        public List<BannedPlayerAddress> BannedPlayerAddresses { get; set; } = new List<BannedPlayerAddress>();
    }

    public class BanSettingsFile
    {
        public List<BannedPlayerNames> BannedPlayerNames { get; set; } = new();
        public List<BannedPlayerAddress> BannedPlayerAddresses { get; set; } = new();
        public int _recordIdCounter { get; set; } = new();
    }
    public class BannedPlayerNames
    {
        public required int recordId { get; set; }
        public required string playerName { get; set; } = string.Empty;
    }
    public class BannedPlayerAddress
    {
        public required int recordId { get; set; }
        [JsonIgnore]
        public required IPAddress playerIP { get; set; }
        public required int subnetMask { get; set; } = 0;

        // For serialization
        [JsonPropertyName("playerIP")]
        public string PlayerIPString
        {
            get => playerIP.ToString();
            set => playerIP = IPAddress.Parse(value);
        }
    }

}
