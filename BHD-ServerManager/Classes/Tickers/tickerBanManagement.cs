using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerBanManagement
    {
        // Global Variables
        private readonly static theInstance thisInstance = CommonCore.theInstance!;
        private readonly static banInstance banInstance = CommonCore.instanceBans!;

        // Ticker Lock
        private static readonly object tickerLock = new object();
        // This method runs the ticker for ban management tasks.
        public static void runTicker(ServerManager thisServer)
        {
            // Ensure UI thread safety
            if (thisServer.InvokeRequired)
            {
                try
                {
                    thisServer.Invoke(new Action(() => runTicker(thisServer)));
                } catch (Exception ex)
                {
                    AppDebug.Log("tickerBanManagement", $"Error invoking runTicker: {ex.Message}");
                }

                return;
            }

            lock (tickerLock)
            {

                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // Method 1: Check for Players to Punt.
                    Check4Bans();
                    // Method 3: Check currently banned players if they are using a VPN or Proxy.
                    //           Future update.
                }
                else
                {
                    // If the server process is not attached, we can assume the server is offline.
                    AppDebug.Log("tickerBanManagement", "Server process is not attached. Ticker Skipping.");
                }
                // Update Regardless
                banInstanceManager.UpdateBannedTables();
            }
        }        
        // Check for Players to Punt
        public static void Check4Bans()
        {
            if(thisInstance.instanceStatus == InstanceStatus.OFFLINE || thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            foreach (var player in thisInstance.playerList)
            {
                playerObject playerRecord = player.Value;
                if (playerRecord.PlayerPing == 0)
                {
                    continue;
                }
                // Returns true if the name is banned
                bool isBanned = banInstance.BannedPlayerNames.Any(b => b.playerName == playerRecord.PlayerNameBase64);
                if (isBanned)
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerRecord.PlayerSlot}");
                    AppDebug.Log("tickerBanManagement", $"Punting player {playerRecord.PlayerNameBase64} in PlayerSlot {playerRecord.PlayerSlot} due to name ban.");
                }
                // Return true if the IP is banned
                bool isIpBanned = IsIpBanned(playerRecord.PlayerIPAddress!, banInstance.BannedPlayerAddresses);
                if (isIpBanned)
                {
                    ServerMemory.WriteMemorySendConsoleCommand($"punt {playerRecord.PlayerSlot}");
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
