using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_ServerManager.Classes.StatsManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerPlayerManagement
    {
        // Global Variables
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static theInstance thisInstance => CommonCore.theInstance!;

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
            // Gather player data off the UI thread
            bool isAttached = ServerMemory.ReadMemoryIsProcessAttached();
            InstanceStatus status = thisInstance.instanceStatus;
            int maxSlots = thisInstance.gameMaxSlots;
            List<playerObject> players = thisInstance.playerList.Values.ToList();

            // Now update the UI
            SafeInvoke(thisServer, () =>
            {
                // If server is not attached or not in a valid state, clear the panel and return
                if (!isAttached || status == InstanceStatus.OFFLINE || status == InstanceStatus.LOADINGMAP)
                {
                    if (thisServer.player_LayoutPanel.Controls.Count > 0)
                        thisServer.player_LayoutPanel.Controls.Clear();
                    return;
                }

                // Efficiently add/remove PlayerCards as needed
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
                        {
                            playerCard?.UpdateStatus(player);
                        }
                        else
                        {
                            playerCard?.ResetStatus(i);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("tickerPlayerManagement", $"Error updating PlayerCard for PlayerSlot {i}: {ex.Message}");
                    }
                }

                thisServer.player_LayoutPanel.ResumeLayout();

                // Update stats grids (these should be UI-thread safe)
                try
                {
                    StatFunctions.PopulatePlayerStatsGrid();
                    StatFunctions.PopulateWeaponStatsGrid();
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerPlayerManagement", $"Error updating stats grids: {ex.Message}");
                }
            });
        }
    }
}