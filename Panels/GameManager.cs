using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Newtonsoft.Json;
using ServerManager.Classes.Enviroment;
using ServerManager.Classes.Objects;
using ServerManager.Properties;

namespace ServerManager.Panels;

public partial class GameManager : Form
{
    protected ServerEnvironment _environment;
    protected ServerInstance _instance;
    public GameManager(ServerInstance serverInstance)
    {
        _environment = ServerEnvironment.Instance;
        _instance = serverInstance;
        InitializeComponent();
        init_GameManager();
    }

    private void init_GameManager()
    {
        stageMapTab();
        resetPlayerList();
    }

    // Map Management
    private void stageMapTab()
    {
        // Load Maps
        loadDefaultMaps();
        // Init. Comboboxes
        setup_GameTypeCombo();        
        
    }
    private void setup_GameTypeCombo()
    {
        // GameType Combo Box
        var dataSource = new List<GameType>();
        GameType allTypes = new GameType
        {
            DatabaseId = 999,
            Name = "All",
            Bitmap = 999,
            ShortName = "All"
        };
        dataSource.AddRange(_environment._gameTypes);
        dataSource.Add(allTypes);
        comboBox_gameTypes.DataSource = dataSource;
        comboBox_gameTypes.DisplayMember = "Name";
        comboBox_gameTypes.ValueMember = "Bitmap";
        comboBox_gameTypes.SelectedItem = allTypes;
    }
    private void loadDefaultMaps()
    {
        // Clear the Available Maps List
        AvailableMapList.Rows.Clear();
        foreach (var map in _instance.mapManagement.DefaultMapList)
        {
            // Check if map properties are valid
            if (!string.IsNullOrEmpty(map.mission_name))
            {
                // Create a new row
                var row = new DataGridViewRow();
                foreach (int gametype in map.GameTypes)
                {
                    string gameTypeShort = _environment._gameTypes.Where(gt => gt.Bitmap == gametype).Select(t => t.ShortName).FirstOrDefault();
                    // Create cells with map details
                    row.CreateCells(AvailableMapList, map.id, map.mission_name, map.mission_file, gameTypeShort, map.game, map.CustomMap, gametype);    
                }
            
                // Add the row to the Available Maps List
                AvailableMapList.Rows.Add(row);
            }
            else
            {
                // Debugging output
                Console.WriteLine($"Map is invalid: Id = {map.id}, MapName = {map.mission_name}");
            }
        }
        foreach (var map in _instance.mapManagement.CustomMapList)
        {
            
            // Check if map properties are valid
            if (!string.IsNullOrEmpty(map.mission_name))
            {
                // Create a new row
                var row = new DataGridViewRow();
                foreach (int gamebits in map.GameTypes)
                {
                    string gameTypeShort = _environment._gameTypes.Where(gt => gt.Bitmap == gamebits).Select(t => t.ShortName).FirstOrDefault();
                    // Create cells with map details
                    row.CreateCells(AvailableMapList, map.id, map.mission_name, map.mission_file, gameTypeShort, map.game, map.CustomMap, gamebits);    
                }
            
                // Add the row to the Available Maps List
                AvailableMapList.Rows.Add(row);
            }
            else
            {
                // Debugging output
                Console.WriteLine($"Map is invalid: Id = {map.id}, MapName = {map.mission_name}");
            }
        }
        AvailableMapList.Refresh();
    }
    
    // Player List Functions
    private void resetPlayerList()
    {
        for (int slot = 1; slot <= 50; slot++)
        {
            if (slot > (int)_instance.serverSettings.Get("MaxSlots"))
            {
                update_playerSlot(slot, "Empty Slot", "", 999);
                continue;
            }
            update_playerSlot(slot, "Empty Slot", "", 0);
        }
    }
    private void update_playerSlot(int slot, string playerName, string playerAddress, int team)
    {
        // Slot Object
        string controlName = $"slot_" + (slot < 10 ? "0" + slot.ToString() : slot.ToString());
        var fieldInfo = this.GetType().GetField(controlName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        Panel control = fieldInfo.GetValue(this) as Panel;

        if (team == 999)
        {
            control.Controls.Clear();
            control.BackgroundImage = null;
            control.BorderStyle = BorderStyle.None;
            return;
        }
        
        control.Cursor = System.Windows.Forms.Cursors.Hand;
        if (team == 0) { control.BackgroundImage = Resources.playerTileBlack; }
        if (team == 1) { control.BackgroundImage = Resources.playerTileRed; }
        if (team == 2) { control.BackgroundImage = Resources.playerTileBlue; }

        control.Controls.Clear();
        
        // Label Player IP
        Label labelPlayerAddress = new Label();
        labelPlayerAddress.Font = new Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        labelPlayerAddress.Location = new Point(35, 5);
        labelPlayerAddress.Margin = new Padding(0);
        labelPlayerAddress.Size = new Size(100, 14);
        labelPlayerAddress.Text = playerName;
        labelPlayerAddress.TextAlign =ContentAlignment.MiddleRight;
        
        // Label Player IP
        Label labelPlayerName = new Label();
        labelPlayerName.Font = new Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        labelPlayerName.Location = new Point(60, 19);
        labelPlayerName.Size = new Size(75, 14);
        labelPlayerName.Text = playerAddress;
        labelPlayerName.TextAlign = ContentAlignment.MiddleRight;
        
        // Label Slot Number
        Label labelSlotNumber = new Label();
        labelSlotNumber.Font = new Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        labelSlotNumber.ForeColor = SystemColors.Desktop;
        labelSlotNumber.Location = new Point(24, 21);
        labelSlotNumber.Size = new Size(24, 20);
        labelSlotNumber.Text = (slot < 10 ? "0" + slot.ToString() : slot.ToString());
        labelSlotNumber.TextAlign = ContentAlignment.MiddleCenter;

        control.Controls.Add(labelPlayerAddress);
        control.Controls.Add(labelPlayerName);
        control.Controls.Add(labelSlotNumber);
        
    }

    // OnChange: Filter Maps
    private void onChange_GameTypeCombo(object sender, EventArgs e)
    {
        loadDefaultMaps();
    
        string selectedBits = comboBox_gameTypes.SelectedValue.ToString();
        if (selectedBits == "999") { return; }
        
        List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

        foreach (DataGridViewRow row in AvailableMapList.Rows)
        {
            if (row.Cells[6].Value.ToString() != selectedBits)
            {
                rowsToRemove.Add(row);
            }
        }

        foreach (DataGridViewRow row in rowsToRemove)
        {
            AvailableMapList.Rows.Remove(row);
        }

        AvailableMapList.Refresh();
    }
    
    // OnClick: Refresh Maps
    private void onClick_refreshMaps(object sender, EventArgs e)
    {
        // Load Default Maps
        loadDefaultMaps();
        onChange_GameTypeCombo(null, null);
    }
}