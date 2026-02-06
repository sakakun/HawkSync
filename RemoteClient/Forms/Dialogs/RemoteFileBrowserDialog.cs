using HawkSyncShared.DTOs.tabProfile;
using RemoteClient.Core;

namespace RemoteClient.Forms.Dialogs;

public partial class RemoteFileBrowserDialog : Form
{
    private string _currentPath = string.Empty;
    private string? _selectedPath;
    private string? _fileFilter;
    private bool _selectFiles;

    public string? SelectedPath => _selectedPath;

    private ListView? listView;
    private ComboBox? cmbDrives;
    private TextBox? txtPath;
    private Button? btnUp;
    private Button? btnOK;
    private Button? btnCancel;

    public RemoteFileBrowserDialog(bool selectFiles = false, string? fileFilter = null)
    {
        _selectFiles = selectFiles;
        _fileFilter = fileFilter;
        InitializeComponent();
        LoadDrives();
    }

    private void InitializeComponent()
    {
        this.Text = _selectFiles ? "Browse Server - Select File" : "Browse Server - Select Folder";
        this.Size = new Size(600, 450);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.MinimumSize = new Size(500, 400);

        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(10)
        };

        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        // Drive selector
        var drivePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        var lblDrive = new Label
        {
            Text = "Drive:",
            AutoSize = true,
            Margin = new Padding(0, 7, 10, 0)
        };

        cmbDrives = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 100
        };
        cmbDrives.SelectedIndexChanged += CmbDrives_SelectedIndexChanged;

        drivePanel.Controls.Add(lblDrive);
        drivePanel.Controls.Add(cmbDrives);

        // Path navigation
        var pathPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1
        };

        pathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        pathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pathPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));

        var lblPath = new Label
        {
            Text = "Location:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        txtPath = new TextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true
        };

        btnUp = new Button
        {
            Text = "↑",
            Dock = DockStyle.Fill,
            Font = new Font(Font.FontFamily, 12, FontStyle.Bold)
        };
        btnUp.Click += BtnUp_Click;

        pathPanel.Controls.Add(lblPath, 0, 0);
        pathPanel.Controls.Add(txtPath, 1, 0);
        pathPanel.Controls.Add(btnUp, 2, 0);

        // File/Folder list
        listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true
        };

        listView.Columns.Add("Name", 300);
        listView.Columns.Add("Type", 100);
        listView.Columns.Add("Size", 100);
        listView.Columns.Add("Modified", 150);

        listView.DoubleClick += ListView_DoubleClick;
        listView.SelectedIndexChanged += ListView_SelectedIndexChanged;

        // Buttons
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 80,
            Height = 30
        };

        btnOK = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Width = 80,
            Height = 30,
            Enabled = false
        };
        btnOK.Click += BtnOK_Click;

        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnOK);

        mainLayout.Controls.Add(drivePanel, 0, 0);
        mainLayout.Controls.Add(pathPanel, 0, 1);
        mainLayout.Controls.Add(listView, 0, 2);
        mainLayout.Controls.Add(buttonPanel, 0, 3);

        this.Controls.Add(mainLayout);
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }

    private async void LoadDrives()
    {
        try
        {
            var response = await ApiCore.ApiClient!.GetServerDrivesAsync();

            if (response?.Success == true && response.Drives.Any())
            {
                cmbDrives!.Items.Clear();
                foreach (var drive in response.Drives)
                {
                    cmbDrives.Items.Add(drive);
                }

                if (cmbDrives.Items.Count > 0)
                {
                    cmbDrives.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show(
                    $"Failed to load drives: {response?.Message ?? "Unknown error"}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error loading drives: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void CmbDrives_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbDrives!.SelectedItem != null)
        {
            await LoadDirectory(cmbDrives.SelectedItem.ToString()!);
        }
    }

    private async void BtnUp_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_currentPath))
        {
            var response = await ApiCore.ApiClient!.GetDirectoryListingAsync(_currentPath);

            if (response?.Success == true && !string.IsNullOrEmpty(response.ParentPath))
            {
                await LoadDirectory(response.ParentPath);
            }
        }
    }

    private async void ListView_DoubleClick(object? sender, EventArgs e)
    {
        if (listView!.SelectedItems.Count > 0)
        {
            var item = listView.SelectedItems[0];
            var entry = item.Tag as FileSystemEntry;

            if (entry != null && entry.IsDirectory)
            {
                await LoadDirectory(entry.FullPath);
            }
            else if (entry != null && _selectFiles)
            {
                _selectedPath = entry.FullPath;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

    private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (listView!.SelectedItems.Count > 0)
        {
            var entry = listView.SelectedItems[0].Tag as FileSystemEntry;

            if (_selectFiles)
            {
                btnOK!.Enabled = entry != null && !entry.IsDirectory;
            }
            else
            {
                btnOK!.Enabled = entry != null && entry.IsDirectory;
            }
        }
        else
        {
            btnOK!.Enabled = !_selectFiles; // Allow selecting current folder
        }
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        if (listView!.SelectedItems.Count > 0)
        {
            var entry = listView.SelectedItems[0].Tag as FileSystemEntry;
            _selectedPath = entry?.FullPath;
        }
        else if (!_selectFiles)
        {
            _selectedPath = _currentPath;
        }
    }

    private async Task LoadDirectory(string path)
    {
        try
        {
            listView!.Items.Clear();
            btnOK!.Enabled = false;

            var response = await ApiCore.ApiClient!.GetDirectoryListingAsync(path, _fileFilter);

            if (response?.Success == true)
            {
                _currentPath = response.CurrentPath;
                txtPath!.Text = _currentPath;

                btnUp!.Enabled = !string.IsNullOrEmpty(response.ParentPath);

                foreach (var entry in response.Entries)
                {
                    var item = new ListViewItem(entry.Name);
                    item.SubItems.Add(entry.IsDirectory ? "Folder" : "File");
                    item.SubItems.Add(entry.IsDirectory ? "" : FormatFileSize(entry.Size));
                    item.SubItems.Add(entry.LastModified.ToString("yyyy-MM-dd HH:mm"));
                    item.Tag = entry;

                    if (entry.IsDirectory)
                    {
                        item.Font = new Font(item.Font, FontStyle.Bold);
                        item.ForeColor = Color.Blue;
                    }

                    listView.Items.Add(item);
                }

                // Allow selecting current folder if not selecting files
                if (!_selectFiles)
                {
                    btnOK.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show(
                    $"Failed to load directory: {response?.Message ?? "Unknown error"}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error loading directory: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}