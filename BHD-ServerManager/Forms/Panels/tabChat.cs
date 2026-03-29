using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabChat : UserControl
    {
        private theInstance? theInstance => CommonCore.theInstance;
        private chatInstance? chatInstance => CommonCore.instanceChat;

        private bool _firstLoadComplete = false;
        private DateTime _lastGridUpdate;
        private bool _autoScrollChat = true;

        public tabChat()
        {
            InitializeComponent();

            chatInstanceManager.LoadSettings();

            CommonCore.Ticker?.Start("ChatTabUpdate", 1000, ChatTickerHook);

            dataGridView_chatMessages.Scroll += dataGridView_chatMessages_Scroll;
        }

        /// <summary>
        /// Ticker hook for periodic updates
        /// </summary>
        public void ChatTickerHook()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ChatTickerHook));
                return;
            }

            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                return;
            }

            // Update Chat Message Grid
            functionEvent_UpdateChatMessagesGrid();
            // Update Auto Message Grid
            functionEvent_UpdateAutoMessages();
            // Update Slap Message Grid
            functionEvent_UpdateSlapMessages();

        }

        private void dataGridView_chatMessages_Scroll(object? sender, ScrollEventArgs e)
        {
            var dgv = dataGridView_chatMessages;
            int firstDisplayedRow = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;
            int visibleRows = dgv.DisplayedRowCount(false);
            bool atBottom = (firstDisplayedRow + visibleRows) >= dgv.Rows.Count;
            _autoScrollChat = atBottom;
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
            var chatLogs = chatInstance!.ChatLog;

            // If status is SCORING or LOADING, clear and reload everything
            if (theInstance?.instanceStatus == InstanceStatus.SCORING ||
                theInstance?.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                dgv.Rows.Clear();
                _autoScrollChat = true; // Reset auto-scroll on reload
                return;
            }

            // Only add new messages
            int existingRows = dgv.Rows.Count;
            for (int i = existingRows; i < chatLogs.Count; i++)
            {
                var entry = chatLogs[i];
                DateTime TimeStamp = entry.MessageTimeStamp;
                dgv.Rows.Add(
                    TimeStamp.ToString("HH:mm:ss"),
                    entry.TeamDisplay,
                    entry.PlayerName,
                    entry.MessageText
                );
            }

            // Auto-scroll to bottom if enabled
            if (_autoScrollChat && dgv.Rows.Count > 10)
            {
                int visibleRows = dgv.DisplayedRowCount(false);
                dgv.FirstDisplayedScrollingRowIndex = Math.Min(dgv.Rows.Count - 1, Math.Max(30, dgv.Rows.Count - visibleRows));
            }
        }

        /// <summary>
        /// Update slap messages grid from manager
        /// </summary>
        public void functionEvent_UpdateSlapMessages()
        {
            var slapMessages = CommonCore.instanceChat!.SlapMessages;
            var dgv = dg_slapMessages;

            // Preserve scroll position
            int scrollIndex = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;

            // Build lookup for fast access
            var managerDict = slapMessages.ToDictionary(x => x.SlapMessageId);

            // Remove rows not in manager
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                if (!managerDict.ContainsKey(id))
                    dgv.Rows.RemoveAt(i);
            }

            // Update existing rows and add new ones
            foreach (var msg in slapMessages)
            {
                bool found = false;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                    if (id == msg.SlapMessageId)
                    {
                        // Update if text changed
                        if (!Equals(dgv.Rows[i].Cells[1].Value, msg.SlapMessageText))
                            dgv.Rows[i].Cells[1].Value = msg.SlapMessageText;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dgv.Rows.Add(msg.SlapMessageId, msg.SlapMessageText);
                }
            }

            // Restore scroll position
            if (dgv.Rows.Count > 0 && scrollIndex < dgv.Rows.Count)
                dgv.FirstDisplayedScrollingRowIndex = scrollIndex;
        }

        public void functionEvent_UpdateAutoMessages()
        {
            var autoMessages = CommonCore.instanceChat!.AutoMessages;
            var dgv = dg_autoMessages;

            // Preserve scroll position
            int scrollIndex = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;

            var managerDict = autoMessages.ToDictionary(x => x.AutoMessageId);

            // Remove rows not in manager
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                if (!managerDict.ContainsKey(id))
                    dgv.Rows.RemoveAt(i);
            }

            // Update existing rows and add new ones
            foreach (var msg in autoMessages)
            {
                bool found = false;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                    if (id == msg.AutoMessageId)
                    {
                        // Update if changed
                        if (!Equals(dgv.Rows[i].Cells[1].Value, msg.AutoMessageTigger))
                            dgv.Rows[i].Cells[1].Value = msg.AutoMessageTigger;
                        if (!Equals(dgv.Rows[i].Cells[2].Value, msg.AutoMessageText))
                            dgv.Rows[i].Cells[2].Value = msg.AutoMessageText;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dgv.Rows.Add(msg.AutoMessageId, msg.AutoMessageTigger, msg.AutoMessageText);
                }
            }

            // Restore scroll position
            if (dgv.Rows.Count > 0 && scrollIndex < dgv.Rows.Count)
                dgv.FirstDisplayedScrollingRowIndex = scrollIndex;
        }


        private void actionKeyPress_slapAddMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string messageText = tb_slapMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(messageText))
                return;

            var result = chatInstanceManager.AddSlapMessage(messageText);

            if (result.Success)
            {
                tb_slapMessage.Clear();
                chatInstanceManager.UpdateGridWithMessages(dg_slapMessages, chatInstanceManager.GetSlapMessages(), 0, 1);
            }
            else
            {
                MessageBox.Show(result.Message, "Error Adding Slap Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

            var result = chatInstanceManager.RemoveSlapMessage(slapMessageId);

            if (result.Success)
            {
                chatInstanceManager.UpdateGridWithMessages(dg_slapMessages, chatInstanceManager.GetSlapMessages(), 0, 1);
            }
            else
            {
                MessageBox.Show(result.Message, "Error Removing Slap Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void actionKeyPress_AddAutoMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string messageText = tb_autoMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(messageText))
                return;

            int triggerSeconds = (int)num_AutoMessageTrigger.Value;

            var result = chatInstanceManager.AddAutoMessage(messageText, triggerSeconds);

            if (result.Success)
            {
                tb_autoMessage.Clear();
                num_AutoMessageTrigger.Value = 0;
                chatInstanceManager.UpdateGridWithMessages(dg_autoMessages, chatInstanceManager.GetAutoMessages(), 0, 1, 2);
            }
            else
            {
                MessageBox.Show(result.Message, "Error Adding Auto Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

            var result = chatInstanceManager.RemoveAutoMessage(autoMessageId);

            if (result.Success)
            {
                chatInstanceManager.UpdateGridWithMessages(dg_autoMessages, chatInstanceManager.GetAutoMessages(), 0, 1, 2);
            }
            else
            {
                MessageBox.Show(result.Message, "Error Removing Auto Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void actionKeyPress_SubmitMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            string message = tb_chatMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(message))
                return;

            int channel = chatInstanceManager.MapChannelIndexToChannel(comboBox_chatGroup.SelectedIndex);

            var result = chatInstanceManager.SendChatMessage(message, channel);

            if (result.Success)
            {
                tb_chatMessage.Clear();
            }
            else
            {
                MessageBox.Show(result.Message, "Cannot Send Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

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