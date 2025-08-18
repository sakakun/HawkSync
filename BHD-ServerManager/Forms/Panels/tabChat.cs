using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabChat : UserControl
    {
        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private chatInstance instanceChat => CommonCore.instanceChat;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "ChatTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.
        private DateTime _lastGridUpdate;                           // Last time the chat messages grid was updated.
        private bool _chatGridFirstLoad;                            // Flag to determine if this is the first load of the chat grid.

        public tabChat()
        {
            InitializeComponent();
        }

        public void fuctionEvent_UpdateSlapMessages()
        {

            dg_slapMessages.Rows.Clear();
            foreach (var slapMsg in chatInstanceManagers.GetSlapMessages())
                dg_slapMessages.Rows.Add(slapMsg.SlapMessageId, slapMsg.SlapMessageText);

        }
        public void functionEvent_UpdateAutoMessages()
        {

            dg_autoMessages.Rows.Clear();
            foreach (var autoMsg in chatInstanceManagers.GetAutoMessages())
                dg_autoMessages.Rows.Add(autoMsg.AutoMessageId, autoMsg.AutoMessageTigger, autoMsg.AutoMessageText);

        }
        public void functionEvent_UpdateChatMessagesGrid()
        {
            // Only allow update if at least 2 seconds have passed
            if ((DateTime.UtcNow - _lastGridUpdate).TotalSeconds < 2)
                return;

            _lastGridUpdate = DateTime.UtcNow;

            var dgv = dataGridView_chatMessages;

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

        public void ChatTickerHook()
        {
            // Ensure the first load is complete before proceeding with updates.
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                return;
            }
            // Update chat UI elements here, e.g., refresh chat messages, update counters, etc.
            functionEvent_UpdateAutoMessages();
            fuctionEvent_UpdateSlapMessages();

            if (theInstance.instanceStatus == InstanceStatus.ONLINE || theInstance.instanceStatus == InstanceStatus.STARTDELAY)
            {
                // Update chat messages grid
                functionEvent_UpdateChatMessagesGrid();
            }

        }

        private void actionKeyPress_slapAddMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                chatInstanceManagers.AddSlapMessage(tb_slapMessage.Text);
                tb_slapMessage.Clear();
                fuctionEvent_UpdateSlapMessages();
            }
        }
        private void actionClick_RemoveSlap(object sender, DataGridViewCellEventArgs e)
        {
            int slapMessageId = Convert.ToInt32(dg_slapMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveSlapMessage(slapMessageId);
            fuctionEvent_UpdateSlapMessages();
        }
        private void actionKeyPressed_AddAutoMessage(object sender, KeyPressEventArgs e)
        {
            chatInstanceManagers.AddAutoMessage(tb_autoMessage.Text.Trim(), (int)num_AutoMessageTrigger.Value);
            functionEvent_UpdateAutoMessages();
            tb_autoMessage.Text = string.Empty;
            num_AutoMessageTrigger.Value = 0;
        }
        private void actionClick_RemoveAutoMessage(object sender, DataGridViewCellEventArgs e)
        {
            int AutoMessageId = Convert.ToInt32(dg_autoMessages.Rows[e.RowIndex].Cells[0].Value);
            chatInstanceManagers.RemoveAutoMessage(AutoMessageId);
            functionEvent_UpdateAutoMessages();
        }
        private void actionKeyPress_SubmitMessage(object sender, KeyPressEventArgs e)
        {
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE ||
                theInstance.instanceStatus == InstanceStatus.LOADINGMAP ||
                theInstance.instanceStatus == InstanceStatus.SCORING)
            {
                tb_chatMessage.Enabled = false;
                return;
            } else
            {
                tb_chatMessage.Enabled = true;
            }

            string message = tb_chatMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            int channel = 0;

            switch (comboBox_chatGroup.SelectedIndex)
            {
                
                case 1: channel = 1; break;
                case 2: channel = 2; break;
                case 3: channel = 3; break;
                case 0: channel = 0; break;
                default: channel = 0; break;

            }

            if (message.Contains("{P:") && message.Contains("}"))
            {
                int startIndex = message.IndexOf("{P:") + 3;
                int endIndex = message.IndexOf("}", startIndex);
                if (endIndex > startIndex)
                {
                    string playerSlot = message.Substring(startIndex, endIndex - startIndex);
                    var playerEntry = theInstance.playerList.FirstOrDefault(p => p.Value.PlayerSlot.ToString() == playerSlot);
                    if (playerEntry.Value == null)
                    {
                        MessageBox.Show($"No player found in PlayerSlot {playerSlot}. Message will not be sent.", "Player Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string playerName = playerEntry.Value.PlayerName ?? string.Empty;
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        playerName = Functions.SanitizePlayerName(playerName);
                        message = message.Replace($"{{P:{playerSlot}}}", playerName);
                    }
                }
            }

            if (message.Length > 59)
            {
                for (int i = 0; i < message.Length; i += 59)
                {
                    string chunk = message.Substring(i, Math.Min(59, message.Length - i));
                    GameManager.WriteMemorySendChatMessage(channel, chunk);
                }
            }
            else
            {
                GameManager.WriteMemorySendChatMessage(channel, message);
            }

            tb_chatMessage.Clear();
        }
    }
}
