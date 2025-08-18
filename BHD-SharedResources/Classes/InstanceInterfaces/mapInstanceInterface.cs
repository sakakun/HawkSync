using BHD_SharedResources.Classes.Instances;
using System.Collections.Generic;

namespace BHD_SharedResources.Classes.InstanceInterfaces
{
    public interface mapInstanceInterface
    {
        void ResetAvailableMaps();
        void LoadDefaultMaps();
        void LoadCustomMaps();
        void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external);
        List<mapFileInfo> BuildCurrentMapPlaylist();
        void CalculateGameTypeBits(List<int> numbers, int target, List<int> result);
        List<mapFileInfo> LoadCustomMapPlaylist(bool external = false);
        string[]? GetFileLinesFromDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName);
    }
}
