namespace BHD_ServerManager.Forms.Panels
{
    partial class tabChat
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chat_TabControl = new TabControl();
            tabChatMessages = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridView_chatMessages = new DataGridView();
            chat_timestamp = new DataGridViewTextBoxColumn();
            chat_chatGroup = new DataGridViewTextBoxColumn();
            chat_playerName = new DataGridViewTextBoxColumn();
            chat_Message = new DataGridViewTextBoxColumn();
            tableLayoutPanel2 = new TableLayoutPanel();
            comboBox_chatGroup = new ComboBox();
            tb_chatMessage = new TextBox();
            tabAutoMessages = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            dg_autoMessages = new DataGridView();
            autoMessageID = new DataGridViewTextBoxColumn();
            autoTrigger = new DataGridViewTextBoxColumn();
            autoMessageText = new DataGridViewTextBoxColumn();
            tableLayoutPanel4 = new TableLayoutPanel();
            tb_autoMessage = new TextBox();
            num_AutoMessageTrigger = new NumericUpDown();
            tabSlapMessages = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            tb_slapMessage = new TextBox();
            dg_slapMessages = new DataGridView();
            slapMessageID = new DataGridViewTextBoxColumn();
            slapMessages = new DataGridViewTextBoxColumn();
            chat_TabControl.SuspendLayout();
            tabChatMessages.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_chatMessages).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            tabAutoMessages.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_autoMessages).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_AutoMessageTrigger).BeginInit();
            tabSlapMessages.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_slapMessages).BeginInit();
            SuspendLayout();
            // 
            // chat_TabControl
            // 
            chat_TabControl.Alignment = TabAlignment.Right;
            chat_TabControl.Controls.Add(tabChatMessages);
            chat_TabControl.Controls.Add(tabAutoMessages);
            chat_TabControl.Controls.Add(tabSlapMessages);
            chat_TabControl.Dock = DockStyle.Fill;
            chat_TabControl.Location = new Point(0, 0);
            chat_TabControl.Margin = new Padding(0);
            chat_TabControl.Multiline = true;
            chat_TabControl.Name = "chat_TabControl";
            chat_TabControl.SelectedIndex = 0;
            chat_TabControl.Size = new Size(902, 362);
            chat_TabControl.TabIndex = 0;
            // 
            // tabChatMessages
            // 
            tabChatMessages.Controls.Add(tableLayoutPanel1);
            tabChatMessages.Location = new Point(4, 4);
            tabChatMessages.Margin = new Padding(0);
            tabChatMessages.Name = "tabChatMessages";
            tabChatMessages.Padding = new Padding(3);
            tabChatMessages.Size = new Size(871, 354);
            tabChatMessages.TabIndex = 0;
            tabChatMessages.Text = "Chat Messages";
            tabChatMessages.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(dataGridView_chatMessages, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.Size = new Size(865, 348);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView_chatMessages
            // 
            dataGridView_chatMessages.AllowUserToAddRows = false;
            dataGridView_chatMessages.AllowUserToDeleteRows = false;
            dataGridView_chatMessages.AllowUserToResizeColumns = false;
            dataGridView_chatMessages.AllowUserToResizeRows = false;
            dataGridView_chatMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_chatMessages.Columns.AddRange(new DataGridViewColumn[] { chat_timestamp, chat_chatGroup, chat_playerName, chat_Message });
            dataGridView_chatMessages.Dock = DockStyle.Fill;
            dataGridView_chatMessages.Location = new Point(0, 0);
            dataGridView_chatMessages.Margin = new Padding(0);
            dataGridView_chatMessages.Name = "dataGridView_chatMessages";
            dataGridView_chatMessages.ReadOnly = true;
            dataGridView_chatMessages.RowHeadersVisible = false;
            dataGridView_chatMessages.Size = new Size(865, 324);
            dataGridView_chatMessages.TabIndex = 0;
            // 
            // chat_timestamp
            // 
            chat_timestamp.HeaderText = "Timestamp";
            chat_timestamp.Name = "chat_timestamp";
            chat_timestamp.ReadOnly = true;
            // 
            // chat_chatGroup
            // 
            chat_chatGroup.HeaderText = "Team";
            chat_chatGroup.Name = "chat_chatGroup";
            chat_chatGroup.ReadOnly = true;
            // 
            // chat_playerName
            // 
            chat_playerName.HeaderText = "Player";
            chat_playerName.Name = "chat_playerName";
            chat_playerName.ReadOnly = true;
            // 
            // chat_Message
            // 
            chat_Message.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            chat_Message.HeaderText = "Message";
            chat_Message.Name = "chat_Message";
            chat_Message.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(comboBox_chatGroup, 0, 0);
            tableLayoutPanel2.Controls.Add(tb_chatMessage, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 324);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(865, 24);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // comboBox_chatGroup
            // 
            comboBox_chatGroup.DisplayMember = "Global";
            comboBox_chatGroup.Dock = DockStyle.Fill;
            comboBox_chatGroup.DropDownHeight = 110;
            comboBox_chatGroup.DropDownWidth = 125;
            comboBox_chatGroup.FormattingEnabled = true;
            comboBox_chatGroup.IntegralHeight = false;
            comboBox_chatGroup.Items.AddRange(new object[] { "Global", "Announcement", "Red Team", "Blue Team" });
            comboBox_chatGroup.Location = new Point(0, 0);
            comboBox_chatGroup.Margin = new Padding(0);
            comboBox_chatGroup.Name = "comboBox_chatGroup";
            comboBox_chatGroup.Size = new Size(125, 23);
            comboBox_chatGroup.TabIndex = 0;
            comboBox_chatGroup.Text = "Global";
            // 
            // tb_chatMessage
            // 
            tb_chatMessage.Dock = DockStyle.Fill;
            tb_chatMessage.Location = new Point(125, 0);
            tb_chatMessage.Margin = new Padding(0);
            tb_chatMessage.Name = "tb_chatMessage";
            tb_chatMessage.Size = new Size(740, 23);
            tb_chatMessage.TabIndex = 1;
            tb_chatMessage.KeyPress += actionKeyPress_SubmitMessage;
            // 
            // tabAutoMessages
            // 
            tabAutoMessages.Controls.Add(tableLayoutPanel3);
            tabAutoMessages.Location = new Point(4, 4);
            tabAutoMessages.Margin = new Padding(0);
            tabAutoMessages.Name = "tabAutoMessages";
            tabAutoMessages.Padding = new Padding(3);
            tabAutoMessages.Size = new Size(871, 354);
            tabAutoMessages.TabIndex = 1;
            tabAutoMessages.Text = "Auto Messages";
            tabAutoMessages.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(dg_autoMessages, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.Size = new Size(865, 348);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // dg_autoMessages
            // 
            dg_autoMessages.AllowUserToAddRows = false;
            dg_autoMessages.AllowUserToDeleteRows = false;
            dg_autoMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_autoMessages.Columns.AddRange(new DataGridViewColumn[] { autoMessageID, autoTrigger, autoMessageText });
            dg_autoMessages.Dock = DockStyle.Fill;
            dg_autoMessages.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_autoMessages.Location = new Point(0, 0);
            dg_autoMessages.Margin = new Padding(0, 0, 4, 0);
            dg_autoMessages.Name = "dg_autoMessages";
            dg_autoMessages.ReadOnly = true;
            dg_autoMessages.RowHeadersVisible = false;
            dg_autoMessages.Size = new Size(865, 324);
            dg_autoMessages.TabIndex = 1;
            dg_autoMessages.CellDoubleClick += actionClick_RemoveAutoMessage;
            // 
            // autoMessageID
            // 
            autoMessageID.HeaderText = "autoMessageID";
            autoMessageID.Name = "autoMessageID";
            autoMessageID.ReadOnly = true;
            autoMessageID.Visible = false;
            // 
            // autoTrigger
            // 
            autoTrigger.HeaderText = "Trigger (Min)";
            autoTrigger.Name = "autoTrigger";
            autoTrigger.ReadOnly = true;
            // 
            // autoMessageText
            // 
            autoMessageText.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            autoMessageText.HeaderText = "Message";
            autoMessageText.Name = "autoMessageText";
            autoMessageText.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(tb_autoMessage, 1, 0);
            tableLayoutPanel4.Controls.Add(num_AutoMessageTrigger, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 324);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(869, 24);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // tb_autoMessage
            // 
            tb_autoMessage.Dock = DockStyle.Fill;
            tb_autoMessage.Location = new Point(125, 0);
            tb_autoMessage.Margin = new Padding(0);
            tb_autoMessage.Name = "tb_autoMessage";
            tb_autoMessage.Size = new Size(744, 23);
            tb_autoMessage.TabIndex = 4;
            tb_autoMessage.KeyPress += actionKeyPressed_AddAutoMessage;
            // 
            // num_AutoMessageTrigger
            // 
            num_AutoMessageTrigger.Dock = DockStyle.Fill;
            num_AutoMessageTrigger.Location = new Point(0, 0);
            num_AutoMessageTrigger.Margin = new Padding(0);
            num_AutoMessageTrigger.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            num_AutoMessageTrigger.Name = "num_AutoMessageTrigger";
            num_AutoMessageTrigger.Size = new Size(125, 23);
            num_AutoMessageTrigger.TabIndex = 3;
            num_AutoMessageTrigger.TextAlign = HorizontalAlignment.Right;
            // 
            // tabSlapMessages
            // 
            tabSlapMessages.Controls.Add(tableLayoutPanel5);
            tabSlapMessages.Location = new Point(4, 4);
            tabSlapMessages.Margin = new Padding(0);
            tabSlapMessages.Name = "tabSlapMessages";
            tabSlapMessages.Padding = new Padding(3);
            tabSlapMessages.Size = new Size(871, 354);
            tabSlapMessages.TabIndex = 2;
            tabSlapMessages.Text = "Slap Messages";
            tabSlapMessages.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.Controls.Add(tb_slapMessage, 0, 1);
            tableLayoutPanel5.Controls.Add(dg_slapMessages, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel5.Size = new Size(865, 348);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // tb_slapMessage
            // 
            tb_slapMessage.Dock = DockStyle.Fill;
            tb_slapMessage.Location = new Point(0, 324);
            tb_slapMessage.Margin = new Padding(0);
            tb_slapMessage.Name = "tb_slapMessage";
            tb_slapMessage.Size = new Size(865, 23);
            tb_slapMessage.TabIndex = 2;
            tb_slapMessage.KeyPress += actionKeyPress_slapAddMessage;
            // 
            // dg_slapMessages
            // 
            dg_slapMessages.AllowUserToAddRows = false;
            dg_slapMessages.AllowUserToDeleteRows = false;
            dg_slapMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_slapMessages.Columns.AddRange(new DataGridViewColumn[] { slapMessageID, slapMessages });
            dg_slapMessages.Dock = DockStyle.Fill;
            dg_slapMessages.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_slapMessages.Location = new Point(0, 0);
            dg_slapMessages.Margin = new Padding(0);
            dg_slapMessages.Name = "dg_slapMessages";
            dg_slapMessages.ReadOnly = true;
            dg_slapMessages.RowHeadersVisible = false;
            dg_slapMessages.Size = new Size(865, 324);
            dg_slapMessages.TabIndex = 1;
            dg_slapMessages.CellContentDoubleClick += actionClick_RemoveSlap;
            // 
            // slapMessageID
            // 
            slapMessageID.HeaderText = "slapMessageID";
            slapMessageID.Name = "slapMessageID";
            slapMessageID.ReadOnly = true;
            slapMessageID.Visible = false;
            // 
            // slapMessages
            // 
            slapMessages.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            slapMessages.HeaderText = "Message";
            slapMessages.Name = "slapMessages";
            slapMessages.ReadOnly = true;
            // 
            // tabChat
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(chat_TabControl);
            Margin = new Padding(0);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabChat";
            Size = new Size(902, 362);
            chat_TabControl.ResumeLayout(false);
            tabChatMessages.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_chatMessages).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tabAutoMessages.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_autoMessages).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_AutoMessageTrigger).EndInit();
            tabSlapMessages.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dg_slapMessages).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl chat_TabControl;
        private TabPage tabChatMessages;
        private TabPage tabAutoMessages;
        private TabPage tabSlapMessages;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridViewTextBoxColumn chat_timestamp;
        private DataGridViewTextBoxColumn chat_chatGroup;
        private DataGridViewTextBoxColumn chat_playerName;
        private DataGridViewTextBoxColumn chat_Message;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox comboBox_chatGroup;
        public DataGridView dataGridView_chatMessages;
        public TextBox tb_chatMessage;
        private TableLayoutPanel tableLayoutPanel3;
        internal DataGridView dg_autoMessages;
        private TableLayoutPanel tableLayoutPanel4;
        private NumericUpDown num_AutoMessageTrigger;
        private TextBox tb_autoMessage;
        private TableLayoutPanel tableLayoutPanel5;
        internal DataGridView dg_slapMessages;
        private DataGridViewTextBoxColumn slapMessageID;
        private DataGridViewTextBoxColumn slapMessages;
        private TextBox tb_slapMessage;
        private DataGridViewTextBoxColumn autoMessageID;
        private DataGridViewTextBoxColumn autoTrigger;
        private DataGridViewTextBoxColumn autoMessageText;
    }
}
