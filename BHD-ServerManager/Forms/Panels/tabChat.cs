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

            chatInstanceManager.UpdateGridWithMessages(dg_autoMessages, chatInstanceManager.GetAutoMessages(), 0, 1, 2);
            chatInstanceManager.UpdateGridWithMessages(dg_slapMessages, chatInstanceManager.GetSlapMessages(), 0, 1);

            if (theInstance?.instanceStatus == InstanceStatus.ONLINE ||
                theInstance?.instanceStatus == InstanceStatus.STARTDELAY)
            {
                UpdateChatMessagesGrid();
            }

            if (theInstance?.instanceStatus == InstanceStatus.SCORING ||
                theInstance?.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                dataGridView_chatMessages.Rows.Clear();
                _autoScrollChat = true;
                return;
            }
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
        public void UpdateChatMessagesGrid()
        {
            if ((DateTime.UtcNow - _lastGridUpdate).TotalSeconds < 2)
                return;

            _lastGridUpdate = DateTime.UtcNow;

            var dgv = dataGridView_chatMessages;
            var chatLogs = chatInstance!.ChatLog;

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

            if (_autoScrollChat && dgv.Rows.Count > 10)
            {
                int visibleRows = dgv.DisplayedRowCount(false);
                dgv.FirstDisplayedScrollingRowIndex = Math.Min(dgv.Rows.Count - 1, Math.Max(30, dgv.Rows.Count - visibleRows));
            }
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