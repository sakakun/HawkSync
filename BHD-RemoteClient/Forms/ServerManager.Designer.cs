using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BHD_RemoteClient.Forms
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            btn_UpdatePath = new FontAwesome.Sharp.IconToolStripButton();
            toolStripStatus = new ToolStripLabel();
            mainPanel = new Panel();
            tabControl = new TabControl();
            tabServer = new TabPage();
            splitContainer = new SplitContainer();
            panelLeft = new Panel();
            groupBox5 = new GroupBox();
            label_maxPlayers = new Label();
            num_maxPlayers = new NumericUpDown();
            label_scoreDelay = new Label();
            num_scoreBoardDelay = new NumericUpDown();
            label_replayMaps = new Label();
            cb_replayMaps = new ComboBox();
            label_respawnTime = new Label();
            num_respawnTime = new NumericUpDown();
            label_startDelay = new Label();
            label_timeLimit = new Label();
            num_gameStartDelay = new NumericUpDown();
            num_gameTimeLimit = new NumericUpDown();
            groupBox2 = new GroupBox();
            scoresTableLayout = new TableLayoutPanel();
            num_scoresFB = new NumericUpDown();
            num_scoresDM = new NumericUpDown();
            label_flagball = new Label();
            num_scoresKOTH = new NumericUpDown();
            label_koth = new Label();
            label_dm = new Label();
            groupBox4 = new GroupBox();
            cb_enableRemote = new CheckBox();
            num_remotePort = new NumericUpDown();
            cb_sessionType = new ComboBox();
            tb_serverPassword = new TextBox();
            cb_novaRequired = new CheckBox();
            num_serverPort = new NumericUpDown();
            cb_serverDedicated = new CheckBox();
            cb_serverIP = new ComboBox();
            groupBox_hostInfo = new GroupBox();
            tb_serverMessage = new TextBox();
            tb_hostName = new TextBox();
            tb_serverName = new TextBox();
            tb_serverID = new TextBox();
            panelRight = new Panel();
            panel3 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            btn_startStop = new Button();
            btn_reset = new Button();
            btn_update = new Button();
            tableLayoutPanel20 = new TableLayoutPanel();
            btn_load = new Button();
            button1 = new Button();
            panel_rightoptions = new Panel();
            tabServerControls = new TabControl();
            tabOptions = new TabPage();
            panel2 = new Panel();
            splitContainer1 = new SplitContainer();
            groupBox_gamePlay = new GroupBox();
            cb_autoRange = new CheckBox();
            num_maxTeamLives = new NumericUpDown();
            label2 = new Label();
            cb_showClays = new CheckBox();
            num_flagReturnTime = new NumericUpDown();
            label1 = new Label();
            cb_showTracers = new CheckBox();
            num_pspTakeoverTimer = new NumericUpDown();
            label_pspTakeover = new Label();
            groupBox6 = new GroupBox();
            tb_redPassword = new TextBox();
            cb_autoBalance = new CheckBox();
            tb_bluePassword = new TextBox();
            groupBox_pingChecks = new GroupBox();
            label5 = new Label();
            label4 = new Label();
            num_minPing = new NumericUpDown();
            num_maxPing = new NumericUpDown();
            label3 = new Label();
            cb_enableMaxCheck = new CheckBox();
            cb_enableMinCheck = new CheckBox();
            groupBox7 = new GroupBox();
            cb_enableLeftLean = new CheckBox();
            cb_customSkins = new CheckBox();
            cb_enableDistroyBuildings = new CheckBox();
            cb_enableFatBullets = new CheckBox();
            cb_enableOneShotKills = new CheckBox();
            groupBox1 = new GroupBox();
            num_maxFFKills = new NumericUpDown();
            cb_enableFFkills = new CheckBox();
            label_maxFFkills = new Label();
            cb_warnFFkils = new CheckBox();
            cb_showTeamTags = new CheckBox();
            tabRestrictions = new TabPage();
            groupBox_weapon = new GroupBox();
            checkBox_selectNone = new CheckBox();
            checkBox_selectAll = new CheckBox();
            cb_weapSatchel = new CheckBox();
            cb_weapAT4 = new CheckBox();
            cb_weapSmokeGrenade = new CheckBox();
            cb_weapClay = new CheckBox();
            cb_weap300Tact = new CheckBox();
            cb_weapFragGrenade = new CheckBox();
            cb_weapFlashBang = new CheckBox();
            cb_weapG36 = new CheckBox();
            groupBox_roles = new GroupBox();
            cb_roleSniper = new CheckBox();
            cb_roleMedic = new CheckBox();
            cb_roleGunner = new CheckBox();
            cb_roleCQB = new CheckBox();
            cb_weapPSG1 = new CheckBox();
            cb_weapG3 = new CheckBox();
            cb_weapMP5 = new CheckBox();
            cb_weapM240 = new CheckBox();
            cb_weapM60 = new CheckBox();
            cb_weapSaw = new CheckBox();
            cb_weapBarret = new CheckBox();
            cb_weapM24 = new CheckBox();
            cb_weapM21 = new CheckBox();
            cb_weapM16203 = new CheckBox();
            cb_weapM16 = new CheckBox();
            cb_weapCAR15203 = new CheckBox();
            cb_weapCAR15 = new CheckBox();
            cb_weapShotgun = new CheckBox();
            cb_weapM9Bereatta = new CheckBox();
            cb_weapColt45 = new CheckBox();
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
            tableLayoutPanel6 = new TableLayoutPanel();
            groupBox9 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            tb_bansPlayerName = new TextBox();
            tb_bansIPAddress = new TextBox();
            cb_banSubMask = new ComboBox();
            btn_addBan = new Button();
            tableLayoutPanel8 = new TableLayoutPanel();
            label13 = new Label();
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
            tabServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelLeft.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxPlayers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoreBoardDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_respawnTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_gameStartDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_gameTimeLimit).BeginInit();
            groupBox2.SuspendLayout();
            scoresTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_scoresFB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresDM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresKOTH).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_remotePort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_serverPort).BeginInit();
            groupBox_hostInfo.SuspendLayout();
            panelRight.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel20.SuspendLayout();
            panel_rightoptions.SuspendLayout();
            tabServerControls.SuspendLayout();
            tabOptions.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox_gamePlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxTeamLives).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_flagReturnTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_pspTakeoverTimer).BeginInit();
            groupBox6.SuspendLayout();
            groupBox_pingChecks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_minPing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_maxPing).BeginInit();
            groupBox7.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxFFKills).BeginInit();
            tabRestrictions.SuspendLayout();
            groupBox_weapon.SuspendLayout();
            groupBox_roles.SuspendLayout();
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
            tableLayoutPanel6.SuspendLayout();
            groupBox9.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
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
            toolStrip.Items.AddRange(new ToolStripItem[] { btn_UpdatePath, toolStripStatus });
            toolStrip.Location = new Point(0, 400);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(920, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
            // 
            // btn_UpdatePath
            // 
            btn_UpdatePath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btn_UpdatePath.IconChar = FontAwesome.Sharp.IconChar.CircleExclamation;
            btn_UpdatePath.IconColor = Color.Red;
            btn_UpdatePath.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btn_UpdatePath.ImageTransparentColor = Color.Magenta;
            btn_UpdatePath.Name = "btn_UpdatePath";
            btn_UpdatePath.Size = new Size(23, 22);
            btn_UpdatePath.Text = "Fix Game Server Path!";
            btn_UpdatePath.ToolTipText = "Contact Server Administrator!";
            btn_UpdatePath.Visible = false;
            btn_UpdatePath.Click += actionClick_SetServerPath;
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
            // tabServer
            // 
            tabServer.Controls.Add(splitContainer);
            tabServer.Location = new Point(4, 24);
            tabServer.Name = "tabServer";
            tabServer.Padding = new Padding(3);
            tabServer.Size = new Size(902, 362);
            tabServer.TabIndex = 0;
            tabServer.Text = "Server";
            tabServer.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 3);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelLeft);
            splitContainer.Panel1MinSize = 406;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelRight);
            splitContainer.Panel2MinSize = 406;
            splitContainer.Size = new Size(896, 356);
            splitContainer.SplitterDistance = 409;
            splitContainer.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(groupBox5);
            panelLeft.Controls.Add(groupBox2);
            panelLeft.Controls.Add(groupBox4);
            panelLeft.Controls.Add(groupBox_hostInfo);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(409, 356);
            panelLeft.TabIndex = 0;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(label_maxPlayers);
            groupBox5.Controls.Add(num_maxPlayers);
            groupBox5.Controls.Add(label_scoreDelay);
            groupBox5.Controls.Add(num_scoreBoardDelay);
            groupBox5.Controls.Add(label_replayMaps);
            groupBox5.Controls.Add(cb_replayMaps);
            groupBox5.Controls.Add(label_respawnTime);
            groupBox5.Controls.Add(num_respawnTime);
            groupBox5.Controls.Add(label_startDelay);
            groupBox5.Controls.Add(label_timeLimit);
            groupBox5.Controls.Add(num_gameStartDelay);
            groupBox5.Controls.Add(num_gameTimeLimit);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(0, 180);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(409, 105);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Server Options";
            // 
            // label_maxPlayers
            // 
            label_maxPlayers.AutoSize = true;
            label_maxPlayers.Location = new Point(22, 73);
            label_maxPlayers.Name = "label_maxPlayers";
            label_maxPlayers.Size = new Size(69, 15);
            label_maxPlayers.TabIndex = 11;
            label_maxPlayers.Text = "Max Players";
            // 
            // num_maxPlayers
            // 
            num_maxPlayers.Location = new Point(127, 69);
            num_maxPlayers.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            num_maxPlayers.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            num_maxPlayers.Name = "num_maxPlayers";
            num_maxPlayers.Size = new Size(64, 23);
            num_maxPlayers.TabIndex = 10;
            num_maxPlayers.TextAlign = HorizontalAlignment.Right;
            num_maxPlayers.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label_scoreDelay
            // 
            label_scoreDelay.AutoSize = true;
            label_scoreDelay.Location = new Point(278, 73);
            label_scoreDelay.Name = "label_scoreDelay";
            label_scoreDelay.Size = new Size(102, 15);
            label_scoreDelay.TabIndex = 9;
            label_scoreDelay.Text = "Score Board Delay";
            // 
            // num_scoreBoardDelay
            // 
            num_scoreBoardDelay.Location = new Point(197, 69);
            num_scoreBoardDelay.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            num_scoreBoardDelay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoreBoardDelay.Name = "num_scoreBoardDelay";
            num_scoreBoardDelay.Size = new Size(64, 23);
            num_scoreBoardDelay.TabIndex = 8;
            toolTip.SetToolTip(num_scoreBoardDelay, "Time in Seconds");
            num_scoreBoardDelay.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label_replayMaps
            // 
            label_replayMaps.AutoSize = true;
            label_replayMaps.Location = new Point(306, 21);
            label_replayMaps.Name = "label_replayMaps";
            label_replayMaps.Size = new Size(74, 15);
            label_replayMaps.TabIndex = 6;
            label_replayMaps.Text = "Replay Maps";
            label_replayMaps.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cb_replayMaps
            // 
            cb_replayMaps.DisplayMember = "Yes";
            cb_replayMaps.FormattingEnabled = true;
            cb_replayMaps.Items.AddRange(new object[] { "No", "Yes", "Cycle" });
            cb_replayMaps.Location = new Point(197, 17);
            cb_replayMaps.Name = "cb_replayMaps";
            cb_replayMaps.Size = new Size(64, 23);
            cb_replayMaps.TabIndex = 7;
            cb_replayMaps.Text = "Cycle";
            // 
            // label_respawnTime
            // 
            label_respawnTime.AutoSize = true;
            label_respawnTime.Location = new Point(296, 47);
            label_respawnTime.Name = "label_respawnTime";
            label_respawnTime.Size = new Size(84, 15);
            label_respawnTime.TabIndex = 5;
            label_respawnTime.Text = "Respawn Time";
            label_respawnTime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_respawnTime
            // 
            num_respawnTime.Location = new Point(197, 43);
            num_respawnTime.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            num_respawnTime.Name = "num_respawnTime";
            num_respawnTime.Size = new Size(64, 23);
            num_respawnTime.TabIndex = 4;
            toolTip.SetToolTip(num_respawnTime, "Time in Seconds");
            num_respawnTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label_startDelay
            // 
            label_startDelay.AutoSize = true;
            label_startDelay.Location = new Point(28, 47);
            label_startDelay.Name = "label_startDelay";
            label_startDelay.Size = new Size(63, 15);
            label_startDelay.TabIndex = 3;
            label_startDelay.Text = "Start Delay";
            // 
            // label_timeLimit
            // 
            label_timeLimit.AutoSize = true;
            label_timeLimit.Location = new Point(27, 21);
            label_timeLimit.Name = "label_timeLimit";
            label_timeLimit.Size = new Size(64, 15);
            label_timeLimit.TabIndex = 2;
            label_timeLimit.Text = "Time Limit";
            // 
            // num_gameStartDelay
            // 
            num_gameStartDelay.Location = new Point(127, 43);
            num_gameStartDelay.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            num_gameStartDelay.Name = "num_gameStartDelay";
            num_gameStartDelay.Size = new Size(64, 23);
            num_gameStartDelay.TabIndex = 1;
            num_gameStartDelay.TextAlign = HorizontalAlignment.Right;
            toolTip.SetToolTip(num_gameStartDelay, "Time in Minutes");
            num_gameStartDelay.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // num_gameTimeLimit
            // 
            num_gameTimeLimit.Location = new Point(127, 17);
            num_gameTimeLimit.Name = "num_gameTimeLimit";
            num_gameTimeLimit.Size = new Size(64, 23);
            num_gameTimeLimit.TabIndex = 0;
            num_gameTimeLimit.TextAlign = HorizontalAlignment.Right;
            toolTip.SetToolTip(num_gameTimeLimit, "Time in Minutes");
            num_gameTimeLimit.Value = new decimal(new int[] { 21, 0, 0, 0 });
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(scoresTableLayout);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(0, 285);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(409, 71);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Scoring";
            // 
            // scoresTableLayout
            // 
            scoresTableLayout.ColumnCount = 3;
            scoresTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            scoresTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            scoresTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            scoresTableLayout.Controls.Add(num_scoresFB, 2, 1);
            scoresTableLayout.Controls.Add(num_scoresDM, 1, 1);
            scoresTableLayout.Controls.Add(label_flagball, 2, 0);
            scoresTableLayout.Controls.Add(num_scoresKOTH, 0, 1);
            scoresTableLayout.Controls.Add(label_koth, 0, 0);
            scoresTableLayout.Controls.Add(label_dm, 1, 0);
            scoresTableLayout.Dock = DockStyle.Fill;
            scoresTableLayout.Location = new Point(3, 19);
            scoresTableLayout.Name = "scoresTableLayout";
            scoresTableLayout.RowCount = 2;
            scoresTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            scoresTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            scoresTableLayout.Size = new Size(403, 49);
            scoresTableLayout.TabIndex = 0;
            // 
            // num_scoresFB
            // 
            num_scoresFB.Dock = DockStyle.Fill;
            num_scoresFB.Location = new Point(271, 22);
            num_scoresFB.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            num_scoresFB.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoresFB.Name = "num_scoresFB";
            num_scoresFB.Size = new Size(129, 23);
            num_scoresFB.TabIndex = 2;
            num_scoresFB.TextAlign = HorizontalAlignment.Center;
            num_scoresFB.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // num_scoresDM
            // 
            num_scoresDM.Dock = DockStyle.Fill;
            num_scoresDM.Location = new Point(137, 22);
            num_scoresDM.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            num_scoresDM.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoresDM.Name = "num_scoresDM";
            num_scoresDM.Size = new Size(128, 23);
            num_scoresDM.TabIndex = 1;
            num_scoresDM.TextAlign = HorizontalAlignment.Center;
            num_scoresDM.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // label_flagball
            // 
            label_flagball.AutoSize = true;
            label_flagball.Dock = DockStyle.Fill;
            label_flagball.Location = new Point(271, 0);
            label_flagball.Name = "label_flagball";
            label_flagball.Size = new Size(129, 19);
            label_flagball.TabIndex = 5;
            label_flagball.Text = "FLAGBALL";
            label_flagball.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // num_scoresKOTH
            // 
            num_scoresKOTH.Dock = DockStyle.Fill;
            num_scoresKOTH.Location = new Point(3, 22);
            num_scoresKOTH.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            num_scoresKOTH.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoresKOTH.Name = "num_scoresKOTH";
            num_scoresKOTH.Size = new Size(128, 23);
            num_scoresKOTH.TabIndex = 0;
            num_scoresKOTH.TextAlign = HorizontalAlignment.Center;
            toolTip.SetToolTip(num_scoresKOTH, "Time in Minutes");
            num_scoresKOTH.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // label_koth
            // 
            label_koth.AutoSize = true;
            label_koth.Dock = DockStyle.Fill;
            label_koth.Location = new Point(3, 0);
            label_koth.Name = "label_koth";
            label_koth.Size = new Size(128, 19);
            label_koth.TabIndex = 3;
            label_koth.Text = "TKOTH + KOTH";
            label_koth.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_dm
            // 
            label_dm.AutoSize = true;
            label_dm.Dock = DockStyle.Fill;
            label_dm.Location = new Point(137, 0);
            label_dm.Name = "label_dm";
            label_dm.Size = new Size(128, 19);
            label_dm.TabIndex = 4;
            label_dm.Text = "TDM + DM";
            label_dm.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(cb_enableRemote);
            groupBox4.Controls.Add(num_remotePort);
            groupBox4.Controls.Add(cb_sessionType);
            groupBox4.Controls.Add(tb_serverPassword);
            groupBox4.Controls.Add(cb_novaRequired);
            groupBox4.Controls.Add(num_serverPort);
            groupBox4.Controls.Add(cb_serverDedicated);
            groupBox4.Controls.Add(cb_serverIP);
            groupBox4.Dock = DockStyle.Top;
            groupBox4.Location = new Point(0, 84);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(409, 96);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Server Details";
            // 
            // cb_enableRemote
            // 
            cb_enableRemote.AutoSize = true;
            cb_enableRemote.Location = new Point(11, 48);
            cb_enableRemote.Name = "cb_enableRemote";
            cb_enableRemote.Size = new Size(144, 19);
            cb_enableRemote.TabIndex = 5;
            cb_enableRemote.Text = "Enable Remote Access";
            cb_enableRemote.UseVisualStyleBackColor = true;
            cb_enableRemote.CheckedChanged += ActionClick_ToggleRemoteAccess;
            // 
            // num_remotePort
            // 
            num_remotePort.Location = new Point(177, 46);
            num_remotePort.Maximum = new decimal(new int[] { 49151, 0, 0, 0 });
            num_remotePort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            num_remotePort.Name = "num_remotePort";
            num_remotePort.Size = new Size(64, 23);
            num_remotePort.TabIndex = 4;
            num_remotePort.TextAlign = HorizontalAlignment.Center;
            toolTip.SetToolTip(num_remotePort, "Remote Port");
            num_remotePort.Value = new decimal(new int[] { 8083, 0, 0, 0 });
            // 
            // cb_sessionType
            // 
            cb_sessionType.Enabled = false;
            cb_sessionType.FormattingEnabled = true;
            cb_sessionType.Items.AddRange(new object[] { "NovaWorld", "Local Only" });
            cb_sessionType.Location = new Point(310, 46);
            cb_sessionType.Name = "cb_sessionType";
            cb_sessionType.Size = new Size(90, 23);
            cb_sessionType.TabIndex = 3;
            cb_sessionType.Text = "NovaWorld";
            // 
            // tb_serverPassword
            // 
            tb_serverPassword.Location = new Point(248, 18);
            tb_serverPassword.MaxLength = 16;
            tb_serverPassword.Name = "tb_serverPassword";
            tb_serverPassword.PlaceholderText = "Server Password";
            tb_serverPassword.Size = new Size(154, 23);
            tb_serverPassword.TabIndex = 2;
            tb_serverPassword.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_novaRequired
            // 
            cb_novaRequired.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cb_novaRequired.AutoSize = true;
            cb_novaRequired.CheckAlign = ContentAlignment.MiddleRight;
            cb_novaRequired.Location = new Point(229, 73);
            cb_novaRequired.Name = "cb_novaRequired";
            cb_novaRequired.Size = new Size(169, 19);
            cb_novaRequired.TabIndex = 1;
            cb_novaRequired.Text = "NovaWorld Login Required";
            cb_novaRequired.UseVisualStyleBackColor = true;
            // 
            // num_serverPort
            // 
            num_serverPort.Location = new Point(177, 18);
            num_serverPort.Maximum = new decimal(new int[] { 65999, 0, 0, 0 });
            num_serverPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_serverPort.Name = "num_serverPort";
            num_serverPort.Size = new Size(64, 23);
            num_serverPort.TabIndex = 1;
            num_serverPort.TextAlign = HorizontalAlignment.Center;
            num_serverPort.Value = new decimal(new int[] { 17479, 0, 0, 0 });
            // 
            // cb_serverDedicated
            // 
            cb_serverDedicated.AutoSize = true;
            cb_serverDedicated.Checked = true;
            cb_serverDedicated.CheckState = CheckState.Checked;
            cb_serverDedicated.Location = new Point(11, 73);
            cb_serverDedicated.Name = "cb_serverDedicated";
            cb_serverDedicated.Size = new Size(150, 19);
            cb_serverDedicated.TabIndex = 0;
            cb_serverDedicated.Text = "Run in Dedicated Mode";
            cb_serverDedicated.UseVisualStyleBackColor = true;
            // 
            // cb_serverIP
            // 
            cb_serverIP.FormattingEnabled = true;
            cb_serverIP.Items.AddRange(new object[] { "0.0.0.0" });
            cb_serverIP.Location = new Point(6, 18);
            cb_serverIP.Name = "cb_serverIP";
            cb_serverIP.Size = new Size(164, 23);
            cb_serverIP.TabIndex = 0;
            // 
            // groupBox_hostInfo
            // 
            groupBox_hostInfo.Controls.Add(tb_serverMessage);
            groupBox_hostInfo.Controls.Add(tb_hostName);
            groupBox_hostInfo.Controls.Add(tb_serverName);
            groupBox_hostInfo.Controls.Add(tb_serverID);
            groupBox_hostInfo.Dock = DockStyle.Top;
            groupBox_hostInfo.Location = new Point(0, 0);
            groupBox_hostInfo.Name = "groupBox_hostInfo";
            groupBox_hostInfo.Size = new Size(409, 84);
            groupBox_hostInfo.TabIndex = 3;
            groupBox_hostInfo.TabStop = false;
            groupBox_hostInfo.Text = "Host Information";
            // 
            // tb_serverMessage
            // 
            tb_serverMessage.Location = new Point(176, 22);
            tb_serverMessage.MaxLength = 85;
            tb_serverMessage.Multiline = true;
            tb_serverMessage.Name = "tb_serverMessage";
            tb_serverMessage.PlaceholderText = "Host/Server Message";
            tb_serverMessage.Size = new Size(224, 52);
            tb_serverMessage.TabIndex = 3;
            // 
            // tb_hostName
            // 
            tb_hostName.Location = new Point(70, 22);
            tb_hostName.MaxLength = 15;
            tb_hostName.Name = "tb_hostName";
            tb_hostName.PlaceholderText = "Host Name";
            tb_hostName.Size = new Size(100, 23);
            tb_hostName.TabIndex = 1;
            tb_hostName.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_serverName
            // 
            tb_serverName.Location = new Point(6, 51);
            tb_serverName.MaxLength = 26;
            tb_serverName.Name = "tb_serverName";
            tb_serverName.PlaceholderText = "Server Name (26 Chars)";
            tb_serverName.Size = new Size(164, 23);
            tb_serverName.TabIndex = 2;
            // 
            // tb_serverID
            // 
            tb_serverID.Location = new Point(6, 22);
            tb_serverID.MaxLength = 5;
            tb_serverID.Name = "tb_serverID";
            tb_serverID.PlaceholderText = "AA###";
            tb_serverID.Size = new Size(58, 23);
            tb_serverID.TabIndex = 0;
            tb_serverID.TextAlign = HorizontalAlignment.Center;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(panel3);
            panelRight.Controls.Add(panel_rightoptions);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(0, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(483, 356);
            panelRight.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(tableLayoutPanel1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 295);
            panel3.Name = "panel3";
            panel3.Size = new Size(483, 61);
            panel3.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btn_startStop, 0, 0);
            tableLayoutPanel1.Controls.Add(btn_reset, 1, 0);
            tableLayoutPanel1.Controls.Add(btn_update, 3, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel20, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(483, 61);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_startStop
            // 
            btn_startStop.Dock = DockStyle.Fill;
            btn_startStop.Location = new Point(3, 3);
            btn_startStop.Name = "btn_startStop";
            btn_startStop.Size = new Size(114, 55);
            btn_startStop.TabIndex = 0;
            btn_startStop.Text = "&Start Server";
            btn_startStop.UseVisualStyleBackColor = true;
            btn_startStop.Click += actionClick_startServer;
            // 
            // btn_reset
            // 
            btn_reset.Dock = DockStyle.Fill;
            btn_reset.Location = new Point(123, 3);
            btn_reset.Name = "btn_reset";
            btn_reset.Size = new Size(114, 55);
            btn_reset.TabIndex = 3;
            btn_reset.Text = "&Reset Changes";
            btn_reset.UseVisualStyleBackColor = true;
            btn_reset.Click += actionClick_resetServerChanges;
            // 
            // btn_update
            // 
            btn_update.Dock = DockStyle.Fill;
            btn_update.Location = new Point(363, 3);
            btn_update.Name = "btn_update";
            btn_update.Size = new Size(117, 55);
            btn_update.TabIndex = 5;
            btn_update.Text = "&Update && Save";
            btn_update.UseVisualStyleBackColor = true;
            btn_update.Click += actionClick_saveUpdateSettings;
            // 
            // tableLayoutPanel20
            // 
            tableLayoutPanel20.ColumnCount = 1;
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel20.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel20.Controls.Add(btn_load, 0, 0);
            tableLayoutPanel20.Controls.Add(button1, 0, 1);
            tableLayoutPanel20.Location = new Point(240, 0);
            tableLayoutPanel20.Margin = new Padding(0);
            tableLayoutPanel20.Name = "tableLayoutPanel20";
            tableLayoutPanel20.RowCount = 2;
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel20.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel20.Size = new Size(120, 61);
            tableLayoutPanel20.TabIndex = 6;
            // 
            // btn_load
            // 
            btn_load.Dock = DockStyle.Fill;
            btn_load.Location = new Point(0, 0);
            btn_load.Margin = new Padding(0);
            btn_load.Name = "btn_load";
            btn_load.Size = new Size(120, 30);
            btn_load.TabIndex = 4;
            btn_load.Text = "&Load Settings";
            btn_load.UseVisualStyleBackColor = true;
            btn_load.Click += actionClick_importServerSettings;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(0, 30);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(120, 31);
            button1.TabIndex = 5;
            button1.Text = "&Export Settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += actionClick_ExportSettings;
            // 
            // panel_rightoptions
            // 
            panel_rightoptions.Controls.Add(tabServerControls);
            panel_rightoptions.Dock = DockStyle.Top;
            panel_rightoptions.Location = new Point(0, 0);
            panel_rightoptions.Name = "panel_rightoptions";
            panel_rightoptions.Size = new Size(483, 295);
            panel_rightoptions.TabIndex = 1;
            // 
            // tabServerControls
            // 
            tabServerControls.Controls.Add(tabOptions);
            tabServerControls.Controls.Add(tabRestrictions);
            tabServerControls.Dock = DockStyle.Fill;
            tabServerControls.Location = new Point(0, 0);
            tabServerControls.Name = "tabServerControls";
            tabServerControls.SelectedIndex = 0;
            tabServerControls.Size = new Size(483, 295);
            tabServerControls.TabIndex = 0;
            // 
            // tabOptions
            // 
            tabOptions.Controls.Add(panel2);
            tabOptions.Location = new Point(4, 24);
            tabOptions.Name = "tabOptions";
            tabOptions.Padding = new Padding(3);
            tabOptions.Size = new Size(475, 267);
            tabOptions.TabIndex = 4;
            tabOptions.Text = "Options";
            tabOptions.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(splitContainer1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(469, 261);
            panel2.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox_gamePlay);
            splitContainer1.Panel1.Controls.Add(groupBox6);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox_pingChecks);
            splitContainer1.Panel2.Controls.Add(groupBox7);
            splitContainer1.Panel2.Controls.Add(groupBox1);
            splitContainer1.Size = new Size(469, 261);
            splitContainer1.SplitterDistance = 118;
            splitContainer1.TabIndex = 1;
            // 
            // groupBox_gamePlay
            // 
            groupBox_gamePlay.Controls.Add(cb_autoRange);
            groupBox_gamePlay.Controls.Add(num_maxTeamLives);
            groupBox_gamePlay.Controls.Add(label2);
            groupBox_gamePlay.Controls.Add(cb_showClays);
            groupBox_gamePlay.Controls.Add(num_flagReturnTime);
            groupBox_gamePlay.Controls.Add(label1);
            groupBox_gamePlay.Controls.Add(cb_showTracers);
            groupBox_gamePlay.Controls.Add(num_pspTakeoverTimer);
            groupBox_gamePlay.Controls.Add(label_pspTakeover);
            groupBox_gamePlay.Dock = DockStyle.Fill;
            groupBox_gamePlay.Location = new Point(150, 0);
            groupBox_gamePlay.Name = "groupBox_gamePlay";
            groupBox_gamePlay.Size = new Size(319, 118);
            groupBox_gamePlay.TabIndex = 3;
            groupBox_gamePlay.TabStop = false;
            groupBox_gamePlay.Text = "Game Play Settings";
            // 
            // cb_autoRange
            // 
            cb_autoRange.AutoSize = true;
            cb_autoRange.Location = new Point(16, 84);
            cb_autoRange.Name = "cb_autoRange";
            cb_autoRange.Size = new Size(121, 19);
            cb_autoRange.TabIndex = 3;
            cb_autoRange.Text = "Allow Auto Range";
            cb_autoRange.UseVisualStyleBackColor = true;
            // 
            // num_maxTeamLives
            // 
            num_maxTeamLives.Font = new System.Drawing.Font("Segoe UI", 8F);
            num_maxTeamLives.Location = new Point(255, 84);
            num_maxTeamLives.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            num_maxTeamLives.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_maxTeamLives.Name = "num_maxTeamLives";
            num_maxTeamLives.Size = new Size(45, 22);
            num_maxTeamLives.TabIndex = 5;
            num_maxTeamLives.TextAlign = HorizontalAlignment.Center;
            num_maxTeamLives.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(154, 85);
            label2.Name = "label2";
            label2.Size = new Size(90, 15);
            label2.TabIndex = 4;
            label2.Text = "Max Team Lives";
            // 
            // cb_showClays
            // 
            cb_showClays.AutoSize = true;
            cb_showClays.Location = new Point(16, 57);
            cb_showClays.Name = "cb_showClays";
            cb_showClays.Size = new Size(118, 19);
            cb_showClays.TabIndex = 3;
            cb_showClays.Text = "Show Team Clays";
            cb_showClays.UseVisualStyleBackColor = true;
            // 
            // num_flagReturnTime
            // 
            num_flagReturnTime.Font = new System.Drawing.Font("Segoe UI", 8F);
            num_flagReturnTime.Location = new Point(255, 57);
            num_flagReturnTime.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            num_flagReturnTime.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_flagReturnTime.Name = "num_flagReturnTime";
            num_flagReturnTime.Size = new Size(45, 22);
            num_flagReturnTime.TabIndex = 3;
            num_flagReturnTime.TextAlign = HorizontalAlignment.Center;
            toolTip.SetToolTip(num_flagReturnTime, "Time in Minutes");
            num_flagReturnTime.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(147, 58);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 2;
            label1.Text = "Flag Return Time";
            // 
            // cb_showTracers
            // 
            cb_showTracers.AutoSize = true;
            cb_showTracers.Location = new Point(16, 30);
            cb_showTracers.Name = "cb_showTracers";
            cb_showTracers.Size = new Size(95, 19);
            cb_showTracers.TabIndex = 3;
            cb_showTracers.Text = "Show Tracers";
            cb_showTracers.UseVisualStyleBackColor = true;
            // 
            // num_pspTakeoverTimer
            // 
            num_pspTakeoverTimer.Font = new System.Drawing.Font("Segoe UI", 8F);
            num_pspTakeoverTimer.Location = new Point(255, 30);
            num_pspTakeoverTimer.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_pspTakeoverTimer.Name = "num_pspTakeoverTimer";
            num_pspTakeoverTimer.Size = new Size(45, 22);
            num_pspTakeoverTimer.TabIndex = 1;
            num_pspTakeoverTimer.TextAlign = HorizontalAlignment.Center;
            toolTip.SetToolTip(num_pspTakeoverTimer, "Time in Seconds");
            num_pspTakeoverTimer.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label_pspTakeover
            // 
            label_pspTakeover.AutoSize = true;
            label_pspTakeover.Location = new Point(137, 31);
            label_pspTakeover.Name = "label_pspTakeover";
            label_pspTakeover.Size = new Size(107, 15);
            label_pspTakeover.TabIndex = 0;
            label_pspTakeover.Text = "PSP Takeover Time";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(tb_redPassword);
            groupBox6.Controls.Add(cb_autoBalance);
            groupBox6.Controls.Add(tb_bluePassword);
            groupBox6.Dock = DockStyle.Left;
            groupBox6.Location = new Point(0, 0);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(0);
            groupBox6.Size = new Size(150, 118);
            groupBox6.TabIndex = 2;
            groupBox6.TabStop = false;
            groupBox6.Text = "Team Options";
            // 
            // tb_redPassword
            // 
            tb_redPassword.Location = new Point(7, 82);
            tb_redPassword.MaxLength = 16;
            tb_redPassword.Name = "tb_redPassword";
            tb_redPassword.PlaceholderText = "Red Team Password";
            tb_redPassword.Size = new Size(136, 23);
            tb_redPassword.TabIndex = 5;
            tb_redPassword.TextAlign = HorizontalAlignment.Center;
            // 
            // cb_autoBalance
            // 
            cb_autoBalance.AutoSize = true;
            cb_autoBalance.Checked = true;
            cb_autoBalance.CheckState = CheckState.Checked;
            cb_autoBalance.Font = new System.Drawing.Font("Segoe UI", 8F);
            cb_autoBalance.Location = new Point(6, 28);
            cb_autoBalance.Margin = new Padding(0);
            cb_autoBalance.Name = "cb_autoBalance";
            cb_autoBalance.Size = new Size(132, 17);
            cb_autoBalance.TabIndex = 3;
            cb_autoBalance.Text = "Auto Balance Players";
            cb_autoBalance.UseVisualStyleBackColor = true;
            // 
            // tb_bluePassword
            // 
            tb_bluePassword.Location = new Point(7, 53);
            tb_bluePassword.MaxLength = 16;
            tb_bluePassword.Name = "tb_bluePassword";
            tb_bluePassword.PlaceholderText = "Blue Team Password";
            tb_bluePassword.Size = new Size(136, 23);
            tb_bluePassword.TabIndex = 4;
            tb_bluePassword.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox_pingChecks
            // 
            groupBox_pingChecks.Controls.Add(label5);
            groupBox_pingChecks.Controls.Add(label4);
            groupBox_pingChecks.Controls.Add(num_minPing);
            groupBox_pingChecks.Controls.Add(num_maxPing);
            groupBox_pingChecks.Controls.Add(label3);
            groupBox_pingChecks.Controls.Add(cb_enableMaxCheck);
            groupBox_pingChecks.Controls.Add(cb_enableMinCheck);
            groupBox_pingChecks.Dock = DockStyle.Fill;
            groupBox_pingChecks.Location = new Point(150, 0);
            groupBox_pingChecks.Name = "groupBox_pingChecks";
            groupBox_pingChecks.Padding = new Padding(0);
            groupBox_pingChecks.Size = new Size(167, 139);
            groupBox_pingChecks.TabIndex = 7;
            groupBox_pingChecks.TabStop = false;
            groupBox_pingChecks.Text = "Ping Checking";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(100, 91);
            label5.Name = "label5";
            label5.Size = new Size(29, 15);
            label5.TabIndex = 6;
            label5.Text = "Max";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(35, 91);
            label4.Name = "label4";
            label4.Size = new Size(28, 15);
            label4.TabIndex = 5;
            label4.Text = "Min";
            // 
            // num_minPing
            // 
            num_minPing.Enabled = false;
            num_minPing.Location = new Point(19, 109);
            num_minPing.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            num_minPing.Name = "num_minPing";
            num_minPing.Size = new Size(60, 23);
            num_minPing.TabIndex = 4;
            num_minPing.TextAlign = HorizontalAlignment.Center;
            num_minPing.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            // 
            // num_maxPing
            // 
            num_maxPing.Enabled = false;
            num_maxPing.Location = new Point(85, 109);
            num_maxPing.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            num_maxPing.Name = "num_maxPing";
            num_maxPing.Size = new Size(60, 23);
            num_maxPing.TabIndex = 3;
            num_maxPing.TextAlign = HorizontalAlignment.Center;
            num_maxPing.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(35, 74);
            label3.Name = "label3";
            label3.Size = new Size(94, 15);
            label3.TabIndex = 2;
            label3.Text = "Kick Thresholds";
            // 
            // cb_enableMaxCheck
            // 
            cb_enableMaxCheck.AutoSize = true;
            cb_enableMaxCheck.Location = new Point(18, 46);
            cb_enableMaxCheck.Name = "cb_enableMaxCheck";
            cb_enableMaxCheck.Size = new Size(122, 19);
            cb_enableMaxCheck.TabIndex = 1;
            cb_enableMaxCheck.Text = "Enable Max Check";
            cb_enableMaxCheck.UseVisualStyleBackColor = true;
            cb_enableMaxCheck.CheckedChanged += actionClick_enableMaxPing;
            // 
            // cb_enableMinCheck
            // 
            cb_enableMinCheck.AutoSize = true;
            cb_enableMinCheck.Location = new Point(18, 26);
            cb_enableMinCheck.Name = "cb_enableMinCheck";
            cb_enableMinCheck.Size = new Size(121, 19);
            cb_enableMinCheck.TabIndex = 0;
            cb_enableMinCheck.Text = "Enable Min Check";
            cb_enableMinCheck.UseVisualStyleBackColor = true;
            cb_enableMinCheck.CheckedChanged += actionClick_enableMinCheck;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(cb_enableLeftLean);
            groupBox7.Controls.Add(cb_customSkins);
            groupBox7.Controls.Add(cb_enableDistroyBuildings);
            groupBox7.Controls.Add(cb_enableFatBullets);
            groupBox7.Controls.Add(cb_enableOneShotKills);
            groupBox7.Dock = DockStyle.Right;
            groupBox7.Location = new Point(317, 0);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(152, 139);
            groupBox7.TabIndex = 6;
            groupBox7.TabStop = false;
            groupBox7.Text = "Miscellaneous";
            // 
            // cb_enableLeftLean
            // 
            cb_enableLeftLean.AutoSize = true;
            cb_enableLeftLean.Enabled = false;
            cb_enableLeftLean.Location = new Point(15, 105);
            cb_enableLeftLean.Name = "cb_enableLeftLean";
            cb_enableLeftLean.Size = new Size(124, 19);
            cb_enableLeftLean.TabIndex = 3;
            cb_enableLeftLean.Text = "Allow Left Leaning";
            cb_enableLeftLean.UseVisualStyleBackColor = true;
            // 
            // cb_customSkins
            // 
            cb_customSkins.AutoSize = true;
            cb_customSkins.Location = new Point(15, 26);
            cb_customSkins.Name = "cb_customSkins";
            cb_customSkins.Size = new Size(131, 19);
            cb_customSkins.TabIndex = 2;
            cb_customSkins.Text = "Allow Custom Skins";
            cb_customSkins.UseVisualStyleBackColor = true;
            // 
            // cb_enableDistroyBuildings
            // 
            cb_enableDistroyBuildings.AutoSize = true;
            cb_enableDistroyBuildings.Location = new Point(15, 46);
            cb_enableDistroyBuildings.Name = "cb_enableDistroyBuildings";
            cb_enableDistroyBuildings.Size = new Size(118, 19);
            cb_enableDistroyBuildings.TabIndex = 2;
            cb_enableDistroyBuildings.Text = "Destroy Buildings";
            cb_enableDistroyBuildings.UseVisualStyleBackColor = true;
            // 
            // cb_enableFatBullets
            // 
            cb_enableFatBullets.AutoSize = true;
            cb_enableFatBullets.Location = new Point(15, 66);
            cb_enableFatBullets.Name = "cb_enableFatBullets";
            cb_enableFatBullets.Size = new Size(80, 19);
            cb_enableFatBullets.TabIndex = 1;
            cb_enableFatBullets.Text = "Fat Bullets";
            cb_enableFatBullets.UseVisualStyleBackColor = true;
            // 
            // cb_enableOneShotKills
            // 
            cb_enableOneShotKills.AutoSize = true;
            cb_enableOneShotKills.Location = new Point(15, 86);
            cb_enableOneShotKills.Name = "cb_enableOneShotKills";
            cb_enableOneShotKills.Size = new Size(99, 19);
            cb_enableOneShotKills.TabIndex = 0;
            cb_enableOneShotKills.Text = "One Shot Kills";
            cb_enableOneShotKills.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(num_maxFFKills);
            groupBox1.Controls.Add(cb_enableFFkills);
            groupBox1.Controls.Add(label_maxFFkills);
            groupBox1.Controls.Add(cb_warnFFkils);
            groupBox1.Controls.Add(cb_showTeamTags);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(150, 139);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Friendly Fire";
            // 
            // num_maxFFKills
            // 
            num_maxFFKills.Location = new Point(89, 95);
            num_maxFFKills.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            num_maxFFKills.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_maxFFKills.Name = "num_maxFFKills";
            num_maxFFKills.Size = new Size(40, 23);
            num_maxFFKills.TabIndex = 8;
            num_maxFFKills.TextAlign = HorizontalAlignment.Center;
            num_maxFFKills.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // cb_enableFFkills
            // 
            cb_enableFFkills.AutoSize = true;
            cb_enableFFkills.Location = new Point(20, 26);
            cb_enableFFkills.Name = "cb_enableFFkills";
            cb_enableFFkills.Size = new Size(100, 19);
            cb_enableFFkills.TabIndex = 4;
            cb_enableFFkills.Text = "Enable FF Kills";
            cb_enableFFkills.UseVisualStyleBackColor = true;
            cb_enableFFkills.CheckedChanged += actionClick_enableFFkills;
            // 
            // label_maxFFkills
            // 
            label_maxFFkills.AutoSize = true;
            label_maxFFkills.Location = new Point(13, 99);
            label_maxFFkills.Name = "label_maxFFkills";
            label_maxFFkills.Size = new Size(68, 15);
            label_maxFFkills.TabIndex = 7;
            label_maxFFkills.Text = "Max FF Kills";
            // 
            // cb_warnFFkils
            // 
            cb_warnFFkils.AutoSize = true;
            cb_warnFFkils.Location = new Point(20, 46);
            cb_warnFFkils.Name = "cb_warnFFkils";
            cb_warnFFkils.Size = new Size(93, 19);
            cb_warnFFkils.TabIndex = 6;
            cb_warnFFkils.Text = "Warn FF Kills";
            cb_warnFFkils.UseVisualStyleBackColor = true;
            // 
            // cb_showTeamTags
            // 
            cb_showTeamTags.AutoSize = true;
            cb_showTeamTags.Location = new Point(20, 66);
            cb_showTeamTags.Name = "cb_showTeamTags";
            cb_showTeamTags.Size = new Size(97, 19);
            cb_showTeamTags.TabIndex = 5;
            cb_showTeamTags.Text = "Show FF Tags";
            cb_showTeamTags.UseVisualStyleBackColor = true;
            // 
            // tabRestrictions
            // 
            tabRestrictions.Controls.Add(groupBox_weapon);
            tabRestrictions.Location = new Point(4, 24);
            tabRestrictions.Name = "tabRestrictions";
            tabRestrictions.Padding = new Padding(3);
            tabRestrictions.Size = new Size(475, 267);
            tabRestrictions.TabIndex = 3;
            tabRestrictions.Text = "Restrictions";
            tabRestrictions.UseVisualStyleBackColor = true;
            // 
            // groupBox_weapon
            // 
            groupBox_weapon.Controls.Add(checkBox_selectNone);
            groupBox_weapon.Controls.Add(checkBox_selectAll);
            groupBox_weapon.Controls.Add(cb_weapSatchel);
            groupBox_weapon.Controls.Add(cb_weapAT4);
            groupBox_weapon.Controls.Add(cb_weapSmokeGrenade);
            groupBox_weapon.Controls.Add(cb_weapClay);
            groupBox_weapon.Controls.Add(cb_weap300Tact);
            groupBox_weapon.Controls.Add(cb_weapFragGrenade);
            groupBox_weapon.Controls.Add(cb_weapFlashBang);
            groupBox_weapon.Controls.Add(cb_weapG36);
            groupBox_weapon.Controls.Add(groupBox_roles);
            groupBox_weapon.Controls.Add(cb_weapPSG1);
            groupBox_weapon.Controls.Add(cb_weapG3);
            groupBox_weapon.Controls.Add(cb_weapMP5);
            groupBox_weapon.Controls.Add(cb_weapM240);
            groupBox_weapon.Controls.Add(cb_weapM60);
            groupBox_weapon.Controls.Add(cb_weapSaw);
            groupBox_weapon.Controls.Add(cb_weapBarret);
            groupBox_weapon.Controls.Add(cb_weapM24);
            groupBox_weapon.Controls.Add(cb_weapM21);
            groupBox_weapon.Controls.Add(cb_weapM16203);
            groupBox_weapon.Controls.Add(cb_weapM16);
            groupBox_weapon.Controls.Add(cb_weapCAR15203);
            groupBox_weapon.Controls.Add(cb_weapCAR15);
            groupBox_weapon.Controls.Add(cb_weapShotgun);
            groupBox_weapon.Controls.Add(cb_weapM9Bereatta);
            groupBox_weapon.Controls.Add(cb_weapColt45);
            groupBox_weapon.Dock = DockStyle.Fill;
            groupBox_weapon.Location = new Point(3, 3);
            groupBox_weapon.Name = "groupBox_weapon";
            groupBox_weapon.Size = new Size(469, 261);
            groupBox_weapon.TabIndex = 0;
            groupBox_weapon.TabStop = false;
            groupBox_weapon.Text = "Weapon Restrictions (Checked = Enabled)";
            // 
            // checkBox_selectNone
            // 
            checkBox_selectNone.AutoSize = true;
            checkBox_selectNone.Location = new Point(220, 47);
            checkBox_selectNone.Name = "checkBox_selectNone";
            checkBox_selectNone.Size = new Size(89, 19);
            checkBox_selectNone.TabIndex = 27;
            checkBox_selectNone.Text = "Select None";
            checkBox_selectNone.UseVisualStyleBackColor = true;
            checkBox_selectNone.Click += actionClick_WeaponCheckedChanged;
            // 
            // checkBox_selectAll
            // 
            checkBox_selectAll.AutoSize = true;
            checkBox_selectAll.Location = new Point(20, 47);
            checkBox_selectAll.Name = "checkBox_selectAll";
            checkBox_selectAll.Size = new Size(74, 19);
            checkBox_selectAll.TabIndex = 26;
            checkBox_selectAll.Text = "Select All";
            checkBox_selectAll.UseVisualStyleBackColor = true;
            checkBox_selectAll.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSatchel
            // 
            cb_weapSatchel.AutoSize = true;
            cb_weapSatchel.Location = new Point(338, 172);
            cb_weapSatchel.Name = "cb_weapSatchel";
            cb_weapSatchel.Size = new Size(105, 19);
            cb_weapSatchel.TabIndex = 25;
            cb_weapSatchel.Text = "Satchel Charge";
            cb_weapSatchel.UseVisualStyleBackColor = true;
            cb_weapSatchel.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapAT4
            // 
            cb_weapAT4.AutoSize = true;
            cb_weapAT4.Location = new Point(220, 197);
            cb_weapAT4.Name = "cb_weapAT4";
            cb_weapAT4.Size = new Size(46, 19);
            cb_weapAT4.TabIndex = 24;
            cb_weapAT4.Text = "AT4";
            cb_weapAT4.UseVisualStyleBackColor = true;
            cb_weapAT4.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSmokeGrenade
            // 
            cb_weapSmokeGrenade.AutoSize = true;
            cb_weapSmokeGrenade.Location = new Point(220, 122);
            cb_weapSmokeGrenade.Name = "cb_weapSmokeGrenade";
            cb_weapSmokeGrenade.Size = new Size(109, 19);
            cb_weapSmokeGrenade.TabIndex = 23;
            cb_weapSmokeGrenade.Text = "Smoke Grenade";
            cb_weapSmokeGrenade.UseVisualStyleBackColor = true;
            cb_weapSmokeGrenade.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapClay
            // 
            cb_weapClay.AutoSize = true;
            cb_weapClay.Location = new Point(220, 172);
            cb_weapClay.Name = "cb_weapClay";
            cb_weapClay.Size = new Size(77, 19);
            cb_weapClay.TabIndex = 22;
            cb_weapClay.Text = "Claymore";
            cb_weapClay.UseVisualStyleBackColor = true;
            cb_weapClay.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weap300Tact
            // 
            cb_weap300Tact.AutoSize = true;
            cb_weap300Tact.Location = new Point(338, 222);
            cb_weap300Tact.Name = "cb_weap300Tact";
            cb_weap300Tact.Size = new Size(125, 19);
            cb_weap300Tact.TabIndex = 15;
            cb_weap300Tact.Text = "MCRT .300 Tactical";
            cb_weap300Tact.UseVisualStyleBackColor = true;
            cb_weap300Tact.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapFragGrenade
            // 
            cb_weapFragGrenade.AutoSize = true;
            cb_weapFragGrenade.Location = new Point(220, 97);
            cb_weapFragGrenade.Name = "cb_weapFragGrenade";
            cb_weapFragGrenade.Size = new Size(96, 19);
            cb_weapFragGrenade.TabIndex = 21;
            cb_weapFragGrenade.Text = "Frag Grenade";
            cb_weapFragGrenade.UseVisualStyleBackColor = true;
            cb_weapFragGrenade.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapFlashBang
            // 
            cb_weapFlashBang.AutoSize = true;
            cb_weapFlashBang.Location = new Point(220, 147);
            cb_weapFlashBang.Name = "cb_weapFlashBang";
            cb_weapFlashBang.Size = new Size(83, 19);
            cb_weapFlashBang.TabIndex = 20;
            cb_weapFlashBang.Text = "Flash Bang";
            cb_weapFlashBang.UseVisualStyleBackColor = true;
            cb_weapFlashBang.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapG36
            // 
            cb_weapG36.AutoSize = true;
            cb_weapG36.Location = new Point(119, 147);
            cb_weapG36.Name = "cb_weapG36";
            cb_weapG36.Size = new Size(46, 19);
            cb_weapG36.TabIndex = 19;
            cb_weapG36.Text = "G36";
            cb_weapG36.UseVisualStyleBackColor = true;
            cb_weapG36.Click += actionClick_WeaponCheckedChanged;
            // 
            // groupBox_roles
            // 
            groupBox_roles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox_roles.Controls.Add(cb_roleSniper);
            groupBox_roles.Controls.Add(cb_roleMedic);
            groupBox_roles.Controls.Add(cb_roleGunner);
            groupBox_roles.Controls.Add(cb_roleCQB);
            groupBox_roles.Enabled = false;
            groupBox_roles.Location = new Point(338, 17);
            groupBox_roles.Name = "groupBox_roles";
            groupBox_roles.Size = new Size(131, 136);
            groupBox_roles.TabIndex = 1;
            groupBox_roles.TabStop = false;
            groupBox_roles.Text = "Role Restrictions";
            // 
            // cb_roleSniper
            // 
            cb_roleSniper.AutoSize = true;
            cb_roleSniper.Location = new Point(30, 104);
            cb_roleSniper.Name = "cb_roleSniper";
            cb_roleSniper.Size = new Size(59, 19);
            cb_roleSniper.TabIndex = 2;
            cb_roleSniper.Text = "Sniper";
            cb_roleSniper.UseVisualStyleBackColor = true;
            // 
            // cb_roleMedic
            // 
            cb_roleMedic.AutoSize = true;
            cb_roleMedic.Location = new Point(30, 80);
            cb_roleMedic.Name = "cb_roleMedic";
            cb_roleMedic.Size = new Size(59, 19);
            cb_roleMedic.TabIndex = 2;
            cb_roleMedic.Text = "Medic";
            cb_roleMedic.UseVisualStyleBackColor = true;
            // 
            // cb_roleGunner
            // 
            cb_roleGunner.AutoSize = true;
            cb_roleGunner.Location = new Point(30, 55);
            cb_roleGunner.Name = "cb_roleGunner";
            cb_roleGunner.Size = new Size(65, 19);
            cb_roleGunner.TabIndex = 2;
            cb_roleGunner.Text = "Gunner";
            cb_roleGunner.UseVisualStyleBackColor = true;
            // 
            // cb_roleCQB
            // 
            cb_roleCQB.AutoSize = true;
            cb_roleCQB.Location = new Point(30, 30);
            cb_roleCQB.Name = "cb_roleCQB";
            cb_roleCQB.Size = new Size(50, 19);
            cb_roleCQB.TabIndex = 2;
            cb_roleCQB.Text = "CQB";
            cb_roleCQB.UseVisualStyleBackColor = true;
            // 
            // cb_weapPSG1
            // 
            cb_weapPSG1.AutoSize = true;
            cb_weapPSG1.Location = new Point(220, 222);
            cb_weapPSG1.Name = "cb_weapPSG1";
            cb_weapPSG1.Size = new Size(58, 19);
            cb_weapPSG1.TabIndex = 6;
            cb_weapPSG1.Text = "PSG-1";
            cb_weapPSG1.UseVisualStyleBackColor = true;
            cb_weapPSG1.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapG3
            // 
            cb_weapG3.AutoSize = true;
            cb_weapG3.Location = new Point(20, 147);
            cb_weapG3.Name = "cb_weapG3";
            cb_weapG3.Size = new Size(40, 19);
            cb_weapG3.TabIndex = 2;
            cb_weapG3.Text = "G3";
            cb_weapG3.UseVisualStyleBackColor = true;
            cb_weapG3.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapMP5
            // 
            cb_weapMP5.AutoSize = true;
            cb_weapMP5.Location = new Point(20, 197);
            cb_weapMP5.Name = "cb_weapMP5";
            cb_weapMP5.Size = new Size(50, 19);
            cb_weapMP5.TabIndex = 2;
            cb_weapMP5.Text = "MP5";
            cb_weapMP5.UseVisualStyleBackColor = true;
            cb_weapMP5.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM240
            // 
            cb_weapM240.AutoSize = true;
            cb_weapM240.Location = new Point(119, 172);
            cb_weapM240.Name = "cb_weapM240";
            cb_weapM240.Size = new Size(55, 19);
            cb_weapM240.TabIndex = 2;
            cb_weapM240.Text = "M240";
            cb_weapM240.UseVisualStyleBackColor = true;
            cb_weapM240.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM60
            // 
            cb_weapM60.AutoSize = true;
            cb_weapM60.Location = new Point(20, 172);
            cb_weapM60.Name = "cb_weapM60";
            cb_weapM60.Size = new Size(49, 19);
            cb_weapM60.TabIndex = 18;
            cb_weapM60.Text = "M60";
            cb_weapM60.UseVisualStyleBackColor = true;
            cb_weapM60.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSaw
            // 
            cb_weapSaw.AutoSize = true;
            cb_weapSaw.Location = new Point(119, 197);
            cb_weapSaw.Name = "cb_weapSaw";
            cb_weapSaw.Size = new Size(50, 19);
            cb_weapSaw.TabIndex = 17;
            cb_weapSaw.Text = "SAW";
            cb_weapSaw.UseVisualStyleBackColor = true;
            cb_weapSaw.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapBarret
            // 
            cb_weapBarret.AutoSize = true;
            cb_weapBarret.Location = new Point(119, 222);
            cb_weapBarret.Name = "cb_weapBarret";
            cb_weapBarret.Size = new Size(57, 19);
            cb_weapBarret.TabIndex = 16;
            cb_weapBarret.Text = "Barret";
            cb_weapBarret.UseVisualStyleBackColor = true;
            cb_weapBarret.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM24
            // 
            cb_weapM24.AutoSize = true;
            cb_weapM24.Location = new Point(338, 197);
            cb_weapM24.Name = "cb_weapM24";
            cb_weapM24.Size = new Size(49, 19);
            cb_weapM24.TabIndex = 14;
            cb_weapM24.Text = "M24";
            cb_weapM24.UseVisualStyleBackColor = true;
            cb_weapM24.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM21
            // 
            cb_weapM21.AutoSize = true;
            cb_weapM21.Location = new Point(20, 222);
            cb_weapM21.Name = "cb_weapM21";
            cb_weapM21.Size = new Size(49, 19);
            cb_weapM21.TabIndex = 13;
            cb_weapM21.Text = "M21";
            cb_weapM21.UseVisualStyleBackColor = true;
            cb_weapM21.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM16203
            // 
            cb_weapM16203.AutoSize = true;
            cb_weapM16203.Location = new Point(119, 122);
            cb_weapM16203.Name = "cb_weapM16203";
            cb_weapM16203.Size = new Size(72, 19);
            cb_weapM16203.TabIndex = 12;
            cb_weapM16203.Text = "M16/203";
            cb_weapM16203.UseVisualStyleBackColor = true;
            cb_weapM16203.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM16
            // 
            cb_weapM16.AutoSize = true;
            cb_weapM16.Location = new Point(119, 98);
            cb_weapM16.Name = "cb_weapM16";
            cb_weapM16.Size = new Size(49, 19);
            cb_weapM16.TabIndex = 11;
            cb_weapM16.Text = "M16";
            cb_weapM16.UseVisualStyleBackColor = true;
            cb_weapM16.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapCAR15203
            // 
            cb_weapCAR15203.AutoSize = true;
            cb_weapCAR15203.Location = new Point(20, 122);
            cb_weapCAR15203.Name = "cb_weapCAR15203";
            cb_weapCAR15203.Size = new Size(87, 19);
            cb_weapCAR15203.TabIndex = 10;
            cb_weapCAR15203.Text = "CAR 15/203";
            cb_weapCAR15203.UseVisualStyleBackColor = true;
            cb_weapCAR15203.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapCAR15
            // 
            cb_weapCAR15.AutoSize = true;
            cb_weapCAR15.Location = new Point(20, 97);
            cb_weapCAR15.Name = "cb_weapCAR15";
            cb_weapCAR15.Size = new Size(64, 19);
            cb_weapCAR15.TabIndex = 9;
            cb_weapCAR15.Text = "CAR 15";
            cb_weapCAR15.UseVisualStyleBackColor = true;
            cb_weapCAR15.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapShotgun
            // 
            cb_weapShotgun.AutoSize = true;
            cb_weapShotgun.Location = new Point(220, 72);
            cb_weapShotgun.Name = "cb_weapShotgun";
            cb_weapShotgun.Size = new Size(71, 19);
            cb_weapShotgun.TabIndex = 8;
            cb_weapShotgun.Text = "Shotgun";
            cb_weapShotgun.UseVisualStyleBackColor = true;
            cb_weapShotgun.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM9Bereatta
            // 
            cb_weapM9Bereatta.AutoSize = true;
            cb_weapM9Bereatta.Location = new Point(119, 72);
            cb_weapM9Bereatta.Name = "cb_weapM9Bereatta";
            cb_weapM9Bereatta.Size = new Size(83, 19);
            cb_weapM9Bereatta.TabIndex = 7;
            cb_weapM9Bereatta.Text = "M9 Beretta";
            cb_weapM9Bereatta.UseVisualStyleBackColor = true;
            cb_weapM9Bereatta.Click += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapColt45
            // 
            cb_weapColt45.AutoSize = true;
            cb_weapColt45.Location = new Point(20, 72);
            cb_weapColt45.Name = "cb_weapColt45";
            cb_weapColt45.Size = new Size(66, 19);
            cb_weapColt45.TabIndex = 6;
            cb_weapColt45.Text = "Colt .45";
            cb_weapColt45.UseVisualStyleBackColor = true;
            cb_weapColt45.Click += actionClick_WeaponCheckedChanged;
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
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dataGridView_chatMessages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
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
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dateTime.DefaultCellStyle = dataGridViewCellStyle7;
            dateTime.HeaderText = "Timestamp";
            dateTime.MaxInputLength = 12;
            dateTime.Name = "dateTime";
            dateTime.Width = 75;
            // 
            // Team
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Team.DefaultCellStyle = dataGridViewCellStyle8;
            Team.HeaderText = "Team";
            Team.MaxInputLength = 24;
            Team.Name = "Team";
            Team.Width = 75;
            // 
            // Player
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Player.DefaultCellStyle = dataGridViewCellStyle9;
            Player.HeaderText = "Player";
            Player.MaxInputLength = 32;
            Player.Name = "Player";
            // 
            // Message
            // 
            Message.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Message.DefaultCellStyle = dataGridViewCellStyle10;
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
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 5, 0);
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
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Controls.Add(groupBox9, 0, 0);
            tableLayoutPanel6.Controls.Add(tableLayoutPanel8, 0, 1);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(360, 0);
            tableLayoutPanel6.Margin = new Padding(0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2441864F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 85.75581F));
            tableLayoutPanel6.Size = new Size(536, 356);
            tableLayoutPanel6.TabIndex = 2;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(tableLayoutPanel7);
            groupBox9.Dock = DockStyle.Fill;
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
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Controls.Add(label13, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 53);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 2;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 8.135593F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 91.86441F));
            tableLayoutPanel8.Size = new Size(530, 300);
            tableLayoutPanel8.TabIndex = 1;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(0, 0);
            label13.Margin = new Padding(0);
            label13.Name = "label13";
            label13.Padding = new Padding(0, 3, 0, 0);
            label13.Size = new Size(530, 24);
            label13.TabIndex = 0;
            label13.Text = "You may add a record with or without both pieces of information.";
            label13.TextAlign = ContentAlignment.TopCenter;
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
            Text = "Black Hawk Down Server Manager - Remote Client";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            mainPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabServer.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxPlayers).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoreBoardDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_respawnTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_gameStartDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_gameTimeLimit).EndInit();
            groupBox2.ResumeLayout(false);
            scoresTableLayout.ResumeLayout(false);
            scoresTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_scoresFB).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresDM).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresKOTH).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_remotePort).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_serverPort).EndInit();
            groupBox_hostInfo.ResumeLayout(false);
            groupBox_hostInfo.PerformLayout();
            panelRight.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel20.ResumeLayout(false);
            panel_rightoptions.ResumeLayout(false);
            tabServerControls.ResumeLayout(false);
            tabOptions.ResumeLayout(false);
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox_gamePlay.ResumeLayout(false);
            groupBox_gamePlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxTeamLives).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_flagReturnTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_pspTakeoverTimer).EndInit();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox_pingChecks.ResumeLayout(false);
            groupBox_pingChecks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_minPing).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_maxPing).EndInit();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxFFKills).EndInit();
            tabRestrictions.ResumeLayout(false);
            groupBox_weapon.ResumeLayout(false);
            groupBox_weapon.PerformLayout();
            groupBox_roles.ResumeLayout(false);
            groupBox_roles.PerformLayout();
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
            tableLayoutPanel6.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
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
        private SplitContainer splitContainer;
        private Panel panelLeft;
        public GroupBox groupBox5;
        public Label label_replayMaps;
        public ComboBox cb_replayMaps;
        public Label label_respawnTime;
        public NumericUpDown num_respawnTime;
        public Label label_startDelay;
        public Label label_timeLimit;
        public NumericUpDown num_gameStartDelay;
        public NumericUpDown num_gameTimeLimit;
        public GroupBox groupBox2;
        public TableLayoutPanel scoresTableLayout;
        public NumericUpDown num_scoresFB;
        public NumericUpDown num_scoresDM;
        public Label label_flagball;
        public NumericUpDown num_scoresKOTH;
        public Label label_koth;
        public Label label_dm;
        public GroupBox groupBox4;
        public ComboBox cb_sessionType;
        public TextBox tb_serverPassword;
        public CheckBox cb_novaRequired;
        public NumericUpDown num_serverPort;
        public CheckBox cb_serverDedicated;
        public ComboBox cb_serverIP;
        public GroupBox groupBox_hostInfo;
        public TextBox tb_serverMessage;
        public TextBox tb_hostName;
        public TextBox tb_serverName;
        public TextBox tb_serverID;
        private Panel panelRight;
        private Panel panel_rightoptions;
        private TabControl tabServerControls;
        private TabPage tabOptions;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btn_startStop;
        private Button btn_reset;
        private Button btn_load;
        private Button btn_update;
        private Panel panel2;
        private SplitContainer splitContainer1;
        public GroupBox groupBox_gamePlay;
        public CheckBox cb_autoRange;
        public NumericUpDown num_maxTeamLives;
        public Label label2;
        public CheckBox cb_showClays;
        public NumericUpDown num_flagReturnTime;
        public Label label1;
        public CheckBox cb_showTracers;
        public NumericUpDown num_pspTakeoverTimer;
        public Label label_pspTakeover;
        public GroupBox groupBox6;
        public TextBox tb_redPassword;
        public CheckBox cb_autoBalance;
        public TextBox tb_bluePassword;
        public GroupBox groupBox_pingChecks;
        public Label label5;
        public Label label4;
        public NumericUpDown num_minPing;
        public NumericUpDown num_maxPing;
        public Label label3;
        public CheckBox cb_enableMaxCheck;
        public CheckBox cb_enableMinCheck;
        public GroupBox groupBox7;
        public CheckBox cb_customSkins;
        public CheckBox cb_enableDistroyBuildings;
        public CheckBox cb_enableFatBullets;
        public CheckBox cb_enableOneShotKills;
        public GroupBox groupBox1;
        public NumericUpDown num_maxFFKills;
        public CheckBox cb_enableFFkills;
        public Label label_maxFFkills;
        public CheckBox cb_warnFFkils;
        public CheckBox cb_showTeamTags;
        public TabPage tabRestrictions;
        public GroupBox groupBox_weapon;
        public CheckBox checkBox_selectNone;
        public CheckBox checkBox_selectAll;
        public CheckBox cb_weapSatchel;
        public CheckBox cb_weapAT4;
        public CheckBox cb_weapSmokeGrenade;
        public CheckBox cb_weapClay;
        public CheckBox cb_weap300Tact;
        public CheckBox cb_weapFragGrenade;
        public CheckBox cb_weapFlashBang;
        public CheckBox cb_weapG36;
        public GroupBox groupBox_roles;
        public CheckBox cb_roleSniper;
        public CheckBox cb_roleMedic;
        public CheckBox cb_roleGunner;
        public CheckBox cb_roleCQB;
        public CheckBox cb_weapPSG1;
        public CheckBox cb_weapG3;
        public CheckBox cb_weapMP5;
        public CheckBox cb_weapM240;
        public CheckBox cb_weapM60;
        public CheckBox cb_weapSaw;
        public CheckBox cb_weapBarret;
        public CheckBox cb_weapM24;
        public CheckBox cb_weapM21;
        public CheckBox cb_weapM16203;
        public CheckBox cb_weapM16;
        public CheckBox cb_weapCAR15203;
        public CheckBox cb_weapCAR15;
        public CheckBox cb_weapShotgun;
        public CheckBox cb_weapM9Bereatta;
        public CheckBox cb_weapColt45;
        private TabPage tabMaps;
        private TabPage tabPlayers;
        public CheckBox cb_enableLeftLean;
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
        public Label label_scoreDelay;
        public NumericUpDown num_scoreBoardDelay;
        public Label label_maxPlayers;
        public NumericUpDown num_maxPlayers;
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
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox groupBox9;
        private TableLayoutPanel tableLayoutPanel7;
        private ComboBox cb_banSubMask;
        private Button btn_addBan;
        private TableLayoutPanel tableLayoutPanel8;
        private Label label13;
        internal TabControl tabControl;
        internal DataGridView dg_playerNames;
        internal DataGridView dg_IPAddresses;
        internal TextBox tb_bansPlayerName;
        internal TextBox tb_bansIPAddress;
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
        private DataGridView dg_AdminUsers;
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
        internal CheckBox cb_enableRemote;
        internal NumericUpDown num_remotePort;
        private FontAwesome.Sharp.IconToolStripButton btn_UpdatePath;
        private TableLayoutPanel tableLayoutPanel20;
        private Button button1;
        public FontAwesome.Sharp.IconButton ib_resetCurrentMaps;
    }
}
