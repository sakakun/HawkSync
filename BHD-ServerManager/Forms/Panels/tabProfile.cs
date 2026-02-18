using HawkSyncShared;
using HawkSyncShared.DTOs.Audit;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
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
        
        // --- Audit Log Fields ---
        private System.Windows.Forms.Timer? _auditFilterTimer;
        private string _currentUserFilter = string.Empty;
        private string _currentCategoryFilter = "All";
        
        public tabProfile()
        {
            // Initialize the form components
            InitializeComponent();
            
            // Initialize audit log UI
            InitializeAuditLogUI();
            
            // Initialize the profile tab with current settings
            Profile_LoadSettings();

            // Start a ticker to update the profile tab every 5 seconds (5000 milliseconds)
            CommonCore.Ticker?.Start("ProfileTabTicker", 5000, Ticker_ProfileTab);
        }

        // --- Form Functions ---

        /// <summary>
        /// Initialize profile tab on load
        /// </summary>
        public void Ticker_ProfileTab()
        {
            // Enable/disable profile controls based on server status
            bool currentState = (theInstance!.instanceStatus == InstanceStatus.OFFLINE);
            btn_profileBrowse1.Enabled = currentState;
            btn_profileBrowse2.Enabled = currentState;
        }

        /// <summary>
        /// Load profile settings via manager
        /// </summary>
        public void Profile_LoadSettings()
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
        public void Profile_SaveSettings()
        {
            // Build settings from UI
            var settings = BuildProfileSettingsFromUI();

            // Save via manager (includes validation)
            var result = theInstanceManager.SaveProfileSettings(settings);

            if (result.Success)
            {
                // *** RELOAD settings so ticker picks up changes ***
                theInstanceManager.LoadProfileSettings();
        
                MessageBox.Show(
                    "Profile settings saved successfully.\n\n", 
                    "Success", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
        
                AppDebug.Log(Name, "Profile settings saved successfully");
            }
            else
            {
                MessageBox.Show(
                    $"Failed to save profile settings:\n\n{result.Message}", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
        
                AppDebug.Log(Name, $"Failed to save profile settings: {result.Message}");
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

            // Remote Connection Settings
            checkBox_enableRemote.Checked = theInstance.profileEnableRemote;
            num_remotePort.Value = theInstance.profileRemotePort;

            // Ping Checking
            cb_enableMinCheck.Checked = theInstance.gameMinPing;
            cb_enableMaxCheck.Checked = theInstance.gameMaxPing;
            num_minPing.Value = theInstance.gameMinPingValue;
            num_maxPing.Value = theInstance.gameMaxPingValue;

            // Application Commandline Arguments (refactored)
            var profileAttributes = new[]
            {
                profileServerAttribute01, profileServerAttribute02, profileServerAttribute03, profileServerAttribute04, profileServerAttribute05,
                profileServerAttribute06, profileServerAttribute07, profileServerAttribute08, profileServerAttribute09, profileServerAttribute10,
                profileServerAttribute11, profileServerAttribute12, profileServerAttribute13, profileServerAttribute14, profileServerAttribute15,
                profileServerAttribute16, profileServerAttribute17, profileServerAttribute18, profileServerAttribute19, profileServerAttribute20,
                profileServerAttribute21
            };

            var instanceAttributes = new[]
            {
                theInstance.profileServerAttribute01, theInstance.profileServerAttribute02, theInstance.profileServerAttribute03, theInstance.profileServerAttribute04, theInstance.profileServerAttribute05,
                theInstance.profileServerAttribute06, theInstance.profileServerAttribute07, theInstance.profileServerAttribute08, theInstance.profileServerAttribute09, theInstance.profileServerAttribute10,
                theInstance.profileServerAttribute11, theInstance.profileServerAttribute12, theInstance.profileServerAttribute13, theInstance.profileServerAttribute14, theInstance.profileServerAttribute15,
                theInstance.profileServerAttribute16, theInstance.profileServerAttribute17, theInstance.profileServerAttribute18, theInstance.profileServerAttribute19, theInstance.profileServerAttribute20,
                theInstance.profileServerAttribute21
            };

            for (int i = 0; i < profileAttributes.Length; i++)
            {
                profileAttributes[i].Checked = instanceAttributes[i];
            }
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
                EnableRemote: checkBox_enableRemote.Checked,
                RemotePort: (int)num_remotePort.Value,
                Attributes: flags
            );
        }

        /// <summary>
        /// Save Profile Button Clicked
        /// </summary>
        private void Profile_ClickSaveSettings(object sender, EventArgs e)
        {
            Profile_SaveSettings();
        }
        /// <summary>
        /// Reset Profile Button Clicked
        /// </summary>
        private void Profile_ClickResetSettings(object sender, EventArgs e)
        {
            Profile_LoadSettings();
        }

        /// <summary>
        /// Open Profile Folder Button Clicked
        /// </summary>
        private void Profile_ClickOpenFolderDialog(object sender, EventArgs e)
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
        private void Profile_ClickOpenFileDialog(object sender, EventArgs e)
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

        #region Audit Logs

        /// <summary>
        /// Initialize audit log UI components
        /// </summary>
        private void InitializeAuditLogUI()
        {
            AppDebug.Log("tabProfile", "Initializing audit log UI...");
            
            // Initialize filter timer for debouncing user filter
            _auditFilterTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // 500ms debounce
            };
            _auditFilterTimer.Tick += (s, e) =>
            {
                _auditFilterTimer.Stop();
                LoadAuditLogs();
            };

            // Set default category filter
            if (cbAuditCategoryFilter.Items.Count > 0 && cbAuditCategoryFilter.SelectedIndex < 0)
            {
                cbAuditCategoryFilter.SelectedIndex = 0; // Select "All"
                AppDebug.Log("tabProfile", $"Set default category filter to: {cbAuditCategoryFilter.SelectedItem}");
            }

            // Load initial data
            AppDebug.Log("tabProfile", "Loading initial audit logs...");
            LoadAuditLogs();
        }

        /// <summary>
        /// Load audit logs from database with current filters
        /// </summary>
        private void LoadAuditLogs()
        {
            try
            {
                AppDebug.Log("tabProfile", "LoadAuditLogs called");
                
                if (!DatabaseManager.IsInitialized)
                {
                    lblAuditStatus.Text = "Database not initialized";
                    AppDebug.Log("tabProfile", "Database not initialized");
                    return;
                }

                // Get logs from last 24 hours
                var startDate = DateTime.Now.AddHours(-24);
                var endDate = DateTime.Now;

                // Apply filters
                string? categoryFilter = _currentCategoryFilter == "All" ? null : _currentCategoryFilter;
                string? userFilter = string.IsNullOrWhiteSpace(_currentUserFilter) ? null : _currentUserFilter;

                AppDebug.Log("tabProfile", $"Fetching audit logs - Category: {_currentCategoryFilter}, User: {_currentUserFilter ?? "All"}");

                var (logs, totalCount) = DatabaseManager.GetAuditLogs(
                    startDate: startDate,
                    endDate: endDate,
                    usernameFilter: userFilter,
                    categoryFilter: categoryFilter,
                    limit: 500
                );

                AppDebug.Log("tabProfile", $"Retrieved {logs.Count} logs out of {totalCount} total");

                // Update grid
                dgvAuditLogs.Rows.Clear();

                foreach (var log in logs)
                {
                    var rowIndex = dgvAuditLogs.Rows.Add(
                        log.Timestamp.ToString("HH:mm:ss"),
                        log.Username,
                        log.ActionCategory,
                        log.ActionType,
                        log.ActionDescription,
                        log.Success ? "✓" : "✗"
                    );

                    var row = dgvAuditLogs.Rows[rowIndex];

                    // Store full log object in Tag for detail view
                    row.Tag = log;

                    // Color code failed actions
                    if (!log.Success)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        row.Cells["Status"].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        row.Cells["Status"].Style.ForeColor = Color.Green;
                    }

                    // Add tooltip with full timestamp
                    row.Cells["Time"].ToolTipText = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    // Add tooltip with full description if truncated
                    if (log.ActionDescription.Length > 40)
                    {
                        row.Cells["Description"].ToolTipText = log.ActionDescription;
                    }
                }

                // Update status label
                lblAuditStatus.Text = $"Showing {logs.Count} of {totalCount} records | Last updated: {DateTime.Now:HH:mm:ss}";

                AppDebug.Log("tabProfile", $"Loaded {logs.Count} audit logs");
            }
            catch (Exception ex)
            {
                AppDebug.Log("tabProfile", $"Failed to load audit logs: {ex.Message}");
                lblAuditStatus.Text = "Error loading audit logs";
            }
        }

        /// <summary>
        /// Category filter changed event handler
        /// </summary>
        private void cbAuditCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentCategoryFilter = cbAuditCategoryFilter.SelectedItem?.ToString() ?? "All";
            LoadAuditLogs();
        }

        /// <summary>
        /// User filter text changed event handler (with debounce)
        /// </summary>
        private void txtAuditUserFilter_TextChanged(object sender, EventArgs e)
        {
            _currentUserFilter = txtAuditUserFilter.Text;
            
            // Reset timer for debouncing
            _auditFilterTimer?.Stop();
            _auditFilterTimer?.Start();
        }

        /// <summary>
        /// Refresh button click event handler
        /// </summary>
        private void btnAuditRefresh_Click(object sender, EventArgs e)
        {
            LoadAuditLogs();
        }

        #endregion
    }
}
