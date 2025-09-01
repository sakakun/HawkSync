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

        private static chatInstance instanceChat = CommonCore.instanceChat!;

        private static DateTime _lastGridUpdate = DateTime.MinValue;
        private static int _nextMessageQueueId = 1;

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
            // To Do: Remove
        }
        public void SendMessageToQueue(bool consoleMsg, int messageType, string messageText)
        {
            var chatQueueObject = new ChatQueueObject
            {
                ConsoleMsg = consoleMsg,
                MessageType = messageType,
                MessageText = messageText
            };
            instanceChat.MessageQueue[_nextMessageQueueId++] = chatQueueObject;
        }
    }
}
