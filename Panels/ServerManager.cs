using System;
using System.Drawing;
using System.Security.AccessControl;
using System.Windows.Forms;
using Newtonsoft.Json;
using ServerManager.Classes.Enviroment;
using ServerManager.Classes.Modules;

namespace ServerManager.Panels
{
    public partial class ServerManager : Form
    {
        protected ServerEnvironment Env;
        protected Ticker _ticker;
        
        public ServerManager()
        {
            Load += ServerManager_Load;
            InitializeComponent();
        }
        // Application Load
        private void ServerManager_Load(object sender, EventArgs e)
        {
            // Load Enviroment
            Env = ServerEnvironment.Instance;
            
            // Tick Event (Instant & 5 Seconds)
            ServerManager_tickEvent();
            _ticker = new Ticker();
            _ticker.Start("Sever Manager Tick", 5000, ServerManager_tickEvent);
        }
        
        // Action: Profile List CellClick
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the correct column (7)
            if (e.ColumnIndex == 7 && e.RowIndex >= 0) // e.RowIndex >= 0 to avoid header click
            {
                string profileName = dataGrid_profiles.Rows[e.RowIndex].Cells["profileName"].Value.ToString();
                ServerProfile profile = Env._serverProfiles.ServerProfileList.Find(p => p.ProfileName == profileName);
                if (Env._serverInstances.TryGetValue(profile, out ServerInstance serverInstance))
                {
                    (new GameManager(serverInstance)).ShowDialog();
                }
            }
        }
        
        // Action: Create new profile
        private void click_addProfile(object sender, EventArgs e)
        {
            (new ServerProfileEditor(false)).ShowDialog();
        }

        // Action: Remove Profile
        private void click_removeProfile(object sender, EventArgs e)
        {
            // Is profile selected?
            if (dataGrid_profiles.SelectedRows.Count < 1) { return; }
            string profileName = dataGrid_profiles.SelectedRows[0].Cells["profileName"].Value.ToString();
            ServerProfile profile = Env._serverProfiles.ServerProfileList.Find(p => p.ProfileName == profileName);
            // Is Instance Running?
            if (Env._serverInstances.TryGetValue(profile, out ServerInstance serverInstance))
            {
                if (serverInstance.instance_Status().isRunning)
                {
                    MessageBox.Show("This profile is currently running.");
                    return;
                }    
            }

            if (Env._serverProfiles.RemoveProfile(profile))
            {
                MessageBox.Show("Profile Removed.");
                tickEvent_profileStatusUpdate();                 
            }
        }
        // Action: Close Application
        private void click_quit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        //
        // Tick Events
        //
        
        // Trigger for Tick Events for Application
        public void ServerManager_tickEvent()
        {
            // Profile Status Update
            tickEvent_profileStatusUpdate();
        }
        // Tick Event: Grab Profile Status
        private void tickEvent_profileStatusUpdate()
        {
            // Check if we need to invoke on the UI thread
            if (dataGrid_profiles.InvokeRequired)
            {
                // Use Invoke to call the method on the UI thread
                dataGrid_profiles.Invoke(new Action(ServerManager_tickEvent));
                return;
            }
            
            // Update Profiles
            dataGrid_profiles.Rows.Clear();
            foreach (var profile in Env._serverInstances)
            {
                // Instance Object
                ServerInstance instance = profile.Value;
                statusObject thisVar = instance.instance_Status();
                var row = new DataGridViewRow();
                row.CreateCells(dataGrid_profiles);
                
                // Set the cell values
                row.Cells[0].Value = thisVar.profileName; // Assuming index 0 corresponds to profileName
                row.Cells[1].Value = thisVar.statusByte; // Assuming index 1 corresponds to statusByte
                row.Cells[2].Value = thisVar.serverPlayerCount; // Assuming index 2 corresponds to serverPlayerCount
                row.Cells[3].Value = thisVar.currentMap; // Assuming index 3 corresponds to currentMap
                row.Cells[4].Value = thisVar.currentMapGameType; // Assuming index 4 corresponds to currentMapGameType
                row.Cells[5].Value = thisVar.currentTimer; // Assuming index 5 corresponds to currentTimer
                row.Cells[6].Value = thisVar.serverStatStatus; // Assuming index 6 corresponds to serverStatStatus
                row.Cells[7].Value = "MANAGE";
                row.Cells[7].Style.Font = new Font("Ariel", 8);
        
                // Add the row to the DataGridView
                dataGrid_profiles.Rows.Add(row);
                dataGrid_profiles.Refresh();
            }
        }
        
    }
}