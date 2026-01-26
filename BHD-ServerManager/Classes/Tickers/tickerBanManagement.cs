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
            int netLimiterAppId = 0;

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
                    }

                    // Only check and punt bans if server is ONLINE
                    if (theInstance.instanceStatus == InstanceStatus.ONLINE)
                    {

                        // Basic Ban Checks (Ignore Whitelist)
                        CheckAndPuntBannedPlayers();

                    }
                }
                else
                {
                    AppDebug.Log("tickerBanManagement", "Server process is not attached. Ticker Skipping.");
                }

            });
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
    }
}