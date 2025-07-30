using BHD_RemoteClient.Classes.GameManagement;
using BHD_RemoteClient.Classes.InstanceManagers;
using BHD_RemoteClient.Classes.StatManagement;
using BHD_RemoteClient.Forms;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        public static void runTicker()
        {
            // Gather data off the UI thread
            bool isAttached = GameManager.ReadMemoryIsProcessAttached();
            
            InstanceStatus status = theInstance.instanceStatus;
            int maxSlots = theInstance.gameMaxSlots;
            List<playerObject> players = theInstance.playerList.Values.ToList();

            DateTime currentTime = DateTime.Now;
            if (!(DateTime.Compare(theInstance.instanceNextUpdateTime, currentTime) < 0))
                return;

            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
                if (isAttached)
                {
                    // Player Management Tasks
                    if (status == InstanceStatus.OFFLINE || status == InstanceStatus.LOADINGMAP)
                    {
                        if (thisServer.player_LayoutPanel.Controls.Count > 0)
                            thisServer.player_LayoutPanel.Controls.Clear();
                        return;
                    }

                    int currentCount = thisServer.player_LayoutPanel.Controls.Count;

                    // Suspend layout for performance
                    thisServer.player_LayoutPanel.SuspendLayout();

                    // Add PlayerCards if needed
                    if (currentCount < maxSlots)
                    {
                        for (int i = currentCount; i < maxSlots; i++)
                        {
                            try
                            {
                                PlayerCard newPlayerCard = new PlayerCard
                                {
                                    Name = $"PlayerSlot_{i}",
                                    Margin = new Padding(0, 1, 0, 1)
                                };
                                thisServer.player_LayoutPanel.Controls.Add(newPlayerCard);
                            }
                            catch (Exception ex)
                            {
                                AppDebug.Log("tickerPlayerManagement", $"Error adding PlayerCard: {ex.Message}");
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
                                    thisServer.player_LayoutPanel.Controls.Remove(controlToRemove);
                            }
                            catch (Exception ex)
                            {
                                AppDebug.Log("tickerPlayerManagement", $"Error removing PlayerCard: {ex.Message}");
                            }
                        }
                    }

                    // Update PlayerCards with current player info
                    for (int i = 1; i <= maxSlots; i++)
                    {
                        try
                        {
                            var controls = thisServer.player_LayoutPanel.Controls.Find($"PlayerSlot_{i - 1}", true);
                            var playerCard = controls.FirstOrDefault() as PlayerCard;
                            var player = players.FirstOrDefault(p => p.PlayerSlot == i);
                            if (player != null)
                                playerCard?.UpdateStatus(player);
                            else
                                playerCard?.ResetStatus(i);
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("tickerPlayerManagement", $"Error updating PlayerCard for PlayerSlot {i}: {ex.Message}");
                        }
                    }

                    thisServer.player_LayoutPanel.ResumeLayout();
                }
                else
                {
                    if (thisServer.player_LayoutPanel.Controls.Count > 0)
                        thisServer.player_LayoutPanel.Controls.Clear();
                }

                // Server Status and Buttons
                thisServer.functionEvent_serverStatus();
                thisServer.functionEvent_swapFieldsStartStop();

                // Server Settings Refresh Tasks
                theInstanceManager.HighlightDifferences();

                // Chat Messages Refresh Tasks
                thisServer.functionEvent_UpdateSlapMessages();
                thisServer.functionEvent_UpdateAutoMessages();
                chatInstanceManagers.UpdateChatMessagesGrid();

                // Ban Management Refresh Tasks
                banInstanceManager.UpdateBannedTables();

                // Admin Refresh Tasks
                thisServer.functionEvent_UpdateAdminList();
                adminInstanceManager.UpdateAdminLogDialog();

                // Stats Refresh Tasks
                StatFunctions.PopulatePlayerStatsGrid();
                StatFunctions.PopulateWeaponStatsGrid();

                // Map Differences Refresh Tasks
                tickerEvent_checkMapDiff();
                tickerEvent_updateLabels();
            });

            theInstance.instanceNextUpdateTime = currentTime.AddSeconds(1);
            theInstance.instanceLastUpdateTime = currentTime;
        }

        private static void tickerEvent_checkMapDiff()
        {
            SafeInvoke(thisServer, () =>
            {
                if (instanceMaps.currentMapPlaylist.Count != thisServer.dataGridView_currentMaps.Rows.Count)
                {
                    thisServer.ib_resetCurrentMaps.BackColor = Color.Red;
                    return;
                }
                for (int i = 0; i < instanceMaps.currentMapPlaylist.Count; i++)
                {
                    if (instanceMaps.currentMapPlaylist[i].MapName != thisServer.dataGridView_currentMaps.Rows[i].Cells["MapName"].Value.ToString())
                    {
                        AppDebug.Log("tickerServerManagement", "Map Playlist Name Mismatch Detected at index " + i);
                        thisServer.ib_resetCurrentMaps.BackColor = Color.Red;
                        return;
                    }
                }
                thisServer.ib_resetCurrentMaps.BackColor = Color.Transparent;
            });
        }

        private static void tickerEvent_updateLabels()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return;

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
    }
}