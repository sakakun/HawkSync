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
        private new string Name = "ProfileTab";
        
        public tabProfile()
        {
            InitializeComponent();
            tickerProfileInit();
        }

        // --- Form Functions ---
        
        /// <summary>
        /// Initialize profile tab on load
        /// </summary>
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

            // Enable/disable profile controls based on server status
            btn_profileBrowse1.Enabled = currentState;
            btn_profileBrowse2.Enabled = currentState;
            btn_resetProfile.Enabled = currentState;
            btn_saveProfile.Enabled = currentState;
        }

        /// <summary>
        /// Load profile settings via manager
        /// </summary>
        public void methodFunction_loadSettings()
        {
            // Load via manager
            var result = theInstanceManager.LoadProfileSettings();
            
            if (!result.Success)
            {
                AppDebug.Log(Name, $"Failed to load profile settings: {result.Message}");
                MessageBox.Show($"Failed to load profile settings: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update UI from instance (manager already updated instance)
            UpdateUIFromInstance();
        }

        /// <summary>
        /// Save profile settings via manager
        /// </summary>
        public void methodFunction_saveSettings()
        {
            // Build settings from UI
            var settings = BuildProfileSettingsFromUI();

            // Save via manager (includes validation)
            var result = theInstanceManager.SaveProfileSettings(settings);

            if (result.Success)
            {
                MessageBox.Show("Profile settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(Name, "Profile settings saved successfully");
            }
            else
            {
                MessageBox.Show($"Failed to save profile settings:\n\n{result.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppDebug.Log(Name, $"Failed to save profile settings: {result.Message}");
            }
        }

        /// <summary>
        /// Export profile settings to JSON
        /// </summary>
        public void methodFunction_exportSettings()
        {
            try
            {
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = $"ProfileSettings_{DateTime.Now:yyyyMMdd_HHmmss}.json"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var result = theInstanceManager.ExportProfileSettings(saveFileDialog.FileName);

                    if (result.Success)
                    {
                        MessageBox.Show($"Profile settings exported successfully to:\n{saveFileDialog.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to export profile settings:\n\n{result.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Error exporting settings: {ex.Message}");
                MessageBox.Show($"Failed to export settings: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Import profile settings from JSON
        /// </summary>
        public void methodFunction_importSettings()
        {
            try
            {
                using OpenFileDialog openFileDialog = new()
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var (success, settings, errorMessage) = theInstanceManager.ImportProfileSettings(openFileDialog.FileName);

                    if (!success || settings == null)
                    {
                        MessageBox.Show($"Failed to import profile settings:\n\n{errorMessage}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update UI from imported settings
                    ApplyProfileSettingsToUI(settings);

                    MessageBox.Show("Profile settings imported successfully!\n\nClick 'Save Profile' to apply these settings.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppDebug.Log(Name, $"Settings imported from: {openFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Error importing settings: {ex.Message}");
                MessageBox.Show($"Failed to import settings: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Helper Methods ---

        /// <summary>
        /// Update UI controls from theInstance
        /// </summary>
        private void UpdateUIFromInstance()
        {
            // File Path Textbox Fields
            tb_profileServerPath.Text = theInstance!.profileServerPath;
            tb_modFile.Text = theInstance.profileModFileName;

            // Host Information
            tb_hostName.Text = theInstance.gameHostName;
            tb_serverName.Text = theInstance.gameServerName;
            tb_serverMessage.Text = theInstance.gameMOTD;

            // Server Details
            if (!string.IsNullOrEmpty(theInstance.profileBindIP) && cb_serverIP.Items.Contains(theInstance.profileBindIP))
            {
                cb_serverIP.SelectedItem = theInstance.profileBindIP;
            }
            else
            {
                cb_serverIP.SelectedIndex = 0; // Default to "0.0.0.0"
            }
            
            num_serverPort.Value = theInstance.profileBindPort;
            tb_serverPassword.Text = theInstance.gamePasswordLobby;
            cb_serverDedicated.Checked = theInstance.gameDedicated;
            cb_requireNova.Checked = theInstance.gameRequireNova;
            serverCountryCode.Text = theInstance.gameCountryCode;

            // Ping Checking
            cb_enableMinCheck.Checked = theInstance.gameMinPing;
            cb_enableMaxCheck.Checked = theInstance.gameMaxPing;
            num_minPing.Value = theInstance.gameMinPingValue;
            num_maxPing.Value = theInstance.gameMaxPingValue;

            // Application Commandline Arguments
            profileServerAttribute01.Checked = theInstance.profileServerAttribute01;
            profileServerAttribute02.Checked = theInstance.profileServerAttribute02;
            profileServerAttribute03.Checked = theInstance.profileServerAttribute03;
            profileServerAttribute04.Checked = theInstance.profileServerAttribute04;
            profileServerAttribute05.Checked = theInstance.profileServerAttribute05;
            profileServerAttribute06.Checked = theInstance.profileServerAttribute06;
            profileServerAttribute07.Checked = theInstance.profileServerAttribute07;
            profileServerAttribute08.Checked = theInstance.profileServerAttribute08;
            profileServerAttribute09.Checked = theInstance.profileServerAttribute09;
            profileServerAttribute10.Checked = theInstance.profileServerAttribute10;
            profileServerAttribute11.Checked = theInstance.profileServerAttribute11;
            profileServerAttribute12.Checked = theInstance.profileServerAttribute12;
            profileServerAttribute13.Checked = theInstance.profileServerAttribute13;
            profileServerAttribute14.Checked = theInstance.profileServerAttribute14;
            profileServerAttribute15.Checked = theInstance.profileServerAttribute15;
            profileServerAttribute16.Checked = theInstance.profileServerAttribute16;
            profileServerAttribute17.Checked = theInstance.profileServerAttribute17;
            profileServerAttribute18.Checked = theInstance.profileServerAttribute18;
            profileServerAttribute19.Checked = theInstance.profileServerAttribute19;
            profileServerAttribute20.Checked = theInstance.profileServerAttribute20;
            profileServerAttribute21.Checked = theInstance.profileServerAttribute21;
        }

        /// <summary>
        /// Build ProfileSettings DTO from UI controls
        /// </summary>
        private ProfileSettings BuildProfileSettingsFromUI()
        {
            var flags = new CommandLineFlags(
                Flag01: profileServerAttribute01.Checked,
                Flag02: profileServerAttribute02.Checked,
                Flag03: profileServerAttribute03.Checked,
                Flag04: profileServerAttribute04.Checked,
                Flag05: profileServerAttribute05.Checked,
                Flag06: profileServerAttribute06.Checked,
                Flag07: profileServerAttribute07.Checked,
                Flag08: profileServerAttribute08.Checked,
                Flag09: profileServerAttribute09.Checked,
                Flag10: profileServerAttribute10.Checked,
                Flag11: profileServerAttribute11.Checked,
                Flag12: profileServerAttribute12.Checked,
                Flag13: profileServerAttribute13.Checked,
                Flag14: profileServerAttribute14.Checked,
                Flag15: profileServerAttribute15.Checked,
                Flag16: profileServerAttribute16.Checked,
                Flag17: profileServerAttribute17.Checked,
                Flag18: profileServerAttribute18.Checked,
                Flag19: profileServerAttribute19.Checked,
                Flag20: profileServerAttribute20.Checked,
                Flag21: profileServerAttribute21.Checked
            );

            return new ProfileSettings(
                ServerPath: tb_profileServerPath.Text,
                ModFileName: tb_modFile.Text,
                HostName: tb_hostName.Text,
                ServerName: tb_serverName.Text,
                MOTD: tb_serverMessage.Text,
                BindIP: cb_serverIP.SelectedItem?.ToString() ?? "0.0.0.0",
                BindPort: (int)num_serverPort.Value,
                LobbyPassword: tb_serverPassword.Text,
                Dedicated: cb_serverDedicated.Checked,
                RequireNova: cb_requireNova.Checked,
                CountryCode: serverCountryCode.Text,
                MinPingEnabled: cb_enableMinCheck.Checked,
                MaxPingEnabled: cb_enableMaxCheck.Checked,
                MinPingValue: (int)num_minPing.Value,
                MaxPingValue: (int)num_maxPing.Value,
                Attributes: flags
            );
        }

        /// <summary>
        /// Apply ProfileSettings DTO to UI controls
        /// </summary>
        private void ApplyProfileSettingsToUI(ProfileSettings settings)
        {
            // File Path Textbox Fields
            tb_profileServerPath.Text = settings.ServerPath;
            tb_modFile.Text = settings.ModFileName;

            // Host Information
            tb_hostName.Text = settings.HostName;
            tb_serverName.Text = settings.ServerName;
            tb_serverMessage.Text = settings.MOTD;

            // Server Details
            if (!string.IsNullOrEmpty(settings.BindIP) && cb_serverIP.Items.Contains(settings.BindIP))
            {
                cb_serverIP.SelectedItem = settings.BindIP;
            }
            else
            {
                cb_serverIP.SelectedIndex = 0;
            }
            
            num_serverPort.Value = settings.BindPort;
            tb_serverPassword.Text = settings.LobbyPassword;
            cb_serverDedicated.Checked = settings.Dedicated;
            cb_requireNova.Checked = settings.RequireNova;
            serverCountryCode.Text = settings.CountryCode;

            // Ping Checking
            cb_enableMinCheck.Checked = settings.MinPingEnabled;
            cb_enableMaxCheck.Checked = settings.MaxPingEnabled;
            num_minPing.Value = settings.MinPingValue;
            num_maxPing.Value = settings.MaxPingValue;

            // Application Commandline Arguments
            profileServerAttribute01.Checked = settings.Attributes.Flag01;
            profileServerAttribute02.Checked = settings.Attributes.Flag02;
            profileServerAttribute03.Checked = settings.Attributes.Flag03;
            profileServerAttribute04.Checked = settings.Attributes.Flag04;
            profileServerAttribute05.Checked = settings.Attributes.Flag05;
            profileServerAttribute06.Checked = settings.Attributes.Flag06;
            profileServerAttribute07.Checked = settings.Attributes.Flag07;
            profileServerAttribute08.Checked = settings.Attributes.Flag08;
            profileServerAttribute09.Checked = settings.Attributes.Flag09;
            profileServerAttribute10.Checked = settings.Attributes.Flag10;
            profileServerAttribute11.Checked = settings.Attributes.Flag11;
            profileServerAttribute12.Checked = settings.Attributes.Flag12;
            profileServerAttribute13.Checked = settings.Attributes.Flag13;
            profileServerAttribute14.Checked = settings.Attributes.Flag14;
            profileServerAttribute15.Checked = settings.Attributes.Flag15;
            profileServerAttribute16.Checked = settings.Attributes.Flag16;
            profileServerAttribute17.Checked = settings.Attributes.Flag17;
            profileServerAttribute18.Checked = settings.Attributes.Flag18;
            profileServerAttribute19.Checked = settings.Attributes.Flag19;
            profileServerAttribute20.Checked = settings.Attributes.Flag20;
            profileServerAttribute21.Checked = settings.Attributes.Flag21;
        }

        // --- Action Click Events ---

        /// <summary>
        /// Save Profile Button Clicked
        /// </summary>
        private void actionClick_SaveProfile(object sender, EventArgs e)
        {
            methodFunction_saveSettings();
        }

        /// <summary>
        /// Reset Profile Button Clicked
        /// </summary>
        private void actionClick_ResetProfile(object sender, EventArgs e)
        {
            methodFunction_loadSettings();
        }

        /// <summary>
        /// Export Profile Settings Button Clicked
        /// </summary>
        private void actionClick_ExportProfile(object sender, EventArgs e)
        {
            methodFunction_exportSettings();
        }

        /// <summary>
        /// Import Profile Settings Button Clicked
        /// </summary>
        private void actionClick_ImportProfile(object sender, EventArgs e)
        {
            methodFunction_importSettings();
        }

        /// <summary>
        /// Open Profile Folder Button Clicked
        /// </summary>
        private void actionClick_profileOpenFolderDialog(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Select the Server Path Folder";
                    
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        tb_profileServerPath.Text = folderBrowserDialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Failed to open profile folder dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile folder dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Open Profile File Button Clicked
        /// </summary>
        private void actionClick_profileOpenFileDialog(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Mod Files (*.pff)|*.pff";
                    openFileDialog.Title = "Select a Mod File";
                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        tb_modFile.Text = fileNameWithoutExtension;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Failed to open profile file dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile file dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}