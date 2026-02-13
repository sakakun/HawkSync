using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HawkSyncShared.Instances
{
    public class theInstance
    {
        // Instance Specific
        public InstanceStatus   instanceStatus              { get; set; } = InstanceStatus.OFFLINE;
        [JsonIgnore]
        public int?             instanceAttachedPID         { get; set; }
        [JsonIgnore]
        public nint             instanceProcessHandle       { get; set; }
        [JsonIgnore]
        public int              instanceCrashCounter        { get; set; } = 0;

        // Instance Tick Flow
        [JsonIgnore]
        public bool             instancePreGameProcRun      { get; set; } = true;
        [JsonIgnore]
        public bool             instanceScoringProcRun      { get; set; } = true;
        [JsonIgnore]
        public DateTime         instanceLastUpdateTime      { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime         instanceNextUpdateTime      { get; set; } = DateTime.Now.AddSeconds(2.0);

        // Profile Server Information
        public int              profileServerType           { get; set; }
        public string           profileServerPath           { get; set; } = String.Empty;
        public string           profileModFileName          { get; set; } = String.Empty;
        public string           profileBindIP               { get; set; } = String.Empty;
        public int              profileBindPort             { get; set; } = 17479;
        public bool             profileEnableRemote         { get; set; } = false; // Enable/Disable Remote Connections
        public int              profileRemotePort           { get; set; } = 9090;  // Remote IP for the server

        // Profile Application Start Attributes
        public bool             profileServerAttribute01    { get; set; } = false;
        public bool             profileServerAttribute02    { get; set; } = false;
        public bool             profileServerAttribute03    { get; set; } = false;
        public bool             profileServerAttribute04    { get; set; } = false;
        public bool             profileServerAttribute05    { get; set; } = false;
        public bool             profileServerAttribute06    { get; set; } = false;
        public bool             profileServerAttribute07    { get; set; } = true;
        public bool             profileServerAttribute08    { get; set; } = true;
		public bool             profileServerAttribute09    { get; set; } = false;
        public bool             profileServerAttribute10    { get; set; } = false;
        public bool             profileServerAttribute11    { get; set; } = false;
        public bool             profileServerAttribute12    { get; set; } = false;
        public bool             profileServerAttribute13    { get; set; } = false;
        public bool             profileServerAttribute14    { get; set; } = false;
        public bool             profileServerAttribute15    { get; set; } = false;
        public bool             profileServerAttribute16    { get; set; } = false;
        public bool             profileServerAttribute17    { get; set; } = false;
        public bool             profileServerAttribute18    { get; set; } = true;
        public bool             profileServerAttribute19    { get; set; } = false;
        public bool             profileServerAttribute20    { get; set; } = false;
        public bool             profileServerAttribute21    { get; set; } = true;

		// Game Settings
        public int              gameMatchID                 { get; set; } = 0;
		public int              gameMatchWinner             { get; set; } = 0;
        public string           gameServerName              { get; set; } = "Untitled Server";
        public string           gameMOTD                    { get; set; } = "Welcome to the server!";
        public string           gameCountryCode             { get; set; } = "US";
        public string           gameHostName                { get; set; } = "HostName";
        public bool             gameDedicated               { get; set; } = true;
        public bool             gameWindowedMode            { get; set; } = true;
        public string           gamePasswordLobby           { get; set; } = "";
        public string           gamePasswordBlue            { get; set; } = "";
        public string           gamePasswordRed             { get; set; } = "";
        public int              gameMaxSlots                { get; set; } = 50;  /* Max Players 50 */
        public int              gameLoopMaps                { get; set; } = 1;   /* 0 = Play One Map, 1 = Loop Maps, 2 = Cycle Maps */
        public bool             gameRequireNova             { get; set; } = false;
        public bool             gameCustomSkins             { get; set; } = false;
        public int              gameScoreKills              { get; set; } = 20;  /* Game Score needed for T/DM Matches to Win */
        public int              gameScoreFlags              { get; set; } = 10;  /* Game Score needed for CTF & FB to Win */
        public int              gameScoreZoneTime           { get; set; } = 10;  /* Game Score needed for T/KOTH to Win */
        public int              gameFriendlyFireKills       { get; set; } = 10;  /* Game Friendly Fire Kills allow before punt. */
        public int              gameTimeLimit               { get; set; } = 22;  /* Time limit per game, minutes */
        public int              gameStartDelay              { get; set; } = 2;   /* Game start delay (minutes) */
        public int              gameRespawnTime             { get; set; } = 20;  /* Respawn Time in Minutes */
        public int              gameScoreBoardDelay         { get; set; } = 20;  /* Score Board Delay in Seconds */
        public int              gamePSPTOTimer              { get; set; } = 20;  /* PSP Take Over Timer in Seconds */
        public int              gameFlagReturnTime          { get; set; } = 4;   /* Flag return time in minutes */
        public int              gameMaxTeamLives            { get; set; } = 20;  /* Max Team Lives */

        // Game Settings: Misc Settings
        public bool             gameOneShotKills            { get; set; } = false;
        public bool             gameFatBullets              { get; set; } = false;
        public bool             gameDestroyBuildings        { get; set; } = false;
        public bool             gameAllowLeftLeaning        { get; set; } = true;

        // Game Settings: Ping Settings
        public bool             gameMinPing                 { get; set; } = false;
        public bool             gameMaxPing                 { get; set; } = false;
        public int              gameMinPingValue            { get; set; } = 0;
        public int              gameMaxPingValue            { get; set; } = 0;

        // Game Settings: Game Options
        public bool             gameOptionAutoBalance       { get; set; } = true;
        public bool             gameOptionFF                { get; set; } = false;
        public bool             gameOptionFFWarn            { get; set; } = false;
        public bool             gameOptionFriendlyTags      { get; set; } = true;
        public bool             gameOptionShowTracers       { get; set; } = false;
        public bool             gameShowTeamClays           { get; set; } = true;
        public bool             gameOptionAutoRange         { get; set; } = false;

        // WebStats Variables
        public bool             WebStatsEnabled             { get; set; } = false;         // Enable/Disable Babstats WebStats
        public string           WebStatsServerPath          { get; set; } = string.Empty;  // URL for the BabStats WebStats
        public string           WebStatsProfileID           { get; set; } = string.Empty;  // Profile ID for the WebStats
        public bool             WebStatsAnnouncements       { get; set; } = false;         // Enable/Disable WebStats Announcements/Ranks
        public int              WebStatsReportInterval      { get; set; } = 60;            // Interval in seconds for the WebStats Report Request
        public int              WebStatsUpdateInterval      { get; set; } = 60;            // Interval in seconds for the WebStats Update Request

        // Weapon Restrictions
        public bool             weaponColt45                { get; set; } = true;
        public bool             weaponM9Beretta             { get; set; } = true;
        public bool             weaponCar15                 { get; set; } = true;
        public bool             weaponCar15203              { get; set; } = true;
        public bool             weaponM16                   { get; set; } = true;
        public bool             weaponM16203                { get; set; } = true;
        public bool             weaponG3                    { get; set; } = true;
        public bool             weaponG36                   { get; set; } = true;
        public bool             weaponM60                   { get; set; } = true;
        public bool             weaponM240                  { get; set; } = true;
        public bool             weaponMP5                   { get; set; } = true;
        public bool             weaponSAW                   { get; set; } = true;
        public bool             weaponMCRT300               { get; set; } = true;
        public bool             weaponM21                   { get; set; } = true;
        public bool             weaponM24                   { get; set; } = true;
        public bool             weaponBarrett               { get; set; } = true;
        public bool             weaponPSG1                  { get; set; } = true;
        public bool             weaponShotgun               { get; set; } = true;
        public bool             weaponFragGrenade           { get; set; } = true;
        public bool             weaponSmokeGrenade          { get; set; } = true;
        public bool             weaponFlashGrenade          { get; set; } = true;
        public bool             weaponSatchelCharges        { get; set; } = true;
        public bool             weaponAT4                   { get; set; } = true;
        public bool             weaponClaymore              { get; set; } = true;

        // Role Restrictions
        public bool             roleCQB                     { get; set; } = true;
        public bool             roleGunner                  { get; set; } = true;
        public bool             roleMedic                   { get; set; } = true;
        public bool             roleSniper                  { get; set; } = true;

        // Game Info
        public TimeSpan         gameInfoTimeRemaining       { get; set; }       /* Time Remaining in Game, used for the Info Panel */
        [Obsolete("This property is deprecated. Use instanceMaps.CurrentGameType instead.")]
        public int              gameInfoGameType            { get; set; }       /* Game Type, used for the Info Panel */
        public int              gameInfoNumPlayers          { get; set; } = 0;  /* Current Number of Players, used for the Info Panel */
        public int              gameInfoWinCond             { get; set; } = 0;  /* Current Game Score, used for the Info Panel */
        public bool             gameInfoIsBlueDefending     { get; set; }       /* Current Game Defending Team, used for the Info Panel */
        public int              gameInfoBlueScore           { get; set; } = 0;  /* Current Blue Score, used for the Info Panel */
        public int              gameInfoRedScore            { get; set; } = 0;  /* Current Red Score, used for the Info Panel */
        public int              gameInfoMapCycleIndex       { get; set; } = 0;  /* Map Cycle Index, used for the Info Panel */

        // Proxy Checking Settings
        public bool             proxyCheckEnabled           { get; set; } = false;
        public string           proxyCheckAPIKey            { get; set; } = string.Empty;
        public decimal          proxyCheckCacheTime         { get; set; } = 60; // In Days
        public int              proxyCheckProxyAction       { get; set; } = 0;  // 0 = Nothing, 1 = Kick, 2 = Ban
        public int              proxyCheckVPNAction         { get; set; } = 0;  // 0 = Nothing, 1 = Kick, 2 = Ban
        public int              proxyCheckTORAction         { get; set; } = 0;  // 0 = Nothing, 1 = Kick, 2 = Ban
        public int              proxyCheckGeoMode           { get; set; } = 0;  // 0 = Nothing, 1 = Block, 2 = Allow
        public int              proxyCheckServiceProvider   { get; set; } = 0;  // 0 = None, 1 = ProxyCheck.io

        // NetLimiter Settings
        public bool             netLimiterEnabled           { get; set; } = false;
        public string           netLimiterHost              { get; set; } = string.Empty;
        public int              netLimiterPort              { get; set; } = 9098;
        public string           netLimiterUsername          { get; set; } = string.Empty;
        public string           netLimiterPassword          { get; set; } = string.Empty;
        public string           netLimiterFilterName        { get; set; } = string.Empty;
        public bool             netLimiterEnableConLimit    { get; set; } = false;
        public decimal          netLimiterConThreshold      { get; set; } = 10; // Number of Connections

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
