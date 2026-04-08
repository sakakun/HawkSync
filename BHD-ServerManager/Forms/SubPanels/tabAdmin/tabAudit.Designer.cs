using System.ComponentModel;

namespace ServerManager.Forms.SubPanels.tabAdmin;

partial class tabAudit
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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        groupBoxLogs = new GroupBox();
        tableLayoutPanelAuditLogs = new TableLayoutPanel();
        panelAuditFilters = new Panel();
        btnAuditRefresh = new Button();
        txtAuditUserFilter = new TextBox();
        lblAuditUserFilter = new Label();
        cbAuditCategoryFilter = new ComboBox();
        lblAuditCategory = new Label();
        dgvAuditLogs = new DataGridView();
        Time = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        Description = new DataGridViewTextBoxColumn();
        Status = new DataGridViewTextBoxColumn();
        lblAuditStatus = new Label();
        groupBoxLogs.SuspendLayout();
        tableLayoutPanelAuditLogs.SuspendLayout();
        panelAuditFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAuditLogs).BeginInit();
        SuspendLayout();
        // 
        // groupBoxLogs
        // 
        groupBoxLogs.Controls.Add(tableLayoutPanelAuditLogs);
        groupBoxLogs.Dock = DockStyle.Fill;
        groupBoxLogs.Location = new Point(0, 0);
        groupBoxLogs.Name = "groupBoxLogs";
        groupBoxLogs.Size = new Size(958, 394);
        groupBoxLogs.TabIndex = 3;
        groupBoxLogs.TabStop = false;
        groupBoxLogs.Text = "Audit Logs (Last 24 Hours)";
        // 
        // tableLayoutPanelAuditLogs
        // 
        tableLayoutPanelAuditLogs.ColumnCount = 1;
        tableLayoutPanelAuditLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanelAuditLogs.Controls.Add(panelAuditFilters, 0, 0);
        tableLayoutPanelAuditLogs.Controls.Add(dgvAuditLogs, 0, 1);
        tableLayoutPanelAuditLogs.Controls.Add(lblAuditStatus, 0, 2);
        tableLayoutPanelAuditLogs.Dock = DockStyle.Fill;
        tableLayoutPanelAuditLogs.Location = new Point(3, 19);
        tableLayoutPanelAuditLogs.Name = "tableLayoutPanelAuditLogs";
        tableLayoutPanelAuditLogs.RowCount = 3;
        tableLayoutPanelAuditLogs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tableLayoutPanelAuditLogs.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelAuditLogs.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
        tableLayoutPanelAuditLogs.Size = new Size(952, 372);
        tableLayoutPanelAuditLogs.TabIndex = 0;
        // 
        // panelAuditFilters
        // 
        panelAuditFilters.Controls.Add(btnAuditRefresh);
        panelAuditFilters.Controls.Add(txtAuditUserFilter);
        panelAuditFilters.Controls.Add(lblAuditUserFilter);
        panelAuditFilters.Controls.Add(cbAuditCategoryFilter);
        panelAuditFilters.Controls.Add(lblAuditCategory);
        panelAuditFilters.Dock = DockStyle.Fill;
        panelAuditFilters.Location = new Point(0, 0);
        panelAuditFilters.Margin = new Padding(0);
        panelAuditFilters.Name = "panelAuditFilters";
        panelAuditFilters.Size = new Size(952, 40);
        panelAuditFilters.TabIndex = 0;
        // 
        // btnAuditRefresh
        // 
        btnAuditRefresh.Location = new Point(350, 8);
        btnAuditRefresh.Name = "btnAuditRefresh";
        btnAuditRefresh.Size = new Size(75, 25);
        btnAuditRefresh.TabIndex = 4;
        btnAuditRefresh.Text = "🔍 Refresh";
        btnAuditRefresh.UseVisualStyleBackColor = true;
        btnAuditRefresh.Click += btnAuditRefresh_Click;
        // 
        // txtAuditUserFilter
        // 
        txtAuditUserFilter.Location = new Point(218, 9);
        txtAuditUserFilter.Name = "txtAuditUserFilter";
        txtAuditUserFilter.PlaceholderText = "Filter by user...";
        txtAuditUserFilter.Size = new Size(120, 23);
        txtAuditUserFilter.TabIndex = 3;
        txtAuditUserFilter.TextChanged += txtAuditUserFilter_TextChanged;
        // 
        // lblAuditUserFilter
        // 
        lblAuditUserFilter.AutoSize = true;
        lblAuditUserFilter.Location = new Point(180, 12);
        lblAuditUserFilter.Name = "lblAuditUserFilter";
        lblAuditUserFilter.Size = new Size(33, 15);
        lblAuditUserFilter.TabIndex = 2;
        lblAuditUserFilter.Text = "User:";
        // 
        // cbAuditCategoryFilter
        // 
        cbAuditCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbAuditCategoryFilter.FormattingEnabled = true;
        cbAuditCategoryFilter.Items.AddRange(new object[] { "All", "Ban", "Chat", "Player", "Map", "Settings", "Server", "Stats", "System", "User" });
        cbAuditCategoryFilter.Location = new Point(68, 9);
        cbAuditCategoryFilter.Name = "cbAuditCategoryFilter";
        cbAuditCategoryFilter.Size = new Size(100, 23);
        cbAuditCategoryFilter.TabIndex = 1;
        cbAuditCategoryFilter.SelectedIndexChanged += cbAuditCategoryFilter_SelectedIndexChanged;
        // 
        // lblAuditCategory
        // 
        lblAuditCategory.AutoSize = true;
        lblAuditCategory.Location = new Point(5, 12);
        lblAuditCategory.Name = "lblAuditCategory";
        lblAuditCategory.Size = new Size(58, 15);
        lblAuditCategory.TabIndex = 0;
        lblAuditCategory.Text = "Category:";
        // 
        // dgvAuditLogs
        // 
        dgvAuditLogs.AllowUserToAddRows = false;
        dgvAuditLogs.AllowUserToDeleteRows = false;
        dgvAuditLogs.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 240, 240);
        dgvAuditLogs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvAuditLogs.BorderStyle = BorderStyle.Fixed3D;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(230, 230, 230);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvAuditLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvAuditLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAuditLogs.Columns.AddRange(new DataGridViewColumn[] { Time, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, Description, Status });
        dgvAuditLogs.Dock = DockStyle.Fill;
        dgvAuditLogs.EnableHeadersVisualStyles = false;
        dgvAuditLogs.Location = new Point(3, 43);
        dgvAuditLogs.MultiSelect = false;
        dgvAuditLogs.Name = "dgvAuditLogs";
        dgvAuditLogs.ReadOnly = true;
        dgvAuditLogs.RowHeadersVisible = false;
        dgvAuditLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAuditLogs.Size = new Size(946, 304);
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
        Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        lblAuditStatus.Dock = DockStyle.Fill;
        lblAuditStatus.Location = new Point(3, 347);
        lblAuditStatus.Name = "lblAuditStatus";
        lblAuditStatus.Padding = new Padding(5, 5, 0, 0);
        lblAuditStatus.Size = new Size(946, 25);
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
        Name = "tabAudit";
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

    private GroupBox groupBoxLogs;
    private TableLayoutPanel tableLayoutPanelAuditLogs;
    private Panel panelAuditFilters;
    private Label lblAuditCategory;
    private ComboBox cbAuditCategoryFilter;
    private Label lblAuditUserFilter;
    private TextBox txtAuditUserFilter;
    private Button btnAuditRefresh;
    private DataGridView dgvAuditLogs;
    private Label lblAuditStatus;
    private DataGridViewTextBoxColumn Time;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn Description;
    private DataGridViewTextBoxColumn Status;
}