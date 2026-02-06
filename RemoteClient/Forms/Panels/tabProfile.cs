using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabProfile;
using HawkSyncShared.Instances;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels;

public partial class tabProfile : UserControl
{
    private theInstance theInstance => CommonCore.theInstance!;

	// ================================================================================
	// EDIT MODE TRACKING
	// ================================================================================

	private bool _isEditing = false;
    private bool _suppressChangeTracking = false;
    private DateTime _lastEditTime = DateTime.MinValue;
    private System.Windows.Forms.Timer? _inactivityTimer;
    private const int INACTIVITY_TIMEOUT_SECONDS = 120; // 2 minutes

    public tabProfile()
    {
        InitializeComponent();

        // Subscribe to server updates
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;

        // Initial load from server
        ApplySettingsToUI();

        // Setup inactivity timer
        SetupInactivityTimer();

        // Wire up change events to all input controls
        WireUpChangeTracking();
    }

    // ================================================================================
    // INACTIVITY TIMER SETUP
    // ================================================================================

    private void SetupInactivityTimer()
    {
        _inactivityTimer = new System.Windows.Forms.Timer
        {
            Interval = 1000 // Check every second
        };
        _inactivityTimer.Tick += InactivityTimer_Tick;
        _inactivityTimer.Start();
    }

