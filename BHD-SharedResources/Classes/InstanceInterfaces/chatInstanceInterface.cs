using BHD_SharedResources.Classes.Instances;
using System.Collections.Generic;

namespace BHD_SharedResources.Classes.InstanceInterfaces
{
    public interface chatInstanceInterface
    {
        void LoadSettings();
        void SaveSettings();
        void AddSlapMessage(string messageText);
        void RemoveSlapMessage(int id);
        void AddAutoMessage(string messageText, int tiggerSeconds);
        void RemoveAutoMessage(int id);
        IReadOnlyList<SlapMessages> GetSlapMessages();
        IReadOnlyList<AutoMessages> GetAutoMessages();
        void UpdateChatMessagesGrid();
        void SendMessageToQueue(bool consoleMsg, int messageType, string messageText);
    }
}
