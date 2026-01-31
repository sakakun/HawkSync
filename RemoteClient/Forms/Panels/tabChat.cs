using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using RemoteClient.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteClient.Forms.Panels
{
    public partial class tabChat : UserControl
    {

        // --- Instance Objects ---
        private theInstance? theInstance => CommonCore.theInstance;
        private chatInstance? chatInstance => CommonCore.instanceChat;
        // --- Class Variables ---
        private DateTime _lastGridUpdate;
        private bool _chatGridFirstLoad = true;

        public tabChat()
        {
            InitializeComponent();

            // Wire up Enter key for chat message send
            tb_chatMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendChatMessage();
                    e.SuppressKeyPress = true;
                }
            };

            // Wire up Enter key for auto message add
            tb_autoMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddAutoMessage();
                    e.SuppressKeyPress = true;
                }
            };

            // Wire up Enter key for slap message add
            tb_slapMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddSlapMessage();
                    e.SuppressKeyPress = true;
                }
            };

            // Wire up double-click for auto message removal
            dg_autoMessages.CellDoubleClick += dg_autoMessages_CellDoubleClick!;

            // Wire up double-click for slap message removal
            dg_slapMessages.CellDoubleClick += dg_slapMessages_CellDoubleClick!;

            // OPTIONALLY: Subscribe to snapshots for server info panel
            ApiCore.OnSnapshotReceived += OnSnapshotReceived;
        }

        private void OnSnapshotReceived(ServerSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ServerSnapshot>(OnSnapshotReceived), snapshot);
                return;
            }

            // Update chat instance
            fuctionEvent_UpdateSlapMessages();
            functionEvent_UpdateAutoMessages();
            functionEvent_UpdateChatMessagesGrid();
        }

        /// <summary>
        /// Update slap messages grid from manager
        /// </summary>
        public void fuctionEvent_UpdateSlapMessages()
        {
            dg_slapMessages.Rows.Clear();
            
            foreach (var slapMsg in chatInstance!.SlapMessages)
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
            
            foreach (var autoMsg in chatInstance!.AutoMessages)
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

        private async void SendChatMessage()
        {
            string message = tb_chatMessage.Text.Trim();
            int channel = comboBox_chatGroup.SelectedIndex;
            if (string.IsNullOrEmpty(message)) return;

            var result = await ApiCore.ApiClient!.SendChatAsync(message, channel);
            if (result.Success) tb_chatMessage.Clear();
            else MessageBox.Show($"Failed to send: {result.Message}");
        }

        private async void AddAutoMessage()
        {
            string message = tb_autoMessage.Text.Trim();
            int interval = (int)num_AutoMessageTrigger.Value;
            if (string.IsNullOrEmpty(message)) return;

            var result = await ApiCore.ApiClient!.SendAutoMessageAsync(message, interval);
            if (result.Success) tb_autoMessage.Clear();
            else MessageBox.Show($"Failed to add auto message: {result.Message}");
        }

        private async void dg_autoMessages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dg_autoMessages.Rows[e.RowIndex].Cells["autoMessageID"].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            var result = await ApiCore.ApiClient!.RemoveAutoMessageAsync(id);
            if (!result.Success) MessageBox.Show($"Failed to remove auto message: {result.Message}");
        }

        private async void AddSlapMessage()
        {
            string message = tb_slapMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            var result = await ApiCore.ApiClient!.SendSlapMessageAsync(message);
            if (result.Success) tb_slapMessage.Clear();
            else MessageBox.Show($"Failed to add slap message: {result.Message}");
        }

        private async void dg_slapMessages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dg_slapMessages.Rows[e.RowIndex].Cells["slapMessageID"].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            var result = await ApiCore.ApiClient!.RemoveSlapMessageAsync(id);
            if (!result.Success) MessageBox.Show($"Failed to remove slap message: {result.Message}");
        }

    }
}