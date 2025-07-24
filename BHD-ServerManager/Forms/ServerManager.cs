using BHD_ServerManager.Classes.RemoteFunctions;
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

namespace BHD_ServerManager.Forms
{
    public partial class ServerManager : Form
    {
        // The Instances (Data)
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;

        // ServerManager Local Variables
        private bool _updatingWeaponCheckboxes = false; // Prevent recursion
        private List<CheckBox> weaponCheckboxes = new();

        public ServerManager()
        {
            InitializeComponent();
            Load += PostServerManagerInitalization;
        }

        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            functionEvent_InitializeWeaponCheckboxes();

            theInstanceManager.CheckSettings();
            banInstanceManager.LoadSettings();
            chatInstanceManagers.LoadSettings();
            mapInstanceManager.ResetAvailableMaps();
            adminInstanceManager.LoadSettings();

            theInstanceManager.InitializeTickers();
            theInstanceManager.GetServerVariables();

            functionEvent_UpdateMapGameTypes();
            functionEvent_InitMapDataGrids();
            functionEvent_firstTimeLoadPlaylistToDataGride();

            cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1;
            functionEvent_UpdateAutoMessages();
            functionEvent_UpdateSlapMessages();
            functionEvent_UpdateAdminList();
            ActionClick_AdminNewUser(null!, null!);
        }

        // --- UI Thread Helper ---
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        // --- Weapon Checkbox Logic ---
        private void functionEvent_InitializeWeaponCheckboxes()
        {
            weaponCheckboxes = new()
            {
                cb_weapColt45, cb_weapM9Bereatta, cb_weapCAR15, cb_weapCAR15203, cb_weapM16, cb_weapM16203,
                cb_weapG3, cb_weapG36, cb_weapM60, cb_weapM240, cb_weapMP5, cb_weapSaw, cb_weap300Tact,
                cb_weapM21, cb_weapM24, cb_weapBarret, cb_weapPSG1, cb_weapShotgun, cb_weapFragGrenade,
                cb_weapSmokeGrenade, cb_weapSatchel, cb_weapAT4, cb_weapFlashBang, cb_weapClay
            };
        }

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

        // --- Server Status and Controls ---
        public void functionEvent_serverStatus()
        {
            toolStripStatus.Text = thisInstance.instanceStatus switch
            {
                InstanceStatus.OFFLINE => "Server is not running.",
                InstanceStatus.ONLINE => "Server is running. Game in progress.",
                InstanceStatus.SCORING => "Server is running. Game has ended, currently scoring.",
                InstanceStatus.LOADINGMAP => "Server is running. Game reset in progress.",
                InstanceStatus.STARTDELAY => "Server is running. Game ready, waiting for start.",
                _ => toolStripStatus.Text
            };
        }

