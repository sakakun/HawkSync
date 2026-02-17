namespace BHD_ServerManager.Forms.SubPanels
{
    partial class ChatHistory
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            groupBox_Filters = new GroupBox();
            btn_ExportCSV = new Button();
            btn_Clear = new Button();
            btn_LoadHistory = new Button();
            textBox_Search = new TextBox();
            label_Search = new Label();
            comboBox_TypeFilter = new ComboBox();
            label_Type = new Label();
            comboBox_PlayerFilter = new ComboBox();
            label_Player = new Label();
            dateTimePicker_To = new DateTimePicker();
            label_To = new Label();
            dateTimePicker_From = new DateTimePicker();
            label_From = new Label();
            comboBox_DateRange = new ComboBox();
            label_DateRange = new Label();
            groupBox_Results = new GroupBox();
            panel_Pagination = new Panel();
            comboBox_PageSize = new ComboBox();
            btn_NextPage = new Button();
            label_Pagination = new Label();
            btn_PrevPage = new Button();
            dataGridView_History = new DataGridView();
            Column_Timestamp = new DataGridViewTextBoxColumn();
            Column_Team = new DataGridViewTextBoxColumn();
            Column_Player = new DataGridViewTextBoxColumn();
            Column_Message = new DataGridViewTextBoxColumn();
            groupBox_Filters.SuspendLayout();
            groupBox_Results.SuspendLayout();
            panel_Pagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_History).BeginInit();
            SuspendLayout();
            // 
            // groupBox_Filters
            // 
            groupBox_Filters.Controls.Add(btn_ExportCSV);
            groupBox_Filters.Controls.Add(btn_Clear);
            groupBox_Filters.Controls.Add(btn_LoadHistory);
            groupBox_Filters.Controls.Add(textBox_Search);
            groupBox_Filters.Controls.Add(label_Search);
            groupBox_Filters.Controls.Add(comboBox_TypeFilter);
            groupBox_Filters.Controls.Add(label_Type);
            groupBox_Filters.Controls.Add(comboBox_PlayerFilter);
            groupBox_Filters.Controls.Add(label_Player);
            groupBox_Filters.Controls.Add(dateTimePicker_To);
            groupBox_Filters.Controls.Add(label_To);
            groupBox_Filters.Controls.Add(dateTimePicker_From);
            groupBox_Filters.Controls.Add(label_From);
            groupBox_Filters.Controls.Add(comboBox_DateRange);
            groupBox_Filters.Controls.Add(label_DateRange);
            groupBox_Filters.Location = new Point(3, 3);
            groupBox_Filters.Name = "groupBox_Filters";
            groupBox_Filters.Size = new Size(929, 100);
            groupBox_Filters.TabIndex = 0;
            groupBox_Filters.TabStop = false;
            groupBox_Filters.Text = "Filters";
            // 
            // btn_ExportCSV
            // 
            btn_ExportCSV.Location = new Point(803, 14);
            btn_ExportCSV.Name = "btn_ExportCSV";
            btn_ExportCSV.Size = new Size(100, 25);
            btn_ExportCSV.TabIndex = 14;
            btn_ExportCSV.Text = "Export CSV";
            btn_ExportCSV.UseVisualStyleBackColor = true;
            btn_ExportCSV.Click += btn_ExportCSV_Click;
            // 
            // btn_Clear
            // 
            btn_Clear.Location = new Point(803, 68);
            btn_Clear.Name = "btn_Clear";
            btn_Clear.Size = new Size(100, 25);
            btn_Clear.TabIndex = 13;
            btn_Clear.Text = "Clear";
            btn_Clear.UseVisualStyleBackColor = true;
            btn_Clear.Click += btn_Clear_Click;
            // 
            // btn_LoadHistory
            // 
            btn_LoadHistory.Location = new Point(803, 41);
            btn_LoadHistory.Name = "btn_LoadHistory";
            btn_LoadHistory.Size = new Size(100, 25);
            btn_LoadHistory.TabIndex = 12;
            btn_LoadHistory.Text = "Load";
            btn_LoadHistory.UseVisualStyleBackColor = true;
            btn_LoadHistory.Click += btn_LoadHistory_Click;
            // 
            // textBox_Search
            // 
            textBox_Search.Location = new Point(128, 58);
            textBox_Search.Name = "textBox_Search";
            textBox_Search.PlaceholderText = "Search message text...";
            textBox_Search.Size = new Size(450, 23);
            textBox_Search.TabIndex = 11;
            textBox_Search.KeyPress += textBox_Search_KeyPress;
            // 
            // label_Search
            // 
            label_Search.AutoSize = true;
            label_Search.Location = new Point(77, 61);
            label_Search.Name = "label_Search";
            label_Search.Size = new Size(45, 15);
            label_Search.TabIndex = 10;
            label_Search.Text = "Search:";
            // 
            // comboBox_TypeFilter
            // 
            comboBox_TypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_TypeFilter.FormattingEnabled = true;
            comboBox_TypeFilter.Items.AddRange(new object[] { "All Types", "Server Messages", "Global Chat", "Team Chat" });
            comboBox_TypeFilter.Location = new Point(644, 25);
            comboBox_TypeFilter.Name = "comboBox_TypeFilter";
            comboBox_TypeFilter.Size = new Size(130, 23);
            comboBox_TypeFilter.TabIndex = 9;
            // 
            // label_Type
            // 
            label_Type.AutoSize = true;
            label_Type.Location = new Point(603, 30);
            label_Type.Name = "label_Type";
            label_Type.Size = new Size(35, 15);
            label_Type.TabIndex = 8;
            label_Type.Text = "Type:";
            // 
            // comboBox_PlayerFilter
            // 
            comboBox_PlayerFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_PlayerFilter.FormattingEnabled = true;
            comboBox_PlayerFilter.Items.AddRange(new object[] { "All Players" });
            comboBox_PlayerFilter.Location = new Point(644, 58);
            comboBox_PlayerFilter.Name = "comboBox_PlayerFilter";
            comboBox_PlayerFilter.Size = new Size(130, 23);
            comboBox_PlayerFilter.TabIndex = 7;
            // 
            // label_Player
            // 
            label_Player.AutoSize = true;
            label_Player.Location = new Point(596, 61);
            label_Player.Name = "label_Player";
            label_Player.Size = new Size(42, 15);
            label_Player.TabIndex = 6;
            label_Player.Text = "Player:";
            // 
            // dateTimePicker_To
            // 
            dateTimePicker_To.CustomFormat = "yyyy-MM-dd HH:mm";
            dateTimePicker_To.Format = DateTimePickerFormat.Custom;
            dateTimePicker_To.Location = new Point(448, 25);
            dateTimePicker_To.Name = "dateTimePicker_To";
            dateTimePicker_To.Size = new Size(130, 23);
            dateTimePicker_To.TabIndex = 5;
            // 
            // label_To
            // 
            label_To.AutoSize = true;
            label_To.Location = new Point(419, 28);
            label_To.Name = "label_To";
            label_To.Size = new Size(23, 15);
            label_To.TabIndex = 4;
            label_To.Text = "To:";
            // 
            // dateTimePicker_From
            // 
            dateTimePicker_From.CustomFormat = "yyyy-MM-dd HH:mm";
            dateTimePicker_From.Format = DateTimePickerFormat.Custom;
            dateTimePicker_From.Location = new Point(283, 24);
            dateTimePicker_From.Name = "dateTimePicker_From";
            dateTimePicker_From.Size = new Size(130, 23);
            dateTimePicker_From.TabIndex = 3;
            // 
            // label_From
            // 
            label_From.AutoSize = true;
            label_From.Location = new Point(239, 28);
            label_From.Name = "label_From";
            label_From.Size = new Size(38, 15);
            label_From.TabIndex = 2;
            label_From.Text = "From:";
            // 
            // comboBox_DateRange
            // 
            comboBox_DateRange.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_DateRange.FormattingEnabled = true;
            comboBox_DateRange.Items.AddRange(new object[] { "Today", "Yesterday", "Last 3 Days", "Last 7 Days", "Last 30 Days", "All Time", "Custom" });
            comboBox_DateRange.Location = new Point(128, 25);
            comboBox_DateRange.Name = "comboBox_DateRange";
            comboBox_DateRange.Size = new Size(105, 23);
            comboBox_DateRange.TabIndex = 1;
            comboBox_DateRange.SelectedIndexChanged += comboBox_DateRange_SelectedIndexChanged;
            // 
            // label_DateRange
            // 
            label_DateRange.AutoSize = true;
            label_DateRange.Location = new Point(52, 28);
            label_DateRange.Name = "label_DateRange";
            label_DateRange.Size = new Size(70, 15);
            label_DateRange.TabIndex = 0;
            label_DateRange.Text = "Date Range:";
            // 
            // groupBox_Results
            // 
            groupBox_Results.Controls.Add(panel_Pagination);
            groupBox_Results.Controls.Add(dataGridView_History);
            groupBox_Results.Location = new Point(3, 109);
            groupBox_Results.Name = "groupBox_Results";
            groupBox_Results.Size = new Size(929, 302);
            groupBox_Results.TabIndex = 1;
            groupBox_Results.TabStop = false;
            groupBox_Results.Text = "Chat History";
            // 
            // panel_Pagination
            // 
            panel_Pagination.Controls.Add(comboBox_PageSize);
            panel_Pagination.Controls.Add(btn_NextPage);
            panel_Pagination.Controls.Add(label_Pagination);
            panel_Pagination.Controls.Add(btn_PrevPage);
            panel_Pagination.Dock = DockStyle.Bottom;
            panel_Pagination.Location = new Point(3, 269);
            panel_Pagination.Name = "panel_Pagination";
            panel_Pagination.Size = new Size(923, 30);
            panel_Pagination.TabIndex = 1;
            // 
            // comboBox_PageSize
            // 
            comboBox_PageSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox_PageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_PageSize.FormattingEnabled = true;
            comboBox_PageSize.Items.AddRange(new object[] { "50", "100", "250", "500" });
            comboBox_PageSize.Location = new Point(849, 3);
            comboBox_PageSize.Name = "comboBox_PageSize";
            comboBox_PageSize.Size = new Size(71, 23);
            comboBox_PageSize.TabIndex = 3;
            comboBox_PageSize.SelectedIndexChanged += comboBox_PageSize_SelectedIndexChanged;
            // 
            // btn_NextPage
            // 
            btn_NextPage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_NextPage.Location = new Point(753, 3);
            btn_NextPage.Name = "btn_NextPage";
            btn_NextPage.Size = new Size(90, 23);
            btn_NextPage.TabIndex = 2;
            btn_NextPage.Text = "Next ▶";
            btn_NextPage.UseVisualStyleBackColor = true;
            btn_NextPage.Click += btn_NextPage_Click;
            // 
            // label_Pagination
            // 
            label_Pagination.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label_Pagination.Location = new Point(100, 7);
            label_Pagination.Name = "label_Pagination";
            label_Pagination.Size = new Size(647, 15);
            label_Pagination.TabIndex = 1;
            label_Pagination.Text = "No data loaded";
            label_Pagination.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_PrevPage
            // 
            btn_PrevPage.Location = new Point(4, 3);
            btn_PrevPage.Name = "btn_PrevPage";
            btn_PrevPage.Size = new Size(90, 23);
            btn_PrevPage.TabIndex = 0;
            btn_PrevPage.Text = "◀ Prev";
            btn_PrevPage.UseVisualStyleBackColor = true;
            btn_PrevPage.Click += btn_PrevPage_Click;
            // 
            // dataGridView_History
            // 
            dataGridView_History.AllowUserToAddRows = false;
            dataGridView_History.AllowUserToDeleteRows = false;
            dataGridView_History.AllowUserToResizeRows = false;
            dataGridView_History.BackgroundColor = SystemColors.Control;
            dataGridView_History.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_History.Columns.AddRange(new DataGridViewColumn[] { Column_Timestamp, Column_Team, Column_Player, Column_Message });
            dataGridView_History.Dock = DockStyle.Fill;
            dataGridView_History.Location = new Point(3, 19);
            dataGridView_History.MultiSelect = false;
            dataGridView_History.Name = "dataGridView_History";
            dataGridView_History.ReadOnly = true;
            dataGridView_History.RowHeadersVisible = false;
            dataGridView_History.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_History.Size = new Size(923, 280);
            dataGridView_History.TabIndex = 0;
            dataGridView_History.CellFormatting += dataGridView_History_CellFormatting;
            // 
            // Column_Timestamp
            // 
            Column_Timestamp.HeaderText = "Timestamp";
            Column_Timestamp.Name = "Column_Timestamp";
            Column_Timestamp.ReadOnly = true;
            Column_Timestamp.Width = 150;
            // 
            // Column_Team
            // 
            Column_Team.HeaderText = "Team";
            Column_Team.Name = "Column_Team";
            Column_Team.ReadOnly = true;
            // 
            // Column_Player
            // 
            Column_Player.HeaderText = "Player";
            Column_Player.Name = "Column_Player";
            Column_Player.ReadOnly = true;
            Column_Player.Width = 150;
            // 
            // Column_Message
            // 
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            Column_Message.DefaultCellStyle = dataGridViewCellStyle1;
            Column_Message.HeaderText = "Message";
            Column_Message.Name = "Column_Message";
            Column_Message.ReadOnly = true;
            Column_Message.Width = 520;
            // 
            // ChatHistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox_Results);
            Controls.Add(groupBox_Filters);
            MaximumSize = new Size(935, 414);
            MinimumSize = new Size(935, 414);
            Name = "ChatHistory";
            Size = new Size(935, 414);
            groupBox_Filters.ResumeLayout(false);
            groupBox_Filters.PerformLayout();
            groupBox_Results.ResumeLayout(false);
            panel_Pagination.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_History).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_Filters;
        private Label label_DateRange;
        private ComboBox comboBox_DateRange;
        private DateTimePicker dateTimePicker_From;
        private Label label_From;
        private DateTimePicker dateTimePicker_To;
        private Label label_To;
        private ComboBox comboBox_PlayerFilter;
        private Label label_Player;
        private ComboBox comboBox_TypeFilter;
        private Label label_Type;
        private TextBox textBox_Search;
        private Label label_Search;
        private Button btn_LoadHistory;
        private Button btn_Clear;
        private Button btn_ExportCSV;
        private GroupBox groupBox_Results;
        private DataGridView dataGridView_History;
        private Panel panel_Pagination;
        private Button btn_PrevPage;
        private Label label_Pagination;
        private Button btn_NextPage;
        private ComboBox comboBox_PageSize;
        private DataGridViewTextBoxColumn Column_Timestamp;
        private DataGridViewTextBoxColumn Column_Team;
        private DataGridViewTextBoxColumn Column_Player;
        private DataGridViewTextBoxColumn Column_Message;
    }
}
