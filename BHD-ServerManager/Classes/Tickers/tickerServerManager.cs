using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
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
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;
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

            // Functions that run all the time, even offline
            HandleRemoteServer();

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
                    SafeInvoke(thisServer, () => thisServer.functionEvent_serverStatus());
                }
            }

            // UI updates that should always run
            SafeInvoke(thisServer, () =>
            {
                // --- UI Update Hooks ---
                thisServer.ProfileTab.tickerProfileTabHook();                                   // Toggle Profile Lock based on server status
                thisServer.ServerTab.tickerServerHook();                                        // Toggle Server Lock based on server status
                thisServer.MapsTab.tickerMapsHook();                                            // Toggle Maps Lock based on server status
                thisServer.AdminTab.AdminsTickerHook();                                         // Update Admins Tab
            });

            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                // If the server is offline, we can skip the rest of the processing
                theInstance.instanceNextUpdateTime = currentTime.AddSeconds(5);
                theInstance.instanceLastUpdateTime = currentTime;
                return;
            }

            // --- Server is online: run status-specific logic in order ---
            // 1. Always update status and basic info
            ServerMemory.ReadMemoryServerStatus();                                  // Server Status
            SafeInvoke(thisServer, () => thisServer.functionEvent_serverStatus());  // Update Server Status

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
            }

            // 2. Loading Map
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                theInstance.instanceScoringProcRun = true;
                tickerEvent_preGameProcessing();                                    // Run pre-game processing
                ServerMemory.UpdatePlayerTeam();                                    // Move players to their teams if applicable
                ServerMemory.UpdateGlobalGameType();                                // Pinger Game Type Update
            }
            // 3. Start Delay
            else if (theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag
                theInstance.instanceCrashCounter = 0;                               // Reset crash counter
                ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
                ServerMemory.GetNextMapType();                                      // Grab the Current Map Type and the Next Map Type
            }
            // 4. Online (game in progress)
            else if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {

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

        // --- Functions that run all the time, even offline ---
        private static void HandleRemoteServer()
        {
            if (thisServer == null) return;

            // UI thread not required for these checks
            if (theInstance.profileEnableRemote && !RemoteServer.IsRunning &&
                RemoteServer.IsPortAvailable(theInstance.profileRemotePort) &&
                RemoteServer.IsPortAvailable(theInstance.profileRemotePort + 1))
            {
                RemoteServer.Start(theInstance.profileRemotePort, theInstance.profileRemotePort + 1);
            }

            if (!theInstance.profileEnableRemote && RemoteServer.IsRunning)
            {
                RemoteServer.Stop();
            }
        }

        // --- Pre-Game Processing (Loading Map) ---
        private static void tickerEvent_preGameProcessing()
        {
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP && theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing...");
                theInstance.instancePreGameProcRun = false;               
                ServerMemory.SetNextMapType();                                                  // Set the Next Map Type
                ServerMemory.UpdateGameScores();                                                // Update the Scores Required to Win the Game
                instanceChat.AutoMessageCounter = 0;
                instanceChat.ChatLog?.Clear();
                theInstance.playerList.Clear();
                StatFunctions.ResetPlayerStats();
            }
            
        }

        // --- Scoring Processing ---
        private static void tickerEvent_scoringGameProcessing()
        {
            if (!theInstance.instanceScoringProcRun)
            {
                return;
            }
            
            theInstance.instanceScoringProcRun = false;
            
            AppDebug.Log("tickerServerManagement", "Scoring Processing...");
            ServerMemory.ReadMemoryWinningTeam();
            AppDebug.Log("tickerServerManagement", "Sending Stats...");
            Task.Run(() => StatFunctions.SendImportData(thisServer!));
            AppDebug.Log("tickerServerManagement", "Updating Maps...");
            ServerMemory.SetNextMapType();                                                  // Set the Next Map Type
            ServerMemory.UpdateGameScores();                                                // Update the Scores Required to Win the Game
            AppDebug.Log("tickerServerManagement", "Kill ScoreBoard...");
            CommonCore.Ticker?.Start("ScoreboardTicker", 1000, () => tickerEvent_Scoreboard());
            AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");

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