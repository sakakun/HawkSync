namespace RemoteClient.Forms.SubPanels.tabAdmin;

partial class tabAudit
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
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        groupBoxLogs = new System.Windows.Forms.GroupBox();
        tableLayoutPanelAuditLogs = new System.Windows.Forms.TableLayoutPanel();
        panelAuditFilters = new System.Windows.Forms.Panel();
        btnAuditRefresh = new System.Windows.Forms.Button();
        txtAuditUserFilter = new System.Windows.Forms.TextBox();
        lblAuditUserFilter = new System.Windows.Forms.Label();
        cbAuditCategoryFilter = new System.Windows.Forms.ComboBox();
        lblAuditCategory = new System.Windows.Forms.Label();
        dgvAuditLogs = new System.Windows.Forms.DataGridView();
        Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
        Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
        lblAuditStatus = new System.Windows.Forms.Label();
        groupBoxLogs.SuspendLayout();
        tableLayoutPanelAuditLogs.SuspendLayout();
        panelAuditFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).BeginInit();
        SuspendLayout();
        // 
        // groupBoxLogs
        // 
        groupBoxLogs.Controls.Add(tableLayoutPanelAuditLogs);
        groupBoxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        groupBoxLogs.Location = new System.Drawing.Point(0, 0);
        groupBoxLogs.Name = "groupBoxLogs";
        groupBoxLogs.Size = new System.Drawing.Size(958, 394);
        groupBoxLogs.TabIndex = 0;
        groupBoxLogs.TabStop = false;
        groupBoxLogs.Text = "Audit Logs (Last 24 Hours)";
        // 
        // tableLayoutPanelAuditLogs
        // 
        tableLayoutPanelAuditLogs.ColumnCount = 1;
        tableLayoutPanelAuditLogs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanelAuditLogs.Controls.Add(panelAuditFilters, 0, 0);
        tableLayoutPanelAuditLogs.Controls.Add(dgvAuditLogs, 0, 1);
        tableLayoutPanelAuditLogs.Controls.Add(lblAuditStatus, 0, 2);
        tableLayoutPanelAuditLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanelAuditLogs.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanelAuditLogs.Name = "tableLayoutPanelAuditLogs";
        tableLayoutPanelAuditLogs.RowCount = 3;
        tableLayoutPanelAuditLogs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
        tableLayoutPanelAuditLogs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanelAuditLogs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        tableLayoutPanelAuditLogs.Size = new System.Drawing.Size(952, 372);
        tableLayoutPanelAuditLogs.TabIndex = 0;
        // 
        // panelAuditFilters
        // 
        panelAuditFilters.Controls.Add(btnAuditRefresh);
        panelAuditFilters.Controls.Add(txtAuditUserFilter);
        panelAuditFilters.Controls.Add(lblAuditUserFilter);
        panelAuditFilters.Controls.Add(cbAuditCategoryFilter);
        panelAuditFilters.Controls.Add(lblAuditCategory);
        panelAuditFilters.Dock = System.Windows.Forms.DockStyle.Fill;
        panelAuditFilters.Location = new System.Drawing.Point(0, 0);
        panelAuditFilters.Margin = new System.Windows.Forms.Padding(0);
        panelAuditFilters.Name = "panelAuditFilters";
        panelAuditFilters.Size = new System.Drawing.Size(952, 40);
        panelAuditFilters.TabIndex = 0;
        // 
        // btnAuditRefresh
        // 
        btnAuditRefresh.Location = new System.Drawing.Point(350, 8);
        btnAuditRefresh.Name = "btnAuditRefresh";
        btnAuditRefresh.Size = new System.Drawing.Size(75, 25);
        btnAuditRefresh.TabIndex = 4;
        btnAuditRefresh.Text = "🔍 Refresh";
        btnAuditRefresh.UseVisualStyleBackColor = true;
        btnAuditRefresh.Click += btnAuditRefresh_Click;
        // 
        // txtAuditUserFilter
        // 
        txtAuditUserFilter.Location = new System.Drawing.Point(218, 9);
        txtAuditUserFilter.Name = "txtAuditUserFilter";
        txtAuditUserFilter.PlaceholderText = "Filter by user...";
        txtAuditUserFilter.Size = new System.Drawing.Size(120, 23);
        txtAuditUserFilter.TabIndex = 3;
        txtAuditUserFilter.TextChanged += txtAuditUserFilter_TextChanged;
        // 
        // lblAuditUserFilter
        // 
        lblAuditUserFilter.AutoSize = true;
        lblAuditUserFilter.Location = new System.Drawing.Point(180, 12);
        lblAuditUserFilter.Name = "lblAuditUserFilter";
        lblAuditUserFilter.Size = new System.Drawing.Size(33, 15);
        lblAuditUserFilter.TabIndex = 2;
        lblAuditUserFilter.Text = "User:";
        // 
        // cbAuditCategoryFilter
        // 
        cbAuditCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbAuditCategoryFilter.FormattingEnabled = true;
        cbAuditCategoryFilter.Items.AddRange(new object[] { "All", "Ban", "Chat", "Player", "Map", "Settings", "Server", "Stats", "System", "User" });
        cbAuditCategoryFilter.Location = new System.Drawing.Point(68, 9);
        cbAuditCategoryFilter.Name = "cbAuditCategoryFilter";
        cbAuditCategoryFilter.Size = new System.Drawing.Size(100, 23);
        cbAuditCategoryFilter.TabIndex = 1;
        cbAuditCategoryFilter.SelectedIndexChanged += cbAuditCategoryFilter_SelectedIndexChanged;
        // 
        // lblAuditCategory
        // 
        lblAuditCategory.AutoSize = true;
        lblAuditCategory.Location = new System.Drawing.Point(5, 12);
        lblAuditCategory.Name = "lblAuditCategory";
        lblAuditCategory.Size = new System.Drawing.Size(58, 15);
        lblAuditCategory.TabIndex = 0;
        lblAuditCategory.Text = "Category:";
        // 
        // dgvAuditLogs
        // 
        dgvAuditLogs.AllowUserToAddRows = false;
        dgvAuditLogs.AllowUserToDeleteRows = false;
        dgvAuditLogs.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)((byte)240)), ((int)((byte)240)), ((int)((byte)240)));
        dgvAuditLogs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvAuditLogs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)((byte)230)), ((int)((byte)230)), ((int)((byte)230)));
        dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        dgvAuditLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Time, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, Description, Status });
        dgvAuditLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        dgvAuditLogs.EnableHeadersVisualStyles = false;
        dgvAuditLogs.Location = new System.Drawing.Point(3, 43);
        dgvAuditLogs.MultiSelect = false;
        dgvAuditLogs.Name = "dgvAuditLogs";
        dgvAuditLogs.ReadOnly = true;
        dgvAuditLogs.RowHeadersVisible = false;
        dgvAuditLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvAuditLogs.Size = new System.Drawing.Size(946, 301);
        dgvAuditLogs.TabIndex = 1;
        // 
        // Time
        // 
        Time.HeaderText = "Time";
        Time.Name = "Time";
        Time.ReadOnly = true;
        Time.Width = 60;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.HeaderText = "User";
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        dataGridViewTextBoxColumn2.Width = 75;
        // 
        // dataGridViewTextBoxColumn3
        // 
        dataGridViewTextBoxColumn3.HeaderText = "Category";
        dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
        dataGridViewTextBoxColumn3.ReadOnly = true;
        dataGridViewTextBoxColumn3.Width = 60;
        // 
        // dataGridViewTextBoxColumn4
        // 
        dataGridViewTextBoxColumn4.HeaderText = "Action";
        dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
        dataGridViewTextBoxColumn4.ReadOnly = true;
        dataGridViewTextBoxColumn4.Width = 55;
        // 
        // Description
        // 
        Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        Description.HeaderText = "Description";
        Description.Name = "Description";
        Description.ReadOnly = true;
        // 
        // Status
        // 
        Status.HeaderText = "✓";
        Status.Name = "Status";
        Status.ReadOnly = true;
        Status.Width = 25;
        // 
        // lblAuditStatus
        // 
        lblAuditStatus.AutoSize = true;
        lblAuditStatus.Dock = System.Windows.Forms.DockStyle.Fill;
        lblAuditStatus.Location = new System.Drawing.Point(3, 347);
        lblAuditStatus.Name = "lblAuditStatus";
        lblAuditStatus.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
        lblAuditStatus.Size = new System.Drawing.Size(946, 25);
        lblAuditStatus.TabIndex = 2;
        lblAuditStatus.Text = "Showing 0 of 0 records";
        // 
        // tabAudit
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(groupBoxLogs);
        MaximumSize = new System.Drawing.Size(958, 394);
        MinimumSize = new System.Drawing.Size(958, 394);
        Size = new System.Drawing.Size(958, 394);
        groupBoxLogs.ResumeLayout(false);
        tableLayoutPanelAuditLogs.ResumeLayout(false);
        tableLayoutPanelAuditLogs.PerformLayout();
        panelAuditFilters.ResumeLayout(false);
        panelAuditFilters.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxLogs;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAuditLogs;
    private System.Windows.Forms.Panel panelAuditFilters;
    private Button btnAuditRefresh;
    private TextBox txtAuditUserFilter;
    private Label lblAuditUserFilter;
    private ComboBox cbAuditCategoryFilter;
    private Label lblAuditCategory;
    private System.Windows.Forms.DataGridView dgvAuditLogs;
    private System.Windows.Forms.Label lblAuditStatus;
    private DataGridViewTextBoxColumn Time;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn Description;
    private DataGridViewTextBoxColumn Status;
}