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
            tabProfilePage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            gp_profileSettings = new GroupBox();
            panel1 = new Panel();
            btn_resetProfile = new Button();
            btn_saveProfile = new Button();
            tb_profileServerPath = new TextBox();
            label_profileServerPath = new Label();
            btn_profileBrowse1 = new Button();
            label1 = new Label();
            tb_modFile = new TextBox();
            btn_profileBrowse2 = new Button();
            cb_profileModifierList1 = new CheckedListBox();
            folderProfileBrowserDialog = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            cb_profileModifierList2 = new CheckedListBox();
            label_NotInUse1 = new Label();
            label2 = new Label();
            tableLayoutPanel1.SuspendLayout();
            profileFileManagers.SuspendLayout();
            tabProfilePage1.SuspendLayout();
            tabProfilePage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            gp_profileSettings.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Controls.Add(profileFileManagers, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(902, 362);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // profileFileManagers
            // 
            profileFileManagers.Controls.Add(tabProfilePage1);
            profileFileManagers.Controls.Add(tabProfilePage2);
            profileFileManagers.Dock = DockStyle.Fill;
            profileFileManagers.Location = new Point(363, 3);
            profileFileManagers.Name = "profileFileManagers";
            profileFileManagers.SelectedIndex = 0;
            profileFileManagers.Size = new Size(536, 356);
            profileFileManagers.TabIndex = 0;
            // 
            // tabProfilePage1
            // 
            tabProfilePage1.Controls.Add(label_NotInUse1);
            tabProfilePage1.Location = new Point(4, 24);
            tabProfilePage1.Name = "tabProfilePage1";
            tabProfilePage1.Padding = new Padding(3);
            tabProfilePage1.Size = new Size(528, 328);
            tabProfilePage1.TabIndex = 0;
            tabProfilePage1.Text = "HawkSync";
            tabProfilePage1.UseVisualStyleBackColor = true;
            // 
            // tabProfilePage2
            // 
            tabProfilePage2.Controls.Add(label2);
            tabProfilePage2.Location = new Point(4, 24);
            tabProfilePage2.Name = "tabProfilePage2";
            tabProfilePage2.Padding = new Padding(3);
            tabProfilePage2.Size = new Size(528, 328);
            tabProfilePage2.TabIndex = 1;
            tabProfilePage2.Text = "Game Server";
            tabProfilePage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(gp_profileSettings, 0, 0);
            tableLayoutPanel2.Controls.Add(panel1, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 89.60674F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10.3932581F));
            tableLayoutPanel2.Size = new Size(354, 356);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // gp_profileSettings
            // 
            gp_profileSettings.Controls.Add(cb_profileModifierList2);
            gp_profileSettings.Controls.Add(cb_profileModifierList1);
            gp_profileSettings.Controls.Add(btn_profileBrowse2);
            gp_profileSettings.Controls.Add(tb_modFile);
            gp_profileSettings.Controls.Add(label1);
            gp_profileSettings.Controls.Add(btn_profileBrowse1);
            gp_profileSettings.Controls.Add(label_profileServerPath);
            gp_profileSettings.Controls.Add(tb_profileServerPath);
            gp_profileSettings.Dock = DockStyle.Fill;
            gp_profileSettings.Location = new Point(3, 3);
            gp_profileSettings.Name = "gp_profileSettings";
            gp_profileSettings.Size = new Size(348, 313);
            gp_profileSettings.TabIndex = 0;
            gp_profileSettings.TabStop = false;
            gp_profileSettings.Text = "Profile Settings";
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_resetProfile);
            panel1.Controls.Add(btn_saveProfile);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 322);
            panel1.Name = "panel1";
            panel1.Size = new Size(348, 31);
            panel1.TabIndex = 1;
            // 
            // btn_resetProfile
            // 
            btn_resetProfile.Location = new Point(84, 4);
            btn_resetProfile.Name = "btn_resetProfile";
            btn_resetProfile.Size = new Size(75, 23);
            btn_resetProfile.TabIndex = 1;
            btn_resetProfile.Text = "Reset";
            btn_resetProfile.UseVisualStyleBackColor = true;
            // 
            // btn_saveProfile
            // 
            btn_saveProfile.Location = new Point(3, 4);
            btn_saveProfile.Name = "btn_saveProfile";
            btn_saveProfile.Size = new Size(75, 23);
            btn_saveProfile.TabIndex = 0;
            btn_saveProfile.Text = "Save";
            btn_saveProfile.UseVisualStyleBackColor = true;
            btn_saveProfile.Click += actionClick_SaveProfile;
            // 
            // tb_profileServerPath
            // 
            tb_profileServerPath.Location = new Point(6, 20);
            tb_profileServerPath.Name = "tb_profileServerPath";
            tb_profileServerPath.Size = new Size(255, 23);
            tb_profileServerPath.TabIndex = 0;
            // 
            // label_profileServerPath
            // 
            label_profileServerPath.AutoSize = true;
            label_profileServerPath.Location = new Point(6, 46);
            label_profileServerPath.Name = "label_profileServerPath";
            label_profileServerPath.Size = new Size(100, 15);
            label_profileServerPath.TabIndex = 1;
            label_profileServerPath.Text = "Game Server Path";
            // 
            // btn_profileBrowse1
            // 
            btn_profileBrowse1.FlatStyle = FlatStyle.Flat;
            btn_profileBrowse1.Location = new Point(267, 20);
            btn_profileBrowse1.Name = "btn_profileBrowse1";
            btn_profileBrowse1.Size = new Size(75, 23);
            btn_profileBrowse1.TabIndex = 2;
            btn_profileBrowse1.Text = "Browse";
            btn_profileBrowse1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(208, 56);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 3;
            label1.Text = "Mod File";
            // 
            // tb_modFile
            // 
            tb_modFile.Location = new Point(6, 74);
            tb_modFile.Name = "tb_modFile";
            tb_modFile.Size = new Size(255, 23);
            tb_modFile.TabIndex = 4;
            // 
            // btn_profileBrowse2
            // 
            btn_profileBrowse2.FlatStyle = FlatStyle.Flat;
            btn_profileBrowse2.Location = new Point(267, 74);
            btn_profileBrowse2.Name = "btn_profileBrowse2";
            btn_profileBrowse2.Size = new Size(75, 23);
            btn_profileBrowse2.TabIndex = 5;
            btn_profileBrowse2.Text = "Browse";
            btn_profileBrowse2.UseVisualStyleBackColor = true;
            // 
            // cb_profileModifierList1
            // 
            cb_profileModifierList1.FormattingEnabled = true;
            cb_profileModifierList1.Items.AddRange(new object[] { "/D", "/WDM", "/FRISK", "/mod", "/betamp", "/noreload", "/w", "/autorestart", "/L", "/BADPACKETS", "/PUNT.TXT" });
            cb_profileModifierList1.Location = new Point(6, 103);
            cb_profileModifierList1.Name = "cb_profileModifierList1";
            cb_profileModifierList1.Size = new Size(165, 202);
            cb_profileModifierList1.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openModFileDialog";
            // 
            // cb_profileModifierList2
            // 
            cb_profileModifierList2.FormattingEnabled = true;
            cb_profileModifierList2.Items.AddRange(new object[] { "/shadowmap", "/s", "/NOSTAT", "/NETLOG", "/NETFLOW", "/SYSDUMP", "/NOSYSDUMP", "/STACKTRACE", "/NOSTACKTRACE", "/LOADBAR", "/DUMP" });
            cb_profileModifierList2.Location = new Point(177, 103);
            cb_profileModifierList2.Name = "cb_profileModifierList2";
            cb_profileModifierList2.Size = new Size(165, 202);
            cb_profileModifierList2.TabIndex = 7;
            // 
            // label_NotInUse1
            // 
            label_NotInUse1.AutoSize = true;
            label_NotInUse1.Location = new Point(164, 181);
            label_NotInUse1.Name = "label_NotInUse1";
            label_NotInUse1.Size = new Size(214, 15);
            label_NotInUse1.TabIndex = 0;
            label_NotInUse1.Text = "File Manager Feature Not Implemented";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(164, 181);
            label2.Name = "label2";
            label2.Size = new Size(214, 15);
            label2.TabIndex = 1;
            label2.Text = "File Manager Feature Not Implemented";
            // 
            // tabProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabProfile";
            Size = new Size(902, 362);
            tableLayoutPanel1.ResumeLayout(false);
            profileFileManagers.ResumeLayout(false);
            tabProfilePage1.ResumeLayout(false);
            tabProfilePage1.PerformLayout();
            tabProfilePage2.ResumeLayout(false);
            tabProfilePage2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            gp_profileSettings.ResumeLayout(false);
            gp_profileSettings.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TabControl profileFileManagers;
        private TabPage tabProfilePage1;
        private TabPage tabProfilePage2;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox gp_profileSettings;
        private Panel panel1;
        private Button btn_resetProfile;
        private Button btn_saveProfile;
        private Label label_profileServerPath;
        private TextBox tb_profileServerPath;
        private Button btn_profileBrowse1;
        private Button btn_profileBrowse2;
        private TextBox tb_modFile;
        private Label label1;
        private CheckedListBox cb_profileModifierList1;
        private CheckedListBox cb_profileModifierList2;
        private FolderBrowserDialog folderProfileBrowserDialog;
        private OpenFileDialog openFileDialog1;
        private Label label_NotInUse1;
        private Label label2;
    }
}
