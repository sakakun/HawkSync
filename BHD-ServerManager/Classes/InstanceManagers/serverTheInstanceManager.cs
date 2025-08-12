using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.Tickers;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverTheInstanceManager : theInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance thisInstance => CommonCore.theInstance!;

        public static string lastKnownSettingsPath = Path.Combine(CommonCore.AppDataPath, "lastKnownSettings.json");

        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;

        public void CheckSettings()
        {
            // Ensure AppData directory exists, with verbose logging and error handling.
            try
            {
                if (!Directory.Exists(CommonCore.AppDataPath))
                {
                    AppDebug.Log("InstanceManager", $"AppData ({CommonCore.AppDataPath}) directory does not exist, creating it now.");
                    Directory.CreateDirectory(CommonCore.AppDataPath);
                }

                if (!File.Exists(lastKnownSettingsPath))
                {
                    AppDebug.Log("InstanceManager", $"Settings file not found at {lastKnownSettingsPath}. Creating a new one with default values.");
                    theInstanceManager.SaveSettings();
                }
                else
                {
                    AppDebug.Log("InstanceManager", $"Settings file found at {lastKnownSettingsPath}. Loading settings.");
                    theInstanceManager.LoadSettings();
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("InstanceManager", $"Error during settings check: {ex.Message}");
                throw;
            }
        }
        public void LoadSettings(bool external = false, string? path = null)
        {
            string settingsPath = external && !string.IsNullOrWhiteSpace(path) ? path : lastKnownSettingsPath;
            AppDebug.Log("InstanceManager", $"Loading settings from {(external ? "external path" : "default path")}: {settingsPath}");

            if (!File.Exists(settingsPath))
            {
                AppDebug.Log("InstanceManager", $"Settings file not found at {settingsPath}. Using default settings.");
                return;
            }

            try
            {
                var tempInstance = JsonSerializer.Deserialize<theInstance>(File.ReadAllText(settingsPath));
                if (tempInstance == null) return;

                if (!external)
                {
                    // Copy all properties from tempInstance to theInstance
                    foreach (var prop in typeof(theInstance).GetProperties())
                    {
                        if (prop.CanRead && prop.CanWrite && prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length == 0)
                        {
                            var value = prop.GetValue(tempInstance);
                            prop.SetValue(theInstance, value);
                        }
                    }
                    theInstance.profileServerPath = tempInstance.profileServerPath != null ? Encoding.UTF8.GetString(Convert.FromBase64String(tempInstance.profileServerPath)) : string.Empty;

                    theInstanceManager.GetServerVariables();
                }
                else
                {
                    theInstanceManager.GetServerVariables(true, tempInstance); // Get the variables from the tempInstance
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("InstanceManager", $"Failed to load settings: {ex.Message}");
            }
        }
        public void SaveSettings(bool external = false, string? path = null)
        {
            string settingsPath = external && !string.IsNullOrWhiteSpace(path) ? path : lastKnownSettingsPath;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                theInstance tempInstance = new theInstance();
                foreach (var prop in typeof(theInstance).GetProperties())
                {
                    if (prop.CanRead && prop.CanWrite && prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length == 0)
                    {
                        var value = prop.GetValue(theInstance);
                        if (value != null)
                        {
                            prop.SetValue(tempInstance, value);
                        }
                    }
                }
                // Only encode if not null or empty
                if (!string.IsNullOrEmpty(tempInstance.profileServerPath))
                {
                    tempInstance.profileServerPath = Convert.ToBase64String(Encoding.UTF8.GetBytes(tempInstance.profileServerPath));
                }
                string json = JsonSerializer.Serialize(tempInstance, options);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("InstanceManager", $"Failed to save settings: {ex.Message}");
            }
        }
        public void ExportSettings()
        {
            // Export the settings to a JSON file using a ShowFileDialog from CoreManager
            string savePath = CommonCore.AppDataPath;
            string exportPath = Functions.ShowFileDialog(true, "Server Settings (*.svset)|*.svset|All files (*.*)|*.*", "Export Server Settings", savePath, "exportedServerSettings.svset")!;
            if (!string.IsNullOrEmpty(exportPath))
            {
                theInstanceManager.SaveSettings(true, exportPath); // Save the settings to the specified path
            }

        }
        public void ImportSettings()
        {
            string savePath = CommonCore.AppDataPath;
            string importPath = Functions.ShowFileDialog(true, "Server Settings (*.svset)|*.svset|All files (*.*)|*.*", "Import Server Settings", savePath, "importServerSettings.svset")!;
            if (!string.IsNullOrEmpty(importPath))
            {
                theInstanceManager.LoadSettings(true, importPath);
            }
        }
        public bool ValidateGameServerPath()
        {
            // Validate the profile server path
            if (string.IsNullOrWhiteSpace(theInstance.profileServerPath) || !Directory.Exists(theInstance.profileServerPath))
            {
                AppDebug.Log("InstanceManager", "Profile server path is invalid or does not exist.");
                MessageBox.Show("The profile server path is invalid or does not exist.  Please 'set' your server path and refresh your map list before starting the server.", "Invalid Profile Server Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // thisServer.btn_UpdatePath.Visible = true; // Enable the button to allow user to set the path
                return false;
            }
            // thisServer.btn_UpdatePath.Visible = false;
            return true;
        }
        public void GetServerVariables(bool import = false, theInstance updatedInstance = null!)
        {

            var newInstance = import && updatedInstance != null ? updatedInstance : theInstance;

            // Set the variables from the newInstance of the ServerManager to the variables located in tabServer
            // Host Information (4 fields)
            {

                thisServer.tb_serverID.Text = newInstance.WebStatsProfileID;
                thisServer.tb_serverName.Text = newInstance.gameServerName;
                thisServer.tb_hostName.Text = newInstance.gameHostName;
                thisServer.tb_serverMessage.Text = newInstance.gameMOTD;
            }
            // Server Details (6 fields)
            {
                if (!string.IsNullOrEmpty(newInstance.profileBindIP) &&
                    thisServer.cb_serverIP.Items.Contains(newInstance.profileBindIP))
                {
                    thisServer.cb_serverIP.SelectedItem = newInstance.profileBindIP; // Select the item in cb_serverIP that matches the value of newInstance.profileBindIP,
                }
                else
                {
                    thisServer.cb_serverIP.SelectedIndex = 0; // or default to the first item (assumed "0.0.0.0") if not found
                }
                thisServer.num_serverPort.Value = newInstance.profileBindPort;
                thisServer.tb_serverPassword.Text = newInstance.gamePasswordLobby;
                thisServer.cb_serverDedicated.Checked = newInstance.gameDedicated;
                thisServer.cb_novaRequired.Checked = newInstance.gameRequireNova;
                thisServer.cb_sessionType.SelectedIndex = newInstance.gameSessionType;
            }
            // Server Options (4 fields)
            {
                thisServer.num_gameTimeLimit.Value = newInstance.gameTimeLimit;
                thisServer.cb_replayMaps.SelectedIndex = newInstance.gameLoopMaps;
                thisServer.num_gameStartDelay.Value = newInstance.gameStartDelay;
                thisServer.num_respawnTime.Value = newInstance.gameRespawnTime;
                thisServer.num_scoreBoardDelay.Value = newInstance.gameScoreBoardDelay;
                thisServer.num_maxPlayers.Value = newInstance.gameMaxSlots;
            }
            // Scoring Options (3 fields)
            {
                thisServer.num_scoresKOTH.Value = newInstance.gameScoreZoneTime;
                thisServer.num_scoresDM.Value = newInstance.gameScoreKills;
                thisServer.num_scoresFB.Value = newInstance.gameScoreFlags;
            }
            // Team Options (3 fields)
            {
                thisServer.cb_autoBalance.Checked = newInstance.gameOptionAutoBalance;
                thisServer.tb_bluePassword.Text = newInstance.gamePasswordBlue;
                thisServer.tb_redPassword.Text = newInstance.gamePasswordRed;
            }
            // Game Play Settings (6 fields)
            {
                thisServer.cb_showTracers.Checked = newInstance.gameOptionShowTracers;
                thisServer.cb_showClays.Checked = newInstance.gameShowTeamClays;
                thisServer.cb_autoRange.Checked = newInstance.gameOptionAutoRange;
                thisServer.num_pspTakeoverTimer.Value = newInstance.gamePSPTOTimer;
                thisServer.num_flagReturnTime.Value = newInstance.gameFlagReturnTime;
                thisServer.num_maxTeamLives.Value = newInstance.gameMaxTeamLives;
            }
            // Friendly Fire (4 fields)
            {
                thisServer.cb_enableFFkills.Checked = newInstance.gameOptionFF;
                thisServer.num_maxFFKills.Enabled = newInstance.gameOptionFF ? true : false;
                thisServer.num_maxFFKills.Value = newInstance.gameFriendlyFireKills;
                thisServer.cb_warnFFkils.Checked = newInstance.gameOptionFFWarn;
                thisServer.cb_showTeamTags.Checked = newInstance.gameOptionFriendlyTags;
            }
            // Ping Checking (4 fields)
            {
                thisServer.cb_enableMinCheck.Checked = newInstance.gameMinPing;
                thisServer.cb_enableMaxCheck.Checked = newInstance.gameMaxPing;
                thisServer.num_minPing.Value = newInstance.gameMinPingValue;
                thisServer.num_maxPing.Value = newInstance.gameMaxPingValue;
                thisServer.num_minPing.Enabled = newInstance.gameMinPing ? true : false;
                thisServer.num_maxPing.Enabled = newInstance.gameMaxPing ? true : false;
            }
            // Misc (4 fields)
            {
                thisServer.cb_customSkins.Checked = newInstance.gameCustomSkins;
                thisServer.cb_enableDistroyBuildings.Checked = newInstance.gameDestroyBuildings;
                thisServer.cb_enableFatBullets.Checked = newInstance.gameFatBullets;
                thisServer.cb_enableOneShotKills.Checked = newInstance.gameOneShotKills;
                thisServer.cb_enableLeftLean.Checked = newInstance.gameAllowLeftLeaning;
            }

            // Restrictions Weapons (Many Fields)

            {
                thisServer.cb_weapColt45.Checked = newInstance.weaponColt45;
                thisServer.cb_weapM9Bereatta.Checked = newInstance.weaponM9Beretta;
                thisServer.cb_weapCAR15.Checked = newInstance.weaponCar15;
                thisServer.cb_weapCAR15203.Checked = newInstance.weaponCar15203;
                thisServer.cb_weapM16.Checked = newInstance.weaponM16;
                thisServer.cb_weapM16203.Checked = newInstance.weaponM16203;
                thisServer.cb_weapG3.Checked = newInstance.weaponG3;
                thisServer.cb_weapG36.Checked = newInstance.weaponG36;
                thisServer.cb_weapM60.Checked = newInstance.weaponM60;
                thisServer.cb_weapM240.Checked = newInstance.weaponM240;
                thisServer.cb_weapMP5.Checked = newInstance.weaponMP5;
                thisServer.cb_weapSaw.Checked = newInstance.weaponSAW;
                thisServer.cb_weap300Tact.Checked = newInstance.weaponMCRT300;
                thisServer.cb_weapM21.Checked = newInstance.weaponM21;
                thisServer.cb_weapM24.Checked = newInstance.weaponM24;
                thisServer.cb_weapBarret.Checked = newInstance.weaponBarrett;
                thisServer.cb_weapPSG1.Checked = newInstance.weaponPSG1;
                thisServer.cb_weapShotgun.Checked = newInstance.weaponShotgun;
                thisServer.cb_weapFragGrenade.Checked = newInstance.weaponFragGrenade;
                thisServer.cb_weapSmokeGrenade.Checked = newInstance.weaponSmokeGrenade;
                thisServer.cb_weapSatchel.Checked = newInstance.weaponSatchelCharges;
                thisServer.cb_weapAT4.Checked = newInstance.weaponAT4;
                thisServer.cb_weapFlashBang.Checked = newInstance.weaponFlashGrenade;
                thisServer.cb_weapClay.Checked = newInstance.weaponClaymore;

                // If the above weap are all checked, then checkBox_selectAll should be checked.
                thisServer.checkBox_selectAll.Checked =
                    thisServer.cb_weapColt45.Checked &&
                    thisServer.cb_weapM9Bereatta.Checked &&
                    thisServer.cb_weapCAR15.Checked &&
                    thisServer.cb_weapCAR15203.Checked &&
                    thisServer.cb_weapM16.Checked &&
                    thisServer.cb_weapM16203.Checked &&
                    thisServer.cb_weapG3.Checked &&
                    thisServer.cb_weapG36.Checked &&
                    thisServer.cb_weapM60.Checked &&
                    thisServer.cb_weapM240.Checked &&
                    thisServer.cb_weapMP5.Checked &&
                    thisServer.cb_weapSaw.Checked &&
                    thisServer.cb_weap300Tact.Checked &&
                    thisServer.cb_weapM21.Checked &&
                    thisServer.cb_weapM24.Checked &&
                    thisServer.cb_weapBarret.Checked &&
                    thisServer.cb_weapPSG1.Checked &&
                    thisServer.cb_weapShotgun.Checked &&
                    thisServer.cb_weapFragGrenade.Checked &&
                    thisServer.cb_weapSmokeGrenade.Checked &&
                    thisServer.cb_weapSatchel.Checked &&
                    thisServer.cb_weapAT4.Checked &&
                    thisServer.cb_weapFlashBang.Checked &&
                    thisServer.cb_weapClay.Checked;

                // If the above weap are all unchecked, then checkBox_selectNone should be checked.
                thisServer.checkBox_selectNone.Checked =
                    !thisServer.cb_weapColt45.Checked &&
                    !thisServer.cb_weapM9Bereatta.Checked &&
                    !thisServer.cb_weapCAR15.Checked &&
                    !thisServer.cb_weapCAR15203.Checked &&
                    !thisServer.cb_weapM16.Checked &&
                    !thisServer.cb_weapM16203.Checked &&
                    !thisServer.cb_weapG3.Checked &&
                    !thisServer.cb_weapG36.Checked &&
                    !thisServer.cb_weapM60.Checked &&
                    !thisServer.cb_weapM240.Checked &&
                    !thisServer.cb_weapMP5.Checked &&
                    !thisServer.cb_weapSaw.Checked &&
                    !thisServer.cb_weap300Tact.Checked &&
                    !thisServer.cb_weapM21.Checked &&
                    !thisServer.cb_weapM24.Checked &&
                    !thisServer.cb_weapBarret.Checked &&
                    !thisServer.cb_weapPSG1.Checked &&
                    !thisServer.cb_weapShotgun.Checked &&
                    !thisServer.cb_weapFragGrenade.Checked &&
                    !thisServer.cb_weapSmokeGrenade.Checked &&
                    !thisServer.cb_weapSatchel.Checked &&
                    !thisServer.cb_weapAT4.Checked &&
                    !thisServer.cb_weapFlashBang.Checked &&
                    !thisServer.cb_weapClay.Checked;
            }

            // Role Restrictions (4 fields
            {
                thisServer.cb_roleCQB.Checked = newInstance.roleCQB;
                thisServer.cb_roleGunner.Checked = newInstance.roleGunner;
                thisServer.cb_roleSniper.Checked = newInstance.roleSniper;
                thisServer.cb_roleMedic.Checked = newInstance.roleMedic;
            }

            // Stats Settings
            thisServer.cb_enableWebStats.Checked = newInstance.WebStatsEnabled;
            thisServer.tb_webStatsServerPath.Text = newInstance.WebStatsServerPath;
            thisServer.cb_enableAnnouncements.Checked = newInstance.WebStatsAnnouncements;
            thisServer.num_WebStatsReport.Value = newInstance.WebStatsReportInterval;
            thisServer.num_WebStatsUpdates.Value = newInstance.WebStatsUpdateInterval;

            bool statsEnabled = newInstance.WebStatsEnabled;
            bool announcementsEnabled = statsEnabled && newInstance.WebStatsAnnouncements;

            thisServer.tb_webStatsServerPath.Enabled = statsEnabled;
            thisServer.cb_enableAnnouncements.Enabled = statsEnabled;
            thisServer.num_WebStatsReport.Enabled = announcementsEnabled;
            thisServer.num_WebStatsUpdates.Enabled = statsEnabled;

            // Remote Settings
            thisServer.cb_enableRemote.Checked = newInstance.profileEnableRemote;
            thisServer.num_remotePort.Value = (int)newInstance.profileRemotePort;
            thisServer.num_remotePort.Enabled = newInstance.profileEnableRemote;

        }
        public void SetServerVariables()
        {

            // Host Information
            thisInstance.WebStatsProfileID = thisServer.tb_serverID.Text;
            thisInstance.gameServerName = thisServer.tb_serverName.Text;
            thisInstance.gameHostName = thisServer.tb_hostName.Text;
            thisInstance.gameMOTD = thisServer.tb_serverMessage.Text;

            // Server Details
            thisInstance.profileBindIP = thisServer.cb_serverIP.SelectedItem?.ToString() ?? "";
            thisInstance.profileBindPort = (int)thisServer.num_serverPort.Value;
            thisInstance.gamePasswordLobby = thisServer.tb_serverPassword.Text;
            thisInstance.gameDedicated = thisServer.cb_serverDedicated.Checked;
            thisInstance.gameRequireNova = thisServer.cb_novaRequired.Checked;
            thisInstance.gameSessionType = thisServer.cb_sessionType.SelectedIndex;

            // Server Options
            thisInstance.gameTimeLimit = (int)thisServer.num_gameTimeLimit.Value;
            thisInstance.gameLoopMaps = thisServer.cb_replayMaps.SelectedIndex;
            thisInstance.gameStartDelay = (int)thisServer.num_gameStartDelay.Value;
            thisInstance.gameRespawnTime = (int)thisServer.num_respawnTime.Value;
            thisInstance.gameScoreBoardDelay = (int)thisServer.num_scoreBoardDelay.Value;
            thisInstance.gameMaxSlots = (int)thisServer.num_maxPlayers.Value;

            // Scoring Options
            thisInstance.gameScoreZoneTime = (int)thisServer.num_scoresKOTH.Value;
            thisInstance.gameScoreKills = (int)thisServer.num_scoresDM.Value;
            thisInstance.gameScoreFlags = (int)thisServer.num_scoresFB.Value;

            // Team Options
            thisInstance.gameOptionAutoBalance = thisServer.cb_autoBalance.Checked;
            thisInstance.gamePasswordBlue = thisServer.tb_bluePassword.Text;
            thisInstance.gamePasswordRed = thisServer.tb_redPassword.Text;

            // Game Play Settings
            thisInstance.gameOptionShowTracers = thisServer.cb_showTracers.Checked;
            thisInstance.gameShowTeamClays = thisServer.cb_showClays.Checked;
            thisInstance.gameOptionAutoRange = thisServer.cb_autoRange.Checked;
            thisInstance.gamePSPTOTimer = (int)thisServer.num_pspTakeoverTimer.Value;
            thisInstance.gameFlagReturnTime = (int)thisServer.num_flagReturnTime.Value;
            thisInstance.gameMaxTeamLives = (int)thisServer.num_maxTeamLives.Value;

            // Friendly Fire
            thisInstance.gameOptionFF = thisServer.cb_enableFFkills.Checked;
            thisInstance.gameFriendlyFireKills = (int)thisServer.num_maxFFKills.Value;
            thisInstance.gameOptionFFWarn = thisServer.cb_warnFFkils.Checked;
            thisInstance.gameOptionFriendlyTags = thisServer.cb_showTeamTags.Checked;

            // Ping Checking
            thisInstance.gameMinPing = thisServer.cb_enableMinCheck.Checked;
            thisInstance.gameMaxPing = thisServer.cb_enableMaxCheck.Checked;
            thisInstance.gameMinPingValue = (int)thisServer.num_minPing.Value;
            thisInstance.gameMaxPingValue = (int)thisServer.num_maxPing.Value;

            // Misc
            thisInstance.gameCustomSkins = thisServer.cb_customSkins.Checked;
            thisInstance.gameDestroyBuildings = thisServer.cb_enableDistroyBuildings.Checked;
            thisInstance.gameFatBullets = thisServer.cb_enableFatBullets.Checked;
            thisInstance.gameOneShotKills = thisServer.cb_enableOneShotKills.Checked;
            thisInstance.gameAllowLeftLeaning = thisServer.cb_enableLeftLean.Checked;

            // Restrictions Weapons
            thisInstance.weaponColt45 = thisServer.cb_weapColt45.Checked;
            thisInstance.weaponM9Beretta = thisServer.cb_weapM9Bereatta.Checked;
            thisInstance.weaponCar15 = thisServer.cb_weapCAR15.Checked;
            thisInstance.weaponCar15203 = thisServer.cb_weapCAR15203.Checked;
            thisInstance.weaponM16 = thisServer.cb_weapM16.Checked;
            thisInstance.weaponM16203 = thisServer.cb_weapM16203.Checked;
            thisInstance.weaponG3 = thisServer.cb_weapG3.Checked;
            thisInstance.weaponG36 = thisServer.cb_weapG36.Checked;
            thisInstance.weaponM60 = thisServer.cb_weapM60.Checked;
            thisInstance.weaponM240 = thisServer.cb_weapM240.Checked;
            thisInstance.weaponMP5 = thisServer.cb_weapMP5.Checked;
            thisInstance.weaponSAW = thisServer.cb_weapSaw.Checked;
            thisInstance.weaponMCRT300 = thisServer.cb_weap300Tact.Checked;
            thisInstance.weaponM21 = thisServer.cb_weapM21.Checked;
            thisInstance.weaponM24 = thisServer.cb_weapM24.Checked;
            thisInstance.weaponBarrett = thisServer.cb_weapBarret.Checked;
            thisInstance.weaponPSG1 = thisServer.cb_weapPSG1.Checked;
            thisInstance.weaponShotgun = thisServer.cb_weapShotgun.Checked;
            thisInstance.weaponFragGrenade = thisServer.cb_weapFragGrenade.Checked;
            thisInstance.weaponSmokeGrenade = thisServer.cb_weapSmokeGrenade.Checked;
            thisInstance.weaponSatchelCharges = thisServer.cb_weapSatchel.Checked;
            thisInstance.weaponAT4 = thisServer.cb_weapAT4.Checked;
            thisInstance.weaponFlashGrenade = thisServer.cb_weapFlashBang.Checked;
            thisInstance.weaponClaymore = thisServer.cb_weapClay.Checked;

            // Role Restrictions
            thisInstance.roleCQB = thisServer.cb_roleCQB.Checked;
            thisInstance.roleGunner = thisServer.cb_roleGunner.Checked;
            thisInstance.roleSniper = thisServer.cb_roleSniper.Checked;
            thisInstance.roleMedic = thisServer.cb_roleMedic.Checked;

            // Stats Settings
            thisInstance.WebStatsEnabled = thisServer.cb_enableWebStats.Checked;
            thisInstance.WebStatsServerPath = thisServer.tb_webStatsServerPath.Text;
            thisInstance.WebStatsAnnouncements = thisServer.cb_enableAnnouncements.Checked;
            thisInstance.WebStatsReportInterval = (int)thisServer.num_WebStatsReport.Value;
            thisInstance.WebStatsUpdateInterval = (int)thisServer.num_WebStatsUpdates.Value;

            // Remote Settings
            thisInstance.profileEnableRemote = thisServer.cb_enableRemote.Checked;
            thisInstance.profileRemotePort = (int)thisServer.num_remotePort.Value;

        }
        public void ValidateGameServerType(string serverPath)
        {
            // Additional checks can be added here if necessary
            if (File.Exists(Path.Combine(serverPath, "EXP1.pff")))
            {
                thisInstance.profileServerType = 1; // BHDTS
                return;
            }
            thisInstance.profileServerType = 0; // BHD
        }
        public void UpdateGameServer()
        {
            // the following code will replace line by line until all variables have a "update" function in ServerMemory

            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
                return;

            // Host Information
            ServerMemory.UpdateServerName();            // gameServerName
            ServerMemory.UpdatePlayerHostName();        // gameHostname
            ServerMemory.UpdateMOTD();                  // gameMOTD

            // Server Details
            ServerMemory.UpdateRequireNovaLogin();      // gameRequireNova

            // Server Options
            ServerMemory.UpdateTimeLimit();             // gameTimeLimit
            ServerMemory.UpdateLoopMaps();              // gameLoopMaps
            ServerMemory.UpdateStartDelay();            // gameStartDelay
            ServerMemory.UpdateRespawnTime();           // gameRespawnTime   
            ServerMemory.UpdateMaxSlots();              // gameMaxSlots

            // Team Options
            ServerMemory.UpdateBluePassword();          // gamePasswordBlue
            ServerMemory.UpdateRedPassword();           // gamePasswordRed

            // Game Play Settings
            ServerMemory.UpdateGamePlayOptions();       //gameOptionShowTracers, gameShowTeamClays, gameOptionAutoRange, gameOptionFF, gameOptionFFWarn, gameOptionFriendlyTags, gameOptionAutoBalance
            ServerMemory.UpdatePSPTakeOverTime();       //gamePSPTOTimer
            ServerMemory.UpdateFlagReturnTime();        //gameFlagReturnTime
            ServerMemory.UpdateMaxTeamLives();          //gameMaxTeamLives

            // Friendly Fire
            ServerMemory.UpdateFriendlyFireKills();     //gameFriendlyFireKills

            // Ping Checking
            ServerMemory.UpdateMinPing();               //gameMinPing
            ServerMemory.UpdateMaxPing();               //gameMaxPing
            ServerMemory.UpdateMinPingValue();          //gameMinPingValue
            ServerMemory.UpdateMaxPingValue();          //gameMaxPingValue

            // Misc
            ServerMemory.UpdateAllowCustomSkins();      //gameCustomSkins
            ServerMemory.UpdateDestroyBuildings();      //gameDestroyBuildings
            ServerMemory.UpdateFatBullets();            //gameFatBullets
            ServerMemory.UpdateOneShotKills();          //gameOneShotKills

            // Restrictions Weapons
            ServerMemory.UpdateWeaponRestrictions();

            // Role Restrictions
            //thisInstance.roleCQB = thisServer.cb_roleCQB.Checked;
            //thisInstance.roleGunner = thisServer.cb_roleGunner.Checked;
            //thisInstance.roleSniper = thisServer.cb_roleSniper.Checked;
            //thisInstance.roleMedic = thisServer.cb_roleMedic.Checked;

            // Update Game Scores for the next game.
            ServerMemory.UpdateGameScores();

            //
            // To Be Moved to Ticker Event
            //

            // Player State Check
            //thisInstance.gameAllowLeftLeaning = thisServer.cb_enableLeftLean.Checked;

        }
        public void InitializeTickers()
        {
            // TODO: Remove the need for the thisServer variable in the ticker methods.
            CommonCore.Ticker.Start("ServerManager", 2000, () => tickerServerManager.runTicker());
            CommonCore.Ticker.Start("ChatManager", 500, () => tickerChatManagement.runTicker());
            CommonCore.Ticker.Start("PlayerManager", 1000, () => tickerPlayerManagement.runTicker());
            CommonCore.Ticker.Start("BanManager", 1000, () => tickerBanManagement.runTicker());

        }
        public void changeTeamGameMode(int currentMapType, int nextMapType)
        {

            bool isCurrentMapTeamMap = Functions.IsMapTeamBased(currentMapType);
            bool isNextMapTeamMap = Functions.IsMapTeamBased(nextMapType);

            if (isNextMapTeamMap == false && isCurrentMapTeamMap == true)
            {
                // TDM -> DM
                // Going to get every current player and add them to the previous PlayerTeam list
                // Then change their PlayerTeam number to PlayerSlot + 5 (DM Team) 6 - 56 aka everyone is on thier own PlayerTeam.
                foreach (var playerRecord in theInstance.playerList)
                {
                    playerObject playerObj = playerRecord.Value;
                    theInstance.playerPreviousTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerTeam
                    });

                    theInstance.playerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerSlot + 5
                    });
                }
            }
            else if (isNextMapTeamMap == true && isCurrentMapTeamMap == false)
            {
                // DM -> TDM
                // Going to attempt to put them back in their own PlayerSlot or randomly assign them to a PlayerTeam.
                foreach (playerTeamObject playerObj in theInstance.playerPreviousTeamList)
                {
                    if (theInstance.playerList[playerObj.slotNum] != null)
                    {
                        theInstance.playerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = playerObj.slotNum,
                            Team = playerObj.Team
                        });
                    }
                }
                foreach (var playerRecord in theInstance.playerList)
                {
                    playerObject player = playerRecord.Value;
                    bool found = false;
                    foreach (playerTeamObject previousPlayer in theInstance.playerPreviousTeamList)
                    {
                        if (player.PlayerSlot == previousPlayer.slotNum)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        int blueteam = 0;
                        int redteam = 0;

                        foreach (playerTeamObject playerTeam in theInstance.playerPreviousTeamList)
                        {
                            if (playerTeam.Team == (int)Teams.TEAM_BLUE)
                            {
                                blueteam++;
                            }
                            else if (playerTeam.Team == (int)Teams.TEAM_RED)
                            {
                                redteam++;
                            }
                        }

                        if (blueteam > redteam)
                        {
                            theInstance.playerChangeTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_RED
                            });
                            theInstance.playerPreviousTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_RED
                            });
                        }
                        else if (blueteam < redteam)
                        {
                            theInstance.playerChangeTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_BLUE
                            });
                            theInstance.playerPreviousTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_BLUE
                            });
                        }
                        else if (blueteam == redteam)
                        {
                            Random rand = new Random();
                            int rnd = rand.Next(1, 2);
                            if (rnd == 1)
                            {
                                theInstance.playerChangeTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_BLUE
                                });
                                theInstance.playerPreviousTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_BLUE
                                });
                            }
                            else if (rnd == 2)
                            {
                                theInstance.playerChangeTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_RED
                                });
                                theInstance.playerPreviousTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_RED
                                });
                            }
                        }
                    }
                }
                theInstance.playerPreviousTeamList.Clear();
            }

        }

        public void HighlightDifferences()
        {
            // Host Information
            HighlightTextBox(thisServer.tb_serverID, theInstance.WebStatsProfileID);
            HighlightTextBox(thisServer.tb_serverName, theInstance.gameServerName);
            HighlightTextBox(thisServer.tb_hostName, theInstance.gameHostName);
            HighlightTextBox(thisServer.tb_serverMessage, theInstance.gameMOTD);

            // Server Details
            HighlightComboBox(thisServer.cb_serverIP, theInstance.profileBindIP);
            HighlightNumericUpDown(thisServer.num_serverPort, theInstance.profileBindPort);
            HighlightTextBox(thisServer.tb_serverPassword, theInstance.gamePasswordLobby);
            HighlightCheckBox(thisServer.cb_serverDedicated, theInstance.gameDedicated);
            HighlightCheckBox(thisServer.cb_novaRequired, theInstance.gameRequireNova);
            HighlightComboBoxIndex(thisServer.cb_sessionType, theInstance.gameSessionType);

            // Server Options
            HighlightNumericUpDown(thisServer.num_gameTimeLimit, theInstance.gameTimeLimit);
            HighlightComboBoxIndex(thisServer.cb_replayMaps, theInstance.gameLoopMaps);
            HighlightNumericUpDown(thisServer.num_gameStartDelay, theInstance.gameStartDelay);
            HighlightNumericUpDown(thisServer.num_respawnTime, theInstance.gameRespawnTime);
            HighlightNumericUpDown(thisServer.num_scoreBoardDelay, theInstance.gameScoreBoardDelay);
            HighlightNumericUpDown(thisServer.num_maxPlayers, theInstance.gameMaxSlots);

            // Scoring Options
            HighlightNumericUpDown(thisServer.num_scoresKOTH, theInstance.gameScoreZoneTime);
            HighlightNumericUpDown(thisServer.num_scoresDM, theInstance.gameScoreKills);
            HighlightNumericUpDown(thisServer.num_scoresFB, theInstance.gameScoreFlags);

            // Team Options
            HighlightCheckBox(thisServer.cb_autoBalance, theInstance.gameOptionAutoBalance);
            HighlightTextBox(thisServer.tb_bluePassword, theInstance.gamePasswordBlue);
            HighlightTextBox(thisServer.tb_redPassword, theInstance.gamePasswordRed);

            // Game Play Settings
            HighlightCheckBox(thisServer.cb_showTracers, theInstance.gameOptionShowTracers);
            HighlightCheckBox(thisServer.cb_showClays, theInstance.gameShowTeamClays);
            HighlightCheckBox(thisServer.cb_autoRange, theInstance.gameOptionAutoRange);
            HighlightNumericUpDown(thisServer.num_pspTakeoverTimer, theInstance.gamePSPTOTimer);
            HighlightNumericUpDown(thisServer.num_flagReturnTime, theInstance.gameFlagReturnTime);
            HighlightNumericUpDown(thisServer.num_maxTeamLives, theInstance.gameMaxTeamLives);

            // Friendly Fire
            HighlightCheckBox(thisServer.cb_enableFFkills, theInstance.gameOptionFF);
            HighlightNumericUpDown(thisServer.num_maxFFKills, theInstance.gameFriendlyFireKills);
            HighlightCheckBox(thisServer.cb_warnFFkils, theInstance.gameOptionFFWarn);
            HighlightCheckBox(thisServer.cb_showTeamTags, theInstance.gameOptionFriendlyTags);

            // Ping Checking
            HighlightCheckBox(thisServer.cb_enableMinCheck, theInstance.gameMinPing);
            HighlightCheckBox(thisServer.cb_enableMaxCheck, theInstance.gameMaxPing);
            HighlightNumericUpDown(thisServer.num_minPing, theInstance.gameMinPingValue);
            HighlightNumericUpDown(thisServer.num_maxPing, theInstance.gameMaxPingValue);

            // Misc
            HighlightCheckBox(thisServer.cb_customSkins, theInstance.gameCustomSkins);
            HighlightCheckBox(thisServer.cb_enableDistroyBuildings, theInstance.gameDestroyBuildings);
            HighlightCheckBox(thisServer.cb_enableFatBullets, theInstance.gameFatBullets);
            HighlightCheckBox(thisServer.cb_enableOneShotKills, theInstance.gameOneShotKills);
            HighlightCheckBox(thisServer.cb_enableLeftLean, theInstance.gameAllowLeftLeaning);

            // Add more fields as needed...
        }

        // Helper methods for highlighting
        public void HighlightTextBox(TextBox tb, string value)
        {
            tb.BackColor = tb.Text == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightComboBox(ComboBox cb, string value)
        {
            cb.BackColor = cb.Text == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightComboBoxIndex(ComboBox cb, int value)
        {
            cb.BackColor = cb.SelectedIndex == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightNumericUpDown(NumericUpDown num, int value)
        {
            num.BackColor = (int)num.Value == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightCheckBox(CheckBox cb, bool value)
        {
            cb.BackColor = cb.Checked == value ? SystemColors.Control : Color.LightYellow;
        }
    }
}
