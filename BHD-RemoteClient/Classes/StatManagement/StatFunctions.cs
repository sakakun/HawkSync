using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;

namespace BHD_RemoteClient.Classes.StatManagement
{
    public static class StatFunctions
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static statInstance instanceStats => CommonCore.instanceStats!;

        // Throttle updates to once every 15 seconds
        private static DateTime _lastPlayerStatsUpdate = DateTime.MinValue;
        // Throttle updates to once every 15 seconds
        private static DateTime _lastWeaponStatsUpdate = DateTime.MinValue;

        public static void PopulatePlayerStatsGrid()
        {
            if ((DateTime.UtcNow - _lastPlayerStatsUpdate).TotalSeconds < 15)
                return;
            _lastPlayerStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewPlayerStats = thisServer.StatsTab.dataGridViewPlayerStats;

            if (dataGridViewPlayerStats.InvokeRequired)
            {
                dataGridViewPlayerStats.Invoke(new Action(() => PopulatePlayerStatsGrid()));
                return;
            }

            dataGridViewPlayerStats.Rows.Clear();

            foreach (var statObj in instanceStats.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                var shotsPerKill = player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
                bool playerActive = theInstance.playerList.Values.Any(p => p.PlayerName == player.PlayerName);

                dataGridViewPlayerStats.Rows.Add(
                    player.PlayerName,
                    player.stat_Suicides,
                    player.stat_Murders,
                    player.stat_Kills,
                    player.stat_Deaths,
                    player.stat_ZoneTime,
                    player.stat_FBCaptures,
                    player.stat_FlagSaves,
                    player.stat_ADTargetsDestroyed,
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
            if ((DateTime.UtcNow - _lastWeaponStatsUpdate).TotalSeconds < 15)
                return;
            _lastWeaponStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewWeaponStats = thisServer.StatsTab.dataGridViewWeaponStats;

            if (dataGridViewWeaponStats.InvokeRequired)
            {
                dataGridViewWeaponStats.Invoke(new Action(() => PopulateWeaponStatsGrid()));
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
    }
}
