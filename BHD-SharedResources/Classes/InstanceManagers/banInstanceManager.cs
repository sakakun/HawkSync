using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class banInstanceManager
    {
        public static banInstanceInterface Implementation { get; set; }

        public static void LoadSettings() => Implementation.LoadSettings();
        public static void SaveSettings() => Implementation.SaveSettings();
        public static void AddBannedPlayer(string playerName, IPAddress playerIP = null!, int subnetMask = 32) => Implementation.AddBannedPlayer(playerName, playerIP, subnetMask);
        public static bool RemoveBannedPlayerName(int recordId) => Implementation.RemoveBannedPlayerName(recordId);
        public static bool RemoveBannedPlayerAddress(int recordId) => Implementation.RemoveBannedPlayerAddress(recordId);
        public static bool RemoveBannedPlayerBoth(int recordId) => Implementation.RemoveBannedPlayerBoth(recordId);
        public static void UpdateBannedTables() => Implementation.UpdateBannedTables();

    }
}
