using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System.Net;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static banInstance banInstance => CommonCore.instanceBans!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

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
            // Always marshal to UI thread for UI updates
            SafeInvoke(thisServer, () =>
            {
                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // Only check and punt bans if server is ONLINE
                    if (thisInstance.instanceStatus == InstanceStatus.ONLINE)
                    {
                        try
                        {
                            Check4Bans();
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerBanManagement", $"Error in Check4Bans: {ex.Message}");
                        }
                    }
                }
                else
                {
                    AppDebug.Log("tickerBanManagement", "Server process is not attached. Ticker Skipping.");
                }

                // Always update the ban tables in the UI
                try
                {
                    banInstanceManager.UpdateBannedTables();
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error updating banned tables: {ex.Message}");
                }
            });
        }

        // Check for Players to Punt (only when ONLINE)
        public static void Check4Bans()
        {
            foreach (var player in thisInstance.playerList)
            {
                playerObject playerRecord = player.Value;
                if (playerRecord.PlayerPing == 0)
                    continue;

                // Name ban check
                bool isBanned = banInstance.BannedPlayerNames.Any(b => b.playerName == playerRecord.PlayerNameBase64);
                if (isBanned)
                {
                    chatInstanceManagers.SendMessageToQueue(true, 0, $"punt {playerRecord.PlayerSlot}");
                    AppDebug.Log("tickerBanManagement", $"Punting player {playerRecord.PlayerNameBase64} in PlayerSlot {playerRecord.PlayerSlot} due to name ban.");
                    continue; // No need to check IP if already banned by name
                }

                // IP ban check
                bool isIpBanned = IsIpBanned(playerRecord.PlayerIPAddress!, banInstance.BannedPlayerAddresses);
                if (isIpBanned)
                {
                    chatInstanceManagers.SendMessageToQueue(true, 0, $"punt {playerRecord.PlayerSlot}");
                    AppDebug.Log("tickerBanManagement", $"Punting player {playerRecord.PlayerIPAddress} in PlayerSlot {playerRecord.PlayerSlot} due to IP ban.");
                }
            }
        }

        // Check to see if an IP is banned
        public static bool IsIpBanned(string ipString, List<BannedPlayerAddress> bannedList)
        {
            if (!IPAddress.TryParse(ipString, out var ip))
                return false;

            foreach (var ban in bannedList)
            {
                if (IsInSubnet(ip, ban.playerIP, ban.subnetMask))
                    return true;
            }
            return false;
        }

        // Checks if an IP is in a given subnet
        private static bool IsInSubnet(IPAddress address, IPAddress subnet, int maskLength)
        {
            var addressBytes = address.GetAddressBytes();
            var subnetBytes = subnet.GetAddressBytes();

            if (addressBytes.Length != subnetBytes.Length)
                return false;

            int fullBytes = maskLength / 8;
            int remainingBits = maskLength % 8;

            for (int i = 0; i < fullBytes; i++)
            {
                if (addressBytes[i] != subnetBytes[i])
                    return false;
            }

            if (remainingBits > 0)
            {
                int mask = (byte)~(0xFF >> remainingBits);
                if ((addressBytes[fullBytes] & mask) != (subnetBytes[fullBytes] & mask))
                    return false;
            }

            return true;
        }
    }
}