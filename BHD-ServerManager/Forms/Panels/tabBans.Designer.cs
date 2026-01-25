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
            blacklist_btnDelete = new FontAwesome.Sharp.IconButton();
            blacklist_btnSave = new FontAwesome.Sharp.IconButton();
            blacklist_btnReset = new FontAwesome.Sharp.IconButton();
            dgPlayerAddressBlacklist = new DataGridView();
            blplayerip_recordID = new DataGridViewTextBoxColumn();
            blplayerip_address = new DataGridViewTextBoxColumn();
            blplayerip_datetime = new DataGridViewTextBoxColumn();
            dgPlayerNamesBlacklist = new DataGridView();
            blplayername_recordID = new DataGridViewTextBoxColumn();
            blplayername_name = new DataGridViewTextBoxColumn();
            blplayername_datetime = new DataGridViewTextBoxColumn();
            tableLayoutPanel2 = new TableLayoutPanel();
            blacklist_btnClose = new FontAwesome.Sharp.IconButton();
            blControlRefresh = new FontAwesome.Sharp.IconButton();
            blControl3 = new FontAwesome.Sharp.IconButton();
            blControl2 = new FontAwesome.Sharp.IconButton();
            blControl1 = new FontAwesome.Sharp.IconButton();
            blacklistForm = new Panel();
            groupBox5 = new GroupBox();
            blacklist_notes = new TextBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            blacklist_TempBan = new CheckBox();
            blacklist_PermBan = new CheckBox();
            blacklist_DateBoxes = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            blacklist_DateStart = new DateTimePicker();
            blacklist_DateEnd = new DateTimePicker();
            blacklist_IPAddress = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            blacklist_IPSubnetTxt = new ComboBox();
            blacklist_IPAddressTxt = new TextBox();
            blacklist_PlayerName = new GroupBox();
            blacklist_PlayerNameTxt = new TextBox();
            tabWhitelist = new TabPage();
            tableLayoutPanel7 = new TableLayoutPanel();
            tableLayoutPanel8 = new TableLayoutPanel();
            wlControlDelete = new FontAwesome.Sharp.IconButton();
            wlControlSave = new FontAwesome.Sharp.IconButton();
            wlControlReset = new FontAwesome.Sharp.IconButton();
            dgPlayerAddressWhitelist = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dgPlayerNamesWhitelist = new DataGridView();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            tableLayoutPanel9 = new TableLayoutPanel();
            wlControlClose = new FontAwesome.Sharp.IconButton();
            wlControlRefresh = new FontAwesome.Sharp.IconButton();
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
            tabProxyChecking = new TabPage();
            tableLayoutPanel16 = new TableLayoutPanel();
            tableLayoutPanel17 = new TableLayoutPanel();
            dgProxyCountryBlockList = new DataGridView();
            geo_recordID = new DataGridViewTextBoxColumn();
            proxy_countyCode = new DataGridViewTextBoxColumn();
            proxy_countyName = new DataGridViewTextBoxColumn();
            tableLayoutPanel15 = new TableLayoutPanel();
            textBox_countryCode = new TextBox();
            textBox_countryName = new TextBox();
            btn_proxyAddCountry = new Button();
            tabControl1 = new TabControl();
            proxyLookup = new TabPage();
            tabPage2 = new TabPage();
            tableLayoutPanel14 = new TableLayoutPanel();
            groupBox12 = new GroupBox();
            tableLayoutPanel19 = new TableLayoutPanel();
            cb_serviceIP2LocationIO = new CheckBox();
            cb_serviceProxyCheckIO = new CheckBox();
            panel3 = new Panel();
            groupBox15 = new GroupBox();
            tableLayoutPanel22 = new TableLayoutPanel();
            checkBox_GeoAllow = new CheckBox();
            checkBox_GeoBlock = new CheckBox();
            checkBox_GeoOff = new CheckBox();
            groupBox14 = new GroupBox();
            tableLayoutPanel21 = new TableLayoutPanel();
            btn_proxyTest = new FontAwesome.Sharp.IconButton();
            btn_proxyReset = new FontAwesome.Sharp.IconButton();
            btn_proxySave = new FontAwesome.Sharp.IconButton();
            groupBox13 = new GroupBox();
            tableLayoutPanel20 = new TableLayoutPanel();
            label5 = new Label();
            label4 = new Label();
            label2 = new Label();
            label3 = new Label();
            label6 = new Label();
            label7 = new Label();
            checkBox_proxyBlock = new CheckBox();
            checkBox_vpnBlock = new CheckBox();
            checkBox_torBlock = new CheckBox();
            checkBox_proxyKick = new CheckBox();
            checkBox_vpnKick = new CheckBox();
            checkBox_torKick = new CheckBox();
            checkBox_proxyNone = new CheckBox();
            checkBox_vpnNone = new CheckBox();
            checkBox_torNone = new CheckBox();
            groupBox11 = new GroupBox();
            tableLayoutPanel18 = new TableLayoutPanel();
            textBox_ProxyAPIKey = new TextBox();
            cb_enableProxyCheck = new CheckBox();
            label1 = new Label();
            num_proxyCacheDays = new NumericUpDown();
            tabNetlimiter = new TabPage();
            tableLayoutPanel23 = new TableLayoutPanel();
            panel4 = new Panel();
            groupBox16 = new GroupBox();
            tableLayoutPanel24 = new TableLayoutPanel();
            label12 = new Label();
            label11 = new Label();
            checkBox13 = new CheckBox();
            numericUpDown1 = new NumericUpDown();
            textBox2 = new TextBox();
            label8 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            tableLayoutPanel25 = new TableLayoutPanel();
            comboBox1 = new ComboBox();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            label9 = new Label();
            checkBox14 = new CheckBox();
            numericUpDown2 = new NumericUpDown();
            label10 = new Label();
            tableLayoutPanel26 = new TableLayoutPanel();
            iconButton3 = new FontAwesome.Sharp.IconButton();
            iconButton2 = new FontAwesome.Sharp.IconButton();
            groupBox17 = new GroupBox();
            toolTip1 = new ToolTip(components);
            banControls.SuspendLayout();
            tabBlacklist.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgPlayerAddressBlacklist).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgPlayerNamesBlacklist).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            blacklistForm.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            blacklist_DateBoxes.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            blacklist_IPAddress.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            blacklist_PlayerName.SuspendLayout();
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
            tabProxyChecking.SuspendLayout();
            tableLayoutPanel16.SuspendLayout();
            tableLayoutPanel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgProxyCountryBlockList).BeginInit();
            tableLayoutPanel15.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel14.SuspendLayout();
            groupBox12.SuspendLayout();
            tableLayoutPanel19.SuspendLayout();
            panel3.SuspendLayout();
            groupBox15.SuspendLayout();
            tableLayoutPanel22.SuspendLayout();
            groupBox14.SuspendLayout();
            tableLayoutPanel21.SuspendLayout();
            groupBox13.SuspendLayout();
            tableLayoutPanel20.SuspendLayout();
            groupBox11.SuspendLayout();
            tableLayoutPanel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_proxyCacheDays).BeginInit();
            tabNetlimiter.SuspendLayout();
            tableLayoutPanel23.SuspendLayout();
            panel4.SuspendLayout();
            groupBox16.SuspendLayout();
            tableLayoutPanel24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            tableLayoutPanel25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            tableLayoutPanel26.SuspendLayout();
            SuspendLayout();
            // 
            // banControls
            // 
            banControls.Controls.Add(tabBlacklist);
            banControls.Controls.Add(tabWhitelist);
            banControls.Controls.Add(tabProxyChecking);
            banControls.Controls.Add(tabNetlimiter);
            banControls.Dock = DockStyle.Fill;
            banControls.ItemSize = new Size(55, 20);
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
            tabBlacklist.Location = new Point(4, 24);
            tabBlacklist.Name = "tabBlacklist";
            tabBlacklist.Padding = new Padding(3);
            tabBlacklist.Size = new Size(958, 394);
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
            tableLayoutPanel1.Controls.Add(blacklistForm, 3, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(952, 388);
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
            tableLayoutPanel3.Controls.Add(blacklist_btnDelete, 2, 0);
            tableLayoutPanel3.Controls.Add(blacklist_btnSave, 4, 0);
            tableLayoutPanel3.Controls.Add(blacklist_btnReset, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(620, 348);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(312, 40);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // blacklist_btnDelete
            // 
            blacklist_btnDelete.Dock = DockStyle.Fill;
            blacklist_btnDelete.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            blacklist_btnDelete.IconColor = Color.Black;
            blacklist_btnDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blacklist_btnDelete.IconSize = 26;
            blacklist_btnDelete.Location = new Point(127, 3);
            blacklist_btnDelete.Name = "blacklist_btnDelete";
            blacklist_btnDelete.Padding = new Padding(0, 3, 0, 0);
            blacklist_btnDelete.Size = new Size(56, 34);
            blacklist_btnDelete.TabIndex = 9;
            toolTip1.SetToolTip(blacklist_btnDelete, "Delete Record(s)");
            blacklist_btnDelete.UseVisualStyleBackColor = true;
            blacklist_btnDelete.Click += Blacklist_Delete_Click;
            // 
            // blacklist_btnSave
            // 
            blacklist_btnSave.Dock = DockStyle.Fill;
            blacklist_btnSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            blacklist_btnSave.IconColor = Color.Black;
            blacklist_btnSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blacklist_btnSave.IconSize = 32;
            blacklist_btnSave.Location = new Point(251, 3);
            blacklist_btnSave.Name = "blacklist_btnSave";
            blacklist_btnSave.Padding = new Padding(0, 3, 0, 0);
            blacklist_btnSave.Size = new Size(58, 34);
            blacklist_btnSave.TabIndex = 8;
            toolTip1.SetToolTip(blacklist_btnSave, "Save");
            blacklist_btnSave.UseVisualStyleBackColor = true;
            blacklist_btnSave.Click += Blacklist_Save_Click;
            // 
            // blacklist_btnReset
            // 
            blacklist_btnReset.Dock = DockStyle.Fill;
            blacklist_btnReset.IconChar = FontAwesome.Sharp.IconChar.Reply;
            blacklist_btnReset.IconColor = Color.Black;
            blacklist_btnReset.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blacklist_btnReset.IconSize = 32;
            blacklist_btnReset.Location = new Point(3, 3);
            blacklist_btnReset.Name = "blacklist_btnReset";
            blacklist_btnReset.Padding = new Padding(0, 3, 0, 0);
            blacklist_btnReset.Size = new Size(56, 34);
            blacklist_btnReset.TabIndex = 10;
            toolTip1.SetToolTip(blacklist_btnReset, "Reset");
            blacklist_btnReset.UseVisualStyleBackColor = true;
            blacklist_btnReset.Click += Blacklist_Reset_Click;
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
            dgPlayerAddressBlacklist.Size = new Size(294, 382);
            dgPlayerAddressBlacklist.TabIndex = 1;
            dgPlayerAddressBlacklist.CellDoubleClick += Blacklist_IPGrid_CellDoubleClick;
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
            dgPlayerNamesBlacklist.Size = new Size(294, 382);
            dgPlayerNamesBlacklist.TabIndex = 0;
            dgPlayerNamesBlacklist.CellDoubleClick += Blacklist_NameGrid_CellDoubleClick;
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
            tableLayoutPanel2.Controls.Add(blacklist_btnClose, 4, 0);
            tableLayoutPanel2.Controls.Add(blControlRefresh, 0, 0);
            tableLayoutPanel2.Controls.Add(blControl3, 3, 0);
            tableLayoutPanel2.Controls.Add(blControl2, 2, 0);
            tableLayoutPanel2.Controls.Add(blControl1, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(620, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(312, 40);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // blacklist_btnClose
            // 
            blacklist_btnClose.Dock = DockStyle.Fill;
            blacklist_btnClose.IconChar = FontAwesome.Sharp.IconChar.Close;
            blacklist_btnClose.IconColor = Color.Black;
            blacklist_btnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blacklist_btnClose.IconSize = 32;
            blacklist_btnClose.Location = new Point(251, 3);
            blacklist_btnClose.Name = "blacklist_btnClose";
            blacklist_btnClose.Padding = new Padding(0, 3, 0, 0);
            blacklist_btnClose.Size = new Size(58, 34);
            blacklist_btnClose.TabIndex = 11;
            toolTip1.SetToolTip(blacklist_btnClose, "Close Record");
            blacklist_btnClose.UseVisualStyleBackColor = true;
            blacklist_btnClose.Click += Blacklist_Close_Click;
            // 
            // blControlRefresh
            // 
            blControlRefresh.Dock = DockStyle.Fill;
            blControlRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            blControlRefresh.IconColor = Color.Black;
            blControlRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            blControlRefresh.IconSize = 32;
            blControlRefresh.Location = new Point(3, 3);
            blControlRefresh.Name = "blControlRefresh";
            blControlRefresh.Padding = new Padding(4, 3, 0, 0);
            blControlRefresh.Size = new Size(56, 34);
            blControlRefresh.TabIndex = 0;
            toolTip1.SetToolTip(blControlRefresh, "Blacklist Refresh");
            blControlRefresh.UseVisualStyleBackColor = true;
            blControlRefresh.Click += Blacklist_Refresh_Click;
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
            blControl3.TabIndex = 0;
            toolTip1.SetToolTip(blControl3, "New Record Both");
            blControl3.UseVisualStyleBackColor = true;
            blControl3.Click += Blacklist_AddBoth_Click;
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
            blControl2.TabIndex = 0;
            toolTip1.SetToolTip(blControl2, "New Address Record");
            blControl2.UseVisualStyleBackColor = true;
            blControl2.Click += Blacklist_AddIPOnly_Click;
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
            blControl1.Click += Blacklist_AddNameOnly_Click;
            // 
            // blacklistForm
            // 
            blacklistForm.Controls.Add(groupBox5);
            blacklistForm.Controls.Add(groupBox4);
            blacklistForm.Controls.Add(blacklist_DateBoxes);
            blacklistForm.Controls.Add(blacklist_IPAddress);
            blacklistForm.Controls.Add(blacklist_PlayerName);
            blacklistForm.Dock = DockStyle.Fill;
            blacklistForm.Location = new Point(620, 40);
            blacklistForm.Margin = new Padding(0);
            blacklistForm.Name = "blacklistForm";
            blacklistForm.Size = new Size(312, 308);
            blacklistForm.TabIndex = 4;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(blacklist_notes);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(0, 195);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(6, 3, 6, 3);
            groupBox5.Size = new Size(312, 113);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Notes";
            // 
            // blacklist_notes
            // 
            blacklist_notes.Dock = DockStyle.Fill;
            blacklist_notes.Location = new Point(6, 19);
            blacklist_notes.Multiline = true;
            blacklist_notes.Name = "blacklist_notes";
            blacklist_notes.PlaceholderText = "Notes";
            blacklist_notes.Size = new Size(300, 91);
            blacklist_notes.TabIndex = 7;
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
            tableLayoutPanel6.Controls.Add(blacklist_TempBan, 0, 0);
            tableLayoutPanel6.Controls.Add(blacklist_PermBan, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle());
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(306, 22);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // blacklist_TempBan
            // 
            blacklist_TempBan.AutoSize = true;
            blacklist_TempBan.Dock = DockStyle.Fill;
            blacklist_TempBan.Location = new Point(3, 3);
            blacklist_TempBan.Name = "blacklist_TempBan";
            blacklist_TempBan.Padding = new Padding(30, 0, 20, 0);
            blacklist_TempBan.Size = new Size(147, 19);
            blacklist_TempBan.TabIndex = 5;
            blacklist_TempBan.Text = "Temporary";
            blacklist_TempBan.TextAlign = ContentAlignment.MiddleCenter;
            blacklist_TempBan.UseVisualStyleBackColor = true;
            blacklist_TempBan.Click += Blacklist_BanTypeToggle_Click;
            // 
            // blacklist_PermBan
            // 
            blacklist_PermBan.AutoSize = true;
            blacklist_PermBan.Dock = DockStyle.Fill;
            blacklist_PermBan.Location = new Point(156, 3);
            blacklist_PermBan.Name = "blacklist_PermBan";
            blacklist_PermBan.Padding = new Padding(30, 0, 20, 0);
            blacklist_PermBan.Size = new Size(147, 19);
            blacklist_PermBan.TabIndex = 6;
            blacklist_PermBan.Text = "Permanent";
            blacklist_PermBan.TextAlign = ContentAlignment.MiddleCenter;
            blacklist_PermBan.UseVisualStyleBackColor = true;
            blacklist_PermBan.Click += Blacklist_BanTypeToggle_Click;
            // 
            // blacklist_DateBoxes
            // 
            blacklist_DateBoxes.Controls.Add(tableLayoutPanel5);
            blacklist_DateBoxes.Dock = DockStyle.Top;
            blacklist_DateBoxes.Location = new Point(0, 100);
            blacklist_DateBoxes.Name = "blacklist_DateBoxes";
            blacklist_DateBoxes.Size = new Size(312, 51);
            blacklist_DateBoxes.TabIndex = 2;
            blacklist_DateBoxes.TabStop = false;
            blacklist_DateBoxes.Text = "Ban Dates - Start && End";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(blacklist_DateStart, 0, 0);
            tableLayoutPanel5.Controls.Add(blacklist_DateEnd, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(306, 29);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // blacklist_DateStart
            // 
            blacklist_DateStart.Location = new Point(3, 3);
            blacklist_DateStart.MaxDate = new DateTime(3026, 1, 18, 0, 0, 0, 0);
            blacklist_DateStart.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            blacklist_DateStart.Name = "blacklist_DateStart";
            blacklist_DateStart.Size = new Size(147, 23);
            blacklist_DateStart.TabIndex = 3;
            blacklist_DateStart.Value = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            // 
            // blacklist_DateEnd
            // 
            blacklist_DateEnd.Checked = false;
            blacklist_DateEnd.Location = new Point(156, 3);
            blacklist_DateEnd.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            blacklist_DateEnd.Name = "blacklist_DateEnd";
            blacklist_DateEnd.Size = new Size(147, 23);
            blacklist_DateEnd.TabIndex = 4;
            blacklist_DateEnd.Value = new DateTime(2026, 1, 18, 15, 43, 49, 0);
            // 
            // blacklist_IPAddress
            // 
            blacklist_IPAddress.Controls.Add(tableLayoutPanel4);
            blacklist_IPAddress.Dock = DockStyle.Top;
            blacklist_IPAddress.Location = new Point(0, 49);
            blacklist_IPAddress.Name = "blacklist_IPAddress";
            blacklist_IPAddress.Size = new Size(312, 51);
            blacklist_IPAddress.TabIndex = 1;
            blacklist_IPAddress.TabStop = false;
            blacklist_IPAddress.Text = "IP Address";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.47059F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.5294113F));
            tableLayoutPanel4.Controls.Add(blacklist_IPSubnetTxt, 1, 0);
            tableLayoutPanel4.Controls.Add(blacklist_IPAddressTxt, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(306, 29);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // blacklist_IPSubnetTxt
            // 
            blacklist_IPSubnetTxt.DisplayMember = "32";
            blacklist_IPSubnetTxt.FormattingEnabled = true;
            blacklist_IPSubnetTxt.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" });
            blacklist_IPSubnetTxt.Location = new Point(237, 3);
            blacklist_IPSubnetTxt.Name = "blacklist_IPSubnetTxt";
            blacklist_IPSubnetTxt.Size = new Size(66, 23);
            blacklist_IPSubnetTxt.TabIndex = 2;
            blacklist_IPSubnetTxt.ValueMember = "32";
            // 
            // blacklist_IPAddressTxt
            // 
            blacklist_IPAddressTxt.Dock = DockStyle.Fill;
            blacklist_IPAddressTxt.Location = new Point(3, 3);
            blacklist_IPAddressTxt.MaxLength = 15;
            blacklist_IPAddressTxt.Name = "blacklist_IPAddressTxt";
            blacklist_IPAddressTxt.PlaceholderText = "000.000.000.000";
            blacklist_IPAddressTxt.Size = new Size(228, 23);
            blacklist_IPAddressTxt.TabIndex = 1;
            blacklist_IPAddressTxt.TextAlign = HorizontalAlignment.Center;
            // 
            // blacklist_PlayerName
            // 
            blacklist_PlayerName.Controls.Add(blacklist_PlayerNameTxt);
            blacklist_PlayerName.Dock = DockStyle.Top;
            blacklist_PlayerName.Location = new Point(0, 0);
            blacklist_PlayerName.Name = "blacklist_PlayerName";
            blacklist_PlayerName.Padding = new Padding(6, 3, 6, 3);
            blacklist_PlayerName.Size = new Size(312, 49);
            blacklist_PlayerName.TabIndex = 0;
            blacklist_PlayerName.TabStop = false;
            blacklist_PlayerName.Text = "Player Name";
            // 
            // blacklist_PlayerNameTxt
            // 
            blacklist_PlayerNameTxt.Dock = DockStyle.Fill;
            blacklist_PlayerNameTxt.Location = new Point(6, 19);
            blacklist_PlayerNameTxt.MaxLength = 16;
            blacklist_PlayerNameTxt.Name = "blacklist_PlayerNameTxt";
            blacklist_PlayerNameTxt.PlaceholderText = "Player Name";
            blacklist_PlayerNameTxt.Size = new Size(300, 23);
            blacklist_PlayerNameTxt.TabIndex = 0;
            blacklist_PlayerNameTxt.TextAlign = HorizontalAlignment.Center;
            // 
            // tabWhitelist
            // 
            tabWhitelist.Controls.Add(tableLayoutPanel7);
            tabWhitelist.Location = new Point(4, 24);
            tabWhitelist.Name = "tabWhitelist";
            tabWhitelist.Padding = new Padding(3);
            tabWhitelist.Size = new Size(958, 394);
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
            tableLayoutPanel7.Size = new Size(952, 388);
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
            tableLayoutPanel8.Controls.Add(wlControlDelete, 2, 0);
            tableLayoutPanel8.Controls.Add(wlControlSave, 4, 0);
            tableLayoutPanel8.Controls.Add(wlControlReset, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(620, 348);
            tableLayoutPanel8.Margin = new Padding(0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle());
            tableLayoutPanel8.Size = new Size(312, 40);
            tableLayoutPanel8.TabIndex = 3;
            // 
            // wlControlDelete
            // 
            wlControlDelete.Dock = DockStyle.Fill;
            wlControlDelete.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            wlControlDelete.IconColor = Color.Black;
            wlControlDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlDelete.IconSize = 26;
            wlControlDelete.Location = new Point(127, 3);
            wlControlDelete.Name = "wlControlDelete";
            wlControlDelete.Padding = new Padding(0, 3, 0, 0);
            wlControlDelete.Size = new Size(56, 34);
            wlControlDelete.TabIndex = 10;
            toolTip1.SetToolTip(wlControlDelete, "Delete Record(s)");
            wlControlDelete.UseVisualStyleBackColor = true;
            wlControlDelete.Click += Whitelist_Delete_Click;
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
            wlControlSave.TabIndex = 9;
            toolTip1.SetToolTip(wlControlSave, "Save");
            wlControlSave.UseVisualStyleBackColor = true;
            wlControlSave.Click += Whitelist_Save_Click;
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
            wlControlReset.TabIndex = 11;
            toolTip1.SetToolTip(wlControlReset, "Reset");
            wlControlReset.UseVisualStyleBackColor = true;
            wlControlReset.Click += Whitelist_Reset_Click;
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
            dgPlayerAddressWhitelist.Size = new Size(294, 382);
            dgPlayerAddressWhitelist.TabIndex = 1;
            dgPlayerAddressWhitelist.CellDoubleClick += Whitelist_IPGrid_CellDoubleClick;
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
            dgPlayerNamesWhitelist.Size = new Size(294, 382);
            dgPlayerNamesWhitelist.TabIndex = 0;
            dgPlayerNamesWhitelist.CellDoubleClick += Whitelist_NameGrid_CellDoubleClick;
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
            tableLayoutPanel9.Controls.Add(wlControlClose, 4, 0);
            tableLayoutPanel9.Controls.Add(wlControlRefresh, 0, 0);
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
            // wlControlClose
            // 
            wlControlClose.Dock = DockStyle.Fill;
            wlControlClose.IconChar = FontAwesome.Sharp.IconChar.Close;
            wlControlClose.IconColor = Color.Black;
            wlControlClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlClose.IconSize = 32;
            wlControlClose.Location = new Point(251, 3);
            wlControlClose.Name = "wlControlClose";
            wlControlClose.Padding = new Padding(0, 3, 0, 0);
            wlControlClose.Size = new Size(58, 34);
            wlControlClose.TabIndex = 12;
            toolTip1.SetToolTip(wlControlClose, "Close Record");
            wlControlClose.UseVisualStyleBackColor = true;
            wlControlClose.Click += Whitelist_Close_Click;
            // 
            // wlControlRefresh
            // 
            wlControlRefresh.Dock = DockStyle.Fill;
            wlControlRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            wlControlRefresh.IconColor = Color.Black;
            wlControlRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            wlControlRefresh.IconSize = 32;
            wlControlRefresh.Location = new Point(3, 3);
            wlControlRefresh.Name = "wlControlRefresh";
            wlControlRefresh.Padding = new Padding(4, 3, 0, 0);
            wlControlRefresh.Size = new Size(56, 34);
            wlControlRefresh.TabIndex = 0;
            toolTip1.SetToolTip(wlControlRefresh, "Whitelist Refresh");
            wlControlRefresh.UseVisualStyleBackColor = true;
            wlControlRefresh.Click += Whitelist_Refresh_Click;
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
            wlControl3.TabIndex = 0;
            toolTip1.SetToolTip(wlControl3, "New Record Both");
            wlControl3.UseVisualStyleBackColor = true;
            wlControl3.Click += Whitelist_AddBoth_Click;
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
            wlControl2.TabIndex = 0;
            toolTip1.SetToolTip(wlControl2, "New Address Record");
            wlControl2.UseVisualStyleBackColor = true;
            wlControl2.Click += Whitelist_AddIPOnly_Click;
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
            wlControl1.Click += Whitelist_AddNameOnly_Click;
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
            panel2.Size = new Size(312, 308);
            panel2.TabIndex = 4;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(textBox_notesWL);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(0, 195);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(6, 3, 6, 3);
            groupBox6.Size = new Size(312, 113);
            groupBox6.TabIndex = 4;
            groupBox6.TabStop = false;
            groupBox6.Text = "Notes";
            // 
            // textBox_notesWL
            // 
            textBox_notesWL.Dock = DockStyle.Fill;
            textBox_notesWL.Location = new Point(6, 19);
            textBox_notesWL.Multiline = true;
            textBox_notesWL.Name = "textBox_notesWL";
            textBox_notesWL.PlaceholderText = "Notes";
            textBox_notesWL.Size = new Size(300, 91);
            textBox_notesWL.TabIndex = 8;
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
            checkBox_tempWL.TabIndex = 6;
            checkBox_tempWL.Text = "Temporary";
            checkBox_tempWL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_tempWL.UseVisualStyleBackColor = true;
            checkBox_tempWL.Click += Whitelist_ExemptTypeToggle_Click;
            // 
            // checkBox_permWL
            // 
            checkBox_permWL.AutoSize = true;
            checkBox_permWL.Dock = DockStyle.Fill;
            checkBox_permWL.Location = new Point(156, 3);
            checkBox_permWL.Name = "checkBox_permWL";
            checkBox_permWL.Padding = new Padding(30, 0, 20, 0);
            checkBox_permWL.Size = new Size(147, 19);
            checkBox_permWL.TabIndex = 7;
            checkBox_permWL.Text = "Permanent";
            checkBox_permWL.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_permWL.UseVisualStyleBackColor = true;
            checkBox_permWL.Click += Whitelist_ExemptTypeToggle_Click;
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
            dateTimePicker_WLstart.TabIndex = 4;
            dateTimePicker_WLstart.Value = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            // 
            // dateTimePicker_WLend
            // 
            dateTimePicker_WLend.Checked = false;
            dateTimePicker_WLend.Location = new Point(156, 3);
            dateTimePicker_WLend.MinDate = new DateTime(2026, 1, 18, 0, 0, 0, 0);
            dateTimePicker_WLend.Name = "dateTimePicker_WLend";
            dateTimePicker_WLend.Size = new Size(147, 23);
            dateTimePicker_WLend.TabIndex = 5;
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
            cb_subnetWL.TabIndex = 3;
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
            textBox_addressWL.TabIndex = 2;
            textBox_addressWL.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(textBox_playerNameWL);
            groupBox10.Dock = DockStyle.Top;
            groupBox10.Location = new Point(0, 0);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new Padding(6, 3, 6, 3);
            groupBox10.Size = new Size(312, 49);
            groupBox10.TabIndex = 0;
            groupBox10.TabStop = false;
            groupBox10.Text = "Player Name";
            // 
            // textBox_playerNameWL
            // 
            textBox_playerNameWL.Dock = DockStyle.Fill;
            textBox_playerNameWL.Location = new Point(6, 19);
            textBox_playerNameWL.MaxLength = 16;
            textBox_playerNameWL.Name = "textBox_playerNameWL";
            textBox_playerNameWL.PlaceholderText = "Player Name";
            textBox_playerNameWL.Size = new Size(300, 23);
            textBox_playerNameWL.TabIndex = 1;
            textBox_playerNameWL.TextAlign = HorizontalAlignment.Center;
            // 
            // tabProxyChecking
            // 
            tabProxyChecking.Controls.Add(tableLayoutPanel16);
            tabProxyChecking.Location = new Point(4, 24);
            tabProxyChecking.Name = "tabProxyChecking";
            tabProxyChecking.Padding = new Padding(3);
            tabProxyChecking.Size = new Size(958, 394);
            tabProxyChecking.TabIndex = 2;
            tabProxyChecking.Text = "Proxy Checking";
            tabProxyChecking.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel16
            // 
            tableLayoutPanel16.ColumnCount = 3;
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel16.Controls.Add(tableLayoutPanel17, 0, 0);
            tableLayoutPanel16.Controls.Add(tabControl1, 2, 0);
            tableLayoutPanel16.Dock = DockStyle.Fill;
            tableLayoutPanel16.Location = new Point(3, 3);
            tableLayoutPanel16.Name = "tableLayoutPanel16";
            tableLayoutPanel16.RowCount = 1;
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel16.Size = new Size(952, 388);
            tableLayoutPanel16.TabIndex = 1;
            // 
            // tableLayoutPanel17
            // 
            tableLayoutPanel17.ColumnCount = 1;
            tableLayoutPanel17.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel17.Controls.Add(dgProxyCountryBlockList, 0, 0);
            tableLayoutPanel17.Controls.Add(tableLayoutPanel15, 0, 1);
            tableLayoutPanel17.Dock = DockStyle.Fill;
            tableLayoutPanel17.Location = new Point(0, 0);
            tableLayoutPanel17.Margin = new Padding(0);
            tableLayoutPanel17.Name = "tableLayoutPanel17";
            tableLayoutPanel17.RowCount = 1;
            tableLayoutPanel17.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel17.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel17.Size = new Size(250, 388);
            tableLayoutPanel17.TabIndex = 0;
            // 
            // dgProxyCountryBlockList
            // 
            dgProxyCountryBlockList.AllowUserToAddRows = false;
            dgProxyCountryBlockList.AllowUserToDeleteRows = false;
            dgProxyCountryBlockList.AllowUserToResizeColumns = false;
            dgProxyCountryBlockList.AllowUserToResizeRows = false;
            dgProxyCountryBlockList.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgProxyCountryBlockList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgProxyCountryBlockList.Columns.AddRange(new DataGridViewColumn[] { geo_recordID, proxy_countyCode, proxy_countyName });
            dgProxyCountryBlockList.Dock = DockStyle.Fill;
            dgProxyCountryBlockList.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgProxyCountryBlockList.Location = new Point(3, 3);
            dgProxyCountryBlockList.Name = "dgProxyCountryBlockList";
            dgProxyCountryBlockList.ReadOnly = true;
            dgProxyCountryBlockList.RowHeadersVisible = false;
            dgProxyCountryBlockList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgProxyCountryBlockList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgProxyCountryBlockList.Size = new Size(244, 352);
            dgProxyCountryBlockList.TabIndex = 5;
            dgProxyCountryBlockList.CellDoubleClick += ProxyCheck_CountryGrid_CellDoubleClick;
            // 
            // geo_recordID
            // 
            geo_recordID.HeaderText = "Record ID";
            geo_recordID.Name = "geo_recordID";
            geo_recordID.ReadOnly = true;
            geo_recordID.Visible = false;
            // 
            // proxy_countyCode
            // 
            proxy_countyCode.HeaderText = "Code";
            proxy_countyCode.Name = "proxy_countyCode";
            proxy_countyCode.ReadOnly = true;
            proxy_countyCode.Width = 50;
            // 
            // proxy_countyName
            // 
            proxy_countyName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            proxy_countyName.HeaderText = "Country Name";
            proxy_countyName.Name = "proxy_countyName";
            proxy_countyName.ReadOnly = true;
            // 
            // tableLayoutPanel15
            // 
            tableLayoutPanel15.ColumnCount = 3;
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 48F));
            tableLayoutPanel15.Controls.Add(textBox_countryCode, 0, 0);
            tableLayoutPanel15.Controls.Add(textBox_countryName, 1, 0);
            tableLayoutPanel15.Controls.Add(btn_proxyAddCountry, 2, 0);
            tableLayoutPanel15.Dock = DockStyle.Fill;
            tableLayoutPanel15.Location = new Point(0, 358);
            tableLayoutPanel15.Margin = new Padding(0);
            tableLayoutPanel15.Name = "tableLayoutPanel15";
            tableLayoutPanel15.RowCount = 1;
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel15.Size = new Size(250, 30);
            tableLayoutPanel15.TabIndex = 3;
            // 
            // textBox_countryCode
            // 
            textBox_countryCode.Anchor = AnchorStyles.None;
            textBox_countryCode.Location = new Point(3, 3);
            textBox_countryCode.MaxLength = 2;
            textBox_countryCode.Name = "textBox_countryCode";
            textBox_countryCode.PlaceholderText = "US";
            textBox_countryCode.Size = new Size(39, 23);
            textBox_countryCode.TabIndex = 0;
            textBox_countryCode.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox_countryName
            // 
            textBox_countryName.Anchor = AnchorStyles.None;
            textBox_countryName.Location = new Point(48, 3);
            textBox_countryName.Name = "textBox_countryName";
            textBox_countryName.PlaceholderText = "Country Name";
            textBox_countryName.Size = new Size(151, 23);
            textBox_countryName.TabIndex = 1;
            textBox_countryName.TextAlign = HorizontalAlignment.Center;
            // 
            // btn_proxyAddCountry
            // 
            btn_proxyAddCountry.Anchor = AnchorStyles.None;
            btn_proxyAddCountry.FlatStyle = FlatStyle.Flat;
            btn_proxyAddCountry.Location = new Point(205, 3);
            btn_proxyAddCountry.Name = "btn_proxyAddCountry";
            btn_proxyAddCountry.Size = new Size(42, 23);
            btn_proxyAddCountry.TabIndex = 2;
            btn_proxyAddCountry.Text = "Add";
            btn_proxyAddCountry.UseVisualStyleBackColor = true;
            btn_proxyAddCountry.Click += ProxyCheck_AddCountry_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(proxyLookup);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(263, 3);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(686, 382);
            tabControl1.TabIndex = 1;
            // 
            // proxyLookup
            // 
            proxyLookup.Location = new Point(4, 24);
            proxyLookup.Name = "proxyLookup";
            proxyLookup.Padding = new Padding(3);
            proxyLookup.Size = new Size(678, 354);
            proxyLookup.TabIndex = 0;
            proxyLookup.Text = "IP Address Lookup";
            proxyLookup.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel14);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(678, 354);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Settings";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel14
            // 
            tableLayoutPanel14.ColumnCount = 5;
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel14.Controls.Add(groupBox12, 2, 0);
            tableLayoutPanel14.Controls.Add(panel3, 0, 0);
            tableLayoutPanel14.Dock = DockStyle.Fill;
            tableLayoutPanel14.Location = new Point(3, 3);
            tableLayoutPanel14.Margin = new Padding(0);
            tableLayoutPanel14.Name = "tableLayoutPanel14";
            tableLayoutPanel14.RowCount = 1;
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel14.Size = new Size(672, 348);
            tableLayoutPanel14.TabIndex = 0;
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(tableLayoutPanel19);
            groupBox12.Dock = DockStyle.Fill;
            groupBox12.Location = new Point(273, 3);
            groupBox12.Name = "groupBox12";
            groupBox12.Padding = new Padding(0);
            groupBox12.Size = new Size(194, 342);
            groupBox12.TabIndex = 1;
            groupBox12.TabStop = false;
            groupBox12.Text = "Service Providers";
            // 
            // tableLayoutPanel19
            // 
            tableLayoutPanel19.ColumnCount = 1;
            tableLayoutPanel19.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel19.Controls.Add(cb_serviceIP2LocationIO, 0, 1);
            tableLayoutPanel19.Controls.Add(cb_serviceProxyCheckIO, 0, 0);
            tableLayoutPanel19.Dock = DockStyle.Fill;
            tableLayoutPanel19.Location = new Point(0, 16);
            tableLayoutPanel19.Name = "tableLayoutPanel19";
            tableLayoutPanel19.RowCount = 9;
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel19.Size = new Size(194, 326);
            tableLayoutPanel19.TabIndex = 0;
            // 
            // cb_serviceIP2LocationIO
            // 
            cb_serviceIP2LocationIO.AutoSize = true;
            cb_serviceIP2LocationIO.Dock = DockStyle.Fill;
            cb_serviceIP2LocationIO.Location = new Point(3, 33);
            cb_serviceIP2LocationIO.Name = "cb_serviceIP2LocationIO";
            cb_serviceIP2LocationIO.Padding = new Padding(15, 3, 10, 0);
            cb_serviceIP2LocationIO.Size = new Size(188, 24);
            cb_serviceIP2LocationIO.TabIndex = 1;
            cb_serviceIP2LocationIO.Text = "IP2Location.io";
            cb_serviceIP2LocationIO.TextAlign = ContentAlignment.MiddleRight;
            cb_serviceIP2LocationIO.UseVisualStyleBackColor = true;
            cb_serviceIP2LocationIO.Click += ProxyCheck_CBServicesChanged;
            // 
            // cb_serviceProxyCheckIO
            // 
            cb_serviceProxyCheckIO.AutoSize = true;
            cb_serviceProxyCheckIO.Dock = DockStyle.Fill;
            cb_serviceProxyCheckIO.Location = new Point(3, 3);
            cb_serviceProxyCheckIO.Name = "cb_serviceProxyCheckIO";
            cb_serviceProxyCheckIO.Padding = new Padding(15, 3, 10, 0);
            cb_serviceProxyCheckIO.Size = new Size(188, 24);
            cb_serviceProxyCheckIO.TabIndex = 0;
            cb_serviceProxyCheckIO.Text = "ProxyCheck.io";
            cb_serviceProxyCheckIO.TextAlign = ContentAlignment.MiddleRight;
            cb_serviceProxyCheckIO.UseVisualStyleBackColor = true;
            cb_serviceProxyCheckIO.Click += ProxyCheck_CBServicesChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(groupBox15);
            panel3.Controls.Add(groupBox14);
            panel3.Controls.Add(groupBox13);
            panel3.Controls.Add(groupBox11);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(250, 348);
            panel3.TabIndex = 2;
            // 
            // groupBox15
            // 
            groupBox15.Controls.Add(tableLayoutPanel22);
            groupBox15.Dock = DockStyle.Fill;
            groupBox15.Location = new Point(0, 232);
            groupBox15.Name = "groupBox15";
            groupBox15.Size = new Size(250, 54);
            groupBox15.TabIndex = 3;
            groupBox15.TabStop = false;
            groupBox15.Text = "Geo Blocking Mode";
            // 
            // tableLayoutPanel22
            // 
            tableLayoutPanel22.ColumnCount = 3;
            tableLayoutPanel22.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel22.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel22.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel22.Controls.Add(checkBox_GeoAllow, 0, 0);
            tableLayoutPanel22.Controls.Add(checkBox_GeoBlock, 1, 0);
            tableLayoutPanel22.Controls.Add(checkBox_GeoOff, 2, 0);
            tableLayoutPanel22.Dock = DockStyle.Fill;
            tableLayoutPanel22.Location = new Point(3, 19);
            tableLayoutPanel22.Name = "tableLayoutPanel22";
            tableLayoutPanel22.RowCount = 1;
            tableLayoutPanel22.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel22.Size = new Size(244, 32);
            tableLayoutPanel22.TabIndex = 0;
            // 
            // checkBox_GeoAllow
            // 
            checkBox_GeoAllow.AutoSize = true;
            checkBox_GeoAllow.Dock = DockStyle.Fill;
            checkBox_GeoAllow.Location = new Point(3, 3);
            checkBox_GeoAllow.Name = "checkBox_GeoAllow";
            checkBox_GeoAllow.Padding = new Padding(5, 3, 5, 0);
            checkBox_GeoAllow.Size = new Size(84, 26);
            checkBox_GeoAllow.TabIndex = 0;
            checkBox_GeoAllow.Text = "Allow";
            checkBox_GeoAllow.TextAlign = ContentAlignment.MiddleRight;
            checkBox_GeoAllow.UseVisualStyleBackColor = true;
            checkBox_GeoAllow.Click += ProxyCheck_CBGEOChange;
            // 
            // checkBox_GeoBlock
            // 
            checkBox_GeoBlock.AutoSize = true;
            checkBox_GeoBlock.Dock = DockStyle.Fill;
            checkBox_GeoBlock.Location = new Point(93, 3);
            checkBox_GeoBlock.Name = "checkBox_GeoBlock";
            checkBox_GeoBlock.Padding = new Padding(5, 3, 5, 0);
            checkBox_GeoBlock.Size = new Size(74, 26);
            checkBox_GeoBlock.TabIndex = 1;
            checkBox_GeoBlock.Text = "Block";
            checkBox_GeoBlock.TextAlign = ContentAlignment.MiddleRight;
            checkBox_GeoBlock.UseVisualStyleBackColor = true;
            checkBox_GeoBlock.Click += ProxyCheck_CBGEOChange;
            // 
            // checkBox_GeoOff
            // 
            checkBox_GeoOff.AutoSize = true;
            checkBox_GeoOff.Checked = true;
            checkBox_GeoOff.CheckState = CheckState.Checked;
            checkBox_GeoOff.Dock = DockStyle.Fill;
            checkBox_GeoOff.Location = new Point(173, 3);
            checkBox_GeoOff.Name = "checkBox_GeoOff";
            checkBox_GeoOff.Padding = new Padding(5, 3, 5, 0);
            checkBox_GeoOff.Size = new Size(68, 26);
            checkBox_GeoOff.TabIndex = 2;
            checkBox_GeoOff.Text = "Off";
            checkBox_GeoOff.TextAlign = ContentAlignment.MiddleRight;
            checkBox_GeoOff.UseVisualStyleBackColor = true;
            checkBox_GeoOff.Click += ProxyCheck_CBGEOChange;
            // 
            // groupBox14
            // 
            groupBox14.Controls.Add(tableLayoutPanel21);
            groupBox14.Dock = DockStyle.Bottom;
            groupBox14.Location = new Point(0, 286);
            groupBox14.Name = "groupBox14";
            groupBox14.Size = new Size(250, 62);
            groupBox14.TabIndex = 2;
            groupBox14.TabStop = false;
            groupBox14.Text = "Controls";
            // 
            // tableLayoutPanel21
            // 
            tableLayoutPanel21.ColumnCount = 5;
            tableLayoutPanel21.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel21.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel21.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel21.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel21.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel21.Controls.Add(btn_proxyTest, 4, 0);
            tableLayoutPanel21.Controls.Add(btn_proxyReset, 0, 0);
            tableLayoutPanel21.Controls.Add(btn_proxySave, 2, 0);
            tableLayoutPanel21.Dock = DockStyle.Fill;
            tableLayoutPanel21.Location = new Point(3, 19);
            tableLayoutPanel21.Name = "tableLayoutPanel21";
            tableLayoutPanel21.RowCount = 1;
            tableLayoutPanel21.RowStyles.Add(new RowStyle());
            tableLayoutPanel21.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel21.Size = new Size(244, 40);
            tableLayoutPanel21.TabIndex = 0;
            // 
            // btn_proxyTest
            // 
            btn_proxyTest.Dock = DockStyle.Fill;
            btn_proxyTest.IconChar = FontAwesome.Sharp.IconChar.SatelliteDish;
            btn_proxyTest.IconColor = Color.Black;
            btn_proxyTest.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btn_proxyTest.IconSize = 28;
            btn_proxyTest.Location = new Point(195, 3);
            btn_proxyTest.Name = "btn_proxyTest";
            btn_proxyTest.Padding = new Padding(0, 3, 0, 0);
            btn_proxyTest.Size = new Size(46, 34);
            btn_proxyTest.TabIndex = 2;
            toolTip1.SetToolTip(btn_proxyTest, "Validate and Test");
            btn_proxyTest.UseVisualStyleBackColor = true;
            btn_proxyTest.Click += ProxyCheck_TestService_Click;
            // 
            // btn_proxyReset
            // 
            btn_proxyReset.Dock = DockStyle.Fill;
            btn_proxyReset.IconChar = FontAwesome.Sharp.IconChar.Reply;
            btn_proxyReset.IconColor = Color.Black;
            btn_proxyReset.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btn_proxyReset.IconSize = 28;
            btn_proxyReset.Location = new Point(3, 3);
            btn_proxyReset.Name = "btn_proxyReset";
            btn_proxyReset.Padding = new Padding(0, 3, 0, 0);
            btn_proxyReset.Size = new Size(42, 34);
            btn_proxyReset.TabIndex = 1;
            toolTip1.SetToolTip(btn_proxyReset, "Reset");
            btn_proxyReset.UseVisualStyleBackColor = true;
            btn_proxyReset.Click += ProxyCheck_LoadSettings;
            // 
            // btn_proxySave
            // 
            btn_proxySave.Dock = DockStyle.Fill;
            btn_proxySave.IconChar = FontAwesome.Sharp.IconChar.Save;
            btn_proxySave.IconColor = Color.Black;
            btn_proxySave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btn_proxySave.IconSize = 28;
            btn_proxySave.Location = new Point(99, 3);
            btn_proxySave.Name = "btn_proxySave";
            btn_proxySave.Padding = new Padding(0, 3, 0, 0);
            btn_proxySave.Size = new Size(42, 34);
            btn_proxySave.TabIndex = 0;
            toolTip1.SetToolTip(btn_proxySave, "Save");
            btn_proxySave.UseVisualStyleBackColor = true;
            btn_proxySave.Click += ProxyCheck_SaveSettings;
            // 
            // groupBox13
            // 
            groupBox13.Controls.Add(tableLayoutPanel20);
            groupBox13.Dock = DockStyle.Top;
            groupBox13.Location = new Point(0, 108);
            groupBox13.Name = "groupBox13";
            groupBox13.Size = new Size(250, 124);
            groupBox13.TabIndex = 1;
            groupBox13.TabStop = false;
            groupBox13.Text = "Detection Default Actions";
            // 
            // tableLayoutPanel20
            // 
            tableLayoutPanel20.ColumnCount = 4;
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel20.Controls.Add(label5, 3, 0);
            tableLayoutPanel20.Controls.Add(label4, 2, 0);
            tableLayoutPanel20.Controls.Add(label2, 1, 0);
            tableLayoutPanel20.Controls.Add(label3, 0, 1);
            tableLayoutPanel20.Controls.Add(label6, 0, 2);
            tableLayoutPanel20.Controls.Add(label7, 0, 3);
            tableLayoutPanel20.Controls.Add(checkBox_proxyBlock, 1, 1);
            tableLayoutPanel20.Controls.Add(checkBox_vpnBlock, 2, 1);
            tableLayoutPanel20.Controls.Add(checkBox_torBlock, 3, 1);
            tableLayoutPanel20.Controls.Add(checkBox_proxyKick, 1, 2);
            tableLayoutPanel20.Controls.Add(checkBox_vpnKick, 2, 2);
            tableLayoutPanel20.Controls.Add(checkBox_torKick, 3, 2);
            tableLayoutPanel20.Controls.Add(checkBox_proxyNone, 1, 3);
            tableLayoutPanel20.Controls.Add(checkBox_vpnNone, 2, 3);
            tableLayoutPanel20.Controls.Add(checkBox_torNone, 3, 3);
            tableLayoutPanel20.Dock = DockStyle.Fill;
            tableLayoutPanel20.Location = new Point(3, 19);
            tableLayoutPanel20.Name = "tableLayoutPanel20";
            tableLayoutPanel20.RowCount = 4;
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 29.5774651F));
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 23.4741783F));
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 23.4741783F));
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 23.4741783F));
            tableLayoutPanel20.Size = new Size(244, 102);
            tableLayoutPanel20.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(196, 0);
            label5.Name = "label5";
            label5.Size = new Size(45, 30);
            label5.TabIndex = 3;
            label5.Text = "TOR";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(148, 0);
            label4.Name = "label4";
            label4.Size = new Size(42, 30);
            label4.TabIndex = 2;
            label4.Text = "VPN";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(100, 0);
            label2.Name = "label2";
            label2.Size = new Size(42, 30);
            label2.TabIndex = 0;
            label2.Text = "Proxy";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 30);
            label3.Name = "label3";
            label3.Size = new Size(91, 23);
            label3.TabIndex = 4;
            label3.Text = "Block";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 53);
            label6.Name = "label6";
            label6.Size = new Size(91, 23);
            label6.TabIndex = 5;
            label6.Text = "Kick";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(3, 76);
            label7.Name = "label7";
            label7.Size = new Size(91, 26);
            label7.TabIndex = 6;
            label7.Text = "Nothing";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // checkBox_proxyBlock
            // 
            checkBox_proxyBlock.AutoSize = true;
            checkBox_proxyBlock.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_proxyBlock.Dock = DockStyle.Fill;
            checkBox_proxyBlock.Location = new Point(100, 33);
            checkBox_proxyBlock.Name = "checkBox_proxyBlock";
            checkBox_proxyBlock.Size = new Size(42, 17);
            checkBox_proxyBlock.TabIndex = 7;
            checkBox_proxyBlock.UseVisualStyleBackColor = true;
            checkBox_proxyBlock.Click += ProxyCheck_CBProxyChange;
            // 
            // checkBox_vpnBlock
            // 
            checkBox_vpnBlock.AutoSize = true;
            checkBox_vpnBlock.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_vpnBlock.Dock = DockStyle.Fill;
            checkBox_vpnBlock.Location = new Point(148, 33);
            checkBox_vpnBlock.Name = "checkBox_vpnBlock";
            checkBox_vpnBlock.Size = new Size(42, 17);
            checkBox_vpnBlock.TabIndex = 8;
            checkBox_vpnBlock.UseVisualStyleBackColor = true;
            checkBox_vpnBlock.Click += ProxyCheck_CBVPNChange;
            // 
            // checkBox_torBlock
            // 
            checkBox_torBlock.AutoSize = true;
            checkBox_torBlock.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_torBlock.Dock = DockStyle.Fill;
            checkBox_torBlock.Location = new Point(196, 33);
            checkBox_torBlock.Name = "checkBox_torBlock";
            checkBox_torBlock.Size = new Size(45, 17);
            checkBox_torBlock.TabIndex = 9;
            checkBox_torBlock.UseVisualStyleBackColor = true;
            checkBox_torBlock.Click += ProxyCheck_CBTORChange;
            // 
            // checkBox_proxyKick
            // 
            checkBox_proxyKick.AutoSize = true;
            checkBox_proxyKick.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_proxyKick.Dock = DockStyle.Fill;
            checkBox_proxyKick.Location = new Point(100, 56);
            checkBox_proxyKick.Name = "checkBox_proxyKick";
            checkBox_proxyKick.Size = new Size(42, 17);
            checkBox_proxyKick.TabIndex = 10;
            checkBox_proxyKick.UseVisualStyleBackColor = true;
            checkBox_proxyKick.Click += ProxyCheck_CBProxyChange;
            // 
            // checkBox_vpnKick
            // 
            checkBox_vpnKick.AutoSize = true;
            checkBox_vpnKick.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_vpnKick.Dock = DockStyle.Fill;
            checkBox_vpnKick.Location = new Point(148, 56);
            checkBox_vpnKick.Name = "checkBox_vpnKick";
            checkBox_vpnKick.Size = new Size(42, 17);
            checkBox_vpnKick.TabIndex = 11;
            checkBox_vpnKick.UseVisualStyleBackColor = true;
            checkBox_vpnKick.Click += ProxyCheck_CBVPNChange;
            // 
            // checkBox_torKick
            // 
            checkBox_torKick.AutoSize = true;
            checkBox_torKick.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_torKick.Dock = DockStyle.Fill;
            checkBox_torKick.Location = new Point(196, 56);
            checkBox_torKick.Name = "checkBox_torKick";
            checkBox_torKick.Size = new Size(45, 17);
            checkBox_torKick.TabIndex = 12;
            checkBox_torKick.UseVisualStyleBackColor = true;
            checkBox_torKick.Click += ProxyCheck_CBTORChange;
            // 
            // checkBox_proxyNone
            // 
            checkBox_proxyNone.AutoSize = true;
            checkBox_proxyNone.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_proxyNone.Checked = true;
            checkBox_proxyNone.CheckState = CheckState.Checked;
            checkBox_proxyNone.Dock = DockStyle.Fill;
            checkBox_proxyNone.Location = new Point(100, 79);
            checkBox_proxyNone.Name = "checkBox_proxyNone";
            checkBox_proxyNone.Size = new Size(42, 20);
            checkBox_proxyNone.TabIndex = 13;
            checkBox_proxyNone.UseVisualStyleBackColor = true;
            checkBox_proxyNone.Click += ProxyCheck_CBProxyChange;
            // 
            // checkBox_vpnNone
            // 
            checkBox_vpnNone.AutoSize = true;
            checkBox_vpnNone.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_vpnNone.Checked = true;
            checkBox_vpnNone.CheckState = CheckState.Checked;
            checkBox_vpnNone.Dock = DockStyle.Fill;
            checkBox_vpnNone.Location = new Point(148, 79);
            checkBox_vpnNone.Name = "checkBox_vpnNone";
            checkBox_vpnNone.Size = new Size(42, 20);
            checkBox_vpnNone.TabIndex = 14;
            checkBox_vpnNone.UseVisualStyleBackColor = true;
            checkBox_vpnNone.Click += ProxyCheck_CBVPNChange;
            // 
            // checkBox_torNone
            // 
            checkBox_torNone.AutoSize = true;
            checkBox_torNone.CheckAlign = ContentAlignment.MiddleCenter;
            checkBox_torNone.Checked = true;
            checkBox_torNone.CheckState = CheckState.Checked;
            checkBox_torNone.Dock = DockStyle.Fill;
            checkBox_torNone.Location = new Point(196, 79);
            checkBox_torNone.Name = "checkBox_torNone";
            checkBox_torNone.Size = new Size(45, 20);
            checkBox_torNone.TabIndex = 15;
            checkBox_torNone.UseVisualStyleBackColor = true;
            checkBox_torNone.Click += ProxyCheck_CBTORChange;
            // 
            // groupBox11
            // 
            groupBox11.Controls.Add(tableLayoutPanel18);
            groupBox11.Dock = DockStyle.Top;
            groupBox11.Location = new Point(0, 0);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(250, 108);
            groupBox11.TabIndex = 0;
            groupBox11.TabStop = false;
            groupBox11.Text = "General";
            // 
            // tableLayoutPanel18
            // 
            tableLayoutPanel18.ColumnCount = 3;
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.Controls.Add(textBox_ProxyAPIKey, 0, 1);
            tableLayoutPanel18.Controls.Add(cb_enableProxyCheck, 0, 0);
            tableLayoutPanel18.Controls.Add(label1, 0, 2);
            tableLayoutPanel18.Controls.Add(num_proxyCacheDays, 2, 2);
            tableLayoutPanel18.Dock = DockStyle.Fill;
            tableLayoutPanel18.ImeMode = ImeMode.NoControl;
            tableLayoutPanel18.Location = new Point(3, 19);
            tableLayoutPanel18.Name = "tableLayoutPanel18";
            tableLayoutPanel18.RowCount = 3;
            tableLayoutPanel18.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel18.Size = new Size(244, 86);
            tableLayoutPanel18.TabIndex = 0;
            // 
            // textBox_ProxyAPIKey
            // 
            tableLayoutPanel18.SetColumnSpan(textBox_ProxyAPIKey, 3);
            textBox_ProxyAPIKey.Dock = DockStyle.Fill;
            textBox_ProxyAPIKey.Location = new Point(4, 31);
            textBox_ProxyAPIKey.Margin = new Padding(4, 3, 4, 3);
            textBox_ProxyAPIKey.Name = "textBox_ProxyAPIKey";
            textBox_ProxyAPIKey.PlaceholderText = "API KEY";
            textBox_ProxyAPIKey.Size = new Size(236, 23);
            textBox_ProxyAPIKey.TabIndex = 2;
            textBox_ProxyAPIKey.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_enableProxyCheck
            // 
            cb_enableProxyCheck.AutoSize = true;
            cb_enableProxyCheck.CheckAlign = ContentAlignment.MiddleRight;
            tableLayoutPanel18.SetColumnSpan(cb_enableProxyCheck, 3);
            cb_enableProxyCheck.Dock = DockStyle.Fill;
            cb_enableProxyCheck.Location = new Point(3, 6);
            cb_enableProxyCheck.Margin = new Padding(3, 6, 3, 3);
            cb_enableProxyCheck.Name = "cb_enableProxyCheck";
            cb_enableProxyCheck.Padding = new Padding(15, 0, 5, 0);
            cb_enableProxyCheck.Size = new Size(238, 19);
            cb_enableProxyCheck.TabIndex = 0;
            cb_enableProxyCheck.Text = "Enable Proxy Checking";
            cb_enableProxyCheck.TextAlign = ContentAlignment.MiddleRight;
            cb_enableProxyCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            tableLayoutPanel18.SetColumnSpan(label1, 2);
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 56);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 0, 3);
            label1.Size = new Size(156, 30);
            label1.TabIndex = 4;
            label1.Text = "Cache IP (Days)";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_proxyCacheDays
            // 
            num_proxyCacheDays.Location = new Point(165, 59);
            num_proxyCacheDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            num_proxyCacheDays.Name = "num_proxyCacheDays";
            num_proxyCacheDays.Size = new Size(74, 23);
            num_proxyCacheDays.TabIndex = 3;
            num_proxyCacheDays.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(num_proxyCacheDays, "Days");
            num_proxyCacheDays.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // tabNetlimiter
            // 
            tabNetlimiter.Controls.Add(tableLayoutPanel23);
            tabNetlimiter.Location = new Point(4, 24);
            tabNetlimiter.Name = "tabNetlimiter";
            tabNetlimiter.Size = new Size(958, 394);
            tabNetlimiter.TabIndex = 4;
            tabNetlimiter.Text = "Netlimiter";
            tabNetlimiter.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel23
            // 
            tableLayoutPanel23.ColumnCount = 4;
            tableLayoutPanel23.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel23.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel23.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel23.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel23.Controls.Add(panel4, 2, 0);
            tableLayoutPanel23.Controls.Add(groupBox17, 0, 0);
            tableLayoutPanel23.Dock = DockStyle.Fill;
            tableLayoutPanel23.Location = new Point(0, 0);
            tableLayoutPanel23.Name = "tableLayoutPanel23";
            tableLayoutPanel23.RowCount = 1;
            tableLayoutPanel23.RowStyles.Add(new RowStyle());
            tableLayoutPanel23.Size = new Size(958, 394);
            tableLayoutPanel23.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(groupBox16);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(691, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(244, 394);
            panel4.TabIndex = 0;
            // 
            // groupBox16
            // 
            groupBox16.Controls.Add(tableLayoutPanel24);
            groupBox16.Dock = DockStyle.Fill;
            groupBox16.Location = new Point(0, 0);
            groupBox16.Name = "groupBox16";
            groupBox16.Size = new Size(244, 394);
            groupBox16.TabIndex = 0;
            groupBox16.TabStop = false;
            groupBox16.Text = "Settings";
            // 
            // tableLayoutPanel24
            // 
            tableLayoutPanel24.ColumnCount = 2;
            tableLayoutPanel24.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel24.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel24.Controls.Add(label12, 0, 5);
            tableLayoutPanel24.Controls.Add(label11, 0, 8);
            tableLayoutPanel24.Controls.Add(checkBox13, 0, 0);
            tableLayoutPanel24.Controls.Add(numericUpDown1, 1, 2);
            tableLayoutPanel24.Controls.Add(textBox2, 0, 2);
            tableLayoutPanel24.Controls.Add(label8, 0, 1);
            tableLayoutPanel24.Controls.Add(textBox3, 0, 3);
            tableLayoutPanel24.Controls.Add(textBox4, 0, 4);
            tableLayoutPanel24.Controls.Add(tableLayoutPanel25, 0, 7);
            tableLayoutPanel24.Controls.Add(label9, 0, 6);
            tableLayoutPanel24.Controls.Add(checkBox14, 0, 9);
            tableLayoutPanel24.Controls.Add(numericUpDown2, 1, 10);
            tableLayoutPanel24.Controls.Add(label10, 0, 10);
            tableLayoutPanel24.Controls.Add(tableLayoutPanel26, 0, 12);
            tableLayoutPanel24.Dock = DockStyle.Fill;
            tableLayoutPanel24.Location = new Point(3, 19);
            tableLayoutPanel24.Name = "tableLayoutPanel24";
            tableLayoutPanel24.RowCount = 14;
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel24.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel24.Size = new Size(238, 372);
            tableLayoutPanel24.TabIndex = 0;
            // 
            // label12
            // 
            label12.AutoSize = true;
            tableLayoutPanel24.SetColumnSpan(label12, 2);
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Segoe UI", 9F);
            label12.Location = new Point(3, 150);
            label12.Name = "label12";
            label12.Padding = new Padding(0, 0, 0, 3);
            label12.Size = new Size(232, 20);
            label12.TabIndex = 12;
            label12.Text = "Credentials Not Required for Local Host";
            label12.TextAlign = ContentAlignment.TopCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            tableLayoutPanel24.SetColumnSpan(label11, 2);
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label11.Location = new Point(3, 220);
            label11.Name = "label11";
            label11.Padding = new Padding(0, 0, 0, 3);
            label11.Size = new Size(232, 20);
            label11.TabIndex = 11;
            label11.Text = "Additional Checks";
            label11.TextAlign = ContentAlignment.BottomCenter;
            // 
            // checkBox13
            // 
            checkBox13.AutoSize = true;
            checkBox13.CheckAlign = ContentAlignment.MiddleRight;
            tableLayoutPanel24.SetColumnSpan(checkBox13, 2);
            checkBox13.Dock = DockStyle.Fill;
            checkBox13.Location = new Point(3, 3);
            checkBox13.Name = "checkBox13";
            checkBox13.Padding = new Padding(0, 3, 5, 0);
            checkBox13.Size = new Size(232, 24);
            checkBox13.TabIndex = 0;
            checkBox13.Text = "Enable NetLimiter";
            checkBox13.TextAlign = ContentAlignment.MiddleRight;
            checkBox13.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Dock = DockStyle.Fill;
            numericUpDown1.Location = new Point(122, 63);
            numericUpDown1.Maximum = new decimal(new int[] { 65999, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(113, 23);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.TextAlign = HorizontalAlignment.Center;
            numericUpDown1.Value = new decimal(new int[] { 9298, 0, 0, 0 });
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(3, 63);
            textBox2.MaxLength = 15;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "127.0.0.1";
            textBox2.Size = new Size(113, 23);
            textBox2.TabIndex = 2;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // label8
            // 
            label8.AutoSize = true;
            tableLayoutPanel24.SetColumnSpan(label8, 2);
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(3, 30);
            label8.Name = "label8";
            label8.Padding = new Padding(0, 0, 0, 3);
            label8.Size = new Size(232, 30);
            label8.TabIndex = 3;
            label8.Text = "NetLimiter Information";
            label8.TextAlign = ContentAlignment.BottomCenter;
            // 
            // textBox3
            // 
            tableLayoutPanel24.SetColumnSpan(textBox3, 2);
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(3, 93);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Username";
            textBox3.Size = new Size(232, 23);
            textBox3.TabIndex = 4;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            tableLayoutPanel24.SetColumnSpan(textBox4, 2);
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(3, 123);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Password";
            textBox4.Size = new Size(232, 23);
            textBox4.TabIndex = 5;
            textBox4.TextAlign = HorizontalAlignment.Center;
            textBox4.UseSystemPasswordChar = true;
            // 
            // tableLayoutPanel25
            // 
            tableLayoutPanel25.ColumnCount = 2;
            tableLayoutPanel24.SetColumnSpan(tableLayoutPanel25, 2);
            tableLayoutPanel25.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel25.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel25.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel25.Controls.Add(iconButton1, 1, 0);
            tableLayoutPanel25.Dock = DockStyle.Fill;
            tableLayoutPanel25.Location = new Point(0, 190);
            tableLayoutPanel25.Margin = new Padding(0);
            tableLayoutPanel25.Name = "tableLayoutPanel25";
            tableLayoutPanel25.RowCount = 1;
            tableLayoutPanel25.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel25.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel25.Size = new Size(238, 30);
            tableLayoutPanel25.TabIndex = 6;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 4);
            comboBox1.Margin = new Padding(3, 4, 3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(184, 23);
            comboBox1.TabIndex = 0;
            // 
            // iconButton1
            // 
            iconButton1.Dock = DockStyle.Fill;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            iconButton1.IconColor = Color.Black;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 24;
            iconButton1.Location = new Point(190, 0);
            iconButton1.Margin = new Padding(0);
            iconButton1.Name = "iconButton1";
            iconButton1.Padding = new Padding(0, 3, 0, 0);
            iconButton1.Size = new Size(48, 30);
            iconButton1.TabIndex = 1;
            iconButton1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            tableLayoutPanel24.SetColumnSpan(label9, 2);
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label9.Location = new Point(3, 170);
            label9.Name = "label9";
            label9.Padding = new Padding(0, 0, 0, 3);
            label9.Size = new Size(232, 20);
            label9.TabIndex = 7;
            label9.Text = "NetLimiter Filters";
            label9.TextAlign = ContentAlignment.BottomCenter;
            // 
            // checkBox14
            // 
            checkBox14.AutoSize = true;
            checkBox14.CheckAlign = ContentAlignment.MiddleRight;
            tableLayoutPanel24.SetColumnSpan(checkBox14, 2);
            checkBox14.Dock = DockStyle.Fill;
            checkBox14.Location = new Point(3, 243);
            checkBox14.Name = "checkBox14";
            checkBox14.Padding = new Padding(0, 3, 5, 0);
            checkBox14.Size = new Size(232, 24);
            checkBox14.TabIndex = 8;
            checkBox14.Text = "Enable Connection Abuse Banning";
            checkBox14.TextAlign = ContentAlignment.MiddleRight;
            checkBox14.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Dock = DockStyle.Fill;
            numericUpDown2.Location = new Point(122, 273);
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(113, 23);
            numericUpDown2.TabIndex = 9;
            numericUpDown2.TextAlign = HorizontalAlignment.Center;
            numericUpDown2.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 270);
            label10.Name = "label10";
            label10.Size = new Size(113, 30);
            label10.TabIndex = 10;
            label10.Text = "Threshold";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel26
            // 
            tableLayoutPanel26.ColumnCount = 5;
            tableLayoutPanel24.SetColumnSpan(tableLayoutPanel26, 2);
            tableLayoutPanel26.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel26.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel26.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel26.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel26.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel26.Controls.Add(iconButton3, 0, 0);
            tableLayoutPanel26.Controls.Add(iconButton2, 2, 0);
            tableLayoutPanel26.Dock = DockStyle.Fill;
            tableLayoutPanel26.Location = new Point(0, 320);
            tableLayoutPanel26.Margin = new Padding(0);
            tableLayoutPanel26.Name = "tableLayoutPanel26";
            tableLayoutPanel26.RowCount = 1;
            tableLayoutPanel26.RowStyles.Add(new RowStyle());
            tableLayoutPanel26.Size = new Size(238, 40);
            tableLayoutPanel26.TabIndex = 13;
            // 
            // iconButton3
            // 
            iconButton3.Dock = DockStyle.Fill;
            iconButton3.IconChar = FontAwesome.Sharp.IconChar.Reply;
            iconButton3.IconColor = Color.Black;
            iconButton3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton3.IconSize = 32;
            iconButton3.Location = new Point(0, 0);
            iconButton3.Margin = new Padding(0);
            iconButton3.Name = "iconButton3";
            iconButton3.Padding = new Padding(0, 3, 0, 0);
            iconButton3.Size = new Size(47, 40);
            iconButton3.TabIndex = 1;
            toolTip1.SetToolTip(iconButton3, "Reset");
            iconButton3.UseVisualStyleBackColor = true;
            // 
            // iconButton2
            // 
            iconButton2.Dock = DockStyle.Fill;
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.Save;
            iconButton2.IconColor = Color.Black;
            iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton2.IconSize = 32;
            iconButton2.Location = new Point(94, 0);
            iconButton2.Margin = new Padding(0);
            iconButton2.Name = "iconButton2";
            iconButton2.Padding = new Padding(0, 3, 0, 0);
            iconButton2.Size = new Size(47, 40);
            iconButton2.TabIndex = 0;
            toolTip1.SetToolTip(iconButton2, "Save");
            iconButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            groupBox17.Dock = DockStyle.Fill;
            groupBox17.Location = new Point(3, 3);
            groupBox17.Name = "groupBox17";
            groupBox17.Size = new Size(662, 394);
            groupBox17.TabIndex = 1;
            groupBox17.TabStop = false;
            groupBox17.Text = "NetLimiter Active Connections";
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
            blacklistForm.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            blacklist_DateBoxes.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            blacklist_IPAddress.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            blacklist_PlayerName.ResumeLayout(false);
            blacklist_PlayerName.PerformLayout();
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
            tabProxyChecking.ResumeLayout(false);
            tableLayoutPanel16.ResumeLayout(false);
            tableLayoutPanel17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgProxyCountryBlockList).EndInit();
            tableLayoutPanel15.ResumeLayout(false);
            tableLayoutPanel15.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tableLayoutPanel14.ResumeLayout(false);
            groupBox12.ResumeLayout(false);
            tableLayoutPanel19.ResumeLayout(false);
            tableLayoutPanel19.PerformLayout();
            panel3.ResumeLayout(false);
            groupBox15.ResumeLayout(false);
            tableLayoutPanel22.ResumeLayout(false);
            tableLayoutPanel22.PerformLayout();
            groupBox14.ResumeLayout(false);
            tableLayoutPanel21.ResumeLayout(false);
            groupBox13.ResumeLayout(false);
            tableLayoutPanel20.ResumeLayout(false);
            tableLayoutPanel20.PerformLayout();
            groupBox11.ResumeLayout(false);
            tableLayoutPanel18.ResumeLayout(false);
            tableLayoutPanel18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_proxyCacheDays).EndInit();
            tabNetlimiter.ResumeLayout(false);
            tableLayoutPanel23.ResumeLayout(false);
            panel4.ResumeLayout(false);
            groupBox16.ResumeLayout(false);
            tableLayoutPanel24.ResumeLayout(false);
            tableLayoutPanel24.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            tableLayoutPanel25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            tableLayoutPanel26.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl banControls;
        private TabPage tabBlacklist;
        private TabPage tabWhitelist;
        private TabPage tabProxyChecking;
        private TabPage tabNetlimiter;
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
        private Panel blacklistForm;
        private GroupBox blacklist_IPAddress;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox blacklist_PlayerName;
        private TextBox blacklist_PlayerNameTxt;
        private ComboBox blacklist_IPSubnetTxt;
        private TextBox blacklist_IPAddressTxt;
        private GroupBox blacklist_DateBoxes;
        private TableLayoutPanel tableLayoutPanel5;
        private DateTimePicker blacklist_DateStart;
        private DateTimePicker blacklist_DateEnd;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel6;
        private CheckBox blacklist_TempBan;
        private CheckBox blacklist_PermBan;
        private GroupBox groupBox5;
        private TextBox blacklist_notes;
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
        private FontAwesome.Sharp.IconButton blacklist_btnSave;
        private FontAwesome.Sharp.IconButton blacklist_btnReset;
        private FontAwesome.Sharp.IconButton wlControlSave;
        private FontAwesome.Sharp.IconButton wlControlReset;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel16;
        private TableLayoutPanel tableLayoutPanel17;
        private DataGridView dgProxyCountryBlockList;
        private TableLayoutPanel tableLayoutPanel15;
        private TextBox textBox_countryCode;
        private TextBox textBox_countryName;
        private Button btn_proxyAddCountry;
        private TabControl tabControl1;
        private TabPage proxyLookup;
        private TabPage tabPage2;
        private TableLayoutPanel tableLayoutPanel14;
        private GroupBox groupBox11;
        private TableLayoutPanel tableLayoutPanel18;
        private CheckBox cb_enableProxyCheck;
        private GroupBox groupBox12;
        private TableLayoutPanel tableLayoutPanel19;
        private CheckBox cb_serviceProxyCheckIO;
        private TextBox textBox_ProxyAPIKey;
        private NumericUpDown num_proxyCacheDays;
        private Panel panel3;
        private GroupBox groupBox13;
        private TableLayoutPanel tableLayoutPanel20;
        private Label label2;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label7;
        private CheckBox checkBox_proxyBlock;
        private CheckBox checkBox_vpnBlock;
        private CheckBox checkBox_torBlock;
        private CheckBox checkBox_proxyKick;
        private CheckBox checkBox_vpnKick;
        private CheckBox checkBox_torKick;
        private CheckBox checkBox_proxyNone;
        private CheckBox checkBox_vpnNone;
        private CheckBox checkBox_torNone;
        private GroupBox groupBox14;
        private TableLayoutPanel tableLayoutPanel21;
        private FontAwesome.Sharp.IconButton btn_proxyReset;
        private FontAwesome.Sharp.IconButton btn_proxySave;
        private GroupBox groupBox15;
        private TableLayoutPanel tableLayoutPanel22;
        private CheckBox checkBox_GeoAllow;
        private CheckBox checkBox_GeoBlock;
        private CheckBox checkBox_GeoOff;
        private TableLayoutPanel tableLayoutPanel23;
        private Panel panel4;
        private GroupBox groupBox16;
        private TableLayoutPanel tableLayoutPanel24;
        private CheckBox checkBox13;
        private NumericUpDown numericUpDown1;
        private TextBox textBox2;
        private Label label8;
        private TextBox textBox3;
        private TextBox textBox4;
        private TableLayoutPanel tableLayoutPanel25;
        private ComboBox comboBox1;
        private FontAwesome.Sharp.IconButton iconButton1;
        private Label label9;
        private CheckBox checkBox14;
        private NumericUpDown numericUpDown2;
        private Label label10;
        private Label label11;
        private Label label12;
        private GroupBox groupBox17;
        private TableLayoutPanel tableLayoutPanel26;
        private FontAwesome.Sharp.IconButton iconButton3;
        private FontAwesome.Sharp.IconButton iconButton2;
        private FontAwesome.Sharp.IconButton blControlRefresh;
        private FontAwesome.Sharp.IconButton wlControlRefresh;
        private FontAwesome.Sharp.IconButton blacklist_btnClose;
        private FontAwesome.Sharp.IconButton blacklist_btnDelete;
        private FontAwesome.Sharp.IconButton wlControlDelete;
        private FontAwesome.Sharp.IconButton wlControlClose;
        private FontAwesome.Sharp.IconButton btn_proxyTest;
        private DataGridViewTextBoxColumn geo_recordID;
        private DataGridViewTextBoxColumn proxy_countyCode;
        private DataGridViewTextBoxColumn proxy_countyName;
        private CheckBox cb_serviceIP2LocationIO;
    }
}
