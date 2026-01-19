namespace BHD_ServerManager.Forms.Panels
{
    partial class tabBans
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
            components = new System.ComponentModel.Container();
            banControls = new TabControl();
            tabBlacklist = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            blControlDelete = new FontAwesome.Sharp.IconButton();
            dgPlayerAddressBlacklist = new DataGridView();
            blplayerip_recordID = new DataGridViewTextBoxColumn();
            blplayerip_address = new DataGridViewTextBoxColumn();
            blplayerip_datetime = new DataGridViewTextBoxColumn();
            dgPlayerNamesBlacklist = new DataGridView();
            blplayername_recordID = new DataGridViewTextBoxColumn();
            blplayername_name = new DataGridViewTextBoxColumn();
            blplayername_datetime = new DataGridViewTextBoxColumn();
            tableLayoutPanel2 = new TableLayoutPanel();
            blControl3 = new FontAwesome.Sharp.IconButton();
            blControl2 = new FontAwesome.Sharp.IconButton();
            blControl1 = new FontAwesome.Sharp.IconButton();
            panel1 = new Panel();
            groupBox5 = new GroupBox();
            textBox_notesBL = new TextBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            checkBox_tempBL = new CheckBox();
            checkBox_permBL = new CheckBox();
            groupBox3 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            dateTimePicker_blStart = new DateTimePicker();
            dateTimePicker_blEnd = new DateTimePicker();
            groupBox2 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            cb_subnetBL = new ComboBox();
            textBox_ipAddressBL = new TextBox();
            groupBox1 = new GroupBox();
            textBox_playerNameBL = new TextBox();
            tabWhitelist = new TabPage();
            tableLayoutPanel7 = new TableLayoutPanel();
            tableLayoutPanel8 = new TableLayoutPanel();
            dgPlayerAddressWhitelist = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dgPlayerNamesWhitelist = new DataGridView();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            tableLayoutPanel9 = new TableLayoutPanel();
            wlControl3 = new FontAwesome.Sharp.IconButton();
            wlControl2 = new FontAwesome.Sharp.IconButton();
            wlControl1 = new FontAwesome.Sharp.IconButton();
            panel2 = new Panel();
            groupBox6 = new GroupBox();
            textBox_notesWL = new TextBox();
            groupBox7 = new GroupBox();
            tableLayoutPanel10 = new TableLayoutPanel();
            checkBox_tempWL = new CheckBox();
            checkBox_permWL = new CheckBox();
            groupBox8 = new GroupBox();
            tableLayoutPanel11 = new TableLayoutPanel();
            dateTimePicker_WLstart = new DateTimePicker();
            dateTimePicker_WLend = new DateTimePicker();
            groupBox9 = new GroupBox();
            tableLayoutPanel12 = new TableLayoutPanel();
            cb_subnetWL = new ComboBox();
            textBox_addressWL = new TextBox();
            groupBox10 = new GroupBox();
            textBox_playerNameWL = new TextBox();
            tabPlayerHistory = new TabPage();
            tabProxyChecking = new TabPage();
            tabNetlimiter = new TabPage();
            tabSettings = new TabPage();
            toolTip1 = new ToolTip(components);
            blControlReset = new FontAwesome.Sharp.IconButton();
            blControlSave = new FontAwesome.Sharp.IconButton();
            wlControlReset = new FontAwesome.Sharp.IconButton();
            wlControlDelete = new FontAwesome.Sharp.IconButton();
            wlControlSave = new FontAwesome.Sharp.IconButton();
            banControls.SuspendLayout();
            tabBlacklist.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgPlayerAddressBlacklist).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgPlayerNamesBlacklist).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox1.SuspendLayout();
            tabWhitelist.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgPlayerAddressWhitelist).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgPlayerNamesWhitelist).BeginInit();
            tableLayoutPanel9.SuspendLayout();
            panel2.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            groupBox8.SuspendLayout();
            tableLayoutPanel11.SuspendLayout();
            groupBox9.SuspendLayout();
            tableLayoutPanel12.SuspendLayout();
            groupBox10.SuspendLayout();
            SuspendLayout();
            // 
            // banControls
            // 
            banControls.Controls.Add(tabBlacklist);
            banControls.Controls.Add(tabWhitelist);
            banControls.Controls.Add(tabPlayerHistory);
            banControls.Controls.Add(tabProxyChecking);
            banControls.Controls.Add(tabNetlimiter);
            banControls.Controls.Add(tabSettings);
            banControls.Dock = DockStyle.Fill;
            banControls.ItemSize = new Size(55, 30);
            banControls.Location = new Point(0, 0);
            banControls.Name = "banControls";
            banControls.Padding = new Point(10, 3);
            banControls.SelectedIndex = 0;
            banControls.Size = new Size(966, 422);
            banControls.TabIndex = 0;
            // 
            // tabBlacklist
            // 
            tabBlacklist.Controls.Add(tableLayoutPanel1);
            tabBlacklist.Location = new Point(4, 34);
            tabBlacklist.Name = "tabBlacklist";
            tabBlacklist.Padding = new Padding(3);
            tabBlacklist.Size = new Size(958, 384);
            tabBlacklist.TabIndex = 0;
            tabBlacklist.Text = "Blacklist";
            tabBlacklist.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 3, 2);
            tableLayoutPanel1.Controls.Add(dgPlayerAddressBlacklist, 1, 0);
            tableLayoutPanel1.Controls.Add(dgPlayerNamesBlacklist, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 3, 0);
            tableLayoutPanel1.Controls.Add(panel1, 3, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(952, 378);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 5;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(blControlSave, 4, 0);
            tableLayoutPanel3.Controls.Add(blControlReset, 0, 0);
            tableLayoutPanel3.Controls.Add(blControlDelete, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(620, 338);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(312, 40);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // blControlDelete
            // 
            blControlDelete.Dock = DockStyle.Fill;
            blControlDelete.IconChar = FontAwesome.Sharp.IconChar.Close;
            blControlDelete.IconColor = Color.Black;
            blControlDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControlDelete.IconSize = 32;
            blControlDelete.Location = new Point(127, 3);
            blControlDelete.Name = "blControlDelete";
            blControlDelete.Padding = new Padding(0, 3, 0, 0);
            blControlDelete.Size = new Size(56, 34);
            blControlDelete.TabIndex = 0;
            toolTip1.SetToolTip(blControlDelete, "Delete Record(s)");
            blControlDelete.UseVisualStyleBackColor = true;
            // 
            // dgPlayerAddressBlacklist
            // 
            dgPlayerAddressBlacklist.AllowUserToAddRows = false;
            dgPlayerAddressBlacklist.AllowUserToDeleteRows = false;
            dgPlayerAddressBlacklist.AllowUserToResizeColumns = false;
            dgPlayerAddressBlacklist.AllowUserToResizeRows = false;
            dgPlayerAddressBlacklist.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgPlayerAddressBlacklist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPlayerAddressBlacklist.Columns.AddRange(new DataGridViewColumn[] { blplayerip_recordID, blplayerip_address, blplayerip_datetime });
            dgPlayerAddressBlacklist.Dock = DockStyle.Fill;
            dgPlayerAddressBlacklist.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgPlayerAddressBlacklist.Location = new Point(303, 3);
            dgPlayerAddressBlacklist.Name = "dgPlayerAddressBlacklist";
            dgPlayerAddressBlacklist.RowHeadersVisible = false;
            tableLayoutPanel1.SetRowSpan(dgPlayerAddressBlacklist, 3);
            dgPlayerAddressBlacklist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgPlayerAddressBlacklist.Size = new Size(294, 372);
            dgPlayerAddressBlacklist.TabIndex = 1;
            // 
            // blplayerip_recordID
            // 
            blplayerip_recordID.HeaderText = "RecordID";
            blplayerip_recordID.Name = "blplayerip_recordID";
            blplayerip_recordID.Visible = false;
            // 
            // blplayerip_address
            // 
            blplayerip_address.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            blplayerip_address.HeaderText = "IP Address";
            blplayerip_address.Name = "blplayerip_address";
            // 
            // blplayerip_datetime
            // 
            blplayerip_datetime.HeaderText = "Record Added";
            blplayerip_datetime.Name = "blplayerip_datetime";
            blplayerip_datetime.Width = 110;
            // 
            // dgPlayerNamesBlacklist
            // 
            dgPlayerNamesBlacklist.AllowUserToAddRows = false;
            dgPlayerNamesBlacklist.AllowUserToDeleteRows = false;
            dgPlayerNamesBlacklist.AllowUserToResizeColumns = false;
            dgPlayerNamesBlacklist.AllowUserToResizeRows = false;
            dgPlayerNamesBlacklist.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgPlayerNamesBlacklist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPlayerNamesBlacklist.Columns.AddRange(new DataGridViewColumn[] { blplayername_recordID, blplayername_name, blplayername_datetime });
            dgPlayerNamesBlacklist.Dock = DockStyle.Fill;
            dgPlayerNamesBlacklist.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgPlayerNamesBlacklist.Location = new Point(3, 3);
            dgPlayerNamesBlacklist.Name = "dgPlayerNamesBlacklist";
            dgPlayerNamesBlacklist.RowHeadersVisible = false;
            tableLayoutPanel1.SetRowSpan(dgPlayerNamesBlacklist, 3);
            dgPlayerNamesBlacklist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgPlayerNamesBlacklist.Size = new Size(294, 372);
            dgPlayerNamesBlacklist.TabIndex = 0;
            // 
            // blplayername_recordID
            // 
            blplayername_recordID.HeaderText = "RecordID";
            blplayername_recordID.Name = "blplayername_recordID";
            blplayername_recordID.Visible = false;
            // 
            // blplayername_name
            // 
            blplayername_name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            blplayername_name.HeaderText = "Player Name";
            blplayername_name.Name = "blplayername_name";
            // 
            // blplayername_datetime
            // 
            blplayername_datetime.HeaderText = "Record Added";
            blplayername_datetime.Name = "blplayername_datetime";
            blplayername_datetime.Width = 110;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Controls.Add(blControl3, 3, 0);
            tableLayoutPanel2.Controls.Add(blControl2, 2, 0);
            tableLayoutPanel2.Controls.Add(blControl1, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(620, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(312, 40);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // blControl3
            // 
            blControl3.Dock = DockStyle.Fill;
            blControl3.IconChar = FontAwesome.Sharp.IconChar.MasksTheater;
            blControl3.IconColor = Color.Black;
            blControl3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControl3.IconSize = 32;
            blControl3.Location = new Point(189, 3);
            blControl3.Name = "blControl3";
            blControl3.Padding = new Padding(0, 3, 0, 0);
            blControl3.Size = new Size(56, 34);
            blControl3.TabIndex = 3;
            toolTip1.SetToolTip(blControl3, "New Record Both");
            blControl3.UseVisualStyleBackColor = true;
            // 
            // blControl2
            // 
            blControl2.Dock = DockStyle.Fill;
            blControl2.IconChar = FontAwesome.Sharp.IconChar.GlobeAmericas;
            blControl2.IconColor = Color.Black;
            blControl2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControl2.IconSize = 28;
            blControl2.Location = new Point(127, 3);
            blControl2.Name = "blControl2";
            blControl2.Padding = new Padding(0, 3, 0, 0);
            blControl2.Size = new Size(56, 34);
            blControl2.TabIndex = 2;
            toolTip1.SetToolTip(blControl2, "New Address Record");
            blControl2.UseVisualStyleBackColor = true;
            // 
            // blControl1
            // 
            blControl1.Dock = DockStyle.Fill;
            blControl1.IconChar = FontAwesome.Sharp.IconChar.PersonCirclePlus;
            blControl1.IconColor = Color.Black;
            blControl1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControl1.IconSize = 32;
            blControl1.Location = new Point(65, 3);
            blControl1.Name = "blControl1";
            blControl1.Padding = new Padding(4, 1, 0, 0);
            blControl1.Size = new Size(56, 34);
            blControl1.TabIndex = 0;
            toolTip1.SetToolTip(blControl1, "New Player Name Record");
            blControl1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox5);
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(620, 40);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(312, 298);
            panel1.TabIndex = 4;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(textBox_notesBL);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(0, 195);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(312, 103);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Notes";
            // 
            // textBox_notesBL
            // 
            textBox_notesBL.Dock = DockStyle.Fill;
            textBox_notesBL.Location = new Point(3, 19);
            textBox_notesBL.Multiline = true;
            textBox_notesBL.Name = "textBox_notesBL";
            textBox_notesBL.PlaceholderText = "Notes";
            textBox_notesBL.Size = new Size(306, 81);
            textBox_notesBL.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel6);
            groupBox4.Dock = DockStyle.Top;
            groupBox4.Location = new Point(0, 151);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(312, 44);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Ban Type";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(checkBox_tempBL, 0, 0);
            tableLayoutPanel6.Controls.Add(checkBox_permBL, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle());
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(306, 22);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // checkBox_tempBL
            // 
            checkBox_tempBL.AutoSize = true;
            checkBox_tempBL.Dock = DockStyle.Fill;
            checkBox_tempBL.Location = new Point(3, 3);
            checkBox_tempBL.Name = "checkBox_tempBL";
            checkBox_tempBL.Padding = new Padding(30, 0, 20, 0);
            checkBox_tempBL.Size = new Size(147, 19);
            checkBox_tempBL.TabIndex = 0;
            checkBox_tempBL.Text = "Temporary";
            checkBox_tempBL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_tempBL.UseVisualStyleBackColor = true;
            // 
            // checkBox_permBL
            // 
            checkBox_permBL.AutoSize = true;
            checkBox_permBL.Dock = DockStyle.Fill;
            checkBox_permBL.Location = new Point(156, 3);
            checkBox_permBL.Name = "checkBox_permBL";
            checkBox_permBL.Padding = new Padding(30, 0, 20, 0);
            checkBox_permBL.Size = new Size(147, 19);
            checkBox_permBL.TabIndex = 1;
            checkBox_permBL.Text = "Permanent";
            checkBox_permBL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_permBL.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel5);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(0, 100);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(312, 51);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Ban Dates - Start && End";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(dateTimePicker_blStart, 0, 0);
            tableLayoutPanel5.Controls.Add(dateTimePicker_blEnd, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(306, 29);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // dateTimePicker_blStart
            // 
            dateTimePicker_blStart.Location = new Point(3, 3);
            dateTimePicker_blStart.MaxDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_blStart.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_blStart.Name = "dateTimePicker_blStart";
            dateTimePicker_blStart.Size = new Size(147, 23);
            dateTimePicker_blStart.TabIndex = 0;
            dateTimePicker_blStart.Value = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            // 
            // dateTimePicker_blEnd
            // 
            dateTimePicker_blEnd.Checked = false;
            dateTimePicker_blEnd.Location = new Point(156, 3);
            dateTimePicker_blEnd.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_blEnd.Name = "dateTimePicker_blEnd";
            dateTimePicker_blEnd.Size = new Size(147, 23);
            dateTimePicker_blEnd.TabIndex = 1;
            dateTimePicker_blEnd.Value = new DateTime(2026, 1, 18, 15, 43, 49, 0);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel4);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 49);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(312, 51);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "IP Address";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.47059F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.5294113F));
            tableLayoutPanel4.Controls.Add(cb_subnetBL, 1, 0);
            tableLayoutPanel4.Controls.Add(textBox_ipAddressBL, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(306, 29);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // cb_subnetBL
            // 
            cb_subnetBL.DisplayMember = "32";
            cb_subnetBL.FormattingEnabled = true;
            cb_subnetBL.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" });
            cb_subnetBL.Location = new Point(237, 3);
            cb_subnetBL.Name = "cb_subnetBL";
            cb_subnetBL.Size = new Size(66, 23);
            cb_subnetBL.TabIndex = 0;
            cb_subnetBL.ValueMember = "32";
            // 
            // textBox_ipAddressBL
            // 
            textBox_ipAddressBL.Dock = DockStyle.Fill;
            textBox_ipAddressBL.Location = new Point(3, 3);
            textBox_ipAddressBL.MaxLength = 15;
            textBox_ipAddressBL.Name = "textBox_ipAddressBL";
            textBox_ipAddressBL.PlaceholderText = "000.000.000.000";
            textBox_ipAddressBL.Size = new Size(228, 23);
            textBox_ipAddressBL.TabIndex = 1;
            textBox_ipAddressBL.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox_playerNameBL);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(312, 49);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Player Name";
            // 
            // textBox_playerNameBL
            // 
            textBox_playerNameBL.Dock = DockStyle.Fill;
            textBox_playerNameBL.Location = new Point(3, 19);
            textBox_playerNameBL.MaxLength = 16;
            textBox_playerNameBL.Name = "textBox_playerNameBL";
            textBox_playerNameBL.PlaceholderText = "Player Name";
            textBox_playerNameBL.Size = new Size(306, 23);
            textBox_playerNameBL.TabIndex = 0;
            textBox_playerNameBL.TextAlign = HorizontalAlignment.Center;
            // 
            // tabWhitelist
            // 
            tabWhitelist.Controls.Add(tableLayoutPanel7);
            tabWhitelist.Location = new Point(4, 34);
            tabWhitelist.Name = "tabWhitelist";
            tabWhitelist.Padding = new Padding(3);
            tabWhitelist.Size = new Size(958, 384);
            tabWhitelist.TabIndex = 1;
            tabWhitelist.Text = "Whitelist";
            tabWhitelist.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 5;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 3, 2);
            tableLayoutPanel7.Controls.Add(dgPlayerAddressWhitelist, 1, 0);
            tableLayoutPanel7.Controls.Add(dgPlayerNamesWhitelist, 0, 0);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel9, 3, 0);
            tableLayoutPanel7.Controls.Add(panel2, 3, 1);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 3);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 3;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel7.Size = new Size(952, 378);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 5;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel8.Controls.Add(wlControlSave, 4, 0);
            tableLayoutPanel8.Controls.Add(wlControlDelete, 2, 0);
            tableLayoutPanel8.Controls.Add(wlControlReset, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(620, 338);
            tableLayoutPanel8.Margin = new Padding(0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle());
            tableLayoutPanel8.Size = new Size(312, 40);
            tableLayoutPanel8.TabIndex = 3;
            // 
            // dgPlayerAddressWhitelist
            // 
            dgPlayerAddressWhitelist.AllowUserToAddRows = false;
            dgPlayerAddressWhitelist.AllowUserToDeleteRows = false;
            dgPlayerAddressWhitelist.AllowUserToResizeColumns = false;
            dgPlayerAddressWhitelist.AllowUserToResizeRows = false;
            dgPlayerAddressWhitelist.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgPlayerAddressWhitelist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPlayerAddressWhitelist.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3 });
            dgPlayerAddressWhitelist.Dock = DockStyle.Fill;
            dgPlayerAddressWhitelist.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgPlayerAddressWhitelist.Location = new Point(303, 3);
            dgPlayerAddressWhitelist.Name = "dgPlayerAddressWhitelist";
            dgPlayerAddressWhitelist.RowHeadersVisible = false;
            tableLayoutPanel7.SetRowSpan(dgPlayerAddressWhitelist, 3);
            dgPlayerAddressWhitelist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgPlayerAddressWhitelist.Size = new Size(294, 372);
            dgPlayerAddressWhitelist.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "RecordID";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.HeaderText = "IP Address";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Record Added";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 110;
            // 
            // dgPlayerNamesWhitelist
            // 
            dgPlayerNamesWhitelist.AllowUserToAddRows = false;
            dgPlayerNamesWhitelist.AllowUserToDeleteRows = false;
            dgPlayerNamesWhitelist.AllowUserToResizeColumns = false;
            dgPlayerNamesWhitelist.AllowUserToResizeRows = false;
            dgPlayerNamesWhitelist.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgPlayerNamesWhitelist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPlayerNamesWhitelist.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6 });
            dgPlayerNamesWhitelist.Dock = DockStyle.Fill;
            dgPlayerNamesWhitelist.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgPlayerNamesWhitelist.Location = new Point(3, 3);
            dgPlayerNamesWhitelist.Name = "dgPlayerNamesWhitelist";
            dgPlayerNamesWhitelist.RowHeadersVisible = false;
            tableLayoutPanel7.SetRowSpan(dgPlayerNamesWhitelist, 3);
            dgPlayerNamesWhitelist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgPlayerNamesWhitelist.Size = new Size(294, 372);
            dgPlayerNamesWhitelist.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "RecordID";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn5.HeaderText = "Player Name";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Record Added";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 110;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 5;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel9.Controls.Add(wlControl3, 3, 0);
            tableLayoutPanel9.Controls.Add(wlControl2, 2, 0);
            tableLayoutPanel9.Controls.Add(wlControl1, 1, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(620, 0);
            tableLayoutPanel9.Margin = new Padding(0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle());
            tableLayoutPanel9.Size = new Size(312, 40);
            tableLayoutPanel9.TabIndex = 2;
            // 
            // wlControl3
            // 
            wlControl3.Dock = DockStyle.Fill;
            wlControl3.IconChar = FontAwesome.Sharp.IconChar.MasksTheater;
            wlControl3.IconColor = Color.Black;
            wlControl3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControl3.IconSize = 32;
            wlControl3.Location = new Point(189, 3);
            wlControl3.Name = "wlControl3";
            wlControl3.Padding = new Padding(0, 3, 0, 0);
            wlControl3.Size = new Size(56, 34);
            wlControl3.TabIndex = 3;
            toolTip1.SetToolTip(wlControl3, "New Record Both");
            wlControl3.UseVisualStyleBackColor = true;
            // 
            // wlControl2
            // 
            wlControl2.Dock = DockStyle.Fill;
            wlControl2.IconChar = FontAwesome.Sharp.IconChar.GlobeAmericas;
            wlControl2.IconColor = Color.Black;
            wlControl2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControl2.IconSize = 28;
            wlControl2.Location = new Point(127, 3);
            wlControl2.Name = "wlControl2";
            wlControl2.Padding = new Padding(0, 3, 0, 0);
            wlControl2.Size = new Size(56, 34);
            wlControl2.TabIndex = 2;
            toolTip1.SetToolTip(wlControl2, "New Address Record");
            wlControl2.UseVisualStyleBackColor = true;
            // 
            // wlControl1
            // 
            wlControl1.Dock = DockStyle.Fill;
            wlControl1.IconChar = FontAwesome.Sharp.IconChar.PersonCirclePlus;
            wlControl1.IconColor = Color.Black;
            wlControl1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControl1.IconSize = 32;
            wlControl1.Location = new Point(65, 3);
            wlControl1.Name = "wlControl1";
            wlControl1.Padding = new Padding(4, 1, 0, 0);
            wlControl1.Size = new Size(56, 34);
            wlControl1.TabIndex = 0;
            toolTip1.SetToolTip(wlControl1, "New Player Name Record");
            wlControl1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(groupBox6);
            panel2.Controls.Add(groupBox7);
            panel2.Controls.Add(groupBox8);
            panel2.Controls.Add(groupBox9);
            panel2.Controls.Add(groupBox10);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(620, 40);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(312, 298);
            panel2.TabIndex = 4;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(textBox_notesWL);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(0, 195);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(312, 103);
            groupBox6.TabIndex = 4;
            groupBox6.TabStop = false;
            groupBox6.Text = "Notes";
            // 
            // textBox_notesWL
            // 
            textBox_notesWL.Dock = DockStyle.Fill;
            textBox_notesWL.Location = new Point(3, 19);
            textBox_notesWL.Multiline = true;
            textBox_notesWL.Name = "textBox_notesWL";
            textBox_notesWL.PlaceholderText = "Notes";
            textBox_notesWL.Size = new Size(306, 81);
            textBox_notesWL.TabIndex = 0;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(tableLayoutPanel10);
            groupBox7.Dock = DockStyle.Top;
            groupBox7.Location = new Point(0, 151);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(312, 44);
            groupBox7.TabIndex = 3;
            groupBox7.TabStop = false;
            groupBox7.Text = "Exempt Type";
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 2;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.Controls.Add(checkBox_tempWL, 0, 0);
            tableLayoutPanel10.Controls.Add(checkBox_permWL, 1, 0);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(3, 19);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 1;
            tableLayoutPanel10.RowStyles.Add(new RowStyle());
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.Size = new Size(306, 22);
            tableLayoutPanel10.TabIndex = 0;
            // 
            // checkBox_tempWL
            // 
            checkBox_tempWL.AutoSize = true;
            checkBox_tempWL.Dock = DockStyle.Fill;
            checkBox_tempWL.Location = new Point(3, 3);
            checkBox_tempWL.Name = "checkBox_tempWL";
            checkBox_tempWL.Padding = new Padding(30, 0, 20, 0);
            checkBox_tempWL.Size = new Size(147, 19);
            checkBox_tempWL.TabIndex = 0;
            checkBox_tempWL.Text = "Temporary";
            checkBox_tempWL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_tempWL.UseVisualStyleBackColor = true;
            // 
            // checkBox_permWL
            // 
            checkBox_permWL.AutoSize = true;
            checkBox_permWL.Dock = DockStyle.Fill;
            checkBox_permWL.Location = new Point(156, 3);
            checkBox_permWL.Name = "checkBox_permWL";
            checkBox_permWL.Padding = new Padding(30, 0, 20, 0);
            checkBox_permWL.Size = new Size(147, 19);
            checkBox_permWL.TabIndex = 1;
            checkBox_permWL.Text = "Permanent";
            checkBox_permWL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_permWL.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(tableLayoutPanel11);
            groupBox8.Dock = DockStyle.Top;
            groupBox8.Location = new Point(0, 100);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(312, 51);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "Exempt Dates - Start && End";
            // 
            // tableLayoutPanel11
            // 
            tableLayoutPanel11.ColumnCount = 2;
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel11.Controls.Add(dateTimePicker_WLstart, 0, 0);
            tableLayoutPanel11.Controls.Add(dateTimePicker_WLend, 1, 0);
            tableLayoutPanel11.Dock = DockStyle.Fill;
            tableLayoutPanel11.Location = new Point(3, 19);
            tableLayoutPanel11.Name = "tableLayoutPanel11";
            tableLayoutPanel11.RowCount = 1;
            tableLayoutPanel11.RowStyles.Add(new RowStyle());
            tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.Size = new Size(306, 29);
            tableLayoutPanel11.TabIndex = 0;
            // 
            // dateTimePicker_WLstart
            // 
            dateTimePicker_WLstart.Location = new Point(3, 3);
            dateTimePicker_WLstart.MaxDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_WLstart.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_WLstart.Name = "dateTimePicker_WLstart";
            dateTimePicker_WLstart.Size = new Size(147, 23);
            dateTimePicker_WLstart.TabIndex = 0;
            dateTimePicker_WLstart.Value = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            // 
            // dateTimePicker_WLend
            // 
            dateTimePicker_WLend.Checked = false;
            dateTimePicker_WLend.Location = new Point(156, 3);
            dateTimePicker_WLend.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_WLend.Name = "dateTimePicker_WLend";
            dateTimePicker_WLend.Size = new Size(147, 23);
            dateTimePicker_WLend.TabIndex = 1;
            dateTimePicker_WLend.Value = new DateTime(2026, 1, 18, 15, 43, 49, 0);
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(tableLayoutPanel12);
            groupBox9.Dock = DockStyle.Top;
            groupBox9.Location = new Point(0, 49);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(312, 51);
            groupBox9.TabIndex = 1;
            groupBox9.TabStop = false;
            groupBox9.Text = "IP Address";
            // 
            // tableLayoutPanel12
            // 
            tableLayoutPanel12.ColumnCount = 2;
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.47059F));
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.5294113F));
            tableLayoutPanel12.Controls.Add(cb_subnetWL, 1, 0);
            tableLayoutPanel12.Controls.Add(textBox_addressWL, 0, 0);
            tableLayoutPanel12.Dock = DockStyle.Fill;
            tableLayoutPanel12.Location = new Point(3, 19);
            tableLayoutPanel12.Name = "tableLayoutPanel12";
            tableLayoutPanel12.RowCount = 1;
            tableLayoutPanel12.RowStyles.Add(new RowStyle());
            tableLayoutPanel12.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel12.Size = new Size(306, 29);
            tableLayoutPanel12.TabIndex = 0;
            // 
            // cb_subnetWL
            // 
            cb_subnetWL.DisplayMember = "32";
            cb_subnetWL.FormattingEnabled = true;
            cb_subnetWL.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" });
            cb_subnetWL.Location = new Point(237, 3);
            cb_subnetWL.Name = "cb_subnetWL";
            cb_subnetWL.Size = new Size(66, 23);
            cb_subnetWL.TabIndex = 0;
            cb_subnetWL.ValueMember = "32";
            // 
            // textBox_addressWL
            // 
            textBox_addressWL.Dock = DockStyle.Fill;
            textBox_addressWL.Location = new Point(3, 3);
            textBox_addressWL.MaxLength = 15;
            textBox_addressWL.Name = "textBox_addressWL";
            textBox_addressWL.PlaceholderText = "000.000.000.000";
            textBox_addressWL.Size = new Size(228, 23);
            textBox_addressWL.TabIndex = 1;
            textBox_addressWL.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(textBox_playerNameWL);
            groupBox10.Dock = DockStyle.Top;
            groupBox10.Location = new Point(0, 0);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(312, 49);
            groupBox10.TabIndex = 0;
            groupBox10.TabStop = false;
            groupBox10.Text = "Player Name";
            // 
            // textBox_playerNameWL
            // 
            textBox_playerNameWL.Dock = DockStyle.Fill;
            textBox_playerNameWL.Location = new Point(3, 19);
            textBox_playerNameWL.MaxLength = 16;
            textBox_playerNameWL.Name = "textBox_playerNameWL";
            textBox_playerNameWL.PlaceholderText = "Player Name";
            textBox_playerNameWL.Size = new Size(306, 23);
            textBox_playerNameWL.TabIndex = 0;
            textBox_playerNameWL.TextAlign = HorizontalAlignment.Center;
            // 
            // tabPlayerHistory
            // 
            tabPlayerHistory.Location = new Point(4, 34);
            tabPlayerHistory.Name = "tabPlayerHistory";
            tabPlayerHistory.Size = new Size(958, 384);
            tabPlayerHistory.TabIndex = 3;
            tabPlayerHistory.Text = "Player History";
            tabPlayerHistory.UseVisualStyleBackColor = true;
            // 
            // tabProxyChecking
            // 
            tabProxyChecking.Location = new Point(4, 34);
            tabProxyChecking.Name = "tabProxyChecking";
            tabProxyChecking.Size = new Size(958, 384);
            tabProxyChecking.TabIndex = 2;
            tabProxyChecking.Text = "Proxy Checking";
            tabProxyChecking.UseVisualStyleBackColor = true;
            // 
            // tabNetlimiter
            // 
            tabNetlimiter.Location = new Point(4, 34);
            tabNetlimiter.Name = "tabNetlimiter";
            tabNetlimiter.Size = new Size(958, 384);
            tabNetlimiter.TabIndex = 4;
            tabNetlimiter.Text = "Netlimiter";
            tabNetlimiter.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            tabSettings.Location = new Point(4, 34);
            tabSettings.Name = "tabSettings";
            tabSettings.Size = new Size(958, 384);
            tabSettings.TabIndex = 5;
            tabSettings.Text = "Settings";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // blControlReset
            // 
            blControlReset.Dock = DockStyle.Fill;
            blControlReset.IconChar = FontAwesome.Sharp.IconChar.Reply;
            blControlReset.IconColor = Color.Black;
            blControlReset.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControlReset.IconSize = 32;
            blControlReset.Location = new Point(3, 3);
            blControlReset.Name = "blControlReset";
            blControlReset.Padding = new Padding(0, 3, 0, 0);
            blControlReset.Size = new Size(56, 34);
            blControlReset.TabIndex = 1;
            toolTip1.SetToolTip(blControlReset, "Reset");
            blControlReset.UseVisualStyleBackColor = true;
            // 
            // blControlSave
            // 
            blControlSave.Dock = DockStyle.Fill;
            blControlSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            blControlSave.IconColor = Color.Black;
            blControlSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControlSave.IconSize = 32;
            blControlSave.Location = new Point(251, 3);
            blControlSave.Name = "blControlSave";
            blControlSave.Padding = new Padding(0, 3, 0, 0);
            blControlSave.Size = new Size(58, 34);
            blControlSave.TabIndex = 2;
            toolTip1.SetToolTip(blControlSave, "Save");
            blControlSave.UseVisualStyleBackColor = true;
            // 
            // wlControlReset
            // 
            wlControlReset.Dock = DockStyle.Fill;
            wlControlReset.IconChar = FontAwesome.Sharp.IconChar.Reply;
            wlControlReset.IconColor = Color.Black;
            wlControlReset.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlReset.IconSize = 32;
            wlControlReset.Location = new Point(3, 3);
            wlControlReset.Name = "wlControlReset";
            wlControlReset.Padding = new Padding(0, 3, 0, 0);
            wlControlReset.Size = new Size(56, 34);
            wlControlReset.TabIndex = 2;
            toolTip1.SetToolTip(wlControlReset, "Reset");
            wlControlReset.UseVisualStyleBackColor = true;
            // 
            // wlControlDelete
            // 
            wlControlDelete.Dock = DockStyle.Fill;
            wlControlDelete.IconChar = FontAwesome.Sharp.IconChar.Close;
            wlControlDelete.IconColor = Color.Black;
            wlControlDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlDelete.IconSize = 32;
            wlControlDelete.Location = new Point(127, 3);
            wlControlDelete.Name = "wlControlDelete";
            wlControlDelete.Padding = new Padding(0, 3, 0, 0);
            wlControlDelete.Size = new Size(56, 34);
            wlControlDelete.TabIndex = 3;
            toolTip1.SetToolTip(wlControlDelete, "Delete Record(s)");
            wlControlDelete.UseVisualStyleBackColor = true;
            // 
            // wlControlSave
            // 
            wlControlSave.Dock = DockStyle.Fill;
            wlControlSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            wlControlSave.IconColor = Color.Black;
            wlControlSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlSave.IconSize = 32;
            wlControlSave.Location = new Point(251, 3);
            wlControlSave.Name = "wlControlSave";
            wlControlSave.Padding = new Padding(0, 3, 0, 0);
            wlControlSave.Size = new Size(58, 34);
            wlControlSave.TabIndex = 4;
            toolTip1.SetToolTip(wlControlSave, "Save");
            wlControlSave.UseVisualStyleBackColor = true;
            // 
            // tabBans
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(banControls);
            Name = "tabBans";
            Size = new Size(966, 422);
            banControls.ResumeLayout(false);
            tabBlacklist.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgPlayerAddressBlacklist).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgPlayerNamesBlacklist).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabWhitelist.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgPlayerAddressWhitelist).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgPlayerNamesWhitelist).EndInit();
            tableLayoutPanel9.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel10.PerformLayout();
            groupBox8.ResumeLayout(false);
            tableLayoutPanel11.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            tableLayoutPanel12.ResumeLayout(false);
            tableLayoutPanel12.PerformLayout();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl banControls;
        private TabPage tabBlacklist;
        private TabPage tabWhitelist;
        private TabPage tabPlayerHistory;
        private TabPage tabProxyChecking;
        private TabPage tabNetlimiter;
        private TabPage tabSettings;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dgPlayerNamesBlacklist;
        private DataGridView dgPlayerAddressBlacklist;
        private DataGridViewTextBoxColumn blplayerip_recordID;
        private DataGridViewTextBoxColumn blplayerip_address;
        private DataGridViewTextBoxColumn blplayerip_datetime;
        private DataGridViewTextBoxColumn blplayername_recordID;
        private DataGridViewTextBoxColumn blplayername_name;
        private DataGridViewTextBoxColumn blplayername_datetime;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBox1;
        private TextBox textBox_playerNameBL;
        private ComboBox cb_subnetBL;
        private TextBox textBox_ipAddressBL;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel5;
        private DateTimePicker dateTimePicker_blStart;
        private DateTimePicker dateTimePicker_blEnd;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel6;
        private CheckBox checkBox_tempBL;
        private CheckBox checkBox_permBL;
        private GroupBox groupBox5;
        private TextBox textBox_notesBL;
        private FontAwesome.Sharp.IconButton blControl1;
        private FontAwesome.Sharp.IconButton blControl3;
        private FontAwesome.Sharp.IconButton blControl2;
        private ToolTip toolTip1;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel8;
        private DataGridView dgPlayerAddressWhitelist;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridView dgPlayerNamesWhitelist;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private TableLayoutPanel tableLayoutPanel9;
        private FontAwesome.Sharp.IconButton wlControl3;
        private FontAwesome.Sharp.IconButton wlControl2;
        private FontAwesome.Sharp.IconButton wlControl1;
        private Panel panel2;
        private GroupBox groupBox6;
        private TextBox textBox_notesWL;
        private GroupBox groupBox7;
        private TableLayoutPanel tableLayoutPanel10;
        private CheckBox checkBox_tempWL;
        private CheckBox checkBox_permWL;
        private GroupBox groupBox8;
        private TableLayoutPanel tableLayoutPanel11;
        private DateTimePicker dateTimePicker_WLstart;
        private DateTimePicker dateTimePicker_WLend;
        private GroupBox groupBox9;
        private TableLayoutPanel tableLayoutPanel12;
        private ComboBox cb_subnetWL;
        private TextBox textBox_addressWL;
        private GroupBox groupBox10;
        private TextBox textBox_playerNameWL;
        private FontAwesome.Sharp.IconButton blControlDelete;
        private FontAwesome.Sharp.IconButton blControlSave;
        private FontAwesome.Sharp.IconButton blControlReset;
        private FontAwesome.Sharp.IconButton wlControlSave;
        private FontAwesome.Sharp.IconButton wlControlDelete;
        private FontAwesome.Sharp.IconButton wlControlReset;
    }
}
