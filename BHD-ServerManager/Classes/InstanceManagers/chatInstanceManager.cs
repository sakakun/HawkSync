using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class chatInstanceManager
    {

        private static chatInstance instanceChat = CommonCore.instanceChat!;

        private static DateTime _lastGridUpdate = DateTime.MinValue;

        // Function: loadChatSettings, loads the chat settings from a JSON file. If it does not exist, it initializes empty lists and saves them.
        public static void LoadSettings()
        {
            try
            {
                instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();
                instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();

                AppDebug.Log("ChatManager", $"Loaded {instanceChat.SlapMessages.Count} slap messages and {instanceChat.AutoMessages.Count} auto messages from database");
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error loading chat settings from database: {ex.Message}");
                
                instanceChat.SlapMessages = new List<SlapMessages>();
                instanceChat.AutoMessages = new List<AutoMessages>();
            }
        }

        // Function: AddSlapMessage, adds a new slap message to the list, sorts the list, updates IDs, and saves the settings.
        public static void AddSlapMessage(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                AppDebug.Log("ChatManager", "Cannot add empty slap message");
                return;
            }

            try
            {
                DatabaseManager.AddSlapMessage(messageText);
                instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();
                AppDebug.Log("ChatManager", $"Added slap message: {messageText}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error adding slap message: {ex.Message}");
                throw;
            }
        }

        public static void RemoveSlapMessage(int id)
        {
            try
            {
                bool removed = DatabaseManager.RemoveSlapMessage(id);
                
                if (removed)
                {
                    instanceChat.SlapMessages = DatabaseManager.GetSlapMessages();
                    AppDebug.Log("ChatManager", $"Removed slap message ID: {id}");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error removing slap message: {ex.Message}");
                throw;
            }
        }

        public static void AddAutoMessage(string messageText, int triggerSeconds)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                AppDebug.Log("ChatManager", "Cannot add empty auto message");
                return;
            }

            try
            {
                DatabaseManager.AddAutoMessage(messageText, triggerSeconds);
                instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();
                AppDebug.Log("ChatManager", $"Added auto message: {messageText} (Trigger: {triggerSeconds}s)");
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error adding auto message: {ex.Message}");
                throw;
            }
        }

        public static void RemoveAutoMessage(int id)
        {
            try
            {
                bool removed = DatabaseManager.RemoveAutoMessage(id);
                
                if (removed)
                {
                    instanceChat.AutoMessages = DatabaseManager.GetAutoMessages();
                    AppDebug.Log("ChatManager", $"Removed auto message ID: {id}");
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error removing auto message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Save a chat log entry to the database.
        /// </summary>
        public static void SaveChatLogEntry(ChatLogObject chatLog)
        {
            try
            {
                DatabaseManager.SaveChatLog(chatLog);
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatManager", $"Error saving chat log entry: {ex.Message}");
            }
        }

        /// <summary>
        /// Get slap messages (read-only).
        /// </summary>
        public static IReadOnlyList<SlapMessages> GetSlapMessages()
        {
            return instanceChat.SlapMessages.AsReadOnly();
        }

        /// <summary>
        /// Get auto messages (read-only).
        /// </summary>
        public static IReadOnlyList<AutoMessages> GetAutoMessages()
        {
            return instanceChat.AutoMessages.AsReadOnly();
        }

        public static void UpdateChatMessagesGrid()
        {
            // No-op: Kept for backward compatibility
        }

    }
}
