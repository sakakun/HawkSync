using HawkSyncShared.DTOs.Audit;
using RemoteClient.Core;
using System.Net.Http.Json;

namespace RemoteClient.Forms.SubPanels.tabAdmin;

public partial class tabAudit : UserControl
{
    private System.Windows.Forms.Timer? _auditFilterTimer;
    private string _currentUserFilter = string.Empty;
    private string _currentCategoryFilter = "All";

    public tabAudit()
    {
        InitializeComponent();
        InitializeAuditLogUI();
    }

    private void InitializeAuditLogUI()
    {
        _auditFilterTimer = new System.Windows.Forms.Timer { Interval = 500 };
        _auditFilterTimer.Tick += (_, _) =>
        {
            _auditFilterTimer.Stop();
            LoadAuditLogs();
        };

        if (cbAuditCategoryFilter.Items.Count > 0 && cbAuditCategoryFilter.SelectedIndex < 0)
            cbAuditCategoryFilter.SelectedIndex = 0;

        LoadAuditLogs();
    }

    private async void LoadAuditLogs()
    {
        try
        {
            if (ApiCore.ApiClient == null)
            {
                lblAuditStatus.Text = "Not connected to server";
                return;
            }

            var request = new AuditLogRequest
            {
                StartDate = DateTime.UtcNow.AddHours(-24),
                EndDate = DateTime.UtcNow,
                UsernameFilter = string.IsNullOrWhiteSpace(_currentUserFilter) ? null : _currentUserFilter,
                CategoryFilter = _currentCategoryFilter == "All" ? null : _currentCategoryFilter,
                Limit = 500
            };

            var httpResponse = await ApiCore.ApiClient._httpClient.PostAsJsonAsync("/api/audit/logs", request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                lblAuditStatus.Text = "Failed to load audit logs";
                return;
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<AuditLogResponse>();
            if (response == null)
            {
                lblAuditStatus.Text = "Failed to parse audit logs";
                return;
            }

            dgvAuditLogs.Rows.Clear();

            foreach (var log in response.Logs)
            {
                var rowIndex = dgvAuditLogs.Rows.Add(
                    log.Timestamp.ToLocalTime().ToString("HH:mm:ss"),
                    log.Username,
                    log.ActionCategory,
                    log.ActionType,
                    log.ActionDescription,
                    log.Success ? "✓" : "✗");

                var row = dgvAuditLogs.Rows[rowIndex];
                row.Tag = log;

                if (!log.Success)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                }

                row.Cells["Time"].ToolTipText = log.Timestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                if (log.ActionDescription.Length > 40)
                    row.Cells["Description"].ToolTipText = log.ActionDescription;
            }

            lblAuditStatus.Text = $"Showing {response.Logs.Count} of {response.TotalCount} records | Last updated: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            lblAuditStatus.Text = $"Error loading audit logs: {ex.Message}";
        }
    }

    private void cbAuditCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        _currentCategoryFilter = cbAuditCategoryFilter.SelectedItem?.ToString() ?? "All";
        LoadAuditLogs();
    }

    private void txtAuditUserFilter_TextChanged(object sender, EventArgs e)
    {
        _currentUserFilter = txtAuditUserFilter.Text;
        _auditFilterTimer?.Stop();
        _auditFilterTimer?.Start();
    }

    private void btnAuditRefresh_Click(object sender, EventArgs e)
    {
        LoadAuditLogs();
    }
}