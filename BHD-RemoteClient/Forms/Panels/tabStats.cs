using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Classes.StatManagement;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_RemoteClient.Forms.Panels
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
        private void functionEvent_HighlightDiffFields()
        {
            // Web Stats Enabled
            cb_enableWebStats.BackColor = cb_enableWebStats.Checked != theInstance.WebStatsEnabled
                ? Color.LightYellow : SystemColors.Control;

            // Web Stats Server Path
            tb_webStatsServerPath.BackColor = tb_webStatsServerPath.Text != theInstance.WebStatsServerPath
                ? Color.LightYellow : SystemColors.Window;

            // Announcements Enabled
            cb_enableAnnouncements.BackColor = cb_enableAnnouncements.Checked != theInstance.WebStatsAnnouncements
                ? Color.LightYellow : SystemColors.Control;

            // Report Interval
            num_WebStatsReport.BackColor = (int)num_WebStatsReport.Value != theInstance.WebStatsReportInterval
                ? Color.LightYellow : SystemColors.Window;

            // Update Interval
            num_WebStatsUpdates.BackColor = (int)num_WebStatsUpdates.Value != theInstance.WebStatsUpdateInterval
                ? Color.LightYellow : SystemColors.Window;

            // Min Required Players Enabled
            cb_WebStatsEnableMinReqPlayers.BackColor = cb_WebStatsEnableMinReqPlayers.Checked != theInstance.WebStatsEnableMinReqPlayers
                ? Color.LightYellow : SystemColors.Control;

            // Min Required Players Value
            num_WebStatsMinReqPlayers.BackColor = (int)num_WebStatsMinReqPlayers.Value != theInstance.WebStatsMinReqPlayers
                ? Color.LightYellow : SystemColors.Window;
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
            cb_WebStatsEnableMinReqPlayers.Checked = newInstance.WebStatsEnableMinReqPlayers;
            num_WebStatsMinReqPlayers.Value = newInstance.WebStatsMinReqPlayers;

            bool statsEnabled = newInstance.WebStatsEnabled;
            bool announcementsEnabled = statsEnabled && newInstance.WebStatsAnnouncements;

            tb_webStatsServerPath.Enabled = statsEnabled;
            cb_enableAnnouncements.Enabled = statsEnabled;
            num_WebStatsReport.Enabled = announcementsEnabled;
            num_WebStatsUpdates.Enabled = statsEnabled;
            num_WebStatsMinReqPlayers.Enabled = statsEnabled && cb_WebStatsEnableMinReqPlayers.Checked;
        }

        public void functionEvent_SaveSettings()
        {
            // Deep copy theInstance using serialization
            theInstance updateInstance = JsonSerializer.Deserialize<theInstance>(JsonSerializer.Serialize(theInstance))!;

            // Stats Settings
            updateInstance.WebStatsEnabled = cb_enableWebStats.Checked;
            updateInstance.WebStatsServerPath = tb_webStatsServerPath.Text;
            updateInstance.WebStatsAnnouncements = cb_enableAnnouncements.Checked;
            updateInstance.WebStatsReportInterval = (int)num_WebStatsReport.Value;
            updateInstance.WebStatsUpdateInterval = (int)num_WebStatsUpdates.Value;
            updateInstance.WebStatsEnableMinReqPlayers = cb_WebStatsEnableMinReqPlayers.Checked;
            updateInstance.WebStatsMinReqPlayers = (int)num_WebStatsMinReqPlayers.Value;

            // Save the settings to file
            if (CmdSetServerVariables.ProcessCommand(updateInstance))
            {
                MessageBox.Show("Server settings applied successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(this.Name, "Server settings applied.");
            }
            else
            {
                AppDebug.Log(this.Name, "Failed to apply server settings."); // Apply the new settings

            }
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

            functionEvent_HighlightDiffFields();

            // Stats Refresh Tasks
            StatFunctions.PopulatePlayerStatsGrid();
            StatFunctions.PopulateWeaponStatsGrid();

        }

        private void ActionEvent_EnableAnnouncements(object sender, EventArgs e) => num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;

        private void ActionEvent_EnableBabStats(object sender, EventArgs e)
        {
            bool enabled = cb_enableWebStats.Checked;
            tb_webStatsServerPath.Enabled = enabled;
            cb_enableAnnouncements.Enabled = enabled;
            num_WebStatsUpdates.Enabled = enabled;
            num_WebStatsReport.Enabled = cb_enableAnnouncements.Checked;
            cb_WebStatsEnableMinReqPlayers.Enabled = enabled;
            num_WebStatsMinReqPlayers.Enabled = enabled;
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

        private void ActionEvent_MinPlayersReqChange(object sender, EventArgs e)
        {
            num_WebStatsMinReqPlayers.Enabled = cb_WebStatsEnableMinReqPlayers.Checked;
        }

        private void ActionClick_ResetChanges(object sender, EventArgs e)
        {
            functionEvent_GetStatSettings(null!);
        }
    }
}
