using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.Services.NetLimiter;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
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
            int associatedIPID = 0)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
                }

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

                AppDebug.Log("banInstanceManager", $"Added blacklist name record: {playerName} (ID: {recordID})");
                return new OperationResult(true, "Blacklist name record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding blacklist name record: {ex}");
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
            int associatedNameID = 0)
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
                    if (expireDate.Value <= DateTime.Now)
                        return new OperationResult(false, "Temporary ban end date must be in the future.");
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

                AppDebug.Log("banInstanceManager", $"Added blacklist IP record: {ipAddress}/{subnetMask} (ID: {recordID})");
                return new OperationResult(true, "Blacklist IP record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding blacklist IP record: {ex}");
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
            string notes)
        {
            try
            {
                // Add name record first
                var nameResult = AddBlacklistNameRecord(playerName, banDate, expireDate, recordType, notes);
                if (!nameResult.Success)
                    return new DualRecordResult(false, nameResult.Message, 0, 0, nameResult.Exception);

                // Add IP record with association
                var ipResult = AddBlacklistIPRecord(ipAddress, subnetMask, banDate, expireDate, recordType, notes, nameResult.RecordID);
                if (!ipResult.Success)
                {
                    // Rollback name record
                    DeleteBlacklistNameRecord(nameResult.RecordID);
                    return new DualRecordResult(false, ipResult.Message, 0, 0, ipResult.Exception);
                }

                // Update name record with IP association
                CreateBidirectionalAssociation(nameResult.RecordID, ipResult.RecordID, RecordCategory.Ban);

                AppDebug.Log("banInstanceManager", $"Created bidirectional blacklist: Name {nameResult.RecordID} <-> IP {ipResult.RecordID}");
                return new DualRecordResult(true, "Blacklist records created successfully.", nameResult.RecordID, ipResult.RecordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding blacklist both records: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Updated blacklist name record ID {recordID}");
                return new OperationResult(true, "Blacklist name record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error updating blacklist name record: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Updated blacklist IP record ID {recordID}");
                return new OperationResult(true, "Blacklist IP record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error updating blacklist IP record: {ex}");
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
                        AppDebug.Log("banInstanceManager", $"Cleared association on IP record {ipRecord.RecordID}");
                    }
                }

                AppDebug.Log("banInstanceManager", $"Deleted blacklist name record ID {recordID}");
                return new OperationResult(true, "Blacklist name record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting blacklist name record: {ex}");
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
                        AppDebug.Log("banInstanceManager", $"Cleared association on name record {nameRecord.RecordID}");
                    }
                }

                AppDebug.Log("banInstanceManager", $"Deleted blacklist IP record ID {recordID}");
                return new OperationResult(true, "Blacklist IP record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting blacklist IP record: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Deleted both blacklist records: Name {nameRecordID}, IP {ipRecordID}");
                return new OperationResult(true, "Both blacklist records deleted successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting both blacklist records: {ex}");
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

            AppDebug.Log("banInstanceManager",
                $"Loaded {instanceBans.BannedPlayerNames.Count} name bans and {instanceBans.BannedPlayerIPs.Count} IP bans");

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
            int associatedIPID = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerName))
                    return new OperationResult(false, "Player name cannot be empty.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
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

                AppDebug.Log("banInstanceManager", $"Added whitelist name record: {playerName} (ID: {recordID})");
                return new OperationResult(true, "Whitelist name record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding whitelist name record: {ex}");
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
            int associatedNameID = 0)
        {
            try
            {
                if (ipAddress == null)
                    return new OperationResult(false, "IP address is required.");

                if (subnetMask < 0 || subnetMask > 32)
                    return new OperationResult(false, "Subnet mask must be between 0 and 32.");

                if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
                {
                    if (expireDate.Value <= DateTime.Now)
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

                AppDebug.Log("banInstanceManager", $"Added whitelist IP record: {ipAddress}/{subnetMask} (ID: {recordID})");
                return new OperationResult(true, "Whitelist IP record created successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding whitelist IP record: {ex}");
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
            string notes)
        {
            try
            {
                var nameResult = AddWhitelistNameRecord(playerName, exemptDate, expireDate, recordType, notes);
                if (!nameResult.Success)
                    return new DualRecordResult(false, nameResult.Message, 0, 0, nameResult.Exception);

                var ipResult = AddWhitelistIPRecord(ipAddress, subnetMask, exemptDate, expireDate, recordType, notes, nameResult.RecordID);
                if (!ipResult.Success)
                {
                    DeleteWhitelistNameRecord(nameResult.RecordID);
                    return new DualRecordResult(false, ipResult.Message, 0, 0, ipResult.Exception);
                }

                CreateBidirectionalAssociation(nameResult.RecordID, ipResult.RecordID, RecordCategory.Whitelist);

                AppDebug.Log("banInstanceManager", $"Created bidirectional whitelist: Name {nameResult.RecordID} <-> IP {ipResult.RecordID}");
                return new DualRecordResult(true, "Whitelist records created successfully.", nameResult.RecordID, ipResult.RecordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding whitelist both records: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Updated whitelist name record ID {recordID}");
                return new OperationResult(true, "Whitelist name record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error updating whitelist name record: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Updated whitelist IP record ID {recordID}");
                return new OperationResult(true, "Whitelist IP record updated successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error updating whitelist IP record: {ex}");
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
                        AppDebug.Log("banInstanceManager", $"Cleared association on IP whitelist record {ipRecord.RecordID}");
                    }
                }

                AppDebug.Log("banInstanceManager", $"Deleted whitelist name record ID {recordID}");
                return new OperationResult(true, "Whitelist name record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting whitelist name record: {ex}");
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
                        AppDebug.Log("banInstanceManager", $"Cleared association on name whitelist record {nameRecord.RecordID}");
                    }
                }

                AppDebug.Log("banInstanceManager", $"Deleted whitelist IP record ID {recordID}");
                return new OperationResult(true, "Whitelist IP record deleted successfully.", recordID);
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting whitelist IP record: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Deleted both whitelist records: Name {nameRecordID}, IP {ipRecordID}");
                return new OperationResult(true, "Both whitelist records deleted successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error deleting both whitelist records: {ex}");
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

            AppDebug.Log("banInstanceManager",
                $"Loaded {instanceBans.WhitelistedNames.Count} name whitelists and {instanceBans.WhitelistedIPs.Count} IP whitelists");

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

                AppDebug.Log("banInstanceManager", "Proxy check settings saved");
                return new OperationResult(true, "Proxy check settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error saving proxy check settings: {ex}");
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
                    AppDebug.Log("banInstanceManager", "Proxy service initialized successfully");
                    return new OperationResult(true, "Proxy service initialized successfully.");
                }

                return new OperationResult(false, "Failed to create proxy service instance.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error initializing proxy service: {ex}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Test proxy service with a given IP address
        /// </summary>
        public static async Task<(bool success, ProxyCheckResult? result, string errorMessage)> TestProxyService(
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
                    AppDebug.Log("banInstanceManager", $"Proxy service test successful for {testIP}");
                    return (true, result, string.Empty);
                }
                else
                {
                    AppDebug.Log("banInstanceManager", $"Proxy service test failed: {result.ErrorMessage}");
                    return (false, result, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error testing proxy service: {ex}");
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

                // Check for duplicates
                if (instanceBans.ProxyBlockedCountries.Any(c => c.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)))
                    return new OperationResult(false, $"Country code '{countryCode}' already exists.");

                // Add to database
                int recordID = DatabaseManager.AddProxyBlockedCountry(countryCode.ToUpper(), countryName);

                if (recordID > 0)
                {
                    var newCountry = new proxyCountry
                    {
                        RecordID = recordID,
                        CountryCode = countryCode.ToUpper(),
                        CountryName = countryName
                    };
                    instanceBans.ProxyBlockedCountries.Add(newCountry);

                    AppDebug.Log("banInstanceManager", $"Added blocked country: {countryCode} - {countryName} (ID: {recordID})");
                    return new OperationResult(true, "Blocked country added successfully.", recordID);
                }

                return new OperationResult(false, "Failed to add blocked country to database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding blocked country: {ex}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Remove a blocked country from the proxy geo-blocking list
        /// </summary>
        public static OperationResult RemoveBlockedCountry(int recordID)
        {
            try
            {
                var country = instanceBans.ProxyBlockedCountries.FirstOrDefault(c => c.RecordID == recordID);
                if (country == null)
                    return new OperationResult(false, $"Country record ID {recordID} not found.");

                if (DatabaseManager.RemoveProxyBlockedCountry(recordID))
                {
                    instanceBans.ProxyBlockedCountries.Remove(country);
                    AppDebug.Log("banInstanceManager", $"Removed blocked country: {country.CountryCode} (ID: {recordID})");
                    return new OperationResult(true, "Blocked country removed successfully.", recordID);
                }

                return new OperationResult(false, "Failed to remove blocked country from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error removing blocked country: {ex}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Load all blocked countries from database
        /// </summary>
        public static List<proxyCountry> LoadBlockedCountries()
        {
            // Assuming instanceBans.ProxyBlockedCountries is already loaded from database
            // If not, add database call here
            AppDebug.Log("banInstanceManager", $"Loaded {instanceBans.ProxyBlockedCountries.Count} blocked countries");
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

                AppDebug.Log("banInstanceManager", "NetLimiter settings saved");
                return new OperationResult(true, "NetLimiter settings saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error saving NetLimiter settings: {ex}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Start NetLimiter bridge process
        /// </summary>
        public static OperationResult StartNetLimiterBridge(string host, int port, string username, string password)
        {
            try
            {
                // If bridge is already running, return success
                if (NetLimiterClient._bridgeProcess != null && !NetLimiterClient._bridgeProcess.HasExited)
                {
                    return new OperationResult(true, "NetLimiter bridge already running.");
                }

                NetLimiterClient.StartBridgeProcess(host, (ushort)port, username, password);

                if (NetLimiterClient._bridgeProcess != null)
                {
                    AppDebug.Log("banInstanceManager", "NetLimiter bridge process started");
                    return new OperationResult(true, "NetLimiter bridge started successfully.");
                }

                return new OperationResult(false, "Failed to start NetLimiter bridge process.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error starting NetLimiter bridge: {ex}");
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
                    AppDebug.Log("banInstanceManager", $"Retrieved {filters.Count} NetLimiter filters");
                    return (true, filters, string.Empty);
                }

                return (false, new List<string>(), "No filters found.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error getting NetLimiter filters: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Added {ipAddress}/{subnetMask} to NetLimiter filter '{filterName}'");
                return new OperationResult(true, $"IP added to filter '{filterName}' successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error adding IP to NetLimiter filter: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Removed {ipAddress}/{subnetMask} from NetLimiter filter '{filterName}'");
                return new OperationResult(true, $"IP removed from filter '{filterName}' successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error removing IP from NetLimiter filter: {ex}");
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

                AppDebug.Log("banInstanceManager", $"Created bidirectional association: Name {nameRecordID} <-> IP {ipRecordID}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("banInstanceManager", $"Error creating bidirectional association: {ex}");
            }
        }

        /// <summary>
        /// Load all ban/whitelist settings and data from database
        /// </summary>
        public static void LoadSettings()
        {
            LoadBlacklistRecords();
            LoadWhitelistRecords();
            LoadBlockedCountries();
            LoadProxyCheckSettings();
            LoadNetLimiterSettings();
            AppDebug.Log("banInstanceManager", "All ban/whitelist settings loaded");
        }
    }
}