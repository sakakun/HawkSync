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
        // Add this class variable:
        private bool _autoScrollChat = true;

        public tabChat()
        {
            InitializeComponent();

            dataGridView_chatMessages.Scroll += dataGridView_chatMessages_Scroll;

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

        // Scroll event handler to track if user is at the bottom
        private void dataGridView_chatMessages_Scroll(object? sender, ScrollEventArgs e)
        {
            var dgv = dataGridView_chatMessages;
            int firstDisplayedRow = dgv.FirstDisplayedScrollingRowIndex >= 0 ? dgv.FirstDisplayedScrollingRowIndex : 0;
            int visibleRows = dgv.DisplayedRowCount(false);
            bool atBottom = (firstDisplayedRow + visibleRows) >= dgv.Rows.Count;
            _autoScrollChat = atBottom;
        }

        private void OnSnapshotReceived(ServerSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ServerSnapshot>(OnSnapshotReceived), snapshot);
                return;
            }

            // Update chat instance
            functionEvent_UpdateSlapMessages();
            functionEvent_UpdateAutoMessages();
            functionEvent_UpdateChatMessagesGrid();
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

        private async void SendChatMessage()
        {
            string message = tb_chatMessage.Text.Trim();
            int channel = MapChannelIndexToChannel(comboBox_chatGroup.SelectedIndex);
            if (string.IsNullOrEmpty(message)) return;

            var result = await ApiCore.ApiClient!.SendChatAsync(message, channel);
            if (result.Success) tb_chatMessage.Clear();
            else MessageBox.Show($"Failed to send: {result.Message}");
        }

        /// <summary>
        /// Map UI channel dropdown index to actual channel number
        /// </summary>
        public static int MapChannelIndexToChannel(int selectedIndex)
        {
            return selectedIndex switch
            {
                1 => 1, // Global
                2 => 2, // Blue
                3 => 3, // Red
                0 => 0, // Server
                _ => 0  // Default to server
            };
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