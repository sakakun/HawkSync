using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System.Net;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;

        // Throttle for NetLimiter connection checks
        private static DateTime _lastNetLimiterCheck = DateTime.MinValue;
        private static readonly TimeSpan _netLimiterCheckInterval = TimeSpan.FromSeconds(10);

        // Throttle for NetLimiter filter sync (once per minute)
        private static DateTime _lastFilterSync = DateTime.MinValue;
        private static readonly TimeSpan _filterSyncInterval = TimeSpan.FromMinutes(1);


        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        // This method runs the ticker for ban management tasks.
        public static void runTicker()
        {

            string gameServerPath = Path.Combine(theInstance.profileServerPath, "dfbhd.exe");

            // Always marshal to UI thread for UI updates
            SafeInvoke(thisServer, () =>
            {
                // Ban Related Ticker Tasks
                thisServer.BanTab.tickerUpdate();

                // Is NetLimiter Enabled? Is the Process Attached?
                if (theInstance.netLimiterEnabled && NetLimiterClient._bridgeProcess == null)
                {
                    // This should reload the filters from NetLimiter and init the bridge process
                    thisServer.BanTab.NetLimiter_RefreshFilters(null!,null!);

                    // If this doesn't work, disable NetLimiter integration
                    if (NetLimiterClient._bridgeProcess == null)
                    {
                        theInstance.netLimiterEnabled = false;
                        thisServer.BanTab.checkBox_EnableNetLimiter.Checked = false;
                        ServerSettings.Set("NetLimiterEnabled", false);
                        MessageBox.Show("Failed to start NetLimiter bridge process. NetLimiter integration has been disabled.", "NetLimiter Integration Disabled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        AppDebug.Log("tickerBanManagement", "NetLimiter integration disabled due to failure to start bridge process.");
                    }                   

                }

                if (ServerMemory.ReadMemoryIsProcessAttached())
                {

                    // Check for Connections for NetLimiter if Enabled (throttled to every 10 seconds)
                    if (theInstance.netLimiterEnabled)
                    {
                        DateTime now = DateTime.Now;
    
                        // Only check if 10 seconds have passed since last check
                        if (now - _lastNetLimiterCheck >= _netLimiterCheckInterval)
                        {
                            _lastNetLimiterCheck = now;
        
                            // Run NetLimiter operations on background thread to avoid blocking UI
                            Task.Run(async () =>
                            {
                                try
                                {
                                    int appId = await NetLimiterClient.GetAppId(gameServerPath);
                                    if (appId != 0)
                                    {
                                        var connections = await NetLimiterClient.GetConnectionsAsync(appId);
    
                                        // Update UI with current connections analysis
                                        if (connections != null && connections.Count > 0)
                                        {
                                            SafeInvoke(thisServer, () =>
                                            {
                                                AnalyzeConnections(connections);
                                            });
                                        }
                                        else
                                        {
                                            SafeInvoke(thisServer, () =>
                                            {
                                                thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Clear();
                                            });
                                        }
                                    }
                                    else
                                    {
                                        SafeInvoke(thisServer, () =>
                                        {
                                            thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Clear();
                                        });
                                        AppDebug.Log("tickerBanManagement", "Failed to get NetLimiter App ID for game server process.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AppDebug.Log("tickerBanManagement", $"NetLimiter background operation error: {ex.Message}");
                                    SafeInvoke(thisServer, () =>
                                    {
                                        thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Clear();
                                    });
                                }
                            });
                        }

                        // Sync NetLimiter filter with ban/whitelist (throttled to once per minute)
                        if (!string.IsNullOrEmpty(theInstance.netLimiterFilterName) && 
                            now - _lastFilterSync >= _filterSyncInterval)
                        {
                            _lastFilterSync = now;

                            // Run filter sync on background thread
                            Task.Run(async () =>
                            {
                                try
                                {
                                    await SyncNetLimiterFilterAsync();
                                }
                                catch (Exception ex)
                                {
                                    AppDebug.Log("tickerBanManagement", $"NetLimiter filter sync error: {ex.Message}");
                                }
                            });
                        }
                    }

                    // Only check and punt bans if server is ONLINE
                    if (theInstance.instanceStatus == InstanceStatus.ONLINE)
                    {

                        // Basic Ban Checks (Ignore Whitelist)
                        CheckAndPuntBannedPlayers();

                        // Proxy/VPN/TOR and Geo-Blocking Checks
                        CheckAndPuntProxyViolations();

                        // Role Restriction Checks
                        CheckAndPuntDisabledRoles();

                    }
                }
                else
                {
                    AppDebug.Log("tickerBanManagement", "Server process is not attached. Ticker Skipping.");
                }

            });
        }

        // Sync NetLimiter filter with ban and whitelist
        private static async Task SyncNetLimiterFilterAsync()
        {
            string filterName = theInstance.netLimiterFilterName;
    
            if (string.IsNullOrEmpty(filterName))
            {
                AppDebug.Log("tickerBanManagement", "NetLimiter filter name not configured. Skipping filter sync.");
                return;
            }

            AppDebug.Log("tickerBanManagement", $"Starting NetLimiter filter sync for filter '{filterName}'");

            try
            {
                // Get current IPs in the NetLimiter filter
                var filterIPs = await NetLimiterClient.GetFilterIpAddressesAsync(filterName);
                var filterIPSet = new HashSet<string>(filterIPs);

                AppDebug.Log("tickerBanManagement", $"Found {filterIPSet.Count} IPs in NetLimiter filter");

                DateTime now = DateTime.Now;

                // Build dictionary of IPs with their subnet masks (excluding expired and whitelisted)
                var shouldBeBannedIPs = new Dictionary<string, int>(); // IP -> SubnetMask

                foreach (var bannedIP in banInstance.BannedPlayerIPs)
                {
                    // Skip information-only records
                    if (bannedIP.RecordType == banInstanceRecordType.Information)
                        continue;

                    // Skip expired temporary bans
                    if (bannedIP.RecordType == banInstanceRecordType.Temporary &&
                        bannedIP.ExpireDate.HasValue &&
                        now > bannedIP.ExpireDate.Value)
                        continue;

                    // Check if this IP is whitelisted
                    bool isWhitelisted = banInstance.WhitelistedIPs.Any(whitelistedIP =>
                    {
                        // Skip expired temporary whitelists
                        if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                            whitelistedIP.ExpireDate.HasValue &&
                            now > whitelistedIP.ExpireDate.Value)
                            return false;

                        return IsIPMatch(bannedIP.PlayerIP, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask);
                    });

                    // Only add to filter if not whitelisted
                    if (!isWhitelisted)
                    {
                        string ipString = bannedIP.PlayerIP.ToString();
                        int subnetMask = bannedIP.SubnetMask;

                        // Store IP with its subnet mask
                        if (!shouldBeBannedIPs.ContainsKey(ipString))
                        {
                            shouldBeBannedIPs.Add(ipString, subnetMask);
                        }
                    }
                }

                AppDebug.Log("tickerBanManagement", $"Calculated {shouldBeBannedIPs.Count} IPs that should be banned");

                // Find IPs to add (in ban list but not in filter)
                var ipsToAdd = shouldBeBannedIPs.Keys.Except(filterIPSet).ToList();

                // Find IPs to remove (in filter but not in ban list)
                var ipsToRemove = filterIPSet.Except(shouldBeBannedIPs.Keys).ToList();

                // Add missing IPs to filter
                int addedCount = 0;
                foreach (var ip in ipsToAdd)
                {
                    try
                    {
                        int subnet = shouldBeBannedIPs[ip];
                        bool added = await NetLimiterClient.AddIpToFilterAsync(filterName, ip, subnet);
                        if (added)
                        {
                            addedCount++;
                            AppDebug.Log("tickerBanManagement", $"Added IP {ip}/{subnet} to NetLimiter filter '{filterName}'");
                        }
                        else
                        {
                            AppDebug.Log("tickerBanManagement", $"Failed to add IP {ip}/{subnet} to NetLimiter filter '{filterName}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerBanManagement", $"Error adding IP {ip} to filter: {ex.Message}");
                    }
                }

                // Remove IPs that shouldn't be in filter
                int removedCount = 0;
                foreach (var ip in ipsToRemove)
                {
                    try
                    {
                        // Default to /32 for removal since we don't track the original subnet from the filter
                        bool removed = await NetLimiterClient.RemoveIpFromFilterAsync(filterName, ip, 32);
                        if (removed)
                        {
                            removedCount++;
                            AppDebug.Log("tickerBanManagement", $"Removed IP {ip} from NetLimiter filter '{filterName}'");
                        }
                        else
                        {
                            AppDebug.Log("tickerBanManagement", $"Failed to remove IP {ip} from NetLimiter filter '{filterName}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerBanManagement", $"Error removing IP {ip} from filter: {ex.Message}");
                    }
                }

                AppDebug.Log("tickerBanManagement", $"NetLimiter filter sync complete: Added {addedCount}, Removed {removedCount} IPs");
            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error during NetLimiter filter sync: {ex.Message}");
                throw;
            }
        }

        // Check active players against ban lists and punt if banned
        public static void CheckAndPuntBannedPlayers()
        {
            // Get current time for comparison
            DateTime now = DateTime.Now;
    
            // Cycle through all players in the player list
            foreach (var kvp in theInstance.playerList)
            {
                int slotNum = kvp.Key;
                playerObject player = kvp.Value;
        
                // Only check players who were seen in the last 6 seconds (active players)
                if ((now - player.PlayerLastSeen).TotalSeconds <= 4)
                {
                    bool isWhitelisted = false;
            
                    // First, check if player is on the whitelist (names)
                    foreach (var whitelistedName in banInstance.WhitelistedNames)
                    {
                        // Skip information-only records
                        if (whitelistedName.RecordType == banInstanceRecordType.Information)
                            continue;
                    
                        // Check if temporary whitelist has expired
                        if (whitelistedName.RecordType == banInstanceRecordType.Temporary && 
                            whitelistedName.ExpireDate.HasValue && 
                            now > whitelistedName.ExpireDate.Value)
                            continue;
                
                        // Check for name match (case-insensitive)
                        if (player.PlayerName.Equals(whitelistedName.PlayerName, StringComparison.OrdinalIgnoreCase))
                        {
                            isWhitelisted = true;
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is whitelisted by name. Skipping ban checks.");
                            break;
                        }
                    }
            
                    // If not whitelisted by name, check if whitelisted by IP
                    if (!isWhitelisted && !string.IsNullOrEmpty(player.PlayerIPAddress))
                    {
                        if (IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
                        {
                            foreach (var whitelistedIP in banInstance.WhitelistedIPs)
                            {
                                // Skip information-only records
                                if (whitelistedIP.RecordType == banInstanceRecordType.Information)
                                    continue;
                            
                                // Check if temporary whitelist has expired
                                if (whitelistedIP.RecordType == banInstanceRecordType.Temporary && 
                                    whitelistedIP.ExpireDate.HasValue && 
                                    now > whitelistedIP.ExpireDate.Value)
                                    continue;
                        
                                // Check for IP match (considering subnet mask)
                                if (IsIPMatch(playerIP, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask))
                                {
                                    isWhitelisted = true;
                                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is whitelisted by IP. Skipping ban checks.");
                                    break;
                                }
                            }
                        }
                    }
            
                    // Skip to next player if whitelisted
                    if (isWhitelisted)
                        continue;
            
                    bool shouldPunt = false;
                    string puntReason = string.Empty;
            
                    // Check against banned player names
                    foreach (var bannedName in banInstance.BannedPlayerNames)
                    {
                        // Skip information-only records
                        if (bannedName.RecordType == banInstanceRecordType.Information)
                            continue;
                    
                        // Check if temporary ban has expired
                        if (bannedName.RecordType == banInstanceRecordType.Temporary && 
                            bannedName.ExpireDate.HasValue && 
                            now > bannedName.ExpireDate.Value)
                            continue;
                
                        // Check for name match (case-insensitive)
                        if (player.PlayerName.Equals(bannedName.PlayerName, StringComparison.OrdinalIgnoreCase))
                        {
                            shouldPunt = true;
                            puntReason = $"Banned name: {bannedName.PlayerName}";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) matched banned name. Notes: {bannedName.Notes}");
                            break;
                        }
                    }
            
                    // If not already flagged, check against banned IPs
                    if (!shouldPunt && !string.IsNullOrEmpty(player.PlayerIPAddress))
                    {
                        if (IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
                        {
                            foreach (var bannedIP in banInstance.BannedPlayerIPs)
                            {
                                // Skip information-only records
                                if (bannedIP.RecordType == banInstanceRecordType.Information)
                                    continue;
                            
                                // Check if temporary ban has expired
                                if (bannedIP.RecordType == banInstanceRecordType.Temporary && 
                                    bannedIP.ExpireDate.HasValue && 
                                    now > bannedIP.ExpireDate.Value)
                                    continue;
                        
                                // Check for IP match (considering subnet mask)
                                if (IsIPMatch(playerIP, bannedIP.PlayerIP, bannedIP.SubnetMask))
                                {
                                    shouldPunt = true;
                                    puntReason = $"Banned IP: {bannedIP.PlayerIP}/{bannedIP.SubnetMask}";
                                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) matched banned IP. Notes: {bannedIP.Notes}");
                                    break;
                                }
                            }
                        }
                    }
            
                    // Punt the player if they're banned
                    if (shouldPunt)
                    {
                        ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                        AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                    }
                }
            }
        }

        // Helper method to check if an IP matches a banned IP with subnet mask
        private static bool IsIPMatch(IPAddress playerIP, IPAddress bannedIP, int subnetMask)
        {
            // If subnet mask is 32 (or 0 for exact match), do exact comparison
            if (subnetMask == 32 || subnetMask == 0)
            {
                return playerIP.Equals(bannedIP);
            }
    
            // Convert IPs to bytes for subnet comparison
            byte[] playerBytes = playerIP.GetAddressBytes();
            byte[] bannedBytes = bannedIP.GetAddressBytes();
    
            // Only compare IPv4 addresses
            if (playerBytes.Length != 4 || bannedBytes.Length != 4)
                return false;
    
            // Calculate how many bits to compare
            int bitsToCompare = subnetMask;
    
            for (int i = 0; i < 4 && bitsToCompare > 0; i++)
            {
                int mask = bitsToCompare >= 8 ? 0xFF : (0xFF << (8 - bitsToCompare)) & 0xFF;
        
                if ((playerBytes[i] & mask) != (bannedBytes[i] & mask))
                    return false;
            
                bitsToCompare -= 8;
            }
    
            return true;
        }

        // Analyze connections and populate DataGridView with proxy status and ban/whitelist info
        private static void AnalyzeConnections(List<ConnectionInfo> connections)
        {
            DateTime now = DateTime.Now;
    
            // Group connections by IP and count
            var ipGroups = connections
                .GroupBy(conn => conn.RemoteAddress)
                .OrderByDescending(g => g.Count())
                .ToList();

            // Clear existing rows
            thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Clear();

            int rowIndex = 0;
            foreach (var ipGroup in ipGroups)
            {
                string ip = ipGroup.Key!;
                int count = ipGroup.Count();
        
                // Check if connection limit enforcement is enabled and threshold exceeded
                if (theInstance.netLimiterEnableConLimit && count >= theInstance.netLimiterConThreshold)
                {
                    // Run ban and NetLimiter add operations asynchronously
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await BanIPForExcessiveConnections(ip, count);
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerBanManagement", $"Error banning IP {ip} for excessive connections: {ex.Message}");
                        }
                    });
                }
        
                // Check VPN/Proxy status
                string vpnStatus = GetVpnStatus(ip);
        
                // Check whitelist/blacklist status
                string listStatus = GetListStatus(ip, now);
        
                // Add row to DataGridView: RowID, IP Address, # Cons, VPN Status, Notes
                thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Add(
                    rowIndex++,  // NL_rowID
                    ip,          // NL_ipAddress
                    count,       // NL_numCons
                    vpnStatus,   // NL_vpnStatus
                    listStatus   // NL_notes
                );
            }
        }

        // Ban IP for excessive connections and add to NetLimiter filter
        private static async Task BanIPForExcessiveConnections(string ipAddress, int connectionCount)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
            {
                AppDebug.Log("tickerBanManagement", $"Invalid IP address format: {ipAddress}");
                return;
            }

            DateTime now = DateTime.Now;

            // Check if IP is already banned
            bool alreadyBanned = banInstance.BannedPlayerIPs.Any(bannedIP =>
            {
                // Skip expired temporary bans
                if (bannedIP.RecordType == banInstanceRecordType.Temporary &&
                    bannedIP.ExpireDate.HasValue &&
                    now > bannedIP.ExpireDate.Value)
                    return false;

                return IsIPMatch(ip, bannedIP.PlayerIP, bannedIP.SubnetMask);
            });

            if (alreadyBanned)
            {
                AppDebug.Log("tickerBanManagement", $"IP {ipAddress} is already banned. Skipping.");
                return;
            }

            // Check if IP is whitelisted
            bool isWhitelisted = banInstance.WhitelistedIPs.Any(whitelistedIP =>
            {
                // Skip expired temporary whitelists
                if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                    whitelistedIP.ExpireDate.HasValue &&
                    now > whitelistedIP.ExpireDate.Value)
                    return false;

                return IsIPMatch(ip, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask);
            });

            if (isWhitelisted)
            {
                AppDebug.Log("tickerBanManagement", $"IP {ipAddress} is whitelisted. Skipping ban for excessive connections.");
                return;
            }

            // Create ban record
            var banRecord = new banInstancePlayerIP
            {
                RecordID = 0, // Will be set by database
                MatchID = theInstance.gameMatchID,
                PlayerIP = ip,
                SubnetMask = 32, // Exact IP match
                Date = now,
                ExpireDate = null, // Permanent ban
                AssociatedName = null,
                RecordType = banInstanceRecordType.Permanent,
                RecordCategory = 0, // Ban
                Notes = $"Auto-banned: Excessive connections ({connectionCount}) exceeded threshold ({theInstance.netLimiterConThreshold})"
            };

            try
            {
                // Add to database
                int recordId = DatabaseManager.AddPlayerIPRecord(banRecord);
                banRecord.RecordID = recordId;

                // Add to in-memory ban list
                banInstance.BannedPlayerIPs.Add(banRecord);

                AppDebug.Log("tickerBanManagement", $"Banned IP {ipAddress} for excessive connections: {connectionCount} connections (threshold: {theInstance.netLimiterConThreshold})");

                // Add to NetLimiter filter if filter name is configured
                if (!string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                {
                    bool addedToFilter = await NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress, 32);
                    
                    if (addedToFilter)
                    {
                        AppDebug.Log("tickerBanManagement", $"Successfully added IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'");
                    }
                    else
                    {
                        AppDebug.Log("tickerBanManagement", $"Failed to add IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'");
                    }
                }
                else
                {
                    AppDebug.Log("tickerBanManagement", $"NetLimiter filter name not configured. IP {ipAddress} not added to filter.");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error adding ban record for IP {ipAddress}: {ex.Message}");
                throw;
            }
        }

        private static void CheckIP(IPAddress Address)
        {
            if (theInstance.proxyCheckEnabled && ProxyCheckManager.IsInitialized)
            {
                try { 
                    AppDebug.Log("CheckIP", $"Attempting to check: {Address.ToString()}");
                    _ = ProxyCheckManager.CheckIPAsync(Address);
                } catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error checking IP {Address} with ProxyCheckManager: {ex.Message}");
                }
            }
        }

        // Get VPN/Proxy status from proxy records
        private static string GetVpnStatus(string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
                return "INVALID IP";
    
            // Find matching proxy record
            var proxyRecord = banInstance.ProxyRecords
                .FirstOrDefault(p => p.IPAddress.Equals(ip));
    
            if (proxyRecord == null)
            {
                CheckIP(ip);
                return "UNCHECKED";
            }
    
            // Check if cache is expired
            if (DateTime.Now > proxyRecord.CacheExpiry)
            {
                CheckIP(ip);
                return "CACHE EXPIRED";
            }
    
            // Build status string
            var statuses = new List<string>();
    
            if (proxyRecord.IsVpn)
                statuses.Add("VPN");
            if (proxyRecord.IsProxy)
                statuses.Add("PROXY");
            if (proxyRecord.IsTor)
                statuses.Add("TOR");
    
            if (statuses.Count > 0)
            {
                string status = string.Join("/", statuses);
                if (proxyRecord.RiskScore > 0)
                    status += $" (Risk:{proxyRecord.RiskScore})";
                if (!string.IsNullOrEmpty(proxyRecord.Provider))
                    status += $" [{proxyRecord.Provider}]";
                return status;
            }
    
            return "CLEAN";
        }

        // Get whitelist/blacklist status
        private static string GetListStatus(string ipAddress, DateTime now)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
                return "ERROR";
    
            var comments = new List<string>();
    
            // Check whitelist
            foreach (var whitelistedIP in banInstance.WhitelistedIPs)
            {
                // Skip expired temporary entries
                if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                    whitelistedIP.ExpireDate.HasValue &&
                    now > whitelistedIP.ExpireDate.Value)
                    continue;
        
                if (IsIPMatch(ip, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask))
                {
                    string wlType = whitelistedIP.RecordType == banInstanceRecordType.Permanent ? "WHITELIST" : "WHITELIST (TEMP)";
                    if (!string.IsNullOrEmpty(whitelistedIP.Notes))
                        wlType += $": {whitelistedIP.Notes}";
                    comments.Add(wlType);
                    break; // Only show first match
                }
            }
    
            // Check blacklist (banned IPs)
            foreach (var bannedIP in banInstance.BannedPlayerIPs)
            {
                // Skip information-only records
                if (bannedIP.RecordType == banInstanceRecordType.Information)
                    continue;
        
                // Skip expired temporary bans
                if (bannedIP.RecordType == banInstanceRecordType.Temporary &&
                    bannedIP.ExpireDate.HasValue &&
                    now > bannedIP.ExpireDate.Value)
                    continue;
        
                if (IsIPMatch(ip, bannedIP.PlayerIP, bannedIP.SubnetMask))
                {
                    string banType = bannedIP.RecordType == banInstanceRecordType.Permanent ? "BANNED" : "BANNED (TEMP)";
                    if (!string.IsNullOrEmpty(bannedIP.Notes))
                        banType += $": {bannedIP.Notes}";
                    comments.Add(banType);
                    break; // Only show first match
                }
            }
    
            // Check blocked countries
            var proxyRecord = banInstance.ProxyRecords
                .FirstOrDefault(p => p.IPAddress.Equals(ip));
    
            if (proxyRecord != null && !string.IsNullOrEmpty(proxyRecord.CountryCode))
            {
                var blockedCountry = banInstance.ProxyBlockedCountries
                    .FirstOrDefault(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
        
                if (blockedCountry != null)
                {
                    comments.Add($"BLOCKED COUNTRY: {blockedCountry.CountryName}");
                }
                else if (!string.IsNullOrEmpty(proxyRecord.CountryCode))
                {
                    comments.Add($"Country: {proxyRecord.CountryCode}");
                }
            }
    
            return comments.Count > 0 ? string.Join(" | ", comments) : "OK";
        }

                // Check active players against proxy/VPN/TOR rules and geo-blocking
        public static void CheckAndPuntProxyViolations()
        {
            // Only run if proxy checking is enabled
            if (!theInstance.proxyCheckEnabled)
                return;

            DateTime now = DateTime.Now;

            // Cycle through all players in the player list
            foreach (var kvp in theInstance.playerList)
            {
                int slotNum = kvp.Key;
                playerObject player = kvp.Value;

                // Only check players who were seen in the last 4 seconds (active players)
                if ((now - player.PlayerLastSeen).TotalSeconds > 4)
                    continue;

                // Skip if player IP is not available
                if (string.IsNullOrEmpty(player.PlayerIPAddress))
                    continue;

                if (!IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
                    continue;

                // Check if player is whitelisted (skip proxy checks for whitelisted players)
                bool isWhitelisted = false;

                // Check name whitelist
                foreach (var whitelistedName in banInstance.WhitelistedNames)
                {
                    if (whitelistedName.RecordType == banInstanceRecordType.Information)
                        continue;

                    if (whitelistedName.RecordType == banInstanceRecordType.Temporary &&
                        whitelistedName.ExpireDate.HasValue &&
                        now > whitelistedName.ExpireDate.Value)
                        continue;

                    if (player.PlayerName.Equals(whitelistedName.PlayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        isWhitelisted = true;
                        break;
                    }
                }

                // Check IP whitelist
                if (!isWhitelisted)
                {
                    foreach (var whitelistedIP in banInstance.WhitelistedIPs)
                    {
                        if (whitelistedIP.RecordType == banInstanceRecordType.Information)
                            continue;

                        if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                            whitelistedIP.ExpireDate.HasValue &&
                            now > whitelistedIP.ExpireDate.Value)
                            continue;

                        if (IsIPMatch(playerIP, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask))
                        {
                            isWhitelisted = true;
                            break;
                        }
                    }
                }

                // Skip whitelisted players
                if (isWhitelisted)
                    continue;

                // Find proxy record for this player's IP
                var proxyRecord = banInstance.ProxyRecords
                    .FirstOrDefault(p => p.IPAddress.Equals(playerIP));

                // If no proxy record exists or cache expired, trigger a check and skip for now
                if (proxyRecord == null || now > proxyRecord.CacheExpiry)
                {
                    CheckIP(playerIP);
                    continue;
                }

                bool shouldPunt = false;
                bool shouldBan = false;
                string puntReason = string.Empty;

                // Check Proxy
                if (proxyRecord.IsProxy && theInstance.proxyCheckProxyAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckProxyAction == 2;
                    puntReason = $"Proxy detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a PROXY. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                // Check VPN
                if (!shouldPunt && proxyRecord.IsVpn && theInstance.proxyCheckVPNAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckVPNAction == 2;
                    puntReason = $"VPN detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    if (!string.IsNullOrEmpty(proxyRecord.Provider))
                        puntReason += $" - {proxyRecord.Provider}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a VPN. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                // Check TOR
                if (!shouldPunt && proxyRecord.IsTor && theInstance.proxyCheckTORAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckTORAction == 2;
                    puntReason = $"TOR detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using TOR. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                // Check Geo-Blocking
                if (!shouldPunt && theInstance.proxyCheckGeoMode > 0 && !string.IsNullOrEmpty(proxyRecord.CountryCode))
                {
                    bool countryInList = banInstance.ProxyBlockedCountries
                        .Any(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));

                    // Mode 1 = Block listed countries
                    // Mode 2 = Allow only listed countries
                    bool shouldBlockCountry = theInstance.proxyCheckGeoMode == 1 ? countryInList : !countryInList;

                    if (shouldBlockCountry)
                    {
                        shouldPunt = true;
                        shouldBan = false; // Geo-blocking only kicks, doesn't auto-ban
                        var country = banInstance.ProxyBlockedCountries
                            .FirstOrDefault(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                        string countryName = country?.CountryName ?? proxyRecord.CountryCode;
                        puntReason = $"Geo-blocked: {countryName} ({proxyRecord.CountryCode})";
                        AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) from blocked country: {countryName}. Action: Kick");
                    }
                }

                // Execute action
                if (shouldPunt)
                {
                    if (shouldBan)
                    {
                        // Create ban record
                        var banRecord = new banInstancePlayerIP
                        {
                            RecordID = 0, // Will be set by database
                            MatchID = theInstance.gameMatchID,
                            PlayerIP = playerIP,
                            SubnetMask = 32, // Exact IP match
                            Date = now,
                            ExpireDate = null, // Permanent ban
                            AssociatedName = null,
                            RecordType = banInstanceRecordType.Permanent,
                            RecordCategory = 0, // Ban
                            Notes = puntReason
                        };

                        try
                        {
                            // Add to database
                            int recordId = DatabaseManager.AddPlayerIPRecord(banRecord);
                            banRecord.RecordID = recordId;

                            // Add to in-memory ban list
                            banInstance.BannedPlayerIPs.Add(banRecord);

                            AppDebug.Log("tickerBanManagement", $"Auto-banned IP {player.PlayerIPAddress}: {puntReason}");

                            // Add to NetLimiter filter if enabled
                            if (!string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                            {
                                _ = NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, player.PlayerIPAddress, 32);
                            }
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerBanManagement", $"Error adding auto-ban record for IP {player.PlayerIPAddress}: {ex.Message}");
                        }
                    }

                    // Punt the player
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
            }
        }

        // Check active players against role restrictions and punt if using disabled role
        public static void CheckAndPuntDisabledRoles()
        {
            DateTime now = DateTime.Now;

            // Cycle through all players in the player list
            foreach (var kvp in theInstance.playerList)
            {
                int slotNum = kvp.Key;
                playerObject player = kvp.Value;

                // Only check players who were seen in the last 4 seconds (active players)
                if ((now - player.PlayerLastSeen).TotalSeconds > 4)
                    continue;

                bool shouldPunt = false;
                string puntReason = string.Empty;

                // Check player's role against disabled roles
                switch (player.RoleID)
                {
                    case (int)CharacterClass.CQB:
                    case (int)CharacterClass.SAS_CQB:
                        if (!theInstance.roleCQB)
                        {
                            shouldPunt = true;
                            puntReason = "CQB role is disabled on this server";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is using disabled CQB role.");
                        }
                        break;

                    case (int)CharacterClass.MEDIC:
                    case (int)CharacterClass.SAS_MEDIC:
                        if (!theInstance.roleMedic)
                        {
                            shouldPunt = true;
                            puntReason = "Medic role is disabled on this server";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is using disabled Medic role.");
                        }
                        break;

                    case (int)CharacterClass.SNIPER:
                    case (int)CharacterClass.SAS_SNIPER:
                        if (!theInstance.roleSniper)
                        {
                            shouldPunt = true;
                            puntReason = "Sniper role is disabled on this server";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is using disabled Sniper role.");
                        }
                        break;

                    case (int)CharacterClass.GUNNER:
                    case (int)CharacterClass.SAS_GUNNER:
                        if (!theInstance.roleGunner)
                        {
                            shouldPunt = true;
                            puntReason = "Gunner role is disabled on this server";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is using disabled Gunner role.");
                        }
                        break;
                }

                // Punt the player if they're using a disabled role
                if (shouldPunt)
                {
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
            }
        }

    }
}