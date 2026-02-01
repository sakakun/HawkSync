using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels
{
    public partial class tabStats : UserControl
    {

        private theInstance theInstance => CommonCore.theInstance!;
        private statInstance statInstance => CommonCore.instanceStats!;
        private playerInstance playerInstance => CommonCore.instancePlayers!;

        // Throttle updates to once every 15 seconds
        private static DateTime _lastPlayerStatsUpdate = DateTime.MinValue;
        // Throttle updates to once every 15 seconds
        private static DateTime _lastWeaponStatsUpdate = DateTime.MinValue;

        private bool _isEditingStats = false;
        private bool _suppressStatsChangeTracking = false;
        private DateTime _lastStatsEditTime = DateTime.MinValue;
        private System.Windows.Forms.Timer? _statsInactivityTimer;
        private const int STATS_INACTIVITY_TIMEOUT_SECONDS = 120;

        public tabStats()
        {
            InitializeComponent();

            // First Load
            methodFunction_loadSettings();

            // Subscribe to server updates
            ApiCore.OnSnapshotReceived += OnSnapshotReceived;

            SetupStatsInactivityTimer();
            WireUpStatsChangeTracking();

        }

        private void OnSnapshotReceived(ServerSnapshot snapshot)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnSnapshotReceived(snapshot)));
                return;
            }
            
            // Load Settings
            methodFunction_loadSettings();
            // Update the stats log when a new snapshot is received
            UpdateWebStatsLog();
            // Update player stats grid
            PopulatePlayerStatsGrid();
            // Update weapon stats grid
            PopulateWeaponStatsGrid();
        }

        private void SetupStatsInactivityTimer()
        {
            _statsInactivityTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _statsInactivityTimer.Tick += StatsInactivityTimer_Tick;
            _statsInactivityTimer.Start();
        }

        private void StatsInactivityTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isEditingStats)
                return;

            if ((DateTime.Now - _lastStatsEditTime).TotalSeconds >= STATS_INACTIVITY_TIMEOUT_SECONDS)
            {
                _isEditingStats = false;
                methodFunction_loadSettings();
                UpdateStatsEditModeIndicator();
                MessageBox.Show(
                    "Stats settings refreshed due to inactivity.\nAny unsaved changes were discarded.",
                    "Auto-Refresh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void WireUpStatsChangeTracking()
        {
            tb_serverID.TextChanged += Stats_ControlValueChanged;
            cb_enableWebStats.CheckedChanged += Stats_ControlValueChanged;
            tb_webStatsServerPath.TextChanged += Stats_ControlValueChanged;
            cb_enableAnnouncements.CheckedChanged += Stats_ControlValueChanged;
            num_WebStatsReport.ValueChanged += Stats_ControlValueChanged;
            num_WebStatsUpdates.ValueChanged += Stats_ControlValueChanged;
        }

        private void Stats_ControlValueChanged(object? sender, EventArgs e)
        {
            if (_suppressStatsChangeTracking)
                return;

            if (!_isEditingStats)
            {
                _isEditingStats = true;
                UpdateStatsEditModeIndicator();
            }
            _lastStatsEditTime = DateTime.Now;
        }


        private void UpdateStatsEditModeIndicator()
        {
            if (_isEditingStats)
            {
                int secondsRemaining = STATS_INACTIVITY_TIMEOUT_SECONDS -
                    (int)(DateTime.Now - _lastStatsEditTime).TotalSeconds;
                btn_SaveSettings.BackColor = Color.Orange;
                btn_SaveSettings.Text = $"Save";
            }
            else
            {
                btn_SaveSettings.BackColor = SystemColors.Control;
                btn_SaveSettings.Text = "Save";
            }
        }

        public void UpdateWebStatsLog()
        {
            var logEntries = statInstance.WebStatsLog ?? new List<StatReportObject>();

            // Build a set of current (date, content) pairs in the grid for quick lookup
            var currentRows = new HashSet<string>();
            foreach (DataGridViewRow row in dg_statsLog.Rows)
            {
                if (row.IsNewRow) continue;
                var dateStr = row.Cells[0].Value as string ?? "";
                var content = row.Cells[1].Value as string ?? "";
                currentRows.Add($"{dateStr}|{content}");
            }

            // Remove rows that are no longer in the log
            for (int i = dg_statsLog.Rows.Count - 1; i >= 0; i--)
            {
                var row = dg_statsLog.Rows[i];
                if (row.IsNewRow) continue;
                var dateStr = row.Cells[0].Value as string ?? "";
                var content = row.Cells[1].Value as string ?? "";
                bool exists = logEntries.Any(e =>
                    e.ReportDate.ToString("yyyy-MM-dd HH:mm:ss") == dateStr &&
                    e.ReportContent == content);
                if (!exists)
                    dg_statsLog.Rows.RemoveAt(i);
            }

            // Add new log entries that are not already present
            foreach (var entry in logEntries)
            {
                var dateStr = entry.ReportDate.ToString("yyyy-MM-dd HH:mm:ss");
                var key = $"{dateStr}|{entry.ReportContent}";
                if (!currentRows.Contains(key))
                {
                    dg_statsLog.Rows.Add(dateStr, entry.ReportContent);
                }
            }
        }

        public void methodFunction_loadSettings()
        {
            tb_serverID.Text = theInstance!.WebStatsProfileID;
            cb_enableWebStats.Checked = theInstance.WebStatsEnabled;
            tb_webStatsServerPath.Text = theInstance.WebStatsServerPath;
            cb_enableAnnouncements.Checked = theInstance.WebStatsAnnouncements;
            num_WebStatsReport.Value = theInstance.WebStatsReportInterval;
            num_WebStatsUpdates.Value = theInstance.WebStatsUpdateInterval;

            UpdateControlStates();

        }

        /// <summary>
        /// Update control enabled states based on settings
        /// </summary>
        private void UpdateControlStates()
        {
            bool statsEnabled = cb_enableWebStats.Checked;
            bool announcementsEnabled = statsEnabled && cb_enableAnnouncements.Checked;

            tb_webStatsServerPath.Enabled = statsEnabled;
            cb_enableAnnouncements.Enabled = statsEnabled;
            num_WebStatsUpdates.Enabled = statsEnabled;
            num_WebStatsReport.Enabled = announcementsEnabled;
        }

        /// <summary>
        /// Enable/disable announcements
        /// </summary>
        private void ActionEvent_EnableAnnouncements(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        /// <summary>
        /// Enable/disable web stats
        /// </summary>
        private void ActionEvent_EnableBabStats(object sender, EventArgs e)
        {
            UpdateControlStates();
        }
        private async void btnSaveWebStats_Click(object sender, EventArgs e)
        {
            var settings = new WebStatsSettings(
                ProfileID: tb_serverID.Text,
                Enabled: cb_enableWebStats.Checked,
                ServerPath: tb_webStatsServerPath.Text,
                Announcements: cb_enableAnnouncements.Checked,
                ReportInterval: (int)num_WebStatsReport.Value,
                UpdateInterval: (int)num_WebStatsUpdates.Value
            );

            try
            {
                var result = await ApiCore.ApiClient!.SaveWebStatsSettingsAsync(settings);

                if (result.Success)
                {
                    MessageBox.Show("Web stats settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Failed to save web stats settings:\n\n{result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving web stats settings:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btn_validate_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
                button.Enabled = false;

            try
            {
                var result = await ApiCore.ApiClient!.ValidateWebStatsConnectionAsync(tb_webStatsServerPath.Text);

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
                    button.Enabled = true;
            }
        }

        public void PopulatePlayerStatsGrid()
        {
            if ((DateTime.UtcNow - _lastPlayerStatsUpdate).TotalSeconds < 15)
                return;
            _lastPlayerStatsUpdate = DateTime.UtcNow;

            dataGridViewPlayerStats.Rows.Clear();

            foreach (var statObj in statInstance.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                var shotsPerKill = player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
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
                return;
            _lastWeaponStatsUpdate = DateTime.UtcNow;

            dataGridViewWeaponStats.Rows.Clear();

            foreach (var statObj in statInstance.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                foreach (var weaponStat in statObj.PlayerWeaponStats)
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

    }
}