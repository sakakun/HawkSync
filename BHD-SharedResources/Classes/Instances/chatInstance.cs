using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BHD_SharedResources.Classes.Instances
{
    public class chatInstance
    {
        public List<SlapMessages> SlapMessages { get; set; } = new List<SlapMessages>();
        public List<AutoMessages> AutoMessages { get; set; } = new List<AutoMessages>();
        public List<ChatLogObject> ChatLog { get; set; } = new List<ChatLogObject>();

        public Dictionary<DateTime, ChatQueueObject> MessageQueue = new();
        [JsonIgnore]
        public int AutoMessageCounter { get; set; } = 0;
        [JsonIgnore]
        public int lastChatLogIndex { get; set; } = 0;
        [JsonIgnore]
        public DateTime lastAutoMessageSent { get; set; } = DateTime.MinValue;

    }

    public class ChatQueueObject
    {
        public bool ConsoleMsg { get; set; } // false = no, true = yes
        public int MessageType { get; set; } // 0 Global, 1 Yellow, 2 Red, 3 Blue
        public string MessageText { get; set; } = string.Empty;
    }

    public class ChatLogObject
    {
        public DateTime MessageTimeStamp { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int MessageType { get; set; } // 0 = server, 1 = global, 2 = PlayerTeam, 3 = other
        public int MessageType2 { get; set; } // 0 = self, 1 = blue, 2 = red, 3 = host
        public string MessageText { get; set; } = string.Empty;
    }

    public class SlapMessages
    {
        public int SlapMessageId { get; set; } = 0;
        public string SlapMessageText { get; set; } = string.Empty;
    }

    public class AutoMessages
    {
        public int AutoMessageId { get; set; } = 0;
        public string AutoMessageText { get; set; } = string.Empty;
        public int AutoMessageTigger { get; set; } = 0;
    }

    public class ChatSettingsObject
    {
        public List<SlapMessages> SlapMessages { get; set; } = new List<SlapMessages>();
        public List<AutoMessages> AutoMessages { get; set; } = new List<AutoMessages>();
    }
}
