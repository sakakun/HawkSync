using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace BHD_ServerManager.Classes.Instances
{
    public class banInstance
    {
        public List<banInstancePlayerName>    BannedPlayerNames     { get; set; } = new();
        public List<banInstancePlayerIP>      BannedPlayerIPs       { get; set; } = new();
        public List<banInstancePlayerName>    WhitelistedNames      { get; set; } = new();
        public List<banInstancePlayerIP>      WhitelistedIPs        { get; set; } = new();
        public List<banInstancePlayerName>    ConnectionHistory     { get; set; } = new();
        public List<banInstancePlayerIP>      IPConnectionHistory   { get; set; } = new();
        public List<proxyRecord>              ProxyRecords          { get; set; } = new();
        public List<proxyCountry>             ProxyBlockedCountries { get; set; } = new();
	}

    public class banInstancePlayerName
    {
        public required int             RecordID            { get; set; }
        public int					    MatchID             { get; set; } = 0;
        public required string          PlayerName          { get; set; }
		public DateTime 			    Date                { get; set; }
        public DateTime?                ExpireDate		    { get; set; }
        public int?                     AssociatedIP        { get; set; }
        public required banInstanceRecordType    RecordType { get; set; }
        public required int             RecordCategory      { get; set; }
		public string 			        Notes               { get; set; } = string.Empty;    

	}

    public class banInstancePlayerIP
    {
        public required int             RecordID            { get; set; }
        public int					    MatchID             { get; set; } = 0;
        public required IPAddress       PlayerIP            { get; set; }
        public required int			    SubnetMask          { get; set; }
		public DateTime 			    Date                { get; set; }
        public DateTime?                ExpireDate		    { get; set; }
        public int?                     AssociatedName      { get; set; }
        public required banInstanceRecordType    RecordType { get; set; }
        public required int             RecordCategory      { get; set; }
		public string 			        Notes               { get; set; } = string.Empty;
    }

    public class proxyRecord
    {
        public required int             RecordID            { get; set; }
		public required IPAddress       IPAddress           { get; set; }
        public bool                     IsVpn               { get; set; }
        public bool                     IsProxy             { get; set; }
        public bool                     IsTor               { get; set; }
        public int                      RiskScore           { get; set; }            // 0-100
        public string?                  Provider            { get; set; }            // VPN provider name
        public string?                  CountryCode         { get; set; }            // Country ISO Code
		public string?                  City                { get; set; }            // City
		public string?                  Region              { get; set; }            // State/Province
		public DateTime                 CacheExpiry         { get; set; }
        public DateTime				    LastChecked         { get; set; }
	}

    public class proxyCountry
    {
        public required int             RecordID            { get; set; }
        public required string          CountryCode         { get; set; }            // Country ISO Code
        public required string          CountryName         { get; set; }            // Country Name
	}

	public enum banInstanceRecordType
    {
        Information = 0,           // Just log, no action
        Temporary = 1,             // Expires after time
        Permanent = 2              // Never expires
    }

    public enum RecordCategory
    {
        Ban = 0,
        Whitelist = 1,
        ConnectionHistory = 2
	}
}

