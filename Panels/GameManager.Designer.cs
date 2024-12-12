using System.ComponentModel;

namespace ServerManager.Panels;

partial class GameManager
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameManager));
        this.panel_gameManager = new System.Windows.Forms.Panel();
        this.gameManager_tabControl = new System.Windows.Forms.TabControl();
        this.tabPage_serverControl = new System.Windows.Forms.TabPage();
        this.spacing_serverPanel = new System.Windows.Forms.Panel();
        this.panel_serverRight = new System.Windows.Forms.Panel();
        this.tabControl_serverExtra = new System.Windows.Forms.TabControl();
        this.serverTP_team = new System.Windows.Forms.TabPage();
        this.serverTC_misc = new System.Windows.Forms.TabPage();
        this.serverTC_restriction = new System.Windows.Forms.TabPage();
        this.serverTC_profileDetails = new System.Windows.Forms.TabPage();
        this.panel_serverLeft = new System.Windows.Forms.Panel();
        this.groupBox_serverOptions = new System.Windows.Forms.GroupBox();
        this.groupBox_serverDetails = new System.Windows.Forms.GroupBox();
        this.groupBox_options = new System.Windows.Forms.GroupBox();
        this.tabPage_mapControls = new System.Windows.Forms.TabPage();
        this.spacing_map = new System.Windows.Forms.Panel();
        this.panel_mapsCenter = new System.Windows.Forms.Panel();
        this.panel_mapsRight = new System.Windows.Forms.Panel();
        this.maps_tabControl2 = new System.Windows.Forms.TabControl();
        this.tabPage_currentMaps = new System.Windows.Forms.TabPage();
        this.tabPage_mapEditors = new System.Windows.Forms.TabPage();
        this.panel_mapsLeft = new System.Windows.Forms.Panel();
        this.maps_tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage_AvailMaps = new System.Windows.Forms.TabPage();
        this.tabPage_Schedule = new System.Windows.Forms.TabPage();
        this.tabPage_MapSettings = new System.Windows.Forms.TabPage();
        this.groupBox_mapOptions = new System.Windows.Forms.GroupBox();
        this.tabPage_messaging = new System.Windows.Forms.TabPage();
        this.tabPage_playerManagement = new System.Windows.Forms.TabPage();
        this.tabPage_stats = new System.Windows.Forms.TabPage();
        this.tabPage_admin = new System.Windows.Forms.TabPage();
        this.groupBox_mapsMT = new System.Windows.Forms.GroupBox();
        this.groupBox_maps_CB = new System.Windows.Forms.GroupBox();
        this.groupBox_mapsMC = new System.Windows.Forms.GroupBox();
        this.groupBox_scores = new System.Windows.Forms.GroupBox();
        this.panel_gameManager.SuspendLayout();
        this.gameManager_tabControl.SuspendLayout();
        this.tabPage_serverControl.SuspendLayout();
        this.spacing_serverPanel.SuspendLayout();
        this.panel_serverRight.SuspendLayout();
        this.tabControl_serverExtra.SuspendLayout();
        this.panel_serverLeft.SuspendLayout();
        this.tabPage_mapControls.SuspendLayout();
        this.spacing_map.SuspendLayout();
        this.panel_mapsCenter.SuspendLayout();
        this.panel_mapsRight.SuspendLayout();
        this.maps_tabControl2.SuspendLayout();
        this.panel_mapsLeft.SuspendLayout();
        this.maps_tabControl1.SuspendLayout();
        this.SuspendLayout();
        // 
        // panel_gameManager
        // 
        this.panel_gameManager.Controls.Add(this.gameManager_tabControl);
        this.panel_gameManager.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel_gameManager.Location = new System.Drawing.Point(0, 0);
        this.panel_gameManager.Name = "panel_gameManager";
        this.panel_gameManager.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
        this.panel_gameManager.Size = new System.Drawing.Size(1053, 542);
        this.panel_gameManager.TabIndex = 0;
        // 
        // gameManager_tabControl
        // 
        this.gameManager_tabControl.Controls.Add(this.tabPage_serverControl);
        this.gameManager_tabControl.Controls.Add(this.tabPage_mapControls);
        this.gameManager_tabControl.Controls.Add(this.tabPage_messaging);
        this.gameManager_tabControl.Controls.Add(this.tabPage_playerManagement);
        this.gameManager_tabControl.Controls.Add(this.tabPage_stats);
        this.gameManager_tabControl.Controls.Add(this.tabPage_admin);
        this.gameManager_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gameManager_tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.gameManager_tabControl.ItemSize = new System.Drawing.Size(70, 30);
        this.gameManager_tabControl.Location = new System.Drawing.Point(10, 5);
        this.gameManager_tabControl.Name = "gameManager_tabControl";
        this.gameManager_tabControl.Padding = new System.Drawing.Point(10, 5);
        this.gameManager_tabControl.SelectedIndex = 0;
        this.gameManager_tabControl.Size = new System.Drawing.Size(1033, 532);
        this.gameManager_tabControl.TabIndex = 0;
        // 
        // tabPage_serverControl
        // 
        this.tabPage_serverControl.Controls.Add(this.spacing_serverPanel);
        this.tabPage_serverControl.Location = new System.Drawing.Point(4, 34);
        this.tabPage_serverControl.Name = "tabPage_serverControl";
        this.tabPage_serverControl.Padding = new System.Windows.Forms.Padding(20);
        this.tabPage_serverControl.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_serverControl.TabIndex = 0;
        this.tabPage_serverControl.Text = "Server";
        this.tabPage_serverControl.UseVisualStyleBackColor = true;
        // 
        // spacing_serverPanel
        // 
        this.spacing_serverPanel.Controls.Add(this.panel_serverRight);
        this.spacing_serverPanel.Controls.Add(this.panel_serverLeft);
        this.spacing_serverPanel.Controls.Add(this.groupBox_options);
        this.spacing_serverPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.spacing_serverPanel.Location = new System.Drawing.Point(20, 20);
        this.spacing_serverPanel.Name = "spacing_serverPanel";
        this.spacing_serverPanel.Size = new System.Drawing.Size(985, 454);
        this.spacing_serverPanel.TabIndex = 0;
        // 
        // panel_serverRight
        // 
        this.panel_serverRight.Controls.Add(this.groupBox_scores);
        this.panel_serverRight.Controls.Add(this.tabControl_serverExtra);
        this.panel_serverRight.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel_serverRight.Location = new System.Drawing.Point(493, 0);
        this.panel_serverRight.Name = "panel_serverRight";
        this.panel_serverRight.Padding = new System.Windows.Forms.Padding(5, 8, 0, 0);
        this.panel_serverRight.Size = new System.Drawing.Size(492, 354);
        this.panel_serverRight.TabIndex = 2;
        // 
        // tabControl_serverExtra
        // 
        this.tabControl_serverExtra.Controls.Add(this.serverTP_team);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_misc);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_restriction);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_profileDetails);
        this.tabControl_serverExtra.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl_serverExtra.Location = new System.Drawing.Point(5, 8);
        this.tabControl_serverExtra.Name = "tabControl_serverExtra";
        this.tabControl_serverExtra.SelectedIndex = 0;
        this.tabControl_serverExtra.Size = new System.Drawing.Size(487, 346);
        this.tabControl_serverExtra.TabIndex = 0;
        // 
        // serverTP_team
        // 
        this.serverTP_team.Location = new System.Drawing.Point(4, 25);
        this.serverTP_team.Name = "serverTP_team";
        this.serverTP_team.Padding = new System.Windows.Forms.Padding(3);
        this.serverTP_team.Size = new System.Drawing.Size(479, 317);
        this.serverTP_team.TabIndex = 0;
        this.serverTP_team.Text = "Team Options";
        this.serverTP_team.UseVisualStyleBackColor = true;
        // 
        // serverTC_misc
        // 
        this.serverTC_misc.Location = new System.Drawing.Point(4, 25);
        this.serverTC_misc.Name = "serverTC_misc";
        this.serverTC_misc.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_misc.Size = new System.Drawing.Size(474, 317);
        this.serverTC_misc.TabIndex = 1;
        this.serverTC_misc.Text = "Misc Options";
        this.serverTC_misc.UseVisualStyleBackColor = true;
        // 
        // serverTC_restriction
        // 
        this.serverTC_restriction.Location = new System.Drawing.Point(4, 25);
        this.serverTC_restriction.Name = "serverTC_restriction";
        this.serverTC_restriction.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_restriction.Size = new System.Drawing.Size(474, 317);
        this.serverTC_restriction.TabIndex = 2;
        this.serverTC_restriction.Text = "Restrictions";
        this.serverTC_restriction.UseVisualStyleBackColor = true;
        // 
        // serverTC_profileDetails
        // 
        this.serverTC_profileDetails.Location = new System.Drawing.Point(4, 25);
        this.serverTC_profileDetails.Name = "serverTC_profileDetails";
        this.serverTC_profileDetails.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_profileDetails.Size = new System.Drawing.Size(474, 317);
        this.serverTC_profileDetails.TabIndex = 3;
        this.serverTC_profileDetails.Text = "Profile Details";
        this.serverTC_profileDetails.UseVisualStyleBackColor = true;
        // 
        // panel_serverLeft
        // 
        this.panel_serverLeft.Controls.Add(this.groupBox_serverOptions);
        this.panel_serverLeft.Controls.Add(this.groupBox_serverDetails);
        this.panel_serverLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel_serverLeft.Location = new System.Drawing.Point(0, 0);
        this.panel_serverLeft.Name = "panel_serverLeft";
        this.panel_serverLeft.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
        this.panel_serverLeft.Size = new System.Drawing.Size(492, 354);
        this.panel_serverLeft.TabIndex = 1;
        // 
        // groupBox_serverOptions
        // 
        this.groupBox_serverOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_serverOptions.Location = new System.Drawing.Point(0, 177);
        this.groupBox_serverOptions.Name = "groupBox_serverOptions";
        this.groupBox_serverOptions.Size = new System.Drawing.Size(487, 177);
        this.groupBox_serverOptions.TabIndex = 1;
        this.groupBox_serverOptions.TabStop = false;
        this.groupBox_serverOptions.Text = "Server Options";
        // 
        // groupBox_serverDetails
        // 
        this.groupBox_serverDetails.Dock = System.Windows.Forms.DockStyle.Top;
        this.groupBox_serverDetails.Location = new System.Drawing.Point(0, 0);
        this.groupBox_serverDetails.Name = "groupBox_serverDetails";
        this.groupBox_serverDetails.Size = new System.Drawing.Size(487, 177);
        this.groupBox_serverDetails.TabIndex = 0;
        this.groupBox_serverDetails.TabStop = false;
        this.groupBox_serverDetails.Text = "Server Details";
        // 
        // groupBox_options
        // 
        this.groupBox_options.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_options.Location = new System.Drawing.Point(0, 354);
        this.groupBox_options.Name = "groupBox_options";
        this.groupBox_options.Size = new System.Drawing.Size(985, 100);
        this.groupBox_options.TabIndex = 0;
        this.groupBox_options.TabStop = false;
        this.groupBox_options.Text = "Options";
        // 
        // tabPage_mapControls
        // 
        this.tabPage_mapControls.Controls.Add(this.spacing_map);
        this.tabPage_mapControls.Location = new System.Drawing.Point(4, 34);
        this.tabPage_mapControls.Name = "tabPage_mapControls";
        this.tabPage_mapControls.Padding = new System.Windows.Forms.Padding(20);
        this.tabPage_mapControls.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_mapControls.TabIndex = 1;
        this.tabPage_mapControls.Text = "Maps";
        this.tabPage_mapControls.UseVisualStyleBackColor = true;
        // 
        // spacing_map
        // 
        this.spacing_map.Controls.Add(this.panel_mapsCenter);
        this.spacing_map.Controls.Add(this.panel_mapsRight);
        this.spacing_map.Controls.Add(this.panel_mapsLeft);
        this.spacing_map.Controls.Add(this.groupBox_mapOptions);
        this.spacing_map.Dock = System.Windows.Forms.DockStyle.Fill;
        this.spacing_map.Location = new System.Drawing.Point(20, 20);
        this.spacing_map.Name = "spacing_map";
        this.spacing_map.Size = new System.Drawing.Size(985, 454);
        this.spacing_map.TabIndex = 0;
        // 
        // panel_mapsCenter
        // 
        this.panel_mapsCenter.Controls.Add(this.groupBox_mapsMC);
        this.panel_mapsCenter.Controls.Add(this.groupBox_maps_CB);
        this.panel_mapsCenter.Controls.Add(this.groupBox_mapsMT);
        this.panel_mapsCenter.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel_mapsCenter.Location = new System.Drawing.Point(425, 0);
        this.panel_mapsCenter.Name = "panel_mapsCenter";
        this.panel_mapsCenter.Padding = new System.Windows.Forms.Padding(10, 16, 10, 0);
        this.panel_mapsCenter.Size = new System.Drawing.Size(135, 354);
        this.panel_mapsCenter.TabIndex = 3;
        // 
        // panel_mapsRight
        // 
        this.panel_mapsRight.Controls.Add(this.maps_tabControl2);
        this.panel_mapsRight.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel_mapsRight.Location = new System.Drawing.Point(560, 0);
        this.panel_mapsRight.Name = "panel_mapsRight";
        this.panel_mapsRight.Size = new System.Drawing.Size(425, 354);
        this.panel_mapsRight.TabIndex = 2;
        // 
        // maps_tabControl2
        // 
        this.maps_tabControl2.Controls.Add(this.tabPage_currentMaps);
        this.maps_tabControl2.Controls.Add(this.tabPage_mapEditors);
        this.maps_tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.maps_tabControl2.Location = new System.Drawing.Point(0, 0);
        this.maps_tabControl2.Name = "maps_tabControl2";
        this.maps_tabControl2.SelectedIndex = 0;
        this.maps_tabControl2.Size = new System.Drawing.Size(425, 354);
        this.maps_tabControl2.TabIndex = 0;
        // 
        // tabPage_currentMaps
        // 
        this.tabPage_currentMaps.Location = new System.Drawing.Point(4, 25);
        this.tabPage_currentMaps.Name = "tabPage_currentMaps";
        this.tabPage_currentMaps.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_currentMaps.Size = new System.Drawing.Size(417, 325);
        this.tabPage_currentMaps.TabIndex = 0;
        this.tabPage_currentMaps.Text = "Current Playlist";
        this.tabPage_currentMaps.UseVisualStyleBackColor = true;
        // 
        // tabPage_mapEditors
        // 
        this.tabPage_mapEditors.Location = new System.Drawing.Point(4, 25);
        this.tabPage_mapEditors.Name = "tabPage_mapEditors";
        this.tabPage_mapEditors.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_mapEditors.Size = new System.Drawing.Size(417, 325);
        this.tabPage_mapEditors.TabIndex = 1;
        this.tabPage_mapEditors.Text = "Playlist Editor";
        this.tabPage_mapEditors.UseVisualStyleBackColor = true;
        // 
        // panel_mapsLeft
        // 
        this.panel_mapsLeft.Controls.Add(this.maps_tabControl1);
        this.panel_mapsLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel_mapsLeft.Location = new System.Drawing.Point(0, 0);
        this.panel_mapsLeft.Name = "panel_mapsLeft";
        this.panel_mapsLeft.Size = new System.Drawing.Size(425, 354);
        this.panel_mapsLeft.TabIndex = 1;
        // 
        // maps_tabControl1
        // 
        this.maps_tabControl1.Controls.Add(this.tabPage_AvailMaps);
        this.maps_tabControl1.Controls.Add(this.tabPage_Schedule);
        this.maps_tabControl1.Controls.Add(this.tabPage_MapSettings);
        this.maps_tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.maps_tabControl1.Location = new System.Drawing.Point(0, 0);
        this.maps_tabControl1.Name = "maps_tabControl1";
        this.maps_tabControl1.SelectedIndex = 0;
        this.maps_tabControl1.Size = new System.Drawing.Size(425, 354);
        this.maps_tabControl1.TabIndex = 0;
        // 
        // tabPage_AvailMaps
        // 
        this.tabPage_AvailMaps.Location = new System.Drawing.Point(4, 25);
        this.tabPage_AvailMaps.Name = "tabPage_AvailMaps";
        this.tabPage_AvailMaps.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_AvailMaps.Size = new System.Drawing.Size(417, 325);
        this.tabPage_AvailMaps.TabIndex = 0;
        this.tabPage_AvailMaps.Text = "Available Maps";
        this.tabPage_AvailMaps.UseVisualStyleBackColor = true;
        // 
        // tabPage_Schedule
        // 
        this.tabPage_Schedule.Location = new System.Drawing.Point(4, 25);
        this.tabPage_Schedule.Name = "tabPage_Schedule";
        this.tabPage_Schedule.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_Schedule.Size = new System.Drawing.Size(417, 325);
        this.tabPage_Schedule.TabIndex = 2;
        this.tabPage_Schedule.Text = "Scheduling";
        this.tabPage_Schedule.UseVisualStyleBackColor = true;
        // 
        // tabPage_MapSettings
        // 
        this.tabPage_MapSettings.Location = new System.Drawing.Point(4, 25);
        this.tabPage_MapSettings.Name = "tabPage_MapSettings";
        this.tabPage_MapSettings.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_MapSettings.Size = new System.Drawing.Size(417, 325);
        this.tabPage_MapSettings.TabIndex = 1;
        this.tabPage_MapSettings.Text = "Settings";
        this.tabPage_MapSettings.UseVisualStyleBackColor = true;
        // 
        // groupBox_mapOptions
        // 
        this.groupBox_mapOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_mapOptions.Location = new System.Drawing.Point(0, 354);
        this.groupBox_mapOptions.Name = "groupBox_mapOptions";
        this.groupBox_mapOptions.Size = new System.Drawing.Size(985, 100);
        this.groupBox_mapOptions.TabIndex = 0;
        this.groupBox_mapOptions.TabStop = false;
        this.groupBox_mapOptions.Text = "Options";
        // 
        // tabPage_messaging
        // 
        this.tabPage_messaging.Location = new System.Drawing.Point(4, 34);
        this.tabPage_messaging.Name = "tabPage_messaging";
        this.tabPage_messaging.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_messaging.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_messaging.TabIndex = 2;
        this.tabPage_messaging.Text = "Messaging";
        this.tabPage_messaging.UseVisualStyleBackColor = true;
        // 
        // tabPage_playerManagement
        // 
        this.tabPage_playerManagement.Location = new System.Drawing.Point(4, 34);
        this.tabPage_playerManagement.Name = "tabPage_playerManagement";
        this.tabPage_playerManagement.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_playerManagement.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_playerManagement.TabIndex = 3;
        this.tabPage_playerManagement.Text = "Players";
        this.tabPage_playerManagement.UseVisualStyleBackColor = true;
        // 
        // tabPage_stats
        // 
        this.tabPage_stats.Location = new System.Drawing.Point(4, 34);
        this.tabPage_stats.Name = "tabPage_stats";
        this.tabPage_stats.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_stats.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_stats.TabIndex = 5;
        this.tabPage_stats.Text = "Stats";
        this.tabPage_stats.UseVisualStyleBackColor = true;
        // 
        // tabPage_admin
        // 
        this.tabPage_admin.Location = new System.Drawing.Point(4, 34);
        this.tabPage_admin.Name = "tabPage_admin";
        this.tabPage_admin.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_admin.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_admin.TabIndex = 4;
        this.tabPage_admin.Text = "Admins";
        this.tabPage_admin.UseVisualStyleBackColor = true;
        // 
        // groupBox_mapsMT
        // 
        this.groupBox_mapsMT.Dock = System.Windows.Forms.DockStyle.Top;
        this.groupBox_mapsMT.Location = new System.Drawing.Point(10, 16);
        this.groupBox_mapsMT.Margin = new System.Windows.Forms.Padding(0);
        this.groupBox_mapsMT.Name = "groupBox_mapsMT";
        this.groupBox_mapsMT.Size = new System.Drawing.Size(115, 100);
        this.groupBox_mapsMT.TabIndex = 0;
        this.groupBox_mapsMT.TabStop = false;
        // 
        // groupBox_maps_CB
        // 
        this.groupBox_maps_CB.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_maps_CB.Location = new System.Drawing.Point(10, 254);
        this.groupBox_maps_CB.Name = "groupBox_maps_CB";
        this.groupBox_maps_CB.Size = new System.Drawing.Size(115, 100);
        this.groupBox_maps_CB.TabIndex = 1;
        this.groupBox_maps_CB.TabStop = false;
        // 
        // groupBox_mapsMC
        // 
        this.groupBox_mapsMC.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox_mapsMC.Location = new System.Drawing.Point(10, 116);
        this.groupBox_mapsMC.Name = "groupBox_mapsMC";
        this.groupBox_mapsMC.Size = new System.Drawing.Size(115, 138);
        this.groupBox_mapsMC.TabIndex = 2;
        this.groupBox_mapsMC.TabStop = false;
        // 
        // groupBox_scores
        // 
        this.groupBox_scores.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_scores.Location = new System.Drawing.Point(5, 254);
        this.groupBox_scores.Name = "groupBox_scores";
        this.groupBox_scores.Size = new System.Drawing.Size(487, 100);
        this.groupBox_scores.TabIndex = 1;
        this.groupBox_scores.TabStop = false;
        this.groupBox_scores.Text = "Scoring";
        // 
        // GameManager
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1053, 542);
        this.Controls.Add(this.panel_gameManager);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "GameManager";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "GameManager";
        this.panel_gameManager.ResumeLayout(false);
        this.gameManager_tabControl.ResumeLayout(false);
        this.tabPage_serverControl.ResumeLayout(false);
        this.spacing_serverPanel.ResumeLayout(false);
        this.panel_serverRight.ResumeLayout(false);
        this.tabControl_serverExtra.ResumeLayout(false);
        this.panel_serverLeft.ResumeLayout(false);
        this.tabPage_mapControls.ResumeLayout(false);
        this.spacing_map.ResumeLayout(false);
        this.panel_mapsCenter.ResumeLayout(false);
        this.panel_mapsRight.ResumeLayout(false);
        this.maps_tabControl2.ResumeLayout(false);
        this.panel_mapsLeft.ResumeLayout(false);
        this.maps_tabControl1.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox groupBox_scores;

    private System.Windows.Forms.GroupBox groupBox_mapsMC;

    private System.Windows.Forms.GroupBox groupBox_maps_CB;

    private System.Windows.Forms.GroupBox groupBox_mapsMT;

    private System.Windows.Forms.TabControl maps_tabControl2;
    private System.Windows.Forms.TabPage tabPage_currentMaps;
    private System.Windows.Forms.TabPage tabPage_mapEditors;

    private System.Windows.Forms.Panel panel_mapsCenter;

    private System.Windows.Forms.Panel panel_mapsRight;

    private System.Windows.Forms.TabPage tabPage_Schedule;

    private System.Windows.Forms.TabControl maps_tabControl1;
    private System.Windows.Forms.TabPage tabPage_AvailMaps;
    private System.Windows.Forms.TabPage tabPage_MapSettings;

    private System.Windows.Forms.Panel panel_mapsLeft;

    private System.Windows.Forms.GroupBox groupBox_mapOptions;

    private System.Windows.Forms.Panel spacing_map;

    private System.Windows.Forms.TabPage serverTC_profileDetails;

    private System.Windows.Forms.TabPage serverTC_restriction;

    private System.Windows.Forms.TabPage tabPage_stats;

    private System.Windows.Forms.TabPage tabPage_admin;

    private System.Windows.Forms.TabControl tabControl_serverExtra;
    private System.Windows.Forms.TabPage serverTP_team;
    private System.Windows.Forms.TabPage serverTC_misc;

    private System.Windows.Forms.GroupBox groupBox_serverOptions;

    private System.Windows.Forms.GroupBox groupBox_serverDetails;

    private System.Windows.Forms.Panel panel_serverRight;

    private System.Windows.Forms.Panel panel_serverLeft;

    private System.Windows.Forms.GroupBox groupBox_options;

    private System.Windows.Forms.Panel spacing_serverPanel;

    private System.Windows.Forms.TabPage tabPage_playerManagement;

    private System.Windows.Forms.TabPage tabPage_messaging;

    private System.Windows.Forms.TabControl gameManager_tabControl;
    private System.Windows.Forms.TabPage tabPage_serverControl;
    private System.Windows.Forms.TabPage tabPage_mapControls;

    private System.Windows.Forms.Panel panel_gameManager;

    #endregion
}