using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteBanInstanceManager : banInstanceInterface
    {
        public static ServerManager thisServer => Program.ServerManagerUI!;
        public static banInstance instanceBans => CommonCore.instanceBans!;

        public void AddBannedPlayer(string playerName, IPAddress playerIP, int subnetMask) => CmdAddBannedPlayer.ProcessCommand(playerName, playerIP, subnetMask);

        public void LoadSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public bool RemoveBannedPlayerAddress(int recordId) => CmdRemoveBannedPlayerAddress.ProcessCommand(recordId);

        public bool RemoveBannedPlayerBoth(int recordId) => CmdRemoveBannedPlayerBoth.ProcessCommand(recordId);

        public bool RemoveBannedPlayerName(int recordId) => CmdRemoveBannedPlayerName.ProcessCommand(recordId);

        public void SaveSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }
        public void UpdateBannedTables()
        {
            // Clear existing rows
            thisServer.dg_playerNames.Rows.Clear();
            thisServer.dg_IPAddresses.Rows.Clear();

            // Add banned player names
            foreach (var bannedName in instanceBans.BannedPlayerNames)
            {
                string decodedName = Encoding.UTF8.GetString(Convert.FromBase64String(bannedName.playerName));
                // Adjust the order and number of columns as needed
                thisServer.dg_playerNames.Rows.Add(
                    bannedName.recordId,
                    decodedName
                );
            }

            // Add banned player addresses
            foreach (var bannedAddress in instanceBans.BannedPlayerAddresses)
            {
                thisServer.dg_IPAddresses.Rows.Add(
                    bannedAddress.recordId,
                    $"{bannedAddress.playerIP}/{bannedAddress.subnetMask}"
                );
            }
        }

    }
}
