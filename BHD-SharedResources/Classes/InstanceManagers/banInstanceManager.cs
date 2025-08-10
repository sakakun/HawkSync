using BHD_SharedResources.Classes.InstanceInterfaces;
using System.Net;

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
        public static void ExportSettings() => Implementation.ExportSettings();
        public static void ImportSettings() => Implementation.ImportSettings();
        public static void ImportSettingsFromBase64(string encodedData, string fileType) => Implementation.ImportSettingsFromBase64(encodedData, fileType);
    }
}
