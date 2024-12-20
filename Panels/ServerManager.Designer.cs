using System.Drawing;
using ServerManager.Classes.Objects;

namespace ServerManager.Panels
{
    partial class ServerManager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManager));
            this.panel_notificationBar = new System.Windows.Forms.Panel();
            this.tabControl_ProfileList = new System.Windows.Forms.TabControl();
            this.tab_Profiles = new System.Windows.Forms.TabPage();
            this.groupBox_profiles = new System.Windows.Forms.GroupBox();
            this.dataGrid_profiles = new System.Windows.Forms.DataGridView();
            this.profileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusByte = new System.Windows.Forms.DataGridViewImageColumn();
            this.serverPlayerCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentMap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentMapGameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentTimer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverStatStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profileOpenBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel_profileControls = new System.Windows.Forms.Panel();
            this.groupBox_legend = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox5_scoring = new System.Windows.Forms.PictureBox();
            this.label_scoring = new System.Windows.Forms.Label();
            this.label_notHosting = new System.Windows.Forms.Label();
            this.pictureBox4_legend = new System.Windows.Forms.PictureBox();
            this.label_active = new System.Windows.Forms.Label();
            this.pictureBox3_legend = new System.Windows.Forms.PictureBox();
            this.label_loading = new System.Windows.Forms.Label();
            this.pictureBox2_legend = new System.Windows.Forms.PictureBox();
            this.label_notActive = new System.Windows.Forms.Label();
            this.groupBox_options = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tab_Options = new System.Windows.Forms.TabPage();
            this.tab_Mods = new System.Windows.Forms.TabPage();
            this.tab_Plugins = new System.Windows.Forms.TabPage();
            this.tab_Updates = new System.Windows.Forms.TabPage();
            this.tab_Admin = new System.Windows.Forms.TabPage();
            this.tab_Help = new System.Windows.Forms.TabPage();
            this.tab_About = new System.Windows.Forms.TabPage();
            this.tabControl_ProfileList.SuspendLayout();
            this.tab_Profiles.SuspendLayout();
            this.groupBox_profiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_profiles)).BeginInit();
            this.panel_profileControls.SuspendLayout();
            this.groupBox_legend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5_scoring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4_legend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3_legend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2_legend)).BeginInit();
            this.groupBox_options.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_notificationBar
            // 
            this.panel_notificationBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_notificationBar.Location = new System.Drawing.Point(10, 488);
            this.panel_notificationBar.Margin = new System.Windows.Forms.Padding(0);
            this.panel_notificationBar.Name = "panel_notificationBar";
            this.panel_notificationBar.Size = new System.Drawing.Size(1033, 49);
            this.panel_notificationBar.TabIndex = 0;
            // 
            // tabControl_ProfileList
            // 
            this.tabControl_ProfileList.Controls.Add(this.tab_Profiles);
            this.tabControl_ProfileList.Controls.Add(this.tab_Options);
            this.tabControl_ProfileList.Controls.Add(this.tab_Mods);
            this.tabControl_ProfileList.Controls.Add(this.tab_Plugins);
            this.tabControl_ProfileList.Controls.Add(this.tab_Updates);
            this.tabControl_ProfileList.Controls.Add(this.tab_Admin);
            this.tabControl_ProfileList.Controls.Add(this.tab_Help);
            this.tabControl_ProfileList.Controls.Add(this.tab_About);
            this.tabControl_ProfileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_ProfileList.HotTrack = true;
            this.tabControl_ProfileList.ItemSize = new System.Drawing.Size(75, 30);
            this.tabControl_ProfileList.Location = new System.Drawing.Point(10, 5);
            this.tabControl_ProfileList.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl_ProfileList.Name = "tabControl_ProfileList";
            this.tabControl_ProfileList.Padding = new System.Drawing.Point(10, 5);
            this.tabControl_ProfileList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl_ProfileList.SelectedIndex = 0;
            this.tabControl_ProfileList.Size = new System.Drawing.Size(1033, 483);
            this.tabControl_ProfileList.TabIndex = 1;
            // 
            // tab_Profiles
            // 
            this.tab_Profiles.Controls.Add(this.groupBox_profiles);
            this.tab_Profiles.Controls.Add(this.panel_profileControls);
            this.tab_Profiles.Location = new System.Drawing.Point(4, 34);
            this.tab_Profiles.Margin = new System.Windows.Forms.Padding(4);
            this.tab_Profiles.Name = "tab_Profiles";
            this.tab_Profiles.Padding = new System.Windows.Forms.Padding(4);
            this.tab_Profiles.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tab_Profiles.Size = new System.Drawing.Size(1025, 445);
            this.tab_Profiles.TabIndex = 0;
            this.tab_Profiles.Text = "Profiles";
            this.tab_Profiles.UseVisualStyleBackColor = true;
            // 
            // groupBox_profiles
            // 
            this.groupBox_profiles.Controls.Add(this.dataGrid_profiles);
            this.groupBox_profiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_profiles.Location = new System.Drawing.Point(4, 4);
            this.groupBox_profiles.Name = "groupBox_profiles";
            this.groupBox_profiles.Padding = new System.Windows.Forms.Padding(7, 2, 7, 10);
            this.groupBox_profiles.Size = new System.Drawing.Size(1017, 337);
            this.groupBox_profiles.TabIndex = 1;
            this.groupBox_profiles.TabStop = false;
            this.groupBox_profiles.Text = "Profiles";
            // 
            // dataGrid_profiles
            // 
            this.dataGrid_profiles.AllowUserToAddRows = false;
            this.dataGrid_profiles.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_profiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid_profiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_profiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.profileName, this.statusByte, this.serverPlayerCount, this.currentMap, this.currentMapGameType, this.currentTimer, this.serverStatStatus, this.profileOpenBtn });
            this.dataGrid_profiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid_profiles.Location = new System.Drawing.Point(7, 18);
            this.dataGrid_profiles.Name = "dataGrid_profiles";
            this.dataGrid_profiles.ReadOnly = true;
            this.dataGrid_profiles.Size = new System.Drawing.Size(1003, 309);
            this.dataGrid_profiles.TabIndex = 0;
            this.dataGrid_profiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellClick);
            // 
            // profileName
            // 
            this.profileName.HeaderText = "Game Profile";
            this.profileName.MaxInputLength = 16;
            this.profileName.MinimumWidth = 150;
            this.profileName.Name = "profileName";
            this.profileName.ReadOnly = true;
            this.profileName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.profileName.Width = 150;
            // 
            // statusByte
            // 
            this.statusByte.HeaderText = "";
            this.statusByte.MinimumWidth = 50;
            this.statusByte.Name = "statusByte";
            this.statusByte.ReadOnly = true;
            this.statusByte.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.statusByte.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.statusByte.Width = 50;
            // 
            // serverPlayerCount
            // 
            this.serverPlayerCount.HeaderText = "Players";
            this.serverPlayerCount.Name = "serverPlayerCount";
            this.serverPlayerCount.ReadOnly = true;
            // 
            // currentMap
            // 
            this.currentMap.HeaderText = "Map";
            this.currentMap.Name = "currentMap";
            this.currentMap.ReadOnly = true;
            // 
            // currentMapGameType
            // 
            this.currentMapGameType.HeaderText = "GameType";
            this.currentMapGameType.Name = "currentMapGameType";
            this.currentMapGameType.ReadOnly = true;
            // 
            // currentTimer
            // 
            this.currentTimer.HeaderText = "Timer";
            this.currentTimer.Name = "currentTimer";
            this.currentTimer.ReadOnly = true;
            // 
            // serverStatStatus
            // 
            this.serverStatStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serverStatStatus.HeaderText = "Status";
            this.serverStatStatus.Name = "serverStatStatus";
            this.serverStatStatus.ReadOnly = true;
            // 
            // profileOpenBtn
            // 
            this.profileOpenBtn.HeaderText = "";
            this.profileOpenBtn.Name = "profileOpenBtn";
            this.profileOpenBtn.ReadOnly = true;
            // 
            // panel_profileControls
            // 
            this.panel_profileControls.Controls.Add(this.groupBox_legend);
            this.panel_profileControls.Controls.Add(this.groupBox_options);
            this.panel_profileControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_profileControls.Location = new System.Drawing.Point(4, 341);
            this.panel_profileControls.Name = "panel_profileControls";
            this.panel_profileControls.Size = new System.Drawing.Size(1017, 100);
            this.panel_profileControls.TabIndex = 0;
            // 
            // groupBox_legend
            // 
            this.groupBox_legend.Controls.Add(this.pictureBox1);
            this.groupBox_legend.Controls.Add(this.pictureBox5_scoring);
            this.groupBox_legend.Controls.Add(this.label_scoring);
            this.groupBox_legend.Controls.Add(this.label_notHosting);
            this.groupBox_legend.Controls.Add(this.pictureBox4_legend);
            this.groupBox_legend.Controls.Add(this.label_active);
            this.groupBox_legend.Controls.Add(this.pictureBox3_legend);
            this.groupBox_legend.Controls.Add(this.label_loading);
            this.groupBox_legend.Controls.Add(this.pictureBox2_legend);
            this.groupBox_legend.Controls.Add(this.label_notActive);
            this.groupBox_legend.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox_legend.Location = new System.Drawing.Point(742, 0);
            this.groupBox_legend.Name = "groupBox_legend";
            this.groupBox_legend.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_legend.Size = new System.Drawing.Size(275, 100);
            this.groupBox_legend.TabIndex = 1;
            this.groupBox_legend.TabStop = false;
            this.groupBox_legend.Text = "Legend";
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::ServerManager.Properties.Resources.notactive;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(13, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox5_scoring
            // 
            this.pictureBox5_scoring.ErrorImage = null;
            this.pictureBox5_scoring.Image = global::ServerManager.Properties.Resources.scoring;
            this.pictureBox5_scoring.InitialImage = null;
            this.pictureBox5_scoring.Location = new System.Drawing.Point(243, 45);
            this.pictureBox5_scoring.Name = "pictureBox5_scoring";
            this.pictureBox5_scoring.Size = new System.Drawing.Size(24, 24);
            this.pictureBox5_scoring.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox5_scoring.TabIndex = 9;
            this.pictureBox5_scoring.TabStop = false;
            // 
            // label_scoring
            // 
            this.label_scoring.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_scoring.Location = new System.Drawing.Point(133, 45);
            this.label_scoring.Name = "label_scoring";
            this.label_scoring.Size = new System.Drawing.Size(100, 24);
            this.label_scoring.TabIndex = 8;
            this.label_scoring.Text = "Scoring";
            this.label_scoring.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_notHosting
            // 
            this.label_notHosting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_notHosting.Location = new System.Drawing.Point(133, 69);
            this.label_notHosting.Name = "label_notHosting";
            this.label_notHosting.Size = new System.Drawing.Size(100, 24);
            this.label_notHosting.TabIndex = 7;
            this.label_notHosting.Text = "Not Hosting";
            this.label_notHosting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox4_legend
            // 
            this.pictureBox4_legend.ErrorImage = null;
            this.pictureBox4_legend.Image = global::ServerManager.Properties.Resources.nothosting;
            this.pictureBox4_legend.InitialImage = null;
            this.pictureBox4_legend.Location = new System.Drawing.Point(243, 69);
            this.pictureBox4_legend.Name = "pictureBox4_legend";
            this.pictureBox4_legend.Size = new System.Drawing.Size(24, 24);
            this.pictureBox4_legend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4_legend.TabIndex = 6;
            this.pictureBox4_legend.TabStop = false;
            // 
            // label_active
            // 
            this.label_active.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_active.Location = new System.Drawing.Point(43, 69);
            this.label_active.Name = "label_active";
            this.label_active.Size = new System.Drawing.Size(100, 24);
            this.label_active.TabIndex = 5;
            this.label_active.Text = "Active";
            this.label_active.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox3_legend
            // 
            this.pictureBox3_legend.ErrorImage = null;
            this.pictureBox3_legend.Image = global::ServerManager.Properties.Resources.hosting;
            this.pictureBox3_legend.InitialImage = null;
            this.pictureBox3_legend.Location = new System.Drawing.Point(13, 69);
            this.pictureBox3_legend.Name = "pictureBox3_legend";
            this.pictureBox3_legend.Size = new System.Drawing.Size(24, 24);
            this.pictureBox3_legend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3_legend.TabIndex = 4;
            this.pictureBox3_legend.TabStop = false;
            // 
            // label_loading
            // 
            this.label_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_loading.Location = new System.Drawing.Point(43, 45);
            this.label_loading.Name = "label_loading";
            this.label_loading.Size = new System.Drawing.Size(100, 24);
            this.label_loading.TabIndex = 3;
            this.label_loading.Text = "Loading";
            this.label_loading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2_legend
            // 
            this.pictureBox2_legend.ErrorImage = null;
            this.pictureBox2_legend.Image = global::ServerManager.Properties.Resources.loading;
            this.pictureBox2_legend.InitialImage = null;
            this.pictureBox2_legend.Location = new System.Drawing.Point(13, 45);
            this.pictureBox2_legend.Name = "pictureBox2_legend";
            this.pictureBox2_legend.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2_legend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2_legend.TabIndex = 2;
            this.pictureBox2_legend.TabStop = false;
            // 
            // label_notActive
            // 
            this.label_notActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_notActive.Location = new System.Drawing.Point(43, 21);
            this.label_notActive.Name = "label_notActive";
            this.label_notActive.Size = new System.Drawing.Size(100, 24);
            this.label_notActive.TabIndex = 1;
            this.label_notActive.Text = "Offline";
            this.label_notActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox_options
            // 
            this.groupBox_options.Controls.Add(this.button6);
            this.groupBox_options.Controls.Add(this.button3);
            this.groupBox_options.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_options.Location = new System.Drawing.Point(0, 0);
            this.groupBox_options.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_options.Name = "groupBox_options";
            this.groupBox_options.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_options.Size = new System.Drawing.Size(734, 100);
            this.groupBox_options.TabIndex = 0;
            this.groupBox_options.TabStop = false;
            this.groupBox_options.Text = "Options";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(593, 37);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 40);
            this.button6.TabIndex = 6;
            this.button6.Text = "Quit";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.click_quit);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(50, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 40);
            this.button3.TabIndex = 3;
            this.button3.Text = "Add Profile";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.click_addProfile);
            // 
            // tab_Options
            // 
            this.tab_Options.Location = new System.Drawing.Point(4, 34);
            this.tab_Options.Margin = new System.Windows.Forms.Padding(4);
            this.tab_Options.Name = "tab_Options";
            this.tab_Options.Padding = new System.Windows.Forms.Padding(4);
            this.tab_Options.Size = new System.Drawing.Size(1025, 445);
            this.tab_Options.TabIndex = 1;
            this.tab_Options.Text = "Options";
            this.tab_Options.UseVisualStyleBackColor = true;
            // 
            // tab_Mods
            // 
            this.tab_Mods.Location = new System.Drawing.Point(4, 34);
            this.tab_Mods.Name = "tab_Mods";
            this.tab_Mods.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Mods.Size = new System.Drawing.Size(1025, 445);
            this.tab_Mods.TabIndex = 2;
            this.tab_Mods.Text = "Exp/Mods";
            this.tab_Mods.UseVisualStyleBackColor = true;
            // 
            // tab_Plugins
            // 
            this.tab_Plugins.Location = new System.Drawing.Point(4, 34);
            this.tab_Plugins.Name = "tab_Plugins";
            this.tab_Plugins.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Plugins.Size = new System.Drawing.Size(1025, 445);
            this.tab_Plugins.TabIndex = 3;
            this.tab_Plugins.Text = "Plugins";
            this.tab_Plugins.UseVisualStyleBackColor = true;
            // 
            // tab_Updates
            // 
            this.tab_Updates.Location = new System.Drawing.Point(4, 34);
            this.tab_Updates.Name = "tab_Updates";
            this.tab_Updates.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Updates.Size = new System.Drawing.Size(1025, 445);
            this.tab_Updates.TabIndex = 4;
            this.tab_Updates.Text = "Updates";
            this.tab_Updates.UseVisualStyleBackColor = true;
            // 
            // tab_Admin
            // 
            this.tab_Admin.Location = new System.Drawing.Point(4, 34);
            this.tab_Admin.Name = "tab_Admin";
            this.tab_Admin.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Admin.Size = new System.Drawing.Size(1025, 445);
            this.tab_Admin.TabIndex = 5;
            this.tab_Admin.Text = "Admin";
            this.tab_Admin.UseVisualStyleBackColor = true;
            // 
            // tab_Help
            // 
            this.tab_Help.Location = new System.Drawing.Point(4, 34);
            this.tab_Help.Name = "tab_Help";
            this.tab_Help.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Help.Size = new System.Drawing.Size(1025, 445);
            this.tab_Help.TabIndex = 6;
            this.tab_Help.Text = "Help";
            this.tab_Help.UseVisualStyleBackColor = true;
            // 
            // tab_About
            // 
            this.tab_About.Location = new System.Drawing.Point(4, 34);
            this.tab_About.Name = "tab_About";
            this.tab_About.Padding = new System.Windows.Forms.Padding(3);
            this.tab_About.Size = new System.Drawing.Size(1025, 445);
            this.tab_About.TabIndex = 7;
            this.tab_About.Text = "About";
            this.tab_About.UseVisualStyleBackColor = true;
            // 
            // ServerManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 542);
            this.Controls.Add(this.tabControl_ProfileList);
            this.Controls.Add(this.panel_notificationBar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServerManager";
            this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HawkSync Server Manager";
            this.tabControl_ProfileList.ResumeLayout(false);
            this.tab_Profiles.ResumeLayout(false);
            this.groupBox_profiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_profiles)).EndInit();
            this.panel_profileControls.ResumeLayout(false);
            this.groupBox_legend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5_scoring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4_legend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3_legend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2_legend)).EndInit();
            this.groupBox_options.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.DataGridViewButtonColumn profileOpenBtn;

        private System.Windows.Forms.DataGridViewTextBoxColumn serverStatStatus;

        private System.Windows.Forms.DataGridViewTextBoxColumn currentTimer;

        private System.Windows.Forms.DataGridViewTextBoxColumn currentMapGameType;

        private System.Windows.Forms.DataGridViewTextBoxColumn currentMap;

        private System.Windows.Forms.DataGridViewTextBoxColumn serverPlayerCount;

        private System.Windows.Forms.DataGridViewImageColumn statusByte;

        private System.Windows.Forms.DataGridViewTextBoxColumn profileName;

        private System.Windows.Forms.Button button6;

        private System.Windows.Forms.Button button3;

        private System.Windows.Forms.Label label_scoring;
        private System.Windows.Forms.PictureBox pictureBox5_scoring;

        private System.Windows.Forms.Label label_notHosting;
        private System.Windows.Forms.PictureBox pictureBox4_legend;

        private System.Windows.Forms.Label label_active;
        private System.Windows.Forms.Label label_loading;
        private System.Windows.Forms.PictureBox pictureBox2_legend;
        private System.Windows.Forms.PictureBox pictureBox3_legend;

        private System.Windows.Forms.Label label_notActive;

        private System.Windows.Forms.DataGridView dataGrid_profiles;

        private System.Windows.Forms.GroupBox groupBox_profiles;

        private System.Windows.Forms.GroupBox groupBox_legend;

        private System.Windows.Forms.GroupBox groupBox_options;

        private System.Windows.Forms.Panel panel_profileControls;

        private System.Windows.Forms.TabPage tab_About;

        private System.Windows.Forms.TabPage tab_Admin;
        private System.Windows.Forms.TabPage tab_Help;

        private System.Windows.Forms.TabPage tab_Updates;

        private System.Windows.Forms.TabPage tab_Plugins;

        private System.Windows.Forms.TabPage tab_Mods;

        private System.Windows.Forms.TabControl tabControl_ProfileList;
        private System.Windows.Forms.TabPage tab_Profiles;
        private System.Windows.Forms.TabPage tab_Options;

        private System.Windows.Forms.Panel panel_notificationBar;

        #endregion
    }
}