namespace BHD_ServerManager.Forms.Panels
{
    partial class tabAdmin
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
            groupBox1 = new GroupBox();
            dataGridView1 = new DataGridView();
            admins_id = new DataGridViewTextBoxColumn();
            admins_username = new DataGridViewTextBoxColumn();
            admins_status = new DataGridViewTextBoxColumn();
            admins_dateCreated = new DataGridViewTextBoxColumn();
            admins_dateLastSeen = new DataGridViewTextBoxColumn();
            tableLayoutPanel2 = new TableLayoutPanel();
            iconButton6 = new FontAwesome.Sharp.IconButton();
            iconButton5 = new FontAwesome.Sharp.IconButton();
            iconButton4 = new FontAwesome.Sharp.IconButton();
            iconButton3 = new FontAwesome.Sharp.IconButton();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            iconButton2 = new FontAwesome.Sharp.IconButton();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            label_Confirm = new Label();
            label_Password = new Label();
            label_Username = new Label();
            textBox_username = new TextBox();
            textBox_password = new TextBox();
            textBox_password2 = new TextBox();
            checkBox_showPass = new CheckBox();
            groupBox3 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            checkBox_permUsers = new CheckBox();
            checkBox_permStats = new CheckBox();
            checkBox_permBans = new CheckBox();
            checkBox_permChat = new CheckBox();
            checkBox_permPlayers = new CheckBox();
            checkBox_permMaps = new CheckBox();
            checkBox_permGamePlay = new CheckBox();
            checkBox_permProfile = new CheckBox();
            checkBox_userActive = new CheckBox();
            groupBox4 = new GroupBox();
            textBox_userNotes = new TextBox();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 3, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 3, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(966, 422);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(23, 3);
            groupBox1.Name = "groupBox1";
            tableLayoutPanel1.SetRowSpan(groupBox1, 3);
            groupBox1.Size = new Size(447, 416);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "User Accounts";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { admins_id, admins_username, admins_status, admins_dateCreated, admins_dateLastSeen });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(441, 394);
            dataGridView1.TabIndex = 0;
            // 
            // admins_id
            // 
            admins_id.HeaderText = "ID";
            admins_id.Name = "admins_id";
            admins_id.ReadOnly = true;
            admins_id.Visible = false;
            // 
            // admins_username
            // 
            admins_username.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            admins_username.HeaderText = "Username";
            admins_username.Name = "admins_username";
            admins_username.ReadOnly = true;
            // 
            // admins_status
            // 
            admins_status.HeaderText = "Status";
            admins_status.Name = "admins_status";
            admins_status.ReadOnly = true;
            // 
            // admins_dateCreated
            // 
            admins_dateCreated.HeaderText = "Created";
            admins_dateCreated.Name = "admins_dateCreated";
            admins_dateCreated.ReadOnly = true;
            // 
            // admins_dateLastSeen
            // 
            admins_dateLastSeen.HeaderText = "Last Login";
            admins_dateLastSeen.Name = "admins_dateLastSeen";
            admins_dateLastSeen.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 6;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.Controls.Add(iconButton6, 5, 0);
            tableLayoutPanel2.Controls.Add(iconButton5, 4, 0);
            tableLayoutPanel2.Controls.Add(iconButton4, 3, 0);
            tableLayoutPanel2.Controls.Add(iconButton3, 2, 0);
            tableLayoutPanel2.Controls.Add(iconButton1, 0, 0);
            tableLayoutPanel2.Controls.Add(iconButton2, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(493, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(453, 40);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // iconButton6
            // 
            iconButton6.Dock = DockStyle.Fill;
            iconButton6.IconChar = FontAwesome.Sharp.IconChar.Cancel;
            iconButton6.IconColor = Color.Black;
            iconButton6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton6.IconSize = 28;
            iconButton6.Location = new Point(378, 3);
            iconButton6.Name = "iconButton6";
            iconButton6.Padding = new Padding(0, 3, 0, 0);
            iconButton6.Size = new Size(72, 34);
            iconButton6.TabIndex = 5;
            toolTip1.SetToolTip(iconButton6, "Close / Cancel");
            iconButton6.UseVisualStyleBackColor = true;
            // 
            // iconButton5
            // 
            iconButton5.Dock = DockStyle.Fill;
            iconButton5.IconChar = FontAwesome.Sharp.IconChar.None;
            iconButton5.IconColor = Color.Black;
            iconButton5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton5.IconSize = 28;
            iconButton5.Location = new Point(303, 3);
            iconButton5.Name = "iconButton5";
            iconButton5.Size = new Size(69, 34);
            iconButton5.TabIndex = 4;
            iconButton5.UseVisualStyleBackColor = true;
            iconButton5.Visible = false;
            // 
            // iconButton4
            // 
            iconButton4.Dock = DockStyle.Fill;
            iconButton4.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            iconButton4.IconColor = Color.Black;
            iconButton4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton4.IconSize = 26;
            iconButton4.Location = new Point(228, 3);
            iconButton4.Name = "iconButton4";
            iconButton4.Padding = new Padding(0, 3, 0, 0);
            iconButton4.Size = new Size(69, 34);
            iconButton4.TabIndex = 3;
            toolTip1.SetToolTip(iconButton4, "Delete User");
            iconButton4.UseVisualStyleBackColor = true;
            // 
            // iconButton3
            // 
            iconButton3.Dock = DockStyle.Fill;
            iconButton3.IconChar = FontAwesome.Sharp.IconChar.Save;
            iconButton3.IconColor = Color.Black;
            iconButton3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton3.IconSize = 28;
            iconButton3.Location = new Point(153, 3);
            iconButton3.Name = "iconButton3";
            iconButton3.Padding = new Padding(0, 4, 0, 0);
            iconButton3.Size = new Size(69, 34);
            iconButton3.TabIndex = 2;
            toolTip1.SetToolTip(iconButton3, "Save");
            iconButton3.UseVisualStyleBackColor = true;
            // 
            // iconButton1
            // 
            iconButton1.Dock = DockStyle.Fill;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            iconButton1.IconColor = Color.Black;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 30;
            iconButton1.Location = new Point(3, 3);
            iconButton1.Name = "iconButton1";
            iconButton1.Padding = new Padding(0, 2, 0, 0);
            iconButton1.Size = new Size(69, 34);
            iconButton1.TabIndex = 0;
            toolTip1.SetToolTip(iconButton1, "Refresh");
            iconButton1.UseVisualStyleBackColor = true;
            // 
            // iconButton2
            // 
            iconButton2.Dock = DockStyle.Fill;
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.PersonCirclePlus;
            iconButton2.IconColor = Color.Black;
            iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton2.IconSize = 32;
            iconButton2.Location = new Point(78, 3);
            iconButton2.Name = "iconButton2";
            iconButton2.Padding = new Padding(6, 2, 0, 0);
            iconButton2.Size = new Size(69, 34);
            iconButton2.TabIndex = 1;
            toolTip1.SetToolTip(iconButton2, "New User");
            iconButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(496, 43);
            groupBox2.Name = "groupBox2";
            tableLayoutPanel1.SetRowSpan(groupBox2, 2);
            groupBox2.Size = new Size(447, 376);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "User Details";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 5;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(label_Confirm, 1, 2);
            tableLayoutPanel3.Controls.Add(label_Password, 1, 1);
            tableLayoutPanel3.Controls.Add(label_Username, 1, 0);
            tableLayoutPanel3.Controls.Add(textBox_username, 2, 0);
            tableLayoutPanel3.Controls.Add(textBox_password, 2, 1);
            tableLayoutPanel3.Controls.Add(textBox_password2, 2, 2);
            tableLayoutPanel3.Controls.Add(checkBox_showPass, 3, 1);
            tableLayoutPanel3.Controls.Add(groupBox3, 1, 4);
            tableLayoutPanel3.Controls.Add(checkBox_userActive, 3, 0);
            tableLayoutPanel3.Controls.Add(groupBox4, 1, 8);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 10;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(441, 354);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label_Confirm
            // 
            label_Confirm.AutoSize = true;
            label_Confirm.Dock = DockStyle.Fill;
            label_Confirm.Location = new Point(23, 60);
            label_Confirm.Name = "label_Confirm";
            label_Confirm.Size = new Size(127, 30);
            label_Confirm.TabIndex = 2;
            label_Confirm.Text = "Confirm:";
            label_Confirm.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_Password
            // 
            label_Password.AutoSize = true;
            label_Password.Dock = DockStyle.Fill;
            label_Password.Location = new Point(23, 30);
            label_Password.Name = "label_Password";
            label_Password.Size = new Size(127, 30);
            label_Password.TabIndex = 1;
            label_Password.Text = "Password:";
            label_Password.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_Username
            // 
            label_Username.AutoSize = true;
            label_Username.Dock = DockStyle.Fill;
            label_Username.Location = new Point(23, 0);
            label_Username.Name = "label_Username";
            label_Username.Size = new Size(127, 30);
            label_Username.TabIndex = 0;
            label_Username.Text = "Username:";
            label_Username.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBox_username
            // 
            textBox_username.BackColor = SystemColors.Control;
            textBox_username.BorderStyle = BorderStyle.FixedSingle;
            textBox_username.Dock = DockStyle.Fill;
            textBox_username.Location = new Point(156, 4);
            textBox_username.Margin = new Padding(3, 4, 3, 3);
            textBox_username.Name = "textBox_username";
            textBox_username.Size = new Size(127, 23);
            textBox_username.TabIndex = 3;
            // 
            // textBox_password
            // 
            textBox_password.BackColor = SystemColors.Control;
            textBox_password.BorderStyle = BorderStyle.FixedSingle;
            textBox_password.Dock = DockStyle.Fill;
            textBox_password.Location = new Point(156, 34);
            textBox_password.Margin = new Padding(3, 4, 3, 3);
            textBox_password.Name = "textBox_password";
            textBox_password.Size = new Size(127, 23);
            textBox_password.TabIndex = 4;
            textBox_password.UseSystemPasswordChar = true;
            // 
            // textBox_password2
            // 
            textBox_password2.BackColor = SystemColors.Control;
            textBox_password2.BorderStyle = BorderStyle.FixedSingle;
            textBox_password2.Dock = DockStyle.Fill;
            textBox_password2.Location = new Point(156, 64);
            textBox_password2.Margin = new Padding(3, 4, 3, 3);
            textBox_password2.Name = "textBox_password2";
            textBox_password2.Size = new Size(127, 23);
            textBox_password2.TabIndex = 5;
            textBox_password2.UseSystemPasswordChar = true;
            // 
            // checkBox_showPass
            // 
            checkBox_showPass.AutoSize = true;
            checkBox_showPass.Dock = DockStyle.Fill;
            checkBox_showPass.Location = new Point(289, 33);
            checkBox_showPass.Name = "checkBox_showPass";
            checkBox_showPass.Padding = new Padding(10, 3, 0, 0);
            checkBox_showPass.Size = new Size(127, 24);
            checkBox_showPass.TabIndex = 6;
            checkBox_showPass.Text = "Show";
            checkBox_showPass.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            tableLayoutPanel3.SetColumnSpan(groupBox3, 3);
            groupBox3.Controls.Add(tableLayoutPanel4);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(23, 123);
            groupBox3.Name = "groupBox3";
            tableLayoutPanel3.SetRowSpan(groupBox3, 4);
            groupBox3.Size = new Size(393, 114);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Permissions";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Controls.Add(checkBox_permUsers, 1, 2);
            tableLayoutPanel4.Controls.Add(checkBox_permStats, 0, 2);
            tableLayoutPanel4.Controls.Add(checkBox_permBans, 2, 1);
            tableLayoutPanel4.Controls.Add(checkBox_permChat, 1, 1);
            tableLayoutPanel4.Controls.Add(checkBox_permPlayers, 0, 1);
            tableLayoutPanel4.Controls.Add(checkBox_permMaps, 2, 0);
            tableLayoutPanel4.Controls.Add(checkBox_permGamePlay, 1, 0);
            tableLayoutPanel4.Controls.Add(checkBox_permProfile, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(3);
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Size = new Size(387, 92);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // checkBox_permUsers
            // 
            checkBox_permUsers.AutoSize = true;
            checkBox_permUsers.Dock = DockStyle.Fill;
            checkBox_permUsers.Location = new Point(133, 62);
            checkBox_permUsers.Name = "checkBox_permUsers";
            checkBox_permUsers.Padding = new Padding(20, 3, 0, 0);
            checkBox_permUsers.Size = new Size(121, 24);
            checkBox_permUsers.TabIndex = 7;
            checkBox_permUsers.Text = "Users";
            checkBox_permUsers.UseVisualStyleBackColor = true;
            // 
            // checkBox_permStats
            // 
            checkBox_permStats.AutoSize = true;
            checkBox_permStats.Dock = DockStyle.Fill;
            checkBox_permStats.Location = new Point(6, 62);
            checkBox_permStats.Name = "checkBox_permStats";
            checkBox_permStats.Padding = new Padding(20, 3, 0, 0);
            checkBox_permStats.Size = new Size(121, 24);
            checkBox_permStats.TabIndex = 6;
            checkBox_permStats.Text = "Stats";
            checkBox_permStats.UseVisualStyleBackColor = true;
            // 
            // checkBox_permBans
            // 
            checkBox_permBans.AutoSize = true;
            checkBox_permBans.Dock = DockStyle.Fill;
            checkBox_permBans.Location = new Point(260, 34);
            checkBox_permBans.Name = "checkBox_permBans";
            checkBox_permBans.Padding = new Padding(20, 3, 0, 0);
            checkBox_permBans.Size = new Size(121, 22);
            checkBox_permBans.TabIndex = 5;
            checkBox_permBans.Text = "Bans";
            checkBox_permBans.UseVisualStyleBackColor = true;
            // 
            // checkBox_permChat
            // 
            checkBox_permChat.AutoSize = true;
            checkBox_permChat.Dock = DockStyle.Fill;
            checkBox_permChat.Location = new Point(133, 34);
            checkBox_permChat.Name = "checkBox_permChat";
            checkBox_permChat.Padding = new Padding(20, 3, 0, 0);
            checkBox_permChat.Size = new Size(121, 22);
            checkBox_permChat.TabIndex = 4;
            checkBox_permChat.Text = "Chat";
            checkBox_permChat.UseVisualStyleBackColor = true;
            // 
            // checkBox_permPlayers
            // 
            checkBox_permPlayers.AutoSize = true;
            checkBox_permPlayers.Dock = DockStyle.Fill;
            checkBox_permPlayers.Location = new Point(6, 34);
            checkBox_permPlayers.Name = "checkBox_permPlayers";
            checkBox_permPlayers.Padding = new Padding(20, 3, 0, 0);
            checkBox_permPlayers.Size = new Size(121, 22);
            checkBox_permPlayers.TabIndex = 3;
            checkBox_permPlayers.Text = "Players";
            checkBox_permPlayers.UseVisualStyleBackColor = true;
            // 
            // checkBox_permMaps
            // 
            checkBox_permMaps.AutoSize = true;
            checkBox_permMaps.Dock = DockStyle.Fill;
            checkBox_permMaps.Location = new Point(260, 6);
            checkBox_permMaps.Name = "checkBox_permMaps";
            checkBox_permMaps.Padding = new Padding(20, 3, 0, 0);
            checkBox_permMaps.Size = new Size(121, 22);
            checkBox_permMaps.TabIndex = 2;
            checkBox_permMaps.Text = "Maps";
            checkBox_permMaps.UseVisualStyleBackColor = true;
            // 
            // checkBox_permGamePlay
            // 
            checkBox_permGamePlay.AutoSize = true;
            checkBox_permGamePlay.Dock = DockStyle.Fill;
            checkBox_permGamePlay.Location = new Point(133, 6);
            checkBox_permGamePlay.Name = "checkBox_permGamePlay";
            checkBox_permGamePlay.Padding = new Padding(20, 3, 0, 0);
            checkBox_permGamePlay.Size = new Size(121, 22);
            checkBox_permGamePlay.TabIndex = 1;
            checkBox_permGamePlay.Text = "Gameplay";
            checkBox_permGamePlay.UseVisualStyleBackColor = true;
            // 
            // checkBox_permProfile
            // 
            checkBox_permProfile.AutoSize = true;
            checkBox_permProfile.Dock = DockStyle.Fill;
            checkBox_permProfile.Location = new Point(6, 6);
            checkBox_permProfile.Name = "checkBox_permProfile";
            checkBox_permProfile.Padding = new Padding(20, 3, 0, 0);
            checkBox_permProfile.Size = new Size(121, 22);
            checkBox_permProfile.TabIndex = 0;
            checkBox_permProfile.Text = "Profile";
            checkBox_permProfile.UseVisualStyleBackColor = true;
            // 
            // checkBox_userActive
            // 
            checkBox_userActive.AutoSize = true;
            checkBox_userActive.Dock = DockStyle.Fill;
            checkBox_userActive.Location = new Point(289, 3);
            checkBox_userActive.Name = "checkBox_userActive";
            checkBox_userActive.Padding = new Padding(10, 3, 0, 0);
            checkBox_userActive.Size = new Size(127, 24);
            checkBox_userActive.TabIndex = 8;
            checkBox_userActive.Text = "Active";
            checkBox_userActive.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            tableLayoutPanel3.SetColumnSpan(groupBox4, 3);
            groupBox4.Controls.Add(textBox_userNotes);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(23, 243);
            groupBox4.Name = "groupBox4";
            tableLayoutPanel3.SetRowSpan(groupBox4, 2);
            groupBox4.Size = new Size(393, 108);
            groupBox4.TabIndex = 9;
            groupBox4.TabStop = false;
            groupBox4.Text = "Notes";
            // 
            // textBox_userNotes
            // 
            textBox_userNotes.BackColor = SystemColors.Control;
            textBox_userNotes.BorderStyle = BorderStyle.None;
            textBox_userNotes.Dock = DockStyle.Fill;
            textBox_userNotes.Location = new Point(3, 19);
            textBox_userNotes.Multiline = true;
            textBox_userNotes.Name = "textBox_userNotes";
            textBox_userNotes.PlaceholderText = "Place user notes here...";
            textBox_userNotes.Size = new Size(387, 86);
            textBox_userNotes.TabIndex = 0;
            // 
            // tabAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new Size(966, 422);
            MinimumSize = new Size(966, 422);
            Name = "tabAdmin";
            Size = new Size(966, 422);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn admins_id;
        private DataGridViewTextBoxColumn admins_username;
        private DataGridViewTextBoxColumn admins_status;
        private DataGridViewTextBoxColumn admins_dateCreated;
        private DataGridViewTextBoxColumn admins_dateLastSeen;
        private TableLayoutPanel tableLayoutPanel2;
        private FontAwesome.Sharp.IconButton iconButton6;
        private FontAwesome.Sharp.IconButton iconButton5;
        private FontAwesome.Sharp.IconButton iconButton4;
        private FontAwesome.Sharp.IconButton iconButton3;
        private FontAwesome.Sharp.IconButton iconButton1;
        private ToolTip toolTip1;
        private FontAwesome.Sharp.IconButton iconButton2;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label_Password;
        private Label label_Username;
        private Label label_Confirm;
        private TextBox textBox_username;
        private TextBox textBox_password;
        private TextBox textBox_password2;
        private CheckBox checkBox_showPass;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox checkBox_permUsers;
        private CheckBox checkBox_permStats;
        private CheckBox checkBox_permBans;
        private CheckBox checkBox_permChat;
        private CheckBox checkBox_permPlayers;
        private CheckBox checkBox_permMaps;
        private CheckBox checkBox_permGamePlay;
        private CheckBox checkBox_permProfile;
        private CheckBox checkBox_userActive;
        private GroupBox groupBox4;
        private TextBox textBox_userNotes;
    }
}