    private void InactivityTimer_Tick(object? sender, EventArgs e)
    {
        if (!_isEditing)
            return;

        // Update countdown display
        UpdateEditModeIndicator();

        // Check if timeout reached
        if ((DateTime.Now - _lastEditTime).TotalSeconds >= INACTIVITY_TIMEOUT_SECONDS)
        {
            // Auto-refresh after inactivity
            _isEditing = false;
            ApplySettingsToUI();
            UpdateEditModeIndicator();

            MessageBox.Show(
                "Form refreshed due to 2 minutes of inactivity.\nAny unsaved changes were discarded.",
                "Auto-Refresh",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    // ================================================================================
    // CHANGE TRACKING
    // ================================================================================

    private void WireUpChangeTracking()
    {
        // Wire up all textboxes
        tb_profileServerPath.TextChanged += Control_ValueChanged;
        tb_modFile.TextChanged += Control_ValueChanged;
        tb_hostName.TextChanged += Control_ValueChanged;
        tb_serverName.TextChanged += Control_ValueChanged;
        tb_serverMessage.TextChanged += Control_ValueChanged;
        tb_serverPassword.TextChanged += Control_ValueChanged;
        serverCountryCode.TextChanged += Control_ValueChanged;

        // Wire up numeric controls
        num_serverPort.ValueChanged += Control_ValueChanged;
        num_remotePort.ValueChanged += Control_ValueChanged;
        num_minPing.ValueChanged += Control_ValueChanged;
        num_maxPing.ValueChanged += Control_ValueChanged;

        // Wire up checkboxes
        cb_serverDedicated.CheckedChanged += Control_ValueChanged;
        cb_requireNova.CheckedChanged += Control_ValueChanged;
        checkBox_enableRemote.CheckedChanged += Control_ValueChanged;
        cb_enableMinCheck.CheckedChanged += Control_ValueChanged;
        cb_enableMaxCheck.CheckedChanged += Control_ValueChanged;

        // Wire up all attribute checkboxes
        for (int i = 1; i <= 21; i++)
        {
            var checkbox = Controls.Find($"profileServerAttribute{i:D2}", true).FirstOrDefault() as CheckBox;
            if (checkbox != null)
            {
                checkbox.CheckedChanged += Control_ValueChanged;
            }
        }

        // Wire up combobox
        cb_serverIP.SelectedIndexChanged += Control_ValueChanged;
    }

    private void Control_ValueChanged(object? sender, EventArgs e)
    {
        // Ignore changes when loading from server
        if (_suppressChangeTracking)
            return;

        // User made a change - enter edit mode
        if (!_isEditing)
        {
            _isEditing = true;
            UpdateEditModeIndicator();
        }

        // Reset inactivity timer
        _lastEditTime = DateTime.Now;
    }

    private void UpdateEditModeIndicator()
    {
        if (_isEditing)
        {
            // Calculate time until auto-refresh
            int secondsRemaining = INACTIVITY_TIMEOUT_SECONDS -
                (int)(DateTime.Now - _lastEditTime).TotalSeconds;

            // Change save button appearance
            btn_saveProfile.BackColor = Color.Orange;
            btn_saveProfile.Text = $"Save";
        }
        else
        {
            // Reset background
            this.BackColor = SystemColors.Control;

            // Reset save button
            btn_saveProfile.BackColor = SystemColors.Control;
            btn_saveProfile.Text = "Save";
        }
    }

    // ================================================================================
    // SNAPSHOT UPDATE (Smart Logic)
    // ================================================================================

    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }

        // ✅ ONLY update if NOT in edit mode
        if (!_isEditing)
        {
            ApplySettingsToUI();
        }
        // else: User is editing - don't overwrite their changes

        bool currentState = (theInstance!.instanceStatus == InstanceStatus.OFFLINE);

        // Enable/disable profile controls based on server status
        btn_profileBrowse1.Enabled = currentState;
        btn_profileBrowse2.Enabled = currentState;

    }

    // ================================================================================
    // LOAD/SAVE OPERATIONS
    // ================================================================================

    private async void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Build settings from UI
            var settings = BuildProfileSettingsFromUI();

            // Validate first
            var validation = await ApiCore.ApiClient!.ValidateProfileSettingsAsync(settings);

            if (!validation.IsValid)
            {
                MessageBox.Show(
                    $"Validation failed:\n\n{string.Join("\n", validation.Errors)}",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Disable UI during save
            btn_saveProfile.Text = "Saving...";

            // Save to server
            var result = await ApiCore.ApiClient.SaveProfileSettingsAsync(settings);

            if (result.Success)
            {
                // Exit edit mode
                _isEditing = false;
                UpdateEditModeIndicator();

                MessageBox.Show(
                    "Profile settings saved successfully!\n\nThe server has been updated.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reload from server to confirm
                ApplySettingsToUI();
            }
            else
            {
                MessageBox.Show(
                    $"Failed to save settings:\n\n{result.Message}",
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error saving settings:\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            UpdateEditModeIndicator();
        }
    }

    private void BtnReset_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Discard all changes and reload from server?",
            "Confirm Reset",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // Exit edit mode
            _isEditing = false;
            UpdateEditModeIndicator();

            ApplySettingsToUI();
        }
    }

    // ================================================================================
    // UI MAPPING METHODS
    // ================================================================================

    private ProfileSettingsRequest BuildProfileSettingsFromUI()
    {
        return new ProfileSettingsRequest
        {
            ServerPath = tb_profileServerPath.Text.Trim(),
            ModFileName = tb_modFile.Text.Trim(),
            HostName = tb_hostName.Text.Trim(),
            ServerName = tb_serverName.Text.Trim(),
            MOTD = tb_serverMessage.Text.Trim(),
            BindIP = cb_serverIP.SelectedItem?.ToString() ?? "0.0.0.0",
            BindPort = (int)num_serverPort.Value,
            LobbyPassword = tb_serverPassword.Text,
            Dedicated = cb_serverDedicated.Checked,
            RequireNova = cb_requireNova.Checked,
            CountryCode = serverCountryCode.Text.Trim().ToUpper(),
            MinPingEnabled = cb_enableMinCheck.Checked,
            MaxPingEnabled = cb_enableMaxCheck.Checked,
            MinPingValue = (int)num_minPing.Value,
            MaxPingValue = (int)num_maxPing.Value,
            EnableRemote = checkBox_enableRemote.Checked,
            RemotePort = (int)num_remotePort.Value,
            Attributes = new CommandLineFlagsDTO
            {
                Flag01 = profileServerAttribute01.Checked,
                Flag02 = profileServerAttribute02.Checked,
                Flag03 = profileServerAttribute03.Checked,
                Flag04 = profileServerAttribute04.Checked,
                Flag05 = profileServerAttribute05.Checked,
                Flag06 = profileServerAttribute06.Checked,
                Flag07 = profileServerAttribute07.Checked,
                Flag08 = profileServerAttribute08.Checked,
                Flag09 = profileServerAttribute09.Checked,
                Flag10 = profileServerAttribute10.Checked,
                Flag11 = profileServerAttribute11.Checked,
                Flag12 = profileServerAttribute12.Checked,
                Flag13 = profileServerAttribute13.Checked,
                Flag14 = profileServerAttribute14.Checked,
                Flag15 = profileServerAttribute15.Checked,
                Flag16 = profileServerAttribute16.Checked,
                Flag17 = profileServerAttribute17.Checked,
                Flag18 = profileServerAttribute18.Checked,
                Flag19 = profileServerAttribute19.Checked,
                Flag20 = profileServerAttribute20.Checked,
                Flag21 = profileServerAttribute21.Checked
            }
        };
    }

    private void ApplySettingsToUI()
    {
        // Suppress change tracking during programmatic updates
        _suppressChangeTracking = true;
        try
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
        finally
        {
            // Always re-enable change tracking
            _suppressChangeTracking = false;
        }
    }

    // ================================================================================
    // FILE BROWSING OPERATIONS
    // ================================================================================

    private void BtnBrowseServerPath_Click(object sender, EventArgs e)
    {
        try
        {
            using var browserDialog = new Dialogs.RemoteFileBrowserDialog(selectFiles: false);
        
            if (browserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(browserDialog.SelectedPath))
            {
                tb_profileServerPath.Text = browserDialog.SelectedPath;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error browsing server:\n\n{ex.Message}",
                "Browse Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnBrowseModFile_Click(object sender, EventArgs e)
    {
        try
        {
            using var browserDialog = new Dialogs.RemoteFileBrowserDialog(selectFiles: true, fileFilter: "*.pff");
        
            if (browserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(browserDialog.SelectedPath))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(browserDialog.SelectedPath);
                tb_modFile.Text = fileNameWithoutExtension;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error browsing server:\n\n{ex.Message}",
                "Browse Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }


}