using HawkSyncShared;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.SubPanels
{
    public partial class ChatHistory : UserControl
    {
        private List<ChatLogObject> _currentLogs = new();
        private int _totalCount = 0;
        private int _currentPage = 1;
        private int _pageSize = 100;
        private bool _isLoading = false;

        public ChatHistory()
        {
            InitializeComponent();
            InitializeControls();
        }

        /// <summary>
        /// Initialize control defaults and load player filter
        /// </summary>
        private void InitializeControls()
        {
            // Set default date range to "Today"
            if (comboBox_DateRange.Items.Count > 0)
                comboBox_DateRange.SelectedIndex = 0;

            // Set default page size to 100
            if (comboBox_PageSize.Items.Count > 0)
                comboBox_PageSize.SelectedIndex = 1; // Assuming "100" is at index 1

            // Set default filters
            if (comboBox_TypeFilter.Items.Count > 0)
                comboBox_TypeFilter.SelectedIndex = 0; // All Types

            // Disable date pickers initially (enabled only for Custom date range)
            if (dateTimePicker_From != null)
                dateTimePicker_From.Enabled = false;
            if (dateTimePicker_To != null)
                dateTimePicker_To.Enabled = false;

            // Set initial pagination state
            UpdatePaginationControls();

            // Load player filter in background
            LoadPlayerFilter();
        }

        /// <summary>
        /// Load distinct player names for the filter dropdown
        /// </summary>
        private void LoadPlayerFilter()
        {
            try
            {
                var players = DatabaseManager.GetDistinctPlayerNames(500);

                comboBox_PlayerFilter.Items.Clear();
                comboBox_PlayerFilter.Items.Add("All Players");

                if (players != null && players.Count > 0)
                {
                    comboBox_PlayerFilter.Items.AddRange(players.ToArray());
                    AppDebug.Log("ChatHistory", $"Loaded {players.Count} player names for filter");
                }

                comboBox_PlayerFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                AppDebug.Log("ChatHistory", $"Failed to load player names: {ex.Message}");
                comboBox_PlayerFilter.Items.Clear();
                comboBox_PlayerFilter.Items.Add("All Players");
                comboBox_PlayerFilter.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Load History button click handler
        /// </summary>
        private void btn_LoadHistory_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            LoadHistory();
        }

        /// <summary>
        /// Main method to load chat history from database with filters
        /// </summary>
        private void LoadHistory()
        {
            if (_isLoading)
            {
                AppDebug.Log("ChatHistory", "Load already in progress, skipping");
                return;
            }

            _isLoading = true;

            try
            {
                // Show loading state
                label_Pagination.Text = "Loading...";
                btn_LoadHistory.Enabled = false;

                // Get date range
                (DateTime? startDate, DateTime? endDate) = GetDateRange();

                // Get player filter
                string? playerFilter = null;
                if (comboBox_PlayerFilter.SelectedIndex > 0)
                {
                    playerFilter = comboBox_PlayerFilter.SelectedItem?.ToString();
                }

                // Get message type filter
                int? typeFilter = null;
                int? teamFilter = null;
                if (comboBox_TypeFilter.SelectedIndex > 0)
                {
                    // Map UI index to message type and team
                    switch (comboBox_TypeFilter.SelectedIndex)
                    {
                        case 1: // Server Messages
                            typeFilter = 0;
                            break;
                        case 2: // Global Chat
                            typeFilter = 1;
                            break;
                        case 3: // All Team Chat
                            typeFilter = 2;
                            break;
                        case 4: // Red Team Chat
                            typeFilter = 2;
                            teamFilter = 2; // Red team
                            break;
                        case 5: // Blue Team Chat
                            typeFilter = 2;
                            teamFilter = 1; // Blue team
                            break;
                    }
                }

                // Get search text
                string? searchText = string.IsNullOrWhiteSpace(textBox_Search.Text)
                    ? null
                    : textBox_Search.Text.Trim();

                // Load from database
                var result = DatabaseManager.GetChatLogsFiltered(
                    startDate,
                    endDate,
                    playerFilter,
                    typeFilter,
                    teamFilter,
                    searchText,
                    _currentPage,
                    _pageSize
                );

                _currentLogs = result.logs;
                _totalCount = result.totalCount;

                AppDebug.Log("ChatHistory", $"Loaded {_currentLogs.Count} of {_totalCount} messages (Page {_currentPage})");

                // Update UI
                UpdateDataGrid();
                UpdatePaginationControls();

                // Show message if no results
                if (_currentLogs.Count == 0)
                {
                    MessageBox.Show(
                        "No chat messages found matching the selected filters.",
                        "No Results",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading chat history:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                AppDebug.Log("ChatHistory", $"Error loading history: {ex.Message}");

                // Reset state
                _currentLogs.Clear();
                _totalCount = 0;
                dataGridView_History.Rows.Clear();
                label_Pagination.Text = "Error loading data";
            }
            finally
            {
                _isLoading = false;
                btn_LoadHistory.Enabled = true;
            }
        }

        /// <summary>
        /// Convert date range combo selection to DateTime values
        /// </summary>
        private (DateTime?, DateTime?) GetDateRange()
        {
            if (comboBox_DateRange.SelectedIndex < 0)
                return (null, null);

            return comboBox_DateRange.SelectedIndex switch
            {
                0 => (DateTime.Today, DateTime.Now),                    // Today
                1 => (DateTime.Today.AddDays(-1), DateTime.Today),      // Yesterday
                2 => (DateTime.Today.AddDays(-3), DateTime.Now),        // Last 3 Days
                3 => (DateTime.Today.AddDays(-7), DateTime.Now),        // Last 7 Days
                4 => (DateTime.Today.AddDays(-30), DateTime.Now),       // Last 30 Days
                5 => (null, null),                                      // All Time
                6 => (dateTimePicker_From.Value, dateTimePicker_To.Value), // Custom
                _ => (null, null)
            };
        }

        /// <summary>
        /// Populate DataGridView with current logs
        /// </summary>
        private void UpdateDataGrid()
        {
            dataGridView_History.Rows.Clear();

            foreach (var log in _currentLogs)
            {
                dataGridView_History.Rows.Add(
                    log.MessageTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.TeamDisplay ?? "Unknown",
                    log.PlayerName,
                    log.MessageText
                );
            }

            AppDebug.Log("ChatHistory", $"Updated grid with {_currentLogs.Count} rows");
        }

        /// <summary>
        /// Update pagination controls and labels
        /// </summary>
        private void UpdatePaginationControls()
        {
            if (_totalCount == 0)
            {
                label_Pagination.Text = "No data loaded";
                btn_PrevPage.Enabled = false;
                btn_NextPage.Enabled = false;
                return;
            }

            int totalPages = (int)Math.Ceiling(_totalCount / (double)_pageSize);

            label_Pagination.Text = $"Page {_currentPage} of {totalPages} ({_totalCount:N0} messages)";

            btn_PrevPage.Enabled = _currentPage > 1;
            btn_NextPage.Enabled = _currentPage < totalPages;

            AppDebug.Log("ChatHistory", $"Pagination: Page {_currentPage}/{totalPages}, Total: {_totalCount}");
        }

        /// <summary>
        /// Previous page button handler
        /// </summary>
        private void btn_PrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadHistory();
            }
        }

        /// <summary>
        /// Next page button handler
        /// </summary>
        private void btn_NextPage_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling(_totalCount / (double)_pageSize);
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadHistory();
            }
        }

        /// <summary>
        /// Page size combo box changed handler
        /// </summary>
        private void comboBox_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_PageSize.SelectedItem == null)
                return;

            // Parse page size from combo box
            string selectedText = comboBox_PageSize.SelectedItem.ToString() ?? "100";

            if (int.TryParse(selectedText, out int newPageSize))
            {
                if (_pageSize != newPageSize)
                {
                    _pageSize = newPageSize;
                    _currentPage = 1; // Reset to first page

                    // Only reload if we have data
                    if (_currentLogs.Count > 0)
                    {
                        LoadHistory();
                    }

                    AppDebug.Log("ChatHistory", $"Page size changed to {_pageSize}");
                }
            }
        }

        /// <summary>
        /// Clear filters button handler
        /// </summary>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            // Reset all filters to defaults
            comboBox_DateRange.SelectedIndex = 0;
            comboBox_PlayerFilter.SelectedIndex = 0;
            comboBox_TypeFilter.SelectedIndex = 0;
            textBox_Search.Clear();

            // Clear results
            dataGridView_History.Rows.Clear();
            _currentLogs.Clear();
            _totalCount = 0;
            _currentPage = 1;

            // Update UI
            UpdatePaginationControls();

            AppDebug.Log("ChatHistory", "Filters cleared");
        }

        /// <summary>
        /// Export to CSV button handler
        /// </summary>
        private void btn_ExportCSV_Click(object sender, EventArgs e)
        {
            if (_currentLogs == null || _currentLogs.Count == 0)
            {
                MessageBox.Show(
                    "No data to export. Please load chat history first.",
                    "Export",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1,
                FileName = $"ChatHistory_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                DefaultExt = "csv",
                Title = "Export Chat History to CSV"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var writer = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);

                    // Write header
                    writer.WriteLine("Timestamp,Team,Player,MessageType,Message");

                    // Write data rows
                    foreach (var log in _currentLogs)
                    {
                        // Escape fields for CSV (wrap in quotes, escape internal quotes)
                        string timestamp = log.MessageTimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
                        string team = EscapeCsvField(log.TeamDisplay ?? "Unknown");
                        string player = EscapeCsvField(log.PlayerName);
                        string messageType = log.MessageType.ToString();
                        string message = EscapeCsvField(log.MessageText);

                        writer.WriteLine($"{timestamp},{team},{player},{messageType},{message}");
                    }

                    MessageBox.Show(
                        $"Successfully exported {_currentLogs.Count} messages to:\n{sfd.FileName}",
                        "Export Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    AppDebug.Log("ChatHistory", $"Exported {_currentLogs.Count} messages to CSV: {sfd.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Export failed:\n\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    AppDebug.Log("ChatHistory", $"Export error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Escape field for CSV format (RFC 4180)
        /// </summary>
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "\"\"";

            // If field contains comma, quote, or newline, wrap in quotes and escape internal quotes
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }

        /// <summary>
        /// Date range combo box changed handler
        /// </summary>
        private void comboBox_DateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable/disable custom date pickers based on selection
            bool isCustom = comboBox_DateRange.SelectedIndex == 6;

            if (dateTimePicker_From != null)
                dateTimePicker_From.Enabled = isCustom;

            if (dateTimePicker_To != null)
                dateTimePicker_To.Enabled = isCustom;

            AppDebug.Log("ChatHistory", $"Date range changed to index {comboBox_DateRange.SelectedIndex}, Custom: {isCustom}");
        }

        /// <summary>
        /// Search textbox key press handler (Enter key triggers search)
        /// </summary>
        private void textBox_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_LoadHistory_Click(sender, e);
                e.Handled = true; // Prevent beep sound
            }
        }

        /// <summary>
        /// DataGridView cell formatting for better readability
        /// </summary>
        private void dataGridView_History_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Color-code based on Team column (index 1)
            if (dataGridView_History.Columns[e.ColumnIndex].Name == "Column_Team" && e.RowIndex < dataGridView_History.Rows.Count)
            {
                var teamCell = dataGridView_History.Rows[e.RowIndex].Cells[1];
                string? teamValue = teamCell.Value?.ToString();

                if (!string.IsNullOrEmpty(teamValue))
                {
                    dataGridView_History.Rows[e.RowIndex].DefaultCellStyle.BackColor = teamValue switch
                    {
                        "Server" => System.Drawing.Color.LightYellow,
                        "Blue" => System.Drawing.Color.LightBlue,
                        "Red" => System.Drawing.Color.LightCoral,
                        "Global" => System.Drawing.Color.LightGreen,
                        _ => System.Drawing.Color.White
                    };
                }
            }
        }

        /// <summary>
        /// Refresh player filter button handler (optional, if you add a refresh button)
        /// </summary>
        public void RefreshPlayerFilter()
        {
            LoadPlayerFilter();
        }

        /// <summary>
        /// Public method to trigger a search programmatically (optional)
        /// </summary>
        public void SearchChatHistory(string searchText)
        {
            textBox_Search.Text = searchText;
            btn_LoadHistory_Click(this, EventArgs.Empty);
        }
    }
}
