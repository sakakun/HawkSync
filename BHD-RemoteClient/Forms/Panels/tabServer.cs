using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
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
using Windows.Storage;

namespace BHD_RemoteClient.Forms.Panels
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
                tb_serverID.Text = newInstance.WebStatsProfileID;
                tb_serverPassword.Text = newInstance.gamePasswordLobby;        
                cb_serverDedicated.Checked = newInstance.gameDedicated;
                cb_requireNova.Checked = newInstance.gameRequireNova;
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

            theInstance updatedInfo = new theInstance(); // Reset to defaults before applying new settings

            // Host Information (4 fields)
            {
                updatedInfo.WebStatsProfileID = tb_serverID.Text;
                updatedInfo.gameServerName = tb_serverName.Text;
                updatedInfo.gameHostName = tb_hostName.Text;
                updatedInfo.gameMOTD = tb_serverMessage.Text;
            }
            // Server Details (6 fields)
            {
                updatedInfo.profileBindIP = cb_serverIP.SelectedItem?.ToString() ?? "0.0.0.0";
                updatedInfo.profileBindPort = (int)num_serverPort.Value;
                updatedInfo.gamePasswordLobby = tb_serverPassword.Text;
                updatedInfo.gameDedicated = cb_serverDedicated.Checked;
                updatedInfo.gameRequireNova = cb_requireNova.Checked;
                updatedInfo.gameSessionType = cb_sessionType.SelectedIndex;
            }
            // Server Options (6 fields)
            {
                updatedInfo.gameTimeLimit = (int)num_gameTimeLimit.Value;
                updatedInfo.gameLoopMaps = cb_replayMaps.SelectedIndex;
                updatedInfo.gameStartDelay = (int)num_gameStartDelay.Value;
                updatedInfo.gameRespawnTime = (int)num_respawnTime.Value;
                updatedInfo.gameScoreBoardDelay = (int)num_scoreBoardDelay.Value;
                updatedInfo.gameMaxSlots = (int)num_maxPlayers.Value;
            }
            // Scoring Options (3 fields)
            {
                updatedInfo.gameScoreZoneTime = (int)num_scoresKOTH.Value;
                updatedInfo.gameScoreKills = (int)num_scoresDM.Value;
                updatedInfo.gameScoreFlags = (int)num_scoresFB.Value;
            }
            // Team Options (3 fields)
            {
                updatedInfo.gameOptionAutoBalance = cb_autoBalance.Checked;
                updatedInfo.gamePasswordBlue = tb_bluePassword.Text;
                updatedInfo.gamePasswordRed = tb_redPassword.Text;
            }
            // Game Play Settings (6 fields)
            {
                updatedInfo.gameOptionShowTracers = cb_showTracers.Checked;
                updatedInfo.gameShowTeamClays = cb_showClays.Checked;
                updatedInfo.gameOptionAutoRange = cb_autoRange.Checked;
                updatedInfo.gamePSPTOTimer = (int)num_pspTakeoverTimer.Value;
                updatedInfo.gameFlagReturnTime = (int)num_flagReturnTime.Value;
                updatedInfo.gameMaxTeamLives = (int)num_maxTeamLives.Value;
            }
            // Friendly Fire (4 fields)
            {
                updatedInfo.gameOptionFF = cb_enableFFkills.Checked;
                updatedInfo.gameFriendlyFireKills = (int)num_maxFFKills.Value;
                updatedInfo.gameOptionFFWarn = cb_warnFFkils.Checked;
                updatedInfo.gameOptionFriendlyTags = cb_showTeamTags.Checked;
            }
            // Ping Checking (4 fields)
            {
                updatedInfo.gameMinPing = cb_enableMinCheck.Checked;
                updatedInfo.gameMaxPing = cb_enableMaxCheck.Checked;
                updatedInfo.gameMinPingValue = (int)num_minPing.Value;
                updatedInfo.gameMaxPingValue = (int)num_maxPing.Value;
            }
            // Misc (5 fields)
            {
                updatedInfo.gameCustomSkins = cb_customSkins.Checked;
                updatedInfo.gameDestroyBuildings = cb_enableDistroyBuildings.Checked;
                updatedInfo.gameFatBullets = cb_enableFatBullets.Checked;
                updatedInfo.gameOneShotKills = cb_enableOneShotKills.Checked;
                updatedInfo.gameAllowLeftLeaning = cb_enableLeftLean.Checked;
            }
            // Restrictions Weapons (Many Fields)
            {
                updatedInfo.weaponColt45 = cb_weapColt45.Checked;
                updatedInfo.weaponM9Beretta = cb_weapM9Bereatta.Checked;
                updatedInfo.weaponCar15 = cb_weapCAR15.Checked;
                updatedInfo.weaponCar15203 = cb_weapCAR15203.Checked;
                updatedInfo.weaponM16 = cb_weapM16.Checked;
                updatedInfo.weaponM16203 = cb_weapM16203.Checked;
                updatedInfo.weaponG3 = cb_weapG3.Checked;
                updatedInfo.weaponG36 = cb_weapG36.Checked;
                updatedInfo.weaponM60 = cb_weapM60.Checked;
                updatedInfo.weaponM240 = cb_weapM240.Checked;
                updatedInfo.weaponMP5 = cb_weapMP5.Checked;
                updatedInfo.weaponSAW = cb_weapSaw.Checked;
                updatedInfo.weaponMCRT300 = cb_weap300Tact.Checked;
                updatedInfo.weaponM21 = cb_weapM21.Checked;
                updatedInfo.weaponM24 = cb_weapM24.Checked;
                updatedInfo.weaponBarrett = cb_weapBarret.Checked;
                updatedInfo.weaponPSG1 = cb_weapPSG1.Checked;
                updatedInfo.weaponShotgun = cb_weapShotgun.Checked;
                updatedInfo.weaponFragGrenade = cb_weapFragGrenade.Checked;
                updatedInfo.weaponSmokeGrenade = cb_weapSmokeGrenade.Checked;
                updatedInfo.weaponSatchelCharges = cb_weapSatchel.Checked;
                updatedInfo.weaponAT4 = cb_weapAT4.Checked;
                updatedInfo.weaponFlashGrenade = cb_weapFlashBang.Checked;
                updatedInfo.weaponClaymore = cb_weapClay.Checked;
            }
            // Role Restrictions (4 fields)
            {
                updatedInfo.roleCQB = cb_roleCQB.Checked;
                updatedInfo.roleGunner = cb_roleGunner.Checked;
                updatedInfo.roleSniper = cb_roleSniper.Checked;
                updatedInfo.roleMedic = cb_roleMedic.Checked;
            }
            // Remote Settings
            {
                updatedInfo.profileEnableRemote = cb_enableRemote.Checked;
                updatedInfo.profileRemotePort = (int)num_remotePort.Value;
            }


            if (CmdSetServerVariables.ProcessCommand(updatedInfo))
            {
                MessageBox.Show("Server settings applied successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(this.Name, "Server settings applied.");
            }
            else
            {
                AppDebug.Log(this.Name, "Failed to apply server settings."); // Apply the new settings

            }
        }
        // --- Highligh Differences ---
        private void functionEvent_HighlighDiffFields()
        {
            // Host Information
            {
                tb_serverID.BackColor = tb_serverID.Text != theInstance.WebStatsProfileID ? Color.LightYellow : SystemColors.Window;
                tb_serverName.BackColor = tb_serverName.Text != theInstance.gameServerName ? Color.LightYellow : SystemColors.Window;
                tb_hostName.BackColor = tb_hostName.Text != theInstance.gameHostName ? Color.LightYellow : SystemColors.Window;
                tb_serverMessage.BackColor = tb_serverMessage.Text != theInstance.gameMOTD ? Color.LightYellow : SystemColors.Window;
            }
            // Server Details
            {
                cb_serverIP.BackColor = cb_serverIP.SelectedItem?.ToString() != theInstance.profileBindIP ? Color.LightYellow : SystemColors.Window;
                num_serverPort.BackColor = (int)num_serverPort.Value != theInstance.profileBindPort ? Color.LightYellow : SystemColors.Window;
                tb_serverID.BackColor = tb_serverID.Text != theInstance.gamePasswordLobby ? Color.LightYellow : SystemColors.Window;
                cb_serverDedicated.BackColor = cb_serverDedicated.Checked != theInstance.gameDedicated ? Color.LightYellow : SystemColors.Window;
                cb_requireNova.BackColor = cb_requireNova.Checked != theInstance.gameRequireNova ? Color.LightYellow : SystemColors.Window;
                cb_sessionType.BackColor = cb_sessionType.SelectedIndex != theInstance.gameSessionType ? Color.LightYellow : SystemColors.Window;
            }
            // Server Options
            {
                num_gameTimeLimit.BackColor = (int)num_gameTimeLimit.Value != theInstance.gameTimeLimit ? Color.LightYellow : SystemColors.Window;
                cb_replayMaps.BackColor = cb_replayMaps.SelectedIndex != theInstance.gameLoopMaps ? Color.LightYellow : SystemColors.Window;
                num_gameStartDelay.BackColor = (int)num_gameStartDelay.Value != theInstance.gameStartDelay ? Color.LightYellow : SystemColors.Window;
                num_respawnTime.BackColor = (int)num_respawnTime.Value != theInstance.gameRespawnTime ? Color.LightYellow : SystemColors.Window;
                num_scoreBoardDelay.BackColor = (int)num_scoreBoardDelay.Value != theInstance.gameScoreBoardDelay ? Color.LightYellow : SystemColors.Window;
                num_maxPlayers.BackColor = (int)num_maxPlayers.Value != theInstance.gameMaxSlots ? Color.LightYellow : SystemColors.Window;
            }
            // Scoring Options
            {
                num_scoresKOTH.BackColor = (int)num_scoresKOTH.Value != theInstance.gameScoreZoneTime ? Color.LightYellow : SystemColors.Window;
                num_scoresDM.BackColor = (int)num_scoresDM.Value != theInstance.gameScoreKills ? Color.LightYellow : SystemColors.Window;
                num_scoresFB.BackColor = (int)num_scoresFB.Value != theInstance.gameScoreFlags ? Color.LightYellow : SystemColors.Window;
            }
            // Team Options
            {
                cb_autoBalance.BackColor = cb_autoBalance.Checked != theInstance.gameOptionAutoBalance ? Color.LightYellow : SystemColors.Window;
                tb_bluePassword.BackColor = tb_bluePassword.Text != theInstance.gamePasswordBlue ? Color.LightYellow : SystemColors.Window;
                tb_redPassword.BackColor = tb_redPassword.Text != theInstance.gamePasswordRed ? Color.LightYellow : SystemColors.Window;
            }
            // Game Play Settings
            {
                cb_showTracers.BackColor = cb_showTracers.Checked != theInstance.gameOptionShowTracers ? Color.LightYellow : SystemColors.Window;
                cb_showClays.BackColor = cb_showClays.Checked != theInstance.gameShowTeamClays ? Color.LightYellow : SystemColors.Window;
                cb_autoRange.BackColor = cb_autoRange.Checked != theInstance.gameOptionAutoRange ? Color.LightYellow : SystemColors.Window;
                num_pspTakeoverTimer.BackColor = (int)num_pspTakeoverTimer.Value != theInstance.gamePSPTOTimer ? Color.LightYellow : SystemColors.Window;
                num_flagReturnTime.BackColor = (int)num_flagReturnTime.Value != theInstance.gameFlagReturnTime ? Color.LightYellow : SystemColors.Window;
                num_maxTeamLives.BackColor = (int)num_maxTeamLives.Value != theInstance.gameMaxTeamLives ? Color.LightYellow : SystemColors.Window;
            }
            // Friendly Fire
            {
                cb_enableFFkills.BackColor = cb_enableFFkills.Checked != theInstance.gameOptionFF ? Color.LightYellow : SystemColors.Window;
                num_maxFFKills.BackColor = (int)num_maxFFKills.Value != theInstance.gameFriendlyFireKills ? Color.LightYellow : SystemColors.Window;
                cb_warnFFkils.BackColor = cb_warnFFkils.Checked != theInstance.gameOptionFFWarn ? Color.LightYellow : SystemColors.Window;
                cb_showTeamTags.BackColor = cb_showTeamTags.Checked != theInstance.gameOptionFriendlyTags ? Color.LightYellow : SystemColors.Window;
            }
            // Ping Checking
            {
                cb_enableMinCheck.BackColor = cb_enableMinCheck.Checked != theInstance.gameMinPing ? Color.LightYellow : SystemColors.Window;
                cb_enableMaxCheck.BackColor = cb_enableMaxCheck.Checked != theInstance.gameMaxPing ? Color.LightYellow : SystemColors.Window;
                num_minPing.BackColor = (int)num_minPing.Value != theInstance.gameMinPingValue ? Color.LightYellow : SystemColors.Window;
                num_maxPing.BackColor = (int)num_maxPing.Value != theInstance.gameMaxPingValue ? Color.LightYellow : SystemColors.Window;
            }
            // Misc
            {
                cb_customSkins.BackColor = cb_customSkins.Checked != theInstance.gameCustomSkins ? Color.LightYellow : SystemColors.Window;
                cb_enableDistroyBuildings.BackColor = cb_enableDistroyBuildings.Checked != theInstance.gameDestroyBuildings ? Color.LightYellow : SystemColors.Window;
                cb_enableFatBullets.BackColor = cb_enableFatBullets.Checked != theInstance.gameFatBullets ? Color.LightYellow : SystemColors.Window;
                cb_enableOneShotKills.BackColor = cb_enableOneShotKills.Checked != theInstance.gameOneShotKills ? Color.LightYellow : SystemColors.Window;
                cb_enableLeftLean.BackColor = cb_enableLeftLean.Checked != theInstance.gameAllowLeftLeaning ? Color.LightYellow : SystemColors.Window;
            }
            // Restrictions Weapons
            {
                cb_weapColt45.BackColor = cb_weapColt45.Checked != theInstance.weaponColt45 ? Color.LightYellow : SystemColors.Window;
                cb_weapM9Bereatta.BackColor = cb_weapM9Bereatta.Checked != theInstance.weaponM9Beretta ? Color.LightYellow : SystemColors.Window;
                cb_weapCAR15.BackColor = cb_weapCAR15.Checked != theInstance.weaponCar15 ? Color.LightYellow : SystemColors.Window;
                cb_weapCAR15203.BackColor = cb_weapCAR15203.Checked != theInstance.weaponCar15203 ? Color.LightYellow : SystemColors.Window;
                cb_weapM16.BackColor = cb_weapM16.Checked != theInstance.weaponM16 ? Color.LightYellow : SystemColors.Window;
                cb_weapM16203.BackColor = cb_weapM16203.Checked != theInstance.weaponM16203 ? Color.LightYellow : SystemColors.Window;
                cb_weapG3.BackColor = cb_weapG3.Checked != theInstance.weaponG3 ? Color.LightYellow : SystemColors.Window;
                cb_weapG36.BackColor = cb_weapG36.Checked != theInstance.weaponG36 ? Color.LightYellow : SystemColors.Window;
                cb_weapM60.BackColor = cb_weapM60.Checked != theInstance.weaponM60 ? Color.LightYellow : SystemColors.Window;
                cb_weapM240.BackColor = cb_weapM240.Checked != theInstance.weaponM240 ? Color.LightYellow : SystemColors.Window;
                cb_weapMP5.BackColor = cb_weapMP5.Checked != theInstance.weaponMP5 ? Color.LightYellow : SystemColors.Window;
                cb_weapSaw.BackColor = cb_weapSaw.Checked != theInstance.weaponSAW ? Color.LightYellow : SystemColors.Window;
                cb_weap300Tact.BackColor = cb_weap300Tact.Checked != theInstance.weaponMCRT300 ? Color.LightYellow : SystemColors.Window;
                cb_weapM21.BackColor = cb_weapM21.Checked != theInstance.weaponM21 ? Color.LightYellow : SystemColors.Window;
                cb_weapM24.BackColor = cb_weapM24.Checked != theInstance.weaponM24 ? Color.LightYellow : SystemColors.Window;
                cb_weapBarret.BackColor = cb_weapBarret.Checked != theInstance.weaponBarrett ? Color.LightYellow : SystemColors.Window;
                cb_weapPSG1.BackColor = cb_weapPSG1.Checked != theInstance.weaponPSG1 ? Color.LightYellow : SystemColors.Window;
                cb_weapShotgun.BackColor = cb_weapShotgun.Checked != theInstance.weaponShotgun ? Color.LightYellow : SystemColors.Window;
                cb_weapFragGrenade.BackColor = cb_weapFragGrenade.Checked != theInstance.weaponFragGrenade ? Color.LightYellow : SystemColors.Window;
                cb_weapSmokeGrenade.BackColor = cb_weapSmokeGrenade.Checked != theInstance.weaponSmokeGrenade ? Color.LightYellow : SystemColors.Window;
                cb_weapSatchel.BackColor = cb_weapSatchel.Checked != theInstance.weaponSatchelCharges ? Color.LightYellow : SystemColors.Window;
                cb_weapAT4.BackColor = cb_weapAT4.Checked != theInstance.weaponAT4 ? Color.LightYellow : SystemColors.Window;
                cb_weapFlashBang.BackColor = cb_weapFlashBang.Checked != theInstance.weaponFlashGrenade ? Color.LightYellow : SystemColors.Window;
                cb_weapClay.BackColor = cb_weapClay.Checked != theInstance.weaponClaymore ? Color.LightYellow : SystemColors.Window;
            }
            // Role Restrictions
            {
                cb_roleCQB.BackColor = cb_roleCQB.Checked != theInstance.roleCQB ? Color.LightYellow : SystemColors.Window;
                cb_roleGunner.BackColor = cb_roleGunner.Checked != theInstance.roleGunner ? Color.LightYellow : SystemColors.Window;
                cb_roleSniper.BackColor = cb_roleSniper.Checked != theInstance.roleSniper ? Color.LightYellow : SystemColors.Window;
                cb_roleMedic.BackColor = cb_roleMedic.Checked != theInstance.roleMedic ? Color.LightYellow : SystemColors.Window;
            }
            // Remote Settings
            {
                cb_enableRemote.BackColor = cb_enableRemote.Checked != theInstance.profileEnableRemote ? Color.LightYellow : SystemColors.Window;
                num_remotePort.BackColor = (int)num_remotePort.Value != (int)theInstance.profileRemotePort ? Color.LightYellow : SystemColors.Window;
            }
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
        private void functionEvent_UpdateServerControls()
        {
            // Is the Server Running?
            bool isOffline = (theInstance.instanceStatus == InstanceStatus.OFFLINE);

            // Server Running? Update Text
            btn_serverControl.Text = isOffline ? "START" : "STOP";
            // Lock Down Settings that shouldn't be changed while the server is running
            functionEvent_SetControlsEnabled(new Control[]
            {
                cb_serverIP,                                // Server IP ComboBox
                num_serverPort,                             // Server Port
                cb_serverDedicated,                         // Dedicated Server Checkbox
                tb_serverPassword,                          // Server Lobby Password
                cb_enableRemote,                            // Enable Remote Access
                num_remotePort,                             // Remote Access Port
                cb_requireNova                              // Nova Required Checkbox
            }, isOffline);
            
            // Update Visibility of Controls
            btn_ServerUpdate.Visible = !isOffline;          // Show the update button only when the server is running
            btn_LockLobby.Visible = !isOffline;             // Show the lock lobby button only when the server is running
        }
        private void functionEvent_SetControlsEnabled(Control[] controls, bool enabled)
        {
            foreach (var control in controls)
                control.Enabled = enabled;
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
            functionEvent_UpdateServerControls();                              // Update the Server Control Button State
            functionEvent_HighlighDiffFields();                                // Highlight fields that differ from the saved instance
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
        // --- Reset Server Settings Button Clicked ---
        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            functionEvent_GetServerSettings();
        }
        // --- Enable/Disable Checkboxes Based on Other Checkbox States ---
        private void actionClick_EnableFFkills(object sender, EventArgs e) => num_maxFFKills.Enabled = cb_enableFFkills.Checked;
        private void actionClick_EnableMinCheck(object sender, EventArgs e) => num_minPing.Enabled = cb_enableMinCheck.Checked;
        private void actionClick_EnableMaxPing(object sender, EventArgs e) => num_maxPing.Enabled = cb_enableMaxCheck.Checked;
        private void actionClick_ToggleRemoteAccess(object sender, EventArgs e) => num_remotePort.Enabled = cb_enableRemote.Checked;
        // --- Import/Export Server Settings ---
        private void actionClick_ImportServerSettings(object sender, EventArgs e) => theInstanceManager.ImportSettings();
        private void actionClick_ExportServerSettings(object sender, EventArgs e) => theInstanceManager.ExportSettings();
        // --- Server Control Button Clicked ---
        private void actionClick_serverControl(object sender, EventArgs e)
        {
            if (theInstanceManager.ValidateGameServerPath() && theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                theInstanceManager.SetServerVariables();
                if (GameManager.startGame())
                {
                    GameManager.ReadMemoryServerStatus();
                    functionEvent_UpdateServerControls();
                }
            }
            else
            {
                GameManager.stopGame();
                functionEvent_UpdateServerControls();
            }
            
        }
        // --- Update Game Server Settings ---
        private void actionClick_GameServerUpdate(object sender, EventArgs e)
        {
            if (GameManager.ReadMemoryIsProcessAttached())
            {
                theInstanceManager.UpdateGameServer();
                MessageBox.Show("Saved settings have been applied to the game server.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // --- Server Lock Lobby ----
        private void actionClick_ServerLockLobby(object sender, EventArgs e)
        {
            GameManager.WriteMemorySendConsoleCommand("lockgame");
        }
    }
}
