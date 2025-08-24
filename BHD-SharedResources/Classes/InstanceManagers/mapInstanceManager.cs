using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using System.Collections.Generic;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class mapInstanceManager
    {
        public static mapInstanceInterface Implementation { get; set; }

        public static List<mapFileInfo> BuildCurrentMapPlaylist() => Implementation.BuildCurrentMapPlaylist();
        public static void CalculateGameTypeBits(List<int> numbers, int target, List<int> result) => Implementation.CalculateGameTypeBits(numbers, target, result);
        public static void LoadCustomMaps() => Implementation.LoadCustomMaps();
        public static void LoadDefaultMaps() => Implementation.LoadDefaultMaps();
        public static void ResetAvailableMaps() => Implementation.ResetAvailableMaps();
        public static void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external) => Implementation.SaveCurrentMapPlaylist(mapList, external);
        public static List<mapFileInfo> LoadCustomMapPlaylist(bool external = false) => Implementation.LoadCustomMapPlaylist(external);
        public static string[]? GetFileLinesFromDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName) => Implementation.GetFileLinesFromDialog(saveDialog, filter, title, initialDirectory, defaultFileName);
        
    }
}
