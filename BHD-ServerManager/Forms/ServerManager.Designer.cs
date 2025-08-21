using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BHD_ServerManager.Forms
{
    partial class ServerManager : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStrip = new ToolStrip();
            toolStripStatus = new ToolStripLabel();
            label_TimeLeft = new ToolStripLabel();
            label_WinCondition = new ToolStripLabel();
            label_RedScore = new ToolStripLabel();
            label_BlueScore = new ToolStripLabel();
            label_PlayersOnline = new ToolStripLabel();
            mainPanel = new Panel();
            tabControl = new TabControl();
            tabProfile = new TabPage();
            tabServer = new TabPage();
            tabMaps = new TabPage();
            tabPlayers = new TabPage();
            tabChat = new TabPage();
            tabBans = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            dg_playerNames = new DataGridView();
            playerRecordID = new DataGridViewTextBoxColumn();
            playerName = new DataGridViewTextBoxColumn();
            groupBox8 = new GroupBox();
            dg_IPAddresses = new DataGridView();
            ipRecordID = new DataGridViewTextBoxColumn();
            address = new DataGridViewTextBoxColumn();
            panel8 = new Panel();
            groupBox10 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            btn_banExport = new Button();
            btn_banImport = new Button();
            groupBox9 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            tb_bansPlayerName = new TextBox();
            tb_bansIPAddress = new TextBox();
            cb_banSubMask = new ComboBox();
            btn_addBan = new Button();
            tabStats = new TabPage();
            tabStatsControl = new TabControl();
            tabPlayerStats = new TabPage();
            dataGridViewPlayerStats = new DataGridView();
            tabWeaponStats = new TabPage();
            dataGridViewWeaponStats = new DataGridView();
            tabBabstats = new TabPage();
            tableLayoutPanel13 = new TableLayoutPanel();
            dg_statsLog = new DataGridView();
            statsLog_DateTime = new DataGridViewTextBoxColumn();
            statLog_Message = new DataGridViewTextBoxColumn();
            panel7 = new Panel();
            btn_SaveSettings = new Button();
            labelReportInterval = new Label();
            labelUpatedInterval = new Label();
            num_WebStatsReport = new NumericUpDown();
            num_WebStatsUpdates = new NumericUpDown();
            cb_enableAnnouncements = new CheckBox();
            btn_validate = new Button();
            label_webServerPath = new Label();
            tb_webStatsServerPath = new TextBox();
            cb_enableWebStats = new CheckBox();
            tabAdmin = new TabPage();
            tableAdminPanel = new TableLayoutPanel();
            dg_adminLog = new DataGridView();
            adminLog_datetime = new DataGridViewTextBoxColumn();
            adminLog_username = new DataGridViewTextBoxColumn();
            adminLog_log = new DataGridViewTextBoxColumn();
            tableLayoutPanel14 = new TableLayoutPanel();
            dg_AdminUsers = new DataGridView();
            admin_id = new DataGridViewTextBoxColumn();
            admin_username = new DataGridViewTextBoxColumn();
            admin_role = new DataGridViewTextBoxColumn();
            tableLayoutPanel15 = new TableLayoutPanel();
            tableLayoutPanel16 = new TableLayoutPanel();
            btn_adminNew = new Button();
            btn_adminAdd = new Button();
            btn_adminSave = new Button();
            btn_adminDelete = new Button();
            tableLayoutPanel17 = new TableLayoutPanel();
            cb_adminRole = new ComboBox();
            tableLayoutPanel18 = new TableLayoutPanel();
            tb_adminUser = new TextBox();
            tableLayoutPanel19 = new TableLayoutPanel();
            tb_adminPass = new TextBox();
            openFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            toolStrip.SuspendLayout();
            mainPanel.SuspendLayout();
            tabControl.SuspendLayout();
            tabBans.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_playerNames).BeginInit();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_IPAddresses).BeginInit();
            panel8.SuspendLayout();
            groupBox10.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox9.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tabStats.SuspendLayout();
            tabStatsControl.SuspendLayout();
            tabPlayerStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).BeginInit();
            tabWeaponStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).BeginInit();
            tabBabstats.SuspendLayout();
            tableLayoutPanel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_statsLog).BeginInit();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).BeginInit();
            tabAdmin.SuspendLayout();
            tableAdminPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_adminLog).BeginInit();
            tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_AdminUsers).BeginInit();
            tableLayoutPanel15.SuspendLayout();
            tableLayoutPanel16.SuspendLayout();
            tableLayoutPanel17.SuspendLayout();
            tableLayoutPanel18.SuspendLayout();
            tableLayoutPanel19.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Dock = DockStyle.Bottom;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripStatus, label_TimeLeft, label_WinCondition, label_RedScore, label_BlueScore, label_PlayersOnline });
            toolStrip.Location = new Point(0, 400);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(920, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
            // 
            // toolStripStatus
            // 
            toolStripStatus.Name = "toolStripStatus";
            toolStripStatus.Size = new Size(117, 22);
            toolStripStatus.Text = "Current Server Status";
            // 
            // label_TimeLeft
            // 
            label_TimeLeft.Alignment = ToolStripItemAlignment.Right;
            label_TimeLeft.Name = "label_TimeLeft";
            label_TimeLeft.Size = new Size(65, 22);
            label_TimeLeft.Text = "[Time Left]";
            label_TimeLeft.ToolTipText = "Future Feature";
            // 
            // label_WinCondition
            // 
            label_WinCondition.Alignment = ToolStripItemAlignment.Right;
            label_WinCondition.Name = "label_WinCondition";
            label_WinCondition.Size = new Size(92, 22);
            label_WinCondition.Text = "[Win Condition]";
            label_WinCondition.ToolTipText = "Future Feature";
            // 
            // label_RedScore
            // 
            label_RedScore.Alignment = ToolStripItemAlignment.Right;
            label_RedScore.Name = "label_RedScore";
            label_RedScore.Size = new Size(67, 22);
            label_RedScore.Text = "[Red Score]";
            label_RedScore.ToolTipText = "Future Feature";
            // 
            // label_BlueScore
            // 
            label_BlueScore.Alignment = ToolStripItemAlignment.Right;
            label_BlueScore.Name = "label_BlueScore";
            label_BlueScore.Size = new Size(70, 22);
            label_BlueScore.Text = "[Blue Score]";
            label_BlueScore.ToolTipText = "Future Feature";
            // 
            // label_PlayersOnline
            // 
            label_PlayersOnline.Alignment = ToolStripItemAlignment.Right;
            label_PlayersOnline.Name = "label_PlayersOnline";
            label_PlayersOnline.Size = new Size(90, 22);
            label_PlayersOnline.Text = "[Players Online]";
            label_PlayersOnline.ToolTipText = "Future Feature";
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(tabControl);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(5);
            mainPanel.Size = new Size(920, 400);
            mainPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabProfile);
            tabControl.Controls.Add(tabServer);
            tabControl.Controls.Add(tabMaps);
            tabControl.Controls.Add(tabPlayers);
            tabControl.Controls.Add(tabChat);
            tabControl.Controls.Add(tabBans);
            tabControl.Controls.Add(tabStats);
            tabControl.Controls.Add(tabAdmin);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(5, 5);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(910, 390);
            tabControl.TabIndex = 0;
            // 
            // tabProfile
            // 
            tabProfile.Location = new Point(4, 24);
            tabProfile.Name = "tabProfile";
            tabProfile.Size = new Size(902, 362);
            tabProfile.TabIndex = 8;
            tabProfile.Text = "Game Profile";
            tabProfile.UseVisualStyleBackColor = true;
            // 
            // tabServer
            // 
            tabServer.Location = new Point(4, 24);
            tabServer.Name = "tabServer";
            tabServer.Padding = new Padding(3);
            tabServer.Size = new Size(902, 362);
            tabServer.TabIndex = 0;
            tabServer.Text = "Game Server";
            tabServer.UseVisualStyleBackColor = true;
            // 
            // tabMaps
            // 
            tabMaps.Location = new Point(4, 24);
            tabMaps.Name = "tabMaps";
            tabMaps.Padding = new Padding(3);
            tabMaps.Size = new Size(902, 362);
            tabMaps.TabIndex = 1;
            tabMaps.Text = "Maps";
            tabMaps.UseVisualStyleBackColor = true;
            // 
            // tabPlayers
            // 
            tabPlayers.Location = new Point(4, 24);
            tabPlayers.Name = "tabPlayers";
            tabPlayers.Size = new Size(902, 362);
            tabPlayers.TabIndex = 2;
            tabPlayers.Text = "Players";
            tabPlayers.UseVisualStyleBackColor = true;
            // 
            // tabChat
            // 
            tabChat.Location = new Point(4, 24);
            tabChat.Name = "tabChat";
            tabChat.Padding = new Padding(3);
            tabChat.Size = new Size(902, 362);
            tabChat.TabIndex = 9;
            tabChat.Text = "Chat";
            tabChat.UseVisualStyleBackColor = true;
            // 
            // tabBans
            // 
            tabBans.Controls.Add(tableLayoutPanel5);
            tabBans.Location = new Point(4, 24);
            tabBans.Name = "tabBans";
            tabBans.Padding = new Padding(3);
            tabBans.Size = new Size(902, 362);
            tabBans.TabIndex = 4;
            tabBans.Text = "Bans";
            tabBans.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 6;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.Controls.Add(groupBox3, 1, 0);
            tableLayoutPanel5.Controls.Add(groupBox8, 3, 0);
            tableLayoutPanel5.Controls.Add(panel8, 5, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 350F));
            tableLayoutPanel5.Size = new Size(896, 356);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dg_playerNames);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(20, 0);
            groupBox3.Margin = new Padding(0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(150, 356);
            groupBox3.TabIndex = 0;
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
            dg_playerNames.Name = "dg_playerNames";
            dg_playerNames.RowHeadersVisible = false;
            dg_playerNames.Size = new Size(144, 334);
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
            groupBox8.Location = new Point(190, 0);
            groupBox8.Margin = new Padding(0);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(150, 356);
            groupBox8.TabIndex = 1;
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
            dg_IPAddresses.Name = "dg_IPAddresses";
            dg_IPAddresses.RowHeadersVisible = false;
            dg_IPAddresses.Size = new Size(144, 334);
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
            // panel8
            // 
            panel8.Controls.Add(groupBox10);
            panel8.Controls.Add(groupBox9);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(360, 0);
            panel8.Margin = new Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new Size(536, 356);
            panel8.TabIndex = 2;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(tableLayoutPanel6);
            groupBox10.Dock = DockStyle.Top;
            groupBox10.Location = new Point(0, 50);
            groupBox10.Name = "groupBox10";
            groupBox10.Padding = new Padding(3, 3, 3, 6);
            groupBox10.Size = new Size(536, 53);
            groupBox10.TabIndex = 1;
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
            tableLayoutPanel6.Size = new Size(530, 28);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // btn_banExport
            // 
            btn_banExport.Dock = DockStyle.Fill;
            btn_banExport.Location = new Point(424, 0);
            btn_banExport.Margin = new Padding(0);
            btn_banExport.Name = "btn_banExport";
            btn_banExport.Size = new Size(106, 28);
            btn_banExport.TabIndex = 0;
            btn_banExport.Text = "Export";
            btn_banExport.UseVisualStyleBackColor = true;
            btn_banExport.Click += actionClick_exportBanSettings;
            // 
            // btn_banImport
            // 
            btn_banImport.Dock = DockStyle.Fill;
            btn_banImport.Location = new Point(318, 0);
            btn_banImport.Margin = new Padding(0);
            btn_banImport.Name = "btn_banImport";
            btn_banImport.Size = new Size(106, 28);
            btn_banImport.TabIndex = 1;
            btn_banImport.Text = "Import";
            toolTip.SetToolTip(btn_banImport, "HawkSync or BMT v3.5 Ban Files Accepted");
            btn_banImport.UseVisualStyleBackColor = true;
            btn_banImport.Click += actionClick_importBans;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(tableLayoutPanel7);
            groupBox9.Dock = DockStyle.Top;
            groupBox9.Location = new Point(0, 0);
            groupBox9.Margin = new Padding(0);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(536, 50);
            groupBox9.TabIndex = 0;
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
            tableLayoutPanel7.Size = new Size(530, 28);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // tb_bansPlayerName
            // 
            tb_bansPlayerName.Dock = DockStyle.Fill;
            tb_bansPlayerName.Location = new Point(3, 3);
            tb_bansPlayerName.Name = "tb_bansPlayerName";
            tb_bansPlayerName.PlaceholderText = "Player Name";
            tb_bansPlayerName.Size = new Size(171, 23);
            tb_bansPlayerName.TabIndex = 0;
            tb_bansPlayerName.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_bansIPAddress
            // 
            tb_bansIPAddress.Dock = DockStyle.Fill;
            tb_bansIPAddress.Location = new Point(180, 3);
            tb_bansIPAddress.Name = "tb_bansIPAddress";
            tb_bansIPAddress.PlaceholderText = "000.000.000.000";
            tb_bansIPAddress.Size = new Size(171, 23);
            tb_bansIPAddress.TabIndex = 1;
            tb_bansIPAddress.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_banSubMask
            // 
            cb_banSubMask.Dock = DockStyle.Fill;
            cb_banSubMask.FormattingEnabled = true;
            cb_banSubMask.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" });
            cb_banSubMask.Location = new Point(357, 3);
            cb_banSubMask.Name = "cb_banSubMask";
            cb_banSubMask.Size = new Size(69, 23);
            cb_banSubMask.TabIndex = 2;
            toolTip.SetToolTip(cb_banSubMask, "These are examples of the type of masks you can do,  that said you'll likely just want 32 for a single IP.\r\n\r\n8 = 10.0.0.0\r\n16 = 172.16.0.0 \r\n24 = 192.16.1.0\r\n32 = 1.1.1.1 ");
            // 
            // btn_addBan
            // 
            btn_addBan.Dock = DockStyle.Fill;
            btn_addBan.Location = new Point(432, 3);
            btn_addBan.Name = "btn_addBan";
            btn_addBan.Size = new Size(95, 23);
            btn_addBan.TabIndex = 3;
            btn_addBan.Text = "ADD";
            btn_addBan.UseVisualStyleBackColor = true;
            btn_addBan.Click += actionClick_addBanInformation;
            // 
            // tabStats
            // 
            tabStats.Controls.Add(tabStatsControl);
            tabStats.Location = new Point(4, 24);
            tabStats.Name = "tabStats";
            tabStats.Padding = new Padding(3);
            tabStats.Size = new Size(902, 362);
            tabStats.TabIndex = 6;
            tabStats.Text = "Stats";
            tabStats.UseVisualStyleBackColor = true;
            // 
            // tabStatsControl
            // 
            tabStatsControl.Controls.Add(tabPlayerStats);
            tabStatsControl.Controls.Add(tabWeaponStats);
            tabStatsControl.Controls.Add(tabBabstats);
            tabStatsControl.Dock = DockStyle.Fill;
            tabStatsControl.Location = new Point(3, 3);
            tabStatsControl.Name = "tabStatsControl";
            tabStatsControl.SelectedIndex = 0;
            tabStatsControl.Size = new Size(896, 356);
            tabStatsControl.TabIndex = 0;
            // 
            // tabPlayerStats
            // 
            tabPlayerStats.Controls.Add(dataGridViewPlayerStats);
            tabPlayerStats.Location = new Point(4, 24);
            tabPlayerStats.Name = "tabPlayerStats";
            tabPlayerStats.Padding = new Padding(3);
            tabPlayerStats.Size = new Size(888, 328);
            tabPlayerStats.TabIndex = 0;
            tabPlayerStats.Text = "Player Stats";
            tabPlayerStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPlayerStats
            // 
            dataGridViewPlayerStats.AllowUserToAddRows = false;
            dataGridViewPlayerStats.AllowUserToDeleteRows = false;
            dataGridViewPlayerStats.AllowUserToResizeColumns = false;
            dataGridViewPlayerStats.AllowUserToResizeRows = false;
            dataGridViewPlayerStats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dataGridViewPlayerStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPlayerStats.Dock = DockStyle.Fill;
            dataGridViewPlayerStats.Location = new Point(3, 3);
            dataGridViewPlayerStats.Name = "dataGridViewPlayerStats";
            dataGridViewPlayerStats.ReadOnly = true;
            dataGridViewPlayerStats.Size = new Size(882, 322);
            dataGridViewPlayerStats.TabIndex = 0;
            // 
            // tabWeaponStats
            // 
            tabWeaponStats.Controls.Add(dataGridViewWeaponStats);
            tabWeaponStats.Location = new Point(4, 24);
            tabWeaponStats.Name = "tabWeaponStats";
            tabWeaponStats.Padding = new Padding(3);
            tabWeaponStats.Size = new Size(888, 328);
            tabWeaponStats.TabIndex = 1;
            tabWeaponStats.Text = "Weapon Stats";
            tabWeaponStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewWeaponStats
            // 
            dataGridViewWeaponStats.AllowUserToAddRows = false;
            dataGridViewWeaponStats.AllowUserToDeleteRows = false;
            dataGridViewWeaponStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWeaponStats.Dock = DockStyle.Fill;
            dataGridViewWeaponStats.Location = new Point(3, 3);
            dataGridViewWeaponStats.Name = "dataGridViewWeaponStats";
            dataGridViewWeaponStats.ReadOnly = true;
            dataGridViewWeaponStats.Size = new Size(882, 322);
            dataGridViewWeaponStats.TabIndex = 0;
            // 
            // tabBabstats
            // 
            tabBabstats.Controls.Add(tableLayoutPanel13);
            tabBabstats.Location = new Point(4, 24);
            tabBabstats.Name = "tabBabstats";
            tabBabstats.Size = new Size(888, 328);
            tabBabstats.TabIndex = 2;
            tabBabstats.Text = "Babstats";
            tabBabstats.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            tableLayoutPanel13.ColumnCount = 2;
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 645F));
            tableLayoutPanel13.Controls.Add(dg_statsLog, 1, 0);
            tableLayoutPanel13.Controls.Add(panel7, 0, 0);
            tableLayoutPanel13.Dock = DockStyle.Fill;
            tableLayoutPanel13.Location = new Point(0, 0);
            tableLayoutPanel13.Name = "tableLayoutPanel13";
            tableLayoutPanel13.RowCount = 1;
            tableLayoutPanel13.RowStyles.Add(new RowStyle());
            tableLayoutPanel13.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel13.Size = new Size(888, 328);
            tableLayoutPanel13.TabIndex = 0;
            // 
            // dg_statsLog
            // 
            dg_statsLog.AllowUserToAddRows = false;
            dg_statsLog.AllowUserToDeleteRows = false;
            dg_statsLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_statsLog.Columns.AddRange(new DataGridViewColumn[] { statsLog_DateTime, statLog_Message });
            dg_statsLog.Dock = DockStyle.Fill;
            dg_statsLog.Location = new Point(246, 3);
            dg_statsLog.Name = "dg_statsLog";
            dg_statsLog.ReadOnly = true;
            dg_statsLog.RowHeadersVisible = false;
            dg_statsLog.Size = new Size(639, 322);
            dg_statsLog.TabIndex = 0;
            // 
            // statsLog_DateTime
            // 
            statsLog_DateTime.HeaderText = "Time Stamp";
            statsLog_DateTime.Name = "statsLog_DateTime";
            statsLog_DateTime.ReadOnly = true;
            // 
            // statLog_Message
            // 
            statLog_Message.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            statLog_Message.HeaderText = "Log Message";
            statLog_Message.Name = "statLog_Message";
            statLog_Message.ReadOnly = true;
            // 
            // panel7
            // 
            panel7.Controls.Add(btn_SaveSettings);
            panel7.Controls.Add(labelReportInterval);
            panel7.Controls.Add(labelUpatedInterval);
            panel7.Controls.Add(num_WebStatsReport);
            panel7.Controls.Add(num_WebStatsUpdates);
            panel7.Controls.Add(cb_enableAnnouncements);
            panel7.Controls.Add(btn_validate);
            panel7.Controls.Add(label_webServerPath);
            panel7.Controls.Add(tb_webStatsServerPath);
            panel7.Controls.Add(cb_enableWebStats);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(237, 322);
            panel7.TabIndex = 1;
            // 
            // btn_SaveSettings
            // 
            btn_SaveSettings.Location = new Point(84, 197);
            btn_SaveSettings.Name = "btn_SaveSettings";
            btn_SaveSettings.Size = new Size(75, 23);
            btn_SaveSettings.TabIndex = 9;
            btn_SaveSettings.Text = "Save";
            btn_SaveSettings.UseVisualStyleBackColor = true;
            // 
            // labelReportInterval
            // 
            labelReportInterval.AutoSize = true;
            labelReportInterval.Location = new Point(13, 136);
            labelReportInterval.Name = "labelReportInterval";
            labelReportInterval.Size = new Size(112, 15);
            labelReportInterval.TabIndex = 8;
            labelReportInterval.Text = "Stats Report Interval";
            labelReportInterval.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelUpatedInterval
            // 
            labelUpatedInterval.AutoSize = true;
            labelUpatedInterval.Location = new Point(13, 165);
            labelUpatedInterval.Name = "labelUpatedInterval";
            labelUpatedInterval.Size = new Size(115, 15);
            labelUpatedInterval.TabIndex = 7;
            labelUpatedInterval.Text = "Stats Update Interval";
            labelUpatedInterval.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // num_WebStatsReport
            // 
            num_WebStatsReport.Location = new Point(134, 132);
            num_WebStatsReport.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            num_WebStatsReport.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            num_WebStatsReport.Name = "num_WebStatsReport";
            num_WebStatsReport.Size = new Size(55, 23);
            num_WebStatsReport.TabIndex = 6;
            num_WebStatsReport.TextAlign = HorizontalAlignment.Center;
            num_WebStatsReport.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // num_WebStatsUpdates
            // 
            num_WebStatsUpdates.Location = new Point(134, 161);
            num_WebStatsUpdates.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            num_WebStatsUpdates.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            num_WebStatsUpdates.Name = "num_WebStatsUpdates";
            num_WebStatsUpdates.Size = new Size(55, 23);
            num_WebStatsUpdates.TabIndex = 5;
            num_WebStatsUpdates.TextAlign = HorizontalAlignment.Center;
            num_WebStatsUpdates.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // cb_enableAnnouncements
            // 
            cb_enableAnnouncements.AutoSize = true;
            cb_enableAnnouncements.CheckAlign = ContentAlignment.MiddleRight;
            cb_enableAnnouncements.Location = new Point(13, 107);
            cb_enableAnnouncements.Name = "cb_enableAnnouncements";
            cb_enableAnnouncements.Size = new Size(152, 19);
            cb_enableAnnouncements.TabIndex = 4;
            cb_enableAnnouncements.Text = "Enable Announcements";
            cb_enableAnnouncements.UseVisualStyleBackColor = true;
            cb_enableAnnouncements.CheckedChanged += ActionEvent_EnableAnnouncements;
            // 
            // btn_validate
            // 
            btn_validate.Location = new Point(151, 75);
            btn_validate.Name = "btn_validate";
            btn_validate.Size = new Size(75, 23);
            btn_validate.TabIndex = 3;
            btn_validate.Text = "Validate";
            btn_validate.UseVisualStyleBackColor = true;
            btn_validate.Click += ActionEvent_TestBabstatConnection;
            // 
            // label_webServerPath
            // 
            label_webServerPath.AutoSize = true;
            label_webServerPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_webServerPath.Location = new Point(13, 72);
            label_webServerPath.Name = "label_webServerPath";
            label_webServerPath.Size = new Size(113, 13);
            label_webServerPath.TabIndex = 2;
            label_webServerPath.Text = "Babstats Server Path";
            // 
            // tb_webStatsServerPath
            // 
            tb_webStatsServerPath.Location = new Point(3, 46);
            tb_webStatsServerPath.Name = "tb_webStatsServerPath";
            tb_webStatsServerPath.PlaceholderText = "http(s)://youdomain.com/babstats_root/";
            tb_webStatsServerPath.Size = new Size(231, 23);
            tb_webStatsServerPath.TabIndex = 1;
            tb_webStatsServerPath.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_enableWebStats
            // 
            cb_enableWebStats.AutoSize = true;
            cb_enableWebStats.Location = new Point(13, 16);
            cb_enableWebStats.Name = "cb_enableWebStats";
            cb_enableWebStats.Size = new Size(163, 19);
            cb_enableWebStats.TabIndex = 0;
            cb_enableWebStats.Text = "Enable Babstats Reporting";
            cb_enableWebStats.UseVisualStyleBackColor = true;
            cb_enableWebStats.CheckedChanged += ActionEvent_EnableBabStats;
            // 
            // tabAdmin
            // 
            tabAdmin.Controls.Add(tableAdminPanel);
            tabAdmin.Location = new Point(4, 24);
            tabAdmin.Name = "tabAdmin";
            tabAdmin.Padding = new Padding(3);
            tabAdmin.Size = new Size(902, 362);
            tabAdmin.TabIndex = 7;
            tabAdmin.Text = "Admin";
            tabAdmin.UseVisualStyleBackColor = true;
            // 
            // tableAdminPanel
            // 
            tableAdminPanel.ColumnCount = 2;
            tableAdminPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableAdminPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 550F));
            tableAdminPanel.Controls.Add(dg_adminLog, 1, 0);
            tableAdminPanel.Controls.Add(tableLayoutPanel14, 0, 0);
            tableAdminPanel.Dock = DockStyle.Fill;
            tableAdminPanel.Location = new Point(3, 3);
            tableAdminPanel.Name = "tableAdminPanel";
            tableAdminPanel.RowCount = 1;
            tableAdminPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAdminPanel.Size = new Size(896, 356);
            tableAdminPanel.TabIndex = 0;
            // 
            // dg_adminLog
            // 
            dg_adminLog.AllowUserToAddRows = false;
            dg_adminLog.AllowUserToDeleteRows = false;
            dg_adminLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_adminLog.Columns.AddRange(new DataGridViewColumn[] { adminLog_datetime, adminLog_username, adminLog_log });
            dg_adminLog.Dock = DockStyle.Fill;
            dg_adminLog.Location = new Point(349, 3);
            dg_adminLog.Name = "dg_adminLog";
            dg_adminLog.ReadOnly = true;
            dg_adminLog.RowHeadersVisible = false;
            dg_adminLog.Size = new Size(544, 350);
            dg_adminLog.TabIndex = 0;
            // 
            // adminLog_datetime
            // 
            adminLog_datetime.HeaderText = "Time Stamp";
            adminLog_datetime.MinimumWidth = 100;
            adminLog_datetime.Name = "adminLog_datetime";
            adminLog_datetime.ReadOnly = true;
            // 
            // adminLog_username
            // 
            adminLog_username.HeaderText = "User";
            adminLog_username.MinimumWidth = 100;
            adminLog_username.Name = "adminLog_username";
            adminLog_username.ReadOnly = true;
            // 
            // adminLog_log
            // 
            adminLog_log.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            adminLog_log.HeaderText = "Log";
            adminLog_log.Name = "adminLog_log";
            adminLog_log.ReadOnly = true;
            // 
            // tableLayoutPanel14
            // 
            tableLayoutPanel14.ColumnCount = 1;
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel14.Controls.Add(dg_AdminUsers, 0, 0);
            tableLayoutPanel14.Controls.Add(tableLayoutPanel15, 0, 1);
            tableLayoutPanel14.Dock = DockStyle.Fill;
            tableLayoutPanel14.Location = new Point(0, 0);
            tableLayoutPanel14.Margin = new Padding(0);
            tableLayoutPanel14.Name = "tableLayoutPanel14";
            tableLayoutPanel14.RowCount = 2;
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 57.55814F));
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 42.44186F));
            tableLayoutPanel14.Size = new Size(346, 356);
            tableLayoutPanel14.TabIndex = 1;
            // 
            // dg_AdminUsers
            // 
            dg_AdminUsers.AllowUserToAddRows = false;
            dg_AdminUsers.AllowUserToDeleteRows = false;
            dg_AdminUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_AdminUsers.Columns.AddRange(new DataGridViewColumn[] { admin_id, admin_username, admin_role });
            dg_AdminUsers.Dock = DockStyle.Fill;
            dg_AdminUsers.Location = new Point(3, 3);
            dg_AdminUsers.Name = "dg_AdminUsers";
            dg_AdminUsers.ReadOnly = true;
            dg_AdminUsers.RowHeadersVisible = false;
            dg_AdminUsers.Size = new Size(340, 198);
            dg_AdminUsers.TabIndex = 0;
            dg_AdminUsers.CellClick += ActionClick_SelectAdmin;
            // 
            // admin_id
            // 
            admin_id.HeaderText = "ID";
            admin_id.Name = "admin_id";
            admin_id.ReadOnly = true;
            admin_id.Width = 50;
            // 
            // admin_username
            // 
            admin_username.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            admin_username.HeaderText = "Username";
            admin_username.Name = "admin_username";
            admin_username.ReadOnly = true;
            // 
            // admin_role
            // 
            admin_role.HeaderText = "Role";
            admin_role.MinimumWidth = 100;
            admin_role.Name = "admin_role";
            admin_role.ReadOnly = true;
            // 
            // tableLayoutPanel15
            // 
            tableLayoutPanel15.ColumnCount = 1;
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel15.Controls.Add(tableLayoutPanel16, 0, 3);
            tableLayoutPanel15.Controls.Add(tableLayoutPanel17, 0, 0);
            tableLayoutPanel15.Controls.Add(tableLayoutPanel18, 0, 1);
            tableLayoutPanel15.Controls.Add(tableLayoutPanel19, 0, 2);
            tableLayoutPanel15.Dock = DockStyle.Fill;
            tableLayoutPanel15.Location = new Point(0, 209);
            tableLayoutPanel15.Margin = new Padding(0, 5, 0, 5);
            tableLayoutPanel15.Name = "tableLayoutPanel15";
            tableLayoutPanel15.RowCount = 4;
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel15.Size = new Size(346, 142);
            tableLayoutPanel15.TabIndex = 1;
            // 
            // tableLayoutPanel16
            // 
            tableLayoutPanel16.ColumnCount = 4;
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel16.Controls.Add(btn_adminNew, 0, 0);
            tableLayoutPanel16.Controls.Add(btn_adminAdd, 1, 0);
            tableLayoutPanel16.Controls.Add(btn_adminSave, 2, 0);
            tableLayoutPanel16.Controls.Add(btn_adminDelete, 3, 0);
            tableLayoutPanel16.Dock = DockStyle.Fill;
            tableLayoutPanel16.Location = new Point(0, 105);
            tableLayoutPanel16.Margin = new Padding(0);
            tableLayoutPanel16.Name = "tableLayoutPanel16";
            tableLayoutPanel16.RowCount = 1;
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel16.Size = new Size(346, 37);
            tableLayoutPanel16.TabIndex = 0;
            // 
            // btn_adminNew
            // 
            btn_adminNew.Dock = DockStyle.Fill;
            btn_adminNew.Location = new Point(3, 3);
            btn_adminNew.Name = "btn_adminNew";
            btn_adminNew.Size = new Size(80, 31);
            btn_adminNew.TabIndex = 0;
            btn_adminNew.Text = "New";
            btn_adminNew.UseVisualStyleBackColor = true;
            btn_adminNew.Click += ActionClick_AdminNewUser;
            // 
            // btn_adminAdd
            // 
            btn_adminAdd.Dock = DockStyle.Fill;
            btn_adminAdd.Location = new Point(89, 3);
            btn_adminAdd.Name = "btn_adminAdd";
            btn_adminAdd.Size = new Size(80, 31);
            btn_adminAdd.TabIndex = 1;
            btn_adminAdd.Text = "Add";
            btn_adminAdd.UseVisualStyleBackColor = true;
            btn_adminAdd.Click += ActionClick_AdminAddNew;
            // 
            // btn_adminSave
            // 
            btn_adminSave.Dock = DockStyle.Fill;
            btn_adminSave.Location = new Point(175, 3);
            btn_adminSave.Name = "btn_adminSave";
            btn_adminSave.Size = new Size(80, 31);
            btn_adminSave.TabIndex = 2;
            btn_adminSave.Text = "Save";
            btn_adminSave.UseVisualStyleBackColor = true;
            btn_adminSave.Click += ActionClick_AdminEditUser;
            // 
            // btn_adminDelete
            // 
            btn_adminDelete.AccessibleDescription = " c";
            btn_adminDelete.Dock = DockStyle.Fill;
            btn_adminDelete.Location = new Point(261, 3);
            btn_adminDelete.Name = "btn_adminDelete";
            btn_adminDelete.Size = new Size(82, 31);
            btn_adminDelete.TabIndex = 3;
            btn_adminDelete.Text = "Delete";
            btn_adminDelete.UseVisualStyleBackColor = true;
            btn_adminDelete.Click += ActionClick_AdminDelete;
            // 
            // tableLayoutPanel17
            // 
            tableLayoutPanel17.ColumnCount = 3;
            tableLayoutPanel17.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel17.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel17.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel17.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel17.Controls.Add(cb_adminRole, 1, 0);
            tableLayoutPanel17.Dock = DockStyle.Fill;
            tableLayoutPanel17.Location = new Point(0, 0);
            tableLayoutPanel17.Margin = new Padding(0);
            tableLayoutPanel17.Name = "tableLayoutPanel17";
            tableLayoutPanel17.RowCount = 1;
            tableLayoutPanel17.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel17.Size = new Size(346, 35);
            tableLayoutPanel17.TabIndex = 1;
            // 
            // cb_adminRole
            // 
            cb_adminRole.Dock = DockStyle.Fill;
            cb_adminRole.FormattingEnabled = true;
            cb_adminRole.Items.AddRange(new object[] { "No Access", "Moderator", "Administrator" });
            cb_adminRole.Location = new Point(69, 10);
            cb_adminRole.Margin = new Padding(0, 10, 0, 0);
            cb_adminRole.Name = "cb_adminRole";
            cb_adminRole.Size = new Size(207, 23);
            cb_adminRole.TabIndex = 1;
            cb_adminRole.Text = "Select Role";
            // 
            // tableLayoutPanel18
            // 
            tableLayoutPanel18.ColumnCount = 3;
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel18.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel18.Controls.Add(tb_adminUser, 1, 0);
            tableLayoutPanel18.Dock = DockStyle.Fill;
            tableLayoutPanel18.Location = new Point(0, 35);
            tableLayoutPanel18.Margin = new Padding(0);
            tableLayoutPanel18.Name = "tableLayoutPanel18";
            tableLayoutPanel18.RowCount = 1;
            tableLayoutPanel18.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel18.Size = new Size(346, 35);
            tableLayoutPanel18.TabIndex = 2;
            // 
            // tb_adminUser
            // 
            tb_adminUser.Dock = DockStyle.Fill;
            tb_adminUser.Location = new Point(69, 10);
            tb_adminUser.Margin = new Padding(0, 10, 0, 0);
            tb_adminUser.Name = "tb_adminUser";
            tb_adminUser.PlaceholderText = "Username";
            tb_adminUser.Size = new Size(207, 23);
            tb_adminUser.TabIndex = 0;
            tb_adminUser.TextAlign = HorizontalAlignment.Center;
            // 
            // tableLayoutPanel19
            // 
            tableLayoutPanel19.ColumnCount = 3;
            tableLayoutPanel19.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel19.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel19.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel19.Controls.Add(tb_adminPass, 1, 0);
            tableLayoutPanel19.Dock = DockStyle.Fill;
            tableLayoutPanel19.Location = new Point(0, 70);
            tableLayoutPanel19.Margin = new Padding(0);
            tableLayoutPanel19.Name = "tableLayoutPanel19";
            tableLayoutPanel19.RowCount = 1;
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel19.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel19.Size = new Size(346, 35);
            tableLayoutPanel19.TabIndex = 3;
            // 
            // tb_adminPass
            // 
            tb_adminPass.Dock = DockStyle.Fill;
            tb_adminPass.Location = new Point(69, 10);
            tb_adminPass.Margin = new Padding(0, 10, 0, 0);
            tb_adminPass.Name = "tb_adminPass";
            tb_adminPass.PasswordChar = '*';
            tb_adminPass.PlaceholderText = "Password";
            tb_adminPass.Size = new Size(207, 23);
            tb_adminPass.TabIndex = 0;
            tb_adminPass.TextAlign = HorizontalAlignment.Center;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // ServerManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 425);
            Controls.Add(mainPanel);
            Controls.Add(toolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ServerManager";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Black Hawk Down Server Manager";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            mainPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabBans.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_playerNames).EndInit();
            groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_IPAddresses).EndInit();
            panel8.ResumeLayout(false);
            groupBox10.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tabStats.ResumeLayout(false);
            tabStatsControl.ResumeLayout(false);
            tabPlayerStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).EndInit();
            tabWeaponStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).EndInit();
            tabBabstats.ResumeLayout(false);
            tableLayoutPanel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_statsLog).EndInit();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).EndInit();
            tabAdmin.ResumeLayout(false);
            tableAdminPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_adminLog).EndInit();
            tableLayoutPanel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_AdminUsers).EndInit();
            tableLayoutPanel15.ResumeLayout(false);
            tableLayoutPanel16.ResumeLayout(false);
            tableLayoutPanel17.ResumeLayout(false);
            tableLayoutPanel18.ResumeLayout(false);
            tableLayoutPanel18.PerformLayout();
            tableLayoutPanel19.ResumeLayout(false);
            tableLayoutPanel19.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip;
        private Panel mainPanel;
        private TabPage tabServer;
        private TabPage tabMaps;
        private TabPage tabPlayers;
        private OpenFileDialog openFileDialog;
        private ToolTip toolTip;
        internal ToolStripLabel toolStripStatus;
        private TabPage tabBans;
        private TableLayoutPanel tableLayoutPanel5;
        private GroupBox groupBox3;
        private GroupBox groupBox8;
        private DataGridViewTextBoxColumn playerRecordID;
        private DataGridViewTextBoxColumn playerName;
        private DataGridViewTextBoxColumn ipRecordID;
        private DataGridViewTextBoxColumn address;
        internal TabControl tabControl;
        private TabPage tabStats;
        private TabControl tabStatsControl;
        private TabPage tabPlayerStats;
        private TabPage tabWeaponStats;
        private TabPage tabBabstats;
        internal DataGridView dataGridViewPlayerStats;
        internal DataGridView dataGridViewWeaponStats;
        private TableLayoutPanel tableLayoutPanel13;
        private DataGridViewTextBoxColumn statsLog_DateTime;
        private DataGridViewTextBoxColumn statLog_Message;
        private Panel panel7;
        private Label label_webServerPath;
        private Label labelReportInterval;
        private Label labelUpatedInterval;
        public DataGridView dg_statsLog;
        public CheckBox cb_enableWebStats;
        public TextBox tb_webStatsServerPath;
        public CheckBox cb_enableAnnouncements;
        public Button btn_validate;
        public NumericUpDown num_WebStatsReport;
        public NumericUpDown num_WebStatsUpdates;
        private Button btn_SaveSettings;
        private TabPage tabAdmin;
        private TableLayoutPanel tableAdminPanel;
        public DataGridView dg_adminLog;
        private TableLayoutPanel tableLayoutPanel14;
        private DataGridViewTextBoxColumn adminLog_datetime;
        private DataGridViewTextBoxColumn adminLog_username;
        private DataGridViewTextBoxColumn adminLog_log;
        private DataGridViewTextBoxColumn admin_id;
        private DataGridViewTextBoxColumn admin_username;
        private DataGridViewTextBoxColumn admin_role;
        private TableLayoutPanel tableLayoutPanel15;
        private TableLayoutPanel tableLayoutPanel16;
        private Button btn_adminNew;
        private Button btn_adminAdd;
        private Button btn_adminSave;
        private Button btn_adminDelete;
        private TableLayoutPanel tableLayoutPanel17;
        private TableLayoutPanel tableLayoutPanel18;
        private TableLayoutPanel tableLayoutPanel19;
        private ComboBox cb_adminRole;
        private TextBox tb_adminUser;
        private TextBox tb_adminPass;
        public DataGridView dg_playerNames;
        public DataGridView dg_IPAddresses;
        public DataGridView dg_AdminUsers;
        private GroupBox groupBox9;
        private TableLayoutPanel tableLayoutPanel7;
        internal TextBox tb_bansPlayerName;
        internal TextBox tb_bansIPAddress;
        private ComboBox cb_banSubMask;
        private Button btn_addBan;
        private Panel panel8;
        private GroupBox groupBox10;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btn_banExport;
        private Button btn_banImport;
        private TabPage tabProfile;
        private ToolStripLabel label_TimeLeft;
        private ToolStripLabel label_WinCondition;
        private ToolStripLabel label_RedScore;
        private ToolStripLabel label_BlueScore;
        private ToolStripLabel label_PlayersOnline;
        private TabPage tabChat;
    }
}
