using HawkSyncShared.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
/*
namespace BHD_ServerManager.Classes.Services.ProxyDetection
{
    internal class ProxyExampleCalls
    {
        public ProxyExampleCalls() { 
            
        // During application startup - initialize once
        var proxyService = new ProxyCheckIoService("0u86pj-j4j5p0-04zv1p-ws1oy3");
        var proxyManager = new ProxyCheckManager(theInstance.BanInstance, proxyService, cacheExpirationDays: 7);

        // Store the manager in a global/static location or dependency injection
        // For example, add it to your theInstance class

        // When a player connects
        var playerIP = IPAddress.Parse("5.6.27.4");
        var result = await proxyManager.CheckIPAsync(playerIP);

        if (result.Success)
        {
            if (result.IsVpn)
            {
                // Handle VPN connection
                Console.WriteLine($"VPN detected: {playerIP} - Provider: {result.Provider}");
            }
    
            if (result.IsProxy)
            {
                // Handle proxy connection
                Console.WriteLine($"Proxy detected: {playerIP}");
            }
    
            if (result.IsTor)
            {
                // Handle Tor connection
                Console.WriteLine($"Tor detected: {playerIP}");
            }
    
            // Check against blocked countries
            if (!string.IsNullOrEmpty(result.CountryCode))
            {
                var isBlocked = theInstance.BanInstance.ProxyBlockedCountries
                    .Any(c => c.CountryCode.Equals(result.CountryCode, StringComparison.OrdinalIgnoreCase));
        
                if (isBlocked)
                {
                    Console.WriteLine($"Blocked country: {result.CountryCode}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Proxy check failed: {result.ErrorMessage}");
        }

        // Periodic cleanup (run this on a timer)
        var removed = proxyManager.CleanupExpiredCache(removeFromDatabase: false);

        // Get statistics
        var stats = proxyManager.GetCacheStatistics();
        Console.WriteLine($"Cache: {stats.ValidEntries} valid, {stats.ExpiredEntries} expired");
        Console.WriteLine($"Detections: {stats.VpnCount} VPNs, {stats.ProxyCount} proxies, {stats.TorCount} Tor");
        }

    }
}

public class theInstance
{
    // ... existing properties ...
    
    public banInstance BanInstance { get; set; }
    public ProxyCheckManager? ProxyManager { get; set; }
    
    public void InitializeProxyChecking(string apiKey)
    {
        var proxyService = new ProxyCheckIoService(apiKey);
        ProxyManager = new ProxyCheckManager(BanInstance, proxyService);
    }
}

// ProxyCheck.io
var proxyCheckService = new ProxyCheckIoService("0u86pj-j4j5p0-04zv1p-ws1oy3");
var manager1 = new ProxyCheckManager(banInstance, proxyCheckService);

// IP2Location.io (requires Security Plan for proxy features)
var ip2LocationService = new IP2LocationService("your-api-key-here");
var manager2 = new ProxyCheckManager(banInstance, ip2LocationService);

// Both work the same way
var result = await manager1.CheckIPAsync(ipAddress);
// or
var result = await manager2.CheckIPAsync(ipAddress);

*/