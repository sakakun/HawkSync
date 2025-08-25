namespace BHD_ServerManager.Forms.Panels
{
    partial class tabStats
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
            tabControl1 = new TabControl();
            tabBabstats = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dg_statsLog = new DataGridView();
            statsLog_DateTime = new DataGridViewTextBoxColumn();
            statLog_Message = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            groupBox4 = new GroupBox();
            btn_SaveSettings = new Button();
            groupBox3 = new GroupBox();
            labelUpatedInterval = new Label();
            num_WebStatsUpdates = new NumericUpDown();
            labelReportInterval = new Label();
            num_WebStatsReport = new NumericUpDown();
            groupBox2 = new GroupBox();
            cb_enableAnnouncements = new CheckBox();
            cb_enableWebStats = new CheckBox();
            groupBox1 = new GroupBox();
            btn_validate = new Button();
            tb_webStatsServerPath = new TextBox();
            tabPlayerStats = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            dataGridViewPlayerStats = new DataGridView();
            panel2 = new Panel();
            label1 = new Label();
            tabWeaponStats = new TabPage();
            dataGridViewWeaponStats = new DataGridView();
            Weapon_PlayerName = new DataGridViewTextBoxColumn();
            WeaponName = new DataGridViewTextBoxColumn();
            Timer = new DataGridViewTextBoxColumn();
            Weapon_Kills = new DataGridViewTextBoxColumn();
            Weapon_Shots = new DataGridViewTextBoxColumn();
            PlayerName = new DataGridViewTextBoxColumn();
            Suicides = new DataGridViewTextBoxColumn();
            Murders = new DataGridViewTextBoxColumn();
            Kills = new DataGridViewTextBoxColumn();
            Deaths = new DataGridViewTextBoxColumn();
            ZoneTime = new DataGridViewTextBoxColumn();
            FBCaptures = new DataGridViewTextBoxColumn();
            FlagSaves = new DataGridViewTextBoxColumn();
            ADTargetsDestroyed = new DataGridViewTextBoxColumn();
            RevivesReceived = new DataGridViewTextBoxColumn();
            RevivesGiven = new DataGridViewTextBoxColumn();
            PSPAttempts = new DataGridViewTextBoxColumn();
            PSPTakeovers = new DataGridViewTextBoxColumn();
            FBCarrierKills = new DataGridViewTextBoxColumn();
            DoubleKills = new DataGridViewTextBoxColumn();
            Headshots = new DataGridViewTextBoxColumn();
            KnifeKills = new DataGridViewTextBoxColumn();
            SniperKills = new DataGridViewTextBoxColumn();
            TKOTHDefenseKills = new DataGridViewTextBoxColumn();
            TKOTHAttackKills = new DataGridViewTextBoxColumn();
            ShotsPerKill = new DataGridViewTextBoxColumn();
            ExperiencePoints = new DataGridViewTextBoxColumn();
            PlayerTeam = new DataGridViewTextBoxColumn();
            PlayerActive = new DataGridViewTextBoxColumn();
            TimePlayed = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            tabBabstats.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_statsLog).BeginInit();
            panel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPlayerStats.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).BeginInit();
            panel2.SuspendLayout();
            tabWeaponStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabBabstats);
            tabControl1.Controls.Add(tabPlayerStats);
            tabControl1.Controls.Add(tabWeaponStats);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(902, 362);
            tabControl1.TabIndex = 0;
            // 
            // tabBabstats
            // 
            tabBabstats.Controls.Add(tableLayoutPanel1);
            tabBabstats.Location = new Point(4, 24);
            tabBabstats.Margin = new Padding(0);
            tabBabstats.Name = "tabBabstats";
            tabBabstats.Padding = new Padding(3);
            tabBabstats.Size = new Size(894, 334);
            tabBabstats.TabIndex = 0;
            tabBabstats.Text = "Babstats";
            tabBabstats.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.5450439F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.4549561F));
            tableLayoutPanel1.Controls.Add(dg_statsLog, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(888, 328);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dg_statsLog
            // 
            dg_statsLog.AllowUserToAddRows = false;
            dg_statsLog.AllowUserToDeleteRows = false;
            dg_statsLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_statsLog.Columns.AddRange(new DataGridViewColumn[] { statsLog_DateTime, statLog_Message });
            dg_statsLog.Dock = DockStyle.Fill;
            dg_statsLog.Location = new Point(292, 3);
            dg_statsLog.Name = "dg_statsLog";
            dg_statsLog.ReadOnly = true;
            dg_statsLog.RowHeadersVisible = false;
            dg_statsLog.Size = new Size(593, 322);
            dg_statsLog.TabIndex = 1;
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
            // panel1
            // 
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(289, 328);
            panel1.TabIndex = 2;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btn_SaveSettings);
            groupBox4.Dock = DockStyle.Bottom;
            groupBox4.Location = new Point(0, 271);
            groupBox4.Margin = new Padding(0, 3, 3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(289, 57);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Controls";
            // 
            // btn_SaveSettings
            // 
            btn_SaveSettings.Location = new Point(6, 22);
            btn_SaveSettings.Name = "btn_SaveSettings";
            btn_SaveSettings.Size = new Size(75, 23);
            btn_SaveSettings.TabIndex = 10;
            btn_SaveSettings.Text = "Save";
            btn_SaveSettings.UseVisualStyleBackColor = true;
            btn_SaveSettings.Click += actionClick_SaveStatSettings;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(labelUpatedInterval);
            groupBox3.Controls.Add(num_WebStatsUpdates);
            groupBox3.Controls.Add(labelReportInterval);
            groupBox3.Controls.Add(num_WebStatsReport);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(0, 137);
            groupBox3.Margin = new Padding(0, 3, 3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(289, 85);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Reporting Intervals (Seconds)";
            // 
            // labelUpatedInterval
            // 
            labelUpatedInterval.AutoSize = true;
            labelUpatedInterval.Location = new Point(89, 55);
            labelUpatedInterval.Name = "labelUpatedInterval";
            labelUpatedInterval.Size = new Size(133, 15);
            labelUpatedInterval.TabIndex = 11;
            labelUpatedInterval.Text = "In-game Player Updates";
            labelUpatedInterval.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // num_WebStatsUpdates
            // 
            num_WebStatsUpdates.Location = new Point(228, 51);
            num_WebStatsUpdates.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            num_WebStatsUpdates.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            num_WebStatsUpdates.Name = "num_WebStatsUpdates";
            num_WebStatsUpdates.Size = new Size(55, 23);
            num_WebStatsUpdates.TabIndex = 10;
            num_WebStatsUpdates.TextAlign = HorizontalAlignment.Center;
            num_WebStatsUpdates.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // labelReportInterval
            // 
            labelReportInterval.AutoSize = true;
            labelReportInterval.Location = new Point(77, 26);
            labelReportInterval.Name = "labelReportInterval";
            labelReportInterval.Size = new Size(145, 15);
            labelReportInterval.TabIndex = 9;
            labelReportInterval.Text = "Announcements / Awards";
            labelReportInterval.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // num_WebStatsReport
            // 
            num_WebStatsReport.Location = new Point(228, 22);
            num_WebStatsReport.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            num_WebStatsReport.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            num_WebStatsReport.Name = "num_WebStatsReport";
            num_WebStatsReport.Size = new Size(55, 23);
            num_WebStatsReport.TabIndex = 7;
            num_WebStatsReport.TextAlign = HorizontalAlignment.Center;
            num_WebStatsReport.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cb_enableAnnouncements);
            groupBox2.Controls.Add(cb_enableWebStats);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 84);
            groupBox2.Margin = new Padding(0, 3, 3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(289, 53);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Options";
            // 
            // cb_enableAnnouncements
            // 
            cb_enableAnnouncements.AutoSize = true;
            cb_enableAnnouncements.CheckAlign = ContentAlignment.MiddleRight;
            cb_enableAnnouncements.Location = new Point(131, 22);
            cb_enableAnnouncements.Name = "cb_enableAnnouncements";
            cb_enableAnnouncements.Size = new Size(152, 19);
            cb_enableAnnouncements.TabIndex = 5;
            cb_enableAnnouncements.Text = "Enable Announcements";
            cb_enableAnnouncements.UseVisualStyleBackColor = true;
            cb_enableAnnouncements.CheckedChanged += ActionEvent_EnableAnnouncements;
            // 
            // cb_enableWebStats
            // 
            cb_enableWebStats.AutoSize = true;
            cb_enableWebStats.CheckAlign = ContentAlignment.MiddleRight;
            cb_enableWebStats.Location = new Point(6, 22);
            cb_enableWebStats.Name = "cb_enableWebStats";
            cb_enableWebStats.Size = new Size(108, 19);
            cb_enableWebStats.TabIndex = 0;
            cb_enableWebStats.Text = "Enable Babstats";
            cb_enableWebStats.UseVisualStyleBackColor = true;
            cb_enableWebStats.CheckedChanged += ActionEvent_EnableBabStats;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_validate);
            groupBox1.Controls.Add(tb_webStatsServerPath);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Margin = new Padding(0, 3, 3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(289, 84);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Stats Web Address (HTTP/HTTPS)";
            // 
            // btn_validate
            // 
            btn_validate.Location = new Point(208, 51);
            btn_validate.Name = "btn_validate";
            btn_validate.Size = new Size(75, 23);
            btn_validate.TabIndex = 4;
            btn_validate.Text = "Validate";
            btn_validate.UseVisualStyleBackColor = true;
            btn_validate.Click += ActionEvent_TestBabstatConnection;
            // 
            // tb_webStatsServerPath
            // 
            tb_webStatsServerPath.Location = new Point(6, 22);
            tb_webStatsServerPath.Name = "tb_webStatsServerPath";
            tb_webStatsServerPath.PlaceholderText = "http(s)://youdomain.com/babstats_root/";
            tb_webStatsServerPath.Size = new Size(277, 23);
            tb_webStatsServerPath.TabIndex = 1;
            tb_webStatsServerPath.TextAlign = HorizontalAlignment.Center;
            // 
            // tabPlayerStats
            // 
            tabPlayerStats.Controls.Add(tableLayoutPanel2);
            tabPlayerStats.Location = new Point(4, 24);
            tabPlayerStats.Name = "tabPlayerStats";
            tabPlayerStats.Padding = new Padding(3);
            tabPlayerStats.Size = new Size(894, 334);
            tabPlayerStats.TabIndex = 1;
            tabPlayerStats.Text = "Player Stats";
            tabPlayerStats.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(dataGridViewPlayerStats, 0, 1);
            tableLayoutPanel2.Controls.Add(panel2, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 83F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(888, 328);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // dataGridViewPlayerStats
            // 
            dataGridViewPlayerStats.AllowUserToAddRows = false;
            dataGridViewPlayerStats.AllowUserToDeleteRows = false;
            dataGridViewPlayerStats.AllowUserToResizeColumns = false;
            dataGridViewPlayerStats.AllowUserToResizeRows = false;
            dataGridViewPlayerStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPlayerStats.Columns.AddRange(new DataGridViewColumn[] { PlayerName, Suicides, Murders, Kills, Deaths, ZoneTime, FBCaptures, FlagSaves, ADTargetsDestroyed, RevivesReceived, RevivesGiven, PSPAttempts, PSPTakeovers, FBCarrierKills, DoubleKills, Headshots, KnifeKills, SniperKills, TKOTHDefenseKills, TKOTHAttackKills, ShotsPerKill, ExperiencePoints, PlayerTeam, PlayerActive, TimePlayed });
            dataGridViewPlayerStats.Dock = DockStyle.Fill;
            dataGridViewPlayerStats.Location = new Point(0, 83);
            dataGridViewPlayerStats.Margin = new Padding(0);
            dataGridViewPlayerStats.Name = "dataGridViewPlayerStats";
            dataGridViewPlayerStats.ReadOnly = true;
            dataGridViewPlayerStats.RowHeadersVisible = false;
            dataGridViewPlayerStats.ScrollBars = ScrollBars.Vertical;
            dataGridViewPlayerStats.Size = new Size(888, 245);
            dataGridViewPlayerStats.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(888, 83);
            panel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(354, 34);
            label1.Name = "label1";
            label1.Size = new Size(181, 15);
            label1.TabIndex = 0;
            label1.Text = "Header Information To Be Added";
            // 
            // tabWeaponStats
            // 
            tabWeaponStats.Controls.Add(dataGridViewWeaponStats);
            tabWeaponStats.Location = new Point(4, 24);
            tabWeaponStats.Name = "tabWeaponStats";
            tabWeaponStats.Size = new Size(894, 334);
            tabWeaponStats.TabIndex = 2;
            tabWeaponStats.Text = "Weapon Stats";
            tabWeaponStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewWeaponStats
            // 
            dataGridViewWeaponStats.AllowUserToAddRows = false;
            dataGridViewWeaponStats.AllowUserToDeleteRows = false;
            dataGridViewWeaponStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWeaponStats.Columns.AddRange(new DataGridViewColumn[] { Weapon_PlayerName, WeaponName, Timer, Weapon_Kills, Weapon_Shots });
            dataGridViewWeaponStats.Dock = DockStyle.Fill;
            dataGridViewWeaponStats.Location = new Point(0, 0);
            dataGridViewWeaponStats.Name = "dataGridViewWeaponStats";
            dataGridViewWeaponStats.ReadOnly = true;
            dataGridViewWeaponStats.RowHeadersVisible = false;
            dataGridViewWeaponStats.Size = new Size(894, 334);
            dataGridViewWeaponStats.TabIndex = 1;
            // 
            // Weapon_PlayerName
            // 
            Weapon_PlayerName.HeaderText = "Player Name";
            Weapon_PlayerName.MinimumWidth = 200;
            Weapon_PlayerName.Name = "Weapon_PlayerName";
            Weapon_PlayerName.ReadOnly = true;
            Weapon_PlayerName.Width = 200;
            // 
            // WeaponName
            // 
            WeaponName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            WeaponName.HeaderText = "Weapon Name";
            WeaponName.Name = "WeaponName";
            WeaponName.ReadOnly = true;
            // 
            // Timer
            // 
            Timer.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Timer.HeaderText = "Time Used (s)";
            Timer.Name = "Timer";
            Timer.ReadOnly = true;
            // 
            // Weapon_Kills
            // 
            Weapon_Kills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Weapon_Kills.HeaderText = "Kills";
            Weapon_Kills.Name = "Weapon_Kills";
            Weapon_Kills.ReadOnly = true;
            // 
            // Weapon_Shots
            // 
            Weapon_Shots.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Weapon_Shots.HeaderText = "Shots";
            Weapon_Shots.Name = "Weapon_Shots";
            Weapon_Shots.ReadOnly = true;
            // 
            // PlayerName
            // 
            PlayerName.HeaderText = "Player Name";
            PlayerName.MinimumWidth = 200;
            PlayerName.Name = "PlayerName";
            PlayerName.ReadOnly = true;
            PlayerName.Width = 200;
            // 
            // Suicides
            // 
            Suicides.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Suicides.HeaderText = "";
            Suicides.Name = "Suicides";
            Suicides.ReadOnly = true;
            Suicides.ToolTipText = "Suicides";
            // 
            // Murders
            // 
            Murders.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Murders.HeaderText = "";
            Murders.Name = "Murders";
            Murders.ReadOnly = true;
            Murders.ToolTipText = "Murders";
            // 
            // Kills
            // 
            Kills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Kills.HeaderText = "";
            Kills.Name = "Kills";
            Kills.ReadOnly = true;
            Kills.ToolTipText = "Kills";
            // 
            // Deaths
            // 
            Deaths.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Deaths.HeaderText = "";
            Deaths.Name = "Deaths";
            Deaths.ReadOnly = true;
            Deaths.ToolTipText = "Deaths";
            // 
            // ZoneTime
            // 
            ZoneTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ZoneTime.HeaderText = "";
            ZoneTime.Name = "ZoneTime";
            ZoneTime.ReadOnly = true;
            ZoneTime.ToolTipText = "Zone Time";
            // 
            // FBCaptures
            // 
            FBCaptures.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FBCaptures.HeaderText = "";
            FBCaptures.Name = "FBCaptures";
            FBCaptures.ReadOnly = true;
            FBCaptures.ToolTipText = "FlagBall Captures";
            // 
            // FlagSaves
            // 
            FlagSaves.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FlagSaves.HeaderText = "";
            FlagSaves.Name = "FlagSaves";
            FlagSaves.ReadOnly = true;
            FlagSaves.ToolTipText = "Flag Saves";
            // 
            // ADTargetsDestroyed
            // 
            ADTargetsDestroyed.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ADTargetsDestroyed.HeaderText = "";
            ADTargetsDestroyed.Name = "ADTargetsDestroyed";
            ADTargetsDestroyed.ReadOnly = true;
            ADTargetsDestroyed.ToolTipText = "ADTargetsDestroyed";
            // 
            // RevivesReceived
            // 
            RevivesReceived.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RevivesReceived.HeaderText = "";
            RevivesReceived.Name = "RevivesReceived";
            RevivesReceived.ReadOnly = true;
            RevivesReceived.ToolTipText = "Revives Received";
            // 
            // RevivesGiven
            // 
            RevivesGiven.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RevivesGiven.HeaderText = "";
            RevivesGiven.Name = "RevivesGiven";
            RevivesGiven.ReadOnly = true;
            RevivesGiven.ToolTipText = "Revives Given";
            // 
            // PSPAttempts
            // 
            PSPAttempts.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PSPAttempts.HeaderText = "";
            PSPAttempts.Name = "PSPAttempts";
            PSPAttempts.ReadOnly = true;
            PSPAttempts.ToolTipText = "PSP Attempts";
            // 
            // PSPTakeovers
            // 
            PSPTakeovers.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PSPTakeovers.HeaderText = "";
            PSPTakeovers.Name = "PSPTakeovers";
            PSPTakeovers.ReadOnly = true;
            PSPTakeovers.ToolTipText = "PSP Takeovers";
            // 
            // FBCarrierKills
            // 
            FBCarrierKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            FBCarrierKills.HeaderText = "";
            FBCarrierKills.Name = "FBCarrierKills";
            FBCarrierKills.ReadOnly = true;
            FBCarrierKills.ToolTipText = "FB Carrier Kills";
            // 
            // DoubleKills
            // 
            DoubleKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DoubleKills.HeaderText = "";
            DoubleKills.Name = "DoubleKills";
            DoubleKills.ReadOnly = true;
            DoubleKills.ToolTipText = "Double Kills";
            // 
            // Headshots
            // 
            Headshots.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Headshots.HeaderText = "";
            Headshots.Name = "Headshots";
            Headshots.ReadOnly = true;
            Headshots.ToolTipText = "Headshots";
            // 
            // KnifeKills
            // 
            KnifeKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            KnifeKills.HeaderText = "";
            KnifeKills.Name = "KnifeKills";
            KnifeKills.ReadOnly = true;
            KnifeKills.ToolTipText = "Knife Kills";
            // 
            // SniperKills
            // 
            SniperKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SniperKills.HeaderText = "";
            SniperKills.Name = "SniperKills";
            SniperKills.ReadOnly = true;
            SniperKills.ToolTipText = "Sniper Kills";
            // 
            // TKOTHDefenseKills
            // 
            TKOTHDefenseKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TKOTHDefenseKills.HeaderText = "";
            TKOTHDefenseKills.Name = "TKOTHDefenseKills";
            TKOTHDefenseKills.ReadOnly = true;
            TKOTHDefenseKills.ToolTipText = "TKOTH Defense Kills";
            // 
            // TKOTHAttackKills
            // 
            TKOTHAttackKills.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TKOTHAttackKills.HeaderText = "";
            TKOTHAttackKills.Name = "TKOTHAttackKills";
            TKOTHAttackKills.ReadOnly = true;
            TKOTHAttackKills.ToolTipText = "TKOTH Attack Kills";
            // 
            // ShotsPerKill
            // 
            ShotsPerKill.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ShotsPerKill.HeaderText = "";
            ShotsPerKill.Name = "ShotsPerKill";
            ShotsPerKill.ReadOnly = true;
            ShotsPerKill.ToolTipText = "Shots Per Kill";
            // 
            // ExperiencePoints
            // 
            ExperiencePoints.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ExperiencePoints.HeaderText = "";
            ExperiencePoints.Name = "ExperiencePoints";
            ExperiencePoints.ReadOnly = true;
            ExperiencePoints.ToolTipText = "Experience Points";
            // 
            // PlayerTeam
            // 
            PlayerTeam.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PlayerTeam.HeaderText = "";
            PlayerTeam.Name = "PlayerTeam";
            PlayerTeam.ReadOnly = true;
            PlayerTeam.ToolTipText = "Player Team";
            // 
            // PlayerActive
            // 
            PlayerActive.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PlayerActive.HeaderText = "";
            PlayerActive.Name = "PlayerActive";
            PlayerActive.ReadOnly = true;
            PlayerActive.ToolTipText = "Active";
            // 
            // TimePlayed
            // 
            TimePlayed.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TimePlayed.HeaderText = "";
            TimePlayed.Name = "TimePlayed";
            TimePlayed.ReadOnly = true;
            TimePlayed.ToolTipText = "Time Played (s)";
            // 
            // tabStats
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabStats";
            Size = new Size(902, 362);
            tabControl1.ResumeLayout(false);
            tabBabstats.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_statsLog).EndInit();
            panel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPlayerStats.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabWeaponStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabBabstats;
        private TabPage tabPlayerStats;
        private TabPage tabWeaponStats;
        private TableLayoutPanel tableLayoutPanel1;
        public DataGridView dg_statsLog;
        private DataGridViewTextBoxColumn statsLog_DateTime;
        private DataGridViewTextBoxColumn statLog_Message;
        private Panel panel1;
        private GroupBox groupBox1;
        private TextBox tb_webStatsServerPath;
        public Button btn_validate;
        private GroupBox groupBox2;
        private CheckBox cb_enableWebStats;
        public CheckBox cb_enableAnnouncements;
        private GroupBox groupBox3;
        public NumericUpDown num_WebStatsReport;
        private Label labelReportInterval;
        private GroupBox groupBox4;
        private Label labelUpatedInterval;
        public NumericUpDown num_WebStatsUpdates;
        private Button btn_SaveSettings;
        private TableLayoutPanel tableLayoutPanel2;
        internal DataGridView dataGridViewPlayerStats;
        private Panel panel2;
        private Label label1;
        internal DataGridView dataGridViewWeaponStats;
        private DataGridViewTextBoxColumn Weapon_PlayerName;
        private DataGridViewTextBoxColumn WeaponName;
        private DataGridViewTextBoxColumn Timer;
        private DataGridViewTextBoxColumn Weapon_Kills;
        private DataGridViewTextBoxColumn Weapon_Shots;
        private DataGridViewTextBoxColumn PlayerName;
        private DataGridViewTextBoxColumn Suicides;
        private DataGridViewTextBoxColumn Murders;
        private DataGridViewTextBoxColumn Kills;
        private DataGridViewTextBoxColumn Deaths;
        private DataGridViewTextBoxColumn ZoneTime;
        private DataGridViewTextBoxColumn FBCaptures;
        private DataGridViewTextBoxColumn FlagSaves;
        private DataGridViewTextBoxColumn ADTargetsDestroyed;
        private DataGridViewTextBoxColumn RevivesReceived;
        private DataGridViewTextBoxColumn RevivesGiven;
        private DataGridViewTextBoxColumn PSPAttempts;
        private DataGridViewTextBoxColumn PSPTakeovers;
        private DataGridViewTextBoxColumn FBCarrierKills;
        private DataGridViewTextBoxColumn DoubleKills;
        private DataGridViewTextBoxColumn Headshots;
        private DataGridViewTextBoxColumn KnifeKills;
        private DataGridViewTextBoxColumn SniperKills;
        private DataGridViewTextBoxColumn TKOTHDefenseKills;
        private DataGridViewTextBoxColumn TKOTHAttackKills;
        private DataGridViewTextBoxColumn ShotsPerKill;
        private DataGridViewTextBoxColumn ExperiencePoints;
        private DataGridViewTextBoxColumn PlayerTeam;
        private DataGridViewTextBoxColumn PlayerActive;
        private DataGridViewTextBoxColumn TimePlayed;
    }
}
