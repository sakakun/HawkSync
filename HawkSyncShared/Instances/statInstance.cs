using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabStats;

namespace HawkSyncShared.Instances
{
    public class statInstance
    {
        // Current match/player stat tracking
        public Dictionary<string, PlayerStatObject> playerStatsList { get; set; } = new();

        // Web Stats Log Records
        public List<StatReportObject> WebStatsLog { get; set; } = new();

        // --------------------------------------------------------------------
        // NEW: Multi-endpoint Babstats runtime state
        // --------------------------------------------------------------------
        public List<BabstatsServerSettings> BabstatsServers { get; set; } = new();
        public List<LobbyServerSettings> LobbyServers { get; set; } = new();
		public Dictionary<int, BabstatsServerRuntimeState> BabstatsServerState { get; set; } = new();

        /// <summary>
        /// Returns only enabled servers in deterministic UI/order sequence.
        /// </summary>
        public IEnumerable<BabstatsServerSettings> GetEnabledBabstatsServers()
        {
            return BabstatsServers
                .Where(s => s.IsEnabled)
                .OrderBy(s => s.SortOrder)
                .ThenBy(s => s.BabstatsServerID);
        }

        /// <summary>
        /// Retrieves a collection of lobby server settings that are currently enabled.
        /// </summary>
        /// <returns>An enumerable collection of enabled lobby server settings, ordered by sort order and lobby server ID.</returns>
        public IEnumerable<LobbyServerSettings> GetEnabledLobbyServers()
        {
            return LobbyServers
                .Where(s => s.IsEnabled)
                .OrderBy(s => s.SortOrder)
                .ThenBy(s => s.LobbyServerID);
		}

        /// <summary>
        /// Get or create runtime state for a Babstats server id.
        /// </summary>
        public BabstatsServerRuntimeState GetOrCreateServerState(int babstatsServerID)
        {
            if (!BabstatsServerState.TryGetValue(babstatsServerID, out var state))
            {
                state = new BabstatsServerRuntimeState();
                BabstatsServerState[babstatsServerID] = state;
            }

            return state;
        }

        public void TrimWebStatsLog()
        {
            var cutoff = DateTime.UtcNow.AddHours(-1);
            WebStatsLog.RemoveAll(e => e.ReportDate < cutoff);
        }

        /// <summary>
        /// Returns one designated announcer endpoint (if any) from enabled servers.
        /// Policy: first enabled server with announcements enabled by SortOrder, then BabstatsServerID.
        /// </summary>
        public BabstatsServerSettings? GetDesignatedAnnouncementServer()
        {
            return GetEnabledBabstatsServers()
                .FirstOrDefault(s => s.EnableAnnouncements);
        }

        // Force Update Flag
        public bool ForceUIUpdate { get; set; } 
    }

    public class BabstatsServerRuntimeState
    {
        public DateTime LastPlayerStatsUpdate { get; set; } = DateTime.MinValue;
        public DateTime LastPlayerStatsReport { get; set; } = DateTime.MinValue;
    }

    public class PlayerStatObject
    {
        public required PlayerObject PlayerStatsCurrent { get; set; }
        public required PlayerObject PlayerStatsPrevious { get; set; }
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