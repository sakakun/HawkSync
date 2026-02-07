using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabGamePlay : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;

        // --- Class Variables ---
        private new string Name = "GamePlayTab";
        private bool _updatingWeaponCheckboxes = false;
        private List<CheckBox> weaponCheckboxes = new();

        public tabGamePlay()
        {
            InitializeComponent();

            // Load Settings
            LoadSettings();
            InitializeWeaponCheckboxes();

            // Start a ticker to update the profile tab every 1 second
            CommonCore.Ticker?.Start("GamePlayTabTicker", 1000, Ticker_GamePlayTab);
            
        }

        /// <summary>
        /// Ticker hook for server updates
        /// </summary>
        public void Ticker_GamePlayTab()
        {
            if(InvokeRequired)
            {
                Invoke(new Action(Ticker_GamePlayTab));
                return;
            }

            UpdateServerControls();
        }

        // --- Form Functions ---

        /// <summary>
        /// Load gameplay settings via manager
        /// </summary>
        public void LoadSettings()
        {
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
        /// Export gameplay settings to JSON
        /// </summary>
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

        /// <summary>
        /// Import gameplay settings from JSON
        /// </summary>
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
            if (theInstance == null) return;
            var settings = gamePlayInstanceManager.BuildGamePlaySettingsFromInstance(theInstance);
            ApplyGamePlaySettingsToUI(settings);
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
            SetWeaponCheckboxes(settings.Weapons);

            // Update select all/none checkboxes
            UpdateWeaponSelectCheckboxes();
        }

        /// <summary>
        /// Helper to set all weapon checkboxes from WeaponRestrictions
        /// </summary>
        private void SetWeaponCheckboxes(WeaponRestrictions weapons)
        {
            cb_weapColt45.Checked = weapons.Colt45;
            cb_weapM9Bereatta.Checked = weapons.M9Beretta;
            cb_weapCAR15.Checked = weapons.CAR15;
            cb_weapCAR15203.Checked = weapons.CAR15203;
            cb_weapM16.Checked = weapons.M16;
            cb_weapM16203.Checked = weapons.M16203;
            cb_weapG3.Checked = weapons.G3;
            cb_weapG36.Checked = weapons.G36;
            cb_weapM60.Checked = weapons.M60;
            cb_weapM240.Checked = weapons.M240;
            cb_weapMP5.Checked = weapons.MP5;
            cb_weapSaw.Checked = weapons.SAW;
            cb_weap300Tact.Checked = weapons.MCRT300;
            cb_weapM21.Checked = weapons.M21;
            cb_weapM24.Checked = weapons.M24;
            cb_weapBarret.Checked = weapons.Barrett;
            cb_weapPSG1.Checked = weapons.PSG1;
            cb_weapShotgun.Checked = weapons.Shotgun;
            cb_weapFragGrenade.Checked = weapons.FragGrenade;
            cb_weapSmokeGrenade.Checked = weapons.SmokeGrenade;
            cb_weapSatchel.Checked = weapons.Satchel;
            cb_weapAT4.Checked = weapons.AT4;
            cb_weapFlashBang.Checked = weapons.FlashBang;
            cb_weapClay.Checked = weapons.Claymore;
        }

        /// <summary>
        /// Initialize weapon checkbox list
        /// </summary>
        private void InitializeWeaponCheckboxes()
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
        public void UpdateServerControls()
        {
            bool isOffline = (theInstance?.instanceStatus == InstanceStatus.OFFLINE);

            btn_serverControl.Text = isOffline ? "START" : "STOP";
            btn_ServerUpdate.Visible = !isOffline;
            btn_LockLobby.Visible = !isOffline;
        }

        // --- Action Click Events ---

        private void actionClick_WeaponCheckedChanged(object sender, EventArgs e)
        {
            if (_updatingWeaponCheckboxes) return;

            _updatingWeaponCheckboxes = true;

            if (sender == checkBox_selectAll && checkBox_selectAll.Checked)
            {
                var result = theInstanceManager.EnableAllWeapons();
                if (result.Success)
                {
                    weaponCheckboxes.ForEach(cb => cb.Checked = true);
                    checkBox_selectNone.Checked = false;
                }
            }
            else if (sender == checkBox_selectNone && checkBox_selectNone.Checked)
            {
                var result = theInstanceManager.DisableAllWeapons();
                if (result.Success)
                {
                    weaponCheckboxes.ForEach(cb => cb.Checked = false);
                    checkBox_selectAll.Checked = false;
                }
            }
            else if (weaponCheckboxes.Contains(sender))
            {
                checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
                checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);
            }

            _updatingWeaponCheckboxes = false;
        }

        private void actionClick_SaveServerSettings(object sender, EventArgs e)
        {
            var settings = gamePlayInstanceManager.BuildGamePlaySettings(this);
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

        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void actionClick_ExportSettings(object sender, EventArgs e)
        {
            ExportSettings();
        }

        private void actionClick_ImportSettings(object sender, EventArgs e)
        {
            ImportSettings();
        }

        private void actionClick_serverControl(object sender, EventArgs e)
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

        private void actionClick_GameServerUpdate(object sender, EventArgs e)
        {
            if (ServerMemory.ReadMemoryIsProcessAttached())
            {
                var settings = gamePlayInstanceManager.BuildGamePlaySettings(this);
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

        private void actionClick_ServerLockLobby(object sender, EventArgs e)
        {
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");
        }
    }
}