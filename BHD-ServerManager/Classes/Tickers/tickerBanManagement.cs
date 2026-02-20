using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.HelperClasses;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.SupportClasses.Networking;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

        private static string gameServerPath { get; set; } = string.Empty;

        // Throttle for NetLimiter filter sync (once per minute)
        private static DateTime _lastFilterSync = DateTime.MinValue;
        private static readonly TimeSpan _filterSyncInterval = TimeSpan.FromMinutes(10);

        // Constant for active player threshold (seconds)
        private const int ActivePlayerThresholdSeconds = 4;

        // Locks
        private static bool _netLimiterLock = false;
        private static bool _netLimiterFilterLock = false;

        // Helper: Enumerate active players (seen within threshold)
        private static IEnumerable<(int SlotNum, PlayerObject Player)> GetActivePlayers()
        {
            DateTime now = DateTime.Now;
            return playerInstance.PlayerList
                .Where(kvp => (now - kvp.Value.PlayerLastSeen).TotalSeconds <= ActivePlayerThresholdSeconds)
                .Select(kvp => (kvp.Key, kvp.Value));
        }

        // Helper: Punt players and log
        private static void PuntPlayers(IEnumerable<(int SlotNum, string PlayerName, string PuntReason)> slotsToPunt)
        {
            foreach (var (slotNum, playerName, puntReason) in slotsToPunt)
            {
                try
                {
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{playerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error punting player '{playerName}' (Slot {slotNum}): {ex.Message}");
                }
            }
        }

        // Helper: Add ban record for IP
        private static banInstancePlayerIP AddBanRecord(IPAddress ip, string notes)
        {
            var banRecord = new banInstancePlayerIP
            {
                RecordID = 0,
                MatchID = theInstance.gameMatchID,
                PlayerIP = ip,
                SubnetMask = 32,
                Date = DateTime.Now,
                ExpireDate = null,
                AssociatedName = null,
                RecordType = banInstanceRecordType.Permanent,
                RecordCategory = 0,
                Notes = notes
            };
            banRecord.RecordID = DatabaseManager.AddPlayerIPRecord(banRecord);
            banInstance.BannedPlayerIPs.Add(banRecord);
            banInstance.ForceUIUpdates = true;
            return banRecord;
        }

        // Helper: Add IP to NetLimiter filter
        private static async Task<bool> AddIpToNetLimiterFilter(string ipAddress)
        {
            if (!string.IsNullOrEmpty(theInstance.netLimiterFilterName))
                return await NetLimiterClient.AddIpToFilterAsync(theInstance.netLimiterFilterName, ipAddress, 32);
            return false;
        }

        // Helper: Evaluate proxy/VPN/TOR/Geo ban logic for an IP
        private static (bool ShouldBan, string BanReason) EvaluateProxyBan(IPAddress ipAddress)
        {
            DateTime now = DateTime.Now;
            var proxyRecord = banInstance.ProxyRecords.FirstOrDefault(p => p.IPAddress.Equals(ipAddress));
            if (proxyRecord == null || now > proxyRecord.CacheExpiry)
            {
                BanHelper.CheckIP(ipAddress);
                proxyRecord = banInstance.ProxyRecords.FirstOrDefault(p => p.IPAddress.Equals(ipAddress));
            }
            if (proxyRecord == null) return (false, string.Empty);

            if (proxyRecord.IsProxy && theInstance.proxyCheckProxyAction == 2)
                return (true, "Proxy detected (Auto-banned)");
            if (proxyRecord.IsVpn && theInstance.proxyCheckVPNAction == 2)
                return (true, $"VPN detected (Auto-banned){(!string.IsNullOrEmpty(proxyRecord.Provider) ? $" - {proxyRecord.Provider}" : "")}");
            if (proxyRecord.IsTor && theInstance.proxyCheckTORAction == 2)
                return (true, "TOR detected (Auto-banned)");
            if (theInstance.proxyCheckGeoMode > 0 && !string.IsNullOrEmpty(proxyRecord.CountryCode))
            {
                bool countryInList = banInstance.ProxyBlockedCountries
                    .Any(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                bool shouldBlockCountry = theInstance.proxyCheckGeoMode == 1 ? countryInList : !countryInList;
                if (shouldBlockCountry)
                {
                    var country = banInstance.ProxyBlockedCountries
                        .FirstOrDefault(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                    string countryName = country?.CountryName ?? proxyRecord.CountryCode;
                    return (true, $"Geo-blocked: {countryName} ({proxyRecord.CountryCode})");
                }
            }
            return (false, string.Empty);
        }

        // Helper: Ban and filter an IP
        private static async Task BanAndFilterIp(IPAddress ip, string reason, string ipString)
        {
            AddBanRecord(ip, reason);
            await AddIpToNetLimiterFilter(ipString);
            AppDebug.Log("tickerBanManagement", $"Auto-banned IP {ipString}: {reason}");
        }

        // This method runs the ticker for ban management tasks.
        public static void runTicker()
        {
            gameServerPath = Path.Combine(theInstance.profileServerPath, "dfbhd.exe");

            // If the game server is offline, don't do anything.
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                AppDebug.Log("tickerBanManagement", "Game Server is Offline");
                return;
            }
            
            if (theInstance.netLimiterEnabled)
            {
                // Run NetlimiterTask as a fire-and-forget task, ignore CS4014 warning
			    Task.Run(NetlimiterTask);
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

        public static async Task NetlimiterTask()
        {
            if (_netLimiterLock)
                return;

            try
            {
                _netLimiterLock = true;
                int appId = await NetLimiterClient.GetAppId(gameServerPath);
                var connections = await NetLimiterClient.GetConnectionsAsync(appId);

                if (connections != null && connections.Count > 0)
                {
                    AnalyzeConnections(connections);
                    await SyncNetLimiterFilterAsync();
                }

            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error acquiring NetLimiter lock: {ex.Message}");
                return;
            }
            finally
            {
                // Ensure lock is released even if there's an error
                _netLimiterLock = false;
            }
        }

        // Analyze connections and populate DataGridView with proxy status and ban/whitelist info.
        // Now also checks each unique IP for proxy/VPN/TOR/Geo violations and bans/adds to filter if needed.
        private static void AnalyzeConnections(List<ConnectionInfo> connections)
        {
            DateTime now = DateTime.Now;

            var ipGroups = connections
                .GroupBy(conn => conn.RemoteAddress)
                .OrderByDescending(g => g.Count())
                .ToList();

            banInstance.NetLimiterConnectionLogs.Clear();

            int rowIndex = 0;

            foreach (var ipGroup in ipGroups)
            {
                string ip = ipGroup.Key!;
                int count = ipGroup.Count();

                // Existing excessive connection ban logic
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

                // Proxy/VPN/TOR/Geo checks for each unique IP (mirroring CheckAndPuntProxyViolations)
                if (theInstance.proxyCheckEnabled && IPAddress.TryParse(ip, out IPAddress? ipAddress))
                {
                    // Skip if already banned or whitelisted
                    if (!banInstance.BannedPlayerIPs.Any(b => !BanHelper.IsExpiredOrInfo(b, now) && BanHelper.IsIPMatch(ipAddress, b.PlayerIP, b.SubnetMask)) &&
                        !banInstance.WhitelistedIPs.Any(w => !BanHelper.IsExpiredOrInfo(w, now) && BanHelper.IsIPMatch(ipAddress, w.PlayerIP, w.SubnetMask)))
                    {
                        var (shouldBan, banReason) = EvaluateProxyBan(ipAddress);
                        if (shouldBan)
                        {
                            _ = Task.Run(async () => await BanAndFilterIp(ipAddress, banReason, ip));
                        }
                    }
                }

                string vpnStatus = BanHelper.GetVpnStatus(ip);
                string listStatus = BanHelper.GetListStatus(ip, now);

                int recordID = rowIndex++;

                AppDebug.Log("tickerBanManagement", $"NetLimiter Connection Log - RecordID: {recordID}, IP: {ip}, Connections: {count}, VPN Status: {vpnStatus}, List Status: {listStatus}");

                banInstance.NetLimiterConnectionLogs.Add(new netLimiterConnLogEntry
                {
                    NL_rowID = recordID,
                    NL_ipAddress = ip,
                    NL_numCons = count,
                    NL_vpnStatus = vpnStatus,
                    NL_notes = listStatus
                });
            }
        }

        // Sync NetLimiter filter with ban and whitelist, subtracting whitelisted ranges from banned ranges
        private static async Task SyncNetLimiterFilterAsync()
        {
            if (_netLimiterFilterLock)
                return; // Skip if already running

            string filterName = theInstance.netLimiterFilterName;

            if (string.IsNullOrEmpty(filterName))
            {
                AppDebug.Log("tickerBanManagement", "NetLimiter filter name not configured. Skipping filter sync.");
                return;
            }

            if ((DateTime.Now - _lastFilterSync) < _filterSyncInterval)
            {
                return; // Skip if last sync was recent
            }

            try
            {
                _netLimiterFilterLock = true;
                
                // Update last sync time
                DateTime now = _lastFilterSync = DateTime.Now;

                // Log start of sync
                AppDebug.Log("tickerBanManagement", $"Starting NetLimiter filter sync for filter '{filterName}'");

                // Get current IPs in the NetLimiter filter
                var filterIPs = await NetLimiterClient.GetFilterIpAddressesAsync(filterName);
                var filterIPSet = new HashSet<string>(filterIPs);

                // Build list of banned ranges
                var bannedRanges = new List<IpRange>();
                foreach (var bannedIP in banInstance.BannedPlayerIPs)
                {
                    if (BanHelper.IsExpiredOrInfo(bannedIP, now))
                        continue;
                    bannedRanges.Add(IpRange.FromCidr(bannedIP.PlayerIP, bannedIP.SubnetMask));
                }

                // Build list of whitelisted ranges
                var whitelistedRanges = new List<IpRange>();
                foreach (var whitelistedIP in banInstance.WhitelistedIPs)
                {
                    if (BanHelper.IsExpiredOrInfo(whitelistedIP, now))
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
                            bool added = await NetLimiterClient.AddIpToFilterAsync(filterName, ipStr, 32);
                            if (added) addedCount++;
                        }
                    }
                    else
                    {
                        bool added = await NetLimiterClient.AddIpToFilterAsync(filterName, ipRangeStr, 32);
                        if (added) addedCount++;
                    }

                    AppDebug.Log("tickerBanManagement", $"Added IP/range {ipRangeStr} to NetLimiter filter '{filterName}'");
                }

                // Remove IPs/ranges that shouldn't be in filter
                int removedCount = 0;
                foreach (var ipRangeStr in ipsToRemove)
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
                AppDebug.Log("tickerBanManagement", $"NetLimiter filter sync complete: Added {addedCount}, Removed {removedCount} IPs");

            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error acquiring NetLimiter filter lock: {ex.Message}");
            }
            finally
            {
                // Ensure lock is released even if there's an error
                _netLimiterFilterLock = false;
            }

        }

        // Check active players against ban lists and punt if banned
        public static void CheckAndPuntBannedPlayers()
        {
            DateTime now = DateTime.Now;
            var slotsToPunt = new List<(int SlotNum, string PlayerName, string PuntReason)>();
            try
            {
                foreach (var (slotNum, player) in GetActivePlayers())
                {
                    if (BanHelper.IsPlayerWhitelisted(player, banInstance, now))
                    {
                        AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}) is whitelisted. Skipping ban checks.");
                        continue;
                    }

                    bool shouldPunt = false;
                    string puntReason = string.Empty;

                    // Check against banned player names
                    foreach (var bannedName in banInstance.BannedPlayerNames)
                    {
                        if (BanHelper.IsExpiredOrInfo(bannedName, now))
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
                                if (BanHelper.IsExpiredOrInfo(bannedIP, now))
                                    continue;

                                if (BanHelper.IsIPMatch(playerIP, bannedIP.PlayerIP, bannedIP.SubnetMask))
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
                            continue;  // Player not in game yet. Needless to spam the "punt".
                        }

                        slotsToPunt.Add((slotNum, player.PlayerName, puntReason));
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error checking banned players: {ex.Message}");
            }

            // Punt players after enumeration
            PuntPlayers(slotsToPunt);
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

            // Exit if already banned or whitelisted
            if (banInstance.BannedPlayerIPs.Any(b => !BanHelper.IsExpiredOrInfo(b, now) && BanHelper.IsIPMatch(ip, b.PlayerIP, b.SubnetMask)))
            {
                AppDebug.Log("tickerBanManagement", $"IP {ipAddress} is already banned. Skipping.");
                return;
            }
            if (banInstance.WhitelistedIPs.Any(w => !BanHelper.IsExpiredOrInfo(w, now) && BanHelper.IsIPMatch(ip, w.PlayerIP, w.SubnetMask)))
            {
                AppDebug.Log("tickerBanManagement", $"IP {ipAddress} is whitelisted. Skipping ban for excessive connections.");
                return;
            }

            var banRecord = AddBanRecord(ip, $"Auto-banned: Excessive connections ({connectionCount}) exceeded threshold ({theInstance.netLimiterConThreshold})");

            try
            {
                AppDebug.Log("tickerBanManagement", $"Banned IP {ipAddress} for excessive connections: {connectionCount} (threshold: {theInstance.netLimiterConThreshold})");

                bool added = await AddIpToNetLimiterFilter(ipAddress);
                AppDebug.Log("tickerBanManagement",
                    added
                        ? $"Successfully added IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'"
                        : $"Failed to add IP {ipAddress} to NetLimiter filter '{theInstance.netLimiterFilterName}'"
                );
            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerBanManagement", $"Error adding ban record for IP {ipAddress}: {ex.Message}");
                throw;
            }
        }

        private static bool IsIpBanned(IPAddress ip, IEnumerable<banInstancePlayerIP> bannedList)
        {
            foreach (var banned in bannedList)
            {
                var range = IpRange.FromCidr(banned.PlayerIP, banned.SubnetMask);
                uint ipUint = IpRange.IpToUint(ip);
                uint start = IpRange.IpToUint(range.Start);
                uint end = IpRange.IpToUint(range.End);
                if (ipUint >= start && ipUint <= end)
                    return true;
            }
            return false;
        }

        public static void CheckAndPuntProxyViolations()
        {
            if (!theInstance.proxyCheckEnabled)
                return;

            DateTime now = DateTime.Now;
            var slotsToPunt = new List<(int SlotNum, string PlayerName, string PuntReason, bool ShouldBan, IPAddress? PlayerIP, string PlayerIPAddress)>();

            foreach (var (slotNum, player) in GetActivePlayers())
            {
                try
                {
                    if (string.IsNullOrEmpty(player.PlayerIPAddress))
                        continue;

                    if (!IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
                        continue;

                    if (BanHelper.IsPlayerWhitelisted(player, banInstance, now))
                        continue;

                    var (shouldBan, banReason) = EvaluateProxyBan(playerIP);

                    bool shouldPunt = false;
                    bool shouldBanFinal = false;
                    string puntReason = string.Empty;

                    if (shouldBan && !IsIpBanned(playerIP, banInstance.BannedPlayerIPs))
                    {
                        shouldBanFinal = true;
                        puntReason = banReason;
                    }
                    else
                    {
                        // Original punt logic for proxy/vpn/tor/geo, but not auto-ban
                        var proxyRecord = banInstance.ProxyRecords.FirstOrDefault(p => p.IPAddress.Equals(playerIP));
                        if (proxyRecord == null || now > proxyRecord.CacheExpiry)
                        {
                            BanHelper.CheckIP(playerIP);
                            continue;
                        }

                        if (proxyRecord.IsProxy && theInstance.proxyCheckProxyAction > 0)
                        {
                            shouldPunt = true;
                            puntReason = $"Proxy detected{(theInstance.proxyCheckProxyAction == 2 ? " (Auto-banned)" : " (Kicked)")}";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a PROXY. Action: {(theInstance.proxyCheckProxyAction == 2 ? "Ban" : "Kick")}");
                        }
                        if (!shouldPunt && proxyRecord.IsVpn && theInstance.proxyCheckVPNAction > 0)
                        {
                            shouldPunt = true;
                            puntReason = $"VPN detected{(theInstance.proxyCheckVPNAction == 2 ? " (Auto-banned)" : " (Kicked)")}";
                            if (!string.IsNullOrEmpty(proxyRecord.Provider))
                                puntReason += $" - {proxyRecord.Provider}";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using a VPN. Action: {(theInstance.proxyCheckVPNAction == 2 ? "Ban" : "Kick")}");
                        }
                        if (!shouldPunt && proxyRecord.IsTor && theInstance.proxyCheckTORAction > 0)
                        {
                            shouldPunt = true;
                            puntReason = $"TOR detected{(theInstance.proxyCheckTORAction == 2 ? " (Auto-banned)" : " (Kicked)")}";
                            AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) is using TOR. Action: {(theInstance.proxyCheckTORAction == 2 ? "Ban" : "Kick")}");
                        }
                        if (!shouldPunt && theInstance.proxyCheckGeoMode > 0 && !string.IsNullOrEmpty(proxyRecord.CountryCode))
                        {
                            bool countryInList = banInstance.ProxyBlockedCountries
                                .Any(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                            bool shouldBlockCountry = theInstance.proxyCheckGeoMode == 1 ? countryInList : !countryInList;
                            if (shouldBlockCountry)
                            {
                                shouldPunt = true;
                                var country = banInstance.ProxyBlockedCountries
                                    .FirstOrDefault(c => c.CountryCode.Equals(proxyRecord.CountryCode, StringComparison.OrdinalIgnoreCase));
                                string countryName = country?.CountryName ?? proxyRecord.CountryCode;
                                puntReason = $"Geo-blocked: {countryName} ({proxyRecord.CountryCode})";
                                AppDebug.Log("tickerBanManagement", $"Player '{player.PlayerName}' (Slot {slotNum}, IP: {player.PlayerIPAddress}) from blocked country: {countryName}. Action: Kick");
                            }
                        }
                    }

                    if (shouldBanFinal)
                    {
                        _ = Task.Run(async () => await BanAndFilterIp(playerIP, puntReason, player.PlayerIPAddress));
                    }

                    if (shouldPunt || shouldBanFinal)
                    {
                        slotsToPunt.Add((slotNum, player.PlayerName, puntReason, shouldBanFinal, playerIP, player.PlayerIPAddress));
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error checking proxy status for player in slot {slotNum}: {ex.Message}");
                }
            }

            // Process punts and bans after enumeration
            foreach (var (slotNum, playerName, puntReason, shouldBan, playerIP, playerIPAddress) in slotsToPunt)
            {
                if (shouldBan && playerIP != null)
                {
                    // Check if already banned (by range or single IP)
                    if (IsIpBanned(playerIP, banInstance.BannedPlayerIPs))
                    {
                        AppDebug.Log("tickerBanManagement", $"IP {playerIPAddress} is already banned. Skipping auto-ban.");
                        continue;
                    }
                }

                try
                {
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + slotNum);
                    AppDebug.Log("tickerBanManagement", $"Punting player '{playerName}' (Slot {slotNum}). Reason: {puntReason}");
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error punting player '{playerName}' (Slot {slotNum}): {ex.Message}");
                }
            }
        }

        public static void CheckAndPuntDisabledRoles()
        {
            DateTime now = DateTime.Now;
            var slotsToPunt = new List<(int SlotNum, string PlayerName, string PuntReason)>();

            foreach (var (slotNum, player) in GetActivePlayers())
            {
                try
                {
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
                        slotsToPunt.Add((slotNum, player.PlayerName, puntReason));
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error checking roles for player in slot {slotNum}: {ex.Message}");
                }
            }

            // Punt players after enumeration
            PuntPlayers(slotsToPunt);
        }
    }
}