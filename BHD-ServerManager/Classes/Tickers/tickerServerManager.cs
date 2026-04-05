using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabStats;
using HawkSyncShared.Instances;
using System.Diagnostics;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerServerManager
    {
        // The Instances (Data)
        private static ServerManagerUI?     thisServer      => Program.ServerManagerUI;
        private static theInstance          theInstance     => CommonCore.theInstance!;
        private static mapInstance          mapInstance     => CommonCore.instanceMaps!;
        private static chatInstance         chatInstance    => CommonCore.instanceChat!;
        // ReSharper disable once UnusedMember.Local
        private static banInstance          banInstance     => CommonCore.instanceBans!;
        private static statInstance         statsInstance   => CommonCore.instanceStats!;
        private static playerInstance       playerInstance  => CommonCore.instancePlayers!;

        // Lock for thread safety (if needed for shared resources)
        private static int isTickerRunning;

        // Runtime state for lobby server heartbeats
        private static readonly Dictionary<int, DateTime> lobbyServerHeartbeatTimes = new();

        public static void runTicker()
        {
            // Skip this tick if the previous one is still running
            if (Interlocked.CompareExchange(ref isTickerRunning, 1, 0) != 0)
                return;

            try
            {
                if (thisServer == null)
                    return;

                // ═══════════════════════════════════════════════════════════
                // CHECK AND MANAGE EMBEDDED API
                // ═══════════════════════════════════════════════════════════
                // This runs every tick (500ms) to ensure API state matches configuration
                theInstanceManager.ManageEmbeddedApi();

                // Only run the rest if it's time for an update
                DateTime currentTime = DateTime.Now;
                if (DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) >= 0)
                    return;

                // If server process is not attached, set status to offline and update UI
                if (!ServerMemory.ReadMemoryIsProcessAttached())
                {
                    theInstance.instanceStatus = InstanceStatus.OFFLINE;
                } else
                {
                    // --- Server is online: run status-specific logic in order ---
                    // 1. Always update status and basic info
                    ServerMemory.ReadMemoryServerStatus();                                  // Server Status
                    ServerMemory.ReadStartDelayTimer();                                     // Start Delay Timer Value
				}

                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                {
                    // If the server is offline, we can skip the rest of the processing
                    theInstance.instanceNextUpdateTime = currentTime.AddSeconds(5);
                    return;
                }

                // 1. General Updates (Always Run When Online)             
                if (theInstance.instanceStatus != InstanceStatus.LOADINGMAP || theInstance.instanceStatus != InstanceStatus.SCORING)
                {
                    // Map Ended, Wait for Updates
                    ServerMemory.UpdateNovaID();                                        // Nova ID Update
                    ServerMemory.ReadMemoryGameTimeLeft();                              // Time Left in the Current Game
                    ServerMemory.ReadMemoryCurrentMissionName();                        // Get Current Mission Name
                    ServerMemory.ReadMemoryCurrentGameType();                           // Get Current Game Type
                    ServerMemory.ReadMemoryCurrentNumPlayers();                         // Get Current Number of Players
                    ServerMemory.ReadMemoryCurrentMapIndex();                           // Read current map index
                    // Score Reading
                    ServerMemory.ReadMemoryCurrentGameWinConditions();                  // Read Current Game Win Conditions
                    ServerMemory.ReadMemoryCurrentGameScores();                         // Read Current Game Scores

                }

                // 2. Loading Map
                if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                {
					theInstance.instanceScoringProcRun = true;
                    tickerEvent_preGameProcessing();                                    // Run pre-game processing
                    ServerMemory.UpdatePlayerTeam();                                    // Move players to their teams if applicable
                }
                // 3. Start Delay
                else if (theInstance.instanceStatus == InstanceStatus.STARTDELAY)
                {
					if (!theInstance.instancePreGameProcRun)
                    {
                        mapInstance.ActualPlayingMapIndex = mapInstance.CurrentMapIndex;    // Set the actual playing map index
                        theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag
                        if (Debugger.IsAttached)
                        {
                             StartServer.readAutoRes();
                        }
                    }
                    ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
                    ServerMemory.GetMapData();                                          // Grab the Current Map Type and the Next Map Type

                }
                // 4. Online (game in progress)
                else if (theInstance.instanceStatus == InstanceStatus.ONLINE)
                {
					theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag                                     

                    ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
                    ServerMemory.GetMapData();                                          // Grab the Current Map Type and the Next Map Type

                    ServerMemory.PollPspState();                                        // 4 Team PSP Polling State
                    ServerMemory.TickFlagScorer();                                      // 4 Team Scoring Checks
                    
                    // Stats update
                    statsInstanceManager.RunPlayerStatsUpdate();                        // Collect Player Stats

                    RunBabstatsOnlineHooks();                                           // Babstats Updates and Reports
                    RunLobbyReportingOnlineHooks();                                     // Lobby Reporting Updates and Reports
				}
                // 5. Scoring
                else if (theInstance.instanceStatus == InstanceStatus.SCORING)
                {
                    tickerEvent_scoringGameProcessing();                                // Run scoring processing
                    ServerMemory.UpdatePlayerTeam();                                    // Move players to their teams if applicable     
                }

                theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
            } 
            finally
            {
                Interlocked.Exchange(ref isTickerRunning, 0);
			}
			
        }

        // --- Pre-Game Processing (Loading Map) ---
        private static void tickerEvent_preGameProcessing()
        {
            if (theInstance.instancePreGameProcRun)
            {   
                theInstance.instancePreGameProcRun = false;   
                
                // Resets
                chatInstance.AutoMessageCounter = 0;
                chatInstance.ChatLog.Clear();
                playerInstance.PlayerList.Clear();
                statsInstanceManager.ResetPlayerStats();

                // New MatchID
                theInstanceManager.GenerateMatchID();

                // Update the Global Game Type (Pinger Reasons)
                ServerMemory.UpdateGlobalGameType();

                // Update the Scores Required to Win the Game
                ServerMemory.UpdateGameScores();
                
            }
            
        }

        // --- Scoring Processing ---
        private static void tickerEvent_scoringGameProcessing()
        {
            if (theInstance.instanceScoringProcRun)
            {
                theInstance.instanceScoringProcRun = false;

                // Read Winning Team
                ServerMemory.ReadMemoryWinningTeam();

				// Final Stats Update
				SendScoringImportToEnabledBabstats();

                // Set the Next Map Type
                ServerMemory.SetNextMapType();

                // Update the Scores Required to Win the Next Game
                ServerMemory.UpdateGameScores();
				// Scoreboard Delay Ticker
				int scoreboardDelay = theInstance.gameScoreBoardDelay * 1000;
				CommonCore.Ticker?.StartOnce("ScoreboardTicker", scoreboardDelay, () => tickerEvent_Scoreboard());
            }
            

        }

        // --- Scoreboard Ticker ---
        private static void tickerEvent_Scoreboard()
        {
            ServerMemory.UpdateScoreBoardTimer();                                   // Set the scoreboard timer to 1 second.
        }

        private static void RunBabstatsOnlineHooks()
        {
            var enabledServers = statsInstance.GetEnabledBabstatsServers().ToList();
            if (enabledServers.Count == 0)
            {
                return;
            }

            BabstatsServerSettings? designatedAnnouncer = statsInstance.GetDesignatedAnnouncementServer();
            DateTime now = DateTime.Now;

            foreach (BabstatsServerSettings server in enabledServers)
            {
                BabstatsServerRuntimeState runtime = statsInstance.GetOrCreateServerState(server.BabstatsServerID);

                if (now > runtime.LastPlayerStatsUpdate.AddSeconds(server.UpdateIntervalSeconds))
                {
                    runtime.LastPlayerStatsUpdate = now;
                    BabstatsServerSettings serverCopy = server;
                    Task.Run(() => statsInstanceManager.SendUpdateData(serverCopy));
                }

                if (!server.EnableAnnouncements)
                {
                    continue;
                }

                if (now > runtime.LastPlayerStatsReport.AddSeconds(server.ReportIntervalSeconds))
                {
                    runtime.LastPlayerStatsReport = now;

                    bool emitAnnouncements = designatedAnnouncer != null &&
                                             designatedAnnouncer.BabstatsServerID == server.BabstatsServerID;

                    BabstatsServerSettings serverCopy = server;
                    if (emitAnnouncements) {
                        Task.Run(() => statsInstanceManager.SendReportData(serverCopy));
                    }
                }
            }
        }

        private static void SendScoringImportToEnabledBabstats()
        {
            var enabledServers = statsInstance.GetEnabledBabstatsServers().ToList();
            if (enabledServers.Count == 0)
            {
                return;
            }

            foreach (BabstatsServerSettings server in enabledServers)
            {
                BabstatsServerSettings serverCopy = server;
                Task.Run(() => statsInstanceManager.SendImportData(serverCopy));
            }
        }

        private static void RunLobbyReportingOnlineHooks()
        { 
            var enabledServers = statsInstance.GetEnabledLobbyServers().ToList();
            if (enabledServers.Count == 0)
            {
                return;
            }

            DateTime now = DateTime.Now;

            foreach (LobbyServerSettings server in enabledServers)
            {
                // Check last heartbeat time for this server
                lobbyServerHeartbeatTimes.TryGetValue(server.LobbyServerID, out DateTime lastHeartbeat);
                if (now > lastHeartbeat.AddSeconds(30))
                {
                    lobbyServerHeartbeatTimes[server.LobbyServerID] = now;
                    LobbyServerSettings serverCopy = server;
                    Task.Run(() => Helpers.LobbyReportHelper.SendHeartbeat(
                        uriString: serverCopy.ServerUri,
                        SKey: serverCopy.SecretKey,
                        reportPort: serverCopy.GamePort.ToString()
                    ));
                }
            }
        }


    }
}