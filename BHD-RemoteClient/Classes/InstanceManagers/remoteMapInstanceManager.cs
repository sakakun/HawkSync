using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;

namespace BHD_RemoteClient.Classes.InstanceManagers
{

    public class remoteMapInstanceManager : mapInstanceInterface
    {
        private static ServerManager thisServer = Program.ServerManagerUI!;
        private static mapInstance instanceMaps = CommonCore.instanceMaps!;

        public List<mapFileInfo> BuildCurrentMapPlaylist()
        {
            DataGridView ogList = thisServer.dataGridView_currentMaps;

            var newPlaylist = new List<mapFileInfo>();


            foreach (DataGridViewRow row in ogList.Rows)
            {
                if (row.IsNewRow) continue;

                string? mapName = row.Cells["MapName"].Value?.ToString();
                string? gameType = row.Cells["MapType"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(mapName) || string.IsNullOrWhiteSpace(gameType))
                    continue;

                var map = instanceMaps.availableMaps
                    .FirstOrDefault(m => string.Equals(m.MapName, mapName, StringComparison.OrdinalIgnoreCase)
                                      && string.Equals(m.MapType, gameType, StringComparison.OrdinalIgnoreCase));

                if (map == null)
                    continue;

                newPlaylist.Add(map);
            }

            return newPlaylist;
        }

        void mapInstanceInterface.CalculateGameTypeBits(List<int> numbers, int target, List<int> result)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void mapInstanceInterface.LoadCustomMaps()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void mapInstanceInterface.LoadDefaultMaps()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void ResetAvailableMaps() => CmdResetAvailableMaps.ProcessCommand();

        public void SaveCurrentMapPlaylist(List<mapFileInfo> mapList, bool external)
        {
            string savePath = CommonCore.AppDataPath;

            if (external)
            {
                savePath = Functions.ShowFileDialog(true, "Map Playlist (*.mpl)|*.mpl|All files (*.*)|*.*", "Save Map Playlist", savePath, "MapPlaylist.mpl")!;
                if (savePath == null)
                {
                    return; // User cancelled
                }
            }
            else
            {
                return;  // No Remote Side
            }

            try
            {
                var sb = new StringBuilder();
                foreach (var map in mapList)
                {
                    sb.AppendLine($"{map.MapFile},{map.MapName},{map.MapType}");
                }
                File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save map playlist:\n" + ex.Message, "Save Error");
            }
        }
    }
}
