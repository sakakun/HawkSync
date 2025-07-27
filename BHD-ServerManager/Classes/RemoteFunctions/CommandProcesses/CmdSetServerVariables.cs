using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdSetServerVariables")]
    public static class CmdSetServerVariables
    {
        private static theInstance thisInstance => CommonCore.theInstance!;

        public static CommandResponse ProcessCommand(object data)
        {
            bool Success = false;
            string Message = string.Empty;
            string ResponseData = string.Empty;

            theInstance updatedInstance = new theInstance();

            AppDebug.Log("CmdSetServerVariables", "Set Server Variables Triggered.");

            try
            {
                AppDebug.Log("CmdSetServerVariables", data.ToString());

                string base64 = data switch
                {
                    string s => s,
                    JsonElement je when je.ValueKind == JsonValueKind.String => je.GetString()!,
                    _ => throw new InvalidCastException("Input data is not a valid Base64 string.")
                };

                byte[] jsonBytes = Convert.FromBase64String(base64);
                string json = Encoding.UTF8.GetString(jsonBytes);
                theInstance obj = JsonSerializer.Deserialize<theInstance>(json)!;

                UpdateTheInstance(obj);
                Success = true;
                Message = "Server variables updated successfully.";
                AppDebug.Log("CmdSetServerVariables", "Was updated? " + Success.ToString() + " Reason: " + Message);
            }
            catch (Exception ex)
            {
                Success = false;
                Message = $"Failed to decode or deserialize: {ex.Message}";
                ResponseData = string.Empty;
                AppDebug.Log("CmdSetServerVariables", "Was updated? " + Success.ToString() + " Reason: " + Message);
            }

            return new CommandResponse
            {
                Success = Success,
                Message = Message,
                ResponseData = null
            };

        }

        private static void UpdateTheInstance(theInstance updatedInstance)
        {
            // Instance Specific (skip [JsonIgnore] and runtime-only properties)

            // Profile Server Information
            thisInstance.profileServerType = updatedInstance.profileServerType;           
            thisInstance.profileBindIP = updatedInstance.profileBindIP;
            thisInstance.profileBindPort = updatedInstance.profileBindPort;
            thisInstance.profileEnableRemote = updatedInstance.profileEnableRemote;

            // Game Settings
            thisInstance.gameMatchWinner = updatedInstance.gameMatchWinner;
            thisInstance.gameServerName = updatedInstance.gameServerName;
            thisInstance.gameMOTD = updatedInstance.gameMOTD;
            thisInstance.gameCountryCode = updatedInstance.gameCountryCode;
            thisInstance.gameHostName = updatedInstance.gameHostName;
            thisInstance.gameDedicated = updatedInstance.gameDedicated;
            thisInstance.gameWindowedMode = updatedInstance.gameWindowedMode;
            thisInstance.gamePasswordLobby = updatedInstance.gamePasswordLobby;
            thisInstance.gamePasswordBlue = updatedInstance.gamePasswordBlue;
            thisInstance.gamePasswordRed = updatedInstance.gamePasswordRed;
            thisInstance.gameSessionType = updatedInstance.gameSessionType;
            thisInstance.gameMaxSlots = updatedInstance.gameMaxSlots;
            thisInstance.gameLoopMaps = updatedInstance.gameLoopMaps;
            thisInstance.gameRequireNova = updatedInstance.gameRequireNova;
            thisInstance.gameCustomSkins = updatedInstance.gameCustomSkins;
            thisInstance.gameScoreKills = updatedInstance.gameScoreKills;
            thisInstance.gameScoreFlags = updatedInstance.gameScoreFlags;
            thisInstance.gameScoreZoneTime = updatedInstance.gameScoreZoneTime;
            thisInstance.gameFriendlyFireKills = updatedInstance.gameFriendlyFireKills;
            thisInstance.gameTimeLimit = updatedInstance.gameTimeLimit;
            thisInstance.gameStartDelay = updatedInstance.gameStartDelay;
            thisInstance.gameRespawnTime = updatedInstance.gameRespawnTime;
            thisInstance.gameScoreBoardDelay = updatedInstance.gameScoreBoardDelay;
            thisInstance.gamePSPTOTimer = updatedInstance.gamePSPTOTimer;
            thisInstance.gameFlagReturnTime = updatedInstance.gameFlagReturnTime;
            thisInstance.gameMaxTeamLives = updatedInstance.gameMaxTeamLives;
            thisInstance.gameOneShotKills = updatedInstance.gameOneShotKills;
            thisInstance.gameFatBullets = updatedInstance.gameFatBullets;

            // Game Play Settings
            thisInstance.gameOptionAutoBalance = updatedInstance.gameOptionAutoBalance;
            thisInstance.gameOptionShowTracers = updatedInstance.gameOptionShowTracers;
            thisInstance.gameShowTeamClays = updatedInstance.gameShowTeamClays;
            thisInstance.gameOptionAutoRange = updatedInstance.gameOptionAutoRange;

            // Friendly Fire
            thisInstance.gameOptionFF = updatedInstance.gameOptionFF;
            thisInstance.gameOptionFFWarn = updatedInstance.gameOptionFFWarn;
            thisInstance.gameOptionFriendlyTags = updatedInstance.gameOptionFriendlyTags;

            // Ping Checking
            thisInstance.gameMinPing = updatedInstance.gameMinPing;
            thisInstance.gameMaxPing = updatedInstance.gameMaxPing;
            thisInstance.gameMinPingValue = updatedInstance.gameMinPingValue;
            thisInstance.gameMaxPingValue = updatedInstance.gameMaxPingValue;

            // Misc
            thisInstance.gameDestroyBuildings = updatedInstance.gameDestroyBuildings;
            thisInstance.gameAllowLeftLeaning = updatedInstance.gameAllowLeftLeaning;

            // Restrictions Weapons
            thisInstance.weaponColt45 = updatedInstance.weaponColt45;
            thisInstance.weaponM9Beretta = updatedInstance.weaponM9Beretta;
            thisInstance.weaponCar15 = updatedInstance.weaponCar15;
            thisInstance.weaponCar15203 = updatedInstance.weaponCar15203;
            thisInstance.weaponM16 = updatedInstance.weaponM16;
            thisInstance.weaponM16203 = updatedInstance.weaponM16203;
            thisInstance.weaponG3 = updatedInstance.weaponG3;
            thisInstance.weaponG36 = updatedInstance.weaponG36;
            thisInstance.weaponM60 = updatedInstance.weaponM60;
            thisInstance.weaponM240 = updatedInstance.weaponM240;
            thisInstance.weaponMP5 = updatedInstance.weaponMP5;
            thisInstance.weaponSAW = updatedInstance.weaponSAW;
            thisInstance.weaponMCRT300 = updatedInstance.weaponMCRT300;
            thisInstance.weaponM21 = updatedInstance.weaponM21;
            thisInstance.weaponM24 = updatedInstance.weaponM24;
            thisInstance.weaponBarrett = updatedInstance.weaponBarrett;
            thisInstance.weaponPSG1 = updatedInstance.weaponPSG1;
            thisInstance.weaponShotgun = updatedInstance.weaponShotgun;
            thisInstance.weaponFragGrenade = updatedInstance.weaponFragGrenade;
            thisInstance.weaponSmokeGrenade = updatedInstance.weaponSmokeGrenade;
            thisInstance.weaponSatchelCharges = updatedInstance.weaponSatchelCharges;
            thisInstance.weaponAT4 = updatedInstance.weaponAT4;
            thisInstance.weaponFlashGrenade = updatedInstance.weaponFlashGrenade;
            thisInstance.weaponClaymore = updatedInstance.weaponClaymore;

            // Role Restrictions
            thisInstance.roleCQB = updatedInstance.roleCQB;
            thisInstance.roleGunner = updatedInstance.roleGunner;
            thisInstance.roleSniper = updatedInstance.roleSniper;
            thisInstance.roleMedic = updatedInstance.roleMedic;

            // Stats Settings
            thisInstance.WebStatsEnabled = updatedInstance.WebStatsEnabled;
            thisInstance.WebStatsProfileID = updatedInstance.WebStatsProfileID;
            thisInstance.WebStatsServerPath = updatedInstance.WebStatsServerPath;
            thisInstance.WebStatsAnnouncements = updatedInstance.WebStatsAnnouncements;
            thisInstance.WebStatsReportInterval = updatedInstance.WebStatsReportInterval;
            thisInstance.WebStatsUpdateInterval = updatedInstance.WebStatsUpdateInterval;

            theInstanceManager.SaveSettings();

            if (thisInstance.instanceStatus != InstanceStatus.OFFLINE)
            {
                theInstanceManager.UpdateGameServer();
            }
            
            theInstanceManager.HighlightDifferences();
        }

    }
}
