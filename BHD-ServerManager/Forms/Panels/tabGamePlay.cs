using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabGamePlay : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;

        // --- Class Variables ---
        private new string Name = "GamePlayTab";
        private bool _firstLoadComplete = false;
        private bool _updatingWeaponCheckboxes = false;
        private List<CheckBox> weaponCheckboxes = new();

        public tabGamePlay()
        {
            InitializeComponent();
            functionEvent_InitializeWeaponCheckboxes();
            methodFunction_loadSettings();
        }

        // --- Form Functions ---

        /// <summary>
        /// Load gameplay settings via manager
        /// </summary>
        public void methodFunction_loadSettings()
        {
            // Load via manager
            var result = theInstanceManager.LoadGamePlaySettings();

            if (!result.Success)
            {
                AppDebug.Log(Name, $"Failed to load gameplay settings: {result.Message}");
                MessageBox.Show($"Failed to load gameplay settings: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update UI from instance (manager already updated instance)
            UpdateUIFromInstance();
        }

        /// <summary>
        /// Save gameplay settings via manager
        /// </summary>
        public void methodFunction_saveSettings()
        {
            // Build settings from UI
            var settings = BuildGamePlaySettingsFromUI();

            // Save via manager (includes validation)
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

        /// <summary>
        /// Export gameplay settings to JSON
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

        /// <summary>
        /// Import gameplay settings from JSON
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
                    var (success, settings, errorMessage) = theInstanceManager.ImportGamePlaySettings(openFileDialog.FileName);

                    if (!success || settings == null)
                    {
                        MessageBox.Show($"Failed to import gameplay settings:\n\n{errorMessage}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update UI from imported settings
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

        /// <summary>
        /// Update UI controls from theInstance
        /// </summary>
        private void UpdateUIFromInstance()
        {
            // Lobby Passwords
            tb_bluePassword.Text = theInstance!.gamePasswordBlue;
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

        /// <summary>
        /// Build GamePlaySettings DTO from UI controls
        /// </summary>
        private GamePlaySettings BuildGamePlaySettingsFromUI()
        {
            var options = new ServerOptions(
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

            var friendlyFire = new FriendlyFireSettings(
                Enabled: cb_enableFFkills.Checked,
                MaxKills: (int)num_maxFFKills.Value,
                WarnOnKill: cb_warnFFkils.Checked,
                ShowFriendlyTags: cb_showTeamTags.Checked
            );

            var roles = new RoleRestrictions(
                CQB: cb_roleCQB.Checked,
                Gunner: cb_roleGunner.Checked,
                Sniper: cb_roleSniper.Checked,
                Medic: cb_roleMedic.Checked
            );

            var weapons = new WeaponRestrictions(
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
                Options: options,
                FriendlyFire: friendlyFire,
                Roles: roles,
                Weapons: weapons
            );
        }

        /// <summary>
        /// Apply GamePlaySettings DTO to UI controls
        /// </summary>
        private void ApplyGamePlaySettingsToUI(GamePlaySettings settings)
        {
            // Lobby Passwords
            tb_bluePassword.Text = settings.BluePassword;
            tb_redPassword.Text = settings.RedPassword;

            // Match Win Conditions
            num_scoresKOTH.Value = settings.ScoreKOTH;
            num_scoresDM.Value = settings.ScoreDM;
            num_scoresFB.Value = settings.ScoreFB;

            // Server Values
            num_gameTimeLimit.Value = settings.TimeLimit;
            cb_replayMaps.SelectedIndex = settings.LoopMaps;
            num_gameStartDelay.Value = settings.StartDelay;
            num_respawnTime.Value = settings.RespawnTime;
            num_scoreBoardDelay.Value = settings.ScoreBoardDelay;
            num_maxPlayers.Value = settings.MaxSlots;
            num_pspTakeoverTimer.Value = settings.PSPTakeoverTimer;
            num_flagReturnTime.Value = settings.FlagReturnTime;
            num_maxTeamLives.Value = settings.MaxTeamLives;

            // Server Options
            cb_autoBalance.Checked = settings.Options.AutoBalance;
            cb_showTracers.Checked = settings.Options.ShowTracers;
            cb_showClays.Checked = settings.Options.ShowClays;
            cb_autoRange.Checked = settings.Options.AutoRange;
            cb_customSkins.Checked = settings.Options.CustomSkins;
            cb_enableDistroyBuildings.Checked = settings.Options.DestroyBuildings;
            cb_enableFatBullets.Checked = settings.Options.FatBullets;
            cb_enableOneShotKills.Checked = settings.Options.OneShotKills;
            cb_enableLeftLean.Checked = settings.Options.AllowLeftLeaning;

            // Friendly Fire
            cb_enableFFkills.Checked = settings.FriendlyFire.Enabled;
            num_maxFFKills.Value = settings.FriendlyFire.MaxKills;
            cb_warnFFkils.Checked = settings.FriendlyFire.WarnOnKill;
            cb_showTeamTags.Checked = settings.FriendlyFire.ShowFriendlyTags;

            // Role Restrictions
            cb_roleCQB.Checked = settings.Roles.CQB;
            cb_roleGunner.Checked = settings.Roles.Gunner;
            cb_roleSniper.Checked = settings.Roles.Sniper;
            cb_roleMedic.Checked = settings.Roles.Medic;

            // Weapon Restrictions
            cb_weapColt45.Checked = settings.Weapons.Colt45;
            cb_weapM9Bereatta.Checked = settings.Weapons.M9Beretta;
            cb_weapCAR15.Checked = settings.Weapons.CAR15;
            cb_weapCAR15203.Checked = settings.Weapons.CAR15203;
            cb_weapM16.Checked = settings.Weapons.M16;
            cb_weapM16203.Checked = settings.Weapons.M16203;
            cb_weapG3.Checked = settings.Weapons.G3;
            cb_weapG36.Checked = settings.Weapons.G36;
            cb_weapM60.Checked = settings.Weapons.M60;
            cb_weapM240.Checked = settings.Weapons.M240;
            cb_weapMP5.Checked = settings.Weapons.MP5;
            cb_weapSaw.Checked = settings.Weapons.SAW;
            cb_weap300Tact.Checked = settings.Weapons.MCRT300;
            cb_weapM21.Checked = settings.Weapons.M21;
            cb_weapM24.Checked = settings.Weapons.M24;
            cb_weapBarret.Checked = settings.Weapons.Barrett;
            cb_weapPSG1.Checked = settings.Weapons.PSG1;
            cb_weapShotgun.Checked = settings.Weapons.Shotgun;
            cb_weapFragGrenade.Checked = settings.Weapons.FragGrenade;
            cb_weapSmokeGrenade.Checked = settings.Weapons.SmokeGrenade;
            cb_weapSatchel.Checked = settings.Weapons.Satchel;
            cb_weapAT4.Checked = settings.Weapons.AT4;
            cb_weapFlashBang.Checked = settings.Weapons.FlashBang;
            cb_weapClay.Checked = settings.Weapons.Claymore;

            // Update select all/none checkboxes
            UpdateWeaponSelectCheckboxes();
        }

        // --- Weapon Checkbox Logic ---

        /// <summary>
        /// Initialize weapon checkbox list
        /// </summary>
        private void functionEvent_InitializeWeaponCheckboxes()
        {
            weaponCheckboxes = new()
            {
                cb_weapColt45, cb_weapM9Bereatta, cb_weapCAR15, cb_weapCAR15203, cb_weapM16, cb_weapM16203,
                cb_weapG3, cb_weapG36, cb_weapM60, cb_weapM240, cb_weapMP5, cb_weapSaw, cb_weap300Tact,
                cb_weapM21, cb_weapM24, cb_weapBarret, cb_weapPSG1, cb_weapShotgun, cb_weapFragGrenade,
                cb_weapSmokeGrenade, cb_weapSatchel, cb_weapAT4, cb_weapFlashBang, cb_weapClay
            };
        }

        /// <summary>
        /// Update select all/none checkbox states based on weapon checkboxes
        /// </summary>
        private void UpdateWeaponSelectCheckboxes()
        {
            if (_updatingWeaponCheckboxes) return;

            _updatingWeaponCheckboxes = true;

            checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
            checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);

            _updatingWeaponCheckboxes = false;
        }

        /// <summary>
        /// Update server control button states
        /// </summary>
        private void functionEvent_UpdateServerControls()
        {
            bool isOffline = (theInstance!.instanceStatus == InstanceStatus.OFFLINE);

            btn_serverControl.Text = isOffline ? "START" : "STOP";
            btn_ServerUpdate.Visible = !isOffline;
            btn_LockLobby.Visible = !isOffline;
        }

        /// <summary>
        /// Ticker hook for server updates
        /// </summary>
        public void tickerServerHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
            }

            functionEvent_UpdateServerControls();
        }

        // --- Action Click Events ---

        /// <summary>
        /// Weapon checkbox changed
        /// </summary>
        private void actionClick_WeaponCheckedChanged(object sender, EventArgs e)
        {
            if (_updatingWeaponCheckboxes) return;

            _updatingWeaponCheckboxes = true;

            if (sender == checkBox_selectAll && checkBox_selectAll.Checked)
            {
                // Enable all weapons via manager
                var result = theInstanceManager.EnableAllWeapons();
                if (result.Success)
                {
                    weaponCheckboxes.ForEach(cb => cb.Checked = true);
                    checkBox_selectNone.Checked = false;
                }
            }
            else if (sender == checkBox_selectNone && checkBox_selectNone.Checked)
            {
                // Disable all weapons via manager
                var result = theInstanceManager.DisableAllWeapons();
                if (result.Success)
                {
                    weaponCheckboxes.ForEach(cb => cb.Checked = false);
                    checkBox_selectAll.Checked = false;
                }
            }
            else if (weaponCheckboxes.Contains(sender))
            {
                // Individual weapon changed - update select all/none
                checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
                checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);
            }

            _updatingWeaponCheckboxes = false;
        }

        /// <summary>
        /// Save server settings button clicked
        /// </summary>
        private void actionClick_SaveServerSettings(object sender, EventArgs e)
        {
            methodFunction_saveSettings();
        }

        /// <summary>
        /// Reset server settings button clicked
        /// </summary>
        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            methodFunction_loadSettings();
        }

        /// <summary>
        /// Export server settings button clicked
        /// </summary>
        private void actionClick_ExportSettings(object sender, EventArgs e)
        {
            methodFunction_exportSettings();
        }

        /// <summary>
        /// Import server settings button clicked
        /// </summary>
        private void actionClick_ImportSettings(object sender, EventArgs e)
        {
            methodFunction_importSettings();
        }

        /// <summary>
        /// Server control button clicked
        /// </summary>
        private void actionClick_serverControl(object sender, EventArgs e)
        {
            if (theInstanceManager.ValidateGameServerPath() && theInstance!.instanceStatus == InstanceStatus.OFFLINE)
            {
                if (StartServer.startGame())
                {
                    ServerMemory.ReadMemoryServerStatus();
                    functionEvent_UpdateServerControls();
                    Program.ServerManagerUI!.MapsTab.methodFunction_UpdateMapControls();
                }
            }
            else
            {
                StartServer.stopGame();
                functionEvent_UpdateServerControls();
            }
        }

        /// <summary>
        /// Update game server settings
        /// </summary>
        private void actionClick_GameServerUpdate(object sender, EventArgs e)
        {
            if (ServerMemory.ReadMemoryIsProcessAttached())
            {
                methodFunction_saveSettings();
                theInstanceManager.UpdateGameServer();
                MessageBox.Show("Saved settings have been applied to the game server.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Server lock lobby
        /// </summary>
        private void actionClick_ServerLockLobby(object sender, EventArgs e)
        {
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");
        }
    }
}