namespace BHD_ServerManager.Forms.Panels
{
    partial class tabGamePlay
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btn_serverControl = new Button();
            btn_LockLobby = new Button();
            btn_LoadSettings = new Button();
            btn_ExportSettings = new Button();
            btn_ResetSettings = new Button();
            btn_SaveSettings = new Button();
            btn_ServerUpdate = new Button();
            panel1 = new Panel();
            groupBox3 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            num_maxTeamLives = new NumericUpDown();
            num_maxPlayers = new NumericUpDown();
            label2 = new Label();
            label_maxPlayers = new Label();
            num_flagReturnTime = new NumericUpDown();
            cb_autoRange = new CheckBox();
            label1 = new Label();
            cb_enableLeftLean = new CheckBox();
            num_pspTakeoverTimer = new NumericUpDown();
            num_scoreBoardDelay = new NumericUpDown();
            label_pspTakeover = new Label();
            label_scoreDelay = new Label();
            cb_autoBalance = new CheckBox();
            cb_enableOneShotKills = new CheckBox();
            num_gameStartDelay = new NumericUpDown();
            label_startDelay = new Label();
            num_respawnTime = new NumericUpDown();
            label_respawnTime = new Label();
            cb_replayMaps = new ComboBox();
            label_replayMaps = new Label();
            cb_showClays = new CheckBox();
            cb_enableFatBullets = new CheckBox();
            cb_enableDistroyBuildings = new CheckBox();
            cb_customSkins = new CheckBox();
            cb_showTracers = new CheckBox();
            label_timeLimit = new Label();
            num_gameTimeLimit = new NumericUpDown();
            groupBox2 = new GroupBox();
            scoresTableLayout = new TableLayoutPanel();
            num_scoresFB = new NumericUpDown();
            num_scoresDM = new NumericUpDown();
            label_flagball = new Label();
            num_scoresKOTH = new NumericUpDown();
            label_koth = new Label();
            label_dm = new Label();
            groupBox_lobbyPasswords = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            tb_redPassword = new TextBox();
            tb_bluePassword = new TextBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            checkBox_selectNone = new CheckBox();
            checkBox_selectAll = new CheckBox();
            cb_weapColt45 = new CheckBox();
            cb_weapMP5 = new CheckBox();
            cb_weapM9Bereatta = new CheckBox();
            cb_weapShotgun = new CheckBox();
            cb_weapM240 = new CheckBox();
            cb_weapFragGrenade = new CheckBox();
            cb_weapM60 = new CheckBox();
            cb_weapG36 = new CheckBox();
            cb_weapSmokeGrenade = new CheckBox();
            cb_weapG3 = new CheckBox();
            cb_weapM16203 = new CheckBox();
            cb_weapFlashBang = new CheckBox();
            cb_weapM16 = new CheckBox();
            cb_weapAT4 = new CheckBox();
            cb_weapCAR15203 = new CheckBox();
            cb_weapClay = new CheckBox();
            cb_weapCAR15 = new CheckBox();
            cb_weapSatchel = new CheckBox();
            cb_weap300Tact = new CheckBox();
            cb_weapBarret = new CheckBox();
            cb_weapPSG1 = new CheckBox();
            cb_weapM21 = new CheckBox();
            cb_weapM24 = new CheckBox();
            cb_weapSaw = new CheckBox();
            groupBox1 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            cb_enableFFkills = new CheckBox();
            cb_showTeamTags = new CheckBox();
            cb_warnFFkils = new CheckBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            label_maxFFkills = new Label();
            num_maxFFKills = new NumericUpDown();
            groupBox5 = new GroupBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            cb_roleCQB = new CheckBox();
            cb_roleGunner = new CheckBox();
            cb_roleMedic = new CheckBox();
            cb_roleSniper = new CheckBox();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxTeamLives).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_maxPlayers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_flagReturnTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_pspTakeoverTimer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoreBoardDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_gameStartDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_respawnTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_gameTimeLimit).BeginInit();
            groupBox2.SuspendLayout();
            scoresTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_scoresFB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresDM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresKOTH).BeginInit();
            groupBox_lobbyPasswords.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxFFKills).BeginInit();
            groupBox5.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.8102379F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.1897621F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox4, 1, 0);
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
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(btn_serverControl, 0, 0);
            tableLayoutPanel2.Controls.Add(btn_LockLobby, 0, 1);
            tableLayoutPanel2.Controls.Add(btn_LoadSettings, 0, 2);
            tableLayoutPanel2.Controls.Add(btn_ExportSettings, 0, 3);
            tableLayoutPanel2.Controls.Add(btn_ResetSettings, 0, 4);
            tableLayoutPanel2.Controls.Add(btn_SaveSettings, 0, 5);
            tableLayoutPanel2.Controls.Add(btn_ServerUpdate, 0, 6);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(868, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857113F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857151F));
            tableLayoutPanel2.Size = new Size(95, 416);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_serverControl
            // 
            btn_serverControl.Dock = DockStyle.Fill;
            btn_serverControl.Font = new Font("Segoe UI", 12F);
            btn_serverControl.Location = new Point(3, 3);
            btn_serverControl.Name = "btn_serverControl";
            btn_serverControl.Size = new Size(89, 53);
            btn_serverControl.TabIndex = 0;
            btn_serverControl.Text = "START";
            toolTip1.SetToolTip(btn_serverControl, "Start/Stop Server");
            btn_serverControl.UseVisualStyleBackColor = true;
            btn_serverControl.Click += actionClick_serverControl;
            // 
            // btn_LockLobby
            // 
            btn_LockLobby.Dock = DockStyle.Fill;
            btn_LockLobby.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_LockLobby.Location = new Point(3, 62);
            btn_LockLobby.Name = "btn_LockLobby";
            btn_LockLobby.Size = new Size(89, 53);
            btn_LockLobby.TabIndex = 1;
            btn_LockLobby.Text = "UN/LOCK LOBBY";
            toolTip1.SetToolTip(btn_LockLobby, "Lock Server from Entry");
            btn_LockLobby.UseVisualStyleBackColor = true;
            btn_LockLobby.Visible = false;
            btn_LockLobby.Click += actionClick_ServerLockLobby;
            // 
            // btn_LoadSettings
            // 
            btn_LoadSettings.Dock = DockStyle.Fill;
            btn_LoadSettings.Font = new Font("Segoe UI", 12F);
            btn_LoadSettings.Location = new Point(3, 121);
            btn_LoadSettings.Name = "btn_LoadSettings";
            btn_LoadSettings.Size = new Size(89, 53);
            btn_LoadSettings.TabIndex = 2;
            btn_LoadSettings.Text = "LOAD";
            toolTip1.SetToolTip(btn_LoadSettings, "Load Settings from File");
            btn_LoadSettings.UseVisualStyleBackColor = true;
            // 
            // btn_ExportSettings
            // 
            btn_ExportSettings.Dock = DockStyle.Fill;
            btn_ExportSettings.Font = new Font("Segoe UI", 12F);
            btn_ExportSettings.Location = new Point(3, 180);
            btn_ExportSettings.Name = "btn_ExportSettings";
            btn_ExportSettings.Size = new Size(89, 53);
            btn_ExportSettings.TabIndex = 3;
            btn_ExportSettings.Text = "EXPORT";
            toolTip1.SetToolTip(btn_ExportSettings, "Export Settings to File");
            btn_ExportSettings.UseVisualStyleBackColor = true;
            // 
            // btn_ResetSettings
            // 
            btn_ResetSettings.Dock = DockStyle.Fill;
            btn_ResetSettings.Font = new Font("Segoe UI", 12F);
            btn_ResetSettings.Location = new Point(3, 239);
            btn_ResetSettings.Name = "btn_ResetSettings";
            btn_ResetSettings.Size = new Size(89, 53);
            btn_ResetSettings.TabIndex = 4;
            btn_ResetSettings.Text = "RESET";
            toolTip1.SetToolTip(btn_ResetSettings, "Reset Changes to Settings");
            btn_ResetSettings.UseVisualStyleBackColor = true;
            btn_ResetSettings.Click += actionClick_ResetSettings;
            // 
            // btn_SaveSettings
            // 
            btn_SaveSettings.Dock = DockStyle.Fill;
            btn_SaveSettings.Font = new Font("Segoe UI", 12F);
            btn_SaveSettings.Location = new Point(3, 298);
            btn_SaveSettings.Name = "btn_SaveSettings";
            btn_SaveSettings.Size = new Size(89, 53);
            btn_SaveSettings.TabIndex = 5;
            btn_SaveSettings.Text = "SAVE";
            toolTip1.SetToolTip(btn_SaveSettings, "Save Settings to Instance and File");
            btn_SaveSettings.UseVisualStyleBackColor = true;
            btn_SaveSettings.Click += actionClick_SaveServerSettings;
            // 
            // btn_ServerUpdate
            // 
            btn_ServerUpdate.Dock = DockStyle.Fill;
            btn_ServerUpdate.Font = new Font("Segoe UI", 12F);
            btn_ServerUpdate.Location = new Point(3, 357);
            btn_ServerUpdate.Name = "btn_ServerUpdate";
            btn_ServerUpdate.Size = new Size(89, 56);
            btn_ServerUpdate.TabIndex = 6;
            btn_ServerUpdate.Text = "UPDATE";
            toolTip1.SetToolTip(btn_ServerUpdate, "Update Game Server using Saved Settings");
            btn_ServerUpdate.UseVisualStyleBackColor = true;
            btn_ServerUpdate.Visible = false;
            btn_ServerUpdate.Click += actionClick_GameServerUpdate;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox_lobbyPasswords);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(442, 416);
            panel1.TabIndex = 1;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel4);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(0, 122);
            groupBox3.Margin = new Padding(0);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(0);
            groupBox3.Size = new Size(442, 294);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Game Play Options";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 97F));
            tableLayoutPanel4.Controls.Add(num_maxTeamLives, 2, 8);
            tableLayoutPanel4.Controls.Add(num_maxPlayers, 2, 5);
            tableLayoutPanel4.Controls.Add(label2, 1, 8);
            tableLayoutPanel4.Controls.Add(label_maxPlayers, 1, 5);
            tableLayoutPanel4.Controls.Add(num_flagReturnTime, 2, 7);
            tableLayoutPanel4.Controls.Add(cb_autoRange, 0, 8);
            tableLayoutPanel4.Controls.Add(label1, 1, 7);
            tableLayoutPanel4.Controls.Add(cb_enableLeftLean, 0, 5);
            tableLayoutPanel4.Controls.Add(num_pspTakeoverTimer, 2, 6);
            tableLayoutPanel4.Controls.Add(num_scoreBoardDelay, 2, 4);
            tableLayoutPanel4.Controls.Add(label_pspTakeover, 1, 6);
            tableLayoutPanel4.Controls.Add(label_scoreDelay, 1, 4);
            tableLayoutPanel4.Controls.Add(cb_autoBalance, 0, 0);
            tableLayoutPanel4.Controls.Add(cb_enableOneShotKills, 0, 4);
            tableLayoutPanel4.Controls.Add(num_gameStartDelay, 2, 3);
            tableLayoutPanel4.Controls.Add(label_startDelay, 1, 3);
            tableLayoutPanel4.Controls.Add(num_respawnTime, 2, 2);
            tableLayoutPanel4.Controls.Add(label_respawnTime, 1, 2);
            tableLayoutPanel4.Controls.Add(cb_replayMaps, 2, 1);
            tableLayoutPanel4.Controls.Add(label_replayMaps, 1, 1);
            tableLayoutPanel4.Controls.Add(cb_showClays, 0, 7);
            tableLayoutPanel4.Controls.Add(cb_enableFatBullets, 0, 3);
            tableLayoutPanel4.Controls.Add(cb_enableDistroyBuildings, 0, 2);
            tableLayoutPanel4.Controls.Add(cb_customSkins, 0, 1);
            tableLayoutPanel4.Controls.Add(cb_showTracers, 0, 6);
            tableLayoutPanel4.Controls.Add(label_timeLimit, 1, 0);
            tableLayoutPanel4.Controls.Add(num_gameTimeLimit, 2, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 16);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(10, 0, 10, 0);
            tableLayoutPanel4.RowCount = 9;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.3402061F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 11.320755F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 9.433962F));
            tableLayoutPanel4.Size = new Size(442, 278);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // num_maxTeamLives
            // 
            num_maxTeamLives.Dock = DockStyle.Fill;
            num_maxTeamLives.Font = new Font("Segoe UI", 8F);
            num_maxTeamLives.Location = new Point(337, 254);
            num_maxTeamLives.Margin = new Padding(3, 6, 3, 3);
            num_maxTeamLives.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            num_maxTeamLives.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_maxTeamLives.Name = "num_maxTeamLives";
            num_maxTeamLives.Size = new Size(92, 22);
            num_maxTeamLives.TabIndex = 5;
            num_maxTeamLives.TextAlign = HorizontalAlignment.Center;
            num_maxTeamLives.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // num_maxPlayers
            // 
            num_maxPlayers.Dock = DockStyle.Fill;
            num_maxPlayers.Location = new Point(337, 161);
            num_maxPlayers.Margin = new Padding(3, 6, 3, 3);
            num_maxPlayers.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            num_maxPlayers.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            num_maxPlayers.Name = "num_maxPlayers";
            num_maxPlayers.Size = new Size(92, 23);
            num_maxPlayers.TabIndex = 10;
            num_maxPlayers.TextAlign = HorizontalAlignment.Center;
            num_maxPlayers.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(172, 248);
            label2.Margin = new Padding(0, 0, 10, 0);
            label2.Name = "label2";
            label2.Size = new Size(152, 30);
            label2.TabIndex = 4;
            label2.Text = "Max Team Lives";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_maxPlayers
            // 
            label_maxPlayers.AutoSize = true;
            label_maxPlayers.Dock = DockStyle.Fill;
            label_maxPlayers.Location = new Point(172, 155);
            label_maxPlayers.Margin = new Padding(0, 0, 10, 0);
            label_maxPlayers.Name = "label_maxPlayers";
            label_maxPlayers.Size = new Size(152, 31);
            label_maxPlayers.TabIndex = 11;
            label_maxPlayers.Text = "Max Players";
            label_maxPlayers.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_flagReturnTime
            // 
            num_flagReturnTime.Dock = DockStyle.Fill;
            num_flagReturnTime.Font = new Font("Segoe UI", 8F);
            num_flagReturnTime.Location = new Point(337, 223);
            num_flagReturnTime.Margin = new Padding(3, 6, 3, 3);
            num_flagReturnTime.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            num_flagReturnTime.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_flagReturnTime.Name = "num_flagReturnTime";
            num_flagReturnTime.Size = new Size(92, 22);
            num_flagReturnTime.TabIndex = 3;
            num_flagReturnTime.TextAlign = HorizontalAlignment.Center;
            num_flagReturnTime.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // cb_autoRange
            // 
            cb_autoRange.AutoSize = true;
            cb_autoRange.Checked = true;
            cb_autoRange.CheckState = CheckState.Checked;
            cb_autoRange.Dock = DockStyle.Fill;
            cb_autoRange.Location = new Point(15, 248);
            cb_autoRange.Margin = new Padding(5, 0, 0, 0);
            cb_autoRange.Name = "cb_autoRange";
            cb_autoRange.Size = new Size(157, 30);
            cb_autoRange.TabIndex = 3;
            cb_autoRange.Text = "Allow Auto Range";
            cb_autoRange.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(172, 217);
            label1.Margin = new Padding(0, 0, 10, 0);
            label1.Name = "label1";
            label1.Size = new Size(152, 31);
            label1.TabIndex = 2;
            label1.Text = "Flag Return Time";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cb_enableLeftLean
            // 
            cb_enableLeftLean.AutoSize = true;
            cb_enableLeftLean.Checked = true;
            cb_enableLeftLean.CheckState = CheckState.Checked;
            cb_enableLeftLean.Dock = DockStyle.Fill;
            cb_enableLeftLean.Enabled = false;
            cb_enableLeftLean.Location = new Point(15, 155);
            cb_enableLeftLean.Margin = new Padding(5, 0, 0, 0);
            cb_enableLeftLean.Name = "cb_enableLeftLean";
            cb_enableLeftLean.Size = new Size(157, 31);
            cb_enableLeftLean.TabIndex = 3;
            cb_enableLeftLean.Text = "Allow Left Leaning";
            cb_enableLeftLean.UseVisualStyleBackColor = true;
            // 
            // num_pspTakeoverTimer
            // 
            num_pspTakeoverTimer.Dock = DockStyle.Fill;
            num_pspTakeoverTimer.Font = new Font("Segoe UI", 8F);
            num_pspTakeoverTimer.Location = new Point(337, 192);
            num_pspTakeoverTimer.Margin = new Padding(3, 6, 3, 3);
            num_pspTakeoverTimer.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_pspTakeoverTimer.Name = "num_pspTakeoverTimer";
            num_pspTakeoverTimer.Size = new Size(92, 22);
            num_pspTakeoverTimer.TabIndex = 1;
            num_pspTakeoverTimer.TextAlign = HorizontalAlignment.Center;
            num_pspTakeoverTimer.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // num_scoreBoardDelay
            // 
            num_scoreBoardDelay.Dock = DockStyle.Fill;
            num_scoreBoardDelay.Location = new Point(337, 130);
            num_scoreBoardDelay.Margin = new Padding(3, 6, 3, 3);
            num_scoreBoardDelay.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            num_scoreBoardDelay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoreBoardDelay.Name = "num_scoreBoardDelay";
            num_scoreBoardDelay.Size = new Size(92, 23);
            num_scoreBoardDelay.TabIndex = 8;
            num_scoreBoardDelay.TextAlign = HorizontalAlignment.Center;
            num_scoreBoardDelay.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label_pspTakeover
            // 
            label_pspTakeover.AutoSize = true;
            label_pspTakeover.Dock = DockStyle.Fill;
            label_pspTakeover.Location = new Point(172, 186);
            label_pspTakeover.Margin = new Padding(0, 0, 10, 0);
            label_pspTakeover.Name = "label_pspTakeover";
            label_pspTakeover.Size = new Size(152, 31);
            label_pspTakeover.TabIndex = 0;
            label_pspTakeover.Text = "PSP Takeover Time";
            label_pspTakeover.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_scoreDelay
            // 
            label_scoreDelay.AutoSize = true;
            label_scoreDelay.Dock = DockStyle.Fill;
            label_scoreDelay.Location = new Point(172, 124);
            label_scoreDelay.Margin = new Padding(0, 0, 10, 0);
            label_scoreDelay.Name = "label_scoreDelay";
            label_scoreDelay.Size = new Size(152, 31);
            label_scoreDelay.TabIndex = 9;
            label_scoreDelay.Text = "Score Board Delay";
            label_scoreDelay.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cb_autoBalance
            // 
            cb_autoBalance.AutoSize = true;
            cb_autoBalance.Checked = true;
            cb_autoBalance.CheckState = CheckState.Checked;
            cb_autoBalance.Dock = DockStyle.Fill;
            cb_autoBalance.Font = new Font("Segoe UI", 8F);
            cb_autoBalance.Location = new Point(15, 0);
            cb_autoBalance.Margin = new Padding(5, 0, 0, 0);
            cb_autoBalance.Name = "cb_autoBalance";
            cb_autoBalance.Size = new Size(157, 31);
            cb_autoBalance.TabIndex = 3;
            cb_autoBalance.Text = "Auto Balance Players";
            cb_autoBalance.UseVisualStyleBackColor = true;
            // 
            // cb_enableOneShotKills
            // 
            cb_enableOneShotKills.AutoSize = true;
            cb_enableOneShotKills.Dock = DockStyle.Fill;
            cb_enableOneShotKills.Location = new Point(15, 124);
            cb_enableOneShotKills.Margin = new Padding(5, 0, 0, 0);
            cb_enableOneShotKills.Name = "cb_enableOneShotKills";
            cb_enableOneShotKills.Size = new Size(157, 31);
            cb_enableOneShotKills.TabIndex = 0;
            cb_enableOneShotKills.Text = "One Shot Kills";
            cb_enableOneShotKills.UseVisualStyleBackColor = true;
            // 
            // num_gameStartDelay
            // 
            num_gameStartDelay.Dock = DockStyle.Fill;
            num_gameStartDelay.Location = new Point(337, 99);
            num_gameStartDelay.Margin = new Padding(3, 6, 3, 3);
            num_gameStartDelay.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            num_gameStartDelay.Name = "num_gameStartDelay";
            num_gameStartDelay.Size = new Size(92, 23);
            num_gameStartDelay.TabIndex = 1;
            num_gameStartDelay.TextAlign = HorizontalAlignment.Center;
            num_gameStartDelay.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label_startDelay
            // 
            label_startDelay.AutoSize = true;
            label_startDelay.Dock = DockStyle.Fill;
            label_startDelay.Location = new Point(172, 93);
            label_startDelay.Margin = new Padding(0, 0, 10, 0);
            label_startDelay.Name = "label_startDelay";
            label_startDelay.Size = new Size(152, 31);
            label_startDelay.TabIndex = 3;
            label_startDelay.Text = "Start Delay";
            label_startDelay.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_respawnTime
            // 
            num_respawnTime.Dock = DockStyle.Fill;
            num_respawnTime.Location = new Point(337, 68);
            num_respawnTime.Margin = new Padding(3, 6, 3, 3);
            num_respawnTime.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            num_respawnTime.Name = "num_respawnTime";
            num_respawnTime.Size = new Size(92, 23);
            num_respawnTime.TabIndex = 4;
            num_respawnTime.TextAlign = HorizontalAlignment.Center;
            num_respawnTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label_respawnTime
            // 
            label_respawnTime.AutoSize = true;
            label_respawnTime.Dock = DockStyle.Fill;
            label_respawnTime.Location = new Point(172, 62);
            label_respawnTime.Margin = new Padding(0, 0, 10, 0);
            label_respawnTime.Name = "label_respawnTime";
            label_respawnTime.Size = new Size(152, 31);
            label_respawnTime.TabIndex = 5;
            label_respawnTime.Text = "Respawn Time";
            label_respawnTime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cb_replayMaps
            // 
            cb_replayMaps.DisplayMember = "Yes";
            cb_replayMaps.Dock = DockStyle.Fill;
            cb_replayMaps.FormattingEnabled = true;
            cb_replayMaps.Items.AddRange(new object[] { "No", "Yes", "Cycle" });
            cb_replayMaps.Location = new Point(337, 37);
            cb_replayMaps.Margin = new Padding(3, 6, 3, 3);
            cb_replayMaps.Name = "cb_replayMaps";
            cb_replayMaps.Size = new Size(92, 23);
            cb_replayMaps.TabIndex = 7;
            cb_replayMaps.Text = "Cycle";
            // 
            // label_replayMaps
            // 
            label_replayMaps.AutoSize = true;
            label_replayMaps.Dock = DockStyle.Fill;
            label_replayMaps.Location = new Point(172, 31);
            label_replayMaps.Margin = new Padding(0, 0, 10, 0);
            label_replayMaps.Name = "label_replayMaps";
            label_replayMaps.Size = new Size(152, 31);
            label_replayMaps.TabIndex = 6;
            label_replayMaps.Text = "Replay Maps";
            label_replayMaps.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cb_showClays
            // 
            cb_showClays.AutoSize = true;
            cb_showClays.Dock = DockStyle.Fill;
            cb_showClays.Location = new Point(15, 217);
            cb_showClays.Margin = new Padding(5, 0, 0, 0);
            cb_showClays.Name = "cb_showClays";
            cb_showClays.Size = new Size(157, 31);
            cb_showClays.TabIndex = 3;
            cb_showClays.Text = "Show Team Clays";
            cb_showClays.UseVisualStyleBackColor = true;
            // 
            // cb_enableFatBullets
            // 
            cb_enableFatBullets.AutoSize = true;
            cb_enableFatBullets.Dock = DockStyle.Fill;
            cb_enableFatBullets.Location = new Point(15, 93);
            cb_enableFatBullets.Margin = new Padding(5, 0, 0, 0);
            cb_enableFatBullets.Name = "cb_enableFatBullets";
            cb_enableFatBullets.Size = new Size(157, 31);
            cb_enableFatBullets.TabIndex = 1;
            cb_enableFatBullets.Text = "Fat Bullets";
            cb_enableFatBullets.UseVisualStyleBackColor = true;
            // 
            // cb_enableDistroyBuildings
            // 
            cb_enableDistroyBuildings.AutoSize = true;
            cb_enableDistroyBuildings.Checked = true;
            cb_enableDistroyBuildings.CheckState = CheckState.Checked;
            cb_enableDistroyBuildings.Dock = DockStyle.Fill;
            cb_enableDistroyBuildings.Location = new Point(15, 62);
            cb_enableDistroyBuildings.Margin = new Padding(5, 0, 0, 0);
            cb_enableDistroyBuildings.Name = "cb_enableDistroyBuildings";
            cb_enableDistroyBuildings.Size = new Size(157, 31);
            cb_enableDistroyBuildings.TabIndex = 2;
            cb_enableDistroyBuildings.Text = "Destroy Buildings";
            cb_enableDistroyBuildings.UseVisualStyleBackColor = true;
            // 
            // cb_customSkins
            // 
            cb_customSkins.AutoSize = true;
            cb_customSkins.Checked = true;
            cb_customSkins.CheckState = CheckState.Checked;
            cb_customSkins.Dock = DockStyle.Fill;
            cb_customSkins.Location = new Point(15, 31);
            cb_customSkins.Margin = new Padding(5, 0, 0, 0);
            cb_customSkins.Name = "cb_customSkins";
            cb_customSkins.Size = new Size(157, 31);
            cb_customSkins.TabIndex = 2;
            cb_customSkins.Text = "Allow Custom Skins";
            cb_customSkins.UseVisualStyleBackColor = true;
            // 
            // cb_showTracers
            // 
            cb_showTracers.AutoSize = true;
            cb_showTracers.Dock = DockStyle.Fill;
            cb_showTracers.Location = new Point(15, 186);
            cb_showTracers.Margin = new Padding(5, 0, 0, 0);
            cb_showTracers.Name = "cb_showTracers";
            cb_showTracers.Size = new Size(157, 31);
            cb_showTracers.TabIndex = 3;
            cb_showTracers.Text = "Show Tracers";
            cb_showTracers.UseVisualStyleBackColor = true;
            // 
            // label_timeLimit
            // 
            label_timeLimit.AutoSize = true;
            label_timeLimit.Dock = DockStyle.Fill;
            label_timeLimit.Location = new Point(172, 0);
            label_timeLimit.Margin = new Padding(0, 0, 10, 0);
            label_timeLimit.Name = "label_timeLimit";
            label_timeLimit.Size = new Size(152, 31);
            label_timeLimit.TabIndex = 2;
            label_timeLimit.Text = "Match Time Limit";
            label_timeLimit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_gameTimeLimit
            // 
            num_gameTimeLimit.Dock = DockStyle.Fill;
            num_gameTimeLimit.Location = new Point(337, 6);
            num_gameTimeLimit.Margin = new Padding(3, 6, 3, 3);
            num_gameTimeLimit.Name = "num_gameTimeLimit";
            num_gameTimeLimit.Size = new Size(92, 23);
            num_gameTimeLimit.TabIndex = 0;
            num_gameTimeLimit.TextAlign = HorizontalAlignment.Center;
            num_gameTimeLimit.Value = new decimal(new int[] { 21, 0, 0, 0 });
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(scoresTableLayout);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 51);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(442, 71);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Match Win Conditions";
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
            scoresTableLayout.Size = new Size(436, 49);
            scoresTableLayout.TabIndex = 0;
            // 
            // num_scoresFB
            // 
            num_scoresFB.Dock = DockStyle.Fill;
            num_scoresFB.Location = new Point(293, 22);
            num_scoresFB.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            num_scoresFB.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoresFB.Name = "num_scoresFB";
            num_scoresFB.Size = new Size(140, 23);
            num_scoresFB.TabIndex = 2;
            num_scoresFB.TextAlign = HorizontalAlignment.Center;
            num_scoresFB.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // num_scoresDM
            // 
            num_scoresDM.Dock = DockStyle.Fill;
            num_scoresDM.Location = new Point(148, 22);
            num_scoresDM.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            num_scoresDM.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_scoresDM.Name = "num_scoresDM";
            num_scoresDM.Size = new Size(139, 23);
            num_scoresDM.TabIndex = 1;
            num_scoresDM.TextAlign = HorizontalAlignment.Center;
            num_scoresDM.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // label_flagball
            // 
            label_flagball.AutoSize = true;
            label_flagball.Dock = DockStyle.Fill;
            label_flagball.Location = new Point(293, 0);
            label_flagball.Name = "label_flagball";
            label_flagball.Size = new Size(140, 19);
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
            num_scoresKOTH.Size = new Size(139, 23);
            num_scoresKOTH.TabIndex = 0;
            num_scoresKOTH.TextAlign = HorizontalAlignment.Center;
            num_scoresKOTH.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // label_koth
            // 
            label_koth.AutoSize = true;
            label_koth.Dock = DockStyle.Fill;
            label_koth.Location = new Point(3, 0);
            label_koth.Name = "label_koth";
            label_koth.Size = new Size(139, 19);
            label_koth.TabIndex = 3;
            label_koth.Text = "TKOTH + KOTH";
            label_koth.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_dm
            // 
            label_dm.AutoSize = true;
            label_dm.Dock = DockStyle.Fill;
            label_dm.Location = new Point(148, 0);
            label_dm.Name = "label_dm";
            label_dm.Size = new Size(139, 19);
            label_dm.TabIndex = 4;
            label_dm.Text = "TDM + DM";
            label_dm.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox_lobbyPasswords
            // 
            groupBox_lobbyPasswords.Controls.Add(tableLayoutPanel3);
            groupBox_lobbyPasswords.Dock = DockStyle.Top;
            groupBox_lobbyPasswords.Location = new Point(0, 0);
            groupBox_lobbyPasswords.Name = "groupBox_lobbyPasswords";
            groupBox_lobbyPasswords.Size = new Size(442, 51);
            groupBox_lobbyPasswords.TabIndex = 4;
            groupBox_lobbyPasswords.TabStop = false;
            groupBox_lobbyPasswords.Text = "Lobby Passwords";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(tb_redPassword, 1, 0);
            tableLayoutPanel3.Controls.Add(tb_bluePassword, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(436, 29);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // tb_redPassword
            // 
            tb_redPassword.Dock = DockStyle.Fill;
            tb_redPassword.Location = new Point(221, 3);
            tb_redPassword.MaxLength = 16;
            tb_redPassword.Name = "tb_redPassword";
            tb_redPassword.PlaceholderText = "Red Team Password";
            tb_redPassword.Size = new Size(212, 23);
            tb_redPassword.TabIndex = 5;
            tb_redPassword.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_bluePassword
            // 
            tb_bluePassword.Dock = DockStyle.Fill;
            tb_bluePassword.Location = new Point(3, 3);
            tb_bluePassword.MaxLength = 16;
            tb_bluePassword.Name = "tb_bluePassword";
            tb_bluePassword.PlaceholderText = "Blue Team Password";
            tb_bluePassword.Size = new Size(212, 23);
            tb_bluePassword.TabIndex = 4;
            tb_bluePassword.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel5);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(451, 3);
            groupBox4.Margin = new Padding(3, 3, 3, 0);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(0);
            groupBox4.Size = new Size(411, 419);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Restrictions";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel5.Controls.Add(checkBox_selectNone, 2, 1);
            tableLayoutPanel5.Controls.Add(checkBox_selectAll, 2, 0);
            tableLayoutPanel5.Controls.Add(cb_weapColt45, 0, 0);
            tableLayoutPanel5.Controls.Add(cb_weapMP5, 1, 8);
            tableLayoutPanel5.Controls.Add(cb_weapM9Bereatta, 0, 1);
            tableLayoutPanel5.Controls.Add(cb_weapShotgun, 0, 2);
            tableLayoutPanel5.Controls.Add(cb_weapM240, 1, 7);
            tableLayoutPanel5.Controls.Add(cb_weapFragGrenade, 0, 3);
            tableLayoutPanel5.Controls.Add(cb_weapM60, 1, 6);
            tableLayoutPanel5.Controls.Add(cb_weapG36, 1, 5);
            tableLayoutPanel5.Controls.Add(cb_weapSmokeGrenade, 0, 4);
            tableLayoutPanel5.Controls.Add(cb_weapG3, 1, 4);
            tableLayoutPanel5.Controls.Add(cb_weapM16203, 1, 3);
            tableLayoutPanel5.Controls.Add(cb_weapFlashBang, 0, 5);
            tableLayoutPanel5.Controls.Add(cb_weapM16, 1, 2);
            tableLayoutPanel5.Controls.Add(cb_weapAT4, 0, 8);
            tableLayoutPanel5.Controls.Add(cb_weapCAR15203, 1, 1);
            tableLayoutPanel5.Controls.Add(cb_weapClay, 0, 6);
            tableLayoutPanel5.Controls.Add(cb_weapCAR15, 1, 0);
            tableLayoutPanel5.Controls.Add(cb_weapSatchel, 0, 7);
            tableLayoutPanel5.Controls.Add(cb_weap300Tact, 2, 4);
            tableLayoutPanel5.Controls.Add(cb_weapBarret, 2, 5);
            tableLayoutPanel5.Controls.Add(cb_weapPSG1, 2, 6);
            tableLayoutPanel5.Controls.Add(cb_weapM21, 2, 7);
            tableLayoutPanel5.Controls.Add(cb_weapM24, 2, 8);
            tableLayoutPanel5.Controls.Add(cb_weapSaw, 2, 3);
            tableLayoutPanel5.Controls.Add(groupBox1, 0, 10);
            tableLayoutPanel5.Controls.Add(groupBox5, 2, 10);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 16);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 14;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857F));
            tableLayoutPanel5.Size = new Size(411, 403);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // checkBox_selectNone
            // 
            checkBox_selectNone.AutoSize = true;
            checkBox_selectNone.Dock = DockStyle.Fill;
            checkBox_selectNone.Location = new Point(283, 28);
            checkBox_selectNone.Margin = new Padding(10, 0, 0, 0);
            checkBox_selectNone.Name = "checkBox_selectNone";
            checkBox_selectNone.Size = new Size(128, 28);
            checkBox_selectNone.TabIndex = 29;
            checkBox_selectNone.Text = "Select None";
            checkBox_selectNone.UseVisualStyleBackColor = true;
            checkBox_selectNone.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // checkBox_selectAll
            // 
            checkBox_selectAll.AutoSize = true;
            checkBox_selectAll.Dock = DockStyle.Fill;
            checkBox_selectAll.Location = new Point(283, 0);
            checkBox_selectAll.Margin = new Padding(10, 0, 0, 0);
            checkBox_selectAll.Name = "checkBox_selectAll";
            checkBox_selectAll.Size = new Size(128, 28);
            checkBox_selectAll.TabIndex = 28;
            checkBox_selectAll.Text = "Select All";
            checkBox_selectAll.UseVisualStyleBackColor = true;
            checkBox_selectAll.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapColt45
            // 
            cb_weapColt45.AutoSize = true;
            cb_weapColt45.Dock = DockStyle.Fill;
            cb_weapColt45.Location = new Point(10, 0);
            cb_weapColt45.Margin = new Padding(10, 0, 0, 0);
            cb_weapColt45.Name = "cb_weapColt45";
            cb_weapColt45.Size = new Size(126, 28);
            cb_weapColt45.TabIndex = 30;
            cb_weapColt45.Text = "Colt .45";
            cb_weapColt45.UseVisualStyleBackColor = true;
            cb_weapColt45.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapMP5
            // 
            cb_weapMP5.AutoSize = true;
            cb_weapMP5.Dock = DockStyle.Fill;
            cb_weapMP5.Location = new Point(146, 224);
            cb_weapMP5.Margin = new Padding(10, 0, 0, 0);
            cb_weapMP5.Name = "cb_weapMP5";
            cb_weapMP5.Size = new Size(127, 28);
            cb_weapMP5.TabIndex = 46;
            cb_weapMP5.Text = "MP5";
            cb_weapMP5.UseVisualStyleBackColor = true;
            cb_weapMP5.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM9Bereatta
            // 
            cb_weapM9Bereatta.AutoSize = true;
            cb_weapM9Bereatta.Dock = DockStyle.Fill;
            cb_weapM9Bereatta.Location = new Point(10, 28);
            cb_weapM9Bereatta.Margin = new Padding(10, 0, 0, 0);
            cb_weapM9Bereatta.Name = "cb_weapM9Bereatta";
            cb_weapM9Bereatta.Size = new Size(126, 28);
            cb_weapM9Bereatta.TabIndex = 31;
            cb_weapM9Bereatta.Text = "M9 Beretta";
            cb_weapM9Bereatta.UseVisualStyleBackColor = true;
            cb_weapM9Bereatta.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapShotgun
            // 
            cb_weapShotgun.AutoSize = true;
            cb_weapShotgun.Dock = DockStyle.Fill;
            cb_weapShotgun.Location = new Point(10, 56);
            cb_weapShotgun.Margin = new Padding(10, 0, 0, 0);
            cb_weapShotgun.Name = "cb_weapShotgun";
            cb_weapShotgun.Size = new Size(126, 28);
            cb_weapShotgun.TabIndex = 32;
            cb_weapShotgun.Text = "Shotgun";
            cb_weapShotgun.UseVisualStyleBackColor = true;
            cb_weapShotgun.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM240
            // 
            cb_weapM240.AutoSize = true;
            cb_weapM240.Dock = DockStyle.Fill;
            cb_weapM240.Location = new Point(146, 196);
            cb_weapM240.Margin = new Padding(10, 0, 0, 0);
            cb_weapM240.Name = "cb_weapM240";
            cb_weapM240.Size = new Size(127, 28);
            cb_weapM240.TabIndex = 44;
            cb_weapM240.Text = "M240";
            cb_weapM240.UseVisualStyleBackColor = true;
            cb_weapM240.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapFragGrenade
            // 
            cb_weapFragGrenade.AutoSize = true;
            cb_weapFragGrenade.Dock = DockStyle.Fill;
            cb_weapFragGrenade.Location = new Point(10, 84);
            cb_weapFragGrenade.Margin = new Padding(10, 0, 0, 0);
            cb_weapFragGrenade.Name = "cb_weapFragGrenade";
            cb_weapFragGrenade.Size = new Size(126, 28);
            cb_weapFragGrenade.TabIndex = 34;
            cb_weapFragGrenade.Text = "Frag Grenade";
            cb_weapFragGrenade.UseVisualStyleBackColor = true;
            cb_weapFragGrenade.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM60
            // 
            cb_weapM60.AutoSize = true;
            cb_weapM60.Dock = DockStyle.Fill;
            cb_weapM60.Location = new Point(146, 168);
            cb_weapM60.Margin = new Padding(10, 0, 0, 0);
            cb_weapM60.Name = "cb_weapM60";
            cb_weapM60.Size = new Size(127, 28);
            cb_weapM60.TabIndex = 45;
            cb_weapM60.Text = "M60";
            cb_weapM60.UseVisualStyleBackColor = true;
            cb_weapM60.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapG36
            // 
            cb_weapG36.AutoSize = true;
            cb_weapG36.Dock = DockStyle.Fill;
            cb_weapG36.Location = new Point(146, 140);
            cb_weapG36.Margin = new Padding(10, 0, 0, 0);
            cb_weapG36.Name = "cb_weapG36";
            cb_weapG36.Size = new Size(127, 28);
            cb_weapG36.TabIndex = 43;
            cb_weapG36.Text = "G36";
            cb_weapG36.UseVisualStyleBackColor = true;
            cb_weapG36.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSmokeGrenade
            // 
            cb_weapSmokeGrenade.AutoSize = true;
            cb_weapSmokeGrenade.Dock = DockStyle.Fill;
            cb_weapSmokeGrenade.Location = new Point(10, 112);
            cb_weapSmokeGrenade.Margin = new Padding(10, 0, 0, 0);
            cb_weapSmokeGrenade.Name = "cb_weapSmokeGrenade";
            cb_weapSmokeGrenade.Size = new Size(126, 28);
            cb_weapSmokeGrenade.TabIndex = 36;
            cb_weapSmokeGrenade.Text = "Smoke Grenade";
            cb_weapSmokeGrenade.UseVisualStyleBackColor = true;
            cb_weapSmokeGrenade.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapG3
            // 
            cb_weapG3.AutoSize = true;
            cb_weapG3.Dock = DockStyle.Fill;
            cb_weapG3.Location = new Point(146, 112);
            cb_weapG3.Margin = new Padding(10, 0, 0, 0);
            cb_weapG3.Name = "cb_weapG3";
            cb_weapG3.Size = new Size(127, 28);
            cb_weapG3.TabIndex = 42;
            cb_weapG3.Text = "G3";
            cb_weapG3.UseVisualStyleBackColor = true;
            cb_weapG3.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM16203
            // 
            cb_weapM16203.AutoSize = true;
            cb_weapM16203.Dock = DockStyle.Fill;
            cb_weapM16203.Location = new Point(146, 84);
            cb_weapM16203.Margin = new Padding(10, 0, 0, 0);
            cb_weapM16203.Name = "cb_weapM16203";
            cb_weapM16203.Size = new Size(127, 28);
            cb_weapM16203.TabIndex = 41;
            cb_weapM16203.Text = "M16/203";
            cb_weapM16203.UseVisualStyleBackColor = true;
            cb_weapM16203.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapFlashBang
            // 
            cb_weapFlashBang.AutoSize = true;
            cb_weapFlashBang.Dock = DockStyle.Fill;
            cb_weapFlashBang.Location = new Point(10, 140);
            cb_weapFlashBang.Margin = new Padding(10, 0, 0, 0);
            cb_weapFlashBang.Name = "cb_weapFlashBang";
            cb_weapFlashBang.Size = new Size(126, 28);
            cb_weapFlashBang.TabIndex = 33;
            cb_weapFlashBang.Text = "Flash Bang";
            cb_weapFlashBang.UseVisualStyleBackColor = true;
            cb_weapFlashBang.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM16
            // 
            cb_weapM16.AutoSize = true;
            cb_weapM16.Dock = DockStyle.Fill;
            cb_weapM16.Location = new Point(146, 56);
            cb_weapM16.Margin = new Padding(10, 0, 0, 0);
            cb_weapM16.Name = "cb_weapM16";
            cb_weapM16.Size = new Size(127, 28);
            cb_weapM16.TabIndex = 40;
            cb_weapM16.Text = "M16";
            cb_weapM16.UseVisualStyleBackColor = true;
            cb_weapM16.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapAT4
            // 
            cb_weapAT4.AutoSize = true;
            cb_weapAT4.Dock = DockStyle.Fill;
            cb_weapAT4.Location = new Point(10, 224);
            cb_weapAT4.Margin = new Padding(10, 0, 0, 0);
            cb_weapAT4.Name = "cb_weapAT4";
            cb_weapAT4.Size = new Size(126, 28);
            cb_weapAT4.TabIndex = 48;
            cb_weapAT4.Text = "AT4";
            cb_weapAT4.UseVisualStyleBackColor = true;
            cb_weapAT4.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapCAR15203
            // 
            cb_weapCAR15203.AutoSize = true;
            cb_weapCAR15203.Dock = DockStyle.Fill;
            cb_weapCAR15203.Location = new Point(146, 28);
            cb_weapCAR15203.Margin = new Padding(10, 0, 0, 0);
            cb_weapCAR15203.Name = "cb_weapCAR15203";
            cb_weapCAR15203.Size = new Size(127, 28);
            cb_weapCAR15203.TabIndex = 39;
            cb_weapCAR15203.Text = "CAR 15/203";
            cb_weapCAR15203.UseVisualStyleBackColor = true;
            cb_weapCAR15203.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapClay
            // 
            cb_weapClay.AutoSize = true;
            cb_weapClay.Dock = DockStyle.Fill;
            cb_weapClay.Location = new Point(10, 168);
            cb_weapClay.Margin = new Padding(10, 0, 0, 0);
            cb_weapClay.Name = "cb_weapClay";
            cb_weapClay.Size = new Size(126, 28);
            cb_weapClay.TabIndex = 35;
            cb_weapClay.Text = "Claymore";
            cb_weapClay.UseVisualStyleBackColor = true;
            cb_weapClay.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapCAR15
            // 
            cb_weapCAR15.AutoSize = true;
            cb_weapCAR15.Dock = DockStyle.Fill;
            cb_weapCAR15.Location = new Point(146, 0);
            cb_weapCAR15.Margin = new Padding(10, 0, 0, 0);
            cb_weapCAR15.Name = "cb_weapCAR15";
            cb_weapCAR15.Size = new Size(127, 28);
            cb_weapCAR15.TabIndex = 38;
            cb_weapCAR15.Text = "CAR 15";
            cb_weapCAR15.UseVisualStyleBackColor = true;
            cb_weapCAR15.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSatchel
            // 
            cb_weapSatchel.AutoSize = true;
            cb_weapSatchel.Dock = DockStyle.Fill;
            cb_weapSatchel.Location = new Point(10, 196);
            cb_weapSatchel.Margin = new Padding(10, 0, 0, 0);
            cb_weapSatchel.Name = "cb_weapSatchel";
            cb_weapSatchel.Size = new Size(126, 28);
            cb_weapSatchel.TabIndex = 37;
            cb_weapSatchel.Text = "Satchel Charge";
            cb_weapSatchel.UseVisualStyleBackColor = true;
            cb_weapSatchel.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weap300Tact
            // 
            cb_weap300Tact.AutoSize = true;
            cb_weap300Tact.Dock = DockStyle.Fill;
            cb_weap300Tact.Location = new Point(283, 112);
            cb_weap300Tact.Margin = new Padding(10, 0, 0, 0);
            cb_weap300Tact.Name = "cb_weap300Tact";
            cb_weap300Tact.Size = new Size(128, 28);
            cb_weap300Tact.TabIndex = 53;
            cb_weap300Tact.Text = "MCRT .300 Tactical";
            cb_weap300Tact.UseVisualStyleBackColor = true;
            cb_weap300Tact.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapBarret
            // 
            cb_weapBarret.AutoSize = true;
            cb_weapBarret.Dock = DockStyle.Fill;
            cb_weapBarret.Location = new Point(283, 140);
            cb_weapBarret.Margin = new Padding(10, 0, 0, 0);
            cb_weapBarret.Name = "cb_weapBarret";
            cb_weapBarret.Size = new Size(128, 28);
            cb_weapBarret.TabIndex = 51;
            cb_weapBarret.Text = "Barret";
            cb_weapBarret.UseVisualStyleBackColor = true;
            cb_weapBarret.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapPSG1
            // 
            cb_weapPSG1.AutoSize = true;
            cb_weapPSG1.Dock = DockStyle.Fill;
            cb_weapPSG1.Location = new Point(283, 168);
            cb_weapPSG1.Margin = new Padding(10, 0, 0, 0);
            cb_weapPSG1.Name = "cb_weapPSG1";
            cb_weapPSG1.Size = new Size(128, 28);
            cb_weapPSG1.TabIndex = 52;
            cb_weapPSG1.Text = "PSG-1";
            cb_weapPSG1.UseVisualStyleBackColor = true;
            cb_weapPSG1.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM21
            // 
            cb_weapM21.AutoSize = true;
            cb_weapM21.Dock = DockStyle.Fill;
            cb_weapM21.Location = new Point(283, 196);
            cb_weapM21.Margin = new Padding(10, 0, 0, 0);
            cb_weapM21.Name = "cb_weapM21";
            cb_weapM21.Size = new Size(128, 28);
            cb_weapM21.TabIndex = 49;
            cb_weapM21.Text = "M21";
            cb_weapM21.UseVisualStyleBackColor = true;
            cb_weapM21.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapM24
            // 
            cb_weapM24.AutoSize = true;
            cb_weapM24.Dock = DockStyle.Fill;
            cb_weapM24.Location = new Point(283, 224);
            cb_weapM24.Margin = new Padding(10, 0, 0, 0);
            cb_weapM24.Name = "cb_weapM24";
            cb_weapM24.Size = new Size(128, 28);
            cb_weapM24.TabIndex = 50;
            cb_weapM24.Text = "M24";
            cb_weapM24.UseVisualStyleBackColor = true;
            cb_weapM24.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // cb_weapSaw
            // 
            cb_weapSaw.AutoSize = true;
            cb_weapSaw.Dock = DockStyle.Fill;
            cb_weapSaw.Location = new Point(283, 84);
            cb_weapSaw.Margin = new Padding(10, 0, 0, 0);
            cb_weapSaw.Name = "cb_weapSaw";
            cb_weapSaw.Size = new Size(128, 28);
            cb_weapSaw.TabIndex = 47;
            cb_weapSaw.Text = "SAW";
            cb_weapSaw.UseVisualStyleBackColor = true;
            cb_weapSaw.CheckedChanged += actionClick_WeaponCheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel6);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 280);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            tableLayoutPanel5.SetRowSpan(groupBox1, 4);
            groupBox1.Size = new Size(136, 123);
            groupBox1.TabIndex = 54;
            groupBox1.TabStop = false;
            groupBox1.Text = "Friendly Fire";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Controls.Add(cb_enableFFkills, 0, 0);
            tableLayoutPanel6.Controls.Add(cb_showTeamTags, 0, 1);
            tableLayoutPanel6.Controls.Add(cb_warnFFkils, 0, 2);
            tableLayoutPanel6.Controls.Add(tableLayoutPanel7, 0, 3);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 16);
            tableLayoutPanel6.Margin = new Padding(0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 4;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.Size = new Size(136, 107);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // cb_enableFFkills
            // 
            cb_enableFFkills.AutoSize = true;
            cb_enableFFkills.Dock = DockStyle.Fill;
            cb_enableFFkills.Location = new Point(10, 0);
            cb_enableFFkills.Margin = new Padding(10, 0, 0, 0);
            cb_enableFFkills.Name = "cb_enableFFkills";
            cb_enableFFkills.Size = new Size(126, 26);
            cb_enableFFkills.TabIndex = 4;
            cb_enableFFkills.Text = "Enable FF Kills";
            cb_enableFFkills.UseVisualStyleBackColor = true;
            // 
            // cb_showTeamTags
            // 
            cb_showTeamTags.AutoSize = true;
            cb_showTeamTags.Dock = DockStyle.Fill;
            cb_showTeamTags.Location = new Point(10, 26);
            cb_showTeamTags.Margin = new Padding(10, 0, 0, 0);
            cb_showTeamTags.Name = "cb_showTeamTags";
            cb_showTeamTags.Size = new Size(126, 26);
            cb_showTeamTags.TabIndex = 5;
            cb_showTeamTags.Text = "Show FF Tags";
            cb_showTeamTags.UseVisualStyleBackColor = true;
            // 
            // cb_warnFFkils
            // 
            cb_warnFFkils.AutoSize = true;
            cb_warnFFkils.Dock = DockStyle.Fill;
            cb_warnFFkils.Location = new Point(10, 52);
            cb_warnFFkils.Margin = new Padding(10, 0, 0, 0);
            cb_warnFFkils.Name = "cb_warnFFkils";
            cb_warnFFkils.Size = new Size(126, 26);
            cb_warnFFkils.TabIndex = 6;
            cb_warnFFkils.Text = "Warn FF Kills";
            cb_warnFFkils.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.9032249F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.0967751F));
            tableLayoutPanel7.Controls.Add(label_maxFFkills, 0, 0);
            tableLayoutPanel7.Controls.Add(num_maxFFKills, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(0, 78);
            tableLayoutPanel7.Margin = new Padding(0);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle());
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Size = new Size(136, 29);
            tableLayoutPanel7.TabIndex = 7;
            // 
            // label_maxFFkills
            // 
            label_maxFFkills.AutoSize = true;
            label_maxFFkills.Dock = DockStyle.Fill;
            label_maxFFkills.Location = new Point(0, 0);
            label_maxFFkills.Margin = new Padding(0);
            label_maxFFkills.Name = "label_maxFFkills";
            label_maxFFkills.Size = new Size(85, 29);
            label_maxFFkills.TabIndex = 7;
            label_maxFFkills.Text = "Max FF Kills";
            label_maxFFkills.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_maxFFKills
            // 
            num_maxFFKills.Location = new Point(88, 3);
            num_maxFFKills.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            num_maxFFKills.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_maxFFKills.Name = "num_maxFFKills";
            num_maxFFKills.Size = new Size(45, 23);
            num_maxFFKills.TabIndex = 8;
            num_maxFFKills.TextAlign = HorizontalAlignment.Center;
            num_maxFFKills.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(tableLayoutPanel8);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(273, 280);
            groupBox5.Margin = new Padding(0);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(0);
            tableLayoutPanel5.SetRowSpan(groupBox5, 4);
            groupBox5.Size = new Size(138, 123);
            groupBox5.TabIndex = 55;
            groupBox5.TabStop = false;
            groupBox5.Text = "Roles";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Controls.Add(cb_roleCQB, 0, 0);
            tableLayoutPanel8.Controls.Add(cb_roleGunner, 0, 1);
            tableLayoutPanel8.Controls.Add(cb_roleMedic, 0, 2);
            tableLayoutPanel8.Controls.Add(cb_roleSniper, 0, 3);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(0, 16);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 4;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel8.Size = new Size(138, 107);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // cb_roleCQB
            // 
            cb_roleCQB.AutoSize = true;
            cb_roleCQB.Dock = DockStyle.Fill;
            cb_roleCQB.Enabled = false;
            cb_roleCQB.Location = new Point(10, 0);
            cb_roleCQB.Margin = new Padding(10, 0, 0, 0);
            cb_roleCQB.Name = "cb_roleCQB";
            cb_roleCQB.Size = new Size(128, 26);
            cb_roleCQB.TabIndex = 2;
            cb_roleCQB.Text = "CQB";
            cb_roleCQB.UseVisualStyleBackColor = true;
            // 
            // cb_roleGunner
            // 
            cb_roleGunner.AutoSize = true;
            cb_roleGunner.Dock = DockStyle.Fill;
            cb_roleGunner.Enabled = false;
            cb_roleGunner.Location = new Point(10, 26);
            cb_roleGunner.Margin = new Padding(10, 0, 0, 0);
            cb_roleGunner.Name = "cb_roleGunner";
            cb_roleGunner.Size = new Size(128, 26);
            cb_roleGunner.TabIndex = 2;
            cb_roleGunner.Text = "Gunner";
            cb_roleGunner.UseVisualStyleBackColor = true;
            // 
            // cb_roleMedic
            // 
            cb_roleMedic.AutoSize = true;
            cb_roleMedic.Dock = DockStyle.Fill;
            cb_roleMedic.Enabled = false;
            cb_roleMedic.Location = new Point(10, 52);
            cb_roleMedic.Margin = new Padding(10, 0, 0, 0);
            cb_roleMedic.Name = "cb_roleMedic";
            cb_roleMedic.Size = new Size(128, 26);
            cb_roleMedic.TabIndex = 2;
            cb_roleMedic.Text = "Medic";
            cb_roleMedic.UseVisualStyleBackColor = true;
            // 
            // cb_roleSniper
            // 
            cb_roleSniper.AutoSize = true;
            cb_roleSniper.Dock = DockStyle.Fill;
            cb_roleSniper.Enabled = false;
            cb_roleSniper.Location = new Point(10, 78);
            cb_roleSniper.Margin = new Padding(10, 0, 0, 0);
            cb_roleSniper.Name = "cb_roleSniper";
            cb_roleSniper.Size = new Size(128, 29);
            cb_roleSniper.TabIndex = 2;
            cb_roleSniper.Text = "Sniper";
            cb_roleSniper.UseVisualStyleBackColor = true;
            // 
            // tabGamePlay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "tabGamePlay";
            Size = new Size(966, 422);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxTeamLives).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_maxPlayers).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_flagReturnTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_pspTakeoverTimer).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoreBoardDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_gameStartDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_respawnTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_gameTimeLimit).EndInit();
            groupBox2.ResumeLayout(false);
            scoresTableLayout.ResumeLayout(false);
            scoresTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_scoresFB).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresDM).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scoresKOTH).EndInit();
            groupBox_lobbyPasswords.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxFFKills).EndInit();
            groupBox5.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        public Button btn_serverControl;
        public Button btn_LockLobby;
        public Button btn_LoadSettings;
        public Button btn_ExportSettings;
        public Button btn_ResetSettings;
        public Button btn_SaveSettings;
        public Button btn_ServerUpdate;
        private ToolTip toolTip1;
        private Panel panel1;
        public GroupBox groupBox_lobbyPasswords;
        public CheckBox cb_novaRequired;
        public GroupBox groupBox2;
        public TableLayoutPanel scoresTableLayout;
        public NumericUpDown num_scoresFB;
        public NumericUpDown num_scoresDM;
        public Label label_flagball;
        public NumericUpDown num_scoresKOTH;
        public Label label_koth;
        public Label label_dm;
        public Label label_maxPlayers;
        public NumericUpDown num_maxPlayers;
        public Label label_scoreDelay;
        public NumericUpDown num_scoreBoardDelay;
        public Label label_replayMaps;
        public ComboBox cb_replayMaps;
        public Label label_respawnTime;
        public NumericUpDown num_respawnTime;
        public Label label_startDelay;
        public Label label_timeLimit;
        public NumericUpDown num_gameStartDelay;
        public NumericUpDown num_gameTimeLimit;
        public TextBox tb_redPassword;
        public CheckBox cb_autoBalance;
        public TextBox tb_bluePassword;
        public CheckBox cb_autoRange;
        public NumericUpDown num_maxTeamLives;
        public Label label2;
        public CheckBox cb_showClays;
        public NumericUpDown num_flagReturnTime;
        public Label label1;
        public CheckBox cb_showTracers;
        public NumericUpDown num_pspTakeoverTimer;
        public Label label_pspTakeover;
        public CheckBox cb_enableLeftLean;
        public CheckBox cb_customSkins;
        public CheckBox cb_enableDistroyBuildings;
        public CheckBox cb_enableFatBullets;
        public CheckBox cb_enableOneShotKills;
        public CheckBox cb_roleSniper;
        public CheckBox cb_roleMedic;
        public CheckBox cb_roleGunner;
        public CheckBox cb_roleCQB;
        public CheckBox checkBox_selectNone;
        public CheckBox checkBox_selectAll;
        public CheckBox cb_weapShotgun;
        public CheckBox cb_weapM9Bereatta;
        public CheckBox cb_weapColt45;
        public CheckBox cb_weapSatchel;
        public CheckBox cb_weapSmokeGrenade;
        public CheckBox cb_weapClay;
        public CheckBox cb_weapFragGrenade;
        public CheckBox cb_weapFlashBang;
        public CheckBox cb_weapCAR15203;
        public CheckBox cb_weapCAR15;
        public CheckBox cb_weapM16203;
        public CheckBox cb_weapM16;
        public CheckBox cb_weapG36;
        public CheckBox cb_weapG3;
        public CheckBox cb_weapM240;
        public CheckBox cb_weapM60;
        public CheckBox cb_weapMP5;
        public CheckBox cb_weapSaw;
        public CheckBox cb_weapAT4;
        public CheckBox cb_weapM21;
        public CheckBox cb_weapM24;
        public CheckBox cb_weapBarret;
        public CheckBox cb_weapPSG1;
        public CheckBox cb_weap300Tact;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel4;
        public NumericUpDown num_maxFFKills;
        public CheckBox cb_enableFFkills;
        public Label label_maxFFkills;
        public CheckBox cb_warnFFkils;
        public CheckBox cb_showTeamTags;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel5;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel8;
    }
}
