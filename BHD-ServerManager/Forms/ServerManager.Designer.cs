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
            tabStats = new TabPage();
            tabStatsControl = new TabControl();
            tabPlayerStats = new TabPage();
            dataGridViewPlayerStats = new DataGridView();
            tabWeaponStats = new TabPage();
            dataGridViewWeaponStats = new DataGridView();
            tabBabstats = new TabPage();
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
            tabStats.SuspendLayout();
            tabStatsControl.SuspendLayout();
            tabPlayerStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).BeginInit();
            tabWeaponStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).BeginInit();
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
            tabBans.Location = new Point(4, 24);
            tabBans.Name = "tabBans";
            tabBans.Padding = new Padding(3);
            tabBans.Size = new Size(902, 362);
            tabBans.TabIndex = 4;
            tabBans.Text = "Bans";
            tabBans.UseVisualStyleBackColor = true;
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
            tabBabstats.Location = new Point(4, 24);
            tabBabstats.Name = "tabBabstats";
            tabBabstats.Size = new Size(888, 328);
            tabBabstats.TabIndex = 2;
            tabBabstats.Text = "Babstats";
            tabBabstats.UseVisualStyleBackColor = true;
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
            tabStats.ResumeLayout(false);
            tabStatsControl.ResumeLayout(false);
            tabPlayerStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayerStats).EndInit();
            tabWeaponStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewWeaponStats).EndInit();
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
        internal TabControl tabControl;
        private TabPage tabStats;
        private TabControl tabStatsControl;
        private TabPage tabPlayerStats;
        private TabPage tabWeaponStats;
        private TabPage tabBabstats;
        internal DataGridView dataGridViewPlayerStats;
        internal DataGridView dataGridViewWeaponStats;
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
        public DataGridView dg_AdminUsers;
        private TabPage tabProfile;
        private ToolStripLabel label_TimeLeft;
        private ToolStripLabel label_WinCondition;
        private ToolStripLabel label_RedScore;
        private ToolStripLabel label_BlueScore;
        private ToolStripLabel label_PlayersOnline;
        private TabPage tabChat;
    }
}
