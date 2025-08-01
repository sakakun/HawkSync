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
