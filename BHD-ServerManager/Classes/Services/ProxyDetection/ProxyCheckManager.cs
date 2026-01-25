using BHD_ServerManager.Classes.Instances;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.SupportClasses
{
    /// <summary>
    /// Manages proxy checking with in-memory caching using banInstance.ProxyRecords.
    /// Reduces database I/O by checking memory first, then database, then API.
    /// </summary>
    public class ProxyCheckManager
    {
        private readonly banInstance _banInstance;
        private readonly IProxyCheckService _proxyService;
        private readonly TimeSpan _cacheExpiration;
        private readonly object _lock = new();

        /// <summary>
        /// Initialize the proxy check manager.
        /// </summary>
        /// <param name="banInstance">The ban instance containing the in-memory proxy records</param>
        /// <param name="proxyService">The proxy check service to use (e.g., ProxyCheck.io)</param>
        /// <param name="cacheExpirationDays">Number of days before cache expires (default: 7)</param>
        public ProxyCheckManager(banInstance banInstance, IProxyCheckService proxyService, int cacheExpirationDays = 7)
        {
            _banInstance = banInstance ?? throw new ArgumentNullException(nameof(banInstance));
            _proxyService = proxyService ?? throw new ArgumentNullException(nameof(proxyService));
            _cacheExpiration = TimeSpan.FromDays(cacheExpirationDays);
        }

        /// <summary>
        /// Load all proxy records from the database into memory.
        /// Call this during application initialization.
        /// </summary>
        public int LoadCacheFromDatabase()
        {
            var dbRecords = DatabaseManager.GetProxyRecords();
            
            lock (_lock)
            {
                // Clear existing records to avoid duplicates
                _banInstance.ProxyRecords.Clear();
                
                // Add all database records to memory
                _banInstance.ProxyRecords.AddRange(dbRecords);
            }

            AppDebug.Log("ProxyCheckManager", $"Loaded {dbRecords.Count} proxy records from database into memory");
            return dbRecords.Count;
        }

        /// <summary>
        /// Reload cache from database, merging with existing in-memory records.
        /// Useful if another process might have updated the database.
        /// </summary>
        public int ReloadCacheFromDatabase(bool replaceExisting = true)
        {
            var dbRecords = DatabaseManager.GetProxyRecords();
            int addedCount = 0;
            int updatedCount = 0;

            lock (_lock)
            {
                foreach (var dbRecord in dbRecords)
                {
                    var existingRecord = _banInstance.ProxyRecords
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
                        _banInstance.ProxyRecords.Add(dbRecord);
                        addedCount++;
                    }
                }
            }

            AppDebug.Log("ProxyCheckManager", 
                $"Reloaded cache from database: {addedCount} added, {updatedCount} updated");
            
            return addedCount + updatedCount;
        }

        /// <summary>
        /// Check an IP address for proxy/VPN/Tor. Uses in-memory cache first, then database, then API.
        /// </summary>
        public async Task<ProxyCheckResult> CheckIPAsync(IPAddress ipAddress)
        {
            if (ipAddress == null)
                throw new ArgumentNullException(nameof(ipAddress));

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
                        if (!_banInstance.ProxyRecords.Any(r => r.IPAddress.Equals(dbRecord.IPAddress)))
                        {
                            _banInstance.ProxyRecords.Add(dbRecord);
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
        private proxyRecord? GetFromMemoryCache(IPAddress ipAddress)
        {
            lock (_lock)
            {
                return _banInstance.ProxyRecords.FirstOrDefault(r => r.IPAddress.Equals(ipAddress));
            }
        }

        /// <summary>
        /// Check if a cache record has expired.
        /// </summary>
        private bool IsCacheExpired(proxyRecord record)
        {
            return DateTime.UtcNow > record.CacheExpiry;
        }

        /// <summary>
        /// Update the cache (both memory and database) with new API results.
        /// </summary>
        private void UpdateCache(IPAddress ipAddress, ProxyCheckResult apiResult, proxyRecord? existingRecord)
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
                        _banInstance.ProxyRecords.Add(newRecord);
                    }

                    AppDebug.Log("ProxyCheckManager", $"Added new cache entry for {ipAddress} (ID: {newId})");
                }
            }
        }

        /// <summary>
        /// Convert a proxyRecord to a ProxyCheckResult.
        /// </summary>
        private ProxyCheckResult ConvertToResult(proxyRecord record, bool fromCache)
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
        public void InvalidateCache(IPAddress ipAddress)
        {
            lock (_lock)
            {
                var record = _banInstance.ProxyRecords.FirstOrDefault(r => r.IPAddress.Equals(ipAddress));
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
        public int CleanupExpiredCache(bool removeFromDatabase = false)
        {
            int removedCount = 0;

            lock (_lock)
            {
                var expiredRecords = _banInstance.ProxyRecords
                    .Where(r => IsCacheExpired(r))
                    .ToList();

                foreach (var record in expiredRecords)
                {
                    if (removeFromDatabase)
                    {
                        DatabaseManager.RemoveProxyRecord(record.RecordID);
                    }

                    _banInstance.ProxyRecords.Remove(record);
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
        public CacheStatistics GetCacheStatistics()
        {
            lock (_lock)
            {
                var total = _banInstance.ProxyRecords.Count;
                var expired = _banInstance.ProxyRecords.Count(r => IsCacheExpired(r));
                var vpnCount = _banInstance.ProxyRecords.Count(r => r.IsVpn);
                var proxyCount = _banInstance.ProxyRecords.Count(r => r.IsProxy);
                var torCount = _banInstance.ProxyRecords.Count(r => r.IsTor);

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