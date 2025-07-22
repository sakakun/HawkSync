using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.StatsManagement;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerPlayerManagement
    {
        // Global Variables
        private readonly static theInstance thisInstance = CommonCore.theInstance!;

        // This class's purpose is to manage the ticker for the server manager application.
        // It will handle the timing and checks for various server-related tasks.
        private static readonly object tickerLock = new object();

        public static void runTicker(ServerManager thisServer)
        {
            // Ensure UI thread safety
            if (thisServer.InvokeRequired)
            {
                try
                {
                    thisServer.Invoke(new Action(() => runTicker(thisServer)));
                } catch (Exception ex)
                {
                    AppDebug.Log("tickerPlayerManagement", $"Error invoking tickerPlayerManagement: {ex.Message}");
                }

                return;
            }

            lock (tickerLock)
            {

                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    if (thisInstance.instanceStatus == InstanceStatus.OFFLINE || thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                    {
                        if (thisServer.player_LayoutPanel.Controls.Count > 0)
                        {
                            thisServer.player_LayoutPanel.Controls.Clear();
                        }
                        return;
                    }

                    int currentCount = thisServer.player_LayoutPanel.Controls.Count;
                    int maxSlots = thisInstance.gameMaxSlots;

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
                            var player = thisInstance.playerList.Values.FirstOrDefault(p => p.PlayerSlot == i);
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
                    AppDebug.Log("tickerPlayerManagement", "Server process is not attached. Ticker Skipping.");
                }

                StatFunctions.PopulatePlayerStatsGrid();   // Populate PlayerStats Grid - This will update the player stats grid in the server manager window
                StatFunctions.PopulateWeaponStatsGrid();   // Update Player Stats - This will update the player stats for the current players in the server
            }
        }
    }
}