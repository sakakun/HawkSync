using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.InstanceInterfaces
{
    public interface mapInstanceInterface
    {
        void ResetAvailableMaps();
        void LoadDefaultMaps();
        void LoadCustomMaps();
        void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external);
        List<mapFileInfo> BuildCurrentMapPlaylist();
        void CalculateGameTypeBits(List<int> numbers, int target, mapFileInfo map);
    }
}
