using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Tickers;
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

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabBans : UserControl
    {

        // --- Instance Objects ---
        private banInstance? instanceBans => CommonCore.instanceBans;
        private theInstance? theInstance => CommonCore.theInstance;
        private bool _initialized = false;
        private int  _blacklistSelectedRecordIDName = -1;
        private int  _blacklistSelectedRecordIDIP   = -1;

		public tabBans()
        {
            InitializeComponent();
            tabBans_InitBlacklistTab();          // Initialize Blacklist Tab 
        }

        private void tabBans_InitBlacklistTab()
        {
            // The Form
            blacklistForm.Visible = false;
            // Player Name
            blacklist_PlayerName.Visible = false;
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddress.Visible = false;
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.SelectedText = "32";
            // Ban Dates
            blacklist_DateEnd.Enabled = false;
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;
            // Control Buttons
            blacklist_btnDelete.Visible = false;
            blacklist_btnSave.Visible = false;
            blacklist_btnReset.Visible = false;
        }

        private void blacklist_newRecord1(object sender, EventArgs e)
        {
            InitializeBlacklistForm(showPlayerName: true, showIPAddress: false);
        }

        private void blacklist_newRecord2(object sender, EventArgs e)
        {
            InitializeBlacklistForm(showPlayerName: false, showIPAddress: true);
        }

        private void blacklist_newRecord3(object sender, EventArgs e)
        {
            InitializeBlacklistForm(showPlayerName: true, showIPAddress: true);
        }

        private void InitializeBlacklistForm(bool showPlayerName, bool showIPAddress)
        {
            // Show the Form & Reset Fields
            // The Form
            blacklistForm.Visible = true;
            // Player Name
            blacklist_PlayerName.Visible = showPlayerName;
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddress.Visible = showIPAddress;
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.Text = "32";
            // Ban Dates
            blacklist_DateStart.ResetText();
            blacklist_DateEnd.Enabled = false;
            blacklist_DateEnd.ResetText();
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;
            // Control Buttons
            blacklist_btnDelete.Visible = false;
            blacklist_btnSave.Visible = true;
            blacklist_btnReset.Visible = true;
        }

        private void blacklist_ResetForm(object sender, EventArgs e)
        {
            // Player Name
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.SelectedText = "32";
            // Ban Dates
            blacklist_DateStart.ResetText();
            blacklist_DateEnd.Enabled = false;
            blacklist_DateEnd.ResetText();
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;

        }

        private void blacklist_SaveForm(object sender, EventArgs e)
        {

        }
    }
}