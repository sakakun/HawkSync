using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.Storage;
using Button = System.Windows.Controls.Button;
using UserControl = System.Windows.Forms.UserControl;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabProfile : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManagerUI theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "ProfileTab";                     // Name of the tab for logging purposes.
        
        public tabProfile()
        {
            InitializeComponent();
            tickerProfileInit();
        }

        // --- Form Functions ---
        // --- Ticker Profile Hook --- Allow to be triggered externally by the Server Manager Ticker
        public void tickerProfileInit()
        {
            methodFunction_loadSettings();

            // If the profileServerPath is not set or does not exist, switch to the Profile tab and show a message box.
            if (theInstance!.profileServerPath == string.Empty || !Directory.Exists(theInstance.profileServerPath))
            {
                theServer.tabControl.SelectedTab = theServer.tabControl.TabPages[0];
                MessageBox.Show("Please set the Server Path in the Profile tab.", "Server Path Not Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                theServer.tabControl.SelectedTab = theServer.tabControl.TabPages[1];
            }

            bool currentState = (theInstance!.instanceStatus == InstanceStatus.OFFLINE);

            // Enable/disable all profile controls (except mod fields)
            btn_profileBrowse1.Enabled = currentState;
            btn_resetProfile.Enabled = currentState;
            btn_saveProfile.Enabled = currentState;

            // Enable/disable mod fields based on /mod checked and OFFLINE status
            btn_profileBrowse2.Enabled = currentState;

        }

        // --- Action Click Events ---
        // --- Save Profile Button Clicked ---
        private void actionClick_SaveProfile(object sender, EventArgs e)
        {   
            // New Save
            methodFunction_saveSetting();
        }
        // --- Reset Profile Button Clicked ---
        private void actionClick_ResetProfile(object sender, EventArgs e)
        {
            // New Reset
            methodFunction_loadSettings();
        }

        // --- Open Profile Folder Button Clicked ---
        private void actionClick_profileOpenFolderDialog(object sender, EventArgs e)
        {
            // This will open the folder dialog and allow the user to select a folder, once selected , it will place the folder path in tb.profileServerPath.
            try
            {
                // Create a new FolderBrowserDialog instance.
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    // Set the description for the dialog.
                    folderBrowserDialog.Description = "Select the Mod Profile Folder";
                    // Show the dialog and check if the user selected a folder.
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Set the text of the tb.profileServerPath to the selected folder path.
                        tb_profileServerPath.Text = folderBrowserDialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, $"Failed to open profile folder dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile folder dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // --- Open Profile File Button Clicked ---
        private void actionClick_profileOpenFileDialog(object sender, EventArgs e)
        {
            // This will open the file dialog and allow the user to select a *.pff file.  Once selected, it will place the file name (no extension) int tb.modFile.
            try
            {
                // Create a new OpenFileDialog instance.
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Set the filter for the file dialog to only show *.pff files.
                    openFileDialog.Filter = "Mod Files (*.pff)|*.pff";
                    openFileDialog.Title = "Select a Mod File";
                    // Show the dialog and check if the user selected a file.
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file name without the extension.
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        // Set the text of the tb_modFile to the selected file name.
                        tb_modFile.Text = fileNameWithoutExtension;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, $"Failed to open profile file dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile file dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
        public void methodFunction_loadSettings()
        {
            // File Path Textbox Fields
            tb_profileServerPath.Text = theInstance!.profileServerPath = ServerSettings.Get("profileServerPath", string.Empty);
            tb_modFile.Text = theInstance.profileModFileName = ServerSettings.Get("profileModFileName", string.Empty);

            // Host Information
            tb_hostName.Text = theInstance.gameHostName = ServerSettings.Get("gameHostName", string.Empty);
            tb_serverName.Text = theInstance.gameServerName = ServerSettings.Get("gameServerName", string.Empty);
            tb_serverMessage.Text = theInstance.gameMOTD = ServerSettings.Get("gameMOTD", string.Empty);

            // Server Details
            theInstance!.profileBindIP = ServerSettings.Get("profileBindIP", "0.0.0.0");
            if (!string.IsNullOrEmpty(theInstance!.profileBindIP) && cb_serverIP.Items.Contains(theInstance.profileBindIP))
            {
                cb_serverIP.SelectedItem = theInstance.profileBindIP; // Select the item in cb_serverIP that matches the value of newInstance.profileBindIP,
            }
            else
            {
                cb_serverIP.SelectedIndex = 0; // or default to the first item (assumed "0.0.0.0") if not found
            }
            num_serverPort.Value = theInstance.profileBindPort = (int) ServerSettings.Get("profileBindPort", (decimal) 17479);
            tb_serverPassword.Text = theInstance.gamePasswordLobby = ServerSettings.Get("gamePasswordLobby", string.Empty);
            cb_serverDedicated.Checked = theInstance.gameDedicated = ServerSettings.Get("gameDedicated", true);
            cb_requireNova.Checked = theInstance.gameRequireNova = ServerSettings.Get("gameRequireNova", false);
            serverCountryCode.Text = theInstance.gameCountryCode = ServerSettings.Get("gameCountryCode", "US");

			// Ping Checking
			cb_enableMinCheck.Checked = theInstance.gameMinPing = ServerSettings.Get("gameMinPing", false);
            cb_enableMaxCheck.Checked = theInstance.gameMaxPing = ServerSettings.Get("gameMaxPing", false);
            num_minPing.Value = theInstance.gameMinPingValue = (int) ServerSettings.Get("gameMinPingValue", (decimal) 0);
            num_maxPing.Value = theInstance.gameMaxPingValue = (int) ServerSettings.Get("gameMaxPingValue", (decimal) 999);

			// Application Commandline Arguments
			profileServerAttribute01.Checked = theInstance!.profileServerAttribute01 = ServerSettings.Get("profileServerAttribute01", false);
            profileServerAttribute02.Checked = theInstance!.profileServerAttribute02 = ServerSettings.Get("profileServerAttribute02", false);
            profileServerAttribute03.Checked = theInstance!.profileServerAttribute03 = ServerSettings.Get("profileServerAttribute03", false);
            profileServerAttribute04.Checked = theInstance!.profileServerAttribute04 = ServerSettings.Get("profileServerAttribute04", false);
            profileServerAttribute05.Checked = theInstance!.profileServerAttribute05 = ServerSettings.Get("profileServerAttribute05", false);
            profileServerAttribute06.Checked = theInstance!.profileServerAttribute06 = ServerSettings.Get("profileServerAttribute06", false);
            profileServerAttribute07.Checked = theInstance!.profileServerAttribute07 = ServerSettings.Get("profileServerAttribute07", true);
            profileServerAttribute08.Checked = theInstance!.profileServerAttribute08 = ServerSettings.Get("profileServerAttribute08", true);
            profileServerAttribute09.Checked = theInstance!.profileServerAttribute09 = ServerSettings.Get("profileServerAttribute09", false);
            profileServerAttribute10.Checked = theInstance!.profileServerAttribute10 = ServerSettings.Get("profileServerAttribute10", false);
            profileServerAttribute11.Checked = theInstance!.profileServerAttribute11 = ServerSettings.Get("profileServerAttribute11", false);
            profileServerAttribute12.Checked = theInstance!.profileServerAttribute12 = ServerSettings.Get("profileServerAttribute12", false);
            profileServerAttribute13.Checked = theInstance!.profileServerAttribute13 = ServerSettings.Get("profileServerAttribute13", false);
            profileServerAttribute14.Checked = theInstance!.profileServerAttribute14 = ServerSettings.Get("profileServerAttribute14", false);
            profileServerAttribute15.Checked = theInstance!.profileServerAttribute15 = ServerSettings.Get("profileServerAttribute15", false);
            profileServerAttribute16.Checked = theInstance!.profileServerAttribute16 = ServerSettings.Get("profileServerAttribute16", false);
            profileServerAttribute17.Checked = theInstance!.profileServerAttribute17 = ServerSettings.Get("profileServerAttribute17", false);
            profileServerAttribute18.Checked = theInstance!.profileServerAttribute18 = ServerSettings.Get("profileServerAttribute18", true);
            profileServerAttribute19.Checked = theInstance!.profileServerAttribute19 = ServerSettings.Get("profileServerAttribute19", false);
            profileServerAttribute20.Checked = theInstance!.profileServerAttribute20 = ServerSettings.Get("profileServerAttribute20", false);
            profileServerAttribute21.Checked = theInstance!.profileServerAttribute21 = ServerSettings.Get("profileServerAttribute21", true);

		}

        public void methodFunction_saveSetting()
        {
			// Textbox Fields
            ServerSettings.Set("profileServerPath", theInstance!.profileServerPath = tb_profileServerPath.Text);
            ServerSettings.Set("profileModFileName", theInstance.profileModFileName = tb_modFile.Text);

            // Host Information
            ServerSettings.Set("gameHostName", theInstance.gameHostName = tb_hostName.Text);
            ServerSettings.Set("gameServerName", theInstance.gameServerName = tb_serverName.Text);
            ServerSettings.Set("gameMOTD", theInstance.gameMOTD = tb_serverMessage.Text);

			// Server Details
            ServerSettings.Set("profileBindIP", theInstance!.profileBindIP = cb_serverIP.SelectedItem!.ToString()!);
            theInstance!.profileBindPort = (int) num_serverPort.Value;
            ServerSettings.Set("profileBindPort", (decimal)theInstance!.profileBindPort);
            ServerSettings.Set("gamePasswordLobby", theInstance.gamePasswordLobby = tb_serverPassword.Text);
            ServerSettings.Set("gameDedicated", theInstance.gameDedicated = cb_serverDedicated.Checked);
            ServerSettings.Set("gameRequireNova", theInstance.gameRequireNova = cb_requireNova.Checked);
            ServerSettings.Set("gameCountryCode", theInstance.gameCountryCode = serverCountryCode.Text);

			// Ping Checking
			ServerSettings.Set("gameMinPing", theInstance.gameMinPing = cb_enableMinCheck.Checked);
            ServerSettings.Set("gameMaxPing", theInstance.gameMaxPing = cb_enableMaxCheck.Checked);
            ServerSettings.Set("gameMinPingValue", (decimal)(theInstance.gameMinPingValue = (int)num_minPing.Value));
            ServerSettings.Set("gameMaxPingValue", (decimal)(theInstance.gameMaxPingValue = (int)num_maxPing.Value));

			// Commandlin Switches
			ServerSettings.Set("profileServerAttribute01", theInstance!.profileServerAttribute01 = profileServerAttribute01.Checked);
            ServerSettings.Set("profileServerAttribute02", theInstance!.profileServerAttribute02 = profileServerAttribute02.Checked);
            ServerSettings.Set("profileServerAttribute03", theInstance!.profileServerAttribute03 = profileServerAttribute03.Checked);
            ServerSettings.Set("profileServerAttribute04", theInstance!.profileServerAttribute04 = profileServerAttribute04.Checked);
            ServerSettings.Set("profileServerAttribute05", theInstance!.profileServerAttribute05 = profileServerAttribute05.Checked);
            ServerSettings.Set("profileServerAttribute06", theInstance!.profileServerAttribute06 = profileServerAttribute06.Checked);
            ServerSettings.Set("profileServerAttribute07", theInstance!.profileServerAttribute07 = profileServerAttribute07.Checked);
            ServerSettings.Set("profileServerAttribute08", theInstance!.profileServerAttribute08 = profileServerAttribute08.Checked);
            ServerSettings.Set("profileServerAttribute09", theInstance!.profileServerAttribute09 = profileServerAttribute09.Checked);
            ServerSettings.Set("profileServerAttribute10", theInstance!.profileServerAttribute10 = profileServerAttribute10.Checked);
            ServerSettings.Set("profileServerAttribute11", theInstance!.profileServerAttribute11 = profileServerAttribute11.Checked);
            ServerSettings.Set("profileServerAttribute12", theInstance!.profileServerAttribute12 = profileServerAttribute12.Checked);
            ServerSettings.Set("profileServerAttribute13", theInstance!.profileServerAttribute13 = profileServerAttribute13.Checked);
            ServerSettings.Set("profileServerAttribute14", theInstance!.profileServerAttribute14 = profileServerAttribute14.Checked);
            ServerSettings.Set("profileServerAttribute15", theInstance!.profileServerAttribute15 = profileServerAttribute15.Checked);
            ServerSettings.Set("profileServerAttribute16", theInstance!.profileServerAttribute16 = profileServerAttribute16.Checked);
            ServerSettings.Set("profileServerAttribute17", theInstance!.profileServerAttribute17 = profileServerAttribute17.Checked);
            ServerSettings.Set("profileServerAttribute18", theInstance!.profileServerAttribute18 = profileServerAttribute18.Checked);
            ServerSettings.Set("profileServerAttribute19", theInstance!.profileServerAttribute19 = profileServerAttribute19.Checked);
            ServerSettings.Set("profileServerAttribute20", theInstance!.profileServerAttribute20 = profileServerAttribute20.Checked);
            ServerSettings.Set("profileServerAttribute21", theInstance!.profileServerAttribute21 = profileServerAttribute21.Checked);

        }


	}
}
