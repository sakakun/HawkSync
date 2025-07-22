using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using BHD_RemoteClient.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BHD_RemoteClient.Classes.GameManagement;
using BHD_SharedResources.Classes.GameManagement;
using BHD_RemoteClient.Classes.InstanceManagers;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_RemoteClient.Classes.StatManagement;

namespace BHD_RemoteClient.Classes.Tickers
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
        private static ServerManager thisServer => Program.ServerManagerUI!;

        // This classs purpose is to manage the ticker for the server manager application.
        // It will handle the timing and checks for various server-related tasks.
        private static readonly object tickerLock = new object();

        public static void runTicker()
        {

            // Ensure UI thread safety
            if (thisServer.InvokeRequired)
            {
                thisServer.Invoke(() => runTicker());
                return;
            }

            DateTime currentTime = DateTime.Now;
            if (!(DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) < 0))
            {
                return;
            }

            if (GameManager.ReadMemoryIsProcessAttached())
            {
                AppDebug.Log("TickerServerManager", "Game process is attached, proceeding with ticker tasks.");

                // Player Management Tasks
                if (theInstance.instanceStatus == InstanceStatus.OFFLINE || theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                {
                    if (thisServer.player_LayoutPanel.Controls.Count > 0)
                    {
                        thisServer.player_LayoutPanel.Controls.Clear();
                    }
                    return;
                }

                int currentCount = thisServer.player_LayoutPanel.Controls.Count;
                int maxSlots = theInstance.gameMaxSlots;

                // Add PlayerCards if needed
                if (currentCount < maxSlots)
                {
                    for (int i = currentCount; i < maxSlots; i++)
                    {
                        try
                        {
                            PlayerCard newPlayerCard = new PlayerCard();
                            newPlayerCard.Name = $"PlayerSlot_{i}";
                            newPlayerCard.Margin = new Padding(0, 1, 0, 1);
                            thisServer.player_LayoutPanel.Controls.Add(newPlayerCard);
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerPlayerManagement", $"Error adding PlayerCard: {ex.Message}");
                            // Log or handle exception as needed
                        }
                    }
                }
                // Remove excess PlayerCards if needed
                else if (currentCount > maxSlots)
                {
                    for (int i = currentCount - 1; i >= maxSlots; i--)
                    {
                        try
                        {
                            var controlsToRemove = thisServer.player_LayoutPanel.Controls.Find($"PlayerSlot_{i}", true);
                            var controlToRemove = controlsToRemove.FirstOrDefault();
                            if (controlToRemove != null)
                            {
                                thisServer.player_LayoutPanel.Controls.Remove(controlToRemove);
                            }
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerPlayerManagement", $"Error removing PlayerCard: {ex.Message}");
                            // Log or handle exception as needed
                        }
                    }
                }

                // Update PlayerCards with current player info
                for (int i = 1; i <= maxSlots; i++)
                {
                    try
                    {
                        // Find the PlayerCard control for the current PlayerSlot
                        var controls = thisServer.player_LayoutPanel.Controls.Find($"PlayerSlot_{i - 1}", true);
                        var playerCard = controls.FirstOrDefault() as PlayerCard;
                        var player = theInstance.playerList.Values.FirstOrDefault(p => p.PlayerSlot == i);
                        if (player != null)
                        {
                            playerCard?.UpdateStatus(player);
                        }
                        else
                        {
                            // If no player is found for this PlayerSlot, clear the PlayerCard
                            playerCard?.ResetStatus(i);
                        }

                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerPlayerManagement", $"Error updating PlayerCard for PlayerSlot {i}: {ex.Message}");
                        // Log or handle exception as needed
                    }
                }

            }
            else
            {
                // If the server process is not attached, clear the player panel
                if (thisServer.player_LayoutPanel.Controls.Count > 0)
                {
                    thisServer.player_LayoutPanel.Controls.Clear();
                }
            }

            // Server Status and Buttons
            thisServer.functionEvent_serverStatus();                // Update server status
            thisServer.functionEvent_swapFieldsStartStop();         // Swap fields for start/stop 

            // Server Settings Refresh Tasks            
            theInstanceManager.HighlightDifferences();              // Update server settings

            // Chat Messages Refresh Tasks
            thisServer.functionEvent_UpdateSlapMessages();          // Update slap messages
            thisServer.functionEvent_UpdateAutoMessages();          // Update auto messages
            remoteChatInstanceManager.UpdateChatMessagesGrid();     // Update chat messages grid

            // Ban Management Refresh Tasks
            banInstanceManager.UpdateBannedTables();                // Update banned tables

            // Admin Refresh Tasks
            thisServer.functionEvent_UpdateAdminList();             // Update admin list
            adminInstanceManager.UpdateAdminLogDialog();            // Update admin log dialog

            // Stats Refresh Tasks
            StatFunctions.PopulatePlayerStatsGrid();              // Populate player stats grid
            StatFunctions.PopulateWeaponStatsGrid();              // Populate weapon stats grid 

            // Map Differeces Refresh Tasks
            tickerEvent_checkMapDiff();

            tickerEvent_updateLabels();

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
        private static void tickerEvent_updateLabels()
        {

            if (theInstance.instanceStatus == InstanceStatus.OFFLINE) {
                return;
            }
            
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
    }
}
