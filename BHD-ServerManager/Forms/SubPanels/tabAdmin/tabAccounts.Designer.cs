using System.ComponentModel;

namespace ServerManager.Forms.SubPanels.tabAdmin;

partial class tabAccounts
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        tableAccounts = new System.Windows.Forms.TableLayoutPanel();
        groupBox1 = new System.Windows.Forms.GroupBox();
        dataGridView1 = new System.Windows.Forms.DataGridView();
        admins_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
        admins_username = new System.Windows.Forms.DataGridViewTextBoxColumn();
        admins_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
        admins_dateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
        admins_dateLastSeen = new System.Windows.Forms.DataGridViewTextBoxColumn();
        tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        iconButton6 = new FontAwesome.Sharp.IconButton();
        iconButton5 = new FontAwesome.Sharp.IconButton();
        iconButton4 = new FontAwesome.Sharp.IconButton();
        iconButton3 = new FontAwesome.Sharp.IconButton();
        iconButton1 = new FontAwesome.Sharp.IconButton();
        iconButton2 = new FontAwesome.Sharp.IconButton();
        groupBox2 = new System.Windows.Forms.GroupBox();
        tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        label_Confirm = new System.Windows.Forms.Label();
        label_Password = new System.Windows.Forms.Label();
        label_Username = new System.Windows.Forms.Label();
        textBox_username = new System.Windows.Forms.TextBox();
        textBox_password = new System.Windows.Forms.TextBox();
        textBox_password2 = new System.Windows.Forms.TextBox();
        checkBox_showPass = new System.Windows.Forms.CheckBox();
        groupBox3 = new System.Windows.Forms.GroupBox();
        tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
        checkBox_permAudit = new System.Windows.Forms.CheckBox();
        checkBox_permUsers = new System.Windows.Forms.CheckBox();
        checkBox_permStats = new System.Windows.Forms.CheckBox();
        checkBox_permBans = new System.Windows.Forms.CheckBox();
        checkBox_permChat = new System.Windows.Forms.CheckBox();
        checkBox_permPlayers = new System.Windows.Forms.CheckBox();
        checkBox_permMaps = new System.Windows.Forms.CheckBox();
        checkBox_permGamePlay = new System.Windows.Forms.CheckBox();
        checkBox_permProfile = new System.Windows.Forms.CheckBox();
        checkBox_userActive = new System.Windows.Forms.CheckBox();
        groupBox4 = new System.Windows.Forms.GroupBox();
        textBox_userNotes = new System.Windows.Forms.TextBox();
        toolTip1 = new System.Windows.Forms.ToolTip(components);
        tableAccounts.SuspendLayout();
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
        // tableAccounts
        // 
        tableAccounts.ColumnCount = 5;
        tableAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableAccounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableAccounts.Controls.Add(groupBox1, 1, 0);
        tableAccounts.Controls.Add(tableLayoutPanel2, 3, 0);
        tableAccounts.Controls.Add(groupBox2, 3, 1);
        tableAccounts.Location = new System.Drawing.Point(0, 0);
        tableAccounts.Name = "tableAccounts";
        tableAccounts.RowCount = 3;
        tableAccounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
        tableAccounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableAccounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
        tableAccounts.Size = new System.Drawing.Size(958, 391);
        tableAccounts.TabIndex = 0;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(dataGridView1);
        groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        groupBox1.Location = new System.Drawing.Point(23, 3);
        groupBox1.Name = "groupBox1";
        tableAccounts.SetRowSpan(groupBox1, 3);
        groupBox1.Size = new System.Drawing.Size(443, 385);
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
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { admins_id, admins_username, admins_status, admins_dateCreated, admins_dateLastSeen });
        dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
        dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        dataGridView1.Location = new System.Drawing.Point(3, 19);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.Size = new System.Drawing.Size(437, 363);
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
        admins_username.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
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
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.666666F));
        tableLayoutPanel2.Controls.Add(iconButton6, 5, 0);
        tableLayoutPanel2.Controls.Add(iconButton5, 4, 0);
        tableLayoutPanel2.Controls.Add(iconButton4, 3, 0);
        tableLayoutPanel2.Controls.Add(iconButton3, 2, 0);
        tableLayoutPanel2.Controls.Add(iconButton1, 0, 0);
        tableLayoutPanel2.Controls.Add(iconButton2, 1, 0);
        tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel2.Location = new System.Drawing.Point(489, 0);
        tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel2.Size = new System.Drawing.Size(449, 40);
        tableLayoutPanel2.TabIndex = 1;
        // 
        // iconButton6
        // 
        iconButton6.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton6.IconChar = FontAwesome.Sharp.IconChar.Cancel;
        iconButton6.IconColor = System.Drawing.Color.Black;
        iconButton6.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton6.IconSize = 28;
        iconButton6.Location = new System.Drawing.Point(373, 3);
        iconButton6.Name = "iconButton6";
        iconButton6.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
        iconButton6.Size = new System.Drawing.Size(73, 34);
        iconButton6.TabIndex = 5;
        toolTip1.SetToolTip(iconButton6, "Close / Cancel");
        iconButton6.UseVisualStyleBackColor = true;
        // 
        // iconButton5
        // 
        iconButton5.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton5.IconChar = FontAwesome.Sharp.IconChar.None;
        iconButton5.IconColor = System.Drawing.Color.Black;
        iconButton5.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton5.IconSize = 28;
        iconButton5.Location = new System.Drawing.Point(299, 3);
        iconButton5.Name = "iconButton5";
        iconButton5.Size = new System.Drawing.Size(68, 34);
        iconButton5.TabIndex = 4;
        iconButton5.UseVisualStyleBackColor = true;
        iconButton5.Visible = false;
        // 
        // iconButton4
        // 
        iconButton4.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton4.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
        iconButton4.IconColor = System.Drawing.Color.Black;
        iconButton4.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton4.IconSize = 26;
        iconButton4.Location = new System.Drawing.Point(225, 3);
        iconButton4.Name = "iconButton4";
        iconButton4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
        iconButton4.Size = new System.Drawing.Size(68, 34);
        iconButton4.TabIndex = 3;
        toolTip1.SetToolTip(iconButton4, "Delete User");
        iconButton4.UseVisualStyleBackColor = true;
        // 
        // iconButton3
        // 
        iconButton3.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton3.IconChar = FontAwesome.Sharp.IconChar.Save;
        iconButton3.IconColor = System.Drawing.Color.Black;
        iconButton3.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton3.IconSize = 28;
        iconButton3.Location = new System.Drawing.Point(151, 3);
        iconButton3.Name = "iconButton3";
        iconButton3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
        iconButton3.Size = new System.Drawing.Size(68, 34);
        iconButton3.TabIndex = 2;
        toolTip1.SetToolTip(iconButton3, "Save");
        iconButton3.UseVisualStyleBackColor = true;
        // 
        // iconButton1
        // 
        iconButton1.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton1.IconChar = FontAwesome.Sharp.IconChar.Refresh;
        iconButton1.IconColor = System.Drawing.Color.Black;
        iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton1.IconSize = 30;
        iconButton1.Location = new System.Drawing.Point(3, 3);
        iconButton1.Name = "iconButton1";
        iconButton1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
        iconButton1.Size = new System.Drawing.Size(68, 34);
        iconButton1.TabIndex = 0;
        toolTip1.SetToolTip(iconButton1, "Refresh");
        iconButton1.UseVisualStyleBackColor = true;
        // 
        // iconButton2
        // 
        iconButton2.Dock = System.Windows.Forms.DockStyle.Fill;
        iconButton2.IconChar = FontAwesome.Sharp.IconChar.PersonCirclePlus;
        iconButton2.IconColor = System.Drawing.Color.Black;
        iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
        iconButton2.IconSize = 32;
        iconButton2.Location = new System.Drawing.Point(77, 3);
        iconButton2.Name = "iconButton2";
        iconButton2.Padding = new System.Windows.Forms.Padding(6, 2, 0, 0);
        iconButton2.Size = new System.Drawing.Size(68, 34);
        iconButton2.TabIndex = 1;
        toolTip1.SetToolTip(iconButton2, "New User");
        iconButton2.UseVisualStyleBackColor = true;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(tableLayoutPanel3);
        groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
        groupBox2.Location = new System.Drawing.Point(492, 43);
        groupBox2.Name = "groupBox2";
        tableAccounts.SetRowSpan(groupBox2, 2);
        groupBox2.Size = new System.Drawing.Size(443, 345);
        groupBox2.TabIndex = 2;
        groupBox2.TabStop = false;
        groupBox2.Text = "User Details";
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 5;
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
        tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel3.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 10;
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel3.Size = new System.Drawing.Size(437, 323);
        tableLayoutPanel3.TabIndex = 0;
        // 
        // label_Confirm
        // 
        label_Confirm.AutoSize = true;
        label_Confirm.Dock = System.Windows.Forms.DockStyle.Fill;
        label_Confirm.Location = new System.Drawing.Point(23, 60);
        label_Confirm.Name = "label_Confirm";
        label_Confirm.Size = new System.Drawing.Size(126, 30);
        label_Confirm.TabIndex = 2;
        label_Confirm.Text = "Confirm:";
        label_Confirm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_Password
        // 
        label_Password.AutoSize = true;
        label_Password.Dock = System.Windows.Forms.DockStyle.Fill;
        label_Password.Location = new System.Drawing.Point(23, 30);
        label_Password.Name = "label_Password";
        label_Password.Size = new System.Drawing.Size(126, 30);
        label_Password.TabIndex = 1;
        label_Password.Text = "Password:";
        label_Password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_Username
        // 
        label_Username.AutoSize = true;
        label_Username.Dock = System.Windows.Forms.DockStyle.Fill;
        label_Username.Location = new System.Drawing.Point(23, 0);
        label_Username.Name = "label_Username";
        label_Username.Size = new System.Drawing.Size(126, 30);
        label_Username.TabIndex = 0;
        label_Username.Text = "Username:";
        label_Username.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // textBox_username
        // 
        textBox_username.BackColor = System.Drawing.SystemColors.Control;
        textBox_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        textBox_username.Dock = System.Windows.Forms.DockStyle.Fill;
        textBox_username.Location = new System.Drawing.Point(155, 4);
        textBox_username.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
        textBox_username.Name = "textBox_username";
        textBox_username.Size = new System.Drawing.Size(126, 23);
        textBox_username.TabIndex = 3;
        // 
        // textBox_password
        // 
        textBox_password.BackColor = System.Drawing.SystemColors.Control;
        textBox_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        textBox_password.Dock = System.Windows.Forms.DockStyle.Fill;
        textBox_password.Location = new System.Drawing.Point(155, 34);
        textBox_password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
        textBox_password.Name = "textBox_password";
        textBox_password.Size = new System.Drawing.Size(126, 23);
        textBox_password.TabIndex = 4;
        textBox_password.UseSystemPasswordChar = true;
        // 
        // textBox_password2
        // 
        textBox_password2.BackColor = System.Drawing.SystemColors.Control;
        textBox_password2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        textBox_password2.Dock = System.Windows.Forms.DockStyle.Fill;
        textBox_password2.Location = new System.Drawing.Point(155, 64);
        textBox_password2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
        textBox_password2.Name = "textBox_password2";
        textBox_password2.Size = new System.Drawing.Size(126, 23);
        textBox_password2.TabIndex = 5;
        textBox_password2.UseSystemPasswordChar = true;
        // 
        // checkBox_showPass
        // 
        checkBox_showPass.AutoSize = true;
        checkBox_showPass.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_showPass.Location = new System.Drawing.Point(287, 33);
        checkBox_showPass.Name = "checkBox_showPass";
        checkBox_showPass.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
        checkBox_showPass.Size = new System.Drawing.Size(126, 24);
        checkBox_showPass.TabIndex = 6;
        checkBox_showPass.Text = "Show";
        checkBox_showPass.UseVisualStyleBackColor = true;
        // 
        // groupBox3
        // 
        tableLayoutPanel3.SetColumnSpan(groupBox3, 3);
        groupBox3.Controls.Add(tableLayoutPanel4);
        groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
        groupBox3.Location = new System.Drawing.Point(23, 123);
        groupBox3.Name = "groupBox3";
        tableLayoutPanel3.SetRowSpan(groupBox3, 4);
        groupBox3.Size = new System.Drawing.Size(390, 114);
        groupBox3.TabIndex = 7;
        groupBox3.TabStop = false;
        groupBox3.Text = "Permissions";
        // 
        // tableLayoutPanel4
        //
        tableLayoutPanel4.ColumnCount = 3;
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.Controls.Add(checkBox_permAudit, 2, 2);
        tableLayoutPanel4.Controls.Add(checkBox_permUsers, 1, 2);
        tableLayoutPanel4.Controls.Add(checkBox_permStats, 0, 2);
        tableLayoutPanel4.Controls.Add(checkBox_permBans, 2, 1);
        tableLayoutPanel4.Controls.Add(checkBox_permChat, 1, 1);
        tableLayoutPanel4.Controls.Add(checkBox_permPlayers, 0, 1);
        tableLayoutPanel4.Controls.Add(checkBox_permMaps, 2, 0);
        tableLayoutPanel4.Controls.Add(checkBox_permGamePlay, 1, 0);
        tableLayoutPanel4.Controls.Add(checkBox_permProfile, 0, 0);
        tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel4.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanel4.Name = "tableLayoutPanel4";
        tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(3);
        tableLayoutPanel4.RowCount = 3;
        tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
        tableLayoutPanel4.Size = new System.Drawing.Size(384, 92);
        tableLayoutPanel4.TabIndex = 0;
        //
        // checkBox_permAudit
        //
        checkBox_permAudit.AutoSize = true;
        checkBox_permAudit.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permAudit.Location = new System.Drawing.Point(258, 62);
        checkBox_permAudit.Name = "checkBox_permAudit";
        checkBox_permAudit.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permAudit.Size = new System.Drawing.Size(120, 24);
        checkBox_permAudit.TabIndex = 8;
        checkBox_permAudit.Text = "Audit";
        checkBox_permAudit.UseVisualStyleBackColor = true;
        //
        // checkBox_permUsers
        //
        checkBox_permUsers.AutoSize = true;
        checkBox_permUsers.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permUsers.Location = new System.Drawing.Point(132, 62);
        checkBox_permUsers.Name = "checkBox_permUsers";
        checkBox_permUsers.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permUsers.Size = new System.Drawing.Size(120, 24);
        checkBox_permUsers.TabIndex = 7;
        checkBox_permUsers.Text = "Users";
        checkBox_permUsers.UseVisualStyleBackColor = true;
        //
        // checkBox_permStats
        //
        checkBox_permStats.AutoSize = true;
        checkBox_permStats.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permStats.Location = new System.Drawing.Point(6, 62);
        checkBox_permStats.Name = "checkBox_permStats";
        checkBox_permStats.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permStats.Size = new System.Drawing.Size(120, 24);
        checkBox_permStats.TabIndex = 6;
        checkBox_permStats.Text = "Stats";
        checkBox_permStats.UseVisualStyleBackColor = true;
        //
        // checkBox_permBans
        //
        checkBox_permBans.AutoSize = true;
        checkBox_permBans.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permBans.Location = new System.Drawing.Point(258, 34);
        checkBox_permBans.Name = "checkBox_permBans";
        checkBox_permBans.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permBans.Size = new System.Drawing.Size(120, 22);
        checkBox_permBans.TabIndex = 5;
        checkBox_permBans.Text = "Bans";
        checkBox_permBans.UseVisualStyleBackColor = true;
        //
        // checkBox_permChat
        // 
        checkBox_permChat.AutoSize = true;
        checkBox_permChat.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permChat.Location = new System.Drawing.Point(132, 34);
        checkBox_permChat.Name = "checkBox_permChat";
        checkBox_permChat.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permChat.Size = new System.Drawing.Size(120, 22);
        checkBox_permChat.TabIndex = 4;
        checkBox_permChat.Text = "Chat";
        checkBox_permChat.UseVisualStyleBackColor = true;
        //
        // checkBox_permPlayers
        //
        checkBox_permPlayers.AutoSize = true;
        checkBox_permPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permPlayers.Location = new System.Drawing.Point(6, 34);
        checkBox_permPlayers.Name = "checkBox_permPlayers";
        checkBox_permPlayers.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permPlayers.Size = new System.Drawing.Size(120, 22);
        checkBox_permPlayers.TabIndex = 3;
        checkBox_permPlayers.Text = "Players";
        checkBox_permPlayers.UseVisualStyleBackColor = true;
        //
        // checkBox_permMaps
        //
        checkBox_permMaps.AutoSize = true;
        checkBox_permMaps.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permMaps.Location = new System.Drawing.Point(258, 6);
        checkBox_permMaps.Name = "checkBox_permMaps";
        checkBox_permMaps.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permMaps.Size = new System.Drawing.Size(120, 22);
        checkBox_permMaps.TabIndex = 2;
        checkBox_permMaps.Text = "Maps";
        checkBox_permMaps.UseVisualStyleBackColor = true;
        //
        // checkBox_permGamePlay
        //
        checkBox_permGamePlay.AutoSize = true;
        checkBox_permGamePlay.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permGamePlay.Location = new System.Drawing.Point(132, 6);
        checkBox_permGamePlay.Name = "checkBox_permGamePlay";
        checkBox_permGamePlay.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permGamePlay.Size = new System.Drawing.Size(120, 22);
        checkBox_permGamePlay.TabIndex = 1;
        checkBox_permGamePlay.Text = "Gameplay";
        checkBox_permGamePlay.UseVisualStyleBackColor = true;
        //
        // checkBox_permProfile
        //
        checkBox_permProfile.AutoSize = true;
        checkBox_permProfile.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_permProfile.Location = new System.Drawing.Point(6, 6);
        checkBox_permProfile.Name = "checkBox_permProfile";
        checkBox_permProfile.Padding = new System.Windows.Forms.Padding(20, 3, 0, 0);
        checkBox_permProfile.Size = new System.Drawing.Size(120, 22);
        checkBox_permProfile.TabIndex = 0;
        checkBox_permProfile.Text = "Profile";
        checkBox_permProfile.UseVisualStyleBackColor = true;
        // 
        // checkBox_userActive
        // 
        checkBox_userActive.AutoSize = true;
        checkBox_userActive.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBox_userActive.Location = new System.Drawing.Point(287, 3);
        checkBox_userActive.Name = "checkBox_userActive";
        checkBox_userActive.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
        checkBox_userActive.Size = new System.Drawing.Size(126, 24);
        checkBox_userActive.TabIndex = 8;
        checkBox_userActive.Text = "Active";
        checkBox_userActive.UseVisualStyleBackColor = true;
        // 
        // groupBox4
        // 
        tableLayoutPanel3.SetColumnSpan(groupBox4, 3);
        groupBox4.Controls.Add(textBox_userNotes);
        groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
        groupBox4.Location = new System.Drawing.Point(23, 243);
        groupBox4.Name = "groupBox4";
        tableLayoutPanel3.SetRowSpan(groupBox4, 2);
        groupBox4.Size = new System.Drawing.Size(390, 77);
        groupBox4.TabIndex = 9;
        groupBox4.TabStop = false;
        groupBox4.Text = "Notes";
        // 
        // textBox_userNotes
        // 
        textBox_userNotes.BackColor = System.Drawing.SystemColors.Control;
        textBox_userNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
        textBox_userNotes.Dock = System.Windows.Forms.DockStyle.Fill;
        textBox_userNotes.Location = new System.Drawing.Point(3, 19);
        textBox_userNotes.Multiline = true;
        textBox_userNotes.Name = "textBox_userNotes";
        textBox_userNotes.PlaceholderText = "Place user notes here...";
        textBox_userNotes.Size = new System.Drawing.Size(384, 55);
        textBox_userNotes.TabIndex = 0;
        // 
        // tabAccounts
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(tableAccounts);
        Margin = new System.Windows.Forms.Padding(0);
        MaximumSize = new System.Drawing.Size(958, 391);
        MinimumSize = new System.Drawing.Size(958, 391);
        Size = new System.Drawing.Size(958, 391);
        tableAccounts.ResumeLayout(false);
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

    private System.Windows.Forms.TableLayoutPanel tableAccounts;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.DataGridView dataGridView1;
    private DataGridViewTextBoxColumn admins_id;
    private DataGridViewTextBoxColumn admins_username;
    private DataGridViewTextBoxColumn admins_status;
    private DataGridViewTextBoxColumn admins_dateCreated;
    private DataGridViewTextBoxColumn admins_dateLastSeen;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private FontAwesome.Sharp.IconButton iconButton6;
    private FontAwesome.Sharp.IconButton iconButton5;
    private FontAwesome.Sharp.IconButton iconButton4;
    private FontAwesome.Sharp.IconButton iconButton3;
    private FontAwesome.Sharp.IconButton iconButton1;
    private ToolTip toolTip1;
    private FontAwesome.Sharp.IconButton iconButton2;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Label label_Password;
    private System.Windows.Forms.Label label_Username;
    private System.Windows.Forms.Label label_Confirm;
    private System.Windows.Forms.TextBox textBox_username;
    private System.Windows.Forms.TextBox textBox_password;
    private System.Windows.Forms.TextBox textBox_password2;
    private System.Windows.Forms.CheckBox checkBox_showPass;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.CheckBox checkBox_permUsers;
    private System.Windows.Forms.CheckBox checkBox_permStats;
    private System.Windows.Forms.CheckBox checkBox_permBans;
    private System.Windows.Forms.CheckBox checkBox_permChat;
    private System.Windows.Forms.CheckBox checkBox_permPlayers;
    private System.Windows.Forms.CheckBox checkBox_permMaps;
    private System.Windows.Forms.CheckBox checkBox_permGamePlay;
    private System.Windows.Forms.CheckBox checkBox_permProfile;
    private System.Windows.Forms.CheckBox checkBox_userActive;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox textBox_userNotes;
    private System.Windows.Forms.CheckBox checkBox_permAudit;
}