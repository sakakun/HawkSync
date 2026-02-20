using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabGameplay;
using HawkSyncShared.Instances;
using RemoteClient.Core;
using System.Drawing;

namespace RemoteClient.Forms.Panels;

public partial class tabGamePlayV2 : UserControl
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
    private bool _updatingButtons = false;
    private List<Button> weaponButtons = new();
    private List<Button> roleButtons = new();
    private List<Button> optionButtons = new();
    private List<Button> friendlyFireButtons = new();

    public tabGamePlayV2()
    {
        InitializeComponent();

        // Initialize button lists
        InitializeButtonLists();

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
        UpdateServerControlButtonState();
    }

    // ================================================================================
    // INITIALIZATION
    // ================================================================================

    private void InitializeButtonLists()
    {
        weaponButtons = new()
        {
            btnWeapon01, btnWeapon02, btnWeapon11, btnWeapon12, btnWeapon15, btnWeapon16,
            btnWeapon13, btnWeapon14, btnWeapon10, btnWeapon17, btnWeapon18, btnWeapon19, btnWeapon23,
            btnWeapon21, btnWeapon22, btnWeapon20, btnWeapon24, btnWeapon03, btnWeapon07,
            btnWeapon09, btnWeapon08, btnWeapon04, btnWeapon06, btnWeapon05
        };

        // Enable right-click for weapon buttons
        foreach (var btn in weaponButtons)
        {
            btn.MouseDown += WeaponButton_MouseDown;
        }

        roleButtons = new() 
        { 
            btn_RolesCQB, btn_RolesGunner, btn_RolesMedic, btn_RolesSniper 
        };

        optionButtons = new()
        {
            btn_AutoBalance, btn_CustomSkins, btn_AllowLeftLeaning, btn_AllowRightLeaning, btn_AutoRange, 
            btn_DestroyBuildings, btn_FatBullets, btn_OneShotKills, btn_ShowTracers, btn_ShowClays
        };

        friendlyFireButtons = new()
        {
            btn_FriendlyFireEnabled, btn_ShowFriendlyTags, btn_WarnOnFFKill
        };
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
        num_FullWeaponThreshold.ValueChanged += Control_ValueChanged;

        // Wire up combobox
        cb_replayMaps.SelectedIndexChanged += Control_ValueChanged;

        // Wire up all buttons
        foreach (var btn in weaponButtons)
            btn.Click += Button_Click;
        foreach (var btn in roleButtons)
            btn.Click += Button_Click;
        foreach (var btn in optionButtons)
            btn.Click += Button_Click;
        foreach (var btn in friendlyFireButtons)
            btn.Click += Button_Click;

        // Wire up weapon select all/none
        btnWeaponSA.Click += WeaponSelectAll_Click;
        btnWeaponSN.Click += WeaponSelectNone_Click;
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

    private void Button_Click(object? sender, EventArgs e)
    {
        if (_suppressChangeTracking || _updatingButtons)
            return;

        var btn = sender as Button;
        if (btn == null) return;

        // For weapon buttons, use 3-state logic (Green/Gold/Gray)
        if (weaponButtons.Contains(btn))
        {
            // Cycle through states on left-click
            if (btn.BackColor == Color.LightGray)
                btn.BackColor = Color.LightGreen;
            else if (btn.BackColor == Color.LightGreen)
                btn.BackColor = Color.Gold;
            else
                btn.BackColor = Color.LightGray;
        }
        else
        {
            // For non-weapon buttons, toggle between Green and Gray
            btn.BackColor = btn.BackColor == Color.LightGreen ? Color.LightGray : Color.LightGreen;
        }

        // Trigger edit mode
        Control_ValueChanged(sender, e);
    }

    private void WeaponButton_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            if (_suppressChangeTracking || _updatingButtons)
                return;

            var btn = sender as Button;
            if (btn == null) return;

            // Right-click: toggle between Gold (limited) and LightGray (disabled)
            if (btn.BackColor == Color.Gold)
                btn.BackColor = Color.LightGray;
            else
                btn.BackColor = Color.Gold;

            // Trigger edit mode
            Control_ValueChanged(sender, e);
        }
    }

    private void WeaponSelectAll_Click(object? sender, EventArgs e)
    {
        if (_suppressChangeTracking || _updatingButtons)
            return;

        _updatingButtons = true;
        weaponButtons.ForEach(btn => btn.BackColor = Color.LightGreen);
        _updatingButtons = false;

        Control_ValueChanged(sender, e);
    }

    private void WeaponSelectNone_Click(object? sender, EventArgs e)
    {
        if (_suppressChangeTracking || _updatingButtons)
            return;

        _updatingButtons = true;
        weaponButtons.ForEach(btn => btn.BackColor = Color.LightGray);
        _updatingButtons = false;

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

    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }

        // Update button visibility based on server status
        UpdateServerControlVisibility();
        UpdateServerControlButtonState();

        // ✅ ONLY update if NOT in edit mode
        if (!_isEditing)
        {
            ApplySettingsToUI();
        }
    }

    // ================================================================================
    // LOAD/SAVE OPERATIONS
    // ================================================================================

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            // Build settings from UI
            var settings = BuildGamePlaySettingsFromUI();

            // Validate first
            var validation = await ApiCore.ApiClient!.GamePlay.ValidateGamePlaySettingsAsync(settings);

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
            var result = await ApiCore.ApiClient.GamePlay.SaveGamePlaySettingsAsync(settings);

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

    private void BtnReset_Click(object? sender, EventArgs e)
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

    private async void BtnLockLobby_Click(object? sender, EventArgs e)
    {
        try
        {
            // Disable button during operation
            btn_LockLobby.Enabled = false;
            var originalText = btn_LockLobby.Text;
            btn_LockLobby.Text = "UN/LOCKING";

            // Call API to lock lobby
            var result = await ApiCore.ApiClient!.GamePlay.LockLobbyAsync();

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

    private async void BtnServerUpdate_Click(object? sender, EventArgs e)
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
            var result = await ApiCore.ApiClient!.GamePlay.UpdateGameServerAsync();

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
            AutoBalance: btn_AutoBalance.BackColor == Color.LightGreen,
            ShowTracers: btn_ShowTracers.BackColor == Color.LightGreen,
            ShowClays: btn_ShowClays.BackColor == Color.LightGreen,
            AutoRange: btn_AutoRange.BackColor == Color.LightGreen,
            CustomSkins: btn_CustomSkins.BackColor == Color.LightGreen,
            DestroyBuildings: btn_DestroyBuildings.BackColor == Color.LightGreen,
            FatBullets: btn_FatBullets.BackColor == Color.LightGreen,
            OneShotKills: btn_OneShotKills.BackColor == Color.LightGreen,
            AllowLeftLeaning: btn_AllowLeftLeaning.BackColor == Color.LightGreen,
            AllowRightLeaning: btn_AllowRightLeaning.BackColor == Color.LightGreen
        );

        var friendlyFire = new FriendlyFireSettingsDTO(
            Enabled: btn_FriendlyFireEnabled.BackColor == Color.LightGreen,
            MaxKills: (int)num_maxFFKills.Value,
            WarnOnKill: btn_WarnOnFFKill.BackColor == Color.LightGreen,
            ShowFriendlyTags: btn_ShowFriendlyTags.BackColor == Color.LightGreen
        );

        var roles = new RoleRestrictionsDTO(
            CQB: btn_RolesCQB.BackColor == Color.LightGreen,
            Gunner: btn_RolesGunner.BackColor == Color.LightGreen,
            Sniper: btn_RolesSniper.BackColor == Color.LightGreen,
            Medic: btn_RolesMedic.BackColor == Color.LightGreen
        );

        var weapons = new WeaponRestrictionsDTO(
            Colt45: btnWeapon01.BackColor == Color.LightGreen,
            M9Beretta: btnWeapon02.BackColor == Color.LightGreen,
            CAR15: btnWeapon11.BackColor == Color.LightGreen,
            CAR15203: btnWeapon12.BackColor == Color.LightGreen,
            M16: btnWeapon15.BackColor == Color.LightGreen,
            M16203: btnWeapon16.BackColor == Color.LightGreen,
            G3: btnWeapon13.BackColor == Color.LightGreen,
            G36: btnWeapon14.BackColor == Color.LightGreen,
            M60: btnWeapon18.BackColor == Color.LightGreen,
            M240: btnWeapon17.BackColor == Color.LightGreen,
            MP5: btnWeapon19.BackColor == Color.LightGreen,
            SAW: btnWeapon10.BackColor == Color.LightGreen,
            MCRT300: btnWeapon23.BackColor == Color.LightGreen,
            M21: btnWeapon21.BackColor == Color.LightGreen,
            M24: btnWeapon22.BackColor == Color.LightGreen,
            Barrett: btnWeapon20.BackColor == Color.LightGreen,
            PSG1: btnWeapon24.BackColor == Color.LightGreen,
            Shotgun: btnWeapon03.BackColor == Color.LightGreen,
            FragGrenade: btnWeapon07.BackColor == Color.LightGreen,
            SmokeGrenade: btnWeapon09.BackColor == Color.LightGreen,
            Satchel: btnWeapon08.BackColor == Color.LightGreen,
            AT4: btnWeapon04.BackColor == Color.LightGreen,
            FlashBang: btnWeapon06.BackColor == Color.LightGreen,
            Claymore: btnWeapon05.BackColor == Color.LightGreen
        );

        var limitedWeapons = new LimitedWeaponRestrictionsDTO(
            Colt45: btnWeapon01.BackColor == Color.Gold,
            M9Beretta: btnWeapon02.BackColor == Color.Gold,
            CAR15: btnWeapon11.BackColor == Color.Gold,
            CAR15203: btnWeapon12.BackColor == Color.Gold,
            M16: btnWeapon15.BackColor == Color.Gold,
            M16203: btnWeapon16.BackColor == Color.Gold,
            G3: btnWeapon13.BackColor == Color.Gold,
            G36: btnWeapon14.BackColor == Color.Gold,
            M60: btnWeapon18.BackColor == Color.Gold,
            M240: btnWeapon17.BackColor == Color.Gold,
            MP5: btnWeapon19.BackColor == Color.Gold,
            SAW: btnWeapon10.BackColor == Color.Gold,
            MCRT300: btnWeapon23.BackColor == Color.Gold,
            M21: btnWeapon21.BackColor == Color.Gold,
            M24: btnWeapon22.BackColor == Color.Gold,
            Barrett: btnWeapon20.BackColor == Color.Gold,
            PSG1: btnWeapon24.BackColor == Color.Gold,
            Shotgun: btnWeapon03.BackColor == Color.Gold,
            FragGrenade: btnWeapon07.BackColor == Color.Gold,
            SmokeGrenade: btnWeapon09.BackColor == Color.Gold,
            Satchel: btnWeapon08.BackColor == Color.Gold,
            AT4: btnWeapon04.BackColor == Color.Gold,
            FlashBang: btnWeapon06.BackColor == Color.Gold,
            Claymore: btnWeapon05.BackColor == Color.Gold
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
            FullWeaponThreshold = (int)num_FullWeaponThreshold.Value,
            Options = options,
            FriendlyFire = friendlyFire,
            Roles = roles,
            Weapons = weapons,
            LimitedWeapons = limitedWeapons
        };
    }

    private void ApplySettingsToUI()
    {
        // Suppress change tracking during programmatic updates
        _suppressChangeTracking = true;
        _updatingButtons = true;

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
            num_FullWeaponThreshold.Value = theInstance.gameFullWeaponThreshold;

            // Server Options (Buttons)
            btn_AutoBalance.BackColor = theInstance.gameOptionAutoBalance ? Color.LightGreen : Color.LightGray;
            btn_CustomSkins.BackColor = theInstance.gameCustomSkins ? Color.LightGreen : Color.LightGray;
            btn_AllowLeftLeaning.BackColor = theInstance.gameAllowLeftLeaning ? Color.LightGreen : Color.LightGray;
            btn_AllowRightLeaning.BackColor = theInstance.gameAllowRightLeaning ? Color.LightGreen : Color.LightGray;
            btn_AutoRange.BackColor = theInstance.gameOptionAutoRange ? Color.LightGreen : Color.LightGray;
            btn_DestroyBuildings.BackColor = theInstance.gameDestroyBuildings ? Color.LightGreen : Color.LightGray;
            btn_FatBullets.BackColor = theInstance.gameFatBullets ? Color.LightGreen : Color.LightGray;
            btn_OneShotKills.BackColor = theInstance.gameOneShotKills ? Color.LightGreen : Color.LightGray;
            btn_ShowTracers.BackColor = theInstance.gameOptionShowTracers ? Color.LightGreen : Color.LightGray;
            btn_ShowClays.BackColor = theInstance.gameShowTeamClays ? Color.LightGreen : Color.LightGray;

            // Friendly Fire (Buttons)
            btn_FriendlyFireEnabled.BackColor = theInstance.gameOptionFF ? Color.LightGreen : Color.LightGray;
            btn_ShowFriendlyTags.BackColor = theInstance.gameOptionFriendlyTags ? Color.LightGreen : Color.LightGray;
            btn_WarnOnFFKill.BackColor = theInstance.gameOptionFFWarn ? Color.LightGreen : Color.LightGray;
            num_maxFFKills.Value = theInstance.gameFriendlyFireKills;

            // Role Restrictions (Buttons)
            btn_RolesCQB.BackColor = theInstance.roleCQB ? Color.LightGreen : Color.LightGray;
            btn_RolesGunner.BackColor = theInstance.roleGunner ? Color.LightGreen : Color.LightGray;
            btn_RolesMedic.BackColor = theInstance.roleMedic ? Color.LightGreen : Color.LightGray;
            btn_RolesSniper.BackColor = theInstance.roleSniper ? Color.LightGreen : Color.LightGray;

            // Weapon Restrictions (Buttons) - 3-state logic
            btnWeapon01.BackColor = theInstance.weaponColt45 ? Color.LightGreen : (theInstance.limitedWeaponColt45 ? Color.Gold : Color.LightGray);
            btnWeapon02.BackColor = theInstance.weaponM9Beretta ? Color.LightGreen : (theInstance.limitedWeaponM9Beretta ? Color.Gold : Color.LightGray);
            btnWeapon11.BackColor = theInstance.weaponCar15 ? Color.LightGreen : (theInstance.limitedWeaponCar15 ? Color.Gold : Color.LightGray);
            btnWeapon12.BackColor = theInstance.weaponCar15203 ? Color.LightGreen : (theInstance.limitedWeaponCar15203 ? Color.Gold : Color.LightGray);
            btnWeapon15.BackColor = theInstance.weaponM16 ? Color.LightGreen : (theInstance.limitedWeaponM16 ? Color.Gold : Color.LightGray);
            btnWeapon16.BackColor = theInstance.weaponM16203 ? Color.LightGreen : (theInstance.limitedWeaponM16203 ? Color.Gold : Color.LightGray);
            btnWeapon13.BackColor = theInstance.weaponG3 ? Color.LightGreen : (theInstance.limitedWeaponG3 ? Color.Gold : Color.LightGray);
            btnWeapon14.BackColor = theInstance.weaponG36 ? Color.LightGreen : (theInstance.limitedWeaponG36 ? Color.Gold : Color.LightGray);
            btnWeapon10.BackColor = theInstance.weaponSAW ? Color.LightGreen : (theInstance.limitedWeaponSAW ? Color.Gold : Color.LightGray);
            btnWeapon17.BackColor = theInstance.weaponM240 ? Color.LightGreen : (theInstance.limitedWeaponM240 ? Color.Gold : Color.LightGray);
            btnWeapon18.BackColor = theInstance.weaponM60 ? Color.LightGreen : (theInstance.limitedWeaponM60 ? Color.Gold : Color.LightGray);
            btnWeapon19.BackColor = theInstance.weaponMP5 ? Color.LightGreen : (theInstance.limitedWeaponMP5 ? Color.Gold : Color.LightGray);
            btnWeapon23.BackColor = theInstance.weaponMCRT300 ? Color.LightGreen : (theInstance.limitedWeaponMCRT300 ? Color.Gold : Color.LightGray);
            btnWeapon21.BackColor = theInstance.weaponM21 ? Color.LightGreen : (theInstance.limitedWeaponM21 ? Color.Gold : Color.LightGray);
            btnWeapon22.BackColor = theInstance.weaponM24 ? Color.LightGreen : (theInstance.limitedWeaponM24 ? Color.Gold : Color.LightGray);
            btnWeapon20.BackColor = theInstance.weaponBarrett ? Color.LightGreen : (theInstance.limitedWeaponBarrett ? Color.Gold : Color.LightGray);
            btnWeapon24.BackColor = theInstance.weaponPSG1 ? Color.LightGreen : (theInstance.limitedWeaponPSG1 ? Color.Gold : Color.LightGray);
            btnWeapon03.BackColor = theInstance.weaponShotgun ? Color.LightGreen : (theInstance.limitedWeaponShotgun ? Color.Gold : Color.LightGray);
            btnWeapon07.BackColor = theInstance.weaponFragGrenade ? Color.LightGreen : (theInstance.limitedWeaponFragGrenade ? Color.Gold : Color.LightGray);
            btnWeapon09.BackColor = theInstance.weaponSmokeGrenade ? Color.LightGreen : (theInstance.limitedWeaponSmokeGrenade ? Color.Gold : Color.LightGray);
            btnWeapon08.BackColor = theInstance.weaponSatchelCharges ? Color.LightGreen : (theInstance.limitedWeaponSatchelCharges ? Color.Gold : Color.LightGray);
            btnWeapon04.BackColor = theInstance.weaponAT4 ? Color.LightGreen : (theInstance.limitedWeaponAT4 ? Color.Gold : Color.LightGray);
            btnWeapon06.BackColor = theInstance.weaponFlashGrenade ? Color.LightGreen : (theInstance.limitedWeaponFlashGrenade ? Color.Gold : Color.LightGray);
            btnWeapon05.BackColor = theInstance.weaponClaymore ? Color.LightGreen : (theInstance.limitedWeaponClaymore ? Color.Gold : Color.LightGray);
        }
        finally
        {
            // Always re-enable change tracking
            _suppressChangeTracking = false;
            _updatingButtons = false;
        }
    }

    // ================================================================================
    // EXPORT/IMPORT OPERATIONS
    // ================================================================================

    private async void BtnExport_Click(object? sender, EventArgs e)
    {
        try
        {
            // Get settings from server
            btn_ExportSettings.Enabled = false;
            btn_ExportSettings.Text = "EXPORTING";

            var exportResponse = await ApiCore.ApiClient!.GamePlay.ExportGamePlaySettingsAsync();

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

    private async void BtnLoad_Click(object? sender, EventArgs e)
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

            // Read the JSON data
            string jsonData = await File.ReadAllTextAsync(openFileDialog.FileName);

            // Disable UI during load
            btn_LoadSettings.Enabled = false;
            btn_LoadSettings.Text = "LOADING";

            // Send to server for import
            var result = await ApiCore.ApiClient!.GamePlay.ImportGamePlaySettingsAsync(jsonData);

            if (result.Success)
            {
                MessageBox.Show(
                    "Gameplay settings imported successfully!\n\nThe settings have been applied to the server.",
                    "Import Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Reload from server to show new settings
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

    private async void BtnServerControl_Click(object? sender, EventArgs e)
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
                result = await ApiCore.ApiClient!.GamePlay.StartServerAsync();
            }
            else
            {
                // Stop the server
                result = await ApiCore.ApiClient!.GamePlay.StopServerAsync();
            }

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
