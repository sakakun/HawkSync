using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using System.ComponentModel;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabStats : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        private statInstance? instanceStats => CommonCore.instanceStats;

        // --- Class Variables ---
        private bool _firstLoadComplete;
        private int _BabstatsSelectedID;
        private int _LobbySelectedID;

        private static bool IsDesignTime =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime || System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");
        
        public tabStats()
        {
            InitializeComponent();
            
            if (IsDesignTime)
                return;
            
            CommonCore.Ticker?.Start("StatsTabTicker", 1000, StatsTickerHook);
        }

        // --- Form Functions ---
        /// <summary>
        /// Ticker hook for stats tab updates.
        /// </summary>
        public void StatsTickerHook()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StatsTickerHook));
                return;
            }

            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                // Babstats Tab
                babstatsFormReset();
                babstatsButtonsReset();
                LoadWebStatsSettings();
                
				// Lobby Tab
                lobbyAction_ResetForm();
                lobbyAction_ButtonReset();
				LoadLobbySettings();
                return;
            }

            if (instanceStats?.ForceUIUpdate == true)
            {
                instanceStats.ForceUIUpdate = false;
                LoadWebStatsSettings();
                LoadLobbySettings();
                LoadLogs();
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

        /// <summary>
        /// Load web stats settings and Babstats server list via manager.
        /// </summary>
        public void LoadWebStatsSettings()
        {
            // Get the Babstats server settings from the database and populate the UI
            List<BabstatsServerSettings> BabstatsServers = DatabaseManager.GetBabstatsServers();

            instanceStats!.BabstatsServers = BabstatsServers;

            // Clear existing items
            babstats_table.Rows.Clear();
            // Add each server to the table
            foreach (var server in BabstatsServers)
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

        /// <summary>
        /// Load web stats settings and Babstats server list via manager.
        /// </summary>
        public void LoadLobbySettings()
        {
            // Get the Babstats server settings from the database and populate the UI
            List<LobbyServerSettings> LobbyServers = DatabaseManager.GetLobbyServers();

            instanceStats!.LobbyServers = LobbyServers;

            // Clear existing items
            lobby_table.Rows.Clear();
            // Add each server to the table
            foreach (var server in LobbyServers)
            {
                int rowIndex = lobby_table.Rows.Add(
                    server.LobbyServerID,                // lobby_id (hidden)
                    server.SiteName,                     // lobby_sitename ("Site Name")
                    server.ServerUri,                    // lobby_siteuri ("Lobby URL")
                    server.GamePort,                     // lobby_gameport ("Game Port")
                    server.SecretKey,                    // lobby_secretkey ("Secret Key")
                    server.IsEnabled                     // lobby_enabled (bool)
				);
                DataGridViewRow row = lobby_table.Rows[rowIndex];
                row.Tag = server; // Store the server settings in the row's Tag for later reference
            }
        }

        public void LoadLogs()
        {
            dg_statsLog.Rows.Clear();

            var theLogs = instanceStats!.WebStatsLog;

            foreach (var log in theLogs)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.Cells[0].Value = log.ReportDate;
                newRow.Cells[1].Value = log.ReportContent;

                dg_statsLog.Rows.Add(newRow);
            }
            
            dg_statsLog.Sort(dg_statsLog.Columns[0], ListSortDirection.Descending);
        }
        
        // --- Event Handlers ---
        private async void OnTestBabstatConnectionClick(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                button.Enabled = false;
            }

            try
            {
                OperationResult result = await theInstanceManager.TestWebStatsConnectionAsync(tb_webStatsServerPath.Text);

                MessageBox.Show(
                    result.Message,
                    result.Success ? "Connection Test" : "Connection Test Failed",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error testing connection", AppDebug.LogLevel.Error, ex);
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

        private void babstatsClick_addServer(object sender, EventArgs e)
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
                    DatabaseManager.DisableAllBabstatsAnnouncements();
                }
            }

            // Add the new server to the database
            DatabaseManager.AddBabstatsServer(newServer);

            // Refresh the server list
            LoadWebStatsSettings();

            // Reset Form
            babstatsFormReset();
            // Reset Buttons
            babstatsButtonsReset();
        }

        private void babstatsClick_saveServer(object sender, EventArgs e)
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
                    DatabaseManager.DisableAllBabstatsAnnouncements();
                }
            }
            // Update the server in the database
            DatabaseManager.UpdateBabstatsServer(updatedServer);

            // Refresh the server list
            LoadWebStatsSettings();
            // Reset Form
            babstatsFormReset();
            // Reset Buttons
            babstatsButtonsReset();

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

        private void babstatsClick_RemoveRecord(object sender, EventArgs e)
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
                if (DatabaseManager.RemoveBabstatsServer(_BabstatsSelectedID))
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

        private void lobbyAction_saveRecord(object sender, EventArgs e)
        {
            // Collect Form Details for new lobby to be added, then add to database and refresh list
            LobbyServerSettings updatedServer = new LobbyServerSettings(
                _LobbySelectedID, // LobbyServerID, will be set by the database
                tb_lobbySiteName.Text.Trim(), // SiteName
                tb_lobbySiteUri.Text.Trim(), // ServerUri
                (int)num_lobbyGamePort.Value, // GamePort
                tb_lobbySecretKey.Text.Trim(), // SecretKey
                cb_lobbyEnabled.Checked, // IsEnabled
				0 // SortOrder
			);

            if (!DatabaseManager.UpdateLobbyServer(updatedServer))
            {
                MessageBox.Show("Failed to update lobby server. Please try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            MessageBox.Show("Lobby server updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// Refresh the server list
            LoadLobbySettings();
			// Reset Form
			lobbyAction_ResetForm();
            // Reset Buttons
            lobbyAction_ButtonReset();

        }

        private void lobbyAction_addRecord(object sender, EventArgs e)
        {
            // Collect Form Details for new lobby to be added, then add to database and refresh list
            LobbyServerSettings NewServer = new LobbyServerSettings(
                0, // LobbyServerID, will be set by the database
                tb_lobbySiteName.Text.Trim(), // SiteName
                tb_lobbySiteUri.Text.Trim(), // ServerUri
                (int)num_lobbyGamePort.Value, // GamePort
                tb_lobbySecretKey.Text.Trim(), // SecretKey
                cb_lobbyEnabled.Checked, // IsEnabled
                0
            );

			// Add the new server to the database
            if (!DatabaseManager.AddLobbyServer(NewServer))
            {
                MessageBox.Show("Failed to add lobby server. Please try again.", "Addition Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

            MessageBox.Show("Lobby server added successfully.", "Addition Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// Refresh the server list
            LoadLobbySettings();
			// Reset Form
			lobbyAction_ResetForm();
            // Reset Buttons
            lobbyAction_ButtonReset();
        }

        private void lobbyAction_removeRecord(object sender, EventArgs e)
        {
            // Collect Form Details for new lobby to be added, then add to database and refresh list
            if(!DatabaseManager.RemoveLobbyServer(_LobbySelectedID))
            {
                MessageBox.Show("Failed to remove lobby server. Please try again.", "Removal Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lobby server removed successfully.", "Removal Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

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