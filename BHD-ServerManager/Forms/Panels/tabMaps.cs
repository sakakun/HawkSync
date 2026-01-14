using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;
using System.Data;
using System.Text;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabMaps : UserControl
    {

        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        private mapInstance? instanceMaps => CommonCore.instanceMaps;

        // --- Class Variables ---
        private new string Name = "MapsTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.
        public tabMaps()
        {
            InitializeComponent();
        }
        // --- Functions ---
        // --- Populate Available Maps ---
        public void methodFunction_loadSourceMaps()
        {
            // Load Default Maps from Database
            List<mapFileInfo> defaultMaps = DatabaseManager.GetDefaultMaps();

            // Clear Current Available Maps
            dataGridView_availableMaps.Rows.Clear();

            // Alway load modType 0
            foreach (mapFileInfo map in defaultMaps)
            {
                // If it is a default map, list it no matter what.
                if (map.ModType == 0)
                {
                    int rowIndex = dataGridView_availableMaps.Rows.Add(
                        map.MapID,
                        map.MapName,
                        map.MapFile,
                        map.ModType,
                        map.MapType,
                        objectGameTypes.GetShortName(map.MapType)
                    );
                    DataGridViewRow row = dataGridView_availableMaps.Rows[rowIndex];

                    string toolTip = $"Map File: {map.MapFile}";

                    row.Cells[1].ToolTipText = toolTip;

                }
            }

            // Load Custom Maps
            methodFunction_loadCustomMapps();

        }

        private void methodFunction_loadCustomMapps()
        {
            // Get Current serverGamePath
            string gamePath = theInstance!.profileServerPath;

            DirectoryInfo d = new DirectoryInfo(gamePath);
            List<string> badMapsList = new List<string>();

            foreach (var file in d.GetFiles("*.bms"))
            {
                using (FileStream fsSourceDDS = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
                {
                    string first_line = string.Empty;
                    string mapName = string.Empty;
                    try
                    {
                        first_line = File.ReadLines(System.IO.Path.Combine(gamePath, file.Name), Encoding.Default).First().ToString();
                    }
                    catch (Exception e)
                    {
                        AppDebug.Log("tabMaps", "theMaps: methodFunction_loadCustomMapps: Skipping Map File: " + e.Message);
                        continue;
                    }

                    first_line = first_line.Replace("", "|").Replace("\0\0\0", "|");
                    string[] first_line_arr = first_line.Split("|".ToCharArray());
                    var first_line_list = new List<string>();
                    foreach (string f in first_line_arr)
                    {
                        string tmp = f.Trim().Replace("\0", "".ToString());
                        if (!string.IsNullOrEmpty(tmp))
                            first_line_list.Add(tmp);
                    }
                    try
                    {
                        mapName = first_line_list[1];
                    }
                    catch
                    {
                        badMapsList.Add(file.Name);
                        continue;
                    }
                    string mapFile = file.Name;

                    // Read BitmapBytes sum at 0x8A
                    fsSourceDDS.Seek(0x8A, SeekOrigin.Begin);
                    var bitmapBytesSum = binaryReader.ReadInt16();

                    // Read Bitmap sum at 0x8B
                    fsSourceDDS.Seek(0x8B, SeekOrigin.Begin);
                    var bitmapSum = binaryReader.ReadInt16();

                    // Get all possible BitmapBytes and Bitmap values (excluding 0)
                    var bitmapBytesNumbers = objectGameTypes.All.Select(gt => gt.BitmapBytes).Where(b => b > 0).OrderByDescending(b => b).ToList();
                    var bitmapNumbers = objectGameTypes.All.Select(gt => gt.Bitmap).Where(b => b > 0).OrderByDescending(b => b).ToList();

                    // Find all game types for this map
                    var gameTypeBits = new List<int>();
                    CalculateGameTypeBits(bitmapBytesNumbers, bitmapBytesSum, gameTypeBits);
                    if (gameTypeBits.Count == 0)
                    {
                        CalculateGameTypeBits(bitmapNumbers, bitmapSum, gameTypeBits);
                    }

                    // If still nothing, check for special case (Attack and Defend)
                    if (gameTypeBits.Count == 0 && (bitmapBytesSum == 128 || bitmapSum == 0))
                    {
                        gameTypeBits.Add(0); // Attack and Defend
                    }

                    // Map to game type info and add a separate entry for each supported game type
                    var gameTypes = new List<int>();
                    foreach (var bit in gameTypeBits)
                    {
                        var match = objectGameTypes.All.FirstOrDefault(gt => gt.Bitmap == bit || gt.BitmapBytes == bit);

                        if (match != null)
                        {
                            gameTypes.Add(match.DatabaseId);

                            var mapEntry = new mapFileInfo
                            {
                                MapID = dataGridView_availableMaps.Rows.Count + 1,
                                MapFile = mapFile,
                                MapName = mapName,
                                MapType = match.DatabaseId,
                                ModType = 9,
                            };
                            int rowIndex = dataGridView_availableMaps.Rows.Add(
                                mapEntry.MapID,
                                mapEntry.MapName,
                                mapEntry.MapFile,
                                mapEntry.ModType,
                                mapEntry.MapType,
                                objectGameTypes.GetShortName(mapEntry.MapType)
                            );
                            DataGridViewRow row = dataGridView_availableMaps.Rows[rowIndex];

                            string toolTip = $"Map File: {mapEntry.MapFile}";

                            row.Cells[1].ToolTipText = toolTip;

                        }
                        else
                        {
                            MessageBox.Show("File Name: " + mapFile + "\n" + " with Bitmap/BitmapBytes: " + bit + "\n" + "Reason: Could not find gametype for map.");
                        }
                    }
                }
            }

            if (badMapsList.Count > 0)
            {
                string badMaps = string.Join("\n", badMapsList);
                MessageBox.Show("Could not read map title from:\n" + badMaps + "\nThis could due to a non-converted, or a corrupted file.", "infoCurrentMapName List Error");
            }
        }
        public void CalculateGameTypeBits(List<int> numbers, int target, List<int> result)
        {
            void Recurse(List<int> nums, int tgt, List<int> partial)
            {
                int sum = partial.Sum();
                if (sum == tgt)
                {
                    result.AddRange(partial);
                    return;
                }
                if (sum >= tgt)
                    return;

                for (int i = 0; i < nums.Count; i++)
                {
                    var remaining = nums.Skip(i + 1).ToList();
                    var nextPartial = new List<int>(partial) { nums[i] };
                    Recurse(remaining, tgt, nextPartial);
                    if (result.Count > 0) return; // Only need the first valid combination
                }
            }
            result.Clear();
            Recurse(numbers, target, new List<int>());
        }


        // --- Populate Current Map Playlist ---
        public void functionEvent_PopulateCurrentMapPlaylist(List<mapFileInfo> ImportedMapList = null!)
        {
            List<mapFileInfo> MapList = ImportedMapList != null ? ImportedMapList : instanceMaps!.currentMapPlaylist;

            // Clear the DataGridView
            dataGridView_currentMaps.Rows.Clear();
            // MapID for the Purpose of the Current Playlist is the "Order" in which they are played.
            int rowIndex = 1;
            // Populate the DataGridView with current map playlist
            foreach (var map in MapList)
            {
                int rowObject = dataGridView_currentMaps.Rows.Add();
                DataGridViewRow row = dataGridView_currentMaps.Rows[rowObject];

                row.Cells["current_MapID"].Value = rowIndex;
                row.Cells["current_MapFileName"].Value = map.MapFile;
                row.Cells["current_MapName"].Value = map.MapName;
                row.Cells["current_MapType"].Value = map.MapType;
                row.Cells["current_MapTypeShort"].Value = objectGameTypes.GetShortName(map.MapType);
                rowIndex++;
            }
        }
        // --- Add Map to Current Playlist from Available Maps ---
        public void functionEvent_AddMapToCurrentPlaylist(object sender)
        {
            // Get the current DataGridView of available maps
            DataGridView dataGridView = (DataGridView)sender;
            // Get row index of the clicked cell
            int rowIndex = dataGridView.CurrentCell.RowIndex;
            DataGridViewRow rowOG = dataGridView.Rows[rowIndex];

            int rowObject = dataGridView_currentMaps.Rows.Add();
            DataGridViewRow rowNew = dataGridView_currentMaps.Rows[rowObject];
            rowNew.Cells["current_MapID"].Value = rowObject + 1;
            rowNew.Cells["current_MapFileName"].Value = rowOG.Cells["avail_MapFileName"].Value;
            rowNew.Cells["current_MapName"].Value = rowOG.Cells["avail_MapName"].Value;
            rowNew.Cells["current_MapType"].Value = rowOG.Cells["avail_MapType"].Value;
            rowNew.Cells["current_MapTypeShort"].Value = rowOG.Cells["avail_MapTypeShort"].Value;
        }
        // --- Reset Map Playlist ---
        public void functionEvent_ResetAvailableMaps()
        {
            dataGridView_availableMaps.Rows.Clear();
		}
        public void functionEvent_LoadCurrentMapPlaylist(bool external = false)
        {
            // Load the current map playlist from the instance manager
            instanceMaps!.currentMapPlaylist = mapInstanceManager.LoadCustomMapPlaylist(external);
            // Populate the DataGridView with the current map playlist
            functionEvent_PopulateCurrentMapPlaylist();
        }
        // --- Update Form Information and Visibility ---
        public void functionEvent_UpdateForm()
        {
            // Enable/Disable Buttons based on Server Status
            bool isOnline = (theInstance!.instanceStatus != InstanceStatus.OFFLINE);

            int nextMapIndex = theInstance!.gameInfoCurrentMapIndex >= instanceMaps!.currentMapPlaylist.Count - 1
                                || theInstance!.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance!.gameInfoCurrentMapIndex + 1;

        }
        public void functionEvent_CheckForMapChanges()
        {
            if (instanceMaps!.currentMapPlaylist.Count != dataGridView_currentMaps.Rows.Count)
            {

                return;
            }
            for (int i = 0; i < instanceMaps!.currentMapPlaylist.Count; i++)
            {
                if (instanceMaps!.currentMapPlaylist[i].MapName != dataGridView_currentMaps.Rows[i].Cells["current_MapName"].Value?.ToString())
                {
                    AppDebug.Log("tickerServerManagement", "Map Playlist Name Mismatch Detected at index " + i);
                    return;
                }
            }
        }
        // --- Ticker Hook for Maps Tab ---
        public void tickerMapsHook()
        {
            // Check if the first load is complete
            if (!_firstLoadComplete)
            {
                // Set the first load complete flag to true
                _firstLoadComplete = true;

                // Get the server settings on first load
                functionEvent_ResetAvailableMaps();                         // Populate the available maps (First Time Only)
                functionEvent_LoadCurrentMapPlaylist();                     // Load the current map playlist (First Time Only)
                functionEvent_PopulateCurrentMapPlaylist();                 // Populate the current map playlist (First Time Only)
            }

            // Update the form information
            functionEvent_CheckForMapChanges();                             // Check for Map Changes
            functionEvent_UpdateForm();                                     // Update Form to Current Map States
        }
        // --- Action Handlers ---
        private void actionClick_RefreshAvailableMaps(object sender, EventArgs e)
        {
            try
            {
                functionEvent_ResetAvailableMaps();
                MessageBox.Show("Available maps have been refreshed.", "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, "actionClick_RefreshAvailableMaps: " + ex.Message.ToString() + "\nStackTrace: " + ex.StackTrace);
            }
        }

        private void actionClick_SaveCurrentPlaylist(object sender, EventArgs e)
        {
            instanceMaps!.currentMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();
            mapInstanceManager.SaveCurrentMapPlaylist(instanceMaps!.currentMapPlaylist, false);
            MessageBox.Show("Current map playlist has been saved.", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actionClick_ClearCurrentPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        private void actionClick_ImportCurrentPlaylist(object sender, EventArgs e)
        {
            functionEvent_LoadCurrentMapPlaylist(true);
        }

        private void actionClick_ExportCurrentPlaylist(object sender, EventArgs e)
        {
            List<mapFileInfo> mapList = mapInstanceManager.BuildCurrentMapPlaylist();
            mapInstanceManager.SaveCurrentMapPlaylist(mapList, true);
        }

        private void actionClick_ResetCurrentMapPlaylist(object sender, EventArgs e)
        {
            functionEvent_PopulateCurrentMapPlaylist();
        }

        private void actionClick_CurrentPlaylistAddMap(object sender, DataGridViewCellEventArgs e)
        {
            functionEvent_AddMapToCurrentPlaylist(sender);
        }

        private void actionClick_DeleteSelectedMap(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current DataGridView of available maps
            DataGridView dataGridView = (DataGridView)sender;
            // Get row index of the clicked cell
            int rowIndex = dataGridView.CurrentCell.RowIndex;
            // Remove Row from DataGridView
            dataGridView.Rows.RemoveAt(rowIndex);
        }

        private void actionClick_UpdateMapPlaylist(object sender, EventArgs e)
        {
            if (theInstance!.instanceStatus == InstanceStatus.STARTDELAY || theInstance!.instanceStatus == InstanceStatus.ONLINE)
            {
                ServerMemory.UpdateMapCycle1();
                ServerMemory.UpdateMapCycle2();
                ServerMemory.UpdateMapListCount();
                MessageBox.Show("The server map list has been updated successfully.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The Maplist saved but server is currently not ready to recieve map updates. Please wait for Status Change of Server.", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void actionClick_PlayMapNext(object sender, EventArgs e)
        {
            int mapIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
            ServerMemory.UpdateNextMap(mapIndex);
            MessageBox.Show(dataGridView_currentMaps.Rows[mapIndex].Cells["current_MapName"].Value + " has been updated to play next.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actionClick_ScoreMap(object sender, EventArgs e)
        {
            ServerMemory.WriteMemoryScoreMap();
        }

        private void actionClick_SkipMap(object sender, EventArgs e)
        {
            if (theInstance!.instanceStatus != InstanceStatus.LOADINGMAP)
            {
                ServerMemory.WriteMemorySendConsoleCommand("resetgames");
            }
            else
            {
                MessageBox.Show("Sorry you can't skip currently, map is currently 'loading' please try again in a moment.", "Cannot Skip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void actionKey_MoveMap(object sender, KeyPressEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            int rowIndex = dataGridView.CurrentCell.RowIndex;

            bool isShift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            if (isShift && (e.KeyChar == 'W' || e.KeyChar == 'w') && rowIndex > 0)
            {
                // Move up
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                dataGridView.Rows.RemoveAt(rowIndex);
                dataGridView.Rows.Insert(rowIndex - 1, row);
                // Update MapID
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells["current_MapID"].Value = i + 1;
                }
                // Select the moved row
                dataGridView.CurrentCell = dataGridView.Rows[rowIndex - 1].Cells[dataGridView.CurrentCell.ColumnIndex];
            }
            else if (isShift && (e.KeyChar == 'S' || e.KeyChar == 's') && rowIndex < dataGridView.Rows.Count - 1)
            {
                // Move down
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                dataGridView.Rows.RemoveAt(rowIndex);
                dataGridView.Rows.Insert(rowIndex + 1, row);
                // Update MapID
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells["current_MapID"].Value = i + 1;
                }
                // Select the moved row
                dataGridView.CurrentCell = dataGridView.Rows[rowIndex + 1].Cells[dataGridView.CurrentCell.ColumnIndex];
            }
        }

    }
}
