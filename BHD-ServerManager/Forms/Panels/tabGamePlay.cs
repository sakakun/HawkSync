using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.GameManagement;
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
using System.Windows.Forms;
using Windows.Storage;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabGamePlay : UserControl
    {
        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;

        // --- Class Variables ---
        private new string Name = "ServerTab";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        private bool _updatingWeaponCheckboxes = false;             // Prevent recursion
        private List<CheckBox> weaponCheckboxes = new();            // List of weapon checkboxes for select all/none functionality

        public tabGamePlay()
        {
            InitializeComponent();
            functionEvent_InitializeWeaponCheckboxes();             // Initialize Weapon checkboxes
        }
        // --- Form Functions ---
        // --- Get Server Settings --- Allow to be triggered externally
        public void methodFunction_loadSettings()
        {
            // Lobby Passwords
            tb_bluePassword.Text = theInstance!.gamePasswordBlue = ServerSettings.Get("gamePasswordBlue", string.Empty);
            tb_redPassword.Text = theInstance!.gamePasswordRed = ServerSettings.Get("gamePasswordRed", string.Empty);

            // Match Win Conditions
            num_scoresKOTH.Value = theInstance!.gameScoreZoneTime = (int)ServerSettings.Get("gameScoreZoneTime", (decimal) 10);
			num_scoresDM.Value = theInstance!.gameScoreKills = (int)ServerSettings.Get("gameScoreKills", (decimal) 200);
			num_scoresFB.Value = theInstance!.gameScoreFlags = (int)ServerSettings.Get("gameScoreFlags", (decimal) 10);

			// Server Values (Right)
			num_gameTimeLimit.Value = theInstance!.gameTimeLimit = (int)ServerSettings.Get("gameTimeLimit", (decimal) 22);
            cb_replayMaps.SelectedIndex = theInstance!.gameLoopMaps = ServerSettings.Get("gameLoopMaps", 0);
			num_gameStartDelay.Value = theInstance!.gameStartDelay = (int)ServerSettings.Get("gameStartDelay", (decimal) 1);
			num_respawnTime.Value = theInstance!.gameRespawnTime = (int)ServerSettings.Get("gameRespawnTime", (decimal) 20);
			num_scoreBoardDelay.Value = theInstance!.gameScoreBoardDelay = (int)ServerSettings.Get("gameScoreBoardDelay", (decimal) 20);
			num_maxPlayers.Value = theInstance!.gameMaxSlots = (int)ServerSettings.Get("gameMaxSlots", (decimal) 50);
            num_pspTakeoverTimer.Value = theInstance!.gamePSPTOTimer = (int)ServerSettings.Get("gamePSPTOTimer", (decimal) 20);
            num_flagReturnTime.Value = theInstance!.gameFlagReturnTime = (int)ServerSettings.Get("gameFlagReturnTime", (decimal) 210);
            num_maxTeamLives.Value = theInstance!.gameMaxTeamLives = (int)ServerSettings.Get("gameMaxTeamLives", (decimal) 100);

            // Server Options (Left Checkboxes)
            cb_autoBalance.Checked = theInstance!.gameOptionAutoBalance = ServerSettings.Get("gameOptionAutoBalance", true);
            cb_customSkins.Checked = theInstance!.gameCustomSkins = ServerSettings.Get("gameCustomSkins", true);
            cb_enableDistroyBuildings.Checked = theInstance!.gameDestroyBuildings = ServerSettings.Get("gameDestroyBuildings", true);
            cb_enableFatBullets.Checked = theInstance!.gameFatBullets = ServerSettings.Get("gameFatBullets", false);
            cb_enableOneShotKills.Checked = theInstance!.gameOneShotKills = ServerSettings.Get("gameOneShotKills", false);
            cb_enableLeftLean.Checked = theInstance!.gameAllowLeftLeaning = ServerSettings.Get("gameAllowLeftLeaning", true);
            cb_showTracers.Checked = theInstance!.gameOptionShowTracers = ServerSettings.Get("gameOptionShowTracers", false);
            cb_showClays.Checked = theInstance!.gameShowTeamClays = ServerSettings.Get("gameShowTeamClays", true);
            cb_autoRange.Checked = theInstance!.gameOptionAutoRange = ServerSettings.Get("gameOptionAutoRange", false);
            
            // Friendly Fire Killings
            cb_enableFFkills.Checked = theInstance!.gameOptionFF = ServerSettings.Get("gameOptionFF", true);
            num_maxFFKills.Value = theInstance!.gameFriendlyFireKills = (int)ServerSettings.Get("gameFriendlyFireKills", (decimal) 10); 
            cb_warnFFkils.Checked = theInstance!.gameOptionFFWarn = ServerSettings.Get("gameOptionFFWarn", false);
            cb_showTeamTags.Checked = theInstance!.gameOptionFriendlyTags = ServerSettings.Get("gameOptionFriendlyTags", false);

            // Role Restrictions
            cb_roleCQB.Checked = theInstance!.roleCQB = ServerSettings.Get("roleCQB", true);
            cb_roleGunner.Checked = theInstance!.roleGunner = ServerSettings.Get("roleGunner", true);
            cb_roleSniper.Checked = theInstance!.roleSniper = ServerSettings.Get("roleSniper", true);
            cb_roleMedic.Checked = theInstance!.roleMedic = ServerSettings.Get("roleMedic", true);

            // Weapon Restrictions
            cb_weapColt45.Checked = theInstance!.weaponColt45 = ServerSettings.Get("weaponColt45", true);
            cb_weapM9Bereatta.Checked = theInstance!.weaponM9Beretta = ServerSettings.Get("weaponM9Beretta", true);
            cb_weapCAR15.Checked = theInstance!.weaponCar15 = ServerSettings.Get("weaponCar15", true);
            cb_weapCAR15203.Checked = theInstance!.weaponCar15203 = ServerSettings.Get("weaponCar15203", true);
            cb_weapM16.Checked = theInstance!.weaponM16 = ServerSettings.Get("weaponM16", true);
            cb_weapM16203.Checked = theInstance!.weaponM16203 = ServerSettings.Get("weaponM16203", true);
            cb_weapG3.Checked = theInstance!.weaponG3 = ServerSettings.Get("weaponG3", true);
            cb_weapG36.Checked = theInstance!.weaponG36 = ServerSettings.Get("weaponG36", true);
            cb_weapM60.Checked = theInstance!.weaponM60 = ServerSettings.Get("weaponM60", true);
            cb_weapM240.Checked = theInstance!.weaponM240 = ServerSettings.Get("weaponM240", true);
            cb_weapMP5.Checked = theInstance!.weaponMP5 = ServerSettings.Get("weaponMP5", true);
            cb_weapSaw.Checked = theInstance!.weaponSAW = ServerSettings.Get("weaponSAW", true);
            cb_weap300Tact.Checked = theInstance!.weaponMCRT300 = ServerSettings.Get("weaponMCRT300", true);
            cb_weapM21.Checked = theInstance!.weaponM21 = ServerSettings.Get("weaponM21", true);
            cb_weapM24.Checked = theInstance!.weaponM24 = ServerSettings.Get("weaponM24", true);
            cb_weapBarret.Checked = theInstance!.weaponBarrett = ServerSettings.Get("weaponBarrett", true);
            cb_weapPSG1.Checked = theInstance!.weaponPSG1 = ServerSettings.Get("weaponPSG1", true);
            cb_weapShotgun.Checked = theInstance!.weaponShotgun = ServerSettings.Get("weaponShotgun", true);
            cb_weapFragGrenade.Checked = theInstance!.weaponFragGrenade = ServerSettings.Get("weaponFragGrenade", true);
            cb_weapSmokeGrenade.Checked = theInstance!.weaponSmokeGrenade = ServerSettings.Get("weaponSmokeGrenade", true);
            cb_weapSatchel.Checked = theInstance!.weaponSatchelCharges = ServerSettings.Get("weaponSatchelCharges", true);
            cb_weapAT4.Checked = theInstance!.weaponAT4 = ServerSettings.Get("weaponAT4", true);
            cb_weapFlashBang.Checked = theInstance!.weaponFlashGrenade = ServerSettings.Get("weaponFlashGrenade", true);
            cb_weapClay.Checked = theInstance!.weaponClaymore = ServerSettings.Get("weaponClaymore", true);

            checkBox_selectAll.Checked = ServerSettings.Get("checkBox_selectAll", true);
            checkBox_selectNone.Checked = ServerSettings.Get("checkBox_selectNone", false);

        }
        public void methodFunction_saveSettings()
        {
            // Lobby Passwords
            ServerSettings.Set("gamePasswordBlue", theInstance!.gamePasswordBlue = tb_bluePassword.Text);
            ServerSettings.Set("gamePasswordRed", theInstance!.gamePasswordRed = tb_redPassword.Text);

            // Match Win Conditions
            ServerSettings.Set("gameScoreZoneTimer", (decimal)(theInstance!.gameScoreZoneTime = (int)num_scoresKOTH.Value));
            ServerSettings.Set("gameScoreKills", (decimal)(theInstance!.gameScoreKills = (int)num_scoresDM.Value));
            ServerSettings.Set("gameScoreFlags", (decimal)(theInstance!.gameScoreFlags = (int)num_scoresFB.Value));

            // Server Values (Right)
            ServerSettings.Set("gameTimeLimit", (decimal)(theInstance!.gameTimeLimit = (int)num_gameTimeLimit.Value));
            ServerSettings.Set("gameLoopMaps", theInstance!.gameLoopMaps = cb_replayMaps.SelectedIndex);
            ServerSettings.Set("gameStartDelay", (decimal)(theInstance!.gameStartDelay = (int)num_gameStartDelay.Value));
            ServerSettings.Set("gameRespawnTime", (decimal)(theInstance!.gameRespawnTime = (int)num_respawnTime.Value));
            ServerSettings.Set("gameScoreBoardDelay", (decimal)(theInstance!.gameScoreBoardDelay = (int)num_scoreBoardDelay.Value));
            ServerSettings.Set("gameMaxSlots", (decimal)(theInstance!.gameMaxSlots = (int)num_maxPlayers.Value));
            ServerSettings.Set("gamePSPTOTimer", (decimal)(theInstance!.gamePSPTOTimer = (int)num_pspTakeoverTimer.Value));
            ServerSettings.Set("gameFlagReturnTime", (decimal)(theInstance!.gameFlagReturnTime = (int)num_flagReturnTime.Value));
            ServerSettings.Set("gameMaxTeamLives", (decimal)(theInstance!.gameMaxTeamLives = (int)num_maxTeamLives.Value));

                        // Server Options (Left Checkboxes)
            ServerSettings.Set("gameOptionAutoBalance", theInstance!.gameOptionAutoBalance = cb_autoBalance.Checked);
            ServerSettings.Set("gameOptionShowTracers", theInstance!.gameOptionShowTracers = cb_showTracers.Checked);
            ServerSettings.Set("gameShowTeamClays", theInstance!.gameShowTeamClays = cb_showClays.Checked);
            ServerSettings.Set("gameOptionAutoRange", theInstance!.gameOptionAutoRange = cb_autoRange.Checked);
            ServerSettings.Set("gameCustomSkins", theInstance!.gameCustomSkins = cb_customSkins.Checked);
            ServerSettings.Set("gameDestroyBuildings", theInstance!.gameDestroyBuildings = cb_enableDistroyBuildings.Checked);
            ServerSettings.Set("gameFatBullets", theInstance!.gameFatBullets = cb_enableFatBullets.Checked);
            ServerSettings.Set("gameOneShotKills", theInstance!.gameOneShotKills = cb_enableOneShotKills.Checked);
            ServerSettings.Set("gameAllowLeftLeaning", theInstance!.gameAllowLeftLeaning = cb_enableLeftLean.Checked);

            // Friendly Fire Killings
            ServerSettings.Set("gameOptionFF", theInstance!.gameOptionFF = cb_enableFFkills.Checked);
            ServerSettings.Set("gameFriendlyFireKills", (decimal)(theInstance!.gameFriendlyFireKills = (int)num_maxFFKills.Value));
            ServerSettings.Set("gameOptionFFWarn", theInstance!.gameOptionFFWarn = cb_warnFFkils.Checked);
            ServerSettings.Set("gameOptionFriendlyTags", theInstance!.gameOptionFriendlyTags = cb_showTeamTags.Checked);

            // Role Restrictions
            ServerSettings.Set("roleCQB", theInstance!.roleCQB = cb_roleCQB.Checked);
            ServerSettings.Set("roleGunner", theInstance!.roleGunner = cb_roleGunner.Checked);
            ServerSettings.Set("roleSniper", theInstance!.roleSniper = cb_roleSniper.Checked);
            ServerSettings.Set("roleMedic", theInstance!.roleMedic = cb_roleMedic.Checked);

            // Weapon Restrictions
            ServerSettings.Set("weaponColt45", theInstance!.weaponColt45 = cb_weapColt45.Checked);
            ServerSettings.Set("weaponM9Beretta", theInstance!.weaponM9Beretta = cb_weapM9Bereatta.Checked);
            ServerSettings.Set("weaponCar15", theInstance!.weaponCar15 = cb_weapCAR15.Checked);
            ServerSettings.Set("weaponCar15203", theInstance!.weaponCar15203 = cb_weapCAR15203.Checked);
            ServerSettings.Set("weaponM16", theInstance!.weaponM16 = cb_weapM16.Checked);
            ServerSettings.Set("weaponM16203", theInstance!.weaponM16203 = cb_weapM16203.Checked);
            ServerSettings.Set("weaponG3", theInstance!.weaponG3 = cb_weapG3.Checked);
            ServerSettings.Set("weaponG36", theInstance!.weaponG36 = cb_weapG36.Checked);
            ServerSettings.Set("weaponM60", theInstance!.weaponM60 = cb_weapM60.Checked);
            ServerSettings.Set("weaponM240", theInstance!.weaponM240 = cb_weapM240.Checked);
            ServerSettings.Set("weaponMP5", theInstance!.weaponMP5 = cb_weapMP5.Checked);
            ServerSettings.Set("weaponSAW", theInstance!.weaponSAW = cb_weapSaw.Checked);
            ServerSettings.Set("weaponMCRT300", theInstance!.weaponMCRT300 = cb_weap300Tact.Checked);
            ServerSettings.Set("weaponM21", theInstance!.weaponM21 = cb_weapM21.Checked);
            ServerSettings.Set("weaponM24", theInstance!.weaponM24 = cb_weapM24.Checked);
            ServerSettings.Set("weaponBarrett", theInstance!.weaponBarrett = cb_weapBarret.Checked);
            ServerSettings.Set("weaponPSG1", theInstance!.weaponPSG1 = cb_weapPSG1.Checked);
            ServerSettings.Set("weaponShotgun", theInstance!.weaponShotgun = cb_weapShotgun.Checked);
            ServerSettings.Set("weaponFragGrenade", theInstance!.weaponFragGrenade = cb_weapFragGrenade.Checked);
            ServerSettings.Set("weaponSmokeGrenade", theInstance!.weaponSmokeGrenade = cb_weapSmokeGrenade.Checked);
            ServerSettings.Set("weaponSatchelCharges", theInstance!.weaponSatchelCharges = cb_weapSatchel.Checked);
            ServerSettings.Set("weaponAT4", theInstance!.weaponAT4 = cb_weapAT4.Checked);
            ServerSettings.Set("weaponFlashGrenade", theInstance!.weaponFlashGrenade = cb_weapFlashBang.Checked);
            ServerSettings.Set("weaponClaymore", theInstance!.weaponClaymore = cb_weapClay.Checked);

            ServerSettings.Set("checkBox_selectAll", checkBox_selectAll.Checked);
            ServerSettings.Set("checkBox_selectNone", checkBox_selectNone.Checked);

        }
        // --- Save Server Settings --- Allow to be triggered externally
        
        // --- Weapon Checkbox Logic ---
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
        private void functionEvent_UpdateServerControls()
        {
            // Is the Server Running?
            bool isOffline = (theInstance!.instanceStatus == InstanceStatus.OFFLINE);

            // Server Running? Update Text
            btn_serverControl.Text = isOffline ? "START" : "STOP";
            // Lock Down Settings that shouldn't be changed while the server is running

            // Update Visibility of Controls
            btn_ServerUpdate.Visible = !isOffline;          // Show the update button only when the server is running
            btn_LockLobby.Visible = !isOffline;             // Show the lock lobby button only when the server is running
        }
        // --- Ticker Server Hook --- Allow to be triggered externally by the Server Manager Ticker
        public void tickerServerHook()
        {
            // Check if the first load is complete
            if (!_firstLoadComplete)
            {
                // Set the first load complete flag to true
                _firstLoadComplete = true;
                // Get the server settings on first load
                methodFunction_loadSettings();
            }
            // Do stuff here that needs to be done every tick
            functionEvent_UpdateServerControls();                              // Update the Server Control Button State
        }
        //  --- Action Click Events ---
        //  --- Weapon Checkbox Changed ---
        private void actionClick_WeaponCheckedChanged(object sender, EventArgs e)
        {
            // Log the checkbox change event
            AppDebug.Log(this.Name, "Weapon checkbox changed: " + (sender as CheckBox)?.Name);

            // Prevent recursion and unnecessary updates
            if (_updatingWeaponCheckboxes) return;
            _updatingWeaponCheckboxes = true;

            AppDebug.Log(this.Name, "Updating weapon checkboxes...");

            // Update the instance based on the checkbox state
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
            else if (weaponCheckboxes.Contains(sender))
            {
                checkBox_selectAll.Checked = weaponCheckboxes.All(cb => cb.Checked);
                checkBox_selectNone.Checked = weaponCheckboxes.All(cb => !cb.Checked);
            }

            AppDebug.Log(this.Name, "Weapon checkboxes updated successfully.");

            // Allow Updating of Weapons Again
            _updatingWeaponCheckboxes = false;
        }
        // --- Save Server Settings Button Clicked ---
        private void actionClick_SaveServerSettings(object sender, EventArgs e)
        {
            methodFunction_saveSettings();
        }
        // --- Reset Server Settings Button Clicked ---
        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            methodFunction_loadSettings();
        }
        // --- Import/Export Server Settings ---
        private void actionClick_ImportServerSettings(object sender, EventArgs e) => theInstanceManager.ImportSettings();
        private void actionClick_ExportServerSettings(object sender, EventArgs e) => theInstanceManager.ExportSettings();
        // --- Server Control Button Clicked ---
        private void actionClick_serverControl(object sender, EventArgs e)
        {
            if (theInstanceManager.ValidateGameServerPath() && theInstance!.instanceStatus == InstanceStatus.OFFLINE)
            {
                if (StartServer.startGame())
                {
                    ServerMemory.ReadMemoryServerStatus();
                    functionEvent_UpdateServerControls();
                }
            }
            else
            {
                StartServer.stopGame();
                functionEvent_UpdateServerControls();
            }

        }
        // --- Update Game Server Settings ---
        private void actionClick_GameServerUpdate(object sender, EventArgs e)
        {
            if (ServerMemory.ReadMemoryIsProcessAttached())
            {
                theInstanceManager.UpdateGameServer();
                MessageBox.Show("Saved settings have been applied to the game server.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // --- Server Lock Lobby ----
        private void actionClick_ServerLockLobby(object sender, EventArgs e)
        {
            ServerMemory.WriteMemorySendConsoleCommand("lockgame");
        }
    }
}
