using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.ComponentModel;
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
        private static banInstance          banInstance     => CommonCore.instanceBans!;
        private static statInstance         statsInstance   => CommonCore.instanceStats!;
        private static playerInstance       playerInstance  => CommonCore.instancePlayers!;

        // Lock for thread safety (if needed for shared resources)
        private static int isTickerRunning = 0;

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        public static void runTicker()
        {
            // Skip this tick if the previous one is still running
            if (Interlocked.CompareExchange(ref isTickerRunning, 1, 0) != 0)
            {
                AppDebug.Log("tickerServerManagement", "Skipping tick - previous tick still running");
                return;
            }

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
                    if (!StartServer.CheckForExistingProcess())
                    {
                        theInstance.instanceStatus = InstanceStatus.OFFLINE;
                    }
                } else
                {
                    // --- Server is online: run status-specific logic in order ---
                    // 1. Always update status and basic info
                    ServerMemory.ReadMemoryServerStatus();                                  // Server Status
                }

                // UI updates that should always run
                SafeInvoke(thisServer, () =>
                {
                    // --- UI Update Hooks ---
                    thisServer.AdminTab.tickerAdmin_Tick();                                         // Update Admin Tab
                    thisServer.MapsTab.UpdateCurrentMapHighlighting();                              // Current Map Highlighting Update
                });

                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                {
                    // If the server is offline, we can skip the rest of the processing
                    theInstance.instanceNextUpdateTime = currentTime.AddSeconds(5);
                    theInstance.instanceLastUpdateTime = currentTime;
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
                    ServerMemory.ReadMapCycleCounter();                                 // Map Cycle Counter (How Maps Have Been Played)
                    ServerMemory.ReadMemoryCurrentMapIndex();                           // Read current map index
                    // Score Reading
                    ServerMemory.ReadMemoryCurrentGameWinConditions();                  // Read Current Game Win Conditions
                    ServerMemory.ReadMemoryCurrentGameScores();                         // Read Current Game Scores
                }

                // 2. Loading Map
                if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                {
                    theInstance.instanceScoringProcRun = true;
                    theInstance.instanceCrashCounter = 0;                               // Reset crash counter
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
                    }
                    ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
                    ServerMemory.GetNextMapType();                                      // Grab the Current Map Type and the Next Map Type

                }
                // 4. Online (game in progress)
                else if (theInstance.instanceStatus == InstanceStatus.ONLINE)
                {
                    theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag
                    ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
                    ServerMemory.GetNextMapType();                                      // Grab the Current Map Type and the Next Map Type

                    // Stats update
                    statsInstanceManager.RunPlayerStatsUpdate();                               // Collect Player Stats
                
                    // WebStats Updates and Reports
                    if (theInstance.WebStatsEnabled)
                    {
                        if (DateTime.Now > statsInstance.lastPlayerStatsUpdate.AddSeconds(theInstance.WebStatsUpdateInterval))
                        {
                            statsInstance.lastPlayerStatsUpdate = DateTime.Now;
                            Task.Run(() => statsInstanceManager.SendUpdateData(thisServer));
                        
                        }
                        if (DateTime.Now > statsInstance.lastPlayerStatsReport.AddSeconds(theInstance.WebStatsReportInterval) && theInstance.WebStatsAnnouncements)
                        {
                            Task.Run(async () =>
                            {
                                statsInstance.lastPlayerStatsReport = DateTime.Now;
                                string ReportResults = await statsInstanceManager.SendReportData(thisServer);
                                // handle ReportResults if needed
                            });
                        
                        }
                    }
                }
                // 5. Scoring
                else if (theInstance.instanceStatus == InstanceStatus.SCORING)
                {
                    tickerEvent_scoringGameProcessing();                                // Run scoring processing
                    ServerMemory.UpdatePlayerTeam();                                    // Move players to their teams if applicable     
                }

                theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
                theInstance.instanceLastUpdateTime = currentTime;
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
                chatInstance.ChatLog?.Clear();
                playerInstance.PlayerList.Clear();
                statsInstanceManager.ResetPlayerStats();

                // New MatchID
                theInstanceManager.GenerateMatchID();

                // Update the Global Game Type (Pinger Reasons)
                ServerMemory.UpdateGlobalGameType();
                // Update the Scores Required to Win the Game
                ServerMemory.UpdateGameScores();
                AppDebug.Log("tickerServerManagement", "Pre-game Complete");
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
				Task.Run(() => statsInstanceManager.SendImportData(thisServer!));
                // Set the Next Map Type
                ServerMemory.SetNextMapType();
                // Update the Scores Required to Win the Next Game
                ServerMemory.UpdateGameScores();
				// Scoreboard Delay Ticker
				int scoreboardDelay = theInstance.gameScoreBoardDelay * 1000;
				CommonCore.Ticker?.StartOnce("ScoreboardTicker", scoreboardDelay, () => tickerEvent_Scoreboard());
				// Log Completion
				AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");
            }
            

        }

        // --- Scoreboard Ticker ---
        private static void tickerEvent_Scoreboard()
        {
            ServerMemory.UpdateScoreBoardTimer();                                   // Set the scoreboard timer to 1 second.
            AppDebug.Log("tickerServerManagement", "Scoreboard Timer Complete");    // Log the completion of the scoreboard timer
            return;                                                                 // Should return to the main ticker loop
        }

    }
}