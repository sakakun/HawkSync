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
        private Dictionary<int, List<mapFileInfo>> MapPlaylists => CommonCore.theInstance!.MapPlaylists;

        // Playlists
        public int MapTypeFilter;                           // Source Map Filter (9=All)

        public tabMaps()
        {
            InitializeComponent();
            tabMaps_loadSettings();
        }

        public void methodFunction_UpdateMapControls()
        {
            // Disable Playnext if Active Playlist isn't selected.
            btn_mapControl1.Enabled = (theInstance!.ActiveMapPlaylist == theInstance!.SelectedMapPlaylist && theInstance!.instanceStatus != InstanceStatus.OFFLINE);
            btn_mapControl2.Enabled = (theInstance!.ActiveMapPlaylist == theInstance!.SelectedMapPlaylist && theInstance!.instanceStatus != InstanceStatus.OFFLINE);
            btn_mapControl3.Enabled = (theInstance!.ActiveMapPlaylist == theInstance!.SelectedMapPlaylist && theInstance!.instanceStatus != InstanceStatus.OFFLINE);
        }


        public void tabMaps_loadSettings()
        {
            // Stage Playlists
            MapPlaylists[0] = new List<mapFileInfo>();
            MapPlaylists[1] = new List<mapFileInfo>();
            MapPlaylists[2] = new List<mapFileInfo>();
            MapPlaylists[3] = new List<mapFileInfo>();
            MapPlaylists[4] = new List<mapFileInfo>();
            MapPlaylists[5] = new List<mapFileInfo>();

            MapTypeFilter = 9;

            // Active Playlist being used by Game Server
            theInstance!.ActiveMapPlaylist = ServerSettings.Get("ActiveMapPlaylist", 1);
            theInstance!.SelectedMapPlaylist = ServerSettings.Get("ActiveMapPlaylist", 1);

            // Load the maps
            methodFunction_loadSourceMaps();

            // Update active playlist display
            btn_activePlaylist.Text = $"P{theInstance!.ActiveMapPlaylist}";

            // Set initial UI state (this will load the playlist)
            methodFunction_selectPlaylist(theInstance!.ActiveMapPlaylist);
        }

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
            string gamePath = ServerSettings.Get("serverGamePath", string.Empty);
            if (gamePath == string.Empty)
            {
                return;
            }

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
                        first_line = File.ReadLines(Path.Combine(gamePath, file.Name), Encoding.Default).First().ToString();
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
        private void actionClick_playlistAddMap(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header or invalid rows
            if (e.RowIndex < 0)
                return;

            var sourceGrid = dataGridView_availableMaps;
            var targetGrid = dataGridView_currentMaps;

            DataGridViewRow row = sourceGrid.Rows[e.RowIndex];

            int newRowIndex = targetGrid.Rows.Add(
                targetGrid.Rows.Count + 1,
                row.Cells[1].Value,
                row.Cells[2].Value,
                row.Cells[3].Value,
                row.Cells[4].Value,
                objectGameTypes.GetShortName((int)row.Cells[4].Value)
             );

            DataGridViewRow newRow = dataGridView_currentMaps.Rows[newRowIndex];

            string toolTip = $"Map File: {row.Cells[2].Value}";

            newRow.Cells[1].ToolTipText = toolTip;

        }
        private void actionClick_playlistRemoveMap(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header
            if (e.RowIndex < 0)
                return;

            var targetGrid = dataGridView_currentMaps;

            targetGrid.Rows.RemoveAt(e.RowIndex);

            methodFunction_renumberPlaylistOrder(targetGrid);
        }
        private void actionClick_playlistMoveRowUp(object sender, EventArgs e)
        {

            DataGridView grid = dataGridView_currentMaps;

            if (grid.SelectedRows.Count == 0)
                return;

            var row = grid.SelectedRows[0];
            int index = row.Index;

            // Already at top
            if (index == 0)
                return;

            grid.Rows.RemoveAt(index);
            grid.Rows.Insert(index - 1, row);

            grid.ClearSelection();
            row.Selected = true;

            methodFunction_renumberPlaylistOrder(grid);
        }
        private void actionClick_playlistMoveRowDown(object sender, EventArgs e)
        {

            DataGridView grid = dataGridView_currentMaps;

            if (grid.SelectedRows.Count == 0)
                return;

            var row = grid.SelectedRows[0];
            int index = row.Index;

            // Already at bottom
            if (index >= grid.Rows.Count - 1)
                return;

            grid.Rows.RemoveAt(index);
            grid.Rows.Insert(index + 1, row);

            grid.ClearSelection();
            row.Selected = true;

            methodFunction_renumberPlaylistOrder(grid);
        }
        private void methodFunction_renumberPlaylistOrder(DataGridView grid)
        {
            int order = 1;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                row.Cells[0].Value = order++;
            }
        }
        private void methodFunction_loadPlaylist(int playlistNum)
        {
            // Refresh Available Maps
            methodFunction_loadSourceMaps();

            // Grab the Playlist from the DB
            MapPlaylists[playlistNum] = DatabaseManager.GetPlaylistMaps(playlistNum);

            // Clear the current playlist in datagrid
            dataGridView_currentMaps.Rows.Clear();

            // Set the Selected Playlist for references.
            theInstance!.SelectedMapPlaylist = playlistNum;

            // Add each record, in playlist to the datagrid
            foreach (mapFileInfo map in MapPlaylists[playlistNum])
            {
                // Map Validation (Is Map Still Available)


                // Add Map to DataGridTable
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    map.MapID,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    objectGameTypes.GetShortName(map.MapType)
                );
                DataGridViewRow row = dataGridView_currentMaps.Rows[rowIndex];
                string toolTip = $"Map File: {map.MapFile}";
                row.Cells[1].ToolTipText = toolTip;
            }
        }
        public void actionClick_saveMapPlaylist(object sender, EventArgs e)
        {
            try
            {
                // Don't allow saving empty playlists
                if (dataGridView_currentMaps.Rows.Count == 0)
                {
                    MessageBox.Show("Cannot save an empty playlist. Add maps before saving.");
                    return;
                }

                // If this is the active playlist and server is running, update the game
                if (theInstance!.ActiveMapPlaylist == theInstance!.SelectedMapPlaylist && theInstance!.instanceStatus != InstanceStatus.OFFLINE)
                {
                    if (theInstance!.instanceStatus == InstanceStatus.ONLINE || theInstance!.instanceStatus == InstanceStatus.STARTDELAY)
                    {
                        DialogResult result = MessageBox.Show(
                            "This is the active playlist. Updating will update the game-server too, continue?",
                            "Update Active Playlist",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            // Backup the Active Playlist (for UpdateMapCycle1 to use as old truth)
                            MapPlaylists[0] = MapPlaylists[theInstance!.ActiveMapPlaylist];

                            // Build Playlist to Memory
                            MapPlaylists[theInstance!.SelectedMapPlaylist] = BuildPlaylistFromGrid();
                            // Save to Database
                            DatabaseManager.SavePlaylist(theInstance!.SelectedMapPlaylist, MapPlaylists[theInstance!.SelectedMapPlaylist]);

                            ServerMemory.UpdateMapCycle1();
                            ServerMemory.UpdateMapCycle2();
                            ServerMemory.UpdateMapListCount();

                            // Update UI
                            btn_activePlaylist.Text = $"P{theInstance!.SelectedMapPlaylist}";
                            theInstance!.ActiveMapPlaylist = theInstance!.SelectedMapPlaylist;

                            MessageBox.Show($"Map Playlist {theInstance!.SelectedMapPlaylist} saved and in-game rotation updated.");
                            return;
                        }
                    }
                }

                // Build Playlist to Memory
                MapPlaylists[theInstance!.SelectedMapPlaylist] = BuildPlaylistFromGrid();

                // Save to Database
                DatabaseManager.SavePlaylist(theInstance!.SelectedMapPlaylist, MapPlaylists[theInstance!.SelectedMapPlaylist]);

                MessageBox.Show($"Map Playlist {theInstance!.SelectedMapPlaylist} has been saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Map Playlist {theInstance!.SelectedMapPlaylist} failed to save.");
                AppDebug.Log("tabMaps:", "actionClick_saveMapPlaylist: " + ex.Message);
            }
        }

        // Builds the Maplist and stores it as an List<mapFileInfo>
        private List<mapFileInfo> BuildPlaylistFromGrid()
        {
            var list = new List<mapFileInfo>();

            DataGridView grid = dataGridView_currentMaps;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                list.Add(new mapFileInfo
                {
                    MapID = (int)row.Cells[0].Value,
                    MapName = row.Cells[1].Value?.ToString()!,
                    MapFile = row.Cells[2].Value?.ToString()!,
                    ModType = (int)row.Cells[3].Value,
                    MapType = (int)row.Cells[4].Value
                });
            }

            return list;
        }

        private void actionClick_loadMapPlaylist1(object sender, EventArgs e)
        {
            methodFunction_selectPlaylist(1);
        }
        private void actionClick_loadMapPlaylist2(object sender, EventArgs e)
        {
            methodFunction_selectPlaylist(2);
        }
        private void actionClick_loadMapPlaylist3(object sender, EventArgs e)
        {
            methodFunction_selectPlaylist(3);
        }
        private void actionClick_loadMapPlaylist4(object sender, EventArgs e)
        {
            methodFunction_selectPlaylist(4);
        }
        private void actionClick_loadMapPlaylist5(object sender, EventArgs e)
        {
            methodFunction_selectPlaylist(5);
        }
        private void methodFunction_selectPlaylist(int playlistID)
        {
            Color selected = SystemColors.ActiveBorder;
            Color notSelected = Button.DefaultBackColor;

            // Set Selected Play List
            theInstance!.SelectedMapPlaylist = playlistID;

            // Change Backcolors
            btn_loadPlaylist1.BackColor = playlistID == 1 ? selected : notSelected;
            btn_loadPlaylist2.BackColor = playlistID == 2 ? selected : notSelected;
            btn_loadPlaylist3.BackColor = playlistID == 3 ? selected : notSelected;
            btn_loadPlaylist4.BackColor = playlistID == 4 ? selected : notSelected;
            btn_loadPlaylist5.BackColor = playlistID == 5 ? selected : notSelected;

            // Update active playlist display
            btn_activePlaylist.Text = $"P{theInstance!.ActiveMapPlaylist}";

            // Update Map Controls
            methodFunction_UpdateMapControls();

            // Load Playlist
            methodFunction_loadPlaylist(playlistID);
        }

        private void actionClick_clearMapPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        private void actionClick_setPlaylistActive(object sender, EventArgs e)
        {
            if (dataGridView_currentMaps.Rows.Count == 0)
            {
                MessageBox.Show("No maps in the currently selected playlist. Add maps, save and try again.");
                return;
            }

            if (theInstance!.instanceStatus != InstanceStatus.OFFLINE && theInstance!.instanceStatus == InstanceStatus.SCORING)
            {
                MessageBox.Show("Server is currently in the process of changing maps, please wait until the game is running to change the map list.");
                return;
            }

            // If already active, nothing to do
            if (theInstance!.ActiveMapPlaylist == theInstance!.SelectedMapPlaylist)
            {
                MessageBox.Show($"Playlist {theInstance!.SelectedMapPlaylist} is already the active playlist.");
                return;
            }

            if (theInstance!.instanceStatus == InstanceStatus.ONLINE || theInstance!.instanceStatus == InstanceStatus.STARTDELAY)
            {
                DialogResult result = MessageBox.Show(
                    "Changing the playlist will update the in-game map rotation and restart from the beginning. Do you wish to continue?",
                    "Confirm Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            // Backup the CURRENT active playlist BEFORE changing (for UpdateMapCycle1)
            MapPlaylists[0] = MapPlaylists[theInstance!.ActiveMapPlaylist];

            // Build and save the selected playlist
            MapPlaylists[theInstance!.SelectedMapPlaylist] = BuildPlaylistFromGrid();

            // Re-validate after building
            if (MapPlaylists[theInstance!.SelectedMapPlaylist].Count == 0)
            {
                MessageBox.Show("Cannot set an empty playlist as active.");
                return;
            }

            DatabaseManager.SavePlaylist(theInstance!.SelectedMapPlaylist, MapPlaylists[theInstance!.SelectedMapPlaylist]);

            // Set current playlist to active
            theInstance!.ActiveMapPlaylist = theInstance!.SelectedMapPlaylist;

            // Update UI button
            btn_activePlaylist.Text = $"P{theInstance!.ActiveMapPlaylist}";

            // Save the active playlist setting
            ServerSettings.Set("ActiveMapPlaylist", theInstance!.ActiveMapPlaylist);

            // If server is running, update the in-game rotation
            if (theInstance!.instanceStatus == InstanceStatus.ONLINE || theInstance!.instanceStatus == InstanceStatus.STARTDELAY)
            {
                ServerMemory.UpdateMapCycle1();
                ServerMemory.UpdateMapCycle2();
            }

            MessageBox.Show($"Playlist {theInstance!.ActiveMapPlaylist} is now active.");
        }

        private void actionClick_refeshMapLists(object sender, EventArgs e)
        {
            // Load the maps
            methodFunction_loadSourceMaps();

            // Reapply current filter
            methodFunction_hideSourceRows(MapTypeFilter);

            // Refresh the playlist data grid
            methodFunction_selectPlaylist(theInstance!.SelectedMapPlaylist);

        }

        private void methodFunction_hideSourceRows(int filterInt)
        {
            Color colorActive = SystemColors.ActiveBorder;
            Color notActive = Button.DefaultBackColor;

            // Set Filter Number
            MapTypeFilter = filterInt;

            btn_mapTypeALL.BackColor = filterInt == 9 ? colorActive : notActive;
            btn_mapTypeDM.BackColor = filterInt == 0 ? colorActive : notActive;
            btn_mapTypeTDM.BackColor = filterInt == 1 ? colorActive : notActive;
            btn_mapTypeCOOP.BackColor = filterInt == 2 ? colorActive : notActive;
            btn_mapTypeTKOTH.BackColor = filterInt == 3 ? colorActive : notActive;
            btn_mapTypeKOTH.BackColor = filterInt == 4 ? colorActive : notActive;
            btn_mapTypeSD.BackColor = filterInt == 5 ? colorActive : notActive;
            btn_mapTypeAD.BackColor = filterInt == 6 ? colorActive : notActive;
            btn_mapTypeCTF.BackColor = filterInt == 7 ? colorActive : notActive;
            btn_mapTypeFB.BackColor = filterInt == 8 ? colorActive : notActive;

            if (MapTypeFilter == 9)
            {
                foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                {
                    if (row.IsNewRow) continue;

                    row.Visible = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                {
                    if (row.IsNewRow) continue;
                    int mapType = (int)row.Cells[4].Value;

                    if (mapType == filterInt)
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }

                }
            }
        }

        private void actionClick_filterMapType0(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(0);
        }

        private void actionClick_filterMapType1(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(1);
        }

        private void actionClick_filterMapType2(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(2);
        }

        private void actionClick_filterMapType3(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(3);
        }

        private void actionClick_filterMapType4(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(4);
        }

        private void actionClick_filterMapType5(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(5);
        }

        private void actionClick_filterMapType6(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(6);
        }

        private void actionClick_filterMapType7(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(7);
        }

        private void actionClick_filterMapType8(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(8);
        }

        private void actionClick_filterMapType9(object sender, EventArgs e)
        {
            methodFunction_hideSourceRows(9);
        }

        private void actionClick_mapPlayNext(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_currentMaps;
            var row = grid.SelectedRows[0];
            int index = row.Index;

            ServerMemory.UpdateNextMap(index);

        }

        private void actionClick_ScoreMap(object sender, EventArgs e)
        {
            ServerMemory.ScoreMap();
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
    }
}
