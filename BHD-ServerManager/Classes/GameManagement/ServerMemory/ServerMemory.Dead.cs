using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Storage;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabPlayers;

namespace BHD_ServerManager.Classes.GameManagement.Memory
{
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static partial class ServerMemory
    {

		// Constants for functions called in this file.


        // Function: GetGameTypeID, Converts the MapType ShortName to and INT value.
        public static int getGameTypeID(string gameType)
        {
            switch (gameType)
            {
                case "DM":
                    return 0;
                case "TDM":
                    return 1;
                case "CP":
                    return 2;
                case "TKOTH":
                    return 3;
                case "KOTH":
                    return 4;
                case "SD":
                    return 5;
                case "AD":
                    return 6;
                case "CTF":
                    return 7;
                case "FB":
                    return 8;
                default:
                    return -1;
            }
        }


	}
}
