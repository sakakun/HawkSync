using BHD_SharedResources.Classes.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace BHD_SharedResources.Classes.Instances
{
    public class theInstance
    {
        // Instance Specific
        [JsonIgnore]
        public int? instanceAttachedPID { get; set; }
        [JsonIgnore]
        public nint instanceProcessHandle { get; set; }
        [JsonIgnore]
        public Process instanceProcess { get; set; }
        public InstanceStatus instanceStatus { get; set; } = InstanceStatus.OFFLINE;
        [JsonIgnore]
        public int instanceCrashCounter { get; set; } = 0;   /* Counter for the number of Crashes */

        // Instance Tick Flow
        [JsonIgnore]
        public bool instancePreGameProcRun { get; set; } = true;
        [JsonIgnore]
        public bool instanceScoringProcRun { get; set; } = true;
        [JsonIgnore]
        public bool instanceMapSkipped { get; set; } = false;
        public DateTime instanceLastUpdateTime { get; set; } = DateTime.Now;
        public DateTime instanceNextUpdateTime { get; set; } = DateTime.Now.AddSeconds(2.0);

        // Profile Server Information
        public int profileServerType { get; set; }
        public string profileServerPath { get; set; } = String.Empty;
        public string profileModFileName { get; set; } = String.Empty;
        public List<string> profileModifierList1 { get; set; } = new();
        public List<string> profileModifierList2 { get; set; } = new();
        public string profileBindIP { get; set; } = String.Empty;
        public int profileBindPort { get; set; } = 17479;
        public bool profileEnableRemote { get; set; } = false; // Enable/Disable Remote Connections
        public int profileRemotePort { get; set; } = 8083;  // Remote IP for the server

        // Game Settings
        public int gameMatchWinner { get; set; } = 0;
        public string gameServerName { get; set; } = "Untitled Server";
        public string gameMOTD { get; set; } = "Welcome to the server!";
        public string gameCountryCode { get; set; } = "US";
        public string gameHostName { get; set; } = "HostName";
        public bool gameDedicated { get; set; } = true;
        public bool gameWindowedMode { get; set; } = true;
        public string gamePasswordLobby { get; set; } = "";
        public string gamePasswordBlue { get; set; } = "";
        public string gamePasswordRed { get; set; } = "";
        public int gameSessionType { get; set; } = 0;   /* Session Type (Internet/LAN) - Currently not working. */
        public int gameMaxSlots { get; set; } = 50;  /* Max Players 50 */
        public int gameLoopMaps { get; set; } = 1;   /* 0 = Play One Map, 1 = Loop Maps, 2 = Cycle Maps */
        public bool gameRequireNova { get; set; } = false;
        public bool gameCustomSkins { get; set; } = false;
        public int gameScoreKills { get; set; } = 20;  /* Game Score needed for T/DM Matches to Win */
        public int gameScoreFlags { get; set; } = 10;  /* Game Score needed for CTF & FB to Win */
        public int gameScoreZoneTime { get; set; } = 10;  /* Game Score needed for T/KOTH to Win */
        public int gameFriendlyFireKills { get; set; } = 10;  /* Game Friendly Fire Kills allow before punt. */
        public int gameTimeLimit { get; set; } = 22;  /* Time limit per game, minutes */
        public int gameStartDelay { get; set; } = 2;   /* Game start delay (minutes) */
        public int gameRespawnTime { get; set; } = 20;  /* Respawn Time in Minutes */
        public int gameScoreBoardDelay { get; set; } = 20;  /* Score Board Delay in Seconds */
        public int gamePSPTOTimer { get; set; } = 20;  /* PSP Take Over Timer in Seconds */
        public int gameFlagReturnTime { get; set; } = 4;   /* Flag return time in minutes */
        public int gameMaxTeamLives { get; set; } = 20;  /* Max Team Lives */

        // Game Settings: Misc Settings
        public bool gameOneShotKills { get; set; } = false;
        public bool gameFatBullets { get; set; } = false;
        public bool gameDestroyBuildings { get; set; } = false;
        public bool gameAllowLeftLeaning { get; set; } = true;

        // Game Settings: Ping Settings
        public bool gameMinPing { get; set; } = false;
        public bool gameMaxPing { get; set; } = false;
        public int gameMinPingValue { get; set; } = 0;
        public int gameMaxPingValue { get; set; } = 0;

        // Game Settings: Game Options
        public bool gameOptionAutoBalance { get; set; } = true;
        public bool gameOptionFF { get; set; } = false;
        public bool gameOptionFFWarn { get; set; } = false;
        public bool gameOptionFriendlyTags { get; set; } = true;
        public bool gameOptionShowTracers { get; set; } = false;
        public bool gameShowTeamClays { get; set; } = true;
        public bool gameOptionAutoRange { get; set; } = false;

        // WebStats Variables
        public bool WebStatsEnabled { get; set; } = false;         // Enable/Disable Babstats WebStats
        public string WebStatsServerPath { get; set; } = string.Empty;  // URL for the BabStats WebStats
        public string WebStatsProfileID { get; set; } = string.Empty;  // Profile ID for the WebStats
        public bool WebStatsAnnouncements { get; set; } = false;         // Enable/Disable WebStats Announcements/Ranks
        public int WebStatsReportInterval { get; set; } = 60;            // Interval in seconds for the WebStats Report Request
        public int WebStatsUpdateInterval { get; set; } = 60;            // Interval in seconds for the WebStats Update Request
        public bool WebStatsEnableMinReqPlayers { get; set; } = false; // Enable/Disable Minimum Required Players for WebStats Update
        public int WebStatsMinReqPlayers { get; set; } = 2;          // Minimum Required Players for WebStats Update

        // Weapon Restrictions
        public bool weaponColt45 { get; set; } = true;
        public bool weaponM9Beretta { get; set; } = true;
        public bool weaponCar15 { get; set; } = true;
        public bool weaponCar15203 { get; set; } = true;
        public bool weaponM16 { get; set; } = true;
        public bool weaponM16203 { get; set; } = true;
        public bool weaponG3 { get; set; } = true;
        public bool weaponG36 { get; set; } = true;
        public bool weaponM60 { get; set; } = true;
        public bool weaponM240 { get; set; } = true;
        public bool weaponMP5 { get; set; } = true;
        public bool weaponSAW { get; set; } = true;
        public bool weaponMCRT300 { get; set; } = true;
        public bool weaponM21 { get; set; } = true;
        public bool weaponM24 { get; set; } = true;
        public bool weaponBarrett { get; set; } = true;
        public bool weaponPSG1 { get; set; } = true;
        public bool weaponShotgun { get; set; } = true;
        public bool weaponFragGrenade { get; set; } = true;
        public bool weaponSmokeGrenade { get; set; } = true;
        public bool weaponFlashGrenade { get; set; } = true;
        public bool weaponSatchelCharges { get; set; } = true;
        public bool weaponAT4 { get; set; } = true;
        public bool weaponClaymore { get; set; } = true;

        // Role Restrictions
        public bool roleCQB { get; set; } = true;
        public bool roleGunner { get; set; } = true;
        public bool roleMedic { get; set; } = true;
        public bool roleSniper { get; set; } = true;

        // Game Info
        public TimeSpan gameInfoTimeRemaining { get; set; }                     /* Time Remaining in Game, used for the Info Panel */
        public string gameInfoMapName { get; set; } = "";                       /* Map Name, used for the Info Panel */
        public string gameInfoGameType { get; set; } = "";                      /* Game Type, used for the Info Panel */
        public int gameInfoCurrentNumPlayers { get; set; } = 0;                 /* Current Number of Players, used for the Info Panel */
        public int gameInfoCurrentGameWinCond { get; set; } = 0;                /* Current Game Score, used for the Info Panel */
        public bool gameInfoCurrentGameDefendingTeamBlue { get; set; }          /* Current Game Defending Team, used for the Info Panel */
        public int gameInfoCurrentBlueScore { get; set; } = 0;                  /* Current Blue Score, used for the Info Panel */
        public int gameInfoCurrentRedScore { get; set; } = 0;                   /* Current Red Score, used for the Info Panel */
        public int gameInfoMapCycleIndex { get; set; } = 0;                     /* Map Cycle Index, used for the Info Panel */
        public int gameInfoCurrentMapIndex { get; set; } = 0;                   /* Current Map Index, used for the Info Panel */
        public int gameInfoNextMapGameType { get; set; } = 0;                   /* Next Map Game Type, used for the Info Panel */

        // Instance Update Statements
        public bool banInstanceUpdated { get; set; } = false;


        // Player Team Information
        public Dictionary<int, playerObject> playerList { get; set; } = new();
        public List<playerTeamObject> playerChangeTeamList { get; set; } = new();
        public List<playerTeamObject> playerPreviousTeamList { get; set; } = new();
    }

    public enum InstanceStatus
    {
        OFFLINE = 0,
        LOADINGMAP = 1,
        STARTDELAY = 2,
        ONLINE = 3,
        SCORING = 4,
    }

}
