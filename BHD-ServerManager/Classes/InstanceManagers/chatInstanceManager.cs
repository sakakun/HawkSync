using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    // ================================================================================
    // DTOs (Data Transfer Objects)
    // ================================================================================

    /// <summary>
    /// Chat log entry for display
    /// </summary>
    public record ChatLogEntry(
        DateTime Timestamp,
        int MessageType,
        int MessageType2,
        string PlayerName,
        string MessageText,
        string TeamDisplay
    );

    /// <summary>
    /// Chat log query parameters
    /// </summary>
    public record ChatLogQuery(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        int MaxResults = 1000
    );

    // ================================================================================
    // Chat Instance Manager - Business Logic Layer
    // ================================================================================

    public static class chatInstanceManager
    {
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static theInstance theInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

		// ================================================================================
		// SETTINGS MANAGEMENT
		// ================================================================================

		/// <summary>
		/// Load chat settings from database
		/// </summary>
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

        // ================================================================================
        // SLAP MESSAGES
        // ================================================================================

        /// <summary>
        /// Add a new slap message
        /// </summary>
        public static OperationResult AddSlapMessage(string messageText)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(messageText))
                    return new OperationResult(false, "Message text cannot be empty.");

                if (messageText.Length > 500)
                    return new OperationResult(false, "Message text cannot exceed 500 characters.");

                // Check for duplicates
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

        /// <summary>
        /// Remove a slap message by ID
        /// </summary>
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

        /// <summary>
        /// Get a slap message by ID
        /// </summary>
        public static (bool success, SlapMessages? message, string errorMessage) GetSlapMessageById(int id)
        {
            try
            {
                if (id <= 0)
                    return (false, null, "Invalid message ID.");

                var message = instanceChat.SlapMessages.FirstOrDefault(m => m.SlapMessageId == id);
                
                if (message == null)
                    return (false, null, $"Slap message with ID {id} not found.");

                return (true, message, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error getting slap message: {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Get all slap messages (read-only)
        /// </summary>
        public static IReadOnlyList<SlapMessages> GetSlapMessages()
        {
            return instanceChat.SlapMessages.AsReadOnly();
        }

        /// <summary>
        /// Get a random slap message
        /// </summary>
        public static (bool success, string? message, string errorMessage) GetRandomSlapMessage()
        {
            try
            {
                if (instanceChat.SlapMessages.Count == 0)
                    return (false, null, "No slap messages available.");

                var random = new Random();
                int index = random.Next(instanceChat.SlapMessages.Count);
                var message = instanceChat.SlapMessages[index].SlapMessageText;

                return (true, message, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error getting random slap message: {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        // ================================================================================
        // AUTO MESSAGES
        // ================================================================================

        /// <summary>
        /// Add a new auto message
        /// </summary>
        public static OperationResult AddAutoMessage(string messageText, int triggerSeconds)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(messageText))
                    return new OperationResult(false, "Message text cannot be empty.");

                if (messageText.Length > 500)
                    return new OperationResult(false, "Message text cannot exceed 500 characters.");

                if (triggerSeconds < 0)
                    return new OperationResult(false, "Trigger seconds cannot be negative.");

                if (triggerSeconds > 86400) // 24 hours
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

        /// <summary>
        /// Remove an auto message by ID
        /// </summary>
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

        /// <summary>
        /// Get an auto message by ID
        /// </summary>
        public static (bool success, AutoMessages? message, string errorMessage) GetAutoMessageById(int id)
        {
            try
            {
                if (id <= 0)
                    return (false, null, "Invalid message ID.");

                var message = instanceChat.AutoMessages.FirstOrDefault(m => m.AutoMessageId == id);
                
                if (message == null)
                    return (false, null, $"Auto message with ID {id} not found.");

                return (true, message, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error getting auto message: {ex.Message}");
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Get all auto messages (read-only)
        /// </summary>
        public static IReadOnlyList<AutoMessages> GetAutoMessages()
        {
            return instanceChat.AutoMessages.AsReadOnly();
        }

        // ================================================================================
        // CHAT LOG OPERATIONS
        // ================================================================================

        /// <summary>
        /// Save a chat log entry to the database
        /// </summary>
        public static OperationResult SaveChatLogEntry(ChatLogObject chatLog)
        {
            try
            {
                if (chatLog == null)
                    return new OperationResult(false, "Chat log entry cannot be null.");

                DatabaseManager.SaveChatLog(chatLog);
                
                AppDebug.Log("chatInstanceManager", $"Saved chat log entry: {chatLog.PlayerName}: {chatLog.MessageText}");
                return new OperationResult(true, "Chat log entry saved successfully.");
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error saving chat log entry: {ex.Message}");
                return new OperationResult(false, $"Error: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Get all chat log entries (in-memory) for display
        /// </summary>
        public static List<ChatLogEntry> GetChatLogs()
        {
            try
            {
                var entries = new List<ChatLogEntry>();

                foreach (var entry in instanceChat.ChatLog)
                {
                    string teamString = GetTeamDisplayName(entry.MessageType, entry.MessageType2);
                    string sanitizedName = Functions.SanitizePlayerName(entry.PlayerName);

                    entries.Add(new ChatLogEntry(
                        Timestamp: entry.MessageTimeStamp,
                        MessageType: entry.MessageType,
                        MessageType2: entry.MessageType2,
                        PlayerName: sanitizedName,
                        MessageText: entry.MessageText,
                        TeamDisplay: teamString
                    ));
                }

                return entries;
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error getting chat logs: {ex.Message}");
                return new List<ChatLogEntry>();
            }
        }

        /// <summary>
        /// Get chat logs from database with query parameters
        /// </summary>
        public static (bool success, List<ChatLogObject> logs, string errorMessage) GetChatLogsFromDatabase(ChatLogQuery query)
        {
            try
            {
                var logs = DatabaseManager.GetChatLogs(
                    query.StartDate,
                    query.EndDate,
                    query.MaxResults
                );

                return (true, logs, string.Empty);
            }
            catch (Exception ex)
            {
                AppDebug.Log("chatInstanceManager", $"Error getting chat logs from database: {ex.Message}");
                return (false, new List<ChatLogObject>(), ex.Message);
            }
        }

        /// <summary>
        /// Clear in-memory chat log
        /// </summary>
        public static void ClearChatLog()
        {
            instanceChat.ChatLog.Clear();
            AppDebug.Log("chatInstanceManager", "Cleared in-memory chat log");
        }

        // ================================================================================
        // MESSAGE SENDING
        // ================================================================================

        /// <summary>
        /// Validate if a message can be sent based on instance status
        /// </summary>
        public static (bool canSend, string errorMessage) CanSendMessage()
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
                return (false, "Cannot send message: Server is offline");

            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
                return (false, "Cannot send message: Server is loading map");

            if (theInstance.instanceStatus == InstanceStatus.SCORING)
                return (false, "Cannot send message: Server is in scoring phase");

            return (true, string.Empty);
        }

        /// <summary>
        /// Parse and replace player slot placeholders in message
        /// </summary>
        public static (bool success, string message, string errorMessage) ParsePlayerSlotReplacements(string message)
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

        /// <summary>
        /// Send a chat message (handles long messages by chunking)
        /// </summary>
        public static OperationResult SendChatMessage(string message, int channel, int maxLength = 59)
        {
            try
            {
                // Validate instance status
                var (canSend, statusError) = CanSendMessage();
                if (!canSend)
                    return new OperationResult(false, statusError);

                // Validate parameters
                if (string.IsNullOrWhiteSpace(message))
                    return new OperationResult(false, "Message cannot be empty.");

                if (channel < 0 || channel > 3)
                    return new OperationResult(false, "Invalid channel. Must be 0-3 (Server, Global, Blue, Red).");

                // Parse player slot replacements
                var (parseSuccess, parsedMessage, parseError) = ParsePlayerSlotReplacements(message);
                if (!parseSuccess)
                    return new OperationResult(false, parseError);

                // Send message (chunked if necessary)
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

        /// <summary>
        /// Send a long message by breaking it into chunks
        /// </summary>
        private static void SendLongMessage(string message, int channel, int maxLength)
        {
            for (int i = 0; i < message.Length; i += maxLength)
            {
                int remainingLength = message.Length - i;
                int chunkLength = Math.Min(maxLength, remainingLength);

                // Try to find a space to break at (word boundary)
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
                
                Thread.Sleep(1000); // Delay between chunks
            }
        }

        // ================================================================================
        // HELPER METHODS
        // ================================================================================

        /// <summary>
        /// Get team display name from message types
        /// </summary>
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
                    _ => "Unknown Team"
                },
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Map UI channel dropdown index to actual channel number
        /// </summary>
        public static int MapChannelIndexToChannel(int selectedIndex)
        {
            return selectedIndex switch
            {
                1 => 1, // Global
                2 => 2, // Blue
                3 => 3, // Red
                0 => 0, // Server
                _ => 0  // Default to server
            };
        }
    }
}