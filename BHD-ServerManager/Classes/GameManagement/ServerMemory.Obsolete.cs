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

namespace BHD_ServerManager.Classes.GameManagement
{
    // This class is a placeholder for Obselte Functions to be reviewed and removed in the future.

    public static partial class ServerMemory
	{

	    // Function: GetGameTypeID, Converts the MapType ShortName to and INT value.
        public static int getGameTypeID(string gameType) => gameType switch
        {
            "DM"   => 0,
            "TDM"  => 1,
            "CP"   => 2,
            "TKOTH"=> 3,
            "KOTH" => 4,
            "SD"   => 5,
            "AD"   => 6,
            "CTF"  => 7,
            "FB"   => 8,
            _      => -1,
        };


	}
}
