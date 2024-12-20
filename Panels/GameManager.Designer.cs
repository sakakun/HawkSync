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
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameManager));
        this.panel_gameManager = new System.Windows.Forms.Panel();
        this.gameManager_tabControl = new System.Windows.Forms.TabControl();
        this.tabPage_serverControl = new System.Windows.Forms.TabPage();
        this.spacing_serverPanel = new System.Windows.Forms.Panel();
        this.panel_serverRight = new System.Windows.Forms.Panel();
        this.groupBox_scores = new System.Windows.Forms.GroupBox();
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
        this.groupBox_mapsMC = new System.Windows.Forms.GroupBox();
        this.groupBox_maps_CB = new System.Windows.Forms.GroupBox();
        this.groupBox_mapsMT = new System.Windows.Forms.GroupBox();
        this.panel_mapsRight = new System.Windows.Forms.Panel();
        this.maps_tabControl2 = new System.Windows.Forms.TabControl();
        this.tabPage_currentMaps = new System.Windows.Forms.TabPage();
        this.CurrentMapPlaylist = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.panel6 = new System.Windows.Forms.Panel();
        this.panel7 = new System.Windows.Forms.Panel();
        this.comboBox_loadMapList = new System.Windows.Forms.ComboBox();
        this.panel8 = new System.Windows.Forms.Panel();
        this.pb_loadPlaylistCurrent = new System.Windows.Forms.PictureBox();
        this.pb_savePlaylistCurrent = new System.Windows.Forms.PictureBox();
        this.panel9 = new System.Windows.Forms.Panel();
        this.tabPage_mapEditors = new System.Windows.Forms.TabPage();
        this.PlayListEditor = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.panel10 = new System.Windows.Forms.Panel();
        this.panel11 = new System.Windows.Forms.Panel();
        this.comboBox_AvailableMapLists = new System.Windows.Forms.ComboBox();
        this.panel12 = new System.Windows.Forms.Panel();
        this.panel13 = new System.Windows.Forms.Panel();
        this.panel_mapsLeft = new System.Windows.Forms.Panel();
        this.maps_tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage_AvailMaps = new System.Windows.Forms.TabPage();
        this.panel2 = new System.Windows.Forms.Panel();
        this.AvailableMapList = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.gametype_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridView3 = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridView2 = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridView1 = new System.Windows.Forms.DataGridView();
        this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.AvailableMaps = new System.Windows.Forms.DataGridView();
        this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.mission_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.mission_file = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.game = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.GameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.CustomMap = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel5 = new System.Windows.Forms.Panel();
        this.comboBox_gameTypes = new System.Windows.Forms.ComboBox();
        this.panel4 = new System.Windows.Forms.Panel();
        this.pictureBox_refresh = new System.Windows.Forms.PictureBox();
        this.panel3 = new System.Windows.Forms.Panel();
        this.tabPage_Schedule = new System.Windows.Forms.TabPage();
        this.tabPage_MapSettings = new System.Windows.Forms.TabPage();
        this.tabPage_messaging = new System.Windows.Forms.TabPage();
        this.spacing_chat = new System.Windows.Forms.Panel();
        this.tabControl_messaging = new System.Windows.Forms.TabControl();
        this.tabPage_chatMsg = new System.Windows.Forms.TabPage();
        this.tabPage_chatMod = new System.Windows.Forms.TabPage();
        this.tabPage_serverMsg = new System.Windows.Forms.TabPage();
        this.tabPage_schedMsg = new System.Windows.Forms.TabPage();
        this.tabPage_playerManagement = new System.Windows.Forms.TabPage();
        this.spacing_players = new System.Windows.Forms.Panel();
        this.groupBox_playerDetails = new System.Windows.Forms.GroupBox();
        this.groupBox_playerBox = new System.Windows.Forms.GroupBox();
        this.slot_01 = new System.Windows.Forms.Panel();
        this.label_01_ip = new System.Windows.Forms.Label();
        this.label_01_playerName = new System.Windows.Forms.Label();
        this.label_01_slot = new System.Windows.Forms.Label();
        this.slot_50 = new System.Windows.Forms.Panel();
        this.slot_11 = new System.Windows.Forms.Panel();
        this.slot_40 = new System.Windows.Forms.Panel();
        this.slot_02 = new System.Windows.Forms.Panel();
        this.label_slot02 = new System.Windows.Forms.Label();
        this.slot_49 = new System.Windows.Forms.Panel();
        this.slot_03 = new System.Windows.Forms.Panel();
        this.label_slot03 = new System.Windows.Forms.Label();
        this.slot_30 = new System.Windows.Forms.Panel();
        this.slot_04 = new System.Windows.Forms.Panel();
        this.slot_48 = new System.Windows.Forms.Panel();
        this.slot_05 = new System.Windows.Forms.Panel();
        this.slot_39 = new System.Windows.Forms.Panel();
        this.slot_06 = new System.Windows.Forms.Panel();
        this.slot_47 = new System.Windows.Forms.Panel();
        this.slot_07 = new System.Windows.Forms.Panel();
        this.slot_20 = new System.Windows.Forms.Panel();
        this.slot_08 = new System.Windows.Forms.Panel();
        this.slot_46 = new System.Windows.Forms.Panel();
        this.slot_09 = new System.Windows.Forms.Panel();
        this.slot_38 = new System.Windows.Forms.Panel();
        this.slot_10 = new System.Windows.Forms.Panel();
        this.slot_45 = new System.Windows.Forms.Panel();
        this.slot_21 = new System.Windows.Forms.Panel();
        this.slot_29 = new System.Windows.Forms.Panel();
        this.slot_12 = new System.Windows.Forms.Panel();
        this.slot_44 = new System.Windows.Forms.Panel();
        this.slot_22 = new System.Windows.Forms.Panel();
        this.slot_37 = new System.Windows.Forms.Panel();
        this.slot_13 = new System.Windows.Forms.Panel();
        this.slot_43 = new System.Windows.Forms.Panel();
        this.slot_23 = new System.Windows.Forms.Panel();
        this.slot_19 = new System.Windows.Forms.Panel();
        this.slot_14 = new System.Windows.Forms.Panel();
        this.slot_42 = new System.Windows.Forms.Panel();
        this.slot_24 = new System.Windows.Forms.Panel();
        this.slot_36 = new System.Windows.Forms.Panel();
        this.slot_15 = new System.Windows.Forms.Panel();
        this.slot_41 = new System.Windows.Forms.Panel();
        this.slot_25 = new System.Windows.Forms.Panel();
        this.slot_28 = new System.Windows.Forms.Panel();
        this.slot_16 = new System.Windows.Forms.Panel();
        this.slot_35 = new System.Windows.Forms.Panel();
        this.slot_31 = new System.Windows.Forms.Panel();
        this.slot_18 = new System.Windows.Forms.Panel();
        this.slot_26 = new System.Windows.Forms.Panel();
        this.slot_34 = new System.Windows.Forms.Panel();
        this.slot_32 = new System.Windows.Forms.Panel();
        this.slot_27 = new System.Windows.Forms.Panel();
        this.slot_17 = new System.Windows.Forms.Panel();
        this.slot_33 = new System.Windows.Forms.Panel();
        this.tabPage_stats = new System.Windows.Forms.TabPage();
        this.tabPage_admin = new System.Windows.Forms.TabPage();
        this.pb_erasePlaylistCurrent = new System.Windows.Forms.PictureBox();
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
        this.tabPage_currentMaps.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.CurrentMapPlaylist)).BeginInit();
        this.panel6.SuspendLayout();
        this.panel7.SuspendLayout();
        this.panel8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pb_loadPlaylistCurrent)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pb_savePlaylistCurrent)).BeginInit();
        this.tabPage_mapEditors.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.PlayListEditor)).BeginInit();
        this.panel10.SuspendLayout();
        this.panel11.SuspendLayout();
        this.panel_mapsLeft.SuspendLayout();
        this.maps_tabControl1.SuspendLayout();
        this.tabPage_AvailMaps.SuspendLayout();
        this.panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.AvailableMapList)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.AvailableMaps)).BeginInit();
        this.panel1.SuspendLayout();
        this.panel5.SuspendLayout();
        this.panel4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_refresh)).BeginInit();
        this.tabPage_messaging.SuspendLayout();
        this.spacing_chat.SuspendLayout();
        this.tabControl_messaging.SuspendLayout();
        this.tabPage_playerManagement.SuspendLayout();
        this.spacing_players.SuspendLayout();
        this.groupBox_playerBox.SuspendLayout();
        this.slot_01.SuspendLayout();
        this.slot_02.SuspendLayout();
        this.slot_03.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pb_erasePlaylistCurrent)).BeginInit();
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
        // groupBox_scores
        // 
        this.groupBox_scores.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_scores.Location = new System.Drawing.Point(5, 294);
        this.groupBox_scores.Name = "groupBox_scores";
        this.groupBox_scores.Size = new System.Drawing.Size(487, 60);
        this.groupBox_scores.TabIndex = 1;
        this.groupBox_scores.TabStop = false;
        this.groupBox_scores.Text = "Scoring";
        // 
        // tabControl_serverExtra
        // 
        this.tabControl_serverExtra.Controls.Add(this.serverTP_team);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_misc);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_restriction);
        this.tabControl_serverExtra.Controls.Add(this.serverTC_profileDetails);
        this.tabControl_serverExtra.Dock = System.Windows.Forms.DockStyle.Top;
        this.tabControl_serverExtra.Location = new System.Drawing.Point(5, 8);
        this.tabControl_serverExtra.Name = "tabControl_serverExtra";
        this.tabControl_serverExtra.SelectedIndex = 0;
        this.tabControl_serverExtra.Size = new System.Drawing.Size(487, 285);
        this.tabControl_serverExtra.TabIndex = 0;
        // 
        // serverTP_team
        // 
        this.serverTP_team.Location = new System.Drawing.Point(4, 25);
        this.serverTP_team.Name = "serverTP_team";
        this.serverTP_team.Padding = new System.Windows.Forms.Padding(3);
        this.serverTP_team.Size = new System.Drawing.Size(479, 256);
        this.serverTP_team.TabIndex = 0;
        this.serverTP_team.Text = "Team Options";
        this.serverTP_team.UseVisualStyleBackColor = true;
        // 
        // serverTC_misc
        // 
        this.serverTC_misc.Location = new System.Drawing.Point(4, 25);
        this.serverTC_misc.Name = "serverTC_misc";
        this.serverTC_misc.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_misc.Size = new System.Drawing.Size(479, 256);
        this.serverTC_misc.TabIndex = 1;
        this.serverTC_misc.Text = "Misc Options";
        this.serverTC_misc.UseVisualStyleBackColor = true;
        // 
        // serverTC_restriction
        // 
        this.serverTC_restriction.Location = new System.Drawing.Point(4, 25);
        this.serverTC_restriction.Name = "serverTC_restriction";
        this.serverTC_restriction.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_restriction.Size = new System.Drawing.Size(479, 256);
        this.serverTC_restriction.TabIndex = 2;
        this.serverTC_restriction.Text = "Restrictions";
        this.serverTC_restriction.UseVisualStyleBackColor = true;
        // 
        // serverTC_profileDetails
        // 
        this.serverTC_profileDetails.Location = new System.Drawing.Point(4, 25);
        this.serverTC_profileDetails.Name = "serverTC_profileDetails";
        this.serverTC_profileDetails.Padding = new System.Windows.Forms.Padding(3);
        this.serverTC_profileDetails.Size = new System.Drawing.Size(479, 256);
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
        this.panel_mapsCenter.Size = new System.Drawing.Size(135, 454);
        this.panel_mapsCenter.TabIndex = 3;
        // 
        // groupBox_mapsMC
        // 
        this.groupBox_mapsMC.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox_mapsMC.Location = new System.Drawing.Point(10, 116);
        this.groupBox_mapsMC.Name = "groupBox_mapsMC";
        this.groupBox_mapsMC.Size = new System.Drawing.Size(115, 238);
        this.groupBox_mapsMC.TabIndex = 2;
        this.groupBox_mapsMC.TabStop = false;
        // 
        // groupBox_maps_CB
        // 
        this.groupBox_maps_CB.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.groupBox_maps_CB.Location = new System.Drawing.Point(10, 354);
        this.groupBox_maps_CB.Name = "groupBox_maps_CB";
        this.groupBox_maps_CB.Size = new System.Drawing.Size(115, 100);
        this.groupBox_maps_CB.TabIndex = 1;
        this.groupBox_maps_CB.TabStop = false;
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
        // panel_mapsRight
        // 
        this.panel_mapsRight.Controls.Add(this.maps_tabControl2);
        this.panel_mapsRight.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel_mapsRight.Location = new System.Drawing.Point(560, 0);
        this.panel_mapsRight.Name = "panel_mapsRight";
        this.panel_mapsRight.Size = new System.Drawing.Size(425, 454);
        this.panel_mapsRight.TabIndex = 2;
        // 
        // maps_tabControl2
        // 
        this.maps_tabControl2.Controls.Add(this.tabPage_currentMaps);
        this.maps_tabControl2.Controls.Add(this.tabPage_mapEditors);
        this.maps_tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.maps_tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.maps_tabControl2.Location = new System.Drawing.Point(0, 0);
        this.maps_tabControl2.Name = "maps_tabControl2";
        this.maps_tabControl2.SelectedIndex = 0;
        this.maps_tabControl2.Size = new System.Drawing.Size(425, 454);
        this.maps_tabControl2.TabIndex = 0;
        // 
        // tabPage_currentMaps
        // 
        this.tabPage_currentMaps.Controls.Add(this.CurrentMapPlaylist);
        this.tabPage_currentMaps.Controls.Add(this.panel6);
        this.tabPage_currentMaps.Location = new System.Drawing.Point(4, 22);
        this.tabPage_currentMaps.Name = "tabPage_currentMaps";
        this.tabPage_currentMaps.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_currentMaps.Size = new System.Drawing.Size(417, 428);
        this.tabPage_currentMaps.TabIndex = 0;
        this.tabPage_currentMaps.Text = "Current Playlist";
        this.tabPage_currentMaps.UseVisualStyleBackColor = true;
        // 
        // CurrentMapPlaylist
        // 
        this.CurrentMapPlaylist.AllowDrop = true;
        this.CurrentMapPlaylist.AllowUserToAddRows = false;
        this.CurrentMapPlaylist.AllowUserToResizeColumns = false;
        this.CurrentMapPlaylist.AllowUserToResizeRows = false;
        this.CurrentMapPlaylist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.CurrentMapPlaylist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        this.CurrentMapPlaylist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.CurrentMapPlaylist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn5, this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7 });
        dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.CurrentMapPlaylist.DefaultCellStyle = dataGridViewCellStyle3;
        this.CurrentMapPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
        this.CurrentMapPlaylist.Location = new System.Drawing.Point(3, 28);
        this.CurrentMapPlaylist.Name = "CurrentMapPlaylist";
        this.CurrentMapPlaylist.ReadOnly = true;
        dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.CurrentMapPlaylist.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
        dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.CurrentMapPlaylist.RowsDefaultCellStyle = dataGridViewCellStyle5;
        this.CurrentMapPlaylist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.CurrentMapPlaylist.Size = new System.Drawing.Size(411, 397);
        this.CurrentMapPlaylist.TabIndex = 5;
        // 
        // dataGridViewTextBoxColumn1
        // 
        this.dataGridViewTextBoxColumn1.HeaderText = "Id";
        this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        this.dataGridViewTextBoxColumn1.ReadOnly = true;
        this.dataGridViewTextBoxColumn1.Visible = false;
        // 
        // dataGridViewTextBoxColumn2
        // 
        this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn2.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        this.dataGridViewTextBoxColumn2.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn3
        // 
        this.dataGridViewTextBoxColumn3.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
        this.dataGridViewTextBoxColumn3.ReadOnly = true;
        this.dataGridViewTextBoxColumn3.Visible = false;
        // 
        // dataGridViewTextBoxColumn4
        // 
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
        this.dataGridViewTextBoxColumn4.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
        this.dataGridViewTextBoxColumn4.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn5
        // 
        this.dataGridViewTextBoxColumn5.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
        this.dataGridViewTextBoxColumn5.ReadOnly = true;
        this.dataGridViewTextBoxColumn5.Visible = false;
        // 
        // dataGridViewTextBoxColumn6
        // 
        this.dataGridViewTextBoxColumn6.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
        this.dataGridViewTextBoxColumn6.ReadOnly = true;
        this.dataGridViewTextBoxColumn6.Visible = false;
        // 
        // dataGridViewTextBoxColumn7
        // 
        this.dataGridViewTextBoxColumn7.HeaderText = "GameType ID";
        this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
        this.dataGridViewTextBoxColumn7.ReadOnly = true;
        this.dataGridViewTextBoxColumn7.Visible = false;
        // 
        // panel6
        // 
        this.panel6.Controls.Add(this.panel7);
        this.panel6.Controls.Add(this.panel8);
        this.panel6.Controls.Add(this.panel9);
        this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel6.Location = new System.Drawing.Point(3, 3);
        this.panel6.Name = "panel6";
        this.panel6.Size = new System.Drawing.Size(411, 25);
        this.panel6.TabIndex = 1;
        // 
        // panel7
        // 
        this.panel7.Controls.Add(this.comboBox_loadMapList);
        this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel7.Location = new System.Drawing.Point(100, 0);
        this.panel7.Name = "panel7";
        this.panel7.Size = new System.Drawing.Size(211, 25);
        this.panel7.TabIndex = 2;
        // 
        // comboBox_loadMapList
        // 
        this.comboBox_loadMapList.Dock = System.Windows.Forms.DockStyle.Fill;
        this.comboBox_loadMapList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.comboBox_loadMapList.FormattingEnabled = true;
        this.comboBox_loadMapList.Location = new System.Drawing.Point(0, 0);
        this.comboBox_loadMapList.Name = "comboBox_loadMapList";
        this.comboBox_loadMapList.Size = new System.Drawing.Size(211, 24);
        this.comboBox_loadMapList.TabIndex = 1;
        // 
        // panel8
        // 
        this.panel8.Controls.Add(this.pb_erasePlaylistCurrent);
        this.panel8.Controls.Add(this.pb_loadPlaylistCurrent);
        this.panel8.Controls.Add(this.pb_savePlaylistCurrent);
        this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel8.Location = new System.Drawing.Point(311, 0);
        this.panel8.Name = "panel8";
        this.panel8.Size = new System.Drawing.Size(100, 25);
        this.panel8.TabIndex = 1;
        // 
        // pb_loadPlaylistCurrent
        // 
        this.pb_loadPlaylistCurrent.ErrorImage = null;
        this.pb_loadPlaylistCurrent.Image = global::ServerManager.Properties.Resources.folderOpen;
        this.pb_loadPlaylistCurrent.InitialImage = null;
        this.pb_loadPlaylistCurrent.Location = new System.Drawing.Point(8, 4);
        this.pb_loadPlaylistCurrent.Name = "pb_loadPlaylistCurrent";
        this.pb_loadPlaylistCurrent.Size = new System.Drawing.Size(15, 15);
        this.pb_loadPlaylistCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pb_loadPlaylistCurrent.TabIndex = 1;
        this.pb_loadPlaylistCurrent.TabStop = false;
        // 
        // pb_savePlaylistCurrent
        // 
        this.pb_savePlaylistCurrent.ErrorImage = null;
        this.pb_savePlaylistCurrent.Image = global::ServerManager.Properties.Resources.save;
        this.pb_savePlaylistCurrent.InitialImage = null;
        this.pb_savePlaylistCurrent.Location = new System.Drawing.Point(31, 4);
        this.pb_savePlaylistCurrent.Name = "pb_savePlaylistCurrent";
        this.pb_savePlaylistCurrent.Size = new System.Drawing.Size(15, 15);
        this.pb_savePlaylistCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pb_savePlaylistCurrent.TabIndex = 0;
        this.pb_savePlaylistCurrent.TabStop = false;
        // 
        // panel9
        // 
        this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel9.Location = new System.Drawing.Point(0, 0);
        this.panel9.Name = "panel9";
        this.panel9.Size = new System.Drawing.Size(100, 25);
        this.panel9.TabIndex = 0;
        // 
        // tabPage_mapEditors
        // 
        this.tabPage_mapEditors.Controls.Add(this.PlayListEditor);
        this.tabPage_mapEditors.Controls.Add(this.panel10);
        this.tabPage_mapEditors.Location = new System.Drawing.Point(4, 22);
        this.tabPage_mapEditors.Name = "tabPage_mapEditors";
        this.tabPage_mapEditors.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_mapEditors.Size = new System.Drawing.Size(417, 428);
        this.tabPage_mapEditors.TabIndex = 1;
        this.tabPage_mapEditors.Text = "Playlist Editor";
        this.tabPage_mapEditors.UseVisualStyleBackColor = true;
        // 
        // PlayListEditor
        // 
        this.PlayListEditor.AllowDrop = true;
        this.PlayListEditor.AllowUserToAddRows = false;
        this.PlayListEditor.AllowUserToResizeColumns = false;
        this.PlayListEditor.AllowUserToResizeRows = false;
        this.PlayListEditor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.PlayListEditor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
        this.PlayListEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.PlayListEditor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11, this.dataGridViewTextBoxColumn12, this.dataGridViewTextBoxColumn37, this.dataGridViewTextBoxColumn38 });
        dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.PlayListEditor.DefaultCellStyle = dataGridViewCellStyle8;
        this.PlayListEditor.Dock = System.Windows.Forms.DockStyle.Fill;
        this.PlayListEditor.Location = new System.Drawing.Point(3, 28);
        this.PlayListEditor.Name = "PlayListEditor";
        this.PlayListEditor.ReadOnly = true;
        dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.PlayListEditor.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
        dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.PlayListEditor.RowsDefaultCellStyle = dataGridViewCellStyle10;
        this.PlayListEditor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.PlayListEditor.Size = new System.Drawing.Size(411, 397);
        this.PlayListEditor.TabIndex = 5;
        // 
        // dataGridViewTextBoxColumn8
        // 
        this.dataGridViewTextBoxColumn8.HeaderText = "Id";
        this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
        this.dataGridViewTextBoxColumn8.ReadOnly = true;
        this.dataGridViewTextBoxColumn8.Visible = false;
        // 
        // dataGridViewTextBoxColumn9
        // 
        this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn9.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
        this.dataGridViewTextBoxColumn9.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn10
        // 
        this.dataGridViewTextBoxColumn10.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
        this.dataGridViewTextBoxColumn10.ReadOnly = true;
        this.dataGridViewTextBoxColumn10.Visible = false;
        // 
        // dataGridViewTextBoxColumn11
        // 
        dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle7;
        this.dataGridViewTextBoxColumn11.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
        this.dataGridViewTextBoxColumn11.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn12
        // 
        this.dataGridViewTextBoxColumn12.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
        this.dataGridViewTextBoxColumn12.ReadOnly = true;
        this.dataGridViewTextBoxColumn12.Visible = false;
        // 
        // dataGridViewTextBoxColumn37
        // 
        this.dataGridViewTextBoxColumn37.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
        this.dataGridViewTextBoxColumn37.ReadOnly = true;
        this.dataGridViewTextBoxColumn37.Visible = false;
        // 
        // dataGridViewTextBoxColumn38
        // 
        this.dataGridViewTextBoxColumn38.HeaderText = "GameType ID";
        this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
        this.dataGridViewTextBoxColumn38.ReadOnly = true;
        this.dataGridViewTextBoxColumn38.Visible = false;
        // 
        // panel10
        // 
        this.panel10.Controls.Add(this.panel11);
        this.panel10.Controls.Add(this.panel12);
        this.panel10.Controls.Add(this.panel13);
        this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel10.Location = new System.Drawing.Point(3, 3);
        this.panel10.Name = "panel10";
        this.panel10.Size = new System.Drawing.Size(411, 25);
        this.panel10.TabIndex = 1;
        // 
        // panel11
        // 
        this.panel11.Controls.Add(this.comboBox_AvailableMapLists);
        this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel11.Location = new System.Drawing.Point(100, 0);
        this.panel11.Name = "panel11";
        this.panel11.Size = new System.Drawing.Size(211, 25);
        this.panel11.TabIndex = 2;
        // 
        // comboBox_AvailableMapLists
        // 
        this.comboBox_AvailableMapLists.Dock = System.Windows.Forms.DockStyle.Fill;
        this.comboBox_AvailableMapLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.comboBox_AvailableMapLists.FormattingEnabled = true;
        this.comboBox_AvailableMapLists.Location = new System.Drawing.Point(0, 0);
        this.comboBox_AvailableMapLists.Name = "comboBox_AvailableMapLists";
        this.comboBox_AvailableMapLists.Size = new System.Drawing.Size(211, 24);
        this.comboBox_AvailableMapLists.TabIndex = 0;
        // 
        // panel12
        // 
        this.panel12.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel12.Location = new System.Drawing.Point(311, 0);
        this.panel12.Name = "panel12";
        this.panel12.Size = new System.Drawing.Size(100, 25);
        this.panel12.TabIndex = 1;
        // 
        // panel13
        // 
        this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel13.Location = new System.Drawing.Point(0, 0);
        this.panel13.Name = "panel13";
        this.panel13.Size = new System.Drawing.Size(100, 25);
        this.panel13.TabIndex = 0;
        // 
        // panel_mapsLeft
        // 
        this.panel_mapsLeft.Controls.Add(this.maps_tabControl1);
        this.panel_mapsLeft.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel_mapsLeft.Location = new System.Drawing.Point(0, 0);
        this.panel_mapsLeft.Name = "panel_mapsLeft";
        this.panel_mapsLeft.Size = new System.Drawing.Size(425, 454);
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
        this.maps_tabControl1.Size = new System.Drawing.Size(425, 454);
        this.maps_tabControl1.TabIndex = 0;
        // 
        // tabPage_AvailMaps
        // 
        this.tabPage_AvailMaps.Controls.Add(this.panel2);
        this.tabPage_AvailMaps.Controls.Add(this.panel1);
        this.tabPage_AvailMaps.Location = new System.Drawing.Point(4, 25);
        this.tabPage_AvailMaps.Name = "tabPage_AvailMaps";
        this.tabPage_AvailMaps.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_AvailMaps.Size = new System.Drawing.Size(417, 425);
        this.tabPage_AvailMaps.TabIndex = 0;
        this.tabPage_AvailMaps.Text = "Available Maps";
        this.tabPage_AvailMaps.UseVisualStyleBackColor = true;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.AvailableMapList);
        this.panel2.Controls.Add(this.dataGridView3);
        this.panel2.Controls.Add(this.dataGridView2);
        this.panel2.Controls.Add(this.dataGridView1);
        this.panel2.Controls.Add(this.AvailableMaps);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(3, 28);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(411, 394);
        this.panel2.TabIndex = 1;
        // 
        // AvailableMapList
        // 
        this.AvailableMapList.AllowUserToAddRows = false;
        this.AvailableMapList.AllowUserToDeleteRows = false;
        this.AvailableMapList.AllowUserToResizeColumns = false;
        this.AvailableMapList.AllowUserToResizeRows = false;
        this.AvailableMapList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.AvailableMapList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
        this.AvailableMapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.AvailableMapList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn31, this.dataGridViewTextBoxColumn32, this.dataGridViewTextBoxColumn33, this.dataGridViewTextBoxColumn35, this.dataGridViewTextBoxColumn34, this.dataGridViewTextBoxColumn36, this.gametype_id });
        this.AvailableMapList.Dock = System.Windows.Forms.DockStyle.Fill;
        this.AvailableMapList.Location = new System.Drawing.Point(0, 0);
        this.AvailableMapList.Name = "AvailableMapList";
        this.AvailableMapList.ReadOnly = true;
        dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.AvailableMapList.RowsDefaultCellStyle = dataGridViewCellStyle13;
        this.AvailableMapList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.AvailableMapList.Size = new System.Drawing.Size(411, 394);
        this.AvailableMapList.TabIndex = 4;
        // 
        // dataGridViewTextBoxColumn31
        // 
        this.dataGridViewTextBoxColumn31.HeaderText = "Id";
        this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
        this.dataGridViewTextBoxColumn31.ReadOnly = true;
        this.dataGridViewTextBoxColumn31.Visible = false;
        // 
        // dataGridViewTextBoxColumn32
        // 
        this.dataGridViewTextBoxColumn32.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn32.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
        this.dataGridViewTextBoxColumn32.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn33
        // 
        this.dataGridViewTextBoxColumn33.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
        this.dataGridViewTextBoxColumn33.ReadOnly = true;
        this.dataGridViewTextBoxColumn33.Visible = false;
        // 
        // dataGridViewTextBoxColumn35
        // 
        dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        this.dataGridViewTextBoxColumn35.DefaultCellStyle = dataGridViewCellStyle12;
        this.dataGridViewTextBoxColumn35.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
        this.dataGridViewTextBoxColumn35.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn34
        // 
        this.dataGridViewTextBoxColumn34.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
        this.dataGridViewTextBoxColumn34.ReadOnly = true;
        this.dataGridViewTextBoxColumn34.Visible = false;
        // 
        // dataGridViewTextBoxColumn36
        // 
        this.dataGridViewTextBoxColumn36.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
        this.dataGridViewTextBoxColumn36.ReadOnly = true;
        this.dataGridViewTextBoxColumn36.Visible = false;
        // 
        // gametype_id
        // 
        this.gametype_id.HeaderText = "GameType ID";
        this.gametype_id.Name = "gametype_id";
        this.gametype_id.ReadOnly = true;
        this.gametype_id.Visible = false;
        // 
        // dataGridView3
        // 
        dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
        this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn25, this.dataGridViewTextBoxColumn26, this.dataGridViewTextBoxColumn27, this.dataGridViewTextBoxColumn28, this.dataGridViewTextBoxColumn29, this.dataGridViewTextBoxColumn30 });
        this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dataGridView3.Location = new System.Drawing.Point(0, 0);
        this.dataGridView3.Name = "dataGridView3";
        this.dataGridView3.ReadOnly = true;
        dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.dataGridView3.RowsDefaultCellStyle = dataGridViewCellStyle15;
        this.dataGridView3.Size = new System.Drawing.Size(411, 394);
        this.dataGridView3.TabIndex = 3;
        // 
        // dataGridViewTextBoxColumn25
        // 
        this.dataGridViewTextBoxColumn25.HeaderText = "Id";
        this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
        this.dataGridViewTextBoxColumn25.ReadOnly = true;
        this.dataGridViewTextBoxColumn25.Visible = false;
        // 
        // dataGridViewTextBoxColumn26
        // 
        this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn26.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
        this.dataGridViewTextBoxColumn26.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn27
        // 
        this.dataGridViewTextBoxColumn27.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
        this.dataGridViewTextBoxColumn27.ReadOnly = true;
        this.dataGridViewTextBoxColumn27.Visible = false;
        // 
        // dataGridViewTextBoxColumn28
        // 
        this.dataGridViewTextBoxColumn28.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
        this.dataGridViewTextBoxColumn28.ReadOnly = true;
        this.dataGridViewTextBoxColumn28.Visible = false;
        // 
        // dataGridViewTextBoxColumn29
        // 
        this.dataGridViewTextBoxColumn29.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
        this.dataGridViewTextBoxColumn29.ReadOnly = true;
        this.dataGridViewTextBoxColumn29.Width = 150;
        // 
        // dataGridViewTextBoxColumn30
        // 
        this.dataGridViewTextBoxColumn30.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
        this.dataGridViewTextBoxColumn30.ReadOnly = true;
        this.dataGridViewTextBoxColumn30.Visible = false;
        // 
        // dataGridView2
        // 
        dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
        this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn19, this.dataGridViewTextBoxColumn20, this.dataGridViewTextBoxColumn21, this.dataGridViewTextBoxColumn22, this.dataGridViewTextBoxColumn23, this.dataGridViewTextBoxColumn24 });
        this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dataGridView2.Location = new System.Drawing.Point(0, 0);
        this.dataGridView2.Name = "dataGridView2";
        this.dataGridView2.ReadOnly = true;
        dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.dataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle17;
        this.dataGridView2.Size = new System.Drawing.Size(411, 394);
        this.dataGridView2.TabIndex = 2;
        // 
        // dataGridViewTextBoxColumn19
        // 
        this.dataGridViewTextBoxColumn19.HeaderText = "Id";
        this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
        this.dataGridViewTextBoxColumn19.ReadOnly = true;
        this.dataGridViewTextBoxColumn19.Visible = false;
        // 
        // dataGridViewTextBoxColumn20
        // 
        this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn20.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
        this.dataGridViewTextBoxColumn20.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn21
        // 
        this.dataGridViewTextBoxColumn21.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
        this.dataGridViewTextBoxColumn21.ReadOnly = true;
        this.dataGridViewTextBoxColumn21.Visible = false;
        // 
        // dataGridViewTextBoxColumn22
        // 
        this.dataGridViewTextBoxColumn22.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
        this.dataGridViewTextBoxColumn22.ReadOnly = true;
        this.dataGridViewTextBoxColumn22.Visible = false;
        // 
        // dataGridViewTextBoxColumn23
        // 
        this.dataGridViewTextBoxColumn23.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
        this.dataGridViewTextBoxColumn23.ReadOnly = true;
        this.dataGridViewTextBoxColumn23.Width = 150;
        // 
        // dataGridViewTextBoxColumn24
        // 
        this.dataGridViewTextBoxColumn24.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
        this.dataGridViewTextBoxColumn24.ReadOnly = true;
        this.dataGridViewTextBoxColumn24.Visible = false;
        // 
        // dataGridView1
        // 
        dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
        this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dataGridViewTextBoxColumn13, this.dataGridViewTextBoxColumn14, this.dataGridViewTextBoxColumn15, this.dataGridViewTextBoxColumn16, this.dataGridViewTextBoxColumn17, this.dataGridViewTextBoxColumn18 });
        this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dataGridView1.Location = new System.Drawing.Point(0, 0);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle19;
        this.dataGridView1.Size = new System.Drawing.Size(411, 394);
        this.dataGridView1.TabIndex = 1;
        // 
        // dataGridViewTextBoxColumn13
        // 
        this.dataGridViewTextBoxColumn13.HeaderText = "Id";
        this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
        this.dataGridViewTextBoxColumn13.ReadOnly = true;
        this.dataGridViewTextBoxColumn13.Visible = false;
        // 
        // dataGridViewTextBoxColumn14
        // 
        this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.dataGridViewTextBoxColumn14.HeaderText = "Map Name";
        this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
        this.dataGridViewTextBoxColumn14.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn15
        // 
        this.dataGridViewTextBoxColumn15.HeaderText = "File Name";
        this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
        this.dataGridViewTextBoxColumn15.ReadOnly = true;
        this.dataGridViewTextBoxColumn15.Visible = false;
        // 
        // dataGridViewTextBoxColumn16
        // 
        this.dataGridViewTextBoxColumn16.HeaderText = "Game Mod";
        this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
        this.dataGridViewTextBoxColumn16.ReadOnly = true;
        this.dataGridViewTextBoxColumn16.Visible = false;
        // 
        // dataGridViewTextBoxColumn17
        // 
        this.dataGridViewTextBoxColumn17.HeaderText = "Game Type";
        this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
        this.dataGridViewTextBoxColumn17.ReadOnly = true;
        this.dataGridViewTextBoxColumn17.Width = 150;
        // 
        // dataGridViewTextBoxColumn18
        // 
        this.dataGridViewTextBoxColumn18.HeaderText = "Custom Map";
        this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
        this.dataGridViewTextBoxColumn18.ReadOnly = true;
        this.dataGridViewTextBoxColumn18.Visible = false;
        // 
        // AvailableMaps
        // 
        dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.AvailableMaps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
        this.AvailableMaps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.AvailableMaps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.Id, this.mission_name, this.mission_file, this.game, this.GameType, this.CustomMap });
        this.AvailableMaps.Dock = System.Windows.Forms.DockStyle.Fill;
        this.AvailableMaps.Location = new System.Drawing.Point(0, 0);
        this.AvailableMaps.Name = "AvailableMaps";
        this.AvailableMaps.ReadOnly = true;
        dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.AvailableMaps.RowsDefaultCellStyle = dataGridViewCellStyle21;
        this.AvailableMaps.Size = new System.Drawing.Size(411, 394);
        this.AvailableMaps.TabIndex = 0;
        // 
        // Id
        // 
        this.Id.HeaderText = "Id";
        this.Id.Name = "Id";
        this.Id.ReadOnly = true;
        this.Id.Visible = false;
        // 
        // mission_name
        // 
        this.mission_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.mission_name.HeaderText = "Map Name";
        this.mission_name.Name = "mission_name";
        this.mission_name.ReadOnly = true;
        // 
        // mission_file
        // 
        this.mission_file.HeaderText = "File Name";
        this.mission_file.Name = "mission_file";
        this.mission_file.ReadOnly = true;
        this.mission_file.Visible = false;
        // 
        // game
        // 
        this.game.HeaderText = "Game Mod";
        this.game.Name = "game";
        this.game.ReadOnly = true;
        this.game.Visible = false;
        // 
        // GameType
        // 
        this.GameType.HeaderText = "Game Type";
        this.GameType.Name = "GameType";
        this.GameType.ReadOnly = true;
        this.GameType.Width = 150;
        // 
        // CustomMap
        // 
        this.CustomMap.HeaderText = "Custom Map";
        this.CustomMap.Name = "CustomMap";
        this.CustomMap.ReadOnly = true;
        this.CustomMap.Visible = false;
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.panel5);
        this.panel1.Controls.Add(this.panel4);
        this.panel1.Controls.Add(this.panel3);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel1.Location = new System.Drawing.Point(3, 3);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(411, 25);
        this.panel1.TabIndex = 0;
        // 
        // panel5
        // 
        this.panel5.Controls.Add(this.comboBox_gameTypes);
        this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel5.Location = new System.Drawing.Point(100, 0);
        this.panel5.Name = "panel5";
        this.panel5.Size = new System.Drawing.Size(211, 25);
        this.panel5.TabIndex = 2;
        // 
        // comboBox_gameTypes
        // 
        this.comboBox_gameTypes.Dock = System.Windows.Forms.DockStyle.Fill;
        this.comboBox_gameTypes.FormattingEnabled = true;
        this.comboBox_gameTypes.Location = new System.Drawing.Point(0, 0);
        this.comboBox_gameTypes.Name = "comboBox_gameTypes";
        this.comboBox_gameTypes.Size = new System.Drawing.Size(211, 24);
        this.comboBox_gameTypes.TabIndex = 0;
        this.comboBox_gameTypes.SelectedIndexChanged += new System.EventHandler(this.onChange_GameTypeCombo);
        // 
        // panel4
        // 
        this.panel4.Controls.Add(this.pictureBox_refresh);
        this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel4.Location = new System.Drawing.Point(311, 0);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(100, 25);
        this.panel4.TabIndex = 1;
        // 
        // pictureBox_refresh
        // 
        this.pictureBox_refresh.Cursor = System.Windows.Forms.Cursors.Hand;
        this.pictureBox_refresh.ErrorImage = null;
        this.pictureBox_refresh.Image = global::ServerManager.Properties.Resources.arrowsRotate;
        this.pictureBox_refresh.InitialImage = null;
        this.pictureBox_refresh.Location = new System.Drawing.Point(8, 4);
        this.pictureBox_refresh.Name = "pictureBox_refresh";
        this.pictureBox_refresh.Size = new System.Drawing.Size(15, 15);
        this.pictureBox_refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_refresh.TabIndex = 0;
        this.pictureBox_refresh.TabStop = false;
        this.pictureBox_refresh.Click += new System.EventHandler(this.onClick_refreshMaps);
        // 
        // panel3
        // 
        this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
        this.panel3.Location = new System.Drawing.Point(0, 0);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(100, 25);
        this.panel3.TabIndex = 0;
        // 
        // tabPage_Schedule
        // 
        this.tabPage_Schedule.Location = new System.Drawing.Point(4, 25);
        this.tabPage_Schedule.Name = "tabPage_Schedule";
        this.tabPage_Schedule.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_Schedule.Size = new System.Drawing.Size(417, 425);
        this.tabPage_Schedule.TabIndex = 2;
        this.tabPage_Schedule.Text = "Scheduling";
        this.tabPage_Schedule.UseVisualStyleBackColor = true;
        // 
        // tabPage_MapSettings
        // 
        this.tabPage_MapSettings.Location = new System.Drawing.Point(4, 25);
        this.tabPage_MapSettings.Name = "tabPage_MapSettings";
        this.tabPage_MapSettings.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_MapSettings.Size = new System.Drawing.Size(417, 425);
        this.tabPage_MapSettings.TabIndex = 1;
        this.tabPage_MapSettings.Text = "Settings";
        this.tabPage_MapSettings.UseVisualStyleBackColor = true;
        // 
        // tabPage_messaging
        // 
        this.tabPage_messaging.Controls.Add(this.spacing_chat);
        this.tabPage_messaging.Location = new System.Drawing.Point(4, 34);
        this.tabPage_messaging.Name = "tabPage_messaging";
        this.tabPage_messaging.Padding = new System.Windows.Forms.Padding(20);
        this.tabPage_messaging.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_messaging.TabIndex = 2;
        this.tabPage_messaging.Text = "Messaging";
        this.tabPage_messaging.UseVisualStyleBackColor = true;
        // 
        // spacing_chat
        // 
        this.spacing_chat.Controls.Add(this.tabControl_messaging);
        this.spacing_chat.Dock = System.Windows.Forms.DockStyle.Fill;
        this.spacing_chat.Location = new System.Drawing.Point(20, 20);
        this.spacing_chat.Name = "spacing_chat";
        this.spacing_chat.Size = new System.Drawing.Size(985, 454);
        this.spacing_chat.TabIndex = 0;
        // 
        // tabControl_messaging
        // 
        this.tabControl_messaging.Controls.Add(this.tabPage_chatMsg);
        this.tabControl_messaging.Controls.Add(this.tabPage_chatMod);
        this.tabControl_messaging.Controls.Add(this.tabPage_serverMsg);
        this.tabControl_messaging.Controls.Add(this.tabPage_schedMsg);
        this.tabControl_messaging.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl_messaging.Location = new System.Drawing.Point(0, 0);
        this.tabControl_messaging.Name = "tabControl_messaging";
        this.tabControl_messaging.SelectedIndex = 0;
        this.tabControl_messaging.Size = new System.Drawing.Size(985, 454);
        this.tabControl_messaging.TabIndex = 0;
        // 
        // tabPage_chatMsg
        // 
        this.tabPage_chatMsg.Location = new System.Drawing.Point(4, 25);
        this.tabPage_chatMsg.Name = "tabPage_chatMsg";
        this.tabPage_chatMsg.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_chatMsg.Size = new System.Drawing.Size(977, 425);
        this.tabPage_chatMsg.TabIndex = 0;
        this.tabPage_chatMsg.Text = "Chat";
        this.tabPage_chatMsg.UseVisualStyleBackColor = true;
        // 
        // tabPage_chatMod
        // 
        this.tabPage_chatMod.Location = new System.Drawing.Point(4, 25);
        this.tabPage_chatMod.Name = "tabPage_chatMod";
        this.tabPage_chatMod.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_chatMod.Size = new System.Drawing.Size(977, 425);
        this.tabPage_chatMod.TabIndex = 3;
        this.tabPage_chatMod.Text = "Moderate";
        this.tabPage_chatMod.UseVisualStyleBackColor = true;
        // 
        // tabPage_serverMsg
        // 
        this.tabPage_serverMsg.Location = new System.Drawing.Point(4, 25);
        this.tabPage_serverMsg.Name = "tabPage_serverMsg";
        this.tabPage_serverMsg.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_serverMsg.Size = new System.Drawing.Size(977, 425);
        this.tabPage_serverMsg.TabIndex = 1;
        this.tabPage_serverMsg.Text = "Server Messages";
        this.tabPage_serverMsg.UseVisualStyleBackColor = true;
        // 
        // tabPage_schedMsg
        // 
        this.tabPage_schedMsg.Location = new System.Drawing.Point(4, 25);
        this.tabPage_schedMsg.Name = "tabPage_schedMsg";
        this.tabPage_schedMsg.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage_schedMsg.Size = new System.Drawing.Size(977, 425);
        this.tabPage_schedMsg.TabIndex = 2;
        this.tabPage_schedMsg.Text = "Schedule Messages";
        this.tabPage_schedMsg.UseVisualStyleBackColor = true;
        // 
        // tabPage_playerManagement
        // 
        this.tabPage_playerManagement.Controls.Add(this.spacing_players);
        this.tabPage_playerManagement.Location = new System.Drawing.Point(4, 34);
        this.tabPage_playerManagement.Name = "tabPage_playerManagement";
        this.tabPage_playerManagement.Padding = new System.Windows.Forms.Padding(20, 5, 20, 5);
        this.tabPage_playerManagement.Size = new System.Drawing.Size(1025, 494);
        this.tabPage_playerManagement.TabIndex = 3;
        this.tabPage_playerManagement.Text = "Players";
        this.tabPage_playerManagement.UseVisualStyleBackColor = true;
        // 
        // spacing_players
        // 
        this.spacing_players.Controls.Add(this.groupBox_playerDetails);
        this.spacing_players.Controls.Add(this.groupBox_playerBox);
        this.spacing_players.Dock = System.Windows.Forms.DockStyle.Fill;
        this.spacing_players.Location = new System.Drawing.Point(20, 5);
        this.spacing_players.Name = "spacing_players";
        this.spacing_players.Size = new System.Drawing.Size(985, 484);
        this.spacing_players.TabIndex = 0;
        // 
        // groupBox_playerDetails
        // 
        this.groupBox_playerDetails.Dock = System.Windows.Forms.DockStyle.Right;
        this.groupBox_playerDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.groupBox_playerDetails.Location = new System.Drawing.Point(807, 0);
        this.groupBox_playerDetails.Name = "groupBox_playerDetails";
        this.groupBox_playerDetails.Size = new System.Drawing.Size(178, 484);
        this.groupBox_playerDetails.TabIndex = 52;
        this.groupBox_playerDetails.TabStop = false;
        this.groupBox_playerDetails.Text = "Player Details";
        // 
        // groupBox_playerBox
        // 
        this.groupBox_playerBox.Controls.Add(this.slot_01);
        this.groupBox_playerBox.Controls.Add(this.slot_50);
        this.groupBox_playerBox.Controls.Add(this.slot_11);
        this.groupBox_playerBox.Controls.Add(this.slot_40);
        this.groupBox_playerBox.Controls.Add(this.slot_02);
        this.groupBox_playerBox.Controls.Add(this.slot_49);
        this.groupBox_playerBox.Controls.Add(this.slot_03);
        this.groupBox_playerBox.Controls.Add(this.slot_30);
        this.groupBox_playerBox.Controls.Add(this.slot_04);
        this.groupBox_playerBox.Controls.Add(this.slot_48);
        this.groupBox_playerBox.Controls.Add(this.slot_05);
        this.groupBox_playerBox.Controls.Add(this.slot_39);
        this.groupBox_playerBox.Controls.Add(this.slot_06);
        this.groupBox_playerBox.Controls.Add(this.slot_47);
        this.groupBox_playerBox.Controls.Add(this.slot_07);
        this.groupBox_playerBox.Controls.Add(this.slot_20);
        this.groupBox_playerBox.Controls.Add(this.slot_08);
        this.groupBox_playerBox.Controls.Add(this.slot_46);
        this.groupBox_playerBox.Controls.Add(this.slot_09);
        this.groupBox_playerBox.Controls.Add(this.slot_38);
        this.groupBox_playerBox.Controls.Add(this.slot_10);
        this.groupBox_playerBox.Controls.Add(this.slot_45);
        this.groupBox_playerBox.Controls.Add(this.slot_21);
        this.groupBox_playerBox.Controls.Add(this.slot_29);
        this.groupBox_playerBox.Controls.Add(this.slot_12);
        this.groupBox_playerBox.Controls.Add(this.slot_44);
        this.groupBox_playerBox.Controls.Add(this.slot_22);
        this.groupBox_playerBox.Controls.Add(this.slot_37);
        this.groupBox_playerBox.Controls.Add(this.slot_13);
        this.groupBox_playerBox.Controls.Add(this.slot_43);
        this.groupBox_playerBox.Controls.Add(this.slot_23);
        this.groupBox_playerBox.Controls.Add(this.slot_19);
        this.groupBox_playerBox.Controls.Add(this.slot_14);
        this.groupBox_playerBox.Controls.Add(this.slot_42);
        this.groupBox_playerBox.Controls.Add(this.slot_24);
        this.groupBox_playerBox.Controls.Add(this.slot_36);
        this.groupBox_playerBox.Controls.Add(this.slot_15);
        this.groupBox_playerBox.Controls.Add(this.slot_41);
        this.groupBox_playerBox.Controls.Add(this.slot_25);
        this.groupBox_playerBox.Controls.Add(this.slot_28);
        this.groupBox_playerBox.Controls.Add(this.slot_16);
        this.groupBox_playerBox.Controls.Add(this.slot_35);
        this.groupBox_playerBox.Controls.Add(this.slot_31);
        this.groupBox_playerBox.Controls.Add(this.slot_18);
        this.groupBox_playerBox.Controls.Add(this.slot_26);
        this.groupBox_playerBox.Controls.Add(this.slot_34);
        this.groupBox_playerBox.Controls.Add(this.slot_32);
        this.groupBox_playerBox.Controls.Add(this.slot_27);
        this.groupBox_playerBox.Controls.Add(this.slot_17);
        this.groupBox_playerBox.Controls.Add(this.slot_33);
        this.groupBox_playerBox.Dock = System.Windows.Forms.DockStyle.Left;
        this.groupBox_playerBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.groupBox_playerBox.Location = new System.Drawing.Point(0, 0);
        this.groupBox_playerBox.Name = "groupBox_playerBox";
        this.groupBox_playerBox.Padding = new System.Windows.Forms.Padding(0);
        this.groupBox_playerBox.Size = new System.Drawing.Size(804, 484);
        this.groupBox_playerBox.TabIndex = 51;
        this.groupBox_playerBox.TabStop = false;
        this.groupBox_playerBox.Text = "Player List";
        // 
        // slot_01
        // 
        this.slot_01.BackgroundImage = global::ServerManager.Properties.Resources.playerTileBlack;
        this.slot_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_01.Controls.Add(this.label_01_ip);
        this.slot_01.Controls.Add(this.label_01_playerName);
        this.slot_01.Controls.Add(this.label_01_slot);
        this.slot_01.Cursor = System.Windows.Forms.Cursors.Hand;
        this.slot_01.Location = new System.Drawing.Point(14, 19);
        this.slot_01.Name = "slot_01";
        this.slot_01.Size = new System.Drawing.Size(150, 40);
        this.slot_01.TabIndex = 1;
        // 
        // label_01_ip
        // 
        this.label_01_ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_01_ip.Location = new System.Drawing.Point(60, 19);
        this.label_01_ip.Name = "label_01_ip";
        this.label_01_ip.Size = new System.Drawing.Size(75, 14);
        this.label_01_ip.TabIndex = 2;
        this.label_01_ip.Text = "000.000.000.000";
        this.label_01_ip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_01_playerName
        // 
        this.label_01_playerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_01_playerName.Location = new System.Drawing.Point(35, 5);
        this.label_01_playerName.Margin = new System.Windows.Forms.Padding(0);
        this.label_01_playerName.Name = "label_01_playerName";
        this.label_01_playerName.Size = new System.Drawing.Size(100, 14);
        this.label_01_playerName.TabIndex = 1;
        this.label_01_playerName.Text = "PlayerNameHere";
        this.label_01_playerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_01_slot
        // 
        this.label_01_slot.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_01_slot.ForeColor = System.Drawing.SystemColors.Desktop;
        this.label_01_slot.Location = new System.Drawing.Point(24, 21);
        this.label_01_slot.Name = "label_01_slot";
        this.label_01_slot.Size = new System.Drawing.Size(24, 20);
        this.label_01_slot.TabIndex = 0;
        this.label_01_slot.Text = "01";
        this.label_01_slot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // slot_50
        // 
        this.slot_50.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_50.Location = new System.Drawing.Point(638, 433);
        this.slot_50.Name = "slot_50";
        this.slot_50.Size = new System.Drawing.Size(150, 40);
        this.slot_50.TabIndex = 50;
        // 
        // slot_11
        // 
        this.slot_11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_11.Location = new System.Drawing.Point(170, 19);
        this.slot_11.Name = "slot_11";
        this.slot_11.Size = new System.Drawing.Size(150, 40);
        this.slot_11.TabIndex = 11;
        // 
        // slot_40
        // 
        this.slot_40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_40.Location = new System.Drawing.Point(482, 433);
        this.slot_40.Name = "slot_40";
        this.slot_40.Size = new System.Drawing.Size(150, 40);
        this.slot_40.TabIndex = 40;
        // 
        // slot_02
        // 
        this.slot_02.BackgroundImage = global::ServerManager.Properties.Resources.playerTileBlue;
        this.slot_02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_02.Controls.Add(this.label_slot02);
        this.slot_02.Location = new System.Drawing.Point(14, 65);
        this.slot_02.Name = "slot_02";
        this.slot_02.Size = new System.Drawing.Size(150, 40);
        this.slot_02.TabIndex = 2;
        // 
        // label_slot02
        // 
        this.label_slot02.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_slot02.ForeColor = System.Drawing.SystemColors.Desktop;
        this.label_slot02.Location = new System.Drawing.Point(24, 21);
        this.label_slot02.Name = "label_slot02";
        this.label_slot02.Size = new System.Drawing.Size(24, 20);
        this.label_slot02.TabIndex = 1;
        this.label_slot02.Text = "02";
        this.label_slot02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // slot_49
        // 
        this.slot_49.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_49.Location = new System.Drawing.Point(638, 387);
        this.slot_49.Name = "slot_49";
        this.slot_49.Size = new System.Drawing.Size(150, 40);
        this.slot_49.TabIndex = 49;
        // 
        // slot_03
        // 
        this.slot_03.BackgroundImage = global::ServerManager.Properties.Resources.playerTileRed;
        this.slot_03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_03.Controls.Add(this.label_slot03);
        this.slot_03.Location = new System.Drawing.Point(14, 111);
        this.slot_03.Name = "slot_03";
        this.slot_03.Size = new System.Drawing.Size(150, 40);
        this.slot_03.TabIndex = 3;
        // 
        // label_slot03
        // 
        this.label_slot03.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_slot03.ForeColor = System.Drawing.SystemColors.Desktop;
        this.label_slot03.Location = new System.Drawing.Point(24, 21);
        this.label_slot03.Name = "label_slot03";
        this.label_slot03.Size = new System.Drawing.Size(24, 20);
        this.label_slot03.TabIndex = 2;
        this.label_slot03.Text = "03";
        this.label_slot03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // slot_30
        // 
        this.slot_30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_30.Location = new System.Drawing.Point(326, 433);
        this.slot_30.Name = "slot_30";
        this.slot_30.Size = new System.Drawing.Size(150, 40);
        this.slot_30.TabIndex = 30;
        // 
        // slot_04
        // 
        this.slot_04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_04.Location = new System.Drawing.Point(14, 157);
        this.slot_04.Name = "slot_04";
        this.slot_04.Size = new System.Drawing.Size(150, 40);
        this.slot_04.TabIndex = 4;
        // 
        // slot_48
        // 
        this.slot_48.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_48.Location = new System.Drawing.Point(638, 341);
        this.slot_48.Name = "slot_48";
        this.slot_48.Size = new System.Drawing.Size(150, 40);
        this.slot_48.TabIndex = 48;
        // 
        // slot_05
        // 
        this.slot_05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_05.Location = new System.Drawing.Point(14, 203);
        this.slot_05.Name = "slot_05";
        this.slot_05.Size = new System.Drawing.Size(150, 40);
        this.slot_05.TabIndex = 5;
        // 
        // slot_39
        // 
        this.slot_39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_39.Location = new System.Drawing.Point(482, 387);
        this.slot_39.Name = "slot_39";
        this.slot_39.Size = new System.Drawing.Size(150, 40);
        this.slot_39.TabIndex = 39;
        // 
        // slot_06
        // 
        this.slot_06.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_06.Location = new System.Drawing.Point(14, 249);
        this.slot_06.Name = "slot_06";
        this.slot_06.Size = new System.Drawing.Size(150, 40);
        this.slot_06.TabIndex = 6;
        // 
        // slot_47
        // 
        this.slot_47.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_47.Location = new System.Drawing.Point(638, 295);
        this.slot_47.Name = "slot_47";
        this.slot_47.Size = new System.Drawing.Size(150, 40);
        this.slot_47.TabIndex = 47;
        // 
        // slot_07
        // 
        this.slot_07.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_07.Location = new System.Drawing.Point(14, 295);
        this.slot_07.Name = "slot_07";
        this.slot_07.Size = new System.Drawing.Size(150, 40);
        this.slot_07.TabIndex = 7;
        // 
        // slot_20
        // 
        this.slot_20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_20.Location = new System.Drawing.Point(170, 433);
        this.slot_20.Name = "slot_20";
        this.slot_20.Size = new System.Drawing.Size(150, 40);
        this.slot_20.TabIndex = 20;
        // 
        // slot_08
        // 
        this.slot_08.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_08.Location = new System.Drawing.Point(14, 341);
        this.slot_08.Name = "slot_08";
        this.slot_08.Size = new System.Drawing.Size(150, 40);
        this.slot_08.TabIndex = 8;
        // 
        // slot_46
        // 
        this.slot_46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_46.Location = new System.Drawing.Point(638, 249);
        this.slot_46.Name = "slot_46";
        this.slot_46.Size = new System.Drawing.Size(150, 40);
        this.slot_46.TabIndex = 46;
        // 
        // slot_09
        // 
        this.slot_09.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_09.Location = new System.Drawing.Point(14, 387);
        this.slot_09.Name = "slot_09";
        this.slot_09.Size = new System.Drawing.Size(150, 40);
        this.slot_09.TabIndex = 9;
        // 
        // slot_38
        // 
        this.slot_38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_38.Location = new System.Drawing.Point(482, 341);
        this.slot_38.Name = "slot_38";
        this.slot_38.Size = new System.Drawing.Size(150, 40);
        this.slot_38.TabIndex = 38;
        // 
        // slot_10
        // 
        this.slot_10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_10.Location = new System.Drawing.Point(14, 433);
        this.slot_10.Name = "slot_10";
        this.slot_10.Size = new System.Drawing.Size(150, 40);
        this.slot_10.TabIndex = 10;
        // 
        // slot_45
        // 
        this.slot_45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_45.Location = new System.Drawing.Point(638, 203);
        this.slot_45.Name = "slot_45";
        this.slot_45.Size = new System.Drawing.Size(150, 40);
        this.slot_45.TabIndex = 45;
        // 
        // slot_21
        // 
        this.slot_21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_21.Location = new System.Drawing.Point(326, 19);
        this.slot_21.Name = "slot_21";
        this.slot_21.Size = new System.Drawing.Size(150, 40);
        this.slot_21.TabIndex = 21;
        // 
        // slot_29
        // 
        this.slot_29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_29.Location = new System.Drawing.Point(326, 387);
        this.slot_29.Name = "slot_29";
        this.slot_29.Size = new System.Drawing.Size(150, 40);
        this.slot_29.TabIndex = 29;
        // 
        // slot_12
        // 
        this.slot_12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_12.Location = new System.Drawing.Point(170, 65);
        this.slot_12.Name = "slot_12";
        this.slot_12.Size = new System.Drawing.Size(150, 40);
        this.slot_12.TabIndex = 12;
        // 
        // slot_44
        // 
        this.slot_44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_44.Location = new System.Drawing.Point(638, 157);
        this.slot_44.Name = "slot_44";
        this.slot_44.Size = new System.Drawing.Size(150, 40);
        this.slot_44.TabIndex = 44;
        // 
        // slot_22
        // 
        this.slot_22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_22.Location = new System.Drawing.Point(326, 65);
        this.slot_22.Name = "slot_22";
        this.slot_22.Size = new System.Drawing.Size(150, 40);
        this.slot_22.TabIndex = 22;
        // 
        // slot_37
        // 
        this.slot_37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_37.Location = new System.Drawing.Point(482, 295);
        this.slot_37.Name = "slot_37";
        this.slot_37.Size = new System.Drawing.Size(150, 40);
        this.slot_37.TabIndex = 37;
        // 
        // slot_13
        // 
        this.slot_13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_13.Location = new System.Drawing.Point(170, 111);
        this.slot_13.Name = "slot_13";
        this.slot_13.Size = new System.Drawing.Size(150, 40);
        this.slot_13.TabIndex = 13;
        // 
        // slot_43
        // 
        this.slot_43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_43.Location = new System.Drawing.Point(638, 111);
        this.slot_43.Name = "slot_43";
        this.slot_43.Size = new System.Drawing.Size(150, 40);
        this.slot_43.TabIndex = 43;
        // 
        // slot_23
        // 
        this.slot_23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_23.Location = new System.Drawing.Point(326, 111);
        this.slot_23.Name = "slot_23";
        this.slot_23.Size = new System.Drawing.Size(150, 40);
        this.slot_23.TabIndex = 23;
        // 
        // slot_19
        // 
        this.slot_19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_19.Location = new System.Drawing.Point(170, 387);
        this.slot_19.Name = "slot_19";
        this.slot_19.Size = new System.Drawing.Size(150, 40);
        this.slot_19.TabIndex = 19;
        // 
        // slot_14
        // 
        this.slot_14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_14.Location = new System.Drawing.Point(170, 157);
        this.slot_14.Name = "slot_14";
        this.slot_14.Size = new System.Drawing.Size(150, 40);
        this.slot_14.TabIndex = 14;
        // 
        // slot_42
        // 
        this.slot_42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_42.Location = new System.Drawing.Point(638, 65);
        this.slot_42.Name = "slot_42";
        this.slot_42.Size = new System.Drawing.Size(150, 40);
        this.slot_42.TabIndex = 42;
        // 
        // slot_24
        // 
        this.slot_24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_24.Location = new System.Drawing.Point(326, 157);
        this.slot_24.Name = "slot_24";
        this.slot_24.Size = new System.Drawing.Size(150, 40);
        this.slot_24.TabIndex = 24;
        // 
        // slot_36
        // 
        this.slot_36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_36.Location = new System.Drawing.Point(482, 249);
        this.slot_36.Name = "slot_36";
        this.slot_36.Size = new System.Drawing.Size(150, 40);
        this.slot_36.TabIndex = 36;
        // 
        // slot_15
        // 
        this.slot_15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_15.Location = new System.Drawing.Point(170, 203);
        this.slot_15.Name = "slot_15";
        this.slot_15.Size = new System.Drawing.Size(150, 40);
        this.slot_15.TabIndex = 15;
        // 
        // slot_41
        // 
        this.slot_41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_41.Location = new System.Drawing.Point(638, 19);
        this.slot_41.Name = "slot_41";
        this.slot_41.Size = new System.Drawing.Size(150, 40);
        this.slot_41.TabIndex = 41;
        // 
        // slot_25
        // 
        this.slot_25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_25.Location = new System.Drawing.Point(326, 203);
        this.slot_25.Name = "slot_25";
        this.slot_25.Size = new System.Drawing.Size(150, 40);
        this.slot_25.TabIndex = 25;
        // 
        // slot_28
        // 
        this.slot_28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_28.Location = new System.Drawing.Point(326, 341);
        this.slot_28.Name = "slot_28";
        this.slot_28.Size = new System.Drawing.Size(150, 40);
        this.slot_28.TabIndex = 28;
        // 
        // slot_16
        // 
        this.slot_16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_16.Location = new System.Drawing.Point(170, 249);
        this.slot_16.Name = "slot_16";
        this.slot_16.Size = new System.Drawing.Size(150, 40);
        this.slot_16.TabIndex = 16;
        // 
        // slot_35
        // 
        this.slot_35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_35.Location = new System.Drawing.Point(482, 203);
        this.slot_35.Name = "slot_35";
        this.slot_35.Size = new System.Drawing.Size(150, 40);
        this.slot_35.TabIndex = 35;
        // 
        // slot_31
        // 
        this.slot_31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_31.Location = new System.Drawing.Point(482, 19);
        this.slot_31.Name = "slot_31";
        this.slot_31.Size = new System.Drawing.Size(150, 40);
        this.slot_31.TabIndex = 31;
        // 
        // slot_18
        // 
        this.slot_18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_18.Location = new System.Drawing.Point(170, 341);
        this.slot_18.Name = "slot_18";
        this.slot_18.Size = new System.Drawing.Size(150, 40);
        this.slot_18.TabIndex = 18;
        // 
        // slot_26
        // 
        this.slot_26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_26.Location = new System.Drawing.Point(326, 249);
        this.slot_26.Name = "slot_26";
        this.slot_26.Size = new System.Drawing.Size(150, 40);
        this.slot_26.TabIndex = 26;
        // 
        // slot_34
        // 
        this.slot_34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_34.Location = new System.Drawing.Point(482, 157);
        this.slot_34.Name = "slot_34";
        this.slot_34.Size = new System.Drawing.Size(150, 40);
        this.slot_34.TabIndex = 34;
        // 
        // slot_32
        // 
        this.slot_32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_32.Location = new System.Drawing.Point(482, 65);
        this.slot_32.Name = "slot_32";
        this.slot_32.Size = new System.Drawing.Size(150, 40);
        this.slot_32.TabIndex = 32;
        // 
        // slot_27
        // 
        this.slot_27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_27.Location = new System.Drawing.Point(326, 295);
        this.slot_27.Name = "slot_27";
        this.slot_27.Size = new System.Drawing.Size(150, 40);
        this.slot_27.TabIndex = 27;
        // 
        // slot_17
        // 
        this.slot_17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_17.Location = new System.Drawing.Point(170, 295);
        this.slot_17.Name = "slot_17";
        this.slot_17.Size = new System.Drawing.Size(150, 40);
        this.slot_17.TabIndex = 17;
        // 
        // slot_33
        // 
        this.slot_33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.slot_33.Location = new System.Drawing.Point(482, 111);
        this.slot_33.Name = "slot_33";
        this.slot_33.Size = new System.Drawing.Size(150, 40);
        this.slot_33.TabIndex = 33;
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
        // pb_erasePlaylistCurrent
        // 
        this.pb_erasePlaylistCurrent.ErrorImage = null;
        this.pb_erasePlaylistCurrent.Image = global::ServerManager.Properties.Resources.eraser;
        this.pb_erasePlaylistCurrent.InitialImage = null;
        this.pb_erasePlaylistCurrent.Location = new System.Drawing.Point(54, 4);
        this.pb_erasePlaylistCurrent.Name = "pb_erasePlaylistCurrent";
        this.pb_erasePlaylistCurrent.Size = new System.Drawing.Size(15, 15);
        this.pb_erasePlaylistCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pb_erasePlaylistCurrent.TabIndex = 2;
        this.pb_erasePlaylistCurrent.TabStop = false;
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
        this.tabPage_currentMaps.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.CurrentMapPlaylist)).EndInit();
        this.panel6.ResumeLayout(false);
        this.panel7.ResumeLayout(false);
        this.panel8.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pb_loadPlaylistCurrent)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pb_savePlaylistCurrent)).EndInit();
        this.tabPage_mapEditors.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.PlayListEditor)).EndInit();
        this.panel10.ResumeLayout(false);
        this.panel11.ResumeLayout(false);
        this.panel_mapsLeft.ResumeLayout(false);
        this.maps_tabControl1.ResumeLayout(false);
        this.tabPage_AvailMaps.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.AvailableMapList)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.AvailableMaps)).EndInit();
        this.panel1.ResumeLayout(false);
        this.panel5.ResumeLayout(false);
        this.panel4.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_refresh)).EndInit();
        this.tabPage_messaging.ResumeLayout(false);
        this.spacing_chat.ResumeLayout(false);
        this.tabControl_messaging.ResumeLayout(false);
        this.tabPage_playerManagement.ResumeLayout(false);
        this.spacing_players.ResumeLayout(false);
        this.groupBox_playerBox.ResumeLayout(false);
        this.slot_01.ResumeLayout(false);
        this.slot_02.ResumeLayout(false);
        this.slot_03.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pb_erasePlaylistCurrent)).EndInit();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.PictureBox pb_erasePlaylistCurrent;

    private System.Windows.Forms.PictureBox pb_loadPlaylistCurrent;

    private System.Windows.Forms.PictureBox pb_savePlaylistCurrent;

    private System.Windows.Forms.PictureBox pictureBox_refresh;

    private System.Windows.Forms.DataGridView CurrentMapPlaylist;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private System.Windows.Forms.DataGridView PlayListEditor;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;

    private System.Windows.Forms.DataGridViewTextBoxColumn gametype_id;

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
    private System.Windows.Forms.DataGridView dataGridView2;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
    private System.Windows.Forms.DataGridView dataGridView3;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
    private System.Windows.Forms.DataGridView AvailableMapList;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;

    private System.Windows.Forms.DataGridViewTextBoxColumn mission_name;
    private System.Windows.Forms.DataGridViewTextBoxColumn mission_file;

    private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    private System.Windows.Forms.DataGridViewTextBoxColumn game;
    private System.Windows.Forms.DataGridViewTextBoxColumn GameType;
    private System.Windows.Forms.DataGridViewTextBoxColumn CustomMap;

    private System.Windows.Forms.GroupBox groupBox_playerDetails;

    private System.Windows.Forms.GroupBox groupBox_playerBox;

    private System.Windows.Forms.ComboBox comboBox_gameTypes;

    private System.Windows.Forms.Panel panel10;
    private System.Windows.Forms.Panel panel11;
    private System.Windows.Forms.Panel panel12;
    private System.Windows.Forms.Panel panel13;

    private System.Windows.Forms.Panel panel7;
    private System.Windows.Forms.ComboBox comboBox_AvailableMapLists;
    private System.Windows.Forms.Panel panel8;
    private System.Windows.Forms.Panel panel9;

    private System.Windows.Forms.DataGridView AvailableMaps;

    private System.Windows.Forms.ComboBox comboBox_loadMapList;

    private System.Windows.Forms.Panel panel2;

    private System.Windows.Forms.Panel panel1;

    private System.Windows.Forms.Label label_01_ip;

    private System.Windows.Forms.Label label_01_playerName;

    private System.Windows.Forms.Label label_slot02;
    private System.Windows.Forms.Label label_slot03;

    private System.Windows.Forms.Label label_01_slot;

    private System.Windows.Forms.Panel slot_48;
    private System.Windows.Forms.Panel slot_45;
    private System.Windows.Forms.Panel slot_46;
    private System.Windows.Forms.Panel slot_47;
    private System.Windows.Forms.Panel slot_39;
    private System.Windows.Forms.Panel slot_40;

    private System.Windows.Forms.Panel slot_35;
    private System.Windows.Forms.Panel slot_36;
    private System.Windows.Forms.Panel slot_37;
    private System.Windows.Forms.Panel slot_38;
    private System.Windows.Forms.Panel slot_44;
    private System.Windows.Forms.Panel slot_43;
    private System.Windows.Forms.Panel slot_42;
    private System.Windows.Forms.Panel slot_41;

    private System.Windows.Forms.Panel slot_30;
    private System.Windows.Forms.Panel slot_29;
    private System.Windows.Forms.Panel slot_28;
    private System.Windows.Forms.Panel slot_27;
    private System.Windows.Forms.Panel slot_26;
    private System.Windows.Forms.Panel slot_25;
    private System.Windows.Forms.Panel slot_24;
    private System.Windows.Forms.Panel slot_23;
    private System.Windows.Forms.Panel slot_22;
    private System.Windows.Forms.Panel slot_21;
    private System.Windows.Forms.Panel slot_50;
    private System.Windows.Forms.Panel slot_49;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Panel panel5;
    private System.Windows.Forms.Panel panel6;
    private System.Windows.Forms.Panel slot_34;
    private System.Windows.Forms.Panel slot_33;
    private System.Windows.Forms.Panel slot_32;
    private System.Windows.Forms.Panel slot_31;

    private System.Windows.Forms.Panel slot_01;
    private System.Windows.Forms.Panel slot_11;
    private System.Windows.Forms.Panel slot_02;
    private System.Windows.Forms.Panel slot_03;
    private System.Windows.Forms.Panel slot_04;
    private System.Windows.Forms.Panel slot_05;
    private System.Windows.Forms.Panel slot_06;
    private System.Windows.Forms.Panel slot_07;
    private System.Windows.Forms.Panel slot_08;
    private System.Windows.Forms.Panel slot_09;
    private System.Windows.Forms.Panel slot_10;
    private System.Windows.Forms.Panel slot_12;
    private System.Windows.Forms.Panel slot_13;
    private System.Windows.Forms.Panel slot_14;
    private System.Windows.Forms.Panel slot_15;
    private System.Windows.Forms.Panel slot_16;
    private System.Windows.Forms.Panel slot_17;
    private System.Windows.Forms.Panel slot_18;
    private System.Windows.Forms.Panel slot_19;
    private System.Windows.Forms.Panel slot_20;
    private System.Windows.Forms.Panel slot_;

    private System.Windows.Forms.Panel spacing_players;

    private System.Windows.Forms.TabPage tabPage_chatMod;

    private System.Windows.Forms.TabPage tabPage_schedMsg;

    private System.Windows.Forms.TabControl tabControl_messaging;
    private System.Windows.Forms.TabPage tabPage_chatMsg;
    private System.Windows.Forms.TabPage tabPage_serverMsg;

    private System.Windows.Forms.Panel spacing_chat;

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