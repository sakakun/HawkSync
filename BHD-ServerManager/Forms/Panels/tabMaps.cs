using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Xaml.Schema;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabMaps : UserControl
    {

        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private mapInstance instanceMaps => CommonCore.instanceMaps;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "MapsTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.
        public tabMaps()
        {
            InitializeComponent();
        }
        // --- Functions ---
        // --- Populate Available Maps ---
        public void functionEvent_PopulateAvailableMapData()
        {
            // Clear the DataGridView
            dataGridView_availableMaps.Rows.Clear();

            // Populate the DataGridView with available maps
            foreach (var map in instanceMaps.availableMaps)
            {
                // Button Cell for removing maps from the playlist and deleting the file.
                DataGridViewButtonCell buttonCell = new DataGridViewButtonCell
                {
                    Value = map.MapFile,
                    FlatStyle = FlatStyle.Flat,                   
                    Style = new DataGridViewCellStyle
                    {
                        BackColor = SystemColors.Window,
                        ForeColor = Color.Red,
                        SelectionBackColor = Color.Red,
                        SelectionForeColor = SystemColors.Window
                    },
                    UseColumnTextForButtonValue = true
                };

                int rowIndex = dataGridView_availableMaps.Rows.Add();
                DataGridViewRow row = dataGridView_availableMaps.Rows[rowIndex];

                row.Cells["avail_MapID"].Value = map.MapID;
                row.Cells["avail_MapFileName"].Value = map.MapFile;
                row.Cells["avail_MapName"].Value = map.MapName;
                row.Cells["avail_MapType"].Value = map.MapType;
                row.Cells["avail_MapDelete"].Value = buttonCell;
            }

        }
        // --- Populate Current Map Playlist ---
        public void functionEvent_PopulateCurrentMapPlaylist(List<mapFileInfo> ImportedMapList = null!)
        {
            List<mapFileInfo> MapList = ImportedMapList != null ? ImportedMapList : instanceMaps.currentMapPlaylist;

            // Clear the DataGridView
            dataGridView_currentMaps.Rows.Clear();
            // MapID for the Purpose of the Current Playlist is the "Order" in which they are played.
            int rowIndex = 1;
            // Populate the DataGridView with current map playlist
            foreach (var map in MapList)
            {
                int rowObject = dataGridView_currentMaps.Rows.Add();
                DataGridViewRow row = dataGridView_currentMaps.Rows[rowObject];

                row.Cells["current_MapID"].Value = rowIndex;
                row.Cells["current_MapFileName"].Value = map.MapFile;
                row.Cells["current_MapName"].Value = map.MapName;
                row.Cells["current_MapType"].Value = map.MapType;

                rowIndex++;
            }
        }
        // --- Add Map to Current Playlist from Available Maps ---
        public void functionEvent_AddMapToCurrentPlaylist(object sender)
        {
            // Get the current DataGridView of available maps
            DataGridView dataGridView = (DataGridView)sender;
            // Get row index of the clicked cell
            int rowIndex = dataGridView.CurrentCell.RowIndex;
            DataGridViewRow rowOG = dataGridView.Rows[rowIndex];

            int rowObject = dataGridView_currentMaps.Rows.Add();
            DataGridViewRow rowNew = dataGridView_currentMaps.Rows[rowObject];
            rowNew.Cells["current_MapID"].Value = rowObject + 1;
            rowNew.Cells["current_MapFileName"].Value = rowOG.Cells["avail_MapFileName"].Value;
            rowNew.Cells["current_MapName"].Value = rowOG.Cells["avail_MapName"].Value;
            rowNew.Cells["current_MapType"].Value = rowOG.Cells["avail_MapType"].Value;
        }
        // --- Reset Map Playlist ---
        public void functionEvent_ResetAvailableMaps()
        {
            mapInstanceManager.ResetAvailableMaps();                        // Reload Map Data from Game Files
            functionEvent_PopulateAvailableMapData();                       // Repopulate the available maps
        }
        public void functionEvent_LoadCurrentMapPlaylist(bool external = false)
        {
            // Load the current map playlist from the instance manager
            instanceMaps.currentMapPlaylist = mapInstanceManager.LoadCustomMapPlaylist(external);
            // Populate the DataGridView with the current map playlist
            functionEvent_PopulateCurrentMapPlaylist();
        }
        // --- Update Form Information and Visibility ---
        public void functionEvent_UpdateForm()
        {
            // Enable/Disable Buttons based on Server Status
            bool isOnline = (theInstance.instanceStatus != InstanceStatus.OFFLINE);
            btn_mapsUpdate.Enabled = isOnline;
            btn_mapsScore.Enabled = isOnline;
            btn_mapsSkip.Enabled = isOnline;
            btn_mapsPlayNext.Enabled = isOnline;

            // Update Labels with Current Information
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                label_currentMapName.Text = "N/A";
                label_currentMapType.Text = "N/A";
                label_nextMapName.Text = "N/A";
                label_nextMapType.Text = "N/A";
                label_timeLeft.Text = "HH:MM:SS";
                return;
            }

            int nextMapIndex = theInstance.gameInfoCurrentMapIndex >= instanceMaps.currentMapPlaylist.Count - 1
                                || theInstance.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance.gameInfoCurrentMapIndex + 1;

            label_currentMapName.Text = theInstance.gameInfoMapName;
            label_currentMapType.Text = objectGameTypes.All.FirstOrDefault(gt => gt.ShortName == theInstance.gameInfoGameType)?.Name;
            label_nextMapName.Text = instanceMaps.currentMapPlaylist[nextMapIndex].MapName;
            label_nextMapType.Text = objectGameTypes.All.FirstOrDefault(gt => gt.ShortName == instanceMaps.currentMapPlaylist[nextMapIndex].MapType)?.Name;
            label_timeLeft.Text = theInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss");

        }
        public void functionEvent_CheckForMapChanges()
        {
            if (instanceMaps.currentMapPlaylist.Count != dataGridView_currentMaps.Rows.Count)
            {
                ib_resetCurrentMaps.BackColor = Color.Red;
                return;
            }
            for (int i = 0; i < instanceMaps.currentMapPlaylist.Count; i++)
            {
                if (instanceMaps.currentMapPlaylist[i].MapName != dataGridView_currentMaps.Rows[i].Cells["current_MapName"].Value?.ToString())
                {
                    AppDebug.Log("tickerServerManagement", "Map Playlist Name Mismatch Detected at index " + i);
                    ib_resetCurrentMaps.BackColor = Color.Red;
                    return;
                }
            }
            ib_resetCurrentMaps.BackColor = Color.Transparent;
        }
        // --- Ticker Hook for Maps Tab ---
        public void tickerMapsHook()
        {
            // Check if the first load is complete
            if (!_firstLoadComplete)
            {
                // Set the first load complete flag to true
                _firstLoadComplete = true;

                // Get the server settings on first load
                functionEvent_ResetAvailableMaps();                         // Populate the available maps (First Time Only)
                functionEvent_LoadCurrentMapPlaylist();                     // Load the current map playlist (First Time Only)
                functionEvent_PopulateCurrentMapPlaylist();                 // Populate the current map playlist (First Time Only)
            }

            // Update the form information
            functionEvent_CheckForMapChanges();                             // Check for Map Changes
            functionEvent_UpdateForm();                                     // Update Form to Current Map States
        }
        // --- Action Handlers ---
        private void actionClick_RefreshAvailableMaps(object sender, EventArgs e)
        {
            try
            {
                functionEvent_ResetAvailableMaps();
                MessageBox.Show("Available maps have been refreshed.", "Refresh Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, "actionClick_RefreshAvailableMaps: " + ex.Message.ToString() + "\nStackTrace: " + ex.StackTrace);
            }
        }

        private void actionClick_SaveCurrentPlaylist(object sender, EventArgs e)
        {
            instanceMaps.currentMapPlaylist = mapInstanceManager.BuildCurrentMapPlaylist();
            mapInstanceManager.SaveCurrentMapPlaylist(instanceMaps.currentMapPlaylist, false);
            MessageBox.Show("Current map playlist has been saved.", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actionClick_ClearCurrentPlaylist(object sender, EventArgs e)
        {
            dataGridView_currentMaps.Rows.Clear();
        }

        private void actionClick_ImportCurrentPlaylist(object sender, EventArgs e)
        {
            functionEvent_LoadCurrentMapPlaylist(true);
        }

        private void actionClick_ExportCurrentPlaylist(object sender, EventArgs e)
        {
            List<mapFileInfo> mapList = mapInstanceManager.BuildCurrentMapPlaylist();
            mapInstanceManager.SaveCurrentMapPlaylist(mapList, true);
        }

        private void actionClick_ResetCurrentMapPlaylist(object sender, EventArgs e)
        {
            functionEvent_PopulateCurrentMapPlaylist();
        }

        private void actionClick_CurrentPlaylistAddMap(object sender, DataGridViewCellEventArgs e)
        {
            functionEvent_AddMapToCurrentPlaylist(sender);
        }

        private void actionClick_DeleteSelectedMap(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current DataGridView of available maps
            DataGridView dataGridView = (DataGridView)sender;
            // Get row index of the clicked cell
            int rowIndex = dataGridView.CurrentCell.RowIndex;
            // Remove Row from DataGridView
            dataGridView.Rows.RemoveAt(rowIndex);
        }

        private void actionClick_UpdateMapPlaylist(object sender, EventArgs e)
        {
            if (theInstance.instanceStatus == InstanceStatus.STARTDELAY || theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                GameManager.UpdateMapCycle1();
                GameManager.UpdateMapCycle2();
                MessageBox.Show("The server map list has been updated successfully.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The Maplist saved but server is currently not ready to recieve map updates. Please wait for Status Change of Server.", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void actionClick_PlayMapNext(object sender, EventArgs e)
        {
            int mapIndex = dataGridView_currentMaps.CurrentCell.RowIndex;
            GameManager.UpdateNextMap(mapIndex);
            MessageBox.Show(dataGridView_currentMaps.Rows[mapIndex].Cells["current_MapName"].Value + " has been updated to play next.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actionClick_ScoreMap(object sender, EventArgs e)
        {
            GameManager.WriteMemoryScoreMap();
        }

        private void actionClick_SkipMap(object sender, EventArgs e)
        {
            if (theInstance.instanceStatus != InstanceStatus.LOADINGMAP)
            {
                theInstance.instanceMapSkipped = true;
                chatInstanceManagers.SendMessageToQueue(true, 0, "resetgames");
                // GameManager.WriteMemorySendConsoleCommand("resetgames");
            }
            else
            {
                MessageBox.Show("Sorry you can't skip currently, map is currently 'loading' please try again in a moment.", "Cannot Skip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void actionKey_MoveMap(object sender, KeyPressEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            int rowIndex = dataGridView.CurrentCell.RowIndex;

            bool isShift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            if (isShift && (e.KeyChar == 'W' || e.KeyChar == 'w') && rowIndex > 0)
            {
                // Move up
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                dataGridView.Rows.RemoveAt(rowIndex);
                dataGridView.Rows.Insert(rowIndex - 1, row);
                // Update MapID
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells["current_MapID"].Value = i + 1;
                }
                // Select the moved row
                dataGridView.CurrentCell = dataGridView.Rows[rowIndex - 1].Cells[dataGridView.CurrentCell.ColumnIndex];
            }
            else if (isShift && (e.KeyChar == 'S' || e.KeyChar == 's') && rowIndex < dataGridView.Rows.Count - 1)
            {
                // Move down
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                dataGridView.Rows.RemoveAt(rowIndex);
                dataGridView.Rows.Insert(rowIndex + 1, row);
                // Update MapID
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells["current_MapID"].Value = i + 1;
                }
                // Select the moved row
                dataGridView.CurrentCell = dataGridView.Rows[rowIndex + 1].Cells[dataGridView.CurrentCell.ColumnIndex];
            }
        }

        private void actionClick_filterChanged(object sender, EventArgs e)
        {
            int selectedIndex = combo_gameTypes.SelectedIndex;

            // Show all if index is 9 or invalid
            if (selectedIndex == 9 || selectedIndex < 0 || selectedIndex >= objectGameTypes.All.Count)
            {
                foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                {
                    row.Visible = true;
                }
                return;
            }

            // Get the ShortName for the selected game type
            string shortName = objectGameTypes.All[selectedIndex].ShortName;

            // Filter rows based on ShortName
            foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
            {
                string? cellValue = row.Cells["avail_MapType"].Value?.ToString();
                row.Visible = string.Equals(cellValue, shortName, StringComparison.OrdinalIgnoreCase);
            }
        }
        private void actionTextChange_MapFilter(object sender, EventArgs e)
        {
            string searchText = combo_gameTypes.Text.Trim();

            // If the text matches a known game type by index, use the existing filter logic
            int selectedIndex = combo_gameTypes.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < objectGameTypes.All.Count)
            {
                string shortName = objectGameTypes.All[selectedIndex].ShortName;
                foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
                {
                    string? cellValue = row.Cells["avail_MapType"].Value?.ToString();
                    row.Visible = string.Equals(cellValue, shortName, StringComparison.OrdinalIgnoreCase);
                }
                return;
            }

            // Otherwise, treat the text as a search for avail_MapName (case-insensitive, partial match)
            foreach (DataGridViewRow row in dataGridView_availableMaps.Rows)
            {
                string? mapName = row.Cells["avail_MapName"].Value?.ToString();
                row.Visible = !string.IsNullOrWhiteSpace(searchText) &&
                              !string.IsNullOrWhiteSpace(mapName) &&
                              mapName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
    }
}
