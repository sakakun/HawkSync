using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.StatsManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabStats : UserControl
    {

        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "StatsTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        public tabStats()
        {
            InitializeComponent();
        }

        public void functionEvent_GetStatSettings(theInstance updatedInstance)
        {
            var newInstance = updatedInstance != null ? updatedInstance : theInstance;

            // Stats Settings
            cb_enableWebStats.Checked = newInstance.WebStatsEnabled;
            tb_webStatsServerPath.Text = newInstance.WebStatsServerPath;
            cb_enableAnnouncements.Checked = newInstance.WebStatsAnnouncements;
            num_WebStatsReport.Value = newInstance.WebStatsReportInterval;
            num_WebStatsUpdates.Value = newInstance.WebStatsUpdateInterval;

            bool statsEnabled = newInstance.WebStatsEnabled;
            bool announcementsEnabled = statsEnabled && newInstance.WebStatsAnnouncements;

            tb_webStatsServerPath.Enabled = statsEnabled;
            cb_enableAnnouncements.Enabled = statsEnabled;
            num_WebStatsReport.Enabled = announcementsEnabled;
            num_WebStatsUpdates.Enabled = statsEnabled;
        }

        public void functionEvent_SaveSettings()
        {
            // Stats Settings
            theInstance.WebStatsEnabled = cb_enableWebStats.Checked;
            theInstance.WebStatsServerPath = tb_webStatsServerPath.Text;
            theInstance.WebStatsAnnouncements = cb_enableAnnouncements.Checked;
            theInstance.WebStatsReportInterval = (int)num_WebStatsReport.Value;
            theInstance.WebStatsUpdateInterval = (int)num_WebStatsUpdates.Value;

            // Save the settings to file
            theInstanceManager.SaveSettings();
        }


        public void StatsTickerHook()
        {
            // Ensure the first load is complete before proceeding with updates.
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                functionEvent_GetStatSettings(null!);
                return;
            }

        }

        private void ActionEvent_EnableAnnouncements(object sender, EventArgs e) => num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;

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

        private void actionClick_SaveStatSettings(object sender, EventArgs e)
        {
            functionEvent_SaveSettings();
        }
    }
}
