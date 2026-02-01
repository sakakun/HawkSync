using HawkSyncShared.ObjectClasses;
using System;
using System.Collections.Generic;

namespace HawkSyncShared.Instances
{
    public class statInstance
    {

        public DateTime lastPlayerStatsUpdate { get; set; } = DateTime.MinValue;
        public DateTime lastPlayerStatsReport { get; set; } = DateTime.MinValue;
        public Dictionary<string, PlayerStatObject> playerStatsList { get; set; } = new Dictionary<string, PlayerStatObject>();

        // Web Stats Log Records
        public List<StatReportObject> WebStatsLog { get; set; } = new List<StatReportObject>();

        // Force Update Flag
        public bool ForceUIUpdate { get; set; } = false;

    }

    public class PlayerStatObject
    {
        public required playerObject PlayerStatsCurrent { get; set; }
        public required playerObject PlayerStatsPrevious { get; set; }
        public required List<WeaponStatObject> PlayerWeaponStats { get; set; }
    }

    public class WeaponStatObject
    {
        public int WeaponID { get; set; }
        public int TimeUsed { get; set; }
        public int Kills { get; set; }
        public int Shots { get; set; }
    }

    public class StatReportObject
    {
        public required DateTime ReportDate { get; set; }
        public required string ReportContent { get; set; }
    }

}