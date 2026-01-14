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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            dg_playerNames = new DataGridView();
            playerRecordID = new DataGridViewTextBoxColumn();
            playerName = new DataGridViewTextBoxColumn();
            groupBox8 = new GroupBox();
            dg_IPAddresses = new DataGridView();
            ipRecordID = new DataGridViewTextBoxColumn();
            address = new DataGridViewTextBoxColumn();
            tabControl1 = new TabControl();
            tabBanControls = new TabPage();
            groupBox1 = new GroupBox();
            groupBox9 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            tb_bansPlayerName = new TextBox();
            tb_bansIPAddress = new TextBox();
            cb_banSubMask = new ComboBox();
            btn_addBan = new Button();
            groupBox10 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            btn_banExport = new Button();
            btn_banImport = new Button();
            tabVPN = new TabPage();
            tabFirewall = new TabPage();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_playerNames).BeginInit();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_IPAddresses).BeginInit();
            tabControl1.SuspendLayout();
            tabBanControls.SuspendLayout();
            groupBox9.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            groupBox10.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.13082F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58.86918F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tabControl1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(966, 422);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox8, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(397, 422);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dg_playerNames);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(0, 0);
            groupBox3.Margin = new Padding(0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(188, 422);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Player Names";
            // 
            // dg_playerNames
            // 
            dg_playerNames.AllowUserToAddRows = false;
            dg_playerNames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_playerNames.ColumnHeadersVisible = false;
            dg_playerNames.Columns.AddRange(new DataGridViewColumn[] { playerRecordID, playerName });
            dg_playerNames.Dock = DockStyle.Fill;
            dg_playerNames.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_playerNames.Location = new Point(3, 19);
            dg_playerNames.Margin = new Padding(0);
            dg_playerNames.Name = "dg_playerNames";
            dg_playerNames.RowHeadersVisible = false;
            dg_playerNames.Size = new Size(182, 400);
            dg_playerNames.TabIndex = 0;
            dg_playerNames.CellDoubleClick += actionDbClick_RemoveRecord;
            // 
            // playerRecordID
            // 
            playerRecordID.HeaderText = "ID";
            playerRecordID.Name = "playerRecordID";
            playerRecordID.Visible = false;
            // 
            // playerName
            // 
            playerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            playerName.HeaderText = "Player Name";
            playerName.Name = "playerName";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(dg_IPAddresses);
            groupBox8.Dock = DockStyle.Fill;
            groupBox8.Location = new Point(208, 0);
            groupBox8.Margin = new Padding(0);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(189, 422);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "IP Addresses";
            // 
            // dg_IPAddresses
            // 
            dg_IPAddresses.AllowUserToAddRows = false;
            dg_IPAddresses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_IPAddresses.ColumnHeadersVisible = false;
            dg_IPAddresses.Columns.AddRange(new DataGridViewColumn[] { ipRecordID, address });
            dg_IPAddresses.Dock = DockStyle.Fill;
            dg_IPAddresses.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_IPAddresses.Location = new Point(3, 19);
            dg_IPAddresses.Margin = new Padding(0);
            dg_IPAddresses.Name = "dg_IPAddresses";
            dg_IPAddresses.RowHeadersVisible = false;
            dg_IPAddresses.Size = new Size(183, 400);
            dg_IPAddresses.TabIndex = 0;
            dg_IPAddresses.CellDoubleClick += actionDbClick_RemoveRecord2;
            // 
            // ipRecordID
            // 
            ipRecordID.HeaderText = "ID";
            ipRecordID.Name = "ipRecordID";
            ipRecordID.Visible = false;
            // 
            // address
            // 
            address.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            address.HeaderText = "IP Address";
            address.Name = "address";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabBanControls);
            tabControl1.Controls.Add(tabVPN);
            tabControl1.Controls.Add(tabFirewall);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(400, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(563, 416);
            tabControl1.TabIndex = 1;
            // 
            // tabBanControls
            // 
            tabBanControls.Controls.Add(groupBox1);
            tabBanControls.Controls.Add(groupBox9);
            tabBanControls.Controls.Add(groupBox10);
            tabBanControls.Location = new Point(4, 24);
            tabBanControls.Name = "tabBanControls";
            tabBanControls.Padding = new Padding(3);
            tabBanControls.Size = new Size(555, 388);
            tabBanControls.TabIndex = 0;
            tabBanControls.Text = "Ban Controls";
            tabBanControls.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 53);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(549, 279);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ban Settings";
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(tableLayoutPanel7);
            groupBox9.Dock = DockStyle.Top;
            groupBox9.Location = new Point(3, 3);
            groupBox9.Margin = new Padding(0);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(549, 50);
            groupBox9.TabIndex = 3;
            groupBox9.TabStop = false;
            groupBox9.Text = "Add Record";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 4;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel7.Controls.Add(tb_bansPlayerName, 0, 0);
            tableLayoutPanel7.Controls.Add(tb_bansIPAddress, 1, 0);
            tableLayoutPanel7.Controls.Add(cb_banSubMask, 2, 0);
            tableLayoutPanel7.Controls.Add(btn_addBan, 3, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle());
            tableLayoutPanel7.Size = new Size(543, 28);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // tb_bansPlayerName
            // 
            tb_bansPlayerName.Dock = DockStyle.Fill;
            tb_bansPlayerName.Location = new Point(3, 3);
            tb_bansPlayerName.Name = "tb_bansPlayerName";
            tb_bansPlayerName.PlaceholderText = "Player Name";
            tb_bansPlayerName.Size = new Size(178, 23);
            tb_bansPlayerName.TabIndex = 0;
            tb_bansPlayerName.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_bansIPAddress
            // 
            tb_bansIPAddress.Dock = DockStyle.Fill;
            tb_bansIPAddress.Location = new Point(187, 3);
            tb_bansIPAddress.Name = "tb_bansIPAddress";
            tb_bansIPAddress.PlaceholderText = "000.000.000.000";
            tb_bansIPAddress.Size = new Size(178, 23);
            tb_bansIPAddress.TabIndex = 1;
            tb_bansIPAddress.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_banSubMask
            // 
            cb_banSubMask.Dock = DockStyle.Fill;
            cb_banSubMask.FormattingEnabled = true;
            cb_banSubMask.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" });
            cb_banSubMask.Location = new Point(371, 3);
            cb_banSubMask.Name = "cb_banSubMask";
            cb_banSubMask.Size = new Size(69, 23);
            cb_banSubMask.TabIndex = 2;
            // 
            // btn_addBan
            // 
            btn_addBan.Dock = DockStyle.Fill;
            btn_addBan.FlatStyle = FlatStyle.Flat;
            btn_addBan.Location = new Point(444, 1);
            btn_addBan.Margin = new Padding(1);
            btn_addBan.Name = "btn_addBan";
            btn_addBan.Size = new Size(98, 27);
            btn_addBan.TabIndex = 3;
            btn_addBan.Text = "ADD";
            btn_addBan.UseVisualStyleBackColor = true;
            btn_addBan.Click += actionClick_addBanInformation;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(tableLayoutPanel6);
            groupBox10.Dock = DockStyle.Bottom;
            groupBox10.Location = new Point(3, 332);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new Padding(3, 3, 3, 6);
            groupBox10.Size = new Size(549, 53);
            groupBox10.TabIndex = 2;
            groupBox10.TabStop = false;
            groupBox10.Text = "Misc";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 5;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Controls.Add(btn_banExport, 4, 0);
            tableLayoutPanel6.Controls.Add(btn_banImport, 3, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(543, 28);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // btn_banExport
            // 
            btn_banExport.Dock = DockStyle.Fill;
            btn_banExport.Location = new Point(432, 0);
            btn_banExport.Margin = new Padding(0);
            btn_banExport.Name = "btn_banExport";
            btn_banExport.Size = new Size(111, 28);
            btn_banExport.TabIndex = 0;
            btn_banExport.Text = "Export";
            btn_banExport.UseVisualStyleBackColor = true;
            btn_banExport.Click += actionClick_exportBanSettings;
            // 
            // btn_banImport
            // 
            btn_banImport.Dock = DockStyle.Fill;
            btn_banImport.Location = new Point(324, 0);
            btn_banImport.Margin = new Padding(0);
            btn_banImport.Name = "btn_banImport";
            btn_banImport.Size = new Size(108, 28);
            btn_banImport.TabIndex = 1;
            btn_banImport.Text = "Import";
            btn_banImport.UseVisualStyleBackColor = true;
            btn_banImport.Click += actionClick_importBans;
            // 
            // tabVPN
            // 
            tabVPN.Location = new Point(4, 24);
            tabVPN.Name = "tabVPN";
            tabVPN.Padding = new Padding(3);
            tabVPN.Size = new Size(473, 374);
            tabVPN.TabIndex = 1;
            tabVPN.Text = "Proxy/VPN";
            tabVPN.UseVisualStyleBackColor = true;
            // 
            // tabFirewall
            // 
            tabFirewall.Location = new Point(4, 24);
            tabFirewall.Name = "tabFirewall";
            tabFirewall.Size = new Size(473, 374);
            tabFirewall.TabIndex = 2;
            tabFirewall.Text = "Firewall";
            tabFirewall.UseVisualStyleBackColor = true;
            // 
            // tabBans
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "tabBans";
            Size = new Size(966, 422);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_playerNames).EndInit();
            groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_IPAddresses).EndInit();
            tabControl1.ResumeLayout(false);
            tabBanControls.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            groupBox10.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox3;
        public DataGridView dg_playerNames;
        private DataGridViewTextBoxColumn playerRecordID;
        private DataGridViewTextBoxColumn playerName;
        private GroupBox groupBox8;
        public DataGridView dg_IPAddresses;
        private DataGridViewTextBoxColumn ipRecordID;
        private DataGridViewTextBoxColumn address;
        private TabControl tabControl1;
        private TabPage tabBanControls;
        private TabPage tabVPN;
        private TabPage tabFirewall;
        private GroupBox groupBox10;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btn_banExport;
        private Button btn_banImport;
        private GroupBox groupBox9;
        private TableLayoutPanel tableLayoutPanel7;
        internal TextBox tb_bansPlayerName;
        internal TextBox tb_bansIPAddress;
        private ComboBox cb_banSubMask;
        private Button btn_addBan;
        private GroupBox groupBox1;
    }
}
