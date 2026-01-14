namespace BHD_ServerManager.Forms.Panels
{
    partial class tabProfile
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
            profileFileManagers = new TabControl();
            tabProfilePage1 = new TabPage();
            groupBox8 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            cb_enableMaxCheck = new CheckBox();
            cb_enableMinCheck = new CheckBox();
            num_maxPing = new NumericUpDown();
            num_minPing = new NumericUpDown();
            groupBox7 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            cb_serverDedicated = new CheckBox();
            cb_requireNova = new CheckBox();
            label3 = new Label();
            tb_serverPassword = new TextBox();
            label1 = new Label();
            num_serverPort = new NumericUpDown();
            cb_serverIP = new ComboBox();
            groupBox6 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            tb_serverMessage = new TextBox();
            serverCountryCode = new TextBox();
            tb_serverName = new TextBox();
            tb_hostName = new TextBox();
            tabProfilePage2 = new TabPage();
            groupBox1 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            tb_modFile = new TextBox();
            btn_profileBrowse2 = new Button();
            groupBox2 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            btn_profileBrowse1 = new Button();
            tb_profileServerPath = new TextBox();
            groupBox3 = new GroupBox();
            tableAtributes = new TableLayoutPanel();
            profileServerAttribute01 = new CheckBox();
            profileServerAttribute02 = new CheckBox();
            profileServerAttribute03 = new CheckBox();
            profileServerAttribute04 = new CheckBox();
            profileServerAttribute05 = new CheckBox();
            profileServerAttribute06 = new CheckBox();
            profileServerAttribute07 = new CheckBox();
            profileServerAttribute08 = new CheckBox();
            profileServerAttribute09 = new CheckBox();
            profileServerAttribute10 = new CheckBox();
            profileServerAttribute11 = new CheckBox();
            profileServerAttribute12 = new CheckBox();
            profileServerAttribute13 = new CheckBox();
            profileServerAttribute14 = new CheckBox();
            profileServerAttribute15 = new CheckBox();
            profileServerAttribute16 = new CheckBox();
            profileServerAttribute17 = new CheckBox();
            profileServerAttribute18 = new CheckBox();
            profileServerAttribute19 = new CheckBox();
            profileServerAttribute20 = new CheckBox();
            profileServerAttribute21 = new CheckBox();
            panel1 = new Panel();
            groupBox5 = new GroupBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            btn_resetProfile = new Button();
            btn_saveProfile = new Button();
            folderProfileBrowserDialog = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            tableLayoutPanel1.SuspendLayout();
            profileFileManagers.SuspendLayout();
            tabProfilePage1.SuspendLayout();
            groupBox8.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxPing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_minPing).BeginInit();
            groupBox7.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_serverPort).BeginInit();
            groupBox6.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tabProfilePage2.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox3.SuspendLayout();
            tableAtributes.SuspendLayout();
            panel1.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53.623188F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46.376812F));
            tableLayoutPanel1.Controls.Add(profileFileManagers, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(966, 422);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // profileFileManagers
            // 
            profileFileManagers.Controls.Add(tabProfilePage1);
            profileFileManagers.Controls.Add(tabProfilePage2);
            profileFileManagers.Dock = DockStyle.Fill;
            profileFileManagers.Location = new Point(521, 3);
            profileFileManagers.Name = "profileFileManagers";
            profileFileManagers.SelectedIndex = 0;
            profileFileManagers.Size = new Size(442, 416);
            profileFileManagers.TabIndex = 0;
            // 
            // tabProfilePage1
            // 
            tabProfilePage1.Controls.Add(groupBox8);
            tabProfilePage1.Controls.Add(groupBox7);
            tabProfilePage1.Controls.Add(groupBox6);
            tabProfilePage1.Location = new Point(4, 24);
            tabProfilePage1.Name = "tabProfilePage1";
            tabProfilePage1.Padding = new Padding(3);
            tabProfilePage1.Size = new Size(434, 388);
            tabProfilePage1.TabIndex = 0;
            tabProfilePage1.Text = "Profile Information";
            tabProfilePage1.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(tableLayoutPanel7);
            groupBox8.Dock = DockStyle.Bottom;
            groupBox8.Location = new Point(3, 300);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(428, 85);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "Connection Ping Restrictions";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75.40603F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.5939674F));
            tableLayoutPanel7.Controls.Add(cb_enableMaxCheck, 0, 1);
            tableLayoutPanel7.Controls.Add(cb_enableMinCheck, 0, 0);
            tableLayoutPanel7.Controls.Add(num_maxPing, 1, 1);
            tableLayoutPanel7.Controls.Add(num_minPing, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Size = new Size(422, 63);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // cb_enableMaxCheck
            // 
            cb_enableMaxCheck.AutoSize = true;
            cb_enableMaxCheck.CheckAlign = ContentAlignment.MiddleRight;
            cb_enableMaxCheck.Dock = DockStyle.Fill;
            cb_enableMaxCheck.Location = new Point(3, 34);
            cb_enableMaxCheck.Name = "cb_enableMaxCheck";
            cb_enableMaxCheck.Size = new Size(312, 26);
            cb_enableMaxCheck.TabIndex = 8;
            cb_enableMaxCheck.Text = "Enable Required Max Ping";
            cb_enableMaxCheck.TextAlign = ContentAlignment.MiddleRight;
            cb_enableMaxCheck.UseVisualStyleBackColor = true;
            // 
            // cb_enableMinCheck
            // 
            cb_enableMinCheck.AutoSize = true;
            cb_enableMinCheck.CheckAlign = ContentAlignment.MiddleRight;
            cb_enableMinCheck.Dock = DockStyle.Fill;
            cb_enableMinCheck.Location = new Point(3, 3);
            cb_enableMinCheck.Name = "cb_enableMinCheck";
            cb_enableMinCheck.Size = new Size(312, 25);
            cb_enableMinCheck.TabIndex = 7;
            cb_enableMinCheck.Text = "Enable Required Min Ping";
            cb_enableMinCheck.TextAlign = ContentAlignment.MiddleRight;
            cb_enableMinCheck.UseVisualStyleBackColor = true;
            // 
            // num_maxPing
            // 
            num_maxPing.Dock = DockStyle.Fill;
            num_maxPing.Enabled = false;
            num_maxPing.Location = new Point(321, 34);
            num_maxPing.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            num_maxPing.Name = "num_maxPing";
            num_maxPing.Size = new Size(98, 23);
            num_maxPing.TabIndex = 6;
            num_maxPing.TextAlign = HorizontalAlignment.Center;
            num_maxPing.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            // 
            // num_minPing
            // 
            num_minPing.Dock = DockStyle.Fill;
            num_minPing.Enabled = false;
            num_minPing.Location = new Point(321, 3);
            num_minPing.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            num_minPing.Name = "num_minPing";
            num_minPing.Size = new Size(98, 23);
            num_minPing.TabIndex = 5;
            num_minPing.TextAlign = HorizontalAlignment.Center;
            num_minPing.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(tableLayoutPanel6);
            groupBox7.Dock = DockStyle.Top;
            groupBox7.Location = new Point(3, 152);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(428, 148);
            groupBox7.TabIndex = 1;
            groupBox7.TabStop = false;
            groupBox7.Text = "Server Details";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43.43435F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.32323F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.242424F));
            tableLayoutPanel6.Controls.Add(cb_serverDedicated, 1, 3);
            tableLayoutPanel6.Controls.Add(cb_requireNova, 1, 2);
            tableLayoutPanel6.Controls.Add(label3, 0, 1);
            tableLayoutPanel6.Controls.Add(tb_serverPassword, 1, 1);
            tableLayoutPanel6.Controls.Add(label1, 0, 0);
            tableLayoutPanel6.Controls.Add(num_serverPort, 2, 0);
            tableLayoutPanel6.Controls.Add(cb_serverIP, 1, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 4;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.Size = new Size(422, 126);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // cb_serverDedicated
            // 
            cb_serverDedicated.AutoSize = true;
            cb_serverDedicated.CheckAlign = ContentAlignment.MiddleRight;
            cb_serverDedicated.Checked = true;
            cb_serverDedicated.CheckState = CheckState.Checked;
            tableLayoutPanel6.SetColumnSpan(cb_serverDedicated, 2);
            cb_serverDedicated.Dock = DockStyle.Fill;
            cb_serverDedicated.Location = new Point(186, 96);
            cb_serverDedicated.Margin = new Padding(3, 3, 10, 3);
            cb_serverDedicated.Name = "cb_serverDedicated";
            cb_serverDedicated.Size = new Size(226, 27);
            cb_serverDedicated.TabIndex = 11;
            cb_serverDedicated.Text = "Run in Dedicated Mode";
            cb_serverDedicated.TextAlign = ContentAlignment.MiddleRight;
            cb_serverDedicated.UseVisualStyleBackColor = true;
            // 
            // cb_requireNova
            // 
            cb_requireNova.AutoSize = true;
            cb_requireNova.CheckAlign = ContentAlignment.MiddleRight;
            tableLayoutPanel6.SetColumnSpan(cb_requireNova, 2);
            cb_requireNova.Dock = DockStyle.Fill;
            cb_requireNova.Location = new Point(186, 65);
            cb_requireNova.Margin = new Padding(3, 3, 10, 3);
            cb_requireNova.Name = "cb_requireNova";
            cb_requireNova.Size = new Size(226, 25);
            cb_requireNova.TabIndex = 10;
            cb_requireNova.Text = "NovaWorld Login Required";
            cb_requireNova.TextAlign = ContentAlignment.MiddleRight;
            cb_requireNova.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 31);
            label3.Name = "label3";
            label3.Size = new Size(177, 31);
            label3.TabIndex = 9;
            label3.Text = "Lobby Password";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tb_serverPassword
            // 
            tableLayoutPanel6.SetColumnSpan(tb_serverPassword, 2);
            tb_serverPassword.Dock = DockStyle.Fill;
            tb_serverPassword.Location = new Point(186, 34);
            tb_serverPassword.MaxLength = 16;
            tb_serverPassword.Name = "tb_serverPassword";
            tb_serverPassword.PlaceholderText = "Server Password";
            tb_serverPassword.Size = new Size(233, 23);
            tb_serverPassword.TabIndex = 6;
            tb_serverPassword.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(177, 31);
            label1.TabIndex = 5;
            label1.Text = "Game IP Infomation";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // num_serverPort
            // 
            num_serverPort.Dock = DockStyle.Fill;
            num_serverPort.Location = new Point(322, 3);
            num_serverPort.Maximum = new decimal(new int[] { 65999, 0, 0, 0 });
            num_serverPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_serverPort.Name = "num_serverPort";
            num_serverPort.Size = new Size(97, 23);
            num_serverPort.TabIndex = 2;
            num_serverPort.TextAlign = HorizontalAlignment.Center;
            num_serverPort.Value = new decimal(new int[] { 17479, 0, 0, 0 });
            // 
            // cb_serverIP
            // 
            cb_serverIP.Dock = DockStyle.Fill;
            cb_serverIP.FormattingEnabled = true;
            cb_serverIP.Items.AddRange(new object[] { "0.0.0.0" });
            cb_serverIP.Location = new Point(186, 3);
            cb_serverIP.Name = "cb_serverIP";
            cb_serverIP.Size = new Size(130, 23);
            cb_serverIP.TabIndex = 3;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(tableLayoutPanel5);
            groupBox6.Dock = DockStyle.Top;
            groupBox6.Location = new Point(3, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(428, 149);
            groupBox6.TabIndex = 0;
            groupBox6.TabStop = false;
            groupBox6.Text = "Host Information";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.6658325F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.66583F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6683331F));
            tableLayoutPanel5.Controls.Add(tb_serverMessage, 0, 1);
            tableLayoutPanel5.Controls.Add(serverCountryCode, 2, 0);
            tableLayoutPanel5.Controls.Add(tb_serverName, 1, 0);
            tableLayoutPanel5.Controls.Add(tb_hostName, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(422, 127);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // tb_serverMessage
            // 
            tableLayoutPanel5.SetColumnSpan(tb_serverMessage, 3);
            tb_serverMessage.Dock = DockStyle.Fill;
            tb_serverMessage.Location = new Point(3, 31);
            tb_serverMessage.MaxLength = 85;
            tb_serverMessage.Multiline = true;
            tb_serverMessage.Name = "tb_serverMessage";
            tb_serverMessage.PlaceholderText = "Server Welcome Message";
            tb_serverMessage.Size = new Size(416, 93);
            tb_serverMessage.TabIndex = 14;
            // 
            // serverCountryCode
            // 
            serverCountryCode.Dock = DockStyle.Fill;
            serverCountryCode.Font = new Font("Segoe UI", 8F);
            serverCountryCode.Location = new Point(353, 3);
            serverCountryCode.MaxLength = 2;
            serverCountryCode.Name = "serverCountryCode";
            serverCountryCode.PlaceholderText = "US";
            serverCountryCode.Size = new Size(66, 22);
            serverCountryCode.TabIndex = 13;
            serverCountryCode.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_serverName
            // 
            tb_serverName.Dock = DockStyle.Fill;
            tb_serverName.Location = new Point(178, 3);
            tb_serverName.MaxLength = 26;
            tb_serverName.Name = "tb_serverName";
            tb_serverName.PlaceholderText = "Lobby Name";
            tb_serverName.Size = new Size(169, 23);
            tb_serverName.TabIndex = 3;
            tb_serverName.TextAlign = HorizontalAlignment.Center;
            // 
            // tb_hostName
            // 
            tb_hostName.Dock = DockStyle.Fill;
            tb_hostName.Location = new Point(3, 3);
            tb_hostName.MaxLength = 15;
            tb_hostName.Name = "tb_hostName";
            tb_hostName.PlaceholderText = "Host Name";
            tb_hostName.Size = new Size(169, 23);
            tb_hostName.TabIndex = 2;
            tb_hostName.TextAlign = HorizontalAlignment.Center;
            // 
            // tabProfilePage2
            // 
            tabProfilePage2.Controls.Add(groupBox1);
            tabProfilePage2.Controls.Add(groupBox2);
            tabProfilePage2.Controls.Add(groupBox3);
            tabProfilePage2.Location = new Point(4, 24);
            tabProfilePage2.Margin = new Padding(0);
            tabProfilePage2.Name = "tabProfilePage2";
            tabProfilePage2.Size = new Size(434, 388);
            tabProfilePage2.TabIndex = 1;
            tabProfilePage2.Text = "Start Modifiers";
            tabProfilePage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 58);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(434, 112);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Custom Mod File Path";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tableLayoutPanel3.Controls.Add(tb_modFile, 0, 0);
            tableLayoutPanel3.Controls.Add(btn_profileBrowse2, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(428, 90);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // tb_modFile
            // 
            tb_modFile.Dock = DockStyle.Fill;
            tb_modFile.Enabled = false;
            tb_modFile.Location = new Point(3, 3);
            tb_modFile.Multiline = true;
            tb_modFile.Name = "tb_modFile";
            tb_modFile.Size = new Size(327, 84);
            tb_modFile.TabIndex = 6;
            // 
            // btn_profileBrowse2
            // 
            btn_profileBrowse2.Dock = DockStyle.Fill;
            btn_profileBrowse2.Enabled = false;
            btn_profileBrowse2.FlatStyle = FlatStyle.Flat;
            btn_profileBrowse2.Location = new Point(336, 3);
            btn_profileBrowse2.Name = "btn_profileBrowse2";
            btn_profileBrowse2.Size = new Size(89, 84);
            btn_profileBrowse2.TabIndex = 5;
            btn_profileBrowse2.Text = "Browse";
            btn_profileBrowse2.UseVisualStyleBackColor = true;
            btn_profileBrowse2.Click += actionClick_profileOpenFileDialog;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel2);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(434, 58);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Game Server Path";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tableLayoutPanel2.Controls.Add(btn_profileBrowse1, 1, 0);
            tableLayoutPanel2.Controls.Add(tb_profileServerPath, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(428, 36);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_profileBrowse1
            // 
            btn_profileBrowse1.Dock = DockStyle.Fill;
            btn_profileBrowse1.FlatStyle = FlatStyle.Flat;
            btn_profileBrowse1.Location = new Point(336, 3);
            btn_profileBrowse1.Name = "btn_profileBrowse1";
            btn_profileBrowse1.Size = new Size(89, 30);
            btn_profileBrowse1.TabIndex = 2;
            btn_profileBrowse1.Text = "Browse";
            btn_profileBrowse1.UseVisualStyleBackColor = true;
            btn_profileBrowse1.Click += actionClick_profileOpenFolderDialog;
            // 
            // tb_profileServerPath
            // 
            tb_profileServerPath.Dock = DockStyle.Fill;
            tb_profileServerPath.Enabled = false;
            tb_profileServerPath.Location = new Point(3, 3);
            tb_profileServerPath.Multiline = true;
            tb_profileServerPath.Name = "tb_profileServerPath";
            tb_profileServerPath.Size = new Size(327, 30);
            tb_profileServerPath.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableAtributes);
            groupBox3.Dock = DockStyle.Bottom;
            groupBox3.Location = new Point(0, 170);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(434, 218);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Application Commandline Switches";
            // 
            // tableAtributes
            // 
            tableAtributes.ColumnCount = 3;
            tableAtributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableAtributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableAtributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableAtributes.Controls.Add(profileServerAttribute01, 0, 0);
            tableAtributes.Controls.Add(profileServerAttribute02, 1, 0);
            tableAtributes.Controls.Add(profileServerAttribute03, 2, 0);
            tableAtributes.Controls.Add(profileServerAttribute04, 0, 1);
            tableAtributes.Controls.Add(profileServerAttribute05, 1, 1);
            tableAtributes.Controls.Add(profileServerAttribute06, 2, 1);
            tableAtributes.Controls.Add(profileServerAttribute07, 0, 2);
            tableAtributes.Controls.Add(profileServerAttribute08, 1, 2);
            tableAtributes.Controls.Add(profileServerAttribute09, 2, 2);
            tableAtributes.Controls.Add(profileServerAttribute10, 0, 3);
            tableAtributes.Controls.Add(profileServerAttribute11, 1, 3);
            tableAtributes.Controls.Add(profileServerAttribute12, 2, 3);
            tableAtributes.Controls.Add(profileServerAttribute13, 0, 4);
            tableAtributes.Controls.Add(profileServerAttribute14, 1, 4);
            tableAtributes.Controls.Add(profileServerAttribute15, 2, 4);
            tableAtributes.Controls.Add(profileServerAttribute16, 0, 5);
            tableAtributes.Controls.Add(profileServerAttribute17, 1, 5);
            tableAtributes.Controls.Add(profileServerAttribute18, 2, 5);
            tableAtributes.Controls.Add(profileServerAttribute19, 0, 6);
            tableAtributes.Controls.Add(profileServerAttribute20, 1, 6);
            tableAtributes.Controls.Add(profileServerAttribute21, 2, 6);
            tableAtributes.Dock = DockStyle.Fill;
            tableAtributes.Location = new Point(3, 19);
            tableAtributes.Margin = new Padding(0);
            tableAtributes.Name = "tableAtributes";
            tableAtributes.RowCount = 7;
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableAtributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableAtributes.Size = new Size(428, 196);
            tableAtributes.TabIndex = 0;
            // 
            // profileServerAttribute01
            // 
            profileServerAttribute01.AutoSize = true;
            profileServerAttribute01.Dock = DockStyle.Fill;
            profileServerAttribute01.Font = new Font("Segoe UI", 9F);
            profileServerAttribute01.Location = new Point(10, 0);
            profileServerAttribute01.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute01.Name = "profileServerAttribute01";
            profileServerAttribute01.Size = new Size(132, 28);
            profileServerAttribute01.TabIndex = 0;
            profileServerAttribute01.Text = "/DUMP";
            profileServerAttribute01.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute02
            // 
            profileServerAttribute02.AutoSize = true;
            profileServerAttribute02.Dock = DockStyle.Fill;
            profileServerAttribute02.Font = new Font("Segoe UI", 9F);
            profileServerAttribute02.Location = new Point(147, 0);
            profileServerAttribute02.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute02.Name = "profileServerAttribute02";
            profileServerAttribute02.Size = new Size(137, 28);
            profileServerAttribute02.TabIndex = 1;
            profileServerAttribute02.Text = "/WDM";
            profileServerAttribute02.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute03
            // 
            profileServerAttribute03.AutoSize = true;
            profileServerAttribute03.Dock = DockStyle.Fill;
            profileServerAttribute03.Font = new Font("Segoe UI", 9F);
            profileServerAttribute03.Location = new Point(294, 0);
            profileServerAttribute03.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute03.Name = "profileServerAttribute03";
            profileServerAttribute03.Size = new Size(134, 28);
            profileServerAttribute03.TabIndex = 0;
            profileServerAttribute03.Text = "/FRISK";
            profileServerAttribute03.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute04
            // 
            profileServerAttribute04.AutoSize = true;
            profileServerAttribute04.Dock = DockStyle.Fill;
            profileServerAttribute04.Font = new Font("Segoe UI", 9F);
            profileServerAttribute04.Location = new Point(10, 28);
            profileServerAttribute04.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute04.Name = "profileServerAttribute04";
            profileServerAttribute04.Size = new Size(132, 28);
            profileServerAttribute04.TabIndex = 0;
            profileServerAttribute04.Text = "/mod";
            profileServerAttribute04.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute05
            // 
            profileServerAttribute05.AutoSize = true;
            profileServerAttribute05.Dock = DockStyle.Fill;
            profileServerAttribute05.Font = new Font("Segoe UI", 9F);
            profileServerAttribute05.Location = new Point(147, 28);
            profileServerAttribute05.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute05.Name = "profileServerAttribute05";
            profileServerAttribute05.Size = new Size(137, 28);
            profileServerAttribute05.TabIndex = 0;
            profileServerAttribute05.Text = "/betamp";
            profileServerAttribute05.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute06
            // 
            profileServerAttribute06.AutoSize = true;
            profileServerAttribute06.Dock = DockStyle.Fill;
            profileServerAttribute06.Font = new Font("Segoe UI", 9F);
            profileServerAttribute06.Location = new Point(294, 28);
            profileServerAttribute06.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute06.Name = "profileServerAttribute06";
            profileServerAttribute06.Size = new Size(134, 28);
            profileServerAttribute06.TabIndex = 0;
            profileServerAttribute06.Text = "/noreload";
            profileServerAttribute06.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute07
            // 
            profileServerAttribute07.AutoSize = true;
            profileServerAttribute07.Checked = true;
            profileServerAttribute07.CheckState = CheckState.Checked;
            profileServerAttribute07.Dock = DockStyle.Fill;
            profileServerAttribute07.Font = new Font("Segoe UI", 9F);
            profileServerAttribute07.Location = new Point(10, 56);
            profileServerAttribute07.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute07.Name = "profileServerAttribute07";
            profileServerAttribute07.Size = new Size(132, 28);
            profileServerAttribute07.TabIndex = 0;
            profileServerAttribute07.Text = "/w";
            profileServerAttribute07.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute08
            // 
            profileServerAttribute08.AutoSize = true;
            profileServerAttribute08.Checked = true;
            profileServerAttribute08.CheckState = CheckState.Checked;
            profileServerAttribute08.Dock = DockStyle.Fill;
            profileServerAttribute08.Font = new Font("Segoe UI", 9F);
            profileServerAttribute08.Location = new Point(147, 56);
            profileServerAttribute08.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute08.Name = "profileServerAttribute08";
            profileServerAttribute08.Size = new Size(137, 28);
            profileServerAttribute08.TabIndex = 0;
            profileServerAttribute08.Text = "/autorestart";
            profileServerAttribute08.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute09
            // 
            profileServerAttribute09.AutoSize = true;
            profileServerAttribute09.Dock = DockStyle.Fill;
            profileServerAttribute09.Font = new Font("Segoe UI", 9F);
            profileServerAttribute09.Location = new Point(294, 56);
            profileServerAttribute09.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute09.Name = "profileServerAttribute09";
            profileServerAttribute09.Size = new Size(134, 28);
            profileServerAttribute09.TabIndex = 0;
            profileServerAttribute09.Text = "/L";
            profileServerAttribute09.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute10
            // 
            profileServerAttribute10.AutoSize = true;
            profileServerAttribute10.Dock = DockStyle.Fill;
            profileServerAttribute10.Font = new Font("Segoe UI", 9F);
            profileServerAttribute10.Location = new Point(10, 84);
            profileServerAttribute10.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute10.Name = "profileServerAttribute10";
            profileServerAttribute10.Size = new Size(132, 28);
            profileServerAttribute10.TabIndex = 0;
            profileServerAttribute10.Text = "/BADPACKETS";
            profileServerAttribute10.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute11
            // 
            profileServerAttribute11.AutoSize = true;
            profileServerAttribute11.Dock = DockStyle.Fill;
            profileServerAttribute11.Font = new Font("Segoe UI", 9F);
            profileServerAttribute11.Location = new Point(147, 84);
            profileServerAttribute11.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute11.Name = "profileServerAttribute11";
            profileServerAttribute11.Size = new Size(137, 28);
            profileServerAttribute11.TabIndex = 0;
            profileServerAttribute11.Text = "/PUNT.TXT";
            profileServerAttribute11.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute12
            // 
            profileServerAttribute12.AutoSize = true;
            profileServerAttribute12.Dock = DockStyle.Fill;
            profileServerAttribute12.Font = new Font("Segoe UI", 9F);
            profileServerAttribute12.Location = new Point(294, 84);
            profileServerAttribute12.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute12.Name = "profileServerAttribute12";
            profileServerAttribute12.Size = new Size(134, 28);
            profileServerAttribute12.TabIndex = 0;
            profileServerAttribute12.Text = "/shadowmap";
            profileServerAttribute12.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute13
            // 
            profileServerAttribute13.AutoSize = true;
            profileServerAttribute13.Dock = DockStyle.Fill;
            profileServerAttribute13.Font = new Font("Segoe UI", 9F);
            profileServerAttribute13.Location = new Point(10, 112);
            profileServerAttribute13.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute13.Name = "profileServerAttribute13";
            profileServerAttribute13.Size = new Size(132, 28);
            profileServerAttribute13.TabIndex = 0;
            profileServerAttribute13.Text = "/s";
            profileServerAttribute13.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute14
            // 
            profileServerAttribute14.AutoSize = true;
            profileServerAttribute14.Dock = DockStyle.Fill;
            profileServerAttribute14.Font = new Font("Segoe UI", 9F);
            profileServerAttribute14.Location = new Point(147, 112);
            profileServerAttribute14.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute14.Name = "profileServerAttribute14";
            profileServerAttribute14.Size = new Size(137, 28);
            profileServerAttribute14.TabIndex = 0;
            profileServerAttribute14.Text = "/NOSTAT";
            profileServerAttribute14.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute15
            // 
            profileServerAttribute15.AutoSize = true;
            profileServerAttribute15.Dock = DockStyle.Fill;
            profileServerAttribute15.Font = new Font("Segoe UI", 9F);
            profileServerAttribute15.Location = new Point(294, 112);
            profileServerAttribute15.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute15.Name = "profileServerAttribute15";
            profileServerAttribute15.Size = new Size(134, 28);
            profileServerAttribute15.TabIndex = 0;
            profileServerAttribute15.Text = "/NETLOG";
            profileServerAttribute15.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute16
            // 
            profileServerAttribute16.AutoSize = true;
            profileServerAttribute16.Dock = DockStyle.Fill;
            profileServerAttribute16.Font = new Font("Segoe UI", 9F);
            profileServerAttribute16.Location = new Point(10, 140);
            profileServerAttribute16.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute16.Name = "profileServerAttribute16";
            profileServerAttribute16.Size = new Size(132, 28);
            profileServerAttribute16.TabIndex = 0;
            profileServerAttribute16.Text = "/NETFLOW";
            profileServerAttribute16.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute17
            // 
            profileServerAttribute17.AutoSize = true;
            profileServerAttribute17.Dock = DockStyle.Fill;
            profileServerAttribute17.Font = new Font("Segoe UI", 9F);
            profileServerAttribute17.Location = new Point(147, 140);
            profileServerAttribute17.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute17.Name = "profileServerAttribute17";
            profileServerAttribute17.Size = new Size(137, 28);
            profileServerAttribute17.TabIndex = 0;
            profileServerAttribute17.Text = "/SYSDUMP";
            profileServerAttribute17.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute18
            // 
            profileServerAttribute18.AutoSize = true;
            profileServerAttribute18.Checked = true;
            profileServerAttribute18.CheckState = CheckState.Checked;
            profileServerAttribute18.Dock = DockStyle.Fill;
            profileServerAttribute18.Font = new Font("Segoe UI", 9F);
            profileServerAttribute18.Location = new Point(294, 140);
            profileServerAttribute18.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute18.Name = "profileServerAttribute18";
            profileServerAttribute18.Size = new Size(134, 28);
            profileServerAttribute18.TabIndex = 0;
            profileServerAttribute18.Text = "/NOSYSDUMP";
            profileServerAttribute18.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute19
            // 
            profileServerAttribute19.AutoSize = true;
            profileServerAttribute19.Dock = DockStyle.Fill;
            profileServerAttribute19.Font = new Font("Segoe UI", 9F);
            profileServerAttribute19.Location = new Point(10, 168);
            profileServerAttribute19.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute19.Name = "profileServerAttribute19";
            profileServerAttribute19.Size = new Size(132, 28);
            profileServerAttribute19.TabIndex = 0;
            profileServerAttribute19.Text = "/STACKTRACE";
            profileServerAttribute19.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute20
            // 
            profileServerAttribute20.AutoSize = true;
            profileServerAttribute20.Dock = DockStyle.Fill;
            profileServerAttribute20.Font = new Font("Segoe UI", 9F);
            profileServerAttribute20.Location = new Point(147, 168);
            profileServerAttribute20.Margin = new Padding(5, 0, 0, 0);
            profileServerAttribute20.Name = "profileServerAttribute20";
            profileServerAttribute20.Size = new Size(137, 28);
            profileServerAttribute20.TabIndex = 0;
            profileServerAttribute20.Text = "/NOSTACKTRACE";
            profileServerAttribute20.UseVisualStyleBackColor = true;
            // 
            // profileServerAttribute21
            // 
            profileServerAttribute21.AutoSize = true;
            profileServerAttribute21.Checked = true;
            profileServerAttribute21.CheckState = CheckState.Checked;
            profileServerAttribute21.Dock = DockStyle.Fill;
            profileServerAttribute21.Font = new Font("Segoe UI", 9F);
            profileServerAttribute21.Location = new Point(294, 168);
            profileServerAttribute21.Margin = new Padding(10, 0, 0, 0);
            profileServerAttribute21.Name = "profileServerAttribute21";
            profileServerAttribute21.Size = new Size(134, 28);
            profileServerAttribute21.TabIndex = 0;
            profileServerAttribute21.Text = "/LOADBAR";
            profileServerAttribute21.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox5);
            panel1.Controls.Add(groupBox4);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(512, 416);
            panel1.TabIndex = 1;
            // 
            // groupBox5
            // 
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(0, 0);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(512, 358);
            groupBox5.TabIndex = 3;
            groupBox5.TabStop = false;
            groupBox5.Text = "Logs";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel4);
            groupBox4.Dock = DockStyle.Bottom;
            groupBox4.Location = new Point(0, 358);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(512, 58);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "Controls";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 5;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Controls.Add(btn_resetProfile, 3, 0);
            tableLayoutPanel4.Controls.Add(btn_saveProfile, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(506, 36);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // btn_resetProfile
            // 
            btn_resetProfile.Dock = DockStyle.Fill;
            btn_resetProfile.Location = new Point(306, 3);
            btn_resetProfile.Name = "btn_resetProfile";
            btn_resetProfile.Size = new Size(95, 30);
            btn_resetProfile.TabIndex = 1;
            btn_resetProfile.Text = "Reset";
            btn_resetProfile.UseVisualStyleBackColor = true;
            btn_resetProfile.Click += actionClick_ResetProfile;
            // 
            // btn_saveProfile
            // 
            btn_saveProfile.Dock = DockStyle.Fill;
            btn_saveProfile.Location = new Point(104, 3);
            btn_saveProfile.Name = "btn_saveProfile";
            btn_saveProfile.Size = new Size(95, 30);
            btn_saveProfile.TabIndex = 0;
            btn_saveProfile.Text = "Save";
            btn_saveProfile.UseVisualStyleBackColor = true;
            btn_saveProfile.Click += actionClick_SaveProfile;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openModFileDialog";
            // 
            // tabProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "tabProfile";
            Size = new Size(966, 422);
            tableLayoutPanel1.ResumeLayout(false);
            profileFileManagers.ResumeLayout(false);
            tabProfilePage1.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_maxPing).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_minPing).EndInit();
            groupBox7.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_serverPort).EndInit();
            groupBox6.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tabProfilePage2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableAtributes.ResumeLayout(false);
            tableAtributes.PerformLayout();
            panel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TabControl profileFileManagers;
        private TabPage tabProfilePage1;
        private TabPage tabProfilePage2;
        private Button btn_resetProfile;
        private Button btn_saveProfile;
        private TextBox tb_profileServerPath;
        private Button btn_profileBrowse1;
        private Button btn_profileBrowse2;
        private FolderBrowserDialog folderProfileBrowserDialog;
        private OpenFileDialog openFileDialog1;
        private GroupBox groupBox3;
        private TableLayoutPanel tableAtributes;
        public CheckBox profileServerAttribute01;
        public CheckBox profileServerAttribute02;
        public CheckBox profileServerAttribute03;
        public CheckBox profileServerAttribute04;
        public CheckBox profileServerAttribute05;
        public CheckBox profileServerAttribute06;
        public CheckBox profileServerAttribute07;
        public CheckBox profileServerAttribute08;
        public CheckBox profileServerAttribute09;
        public CheckBox profileServerAttribute10;
        public CheckBox profileServerAttribute11;
        public CheckBox profileServerAttribute12;
        public CheckBox profileServerAttribute13;
        public CheckBox profileServerAttribute14;
        public CheckBox profileServerAttribute15;
        public CheckBox profileServerAttribute16;
        public CheckBox profileServerAttribute17;
        public CheckBox profileServerAttribute18;
        public CheckBox profileServerAttribute19;
        public CheckBox profileServerAttribute20;
        public CheckBox profileServerAttribute21;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox tb_modFile;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBox5;
        private GroupBox groupBox8;
        private GroupBox groupBox7;
        private GroupBox groupBox6;
        private TableLayoutPanel tableLayoutPanel5;
        public TextBox tb_hostName;
        public TextBox tb_serverName;
        public TextBox serverCountryCode;
        public TextBox tb_serverMessage;
        private TableLayoutPanel tableLayoutPanel6;
        public NumericUpDown num_serverPort;
        public ComboBox cb_serverIP;
        private Label label1;
        private Label label3;
        public TextBox tb_serverPassword;
        public CheckBox cb_requireNova;
        public CheckBox cb_serverDedicated;
        private TableLayoutPanel tableLayoutPanel7;
        public NumericUpDown num_maxPing;
        public CheckBox cb_enableMinCheck;
        public CheckBox cb_enableMaxCheck;
        public NumericUpDown num_minPing;
    }
}
