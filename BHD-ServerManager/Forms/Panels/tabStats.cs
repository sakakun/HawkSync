using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BHD_ServerManager.Classes.InstanceManagers.theInstanceManager;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabStats : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        
        // --- Class Variables ---
        private new string Name = "StatsTab";
        private bool _firstLoadComplete = false;

        public tabStats()
        {
            InitializeComponent();
        }

        // --- Form Functions ---

        /// <summary>
        /// Load web stats settings via manager
        /// </summary>
        public void methodFunction_loadSettings()
        {
            // Load via manager
            var result = theInstanceManager.LoadWebStatsSettings();

            if (!result.Success)
            {
                AppDebug.Log(Name, $"Failed to load web stats settings: {result.Message}");
                MessageBox.Show($"Failed to load web stats settings:\n\n{result.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update UI from instance (manager already updated instance)
            UpdateUIFromInstance();
        }

        /// <summary>
        /// Save web stats settings via manager
        /// </summary>
        public void methodFunction_saveSettings()
        {
            // Build settings from UI
            var settings = BuildWebStatsSettingsFromUI();

            // Save via manager (includes validation)
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
            tb_serverID.Text = theInstance!.WebStatsProfileID;
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

        /// <summary>
        /// Ticker hook for stats tab updates
        /// </summary>
        public void StatsTickerHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                methodFunction_loadSettings();
                return;
            }

            // Additional ticker logic can go here if needed
        }

        // --- Legacy Method (for backwards compatibility) ---
        
        /// <summary>
        /// Legacy method - calls new load method
        /// </summary>
        [Obsolete("Use methodFunction_loadSettings() instead")]
        public void functionEvent_GetStatSettings(theInstance updatedInstance)
        {
            methodFunction_loadSettings();
        }

        // --- Action Click Events ---

        /// <summary>
        /// Save stats settings button clicked
        /// </summary>
        private void actionClick_SaveStatSettings(object sender, EventArgs e)
        {
            methodFunction_saveSettings();
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

        /// <summary>
        /// Test connection to Babstats server (async)
        /// </summary>
        private async void ActionEvent_TestBabstatConnection(object sender, EventArgs e)
        {
            // Disable button during test
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
                // Re-enable button
                if (button != null)
                    button.Enabled = true;
            }
        }
    }
}