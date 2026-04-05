using System.Text.Json.Serialization;

namespace HawkSyncShared.Instances
{
    public class chatInstance
    {
        public List<SlapMessages> SlapMessages { get; set; } = new List<SlapMessages>();
        public List<AutoMessages> AutoMessages { get; set; } = new List<AutoMessages>();
        public List<ChatLogObject> ChatLog { get; set; } = new List<ChatLogObject>();
        [JsonIgnore]
        public int AutoMessageCounter { get; set; }
        [JsonIgnore]
        public DateTime lastAutoMessageSent { get; set; } = DateTime.MinValue;
        [JsonIgnore]
        public Queue<QueuedChatMessage> MessageQueue { get; set; } = new Queue<QueuedChatMessage>();
        [JsonIgnore]
        public DateTime LastMessageSent { get; set; } = DateTime.MinValue;
        [JsonIgnore]
        public bool IsProcessingMessage { get; set; }

    }

    public record QueuedChatMessage
    {
        public required string Message;
        public int Channel;
        public int MaxLength = 59;
        public DateTime QueuedAt = DateTime.Now;
    }

    public class ChatLogObject
    {
        public DateTime         MessageTimeStamp        { get; set; }
        public string           PlayerName              { get; set; } = string.Empty;
        public int              MessageType             { get; set; } // 0 = server, 1 = global, 2 = PlayerTeam, 3 = other
        public int              MessageType2            { get; set; } // 0 = self, 1 = blue, 2 = red, 3 = host
        public string           MessageText             { get; set; } = string.Empty;
        public string           TeamDisplay             { get; set; } = string.Empty;
    }

    public class SlapMessages
    {
        public int SlapMessageId { get; set; }
        public string SlapMessageText { get; set; } = string.Empty;
    }

    public class AutoMessages
    {
        public int AutoMessageId { get; set; }
        public string AutoMessageText { get; set; } = string.Empty;
        public int AutoMessageTigger { get; set; }
    }

    public class ChatSettingsObject
    {
        public List<SlapMessages> SlapMessages { get; set; } = new List<SlapMessages>();
        public List<AutoMessages> AutoMessages { get; set; } = new List<AutoMessages>();
    }
}
