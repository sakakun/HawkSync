using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteChatInstanceManager : chatInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static chatInstance chatInstance => CommonCore.instanceChat!;
        private static bool _chatGridFirstLoad = true;
        private static DateTime _lastGridUpdate = DateTime.MinValue;


        public void AddAutoMessage(string messageText, int tiggerSeconds) => CmdAddAutoMessage.ProcessCommand(messageText, tiggerSeconds);
        public void AddSlapMessage(string messageText) => CmdAddSlapMessage.ProcessCommand(messageText);

        public IReadOnlyList<SlapMessages> GetSlapMessages()
        {
            return chatInstance.SlapMessages.AsReadOnly();
        }
        public IReadOnlyList<AutoMessages> GetAutoMessages()
        {
            return chatInstance.AutoMessages.AsReadOnly();
        }

        public void LoadSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void RemoveAutoMessage(int id) => CmdRemoveAutoMessage.ProcessCommand(id);

        public void RemoveSlapMessage(int id) => CmdRemoveSlapMessage.ProcessCommand(id);

        public void SaveSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void UpdateChatMessagesGrid()
        {
            // To Do: Remove
        }

    }
}
