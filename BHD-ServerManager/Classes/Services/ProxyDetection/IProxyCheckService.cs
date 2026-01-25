using System.Net;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.SupportClasses
{
    /// <summary>
    /// Result from a proxy check service.
    /// </summary>
    public class ProxyCheckResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsProxy { get; set; }
        public bool IsVpn { get; set; }
        public bool IsTor { get; set; }
        public int RiskScore { get; set; }
        public string? Provider { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }

        /// <summary>
        /// Helper property to check if any detection is positive.
        /// </summary>
        public bool IsDetected => IsProxy || IsVpn || IsTor;
    }

    /// <summary>
    /// Interface for proxy check services.
    /// </summary>
    public interface IProxyCheckService
    {
        /// <summary>
        /// Check if an IP address is a proxy, VPN, or Tor.
        /// </summary>
        Task<ProxyCheckResult> CheckIPAsync(IPAddress ipAddress);

        /// <summary>
        /// Get the service name.
        /// </summary>
        string ServiceName { get; }
    }
}