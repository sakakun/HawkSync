using HawkSyncShared.Instances;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BHD_ServerManager.Classes.SupportClasses
{
    /// <summary>
    /// Static manager for proxy checking with in-memory caching using CommonCore.banInstance.ProxyRecords.
    /// Reduces database I/O by checking memory first, then database, then API.
    /// </summary>
    public static class ProxyCheckManager
    {
        private static IProxyCheckService? _proxyService;
        private static TimeSpan _cacheExpiration = TimeSpan.FromDays(7);
        private static readonly object _lock = new();

        public static bool IsInitialized => _proxyService != null;

        /// <summary>
        /// Initialize the proxy check manager with a service provider.
        /// Call this during application startup after CommonCore.InitializeCore().
        /// </summary>
        /// <param name="proxyService">The proxy check service to use (e.g., ProxyCheck.io)</param>
        /// <param name="cacheExpirationDays">Number of days before cache expires (default: 7)</param>
        public static void Initialize(IProxyCheckService proxyService, int cacheExpirationDays = 7)
        {
            _proxyService = proxyService ?? throw new ArgumentNullException(nameof(proxyService));
            _cacheExpiration = TimeSpan.FromDays(cacheExpirationDays);
            AppDebug.Log("ProxyCheckManager", $"Initialized with {proxyService.ServiceName}, cache expiration: {cacheExpirationDays} days");
        }

        /// <summary>
        /// Reload cache from database, merging with existing in-memory records.
        /// Useful if another process might have updated the database.
        /// </summary>
        public static int ReloadCacheFromDatabase(bool replaceExisting = true)
        {
            if (CommonCore.instanceBans == null)
                throw new InvalidOperationException("CommonCore.banInstance is not initialized.");

            var dbRecords = DatabaseManager.GetProxyRecords();
            int addedCount = 0;
            int updatedCount = 0;

            lock (_lock)
            {
                foreach (var dbRecord in dbRecords)
                {
                    var existingRecord = CommonCore.instanceBans.ProxyRecords
                        .FirstOrDefault(r => r.IPAddress.Equals(dbRecord.IPAddress));

                    if (existingRecord != null)
                    {
                        if (replaceExisting && dbRecord.LastChecked > existingRecord.LastChecked)
                        {
                            // Update existing record with newer database data
                            existingRecord.IsVpn = dbRecord.IsVpn;
                            existingRecord.IsProxy = dbRecord.IsProxy;
                            existingRecord.IsTor = dbRecord.IsTor;
                            existingRecord.RiskScore = dbRecord.RiskScore;
                            existingRecord.Provider = dbRecord.Provider;
                            existingRecord.CountryCode = dbRecord.CountryCode;
                            existingRecord.City = dbRecord.City;
                            existingRecord.Region = dbRecord.Region;
                            existingRecord.CacheExpiry = dbRecord.CacheExpiry;
                            existingRecord.LastChecked = dbRecord.LastChecked;
                            updatedCount++;
                        }
                    }
                    else
                    {
                        // Add new record
                        CommonCore.instanceBans.ProxyRecords.Add(dbRecord);
                        addedCount++;
                    }
                }
            }

            AppDebug.Log("ProxyCheckManager", 
                $"Reloaded cache from database: {addedCount} added, {updatedCount} updated");
            
            return addedCount + updatedCount;
        }
        
        /// <summary>
        /// Check if an IP address is internal/private.
        /// </summary>
        private static bool IsInternalIP(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                // Check for IPv6 loopback (::1) and link-local (fe80::/10)
                if (IPAddress.IsLoopback(ipAddress))
                    return true;

                byte[] bytes = ipAddress.GetAddressBytes();
                // Link-local: fe80::/10
                if (bytes[0] == 0xfe && (bytes[1] & 0xc0) == 0x80)
                    return true;

                // Unique local: fc00::/7
                if ((bytes[0] & 0xfe) == 0xfc)
                    return true;

                return false;
            }

            // IPv4 checks
            if (IPAddress.IsLoopback(ipAddress))
                return true;

            byte[] addressBytes = ipAddress.GetAddressBytes();

            // 10.0.0.0/8
            if (addressBytes[0] == 10)
                return true;

            // 172.16.0.0/12
            if (addressBytes[0] == 172 && addressBytes[1] >= 16 && addressBytes[1] <= 31)
                return true;

            // 192.168.0.0/16
            if (addressBytes[0] == 192 && addressBytes[1] == 168)
                return true;

            // 169.254.0.0/16 (link-local)
            if (addressBytes[0] == 169 && addressBytes[1] == 254)
                return true;

            return false;
        }
        /// <summary>
        /// Check an IP address for proxy/VPN/Tor. Uses in-memory cache first, then database, then API.
        /// </summary>
        public static async Task<ProxyCheckResult> CheckIPAsync(IPAddress ipAddress)
        {
            AppDebug.Log("CheckIPAsync", $"Attempting to check: {ipAddress.ToString()}");

            if (CommonCore.instanceBans == null)
                throw new InvalidOperationException("CommonCore.banInstance is not initialized.");

            if (_proxyService == null)
                throw new InvalidOperationException("ProxyCheckManager is not initialized. Call Initialize() first.");

            if (ipAddress == null)
                throw new ArgumentNullException(nameof(ipAddress));

            // Step 0: Check if IP is internal/private - skip proxy check
            if (IsInternalIP(ipAddress))
            {
                AppDebug.Log("ProxyCheckManager", $"Internal IP detected: {ipAddress} - Skipping proxy check");
                return new ProxyCheckResult
                {
                    Success = true,
                    IsProxy = false,
                    IsVpn = false,
                    IsTor = false,
                    RiskScore = 0,
                    ErrorMessage = "Internal IP Proxy Check Skipped"
                };
            }

            // Step 1: Check in-memory cache first
            var cachedRecord = GetFromMemoryCache(ipAddress);
            if (cachedRecord != null && !IsCacheExpired(cachedRecord))
            {
                AppDebug.Log("ProxyCheckManager", $"Cache HIT (Memory): {ipAddress}");
                return ConvertToResult(cachedRecord, fromCache: true);
            }

            // Step 2: If expired in memory, check database (might have been updated by another process)
            if (cachedRecord == null)
            {
                var dbRecord = DatabaseManager.GetProxyRecordByIP(ipAddress);
                if (dbRecord != null)
                {
                    // Add to memory cache
                    lock (_lock)
                    {
                        if (!CommonCore.instanceBans.ProxyRecords.Any(r => r.IPAddress.Equals(dbRecord.IPAddress)))
                        {
                            CommonCore.instanceBans.ProxyRecords.Add(dbRecord);
                        }
                    }

                    if (!IsCacheExpired(dbRecord))
                    {
                        AppDebug.Log("ProxyCheckManager", $"Cache HIT (Database): {ipAddress}");
                        return ConvertToResult(dbRecord, fromCache: true);
                    }

                    cachedRecord = dbRecord;
                }
            }

            // Step 3: Cache miss or expired - query the API
            AppDebug.Log("ProxyCheckManager", $"Cache MISS: {ipAddress} - Querying {_proxyService.ServiceName}");
            var apiResult = await _proxyService.CheckIPAsync(ipAddress);

            if (!apiResult.Success)
            {
                // API failed - if we have an expired cached record, return it with a warning
                if (cachedRecord != null)
                {
                    AppDebug.Log("ProxyCheckManager", $"API failed, using expired cache for {ipAddress}");
                    var result = ConvertToResult(cachedRecord, fromCache: true);
                    result.ErrorMessage = $"Using expired cache. API error: {apiResult.ErrorMessage}";
                    return result;
                }

                return apiResult;
            }

            // Step 4: Update or create the record
            UpdateCache(ipAddress, apiResult, cachedRecord);

            return apiResult;
        }

        /// <summary>
        /// Get a record from the in-memory cache.
        /// </summary>
        private static proxyRecord? GetFromMemoryCache(IPAddress ipAddress)
        {
            lock (_lock)
            {
                return CommonCore.instanceBans!.ProxyRecords.FirstOrDefault(r => r.IPAddress.Equals(ipAddress));
            }
        }

        /// <summary>
        /// Check if a cache record has expired.
        /// </summary>
        private static bool IsCacheExpired(proxyRecord record)
        {
            return DateTime.UtcNow > record.CacheExpiry;
        }

        /// <summary>
        /// Update the cache (both memory and database) with new API results.
        /// </summary>
        private static void UpdateCache(IPAddress ipAddress, ProxyCheckResult apiResult, proxyRecord? existingRecord)
        {
            var now = DateTime.UtcNow;

            if (existingRecord != null)
            {
                // Update existing record
                lock (_lock)
                {
                    existingRecord.IsVpn = apiResult.IsVpn;
                    existingRecord.IsProxy = apiResult.IsProxy;
                    existingRecord.IsTor = apiResult.IsTor;
                    existingRecord.RiskScore = apiResult.RiskScore;
                    existingRecord.Provider = apiResult.Provider;
                    existingRecord.CountryCode = apiResult.CountryCode;
                    existingRecord.City = apiResult.City;
                    existingRecord.Region = apiResult.Region;
                    existingRecord.LastChecked = now;
                    existingRecord.CacheExpiry = now.Add(_cacheExpiration);
                }

                // Update database
                DatabaseManager.UpdateProxyRecord(existingRecord);
                AppDebug.Log("ProxyCheckManager", $"Updated cache for {ipAddress}");
            }
            else
            {
                // Create new record
                var newRecord = new proxyRecord
                {
                    RecordID = 0,
                    IPAddress = ipAddress,
                    IsVpn = apiResult.IsVpn,
                    IsProxy = apiResult.IsProxy,
                    IsTor = apiResult.IsTor,
                    RiskScore = apiResult.RiskScore,
                    Provider = apiResult.Provider,
                    CountryCode = apiResult.CountryCode,
                    City = apiResult.City,
                    Region = apiResult.Region,
                    LastChecked = now,
                    CacheExpiry = now.Add(_cacheExpiration)
                };

                // Add to database first to get the RecordID
                int newId = DatabaseManager.AddProxyRecord(newRecord);
                if (newId > 0)
                {
                    newRecord.RecordID = newId;

                    // Add to memory cache
                    lock (_lock)
                    {
                        CommonCore.instanceBans!.ProxyRecords.Add(newRecord);
                    }

                    AppDebug.Log("ProxyCheckManager", $"Added new cache entry for {ipAddress} (ID: {newId})");
                }
            }
        }

        /// <summary>
        /// Convert a proxyRecord to a ProxyCheckResult.
        /// </summary>
        private static ProxyCheckResult ConvertToResult(proxyRecord record, bool fromCache)
        {
            return new ProxyCheckResult
            {
                Success = true,
                IsProxy = record.IsProxy,
                IsVpn = record.IsVpn,
                IsTor = record.IsTor,
                RiskScore = record.RiskScore,
                Provider = record.Provider,
                CountryCode = record.CountryCode,
                City = record.City,
                Region = record.Region
            };
        }

        /// <summary>
        /// Manually invalidate cache for a specific IP (forces re-check on next query).
        /// </summary>
        public static void InvalidateCache(IPAddress ipAddress)
        {
            if (CommonCore.instanceBans == null)
                throw new InvalidOperationException("CommonCore.banInstance is not initialized.");

            lock (_lock)
            {
                var record = CommonCore.instanceBans.ProxyRecords.FirstOrDefault(r => r.IPAddress.Equals(ipAddress));
                if (record != null)
                {
                    record.CacheExpiry = DateTime.UtcNow.AddSeconds(-1);
                    AppDebug.Log("ProxyCheckManager", $"Invalidated cache for {ipAddress}");
                }
            }
        }

        /// <summary>
        /// Clear expired cache entries from memory and optionally from database.
        /// </summary>
        public static int CleanupExpiredCache(bool removeFromDatabase = false)
        {
            if (CommonCore.instanceBans == null)
                throw new InvalidOperationException("CommonCore.banInstance is not initialized.");

            int removedCount = 0;

            lock (_lock)
            {
                var expiredRecords = CommonCore.instanceBans.ProxyRecords
                    .Where(r => IsCacheExpired(r))
                    .ToList();

                foreach (var record in expiredRecords)
                {
                    if (removeFromDatabase)
                    {
                        DatabaseManager.RemoveProxyRecord(record.RecordID);
                    }

                    CommonCore.instanceBans.ProxyRecords.Remove(record);
                    removedCount++;
                }
            }

            if (removedCount > 0)
            {
                AppDebug.Log("ProxyCheckManager", $"Cleaned up {removedCount} expired cache entries");
            }

            return removedCount;
        }

        /// <summary>
        /// Get cache statistics.
        /// </summary>
        public static CacheStatistics GetCacheStatistics()
        {
            if (CommonCore.instanceBans == null)
                throw new InvalidOperationException("CommonCore.banInstance is not initialized.");

            lock (_lock)
            {
                var total = CommonCore.instanceBans.ProxyRecords.Count;
                var expired = CommonCore.instanceBans.ProxyRecords.Count(r => IsCacheExpired(r));
                var vpnCount = CommonCore.instanceBans.ProxyRecords.Count(r => r.IsVpn);
                var proxyCount = CommonCore.instanceBans.ProxyRecords.Count(r => r.IsProxy);
                var torCount = CommonCore.instanceBans.ProxyRecords.Count(r => r.IsTor);

                return new CacheStatistics
                {
                    TotalEntries = total,
                    ExpiredEntries = expired,
                    ValidEntries = total - expired,
                    VpnCount = vpnCount,
                    ProxyCount = proxyCount,
                    TorCount = torCount
                };
            }
        }
    }

    /// <summary>
    /// Cache statistics information.
    /// </summary>
    public class CacheStatistics
    {
        public int TotalEntries { get; set; }
        public int ValidEntries { get; set; }
        public int ExpiredEntries { get; set; }
        public int VpnCount { get; set; }
        public int ProxyCount { get; set; }
        public int TorCount { get; set; }
    }
}