namespace BHD_ServerManager.Forms.Panels
{
    partial class tabAdmins
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
            subTabAdmins = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            dg_AdminUsers = new DataGridView();
            admin_id = new DataGridViewTextBoxColumn();
            admin_username = new DataGridViewTextBoxColumn();
            admin_role = new DataGridViewTextBoxColumn();
            connectionStatus = new DataGridViewTextBoxColumn();
            lastActive = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            groupBox4 = new GroupBox();
            tb_adminPass = new TextBox();
            tb_adminUser = new TextBox();
            groupBox3 = new GroupBox();
            cb_roleDisabled = new CheckBox();
            cb_roleModerator = new CheckBox();
            cb_roleAdmin = new CheckBox();
            panel2 = new Panel();
            label1 = new Label();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            btn_adminDelete = new Button();
            btn_adminSave = new Button();
            btn_adminAdd = new Button();
            btn_adminNew = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            btn_adminImport = new Button();
            btn_AdminExport = new Button();
            subTabPermissions = new TabPage();
            subTabAdminLogs = new TabPage();
            dg_adminLog = new DataGridView();
            adminLog_datetime = new DataGridViewTextBoxColumn();
            adminLog_username = new DataGridViewTextBoxColumn();
            adminLog_log = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            subTabAdmins.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_AdminUsers).BeginInit();
            panel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            subTabAdminLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_adminLog).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(subTabAdmins);
            tabControl1.Controls.Add(subTabPermissions);
            tabControl1.Controls.Add(subTabAdminLogs);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(902, 362);
            tabControl1.TabIndex = 0;
            // 
            // subTabAdmins
            // 
            subTabAdmins.Controls.Add(tableLayoutPanel1);
            subTabAdmins.Location = new Point(4, 24);
            subTabAdmins.Margin = new Padding(0);
            subTabAdmins.Name = "subTabAdmins";
            subTabAdmins.Padding = new Padding(3);
            subTabAdmins.Size = new Size(894, 334);
            subTabAdmins.TabIndex = 0;
            subTabAdmins.Text = "Admins";
            subTabAdmins.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 520F));
            tableLayoutPanel1.Controls.Add(dg_AdminUsers, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(888, 328);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dg_AdminUsers
            // 
            dg_AdminUsers.AllowUserToAddRows = false;
            dg_AdminUsers.AllowUserToDeleteRows = false;
            dg_AdminUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_AdminUsers.Columns.AddRange(new DataGridViewColumn[] { admin_id, admin_username, admin_role, connectionStatus, lastActive });
            dg_AdminUsers.Dock = DockStyle.Fill;
            dg_AdminUsers.Location = new Point(371, 3);
            dg_AdminUsers.Name = "dg_AdminUsers";
            dg_AdminUsers.ReadOnly = true;
            dg_AdminUsers.RowHeadersVisible = false;
            dg_AdminUsers.Size = new Size(514, 322);
            dg_AdminUsers.TabIndex = 1;
            dg_AdminUsers.CellDoubleClick += ActionClick_SelectAdmin;
            // 
            // admin_id
            // 
            admin_id.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            admin_id.HeaderText = "ID";
            admin_id.MinimumWidth = 50;
            admin_id.Name = "admin_id";
            admin_id.ReadOnly = true;
            admin_id.Width = 50;
            // 
            // admin_username
            // 
            admin_username.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            admin_username.HeaderText = "Username";
            admin_username.MinimumWidth = 150;
            admin_username.Name = "admin_username";
            admin_username.ReadOnly = true;
            admin_username.Width = 150;
            // 
            // admin_role
            // 
            admin_role.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            admin_role.HeaderText = "Role";
            admin_role.MinimumWidth = 100;
            admin_role.Name = "admin_role";
            admin_role.ReadOnly = true;
            // 
            // connectionStatus
            // 
            connectionStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            connectionStatus.HeaderText = "Status";
            connectionStatus.MinimumWidth = 50;
            connectionStatus.Name = "connectionStatus";
            connectionStatus.ReadOnly = true;
            // 
            // lastActive
            // 
            lastActive.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            lastActive.HeaderText = "Last Seen";
            lastActive.MinimumWidth = 150;
            lastActive.Name = "lastActive";
            lastActive.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(368, 328);
            panel1.TabIndex = 2;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tb_adminPass);
            groupBox4.Controls.Add(tb_adminUser);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(0, 135);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(225, 73);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "User Credentials";
            // 
            // tb_adminPass
            // 
            tb_adminPass.Dock = DockStyle.Bottom;
            tb_adminPass.Location = new Point(3, 47);
            tb_adminPass.Margin = new Padding(0, 10, 0, 0);
            tb_adminPass.Name = "tb_adminPass";
            tb_adminPass.PasswordChar = '*';
            tb_adminPass.PlaceholderText = "Password";
            tb_adminPass.Size = new Size(219, 23);
            tb_adminPass.TabIndex = 2;
            tb_adminPass.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_adminUser
            // 
            tb_adminUser.Dock = DockStyle.Top;
            tb_adminUser.Location = new Point(3, 19);
            tb_adminUser.Margin = new Padding(0, 10, 0, 0);
            tb_adminUser.Name = "tb_adminUser";
            tb_adminUser.PlaceholderText = "Username";
            tb_adminUser.Size = new Size(219, 23);
            tb_adminUser.TabIndex = 1;
            tb_adminUser.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cb_roleDisabled);
            groupBox3.Controls.Add(cb_roleModerator);
            groupBox3.Controls.Add(cb_roleAdmin);
            groupBox3.Dock = DockStyle.Right;
            groupBox3.Location = new Point(225, 135);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(143, 73);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Roles";
            // 
            // cb_roleDisabled
            // 
            cb_roleDisabled.AutoSize = true;
            cb_roleDisabled.CheckAlign = ContentAlignment.MiddleRight;
            cb_roleDisabled.Location = new Point(64, 50);
            cb_roleDisabled.Name = "cb_roleDisabled";
            cb_roleDisabled.Size = new Size(71, 19);
            cb_roleDisabled.TabIndex = 3;
            cb_roleDisabled.Tag = "0";
            cb_roleDisabled.Text = "Disabled";
            cb_roleDisabled.UseVisualStyleBackColor = true;
            cb_roleDisabled.Click += actionClick_roleDisabled;
            // 
            // cb_roleModerator
            // 
            cb_roleModerator.AutoSize = true;
            cb_roleModerator.CheckAlign = ContentAlignment.MiddleRight;
            cb_roleModerator.Location = new Point(53, 31);
            cb_roleModerator.Name = "cb_roleModerator";
            cb_roleModerator.Size = new Size(82, 19);
            cb_roleModerator.TabIndex = 2;
            cb_roleModerator.Tag = "1";
            cb_roleModerator.Text = "Moderator";
            cb_roleModerator.UseVisualStyleBackColor = true;
            cb_roleModerator.Click += actionClick_roleModerator;
            // 
            // cb_roleAdmin
            // 
            cb_roleAdmin.AutoSize = true;
            cb_roleAdmin.CheckAlign = ContentAlignment.MiddleRight;
            cb_roleAdmin.Location = new Point(36, 12);
            cb_roleAdmin.Name = "cb_roleAdmin";
            cb_roleAdmin.Size = new Size(99, 19);
            cb_roleAdmin.TabIndex = 0;
            cb_roleAdmin.Tag = "2";
            cb_roleAdmin.Text = "Administrator";
            cb_roleAdmin.UseVisualStyleBackColor = true;
            cb_roleAdmin.Click += actionClick_roleAdmin;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(368, 135);
            panel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(100, 60);
            label1.Name = "label1";
            label1.Size = new Size(169, 15);
            label1.TabIndex = 0;
            label1.Text = "Admin Panel Unclaimed Space";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(0, 208);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(368, 60);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Controls";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(btn_adminDelete, 3, 0);
            tableLayoutPanel3.Controls.Add(btn_adminSave, 2, 0);
            tableLayoutPanel3.Controls.Add(btn_adminAdd, 1, 0);
            tableLayoutPanel3.Controls.Add(btn_adminNew, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(362, 38);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // btn_adminDelete
            // 
            btn_adminDelete.AccessibleDescription = " c";
            btn_adminDelete.Dock = DockStyle.Fill;
            btn_adminDelete.Location = new Point(273, 3);
            btn_adminDelete.Name = "btn_adminDelete";
            btn_adminDelete.Size = new Size(86, 32);
            btn_adminDelete.TabIndex = 4;
            btn_adminDelete.Text = "Delete";
            btn_adminDelete.UseVisualStyleBackColor = true;
            btn_adminDelete.Click += ActionClick_AdminDelete;
            // 
            // btn_adminSave
            // 
            btn_adminSave.Dock = DockStyle.Fill;
            btn_adminSave.Location = new Point(183, 3);
            btn_adminSave.Name = "btn_adminSave";
            btn_adminSave.Size = new Size(84, 32);
            btn_adminSave.TabIndex = 3;
            btn_adminSave.Text = "Save";
            btn_adminSave.UseVisualStyleBackColor = true;
            btn_adminSave.Click += ActionClick_AdminEditUser;
            // 
            // btn_adminAdd
            // 
            btn_adminAdd.Dock = DockStyle.Fill;
            btn_adminAdd.Location = new Point(93, 3);
            btn_adminAdd.Name = "btn_adminAdd";
            btn_adminAdd.Size = new Size(84, 32);
            btn_adminAdd.TabIndex = 2;
            btn_adminAdd.Text = "Add";
            btn_adminAdd.UseVisualStyleBackColor = true;
            btn_adminAdd.Click += ActionClick_AdminAddNew;
            // 
            // btn_adminNew
            // 
            btn_adminNew.Dock = DockStyle.Fill;
            btn_adminNew.Location = new Point(3, 3);
            btn_adminNew.Name = "btn_adminNew";
            btn_adminNew.Size = new Size(84, 32);
            btn_adminNew.TabIndex = 1;
            btn_adminNew.Text = "New";
            btn_adminNew.UseVisualStyleBackColor = true;
            btn_adminNew.Click += ActionClick_AdminNewUser;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(0, 268);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(368, 60);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Misc";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(btn_adminImport, 2, 0);
            tableLayoutPanel2.Controls.Add(btn_AdminExport, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(362, 38);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_adminImport
            // 
            btn_adminImport.Dock = DockStyle.Fill;
            btn_adminImport.Location = new Point(183, 3);
            btn_adminImport.Name = "btn_adminImport";
            btn_adminImport.Size = new Size(84, 32);
            btn_adminImport.TabIndex = 0;
            btn_adminImport.Text = "Import";
            btn_adminImport.UseVisualStyleBackColor = true;
            // 
            // btn_AdminExport
            // 
            btn_AdminExport.Dock = DockStyle.Fill;
            btn_AdminExport.Location = new Point(273, 3);
            btn_AdminExport.Name = "btn_AdminExport";
            btn_AdminExport.Size = new Size(86, 32);
            btn_AdminExport.TabIndex = 1;
            btn_AdminExport.Text = "Export";
            btn_AdminExport.UseVisualStyleBackColor = true;
            // 
            // subTabPermissions
            // 
            subTabPermissions.Location = new Point(4, 24);
            subTabPermissions.Name = "subTabPermissions";
            subTabPermissions.Padding = new Padding(3);
            subTabPermissions.Size = new Size(894, 334);
            subTabPermissions.TabIndex = 1;
            subTabPermissions.Text = "Permissions";
            subTabPermissions.UseVisualStyleBackColor = true;
            // 
            // subTabAdminLogs
            // 
            subTabAdminLogs.Controls.Add(dg_adminLog);
            subTabAdminLogs.Location = new Point(4, 24);
            subTabAdminLogs.Name = "subTabAdminLogs";
            subTabAdminLogs.Size = new Size(894, 334);
            subTabAdminLogs.TabIndex = 2;
            subTabAdminLogs.Text = "Logs";
            subTabAdminLogs.UseVisualStyleBackColor = true;
            // 
            // dg_adminLog
            // 
            dg_adminLog.AllowUserToAddRows = false;
            dg_adminLog.AllowUserToDeleteRows = false;
            dg_adminLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_adminLog.Columns.AddRange(new DataGridViewColumn[] { adminLog_datetime, adminLog_username, adminLog_log });
            dg_adminLog.Dock = DockStyle.Fill;
            dg_adminLog.Location = new Point(0, 0);
            dg_adminLog.Name = "dg_adminLog";
            dg_adminLog.ReadOnly = true;
            dg_adminLog.RowHeadersVisible = false;
            dg_adminLog.Size = new Size(894, 334);
            dg_adminLog.TabIndex = 1;
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
            // tabAdmins
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Margin = new Padding(0);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabAdmins";
            Size = new Size(902, 362);
            tabControl1.ResumeLayout(false);
            subTabAdmins.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_AdminUsers).EndInit();
            panel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            subTabAdminLogs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dg_adminLog).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage subTabAdmins;
        private TabPage subTabPermissions;
        private TabPage subTabAdminLogs;
        public DataGridView dg_adminLog;
        private DataGridViewTextBoxColumn adminLog_datetime;
        private DataGridViewTextBoxColumn adminLog_username;
        private DataGridViewTextBoxColumn adminLog_log;
        private TableLayoutPanel tableLayoutPanel1;
        public DataGridView dg_AdminUsers;
        private DataGridViewTextBoxColumn admin_id;
        private DataGridViewTextBoxColumn admin_username;
        private DataGridViewTextBoxColumn admin_role;
        private DataGridViewTextBoxColumn connectionStatus;
        private DataGridViewTextBoxColumn lastActive;
        private Panel panel1;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private Panel panel2;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btn_adminNew;
        private Button btn_adminAdd;
        private Button btn_adminSave;
        private Button btn_adminDelete;
        private Button btn_adminImport;
        private Button btn_AdminExport;
        private TextBox tb_adminUser;
        private TextBox tb_adminPass;
        private CheckBox cb_roleDisabled;
        private CheckBox cb_roleModerator;
        private CheckBox cb_roleAdmin;
        private Label label1;
    }
}
