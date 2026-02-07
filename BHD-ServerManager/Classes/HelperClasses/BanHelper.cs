using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BHD_ServerManager.Classes.HelperClasses
{
    public static class BanHelper
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;

        public static bool IsIPMatch(IPAddress playerIP, IPAddress bannedIP, int subnetMask)
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

        // Should be moved to BanHelper.cs
        public static bool IsExpiredOrInfo(dynamic record, DateTime now)
        {
            if (record.RecordType == banInstanceRecordType.Information)
                return true;

            if (record.RecordType == banInstanceRecordType.Temporary &&
                record.ExpireDate.HasValue &&
                now > record.ExpireDate.Value)
                return true;

            return false;
        }

        // Should be moved to BanHelper.cs
        public static bool IsPlayerWhitelisted(PlayerObject player, banInstance banInstance, DateTime now)
        {
            foreach (var whitelistedName in banInstance.WhitelistedNames)
            {
                if (IsExpiredOrInfo(whitelistedName, now))
                    continue;

                if (player.PlayerName.Equals(whitelistedName.PlayerName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            if (!string.IsNullOrEmpty(player.PlayerIPAddress) && IPAddress.TryParse(player.PlayerIPAddress, out IPAddress? playerIP))
            {
                foreach (var whitelistedIP in banInstance.WhitelistedIPs)
                {
                    if (IsExpiredOrInfo(whitelistedIP, now))
                        continue;

                    if (IsIPMatch(playerIP, whitelistedIP.PlayerIP, whitelistedIP.SubnetMask))
                        return true;
                }
            }

            return false;
        }

        // Should be moved to BanHelper.cs
        public static void CheckIP(IPAddress Address)
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

        // Should be moved to BanHelper.cs
        public static string GetVpnStatus(string ipAddress)
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

        // Should be moved to BanHelper.cs
        public static string GetListStatus(string ipAddress, DateTime now)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
                return "ERROR";

            var comments = new List<string>();

            foreach (var whitelistedIP in banInstance.WhitelistedIPs)
            {
                if (IsExpiredOrInfo(whitelistedIP, now))
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
                if (IsExpiredOrInfo(bannedIP, now))
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

    }
}
