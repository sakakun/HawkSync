using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHD_ServerManager.Classes.GameManagement;

namespace BHD_ServerManager.Classes.Tickers
{
    public static class tickerChatManagement
    {
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        private static readonly object tickerLock = new();

        // For deduplication of chat messages
        private static string? _lastProcessedPlayerName = null;
        private static string? _lastProcessedMessageText = null;

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        public static void runTicker()
        {
            // Always ensure UI thread safety for UI-bound operations
            if (thisServer.InvokeRequired)
            {
                try
                {
                    thisServer.BeginInvoke(new Action(runTicker));
                }
                catch (Exception ex)
                {
                    AppDebug.Log("tickerChatManagement", $"Error invoking runTicker: {ex.Message}");
                }
                return;
            }

            lock (tickerLock)
            {
                // Only process chat when server is online or in start delay
                if (thisInstance.instanceStatus != InstanceStatus.ONLINE &&
                    thisInstance.instanceStatus != InstanceStatus.STARTDELAY)
                {
                    return;
                }

                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // Process auto messages (non-blocking)
                    Task.Run(ProcessAutoMessages);

                    // Process latest chat message and update UI (non-blocking)
                    Task.Run(() =>
                    {
                        ProcessChatMessages();
                        SafeInvoke(thisServer.dataGridView_chatMessages, () =>
                        {
                            UpdateChatMessagesGrid(thisServer);
                        });
                    });
                }
                else
                {
                    AppDebug.Log("tickerChatManagement", "Server process is not attached. Ticker Skipping.");
                }
            }
        }

        public static void ProcessChatMessages()
        {
            var latestMessage = ServerMemory.ReadMemoryLastChatMessage();
            if (latestMessage == null || latestMessage.Length < 3)
                return;

            string lastMessage = latestMessage[1];
            string msgTypeBytes = latestMessage[2];

            if (string.IsNullOrWhiteSpace(lastMessage))
                return;

            var chatLog = instanceChat.ChatLog;
            if (chatLog == null)
                return;

            int msgStart = lastMessage.IndexOf(':');
            if (msgStart < 0)
                return;

            string playerName = lastMessage.Substring(0, msgStart).Trim();
            string playerMessage = lastMessage.Substring(msgStart + 1).Trim();

            // Prevent duplicate messages based on last processed message
            if (_lastProcessedPlayerName == playerName && _lastProcessedMessageText == playerMessage)
                return;

            lock (chatLog)
            {
                // Message type mapping
                int msgType = msgTypeBytes switch
                {
                    "00FFFFFF" => 0, // host
                    "FFC0A0FF" => 1, // global
                    "00FF00FF" => 2, // teamchat
                    _ => 3
                };

                // If host message, trigger countdown killer
                if (msgType == 0 && int.TryParse(latestMessage[0], out int chatLogAddr))
                    ServerMemory.WriteMemoryChatCountDownKiller(chatLogAddr);

                // Find PlayerTeam number
                int teamNum = 3;
                foreach (var player in thisInstance.playerList.Values)
                {
                    if (player.PlayerName == playerName)
                    {
                        teamNum = player.PlayerTeam;
                        break;
                    }
                }

                chatLog.Add(new ChatLogObject
                {
                    PlayerName = playerName,
                    MessageText = playerMessage,
                    MessageType = msgType,
                    MessageType2 = teamNum,
                    MessageTimeStamp = DateTime.Now
                });

                // Update last processed message for deduplication
                _lastProcessedPlayerName = playerName;
                _lastProcessedMessageText = playerMessage;

                AppDebug.Log("tickerChatManagement", $"Chat Message: {playerName} ({teamNum}) - {playerMessage} (Type: {msgType})");
            }
        }

        public static void ProcessAutoMessages()
        {
            // Do not send auto messages during scoring
            if (thisInstance.instanceStatus == InstanceStatus.SCORING)
            {
                instanceChat.AutoMessageCounter = 0;
                return;
            }

            var autoMessages = instanceChat.AutoMessages;
            if (autoMessages == null || autoMessages.Count == 0)
                return;

            // Calculate elapsed minutes since map started
            double elapsedMinutes = Math.Max(0, thisInstance.gameTimeLimit - thisInstance.gameInfoTimeRemaining.TotalMinutes);

            lock (autoMessages)
            {
                // Reset counter if out of range (e.g., map restart)
                if (instanceChat.AutoMessageCounter < 0 || instanceChat.AutoMessageCounter >= autoMessages.Count)
                    instanceChat.AutoMessageCounter = 0;

                while (instanceChat.AutoMessageCounter < autoMessages.Count)
                {
                    var autoMsg = autoMessages[instanceChat.AutoMessageCounter];

                    // Wait 10 seconds between messages
                    TimeSpan timeSinceLastMessage = DateTime.Now - instanceChat.lastAutoMessageSent;
                    if (instanceChat.lastAutoMessageSent != DateTime.MinValue &&
                        timeSinceLastMessage.TotalSeconds < 10)
                    {
                        break;
                    }

                    if (elapsedMinutes >= autoMsg.AutoMessageTigger)
                    {
                        ServerMemory.WriteMemorySendChatMessage(1, autoMsg.AutoMessageText);
                        instanceChat.AutoMessageCounter++;
                        instanceChat.lastAutoMessageSent = DateTime.Now;
                    }
                    else
                    {
                        // The next message's trigger time hasn't been reached yet
                        break;
                    }
                }
            }
        }

        private static void UpdateChatMessagesGrid(ServerManager thisServer)
        {
            int lastChatLogIndex = instanceChat.lastChatLogIndex;
            List<ChatLogObject> chatLog = instanceChat.ChatLog;

            if (chatLog == null || chatLog.Count == 0 || lastChatLogIndex >= chatLog.Count)
                return;

            for (int i = lastChatLogIndex; i < chatLog.Count; i++)
            {
                var entry = chatLog[i];
                string teamString = entry.MessageType switch
                {
                    0 => "Server",
                    1 => "Global",
                    3 => "Other",
                    2 => entry.MessageType2 switch
                    {
                        1 => "Blue",
                        2 => "Red",
                        _ => "Other"
                    },
                    _ => "Other"
                };

                // Sanitize player name
                entry.PlayerName = Functions.SanitizePlayerName(entry.PlayerName);

                thisServer.dataGridView_chatMessages.Rows.Add(
                    entry.MessageTimeStamp.ToString("HH:mm:ss"),
                    teamString,
                    entry.PlayerName,
                    entry.MessageText
                );
            }

            instanceChat.lastChatLogIndex = chatLog.Count;

            // Auto-scroll to the bottom if there are rows
            var dgv = thisServer.dataGridView_chatMessages;
            if (dgv.Rows.Count > 0)
            {
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
            }
        }
    }
}