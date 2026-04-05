using ServerManager.Forms;
using HawkSyncShared;
using ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using ServerManager.Classes.SupportClasses;
using ServerManager.Classes.Services;
using ServerManager.Classes.Services.NetLimiter;
using ServerManager.Classes.SupportClasses.Networking;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ServerManager.Classes.InstanceManagers
{
    // ================================================================================
    // DTOs (Data Transfer Objects)
    // ================================================================================

    /// <summary>
    /// Result object for operation outcomes
    /// </summary>
    public record OperationResult(
        bool Success,
        string Message = "",
        int RecordID = 0,
        Exception? Exception = null
    );

    /// <summary>
    /// Result object for operations involving bidirectional records
    /// </summary>
    public record DualRecordResult(
        bool Success,
        string Message = "",
        int NameRecordID = 0,
        int IPRecordID = 0,
        Exception? Exception = null
    );

    /// <summary>
    /// Proxy check settings data transfer object
    /// </summary>
    public record ProxyCheckSettings(
        bool Enabled,
        string ApiKey,
        decimal CacheTime,
        int ProxyAction,
        int VpnAction,
        int TorAction,
        int GeoMode,
        int ServiceProvider
    );

    /// <summary>
    /// NetLimiter settings data transfer object
    /// </summary>
    public record NetLimiterSettings(
        bool Enabled,
        string Host,
        int Port,
        string Username,
        string Password,
        string FilterName,
        bool EnableConLimit,
        decimal ConThreshold
    );

    /// <summary>
    /// Delete action options for associated records
    /// </summary>
    public enum RecordDeleteAction
    {
        None,
        Both,
        NameOnly,
        IPOnly
    }

    // ================================================================================
    // Ban Instance Manager - Business Logic Layer
    // ================================================================================

    public static class banInstanceManager
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance instanceBans => CommonCore.instanceBans!;

        // ================================================================================
        // BLACKLIST OPERATIONS
        // ================================================================================

        /// <summary>
        /// Add a new blacklist name record
        /// </summary>
        public static OperationResult AddBlacklistNameRecord(
            string playerName,
            DateTime banDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int associatedIPID = 0,
            bool ignoreValidation = false)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now && !ignoreValidation)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
                }

                // Duplicate check: player name (case-insensitive)
                var existingName = instanceBans.BannedPlayerNames
                    .FirstOrDefault(x => x.PlayerName.Equals(playerName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingName != null)
                    return new OperationResult(false, "Player name already exists in the blacklist.", existingName.RecordID);

                // Create record
                var nameRecord = new banInstancePlayerName
                {
                    RecordID = 0,
                    MatchID = 0,
                    PlayerName = playerName.Trim(),
                    Date = banDate,
                    ExpireDate = expireDate,
                    AssociatedIP = associatedIPID,
                    RecordType = recordType,
                    RecordCategory = (int)RecordCategory.Ban,
                    Notes = notes.Trim()
                };

                // Add to database
                int recordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                nameRecord.RecordID = recordID;

                // Add to in-memory list
                instanceBans.BannedPlayerNames.Add(nameRecord);

                return new OperationResult(true, "Blacklist name record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding blacklist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Add a new blacklist IP record
        /// </summary>
        public static OperationResult AddBlacklistIPRecord(
            IPAddress ipAddress,
            int subnetMask,
            DateTime banDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int associatedNameID = 0,
            bool ignorevalidation = false)
        {
            try
            {
                // Validation
                if (ipAddress == null)
                    return new OperationResult(false, "IP address is required.");

                if (subnetMask < 0 || subnetMask > 32)
                    return new OperationResult(false, "Subnet mask must be between 0 and 32.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now && !ignorevalidation)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
                }

                // Duplicate check: exact IP/subnet
                var existingIP = instanceBans.BannedPlayerIPs
                    .FirstOrDefault(x => x.PlayerIP.Equals(ipAddress) && x.SubnetMask == subnetMask);
                if (existingIP != null)
                    return new OperationResult(false, "IP address already exists in the blacklist.", existingIP.RecordID);

                // Duplicate check: range overlap
                var newRange = IpRange.FromCidr(ipAddress, subnetMask);
                foreach (var ipRec in instanceBans.BannedPlayerIPs)
                {
                    var existingRange = IpRange.FromCidr(ipRec.PlayerIP, ipRec.SubnetMask);
                    uint newStart = IpRange.IpToUint(newRange.Start);
                    uint newEnd = IpRange.IpToUint(newRange.End);
                    uint existStart = IpRange.IpToUint(existingRange.Start);
                    uint existEnd = IpRange.IpToUint(existingRange.End);

                    if (!(newEnd < existStart || newStart > existEnd))
                        return new OperationResult(false, "IP range overlaps with an existing banned range.", ipRec.RecordID);
                }

                // Create record
                var ipRecord = new banInstancePlayerIP
                {
                    RecordID = 0,
                    MatchID = 0,
                    PlayerIP = ipAddress,
                    SubnetMask = subnetMask,
                    Date = banDate,
                    ExpireDate = expireDate,
                    AssociatedName = associatedNameID,
                    RecordType = recordType,
                    RecordCategory = (int)RecordCategory.Ban,
                    Notes = notes.Trim()
                };

                // Add to database
                int recordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                ipRecord.RecordID = recordID;

                // Add to in-memory list
                instanceBans.BannedPlayerIPs.Add(ipRecord);

                // Add to NetLimiter filter if enabled
                if (theInstance.netLimiterEnabled && !string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                {
                    _ = NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress.ToString(), subnetMask);
                }

                return new OperationResult(true, "Blacklist IP record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding blacklist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Add both name and IP blacklist records with bidirectional association
        /// </summary>
        public static DualRecordResult AddBlacklistBothRecords(
            string playerName,
            IPAddress ipAddress,
            int subnetMask,
            DateTime banDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            bool ignorevalidation = false)
        {
            try
            {
                // Duplicate check: player name
                var existingName = instanceBans.BannedPlayerNames
                    .FirstOrDefault(x => x.PlayerName.Equals(playerName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingName != null)
                    return new DualRecordResult(false, "Player name already exists in the blacklist.", existingName.RecordID, 0);

                // Duplicate check: exact IP/subnet
                var existingIP = instanceBans.BannedPlayerIPs
                    .FirstOrDefault(x => x.PlayerIP.Equals(ipAddress) && x.SubnetMask == subnetMask);
                if (existingIP != null)
                    return new DualRecordResult(false, "IP address already exists in the blacklist.", 0, existingIP.RecordID);

                // Duplicate check: range overlap
                var newRange = IpRange.FromCidr(ipAddress, subnetMask);
                foreach (var ipRec in instanceBans.BannedPlayerIPs)
                {
                    var existingRange = IpRange.FromCidr(ipRec.PlayerIP, ipRec.SubnetMask);
                    uint newStart = IpRange.IpToUint(newRange.Start);
                    uint newEnd = IpRange.IpToUint(newRange.End);
                    uint existStart = IpRange.IpToUint(existingRange.Start);
                    uint existEnd = IpRange.IpToUint(existingRange.End);

                    if (!(newEnd < existStart || newStart > existEnd))
                        return new DualRecordResult(false, "IP range overlaps with an existing banned range.", 0, ipRec.RecordID);
                }

                // Add name record first
                var nameResult = AddBlacklistNameRecord(playerName, banDate, expireDate, recordType, notes, 0, ignorevalidation);
                if (!nameResult.Success)
                    return new DualRecordResult(false, nameResult.Message, 0, 0, nameResult.Exception);

                // Add IP record with association
                var ipResult = AddBlacklistIPRecord(ipAddress, subnetMask, banDate, expireDate, recordType, notes, nameResult.RecordID, ignorevalidation);
                if (!ipResult.Success)
                {
                    // Rollback name record
                    DeleteBlacklistNameRecord(nameResult.RecordID);
                    return new DualRecordResult(false, ipResult.Message, 0, 0, ipResult.Exception);
                }

                // Update name record with IP association
                CreateBidirectionalAssociation(nameResult.RecordID, ipResult.RecordID, RecordCategory.Ban);

                return new DualRecordResult(true, "Blacklist records created successfully.", nameResult.RecordID, ipResult.RecordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding blacklist both records", AppDebug.LogLevel.Error, ex);
                return new DualRecordResult(false, $"Error: {ex.Message}", 0, 0, ex);
            }
        }

        /// <summary>
        /// Update an existing blacklist name record
        /// </summary>
        public static OperationResult UpdateBlacklistNameRecord(
            int recordID,
            string playerName,
            DateTime banDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int? associatedIPID = null)
        {
            try
            {
                var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == recordID);
                if (nameRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                // Validation
                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
                }

                // Update fields
                nameRecord.PlayerName = playerName.Trim();
                nameRecord.Date = banDate;
                nameRecord.ExpireDate = expireDate;
                nameRecord.RecordType = recordType;
                nameRecord.Notes = notes.Trim();
                if (associatedIPID.HasValue)
                    nameRecord.AssociatedIP = associatedIPID.Value;

                // Update database
                DatabaseManager.UpdatePlayerNameRecord(nameRecord);

                return new OperationResult(true, "Blacklist name record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error updating blacklist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Update an existing blacklist IP record
        /// </summary>
        public static OperationResult UpdateBlacklistIPRecord(
            int recordID,
            IPAddress ipAddress,
            int subnetMask,
            DateTime banDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int? associatedNameID = null)
        {
            try
            {
                var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == recordID);
                if (ipRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                // Validation
                if (ipAddress == null)
                    return new OperationResult(false, "IP address is required.");

                if (subnetMask < 0 || subnetMask > 32)
                    return new OperationResult(false, "Subnet mask must be between 0 and 32.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
                }

                // Store old values for NetLimiter update
                string oldIP = ipRecord.PlayerIP.ToString();
                int oldSubnet = ipRecord.SubnetMask;
                bool ipChanged = !ipRecord.PlayerIP.Equals(ipAddress) || ipRecord.SubnetMask != subnetMask;

                // Update fields
                ipRecord.PlayerIP = ipAddress;
                ipRecord.SubnetMask = subnetMask;
                ipRecord.Date = banDate;
                ipRecord.ExpireDate = expireDate;
                ipRecord.RecordType = recordType;
                ipRecord.Notes = notes.Trim();
                if (associatedNameID.HasValue)
                    ipRecord.AssociatedName = associatedNameID.Value;

                // Update database
                DatabaseManager.UpdatePlayerIPRecord(ipRecord);

                // Update NetLimiter filter if enabled and IP/subnet changed
                if (theInstance.netLimiterEnabled && !string.IsNullOrEmpty(theInstance.netLimiterFilterName) && ipChanged)
                {
                    _ = Task.Run(async () =>
                    {
                        await NetLimiterClient.RemoveIpFromFilterAsync(theInstance.netLimiterFilterName, oldIP, oldSubnet);
                        await NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress.ToString(), subnetMask);
                    });
                }

                return new OperationResult(true, "Blacklist IP record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error updating blacklist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete blacklist name record and optionally clear association
        /// </summary>
        public static OperationResult DeleteBlacklistNameRecord(int recordID, bool clearAssociation = true)
        {
            try
            {
                var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == recordID);
                if (nameRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                int associatedIPID = (int)(nameRecord.AssociatedIP != null ? nameRecord.AssociatedIP : 0);

                // Delete from database
                DatabaseManager.RemovePlayerNameRecord(recordID);

                // Remove from in-memory list
                instanceBans.BannedPlayerNames.Remove(nameRecord);

                // Clear association on linked IP record
                if (clearAssociation && associatedIPID > 0)
                {
                    var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == associatedIPID);
                    if (ipRecord != null)
                    {
                        ipRecord.AssociatedName = 0;
                        DatabaseManager.UpdatePlayerIPRecord(ipRecord);
                    }
                }

                return new OperationResult(true, "Blacklist name record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting blacklist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete blacklist IP record and optionally clear association
        /// </summary>
        public static OperationResult DeleteBlacklistIPRecord(int recordID, bool clearAssociation = true)
        {
            try
            {
                var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == recordID);
                if (ipRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                int associatedNameID = (int)(ipRecord.AssociatedName != null?ipRecord.AssociatedName:0);

                // Delete from database
                DatabaseManager.RemovePlayerIPRecord(recordID);

                // Remove from in-memory list
                instanceBans.BannedPlayerIPs.Remove(ipRecord);

                // Remove from NetLimiter filter if enabled
                if (theInstance.netLimiterEnabled && !string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                {
                    _ = NetLimiterClient.RemoveIpFromFilterAsync(theInstance.netLimiterFilterName, ipRecord.PlayerIP.ToString(), ipRecord.SubnetMask);
                }

                // Clear association on linked name record
                if (clearAssociation && associatedNameID > 0)
                {
                    var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == associatedNameID);
                    if (nameRecord != null)
                    {
                        nameRecord.AssociatedIP = 0;
                        DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    }
                }

                return new OperationResult(true, "Blacklist IP record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting blacklist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete both associated name and IP blacklist records
        /// </summary>
        public static OperationResult DeleteBlacklistBothRecords(int nameRecordID, int ipRecordID)
        {
            try
            {
                var nameResult = DeleteBlacklistNameRecord(nameRecordID, clearAssociation: false);
                var ipResult = DeleteBlacklistIPRecord(ipRecordID, clearAssociation: false);

                if (!nameResult.Success || !ipResult.Success)
                {
                    return new OperationResult(false,
                        $"Name: {nameResult.Message}; IP: {ipResult.Message}");
                }

                return new OperationResult(true, "Both blacklist records deleted successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting both blacklist records", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Reload all blacklist records from database
        /// </summary>
        public static (List<banInstancePlayerName> names, List<banInstancePlayerIP> ips) LoadBlacklistRecords()
        {
            instanceBans.BannedPlayerNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Ban);
            instanceBans.BannedPlayerIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Ban);
            return (instanceBans.BannedPlayerNames, instanceBans.BannedPlayerIPs);
        }

        // ================================================================================
        // WHITELIST OPERATIONS
        // ================================================================================

        /// <summary>
        /// Add a new whitelist name record
        /// </summary>
        public static OperationResult AddWhitelistNameRecord(
            string playerName,
            DateTime exemptDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int associatedIPID = 0,
            bool ignorevalidation = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now && !ignorevalidation)
                        return new OperationResult(false, "Temporary whitelist end date must be in the future.");
                }

                var nameRecord = new banInstancePlayerName
                {
                    RecordID = 0,
                    MatchID = 0,
                    PlayerName = playerName.Trim(),
                    Date = exemptDate,
                    ExpireDate = expireDate,
                    AssociatedIP = associatedIPID,
                    RecordType = recordType,
                    RecordCategory = (int)RecordCategory.Whitelist,
                    Notes = notes.Trim()
                };

                int recordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                nameRecord.RecordID = recordID;
                instanceBans.WhitelistedNames.Add(nameRecord);

                return new OperationResult(true, "Whitelist name record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding whitelist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Add a new whitelist IP record
        /// </summary>
        public static OperationResult AddWhitelistIPRecord(
            IPAddress ipAddress,
            int subnetMask,
            DateTime exemptDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int associatedNameID = 0,
            bool ignorevalidation = false)
        {
            try
            {
                if (ipAddress == null)
                    return new OperationResult(false, "IP address is required.");

                if (subnetMask < 0 || subnetMask > 32)
                    return new OperationResult(false, "Subnet mask must be between 0 and 32.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now && !ignorevalidation)
                        return new OperationResult(false, "Temporary whitelist end date must be in the future.");
                }

                var ipRecord = new banInstancePlayerIP
                {
                    RecordID = 0,
                    MatchID = 0,
                    PlayerIP = ipAddress,
                    SubnetMask = subnetMask,
                    Date = exemptDate,
                    ExpireDate = expireDate,
                    AssociatedName = associatedNameID,
                    RecordType = recordType,
                    RecordCategory = (int)RecordCategory.Whitelist,
                    Notes = notes.Trim()
                };

                int recordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                ipRecord.RecordID = recordID;
                instanceBans.WhitelistedIPs.Add(ipRecord);

                return new OperationResult(true, "Whitelist IP record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding whitelist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Add both name and IP whitelist records with bidirectional association
        /// </summary>
        public static DualRecordResult AddWhitelistBothRecords(
            string playerName,
            IPAddress ipAddress,
            int subnetMask,
            DateTime exemptDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            bool ignorevalidation = false)
        {
            try
            {
                var nameResult = AddWhitelistNameRecord(playerName, exemptDate, expireDate, recordType, notes, 0, ignorevalidation);
                if (!nameResult.Success)
                    return new DualRecordResult(false, nameResult.Message, 0, 0, nameResult.Exception);

                var ipResult = AddWhitelistIPRecord(ipAddress, subnetMask, exemptDate, expireDate, recordType, notes, nameResult.RecordID, ignorevalidation);
                if (!ipResult.Success)
                {
                    DeleteWhitelistNameRecord(nameResult.RecordID);
                    return new DualRecordResult(false, ipResult.Message, 0, 0, ipResult.Exception);
                }

                CreateBidirectionalAssociation(nameResult.RecordID, ipResult.RecordID, RecordCategory.Whitelist);

                return new DualRecordResult(true, "Whitelist records created successfully.", nameResult.RecordID, ipResult.RecordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding whitelist both records", AppDebug.LogLevel.Error, ex);
                return new DualRecordResult(false, $"Error: {ex.Message}", 0, 0, ex);
            }
        }

        /// <summary>
        /// Update an existing whitelist name record
        /// </summary>
        public static OperationResult UpdateWhitelistNameRecord(
            int recordID,
            string playerName,
            DateTime exemptDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int? associatedIPID = null)
        {
            try
            {
                var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == recordID);
                if (nameRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary whitelist end date must be in the future.");
                }

                nameRecord.PlayerName = playerName.Trim();
                nameRecord.Date = exemptDate;
                nameRecord.ExpireDate = expireDate;
                nameRecord.RecordType = recordType;
                nameRecord.Notes = notes.Trim();
                if (associatedIPID.HasValue)
                    nameRecord.AssociatedIP = associatedIPID.Value;

                DatabaseManager.UpdatePlayerNameRecord(nameRecord);

                return new OperationResult(true, "Whitelist name record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error updating whitelist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Update an existing whitelist IP record
        /// </summary>
        public static OperationResult UpdateWhitelistIPRecord(
            int recordID,
            IPAddress ipAddress,
            int subnetMask,
            DateTime exemptDate,
            DateTime? expireDate,
            banInstanceRecordType recordType,
            string notes,
            int? associatedNameID = null)
        {
            try
            {
                var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == recordID);
                if (ipRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                if (ipAddress == null)
                    return new OperationResult(false, "IP address is required.");

                if (subnetMask < 0 || subnetMask > 32)
                    return new OperationResult(false, "Subnet mask must be between 0 and 32.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary whitelist end date must be in the future.");
                }

                ipRecord.PlayerIP = ipAddress;
                ipRecord.SubnetMask = subnetMask;
                ipRecord.Date = exemptDate;
                ipRecord.ExpireDate = expireDate;
                ipRecord.RecordType = recordType;
                ipRecord.Notes = notes.Trim();
                if (associatedNameID.HasValue)
                    ipRecord.AssociatedName = associatedNameID.Value;

                DatabaseManager.UpdatePlayerIPRecord(ipRecord);

                return new OperationResult(true, "Whitelist IP record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error updating whitelist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete whitelist name record and optionally clear association
        /// </summary>
        public static OperationResult DeleteWhitelistNameRecord(int recordID, bool clearAssociation = true)
        {
            try
            {
                var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == recordID);
                if (nameRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                int associatedIPID = (int)(nameRecord.AssociatedIP != null?nameRecord.AssociatedIP:0);

                DatabaseManager.RemovePlayerNameRecord(recordID);
                instanceBans.WhitelistedNames.Remove(nameRecord);

                if (clearAssociation && associatedIPID > 0)
                {
                    var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == associatedIPID);
                    if (ipRecord != null)
                    {
                        ipRecord.AssociatedName = 0;
                        DatabaseManager.UpdatePlayerIPRecord(ipRecord);
                    }
                }

                return new OperationResult(true, "Whitelist name record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting whitelist name record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete whitelist IP record and optionally clear association
        /// </summary>
        public static OperationResult DeleteWhitelistIPRecord(int recordID, bool clearAssociation = true)
        {
            try
            {
                var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == recordID);
                if (ipRecord == null)
                    return new OperationResult(false, $"Record ID {recordID} not found.");

                int associatedNameID = (int)(ipRecord.AssociatedName!=null?ipRecord.AssociatedName:0);

                DatabaseManager.RemovePlayerIPRecord(recordID);
                instanceBans.WhitelistedIPs.Remove(ipRecord);

                if (clearAssociation && associatedNameID > 0)
                {
                    var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == associatedNameID);
                    if (nameRecord != null)
                    {
                        nameRecord.AssociatedIP = 0;
                        DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    }
                }

                return new OperationResult(true, "Whitelist IP record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting whitelist IP record", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Delete both associated name and IP whitelist records
        /// </summary>
        public static OperationResult DeleteWhitelistBothRecords(int nameRecordID, int ipRecordID)
        {
            try
            {
                var nameResult = DeleteWhitelistNameRecord(nameRecordID, clearAssociation: false);
                var ipResult = DeleteWhitelistIPRecord(ipRecordID, clearAssociation: false);

                if (!nameResult.Success || !ipResult.Success)
                {
                    return new OperationResult(false,
                        $"Name: {nameResult.Message}; IP: {ipResult.Message}");
                }

                return new OperationResult(true, "Both whitelist records deleted successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error deleting both whitelist records", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Reload all whitelist records from database
        /// </summary>
        public static (List<banInstancePlayerName> names, List<banInstancePlayerIP> ips) LoadWhitelistRecords()
        {
            instanceBans.WhitelistedNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Whitelist);
            instanceBans.WhitelistedIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Whitelist);
            return (instanceBans.WhitelistedNames, instanceBans.WhitelistedIPs);
        }

        // ================================================================================
        // PROXY CHECK OPERATIONS
        // ================================================================================

        /// <summary>
        /// Load proxy check settings from database
        /// </summary>
        public static ProxyCheckSettings LoadProxyCheckSettings()
        {
            theInstance.proxyCheckEnabled = ServerSettings.Get("proxyCheckEnabled", theInstance.proxyCheckEnabled);
            theInstance.proxyCheckAPIKey = ServerSettings.Get("proxyCheckAPIKey", theInstance.proxyCheckAPIKey);
            theInstance.proxyCheckCacheTime = ServerSettings.Get("proxyCheckCacheTime", theInstance.proxyCheckCacheTime);
            theInstance.proxyCheckProxyAction = ServerSettings.Get("proxyCheckProxyAction", theInstance.proxyCheckProxyAction);
            theInstance.proxyCheckVPNAction = ServerSettings.Get("proxyCheckVPNAction", theInstance.proxyCheckVPNAction);
            theInstance.proxyCheckTORAction = ServerSettings.Get("proxyCheckTORAction", theInstance.proxyCheckTORAction);
            theInstance.proxyCheckGeoMode = ServerSettings.Get("proxyCheckGeoMode", theInstance.proxyCheckGeoMode);
            theInstance.proxyCheckServiceProvider = ServerSettings.Get("proxyCheckServiceProvider", theInstance.proxyCheckServiceProvider);

            return new ProxyCheckSettings(
                theInstance.proxyCheckEnabled,
                theInstance.proxyCheckAPIKey,
                theInstance.proxyCheckCacheTime,
                theInstance.proxyCheckProxyAction,
                theInstance.proxyCheckVPNAction,
                theInstance.proxyCheckTORAction,
                theInstance.proxyCheckGeoMode,
                theInstance.proxyCheckServiceProvider
            );
        }

        /// <summary>
        /// Save proxy check settings to database
        /// </summary>
        public static OperationResult SaveProxyCheckSettings(ProxyCheckSettings settings)
        {
            try
            {
                theInstance.proxyCheckEnabled = settings.Enabled;
                theInstance.proxyCheckAPIKey = settings.ApiKey;
                theInstance.proxyCheckCacheTime = settings.CacheTime;
                theInstance.proxyCheckProxyAction = settings.ProxyAction;
                theInstance.proxyCheckVPNAction = settings.VpnAction;
                theInstance.proxyCheckTORAction = settings.TorAction;
                theInstance.proxyCheckGeoMode = settings.GeoMode;
                theInstance.proxyCheckServiceProvider = settings.ServiceProvider;

                ServerSettings.Set("proxyCheckEnabled", settings.Enabled);
                ServerSettings.Set("proxyCheckAPIKey", settings.ApiKey);
                ServerSettings.Set("proxyCheckCacheTime", settings.CacheTime);
                ServerSettings.Set("proxyCheckProxyAction", settings.ProxyAction);
                ServerSettings.Set("proxyCheckVPNAction", settings.VpnAction);
                ServerSettings.Set("proxyCheckTORAction", settings.TorAction);
                ServerSettings.Set("proxyCheckGeoMode", settings.GeoMode);
                ServerSettings.Set("proxyCheckServiceProvider", settings.ServiceProvider);

                return new OperationResult(true, "Proxy check settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error saving proxy check settings", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Initialize proxy check service based on current settings
        /// </summary>
        public static OperationResult InitializeProxyService()
        {
            try
            {
                if (!theInstance.proxyCheckEnabled)
                    return new OperationResult(false, "Proxy check is not enabled.");

                if (ProxyCheckManager.IsInitialized)
                    return new OperationResult(true, "Proxy service already initialized.");

                IProxyCheckService? proxyService = null;

                if (theInstance.proxyCheckServiceProvider == 1)
                {
                    proxyService = new ProxyCheckIoService(theInstance.proxyCheckAPIKey);
                }
                else if (theInstance.proxyCheckServiceProvider == 2)
                {
                    proxyService = new IP2LocationService(theInstance.proxyCheckAPIKey);
                }
                else
                {
                    return new OperationResult(false, "No valid proxy service provider selected.");
                }

                if (proxyService != null)
                {
                    ProxyCheckManager.Initialize(proxyService, cacheExpirationDays: (int)theInstance.proxyCheckCacheTime);
                    return new OperationResult(true, "Proxy service initialized successfully.");
                }

                return new OperationResult(false, "Failed to create proxy service instance.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error initializing proxy service", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Test proxy service with a given IP address
        /// </summary>
        public static async Task<(bool success, ProxyCheckResult? result, string? errorMessage)> TestProxyService(
            string apiKey,
            int serviceProvider,
            IPAddress testIP)
        {
            try
            {
                IProxyCheckService? proxyService = null;

                if (serviceProvider == 1)
                {
                    proxyService = new ProxyCheckIoService(apiKey);
                }
                else if (serviceProvider == 2)
                {
                    proxyService = new IP2LocationService(apiKey);
                }
                else
                {
                    return (false, null, "Invalid service provider.");
                }

                if (proxyService == null)
                    return (false, null, "Failed to create proxy service instance.");

                var result = await proxyService.CheckIPAsync(testIP);

                if (result.Success)
                {
                    return (true, result, string.Empty);
                }
                else
                {
                    AppDebug.Log($"Proxy service test failed:", AppDebug.LogLevel.Error, new Exception(result.ErrorMessage));
                    return (false, result, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error testing proxy service",AppDebug.LogLevel.Error, ex);
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Add a blocked country to the proxy geo-blocking list
        /// </summary>
        public static OperationResult AddBlockedCountry(string countryCode, string countryName)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
                    return new OperationResult(false, "Country code must be exactly 2 characters.");

                if (string.IsNullOrWhiteSpace(countryName))
                    return new OperationResult(false, "Country name cannot be empty.");

                // Check for duplicates (case-insensitive)
                if (instanceBans.ProxyBlockedCountries.Any(c => c.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)))
                    return new OperationResult(false, $"Country code '{countryCode}' already exists.");

                // Add to database
                int recordID = DatabaseManager.AddProxyBlockedCountry(countryCode.ToUpperInvariant(), countryName);

                // If the insert failed due to UNIQUE constraint, try to fetch the existing record
                if (recordID <= 0)
                {
                    // Try to get the existing record's ID
                    var existing = instanceBans.ProxyBlockedCountries
                        .FirstOrDefault(c => c.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                        return new OperationResult(false, $"Country code '{countryCode}' already exists.", existing.RecordID);

                    return new OperationResult(false, "Failed to add blocked country to database.");
                }

                var newCountry = new proxyCountry
                {
                    RecordID = recordID,
                    CountryCode = countryCode.ToUpperInvariant(),
                    CountryName = countryName
                };
                instanceBans.ProxyBlockedCountries.Add(newCountry);

                return new OperationResult(true, "Blocked country added successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding blocked country", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Remove a blocked country from the proxy geo-blocking list
        /// </summary>
        public static OperationResult RemoveBlockedCountry(int recordID)
        {
            var country = instanceBans.ProxyBlockedCountries.FirstOrDefault(c => c.RecordID == recordID);

            if (country == null)
                return new OperationResult(false, $"Country record ID {recordID} not found.");

            try
            {

                if (DatabaseManager.RemoveProxyBlockedCountry(recordID))
                {
                    instanceBans.ProxyBlockedCountries.Remove(country);
                    return new OperationResult(true, "Blocked country removed successfully.", recordID);
                }

                return new OperationResult(false, "Failed to remove blocked country from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error removing blocked country", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Load all blocked countries from database
        /// </summary>
        [Obsolete]
        public static List<proxyCountry> LoadBlockedCountries()
        {
            // Assuming banInstance.ProxyBlockedCountries is already loaded from database
            // If not, add database call here
            return instanceBans.ProxyBlockedCountries;
        }

        // ================================================================================
        // NETLIMITER OPERATIONS
        // ================================================================================

        /// <summary>
        /// Load NetLimiter settings from database
        /// </summary>
        public static NetLimiterSettings LoadNetLimiterSettings()
        {
            theInstance.netLimiterEnabled = ServerSettings.Get("netLimiterEnabled", theInstance.netLimiterEnabled);
            theInstance.netLimiterHost = ServerSettings.Get("netLimiterHost", theInstance.netLimiterHost);
            theInstance.netLimiterPort = ServerSettings.Get("netLimiterPort", theInstance.netLimiterPort);
            theInstance.netLimiterUsername = ServerSettings.Get("netLimiterUsername", theInstance.netLimiterUsername);
            theInstance.netLimiterPassword = ServerSettings.Get("netLimiterPassword", theInstance.netLimiterPassword);
            theInstance.netLimiterFilterName = ServerSettings.Get("netLimiterFilterName", theInstance.netLimiterFilterName);
            theInstance.netLimiterEnableConLimit = ServerSettings.Get("netLimiterEnableConLimit", theInstance.netLimiterEnableConLimit);
            theInstance.netLimiterConThreshold = ServerSettings.Get("netLimiterConThreshold", theInstance.netLimiterConThreshold);

            return new NetLimiterSettings(
                theInstance.netLimiterEnabled,
                theInstance.netLimiterHost,
                theInstance.netLimiterPort,
                theInstance.netLimiterUsername,
                theInstance.netLimiterPassword,
                theInstance.netLimiterFilterName,
                theInstance.netLimiterEnableConLimit,
                theInstance.netLimiterConThreshold
            );
        }

        /// <summary>
        /// Save NetLimiter settings to database
        /// </summary>
        public static OperationResult SaveNetLimiterSettings(NetLimiterSettings settings)
        {
            try
            {
                theInstance.netLimiterEnabled = settings.Enabled;
                theInstance.netLimiterHost = settings.Host;
                theInstance.netLimiterPort = settings.Port;
                theInstance.netLimiterUsername = settings.Username;
                theInstance.netLimiterPassword = settings.Password;
                theInstance.netLimiterFilterName = settings.FilterName;
                theInstance.netLimiterEnableConLimit = settings.EnableConLimit;
                theInstance.netLimiterConThreshold = settings.ConThreshold;

                ServerSettings.Set("netLimiterEnabled", settings.Enabled);
                ServerSettings.Set("netLimiterHost", settings.Host);
                ServerSettings.Set("netLimiterPort", settings.Port);
                ServerSettings.Set("netLimiterUsername", settings.Username);
                ServerSettings.Set("netLimiterPassword", settings.Password);
                ServerSettings.Set("netLimiterFilterName", settings.FilterName);
                ServerSettings.Set("netLimiterEnableConLimit", settings.EnableConLimit);
                ServerSettings.Set("netLimiterConThreshold", settings.ConThreshold);

                return new OperationResult(true, "NetLimiter settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error saving NetLimiter settings", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Start NetLimiter bridge process
        /// </summary>
        public static async Task<OperationResult> StartNetLimiterBridgeAsync(string host, int port, string username, string password)
        {
            try
            {
                // If bridge is already running, return success
                if (NetLimiterClient._bridgeProcess != null && !NetLimiterClient._bridgeProcess.HasExited)
                {
                    return new OperationResult(true, "NetLimiter bridge already running.");
                }

                await NetLimiterClient.StartBridgeProcessAsync(host, (ushort)port, username, password);

                if (NetLimiterClient._bridgeProcess != null)
                {
                    return new OperationResult(true, "NetLimiter bridge started successfully.");
                }

                return new OperationResult(false, "Failed to start NetLimiter bridge process.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error starting NetLimiter bridge", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Get list of available NetLimiter filters
        /// </summary>
        public static async Task<(bool success, List<string> filters, string errorMessage)> GetNetLimiterFilters()
        {
            try
            {
                var filters = await NetLimiterClient.GetFilterNamesAsync();

                if (filters.Count > 0)
                {
                    return (true, filters, string.Empty);
                }

                return (false, new List<string>(), "No filters found.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error getting NetLimiter filters", AppDebug.LogLevel.Error, ex);
                return (false, new List<string>(), ex.Message);
            }
        }

        /// <summary>
        /// Add IP to NetLimiter filter
        /// </summary>
        public static async Task<OperationResult> AddIPToNetLimiterFilter(string filterName, IPAddress ipAddress, int subnetMask)
        {
            try
            {
                if (!theInstance.netLimiterEnabled)
                    return new OperationResult(false, "NetLimiter is not enabled.");

                if (string.IsNullOrEmpty(filterName))
                    return new OperationResult(false, "Filter name is required.");

                await NetLimiterClient.AddIpToFilterAsync(filterName, ipAddress.ToString(), subnetMask);

                return new OperationResult(true, $"IP added to filter '{filterName}' successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding IP to NetLimiter filter", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Remove IP from NetLimiter filter
        /// </summary>
        public static async Task<OperationResult> RemoveIPFromNetLimiterFilter(string filterName, IPAddress ipAddress, int subnetMask)
        {
            try
            {
                if (!theInstance.netLimiterEnabled)
                    return new OperationResult(false, "NetLimiter is not enabled.");

                if (string.IsNullOrEmpty(filterName))
                    return new OperationResult(false, "Filter name is required.");

                await NetLimiterClient.RemoveIpFromFilterAsync(filterName, ipAddress.ToString(), subnetMask);

                return new OperationResult(true, $"IP removed from filter '{filterName}' successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error removing IP from NetLimiter filter", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // ================================================================================
        // HELPER METHODS
        // ================================================================================

        /// <summary>
        /// Create bidirectional association between name and IP records
        /// </summary>
        private static void CreateBidirectionalAssociation(int nameRecordID, int ipRecordID, RecordCategory category)
        {
            try
            {
                if (category == RecordCategory.Ban)
                {
                    var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == nameRecordID);
                    if (nameRecord != null)
                    {
                        nameRecord.AssociatedIP = ipRecordID;
                        DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    }
                }
                else if (category == RecordCategory.Whitelist)
                {
                    var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == nameRecordID);
                    if (nameRecord != null)
                    {
                        nameRecord.AssociatedIP = ipRecordID;
                        DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error creating bidirectional association", AppDebug.LogLevel.Error, ex);
            }
        }

        /// <summary>
        /// Load all ban/whitelist settings and data from database
        /// </summary>
        public static void LoadSettings()
        {
            // Database Records
            CommonCore.instanceBans!.BannedPlayerNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Ban);
            CommonCore.instanceBans!.BannedPlayerIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Ban);
            CommonCore.instanceBans!.WhitelistedNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Whitelist);
            CommonCore.instanceBans!.WhitelistedIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Whitelist);
            CommonCore.instanceBans!.ConnectionHistory = DatabaseManager.GetPlayerNameRecords(RecordCategory.ConnectionHistory);
            CommonCore.instanceBans!.IPConnectionHistory = DatabaseManager.GetPlayerIPRecords(RecordCategory.ConnectionHistory);
            CommonCore.instanceBans!.ProxyRecords = DatabaseManager.GetProxyRecords();
            CommonCore.instanceBans!.ProxyBlockedCountries = DatabaseManager.GetProxyBlockedCountries();

            LoadProxyCheckSettings();
            LoadNetLimiterSettings();
        }
    }
}