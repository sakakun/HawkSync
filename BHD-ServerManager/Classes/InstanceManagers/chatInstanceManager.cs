using BHD_ServerManager.Forms;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class chatInstanceManager
    {
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

        public static OperationResult LoadSettings()
        {
            try
            {
                instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();
                instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();

                return new OperationResult(true, "Settings loaded successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error loading chat settings", AppDebug.LogLevel.Error, ex);

                instanceChat.SlapMessages = new List<SlapMessages>();
                instanceChat.AutoMessages = new List<AutoMessages>();

                return new OperationResult(false, $"Error loading settings: {ex.Message}", 0, ex);
            }
        }

        // --- Grid Update Helper ---
        public static void UpdateGridWithMessages<T>(DataGridView dgv, IReadOnlyList<T> messages, params int[] columnIndices)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action(() => UpdateGridWithMessages(dgv, messages, columnIndices)));
                return;
            }

            int scrollIndex = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;

            // With this:
            var managerDict = messages.ToDictionary(
                m =>
                {
                    var idProp = m?.GetType().GetProperty("SlapMessageId") ?? m?.GetType().GetProperty("AutoMessageId");
                    return Convert.ToInt32(idProp?.GetValue(m));
                }
            );

            // Remove rows not in manager
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                bool exists = messages.Any(m =>
                {
                    var type = m?.GetType();
                    var slapProp = type?.GetProperty("SlapMessageId");
                    var autoProp = type?.GetProperty("AutoMessageId");
                    object? value = slapProp?.GetValue(m) ?? autoProp?.GetValue(m);
                    return value != null && Convert.ToInt32(value) == id;
                });
                if (!exists)
                    dgv.Rows.RemoveAt(i);
            }

            // Update existing rows and add new ones
            foreach (var msg in messages)
            {
                var msgType = msg?.GetType();
                var slapProp = msgType?.GetProperty("SlapMessageId");
                var autoProp = msgType?.GetProperty("AutoMessageId");
                object? idValue = slapProp?.GetValue(msg) ?? autoProp?.GetValue(msg);

                if (idValue == null)
                    continue; // Skip if no ID property found

                int id = Convert.ToInt32(idValue);
                bool found = false;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    int rowId = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                    if (rowId == id)
                    {
                        for (int c = 1; c < columnIndices.Length; c++)
                        {
                            var prop = msgType?.GetProperties()[columnIndices[c]];
                            if (prop != null)
                            {
                                var propValue = prop.GetValue(msg);
                                if (!Equals(dgv.Rows[i].Cells[c].Value, propValue))
                                    dgv.Rows[i].Cells[c].Value = propValue;
                            }
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    object[] values = new object[columnIndices.Length];
                    for (int c = 0; c < columnIndices.Length; c++)
                    {
                        var prop = msgType?.GetProperties()[columnIndices[c]];
                        var propValue = prop != null ? prop.GetValue(msg) : null;
                        values[c] = propValue ?? DBNull.Value;
                    }
                    dgv.Rows.Add(values);
                }
            }

            if (dgv.Rows.Count > 0 && scrollIndex < dgv.Rows.Count)
                dgv.FirstDisplayedScrollingRowIndex = scrollIndex;
        }

        // --- Slap Messages ---
        public static OperationResult AddSlapMessage(string messageText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(messageText))
                    return new OperationResult(false, "Message text cannot be empty.");

                if (messageText.Length > 500)
                    return new OperationResult(false, "Message text cannot exceed 500 characters.");

                if (instanceChat.SlapMessages.Any(m => m.SlapMessageText.Equals(messageText, StringComparison.OrdinalIgnoreCase)))
                    return new OperationResult(false, "This slap message already exists.");

                int newId = DatabaseManager.AddSlapMessage(messageText);
                instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();

                return new OperationResult(true, "Slap message added successfully.", newId);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding slap message", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        public static OperationResult RemoveSlapMessage(int id)
        {
            try
            {
                if (id <= 0)
                    return new OperationResult(false, "Invalid message ID.");

                var message = instanceChat.SlapMessages.FirstOrDefault(m => m.SlapMessageId == id);
                if (message == null)
                    return new OperationResult(false, $"Slap message with ID {id} not found.");

                bool removed = DatabaseManager.RemoveSlapMessage(id);

                if (removed)
                {
                    instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();
                    return new OperationResult(true, "Slap message removed successfully.", id);
                }
                
                return new OperationResult(false, "Failed to remove slap message from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error removing slap message", AppDebug.LogLevel.Error,ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        public static IReadOnlyList<SlapMessages> GetSlapMessages()
        {
            return instanceChat.SlapMessages.AsReadOnly();
        }

        // --- Auto Messages ---
        public static OperationResult AddAutoMessage(string messageText, int triggerSeconds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(messageText))
                    return new OperationResult(false, "Message text cannot be empty.");

                if (messageText.Length > 500)
                    return new OperationResult(false, "Message text cannot exceed 500 characters.");

                if (triggerSeconds < 0)
                    return new OperationResult(false, "Trigger seconds cannot be negative.");

                if (triggerSeconds > 86400)
                    return new OperationResult(false, "Trigger seconds cannot exceed 24 hours (86400 seconds).");

                int newId = DatabaseManager.AddAutoMessage(messageText, triggerSeconds);
                instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();

                return new OperationResult(true, "Auto message added successfully.", newId);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error adding auto message", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        public static OperationResult RemoveAutoMessage(int id)
        {
            try
            {
                if (id <= 0)
                    return new OperationResult(false, "Invalid message ID.");

                var message = instanceChat.AutoMessages.FirstOrDefault(m => m.AutoMessageId == id);
                if (message == null)
                    return new OperationResult(false, $"Auto message with ID {id} not found.");

                bool removed = DatabaseManager.RemoveAutoMessage(id);

                if (removed)
                {
                    instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();
                    return new OperationResult(true, "Auto message removed successfully.", id);
                }

                return new OperationResult(false, "Failed to remove auto message from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error removing auto message", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        public static IReadOnlyList<AutoMessages> GetAutoMessages()
        {
            return instanceChat.AutoMessages.AsReadOnly();
        }

        // --- Chat Message Sending ---
        public static OperationResult SendChatMessage(string message, int channel, int maxLength = 59)
        {
            try
            {
                var (canSend, statusError) = CanSendMessage();
                if (!canSend) return new OperationResult(false, statusError);

                if (string.IsNullOrWhiteSpace(message))
                    return new OperationResult(false, "Message cannot be empty.");

                var (parseSuccess, parsedMessage, parseError) = ParsePlayerSlotReplacements(message);
                if (!parseSuccess) return new OperationResult(false, parseError);

                // Queue the message instead of sending immediately
                lock (instanceChat.MessageQueue)
                {
                    instanceChat.MessageQueue.Enqueue(new QueuedChatMessage
                    {
                        Message = parsedMessage,
                        Channel = channel,
                        MaxLength = maxLength,
                        QueuedAt = DateTime.Now
                    });
                }

                return new OperationResult(true, "Message queued successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error queuing chat message", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        // --- Internal message sending (called by ticker) ---
        internal static void ProcessQueuedMessage(QueuedChatMessage queuedMessage)
        {
            string prefix = queuedMessage.Channel switch { 4 => "R~", 5 => "B~", _ => string.Empty };
            string messageWithPrefix = prefix + queuedMessage.Message;

            if (messageWithPrefix.Length <= queuedMessage.MaxLength)
            {
                ServerMemory.WriteMemorySendChatMessage(queuedMessage.Channel, messageWithPrefix);
            }
            else
            {
                SendLongMessage(queuedMessage.Message, queuedMessage.Channel, queuedMessage.MaxLength, prefix);
            }
        }

        private static void SendLongMessage(string message, int channel, int maxLength, string prefix)
        {
            message = message.Trim();
            int maxContentLength = maxLength - prefix.Length;
            int position = 0;
            int chunkNum = 1;

            while (position < message.Length)
            {
                int chunkLength = Math.Min(maxContentLength, message.Length - position);

                // Break at word boundary if not the last chunk
                if (chunkLength == maxContentLength && position + chunkLength < message.Length)
                {
                    int lastSpace = message.LastIndexOf(' ', position + chunkLength - 1, chunkLength);
                    if (lastSpace > position && (lastSpace - position) >= (maxContentLength / 2))
                        chunkLength = lastSpace - position;
                }

                string chunk = message.Substring(position, chunkLength).TrimEnd();
                ServerMemory.WriteMemorySendChatMessage(channel, prefix + chunk);

                position += chunkLength;
                while (position < message.Length && message[position] == ' ') position++;

                if (position < message.Length)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        public static int MapChannelIndexToChannel(int selectedIndex)
        {
            return selectedIndex switch
            {
                1 => 3,
                2 => 4,
                3 => 5,
				0 => 1,
                _ => 1
            };
        }

        // --- Helper Methods ---
        private static (bool canSend, string errorMessage) CanSendMessage()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return (false, "Cannot send message: Server is offline");

            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                return (false, "Cannot send message: Server is loading map");

            if (theInstance.instanceStatus == InstanceStatus.SCORING)
                return (false, "Cannot send message: Server is in scoring phase");

            return (true, string.Empty);
        }

        private static (bool success, string message, string errorMessage) ParsePlayerSlotReplacements(string message)
        {
            try
            {
                if (!message.Contains("{P:") || !message.Contains("}"))
                    return (true, message, string.Empty);

                string parsedMessage = message;
                int startIndex = message.IndexOf("{P:");

                while (startIndex != -1)
                {
                    int endIndex = message.IndexOf("}", startIndex);
                    if (endIndex <= startIndex)
                        break;

                    string playerSlotText = message.Substring(startIndex + 3, endIndex - startIndex - 3);

                    if (!int.TryParse(playerSlotText, out int playerSlot))
                    {
                        return (false, message, $"Invalid player slot format: {playerSlotText}");
                    }

                    var playerEntry = playerInstance.PlayerList.FirstOrDefault(p => p.Value.PlayerSlot == playerSlot);

                    if (playerEntry.Value == null)
                    {
                        return (false, message, $"No player found in PlayerSlot {playerSlot}");
                    }

                    string playerName = playerEntry.Value.PlayerName ?? string.Empty;
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        playerName = Functions.SanitizePlayerName(playerName);
                        parsedMessage = parsedMessage.Replace($"{{P:{playerSlot}}}", playerName);
                    }

                    startIndex = parsedMessage.IndexOf("{P:", endIndex);
                }

                return (true, parsedMessage, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error parsing player slot replacements", AppDebug.LogLevel.Error, ex);
                return (false, message, ex.Message);
            }
        }

        public static string GetTeamDisplayName(int messageType, int messageType2)
        {
            return messageType switch
            {
                0 => "Server",
                1 => "Global",
                2 => messageType2 switch
                {
                    1 => "Blue",
                    2 => "Red",
                    3 => "Yellow",
                    4 => "Violet",
                    _ => "Unknown Team"
                },
                _ => "Unknown"
            };
        }
        public static OperationResult SaveChatLogEntry(ChatLogObject chatLog)
        {
            try
            {
                if (chatLog == null)
                    return new OperationResult(false, "Chat log entry cannot be null.");

                DatabaseManager.SaveChatLog(chatLog);
                
                return new OperationResult(true, "Chat log entry saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error saving chat log entry", AppDebug.LogLevel.Error, ex);
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }
    }
}