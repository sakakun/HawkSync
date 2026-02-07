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

                AppDebug.Log("chatInstanceManager",
                    $"Loaded {instanceChat.SlapMessages.Count} slap messages and {instanceChat.AutoMessages.Count} auto messages");

                return new OperationResult(true, "Settings loaded successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error loading chat settings: {ex.Message}");

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

            var managerDict = messages.ToDictionary(m => dgv.Rows.Count > 0 ? dgv.Rows[0].Cells[0].Value : 0);

            // Remove rows not in manager
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                if (!messages.Any(m => Convert.ToInt32(m.GetType().GetProperty("SlapMessageId")?.GetValue(m) ?? m.GetType().GetProperty("AutoMessageId")?.GetValue(m)) == id))
                    dgv.Rows.RemoveAt(i);
            }

            // Update existing rows and add new ones
            foreach (var msg in messages)
            {
                int id = Convert.ToInt32(msg.GetType().GetProperty("SlapMessageId")?.GetValue(msg) ?? msg.GetType().GetProperty("AutoMessageId")?.GetValue(msg));
                bool found = false;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    int rowId = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                    if (rowId == id)
                    {
                        for (int c = 1; c < columnIndices.Length; c++)
                        {
                            var prop = msg.GetType().GetProperties()[columnIndices[c]];
                            if (!Equals(dgv.Rows[i].Cells[c].Value, prop.GetValue(msg)))
                                dgv.Rows[i].Cells[c].Value = prop.GetValue(msg);
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
                        values[c] = msg.GetType().GetProperties()[columnIndices[c]].GetValue(msg);
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

                AppDebug.Log("chatInstanceManager", $"Added slap message: {messageText} (ID: {newId})");
                return new OperationResult(true, "Slap message added successfully.", newId);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error adding slap message: {ex.Message}");
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
                    AppDebug.Log("chatInstanceManager", $"Removed slap message ID: {id}");
                    return new OperationResult(true, "Slap message removed successfully.", id);
                }

                return new OperationResult(false, "Failed to remove slap message from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error removing slap message: {ex.Message}");
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

                AppDebug.Log("chatInstanceManager", $"Added auto message: {messageText} (Trigger: {triggerSeconds}s, ID: {newId})");
                return new OperationResult(true, "Auto message added successfully.", newId);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error adding auto message: {ex.Message}");
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
                    AppDebug.Log("chatInstanceManager", $"Removed auto message ID: {id}");
                    return new OperationResult(true, "Auto message removed successfully.", id);
                }

                return new OperationResult(false, "Failed to remove auto message from database.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error removing auto message: {ex.Message}");
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
                if (!canSend)
                    return new OperationResult(false, statusError);

                if (string.IsNullOrWhiteSpace(message))
                    return new OperationResult(false, "Message cannot be empty.");

                if (channel < 0 || channel > 3)
                    return new OperationResult(false, "Invalid channel. Must be 0-3 (Server, Global, Blue, Red).");

                var (parseSuccess, parsedMessage, parseError) = ParsePlayerSlotReplacements(message);
                if (!parseSuccess)
                    return new OperationResult(false, parseError);

                if (parsedMessage.Length <= maxLength)
                {
                    ServerMemory.WriteMemorySendChatMessage(channel, parsedMessage);
                    AppDebug.Log("chatInstanceManager", $"Sent message to channel {channel}: {parsedMessage}");
                }
                else
                {
                    SendLongMessage(parsedMessage, channel, maxLength);
                }

                return new OperationResult(true, "Message sent successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error sending chat message: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        public static int MapChannelIndexToChannel(int selectedIndex)
        {
            return selectedIndex switch
            {
                1 => 1,
                2 => 2,
                3 => 3,
                0 => 0,
                _ => 0
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
                AppDebug.Log("chatInstanceManager", $"Error parsing player slot replacements: {ex.Message}");
                return (false, message, ex.Message);
            }
        }

        private static void SendLongMessage(string message, int channel, int maxLength)
        {
            for (int i = 0; i < message.Length; i += maxLength)
            {
                int remainingLength = message.Length - i;
                int chunkLength = Math.Min(maxLength, remainingLength);

                if (chunkLength == maxLength && i + maxLength < message.Length)
                {
                    int lastSpace = message.LastIndexOf(' ', i + maxLength, maxLength);
                    if (lastSpace > i)
                    {
                        chunkLength = lastSpace - i;
                    }
                }

                string chunk = message.Substring(i, chunkLength).Trim();
                AppDebug.Log("chatInstanceManager", $"Sending chunk to channel {channel}: {chunk}");
                ServerMemory.WriteMemorySendChatMessage(channel, chunk);

                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}