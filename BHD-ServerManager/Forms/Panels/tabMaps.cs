using BHD_ServerManager.Classes.GameManagement;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Data;
using System.Text;
using HawkSyncShared.DTOs.tabMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabMaps : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        private mapInstance mapInstance => CommonCore.instanceMaps!;

        // --- Class Variables ---
        private const int MapTypeAll = 9;
        private const int PlaylistMin = 1;
        private const int PlaylistMax = 5;
        public int MapTypeFilter;

        public tabMaps()
        {
            InitializeComponent();
            LoadSettings();

            CommonCore.Ticker?.Start("tabMaps", 1000, Ticker_tabMaps);
        }

        private void Ticker_tabMaps()
        {
            UpdateMapControls();
            UpdateCurrentMapHighlighting();
        }

        /// <summary>
        /// Update map control button states based on active playlist and server status
        /// </summary>
        public void UpdateMapControls()
        {
            bool isActivePlaylist = mapInstance.ActivePlaylist == mapInstance.SelectedPlaylist;
            bool isServerOnline = theInstance?.instanceStatus != InstanceStatus.OFFLINE;

            btn_mapControl1.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl2.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl3.Enabled = isActivePlaylist && isServerOnline;
        }

        /// <summary>
        /// Initialize map management settings and load data
        /// </summary>
        public void LoadSettings()
        {
            MapTypeFilter = MapTypeAll;

            mapInstanceManager.Initialize();

            btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";

            LoadSourceMaps();
            SelectPlaylist(mapInstance.ActivePlaylist);
        }

        /// <summary>
        /// Load available source maps (default + custom)
        /// </summary>
        public void LoadSourceMaps()
        {
            dataGridView_availableMaps.Rows.Clear();

            var result = mapInstanceManager.LoadAvailableMaps();

            var allMaps = new List<MapObject>();
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
                    GameTypeObject.GetShortName(map.MapType)
                );

                DataGridViewRow row = dataGridView_availableMaps.Rows[rowIndex];
                row.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }

            AppDebug.Log("tabMaps", $"Loaded {allMaps.Count} available maps");
        }

        private void OnPlaylistAddMap(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var sourceGrid = dataGridView_availableMaps;
            var targetGrid = dataGridView_currentMaps;

            DataGridViewRow row = sourceGrid.Rows[e.RowIndex];

            int newRowIndex = targetGrid.Rows.Add(
                targetGrid.Rows.Count + 1,
                row.Cells[1].Value!,
                row.Cells[2].Value!,
                row.Cells[3].Value!,
                row.Cells[4].Value!,
                GameTypeObject.GetShortName((int)row.Cells[4].Value!)
            );

            DataGridViewRow newRow = targetGrid.Rows[newRowIndex];
            newRow.Cells[1].ToolTipText = $"Map File: {row.Cells[2].Value}";
        }

        private void OnPlaylistRemoveMap(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dataGridView_currentMaps.Rows.RemoveAt(e.RowIndex);
            RenumberPlaylistOrder(dataGridView_currentMaps);
        }

        private void OnPlaylistMoveRowUp(object sender, EventArgs e)
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

            RenumberPlaylistOrder(grid);
        }

        private void OnPlaylistMoveRowDown(object sender, EventArgs e)
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

            RenumberPlaylistOrder(grid);
        }

        private void RenumberPlaylistOrder(DataGridView grid)
        {
            int order = 1;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;
                row.Cells[0].Value = order++;
            }
        }

        private void LoadPlaylist(int playlistNum)
        {
            LoadSourceMaps();

            var result = mapInstanceManager.LoadPlaylist(playlistNum);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error Loading Playlist",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridView_currentMaps.Rows.Clear();
            mapInstance.SelectedPlaylist = playlistNum;

            var (success, maps, error) = mapInstanceManager.GetPlaylistMaps(playlistNum);

            if (!success)
            {
                MessageBox.Show(error, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var map in maps)
            {
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    map.MapID,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    GameTypeObject.GetShortName(map.MapType)
                );

                DataGridViewRow row = dataGridView_currentMaps.Rows[rowIndex];
                row.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }
        }

        public void OnSaveMapPlaylist(object sender, EventArgs e)
        {
            try
            {
                var playlist = BuildPlaylistFromGrid();

                if (playlist.Count == 0)
                {
                    MessageBox.Show("Cannot save an empty playlist. Add maps before saving.",
                        "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (mapInstance.ActivePlaylist == mapInstance.SelectedPlaylist &&
                    theInstance?.instanceStatus != InstanceStatus.OFFLINE)
                {
                    if (theInstance?.instanceStatus == InstanceStatus.ONLINE ||
                        theInstance?.instanceStatus == InstanceStatus.STARTDELAY)
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
                            mapInstance.Playlists[0] = mapInstance.Playlists[mapInstance.ActivePlaylist];

                            var saveResult = mapInstanceManager.SavePlaylist(mapInstance.SelectedPlaylist, playlist);

                            if (!saveResult.Success)
                            {
                                MessageBox.Show(saveResult.Message, "Error Saving Playlist",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            var updateResult = mapInstanceManager.UpdateServerMapCycle();

                            if (!updateResult.Success)
                            {
                                MessageBox.Show(updateResult.Message, "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            btn_activePlaylist.Text = $"P{mapInstance.SelectedPlaylist}";
                            mapInstance.ActivePlaylist = mapInstance.SelectedPlaylist;

                            MessageBox.Show($"Map Playlist {mapInstance.SelectedPlaylist} saved and in-game rotation updated.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                var playlistResult = mapInstanceManager.SavePlaylist(mapInstance.SelectedPlaylist, playlist);

                if (playlistResult.Success)
                {
                    MessageBox.Show($"Map Playlist {mapInstance.SelectedPlaylist} has been saved.",
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
                AppDebug.Log("tabMaps", $"OnSaveMapPlaylist: {ex.Message}");
            }
        }

        private List<MapObject> BuildPlaylistFromGrid()
        {
            var list = new List<MapObject>();
            DataGridView grid = dataGridView_currentMaps;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                list.Add(new MapObject
                {
                    MapID = (int)row.Cells[0].Value!,
                    MapName = row.Cells[1].Value?.ToString()!,
                    MapFile = row.Cells[2].Value?.ToString()!,
                    ModType = (int)row.Cells[3].Value!,
                    MapType = (int)row.Cells[4].Value!
                });
            }

            return list;
        }

        // Playlist selection button handlers
        private void OnLoadMapPlaylist1(object sender, EventArgs e) => SelectPlaylist(1);
        private void OnLoadMapPlaylist2(object sender, EventArgs e) => SelectPlaylist(2);
        private void OnLoadMapPlaylist3(object sender, EventArgs e) => SelectPlaylist(3);
        private void OnLoadMapPlaylist4(object sender, EventArgs e) => SelectPlaylist(4);
        private void OnLoadMapPlaylist5(object sender, EventArgs e) => SelectPlaylist(5);

        private void SelectPlaylist(int playlistID)
        {
            Color selected = SystemColors.ActiveBorder;
            Color notSelected = Button.DefaultBackColor;

            mapInstance.SelectedPlaylist = playlistID;

            btn_loadPlaylist1.BackColor = playlistID == 1 ? selected : notSelected;
            btn_loadPlaylist2.BackColor = playlistID == 2 ? selected : notSelected;
            btn_loadPlaylist3.BackColor = playlistID == 3 ? selected : notSelected;
            btn_loadPlaylist4.BackColor = playlistID == 4 ? selected : notSelected;
            btn_loadPlaylist5.BackColor = playlistID == 5 ? selected : notSelected;

            btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";

            LoadPlaylist(playlistID);
        }

        private void OnClearMapPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        private void OnSetPlaylistActive(object sender, EventArgs e)
        {
            var playlist = BuildPlaylistFromGrid();

            if (playlist.Count == 0)
            {
                MessageBox.Show("No maps in the currently selected playlist. Add maps, save and try again.",
                    "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mapInstance.ActivePlaylist == mapInstance.SelectedPlaylist)
            {
                MessageBox.Show($"Playlist {mapInstance.SelectedPlaylist} is already the active playlist.",
                    "Already Active", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (theInstance?.instanceStatus == InstanceStatus.ONLINE ||
                theInstance?.instanceStatus == InstanceStatus.STARTDELAY)
            {
                DialogResult result = MessageBox.Show(
                    "Changing the playlist will update the in-game map rotation and restart from the beginning. Do you wish to continue?",
                    "Confirm Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            var saveResult = mapInstanceManager.SavePlaylist(mapInstance.SelectedPlaylist, playlist);

            if (!saveResult.Success)
            {
                MessageBox.Show(saveResult.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var activateResult = mapInstanceManager.SetActivePlaylist(mapInstance.SelectedPlaylist);

            if (activateResult.Success)
            {
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

        public void OnRefreshMapLists(object sender, EventArgs e)
        {
            LoadSourceMaps();
            HideSourceRows(MapTypeFilter);
            SelectPlaylist(mapInstance.SelectedPlaylist);
        }

        private void HideSourceRows(int filterInt)
        {
            Color colorActive = SystemColors.ActiveBorder;
            Color notActive = Button.DefaultBackColor;

            MapTypeFilter = filterInt;

            btn_mapTypeALL.BackColor = filterInt == MapTypeAll ? colorActive : notActive;
            btn_mapTypeDM.BackColor = filterInt == 0 ? colorActive : notActive;
            btn_mapTypeTDM.BackColor = filterInt == 1 ? colorActive : notActive;
            btn_mapTypeCOOP.BackColor = filterInt == 2 ? colorActive : notActive;
            btn_mapTypeTKOTH.BackColor = filterInt == 3 ? colorActive : notActive;
            btn_mapTypeKOTH.BackColor = filterInt == 4 ? colorActive : notActive;
            btn_mapTypeSD.BackColor = filterInt == 5 ? colorActive : notActive;
            btn_mapTypeAD.BackColor = filterInt == 6 ? colorActive : notActive;
            btn_mapTypeCTF.BackColor = filterInt == 7 ? colorActive : notActive;
            btn_mapTypeFB.BackColor = filterInt == 8 ? colorActive : notActive;

            if (MapTypeFilter == MapTypeAll)
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
                    int mapType = (int)row.Cells[4].Value!;
                    row.Visible = (mapType == filterInt);
                }
            }
        }

        // Map type filter button handlers
        private void OnFilterMapType0(object sender, EventArgs e) => HideSourceRows(0);
        private void OnFilterMapType1(object sender, EventArgs e) => HideSourceRows(1);
        private void OnFilterMapType2(object sender, EventArgs e) => HideSourceRows(2);
        private void OnFilterMapType3(object sender, EventArgs e) => HideSourceRows(3);
        private void OnFilterMapType4(object sender, EventArgs e) => HideSourceRows(4);
        private void OnFilterMapType5(object sender, EventArgs e) => HideSourceRows(5);
        private void OnFilterMapType6(object sender, EventArgs e) => HideSourceRows(6);
        private void OnFilterMapType7(object sender, EventArgs e) => HideSourceRows(7);
        private void OnFilterMapType8(object sender, EventArgs e) => HideSourceRows(8);
        private void OnFilterMapType9(object sender, EventArgs e) => HideSourceRows(MapTypeAll);

        private void OnMapPlayNext(object sender, EventArgs e)
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

        private void OnScoreMap(object sender, EventArgs e)
        {
            var result = mapInstanceManager.ScoreMap();

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSkipMap(object sender, EventArgs e)
        {
            var result = mapInstanceManager.SkipMap();

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Cannot Skip",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void OnBackupPlaylist(object sender, EventArgs e)
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
                FileName = $"Playlist_{mapInstance.SelectedPlaylist}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var result = mapInstanceManager.ExportPlaylistToJson(mapInstance.SelectedPlaylist, saveDialog.FileName);

                if (result.Success)
                {
                    MessageBox.Show($"Playlist {mapInstance.SelectedPlaylist} backed up successfully to:\n{saveDialog.FileName}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void OnImportPlaylist(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "JSON or MPL Files (*.json;*.mpl)|*.json;*.mpl|JSON Files (*.json)|*.json|MPL Files (*.mpl)|*.mpl|All Files (*.*)|*.*",
                DefaultExt = "json"
            };

            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            string filePath = openDialog.FileName;
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            if (extension == ".mpl")
            {
                ImportMplPlaylist(filePath);
                return;
            }

            var (success, maps, importedCount, skippedCount, errorMessage) =
                mapInstanceManager.ImportPlaylistFromJson(filePath);

            if (!success)
            {
                MessageBox.Show(errorMessage, "Import Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridView_currentMaps.Rows.Clear();

            foreach (var map in maps)
            {
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    dataGridView_currentMaps.Rows.Count + 1,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    GameTypeObject.GetShortName(map.MapType)
                );

                DataGridViewRow newRow = dataGridView_currentMaps.Rows[rowIndex];
                newRow.Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }

            string message = $"Import complete.\n\nImported: {importedCount} maps";
            if (skippedCount > 0)
            {
                message += $"\nSkipped: {skippedCount} unavailable maps";
            }
            message += "\n\nRemember to save the playlist to apply changes.";

            MessageBox.Show(message, "Import Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportMplPlaylist(string filePath)
        {
            var lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));
            var DefaultMaps = mapInstance.DefaultMaps;
            var CustomMaps = mapInstance.CustomMaps;
            int importedCount = 0, skippedCount = 0;

            dataGridView_currentMaps.Rows.Clear();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(new[] { "[-]" }, StringSplitOptions.None);
                if (parts.Length != 3)
                {
                    skippedCount++;
                    continue;
                }

                if (!int.TryParse(parts[0], out int mapType)) { skippedCount++; continue; }
                if (!int.TryParse(parts[1], out int modType)) { skippedCount++; continue; }
                string fileName = parts[2].Trim();

                var map1 = DefaultMaps.FirstOrDefault(m =>
                    m.MapType == mapType &&
                    string.Equals(m.MapFile, fileName, StringComparison.OrdinalIgnoreCase));

                var map2 = CustomMaps.FirstOrDefault(m =>
                    m.MapType == mapType &&
                    string.Equals(m.MapFile, fileName, StringComparison.OrdinalIgnoreCase));

                if (map1 == null && map2 == null)
                {
                    skippedCount++;
                    continue;
                }

                var map = map1 ?? map2!;

                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    dataGridView_currentMaps.Rows.Count + 1,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    GameTypeObject.GetShortName(map.MapType)
                );

                dataGridView_currentMaps.Rows[rowIndex].Cells[1].ToolTipText = $"Map File: {map.MapFile}";
                importedCount++;
            }

            string message = $"MPL Import complete.\n\nImported: {importedCount} maps";
            if (skippedCount > 0)
                message += $"\nSkipped: {skippedCount} unavailable or invalid maps";
            message += "\n\nRemember to save the playlist to apply changes.";

            MessageBox.Show(message, "MPL Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void OnRandomizePlaylist(object sender, EventArgs e)
        {
            var playlist = BuildPlaylistFromGrid();

            if (playlist.Count == 0)
            {
                MessageBox.Show("Cannot randomize an empty playlist.",
                    "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = mapInstanceManager.RandomizePlaylist(playlist);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridView_currentMaps.Rows.Clear();

            foreach (var map in playlist)
            {
                int rowIndex = dataGridView_currentMaps.Rows.Add(
                    map.MapID,
                    map.MapName,
                    map.MapFile,
                    map.ModType,
                    map.MapType,
                    GameTypeObject.GetShortName(map.MapType)
                );

                dataGridView_currentMaps.Rows[rowIndex].Cells[1].ToolTipText = $"Map File: {map.MapFile}";
            }

            MessageBox.Show(
                $"Playlist {mapInstance.SelectedPlaylist} has been randomized.\n\nRemember to save the playlist to apply changes.",
                "Playlist Randomized",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void UpdateCurrentMapHighlighting()
        {
            foreach (DataGridViewRow row in dataGridView_currentMaps.Rows)
            {
                if (row.IsNewRow) continue;
                row.DefaultCellStyle.BackColor = Color.White;
            }

            if (mapInstance.ActivePlaylist != mapInstance.SelectedPlaylist)
                return;

            if (theInstance?.instanceStatus == InstanceStatus.OFFLINE)
                return;

            int actualCurrentMapIndex = mapInstance.ActualPlayingMapIndex;
            int currentMapIndex = mapInstance.CurrentMapIndex;
            int nextMapIndex = mapInstance.CurrentMapIndex + 1;

            if (nextMapIndex >= dataGridView_currentMaps.Rows.Count)
            {
                nextMapIndex = 0;
            }

            AppDebug.Log("UpdateCurrentMapHighlighting", $"Map Index: Actual: {actualCurrentMapIndex} Current: {currentMapIndex} Next: {nextMapIndex} Row Count: {dataGridView_currentMaps.Rows.Count}");

            if (actualCurrentMapIndex == nextMapIndex)
            {
                dataGridView_currentMaps.Rows[actualCurrentMapIndex].DefaultCellStyle.BackColor = Color.Yellow;
            }
            else
            {
                if (theInstance?.instanceStatus != InstanceStatus.LOADINGMAP && theInstance?.instanceStatus != InstanceStatus.SCORING)
                {
                    dataGridView_currentMaps.Rows[actualCurrentMapIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                dataGridView_currentMaps.Rows[nextMapIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
        }
    }
}