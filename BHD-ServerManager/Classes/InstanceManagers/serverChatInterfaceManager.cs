using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public class serverChatInterfaceManager : chatInstanceInterface
    {

        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static chatInstance instanceChat = CommonCore.instanceChat!;

        private static bool _chatGridFirstLoad = true;
        private static DateTime _lastGridUpdate = DateTime.MinValue;

        // Function: loadChatSettings, loads the chat settings from a JSON file. If it does not exist, it initializes empty lists and saves them.
        public void LoadSettings()
        {
            // Load the chat settings from the JSON file
            // If the file does not exist, it will create an empty list and save it.
            string chatSettingsPath = Path.Combine(CommonCore.AppDataPath, "ChatSettings.json");
            if (!File.Exists(chatSettingsPath))
            {
                instanceChat.SlapMessages = new List<SlapMessages>();
                instanceChat.AutoMessages = new List<AutoMessages>();
                chatInstanceManagers.SaveSettings();
                return;
            }
            try
            {
                var json = File.ReadAllText(chatSettingsPath);
                var settings = JsonSerializer.Deserialize<ChatSettingsObject>(json);
                instanceChat.SlapMessages = settings?.SlapMessages ?? new List<SlapMessages>();
                instanceChat.AutoMessages = settings?.AutoMessages ?? new List<AutoMessages>();
            }
            catch (IOException ex)
            {
                AppDebug.Log("ChatManager", $"Error reading chat settings file: {ex.Message}");
            }
        }

        // Function: saveChatSettings, saves the current chat settings to a JSON file.
        public void SaveSettings()
        {
            string chatSettingsPath = Path.Combine(CommonCore.AppDataPath, "ChatSettings.json");
            var chatSettings = new ChatSettingsObject
            {
                SlapMessages = instanceChat.SlapMessages,
                AutoMessages = instanceChat.AutoMessages
            };
            try
            {
                var json = JsonSerializer.Serialize(chatSettings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(chatSettingsPath, json);
            }
            catch (IOException ex)
            {
                AppDebug.Log("ChatManager", $"Error writing chat settings file: {ex.Message}");
            }
        }
        // Function: AddSlapMessage, adds a new slap message to the list, sorts the list, updates IDs, and saves the settings.
        public void AddSlapMessage(string messageText)
        {
            var newSlapMessage = new SlapMessages
            {
                SlapMessageText = messageText
            };

            instanceChat.SlapMessages.Add(newSlapMessage);

            instanceChat.SlapMessages = instanceChat.SlapMessages
                .OrderBy(sm => sm.SlapMessageText, StringComparer.OrdinalIgnoreCase)
                .ToList();

            for (int i = 0; i < instanceChat.SlapMessages.Count; i++)
            {
                instanceChat.SlapMessages[i].SlapMessageId = i + 1;
            }

            chatInstanceManagers.SaveSettings();
        }
        public void RemoveSlapMessage(int id)
        {
            var slapMessage = instanceChat.SlapMessages.FirstOrDefault(sm => sm.SlapMessageId == id);
            if (slapMessage != null)
            {
                instanceChat.SlapMessages.Remove(slapMessage);

                for (int i = 0; i < instanceChat.SlapMessages.Count; i++)
                {
                    instanceChat.SlapMessages[i].SlapMessageId = i + 1;
                }
                chatInstanceManagers.SaveSettings();
            }
        }
        public void AddAutoMessage(string messageText, int tiggerSeconds)
        {
            var newAutoMessage = new AutoMessages
            {
                AutoMessageText = messageText,
                AutoMessageTigger = tiggerSeconds
            };

            instanceChat.AutoMessages.Add(newAutoMessage);

            instanceChat.AutoMessages = instanceChat.AutoMessages
                .OrderBy(am => am.AutoMessageTigger)
                .ToList();

            for (int i = 0; i < instanceChat.AutoMessages.Count; i++)
            {
                instanceChat.AutoMessages[i].AutoMessageId = i + 1;
            }

            chatInstanceManagers.SaveSettings();
        }
        public void RemoveAutoMessage(int id)
        {
            var autoMessage = instanceChat.AutoMessages.FirstOrDefault(am => am.AutoMessageId == id);
            if (autoMessage != null)
            {
                instanceChat.AutoMessages.Remove(autoMessage);

                for (int i = 0; i < instanceChat.AutoMessages.Count; i++)
                {
                    instanceChat.AutoMessages[i].AutoMessageId = i + 1;
                }
                chatInstanceManagers.SaveSettings();
            }
        }

        public IReadOnlyList<SlapMessages> GetSlapMessages()
        {
            return instanceChat.SlapMessages.AsReadOnly();
        }
        public IReadOnlyList<AutoMessages> GetAutoMessages()
        {
            return instanceChat.AutoMessages.AsReadOnly();
        }

        public void UpdateChatMessagesGrid()
        {
            // Only allow update if at least 2 seconds have passed
            if ((DateTime.UtcNow - _lastGridUpdate).TotalSeconds < 2)
                return;

            _lastGridUpdate = DateTime.UtcNow;

            var dgv = thisServer.dataGridView_chatMessages;

            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action(UpdateChatMessagesGrid));
                return;
            }

            // Save scroll position
            int firstDisplayedRow = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;
            int visibleRows = dgv.DisplayedRowCount(false);
            bool wasAtBottom = (firstDisplayedRow + visibleRows) >= dgv.Rows.Count;

            // Clear and repopulate
            dgv.Rows.Clear();
            foreach (var entry in CommonCore.instanceChat!.ChatLog)
            {
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

                entry.PlayerName = Functions.SanitizePlayerName(entry.PlayerName);

                dgv.Rows.Add(
                    entry.MessageTimeStamp.ToString("HH:mm:ss"),
                    teamString,
                    entry.PlayerName,
                    entry.MessageText
                );
            }

            // Restore scroll position safely
            if (dgv.Rows.Count > 0)
            {
                int targetRow;
                if (_chatGridFirstLoad)
                {
                    // Always scroll to bottom on first load
                    targetRow = dgv.Rows.Count - visibleRows;
                    if (targetRow < 0) targetRow = 0;
                    _chatGridFirstLoad = false;
                }
                else if (wasAtBottom)
                {
                    if (visibleRows >= dgv.Rows.Count)
                    {
                        targetRow = 0;
                    }
                    else
                    {
                        targetRow = dgv.Rows.Count - visibleRows;
                        if (targetRow < 0) targetRow = 0;
                    }
                }
                else
                {
                    targetRow = firstDisplayedRow;
                    if (targetRow >= dgv.Rows.Count) targetRow = dgv.Rows.Count - 1;
                    if (targetRow < 0) targetRow = 0;
                }

                // Only set scroll index if valid
                if (targetRow >= 0 && targetRow < dgv.Rows.Count)
                {
                    dgv.FirstDisplayedScrollingRowIndex = targetRow;
                }
            }
        }

    }
}
