using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
                thisServer.functionEvent_UpdateAdminLists();
            });
            adminInstanceManager.UpdateAdminLogDialog();
            SafeInvoke(thisServer, () => theInstanceManager.HighlightDifferences());
            tickerEvent_checkMapDiff();

            // Only run the rest if it's time for an update
            DateTime currentTime = DateTime.Now;
            if (DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) >= 0)
                return;

            // If server process is not attached, set status to offline and update UI
            if (!ServerMemory.ReadMemoryIsProcessAttached())
            {
                theInstance.instanceStatus = InstanceStatus.OFFLINE;
                SafeInvoke(thisServer, () => thisServer.functionEvent_serverStatus());
                theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
                theInstance.instanceLastUpdateTime = currentTime;
                return;
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
            ServerMemory.UpdateMapCycleCounter();

            // 2. Loading Map
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                tickerEvent_preGameProcessing();
                ServerMemory.UpdatePlayerTeam();
                // UI: update labels
                tickerEvent_updateLabels(thisServer);
            }
            // 3. Start Delay
            else if (theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                theInstance.instanceCrashCounter = 0;
                ServerMemory.UpdateGameScores();
                ServerMemory.ReadMemoryCurrentMapIndex();
                ServerMemory.UpdatePlayerTeam();
                ServerMemory.ReadMemoryGeneratePlayerList();
                tickerEvent_updateLabels(thisServer);
            }
            // 4. Online (game in progress)
            else if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                theInstance.instanceCrashCounter = 0;
                ServerMemory.UpdateGameScores();
                ServerMemory.ReadMemoryCurrentMapIndex();
                ServerMemory.UpdatePlayerTeam();
                ServerMemory.ReadMemoryGeneratePlayerList();
                tickerEvent_updateLabels(thisServer);

                // Stats update
                StatFunctions.RunPlayerStatsUpdate();
                if (theInstance.WebStatsEnabled)
                {
                    if (DateTime.Now > instanceStats.lastPlayerStatsUpdate.AddSeconds(theInstance.WebStatsUpdateInterval))
                    {
                        Task.Run(() => StatFunctions.SendUpdateData(thisServer));
                        instanceStats.lastPlayerStatsUpdate = DateTime.Now;
                    }
                    if (DateTime.Now > instanceStats.lastPlayerStatsReport.AddSeconds(theInstance.WebStatsReportInterval))
                    {
                        Task.Run(async () =>
                        {
                            string ReportResults = await StatFunctions.SendReportData(thisServer);
                            // handle ReportResults if needed
                        });
                        instanceStats.lastPlayerStatsReport = DateTime.Now;
                    }
                }
            }
            // 5. Scoring
            else if (theInstance.instanceStatus == InstanceStatus.SCORING)
            {
                tickerEvent_scoringGameProcessing();
                ServerMemory.UpdatePlayerTeam();
                tickerEvent_updateLabels(thisServer);
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
            }
            else if (theInstance.instanceStatus != InstanceStatus.LOADINGMAP && !theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing Reset...");
                theInstance.instancePreGameProcRun = true;
            }
        }

        // --- Scoring Processing ---
        private static void tickerEvent_scoringGameProcessing()
        {
            ServerMemory.ReadMemoryWinningTeam();

            if (theInstance.instanceStatus == InstanceStatus.SCORING && theInstance.instanceScoringProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Scoring Processing...");
                theInstance.instanceScoringProcRun = false;

                instanceChat.AutoMessageCounter = 0;

                Task.Run(() => StatFunctions.SendImportData(thisServer!));
                StatFunctions.ResetPlayerStats();
                ServerMemory.UpdateNextMapGameType();
                CommonCore.Ticker?.Start("ScoreboardTicker", 500, () => tickerEvent_Scoreboard());
                AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");
            }
            else if (theInstance.instanceStatus != InstanceStatus.SCORING && !theInstance.instanceScoringProcRun)
            {
                theInstance.instanceScoringProcRun = true;
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
            Thread.Sleep(theInstance.gameScoreBoardDelay * 1000);
            ServerMemory.UpdateScoreBoardTimer();
            AppDebug.Log("tickerServerManagement", "Scoreboard Timer Complete");
            CommonCore.Ticker?.Stop("ScoreboardTicker");
        }
    }
}