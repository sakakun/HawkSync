using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHD_ServerManager.Classes.GameManagement;

namespace BHD_ServerManager.Classes.Tickers
{
    public class tickerChatManagement()
    {
        private readonly static theInstance thisInstance = CommonCore.theInstance!;
        private readonly static chatInstance instanceChat = CommonCore.instanceChat!;

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
                    AppDebug.Log("tickerChatManagement", $"Error invoking runTicker: {ex.Message}");
                }

                return;
            }

            lock (tickerLock)
            {
                if (thisInstance.instanceStatus == InstanceStatus.OFFLINE || thisInstance.instanceStatus == InstanceStatus.LOADINGMAP || thisInstance.instanceStatus == InstanceStatus.SCORING)
                {
                    return;
                }

                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // Process Auto Messages
                    ProcessAutoMessages();
                    // Process Latest Chat Message
                    ProcessChatMessages();
                    // Update Chat Messages Grid
                    UpdateChatMessagesGrid(thisServer);
                }
                else
                {
                    // If the server process is not attached, we can assume the server is offline.
                    AppDebug.Log("tickerChatManagement", "Server process is not attached. Ticker Skipping.");
                }
            }
        }
        public static void ProcessChatMessages()
        {
            var latestMessage = ServerMemory.ReadMemoryLastChatMessage();
            if (latestMessage == null || latestMessage.Length < 3)
                return;

            if (!int.TryParse(latestMessage[0], out int chatLogAddr))
                return;

            string lastMessage = latestMessage[1];
            string msgTypeBytes = latestMessage[2];

            if (string.IsNullOrWhiteSpace(lastMessage))
                return;

            var chatLog = instanceChat.ChatLog;

            // Extract player name and message
            int msgStart = lastMessage.IndexOf(':');
            if (msgStart < 0)
                return;

            string playerName = lastMessage.Substring(0, msgStart).Trim();
            string playerMessage = lastMessage.Substring(msgStart + 1).Trim();

            // Prevent duplicate messages
            if (chatLog.Count > 0)
            {
                var lastLog = chatLog[^1];
                if (lastLog.PlayerName == playerName && lastLog.MessageText == playerMessage)
                    return;
            }

            // Message type mapping
            int msgType = msgTypeBytes switch
            {
                "00FFFFFF" => 0, // host
                "FFC0A0FF" => 1, // global
                "00FF00FF" => 2, // teamchat
                _ => 3
            };

            if (msgType == 0)
                ServerMemory.WriteMemoryChatCountDownKiller(chatLogAddr);

            // Find PlayerTeam number
            int teamNum = 3;

            foreach (var item in thisInstance.playerList)
            {
                if (item.Value.PlayerName == playerName)
                {
                    teamNum = item.Value.PlayerTeam;
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
            AppDebug.Log("tickerChatManagement", $"Chat Message: {playerName} ({teamNum}) - {playerMessage} (Type: {msgType})");
        }
        public static void ProcessAutoMessages()
        {
            // Do not send auto messages during scoring
            if (thisInstance.instanceStatus == InstanceStatus.SCORING)
            {
                instanceChat.AutoMessageCounter = 0;
                return;
            }

            // Ensure there are messages to send
            if (instanceChat.AutoMessages == null || instanceChat.AutoMessages.Count == 0)
                return;

            // Calculate elapsed seconds since map started
            double elapsedMinutes = (thisInstance.gameTimeLimit) - thisInstance.gameInfoTimeRemaining.TotalMinutes;

            // Send all messages whose trigger time has passed, but only once each
            while (instanceChat.AutoMessageCounter < instanceChat.AutoMessages.Count)
            {
                // Calculate the time since the last auto message was sent
                TimeSpan timeSinceLastMessage = DateTime.Now - instanceChat.lastAutoMessageSent;

                // Wait 10 seconds between messages
                if (instanceChat.lastAutoMessageSent != DateTime.MinValue &&
                    timeSinceLastMessage.TotalSeconds < 10)
                {
                    // Not enough time has passed since the last message
                    break;
                }

                var autoMsg = instanceChat.AutoMessages[instanceChat.AutoMessageCounter];

                if (elapsedMinutes >= autoMsg.AutoMessageTigger)
                {
                    // Send the message (replace with your actual send logic)
                    ServerMemory.WriteMemorySendChatMessage(1, autoMsg.AutoMessageText);

                    // Move to the next message
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
        private static void UpdateChatMessagesGrid(ServerManager thisServer)
        {
            if (thisServer.dataGridView_chatMessages.InvokeRequired)
            {
                thisServer.dataGridView_chatMessages.Invoke(new Action(() => UpdateChatMessagesGrid(thisServer)));
                return;
            }

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
