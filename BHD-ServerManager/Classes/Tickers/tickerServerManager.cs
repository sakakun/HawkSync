using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.ComponentModel;
using System.Diagnostics;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerServerManager
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static statInstance instanceStats => CommonCore.instanceStats!;
        private static ServerManager? thisServer => Program.ServerManagerUI;

        // Lock for thread safety (if needed for shared resources)
        private static readonly object tickerLock = new();

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
            if (thisServer == null)
                return;

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
                // Server UI Updates
                thisServer.functionEvent_tickerServerGUI();                                     // Ticker for the Main Server GUI

                // --- UI Update Hooks ---
                thisServer.ProfileTab.tickerProfileTabHook();                                   // Toggle Profile Lock based on server status
                thisServer.ServerTab.tickerServerHook();                                        // Toggle Server Lock based on server status
                thisServer.MapsTab.tickerMapsHook();                                            // Toggle Maps Lock based on server status
                thisServer.PlayersTab.tickerPlayerHook();                                       // Update Players Tab
                thisServer.ChatTab.ChatTickerHook();                                            // Update Chat Tab
                thisServer.BanTab.BanTickerHook();                                              // Update Bans Tab
                thisServer.StatsTab.StatsTickerHook();                                          // Update Stats Tab
            });

            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                // If the server is offline, we can skip the rest of the processing
                theInstance.instanceNextUpdateTime = currentTime.AddSeconds(5);
                theInstance.instanceLastUpdateTime = currentTime;
                return;
            }

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
                theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag
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
                StatFunctions.RunPlayerStatsUpdate();                               // Collect Player Stats
                
                // WebStats Updates and Reports
                if (theInstance.WebStatsEnabled)
                {
                    if (DateTime.Now > instanceStats.lastPlayerStatsUpdate.AddSeconds(theInstance.WebStatsUpdateInterval))
                    {
                        instanceStats.lastPlayerStatsUpdate = DateTime.Now;
                        Task.Run(() => StatFunctions.SendUpdateData(thisServer));
                        
                    }
                    if (DateTime.Now > instanceStats.lastPlayerStatsReport.AddSeconds(theInstance.WebStatsReportInterval) && theInstance.WebStatsAnnouncements)
                    {
                        Task.Run(async () =>
                        {
                            instanceStats.lastPlayerStatsReport = DateTime.Now;
                            string ReportResults = await StatFunctions.SendReportData(thisServer);
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

        // --- Pre-Game Processing (Loading Map) ---
        private static void tickerEvent_preGameProcessing()
        {
            if (theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing...");
                theInstance.instancePreGameProcRun = false;               
                instanceChat.AutoMessageCounter = 0;
                instanceChat.ChatLog?.Clear();
                theInstance.playerList.Clear();
                StatFunctions.ResetPlayerStats();

                AppDebug.Log("tickerServerManagement", "Updating Maps...");
                ServerMemory.UpdateGlobalGameType();
            }
            ServerMemory.UpdateGameScores();                                                // Update the Scores Required to Win the Game
        }

        // --- Scoring Processing ---
        private static void tickerEvent_scoringGameProcessing()
        {
            if (theInstance.instanceScoringProcRun)
            {
                theInstance.instanceScoringProcRun = false;

                AppDebug.Log("tickerServerManagement", "Scoring Processing...");
                ServerMemory.ReadMemoryWinningTeam();
                AppDebug.Log("tickerServerManagement", "Sending Stats...");
                Task.Run(() => StatFunctions.SendImportData(thisServer!));
                ServerMemory.SetNextMapType();     // Set the Next Map Type
                AppDebug.Log("tickerServerManagement", "Kill ScoreBoard...");
                CommonCore.Ticker?.Start("ScoreboardTicker", 1000, () => tickerEvent_Scoreboard());
                AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");
            }
            
            ServerMemory.UpdateGameScores();                                                // Update the Scores Required to Win the Game
        }

        // --- Scoreboard Ticker ---
        private static void tickerEvent_Scoreboard()
        {
            Thread.Sleep(theInstance.gameScoreBoardDelay * 1000);                   // Wait for the specified delay
            ServerMemory.UpdateScoreBoardTimer();                                   // Set the scoreboard timer to 1 second.
            AppDebug.Log("tickerServerManagement", "Scoreboard Timer Complete");    // Log the completion of the scoreboard timer
            CommonCore.Ticker?.Stop("ScoreboardTicker");                            // Stop the scoreboard ticker
            return;                                                                 // Should return to the main ticker loop
        }

    }
}