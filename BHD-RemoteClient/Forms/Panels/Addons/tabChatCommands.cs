using BHD_RemoteClient.Classes.GameManagement;
using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Chat;
using Windows.Storage;

namespace BHD_RemoteClient.Forms.Panels.Addons
{
    public partial class tabChatCommands : UserControl
    {

        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private mapInstance instanceMaps => CommonCore.instanceMaps;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;

        // --- Chat Commands Skip Map Variables ---
        private bool _chatCommandSkipMap_VotingStarted = false;
        private bool _chatCommandSkipMap_VotingPassed = false;
        private int _chatCommandSkipMap_VotingSessions = 0;
        private List<string> _chatCommandSkipMap_VotedPlayers = new List<string>();
        private DateTime _chatCommandSkipMap_VotingEnds = DateTime.MinValue;

        // Thread Blocking
        private readonly object _skipMapTickerLock = new();
        private readonly object _skipMapTicker2Lock = new();
        private readonly object _skipMapTicker3Lock = new();
        private DateTime _lastVotingStart = DateTime.MinValue;

        // --- Class Variables ---
        private new string Name = "ChatCommands";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        public tabChatCommands()
        {
            InitializeComponent();
        }
        public void functionAction_HighlightChanges()
        {
            // Compare each UI field to theInstance property and highlight if different
            HighlightControl(cb_ccEnableSkipping, cb_ccEnableSkipping.Checked != theInstance.chatCommandSkipMap_Enabled);
            HighlightControl(cb_ccEnableStartDelaySkipMap, cb_ccEnableStartDelaySkipMap.Checked != theInstance.chatCommandSkipMap_StartDelay);
            HighlightControl(cb_ccEnableInGameSkipMap, cb_ccEnableInGameSkipMap.Checked != theInstance.chatCommandSkipMap_InGame);
            HighlightControl(num_SkipPercentRequired, (int)num_SkipPercentRequired.Value != theInstance.chatCommandSkipMap_VotePercent);
            HighlightControl(num_SkipVotingStarts, (int)num_SkipVotingStarts.Value != theInstance.chatCommandSkipMap_VotingStarts);
            HighlightControl(num_SkipVotingPeriod, (int)num_SkipVotingPeriod.Value != theInstance.chatCommandSkipMap_VotingPeriod);
            HighlightControl(num_SkipVotingMaxSessions, (int)num_SkipVotingMaxSessions.Value != theInstance.chatCommandSkipMap_MaxVotingSessions);

        }

        // Helper to highlight controls
        private void HighlightControl(Control control, bool highlight)
        {
            control.BackColor = highlight ? Color.LightYellow : SystemColors.Window;
        }

        public void functionEvent_GetChatCommandSettings(theInstance updatedInstance = null!)
        {
            theInstance thisInstance = updatedInstance != null ? updatedInstance : theInstance;

            // Chat Command: Skip Map
            cb_ccEnableSkipping.Checked = thisInstance.chatCommandSkipMap_Enabled;
            cb_ccEnableStartDelaySkipMap.Checked = thisInstance.chatCommandSkipMap_StartDelay;
            cb_ccEnableInGameSkipMap.Checked = thisInstance.chatCommandSkipMap_InGame;
            num_SkipPercentRequired.Value = thisInstance.chatCommandSkipMap_VotePercent;
            num_SkipVotingStarts.Value = thisInstance.chatCommandSkipMap_VotingStarts;
            num_SkipVotingPeriod.Value = thisInstance.chatCommandSkipMap_VotingPeriod;
            num_SkipVotingMaxSessions.Value = thisInstance.chatCommandSkipMap_MaxVotingSessions;

            // Chat Command: Console Commands
            label_consoleCommands.Text = "Feature: " + (thisInstance.chatCommandConsoleCommands ? "Enabled" : "Disabled");
            btn_commandConsole.Enabled = thisInstance.chatCommandConsoleCommands;
        }

        public void functionEvent_SetChatCommandSettings()
        {

            theInstance updatedInfo = JsonSerializer.Deserialize<theInstance>(JsonSerializer.Serialize(theInstance))!;


            // Chat Coommand: Skip Map
            updatedInfo.chatCommandSkipMap_Enabled = cb_ccEnableSkipping.Checked;
            updatedInfo.chatCommandSkipMap_StartDelay = cb_ccEnableStartDelaySkipMap.Checked;
            updatedInfo.chatCommandSkipMap_InGame = cb_ccEnableInGameSkipMap.Checked;
            updatedInfo.chatCommandSkipMap_VotePercent = (int)num_SkipPercentRequired.Value;
            updatedInfo.chatCommandSkipMap_VotingStarts = (int)num_SkipVotingStarts.Value;
            updatedInfo.chatCommandSkipMap_VotingPeriod = (int)num_SkipVotingPeriod.Value;
            updatedInfo.chatCommandSkipMap_MaxVotingSessions = (int)num_SkipVotingMaxSessions.Value;

            if (CmdSetServerVariables.ProcessCommand(updatedInfo))
            {
                MessageBox.Show("Server settings applied successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppDebug.Log(this.Name, "Server settings applied.");
            }
            else
            {
                AppDebug.Log(this.Name, "Failed to apply server settings."); // Apply the new settings

            }

        }

        public void functionEvent_AttachGameClient()
        {
            string file_name = "dfbhd.exe";
            bool attached = false;

            foreach (var searchProcess in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(file_name)))
            {
                try
                {
                    AppDebug.Log("tabChatCommands", $"Found game process: {searchProcess.ProcessName} (PID: {searchProcess.Id})");
                    theInstance.instanceAttachedPID = searchProcess.Id;
                    theInstance.instanceProcessHandle = searchProcess.Handle;
                    ClientMemory.AttachToGameProcess(searchProcess.Id);
                    attached = true;
                    MessageBox.Show("Successfully attached to the game process.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                catch (Win32Exception)
                {
                    continue;
                }
            }

            AppDebug.Log("tabChatCommands", $"'{theInstance.instanceAttachedPID}': {theInstance.instanceProcess}");

            if (!attached)
            {
                AppDebug.Log("tabChatCommands", "No accessible game process found.");
                MessageBox.Show("No accessible game process found. Please ensure the game is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void functionEvent_IsGameClientAttached()
        {
            label_consoleCommands.Text = "Feature: " + (theInstance.chatCommandConsoleCommands ? "Enabled" : "Disabled");
            btn_commandConsole.Text = ClientMemory.ReadMemoryIsProcessAttached() ? "Game Attached" : "Attach Game";
            btn_commandConsole.Enabled = !ClientMemory.ReadMemoryIsProcessAttached() && theInstance.chatCommandConsoleCommands;
        }


        // Ticker Hook for the "Settings" Updates
        public void TickerChatCommandsHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                functionEvent_GetChatCommandSettings();
            }
            // Check Game/Client Status
            functionEvent_IsGameClientAttached();

            // Highlight Changes
            functionAction_HighlightChanges();
        }




        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            functionEvent_GetChatCommandSettings();
        }

        private void actionClick_SaveSettings(object sender, EventArgs e)
        {
            functionEvent_SetChatCommandSettings();               // Set the Instance Settings from the UI to the Instance Object.
        }

        private void actionClick_AttachGameClient(object sender, EventArgs e)
        {
            // Check Game/Client Status
            functionEvent_AttachGameClient();
        }

    }
}
