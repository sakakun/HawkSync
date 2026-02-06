using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System.Net;
using HawkSyncShared.DTOs.tabPlayers;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

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
                    thisServer.BanTab.NetLimiter_RefreshFilters(null!, null!);

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

        // Helper class for IP range math
        private class IpRange
        {
            public IPAddress Start { get; }
            public IPAddress End { get; }

            public IpRange(IPAddress start, IPAddress end)
            {
                Start = start;
                End = end;
            }

            public static IpRange FromCidr(IPAddress baseAddress, int subnetMask)
            {
                uint ip = IpToUint(baseAddress);
                uint mask = subnetMask == 0 ? 0 : 0xFFFFFFFF << (32 - subnetMask);
                uint start = ip & mask;
                uint end = start | ~mask;
                return new IpRange(UintToIp(start), UintToIp(end));
            }

            public List<IpRange> Subtract(IpRange other)
            {
                var result = new List<IpRange>();
                uint s1 = IpToUint(Start), e1 = IpToUint(End);
                uint s2 = IpToUint(other.Start), e2 = IpToUint(other.End);

                if (e2 < s1 || s2 > e1)
                {
                    // No overlap
                    result.Add(this);
                    return result;
                }
                if (s2 > s1)
                    result.Add(new IpRange(UintToIp(s1), UintToIp(s2 - 1)));
                if (e2 < e1)
                    result.Add(new IpRange(UintToIp(e2 + 1), UintToIp(e1)));
                return result;
            }

            public static uint IpToUint(IPAddress ip)
            {
                var bytes = ip.GetAddressBytes();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return BitConverter.ToUInt32(bytes, 0);
            }

            public static IPAddress UintToIp(uint ip)
            {
                var bytes = BitConverter.GetBytes(ip);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return new IPAddress(bytes);
            }

            public override string ToString()
            {
                return Start.Equals(End) ? Start.ToString() : $"{Start}-{End}";
            }
        }

        // Sync NetLimiter filter with ban and whitelist, subtracting whitelisted ranges from banned ranges
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

                // Build list of banned ranges
                var bannedRanges = new List<IpRange>();
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

                    bannedRanges.Add(IpRange.FromCidr(bannedIP.PlayerIP, bannedIP.SubnetMask));
                }

                // Build list of whitelisted ranges
                var whitelistedRanges = new List<IpRange>();
                foreach (var whitelistedIP in banInstance.WhitelistedIPs)
                {
                    // Skip expired temporary whitelists
                    if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                        whitelistedIP.ExpireDate.HasValue &&
                        now > whitelistedIP.ExpireDate.Value)
                        continue;

                    whitelistedRanges.Add(IpRange.FromCidr(whitelistedIP.PlayerIP, whitelistedIP.SubnetMask));
                }

                // Subtract whitelisted ranges from banned ranges
                var finalBannedRanges = new List<IpRange>();
                foreach (var banned in bannedRanges)
                {
                    var toProcess = new List<IpRange> { banned };
                    foreach (var white in whitelistedRanges)
                    {
                        var next = new List<IpRange>();
                        foreach (var range in toProcess)
                            next.AddRange(range.Subtract(white));
                        toProcess = next;
                        if (toProcess.Count == 0) break;
                    }
                    finalBannedRanges.AddRange(toProcess);
                }

                AppDebug.Log("tickerBanManagement", $"Calculated {finalBannedRanges.Count} IP ranges that should be banned (after whitelist subtraction)");

                // Build set of IPs/ranges to add to filter
                var shouldBeBannedIPs = new HashSet<string>();
                foreach (var range in finalBannedRanges)
                {
                    shouldBeBannedIPs.Add(range.ToString());
                }

                // Find IPs/ranges to add (in ban list but not in filter)
                var ipsToAdd = shouldBeBannedIPs.Except(filterIPSet).ToList();

                // Find IPs/ranges to remove (in filter but not in ban list)
                var ipsToRemove = filterIPSet.Except(shouldBeBannedIPs).ToList();

                // Add missing IPs/ranges to filter
                int addedCount = 0;
                foreach (var ipRangeStr in ipsToAdd)
                {
                    try
                    {
                        if (ipRangeStr.Contains("-"))
                        {
                            // Range, add each IP (could be optimized to CIDR blocks if NetLimiter supports)
                            var parts = ipRangeStr.Split('-');
                            var start = IPAddress.Parse(parts[0]);
                            var end = IPAddress.Parse(parts[1]);
                            uint s = IpRange.IpToUint(start);
                            uint e = IpRange.IpToUint(end);
                            for (uint ip = s; ip <= e; ip++)
                            {
                                var ipStr = IpRange.UintToIp(ip).ToString();
                                bool added = await NetLimiterClient.AddIpToFilterAsync(filterName, ipStr, 32);
                                if (added) addedCount++;
                            }
                        }
                        else
                        {
                            // Single IP
                            bool added = await NetLimiterClient.AddIpToFilterAsync(filterName, ipRangeStr, 32);
                            if (added) addedCount++;
                        }
                        AppDebug.Log("tickerBanManagement", $"Added IP/range {ipRangeStr} to NetLimiter filter '{filterName}'");
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerBanManagement", $"Error adding IP/range {ipRangeStr} to filter: {ex.Message}");
                    }
                }

                // Remove IPs/ranges that shouldn't be in filter
                int removedCount = 0;
                foreach (var ipRangeStr in ipsToRemove)
                {
                    try
                    {
                        if (ipRangeStr.Contains("-"))
                        {
                            var parts = ipRangeStr.Split('-');
                            var start = IPAddress.Parse(parts[0]);
                            var end = IPAddress.Parse(parts[1]);
                            uint s = IpRange.IpToUint(start);
                            uint e = IpRange.IpToUint(end);
                            for (uint ip = s; ip <= e; ip++)
                            {
                                var ipStr = IpRange.UintToIp(ip).ToString();
                                bool removed = await NetLimiterClient.RemoveIpFromFilterAsync(filterName, ipStr, 32);
                                if (removed) removedCount++;
                            }
                        }
                        else
                        {
                            bool removed = await NetLimiterClient.RemoveIpFromFilterAsync(filterName, ipRangeStr, 32);
                            if (removed) removedCount++;
                        }
                        AppDebug.Log("tickerBanManagement", $"Removed IP/range {ipRangeStr} from NetLimiter filter '{filterName}'");
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerBanManagement", $"Error removing IP/range {ipRangeStr} from filter: {ex.Message}");
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
            DateTime now = DateTime.Now;

            foreach (var kvp in playerInstance.PlayerList)
            {
                int slotNum = kvp.Key;
                PlayerObject player = kvp.Value;

                // Only check players who were seen in the last 6 seconds (active players)
                if ((now - player.PlayerLastSeen).TotalSeconds <= 4)
                {
                    bool isWhitelisted = false;

                    // First, check if player is on the whitelist (names)
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
                                if (whitelistedIP.RecordType == banInstanceRecordType.Information)
                                    continue;

                                if (whitelistedIP.RecordType == banInstanceRecordType.Temporary &&
                                    whitelistedIP.ExpireDate.HasValue &&
                                    now > whitelistedIP.ExpireDate.Value)
                                    continue;

                                if (IsIPMatch(playerIP, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask))
                                {
                                    isWhitelisted = true;
                                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is whitelisted by IP. Skipping ban checks.");
                                    break;
                                }
                            }
                        }
                    }

                    if (isWhitelisted)
                        continue;

                    bool shouldPunt = false;
                    string puntReason = string.Empty;

                    // Check against banned player names
                    foreach (var bannedName in banInstance.BannedPlayerNames)
                    {
                        if (bannedName.RecordType == banInstanceRecordType.Information)
                            continue;

                        if (bannedName.RecordType == banInstanceRecordType.Temporary &&
                            bannedName.ExpireDate.HasValue &&
                            now > bannedName.ExpireDate.Value)
                            continue;

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
                                if (bannedIP.RecordType == banInstanceRecordType.Information)
                                    continue;

                                if (bannedIP.RecordType == banInstanceRecordType.Temporary &&
                                    bannedIP.ExpireDate.HasValue &&
                                    now > bannedIP.ExpireDate.Value)
                                    continue;

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

                    if (shouldPunt)
                    {
                        if (player.PlayerPing <= 0)
                        {
                            return;  // Player not in game yet. Needless to spam the "punt".
                        }

                        ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                        AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                    }
                }
            }
        }

        // Helper method to check if an IP matches a banned IP with subnet mask
        private static bool IsIPMatch(IPAddress playerIP, IPAddress bannedIP, int subnetMask)
        {
            if (subnetMask == 32 || subnetMask == 0)
            {
                return playerIP.Equals(bannedIP);
            }

            byte[] playerBytes = playerIP.GetAddressBytes();
            byte[] bannedBytes = bannedIP.GetAddressBytes();

            if (playerBytes.Length != 4 || bannedBytes.Length != 4)
                return false;

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

            var ipGroups = connections
                .GroupBy(conn => conn.RemoteAddress)
                .OrderByDescending(g => g.Count())
                .ToList();

            thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Clear();
            banInstance.NetLimiterConnectionLogs.Clear();

            int rowIndex = 0;
            foreach (var ipGroup in ipGroups)
            {
                string ip = ipGroup.Key!;
                int count = ipGroup.Count();

                if (theInstance.netLimiterEnableConLimit && count >= theInstance.netLimiterConThreshold)
                {
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

                string vpnStatus = GetVpnStatus(ip);
                string listStatus = GetListStatus(ip, now);

                int recordID = rowIndex++;

                banInstance.NetLimiterConnectionLogs.Add(new netLimiterConnLogEntry
                {
                    NL_rowID = recordID,
                    NL_ipAddress = ip,
                    NL_numCons = count,
                    NL_vpnStatus = vpnStatus,
                    NL_notes = listStatus
                });

                thisServer.BanTab.dg_NetlimiterConnectionLog.Rows.Add(
                    recordID,
                    ip,
                    count,
                    vpnStatus,
                    listStatus
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

            bool alreadyBanned = banInstance.BannedPlayerIPs.Any(bannedIP =>
            {
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

            bool isWhitelisted = banInstance.WhitelistedIPs.Any(whitelistedIP =>
            {
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

            var banRecord = new banInstancePlayerIP
            {
                RecordID = 0,
                MatchID = theInstance.gameMatchID,
                PlayerIP = ip,
                SubnetMask = 32,
                Date = now,
                ExpireDate = null,
                AssociatedName = null,
                RecordType = banInstanceRecordType.Permanent,
                RecordCategory = 0,
                Notes = $"Auto-banned: Excessive connections ({connectionCount}) exceeded threshold ({theInstance.netLimiterConThreshold})"
            };

            try
            {
                int recordId = DatabaseManager.AddPlayerIPRecord(banRecord);
                banRecord.RecordID = recordId;

                banInstance.BannedPlayerIPs.Add(banRecord);

                AppDebug.Log("tickerBanManagement", $"Banned IP {ipAddress} for excessive connections: {connectionCount} connections (threshold: {theInstance.netLimiterConThreshold})");

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
                try
                {
                    AppDebug.Log("CheckIP", $"Attempting to check: {Address.ToString()}");
                    _ = ProxyCheckManager.CheckIPAsync(Address);
                }
                catch (Exception ex)
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

            var proxyRecord = banInstance.ProxyRecords
                .FirstOrDefault(p => p.IPAddress.Equals(ip));

            if (proxyRecord == null)
            {
                CheckIP(ip);
                return "UNCHECKED";
            }

            if (DateTime.Now > proxyRecord.CacheExpiry)
            {
                CheckIP(ip);
                return "CACHE EXPIRED";
            }

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

            foreach (var whitelistedIP in banInstance.WhitelistedIPs)
            {
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
                    break;
                }
            }

            foreach (var bannedIP in banInstance.BannedPlayerIPs)
            {
                if (bannedIP.RecordType == banInstanceRecordType.Information)
                    continue;

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
                    break;
                }
            }

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
            if (!theInstance.proxyCheckEnabled)
                return;

            DateTime now = DateTime.Now;

            foreach (var kvp in playerInstance.PlayerList)
            {
                int slotNum = kvp.Key;
                PlayerObject player = kvp.Value;

                if ((now - player.PlayerLastSeen).TotalSeconds > 4)
                    continue;

                if (string.IsNullOrEmpty(player.PlayerIPAddress))
                    continue;

                if (!IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
                    continue;

                bool isWhitelisted = false;

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

                if (isWhitelisted)
                    continue;

                var proxyRecord = banInstance.ProxyRecords
                    .FirstOrDefault(p => p.IPAddress.Equals(playerIP));

                if (proxyRecord == null || now > proxyRecord.CacheExpiry)
                {
                    CheckIP(playerIP);
                    continue;
                }

                bool shouldPunt = false;
                bool shouldBan = false;
                string puntReason = string.Empty;

                if (proxyRecord.IsProxy && theInstance.proxyCheckProxyAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckProxyAction == 2;
                    puntReason = $"Proxy detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a PROXY. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                if (!shouldPunt && proxyRecord.IsVpn && theInstance.proxyCheckVPNAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckVPNAction == 2;
                    puntReason = $"VPN detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    if (!string.IsNullOrEmpty(proxyRecord.Provider))
                        puntReason += $" - {proxyRecord.Provider}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a VPN. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                if (!shouldPunt && proxyRecord.IsTor && theInstance.proxyCheckTORAction > 0)
                {
                    shouldPunt = true;
                    shouldBan = theInstance.proxyCheckTORAction == 2;
                    puntReason = $"TOR detected{(shouldBan ? " (Auto-banned)" : " (Kicked)")}";
                    AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using TOR. Action: {(shouldBan ? "Ban" : "Kick")}");
                }

                if (!shouldPunt && theInstance.proxyCheckGeoMode > 0 && !string.IsNullOrEmpty(proxyRecord.CountryCode))
                {
                    bool countryInList = banInstance.ProxyBlockedCountries
                        .Any(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));

                    bool shouldBlockCountry = theInstance.proxyCheckGeoMode == 1 ? countryInList : !countryInList;

                    if (shouldBlockCountry)
                    {
                        shouldPunt = true;
                        shouldBan = false;
                        var country = banInstance.ProxyBlockedCountries
                            .FirstOrDefault(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                        string countryName = country?.CountryName ?? proxyRecord.CountryCode;
                        puntReason = $"Geo-blocked: {countryName} ({proxyRecord.CountryCode})";
                        AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) from blocked country: {countryName}. Action: Kick");
                    }
                }

                if (shouldPunt)
                {
                    if (shouldBan)
                    {
                        var banRecord = new banInstancePlayerIP
                        {
                            RecordID = 0,
                            MatchID = theInstance.gameMatchID,
                            PlayerIP = playerIP,
                            SubnetMask = 32,
                            Date = now,
                            ExpireDate = null,
                            AssociatedName = null,
                            RecordType = banInstanceRecordType.Permanent,
                            RecordCategory = 0,
                            Notes = puntReason
                        };

                        try
                        {
                            int recordId = DatabaseManager.AddPlayerIPRecord(banRecord);
                            banRecord.RecordID = recordId;

                            banInstance.BannedPlayerIPs.Add(banRecord);

                            AppDebug.Log("tickerBanManagement", $"Auto-banned IP {player.PlayerIPAddress}: {puntReason}");

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

                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
            }
        }

        // Check active players against role restrictions and punt if using disabled role
        public static void CheckAndPuntDisabledRoles()
        {
            DateTime now = DateTime.Now;

            foreach (var kvp in playerInstance.PlayerList)
            {
                int slotNum = kvp.Key;
                PlayerObject player = kvp.Value;

                if ((now - player.PlayerLastSeen).TotalSeconds > 4)
                    continue;

                bool shouldPunt = false;
                string puntReason = string.Empty;

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

                if (shouldPunt)
                {
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{player.PlayerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
            }
        }
    }
}