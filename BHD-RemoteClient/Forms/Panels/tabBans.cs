using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_RemoteClient.Forms.Panels
{
    public partial class tabBans : UserControl
    {

        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private banInstance instanceBans => CommonCore.instanceBans;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "BanTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        public tabBans()
        {
            InitializeComponent();
        }

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


        public void BanTickerHook()
        {
            // Ensure the first load is complete before proceeding with updates.
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                // Set the initial state of the UI components.
                cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1;
                return;
            }
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
        private void actionClick_importBans(object sender, EventArgs e)
        {
            // Ask if the user wants to clear existing bans
            // If yes, clear the bans first
            // If no, just import the new bans
            var result = MessageBox.Show(
                "Do you want to clear existing bans before importing new ones?",
                "Clear Existing Bans",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                instanceBans.BannedPlayerAddresses.Clear();
                instanceBans.BannedPlayerNames.Clear();
                banInstanceManager.UpdateBannedTables();
                banInstanceManager.ImportSettings();
            }
            else if (result == DialogResult.No)
            {
                banInstanceManager.ImportSettings();
            }
            else
            {
                // User cancelled the import
                return;
            }
        }

        private void actionClick_exportBanSettings(object sender, EventArgs e)
        {
            banInstanceManager.ExportSettings();
        }
    }
}