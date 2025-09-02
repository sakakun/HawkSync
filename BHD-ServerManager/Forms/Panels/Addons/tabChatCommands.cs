using BHD_ServerManager.Classes.GameManagement;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Chat;
using Windows.Storage;

namespace BHD_ServerManager.Forms.Panels.Addons
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

        }

        public void functionAction_GetSettings(theInstance updatedInstance = null!)
        {
            theInstance thisInstance = updatedInstance != null ? updatedInstance : theInstance;

            // Chat Command: Skip Map
            cb_ccEnableSkipping.Checked = thisInstance.chatCommandSkipMap_Enabled;
            num_SkipPercentRequired.Value = thisInstance.chatCommandSkipMap_VotePercent;
            num_SkipVotingStarts.Value = thisInstance.chatCommandSkipMap_VotingStarts;
            num_SkipVotingPeriod.Value = thisInstance.chatCommandSkipMap_VotingPeriod;
            num_SkipVotingMaxSessions.Value = thisInstance.chatCommandSkipMap_MaxVotingSessions;

        }

        public void functionAction_SetSettings()
        {

            // Chat Coommand: Skip Map
            theInstance.chatCommandSkipMap_Enabled = cb_ccEnableSkipping.Checked;
            theInstance.chatCommandSkipMap_VotePercent = (int)num_SkipPercentRequired.Value;
            theInstance.chatCommandSkipMap_VotingStarts = (int)num_SkipVotingStarts.Value;
            theInstance.chatCommandSkipMap_VotingPeriod = (int)num_SkipVotingPeriod.Value;
            theInstance.chatCommandSkipMap_MaxVotingSessions = (int)num_SkipVotingMaxSessions.Value;

        }

        // Ticker Hook for the "Settings" Updates
        public void TickerChatCommandsHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                functionAction_GetSettings();
            }

            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP ||
                theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                // Command: !skipmap Voting Reset
                _chatCommandSkipMap_VotingSessions = 0;
                ResetSkipMapVotingState();

            }

            if (theInstance.instanceStatus == InstanceStatus.ONLINE)
            {
                ChatCommandSkipMapTicker();
            }

        }

        public void ChatCommandSkipMapTicker()
        {
            lock (_skipMapTickerLock)
            {
                if (!theInstance.chatCommandSkipMap_Enabled) return;
                if (_chatCommandSkipMap_VotingSessions >= theInstance.chatCommandSkipMap_MaxVotingSessions) return;

                double bufferSeconds = (theInstance.chatCommandSkipMap_VotingPeriod * 60) + 20;

                // Start voting session
                if (!_chatCommandSkipMap_VotingStarted &&
                    theInstance.gameInfoTimeRemaining.TotalMinutes < (theInstance.gameTimeLimit - theInstance.chatCommandSkipMap_VotingStarts) &&
                    (DateTime.Now - _lastVotingStart).TotalSeconds > bufferSeconds)
                {
                    _chatCommandSkipMap_VotingSessions++;
                    _chatCommandSkipMap_VotedPlayers.Clear();
                    _chatCommandSkipMap_VotingStarted = true;
                    _chatCommandSkipMap_VotingPassed = false;
                    _lastVotingStart = DateTime.Now;

                    int nextMapIndex = theInstance.gameInfoCurrentMapIndex >= instanceMaps.currentMapPlaylist.Count - 1
                                || theInstance.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance.gameInfoCurrentMapIndex + 1;

                    mapFileInfo mapInfo = instanceMaps.currentMapPlaylist[nextMapIndex];
                    mapFileInfo mapInfo2 = instanceMaps.currentMapPlaylist[nextMapIndex+1];
                    chatInstanceManagers.SendMessageToQueue(false, 1, $"Next Map is {mapInfo.MapName} ({mapInfo.MapType}).");
                    chatInstanceManagers.SendMessageToQueue(false, 1, $"Skipping will play {mapInfo2.MapName} ({mapInfo2.MapType}) next.");
                    chatInstanceManagers.SendMessageToQueue(false, 1, $"Skip Map voting session {_chatCommandSkipMap_VotingSessions} is now open!");
                    chatInstanceManagers.SendMessageToQueue(false, 1, "Type !skipmap in chat to vote to skip the next map.");
                    chatInstanceManagers.SendMessageToQueue(false, 1, $"Voting session {_chatCommandSkipMap_VotingSessions} ends in {theInstance.chatCommandSkipMap_VotingPeriod} minutes.");

                    _chatCommandSkipMap_VotingEnds = DateTime.Now.AddMinutes(theInstance.chatCommandSkipMap_VotingPeriod);
                    return;
                }

                AppDebug.Log("Voting Logic", $"Voting Started: {_chatCommandSkipMap_VotingStarted}, {DateTime.Now} - {_chatCommandSkipMap_VotingEnds}" );

                // Voting ended, check if passed
                if (_chatCommandSkipMap_VotingStarted && DateTime.Now >= _chatCommandSkipMap_VotingEnds)
                {
                    int nextMapIndex = theInstance.gameInfoCurrentMapIndex >= instanceMaps.currentMapPlaylist.Count - 1
                                || theInstance.gameInfoCurrentMapIndex < 0
                                ? 0
                                : theInstance.gameInfoCurrentMapIndex + 1;

                    int currentPlayers = theInstance.gameInfoCurrentNumPlayers;
                    int votesNeeded = (int)Math.Ceiling((theInstance.chatCommandSkipMap_VotePercent / 100.0) * currentPlayers);
                    int votesReceived = _chatCommandSkipMap_VotedPlayers.Count;

                    if (votesReceived >= votesNeeded && currentPlayers > 0)
                    {
                        _chatCommandSkipMap_VotingPassed = true;
                        _chatCommandSkipMap_VotingEnds = DateTime.Now; // End voting immediately
                    }

                    if (_chatCommandSkipMap_VotingPassed)
                    {
                        AppDebug.Log("VotingSkipMap", "Vote Passed");
                        chatInstanceManagers.SendMessageToQueue(false, 1, $"Voting session {_chatCommandSkipMap_VotingSessions} passed! The next map will not be played.");
                        GameManager.UpdateNextMap(nextMapIndex + 1);
                        ResetSkipMapVotingState();
                    }
                    else
                    {
                        chatInstanceManagers.SendMessageToQueue(false, 1, $"Skip Map voting session {_chatCommandSkipMap_VotingSessions} has ended. Map will be played.");
                        ResetSkipMapVotingState();
                        _chatCommandSkipMap_VotingSessions = theInstance.chatCommandSkipMap_MaxVotingSessions;
                    }

                    mapFileInfo mapInfo = instanceMaps.currentMapPlaylist[nextMapIndex + 1];
                    chatInstanceManagers.SendMessageToQueue(false, 1, $"Next Map will be {mapInfo.MapName} ({mapInfo.MapType})");
                }
            }
        }

        private void ResetSkipMapVotingState()
        {
            _chatCommandSkipMap_VotingStarted = false;
            _chatCommandSkipMap_VotedPlayers.Clear();
            _chatCommandSkipMap_VotingEnds = DateTime.MinValue;
            _chatCommandSkipMap_VotingPassed = false;
        }

        public void ChatCommandSkipChatHook(ChatLogObject chatMessage)
        {
            if (!theInstance.chatCommandSkipMap_Enabled) return;

            // Prevent votes after voting has ended
            if (!_chatCommandSkipMap_VotingStarted || DateTime.Now >= _chatCommandSkipMap_VotingEnds)
                return;

            AppDebug.Log("SkipMap", $"Message Sent: {chatMessage.MessageText}");
            if (chatMessage.MessageText != null && chatMessage.MessageText.StartsWith("!skipmap", StringComparison.OrdinalIgnoreCase))
            {
                if (!_chatCommandSkipMap_VotedPlayers.Contains(chatMessage.PlayerName))
                {
                    _chatCommandSkipMap_VotedPlayers.Add(chatMessage.PlayerName);
                    int currentPlayers = theInstance.gameInfoCurrentNumPlayers;
                    int votesNeeded = (int)Math.Ceiling((theInstance.chatCommandSkipMap_VotePercent / 100.0) * currentPlayers);
                    int votesReceived = _chatCommandSkipMap_VotedPlayers.Count;

                    ServerMemory.WriteMemorySendChatMessage(1, $"{chatMessage.PlayerName} has voted to skip the next map. ({votesReceived}/{votesNeeded} votes)");
                }
            }
        }

        private void actionClick_ResetSettings(object sender, EventArgs e)
        {
            functionAction_GetSettings();
        }

        private void actionClick_SaveSettings(object sender, EventArgs e)
        {
            functionAction_SetSettings();               // Set the Instance Settings from the UI to the Instance Object.
            theInstanceManager.SaveSettings();          // Save the Instance Settings to file.
        }
    }
}
