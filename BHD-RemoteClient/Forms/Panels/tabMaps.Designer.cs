namespace BHD_RemoteClient.Forms.Panels
{
    partial class tabMaps
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tabMaps));
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btn_mapsSkip = new Button();
            btn_mapsScore = new Button();
            btn_mapsPlayNext = new Button();
            btn_mapsUpdate = new Button();
            btn_mapsUpload = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            combo_gameTypes = new ComboBox();
            dataGridView_availableMaps = new DataGridView();
            avail_MapID = new DataGridViewTextBoxColumn();
            avail_MapName = new DataGridViewTextBoxColumn();
            avail_MapFileName = new DataGridViewTextBoxColumn();
            avail_MapType = new DataGridViewTextBoxColumn();
            avail_MapDelete = new DataGridViewButtonColumn();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            ib_SaveMapList = new FontAwesome.Sharp.IconButton();
            ib_mapRefresh = new FontAwesome.Sharp.IconButton();
            ib_resetCurrentMaps = new FontAwesome.Sharp.IconButton();
            ib_exportMapList = new FontAwesome.Sharp.IconButton();
            ib_importMapList = new FontAwesome.Sharp.IconButton();
            ib_clearMapList = new FontAwesome.Sharp.IconButton();
            dataGridView_currentMaps = new DataGridView();
            current_MapID = new DataGridViewTextBoxColumn();
            current_MapName = new DataGridViewTextBoxColumn();
            current_MapFileName = new DataGridViewTextBoxColumn();
            current_MapType = new DataGridViewTextBoxColumn();
            tableLayoutPanel6 = new TableLayoutPanel();
            label_mapLabel1 = new Label();
            label_currentMapName = new Label();
            label_currentMapType = new Label();
            label_mapLabel2 = new Label();
            label_nextMapName = new Label();
            label_nextMapType = new Label();
            label_mapLabel3 = new Label();
            label_timeLeft = new Label();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_availableMaps).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_currentMaps).BeginInit();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 4, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 2, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(902, 362);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(btn_mapsSkip, 0, 5);
            tableLayoutPanel2.Controls.Add(btn_mapsScore, 0, 4);
            tableLayoutPanel2.Controls.Add(btn_mapsPlayNext, 0, 3);
            tableLayoutPanel2.Controls.Add(btn_mapsUpdate, 0, 2);
            tableLayoutPanel2.Controls.Add(btn_mapsUpload, 0, 0);
            tableLayoutPanel2.Location = new Point(805, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(94, 350);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // btn_mapsSkip
            // 
            btn_mapsSkip.Dock = DockStyle.Fill;
            btn_mapsSkip.Location = new Point(3, 253);
            btn_mapsSkip.Name = "btn_mapsSkip";
            btn_mapsSkip.Size = new Size(88, 44);
            btn_mapsSkip.TabIndex = 4;
            btn_mapsSkip.Text = "SKIP";
            btn_mapsSkip.UseVisualStyleBackColor = true;
            btn_mapsSkip.Click += actionClick_SkipMap;
            // 
            // btn_mapsScore
            // 
            btn_mapsScore.Dock = DockStyle.Fill;
            btn_mapsScore.Location = new Point(3, 203);
            btn_mapsScore.Name = "btn_mapsScore";
            btn_mapsScore.Size = new Size(88, 44);
            btn_mapsScore.TabIndex = 2;
            btn_mapsScore.Text = "SCORE";
            btn_mapsScore.UseVisualStyleBackColor = true;
            btn_mapsScore.Click += actionClick_ScoreMap;
            // 
            // btn_mapsPlayNext
            // 
            btn_mapsPlayNext.Dock = DockStyle.Fill;
            btn_mapsPlayNext.Location = new Point(3, 153);
            btn_mapsPlayNext.Name = "btn_mapsPlayNext";
            btn_mapsPlayNext.Size = new Size(88, 44);
            btn_mapsPlayNext.TabIndex = 1;
            btn_mapsPlayNext.Text = "PLAY NEXT";
            btn_mapsPlayNext.UseVisualStyleBackColor = true;
            btn_mapsPlayNext.Click += actionClick_PlayMapNext;
            // 
            // btn_mapsUpdate
            // 
            btn_mapsUpdate.Dock = DockStyle.Fill;
            btn_mapsUpdate.Location = new Point(3, 103);
            btn_mapsUpdate.Name = "btn_mapsUpdate";
            btn_mapsUpdate.Size = new Size(88, 44);
            btn_mapsUpdate.TabIndex = 0;
            btn_mapsUpdate.Text = "UPDATE";
            btn_mapsUpdate.UseVisualStyleBackColor = true;
            btn_mapsUpdate.Click += actionClick_UpdateMapPlaylist;
            // 
            // btn_mapsUpload
            // 
            btn_mapsUpload.Dock = DockStyle.Fill;
            btn_mapsUpload.Enabled = false;
            btn_mapsUpload.Location = new Point(3, 3);
            btn_mapsUpload.Name = "btn_mapsUpload";
            btn_mapsUpload.Size = new Size(88, 44);
            btn_mapsUpload.TabIndex = 5;
            btn_mapsUpload.Text = "UPLOAD";
            toolTip1.SetToolTip(btn_mapsUpload, "Future Feature");
            btn_mapsUpload.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(combo_gameTypes, 0, 0);
            tableLayoutPanel3.Controls.Add(dataGridView_availableMaps, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(300, 362);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // combo_gameTypes
            // 
            combo_gameTypes.Dock = DockStyle.Fill;
            combo_gameTypes.Font = new Font("Segoe UI", 10F);
            combo_gameTypes.FormattingEnabled = true;
            combo_gameTypes.ItemHeight = 17;
            combo_gameTypes.Items.AddRange(new object[] { "Deathmatch", "Team Deathmatch", "Cooperative", "Team King of the Hill", "King of the Hill", "Search and Destroy", "Attack and Defend", "Capture the Flag", "Flagball", "All Game Modes" });
            combo_gameTypes.Location = new Point(0, 0);
            combo_gameTypes.Margin = new Padding(0);
            combo_gameTypes.Name = "combo_gameTypes";
            combo_gameTypes.Size = new Size(300, 25);
            combo_gameTypes.TabIndex = 1;
            combo_gameTypes.Text = "All Game Modes";
            combo_gameTypes.SelectedIndexChanged += actionClick_filterChanged;
            combo_gameTypes.TextChanged += actionTextChange_MapFilter;
            // 
            // dataGridView_availableMaps
            // 
            dataGridView_availableMaps.AllowUserToAddRows = false;
            dataGridView_availableMaps.AllowUserToDeleteRows = false;
            dataGridView_availableMaps.AllowUserToResizeColumns = false;
            dataGridView_availableMaps.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView_availableMaps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView_availableMaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_availableMaps.Columns.AddRange(new DataGridViewColumn[] { avail_MapID, avail_MapName, avail_MapFileName, avail_MapType, avail_MapDelete });
            dataGridView_availableMaps.Dock = DockStyle.Fill;
            dataGridView_availableMaps.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_availableMaps.Location = new Point(3, 29);
            dataGridView_availableMaps.Name = "dataGridView_availableMaps";
            dataGridView_availableMaps.ReadOnly = true;
            dataGridView_availableMaps.RowHeadersVisible = false;
            dataGridView_availableMaps.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView_availableMaps.Size = new Size(294, 330);
            dataGridView_availableMaps.TabIndex = 2;
            dataGridView_availableMaps.CellDoubleClick += actionClick_CurrentPlaylistAddMap;
            // 
            // avail_MapID
            // 
            avail_MapID.HeaderText = "Map ID";
            avail_MapID.Name = "avail_MapID";
            avail_MapID.ReadOnly = true;
            avail_MapID.Visible = false;
            // 
            // avail_MapName
            // 
            avail_MapName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            avail_MapName.HeaderText = "Map Name";
            avail_MapName.MaxInputLength = 64;
            avail_MapName.Name = "avail_MapName";
            avail_MapName.ReadOnly = true;
            avail_MapName.Resizable = DataGridViewTriState.False;
            // 
            // avail_MapFileName
            // 
            avail_MapFileName.HeaderText = "Map File Name";
            avail_MapFileName.Name = "avail_MapFileName";
            avail_MapFileName.ReadOnly = true;
            avail_MapFileName.Visible = false;
            // 
            // avail_MapType
            // 
            avail_MapType.HeaderText = "Type";
            avail_MapType.MaxInputLength = 5;
            avail_MapType.MinimumWidth = 60;
            avail_MapType.Name = "avail_MapType";
            avail_MapType.ReadOnly = true;
            avail_MapType.Resizable = DataGridViewTriState.False;
            avail_MapType.Width = 60;
            // 
            // avail_MapDelete
            // 
            avail_MapDelete.HeaderText = "";
            avail_MapDelete.MinimumWidth = 32;
            avail_MapDelete.Name = "avail_MapDelete";
            avail_MapDelete.ReadOnly = true;
            avail_MapDelete.Resizable = DataGridViewTriState.False;
            avail_MapDelete.Text = "X";
            avail_MapDelete.UseColumnTextForButtonValue = true;
            avail_MapDelete.Width = 32;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.Controls.Add(tableLayoutPanel5, 0, 0);
            tableLayoutPanel4.Controls.Add(dataGridView_currentMaps, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(320, 0);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(300, 362);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 6;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel5.Controls.Add(ib_SaveMapList, 1, 0);
            tableLayoutPanel5.Controls.Add(ib_mapRefresh, 0, 0);
            tableLayoutPanel5.Controls.Add(ib_resetCurrentMaps, 5, 0);
            tableLayoutPanel5.Controls.Add(ib_exportMapList, 4, 0);
            tableLayoutPanel5.Controls.Add(ib_importMapList, 3, 0);
            tableLayoutPanel5.Controls.Add(ib_clearMapList, 2, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(300, 26);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // ib_SaveMapList
            // 
            ib_SaveMapList.Dock = DockStyle.Fill;
            ib_SaveMapList.FlatStyle = FlatStyle.Flat;
            ib_SaveMapList.Font = new Font("Segoe UI", 8.25F);
            ib_SaveMapList.IconChar = FontAwesome.Sharp.IconChar.Save;
            ib_SaveMapList.IconColor = Color.Black;
            ib_SaveMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_SaveMapList.IconSize = 24;
            ib_SaveMapList.Location = new Point(50, 0);
            ib_SaveMapList.Margin = new Padding(0);
            ib_SaveMapList.Name = "ib_SaveMapList";
            ib_SaveMapList.Size = new Size(50, 26);
            ib_SaveMapList.TabIndex = 1;
            toolTip1.SetToolTip(ib_SaveMapList, resources.GetString("ib_SaveMapList.ToolTip"));
            ib_SaveMapList.UseVisualStyleBackColor = true;
            ib_SaveMapList.Click += actionClick_SaveCurrentPlaylist;
            // 
            // ib_mapRefresh
            // 
            ib_mapRefresh.Dock = DockStyle.Fill;
            ib_mapRefresh.FlatStyle = FlatStyle.Flat;
            ib_mapRefresh.Font = new Font("Segoe UI", 8.25F);
            ib_mapRefresh.IconChar = FontAwesome.Sharp.IconChar.Refresh;
            ib_mapRefresh.IconColor = Color.Black;
            ib_mapRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_mapRefresh.IconSize = 24;
            ib_mapRefresh.Location = new Point(0, 0);
            ib_mapRefresh.Margin = new Padding(0);
            ib_mapRefresh.Name = "ib_mapRefresh";
            ib_mapRefresh.Size = new Size(50, 26);
            ib_mapRefresh.TabIndex = 0;
            toolTip1.SetToolTip(ib_mapRefresh, "Refresh Available Maps");
            ib_mapRefresh.UseVisualStyleBackColor = true;
            ib_mapRefresh.Click += actionClick_RefreshAvailableMaps;
            // 
            // ib_resetCurrentMaps
            // 
            ib_resetCurrentMaps.Dock = DockStyle.Fill;
            ib_resetCurrentMaps.FlatStyle = FlatStyle.Flat;
            ib_resetCurrentMaps.Font = new Font("Segoe UI", 8.25F);
            ib_resetCurrentMaps.IconChar = FontAwesome.Sharp.IconChar.Reply;
            ib_resetCurrentMaps.IconColor = Color.Black;
            ib_resetCurrentMaps.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_resetCurrentMaps.IconSize = 24;
            ib_resetCurrentMaps.Location = new Point(250, 0);
            ib_resetCurrentMaps.Margin = new Padding(0);
            ib_resetCurrentMaps.Name = "ib_resetCurrentMaps";
            ib_resetCurrentMaps.Size = new Size(50, 26);
            ib_resetCurrentMaps.TabIndex = 4;
            toolTip1.SetToolTip(ib_resetCurrentMaps, "Reset Playlist Back to Live");
            ib_resetCurrentMaps.UseVisualStyleBackColor = true;
            ib_resetCurrentMaps.Click += actionClick_ResetCurrentMapPlaylist;
            // 
            // ib_exportMapList
            // 
            ib_exportMapList.Dock = DockStyle.Fill;
            ib_exportMapList.FlatStyle = FlatStyle.Flat;
            ib_exportMapList.Font = new Font("Segoe UI", 8.25F);
            ib_exportMapList.IconChar = FontAwesome.Sharp.IconChar.BoxesPacking;
            ib_exportMapList.IconColor = Color.Black;
            ib_exportMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_exportMapList.IconSize = 24;
            ib_exportMapList.Location = new Point(200, 0);
            ib_exportMapList.Margin = new Padding(0);
            ib_exportMapList.Name = "ib_exportMapList";
            ib_exportMapList.Size = new Size(50, 26);
            ib_exportMapList.TabIndex = 3;
            toolTip1.SetToolTip(ib_exportMapList, "Export Maplist");
            ib_exportMapList.UseVisualStyleBackColor = true;
            ib_exportMapList.Click += actionClick_ExportCurrentPlaylist;
            // 
            // ib_importMapList
            // 
            ib_importMapList.Dock = DockStyle.Fill;
            ib_importMapList.FlatStyle = FlatStyle.Flat;
            ib_importMapList.Font = new Font("Segoe UI", 8.25F);
            ib_importMapList.IconChar = FontAwesome.Sharp.IconChar.BoxOpen;
            ib_importMapList.IconColor = Color.Black;
            ib_importMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_importMapList.IconSize = 24;
            ib_importMapList.Location = new Point(150, 0);
            ib_importMapList.Margin = new Padding(0);
            ib_importMapList.Name = "ib_importMapList";
            ib_importMapList.Size = new Size(50, 26);
            ib_importMapList.TabIndex = 2;
            toolTip1.SetToolTip(ib_importMapList, "Import Maplist");
            ib_importMapList.UseVisualStyleBackColor = true;
            ib_importMapList.Click += actionClick_ImportCurrentPlaylist;
            // 
            // ib_clearMapList
            // 
            ib_clearMapList.Dock = DockStyle.Fill;
            ib_clearMapList.FlatStyle = FlatStyle.Flat;
            ib_clearMapList.Font = new Font("Segoe UI", 8.25F);
            ib_clearMapList.IconChar = FontAwesome.Sharp.IconChar.Eraser;
            ib_clearMapList.IconColor = Color.Black;
            ib_clearMapList.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ib_clearMapList.IconSize = 24;
            ib_clearMapList.Location = new Point(100, 0);
            ib_clearMapList.Margin = new Padding(0);
            ib_clearMapList.Name = "ib_clearMapList";
            ib_clearMapList.Size = new Size(50, 26);
            ib_clearMapList.TabIndex = 1;
            toolTip1.SetToolTip(ib_clearMapList, "Clear Playlist");
            ib_clearMapList.UseVisualStyleBackColor = true;
            ib_clearMapList.Click += actionClick_ClearCurrentPlaylist;
            // 
            // dataGridView_currentMaps
            // 
            dataGridView_currentMaps.AllowUserToAddRows = false;
            dataGridView_currentMaps.AllowUserToDeleteRows = false;
            dataGridView_currentMaps.AllowUserToResizeColumns = false;
            dataGridView_currentMaps.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView_currentMaps.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView_currentMaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_currentMaps.Columns.AddRange(new DataGridViewColumn[] { current_MapID, current_MapName, current_MapFileName, current_MapType });
            dataGridView_currentMaps.Dock = DockStyle.Fill;
            dataGridView_currentMaps.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_currentMaps.Location = new Point(3, 29);
            dataGridView_currentMaps.Name = "dataGridView_currentMaps";
            dataGridView_currentMaps.ReadOnly = true;
            dataGridView_currentMaps.RowHeadersVisible = false;
            dataGridView_currentMaps.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView_currentMaps.Size = new Size(294, 330);
            dataGridView_currentMaps.TabIndex = 1;
            dataGridView_currentMaps.CellDoubleClick += actionClick_DeleteSelectedMap;
            dataGridView_currentMaps.KeyPress += actionKey_MoveMap;
            // 
            // current_MapID
            // 
            current_MapID.HeaderText = "ID";
            current_MapID.MaxInputLength = 3;
            current_MapID.MinimumWidth = 40;
            current_MapID.Name = "current_MapID";
            current_MapID.ReadOnly = true;
            current_MapID.Resizable = DataGridViewTriState.False;
            current_MapID.Width = 40;
            // 
            // current_MapName
            // 
            current_MapName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            current_MapName.HeaderText = "Map Name";
            current_MapName.Name = "current_MapName";
            current_MapName.ReadOnly = true;
            current_MapName.Resizable = DataGridViewTriState.False;
            // 
            // current_MapFileName
            // 
            current_MapFileName.HeaderText = "Map File Name";
            current_MapFileName.Name = "current_MapFileName";
            current_MapFileName.ReadOnly = true;
            current_MapFileName.Visible = false;
            // 
            // current_MapType
            // 
            current_MapType.HeaderText = "Type";
            current_MapType.MaxInputLength = 5;
            current_MapType.MinimumWidth = 60;
            current_MapType.Name = "current_MapType";
            current_MapType.ReadOnly = true;
            current_MapType.Resizable = DataGridViewTriState.False;
            current_MapType.Width = 60;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel6.Controls.Add(label_mapLabel1, 0, 1);
            tableLayoutPanel6.Controls.Add(label_currentMapName, 0, 2);
            tableLayoutPanel6.Controls.Add(label_currentMapType, 0, 3);
            tableLayoutPanel6.Controls.Add(label_mapLabel2, 0, 5);
            tableLayoutPanel6.Controls.Add(label_nextMapName, 0, 6);
            tableLayoutPanel6.Controls.Add(label_nextMapType, 0, 7);
            tableLayoutPanel6.Controls.Add(label_mapLabel3, 0, 9);
            tableLayoutPanel6.Controls.Add(label_timeLeft, 0, 10);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(623, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 13;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.69242525F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.691656F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 7.691656F));
            tableLayoutPanel6.Size = new Size(176, 356);
            tableLayoutPanel6.TabIndex = 4;
            // 
            // label_mapLabel1
            // 
            label_mapLabel1.AutoSize = true;
            label_mapLabel1.Dock = DockStyle.Fill;
            label_mapLabel1.FlatStyle = FlatStyle.Flat;
            label_mapLabel1.Font = new Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic);
            label_mapLabel1.Location = new Point(3, 27);
            label_mapLabel1.Name = "label_mapLabel1";
            label_mapLabel1.Size = new Size(170, 27);
            label_mapLabel1.TabIndex = 0;
            label_mapLabel1.Text = "Current Map";
            label_mapLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_currentMapName
            // 
            label_currentMapName.AutoSize = true;
            label_currentMapName.Dock = DockStyle.Fill;
            label_currentMapName.Location = new Point(3, 54);
            label_currentMapName.Name = "label_currentMapName";
            label_currentMapName.Size = new Size(170, 27);
            label_currentMapName.TabIndex = 1;
            label_currentMapName.Text = "Map Name";
            label_currentMapName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_currentMapType
            // 
            label_currentMapType.AutoSize = true;
            label_currentMapType.Dock = DockStyle.Fill;
            label_currentMapType.Location = new Point(3, 81);
            label_currentMapType.Name = "label_currentMapType";
            label_currentMapType.Size = new Size(170, 27);
            label_currentMapType.TabIndex = 2;
            label_currentMapType.Text = "Map Type (Long)";
            label_currentMapType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_mapLabel2
            // 
            label_mapLabel2.AutoSize = true;
            label_mapLabel2.Dock = DockStyle.Fill;
            label_mapLabel2.Font = new Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic);
            label_mapLabel2.Location = new Point(3, 135);
            label_mapLabel2.Name = "label_mapLabel2";
            label_mapLabel2.Size = new Size(170, 27);
            label_mapLabel2.TabIndex = 3;
            label_mapLabel2.Text = "Next Map";
            label_mapLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_nextMapName
            // 
            label_nextMapName.AutoSize = true;
            label_nextMapName.Dock = DockStyle.Fill;
            label_nextMapName.Location = new Point(3, 162);
            label_nextMapName.Name = "label_nextMapName";
            label_nextMapName.Size = new Size(170, 27);
            label_nextMapName.TabIndex = 4;
            label_nextMapName.Text = "Map Name";
            label_nextMapName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_nextMapType
            // 
            label_nextMapType.AutoSize = true;
            label_nextMapType.Dock = DockStyle.Fill;
            label_nextMapType.Location = new Point(3, 189);
            label_nextMapType.Name = "label_nextMapType";
            label_nextMapType.Size = new Size(170, 27);
            label_nextMapType.TabIndex = 5;
            label_nextMapType.Text = "Map Type (Long)";
            label_nextMapType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_mapLabel3
            // 
            label_mapLabel3.AutoSize = true;
            label_mapLabel3.Dock = DockStyle.Fill;
            label_mapLabel3.Font = new Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic);
            label_mapLabel3.Location = new Point(3, 243);
            label_mapLabel3.Name = "label_mapLabel3";
            label_mapLabel3.Size = new Size(170, 27);
            label_mapLabel3.TabIndex = 6;
            label_mapLabel3.Text = "Time Left";
            label_mapLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_timeLeft
            // 
            label_timeLeft.AutoSize = true;
            label_timeLeft.Dock = DockStyle.Fill;
            label_timeLeft.Location = new Point(3, 270);
            label_timeLeft.Name = "label_timeLeft";
            label_timeLeft.Size = new Size(170, 27);
            label_timeLeft.TabIndex = 7;
            label_timeLeft.Text = "HH:MM:SS";
            label_timeLeft.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabMaps
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabMaps";
            Size = new Size(902, 362);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_availableMaps).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_currentMaps).EndInit();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private ToolTip toolTip1;
        internal ComboBox combo_gameTypes;
        private DataGridViewTextBoxColumn current_MapID;
        private DataGridViewTextBoxColumn current_MapName;
        private DataGridViewTextBoxColumn current_MapFileName;
        private DataGridViewTextBoxColumn current_MapType;
        public Button btn_mapsSkip;
        public Button btn_mapsUpload;
        public Button btn_mapsUpdate;
        public Button btn_mapsPlayNext;
        public Button btn_mapsScore;
        public FontAwesome.Sharp.IconButton ib_clearMapList;
        public FontAwesome.Sharp.IconButton ib_importMapList;
        public FontAwesome.Sharp.IconButton ib_exportMapList;
        public FontAwesome.Sharp.IconButton ib_resetCurrentMaps;
        public FontAwesome.Sharp.IconButton ib_mapRefresh;
        public FontAwesome.Sharp.IconButton ib_SaveMapList;
        public DataGridView dataGridView_availableMaps;
        public DataGridView dataGridView_currentMaps;
        private DataGridViewTextBoxColumn avail_MapID;
        private DataGridViewTextBoxColumn avail_MapName;
        private DataGridViewTextBoxColumn avail_MapFileName;
        private DataGridViewTextBoxColumn avail_MapType;
        private DataGridViewButtonColumn avail_MapDelete;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label_mapLabel1;
        private Label label_currentMapName;
        private Label label_currentMapType;
        private Label label_mapLabel2;
        private Label label_nextMapName;
        private Label label_nextMapType;
        private Label label_mapLabel3;
        private Label label_timeLeft;
    }
}
