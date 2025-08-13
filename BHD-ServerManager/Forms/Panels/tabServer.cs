using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabServer : UserControl
    {
        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "ServerTab";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        private bool _updatingWeaponCheckboxes = false;             // Prevent recursion
        private List<CheckBox> weaponCheckboxes = new();            // List of weapon checkboxes for select all/none functionality

        public tabServer()
        {
            InitializeComponent();
            functionEvent_InitializeWeaponCheckboxes();             // Initialize Weapon checkboxes
        }
        // --- Form Functions ---
        // --- Get Server Settings --- Allow to be triggered externally
        public void functionEvent_GetServerSettings(theInstance updatedInstance = null!)
        {
            // Log the action
            AppDebug.Log(this.Name, "Getting server settings...");
            // If updateInstance is null, use the current instance
            var newInstance = updatedInstance != null ? updatedInstance : theInstance;

            // Host Information (4 fields)
            {
                tb_serverID.Text = newInstance.WebStatsProfileID;
                tb_serverName.Text = newInstance.gameServerName;
                tb_hostName.Text = newInstance.gameHostName;
                tb_serverMessage.Text = newInstance.gameMOTD;
            }
            // Server Details (6 fields)
            {
                if (!string.IsNullOrEmpty(newInstance.profileBindIP) &&
                    cb_serverIP.Items.Contains(newInstance.profileBindIP))
                {
                    cb_serverIP.SelectedItem = newInstance.profileBindIP; // Select the item in cb_serverIP that matches the value of newInstance.profileBindIP,
                }
                else
                {
                    cb_serverIP.SelectedIndex = 0; // or default to the first item (assumed "0.0.0.0") if not found
                }
                num_serverPort.Value = newInstance.profileBindPort;
                tb_serverID.Text = newInstance.gamePasswordLobby;
                cb_serverDedicated.Checked = newInstance.gameDedicated;
                cb_novaRequired.Checked = newInstance.gameRequireNova;
                cb_sessionType.SelectedIndex = newInstance.gameSessionType;
            }
            // Server Options (4 fields)
            {
                num_gameTimeLimit.Value = newInstance.gameTimeLimit;
                cb_replayMaps.SelectedIndex = newInstance.gameLoopMaps;
                num_gameStartDelay.Value = newInstance.gameStartDelay;
                num_respawnTime.Value = newInstance.gameRespawnTime;
                num_scoreBoardDelay.Value = newInstance.gameScoreBoardDelay;
                num_maxPlayers.Value = newInstance.gameMaxSlots;
            }
            // Scoring Options (3 fields)
            {
                num_scoresKOTH.Value = newInstance.gameScoreZoneTime;
                num_scoresDM.Value = newInstance.gameScoreKills;
                num_scoresFB.Value = newInstance.gameScoreFlags;
            }
            // Team Options (3 fields)
            {
                cb_autoBalance.Checked = newInstance.gameOptionAutoBalance;
                tb_bluePassword.Text = newInstance.gamePasswordBlue;
                tb_redPassword.Text = newInstance.gamePasswordRed;
            }
            // Game Play Settings (6 fields)
            {
                cb_showTracers.Checked = newInstance.gameOptionShowTracers;
                cb_showClays.Checked = newInstance.gameShowTeamClays;
                cb_autoRange.Checked = newInstance.gameOptionAutoRange;
                num_pspTakeoverTimer.Value = newInstance.gamePSPTOTimer;
                num_flagReturnTime.Value = newInstance.gameFlagReturnTime;
                num_maxTeamLives.Value = newInstance.gameMaxTeamLives;
            }
            // Friendly Fire (4 fields)
            {
                cb_enableFFkills.Checked = newInstance.gameOptionFF;
                num_maxFFKills.Enabled = newInstance.gameOptionFF ? true : false;
                num_maxFFKills.Value = newInstance.gameFriendlyFireKills;
                cb_warnFFkils.Checked = newInstance.gameOptionFFWarn;
                cb_showTeamTags.Checked = newInstance.gameOptionFriendlyTags;
            }
            // Ping Checking (4 fields)
            {
                cb_enableMinCheck.Checked = newInstance.gameMinPing;
                cb_enableMaxCheck.Checked = newInstance.gameMaxPing;
                num_minPing.Value = newInstance.gameMinPingValue;
                num_maxPing.Value = newInstance.gameMaxPingValue;
                num_minPing.Enabled = newInstance.gameMinPing ? true : false;
                num_maxPing.Enabled = newInstance.gameMaxPing ? true : false;
            }
            // Misc (4 fields)
            {
                cb_customSkins.Checked = newInstance.gameCustomSkins;
                cb_enableDistroyBuildings.Checked = newInstance.gameDestroyBuildings;
                cb_enableFatBullets.Checked = newInstance.gameFatBullets;
                cb_enableOneShotKills.Checked = newInstance.gameOneShotKills;
                cb_enableLeftLean.Checked = newInstance.gameAllowLeftLeaning;
            }
            // Restrictions Weapons (Many Fields)
            {
                cb_weapColt45.Checked = newInstance.weaponColt45;
                cb_weapM9Bereatta.Checked = newInstance.weaponM9Beretta;
                cb_weapCAR15.Checked = newInstance.weaponCar15;
                cb_weapCAR15203.Checked = newInstance.weaponCar15203;
                cb_weapM16.Checked = newInstance.weaponM16;
                cb_weapM16203.Checked = newInstance.weaponM16203;
                cb_weapG3.Checked = newInstance.weaponG3;
                cb_weapG36.Checked = newInstance.weaponG36;
                cb_weapM60.Checked = newInstance.weaponM60;
                cb_weapM240.Checked = newInstance.weaponM240;
                cb_weapMP5.Checked = newInstance.weaponMP5;
                cb_weapSaw.Checked = newInstance.weaponSAW;
                cb_weap300Tact.Checked = newInstance.weaponMCRT300;
                cb_weapM21.Checked = newInstance.weaponM21;
                cb_weapM24.Checked = newInstance.weaponM24;
                cb_weapBarret.Checked = newInstance.weaponBarrett;
                cb_weapPSG1.Checked = newInstance.weaponPSG1;
                cb_weapShotgun.Checked = newInstance.weaponShotgun;
                cb_weapFragGrenade.Checked = newInstance.weaponFragGrenade;
                cb_weapSmokeGrenade.Checked = newInstance.weaponSmokeGrenade;
                cb_weapSatchel.Checked = newInstance.weaponSatchelCharges;
                cb_weapAT4.Checked = newInstance.weaponAT4;
                cb_weapFlashBang.Checked = newInstance.weaponFlashGrenade;
                cb_weapClay.Checked = newInstance.weaponClaymore;

                // If the above weap are all checked, then checkBox_selectAll should be checked.
                checkBox_selectAll.Checked =
                    cb_weapColt45.Checked &&
                    cb_weapM9Bereatta.Checked &&
                    cb_weapCAR15.Checked &&
                    cb_weapCAR15203.Checked &&
                    cb_weapM16.Checked &&
                    cb_weapM16203.Checked &&
                    cb_weapG3.Checked &&
                    cb_weapG36.Checked &&
                    cb_weapM60.Checked &&
                    cb_weapM240.Checked &&
                    cb_weapMP5.Checked &&
                    cb_weapSaw.Checked &&
                    cb_weap300Tact.Checked &&
                    cb_weapM21.Checked &&
                    cb_weapM24.Checked &&
                    cb_weapBarret.Checked &&
                    cb_weapPSG1.Checked &&
                    cb_weapShotgun.Checked &&
                    cb_weapFragGrenade.Checked &&
                    cb_weapSmokeGrenade.Checked &&
                    cb_weapSatchel.Checked &&
                    cb_weapAT4.Checked &&
                    cb_weapFlashBang.Checked &&
                    cb_weapClay.Checked;

                // If the above weap are all unchecked, then checkBox_selectNone should be checked.
                checkBox_selectNone.Checked =
                    !cb_weapColt45.Checked &&
                    !cb_weapM9Bereatta.Checked &&
                    !cb_weapCAR15.Checked &&
                    !cb_weapCAR15203.Checked &&
                    !cb_weapM16.Checked &&
                    !cb_weapM16203.Checked &&
                    !cb_weapG3.Checked &&
                    !cb_weapG36.Checked &&
                    !cb_weapM60.Checked &&
                    !cb_weapM240.Checked &&
                    !cb_weapMP5.Checked &&
                    !cb_weapSaw.Checked &&
                    !cb_weap300Tact.Checked &&
                    !cb_weapM21.Checked &&
                    !cb_weapM24.Checked &&
                    !cb_weapBarret.Checked &&
                    !cb_weapPSG1.Checked &&
                    !cb_weapShotgun.Checked &&
                    !cb_weapFragGrenade.Checked &&
                    !cb_weapSmokeGrenade.Checked &&
                    !cb_weapSatchel.Checked &&
                    !cb_weapAT4.Checked &&
                    !cb_weapFlashBang.Checked &&
                    !cb_weapClay.Checked;
            }
            // Role Restrictions (4 fields
            {
                cb_roleCQB.Checked = newInstance.roleCQB;
                cb_roleGunner.Checked = newInstance.roleGunner;
                cb_roleSniper.Checked = newInstance.roleSniper;
                cb_roleMedic.Checked = newInstance.roleMedic;
            }
            // Remote Settings
            {
                cb_enableRemote.Checked = newInstance.profileEnableRemote;
                num_remotePort.Value = (int)newInstance.profileRemotePort;
                num_remotePort.Enabled = newInstance.profileEnableRemote;
            }

        }
        // --- Save Server Settings --- Allow to be triggered externally
        public void functionEvent_SaveServerSettings()
        {
            // Log the action
            AppDebug.Log(this.Name, "Saving server settings...");

            // Host Information (4 fields)
            theInstance.WebStatsProfileID = tb_serverID.Text;
            theInstance.gameServerName = tb_serverName.Text;
            theInstance.gameHostName = tb_hostName.Text;
            theInstance.gameMOTD = tb_serverMessage.Text;

            // Server Details (6 fields)
            theInstance.profileBindIP = cb_serverIP.SelectedItem?.ToString() ?? "0.0.0.0";
            theInstance.profileBindPort = (int)num_serverPort.Value;
            theInstance.gamePasswordLobby = tb_serverID.Text;
            theInstance.gameDedicated = cb_serverDedicated.Checked;
            theInstance.gameRequireNova = cb_novaRequired.Checked;
            theInstance.gameSessionType = cb_sessionType.SelectedIndex;

            // Server Options (6 fields)
            theInstance.gameTimeLimit = (int)num_gameTimeLimit.Value;
            theInstance.gameLoopMaps = cb_replayMaps.SelectedIndex;
            theInstance.gameStartDelay = (int)num_gameStartDelay.Value;
            theInstance.gameRespawnTime = (int)num_respawnTime.Value;
            theInstance.gameScoreBoardDelay = (int)num_scoreBoardDelay.Value;
            theInstance.gameMaxSlots = (int)num_maxPlayers.Value;

            // Scoring Options (3 fields)
            theInstance.gameScoreZoneTime = (int)num_scoresKOTH.Value;
            theInstance.gameScoreKills = (int)num_scoresDM.Value;
            theInstance.gameScoreFlags = (int)num_scoresFB.Value;

            // Team Options (3 fields)
            theInstance.gameOptionAutoBalance = cb_autoBalance.Checked;
            theInstance.gamePasswordBlue = tb_bluePassword.Text;
            theInstance.gamePasswordRed = tb_redPassword.Text;

            // Game Play Settings (6 fields)
            theInstance.gameOptionShowTracers = cb_showTracers.Checked;
            theInstance.gameShowTeamClays = cb_showClays.Checked;
            theInstance.gameOptionAutoRange = cb_autoRange.Checked;
            theInstance.gamePSPTOTimer = (int)num_pspTakeoverTimer.Value;
            theInstance.gameFlagReturnTime = (int)num_flagReturnTime.Value;
            theInstance.gameMaxTeamLives = (int)num_maxTeamLives.Value;

            // Friendly Fire (4 fields)
            theInstance.gameOptionFF = cb_enableFFkills.Checked;
            theInstance.gameFriendlyFireKills = (int)num_maxFFKills.Value;
            theInstance.gameOptionFFWarn = cb_warnFFkils.Checked;
            theInstance.gameOptionFriendlyTags = cb_showTeamTags.Checked;

            // Ping Checking (4 fields)
            theInstance.gameMinPing = cb_enableMinCheck.Checked;
            theInstance.gameMaxPing = cb_enableMaxCheck.Checked;
            theInstance.gameMinPingValue = (int)num_minPing.Value;
            theInstance.gameMaxPingValue = (int)num_maxPing.Value;

            // Misc (5 fields)
            theInstance.gameCustomSkins = cb_customSkins.Checked;
            theInstance.gameDestroyBuildings = cb_enableDistroyBuildings.Checked;
            theInstance.gameFatBullets = cb_enableFatBullets.Checked;
            theInstance.gameOneShotKills = cb_enableOneShotKills.Checked;
            theInstance.gameAllowLeftLeaning = cb_enableLeftLean.Checked;

            // Restrictions Weapons (Many Fields)
            theInstance.weaponColt45 = cb_weapColt45.Checked;
            theInstance.weaponM9Beretta = cb_weapM9Bereatta.Checked;
            theInstance.weaponCar15 = cb_weapCAR15.Checked;
            theInstance.weaponCar15203 = cb_weapCAR15203.Checked;
            theInstance.weaponM16 = cb_weapM16.Checked;
            theInstance.weaponM16203 = cb_weapM16203.Checked;
            theInstance.weaponG3 = cb_weapG3.Checked;
            theInstance.weaponG36 = cb_weapG36.Checked;
            theInstance.weaponM60 = cb_weapM60.Checked;
            theInstance.weaponM240 = cb_weapM240.Checked;
            theInstance.weaponMP5 = cb_weapMP5.Checked;
            theInstance.weaponSAW = cb_weapSaw.Checked;
            theInstance.weaponMCRT300 = cb_weap300Tact.Checked;
            theInstance.weaponM21 = cb_weapM21.Checked;
            theInstance.weaponM24 = cb_weapM24.Checked;
            theInstance.weaponBarrett = cb_weapBarret.Checked;
            theInstance.weaponPSG1 = cb_weapPSG1.Checked;
            theInstance.weaponShotgun = cb_weapShotgun.Checked;
            theInstance.weaponFragGrenade = cb_weapFragGrenade.Checked;
            theInstance.weaponSmokeGrenade = cb_weapSmokeGrenade.Checked;
            theInstance.weaponSatchelCharges = cb_weapSatchel.Checked;
            theInstance.weaponAT4 = cb_weapAT4.Checked;
            theInstance.weaponFlashGrenade = cb_weapFlashBang.Checked;
            theInstance.weaponClaymore = cb_weapClay.Checked;

            // Role Restrictions (4 fields)
            theInstance.roleCQB = cb_roleCQB.Checked;
            theInstance.roleGunner = cb_roleGunner.Checked;
            theInstance.roleSniper = cb_roleSniper.Checked;
            theInstance.roleMedic = cb_roleMedic.Checked;

            // Remote Settings
            theInstance.profileEnableRemote = cb_enableRemote.Checked;
            theInstance.profileRemotePort = (int)num_remotePort.Value;

            AppDebug.Log(this.Name, "Server settings saved.");
        }
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
        // --- Ticker Server Hook --- Allow to be triggered externally by the Server Manager Ticker
        public void tickerServerHook()
        {
            // Check if the first load is complete
            if (!_firstLoadComplete)
            {
                // Set the first load complete flag to true
                _firstLoadComplete = true;
                // Get the server settings on first load
                functionEvent_GetServerSettings();
            }
            // Do stuff here that needs to be done every tick
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
            functionEvent_SaveServerSettings();
        }

        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            functionEvent_GetServerSettings();
        }
    }
}
