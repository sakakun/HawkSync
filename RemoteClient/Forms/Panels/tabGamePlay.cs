using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels;

public partial class tabGamePlay : UserControl
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
    private bool _updatingWeaponCheckboxes = false;
    private List<CheckBox> weaponCheckboxes = new();

    public tabGamePlay()
    {
        InitializeComponent();

        // Initialize weapon checkboxes
        InitializeWeaponCheckboxes();

        // Subscribe to server updates
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;
        
        // Initial load from server
        ApplySettingsToUI();

        // Setup inactivity timer
        SetupInactivityTimer();

        // Wire up change events to all input controls
        WireUpChangeTracking();

        // Initialize button visibility and state
        UpdateServerControlVisibility();
        UpdateServerControlButtonState(); // Add this line
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
        // Wire up textboxes
        tb_bluePassword.TextChanged += Control_ValueChanged;
        tb_redPassword.TextChanged += Control_ValueChanged;

        // Wire up numeric controls
        num_scoresKOTH.ValueChanged += Control_ValueChanged;
        num_scoresDM.ValueChanged += Control_ValueChanged;
        num_scoresFB.ValueChanged += Control_ValueChanged;
        num_gameTimeLimit.ValueChanged += Control_ValueChanged;
        num_gameStartDelay.ValueChanged += Control_ValueChanged;
        num_respawnTime.ValueChanged += Control_ValueChanged;
        num_scoreBoardDelay.ValueChanged += Control_ValueChanged;
        num_maxPlayers.ValueChanged += Control_ValueChanged;
        num_pspTakeoverTimer.ValueChanged += Control_ValueChanged;
        num_flagReturnTime.ValueChanged += Control_ValueChanged;
        num_maxTeamLives.ValueChanged += Control_ValueChanged;
        num_maxFFKills.ValueChanged += Control_ValueChanged;

        // Wire up checkboxes
        cb_autoBalance.CheckedChanged += Control_ValueChanged;
        cb_showTracers.CheckedChanged += Control_ValueChanged;
        cb_showClays.CheckedChanged += Control_ValueChanged;
        cb_autoRange.CheckedChanged += Control_ValueChanged;
        cb_customSkins.CheckedChanged += Control_ValueChanged;
        cb_enableDistroyBuildings.CheckedChanged += Control_ValueChanged;
        cb_enableFatBullets.CheckedChanged += Control_ValueChanged;
        cb_enableOneShotKills.CheckedChanged += Control_ValueChanged;
        cb_enableLeftLean.CheckedChanged += Control_ValueChanged;
        cb_enableFFkills.CheckedChanged += Control_ValueChanged;
        cb_warnFFkils.CheckedChanged += Control_ValueChanged;
        cb_showTeamTags.CheckedChanged += Control_ValueChanged;
        cb_roleCQB.CheckedChanged += Control_ValueChanged;
        cb_roleGunner.CheckedChanged += Control_ValueChanged;
        cb_roleSniper.CheckedChanged += Control_ValueChanged;
        cb_roleMedic.CheckedChanged += Control_ValueChanged;

        // Wire up combobox
        cb_replayMaps.SelectedIndexChanged += Control_ValueChanged;

        // Wire up weapon checkboxes
        foreach (var checkbox in weaponCheckboxes)
        {
            checkbox.CheckedChanged += Control_ValueChanged;
        }

        // Wire up select all/none (special handling)
        checkBox_selectAll.CheckedChanged += SelectAllNone_CheckedChanged;
        checkBox_selectNone.CheckedChanged += SelectAllNone_CheckedChanged;
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

        // Update select all/none checkboxes if weapon changed
        if (sender is CheckBox cb && weaponCheckboxes.Contains(cb))
        {
            UpdateWeaponSelectCheckboxes();
        }
    }

    private void SelectAllNone_CheckedChanged(object? sender, EventArgs e)
    {
        if (_suppressChangeTracking || _updatingWeaponCheckboxes)
            return;

        _updatingWeaponCheckboxes = true;

        if (sender == checkBox_selectAll && checkBox_selectAll.Checked)
        {
            weaponCheckboxes.ForEach(cb => cb.Checked = true);
            checkBox_selectNone.Checked = false;
        }
        else if (sender == checkBox_selectNone && checkBox_selectNone.Checked)
        {
            weaponCheckboxes.ForEach(cb => cb.Checked = false);
            checkBox_selectAll.Checked = false;
        }

        _updatingWeaponCheckboxes = false;

        // Trigger edit mode
        Control_ValueChanged(sender, e);
    }

    private void UpdateEditModeIndicator()
    {
        if (_isEditing)
        {
            // Change save button appearance
            btn_SaveSettings.BackColor = Color.Orange;
            btn_SaveSettings.Text = "SAVE";
        }
        else
        {
            // Reset save button
            btn_SaveSettings.BackColor = SystemColors.Control;
            btn_SaveSettings.Text = "SAVE";
        }
    }

    // ================================================================================
    // SERVER CONTROL VISIBILITY
    // ================================================================================

    private void UpdateServerControlVisibility()
    {
        if (InvokeRequired)
        {
            Invoke(UpdateServerControlVisibility);
            return;
        }

        // Show update and lock lobby buttons only when server is running (not OFFLINE)
        bool isServerRunning = theInstance.instanceStatus != InstanceStatus.OFFLINE;
        btn_ServerUpdate.Visible = isServerRunning;
        btn_LockLobby.Visible = isServerRunning;
    }

    // ================================================================================
    // SNAPSHOT UPDATE (Smart Logic)
    // ================================================================================

    // Update the OnSnapshotReceived method:
    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }

        // Update button visibility based on server status
        UpdateServerControlVisibility();
        UpdateServerControlButtonState(); // Add this line

        // ✅ ONLY update if NOT in edit mode
        if (!_isEditing)
        {
            ApplySettingsToUI();
        }
        // else: User is editing - don't overwrite their changes
    }

    // ================================================================================
    // LOAD/SAVE OPERATIONS
    // ================================================================================

    private async void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Build settings from UI
            var settings = BuildGamePlaySettingsFromUI();

            // Validate first
            var validation = await ApiCore.ApiClient!.ValidateGamePlaySettingsAsync(settings);

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
            btn_SaveSettings.Text = "SAVING";
            btn_SaveSettings.Enabled = false;

            // Save to server
            var result = await ApiCore.ApiClient.SaveGamePlaySettingsAsync(settings);

            if (result.Success)
            {
                // Exit edit mode
                _isEditing = false;
                UpdateEditModeIndicator();

                MessageBox.Show(
                    "Gameplay settings saved successfully!\n\nThe server has been updated.",
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
            btn_SaveSettings.Enabled = true;
            btn_SaveSettings.Text = "SAVE";
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

    private async void BtnLockLobby_Click(object sender, EventArgs e)
    {
        try
        {
            // Disable button during operation
            btn_LockLobby.Enabled = false;
            var originalText = btn_LockLobby.Text;
            btn_LockLobby.Text = "UN/LOCKING";

            // Call API to lock lobby
            var result = await ApiCore.ApiClient!.LockLobbyAsync();

            if (result.Success)
            {
                MessageBox.Show(
                    result.Message,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Failed to un/lock lobby:\n\n{result.Message}",
                    "Lock Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error un/locking lobby:\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btn_LockLobby.Enabled = true;
            btn_LockLobby.Text = "UN/LOCK LOBBY";
        }
    }

    private async void BtnServerUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Confirm action
            var confirmResult = MessageBox.Show(
                "Apply current gameplay settings to the running game server?\n\nThis will update the live server immediately.",
                "Confirm Server Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            // Disable button during operation
            btn_ServerUpdate.Enabled = false;
            btn_ServerUpdate.Text = "UPDATING";

            // Call API to update server
            var result = await ApiCore.ApiClient!.UpdateGameServerAsync();

            if (result.Success)
            {
                MessageBox.Show(
                    result.Message,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Failed to update game server:\n\n{result.Message}",
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error updating game server:\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btn_ServerUpdate.Enabled = true;
            btn_ServerUpdate.Text = "UPDATE";
        }
    }

    // ================================================================================
    // UI MAPPING METHODS
    // ================================================================================

    private GamePlaySettingsRequest BuildGamePlaySettingsFromUI()
    {
        var options = new ServerOptionsDTO(
            AutoBalance: cb_autoBalance.Checked,
            ShowTracers: cb_showTracers.Checked,
            ShowClays: cb_showClays.Checked,
            AutoRange: cb_autoRange.Checked,
            CustomSkins: cb_customSkins.Checked,
            DestroyBuildings: cb_enableDistroyBuildings.Checked,
            FatBullets: cb_enableFatBullets.Checked,
            OneShotKills: cb_enableOneShotKills.Checked,
            AllowLeftLeaning: cb_enableLeftLean.Checked
        );

        var friendlyFire = new FriendlyFireSettingsDTO(
            Enabled: cb_enableFFkills.Checked,
            MaxKills: (int)num_maxFFKills.Value,
            WarnOnKill: cb_warnFFkils.Checked,
            ShowFriendlyTags: cb_showTeamTags.Checked
        );

        var roles = new RoleRestrictionsDTO(
            CQB: cb_roleCQB.Checked,
            Gunner: cb_roleGunner.Checked,
            Sniper: cb_roleSniper.Checked,
            Medic: cb_roleMedic.Checked
        );

        var weapons = new WeaponRestrictionsDTO(
            Colt45: cb_weapColt45.Checked,
            M9Beretta: cb_weapM9Bereatta.Checked,
            CAR15: cb_weapCAR15.Checked,
            CAR15203: cb_weapCAR15203.Checked,
            M16: cb_weapM16.Checked,
            M16203: cb_weapM16203.Checked,
            G3: cb_weapG3.Checked,
            G36: cb_weapG36.Checked,
            M60: cb_weapM60.Checked,
            M240: cb_weapM240.Checked,
            MP5: cb_weapMP5.Checked,
            SAW: cb_weapSaw.Checked,
            MCRT300: cb_weap300Tact.Checked,
            M21: cb_weapM21.Checked,
            M24: cb_weapM24.Checked,
            Barrett: cb_weapBarret.Checked,
            PSG1: cb_weapPSG1.Checked,
            Shotgun: cb_weapShotgun.Checked,
            FragGrenade: cb_weapFragGrenade.Checked,
            SmokeGrenade: cb_weapSmokeGrenade.Checked,
            Satchel: cb_weapSatchel.Checked,
            AT4: cb_weapAT4.Checked,
            FlashBang: cb_weapFlashBang.Checked,
            Claymore: cb_weapClay.Checked
        );

        return new GamePlaySettingsRequest
        {
            BluePassword = tb_bluePassword.Text,
            RedPassword = tb_redPassword.Text,
            ScoreKOTH = (int)num_scoresKOTH.Value,
            ScoreDM = (int)num_scoresDM.Value,
            ScoreFB = (int)num_scoresFB.Value,
            TimeLimit = (int)num_gameTimeLimit.Value,
            LoopMaps = cb_replayMaps.SelectedIndex,
            StartDelay = (int)num_gameStartDelay.Value,
            RespawnTime = (int)num_respawnTime.Value,
            ScoreBoardDelay = (int)num_scoreBoardDelay.Value,
            MaxSlots = (int)num_maxPlayers.Value,
            PSPTakeoverTimer = (int)num_pspTakeoverTimer.Value,
            FlagReturnTime = (int)num_flagReturnTime.Value,
            MaxTeamLives = (int)num_maxTeamLives.Value,
            Options = options,
            FriendlyFire = friendlyFire,
            Roles = roles,
            Weapons = weapons
        };
    }

    private void ApplySettingsToUI()
    {
        // Suppress change tracking during programmatic updates
        _suppressChangeTracking = true;
        _updatingWeaponCheckboxes = true;

        try
        {
            // Lobby Passwords
            tb_bluePassword.Text = theInstance.gamePasswordBlue;
            tb_redPassword.Text = theInstance.gamePasswordRed;

            // Match Win Conditions
            num_scoresKOTH.Value = theInstance.gameScoreZoneTime;
            num_scoresDM.Value = theInstance.gameScoreKills;
            num_scoresFB.Value = theInstance.gameScoreFlags;

            // Server Values
            num_gameTimeLimit.Value = theInstance.gameTimeLimit;
            cb_replayMaps.SelectedIndex = theInstance.gameLoopMaps;
            num_gameStartDelay.Value = theInstance.gameStartDelay;
            num_respawnTime.Value = theInstance.gameRespawnTime;
            num_scoreBoardDelay.Value = theInstance.gameScoreBoardDelay;
            num_maxPlayers.Value = theInstance.gameMaxSlots;
            num_pspTakeoverTimer.Value = theInstance.gamePSPTOTimer;
            num_flagReturnTime.Value = theInstance.gameFlagReturnTime;
            num_maxTeamLives.Value = theInstance.gameMaxTeamLives;

            // Server Options
            cb_autoBalance.Checked = theInstance.gameOptionAutoBalance;
            cb_customSkins.Checked = theInstance.gameCustomSkins;
            cb_enableDistroyBuildings.Checked = theInstance.gameDestroyBuildings;
            cb_enableFatBullets.Checked = theInstance.gameFatBullets;
            cb_enableOneShotKills.Checked = theInstance.gameOneShotKills;
            cb_enableLeftLean.Checked = theInstance.gameAllowLeftLeaning;
            cb_showTracers.Checked = theInstance.gameOptionShowTracers;
            cb_showClays.Checked = theInstance.gameShowTeamClays;
            cb_autoRange.Checked = theInstance.gameOptionAutoRange;

            // Friendly Fire
            cb_enableFFkills.Checked = theInstance.gameOptionFF;
            num_maxFFKills.Value = theInstance.gameFriendlyFireKills;
            cb_warnFFkils.Checked = theInstance.gameOptionFFWarn;
            cb_showTeamTags.Checked = theInstance.gameOptionFriendlyTags;

            // Role Restrictions
            cb_roleCQB.Checked = theInstance.roleCQB;
            cb_roleGunner.Checked = theInstance.roleGunner;
            cb_roleSniper.Checked = theInstance.roleSniper;
            cb_roleMedic.Checked = theInstance.roleMedic;

            // Weapon Restrictions
            cb_weapColt45.Checked = theInstance.weaponColt45;
            cb_weapM9Bereatta.Checked = theInstance.weaponM9Beretta;
            cb_weapCAR15.Checked = theInstance.weaponCar15;
            cb_weapCAR15203.Checked = theInstance.weaponCar15203;
            cb_weapM16.Checked = theInstance.weaponM16;
            cb_weapM16203.Checked = theInstance.weaponM16203;
            cb_weapG3.Checked = theInstance.weaponG3;
            cb_weapG36.Checked = theInstance.weaponG36;
            cb_weapM60.Checked = theInstance.weaponM60;
            cb_weapM240.Checked = theInstance.weaponM240;
            cb_weapMP5.Checked = theInstance.weaponMP5;
            cb_weapSaw.Checked = theInstance.weaponSAW;
            cb_weap300Tact.Checked = theInstance.weaponMCRT300;
            cb_weapM21.Checked = theInstance.weaponM21;
            cb_weapM24.Checked = theInstance.weaponM24;
            cb_weapBarret.Checked = theInstance.weaponBarrett;
            cb_weapPSG1.Checked = theInstance.weaponPSG1;
            cb_weapShotgun.Checked = theInstance.weaponShotgun;
            cb_weapFragGrenade.Checked = theInstance.weaponFragGrenade;
            cb_weapSmokeGrenade.Checked = theInstance.weaponSmokeGrenade;
            cb_weapSatchel.Checked = theInstance.weaponSatchelCharges;
            cb_weapAT4.Checked = theInstance.weaponAT4;
            cb_weapFlashBang.Checked = theInstance.weaponFlashGrenade;
            cb_weapClay.Checked = theInstance.weaponClaymore;

            // Update select all/none checkboxes
            UpdateWeaponSelectCheckboxes();
        }
        finally
        {
            // Always re-enable change tracking
            _suppressChangeTracking = false;
            _updatingWeaponCheckboxes = false;
        }
    }

    // ================================================================================
    // WEAPON CHECKBOX LOGIC
    // ================================================================================

    private void InitializeWeaponCheckboxes()
    {
        weaponCheckboxes = new()
        {
            cb_weapColt45, cb_weapM9Bereatta, cb_weapCAR15, cb_weapCAR15203, 
            cb_weapM16, cb_weapM16203, cb_weapG3, cb_weapG36, 
            cb_weapM60, cb_weapM240, cb_weapMP5, cb_weapSaw, 
            cb_weap300Tact, cb_weapM21, cb_weapM24, cb_weapBarret, 
            cb_weapPSG1, cb_weapShotgun, cb_weapFragGrenade, cb_weapSmokeGrenade, 
            cb_weapSatchel, cb_weapAT4, cb_weapFlashBang, cb_weapClay
        };
    }

    private void UpdateWeaponSelectCheckboxes()
    {
        if (_updatingWeaponCheckboxes) 
            return;

        _updatingWeaponCheckboxes = true;

        checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
        checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);

        _updatingWeaponCheckboxes = false;
    }

    // ================================================================================
    // EXPORT/IMPORT OPERATIONS
    // ================================================================================

    private async void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            // Get settings from server
            btn_ExportSettings.Enabled = false;
            btn_ExportSettings.Text = "EXPORTING";

            var exportResponse = await ApiCore.ApiClient!.ExportGamePlaySettingsAsync();

            if (exportResponse == null || !exportResponse.Success)
            {
                MessageBox.Show(
                    $"Failed to export settings:\n\n{exportResponse?.Message ?? "Unknown error"}",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Prompt user to save the file
            using SaveFileDialog saveFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = exportResponse.FileName
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the JSON data to file
                await File.WriteAllTextAsync(saveFileDialog.FileName, exportResponse.JsonData);

                MessageBox.Show(
                    $"Gameplay settings exported successfully to:\n{saveFileDialog.FileName}",
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error exporting settings:\n\n{ex.Message}",
                "Export Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btn_ExportSettings.Enabled = true;
            btn_ExportSettings.Text = "EXPORT";
        }
    }

    private async void BtnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            // Prompt user to select a file
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // Read the JSON file
            string jsonData = await File.ReadAllTextAsync(openFileDialog.FileName);

            // Confirm before importing
            var confirmResult = MessageBox.Show(
                "Import these settings to the server?\n\nThis will overwrite the current server settings.",
                "Confirm Import",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            // Send to server
            btn_LoadSettings.Enabled = false;
            btn_LoadSettings.Text = "IMPORTING";

            var result = await ApiCore.ApiClient!.ImportGamePlaySettingsAsync(jsonData);

            if (result.Success)
            {
                MessageBox.Show(
                    "Gameplay settings imported successfully!\n\nThe server has been updated.",
                    "Import Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reload UI to reflect imported settings
                ApplySettingsToUI();
            }
            else
            {
                MessageBox.Show(
                    $"Failed to import settings:\n\n{result.Message}",
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (System.Text.Json.JsonException ex)
        {
            MessageBox.Show(
                $"Invalid JSON file format:\n\n{ex.Message}",
                "Import Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error importing settings:\n\n{ex.Message}",
                "Import Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btn_LoadSettings.Enabled = true;
            btn_LoadSettings.Text = "LOAD";
        }
    }

    // ================================================================================
    // SERVER CONTROL OPERATIONS
    // ================================================================================

    private async void BtnServerControl_Click(object sender, EventArgs e)
    {
        try
        {
            bool isOffline = theInstance.instanceStatus == InstanceStatus.OFFLINE;
            string action = isOffline ? "start" : "stop";

            // Confirm action
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to {action} the game server?",
                $"Confirm Server {action.ToUpper()}",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            // Disable button during operation
            btn_serverControl.Enabled = false;
            btn_serverControl.Text = isOffline ? "STARTING" : "STOPPING";

            CommandResult result;

            if (isOffline)
            {
                // Start the server
                result = await ApiCore.ApiClient!.StartServerAsync();
            }
            else
            {
                // Stop the server
                result = await ApiCore.ApiClient!.StopServerAsync();
            }

            if (result.Success)
            {
                MessageBox.Show(
                    result.Message,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Button state will update automatically via snapshot
            }
            else
            {
                MessageBox.Show(
                    $"Failed to {action} server:\n\n{result.Message}",
                    $"{action.ToUpper()} Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error controlling server:\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btn_serverControl.Enabled = true;
            UpdateServerControlButtonState();
        }
    }

    private void UpdateServerControlButtonState()
    {
        if (InvokeRequired)
        {
            Invoke(UpdateServerControlButtonState);
            return;
        }

        bool isOffline = theInstance.instanceStatus == InstanceStatus.OFFLINE;
        btn_serverControl.Text = isOffline ? "START" : "STOP";
    
    }

}