using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;

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
                    // Recover auto message counter before processing auto messages
                    RecoverAutoMessageCounter();

                    // Process auto messages (non-blocking)
                    Task.Run(ProcessAutoMessages);

                    // Process latest chat message and update UI (non-blocking)
                    Task.Run(ProcessChatMessages);
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

                ChatLogObject newChatLog = new ChatLogObject
                {
                    PlayerName = playerName,
                    MessageText = playerMessage,
                    MessageType = msgType,
                    MessageType2 = teamNum,
                    MessageTimeStamp = DateTime.Now
                };

                chatLog.Add(newChatLog);

                chatInstanceManager.SaveChatLogEntry(newChatLog);

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


    }
}