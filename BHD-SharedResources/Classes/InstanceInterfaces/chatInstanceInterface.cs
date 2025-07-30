using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;

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
    }
}
