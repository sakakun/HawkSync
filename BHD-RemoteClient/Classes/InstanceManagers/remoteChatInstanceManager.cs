using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteChatInstanceManager : chatInstanceInterface
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static chatInstance chatInstance => CommonCore.instanceChat!;

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

        public static void UpdateChatMessagesGrid()
        {
            if (thisServer.dataGridView_chatMessages.InvokeRequired)
            {
                thisServer.dataGridView_chatMessages.Invoke(new Action(() => remoteChatInstanceManager.UpdateChatMessagesGrid()));
                return;
            }

            thisServer.dataGridView_chatMessages.Rows.Clear();

            for (int i = 0; i < chatInstance.ChatLog.Count(); i++)
            {
                var entry = chatInstance.ChatLog[i];
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

                // Sanitize player name
                entry.PlayerName = Functions.SanitizePlayerName(entry.PlayerName);

                thisServer.dataGridView_chatMessages.Rows.Add(
                    entry.MessageTimeStamp.ToString("HH:mm:ss"),
                    teamString,
                    entry.PlayerName,
                    entry.MessageText
                );
            }


        }

    }
}
