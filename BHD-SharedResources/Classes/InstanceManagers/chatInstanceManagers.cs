using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using System.Collections.Generic;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class chatInstanceManagers
    {
        public static chatInstanceInterface Implementation { get; set; }
        public static void LoadSettings() => Implementation.LoadSettings();
        public static void SaveSettings() => Implementation.SaveSettings();
        public static void AddSlapMessage(string messageText) => Implementation.AddSlapMessage(messageText);
        public static void RemoveSlapMessage(int id) => Implementation.RemoveSlapMessage(id);
        public static void AddAutoMessage(string messageText, int tiggerSeconds) => Implementation.AddAutoMessage(messageText, tiggerSeconds);
        public static void RemoveAutoMessage(int id) => Implementation.RemoveAutoMessage(id);
        public static IReadOnlyList<SlapMessages> GetSlapMessages() => Implementation.GetSlapMessages();
        public static IReadOnlyList<AutoMessages> GetAutoMessages() => Implementation.GetAutoMessages();
        public static void UpdateChatMessagesGrid() => Implementation.UpdateChatMessagesGrid();
    }
}
