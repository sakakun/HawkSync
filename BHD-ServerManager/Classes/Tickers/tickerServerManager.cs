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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        // This classs purpose is to manage the ticker for the server manager application.
        // It will handle the timing and checks for various server-related tasks.
        private static readonly object tickerLock = new object();

        public static void runTicker()
        {

            // Ensure UI thread safety
            if (thisServer!.InvokeRequired)
            {
                thisServer.Invoke(() => runTicker());
                return;
            }

            if (theInstance.profileEnableRemote && !RemoteServer.IsRunning && RemoteServer.IsPortAvailable(theInstance.profileRemotePort) && RemoteServer.IsPortAvailable(theInstance.profileRemotePort + 1))
            {
                RemoteServer.Start(
                    theInstance.profileRemotePort,
                    theInstance.profileRemotePort + 1                );
            }
            if (!theInstance.profileEnableRemote && RemoteServer.IsRunning)
            {
                RemoteServer.Stop();
            }

            DateTime currentTime = DateTime.Now;
            if (!(DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) < 0))
            {
                return;
            }

            if (ServerMemory.ReadMemoryIsProcessAttached())
            {
                // Server Online and Running
                ServerMemory.ReadMemoryServerStatus();                         // Get Server Status (Online/Offline/Etc)
                thisServer.functionEvent_serverStatus();                // Trigger Status Message (Status Bar)

                ServerMemory.UpdateNovaID();                            // Update Nova ID for the server instance
                ServerMemory.ReadMemoryGameTimeLeft();                       // Get the time left for the current map
                ServerMemory.ReadMemoryCurrentMissionName();                       // Get the current mission being played
                ServerMemory.ReadMemoryCurrentGameType();                      // Get the current game type being played
                ServerMemory.ReadMemoryCurrentNumPlayers();                       // Get the current number of players in the server
                ServerMemory.UpdateGlobalGameType();                    // Pinger Fix: GameType for the server instance, this does not use the above value. Memory reads and edits only.
                ServerMemory.UpdateMapCycleCounter();                   // Update the map cycle counter for the server instance
                                                                        // TO DO: get_currentMap (map object)                   // Get the current map object being played

                // NOT(InstanceStatus.LOADINGMAP) 
                ServerMemory.ReadMemoryGeneratePlayerList();                       // Get the current players in the server and thier stats

                // STATS ENABLED, InstanceStatus.ONLINE
                if (theInstance.instanceStatus == InstanceStatus.ONLINE)
                {
                    StatFunctions.RunPlayerStatsUpdate();                   // Collect PlayerStats - This will update the player stats for the current players in the server

                    if (theInstance.WebStatsEnabled)
                    {
                        if (DateTime.Now > instanceStats.lastPlayerStatsUpdate.AddSeconds(theInstance.WebStatsUpdateInterval))
                        {
                            Task.Run(() => StatFunctions.SendUpdateData(thisServer));
                            instanceStats.lastPlayerStatsUpdate = DateTime.Now; // Update the last player stats update time
                        }
                        if (DateTime.Now > instanceStats.lastPlayerStatsReport.AddSeconds(theInstance.WebStatsReportInterval))
                        {
                            Task.Run(async () =>
                            {
                                string ReportResults = await StatFunctions.SendReportData(thisServer);
                                // handle ReportResults if needed
                            });
                            instanceStats.lastPlayerStatsReport = DateTime.Now; // Update the last player stats update time
                        }
                    }
                }


                // InstanceStatus.LOADINGMAP
                //        Must only be run once per loading
                tickerEvent_preGameProcessing();                        // Pre Game Processing

                // InstanceStatus.SCORING
                //        Must only be run once per scoring
                tickerEvent_scoringGameProcessing();                    // Score Game Process

                // InstanceStatus.LOADINGMAP || InstanceStatus.SCORING
                ServerMemory.UpdatePlayerTeam();                        // UpdatePlayerTeam - If requested, change PlayerTeam of players in the playerChangeTeam list

                // InstanceStatus.STARTDELAY || InstanceStatus.ONLINE
                theInstance.instanceCrashCounter = 0;
                ServerMemory.UpdateGameScores();                        // Update the scores for the next map.
                ServerMemory.ReadMemoryCurrentMapIndex();

                // Trigger Server Manager Window Updates
                tickerEvent_updateLabels(thisServer);

            }
            else
            {
                // Server Offline
                theInstance.instanceStatus = InstanceStatus.OFFLINE;   // Set Server Status
                thisServer.functionEvent_serverStatus();               // Trigger Status Message
            }

            theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
            theInstance.instanceLastUpdateTime = currentTime;

            // This changes the state of various fields in the server manager based on the server status.
            thisServer.functionEvent_swapFieldsStartStop();

            // Admin Dialogs
            thisServer.functionEvent_UpdateAdminList();
            adminInstanceManager.UpdateAdminLogDialog();
            // Settings Spot the Differences
            theInstanceManager.HighlightDifferences();
            // Map Playlist Different
            tickerEvent_checkMapDiff();

        }

        private static void tickerEvent_checkMapDiff()
        {
            // Compare the currentMapPlaylist with the dg_currentMapPlaylist in the ServerManager
            if (instanceMaps.currentMapPlaylist.Count != thisServer!.dataGridView_currentMaps.Rows.Count)
            {
                thisServer!.ib_resetCurrentMaps.BackColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                for (int i = 0; i < instanceMaps.currentMapPlaylist.Count; i++)
                {
                    if (instanceMaps.currentMapPlaylist[i].MapName != thisServer.dataGridView_currentMaps.Rows[i].Cells["MapName"].Value.ToString())
                    {
                        AppDebug.Log("tickerServerManagement", "Map Playlist Name Mismatch Detected at index " + i);
                        thisServer!.ib_resetCurrentMaps.BackColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }
            thisServer!.ib_resetCurrentMaps.BackColor = Color.Transparent; // Reset the color if no differences are found
        }

        private static void tickerEvent_preGameProcessing()
        {
            // This method is called when the server enters the "LOADINGMAP" state.
            // This method should be called only once per map load.
            //CoreManager.DebugLog("Pre-game Processing Starting...");
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP && theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing...");
                theInstance.instancePreGameProcRun = false;
                
                // Tasks
                instanceChat.ChatLog!.Clear();
                theInstance.playerList.Clear();
            }
            // Do NOT reset instancePreGameProcRun to true until the state has actually changed
            else if (theInstance.instanceStatus != InstanceStatus.LOADINGMAP && !theInstance.instancePreGameProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Pre-game Processing Reset...");
                theInstance.instancePreGameProcRun = true;
            }

            // If Server is in the Start Delay state

            //CoreManager.DebugLog("Pre-game Processing Complete...");
        }
        private static void tickerEvent_scoringGameProcessing()
        {
            ServerMemory.ReadMemoryWinningTeam();

            if (theInstance.instanceStatus == InstanceStatus.SCORING && theInstance.instanceScoringProcRun)
            {
                AppDebug.Log("tickerServerManagement", "Scoring Processing...");
                theInstance.instanceScoringProcRun = false;

                // Tasks
                instanceChat.AutoMessageCounter = 0; // Reset Auto Message Counter

                Task.Run(() => StatFunctions.SendImportData(thisServer!));
                // Generate Stat Data - Make POST Request to the WebStats Server}                
                StatFunctions.ResetPlayerStats();                                        // Stats Reset - Reset Player Stats for the next game
                ServerMemory.UpdateNextMapGameType();                                   // Map Game Type - Update the next map game type based on the current game type                                                                                        // Scoreboard Ticker - Do this last to make sure that we don't move on to next state before stats are processed.
                CommonCore.Ticker!.Start("ScoreboardTicker", 500, () => tickerEvent_Scoreboard());     // Start the Scoreboard Ticker
                AppDebug.Log("tickerServerManagement", "Scoring Processing Complete.");
            }
            // Do NOT reset instanceScoringProcRun to true until the state has actually changed
            else if (theInstance.instanceStatus != InstanceStatus.SCORING && !theInstance.instanceScoringProcRun)
            {
                theInstance.instanceScoringProcRun = true;
            }
            return;            
        }
        private static void tickerEvent_updateLabels(ServerManager thisServer)
        {
            int nextMapIndex = theInstance.gameInfoCurrentMapIndex >= instanceMaps.currentMapPlaylist.Count - 1
                                || theInstance.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance.gameInfoCurrentMapIndex + 1;

            void updateLabels()
            {
                // Update the labels in the ServerManager window with the current game information
                // Current Map Playing
                thisServer.label_dataCurrentMap.Text = theInstance.gameInfoMapName;
                // Next Map Playing
                thisServer.label_dataNextMap.Text = instanceMaps.currentMapPlaylist[nextMapIndex].MapName;
                // Time Left
                thisServer.label_dataTimeLeft.Text = theInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss");
            }

            if (thisServer.InvokeRequired)
            {
                thisServer.Invoke(new Action(updateLabels));
            }
            else
            {
                updateLabels();
            }
        }
        private static void tickerEvent_Scoreboard()
        {
            Thread.Sleep(theInstance.gameScoreBoardDelay * 1000);
            ServerMemory.UpdateScoreBoardTimer();
            AppDebug.Log("tickerServerManagement", "Scoreboard Timer Complete");
            CommonCore.Ticker!.Stop("ScoreboardTicker");
        }

    }
}
