using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class mapInstanceManager
    {
        public static mapInstanceInterface Implementation { get; set; }

        public static List<mapFileInfo> BuildCurrentMapPlaylist() => Implementation.BuildCurrentMapPlaylist();
        public static void CalculateGameTypeBits(List<int> numbers, int target, mapFileInfo map) => Implementation.CalculateGameTypeBits(numbers, target, map);
        public static void LoadCustomMaps() => Implementation.LoadCustomMaps();
        public static void LoadDefaultMaps() => Implementation.LoadDefaultMaps();
        public static void ResetAvailableMaps() => Implementation.ResetAvailableMaps();
        public static void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external) => Implementation.SaveCurrentMapPlaylist(mapList, external);
    }
}
