using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Forms.Panels;
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

        // Server Manager Tabs
        public tabProfile ProfileTab = null!;                   // The Profile Tab User Control
        public tabServer  ServerTab  = null!;                   // The Server Tab User Control
        public tabMaps    MapsTab    = null!;                   // The Maps Tab User Control
        public tabPlayers PlayersTab = null!;                   // The Players Tab User Control
        public tabChat    ChatTab    = null!;                   // The Chat Tab User Control

        public ServerManager()
        {
            InitializeComponent();
            Load += PostServerManagerInitalization;
        }

        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            functionEvent_loadPanels();                                         // Load the User Control Tabs

            theInstanceManager.CheckSettings();
            banInstanceManager.LoadSettings();
            chatInstanceManagers.LoadSettings();
            adminInstanceManager.LoadSettings();

            theInstanceManager.InitializeTickers();
            theInstanceManager.GetServerVariables();

            cb_banSubMask.SelectedIndex = cb_banSubMask.Items.Count - 1;
            adminInstanceManager.UpdateAdminLogDialog();
            ActionClick_AdminNewUser(null!, null!);
        }

        private void functionEvent_loadPanels()
        {
            // Load the User Controls into the TabPages
            tabProfile.Controls.Add(ProfileTab = new tabProfile());
            tabServer.Controls.Add(ServerTab = new tabServer());
            tabMaps.Controls.Add(MapsTab = new tabMaps());
            tabPlayers.Controls.Add(PlayersTab = new tabPlayers());
            tabChat.Controls.Add(ChatTab = new tabChat());
        }

        // --- UI Thread Helper ---
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
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