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
            tabPlayerStats = new TabPage();
            tabWeaponStats = new TabPage();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            btn_validate = new Button();
            groupBox2 = new GroupBox();
            cb_enableWebStats = new CheckBox();
            cb_enableAnnouncements = new CheckBox();
            groupBox3 = new GroupBox();
            num_WebStatsReport = new NumericUpDown();
            labelReportInterval = new Label();
            labelUpatedInterval = new Label();
            num_WebStatsUpdates = new NumericUpDown();
            groupBox4 = new GroupBox();
            btn_SaveSettings = new Button();
            tabControl1.SuspendLayout();
            tabBabstats.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dg_statsLog).BeginInit();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).BeginInit();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabBabstats);
            tabControl1.Controls.Add(tabPlayerStats);
            tabControl1.Controls.Add(tabWeaponStats);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(902, 362);
            tabControl1.TabIndex = 0;
            // 
            // tabBabstats
            // 
            tabBabstats.Controls.Add(tableLayoutPanel1);
            tabBabstats.Location = new Point(4, 24);
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
            // tabPlayerStats
            // 
            tabPlayerStats.Location = new Point(4, 24);
            tabPlayerStats.Name = "tabPlayerStats";
            tabPlayerStats.Padding = new Padding(3);
            tabPlayerStats.Size = new Size(894, 334);
            tabPlayerStats.TabIndex = 1;
            tabPlayerStats.Text = "Player Stats";
            tabPlayerStats.UseVisualStyleBackColor = true;
            // 
            // tabWeaponStats
            // 
            tabWeaponStats.Location = new Point(4, 24);
            tabWeaponStats.Name = "tabWeaponStats";
            tabWeaponStats.Size = new Size(894, 334);
            tabWeaponStats.TabIndex = 2;
            tabWeaponStats.Text = "Weapon Stats";
            tabWeaponStats.UseVisualStyleBackColor = true;
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
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_validate);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(289, 84);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Stats Web Address (HTTP/HTTPS)";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 22);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "http(s)://youdomain.com/babstats_root/";
            textBox1.Size = new Size(277, 23);
            textBox1.TabIndex = 1;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // btn_validate
            // 
            btn_validate.Location = new Point(208, 51);
            btn_validate.Name = "btn_validate";
            btn_validate.Size = new Size(75, 23);
            btn_validate.TabIndex = 4;
            btn_validate.Text = "Validate";
            btn_validate.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cb_enableAnnouncements);
            groupBox2.Controls.Add(cb_enableWebStats);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 84);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(289, 53);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Options";
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
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(labelUpatedInterval);
            groupBox3.Controls.Add(num_WebStatsUpdates);
            groupBox3.Controls.Add(labelReportInterval);
            groupBox3.Controls.Add(num_WebStatsReport);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(0, 137);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(289, 85);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Reporting Intervals (Seconds)";
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
            // groupBox4
            // 
            groupBox4.Controls.Add(btn_SaveSettings);
            groupBox4.Dock = DockStyle.Bottom;
            groupBox4.Location = new Point(0, 271);
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
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsReport).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_WebStatsUpdates).EndInit();
            groupBox4.ResumeLayout(false);
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
        private TextBox textBox1;
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
    }
}
