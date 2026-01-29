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
        private mapInstance mapInstance => CommonCore.instanceMaps!;

        // --- Class Variables ---
        public int MapTypeFilter;

        public tabMaps()
        {
            InitializeComponent();
            tabMaps_loadSettings();
        }

        /// <summary>
        /// Update map control button states based on active playlist and server status
        /// </summary>
        public void methodFunction_UpdateMapControls()
        {
            bool isActivePlaylist = mapInstance.ActivePlaylist == mapInstance!.SelectedPlaylist;
            bool isServerOnline = theInstance!.instanceStatus != InstanceStatus.OFFLINE;

            btn_mapControl1.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl2.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl3.Enabled = isActivePlaylist && isServerOnline;
        }

        /// <summary>
        /// Initialize map management settings and load data
        /// </summary>
        public void tabMaps_loadSettings()
        {
            MapTypeFilter = 9; // Show all map types by default

            // Initialize via manager
            mapInstanceManager.Initialize();

            // Update UI
            btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";

            // Load available maps
            methodFunction_loadSourceMaps();

            // Set initial UI state (this will load the playlist)
            methodFunction_selectPlaylist(mapInstance.ActivePlaylist);
        }

        /// <summary>
        /// Load available source maps (default + custom)
        /// </summary>
        public void methodFunction_loadSourceMaps()
        {
            dataGridView_availableMaps.Rows.Clear();

            // Load maps via manager
            var result = mapInstanceManager.LoadAvailableMaps();

            // Add all maps to grid
            var allMaps = new List<mapFileInfo>();
            allMaps.AddRange(result.DefaultMaps);
            allMaps.AddRange(result.CustomMaps);

            foreach (var map in allMaps)
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
                row.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }

            AppDebug.Log("tabMaps", $"Loaded {allMaps.Count} available maps");
        }

        /// <summary>
        /// Add a map from available list to current playlist
        /// </summary>
        private void actionClick_playlistAddMap(object sender, DataGridViewCellEventArgs e)
        {
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

            DataGridViewRow newRow = targetGrid.Rows[newRowIndex];
            newRow.Cells[1].ToolTipText = $"Map File: {row.Cells[2].Value}";
        }

        /// <summary>
        /// Remove a map from current playlist
        /// </summary>
        private void actionClick_playlistRemoveMap(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dataGridView_currentMaps.Rows.RemoveAt(e.RowIndex);
            methodFunction_renumberPlaylistOrder(dataGridView_currentMaps);
        }

        /// <summary>
        /// Move selected row up in playlist
        /// </summary>
        private void actionClick_playlistMoveRowUp(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_currentMaps;

            if (grid.SelectedRows.Count == 0)
                return;

            var row = grid.SelectedRows[0];
            int index = row.Index;

            if (index == 0)
                return;

            grid.Rows.RemoveAt(index);
            grid.Rows.Insert(index - 1, row);

            grid.ClearSelection();
            row.Selected = true;

            methodFunction_renumberPlaylistOrder(grid);
        }

        /// <summary>
        /// Move selected row down in playlist
        /// </summary>
        private void actionClick_playlistMoveRowDown(object sender, EventArgs e)
        {
            DataGridView grid = dataGridView_currentMaps;

            if (grid.SelectedRows.Count == 0)
                return;

            var row = grid.SelectedRows[0];
            int index = row.Index;

            if (index >= grid.Rows.Count - 1)
                return;

            grid.Rows.RemoveAt(index);
            grid.Rows.Insert(index + 1, row);

            grid.ClearSelection();
            row.Selected = true;

            methodFunction_renumberPlaylistOrder(grid);
        }

        /// <summary>
        /// Renumber playlist MapID values sequentially
        /// </summary>
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

        /// <summary>
        /// Load a specific playlist into the UI
        /// </summary>
        private void methodFunction_loadPlaylist(int playlistNum)
        {
            // Refresh available maps
            methodFunction_loadSourceMaps();

            // Load playlist via manager
            var result = mapInstanceManager.LoadPlaylist(playlistNum);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error Loading Playlist",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Clear current grid
            dataGridView_currentMaps.Rows.Clear();

            // Set selected playlist
            mapInstance!.SelectedPlaylist = playlistNum;

            // Get maps from manager
            var (success, maps, error) = mapInstanceManager.GetPlaylistMaps(playlistNum);

            if (!success)
            {
                MessageBox.Show(error, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add each map to the grid
            foreach (var map in maps)
            {
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    map.MapID,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    objectGameTypes.GetShortName(map.MapType)
                );

                DataGridViewRow row = dataGridView_currentMaps.Rows[rowIndex];
                row.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }
        }

        /// <summary>
        /// Save current playlist to database
        /// </summary>
        public void actionClick_saveMapPlaylist(object sender, EventArgs e)
        {
            try
            {
                // Build playlist from grid
                var playlist = BuildPlaylistFromGrid();

                if (playlist.Count == 0)
                {
                    MessageBox.Show("Cannot save an empty playlist. Add maps before saving.",
                        "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if this is the active playlist and server is running
                if (mapInstance.ActivePlaylist == mapInstance!.SelectedPlaylist &&
                    theInstance!.instanceStatus != InstanceStatus.OFFLINE)
                {
                    if (theInstance!.instanceStatus == InstanceStatus.ONLINE ||
                        theInstance!.instanceStatus == InstanceStatus.STARTDELAY)
                    {
                        DialogResult result = MessageBox.Show(
                            "This is the active playlist. Updating will update the game-server too, continue?",
                            "Update Active Playlist",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                            return;

                        if (result == DialogResult.Yes)
                        {
                            // Backup the active playlist
                            mapInstance.Playlists[0] = mapInstance.Playlists[mapInstance.ActivePlaylist];

                            // Save via manager
                            var saveResult = mapInstanceManager.SavePlaylist(mapInstance!.SelectedPlaylist, playlist);

                            if (!saveResult.Success)
                            {
                                MessageBox.Show(saveResult.Message, "Error Saving Playlist",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Update server memory
                            var updateResult = mapInstanceManager.UpdateServerMapCycle();

                            if (!updateResult.Success)
                            {
                                MessageBox.Show(updateResult.Message, "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            // Update UI
                            btn_activePlaylist.Text = $"P{mapInstance!.SelectedPlaylist}";
                            mapInstance.ActivePlaylist = mapInstance!.SelectedPlaylist;

                            MessageBox.Show($"Map Playlist {mapInstance!.SelectedPlaylist} saved and in-game rotation updated.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                // Save via manager
                var playlistResult = mapInstanceManager.SavePlaylist(mapInstance!.SelectedPlaylist, playlist);

                if (playlistResult.Success)
                {
                    MessageBox.Show($"Map Playlist {mapInstance!.SelectedPlaylist} has been saved.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(playlistResult.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving playlist: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("tabMaps", $"actionClick_saveMapPlaylist: {ex.Message}");
            }
        }

        /// <summary>
        /// Build playlist from current DataGridView
        /// </summary>
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

        /// <summary>
        /// Playlist selection button handlers
        /// </summary>
        private void actionClick_loadMapPlaylist1(object sender, EventArgs e) => methodFunction_selectPlaylist(1);
        private void actionClick_loadMapPlaylist2(object sender, EventArgs e) => methodFunction_selectPlaylist(2);
        private void actionClick_loadMapPlaylist3(object sender, EventArgs e) => methodFunction_selectPlaylist(3);
        private void actionClick_loadMapPlaylist4(object sender, EventArgs e) => methodFunction_selectPlaylist(4);
        private void actionClick_loadMapPlaylist5(object sender, EventArgs e) => methodFunction_selectPlaylist(5);

        /// <summary>
        /// Select and load a specific playlist
        /// </summary>
        private void methodFunction_selectPlaylist(int playlistID)
        {
            Color selected = SystemColors.ActiveBorder;
            Color notSelected = Button.DefaultBackColor;

            // Set selected playlist
            mapInstance!.SelectedPlaylist = playlistID;

            // Update button colors
            btn_loadPlaylist1.BackColor = playlistID == 1 ? selected : notSelected;
            btn_loadPlaylist2.BackColor = playlistID == 2 ? selected : notSelected;
            btn_loadPlaylist3.BackColor = playlistID == 3 ? selected : notSelected;
            btn_loadPlaylist4.BackColor = playlistID == 4 ? selected : notSelected;
            btn_loadPlaylist5.BackColor = playlistID == 5 ? selected : notSelected;

            // Update active playlist display
            btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";

            // Update map controls
            methodFunction_UpdateMapControls();

            // Load playlist
            methodFunction_loadPlaylist(playlistID);
        }

        /// <summary>
        /// Clear current playlist
        /// </summary>
        private void actionClick_clearMapPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        /// <summary>
        /// Set current playlist as active
        /// </summary>
        private void actionClick_setPlaylistActive(object sender, EventArgs e)
        {
            var playlist = BuildPlaylistFromGrid();

            if (playlist.Count == 0)
            {
                MessageBox.Show("No maps in the currently selected playlist. Add maps, save and try again.",
                    "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if already active
            if (mapInstance.ActivePlaylist == mapInstance!.SelectedPlaylist)
            {
                MessageBox.Show($"Playlist {mapInstance!.SelectedPlaylist} is already the active playlist.",
                    "Already Active", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirm if server is running
            if (theInstance!.instanceStatus == InstanceStatus.ONLINE ||
                theInstance!.instanceStatus == InstanceStatus.STARTDELAY)
            {
                DialogResult result = MessageBox.Show(
                    "Changing the playlist will update the in-game map rotation and restart from the beginning. Do you wish to continue?",
                    "Confirm Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            // Save playlist first
            var saveResult = mapInstanceManager.SavePlaylist(mapInstance!.SelectedPlaylist, playlist);

            if (!saveResult.Success)
            {
                MessageBox.Show(saveResult.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set as active via manager
            var activateResult = mapInstanceManager.SetActivePlaylist(mapInstance!.SelectedPlaylist);

            if (activateResult.Success)
            {
                // Update UI
                btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";

                MessageBox.Show($"Playlist {mapInstance.ActivePlaylist} is now active.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(activateResult.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refresh all map lists
        /// </summary>
        private void actionClick_refeshMapLists(object sender, EventArgs e)
        {
            methodFunction_loadSourceMaps();
            methodFunction_hideSourceRows(MapTypeFilter);
            methodFunction_selectPlaylist(mapInstance!.SelectedPlaylist);
        }

        /// <summary>
        /// Filter available maps by type
        /// </summary>
        private void methodFunction_hideSourceRows(int filterInt)
        {
            Color colorActive = SystemColors.ActiveBorder;
            Color notActive = Button.DefaultBackColor;

            MapTypeFilter = filterInt;

            // Update filter button colors
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

            // Apply filter
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
                    row.Visible = (mapType == filterInt);
                }
            }
        }

        /// <summary>
        /// Map type filter button handlers
        /// </summary>
        private void actionClick_filterMapType0(object sender, EventArgs e) => methodFunction_hideSourceRows(0);
        private void actionClick_filterMapType1(object sender, EventArgs e) => methodFunction_hideSourceRows(1);
        private void actionClick_filterMapType2(object sender, EventArgs e) => methodFunction_hideSourceRows(2);
        private void actionClick_filterMapType3(object sender, EventArgs e) => methodFunction_hideSourceRows(3);
        private void actionClick_filterMapType4(object sender, EventArgs e) => methodFunction_hideSourceRows(4);
        private void actionClick_filterMapType5(object sender, EventArgs e) => methodFunction_hideSourceRows(5);
        private void actionClick_filterMapType6(object sender, EventArgs e) => methodFunction_hideSourceRows(6);
        private void actionClick_filterMapType7(object sender, EventArgs e) => methodFunction_hideSourceRows(7);
        private void actionClick_filterMapType8(object sender, EventArgs e) => methodFunction_hideSourceRows(8);
        private void actionClick_filterMapType9(object sender, EventArgs e) => methodFunction_hideSourceRows(9);

        /// <summary>
        /// Set next map to play
        /// </summary>
        private void actionClick_mapPlayNext(object sender, EventArgs e)
        {
            if (dataGridView_currentMaps.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a map from the playlist.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridView grid = dataGridView_currentMaps;
            var row = grid.SelectedRows[0];
            int index = row.Index;

            var result = mapInstanceManager.SetNextMap(index);

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Score current map
        /// </summary>
        private void actionClick_ScoreMap(object sender, EventArgs e)
        {
            var result = mapInstanceManager.ScoreMap();

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Skip current map
        /// </summary>
        private void actionClick_SkipMap(object sender, EventArgs e)
        {
            var result = mapInstanceManager.SkipMap();

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Cannot Skip",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Backup playlist to JSON file
        /// </summary>
        public void actionClick_backupPlaylist(object sender, EventArgs e)
        {
            var playlist = BuildPlaylistFromGrid();

            if (playlist.Count == 0)
            {
                MessageBox.Show("Cannot backup an empty playlist.",
                    "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                DefaultExt = "json",
                FileName = $"Playlist_{mapInstance!.SelectedPlaylist}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var result = mapInstanceManager.ExportPlaylistToJson(mapInstance!.SelectedPlaylist, saveDialog.FileName);

                if (result.Success)
                {
                    MessageBox.Show($"Playlist {mapInstance!.SelectedPlaylist} backed up successfully to:\n{saveDialog.FileName}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Import playlist from JSON file
        /// </summary>
        public void actionClick_importPlaylist(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                DefaultExt = "json"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var (success, maps, importedCount, skippedCount, errorMessage) =
                    mapInstanceManager.ImportPlaylistFromJson(openDialog.FileName);

                if (!success)
                {
                    MessageBox.Show(errorMessage, "Import Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Clear current playlist
                dataGridView_currentMaps.Rows.Clear();

                // Add imported maps to grid
                foreach (var map in maps)
                {
                    int rowIndex = dataGridView_currentMaps.Rows.Add(
                        dataGridView_currentMaps.Rows.Count + 1,
                        map.MapName,
                        map.MapFile,
                        map.ModType,
                        map.MapType,
                        objectGameTypes.GetShortName(map.MapType)
                    );

                    DataGridViewRow newRow = dataGridView_currentMaps.Rows[rowIndex];
                    newRow.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
                }

                // Show results
                string message = $"Import complete.\n\nImported: {importedCount} maps";
                if (skippedCount > 0)
                {
                    message += $"\nSkipped: {skippedCount} unavailable maps";
                }
                message += "\n\nRemember to save the playlist to apply changes.";

                MessageBox.Show(message, "Import Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Randomize current playlist order
        /// </summary>
        public void actionClick_randomizePlaylist(object sender, EventArgs e)
        {
            var playlist = BuildPlaylistFromGrid();

            if (playlist.Count == 0)
            {
                MessageBox.Show("Cannot randomize an empty playlist.",
                    "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Randomize via manager
            var result = mapInstanceManager.RandomizePlaylist(playlist);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Clear and repopulate grid with randomized order
            dataGridView_currentMaps.Rows.Clear();

            foreach (var map in playlist)
            {
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    map.MapID,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    objectGameTypes.GetShortName(map.MapType)
                );

                dataGridView_currentMaps.Rows[rowIndex].Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }

            MessageBox.Show(
                $"Playlist {mapInstance!.SelectedPlaylist} has been randomized.\n\nRemember to save the playlist to apply changes.",
                "Playlist Randomized",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Update map highlighting in current playlist grid (called by ticker)
        /// Shows current map in green, next map in blue, or yellow if looping same map
        /// </summary>
        public void UpdateCurrentMapHighlighting()
        {



            // Clear all row backgrounds first
            foreach (DataGridViewRow row in dataGridView_currentMaps.Rows)
            {
                if (row.IsNewRow) continue;
                row.DefaultCellStyle.BackColor = Color.White;
            }

            // Only highlight if viewing the active playlist and server is online
            if (mapInstance.ActivePlaylist != mapInstance.SelectedPlaylist)
                return;

            if (theInstance?.instanceStatus == InstanceStatus.OFFLINE)
                return;

            // Use the stable "actually playing" index for UI display
            int actualCurrentMapIndex = mapInstance.ActualPlayingMapIndex;

            int currentMapIndex = mapInstance.CurrentMapIndex;

            // Calculate next map naturally from the actual current map
            int nextMapIndex = mapInstance.CurrentMapIndex + 1;
            
            if (nextMapIndex >= dataGridView_currentMaps.Rows.Count)
            {
                nextMapIndex = 0; // Loop to beginning
            }
            
            AppDebug.Log("UpdateCurrentMapHighlighting", $"Map Index: Actual: {actualCurrentMapIndex} Current: {currentMapIndex} Next: {nextMapIndex} Row Count: {dataGridView_currentMaps.Rows.Count}");                

            // Check if current and next are the same (looping same map)
            if (actualCurrentMapIndex == nextMapIndex)
            {
                // Yellow for looping the same map
                dataGridView_currentMaps.Rows[actualCurrentMapIndex].DefaultCellStyle.BackColor = Color.Yellow;
            }
            else
            {

                if(theInstance!.instanceStatus != InstanceStatus.LOADINGMAP || theInstance!.instanceStatus != InstanceStatus.SCORING)
                {
                    dataGridView_currentMaps.Rows[actualCurrentMapIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }               

                // Next map is blue
                dataGridView_currentMaps.Rows[nextMapIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }
        }
}