        public void functionEvent_swapFieldsStartStop()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(functionEvent_swapFieldsStartStop));
                return;
            }

            bool isOffline = thisInstance.instanceStatus == InstanceStatus.OFFLINE;

            btn_startStop.Text = isOffline ? "Start Server" : "Stop Server";

            SetControlsEnabled(new Control[]
            {
                cb_serverIP, num_serverPort, cb_serverDedicated, tb_serverPassword, cb_enableRemote, num_remotePort
            }, isOffline);

            SetControlsEnabled(new Control[]
            {
                btn_mapsPlayNext, btn_mapsScore, btn_mapsSkip
            }, !isOffline);
        }

        private static void SetControlsEnabled(Control[] controls, bool enabled)
        {
            foreach (var control in controls)
                control.Enabled = enabled;
        }

        private void actionClick_resetServerChanges(object sender, EventArgs e) => theInstanceManager.GetServerVariables();

        private void actionClick_SetServerPath(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = @"C:\",
                Filter = "dfbhd.exe|dfbhd.exe",
                Title = "Select dfbhd.exe"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                thisInstance.profileServerPath = Path.GetDirectoryName(openFileDialog.FileName) ?? string.Empty;
                if (!string.IsNullOrEmpty(thisInstance.profileServerPath))
                {
                    MessageBox.Show($"Server Path set to: {thisInstance.profileServerPath}", "Server Path Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    theInstanceManager.ValidateGameServerType(thisInstance.profileServerPath);
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

        private void actionClick_saveUpdateSettings(object sender, EventArgs e)
        {
            theInstanceManager.SetServerVariables();
            theInstanceManager.SaveSettings();

            if (GameManager.ReadMemoryIsProcessAttached())
            {
                theInstanceManager.UpdateGameServer();
                MessageBox.Show("Settings have been saved and game server updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void actionClick_startServer(object sender, EventArgs e)
        {
            if (theInstanceManager.ValidateGameServerPath() && thisInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                theInstanceManager.SetServerVariables();
                if (GameManager.startGame())
                {
                    GameManager.ReadMemoryServerStatus();
                    functionEvent_swapFieldsStartStop();
                }
            }
            else
            {
                GameManager.stopGame();
            }
        }

        private void actionClick_enableFFkills(object sender, EventArgs e) => num_maxFFKills.Enabled = cb_enableFFkills.Checked;
        private void actionClick_enableMinCheck(object sender, EventArgs e) => num_minPing.Enabled = cb_enableMinCheck.Checked;
        private void actionClick_enableMaxPing(object sender, EventArgs e) => num_maxPing.Enabled = cb_enableMaxCheck.Checked;
        private void ActionClick_ToggleRemoteAccess(object sender, EventArgs e) => num_remotePort.Enabled = cb_enableRemote.Checked;

        private void actionClick_importServerSettings(object sender, EventArgs e) => theInstanceManager.ImportSettings();
        private void actionClick_ExportSettings(object sender, EventArgs e) => theInstanceManager.ExportSettings();

        // --- Map Tab ---
        private void functionEvent_UpdateMapGameTypes()
        {
            foreach (var gameType in objectGameTypes.All)
                combo_gameTypes.Items.Add(gameType.Name!);
            combo_gameTypes.Items.RemoveAt(0);
            combo_gameTypes.Items.Add("All Game Types");
            combo_gameTypes.SelectedIndex = 9;
        }

        private void functionEvent_InitMapDataGrids()
        {
            functionEvent_SetupAvailableMapsGrid();
            functionEvent_PopulateMapDataGrid(dataGridView_availableMaps, CommonCore.instanceMaps.availableMaps, false);

            functionEvent_SetupCurrentMapsGrid();
            functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, CommonCore.instanceMaps.currentMapPlaylist, true);
        }

        private void functionEvent_SetupAvailableMapsGrid()
        {
            SetupDataGridView(dataGridView_availableMaps, new[] { ("MapName", "Name", 120), ("GameType", "Type", 50) });
        }

        private void functionEvent_SetupCurrentMapsGrid()
        {
            SetupDataGridView(dataGridView_currentMaps, new[] { ("MapNum", "#", 30), ("MapName", "Name", 120), ("GameType", "Type", 50) });
        }

        // Helper for DataGridView setup (could be moved to a static class)
        private static void SetupDataGridView(DataGridView dgv, (string name, string header, int width)[] columns)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            foreach (var (name, header, width) in columns)
            {
                dgv.Columns.Add(name, header);
                dgv.Columns[name].Width = width;
                dgv.Columns[name].AutoSizeMode = name == "MapName" ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None;
                if (name == "GameType")
                {
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns[name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (name == "MapNum")
                {
                    dgv.Columns[name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void functionEvent_PopulateMapDataGrid(DataGridView dgv, IEnumerable<mapFileInfo> maps, bool includeRowNum)
        {
            SafeInvoke(dgv, () =>
            {
                dgv.Rows.Clear();
                int rowNum = 1;
                foreach (var map in maps)
                {
                    if (includeRowNum)
                        dgv.Rows.Add(rowNum++, map.MapName, map.GameType);
                    else
                        dgv.Rows.Add(map.MapName, map.GameType);
                }
            });
        }

        public void functionEvent_UpdateMapRowNumbers(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
                if (!dgv.Rows[i].IsNewRow)
                    dgv.Rows[i].Cells["MapNum"].Value = i + 1;
        }

        public void functionEvent_RefreshAvailableMaps()
        {
            mapInstanceManager.ResetAvailableMaps();
            functionEvent_PopulateMapDataGrid(dataGridView_availableMaps, instanceMaps.availableMaps, false);
        }

        private string[]? functionEvent_GetFileLinesFromDialog(bool saveDialog, string filter, string title, string initialDirectory, string defaultFileName)
        {
            string? filePath = Functions.ShowFileDialog(saveDialog, filter, title, initialDirectory, defaultFileName);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;
            return File.ReadAllLines(filePath, Encoding.UTF8);
        }

        private IEnumerable<mapFileInfo> functionEvent_ParseMapFileInfos(string[] lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                yield return new mapFileInfo { MapFile = parts[0], MapName = parts[1], GameType = parts[2] };
            }
        }

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

        public void functionEvent_LoadCurrentPlaylistToDataGrid() =>
            functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, instanceMaps.currentMapPlaylist, true);

        public void functionEvent_firstTimeLoadPlaylistToDataGride()
        {
            string? startPath = CommonCore.AppDataPath;
            string filePath = Path.Combine(startPath, "currentMapPlaylist.mpl");
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                functionEvent_PopulateMapDataGrid(dataGridView_currentMaps, functionEvent_ParseMapFileInfos(lines), true);
                actionClick_saveCurrentMapList(null!, null!);
            }
            else
            {
                MessageBox.Show("No current map playlist found. Please import or create a new playlist.", "No Playlist Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- MapTab Action Handlers ---
        private void actionClick_refreshMaps(object sender, EventArgs e)
        {
            functionEvent_RefreshAvailableMaps();
            combo_gameTypes.SelectedIndex = 9;
        }

        private void actionClick_clearCurrentMapPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        private void actionClick_addMap2Current(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView_currentMaps.Rows.Count < 128)
            {
                DataGridViewRow selectedRow = dataGridView_availableMaps.Rows[e.RowIndex];
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

        private void actionClick_delRowFromCurrent(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView_currentMaps.Rows.RemoveAt(e.RowIndex);
                functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
            }
        }

        private void actionClick_saveCurrentMapList(object sender, EventArgs e)
        {
            instanceMaps.previousMapPlaylist = instanceMaps.currentMapPlaylist;
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();
            instanceMaps.currentMapPlaylist = newMapPlaylist;
            mapInstanceManager.SaveCurrentMapPlaylist(newMapPlaylist, false);
        }

        private void actionClick_exportCurrentPlaylist(object sender, EventArgs e)
        {
            List<mapFileInfo> newMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();
            mapInstanceManager.SaveCurrentMapPlaylist(newMapPlaylist, true);
        }

        private void actionClick_importMapPlaylist(object sender, EventArgs e)
        {
            functionEvent_ImportMapPlaylistToCurrentDataGrid();
        }

        private void actionClick_resetCurrentMapList(object sender, EventArgs e)
        {
            functionEvent_LoadCurrentPlaylistToDataGrid();
            functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
        }

        private void actionChange_mapFilterGameType(object sender, EventArgs e)
        {
            if (combo_gameTypes.SelectedIndex >= 0)
            {
                string selectedGameType = string.Empty;
                if (combo_gameTypes.SelectedIndex != 9)
                {
                    selectedGameType = objectGameTypes.All[combo_gameTypes.SelectedIndex].ShortName!;
                }

                foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                {
                    row.Visible = combo_gameTypes.SelectedIndex == 9 ||
                        row.Cells["GameType"].Value?.ToString() == selectedGameType;
                }
            }
        }

        private void actionKeyEvent_moveMap(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.W || e.KeyChar == (char)Keys.S)
            {
                int currentRowIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
                int newRowIndex = e.KeyChar == (char)Keys.W ? currentRowIndex - 1 : currentRowIndex + 1;
                if (newRowIndex >= 0 && newRowIndex < dataGridView_currentMaps.Rows.Count)
                {
                    DataGridViewRow selectedRow = dataGridView_currentMaps.Rows[currentRowIndex];
                    dataGridView_currentMaps.Rows.RemoveAt(currentRowIndex);
                    dataGridView_currentMaps.Rows.Insert(newRowIndex, selectedRow);
                    dataGridView_currentMaps.CurrentCell = dataGridView_currentMaps.Rows[newRowIndex].Cells[0];
                    functionEvent_UpdateMapRowNumbers(dataGridView_currentMaps);
                }
            }
        }

        private void actionClick_updateGameServerMaps(object sender, EventArgs e)
        {
            actionClick_saveCurrentMapList(sender, e);

            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                MessageBox.Show("The server map list has been updated successfully.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (thisInstance.instanceStatus == InstanceStatus.STARTDELAY || thisInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                GameManager.UpdateMapCycle1();
                GameManager.UpdateMapCycle2();
                MessageBox.Show("The server map list has been updated successfully.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The Maplist saved but server is currently not ready to recieve map updates. Please wait for Status Change of Server.", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void actionClick_mapPlayNext(object sender, EventArgs e)
        {
            int mapIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
            GameManager.UpdateNextMap(mapIndex);
            MessageBox.Show(dataGridView_currentMaps.Rows[mapIndex].Cells[1].Value + " has been updated to play next.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actionClick_mapScore(object sender, EventArgs e)
        {
            GameManager.WriteMemoryScoreMap();
        }

        private void actionClick_mapSkip(object sender, EventArgs e)
        {
            if (thisInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                thisInstance.instanceMapSkipped = true;
                GameManager.WriteMemorySendConsoleCommand("resetgames");
            }
            else
            {
                MessageBox.Show("Sorry you can't skip currently, must wait until the map is playing to use this.", "Cannot Skip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- Chat Tab ---
        public void functionEvent_UpdateSlapMessages()
        {
            SafeInvoke(dg_slapMessages, () =>
            {
                dg_slapMessages.Rows.Clear();
                foreach (var slapMsg in chatInstanceManagers.GetSlapMessages())
                    dg_slapMessages.Rows.Add(slapMsg.SlapMessageId, slapMsg.SlapMessageText);
            });
        }

        public void functionEvent_UpdateAutoMessages()
        {
            SafeInvoke(dg_autoMessages, () =>
            {
                dg_autoMessages.Rows.Clear();
                foreach (var autoMsg in chatInstanceManagers.GetAutoMessages())
                    dg_autoMessages.Rows.Add(autoMsg.AutoMessageId, autoMsg.AutoMessageTigger, autoMsg.AutoMessageText);
            });
        }

        private void actionKeyEvent_submitMessage(object sender, EventArgs e)
        {
            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE ||
                thisInstance.instanceStatus == InstanceStatus.LOADINGMAP ||
                thisInstance.instanceStatus == InstanceStatus.SCORING)
            {
                MessageBox.Show("Server is not in a state to accept messages, please start or wait for the map to be ready or playing.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string message = tb_chatMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            int channel = 0;
            if (message.Length >= 2 && message[0] == '/')
            {
                switch (message.Substring(1, 1).ToLower())
                {
                    case "y": channel = 1; break;
                    case "r": channel = 2; break;
                    case "b": channel = 3; break;
                    default: channel = 0; break;
                }
                message = message.Substring(2);
            }

            if (message.Contains("{P:") && message.Contains("}"))
            {
                int startIndex = message.IndexOf("{P:") + 3;
                int endIndex = message.IndexOf("}", startIndex);
                if (endIndex > startIndex)
                {
                    string playerSlot = message.Substring(startIndex, endIndex - startIndex);
                    var playerEntry = thisInstance.playerList.FirstOrDefault(p => p.Value.PlayerSlot.ToString() == playerSlot);
                    if (playerEntry.Value == null)
                    {
                        MessageBox.Show($"No player found in PlayerSlot {playerSlot}. Message will not be sent.", "Player Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string playerName = playerEntry.Value.PlayerName ?? string.Empty;
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        playerName = Functions.SanitizePlayerName(playerName);
                        message = message.Replace($"{{P:{playerSlot}}}", playerName);
                    }
                }
            }

            if (message.Length > 59)
            {
                for (int i = 0; i < message.Length; i += 59)
                {
                    string chunk = message.Substring(i, Math.Min(59, message.Length - i));
                    GameManager.WriteMemorySendChatMessage(channel, chunk);
                }
            }
            else
            {
                GameManager.WriteMemorySendChatMessage(channel, message);
            }

            tb_chatMessage.Clear();
        }

        private void actionKeyEvent_submitEnterMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                actionKeyEvent_submitMessage(sender, e);
            }
        }

        private void actionClick_addSlapMessage(object sender, EventArgs e)
        {
            chatInstanceManagers.AddSlapMessage(tb_slapMessage.Text.Trim());
            functionEvent_UpdateSlapMessages();
            tb_slapMessage.Text = string.Empty;
        }

        private void actionClick_addAutoMessages(object sender, EventArgs e)
        {
            chatInstanceManagers.AddAutoMessage(tb_autoMessage.Text.Trim(), (int)num_AutoMessageTrigger.Value);
            functionEvent_UpdateAutoMessages();
            tb_autoMessage.Text = string.Empty;
            num_AutoMessageTrigger.Value = 0;
        }

        private void actionDbClick_RemoveAutoMessage(object sender, DataGridViewCellEventArgs e)
        {
            int AutoMessageId = Convert.ToInt32(dg_autoMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveAutoMessage(AutoMessageId);
            functionEvent_UpdateAutoMessages();
        }

        private void actionDbClick_RemoveSlapMessage(object sender, DataGridViewCellEventArgs e)
        {
            int slapMessageId = Convert.ToInt32(dg_slapMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveSlapMessage(slapMessageId);
            functionEvent_UpdateSlapMessages();
        }

        // --- Ban Tab ---
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
                    banInstanceManager.RemoveBannedPlayerBoth(recordId);
                else if (result == DialogResult.No)
                    banInstanceManager.RemoveBannedPlayerName(recordId);
                else if (result == DialogResult.Cancel)
                    banInstanceManager.RemoveBannedPlayerAddress(recordId);
            }
            else if (foundInNames)
                banInstanceManager.RemoveBannedPlayerName(recordId);
            else if (foundInAddresses)
                banInstanceManager.RemoveBannedPlayerAddress(recordId);
        }

        private void actionClick_addBanInformation(object sender, EventArgs e)
        {
            string playerName = tb_bansPlayerName.Text.Trim();
            string playerIP = tb_bansIPAddress.Text.Trim();
            int submask = cb_banSubMask.SelectedIndex + 1;

            string EncodedPlayerName = string.Empty;
            IPAddress ipAddress = null!;

            if (!string.IsNullOrEmpty(playerName))
                EncodedPlayerName = Convert.ToBase64String(Encoding.UTF8.GetBytes(playerName));
            if (!string.IsNullOrEmpty(playerIP))
                ipAddress = IPAddress.Parse(playerIP);

            banInstanceManager.AddBannedPlayer(EncodedPlayerName, ipAddress!, submask);

            MessageBox.Show("Ban information has been added successfully.", "Ban Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

            banInstanceManager.UpdateBannedTables();

            tb_bansPlayerName.Text = string.Empty;
            tb_bansIPAddress.Text = string.Empty;
            cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1;
        }

        private void actionDbClick_RemoveRecord2(object sender, DataGridViewCellEventArgs e)
        {
            int recordId = Convert.ToInt32(dg_IPAddresses.Rows[e.RowIndex].Cells[0].Value);
            functionEvent_RemoveBannedPlayer(recordId);
            banInstanceManager.UpdateBannedTables();
        }

        private void actionDbClick_RemoveRecord(object sender, DataGridViewCellEventArgs e)
        {
            int recordId = Convert.ToInt32(dg_playerNames.Rows[e.RowIndex].Cells[0].Value);
            functionEvent_RemoveBannedPlayer(recordId);
        }

        // --- Admin Tab ---
        public void functionEvent_UpdateAdminList()
        {
            SafeInvoke(dg_AdminUsers, () =>
            {
                dg_AdminUsers.Rows.Clear();
                foreach (var admin in instanceAdmin.Admins)
                    dg_AdminUsers.Rows.Add(admin.UserId, admin.Username, admin.Role.ToString());
            });
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
                ActionClick_AdminNewUser(sender, e);
                functionEvent_UpdateAdminList();
            }
            else
            {
                MessageBox.Show("Failed to add admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionClick_AdminEditUser(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[dg_AdminUsers.CurrentCell.RowIndex].Cells[0].Value);
            AdminRoles selectedRole = (AdminRoles)cb_adminRole.SelectedIndex;

            if (instanceAdmin.Admins.Any(a => a.Username.Equals(tb_adminUser.Text, StringComparison.OrdinalIgnoreCase) && a.UserId != userID))
            {
                MessageBox.Show("An admin account with this username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? password = string.IsNullOrWhiteSpace(tb_adminPass.Text) ? null : tb_adminPass.Text;

            if (adminInstanceManager.updateAdminAccount(userID, tb_adminUser.Text, password, selectedRole))
            {
                MessageBox.Show("Admin account has been updated successfully.", "Admin Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActionClick_AdminNewUser(sender, e);
                functionEvent_UpdateAdminList();
            }
            else
            {
                MessageBox.Show("Failed to update admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionClick_SelectAdmin(object sender, DataGridViewCellEventArgs e)
        {
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[e.RowIndex].Cells[0].Value);
            tb_adminUser.Text = instanceAdmin.Admins.Where(a => a.UserId == userID).Select(a => a.Username).FirstOrDefault() ?? string.Empty;
            tb_adminPass.Text = string.Empty;
            tb_adminPass.PlaceholderText = "Leave Empty to Keep Current";
            cb_adminRole.SelectedIndex = instanceAdmin.Admins.Where(a => a.UserId == userID).Select(a => (int)a.Role).FirstOrDefault();
            btn_adminNew.Enabled = true;
            btn_adminAdd.Enabled = false;
            btn_adminSave.Enabled = true;
            btn_adminDelete.Enabled = true;
        }

        private void ActionClick_AdminDelete(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[dg_AdminUsers.CurrentCell.RowIndex].Cells[0].Value);
            var confirmResult = MessageBox.Show("Are you sure you want to delete this admin account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                if (adminInstanceManager.removeAdminAccount(userID))
                {
                    MessageBox.Show("Admin account has been deleted successfully.", "Admin Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActionClick_AdminNewUser(sender, e);
                    functionEvent_UpdateAdminList();
                }
                else
                {
                    MessageBox.Show("Failed to delete admin account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Stats Tab ---
        private void ActionEvent_EnableAnnouncements(object sender, EventArgs e) =>
            num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;

        private void ActionEvent_EnableBabStats(object sender, EventArgs e)
        {
            bool enabled = cb_enableWebStats.Checked;
            tb_webStatsServerPath.Enabled = enabled;
            cb_enableAnnouncements.Enabled = enabled;
            num_WebStatsUpdates.Enabled = enabled;
            num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;
        }

        private async void ActionEvent_TestBabstatConnection(object sender, EventArgs e)
        {
            bool result = await Task.Run(() => StatsManager.TestBabstatsConnection(tb_webStatsServerPath.Text));
            MessageBox.Show(
                result ? "Connection to Babstats server is successful." : "Failed to connect to Babstats server. Please check the server path and try again.",
                "Connection Test" + (result ? "" : " Failed"),
                MessageBoxButtons.OK,
                result ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );
        }

        // --- Form Closing ---
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            CommonCore.Ticker?.Stop("ServerManager");
            CommonCore.Ticker?.Stop("ChatManager");
            CommonCore.Ticker?.Stop("PlayerManager");
            CommonCore.Ticker?.Stop("BanManager");
            RemoteServer.Stop();
            theInstanceManager.SaveSettings();
            base.OnFormClosing(e);
        }
    }
}