using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.DTOs.tabStats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HawkSyncShared.Instances
{
    public class statInstance
    {
        // --------------------------------------------------------------------
        // Transitional legacy scheduling fields (kept for compatibility)
        // Remove after ticker/send pipeline is fully migrated to per-server state.
        // --------------------------------------------------------------------
        public DateTime lastPlayerStatsUpdate { get; set; } = DateTime.MinValue;
        public DateTime lastPlayerStatsReport { get; set; } = DateTime.MinValue;

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
		/// Ensure runtime state entries exist for all configured servers.
		/// Call after loading/reloading server list from DB.
		/// </summary>
		public void EnsureBabstatsRuntimeState()
        {
            var validIds = new HashSet<int>(BabstatsServers.Select(s => s.BabstatsServerID));

            foreach (var server in BabstatsServers)
            {
                if (!BabstatsServerState.ContainsKey(server.BabstatsServerID))
                {
                    BabstatsServerState[server.BabstatsServerID] = new BabstatsServerRuntimeState();
                }
            }

            var staleIds = BabstatsServerState.Keys.Where(id => !validIds.Contains(id)).ToList();
            foreach (var staleId in staleIds)
            {
                BabstatsServerState.Remove(staleId);
            }
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
            var cutoff = DateTime.Now.AddHours(-1);
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
        public bool ForceUIUpdate { get; set; } = false;
    }

    public class BabstatsServerRuntimeState
    {
        public DateTime LastPlayerStatsUpdate { get; set; } = DateTime.MinValue;
        public DateTime LastPlayerStatsReport { get; set; } = DateTime.MinValue;
        public int ConsecutiveFailureCount { get; set; } = 0;
        public string LastError { get; set; } = string.Empty;
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

        // Optional context fields for multi-endpoint logging
        public int? BabstatsServerID { get; set; }
        public string? BabstatsServerName { get; set; }
    }
}