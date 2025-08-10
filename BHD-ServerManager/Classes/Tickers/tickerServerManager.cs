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

            // UI updates that should always run
            SafeInvoke(thisServer, () =>
            {
                thisServer.functionEvent_swapFieldsStartStop();
                adminInstanceManager.UpdateAdminLogDialog();
            });

            // UI Updates Regardless of Server Status
            adminInstanceManager.UpdateAdminLogDialog();                                        // Admin Log Tab
            SafeInvoke(thisServer, () => theInstanceManager.HighlightDifferences());            // Instance Settings Differences Check
            tickerEvent_checkMapDiff();                                                         // Map Playlist Difference Check
            tickerEvent_updateLabels(thisServer);                                               // Update the Labels on the UI

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
                    theInstance.instanceNextUpdateTime = currentTime.AddSeconds(20);
                    theInstance.instanceLastUpdateTime = currentTime;
                    return;
                }
            }

            // --- Server is online: run status-specific logic in order ---
            // 1. Always update status and basic info
            ServerMemory.ReadMemoryServerStatus();
            SafeInvoke(thisServer, () => thisServer.functionEvent_serverStatus());
            ServerMemory.UpdateNovaID();
            ServerMemory.ReadMemoryGameTimeLeft();
            ServerMemory.ReadMemoryCurrentMissionName();
            ServerMemory.ReadMemoryCurrentGameType();
            ServerMemory.ReadMemoryCurrentNumPlayers();
            ServerMemory.UpdateGlobalGameType();
            ServerMemory.UpdateGameScores();                                        // Update game score limits
            ServerMemory.UpdateMapCycleCounter();

            // 2. Loading Map
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                tickerEvent_preGameProcessing();                                    // Run pre-game processing
                ServerMemory.UpdatePlayerTeam();                                    // Move players to their teams if applicable
            }
            // 3. Start Delay
            else if (theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                theInstance.instancePreGameProcRun = true;                          // Reset pre-game processing flag
                theInstance.instanceCrashCounter = 0;                               // Reset crash counter
                ServerMemory.ReadMemoryCurrentMapIndex();                           // Read current map index
                ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.
            }
            // 4. Online (game in progress)
            else if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                ServerMemory.ReadMemoryCurrentMapIndex();                           // Read current map index
                ServerMemory.ReadMemoryGeneratePlayerList();                        // Generate player list.

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

            if (theInstance.instanceStatus != InstanceStatus.SCORING && !theInstance.instanceScoringProcRun)
            {
                // If not scoring, reset scoring processing flag
                theInstance.instanceScoringProcRun = true;
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

        // --- Map Playlist Difference Check ---
        private static void tickerEvent_checkMapDiff()
        {
            if (thisServer == null) return;

            SafeInvoke(thisServer, () =>
            {
                if (instanceMaps.currentMapPlaylist.Count != thisServer.dataGridView_currentMaps.Rows.Count)
                {
                    thisServer.ib_resetCurrentMaps.BackColor = Color.Red;
                    return;
                }
                for (int i = 0; i < instanceMaps.currentMapPlaylist.Count; i++)
                {
                    if (instanceMaps.currentMapPlaylist[i].MapName != thisServer.dataGridView_currentMaps.Rows[i].Cells["MapName"].Value?.ToString())
                    {
                        AppDebug.Log("tickerServerManagement", "Map Playlist Name Mismatch Detected at index " + i);
                        thisServer.ib_resetCurrentMaps.BackColor = Color.Red;
                        return;
                    }
                }
                thisServer.ib_resetCurrentMaps.BackColor = Color.Transparent;
            });
        }

        // --- Pre-Game Processing (Loading Map) ---
        private static void tickerEvent_preGameProcessing()
        {
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP && theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing...");
                theInstance.instancePreGameProcRun = false;
                instanceChat.ChatLog?.Clear();
                theInstance.playerList.Clear();
                StatFunctions.ResetPlayerStats();
            }
            
        }

        // --- Scoring Processing ---
        private static void tickerEvent_scoringGameProcessing()
        {
            ServerMemory.ReadMemoryWinningTeam();

            if (theInstance.instanceScoringProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Scoring Processing...");
                theInstance.instanceScoringProcRun = false;
                instanceChat.AutoMessageCounter = 0;
                Task.Run(() => StatFunctions.SendImportData(thisServer!));
                ServerMemory.UpdateNextMapGameType();
                CommonCore.Ticker?.Start("ScoreboardTicker", 500, () => tickerEvent_Scoreboard());
                AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");
            }

        }

        // --- UI Label Updates ---
        private static void tickerEvent_updateLabels(ServerManager thisServer)
        {
            int nextMapIndex = theInstance.gameInfoCurrentMapIndex >= instanceMaps.currentMapPlaylist.Count - 1
                                || theInstance.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance.gameInfoCurrentMapIndex + 1;

            SafeInvoke(thisServer, () =>
            {
                thisServer.label_dataCurrentMap.Text = theInstance.gameInfoMapName;
                thisServer.label_dataNextMap.Text = instanceMaps.currentMapPlaylist.Count > 0
                    ? instanceMaps.currentMapPlaylist[nextMapIndex].MapName
                    : string.Empty;
                thisServer.label_dataTimeLeft.Text = theInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss");
            });
        }

        // --- Scoreboard Ticker ---
        private static void tickerEvent_Scoreboard()
        {
            Thread.Sleep(theInstance.gameScoreBoardDelay * 1000);                   // Wait for the specified delay
            ServerMemory.UpdateScoreBoardTimer();                                   // Set the scoreboard timer to 1 second.
            AppDebug.Log("tickerServerManagement", "Scoreboard Timer Complete");    // Log the completion of the scoreboard timer
            CommonCore.Ticker?.Stop("ScoreboardTicker");                            // Stop the scoreboard ticker
        }

    }
}