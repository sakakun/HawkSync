using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.Instances;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels
{
	public partial class tabStats : UserControl
	{
		private theInstance theInstance => CommonCore.theInstance!;
		private statInstance statInstance => CommonCore.instanceStats!;
		private playerInstance playerInstance => CommonCore.instancePlayers!;

		private static DateTime _lastPlayerStatsUpdate = DateTime.MinValue;
		private static DateTime _lastWeaponStatsUpdate = DateTime.MinValue;
		private List<BabstatsServerSettings> _BabstatServers = new List<BabstatsServerSettings>();
		private List<LobbyServerSettings> _LobbyServers = new List<LobbyServerSettings>();
		private int _BabstatsSelectedID;
		private int _LobbySelectedID;

		public tabStats()
		{
			InitializeComponent();

			// Babstats Server List
			LoadWebStatsSettings();
			babstatsFormReset();
			babstatsButtonsReset();
			// Lobby Server List
			LoadLobbySettings();



			ApiCore.OnSnapshotReceived += OnSnapshotReceived;

		}

		private bool AreBabstatsServerListsEqual(List<BabstatsServerSettings> listA, List<BabstatsServerSettings> listB)
		{
			if (ReferenceEquals(listA, listB))
				return true;
			if (listA == null || listB == null)
				return false;
			if (listA.Count != listB.Count)
				return false;

			// Compare each item (order-insensitive)
			var setA = new HashSet<BabstatsServerSettings>(listA);
			var setB = new HashSet<BabstatsServerSettings>(listB);
			return setA.SetEquals(setB);
		}

		private void LoadWebStatsSettings()
		{

			if (!AreBabstatsServerListsEqual(statInstance.BabstatsServers, _BabstatServers))
			{
				_BabstatServers = statInstance.BabstatsServers;
			}
			else
			{
				return;
			}

			// Clear existing items
			babstats_table.Rows.Clear();
			// Add each server to the table
			foreach (var server in _BabstatServers)
			{
				int rowIndex = babstats_table.Rows.Add(
					server.BabstatsServerID,                // babstats_id (hidden)
					server.ProfileID,                       // babstats_code ("Profile ID")
					server.ServerPath,                      // babstats_siteurl ("Babstats URL")
					server.IsEnabled,                       // babstats_enabled (bool)
					server.UpdateIntervalSeconds,           // babstats_updateinterval ("Update (s)")
					server.EnableAnnouncements,             // babstats_annoucements (bool)
					server.ReportIntervalSeconds            // babstats_reportinterval ("Report (s)")
				);
				DataGridViewRow row = babstats_table.Rows[rowIndex];
				row.Tag = server; // Store the server settings in the row's Tag for later reference
			}
		}

		private bool AreLobbyServerListsEqual(List<LobbyServerSettings> listA, List<LobbyServerSettings> listB)
		{
			if (ReferenceEquals(listA, listB))
				return true;
			if (listA == null || listB == null)
				return false;
			if (listA.Count != listB.Count)
				return false;

			// Compare each item (order-insensitive)
			var setA = new HashSet<LobbyServerSettings>(listA);
			var setB = new HashSet<LobbyServerSettings>(listB);
			return setA.SetEquals(setB);
		}

		private void LoadLobbySettings()
		{

			if (!AreLobbyServerListsEqual(statInstance.LobbyServers, _LobbyServers))
			{
				_LobbyServers = statInstance.LobbyServers;
			}
			else
			{
				return;
			}

			// Clear existing items
			lobby_table.Rows.Clear();
			// Add each server to the table
			foreach (var server in _LobbyServers)
			{
				int rowIndex = lobby_table.Rows.Add(
					server.LobbyServerID,                // lobby_id (hidden)
					server.SiteName,                    // lobby_sitename ("Site Name")
					server.ServerUri,                    // lobby_serveruri ("Server URI")
					server.GamePort,                    // lobby_gameport ("Game Port")
					server.SecretKey,                    // lobby_secretkey
					server.IsEnabled                     // lobby_enabled
				);

				DataGridViewRow row = lobby_table.Rows[rowIndex];
				row.Tag = server; // Store the server settings in the row's Tag for later reference
			}
		}

		private async void OnSnapshotReceived(ServerSnapshot snapshot)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => OnSnapshotReceived(snapshot)));
				return;
			}

			LoadWebStatsSettings();
			LoadLobbySettings();
			UpdateWebStatsLog();

			PopulatePlayerStatsGrid();
			PopulateWeaponStatsGrid();
		}

		public void UpdateWebStatsLog()
		{
			List<StatReportObject> logEntries = statInstance.WebStatsLog ?? new List<StatReportObject>();

			var currentRows = new HashSet<string>();
			foreach (DataGridViewRow row in dg_statsLog.Rows)
			{
				if (row.IsNewRow)
				{
					continue;
				}

				string dateStr = row.Cells[0].Value as string ?? string.Empty;
				string content = row.Cells[1].Value as string ?? string.Empty;
				currentRows.Add($"{dateStr}|{content}");
			}

			for (int i = dg_statsLog.Rows.Count - 1; i >= 0; i--)
			{
				DataGridViewRow row = dg_statsLog.Rows[i];
				if (row.IsNewRow)
				{
					continue;
				}

				string dateStr = row.Cells[0].Value as string ?? string.Empty;
				string content = row.Cells[1].Value as string ?? string.Empty;
				bool exists = logEntries.Any(e =>
					e.ReportDate.ToString("yyyy-MM-dd HH:mm:ss") == dateStr &&
					e.ReportContent == content);

				if (!exists)
				{
					dg_statsLog.Rows.RemoveAt(i);
				}
			}

			bool rowsAdded = false;
			foreach (StatReportObject entry in logEntries)
			{
				string dateStr = entry.ReportDate.ToString("yyyy-MM-dd HH:mm:ss");
				string key = $"{dateStr}|{entry.ReportContent}";

				if (!currentRows.Contains(key))
				{
					dg_statsLog.Rows.Add(dateStr, entry.ReportContent);
					rowsAdded = true;
				}
			}

			if (rowsAdded || dg_statsLog.SortedColumn == null)
			{
				ApplySortToStatsLog(dg_statsLog);
			}
		}

		private void ApplySortToStatsLog(DataGridView grid)
		{
			if (grid.Rows.Count == 0)
			{
				return;
			}

			DataGridViewColumn? sortColumn = grid.SortedColumn;
			SortOrder sortOrder = grid.SortOrder;

			if (sortColumn == null || sortOrder == SortOrder.None)
			{
				if (grid.Columns.Count > 0)
				{
					grid.Sort(grid.Columns[0], System.ComponentModel.ListSortDirection.Descending);
				}
			}
			else
			{
				var direction = sortOrder == SortOrder.Ascending
					? System.ComponentModel.ListSortDirection.Ascending
					: System.ComponentModel.ListSortDirection.Descending;
				grid.Sort(sortColumn, direction);
			}
		}

		private async void OnTestBabstatConnectionClick(object? sender, EventArgs e)
		{
			Button? button = sender as Button;
			if (button != null)
			{
				button.Enabled = false;
			}

			try
			{
				CommandResult result = await ApiCore.ApiClient!.Stats.ValidateWebStatsConnectionAsync(tb_webStatsServerPath.Text);
				MessageBox.Show(
					result.Message,
					result.Success ? "Connection Test" : "Connection Test Failed",
					MessageBoxButtons.OK,
					result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"An error occurred while testing the connection:\n\n{ex.Message}",
					"Connection Test Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
			finally
			{
				if (button != null)
				{
					button.Enabled = true;
				}
			}
		}

		public void PopulatePlayerStatsGrid()
		{
			if ((DateTime.UtcNow - _lastPlayerStatsUpdate).TotalSeconds < 15)
			{
				return;
			}

			_lastPlayerStatsUpdate = DateTime.UtcNow;
			dataGridViewPlayerStats.Rows.Clear();

			foreach (PlayerStatObject statObj in statInstance.playerStatsList.Values)
			{
				PlayerObject player = statObj.PlayerStatsCurrent;
				string shotsPerKill = player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
				bool playerActive = playerInstance.PlayerList.Values.Any(p => p.PlayerName == player.PlayerName);

				dataGridViewPlayerStats.Rows.Add(
					player.PlayerName,
					player.stat_Suicides,
					player.stat_Murders,
					player.stat_Kills,
					player.stat_Deaths,
					player.stat_ZoneTime,
					player.stat_FBCaptures,
					player.stat_FlagSaves,
					player.stat_SDADTargetsDestroyed,
					player.stat_RevivesReceived,
					player.stat_RevivesGiven,
					player.stat_PSPAttempts,
					player.stat_PSPTakeovers,
					player.stat_FBCarrierKills,
					player.stat_DoubleKills,
					player.stat_Headshots,
					player.stat_KnifeKills,
					player.stat_SniperKills,
					player.stat_TKOTHDefenseKills,
					player.stat_TKOTHAttackKills,
					shotsPerKill,
					player.stat_ExperiencePoints,
					player.PlayerTeam,
					playerActive ? "1" : "0",
					player.PlayerTimePlayed
				);
			}
		}

		public void PopulateWeaponStatsGrid()
		{
			if ((DateTime.UtcNow - _lastWeaponStatsUpdate).TotalSeconds < 15)
			{
				return;
			}

			_lastWeaponStatsUpdate = DateTime.UtcNow;
			dataGridViewWeaponStats.Rows.Clear();

			foreach (PlayerStatObject statObj in statInstance.playerStatsList.Values)
			{
				PlayerObject player = statObj.PlayerStatsCurrent;
				foreach (WeaponStatObject weaponStat in statObj.PlayerWeaponStats)
				{
					dataGridViewWeaponStats.Rows.Add(
						player.PlayerName,
						((WeaponStack)weaponStat.WeaponID).ToString(),
						weaponStat.TimeUsed,
						weaponStat.Kills,
						weaponStat.Shots
					);
				}
			}
		}

		private void babstatsButtonsReset()
		{
			btn_SaveSettings.Enabled = false;
			btn_RemoveServer.Enabled = false;
			btn_AddServer.Enabled = true;
			btn_NewServer.Enabled = true;
			btn_NewServer.Text = "New";
		}
		private void babstatsFormReset()
		{
			_BabstatsSelectedID = 0;
			tb_webStatsServerPath.Text = string.Empty;
			tb_serverID.Text = string.Empty;
			cb_enableWebStats.Checked = false;
			cb_enableAnnouncements.Checked = false;
			num_WebStatsReport.Value = 60;
			num_WebStatsUpdates.Value = 60;
		}

		private void babstatsClick_openRecord(object sender, EventArgs e)
		{
			// Ensure a row is selected
			if (babstats_table.SelectedRows.Count == 0)
				return;

			// Get the selected row and its associated BabstatsServerSettings
			DataGridViewRow selectedRow = babstats_table.SelectedRows[0];
			if (selectedRow.Tag is not BabstatsServerSettings server)
				return;

			// Populate form fields
			_BabstatsSelectedID = server.BabstatsServerID;
			tb_webStatsServerPath.Text = server.ServerPath;
			tb_serverID.Text = server.ProfileID;
			cb_enableWebStats.Checked = server.IsEnabled;
			cb_enableAnnouncements.Checked = server.EnableAnnouncements;
			num_WebStatsReport.Value = Math.Max(num_WebStatsReport.Minimum, Math.Min(num_WebStatsReport.Maximum, server.ReportIntervalSeconds));
			num_WebStatsUpdates.Value = Math.Max(num_WebStatsUpdates.Minimum, Math.Min(num_WebStatsUpdates.Maximum, server.UpdateIntervalSeconds));


			// Buttons enabled: Save, Remove
			btn_SaveSettings.Enabled = true;
			btn_RemoveServer.Enabled = true;
			btn_AddServer.Enabled = false;
			btn_NewServer.Enabled = true;
			btn_NewServer.Text = "Cancel";
		}

		private async void babstatsClick_addServer(object sender, EventArgs e)
		{
			// New Record - Clear selected ID and form for new entry, this should already be done because of the form reset when clicking "New", but we'll do it again here to be safe.
			_BabstatsSelectedID = 0;

			// Collect Form Details for new server to be added, then add to database and refresh list
			BabstatsServerSettings newServer = new BabstatsServerSettings(
				0, // BabstatsServerID, will be set by the database
				string.Empty, // DisplayName
				tb_webStatsServerPath.Text.Trim(), // ServerPath
				tb_serverID.Text.Trim(), // ProfileID
				cb_enableWebStats.Checked, // IsEnabled
				cb_enableAnnouncements.Checked, // EnableAnnouncements
				(int)num_WebStatsReport.Value, // ReportIntervalSeconds
				(int)num_WebStatsUpdates.Value, // UpdateIntervalSeconds
				0 // SortOrder
			);

			if (cb_enableAnnouncements.Checked)
			{
				DialogResult dialogResult = MessageBox.Show(
					"You have enabled Announcements for this Babstats server. This will set Announcements to false for all other Babstats servers. Do you want to continue?",
					"Enable Announcements",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning
				);
				if (dialogResult == DialogResult.No)
				{
					return;
				}
				else
				{
					// Set Announcements to false for all other servers
					_ = await ApiCore.ApiClient!.Stats.ClearBabstatsAnnoucementsAsync();
				}
			}

			// Add the new server via API
			CommandResult result = await ApiCore.ApiClient!.Stats.AddBabstatsServersAsync(newServer);

			MessageBox.Show(
				result.Message,
				result.Success ? "Babstats Server Updated" : "Failed to update Babstats Server",
				MessageBoxButtons.OK,
				result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
			);

			// Refresh the server list
			LoadWebStatsSettings();
			// Reset Form
			babstatsFormReset();
			// Reset Buttons
			babstatsButtonsReset();

		}

		private async void babstatsClick_saveServer(object sender, EventArgs e)
		{
			// Collect Form Details for new server to be added, then add to database and refresh list
			BabstatsServerSettings updatedServer = new BabstatsServerSettings(
				_BabstatsSelectedID, // BabstatsServerID, will be set by the database
				string.Empty, // DisplayName
				tb_webStatsServerPath.Text.Trim(), // ServerPath
				tb_serverID.Text.Trim(), // ProfileID
				cb_enableWebStats.Checked, // IsEnabled
				cb_enableAnnouncements.Checked, // EnableAnnouncements
				(int)num_WebStatsReport.Value, // ReportIntervalSeconds
				(int)num_WebStatsUpdates.Value, // UpdateIntervalSeconds
				0 // SortOrder
			);

			if (cb_enableAnnouncements.Checked)
			{
				DialogResult dialogResult = MessageBox.Show(
					"You have enabled Announcements for this Babstats server. This will set Announcements to false for all other Babstats servers. Do you want to continue?",
					"Enable Announcements",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning
				);
				if (dialogResult == DialogResult.No)
				{
					return;
				}
				else
				{
					// Set Announcements to false for all other servers
					_ = await ApiCore.ApiClient!.Stats.ClearBabstatsAnnoucementsAsync();
				}
			}
			// Update the server in the database
			CommandResult result = await ApiCore.ApiClient!.Stats.SaveBabstatsServersAsync(updatedServer);

			MessageBox.Show(
				result.Message,
				result.Success ? "Babstats Server Updated" : "Failed to update Babstats Server",
				MessageBoxButtons.OK,
				result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
			);

			// Refresh the server list
			LoadWebStatsSettings();
			// Reset Form
			babstatsFormReset();
			// Reset Buttons
			babstatsButtonsReset();
		}

		private async void babstatsClick_RemoveRecord(object sender, EventArgs e)
		{
			// Remove the selected server from the database, then refresh list and reset form/buttons
			// Dialog to confirm deletion
			DialogResult dialogResult = MessageBox.Show(
				"Are you sure you want to remove this Babstats server? This action cannot be undone.",
				"Confirm Removal",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning
			);

			if (dialogResult == DialogResult.No)
			{
				return;
			}

			if (dialogResult == DialogResult.Yes)
			{
				// Update the server in the database
				CommandResult result = await ApiCore.ApiClient!.Stats.RemoveBabstatsServersAsync(_BabstatsSelectedID);
				// 
				if (result.Success)
				{
					MessageBox.Show("Babstats server removed successfully.", "Removal Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}
				else
				{
					MessageBox.Show("Failed to remove Babstats server. Please try again.", "Removal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// Refresh the server list
			LoadWebStatsSettings();
			// Reset Form
			babstatsFormReset();
			// Reset Buttons
			babstatsButtonsReset();
		}

		private void babstatsClick_newServer(object sender, EventArgs e)
		{
			// Reset Form
			babstatsFormReset();
			// Reset Buttons
			babstatsButtonsReset();
		}

		private void lobbyAction_RandomizeKey(object sender, EventArgs e)
		{
			// Generate a 16-character cryptographically-secure alphanumeric key
			const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			const int length = 20;

			Span<char> chars = stackalloc char[length];
			Span<byte> randomBytes = stackalloc byte[length];

			System.Security.Cryptography.RandomNumberGenerator.Fill(randomBytes);

			for (int i = 0; i < length; i++)
			{
				chars[i] = alphabet[randomBytes[i] % alphabet.Length];
			}

			tb_lobbySecretKey.Text = new string(chars);
		}

		private async void lobbyAction_saveRecord(object sender, EventArgs e)
		{

			LobbyServerSettings saveRequest = new LobbyServerSettings(
				_LobbySelectedID,
				tb_lobbySiteName.Text.Trim(),
				tb_lobbySiteUri.Text.Trim(),
				(int)num_lobbyGamePort.Value,
				tb_lobbySecretKey.Text.Trim(),
				cb_lobbyEnabled.Checked,
				0
			);

			// Update the server in the database
			CommandResult result = await ApiCore.ApiClient!.Stats.SaveLobbyServersAsync(saveRequest);

			MessageBox.Show(
				result.Message,
				result.Success ? "Babstats Server Updated" : "Failed to update Babstats Server",
				MessageBoxButtons.OK,
				result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
			);

			// Refresh the server list
			LoadLobbySettings();
			// Reset Form
			lobbyAction_ResetForm();
			// Reset Buttons
			lobbyAction_ButtonReset();

		}

		private async void lobbyAction_addRecord(object sender, EventArgs e)
		{
			LobbyServerSettings newServer = new LobbyServerSettings(
				0,
				tb_lobbySiteName.Text.Trim(),
				tb_lobbySiteUri.Text.Trim(),
				(int)num_lobbyGamePort.Value,
				tb_lobbySecretKey.Text.Trim(),
				cb_lobbyEnabled.Checked,
				0
			);

			// Add the new server via API
			CommandResult result = await ApiCore.ApiClient!.Stats.AddLobbyServersAsync(newServer);

			MessageBox.Show(
				result.Message,
				result.Success ? "Babstats Server Updated" : "Failed to update Babstats Server",
				MessageBoxButtons.OK,
				result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
			);

			// Refresh the server list
			LoadLobbySettings();
			// Reset Form
			lobbyAction_ResetForm();
			// Reset Buttons
			lobbyAction_ButtonReset();
		}

		private async void lobbyAction_removeRecord(object sender, EventArgs e)
		{
			// Remove the selected server from the database, then refresh list and reset form/buttons
			// Dialog to confirm deletion
			DialogResult dialogResult = MessageBox.Show(
				"Are you sure you want to remove this Lobby server? This action cannot be undone.",
				"Confirm Removal",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning
			);

			if (dialogResult == DialogResult.No)
			{
				return;
			}

			if (dialogResult == DialogResult.Yes)
			{
				// Update the server in the database
				CommandResult result = await ApiCore.ApiClient!.Stats.RemoveLobbyServersAsync(_LobbySelectedID);
				// 
				if (result.Success)
				{
					MessageBox.Show("Babstats server removed successfully.", "Removal Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}
				else
				{
					MessageBox.Show("Failed to remove Babstats server. Please try again.", "Removal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// Refresh the server list
			LoadLobbySettings();
			// Reset Form
			lobbyAction_ResetForm();
			// Reset Buttons
			lobbyAction_ButtonReset();
		}

		private void lobbyAction_newRecord(object sender, EventArgs e)
		{
			// Reset Form
			lobbyAction_ResetForm();
			// Reset Buttons
			lobbyAction_ButtonReset();
		}

		private void lobbyAction_ResetForm()
		{
			_LobbySelectedID = 0;
			tb_lobbySiteUri.Text = string.Empty;
			num_lobbyGamePort.Value = theInstance!.profileBindPort;
			tb_lobbySecretKey.Text = string.Empty;
			tb_lobbySiteName.Text = string.Empty;
			cb_lobbyEnabled.Checked = false;
		}

		private void lobbyAction_ButtonReset()
		{
			btn_lobbySave.Enabled = false;
			btn_lobbyAdd.Enabled = true;
			btn_lobbyNew.Enabled = true;
			btn_lobbyRemove.Enabled = false;
			btn_lobbyNew.Text = "New";
		}

		private void lobbyAction_selectRecord(object sender, EventArgs e)
		{
            // Collect Form Details from the selected Lobby record, then populate form for editing
            if (lobby_table.SelectedRows.Count == 0)
                return;

            // Get the selected row and its associated BabstatsServerSettings
            DataGridViewRow selectedRow = lobby_table.SelectedRows[0];
            if (selectedRow.Tag is not LobbyServerSettings server)
                return;
            
            // Load the form from the table
            _LobbySelectedID = server.LobbyServerID;
            tb_lobbySiteUri.Text = server.ServerUri;
            tb_lobbySiteName.Text = server.SiteName;
            tb_lobbySecretKey.Text = server.SecretKey;
            num_lobbyGamePort.Value = Math.Max(num_lobbyGamePort.Minimum, Math.Min(num_lobbyGamePort.Maximum, server.GamePort));
            cb_lobbyEnabled.Checked = server.IsEnabled;

			// Buttons stats
			btn_lobbySave.Enabled = true;
            btn_lobbyAdd.Enabled = false;
            btn_lobbyRemove.Enabled = true;
            btn_lobbyNew.Enabled = true;
            btn_lobbyNew.Text = "Cancel";
		}
	}
}