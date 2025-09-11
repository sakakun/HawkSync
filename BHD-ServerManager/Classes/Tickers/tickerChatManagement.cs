using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Windows.Forms;

namespace BHD_ServerManager.Classes.Tickers
{
    public static class tickerChatManagement
    {
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        private static readonly object tickerLock = new();
        private static bool _autoMessageRecoveryDone = false;

        // For deduplication of chat messages
        private static string? _lastProcessedPlayerName = null;
        private static string? _lastProcessedMessageText = null;

        // Helper for UI thread safety

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

                // Ensure the chat tab is initialized before proceeding
                thisServer.ChatTab.ChatTickerHook();


                if (ServerMemory.ReadMemoryIsProcessAttached())
                {
                    // --- Incoming Chat Messages ---
                    // Process latest chat message and update UI (non-blocking)
                    Task.Run(ProcessChatMessages);

                    // --- Outgoing Chat/Console Messages ---
                    // Recover auto message counter before processing auto messages
                    RecoverAutoMessageCounter();
                    // Process auto messages (non-blocking)
                    Task.Run(ProcessAutoMessages);
                    // Process Queued Messages (non-blocking)
                    Task.Run(ProcessQueuedMessages);
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

            lock (chatLog)
            {
                // Prevent duplicate messages based on last processed message
                if (_lastProcessedPlayerName == playerName && _lastProcessedMessageText == playerMessage)
                    return;

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

                ChatLogObject newChat = new ChatLogObject
                {
                    PlayerName = playerName,
                    MessageText = playerMessage,
                    MessageType = msgType,
                    MessageType2 = teamNum,
                    MessageTimeStamp = DateTime.Now
                };

                chatLog.Add(newChat);
                // --- Chat Command Hooks ---
                // Ignore Server Messages
                if (newChat.MessageType != 0)
                {
                    thisServer.AddonsTab.ChatCommandsTab.TickerChatCommandsHook2(newChat);
                }

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

            


            // Calculate elapsed minutes since game play started (after start delay)
            double elapsedMinutes = Math.Max(0, thisInstance.gameTimeLimit - thisInstance.gameInfoTimeRemaining.TotalMinutes);

            lock (autoMessages)
            {
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
                        chatInstanceManagers.SendMessageToQueue(false, 1, autoMsg.AutoMessageText);
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

        public static void RecoverAutoMessageCounter()
        {
            // Only run recovery once per session
            if (_autoMessageRecoveryDone)
                return;

            var autoMessages = instanceChat.AutoMessages;
            if (autoMessages == null || autoMessages.Count == 0)
                return;

            // Calculate elapsed minutes since map started
            double elapsedMinutes = Math.Max(0, thisInstance.gameTimeLimit - thisInstance.gameInfoTimeRemaining.TotalMinutes);

            int counter = 0;
            foreach (var autoMsg in autoMessages)
            {
                if (elapsedMinutes >= autoMsg.AutoMessageTigger)
                    counter++;
                else
                    break;
            }
            instanceChat.AutoMessageCounter = counter;

            // If at least one message should have been sent, and lastAutoMessageSent is MinValue, set it to now
            if (counter > 0 && instanceChat.lastAutoMessageSent == DateTime.MinValue)
            {
                instanceChat.lastAutoMessageSent = DateTime.Now;
            }

            // Mark recovery as done
            _autoMessageRecoveryDone = true;
        }

        public static void ProcessQueuedMessages()
        {
            var messageQueue = instanceChat.MessageQueue;
            if (messageQueue == null || messageQueue.Count == 0)
                return;
            List<int> keysToRemove = new List<int>();

            lock (messageQueue)
            {
                // Sort the keys in ascending order
                var sortedKeys = messageQueue.Keys.OrderBy(k => k).ToList();
                foreach (var recordID in sortedKeys)
                {
                    ChatQueueObject msgObj = messageQueue[recordID];
                    Thread.Sleep(1000); // 1 second delay between messages
                    if (msgObj.ConsoleMsg)
                    {
                        // Send the console message
                        ServerMemory.WriteMemorySendConsoleCommand(msgObj.MessageText);
                        keysToRemove.Add(recordID);
                        continue;
                    }
                    else
                    {
                        string message = msgObj.MessageText;
                        if (message.Length > 55)
                        {
                            // Split message into chunks without breaking words
                            int maxLen = 55;
                            int start = 0;
                            while (start < message.Length)
                            {
                                int length = Math.Min(maxLen, message.Length - start);
                                // If the chunk ends in the middle of a word, move back to the last space
                                if (start + length < message.Length && !char.IsWhiteSpace(message[start + length]))
                                {
                                    int lastSpace = message.LastIndexOf(' ', start + length - 1, length);
                                    if (lastSpace > start)
                                    {
                                        length = lastSpace - start;
                                    }
                                }
                                string chunk = message.Substring(start, length).Trim();
                                ServerMemory.WriteMemorySendChatMessage(msgObj.MessageType, chunk);
                                Thread.Sleep(1000);
                                start += length;
                                // Skip any spaces at the start of the next chunk
                                while (start < message.Length && char.IsWhiteSpace(message[start]))
                                    start++;
                            }
                        }
                        else
                        {
                            ServerMemory.WriteMemorySendChatMessage(msgObj.MessageType, msgObj.MessageText);
                        }

                        // Send the chat message
                        keysToRemove.Add(recordID);
                        continue;
                    }
                }
                // Remove processed messages from the queue
                foreach (var key in keysToRemove)
                {
                    messageQueue.Remove(key);
                }
            }
        }

    }
}