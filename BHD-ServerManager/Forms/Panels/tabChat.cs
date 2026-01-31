using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
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
        private theInstance? theInstance => CommonCore.theInstance;
        private chatInstance? chatInstance => CommonCore.instanceChat;

        // --- Class Variables ---
        private bool _firstLoadComplete = false;
        private DateTime _lastGridUpdate;
        private bool _chatGridFirstLoad = true;

        public tabChat()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update slap messages grid from manager
        /// </summary>
        public void fuctionEvent_UpdateSlapMessages()
        {
            dg_slapMessages.Rows.Clear();
            
            foreach (var slapMsg in chatInstanceManager.GetSlapMessages())
            {
                dg_slapMessages.Rows.Add(slapMsg.SlapMessageId, slapMsg.SlapMessageText);
            }
        }

        /// <summary>
        /// Update auto messages grid from manager
        /// </summary>
        public void functionEvent_UpdateAutoMessages()
        {
            dg_autoMessages.Rows.Clear();
            
            foreach (var autoMsg in chatInstanceManager.GetAutoMessages())
            {
                dg_autoMessages.Rows.Add(autoMsg.AutoMessageId, autoMsg.AutoMessageTigger, autoMsg.AutoMessageText);
            }
        }

        /// <summary>
        /// Update chat messages grid from in-memory logs
        /// </summary>
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

            // Clear and repopulate from manager
            dgv.Rows.Clear();
            
            var chatLogs = chatInstance!.ChatLog;
            
            foreach (ChatLogObject entry in chatLogs)
            {
                DateTime TimeStamp = entry.MessageTimeStamp;

                dgv.Rows.Add(
                    TimeStamp.ToString("HH:mm:ss"),
                    entry.TeamDisplay,
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

        /// <summary>
        /// Ticker hook for periodic updates
        /// </summary>
        public void ChatTickerHook()
        {
            // Ensure the first load is complete before proceeding with updates
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                return;
            }

            // Update chat UI elements
            functionEvent_UpdateAutoMessages();
            fuctionEvent_UpdateSlapMessages();

            if (theInstance?.instanceStatus == InstanceStatus.ONLINE || 
                theInstance?.instanceStatus == InstanceStatus.STARTDELAY)
            {
                // Update chat messages grid
                functionEvent_UpdateChatMessagesGrid();
            }
        }

        /// <summary>
        /// Handle adding a slap message via Enter key
        /// </summary>
        private void actionKeyPress_slapAddMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string messageText = tb_slapMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(messageText))
                return;

            // Add via manager
            var result = chatInstanceManager.AddSlapMessage(messageText);
            
            if (result.Success)
            {
                tb_slapMessage.Clear();
                fuctionEvent_UpdateSlapMessages();
            }
            else
            {
                MessageBox.Show(result.Message, "Error Adding Slap Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handle removing a slap message via double-click
        /// </summary>
        private void actionClick_RemoveSlap(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int slapMessageId = Convert.ToInt32(dg_slapMessages.Rows[e.RowIndex].Cells[0].Value);
            
            var dialogResult = MessageBox.Show(
                "Are you sure you want to remove this slap message?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
                return;

            // Remove via manager
            var result = chatInstanceManager.RemoveSlapMessage(slapMessageId);
            
            if (result.Success)
            {
                fuctionEvent_UpdateSlapMessages();
            }
            else
            {
                MessageBox.Show(result.Message, "Error Removing Slap Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle adding an auto message via Enter key
        /// </summary>
        private void actionKeyPress_AddAutoMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string messageText = tb_autoMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(messageText))
                return;

            int triggerSeconds = (int)num_AutoMessageTrigger.Value;

            // Add via manager
            var result = chatInstanceManager.AddAutoMessage(messageText, triggerSeconds);
            
            if (result.Success)
            {
                tb_autoMessage.Clear();
                num_AutoMessageTrigger.Value = 0;
                functionEvent_UpdateAutoMessages();
            }
            else
            {
                MessageBox.Show(result.Message, "Error Adding Auto Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handle removing an auto message via double-click
        /// </summary>
        private void actionClick_RemoveAutoMessage(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int autoMessageId = Convert.ToInt32(dg_autoMessages.Rows[e.RowIndex].Cells[0].Value);
            
            var dialogResult = MessageBox.Show(
                "Are you sure you want to remove this auto message?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
                return;

            // Remove via manager
            var result = chatInstanceManager.RemoveAutoMessage(autoMessageId);
            
            if (result.Success)
            {
                functionEvent_UpdateAutoMessages();
            }
            else
            {
                MessageBox.Show(result.Message, "Error Removing Auto Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle submitting a chat message via Enter key
        /// </summary>
        private void actionKeyPress_SubmitMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string message = tb_chatMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(message))
                return;

            // Map UI channel dropdown to channel number via manager
            int channel = chatInstanceManager.MapChannelIndexToChannel(comboBox_chatGroup.SelectedIndex);

            // Send message via manager
            var result = chatInstanceManager.SendChatMessage(message, channel);
            
            if (result.Success)
            {
                tb_chatMessage.Clear();
            }
            else
            {
                // Show error to user
                MessageBox.Show(result.Message, "Cannot Send Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Clear textbox anyway if it was a validation error
                if (!result.Message.Contains("offline") && 
                    !result.Message.Contains("loading") && 
                    !result.Message.Contains("scoring"))
                {
                    tb_chatMessage.Clear();
                }
            }
        }
    }
}