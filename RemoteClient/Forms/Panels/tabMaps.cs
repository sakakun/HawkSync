using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels
{
    public partial class tabMaps : UserControl
    {

        private static theInstance? theInstance => CommonCore.theInstance!;
        private static mapInstance mapInstance => CommonCore.instanceMaps!;

        // Local state
        private List<MapDTO> _availableMaps = new();
        private List<MapDTO> _currentPlaylist = new();
        private int _selectedPlaylist = 1;

        public tabMaps()
        {
            InitializeComponent();
            LoadInitialData();
            WireUpEvents();

                    
            // OPTIONALLY: Subscribe to snapshots for server info panel
            ApiCore.OnSnapshotReceived += OnSnapshotReceived;

        }

        // EVENT HANDLER - Snapshot received
        private void OnSnapshotReceived(ServerSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnSnapshotReceived(snapshot));
                return;
            }

            // Update UI
            methodFunction_UpdateMapControls();
            btn_activePlaylist.Text = $"P{mapInstance.ActivePlaylist}";
        
            // Update the Core Instances
            UpdateCurrentMapHighlighting();
        }

        public void methodFunction_UpdateMapControls()
        {
            bool isActivePlaylist = mapInstance.ActivePlaylist == mapInstance!.SelectedPlaylist;
            bool isServerOnline = theInstance!.instanceStatus != InstanceStatus.OFFLINE;

            btn_mapControl1.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl2.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl3.Enabled = isActivePlaylist && isServerOnline;
        }

        private async void LoadInitialData()
        {
            await RefreshAvailableMaps();
            await LoadPlaylist(_selectedPlaylist);
        }

        private void WireUpEvents()
        {
            // Map type filter buttons
            btn_mapTypeALL.Click += (s, e) => FilterAvailableMaps(null);
            btn_mapTypeDM.Click += (s, e) => FilterAvailableMaps(0);
            btn_mapTypeTDM.Click += (s, e) => FilterAvailableMaps(1);
            btn_mapTypeCOOP.Click += (s, e) => FilterAvailableMaps(2);
            btn_mapTypeTKOTH.Click += (s, e) => FilterAvailableMaps(3);
            btn_mapTypeKOTH.Click += (s, e) => FilterAvailableMaps(4);
            btn_mapTypeSD.Click += (s, e) => FilterAvailableMaps(5);
            btn_mapTypeAD.Click += (s, e) => FilterAvailableMaps(6);
            btn_mapTypeCTF.Click += (s, e) => FilterAvailableMaps(7);
            btn_mapTypeFB.Click += (s, e) => FilterAvailableMaps(8);

            // Playlist selection
            btn_loadPlaylist1.Click += (s, e) => LoadPlaylist(1);
            btn_loadPlaylist2.Click += (s, e) => LoadPlaylist(2);
            btn_loadPlaylist3.Click += (s, e) => LoadPlaylist(3);
            btn_loadPlaylist4.Click += (s, e) => LoadPlaylist(4);
            btn_loadPlaylist5.Click += (s, e) => LoadPlaylist(5);

            // Playlist editing
            dataGridView_availableMaps.CellDoubleClick += (s, e) => AddMapToPlaylist(e.RowIndex);
            dataGridView_currentMaps.CellDoubleClick += (s, e) => RemoveMapFromPlaylist(e.RowIndex);
            btn_playListControlB4.Click += (s, e) => MoveMapUp();
            btn_playListControlB3.Click += (s, e) => MoveMapDown();
            btn_playListControlB1.Click += (s, e) => ClearPlaylist();
            btn_playListControlB2.Click += async (s, e) => await SavePlaylist();
            btn_playListControlB5.Click += async (s, e) => await SetActivePlaylist();

            // Import/Export
            btn_mapControl7.Click += async (s, e) => await ExportPlaylist();
            btn_mapControl6.Click += async (s, e) => await ImportPlaylist();

            // Randomize
            btn_mapControl5.Click += (s, e) => RandomizePlaylist();

            // Server actions
            btn_mapControl4.Click += async (s, e) => await RefreshAvailableMaps();
            btn_mapControl3.Click += async (s, e) => await SkipMap();
            btn_mapControl2.Click += async (s, e) => await ScoreMap();
            btn_mapControl1.Click += async (s, e) => await PlayNextMap();
        }

        // --- UI/Local Playlist Logic ---

        private void FilterAvailableMaps(int? mapType)
        {
            dataGridView_availableMaps.Rows.Clear();
            var filtered = mapType == null
                ? _availableMaps
                : _availableMaps.Where(m => m.MapType == mapType.Value).ToList();

            foreach (var map in filtered)
            {
                dataGridView_availableMaps.Rows.Add(
                    map.MapID, map.MapName, map.MapFile, map.ModType, map.MapType, GetShortType(map.MapType));
            }
        }

        private void AddMapToPlaylist(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView_availableMaps.Rows.Count) return;
            var row = dataGridView_availableMaps.Rows[rowIndex];
            var map = new MapDTO
            {
                MapID = (int)row.Cells[0].Value!,
                MapName = row.Cells[1].Value!.ToString()!,
                MapFile = row.Cells[2].Value!.ToString()!,
                ModType = (int)row.Cells[3].Value!,
                MapType = (int)row.Cells[4].Value!
            };
            _currentPlaylist.Add(map);
            RefreshCurrentPlaylistGrid();
        }

        private void RemoveMapFromPlaylist(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _currentPlaylist.Count) return;
            _currentPlaylist.RemoveAt(rowIndex);
            RefreshCurrentPlaylistGrid();
        }

        private void MoveMapUp()
        {
            var grid = dataGridView_currentMaps;
            if (grid.SelectedRows.Count == 0) return;
            int idx = grid.SelectedRows[0].Index;
            if (idx <= 0) return;
            var item = _currentPlaylist[idx];
            _currentPlaylist.RemoveAt(idx);
            _currentPlaylist.Insert(idx - 1, item);
            RefreshCurrentPlaylistGrid();
            grid.Rows[idx - 1].Selected = true;
        }

        private void MoveMapDown()
        {
            var grid = dataGridView_currentMaps;
            if (grid.SelectedRows.Count == 0) return;
            int idx = grid.SelectedRows[0].Index;
            if (idx >= _currentPlaylist.Count - 1) return;
            var item = _currentPlaylist[idx];
            _currentPlaylist.RemoveAt(idx);
            _currentPlaylist.Insert(idx + 1, item);
            RefreshCurrentPlaylistGrid();
            grid.Rows[idx + 1].Selected = true;
        }

        private void ClearPlaylist()
        {
            _currentPlaylist.Clear();
            RefreshCurrentPlaylistGrid();
        }

        private void RandomizePlaylist()
        {
            if (_currentPlaylist.Count == 0)
            {
                MessageBox.Show("Cannot randomize an empty playlist.", "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rnd = new Random();
            _currentPlaylist = _currentPlaylist.OrderBy(x => rnd.Next()).ToList();
            RefreshCurrentPlaylistGrid();
            MessageBox.Show("Playlist randomized. Remember to save to apply changes.", "Randomized", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshCurrentPlaylistGrid()
        {
            dataGridView_currentMaps.Rows.Clear();
            int id = 1;
            foreach (var map in _currentPlaylist)
            {
                dataGridView_currentMaps.Rows.Add(
                    id++, map.MapName, map.MapFile, map.ModType, map.MapType, GetShortType(map.MapType));
            }
        }

        private string GetShortType(int mapType)
        {
            // Implement as needed, e.g. switch/case for short names
            return mapType switch
            {
                0 => "DM",
                1 => "TDM",
                2 => "COOP",
                3 => "TKOTH",
                4 => "KOTH",
                5 => "SD",
                6 => "AD",
                7 => "CTF",
                8 => "FB",
                _ => "?"
            };
        }

        // --- API/Server Actions ---

        private async Task RefreshAvailableMaps()
        {
            var maps = await ApiCore.ApiClient!.GetAvailableMapsAsync();
            _availableMaps = maps ?? new List<MapDTO>();
            FilterAvailableMaps(null);
        }

        private async Task LoadPlaylist(int playlistId)
        {
            _selectedPlaylist = playlistId;
            var playlist = await ApiCore.ApiClient!.GetPlaylistAsync(playlistId);
            _currentPlaylist = playlist?.Maps ?? new List<MapDTO>();
            RefreshCurrentPlaylistGrid();
        }

        private async Task SavePlaylist()
        {
            if (_currentPlaylist.Count == 0)
            {
                MessageBox.Show("Cannot save an empty playlist.", "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dto = new PlaylistDTO { PlaylistID = _selectedPlaylist, Maps = _currentPlaylist };
            var result = await ApiCore.ApiClient!.SavePlaylistAsync(dto);
            MessageBox.Show(result.Message, result.Success ? "Success" : "Error",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async Task SetActivePlaylist()
        {
            if (_currentPlaylist.Count == 0)
            {
                MessageBox.Show("Cannot set an empty playlist as active.", "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dto = new PlaylistDTO { PlaylistID = _selectedPlaylist, Maps = _currentPlaylist };
            var result = await ApiCore.ApiClient!.SetActivePlaylistAsync(dto);
            MessageBox.Show(result.Message, result.Success ? "Success" : "Error",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async Task ExportPlaylist()
        {
            var playlist = await ApiCore.ApiClient!.ExportPlaylistAsync(_selectedPlaylist);
            if (playlist == null)
            {
                MessageBox.Show("Failed to export playlist.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using var saveDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                FileName = $"Playlist_{_selectedPlaylist}_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(playlist, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(saveDialog.FileName, json);
                MessageBox.Show($"Playlist exported to:\n{saveDialog.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ImportPlaylist()
        {
            using var openDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            };
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var json = await File.ReadAllTextAsync(openDialog.FileName);
                var playlist = System.Text.Json.JsonSerializer.Deserialize<PlaylistDTO>(json);
                if (playlist == null)
                {
                    MessageBox.Show("Invalid playlist file.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                playlist.PlaylistID = _selectedPlaylist;
                var result = await ApiCore.ApiClient!.ImportPlaylistAsync(playlist);
                if (result.Success)
                {
                    await LoadPlaylist(_selectedPlaylist);
                    MessageBox.Show("Playlist imported and loaded. Remember to save to apply changes.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task SkipMap()
        {
            var result = await ApiCore.ApiClient!.SkipMapAsync();
            MessageBox.Show(result.Message, result.Success ? "Success" : "Error",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async Task ScoreMap()
        {
            var result = await ApiCore.ApiClient!.ScoreMapAsync();
            MessageBox.Show(result.Message, result.Success ? "Success" : "Error",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async Task PlayNextMap()
        {
            var grid = dataGridView_currentMaps;
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a map in the playlist.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idx = grid.SelectedRows[0].Index;
            var result = await ApiCore.ApiClient!.PlayNextMapAsync(idx);
            MessageBox.Show(result.Message, result.Success ? "Success" : "Error",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
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
            if (mapInstance.ActivePlaylist != _selectedPlaylist)
            {
                AppDebug.Log("UpdateCurrentMapHighlighting", $"ActivePlaylist {mapInstance.ActivePlaylist} SelectedPlaylist {_selectedPlaylist}");
                return;
            }                

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