using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System.Data;
using System.Net;
using System.Text;

namespace BHD_RemoteClient.Forms
{
    public partial class ServerManager : Form
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;

        // ServerManager Local Variables
        private bool _updatingWeaponCheckboxes = false; // Prevent recursion
        List<CheckBox> weaponCheckboxes = new List<CheckBox>(); // List to hold all weapon checkboxes

        // Admin Selections
        private int adminSelectedId = -1; // Selected Admin ID for actions

        // Main constructor for the ServerManager
        public ServerManager()
        {
            InitializeComponent();

            // Instance Initialization
            theInstanceManager.CheckSettings();
            banInstanceManager.LoadSettings();
            chatInstanceManagers.LoadSettings();
            adminInstanceManager.LoadSettings();

            this.Load += PostServerManagerInitalization;
        }

        // Init. Form of the ServerManager
        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            // Start All Tickers
            theInstanceManager.InitializeTickers();
            // Forms and Field Initialization
            InitializeWeaponCheckboxes();
            theInstanceManager.GetServerVariables();
            // Maps Tab
            functionEvent_UpdateMapGameTypes();                         // Populate Game Types
            functionEvent_InitMapDataGrids();                           // Setup Current Maps DataGridView
            mapInstanceManager.ResetAvailableMaps();
            // Ban Tab
            cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1;
            // Chat Messages
            functionEvent_UpdateAutoMessages();
            functionEvent_UpdateSlapMessages();
            // Admins Tab
            adminInstanceManager.UpdateAdminLogDialog();
            ActionClick_AdminNewUser(null!, null!);                     // Initialize the Admins Tab with a new user

        }
        // Initializes the weapon checkboxes list
        private void InitializeWeaponCheckboxes()
        {
            weaponCheckboxes = new List<CheckBox>
            {
                cb_weapColt45, cb_weapM9Bereatta, cb_weapCAR15, cb_weapCAR15203, cb_weapM16, cb_weapM16203,
                cb_weapG3, cb_weapG36, cb_weapM60, cb_weapM240, cb_weapMP5, cb_weapSaw, cb_weap300Tact,
                cb_weapM21, cb_weapM24, cb_weapBarret, cb_weapPSG1, cb_weapShotgun, cb_weapFragGrenade,
                cb_weapSmokeGrenade, cb_weapSatchel, cb_weapAT4, cb_weapFlashBang, cb_weapClay
            };
        }

        //
        // Event Handlers for ServerManager
        // Description: Function calls not made by a Click or KeyPress event.
        //
        // Function: functionEvent_serverStatus, Updates the status label based on the server's current status.
        internal void functionEvent_serverStatus()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                toolStripStatus.Text = "Server is not running.";
            }
            if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                toolStripStatus.Text = "Server is running. Game in progress.";
            }
            if (theInstance.instanceStatus == InstanceStatus.SCORING)
            {
                toolStripStatus.Text = "Server is running. Game has ended, currently scoring.";
            }
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                toolStripStatus.Text = "Server is running. Game reset in progress.";
            }
            if (theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                toolStripStatus.Text = "Server is running. Game ready, waiting for start.";
            }
        }
        // Scope: ServerTab, Function: functionEvent_swapFieldsStartStop, Updates the Start/Stop button and enables/disables controls based on server status use actions.
        internal void functionEvent_swapFieldsStartStop()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(functionEvent_swapFieldsStartStop));
                return;
            }

            bool isOffline = theInstance.instanceStatus == InstanceStatus.OFFLINE;

            btn_startStop.Text = isOffline ? "Start Server" : "Stop Server";

            // Controls enabled only when server is offline
            var offlineControls = new Control[]
            {
                num_serverPort,
                cb_serverDedicated,
                tb_serverPassword,
                cb_serverIP
            };

            foreach (var control in offlineControls)
                control.Enabled = isOffline;

            // Controls enabled only when server is NOT offline
            var onlineOnlyControls = new Control[]
            {
                btn_mapsPlayNext,
                btn_mapsScore,
                btn_mapsSkip
            };

            foreach (var control in onlineOnlyControls)
                control.Enabled = !isOffline;
        }

        // Scope: BanTab, Function: functionEvent_RemoveBannedPlayer, Handles the removal of a banned player based on their record ID.
        private void functionEvent_RemoveBannedPlayer(int recordId)
        {
            bool foundInNames = instanceBans.BannedPlayerNames.Any(b => b.recordId == recordId);
            bool foundInAddresses = instanceBans.BannedPlayerAddresses.Any(b => b.recordId == recordId);

            if (!foundInNames && !foundInAddresses)
            {
                MessageBox.Show($"No ban record found with ID {recordId}.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (foundInNames && foundInAddresses)
            {
                var result = MessageBox.Show(
                    "This record ID exists in both player name and IP bans.\n\n" +
                    "Click Yes to remove both.\n" +
                    "Click No to remove only the player name ban.\n" +
                    "Click Cancel to remove only the IP ban.",
                    "Remove Ban Record",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    banInstanceManager.RemoveBannedPlayerBoth(recordId);
                }
                else if (result == DialogResult.No)
                {
                    banInstanceManager.RemoveBannedPlayerName(recordId);
                }
                else if (result == DialogResult.Cancel)
                {
                    banInstanceManager.RemoveBannedPlayerAddress(recordId);
                }
            }
            else if (foundInNames)
            {
                banInstanceManager.RemoveBannedPlayerName(recordId);
            }
            else if (foundInAddresses)
            {
                banInstanceManager.RemoveBannedPlayerAddress(recordId);
            }
        }
        // Scope: ChatTab, Function: functionEvent_UpdateSlapMessages, Updates the slap messages grid with the current slap messages.
        public void functionEvent_UpdateSlapMessages()
        {
            dg_slapMessages.Rows.Clear();
            foreach (var slapMsg in chatInstanceManagers.GetSlapMessages())
            {
                dg_slapMessages.Rows.Add(slapMsg.SlapMessageId, slapMsg.SlapMessageText);
            }
        }
        // Scope: ChatTab, Function: functionEvent_UpdateAutoMessages, Updates the auto messages grid with the current auto messages.
        public void functionEvent_UpdateAutoMessages()
        {
            dg_autoMessages.Rows.Clear();
            foreach (var autoMsg in chatInstanceManagers.GetAutoMessages())
            {
                dg_autoMessages.Rows.Add(autoMsg.AutoMessageId, autoMsg.AutoMessageTigger, autoMsg.AutoMessageText);
            }
        }
        // Scope: MapTab, Function: functionEvent_UpdateMapGameTypes, Updates the game types combo box with the current game types.
        private void functionEvent_UpdateMapGameTypes()
        {
            foreach (var gameType in objectGameTypes.All)
            {
                combo_gameTypes.Items.Add(gameType.Name!);
            }
            combo_gameTypes.Items.RemoveAt(0);
            combo_gameTypes.Items.Add("All Game Types");
            combo_gameTypes.SelectedIndex = 9;
        }
        // Scope: MapTab, Function: functionEvent_InitMapDataGrids, Initializes the available and current maps DataGridViews.
        private void functionEvent_InitMapDataGrids()
        {
            // Setup available maps DataGridView
            functionEvent_SetupAvailableMapsGrid();
            functionEvent_PopulateMapDataGrid(dataGridView_availableMaps, CommonCore.instanceMaps.availableMaps, false);

            // Setup current maps DataGridView
            functionEvent_SetupCurrentMapsGrid();
            functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, CommonCore.instanceMaps.currentMapPlaylist, true);
        }
        // Scope: MapTab, Function: functionEvent_SetupAvailableMapsGrid, Sets up the available maps DataGridView with columns and styles.
        private void functionEvent_SetupAvailableMapsGrid()
        {
            DataGridView dgv = dataGridView_availableMaps;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.Columns.Add("MapName", "Name");
            dgv.Columns.Add("MapType", "Type");
            dgv.Columns["MapName"].Width = 120;
            dgv.Columns["MapType"].Width = 50;
            dgv.Columns["MapName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["MapType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["MapType"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        // Scope: MapTab, Function: functionEvent_SetupCurrentMapsGrid, Sets up the current maps DataGridView with columns and styles.
        private void functionEvent_SetupCurrentMapsGrid()
        {
            DataGridView dgv = dataGridView_currentMaps;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.Columns.Add("MapNum", "#");
            dgv.Columns.Add("MapName", "Name");
            dgv.Columns.Add("MapType", "Type");
            dgv.Columns["MapNum"].Width = 30;
            dgv.Columns["MapName"].Width = 120;
            dgv.Columns["MapType"].Width = 50;
            dgv.Columns["MapType"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["MapNum"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns["MapNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["MapType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["MapName"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns["MapType"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns["MapName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        // Scope: MapTab, Function: functionEvent_PopulateMapDataGrid, Populates the given DataGridView with map data.
        public void functionEvent_PopulateMapDataGrid(DataGridView dgv, IEnumerable<mapFileInfo> maps, bool includeRowNum)
        {
            dgv.Rows.Clear();
            int rowNum = 1;
            foreach (var map in maps)
            {
                if (includeRowNum)
                    dgv.Rows.Add(rowNum++, map.MapName, map.MapType);
                else
                    dgv.Rows.Add(map.MapName, map.MapType);
            }
        }
        // Scope: MapTab, Function: functionEvent_UpdateMapRowNumbers, Updates the row numbers in the current maps DataGridView.
        public void functionEvent_UpdateMapRowNumbers(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (!dgv.Rows[i].IsNewRow)
                    dgv.Rows[i].Cells["MapNum"].Value = i + 1;
            }
        }
        // Scope: MapTab, Function: functionEvent_RefreshAvailableMaps, Refreshes the available maps in the map manager.
        public void functionEvent_RefreshAvailableMaps()
        {
            mapInstanceManager.ResetAvailableMaps();
            functionEvent_PopulateMapDataGrid(dataGridView_availableMaps, instanceMaps.availableMaps, false);
        }
        // Scope: MapTab, Function: GetFileLinesFromDialog
        private string[]? functionEvent_GetFileLinesFromDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName)
        {
            string? filePath = Functions.ShowFileDialog(saveDialog, filter, title, initialDirectory, defaultFileName);

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;

            return File.ReadAllLines(filePath, Encoding.UTF8);
        }
        // Scope: MapTab, Function: ParseMapFileInfos, Parses map file information from a string array.
        private IEnumerable<mapFileInfo> functionEvent_ParseMapFileInfos(string[] lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                yield return new mapFileInfo { MapFile = parts[0], MapName = parts[1], MapType = parts[2] };
            }
        }
        // Scope: MapTab, Function: functionEvent_ImportMapPlaylistToCurrentDataGrid, Imports a map playlist from a file to the current maps DataGridView.
        public void functionEvent_ImportMapPlaylistToCurrentDataGrid()
        {
            string? startPath = CommonCore.AppDataPath;
            var lines = functionEvent_GetFileLinesFromDialog(false, "Map Playlist (*.mpl)|*.mpl|All files (*.*)|*.*", "Open Map Playlist File", startPath, "currentMapPlaylist.mpl");
            if (lines == null)
            {
                MessageBox.Show("Map playlist file not found or could not be opened.", "Import Error");
                return;
            }
            functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, functionEvent_ParseMapFileInfos(lines), true);
        }
        // Scope: MapTab, Function: functionEvent_LoadCurrentPlaylistToDataGrid, Loads the current map playlist into the DataGridView.
        public void functionEvent_LoadCurrentPlaylistToDataGrid()
        {
            functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, instanceMaps.currentMapPlaylist, true);
        }
        // Action Click Handlers
        //
        // Scope: Program, Function: OnFormClosing override, Handles the form closing event to prompt the user and perform cleanup.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Example: Prompt user before closing
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Prevent the form from closing
                return;
            }

            // Place any cleanup logic here (e.g., save settings, stop tickers, etc.)
            CommonCore.Ticker?.Stop("ServerManager");
            CommonCore.Ticker?.Stop("ChatManager");
            CommonCore.Ticker?.Stop("PlayerManager");
            CommonCore.Ticker?.Stop("BanManager");

            // Save settings before closing
            theInstanceManager.SaveSettings();
            base.OnFormClosing(e);

        }

        // Scope: ServerTab, Function: actionClick_WeaponCheckedChanged, Handles the weapon checkboxes to select/deselect all weapons.
        private void actionClick_WeaponCheckedChanged(object sender, EventArgs e)
        {
            if (_updatingWeaponCheckboxes) return;
            _updatingWeaponCheckboxes = true;

            if (sender == checkBox_selectAll && checkBox_selectAll.Checked)
            {
                weaponCheckboxes.ForEach(cb => cb.Checked = true);
                checkBox_selectNone.Checked = false;
            }
            else if (sender == checkBox_selectNone && checkBox_selectNone.Checked)
            {
                weaponCheckboxes.ForEach(cb => cb.Checked = false);
                checkBox_selectAll.Checked = false;
            }
            else if (weaponCheckboxes.Contains(sender))
            {
                checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
                checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);
            }

            _updatingWeaponCheckboxes = false;
        }
        // Scope: ServerTab, Function: actionClick_resetServerChanges, Resets server settings to the last saved state.
        private void actionClick_resetServerChanges(object sender, EventArgs e)
        {
            theInstanceManager.GetServerVariables();
        }
        // Scope: ServerTab, Function: actionClick_SetServerPath, Opens a file dialog to set the server path.
        private void actionClick_SetServerPath(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "dfbhd.exe|dfbhd.exe";
                openFileDialog.Title = "Select dfbhd.exe";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Save the selected path to the instance
                    theInstance.profileServerPath = Path.GetDirectoryName(openFileDialog.FileName) ?? string.Empty;
                    if (theInstance.profileServerPath != string.Empty)
                    {
                        MessageBox.Show($"Server Path set to: {theInstance.profileServerPath}", "Server Path Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        theInstanceManager.ValidateGameServerType(theInstance.profileServerPath);
                        theInstanceManager.SetServerVariables();
                        theInstanceManager.SaveSettings();
                        MessageBox.Show("Server path has been set successfully. Settings updated and saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to set server path. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
        // Scope: ServerTab, Function: actionClick_saveUpdateSettings, Saves the server settings and updates the game server.
        private void actionClick_saveUpdateSettings(object sender, EventArgs e)
        {
            // Save the settings from the form to the instance & update last know settings file.
            theInstanceManager.SetServerVariables();
            // Saving is done by the server, you must export the settings to save them locally.
        }
        // Scope: MapsTab, Function: actionClick_refreshMaps, Refreshes the available maps in the map manager.
        private void actionClick_refreshMaps(object sender, EventArgs e)
        {
            functionEvent_RefreshAvailableMaps();
            combo_gameTypes.SelectedIndex = 9; // Reset the MapType filter to "All"
        }
        // Scope: MapsTab, Function: actionClick_clearCurrentMapPlaylist, Clears the current map playlist.
        private void actionClick_clearCurrentMapPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }
        // Scope: MapsTab, Function: actionClick_addMap2Current, Adds a selected map from the available maps to the current map playlist.
        private void actionClick_addMap2Current(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView_currentMaps.Rows.Count < 128)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView_availableMaps.Rows[e.RowIndex];

                // Clone the row and add it to currentMaps
                int rowNum = dataGridView_currentMaps.Rows.Count + 1;
                int newRowIdx = dataGridView_currentMaps.Rows.Add();
                dataGridView_currentMaps.Rows[newRowIdx].Cells[0].Value = rowNum;
                dataGridView_currentMaps.Rows[newRowIdx].Cells[1].Value = selectedRow.Cells[0].Value;
                dataGridView_currentMaps.Rows[newRowIdx].Cells[2].Value = selectedRow.Cells[1].Value;
            }
            if (dataGridView_currentMaps.Rows.Count == 128)
            {
                MessageBox.Show("You have reached the maximum number of maps (128) in the current map playlist.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Scope: MapsTab, Function: actionClick_delRowFromCurrent, Deletes a selected row from the current map playlist.
        private void actionClick_delRowFromCurrent(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView_currentMaps.Rows.RemoveAt(e.RowIndex);
                functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
            }
        }
        // Scope: MapsTab, Function: actionClick_saveCurrentMapList, Saves the current map list to the instance and updates the map playlist file.
        private void actionClick_saveCurrentMapList(object sender, EventArgs e)
        {
            // Build new Map Playlist
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();
            // Update the currentMapPlaylist Array
            instanceMaps.currentMapPlaylist = newMapPlaylist;
            // Save to File
            mapInstanceManager.SaveCurrentMapPlaylist(newMapPlaylist, false);
        }
        // Scope: MapsTab, Function: actionClick_exportCurrentPlaylist, Exports the current map playlist to a file.
        private void actionClick_exportCurrentPlaylist(object sender, EventArgs e)
        {
            // Build newMapPlaylist
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();

            // Save to File
            mapInstanceManager.SaveCurrentMapPlaylist(newMapPlaylist, true);
        }
        // Scope: MapsTab, Function: actionClick_importMapPlaylist, Imports a map playlist from a file to the current data grid.
        private void actionClick_importMapPlaylist(object sender, EventArgs e)
        {
            functionEvent_ImportMapPlaylistToCurrentDataGrid();
        }
        // Scope: MapsTab, Function: actionClick_resetCurrentMapList, Resets the current map list to the last saved state.
        private void actionClick_resetCurrentMapList(object sender, EventArgs e)
        {
            functionEvent_LoadCurrentPlaylistToDataGrid();
            functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
        }
        // Scope: MapsTab, Function: actionChange_mapFilterGameType, Filters the available maps based on the selected game type.
        private void actionChange_mapFilterGameType(object sender, EventArgs e)
        {
            // MapTypes.All, the index of the selected item in the ComboBox is the DatabaseId of the MapType
            // Take the ShortName of the selected MapType and filter the available maps accordingly, the column is named "MapType"
            // If the selected index is 9, remove filtering and show all items.
            if (combo_gameTypes.SelectedIndex >= 0)
            {
                string selectedGameType = string.Empty;
                if (combo_gameTypes.SelectedIndex != 9)
                {
                    selectedGameType = objectGameTypes.All[combo_gameTypes.SelectedIndex].ShortName!;
                }

                if (combo_gameTypes.SelectedIndex == 9)
                {
                    // Show all maps
                    foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                    {
                        row.Visible = true; // Show all rows
                    }
                }
                else
                {
                    // Filter maps by the selected game type
                    foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                    {
                        if (row.Cells["MapType"].Value?.ToString() != selectedGameType)
                        {
                            // Hide the row if it doesn't match the selected game type
                            row.Visible = false;
                        }
                        else
                        {
                            // Show the row if it matches the selected game type
                            row.Visible = true;
                        }
                    }
                }
            }
        }
        // Scope: MapsTab, Function: actionKeyEvent_moveMap, Moves the selected map up or down in the current map playlist using W and S keys.
        private void actionKeyEvent_moveMap(object sender, KeyPressEventArgs e)
        {
            // Check if the keys pressed are the Shift key and the W or S key
            if (e.KeyChar == (char)Keys.W || e.KeyChar == (char)Keys.S)
            {
                // Get the currently selected row
                int currentRowIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
                // Determine the new row index based on the key pressed
                int newRowIndex = e.KeyChar == (char)Keys.W ? currentRowIndex - 1 : currentRowIndex + 1;
                // Ensure the new index is within bounds
                if (newRowIndex >= 0 && newRowIndex < dataGridView_currentMaps.Rows.Count)
                {
                    // Move the selected row to the new index
                    DataGridViewRow selectedRow = dataGridView_currentMaps.Rows[currentRowIndex];
                    dataGridView_currentMaps.Rows.RemoveAt(currentRowIndex);
                    dataGridView_currentMaps.Rows.Insert(newRowIndex, selectedRow);
                    dataGridView_currentMaps.CurrentCell = dataGridView_currentMaps.Rows[newRowIndex].Cells[0]; // Set focus to the moved row
                    functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
                }
            }
        }
        // Scope: ServerTab, Function: actionClick_startServer, Starts or stops the game server based on the current status.
        private void actionClick_startServer(object sender, EventArgs e)
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                theInstanceManager.SetServerVariables();

                if (GameManager.startGame())
                {
                    MessageBox.Show("Server start command sent, please wait 10 seconds for the game to start.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Poll for server status for up to 10 seconds
                bool started = false;

                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(500); // Wait for 0.5 seconds
                    if (theInstance.instanceStatus != InstanceStatus.OFFLINE)
                    {
                        started = true;
                        break;
                    }
                }

                if (started)
                {
                    MessageBox.Show("Game server started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Server start command sent, but server did not come online in time.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }
            else if (theInstance.instanceStatus != InstanceStatus.OFFLINE)
            {
                if (GameManager.stopGame())
                {
                    MessageBox.Show("Stop command has been sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to stop the game server or it was not running.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // else: do nothing, or optionally log, but don't show a message
        }
        // Scope: MapsTab, Function: actionClick_updateGameServerMaps, Updates the game server maps based on the current map list.
        private void actionClick_updateGameServerMaps(object sender, EventArgs e)
        {
            if (CmdUpdateMapCycle.ProcessCommand())
            {
                MessageBox.Show("The server map list has been updated successfully.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update the server map list. Please check the server status and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Scope: MapsTab, Function: actionClick_mapPlayNext, Updates the next map to be played based on the selected row in the current maps data grid.
        private void actionClick_mapPlayNext(object sender, EventArgs e)
        {
            int mapIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
            GameManager.UpdateNextMap(mapIndex);
            MessageBox.Show(dataGridView_currentMaps.Rows[mapIndex].Cells[1].Value + " has been updated to play next.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Scope: ServerTab, Function: actionClick_enableFFkills, Enables or disables the friendly fire kills setting based on the checkbox state.
        private void actionClick_enableFFkills(object sender, EventArgs e)
        {
            num_maxFFKills.Enabled = cb_enableFFkills.Checked;
        }
        // Scope: ServerTab, Function: actionClick_enableMinCheck, Enables or disables the minimum ping check based on the checkbox state.
        private void actionClick_enableMinCheck(object sender, EventArgs e)
        {
            num_minPing.Enabled = cb_enableMinCheck.Checked;
        }
        // Scope: ServerTab, Function: actionClick_enableMaxPing, Enables or disables the maximum ping check based on the checkbox state.
        private void actionClick_enableMaxPing(object sender, EventArgs e)
        {
            num_maxPing.Enabled = cb_enableMaxCheck.Checked;
        }
        // Scope: MapsTab, Function: actionClick_mapScore, forces the server to score the current map.
        private void actionClick_mapScore(object sender, EventArgs e)
        {
            GameManager.WriteMemoryScoreMap();
        }
        // Scope: ChatTab, Function: actionKeyEvent_submitMessage, Submits a chat message based on the input from the chat text box.
        private void actionKeyEvent_submitMessage(object sender, EventArgs e)
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE || theInstance.instanceStatus == InstanceStatus.LOADINGMAP || theInstance.instanceStatus == InstanceStatus.SCORING)
            {
                MessageBox.Show("Server is not in a state to accept messages, please start or wait for the map to be ready or playing.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string message = tb_chatMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            // Determine the channel based on the selected index
            int channel = 0; // Default to Global Chat (Blue)
            // Check to see if the first two characters are a valid channel prefix
            if (message.Length >= 2 && message[0] == '/')
            {
                switch (message.Substring(1, 1).ToLower())
                {
                    case "y":
                        channel = 1; // Yellow
                        break;
                    case "r":
                        channel = 2; // Red Team
                        break;
                    case "b":
                        channel = 3; // Blue Team
                        break;
                    default:
                        channel = 0; // Default to Global Chat (Blue)
                        break;
                }
                message = message.Substring(2); // Remove the prefix from the message
            }

            // Check to see if the message has {P:##} this is a players PlayerSlot number. Grab the player's name from the player list and replace the {P:##} with the players name.
            if (message.Contains("{P:") && message.Contains("}"))
            {
                // Extract the player PlayerSlot number
                int startIndex = message.IndexOf("{P:") + 3;
                int endIndex = message.IndexOf("}", startIndex);
                if (endIndex > startIndex)
                {
                    string playerSlot = message.Substring(startIndex, endIndex - startIndex);
                    // Find the player in the player list
                    var playerEntry = theInstance.playerList.FirstOrDefault(p => p.Value.PlayerSlot.ToString() == playerSlot);
                    if (playerEntry.Value == null)
                    {
                        MessageBox.Show($"No player found in PlayerSlot {playerSlot}. Message will not be sent.", "Player Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string playerName = playerEntry.Value.PlayerName ?? string.Empty;
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        // Sanitize the player name and replace in the message
                        playerName = Functions.SanitizePlayerName(playerName);
                        message = message.Replace($"{{P:{playerSlot}}}", playerName);

                    }
                }
            }

            // If final message is longer than 59 character break the message into multiple messages as needed.
            if (message.Length > 59)
            {
                // Split the message into chunks of 59 characters
                for (int i = 0; i < message.Length; i += 59)
                {
                    string chunk = message.Substring(i, Math.Min(59, message.Length - i));
                    GameManager.WriteMemorySendChatMessage(channel, chunk);
                }
            }
            else
            {
                // Send the message as is
                GameManager.WriteMemorySendChatMessage(channel, message);
            }

            tb_chatMessage.Clear(); // Clear the input field after sending the message

        }
        // Scope: ChatTab, Function: actionKeyEvent_submitEnterMessage, Submits the chat message when the Enter key is pressed.
        private void actionKeyEvent_submitEnterMessage(object sender, KeyPressEventArgs e)
        {
            // If key pressed is Enter, submit the message
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the beep sound on Enter key
                actionKeyEvent_submitMessage(sender, e); // Call the submit message function
            }
        }
        // Scope: BanTab, Function: actionClick_addBanInformation, Adds a new ban record based on the input from the ban information fields.
        private void actionClick_addBanInformation(object sender, EventArgs e)
        {
            // Collect
            string playerName = tb_bansPlayerName.Text.Trim();
            string playerIP = tb_bansIPAddress.Text.Trim();
            int submask = cb_banSubMask.SelectedIndex + 1;

            // Prep
            string EncodedPlayerName = string.Empty;
            IPAddress ipAddress = null!;

            if (!string.IsNullOrEmpty(playerName))
            {
                EncodedPlayerName = Convert.ToBase64String(Encoding.UTF8.GetBytes(playerName));
            }
            if (!string.IsNullOrEmpty(playerIP))
            {
                ipAddress = IPAddress.Parse(playerIP);
            }

            // Submit New Record
            banInstanceManager.AddBannedPlayer(EncodedPlayerName, ipAddress!, submask);

            banInstanceManager.UpdateBannedTables();

            MessageBox.Show("Ban information has been added successfully.", "Ban Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tb_bansPlayerName.Text = string.Empty; // Clear the player name field
            tb_bansIPAddress.Text = string.Empty; // Clear the IP PlayerIPAddress field
            cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1; // Reset the submask to default
        }
        // Scope: BanTab, Function: actionDbClick_RemoveRecord2, Removes a ban record based on a double-click event on the IP addresses data grid.
        private void actionDbClick_RemoveRecord2(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int recordId = Convert.ToInt32(dg_IPAddresses.Rows[e.RowIndex].Cells[0].Value);
            functionEvent_RemoveBannedPlayer(recordId); // Call the method to remove the ban record
            banInstanceManager.UpdateBannedTables(); // Refresh the ban lists after removing a record
        }
        // Scope: BanTab, Function: actionDbClick_RemoveRecord, Removes a ban record based on a double-click event on the player names data grid.
        private void actionDbClick_RemoveRecord(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int recordId = Convert.ToInt32(dg_playerNames.Rows[e.RowIndex].Cells[0].Value);
            functionEvent_RemoveBannedPlayer(recordId); // Call the method to remove the ban record
        }
        // Scope: ChatTab, Function: actionClick_addSlapMessage, Adds a slap message based on the input from the slap message text box.
        private void actionClick_addSlapMessage(object sender, EventArgs e)
        {
            chatInstanceManagers.AddSlapMessage(tb_slapMessage.Text.Trim());
            functionEvent_UpdateSlapMessages();
            tb_slapMessage.Text = string.Empty; // Clear the slap message text box after adding a slap message
        }
        // Scope: ChatTab, Function: actionClick_addAutoMessages, Adds an auto message based on the input from the auto message text box and trigger value.
        private void actionClick_addAutoMessages(object sender, EventArgs e)
        {
            chatInstanceManagers.AddAutoMessage(tb_autoMessage.Text.Trim(), (int)num_AutoMessageTrigger.Value);
            functionEvent_UpdateAutoMessages();
            tb_autoMessage.Text = string.Empty; // Clear the auto message text box after adding an auto message
            num_AutoMessageTrigger.Value = 0; // Reset the trigger value to default
        }
        // Scope: ChatTab, Function: actionDbClick_RemoveAutoMessage, Removes a auto message based on a double-click event on the slap messages data grid.
        private void actionDbClick_RemoveAutoMessage(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int AutoMessageId = Convert.ToInt32(dg_autoMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveAutoMessage(AutoMessageId); // Call the method to remove the auto message
            functionEvent_UpdateAutoMessages();
        }
        // Scope: ChatTab, Function: actionDbClick_RemoveSlapMessage, Removes a slap message based on a double-click event on the slap messages data grid.
        private void actionDbClick_RemoveSlapMessage(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int slapMessageId = Convert.ToInt32(dg_slapMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveSlapMessage(slapMessageId); // Call the method to remove the slap message
            functionEvent_UpdateSlapMessages();
        }
        // Scope: MapTab, Function: actionClick_mapSkip, Skips the current map if the server is online.
        private void actionClick_mapSkip(object sender, EventArgs e)
        {
            if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                theInstance.instanceMapSkipped = true;
                CmdMapSkip.ProcessCommand();
            }
            else
            {
                MessageBox.Show("Sorry you can't skip currently, must wait until the map is playing to use this.", "Cannot Skip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        // Scope: ServerTab, Function: actionClick_importServerSettings, Imports server settings from a file.
        private void actionClick_importServerSettings(object sender, EventArgs e)
        {
            theInstanceManager.ImportSettings();
        }
        // Scope: StatsTab, Function: ActionEvent_EnableAnnouncements, Enables or disables the web stats announcements.
        private void ActionEvent_EnableAnnouncements(object sender, EventArgs e)
        {
            num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;
        }
        // Scope: StatsTab, Function: ActionEvent_EnableBabStats, Enables or disables the web stats.
        private void ActionEvent_EnableBabStats(object sender, EventArgs e)
        {
            tb_webStatsServerPath.Enabled = cb_enableWebStats.Checked;
            cb_enableAnnouncements.Enabled = cb_enableWebStats.Checked;
            num_WebStatsUpdates.Enabled = cb_enableWebStats.Checked;
            num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;

        }
        // Scope: StatsTab, Function: ActionEvent_TestBabstatConnection, Tests the connection to the Babstats server.
        private void ActionEvent_TestBabstatConnection(object sender, EventArgs e)
        {
            if (StatsManager.TestBabstatsConnection(tb_webStatsServerPath.Text))
            {
                MessageBox.Show("Connection to Babstats server is successful.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to connect to Babstats server. Please check the server path and try again.", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Scope: ServerTab, Function: ActionClick_ToggleRemoteAccess, Toggles the remote access settings based on the checkbox state.
        private void ActionClick_ToggleRemoteAccess(object sender, EventArgs e)
        {
            num_remotePort.Enabled = cb_enableRemote.Checked;
        }

        private void ActionClick_AdminNewUser(object sender, EventArgs e)
        {
            btn_adminAdd.Enabled = true;
            btn_adminSave.Enabled = false;
            btn_adminDelete.Enabled = false;
            cb_adminRole.SelectedItem = null;
            cb_adminRole.Text = "Select Role";
            tb_adminUser.Text = string.Empty;
            tb_adminPass.Text = string.Empty;
            tb_adminPass.PlaceholderText = "Password";
            adminSelectedId = -1; // Reset the selected admin ID
        }
        private void ActionClick_AdminAddNew(object sender, EventArgs e)
        {
            AdminRoles selectedRole = (AdminRoles)cb_adminRole.SelectedIndex;

            if (string.IsNullOrWhiteSpace(tb_adminUser.Text) || string.IsNullOrWhiteSpace(tb_adminPass.Text) || selectedRole == AdminRoles.None)
            {
                MessageBox.Show("Please fill in all fields before adding a new admin account.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (instanceAdmin.Admins.Any(a => a.Username.Equals(tb_adminUser.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("An admin account with this username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (adminInstanceManager.addAdminAccount(tb_adminUser.Text, tb_adminPass.Text, selectedRole))
            {
                MessageBox.Show("Admin account has been added successfully.", "Admin Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Reset the fields after adding the admin account
                ActionClick_AdminNewUser(sender, e);
                adminInstanceManager.UpdateAdminLogDialog();
            }
            else
            {
                MessageBox.Show("Failed to add admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActionClick_AdminEditUser(object sender, EventArgs e)
        {
            AdminRoles selectedRole = (AdminRoles)cb_adminRole.SelectedIndex;

            // Only block if username exists for another user
            if (instanceAdmin.Admins.Any(a => a.Username.Equals(tb_adminUser.Text, StringComparison.OrdinalIgnoreCase) && a.UserId != adminSelectedId))
            {
                MessageBox.Show("An admin account with this username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Only update password if a new one is provided
            string? password = string.IsNullOrWhiteSpace(tb_adminPass.Text) ? null : tb_adminPass.Text;

            if (adminInstanceManager.updateAdminAccount(adminSelectedId, tb_adminUser.Text, password, selectedRole))
            {
                MessageBox.Show("Admin account has been updated successfully.", "Admin Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActionClick_AdminNewUser(sender, e);
                adminInstanceManager.UpdateAdminLogDialog();
            }
            else
            {
                MessageBox.Show("Failed to update admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActionClick_SelectAdmin(object sender, DataGridViewCellEventArgs e)
        {
            adminSelectedId = Convert.ToInt32(dg_AdminUsers.Rows[e.RowIndex].Cells[0].Value);
            tb_adminUser.Text = instanceAdmin.Admins.Where(a => a.UserId == adminSelectedId).Select(a => a.Username).FirstOrDefault() ?? string.Empty;
            tb_adminPass.Text = string.Empty;
            tb_adminPass.PlaceholderText = "Leave Empty to Keep Current";
            cb_adminRole.SelectedIndex = instanceAdmin.Admins.Where(a => a.UserId == adminSelectedId).Select(a => (int)a.Role).FirstOrDefault();
            btn_adminNew.Enabled = true;
            btn_adminAdd.Enabled = false;
            btn_adminSave.Enabled = true;
            btn_adminDelete.Enabled = true;
        }
        private void ActionClick_AdminDelete(object sender, EventArgs e)
        {
            // Confirm deletion
            var confirmResult = MessageBox.Show($"Are you sure you want to delete this admin with ID ({adminSelectedId}) account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                if (adminInstanceManager.removeAdminAccount(adminSelectedId))
                {
                    MessageBox.Show("Admin account has been deleted successfully.", "Admin Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActionClick_AdminNewUser(sender, e); // Reset the fields after deletion
                    adminInstanceManager.UpdateAdminLogDialog();
                }
                else
                {
                    MessageBox.Show("Failed to delete admin account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void actionClick_ExportSettings(object sender, EventArgs e)
        {
            theInstanceManager.ExportSettings();
        }

        private void actionClick_exportBanSettings(object sender, EventArgs e)
        {
            banInstanceManager.ExportSettings();
        }

        private void actionClick_importBanSettings(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Do you want to clear existing bans before importing new ones?",
                "Clear Existing Bans",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CmdImportBanSettings.ImportSettings(true);
            }
            else if (result == DialogResult.No)
            {
                CmdImportBanSettings.ImportSettings();
            }
            else
            {
                // User cancelled the import
                return;
            }

            
        }
    }
}

