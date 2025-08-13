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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            toolStripStatus = new ToolStripLabel();
            mainPanel = new Panel();
            tabControl = new TabControl();
            tabProfile = new TabPage();
            tabServer = new TabPage();
            tabMaps = new TabPage();
            mapsTable = new TableLayoutPanel();
            mapPanel1 = new Panel();
            mapsPanel1_table = new TableLayoutPanel();
            combo_gameTypes = new ComboBox();
            dataGridView_availableMaps = new DataGridView();
            mapsPanel2 = new Panel();
            mapsPanel2_table = new TableLayoutPanel();
            dataGridView_currentMaps = new DataGridView();
            tableMapControls = new TableLayoutPanel();
            ib_resetCurrentMaps = new FontAwesome.Sharp.IconButton();
            ib_exportMapList = new FontAwesome.Sharp.IconButton();
            ib_importMapList = new FontAwesome.Sharp.IconButton();
            ib_mapRefresh = new FontAwesome.Sharp.IconButton();
            ib_clearMapList = new FontAwesome.Sharp.IconButton();
            panel1 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btn_mapsUpdate = new Button();
            btn_mapsPlayNext = new Button();
            btn_mapsScore = new Button();
            btn_mapsSkip = new Button();
            panel4 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label6 = new Label();
            label_dataCurrentMap = new Label();
            label7 = new Label();
            label_dataNextMap = new Label();
            label8 = new Label();
            label_dataTimeLeft = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            tabPlayers = new TabPage();
            panel6 = new Panel();
            player_LayoutPanel = new FlowLayoutPanel();
            tabChat = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            tb_chatMessage = new TextBox();
            btn_sendChat = new Button();
            panel5 = new Panel();
            dataGridView_chatMessages = new DataGridView();
            dateTime = new DataGridViewTextBoxColumn();
            Team = new DataGridViewTextBoxColumn();
            Player = new DataGridViewTextBoxColumn();
            Message = new DataGridViewTextBoxColumn();
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
            tabMessages = new TabPage();
            tabMessagesControl = new TabControl();
            tabAuto = new TabPage();
            tableLayoutPanel9 = new TableLayoutPanel();
            dg_autoMessages = new DataGridView();
            autoMessageID = new DataGridViewTextBoxColumn();
            autoTrigger = new DataGridViewTextBoxColumn();
            autoMessageText = new DataGridViewTextBoxColumn();
            tableLayoutPanel12 = new TableLayoutPanel();
            tb_autoMessage = new TextBox();
            btn_addAutoMessage = new Button();
            num_AutoMessageTrigger = new NumericUpDown();
            tabSlaps = new TabPage();
            tableLayoutPanel10 = new TableLayoutPanel();
            dg_slapMessages = new DataGridView();
            slapMessageID = new DataGridViewTextBoxColumn();
            slapMessages = new DataGridViewTextBoxColumn();
            tableLayoutPanel11 = new TableLayoutPanel();
            tb_slapMessage = new TextBox();
            btn_addSlap = new Button();
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
            tabMaps.SuspendLayout();
            mapsTable.SuspendLayout();
            mapPanel1.SuspendLayout();
            mapsPanel1_table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_availableMaps).BeginInit();
            mapsPanel2.SuspendLayout();
            mapsPanel2_table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_currentMaps).BeginInit();
            tableMapControls.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tabPlayers.SuspendLayout();
            tabChat.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_chatMessages).BeginInit();
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
            tabMessages.SuspendLayout();
            tabMessagesControl.SuspendLayout();
            tabAuto.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_autoMessages).BeginInit();
            tableLayoutPanel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_AutoMessageTrigger).BeginInit();
            tabSlaps.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_slapMessages).BeginInit();
            tableLayoutPanel11.SuspendLayout();
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
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripStatus });
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
            tabControl.Controls.Add(tabMessages);
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
            tabMaps.Controls.Add(mapsTable);
            tabMaps.Location = new Point(4, 24);
            tabMaps.Name = "tabMaps";
            tabMaps.Padding = new Padding(3);
            tabMaps.Size = new Size(902, 362);
            tabMaps.TabIndex = 1;
            tabMaps.Text = "Maps";
            tabMaps.UseVisualStyleBackColor = true;
            // 
            // mapsTable
            // 
            mapsTable.ColumnCount = 4;
            mapsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            mapsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            mapsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            mapsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mapsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            mapsTable.Controls.Add(mapPanel1, 0, 0);
            mapsTable.Controls.Add(mapsPanel2, 2, 0);
            mapsTable.Controls.Add(panel1, 1, 0);
            mapsTable.Controls.Add(panel4, 3, 0);
            mapsTable.Dock = DockStyle.Fill;
            mapsTable.Location = new Point(3, 3);
            mapsTable.Name = "mapsTable";
            mapsTable.RowCount = 1;
            mapsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mapsTable.Size = new Size(896, 356);
            mapsTable.TabIndex = 0;
            // 
            // mapPanel1
            // 
            mapPanel1.Controls.Add(mapsPanel1_table);
            mapPanel1.Dock = DockStyle.Fill;
            mapPanel1.Location = new Point(3, 3);
            mapPanel1.Name = "mapPanel1";
            mapPanel1.Size = new Size(244, 350);
            mapPanel1.TabIndex = 0;
            // 
            // mapsPanel1_table
            // 
            mapsPanel1_table.ColumnCount = 1;
            mapsPanel1_table.ColumnStyles.Add(new ColumnStyle());
            mapsPanel1_table.Controls.Add(combo_gameTypes, 0, 0);
            mapsPanel1_table.Controls.Add(dataGridView_availableMaps, 0, 1);
            mapsPanel1_table.Dock = DockStyle.Fill;
            mapsPanel1_table.Location = new Point(0, 0);
            mapsPanel1_table.Name = "mapsPanel1_table";
            mapsPanel1_table.RowCount = 2;
            mapsPanel1_table.RowStyles.Add(new RowStyle(SizeType.Percent, 8.46947F));
            mapsPanel1_table.RowStyles.Add(new RowStyle(SizeType.Percent, 91.53053F));
            mapsPanel1_table.Size = new Size(244, 350);
            mapsPanel1_table.TabIndex = 0;
            // 
            // combo_gameTypes
            // 
            combo_gameTypes.Dock = DockStyle.Fill;
            combo_gameTypes.FormattingEnabled = true;
            combo_gameTypes.Items.AddRange(new object[] { "All Game Modes" });
            combo_gameTypes.Location = new Point(3, 3);
            combo_gameTypes.Name = "combo_gameTypes";
            combo_gameTypes.Size = new Size(238, 23);
            combo_gameTypes.TabIndex = 0;
            combo_gameTypes.SelectedIndexChanged += actionChange_mapFilterGameType;
            // 
            // dataGridView_availableMaps
            // 
            dataGridView_availableMaps.AllowUserToAddRows = false;
            dataGridView_availableMaps.AllowUserToDeleteRows = false;
            dataGridView_availableMaps.AllowUserToResizeColumns = false;
            dataGridView_availableMaps.AllowUserToResizeRows = false;
            dataGridView_availableMaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_availableMaps.Dock = DockStyle.Fill;
            dataGridView_availableMaps.Location = new Point(3, 32);
            dataGridView_availableMaps.MultiSelect = false;
            dataGridView_availableMaps.Name = "dataGridView_availableMaps";
            dataGridView_availableMaps.ReadOnly = true;
            dataGridView_availableMaps.RowHeadersVisible = false;
            dataGridView_availableMaps.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView_availableMaps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_availableMaps.ShowEditingIcon = false;
            dataGridView_availableMaps.Size = new Size(238, 315);
            dataGridView_availableMaps.TabIndex = 1;
            dataGridView_availableMaps.CellDoubleClick += actionClick_addMap2Current;
            // 
            // mapsPanel2
            // 
            mapsPanel2.Controls.Add(mapsPanel2_table);
            mapsPanel2.Dock = DockStyle.Fill;
            mapsPanel2.Location = new Point(353, 3);
            mapsPanel2.Name = "mapsPanel2";
            mapsPanel2.Size = new Size(244, 350);
            mapsPanel2.TabIndex = 2;
            // 
            // mapsPanel2_table
            // 
            mapsPanel2_table.ColumnCount = 1;
            mapsPanel2_table.ColumnStyles.Add(new ColumnStyle());
            mapsPanel2_table.Controls.Add(dataGridView_currentMaps, 0, 1);
            mapsPanel2_table.Controls.Add(tableMapControls, 0, 0);
            mapsPanel2_table.Dock = DockStyle.Fill;
            mapsPanel2_table.Location = new Point(0, 0);
            mapsPanel2_table.Name = "mapsPanel2_table";
            mapsPanel2_table.RowCount = 2;
            mapsPanel2_table.RowStyles.Add(new RowStyle(SizeType.Percent, 8.519183F));
            mapsPanel2_table.RowStyles.Add(new RowStyle(SizeType.Percent, 91.48081F));
            mapsPanel2_table.Size = new Size(244, 350);
            mapsPanel2_table.TabIndex = 1;
            // 
            // dataGridView_currentMaps
            // 
            dataGridView_currentMaps.AllowUserToAddRows = false;
            dataGridView_currentMaps.AllowUserToResizeColumns = false;
            dataGridView_currentMaps.AllowUserToResizeRows = false;
            dataGridView_currentMaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_currentMaps.Dock = DockStyle.Fill;
            dataGridView_currentMaps.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_currentMaps.Location = new Point(3, 32);
            dataGridView_currentMaps.MultiSelect = false;
            dataGridView_currentMaps.Name = "dataGridView_currentMaps";
            dataGridView_currentMaps.RowHeadersVisible = false;
            dataGridView_currentMaps.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView_currentMaps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_currentMaps.ShowEditingIcon = false;
            dataGridView_currentMaps.Size = new Size(238, 315);
            dataGridView_currentMaps.TabIndex = 0;
            dataGridView_currentMaps.CellDoubleClick += actionClick_delRowFromCurrent;
            dataGridView_currentMaps.KeyPress += actionKeyEvent_moveMap;
            // 
            // tableMapControls
            // 
            tableMapControls.ColumnCount = 6;
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableMapControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            tableMapControls.Controls.Add(ib_resetCurrentMaps, 5, 0);
            tableMapControls.Controls.Add(ib_exportMapList, 4, 0);
            tableMapControls.Controls.Add(ib_importMapList, 3, 0);
            tableMapControls.Controls.Add(ib_mapRefresh, 0, 0);
            tableMapControls.Controls.Add(ib_clearMapList, 2, 0);
            tableMapControls.Dock = DockStyle.Fill;
            tableMapControls.Location = new Point(3, 3);
            tableMapControls.Name = "tableMapControls";
            tableMapControls.RowCount = 1;
            tableMapControls.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMapControls.Size = new Size(238, 23);
            tableMapControls.TabIndex = 1;
            // 
            // ib_resetCurrentMaps
            // 
            ib_resetCurrentMaps.Dock = DockStyle.Fill;
            ib_resetCurrentMaps.FlatStyle = FlatStyle.Flat;
            ib_resetCurrentMaps.IconChar = FontAwesome.Sharp.IconChar.Reply;
            ib_resetCurrentMaps.IconColor = Color.Black;
            ib_resetCurrentMaps.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_resetCurrentMaps.IconSize = 24;
            ib_resetCurrentMaps.Location = new Point(196, 0);
            ib_resetCurrentMaps.Margin = new Padding(0);
            ib_resetCurrentMaps.Name = "ib_resetCurrentMaps";
            ib_resetCurrentMaps.Padding = new Padding(0, 2, 0, 0);
            ib_resetCurrentMaps.Size = new Size(42, 23);
            ib_resetCurrentMaps.TabIndex = 5;
            toolTip.SetToolTip(ib_resetCurrentMaps, "Reset Current Lists");
            ib_resetCurrentMaps.UseVisualStyleBackColor = true;
            ib_resetCurrentMaps.Click += actionClick_resetCurrentMapList;
            // 
            // ib_exportMapList
            // 
            ib_exportMapList.Dock = DockStyle.Fill;
            ib_exportMapList.FlatStyle = FlatStyle.Flat;
            ib_exportMapList.IconChar = FontAwesome.Sharp.IconChar.BoxesPacking;
            ib_exportMapList.IconColor = Color.Black;
            ib_exportMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_exportMapList.IconSize = 24;
            ib_exportMapList.Location = new Point(156, 0);
            ib_exportMapList.Margin = new Padding(0);
            ib_exportMapList.Name = "ib_exportMapList";
            ib_exportMapList.Padding = new Padding(0, 2, 0, 0);
            ib_exportMapList.Size = new Size(40, 23);
            ib_exportMapList.TabIndex = 4;
            toolTip.SetToolTip(ib_exportMapList, "Export Map List");
            ib_exportMapList.UseVisualStyleBackColor = true;
            ib_exportMapList.Click += actionClick_exportCurrentPlaylist;
            // 
            // ib_importMapList
            // 
            ib_importMapList.Dock = DockStyle.Fill;
            ib_importMapList.FlatStyle = FlatStyle.Flat;
            ib_importMapList.IconChar = FontAwesome.Sharp.IconChar.BoxOpen;
            ib_importMapList.IconColor = Color.Black;
            ib_importMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_importMapList.IconSize = 24;
            ib_importMapList.Location = new Point(116, 0);
            ib_importMapList.Margin = new Padding(0);
            ib_importMapList.Name = "ib_importMapList";
            ib_importMapList.Padding = new Padding(0, 2, 0, 0);
            ib_importMapList.Size = new Size(40, 23);
            ib_importMapList.TabIndex = 3;
            toolTip.SetToolTip(ib_importMapList, "Import Map List");
            ib_importMapList.UseVisualStyleBackColor = true;
            ib_importMapList.Click += actionClick_importMapPlaylist;
            // 
            // ib_mapRefresh
            // 
            ib_mapRefresh.Dock = DockStyle.Fill;
            ib_mapRefresh.FlatStyle = FlatStyle.Flat;
            ib_mapRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            ib_mapRefresh.IconColor = Color.Black;
            ib_mapRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_mapRefresh.IconSize = 24;
            ib_mapRefresh.Location = new Point(0, 0);
            ib_mapRefresh.Margin = new Padding(0);
            ib_mapRefresh.Name = "ib_mapRefresh";
            ib_mapRefresh.Padding = new Padding(0, 2, 0, 0);
            ib_mapRefresh.Size = new Size(38, 23);
            ib_mapRefresh.TabIndex = 0;
            toolTip.SetToolTip(ib_mapRefresh, "Refresh Available Maps");
            ib_mapRefresh.UseVisualStyleBackColor = true;
            ib_mapRefresh.Click += actionClick_refreshMaps;
            // 
            // ib_clearMapList
            // 
            ib_clearMapList.Dock = DockStyle.Fill;
            ib_clearMapList.FlatStyle = FlatStyle.Flat;
            ib_clearMapList.IconChar = FontAwesome.Sharp.IconChar.Eraser;
            ib_clearMapList.IconColor = Color.Black;
            ib_clearMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_clearMapList.IconSize = 24;
            ib_clearMapList.Location = new Point(76, 0);
            ib_clearMapList.Margin = new Padding(0);
            ib_clearMapList.Name = "ib_clearMapList";
            ib_clearMapList.Padding = new Padding(0, 2, 0, 0);
            ib_clearMapList.Size = new Size(40, 23);
            ib_clearMapList.TabIndex = 2;
            toolTip.SetToolTip(ib_clearMapList, "Clear Current Map List");
            ib_clearMapList.UseVisualStyleBackColor = true;
            ib_clearMapList.Click += actionClick_clearCurrentMapPlaylist;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(253, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(94, 350);
            panel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(btn_mapsUpdate, 0, 1);
            tableLayoutPanel2.Controls.Add(btn_mapsPlayNext, 0, 2);
            tableLayoutPanel2.Controls.Add(btn_mapsScore, 0, 3);
            tableLayoutPanel2.Controls.Add(btn_mapsSkip, 0, 4);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(94, 350);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_mapsUpdate
            // 
            btn_mapsUpdate.Dock = DockStyle.Fill;
            btn_mapsUpdate.Location = new Point(3, 78);
            btn_mapsUpdate.Name = "btn_mapsUpdate";
            btn_mapsUpdate.Size = new Size(88, 44);
            btn_mapsUpdate.TabIndex = 0;
            btn_mapsUpdate.Text = "Update && Save Maps";
            btn_mapsUpdate.UseVisualStyleBackColor = true;
            btn_mapsUpdate.Click += actionClick_updateGameServerMaps;
            // 
            // btn_mapsPlayNext
            // 
            btn_mapsPlayNext.Dock = DockStyle.Fill;
            btn_mapsPlayNext.Location = new Point(3, 128);
            btn_mapsPlayNext.Name = "btn_mapsPlayNext";
            btn_mapsPlayNext.Size = new Size(88, 44);
            btn_mapsPlayNext.TabIndex = 1;
            btn_mapsPlayNext.Text = "Play Next";
            btn_mapsPlayNext.UseVisualStyleBackColor = true;
            btn_mapsPlayNext.Click += actionClick_mapPlayNext;
            // 
            // btn_mapsScore
            // 
            btn_mapsScore.Dock = DockStyle.Fill;
            btn_mapsScore.Location = new Point(3, 178);
            btn_mapsScore.Name = "btn_mapsScore";
            btn_mapsScore.Size = new Size(88, 44);
            btn_mapsScore.TabIndex = 2;
            btn_mapsScore.Text = "Score Map";
            btn_mapsScore.UseVisualStyleBackColor = true;
            btn_mapsScore.Click += actionClick_mapScore;
            // 
            // btn_mapsSkip
            // 
            btn_mapsSkip.Dock = DockStyle.Fill;
            btn_mapsSkip.Location = new Point(3, 228);
            btn_mapsSkip.Name = "btn_mapsSkip";
            btn_mapsSkip.Size = new Size(88, 44);
            btn_mapsSkip.TabIndex = 3;
            btn_mapsSkip.Text = "Skip Map";
            btn_mapsSkip.UseVisualStyleBackColor = true;
            btn_mapsSkip.Click += actionClick_mapSkip;
            // 
            // panel4
            // 
            panel4.Controls.Add(tableLayoutPanel3);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(603, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(290, 350);
            panel4.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.6896553F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 59.3103447F));
            tableLayoutPanel3.Controls.Add(label6, 0, 1);
            tableLayoutPanel3.Controls.Add(label_dataCurrentMap, 1, 1);
            tableLayoutPanel3.Controls.Add(label7, 0, 2);
            tableLayoutPanel3.Controls.Add(label_dataNextMap, 1, 2);
            tableLayoutPanel3.Controls.Add(label8, 0, 4);
            tableLayoutPanel3.Controls.Add(label_dataTimeLeft, 1, 4);
            tableLayoutPanel3.Controls.Add(label9, 0, 14);
            tableLayoutPanel3.Controls.Add(label10, 0, 15);
            tableLayoutPanel3.Controls.Add(label11, 1, 14);
            tableLayoutPanel3.Controls.Add(label12, 1, 15);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 16;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(290, 350);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 24);
            label6.Name = "label6";
            label6.Size = new Size(112, 24);
            label6.TabIndex = 2;
            label6.Text = "[Current Map]";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_dataCurrentMap
            // 
            label_dataCurrentMap.AutoSize = true;
            label_dataCurrentMap.Dock = DockStyle.Fill;
            label_dataCurrentMap.Location = new Point(121, 24);
            label_dataCurrentMap.Name = "label_dataCurrentMap";
            label_dataCurrentMap.Size = new Size(166, 24);
            label_dataCurrentMap.TabIndex = 1;
            label_dataCurrentMap.Text = "No Map Data...";
            label_dataCurrentMap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(3, 48);
            label7.Name = "label7";
            label7.Size = new Size(112, 24);
            label7.TabIndex = 3;
            label7.Text = "[Next Map]";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_dataNextMap
            // 
            label_dataNextMap.AutoSize = true;
            label_dataNextMap.Dock = DockStyle.Fill;
            label_dataNextMap.Location = new Point(121, 48);
            label_dataNextMap.Name = "label_dataNextMap";
            label_dataNextMap.Size = new Size(166, 24);
            label_dataNextMap.TabIndex = 4;
            label_dataNextMap.Text = "No Map Data...";
            label_dataNextMap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(3, 96);
            label8.Name = "label8";
            label8.Size = new Size(112, 24);
            label8.TabIndex = 5;
            label8.Text = "[Time Left]";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_dataTimeLeft
            // 
            label_dataTimeLeft.AutoSize = true;
            label_dataTimeLeft.Dock = DockStyle.Fill;
            label_dataTimeLeft.Location = new Point(121, 96);
            label_dataTimeLeft.Name = "label_dataTimeLeft";
            label_dataTimeLeft.Size = new Size(166, 24);
            label_dataTimeLeft.TabIndex = 6;
            label_dataTimeLeft.Text = "No Map Data...";
            label_dataTimeLeft.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(3, 304);
            label9.Name = "label9";
            label9.Size = new Size(112, 20);
            label9.TabIndex = 7;
            label9.Text = "[Move Map Up]";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 324);
            label10.Name = "label10";
            label10.Size = new Size(112, 26);
            label10.TabIndex = 8;
            label10.Text = "[Move Map Down]";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(121, 304);
            label11.Name = "label11";
            label11.Size = new Size(166, 20);
            label11.TabIndex = 9;
            label11.Text = "Shift + W";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(121, 324);
            label12.Name = "label12";
            label12.Size = new Size(166, 26);
            label12.TabIndex = 10;
            label12.Text = "Shift + S";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabPlayers
            // 
            tabPlayers.Controls.Add(panel6);
            tabPlayers.Controls.Add(player_LayoutPanel);
            tabPlayers.Location = new Point(4, 24);
            tabPlayers.Name = "tabPlayers";
            tabPlayers.Size = new Size(902, 362);
            tabPlayers.TabIndex = 2;
            tabPlayers.Text = "Players";
            tabPlayers.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            panel6.Dock = DockStyle.Bottom;
            panel6.Location = new Point(0, 328);
            panel6.Name = "panel6";
            panel6.Size = new Size(902, 34);
            panel6.TabIndex = 1;
            // 
            // player_LayoutPanel
            // 
            player_LayoutPanel.Dock = DockStyle.Fill;
            player_LayoutPanel.Location = new Point(0, 0);
            player_LayoutPanel.Name = "player_LayoutPanel";
            player_LayoutPanel.Size = new Size(902, 362);
            player_LayoutPanel.TabIndex = 0;
            // 
            // tabChat
            // 
            tabChat.Controls.Add(tableLayoutPanel4);
            tabChat.Controls.Add(panel5);
            tabChat.Location = new Point(4, 24);
            tabChat.Name = "tabChat";
            tabChat.Size = new Size(902, 362);
            tabChat.TabIndex = 3;
            tabChat.Text = "Chat";
            tabChat.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel4.Controls.Add(tb_chatMessage, 0, 0);
            tableLayoutPanel4.Controls.Add(btn_sendChat, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Bottom;
            tableLayoutPanel4.Location = new Point(0, 336);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(902, 26);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // tb_chatMessage
            // 
            tb_chatMessage.AcceptsReturn = true;
            tb_chatMessage.Dock = DockStyle.Fill;
            tb_chatMessage.Location = new Point(3, 3);
            tb_chatMessage.Name = "tb_chatMessage";
            tb_chatMessage.Size = new Size(760, 23);
            tb_chatMessage.TabIndex = 0;
            tb_chatMessage.KeyPress += actionKeyEvent_submitEnterMessage;
            // 
            // btn_sendChat
            // 
            btn_sendChat.Dock = DockStyle.Fill;
            btn_sendChat.FlatStyle = FlatStyle.Flat;
            btn_sendChat.Location = new Point(766, 0);
            btn_sendChat.Margin = new Padding(0);
            btn_sendChat.Name = "btn_sendChat";
            btn_sendChat.Size = new Size(136, 26);
            btn_sendChat.TabIndex = 1;
            btn_sendChat.Text = "Send";
            btn_sendChat.UseVisualStyleBackColor = true;
            btn_sendChat.Click += actionKeyEvent_submitMessage;
            // 
            // panel5
            // 
            panel5.Controls.Add(dataGridView_chatMessages);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(0, 0, 0, 27);
            panel5.Size = new Size(902, 362);
            panel5.TabIndex = 1;
            // 
            // dataGridView_chatMessages
            // 
            dataGridView_chatMessages.AllowUserToAddRows = false;
            dataGridView_chatMessages.AllowUserToDeleteRows = false;
            dataGridView_chatMessages.AllowUserToResizeColumns = false;
            dataGridView_chatMessages.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView_chatMessages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView_chatMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_chatMessages.Columns.AddRange(new DataGridViewColumn[] { dateTime, Team, Player, Message });
            dataGridView_chatMessages.Dock = DockStyle.Fill;
            dataGridView_chatMessages.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_chatMessages.Location = new Point(0, 0);
            dataGridView_chatMessages.Name = "dataGridView_chatMessages";
            dataGridView_chatMessages.RowHeadersVisible = false;
            dataGridView_chatMessages.Size = new Size(902, 335);
            dataGridView_chatMessages.TabIndex = 0;
            // 
            // dateTime
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dateTime.DefaultCellStyle = dataGridViewCellStyle2;
            dateTime.HeaderText = "Timestamp";
            dateTime.MaxInputLength = 12;
            dateTime.Name = "dateTime";
            dateTime.Width = 75;
            // 
            // Team
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Team.DefaultCellStyle = dataGridViewCellStyle3;
            Team.HeaderText = "Team";
            Team.MaxInputLength = 24;
            Team.Name = "Team";
            Team.Width = 75;
            // 
            // Player
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Player.DefaultCellStyle = dataGridViewCellStyle4;
            Player.HeaderText = "Player";
            Player.MaxInputLength = 32;
            Player.Name = "Player";
            // 
            // Message
            // 
            Message.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Message.DefaultCellStyle = dataGridViewCellStyle5;
            Message.HeaderText = "Message";
            Message.MaxInputLength = 90;
            Message.Name = "Message";
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
            // tabMessages
            // 
            tabMessages.Controls.Add(tabMessagesControl);
            tabMessages.Location = new Point(4, 24);
            tabMessages.Name = "tabMessages";
            tabMessages.Size = new Size(902, 362);
            tabMessages.TabIndex = 5;
            tabMessages.Text = "Messages";
            tabMessages.UseVisualStyleBackColor = true;
            // 
            // tabMessagesControl
            // 
            tabMessagesControl.Controls.Add(tabAuto);
            tabMessagesControl.Controls.Add(tabSlaps);
            tabMessagesControl.Dock = DockStyle.Fill;
            tabMessagesControl.Location = new Point(0, 0);
            tabMessagesControl.Name = "tabMessagesControl";
            tabMessagesControl.SelectedIndex = 0;
            tabMessagesControl.Size = new Size(902, 362);
            tabMessagesControl.TabIndex = 0;
            // 
            // tabAuto
            // 
            tabAuto.Controls.Add(tableLayoutPanel9);
            tabAuto.Location = new Point(4, 24);
            tabAuto.Name = "tabAuto";
            tabAuto.Padding = new Padding(3);
            tabAuto.Size = new Size(894, 334);
            tabAuto.TabIndex = 0;
            tabAuto.Text = "Auto Messages";
            tabAuto.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 1;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Controls.Add(dg_autoMessages, 0, 0);
            tableLayoutPanel9.Controls.Add(tableLayoutPanel12, 0, 1);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(3, 3);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 2;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel9.Size = new Size(888, 328);
            tableLayoutPanel9.TabIndex = 0;
            // 
            // dg_autoMessages
            // 
            dg_autoMessages.AllowUserToAddRows = false;
            dg_autoMessages.AllowUserToDeleteRows = false;
            dg_autoMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_autoMessages.Columns.AddRange(new DataGridViewColumn[] { autoMessageID, autoTrigger, autoMessageText });
            dg_autoMessages.Dock = DockStyle.Fill;
            dg_autoMessages.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_autoMessages.Location = new Point(3, 3);
            dg_autoMessages.Name = "dg_autoMessages";
            dg_autoMessages.ReadOnly = true;
            dg_autoMessages.RowHeadersVisible = false;
            dg_autoMessages.Size = new Size(882, 292);
            dg_autoMessages.TabIndex = 0;
            dg_autoMessages.CellDoubleClick += actionDbClick_RemoveAutoMessage;
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
            // tableLayoutPanel12
            // 
            tableLayoutPanel12.ColumnCount = 3;
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel12.Controls.Add(tb_autoMessage, 1, 0);
            tableLayoutPanel12.Controls.Add(btn_addAutoMessage, 2, 0);
            tableLayoutPanel12.Controls.Add(num_AutoMessageTrigger, 0, 0);
            tableLayoutPanel12.Dock = DockStyle.Fill;
            tableLayoutPanel12.Location = new Point(0, 298);
            tableLayoutPanel12.Margin = new Padding(0);
            tableLayoutPanel12.Name = "tableLayoutPanel12";
            tableLayoutPanel12.RowCount = 1;
            tableLayoutPanel12.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel12.Size = new Size(888, 30);
            tableLayoutPanel12.TabIndex = 1;
            // 
            // tb_autoMessage
            // 
            tb_autoMessage.Dock = DockStyle.Fill;
            tb_autoMessage.Location = new Point(78, 3);
            tb_autoMessage.Name = "tb_autoMessage";
            tb_autoMessage.Size = new Size(707, 23);
            tb_autoMessage.TabIndex = 0;
            // 
            // btn_addAutoMessage
            // 
            btn_addAutoMessage.Dock = DockStyle.Fill;
            btn_addAutoMessage.Location = new Point(791, 3);
            btn_addAutoMessage.Name = "btn_addAutoMessage";
            btn_addAutoMessage.Size = new Size(94, 24);
            btn_addAutoMessage.TabIndex = 1;
            btn_addAutoMessage.Text = "Add Message";
            btn_addAutoMessage.UseVisualStyleBackColor = true;
            btn_addAutoMessage.Click += actionClick_addAutoMessages;
            // 
            // num_AutoMessageTrigger
            // 
            num_AutoMessageTrigger.Dock = DockStyle.Fill;
            num_AutoMessageTrigger.Location = new Point(3, 3);
            num_AutoMessageTrigger.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            num_AutoMessageTrigger.Name = "num_AutoMessageTrigger";
            num_AutoMessageTrigger.Size = new Size(69, 23);
            num_AutoMessageTrigger.TabIndex = 2;
            num_AutoMessageTrigger.TextAlign = HorizontalAlignment.Right;
            toolTip.SetToolTip(num_AutoMessageTrigger, "Trigger in Minutes from Start of Map\r\n0 = Get Triggered During Start Delay\r\n");
            // 
            // tabSlaps
            // 
            tabSlaps.Controls.Add(tableLayoutPanel10);
            tabSlaps.Location = new Point(4, 24);
            tabSlaps.Name = "tabSlaps";
            tabSlaps.Padding = new Padding(3);
            tabSlaps.Size = new Size(894, 334);
            tabSlaps.TabIndex = 1;
            tabSlaps.Text = "Slap Messages";
            tabSlaps.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 1;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.Controls.Add(dg_slapMessages, 0, 0);
            tableLayoutPanel10.Controls.Add(tableLayoutPanel11, 0, 1);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(3, 3);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 2;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel10.Size = new Size(888, 328);
            tableLayoutPanel10.TabIndex = 1;
            // 
            // dg_slapMessages
            // 
            dg_slapMessages.AllowUserToAddRows = false;
            dg_slapMessages.AllowUserToDeleteRows = false;
            dg_slapMessages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_slapMessages.Columns.AddRange(new DataGridViewColumn[] { slapMessageID, slapMessages });
            dg_slapMessages.Dock = DockStyle.Fill;
            dg_slapMessages.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_slapMessages.Location = new Point(3, 3);
            dg_slapMessages.Name = "dg_slapMessages";
            dg_slapMessages.ReadOnly = true;
            dg_slapMessages.RowHeadersVisible = false;
            dg_slapMessages.Size = new Size(882, 292);
            dg_slapMessages.TabIndex = 0;
            dg_slapMessages.CellDoubleClick += actionDbClick_RemoveSlapMessage;
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
            // tableLayoutPanel11
            // 
            tableLayoutPanel11.ColumnCount = 2;
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel11.Controls.Add(tb_slapMessage, 0, 0);
            tableLayoutPanel11.Controls.Add(btn_addSlap, 1, 0);
            tableLayoutPanel11.Dock = DockStyle.Fill;
            tableLayoutPanel11.Location = new Point(0, 298);
            tableLayoutPanel11.Margin = new Padding(0);
            tableLayoutPanel11.Name = "tableLayoutPanel11";
            tableLayoutPanel11.RowCount = 1;
            tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.Size = new Size(888, 30);
            tableLayoutPanel11.TabIndex = 1;
            // 
            // tb_slapMessage
            // 
            tb_slapMessage.Dock = DockStyle.Fill;
            tb_slapMessage.Location = new Point(3, 3);
            tb_slapMessage.Name = "tb_slapMessage";
            tb_slapMessage.Size = new Size(782, 23);
            tb_slapMessage.TabIndex = 0;
            // 
            // btn_addSlap
            // 
            btn_addSlap.Dock = DockStyle.Fill;
            btn_addSlap.Location = new Point(791, 3);
            btn_addSlap.Name = "btn_addSlap";
            btn_addSlap.Size = new Size(94, 24);
            btn_addSlap.TabIndex = 1;
            btn_addSlap.Text = "Add Message";
            btn_addSlap.UseVisualStyleBackColor = true;
            btn_addSlap.Click += actionClick_addSlapMessage;
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
            btn_SaveSettings.Click += actionClick_saveUpdateSettings;
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
            Name = "ServerManager";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Black Hawk Down Server Manager";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            mainPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabMaps.ResumeLayout(false);
            mapsTable.ResumeLayout(false);
            mapPanel1.ResumeLayout(false);
            mapsPanel1_table.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_availableMaps).EndInit();
            mapsPanel2.ResumeLayout(false);
            mapsPanel2_table.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_currentMaps).EndInit();
            tableMapControls.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tabPlayers.ResumeLayout(false);
            tabChat.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_chatMessages).EndInit();
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
            tabMessages.ResumeLayout(false);
            tabMessagesControl.ResumeLayout(false);
            tabAuto.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_autoMessages).EndInit();
            tableLayoutPanel12.ResumeLayout(false);
            tableLayoutPanel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_AutoMessageTrigger).EndInit();
            tabSlaps.ResumeLayout(false);
            tableLayoutPanel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_slapMessages).EndInit();
            tableLayoutPanel11.ResumeLayout(false);
            tableLayoutPanel11.PerformLayout();
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
        private TabPage tabChat;
        private TabPage tabServer;
        private TabPage tabMaps;
        private TabPage tabPlayers;
        private OpenFileDialog openFileDialog;
        private TableLayoutPanel mapsTable;
        private Panel mapPanel1;
        private TableLayoutPanel mapsPanel1_table;
        internal ComboBox combo_gameTypes;
        internal DataGridView dataGridView_availableMaps;
        private ToolTip toolTip;
        private Panel mapsPanel2;
        private TableLayoutPanel mapsPanel2_table;
        internal DataGridView dataGridView_currentMaps;
        private TableLayoutPanel tableMapControls;
        private FontAwesome.Sharp.IconButton ib_exportMapList;
        private FontAwesome.Sharp.IconButton ib_importMapList;
        private FontAwesome.Sharp.IconButton ib_mapRefresh;
        private FontAwesome.Sharp.IconButton ib_clearMapList;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btn_mapsScore;
        private Button btn_mapsSkip;
        private Button btn_mapsUpdate;
        private Button btn_mapsPlayNext;
        internal ToolStripLabel toolStripStatus;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label6;
        private Label label7;
        internal Label label_dataCurrentMap;
        internal Label label_dataNextMap;
        private Label label8;
        internal Label label_dataTimeLeft;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox tb_chatMessage;
        private Button btn_sendChat;
        private Panel panel5;
        internal DataGridView dataGridView_chatMessages;
        private DataGridViewTextBoxColumn dateTime;
        private DataGridViewTextBoxColumn Team;
        private DataGridViewTextBoxColumn Player;
        private DataGridViewTextBoxColumn Message;
        public FlowLayoutPanel player_LayoutPanel;
        private Panel panel6;
        private TabPage tabBans;
        private TableLayoutPanel tableLayoutPanel5;
        private GroupBox groupBox3;
        private GroupBox groupBox8;
        private DataGridViewTextBoxColumn playerRecordID;
        private DataGridViewTextBoxColumn playerName;
        private DataGridViewTextBoxColumn ipRecordID;
        private DataGridViewTextBoxColumn address;
        internal TabControl tabControl;
        private TabPage tabMessages;
        private TabControl tabMessagesControl;
        private TabPage tabAuto;
        private TableLayoutPanel tableLayoutPanel9;
        private TabPage tabSlaps;
        private TableLayoutPanel tableLayoutPanel10;
        private DataGridViewTextBoxColumn slapMessageID;
        private DataGridViewTextBoxColumn slapMessages;
        internal DataGridView dg_autoMessages;
        internal DataGridView dg_slapMessages;
        private TableLayoutPanel tableLayoutPanel11;
        private TextBox tb_slapMessage;
        private Button btn_addSlap;
        private TableLayoutPanel tableLayoutPanel12;
        private TextBox tb_autoMessage;
        private Button btn_addAutoMessage;
        private NumericUpDown num_AutoMessageTrigger;
        private DataGridViewTextBoxColumn autoMessageID;
        private DataGridViewTextBoxColumn autoTrigger;
        private DataGridViewTextBoxColumn autoMessageText;
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
        public FontAwesome.Sharp.IconButton ib_resetCurrentMaps;
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
    }
}
