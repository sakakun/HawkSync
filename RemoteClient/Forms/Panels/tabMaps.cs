using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using RemoteClient.Core;
using System.Text;

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
        private bool IsEditMode = false;

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

            _ = EnsurePlaylistSynced();

            // Update the Core Instances
            UpdateCurrentMapHighlighting();
        }

        public void methodFunction_UpdateMapControls()
        {
            bool isActivePlaylist = (mapInstance.ActivePlaylist == _selectedPlaylist);
            bool isServerOnline = (theInstance!.instanceStatus != InstanceStatus.OFFLINE);

            btn_mapControl1.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl2.Enabled = isActivePlaylist && isServerOnline;
            btn_mapControl3.Enabled = isActivePlaylist && isServerOnline;
        }

        private async void LoadInitialData()
        {
            await RefreshAvailableMaps();
            await LoadPlaylist(_selectedPlaylist);
        }

        private async Task EnsurePlaylistSynced()
        {
            if (IsEditMode)
                return;

            if (_selectedPlaylist == mapInstance.ActivePlaylist)
            {
                var activePlaylist = mapInstance.Playlists[mapInstance.ActivePlaylist];

                bool playlistsMatch = _currentPlaylist.Count == activePlaylist.Count &&
                    !_currentPlaylist.Where((map, idx) =>
                        map.MapName != activePlaylist[idx].MapName ||
                        map.MapType != activePlaylist[idx].MapType
                    ).Any();

                if (!playlistsMatch)
                {
                    await LoadPlaylist(_selectedPlaylist);
                }
            }
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
            btn_loadPlaylist1.Click += (s, e) => actionClick_loadMapPlaylist1();
            btn_loadPlaylist2.Click += (s, e) => actionClick_loadMapPlaylist2();
            btn_loadPlaylist3.Click += (s, e) => actionClick_loadMapPlaylist3();
            btn_loadPlaylist4.Click += (s, e) => actionClick_loadMapPlaylist4();
            btn_loadPlaylist5.Click += (s, e) => actionClick_loadMapPlaylist5();

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
            IsEditMode = true;
            RefreshCurrentPlaylistGrid();
        }

        private void RemoveMapFromPlaylist(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _currentPlaylist.Count) return;
            _currentPlaylist.RemoveAt(rowIndex);
            RefreshCurrentPlaylistGrid();
            IsEditMode = true;
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
            IsEditMode = true;
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
            IsEditMode = true;
        }

        private void ClearPlaylist()
        {
            _currentPlaylist.Clear();
            IsEditMode = true;
            RefreshCurrentPlaylistGrid();
        }

        private void RandomizePlaylist()
        {
            if (_currentPlaylist.Count == 0)
            {
                MessageBox.Show("Cannot randomize an empty playlist.", "Empty Playlist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IsEditMode = true;
            // Fisher-Yates shuffle
            var random = new Random();
            for (int i = _currentPlaylist.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = _currentPlaylist[i];
                _currentPlaylist[i] = _currentPlaylist[j];
                _currentPlaylist[j] = temp;
            }

            // Renumber MapIDs sequentially (1-based)
            for (int i = 0; i < _currentPlaylist.Count; i++)
            {
                _currentPlaylist[i].MapID = i + 1;
            }

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
        
        /// <summary>
        /// Playlist selection button handlers
        /// </summary>
        private void actionClick_loadMapPlaylist1() => methodFunction_selectPlaylist(1);
        private void actionClick_loadMapPlaylist2() => methodFunction_selectPlaylist(2);
        private void actionClick_loadMapPlaylist3() => methodFunction_selectPlaylist(3);
        private void actionClick_loadMapPlaylist4() => methodFunction_selectPlaylist(4);
        private void actionClick_loadMapPlaylist5() => methodFunction_selectPlaylist(5);

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
            _ = LoadPlaylist(playlistID);
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
            IsEditMode = false;
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
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "JSON or MPL Files (*.json;*.mpl)|*.json;*.mpl|JSON Files (*.json)|*.json|MPL Files (*.mpl)|*.mpl|All Files (*.*)|*.*",
                DefaultExt = "json"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openDialog.FileName;
                string extension = Path.GetExtension(filePath).ToLowerInvariant();

                if (extension == ".mpl")
                {
                    // --- MPL Import Logic ---
                    var lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));
                    var DefaultMaps = mapInstance.DefaultMaps;
                    var CustomMaps = mapInstance.CustomMaps;
                    var mapList = new List<MapDTO>();
                    int importedCount = 0, skippedCount = 0;

                    int mapId = 1;
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

                        // Try to find in default or custom maps
                        var map1 = DefaultMaps.FirstOrDefault(m =>
                            m.MapType == mapType &&
                            string.Equals(m.MapFile, fileName, StringComparison.OrdinalIgnoreCase));

                        var map2 = CustomMaps.FirstOrDefault(m =>
                            m.MapType == mapType &&
                            string.Equals(m.MapFile, fileName, StringComparison.OrdinalIgnoreCase));

                        var map = map1 ?? map2;
                        if (map == null)
                        {
                            skippedCount++;
                            continue;
                        }

                        mapList.Add(new MapDTO
                        {
                            MapID = mapId++,
                            MapName = map.MapName,
                            MapFile = map.MapFile,
                            ModType = map.ModType,
                            MapType = map.MapType
                        });
                        importedCount++;
                    }

                    if (mapList.Count == 0)
                    {
                        MessageBox.Show("No valid maps found in MPL file.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var playlist1 = new PlaylistDTO
                    {
                        PlaylistID = _selectedPlaylist,
                        Maps = mapList
                    };

                    var result = await ApiCore.ApiClient!.ImportPlaylistAsync(playlist1);
                    if (result.Success)
                    {
                        await LoadPlaylist(_selectedPlaylist);
                        string msg = $"MPL Import complete.\n\nImported: {importedCount} maps";
                        if (skippedCount > 0)
                            msg += $"\nSkipped: {skippedCount} unavailable or invalid maps";
                        msg += "\n\nRemember to save to apply changes.";
                        MessageBox.Show(msg, "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                // --- JSON Import Logic ---
                var json = await File.ReadAllTextAsync(openDialog.FileName);
                var playlist2 = System.Text.Json.JsonSerializer.Deserialize<PlaylistDTO>(json);
                if (playlist2 == null)
                {
                    MessageBox.Show("Invalid playlist file.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                playlist2.PlaylistID = _selectedPlaylist;
                var jsonResult = await ApiCore.ApiClient!.ImportPlaylistAsync(playlist2);
                if (jsonResult.Success)
                {
                    await LoadPlaylist(_selectedPlaylist);
                    MessageBox.Show("Playlist imported and loaded. Remember to save to apply changes.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(jsonResult.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // No Maps In Current Playlist (Race Condition Issues)
            if (dataGridView_currentMaps.Rows.Count == 0)
                return;

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