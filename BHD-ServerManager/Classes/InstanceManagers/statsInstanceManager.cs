using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.Instances;
using HawkSyncShared.DTOs.tabPlayers;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class statsInstanceManager
    {
        private static statInstance instanceStats => CommonCore.instanceStats!;
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

        private const int PlayerStatsUpdateThrottleSeconds = 15;
        private const int WeaponStatsUpdateThrottleSeconds = 15;

        private static DateTime _lastPlayerStatsUpdate = DateTime.MinValue;
        private static DateTime _lastWeaponStatsUpdate = DateTime.MinValue;

        public static void RunPlayerStatsUpdate()
        {
            foreach (var playerRecord in playerInstance.PlayerList)
            {
                var playerObj = playerRecord.Value;
                if (playerObj != null && playerObj.PlayerNameBase64 != null)
                {
                    UpdatePlayerStats(playerObj);
                }
            }
        }

        public static void UpdatePlayerStats(PlayerObject CurrentPlayerObject)
        {
            if (CurrentPlayerObject == null || string.IsNullOrEmpty(CurrentPlayerObject.PlayerNameBase64))
                return;

            if (!instanceStats.playerStatsList.TryGetValue(CurrentPlayerObject.PlayerNameBase64, out var statObj))
            {
                statObj = new PlayerStatObject
                {
                    PlayerStatsCurrent = ClonePlayerObject(CurrentPlayerObject),
                    PlayerStatsPrevious = ClonePlayerObject(CurrentPlayerObject),
                    PlayerWeaponStats = new List<WeaponStatObject>()
                };
                instanceStats.playerStatsList[CurrentPlayerObject.PlayerNameBase64] = statObj;
                UpdateWeaponStats(statObj, CurrentPlayerObject, isNew: true);
            }
            else
            {
                statObj.PlayerStatsPrevious = ClonePlayerObject(statObj.PlayerStatsCurrent);
                statObj.PlayerStatsCurrent = ClonePlayerObject(CurrentPlayerObject);
                UpdateWeaponStats(statObj, CurrentPlayerObject, isNew: false);
            }
        }

        private static void UpdateWeaponStats(PlayerStatObject statObj, PlayerObject player, bool isNew)
        {
            if (isNew)
            {
                statObj.PlayerWeaponStats.Add(new WeaponStatObject
                {
                    WeaponID = player.SelectedWeaponID,
                    TimeUsed = player.PlayerTimePlayed,
                    Kills = player.stat_Kills,
                    Shots = player.stat_TotalShotsFired
                });
            }
            else
            {
                var diff = ComparePlayerStats(statObj.PlayerStatsCurrent, statObj.PlayerStatsPrevious);
                var weaponStat = statObj.PlayerWeaponStats
                    .FirstOrDefault(w => w.WeaponID == player.SelectedWeaponID);

                if (weaponStat == null)
                {
                    weaponStat = new WeaponStatObject
                    {
                        WeaponID = player.SelectedWeaponID
                    };
                    statObj.PlayerWeaponStats.Add(weaponStat);
                }

                weaponStat.TimeUsed += diff.PlayerTimePlayed;
                weaponStat.Kills += diff.stat_Kills;
                weaponStat.Shots += diff.stat_TotalShotsFired;
            }
        }

        public static PlayerObject ComparePlayerStats(PlayerObject CurrentPlayerObject, PlayerObject PreviousPlayerObject)
        {
            if (CurrentPlayerObject == null || PreviousPlayerObject == null)
                return new PlayerObject();

            return new PlayerObject
            {
                PlayerSlot = CurrentPlayerObject.PlayerSlot,
                PlayerName = CurrentPlayerObject.PlayerName,
                PlayerNameBase64 = CurrentPlayerObject.PlayerNameBase64,
                PlayerIPAddress = CurrentPlayerObject.PlayerIPAddress,
                PlayerTeam = CurrentPlayerObject.PlayerTeam,
                PlayerPing = CurrentPlayerObject.PlayerPing,
                PlayerTimePlayed = CurrentPlayerObject.PlayerTimePlayed - PreviousPlayerObject.PlayerTimePlayed,
                RoleID = CurrentPlayerObject.RoleID,
                RoleName = CurrentPlayerObject.RoleName,
                ActiveZoneTime = CurrentPlayerObject.ActiveZoneTime - PreviousPlayerObject.ActiveZoneTime,
                stat_Kills = CurrentPlayerObject.stat_Kills - PreviousPlayerObject.stat_Kills,
                stat_Deaths = CurrentPlayerObject.stat_Deaths - PreviousPlayerObject.stat_Deaths,
                stat_ZoneTime = CurrentPlayerObject.stat_ZoneTime - PreviousPlayerObject.stat_ZoneTime,
                stat_ZoneKills = CurrentPlayerObject.stat_ZoneKills - PreviousPlayerObject.stat_ZoneKills,
                stat_ZoneDefendKills = CurrentPlayerObject.stat_ZoneDefendKills - PreviousPlayerObject.stat_ZoneDefendKills,
                stat_RevivesGiven = CurrentPlayerObject.stat_RevivesGiven - PreviousPlayerObject.stat_RevivesGiven,
                stat_FBCaptures = CurrentPlayerObject.stat_FBCaptures - PreviousPlayerObject.stat_FBCaptures,
                stat_Suicides = CurrentPlayerObject.stat_Suicides - PreviousPlayerObject.stat_Suicides,
                stat_Murders = CurrentPlayerObject.stat_Murders - PreviousPlayerObject.stat_Murders,
                stat_Headshots = CurrentPlayerObject.stat_Headshots - PreviousPlayerObject.stat_Headshots,
                stat_KnifeKills = CurrentPlayerObject.stat_KnifeKills - PreviousPlayerObject.stat_KnifeKills,
                stat_RevivesReceived = CurrentPlayerObject.stat_RevivesReceived - PreviousPlayerObject.stat_RevivesReceived,
                stat_PSPAttempts = CurrentPlayerObject.stat_PSPAttempts - PreviousPlayerObject.stat_PSPAttempts,
                stat_PSPTakeovers = CurrentPlayerObject.stat_PSPTakeovers - PreviousPlayerObject.stat_PSPTakeovers,
                stat_DoubleKills = CurrentPlayerObject.stat_DoubleKills - PreviousPlayerObject.stat_DoubleKills,
                stat_FBCarrierKills = CurrentPlayerObject.stat_FBCarrierKills - PreviousPlayerObject.stat_FBCarrierKills,
                stat_FBCarrierDeaths = CurrentPlayerObject.stat_FBCarrierDeaths - PreviousPlayerObject.stat_FBCarrierDeaths,
                stat_ExperiencePoints = CurrentPlayerObject.stat_ExperiencePoints - PreviousPlayerObject.stat_ExperiencePoints,
                stat_SDADTargetsDestroyed = CurrentPlayerObject.stat_SDADTargetsDestroyed - PreviousPlayerObject.stat_SDADTargetsDestroyed,
                stat_SDADDefenseKills = CurrentPlayerObject.stat_SDADDefenseKills - PreviousPlayerObject.stat_SDADDefenseKills,
                stat_SDADAttackKills = CurrentPlayerObject.stat_SDADAttackKills - PreviousPlayerObject.stat_SDADAttackKills,
                stat_FlagSaves = CurrentPlayerObject.stat_FlagSaves - PreviousPlayerObject.stat_FlagSaves,
                stat_SniperKills = CurrentPlayerObject.stat_SniperKills - PreviousPlayerObject.stat_SniperKills,
                stat_TKOTHDefenseKills = CurrentPlayerObject.stat_TKOTHDefenseKills - PreviousPlayerObject.stat_TKOTHDefenseKills,
                stat_TKOTHAttackKills = CurrentPlayerObject.stat_TKOTHAttackKills - PreviousPlayerObject.stat_TKOTHAttackKills,
                stat_TotalShotsFired = CurrentPlayerObject.stat_TotalShotsFired - PreviousPlayerObject.stat_TotalShotsFired,
                PlayerWeapons = new List<string>(CurrentPlayerObject.PlayerWeapons),
                SelectedWeaponID = CurrentPlayerObject.SelectedWeaponID,
                SelectedWeaponName = CurrentPlayerObject.SelectedWeaponName
            };
        }

        private static PlayerObject ClonePlayerObject(PlayerObject obj)
        {
            if (obj == null) return new PlayerObject();
            return new PlayerObject
            {
                PlayerSlot = obj.PlayerSlot,
                PlayerName = obj.PlayerName,
                PlayerNameBase64 = obj.PlayerNameBase64,
                PlayerIPAddress = obj.PlayerIPAddress,
                PlayerTeam = obj.PlayerTeam,
                PlayerPing = obj.PlayerPing,
                PlayerTimePlayed = obj.PlayerTimePlayed,
                RoleID = obj.RoleID,
                RoleName = obj.RoleName,
                stat_Kills = obj.stat_Kills,
                stat_Deaths = obj.stat_Deaths,
                stat_ZoneTime = obj.stat_ZoneTime,
                stat_ZoneKills = obj.stat_ZoneKills,
                stat_ZoneDefendKills = obj.stat_ZoneDefendKills,
                stat_RevivesGiven = obj.stat_RevivesGiven,
                stat_FBCaptures = obj.stat_FBCaptures,
                stat_Suicides = obj.stat_Suicides,
                stat_Murders = obj.stat_Murders,
                stat_Headshots = obj.stat_Headshots,
                stat_KnifeKills = obj.stat_KnifeKills,
                stat_RevivesReceived = obj.stat_RevivesReceived,
                stat_PSPAttempts = obj.stat_PSPAttempts,
                stat_PSPTakeovers = obj.stat_PSPTakeovers,
                stat_DoubleKills = obj.stat_DoubleKills,
                stat_FBCarrierKills = obj.stat_FBCarrierKills,
                stat_FBCarrierDeaths = obj.stat_FBCarrierDeaths,
                stat_ExperiencePoints = obj.stat_ExperiencePoints,
                stat_SDADTargetsDestroyed = obj.stat_SDADTargetsDestroyed,
                stat_FlagSaves = obj.stat_FlagSaves,
                stat_SniperKills = obj.stat_SniperKills,
                stat_TKOTHDefenseKills = obj.stat_TKOTHDefenseKills,
                stat_TKOTHAttackKills = obj.stat_TKOTHAttackKills,
                stat_TotalShotsFired = obj.stat_TotalShotsFired,
                PlayerWeapons = new List<string>(obj.PlayerWeapons),
                SelectedWeaponID = obj.SelectedWeaponID,
                SelectedWeaponName = obj.SelectedWeaponName
            };
        }

        // --- UI Thread-Safe Grid Population ---
        public static void PopulatePlayerStatsGrid()
        {
            if ((DateTime.UtcNow - _lastPlayerStatsUpdate).TotalSeconds < PlayerStatsUpdateThrottleSeconds)
                return;
            _lastPlayerStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewPlayerStats = thisServer.StatsTab.dataGridViewPlayerStats;

            if (dataGridViewPlayerStats.InvokeRequired)
            {
                dataGridViewPlayerStats.Invoke(new Action(PopulatePlayerStatsGrid));
                return;
            }

            dataGridViewPlayerStats.Rows.Clear();

            foreach (var statObj in instanceStats.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                var shotsPerKill = player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
                bool playerActive = playerInstance.PlayerList.Values.Any(p => p.PlayerName == player.PlayerName);

                dataGridViewPlayerStats.Rows.Add(
                    player.PlayerName,
                    player.stat_Suicides,
                    player.stat_Murders,
                    player.stat_Kills,
                    player.stat_Deaths,
                    player.stat_ZoneTime,
                    player.stat_FBCaptures,
                    player.stat_FlagSaves,
                    player.stat_SDADTargetsDestroyed,
                    player.stat_RevivesReceived,
                    player.stat_RevivesGiven,
                    player.stat_PSPAttempts,
                    player.stat_PSPTakeovers,
                    player.stat_FBCarrierKills,
                    player.stat_DoubleKills,
                    player.stat_Headshots,
                    player.stat_KnifeKills,
                    player.stat_SniperKills,
                    player.stat_TKOTHDefenseKills,
                    player.stat_TKOTHAttackKills,
                    shotsPerKill,
                    player.stat_ExperiencePoints,
                    player.PlayerTeam,
                    playerActive ? "1" : "0",
                    player.PlayerTimePlayed
                );
            }
        }

        public static void PopulateWeaponStatsGrid()
        {
            if ((DateTime.UtcNow - _lastWeaponStatsUpdate).TotalSeconds < WeaponStatsUpdateThrottleSeconds)
                return;
            _lastWeaponStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewWeaponStats = thisServer.StatsTab.dataGridViewWeaponStats;

            if (dataGridViewWeaponStats.InvokeRequired)
            {
                dataGridViewWeaponStats.Invoke(new Action(PopulateWeaponStatsGrid));
                return;
            }

            dataGridViewWeaponStats.Rows.Clear();

            foreach (var statObj in instanceStats.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                foreach (var weaponStat in statObj.PlayerWeaponStats)
                {
                    dataGridViewWeaponStats.Rows.Add(
                        player.PlayerName,
                        ((WeaponStack)weaponStat.WeaponID).ToString(),
                        weaponStat.TimeUsed,
                        weaponStat.Kills,
                        weaponStat.Shots
                    );
                }
            }
        }

        // --- Other business logic and network methods remain unchanged ---
        // (GenerateImportData, GenerateGameLine, GeneratePlayerLines, GeneratePlayerStatLine, GenerateWeaponStatLines, GenerateUpdateData, GenerateReportData, ResetPlayerStats, SendImportData, SendUpdateData, SendReportData, TestBabstatsConnectionAsync, SendBabstatsData, AddStatsLogRowSafe)
    }
}