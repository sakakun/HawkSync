using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using HawkSyncShared.DTOs.tabStats;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabStats : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        private statInstance? instanceStats => CommonCore.instanceStats;

        // --- Class Variables ---
        private bool _firstLoadComplete = false;

        public tabStats()
        {
            InitializeComponent();
            LoadWebStatsSettings();

            CommonCore.Ticker?.Start("StatsTabTicker", 1000, StatsTickerHook);
        }

        // --- Form Functions ---
        /// <summary>
        /// Ticker hook for stats tab updates
        /// </summary>
        public void StatsTickerHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                LoadWebStatsSettings();
                return;
            }

            if (instanceStats?.ForceUIUpdate == true)
            {
                instanceStats.ForceUIUpdate = false;
                LoadWebStatsSettings();
            }
        }

        /// <summary>
        /// Load web stats settings via manager
        /// </summary>
        public void LoadWebStatsSettings()
        {
            var result = theInstanceManager.LoadWebStatsSettings();

            if (!result.Success)
            {
                AppDebug.Log(Name, $"Failed to load web stats settings: {result.Message}");
                MessageBox.Show($"Failed to load web stats settings:\n\n{result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateUIFromInstance();
        }

        /// <summary>
        /// Save web stats settings via manager
        /// </summary>
        public void SaveWebStatsSettings()
        {
            var settings = BuildWebStatsSettingsFromUI();

            var result = theInstanceManager.SaveWebStatsSettings(settings);

            if (result.Success)
            {
                MessageBox.Show("Web stats settings saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(Name, "Web stats settings saved successfully");
            }
            else
            {
                MessageBox.Show($"Failed to save web stats settings:\n\n{result.Message}", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppDebug.Log(Name, $"Failed to save web stats settings: {result.Message}");
            }
        }

        // --- Helper Methods ---

        /// <summary>
        /// Update UI controls from theInstance
        /// </summary>
        private void UpdateUIFromInstance()
        {
            if (theInstance == null) return;

            tb_serverID.Text = theInstance.WebStatsProfileID;
            cb_enableWebStats.Checked = theInstance.WebStatsEnabled;
            tb_webStatsServerPath.Text = theInstance.WebStatsServerPath;
            cb_enableAnnouncements.Checked = theInstance.WebStatsAnnouncements;
            num_WebStatsReport.Value = theInstance.WebStatsReportInterval;
            num_WebStatsUpdates.Value = theInstance.WebStatsUpdateInterval;

            UpdateControlStates();
        }

        /// <summary>
        /// Build WebStatsSettings DTO from UI controls
        /// </summary>
        private WebStatsSettings BuildWebStatsSettingsFromUI()
        {
            return new WebStatsSettings(
                ProfileID: tb_serverID.Text,
                Enabled: cb_enableWebStats.Checked,
                ServerPath: tb_webStatsServerPath.Text,
                Announcements: cb_enableAnnouncements.Checked,
                ReportInterval: (int)num_WebStatsReport.Value,
                UpdateInterval: (int)num_WebStatsUpdates.Value
            );
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

        // --- Event Handlers ---

        private void OnSaveStatSettingsClick(object sender, EventArgs e)
        {
            SaveWebStatsSettings();
        }

        private void OnEnableAnnouncementsChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void OnEnableWebStatsChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private async void OnTestBabstatConnectionClick(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
                button.Enabled = false;

            try
            {
                var result = await theInstanceManager.TestWebStatsConnectionAsync(tb_webStatsServerPath.Text);

                MessageBox.Show(
                    result.Message,
                    result.Success ? "Connection Test" : "Connection Test Failed",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Error testing connection: {ex.Message}");
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
    }
}