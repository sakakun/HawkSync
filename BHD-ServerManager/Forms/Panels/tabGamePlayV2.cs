using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
	public partial class tabGamePlayV2 : UserControl
	{
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;

        // --- Class Variables ---
        private new string Name = "GamePlayTabV2";
        private List<Button> weaponButtons = new();
        private List<Button> roleButtons = new();

        public tabGamePlayV2()
        {
            InitializeComponent();

            // Load Settings
            LoadSettings();
            InitializeWeaponButtons();
            InitializeRoleButtons();

            // Start a ticker to update the profile tab every 1 second
            CommonCore.Ticker?.Start("GamePlayTabV2Ticker", 1000, Ticker_GamePlayTab);
            WireEvents();
        }

        /// <summary>
        /// Ticker hook for server updates
        /// </summary>
        public void Ticker_GamePlayTab()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Ticker_GamePlayTab));
                return;
            }
            UpdateServerControls();
        }

        // --- Form Functions ---

        public void LoadSettings()
        {
            var result = theInstanceManager.LoadGamePlaySettings();
            if (!result.Success)
            {
                AppDebug.Log(Name, $"Failed to load gameplay settings: {result.Message}");
                MessageBox.Show($"Failed to load gameplay settings: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateUIFromInstance();
        }

        public void ExportSettings()
        {
            try
            {
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = $"GamePlaySettings_{DateTime.Now:yyyyMMdd_HHmmss}.json"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var result = theInstanceManager.ExportGamePlaySettings(saveFileDialog.FileName);
                    if (result.Success)
                    {
                        MessageBox.Show($"Gameplay settings exported successfully to:\n{saveFileDialog.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to export gameplay settings:\n\n{result.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(Name, $"Error exporting settings: {ex.Message}");
                MessageBox.Show($"Failed to export settings: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ImportSettings()
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
                    var (success, settings, errorMessage) = theInstanceManager.ImportGamePlaySettings(openFileDialog.FileName);
                    if (!success || settings == null)
                    {
                        MessageBox.Show($"Failed to import gameplay settings:\n\n{errorMessage}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ApplyGamePlaySettingsToUI(settings);
                    MessageBox.Show("Gameplay settings imported successfully!\n\nClick 'Save Server Settings' to apply these settings.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void UpdateUIFromInstance()
        {
            if (theInstance == null) return;
            var settings = gamePlayInstanceManager.BuildGamePlaySettingsFromInstance(theInstance);
            ApplyGamePlaySettingsToUI(settings);
        }

        private void ApplyGamePlaySettingsToUI(GamePlaySettings settings)
        {
            tb_bluePassword.Text = settings.BluePassword;
            tb_redPassword.Text = settings.RedPassword;
            num_scoresKOTH.Value = settings.ScoreKOTH;
            num_scoresDM.Value = settings.ScoreDM;
            num_scoresFB.Value = settings.ScoreFB;
            num_gameTimeLimit.Value = settings.TimeLimit;
            cb_replayMaps.SelectedIndex = settings.LoopMaps;
            num_gameStartDelay.Value = settings.StartDelay;
            num_respawnTime.Value = settings.RespawnTime;
            num_scoreBoardDelay.Value = settings.ScoreBoardDelay;
            num_maxPlayers.Value = settings.MaxSlots;
            num_pspTakeoverTimer.Value = settings.PSPTakeoverTimer;
            num_flagReturnTime.Value = settings.FlagReturnTime;
            num_maxTeamLives.Value = settings.MaxTeamLives;
            num_FullWeaponThreshold.Value = settings.FullWeaponThreshold;
            // Server Options
            SetOptionButtons(settings.Options);
            // Friendly Fire
            SetFriendlyFireButtons(settings.FriendlyFire);
            // Role Restrictions
            SetRoleButtons(settings.Roles);
            // Weapon Restrictions
            SetWeaponButtons(settings.Weapons, settings.RestrictedWeapons);
        }

        private void SetOptionButtons(ServerOptions options)
        {
            btn_AutoBalance.BackColor = options.AutoBalance ? Color.LightGreen : Color.LightGray;
            btn_CustomSkins.BackColor = options.CustomSkins ? Color.LightGreen : Color.LightGray;
            btn_AllowLeftLeaning.BackColor = options.AllowLeftLeaning ? Color.LightGreen : Color.LightGray;
            btn_AllowRightLeaning.BackColor = options.AllowRightLeaning ? Color.LightGreen : Color.LightGray;
            btn_AutoRange.BackColor = options.AutoRange ? Color.LightGreen : Color.LightGray;
            btn_DestroyBuildings.BackColor = options.DestroyBuildings ? Color.LightGreen : Color.LightGray;
            btn_FatBullets.BackColor = options.FatBullets ? Color.LightGreen : Color.LightGray;
            btn_OneShotKills.BackColor = options.OneShotKills ? Color.LightGreen : Color.LightGray;
            btn_ShowTracers.BackColor = options.ShowTracers ? Color.LightGreen : Color.LightGray;
            btn_ShowClays.BackColor = options.ShowClays ? Color.LightGreen : Color.LightGray;
        }

        private void SetFriendlyFireButtons(FriendlyFireSettings ff)
        {
            btn_FriendlyFireEnabled.BackColor = ff.Enabled ? Color.LightGreen : Color.LightGray;
            btn_ShowFriendlyTags.BackColor = ff.ShowFriendlyTags ? Color.LightGreen : Color.LightGray;
            btn_WarnOnFFKill.BackColor = ff.WarnOnKill ? Color.LightGreen : Color.LightGray;
            num_maxFFKills.Value = ff.MaxKills;
        }

        private void SetRoleButtons(RoleRestrictions roles)
        {
            btn_RolesCQB.BackColor = roles.CQB ? Color.LightGreen : Color.LightGray;
            btn_RolesGunner.BackColor = roles.Gunner ? Color.LightGreen : Color.LightGray;
            btn_RolesMedic.BackColor = roles.Medic ? Color.LightGreen : Color.LightGray;
            btn_RolesSniper.BackColor = roles.Sniper ? Color.LightGreen : Color.LightGray;
        }

        private void SetWeaponButtons(WeaponEnablement weapons, RestrictedWeapons restrictedWeapons)
        {
            // Map each weapon to its button with 3-state logic
            // Gold = Always enabled (bypass threshold), Green = Enabled with threshold, Gray = Disabled
            // Check restricted first: if true, show Gold; else check weapon enabled for Green; else Gray
            btnWeapon01.BackColor = restrictedWeapons.Colt45 ? Color.Gold : (weapons.Colt45 ? Color.LightGreen : Color.LightGray);
            btnWeapon02.BackColor = restrictedWeapons.M9Beretta ? Color.Gold : (weapons.M9Beretta ? Color.LightGreen : Color.LightGray);
            btnWeapon11.BackColor = restrictedWeapons.CAR15 ? Color.Gold : (weapons.CAR15 ? Color.LightGreen : Color.LightGray);
            btnWeapon12.BackColor = restrictedWeapons.CAR15203 ? Color.Gold : (weapons.CAR15203 ? Color.LightGreen : Color.LightGray);
            btnWeapon15.BackColor = restrictedWeapons.M16 ? Color.Gold : (weapons.M16 ? Color.LightGreen : Color.LightGray);
            btnWeapon16.BackColor = restrictedWeapons.M16203 ? Color.Gold : (weapons.M16203 ? Color.LightGreen : Color.LightGray);
            btnWeapon13.BackColor = restrictedWeapons.G3 ? Color.Gold : (weapons.G3 ? Color.LightGreen : Color.LightGray);
            btnWeapon14.BackColor = restrictedWeapons.G36 ? Color.Gold : (weapons.G36 ? Color.LightGreen : Color.LightGray);
            btnWeapon10.BackColor = restrictedWeapons.SAW ? Color.Gold : (weapons.SAW ? Color.LightGreen : Color.LightGray);
            btnWeapon17.BackColor = restrictedWeapons.M240 ? Color.Gold : (weapons.M240 ? Color.LightGreen : Color.LightGray);
            btnWeapon18.BackColor = restrictedWeapons.M60 ? Color.Gold : (weapons.M60 ? Color.LightGreen : Color.LightGray);
            btnWeapon19.BackColor = restrictedWeapons.MP5 ? Color.Gold : (weapons.MP5 ? Color.LightGreen : Color.LightGray);
            btnWeapon23.BackColor = restrictedWeapons.MCRT300 ? Color.Gold : (weapons.MCRT300 ? Color.LightGreen : Color.LightGray);
            btnWeapon21.BackColor = restrictedWeapons.M21 ? Color.Gold : (weapons.M21 ? Color.LightGreen : Color.LightGray);
            btnWeapon22.BackColor = restrictedWeapons.M24 ? Color.Gold : (weapons.M24 ? Color.LightGreen : Color.LightGray);
            btnWeapon20.BackColor = restrictedWeapons.Barrett ? Color.Gold : (weapons.Barrett ? Color.LightGreen : Color.LightGray);
            btnWeapon24.BackColor = restrictedWeapons.PSG1 ? Color.Gold : (weapons.PSG1 ? Color.LightGreen : Color.LightGray);
            btnWeapon03.BackColor = restrictedWeapons.Shotgun ? Color.Gold : (weapons.Shotgun ? Color.LightGreen : Color.LightGray);
            btnWeapon07.BackColor = restrictedWeapons.FragGrenade ? Color.Gold : (weapons.FragGrenade ? Color.LightGreen : Color.LightGray);
            btnWeapon09.BackColor = restrictedWeapons.SmokeGrenade ? Color.Gold : (weapons.SmokeGrenade ? Color.LightGreen : Color.LightGray);
            btnWeapon08.BackColor = restrictedWeapons.Satchel ? Color.Gold : (weapons.Satchel ? Color.LightGreen : Color.LightGray);
            btnWeapon04.BackColor = restrictedWeapons.AT4 ? Color.Gold : (weapons.AT4 ? Color.LightGreen : Color.LightGray);
            btnWeapon06.BackColor = restrictedWeapons.FlashBang ? Color.Gold : (weapons.FlashBang ? Color.LightGreen : Color.LightGray);
            btnWeapon05.BackColor = restrictedWeapons.Claymore ? Color.Gold : (weapons.Claymore ? Color.LightGreen : Color.LightGray);
        }

        private void InitializeWeaponButtons()
        {
            weaponButtons = new()
            {
                btnWeapon01, btnWeapon02, btnWeapon11, btnWeapon12, btnWeapon15, btnWeapon16,
                btnWeapon13, btnWeapon14, btnWeapon10, btnWeapon17, btnWeapon18, btnWeapon19, btnWeapon23,
                btnWeapon21, btnWeapon22, btnWeapon20, btnWeapon24, btnWeapon03, btnWeapon07,
                btnWeapon09, btnWeapon08, btnWeapon04, btnWeapon06, btnWeapon05
            };

            // Enable right-click context menu for weapon buttons
            foreach (var btn in weaponButtons)
            {
                btn.MouseDown += WeaponButton_MouseDown;
            }
        }

        private void InitializeRoleButtons()
        {
            roleButtons = new() { btn_RolesCQB, btn_RolesGunner, btn_RolesMedic, btn_RolesSniper };
        }

        private void WireEvents()
        {
            // Weapon select all/none
            btnWeaponSA.Click += actionClick_WeaponSelectAll;
            btnWeaponSN.Click += actionClick_WeaponSelectNone;
            // Weapon buttons
            foreach (var btn in weaponButtons)
                btn.Click += actionClick_WeaponButtonChanged;
            // Role buttons
            foreach (var btn in roleButtons)
                btn.Click += actionClick_RoleButtonChanged;
            // Option buttons
            btn_AutoBalance.Click += actionClick_OptionButtonChanged;
            btn_CustomSkins.Click += actionClick_OptionButtonChanged;
            btn_AllowLeftLeaning.Click += actionClick_OptionButtonChanged;
            btn_AllowRightLeaning.Click += actionClick_OptionButtonChanged;
            btn_AutoRange.Click += actionClick_OptionButtonChanged;
            btn_DestroyBuildings.Click += actionClick_OptionButtonChanged;
            btn_FatBullets.Click += actionClick_OptionButtonChanged;
            btn_OneShotKills.Click += actionClick_OptionButtonChanged;
            btn_ShowTracers.Click += actionClick_OptionButtonChanged;
            btn_ShowClays.Click += actionClick_OptionButtonChanged;
            // Friendly fire buttons
            btn_FriendlyFireEnabled.Click += actionClick_FriendlyFireButtonChanged;
            btn_ShowFriendlyTags.Click += actionClick_FriendlyFireButtonChanged;
            btn_WarnOnFFKill.Click += actionClick_FriendlyFireButtonChanged;
        }

        public void UpdateServerControls()
        {
            bool isOffline = (theInstance?.instanceStatus == InstanceStatus.OFFLINE);
            btn_serverControl.Text = isOffline ? "START" : "STOP";
            btn_ServerUpdate.Visible = !isOffline;
            btn_LockLobby.Visible = !isOffline;
        }

        // --- Action Click Events ---

        private void actionClick_WeaponSelectAll(object? sender, EventArgs e)
        {
            var result = theInstanceManager.EnableAllWeapons();
            if (result.Success)
            {
                weaponButtons.ForEach(btn => btn.BackColor = Color.LightGreen);
            }
        }

        private void actionClick_WeaponSelectNone(object? sender, EventArgs e)
        {
            var result = theInstanceManager.DisableAllWeapons();
            if (result.Success)
            {
                weaponButtons.ForEach(btn => btn.BackColor = Color.LightGray);
            }
        }

        private void actionClick_WeaponButtonChanged(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            
            // Left click: Toggle between Disabled (Gray) and Threshold-based (Green)
            // Gray (Disabled) ↔ Green (Available when threshold met)
            if (btn.BackColor == Color.LightGray)
                btn.BackColor = Color.LightGreen;
            else if (btn.BackColor == Color.LightGreen)
                btn.BackColor = Color.LightGray;
            else if (btn.BackColor == Color.Gold)
                btn.BackColor = Color.LightGray; // If Gold, left-click resets to Gray
        }

        private void WeaponButton_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var btn = sender as Button;
                if (btn == null) return;

                // Right click: Cycle through enabled states
                // Gray (weapon disabled) → Green (weapon enabled with threshold)
                // Green (threshold-based) → Gold (always enabled, bypass threshold)
                // Gold (always enabled) → Green (back to threshold-based)
                // This enforces: restrictedWeapon* can only be true when weapon* is true
                if (btn.BackColor == Color.LightGray)
                {
                    // Gray → Green: Enable weapon first (cannot jump to Gold from disabled state)
                    btn.BackColor = Color.LightGreen;
                }
                else if (btn.BackColor == Color.LightGreen)
                {
                    // Green → Gold: Add restricted override
                    btn.BackColor = Color.Gold;
                }
                else if (btn.BackColor == Color.Gold)
                {
                    // Gold → Green: Remove restricted override, keep weapon enabled
                    btn.BackColor = Color.LightGreen;
                }
            }
        }

        private void actionClick_RoleButtonChanged(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            btn.BackColor = btn.BackColor == Color.LightGreen ? Color.LightGray : Color.LightGreen;
        }

        private void actionClick_OptionButtonChanged(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            btn.BackColor = btn.BackColor == Color.LightGreen ? Color.LightGray : Color.LightGreen;
        }

        private void actionClick_FriendlyFireButtonChanged(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            btn.BackColor = btn.BackColor == Color.LightGreen ? Color.LightGray : Color.LightGreen;
        }

        private void actionClick_SaveServerSettings(object? sender, EventArgs e)
        {
            var settings = BuildGamePlaySettingsFromUI();
            var result = theInstanceManager.SaveGamePlaySettings(settings);
            if (result.Success)
            {
                MessageBox.Show("Gameplay settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(Name, "Gameplay settings saved successfully");
            }
            else
            {
                MessageBox.Show($"Failed to save gameplay settings:\n\n{result.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppDebug.Log(Name, $"Failed to save gameplay settings: {result.Message}");
            }
        }

        private void actionClick_ResetSettings(object? sender, EventArgs e)
        {
            LoadSettings();
        }

        private void actionClick_ExportSettings(object? sender, EventArgs e)
        {
            ExportSettings();
        }

        private void actionClick_ImportSettings(object? sender, EventArgs e)
        {
            ImportSettings();
        }

        private void actionClick_serverControl(object? sender, EventArgs e)
        {
            if (theInstanceManager.ValidateGameServerPath() && theInstance?.instanceStatus == InstanceStatus.OFFLINE)
            {
                if (StartServer.startGame())
                {
                    ServerMemory.ReadMemoryServerStatus();
                }
            }
            else
            {
                StartServer.stopGame();
            }
        }

        private void actionClick_GameServerUpdate(object? sender, EventArgs e)
        {
            if (CommonCore.theInstance!.instanceStatus != InstanceStatus.OFFLINE)
            {
                var settings = BuildGamePlaySettingsFromUI();
                var result = theInstanceManager.SaveGamePlaySettings(settings);
                if (result.Success)
                {
                    theInstanceManager.UpdateGameServer();
                    MessageBox.Show("Gameplay settings saved and updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppDebug.Log(Name, "Gameplay settings saved and updated successfully.");
                }
                else
                {
                    MessageBox.Show($"Failed to save gameplay settings:\n\n{result.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AppDebug.Log(Name, $"Failed to save gameplay settings: {result.Message}");
                }
            }
        }

        private void actionClick_ServerLockLobby(object? sender, EventArgs e)
        {
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");
        }

        private GamePlaySettings BuildGamePlaySettingsFromUI()
        {
            var options = new ServerOptions(
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

            var friendlyFire = new FriendlyFireSettings(
                Enabled: btn_FriendlyFireEnabled.BackColor == Color.LightGreen,
                MaxKills: (int)num_maxFFKills.Value,
                WarnOnKill: btn_WarnOnFFKill.BackColor == Color.LightGreen,
                ShowFriendlyTags: btn_ShowFriendlyTags.BackColor == Color.LightGreen
            );

            var roles = new RoleRestrictions(
                CQB: btn_RolesCQB.BackColor == Color.LightGreen,
                Gunner: btn_RolesGunner.BackColor == Color.LightGreen,
                Sniper: btn_RolesSniper.BackColor == Color.LightGreen,
                Medic: btn_RolesMedic.BackColor == Color.LightGreen
            );

            var weapons = new WeaponEnablement(
                Colt45: btnWeapon01.BackColor == Color.LightGreen || btnWeapon01.BackColor == Color.Gold,
                M9Beretta: btnWeapon02.BackColor == Color.LightGreen || btnWeapon02.BackColor == Color.Gold,
                CAR15: btnWeapon11.BackColor == Color.LightGreen || btnWeapon11.BackColor == Color.Gold,
                CAR15203: btnWeapon12.BackColor == Color.LightGreen || btnWeapon12.BackColor == Color.Gold,
                M16: btnWeapon15.BackColor == Color.LightGreen || btnWeapon15.BackColor == Color.Gold,
                M16203: btnWeapon16.BackColor == Color.LightGreen || btnWeapon16.BackColor == Color.Gold,
                G3: btnWeapon13.BackColor == Color.LightGreen || btnWeapon13.BackColor == Color.Gold,
                G36: btnWeapon14.BackColor == Color.LightGreen || btnWeapon14.BackColor == Color.Gold,
                M60: btnWeapon18.BackColor == Color.LightGreen || btnWeapon18.BackColor == Color.Gold,
                M240: btnWeapon17.BackColor == Color.LightGreen || btnWeapon17.BackColor == Color.Gold,
                MP5: btnWeapon19.BackColor == Color.LightGreen || btnWeapon19.BackColor == Color.Gold,
                SAW: btnWeapon10.BackColor == Color.LightGreen || btnWeapon10.BackColor == Color.Gold,
                MCRT300: btnWeapon23.BackColor == Color.LightGreen || btnWeapon23.BackColor == Color.Gold,
                M21: btnWeapon21.BackColor == Color.LightGreen || btnWeapon21.BackColor == Color.Gold,
                M24: btnWeapon22.BackColor == Color.LightGreen || btnWeapon22.BackColor == Color.Gold,
                Barrett: btnWeapon20.BackColor == Color.LightGreen || btnWeapon20.BackColor == Color.Gold,
                PSG1: btnWeapon24.BackColor == Color.LightGreen || btnWeapon24.BackColor == Color.Gold,
                Shotgun: btnWeapon03.BackColor == Color.LightGreen || btnWeapon03.BackColor == Color.Gold,
                FragGrenade: btnWeapon07.BackColor == Color.LightGreen || btnWeapon07.BackColor == Color.Gold,
                SmokeGrenade: btnWeapon09.BackColor == Color.LightGreen || btnWeapon09.BackColor == Color.Gold,
                Satchel: btnWeapon08.BackColor == Color.LightGreen || btnWeapon08.BackColor == Color.Gold,
                AT4: btnWeapon04.BackColor == Color.LightGreen || btnWeapon04.BackColor == Color.Gold,
                FlashBang: btnWeapon06.BackColor == Color.LightGreen || btnWeapon06.BackColor == Color.Gold,
                Claymore: btnWeapon05.BackColor == Color.LightGreen || btnWeapon05.BackColor == Color.Gold
            );

            var restrictedWeapons = new RestrictedWeapons (
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

            return new GamePlaySettings(
                BluePassword: tb_bluePassword.Text,
                RedPassword: tb_redPassword.Text,
                ScoreKOTH: (int)num_scoresKOTH.Value,
                ScoreDM: (int)num_scoresDM.Value,
                ScoreFB: (int)num_scoresFB.Value,
                TimeLimit: (int)num_gameTimeLimit.Value,
                LoopMaps: cb_replayMaps.SelectedIndex,
                StartDelay: (int)num_gameStartDelay.Value,
                RespawnTime: (int)num_respawnTime.Value,
                ScoreBoardDelay: (int)num_scoreBoardDelay.Value,
                MaxSlots: (int)num_maxPlayers.Value,
                PSPTakeoverTimer: (int)num_pspTakeoverTimer.Value,
                FlagReturnTime: (int)num_flagReturnTime.Value,
                MaxTeamLives: (int)num_maxTeamLives.Value,
                FullWeaponThreshold: (int)num_FullWeaponThreshold.Value,
                Options: options,
                FriendlyFire: friendlyFire,
                Roles: roles,
                Weapons: weapons,
                RestrictedWeapons: restrictedWeapons
            );
        }


    }
}
