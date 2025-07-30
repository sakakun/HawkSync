using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            DataGridView dataGridViewPlayerStats = thisServer.dataGridViewPlayerStats;

            if (dataGridViewPlayerStats.InvokeRequired)
            {
                dataGridViewPlayerStats.Invoke(new Action(() => PopulatePlayerStatsGrid()));
                return;
            }

            if (dataGridViewPlayerStats.Columns.Count == 0)
            {
                dataGridViewPlayerStats.Columns.Add("PlayerName", "Player Name");
                dataGridViewPlayerStats.Columns.Add("Suicides", "Suicides");
                dataGridViewPlayerStats.Columns.Add("Murders", "Murders");
                dataGridViewPlayerStats.Columns.Add("Kills", "Kills");
                dataGridViewPlayerStats.Columns.Add("Deaths", "Deaths");
                dataGridViewPlayerStats.Columns.Add("ZoneTime", "Zone Time");
                dataGridViewPlayerStats.Columns.Add("FBCaptures", "Flag Captures");
                dataGridViewPlayerStats.Columns.Add("FlagSaves", "Flag Saves");
                dataGridViewPlayerStats.Columns.Add("ADTargetsDestroyed", "Targets Destroyed");
                dataGridViewPlayerStats.Columns.Add("RevivesReceived", "Revives Received");
                dataGridViewPlayerStats.Columns.Add("RevivesGiven", "Revives Given");
                dataGridViewPlayerStats.Columns.Add("PSPAttempts", "PSP Attempts");
                dataGridViewPlayerStats.Columns.Add("PSPTakeovers", "PSP Takeovers");
                dataGridViewPlayerStats.Columns.Add("FBCarrierKills", "FB Carrier Kills");
                dataGridViewPlayerStats.Columns.Add("DoubleKills", "Double Kills");
                dataGridViewPlayerStats.Columns.Add("Headshots", "Headshots");
                dataGridViewPlayerStats.Columns.Add("KnifeKills", "Knife Kills");
                dataGridViewPlayerStats.Columns.Add("SniperKills", "Sniper Kills");
                dataGridViewPlayerStats.Columns.Add("TKOTHDefenseKills", "TKOTH Defense Kills");
                dataGridViewPlayerStats.Columns.Add("TKOTHAttackKills", "TKOTH Attack Kills");
                dataGridViewPlayerStats.Columns.Add("ShotsPerKill", "Shots Per Kill");
                dataGridViewPlayerStats.Columns.Add("ExperiencePoints", "Experience Points");
                dataGridViewPlayerStats.Columns.Add("PlayerTeam", "Player Team");
                dataGridViewPlayerStats.Columns.Add("PlayerActive", "Active");
                dataGridViewPlayerStats.Columns.Add("TimePlayed", "Time Played (s)");
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
            foreach (DataGridViewColumn col in dataGridViewPlayerStats.Columns)
            {
                col.HeaderCell.ToolTipText = col.HeaderText;
            }

            dataGridViewPlayerStats.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPlayerStats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPlayerStats.RowHeadersVisible = false;
            dataGridViewPlayerStats.Columns[0].MinimumWidth = 100;

            for (int i = 1; i < dataGridViewPlayerStats.Columns.Count; i++)
            {
                dataGridViewPlayerStats.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        public static void PopulateWeaponStatsGrid()
        {
            if ((DateTime.UtcNow - _lastWeaponStatsUpdate).TotalSeconds < 15)
                return;
            _lastWeaponStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewWeaponStats = thisServer.dataGridViewWeaponStats;

            if (dataGridViewWeaponStats.InvokeRequired)
            {
                dataGridViewWeaponStats.Invoke(new Action(() => PopulateWeaponStatsGrid()));
                return;
            }

            if (dataGridViewWeaponStats.Columns.Count == 0)
            {
                dataGridViewWeaponStats.Columns.Add("PlayerName", "Player Name");
                dataGridViewWeaponStats.Columns.Add("WeaponName", "Weapon Name");
                dataGridViewWeaponStats.Columns.Add("Timer", "Time Used (s)");
                dataGridViewWeaponStats.Columns.Add("Kills", "Kills");
                dataGridViewWeaponStats.Columns.Add("Shots", "Shots");

                foreach (DataGridViewColumn col in dataGridViewWeaponStats.Columns)
                {
                    col.HeaderCell.ToolTipText = col.HeaderText;
                }

                dataGridViewWeaponStats.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewWeaponStats.Columns[0].MinimumWidth = 100;

                for (int i = 1; i < dataGridViewWeaponStats.Columns.Count; i++)
                {
                    dataGridViewWeaponStats.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
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

            dataGridViewWeaponStats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewWeaponStats.RowHeadersVisible = false;
        }
    }
}
