using ServerManager.Classes.SupportClasses;
using HawkSyncShared.SupportClasses;
using System.ComponentModel;
using UserControl = System.Windows.Forms.UserControl;
namespace ServerManager.Forms.SubPanels.tabAdmin;
public partial class tabAudit : UserControl
{
    // --- Audit Log Fields ---
    private System.Windows.Forms.Timer? _auditFilterTimer;
    private string _currentUserFilter = string.Empty;
    private string _currentCategoryFilter = "All";
    
    private static bool IsDesignTime =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime || System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");
    
    public tabAudit()
    {
        InitializeComponent();
        
        if (IsDesignTime)
            return;
        
        InitializeAuditLogUI();
    }
    /// <summary>
    /// Initialize audit log UI components
    /// </summary>
    private void InitializeAuditLogUI()
    {
        // Initialize filter timer for debouncing user filter
        _auditFilterTimer = new System.Windows.Forms.Timer
        {
            Interval = 500 // 500ms debounce
        };
        _auditFilterTimer.Tick += (_, _) =>
        {
            _auditFilterTimer.Stop();
            LoadAuditLogs();
        };
        // Set default category filter
        if (cbAuditCategoryFilter.Items.Count > 0 && cbAuditCategoryFilter.SelectedIndex < 0)
        {
            cbAuditCategoryFilter.SelectedIndex = 0; // Select "All"
        }
        // Load initial data
        LoadAuditLogs();
    }
    /// <summary>
    /// Load audit logs from database with current filters
    /// </summary>
    private void LoadAuditLogs()
    {
        try
        {
            if (!DatabaseManager.IsInitialized)
            {
                lblAuditStatus.Text = "Database not initialized";
                AppDebug.Log("Database not initialized", AppDebug.LogLevel.Warning);
                return;
            }
            // Get logs from last 24 hours
            var startDate = DateTime.UtcNow.AddHours(-24);
            var endDate = DateTime.UtcNow;
            // Apply filters
            string? categoryFilter = _currentCategoryFilter == "All" ? null : _currentCategoryFilter;
            string? userFilter = string.IsNullOrWhiteSpace(_currentUserFilter) ? null : _currentUserFilter;
            var (logs, totalCount) = DatabaseManager.GetAuditLogs(
                startDate: startDate,
                endDate: endDate,
                usernameFilter: userFilter,
                categoryFilter: categoryFilter,
                limit: 500
            );
            // Update grid
            dgvAuditLogs.Rows.Clear();
            foreach (var log in logs)
            {
                var rowIndex = dgvAuditLogs.Rows.Add(
                    log.Timestamp.ToLocalTime().ToString("HH:mm:ss"),
                    log.Username,
                    log.ActionCategory,
                    log.ActionType,
                    log.ActionDescription,
                    log.Success ? "✓" : "✗"
                );
                var row = dgvAuditLogs.Rows[rowIndex];
                // Store full log object in Tag for detail view
                row.Tag = log;
                // Color code failed actions
                if (!log.Success)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                }
                // Add tooltip with full timestamp
                row.Cells["Time"].ToolTipText = log.Timestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                // Add tooltip with full description if truncated
                if (log.ActionDescription.Length > 40)
                {
                    row.Cells["Description"].ToolTipText = log.ActionDescription;
                }
            }
            // Update status label
            lblAuditStatus.Text = $"Showing {logs.Count} of {totalCount} records | Last updated: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            AppDebug.Log($"Failed to load audit logs", AppDebug.LogLevel.Error, ex);
            lblAuditStatus.Text = "Error loading audit logs";
        }
    }
    /// <summary>
    /// Category filter changed event handler
    /// </summary>
    private void cbAuditCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        _currentCategoryFilter = cbAuditCategoryFilter.SelectedItem?.ToString() ?? "All";
        LoadAuditLogs();
    }
    /// <summary>
    /// User filter text changed event handler (with debounce)
    /// </summary>
    private void txtAuditUserFilter_TextChanged(object sender, EventArgs e)
    {
        _currentUserFilter = txtAuditUserFilter.Text;
        // Reset timer for debouncing
        _auditFilterTimer?.Stop();
        _auditFilterTimer?.Start();
    }
    /// <summary>
    /// Refresh button click event handler
    /// </summary>
    private void btnAuditRefresh_Click(object sender, EventArgs e)
    {
        LoadAuditLogs();
    }
}
