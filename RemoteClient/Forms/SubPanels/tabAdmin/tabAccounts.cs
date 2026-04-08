using System.ComponentModel;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabAdmin;
using RemoteClient.Core;

namespace RemoteClient.Forms.SubPanels.tabAdmin;

public partial class tabAccounts : UserControl
{
    // ================================================================================
    // FIELDS
    // ================================================================================

    private int _selectedUserID = -1;
    private bool _isEditMode;

    private int? _lastSelectedUserId;
    private int _lastScrollIndex;

    private static bool IsDesignTime =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
        System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");

    private enum FormMode
    {
        View,
        Add,
        Edit
    }

    // ================================================================================
    // CONSTRUCTOR & INITIALIZATION
    // ================================================================================

    public tabAccounts()
    {
        InitializeComponent();

        if (IsDesignTime)
            return;

        InitializeEvents();
        LoadUserList();
        SetFormMode(FormMode.View);

        dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;
        Disposed += (_, _) => ApiCore.OnSnapshotReceived -= OnSnapshotReceived;
    }

    private void DataGridView1_SelectionChanged(object? sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow != null)
        {
            var val = dataGridView1.CurrentRow.Cells[0].Value;
            if (val != null && int.TryParse(val.ToString(), out int id) && id > 0)
                _lastSelectedUserId = id;
            else
                _lastSelectedUserId = null;
        }
        else
        {
            _lastSelectedUserId = null;
        }
    }

    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (IsDisposed || !IsHandleCreated)
            return;

        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }

        LoadUserList();
    }

    private void InitializeEvents()
    {
        dataGridView1.CellDoubleClick += DgUserList_CellDoubleClick;

        iconButton1.Click += BtnRefresh_Click;
        iconButton2.Click += BtnAddUser_Click;
        iconButton3.Click += BtnSave_Click;
        iconButton4.Click += BtnDelete_Click;
        iconButton6.Click += BtnCancel_Click;

        checkBox_showPass.CheckedChanged += CheckBox_showPass_CheckedChanged;
    }

    // ================================================================================
    // DATA LOADING
    // ================================================================================

    private void LoadUserList()
    {
        if (CommonCore.instanceAdmin == null)
            return;

        if (dataGridView1.Rows.Count > 0)
            _lastScrollIndex = dataGridView1.FirstDisplayedScrollingRowIndex;

        var users = CommonCore.instanceAdmin.Users.OrderBy(u => u.UserID).ToList();
        var userDict = users.ToDictionary(u => u.UserID);
        var onlineUsers = new HashSet<string>(CommonCore.instanceAdmin.ActiveSessions.Keys, StringComparer.OrdinalIgnoreCase);

        var gridUserIds = new HashSet<int>();
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;
            int userId = Convert.ToInt32(row.Cells[0].Value);
            gridUserIds.Add(userId);

            if (userDict.TryGetValue(userId, out var user))
            {
                bool isOnline = onlineUsers.Contains(user.Username);
                string status = isOnline ? "Online" : (user.IsActive ? "Active" : "Disabled");

                bool needsUpdate =
                    row.Cells[1].Value?.ToString() != user.Username ||
                    row.Cells[2].Value?.ToString() != status ||
                    row.Cells[3].Value?.ToString() != user.Created.ToLocalTime().ToString("yyyy-MM-dd") ||
                    row.Cells[4].Value?.ToString() != (user.LastLogin?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "Never");

                if (needsUpdate)
                {
                    row.Cells[1].Value = user.Username;
                    row.Cells[2].Value = status;
                    row.Cells[3].Value = user.Created.ToLocalTime().ToString("yyyy-MM-dd");
                    row.Cells[4].Value = user.LastLogin?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "Never";
                }

                row.DefaultCellStyle.BackColor = isOnline
                    ? Color.LightGreen
                    : dataGridView1.DefaultCellStyle.BackColor;
            }
            else
            {
                dataGridView1.Rows.Remove(row);
            }
        }

        foreach (var user in users)
        {
            if (!gridUserIds.Contains(user.UserID))
            {
                bool isOnline = onlineUsers.Contains(user.Username);
                string status = isOnline ? "Online" : (user.IsActive ? "Active" : "Disabled");

                int rowIndex = dataGridView1.Rows.Add(
                    user.UserID,
                    user.Username,
                    status,
                    user.Created.ToLocalTime().ToString("yyyy-MM-dd"),
                    user.LastLogin?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "Never");

                if (isOnline)
                    dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        dataGridView1.ClearSelection();
        if (_lastSelectedUserId.HasValue)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells[0].Value) == _lastSelectedUserId.Value)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        if (dataGridView1.RowCount > 0 && _lastScrollIndex >= 0 && _lastScrollIndex < dataGridView1.RowCount)
            dataGridView1.FirstDisplayedScrollingRowIndex = _lastScrollIndex;
    }

    // ================================================================================
    // FORM MODE MANAGEMENT
    // ================================================================================

    private void SetFormMode(FormMode mode)
    {
        switch (mode)
        {
            case FormMode.View:
                _isEditMode = false;
                _selectedUserID = -1;
                groupBox2.Enabled = false;
                iconButton3.Enabled = false;
                iconButton4.Enabled = false;
                iconButton6.Enabled = false;
                iconButton2.Enabled = true;
                textBox_password2.Visible = false;
                label_Confirm.Visible = false;
                ClearForm();
                break;

            case FormMode.Add:
                _isEditMode = false;
                _selectedUserID = -1;
                groupBox2.Enabled = true;
                iconButton3.Enabled = true;
                iconButton4.Enabled = false;
                iconButton6.Enabled = true;
                iconButton2.Enabled = false;
                textBox_password2.Visible = true;
                label_Confirm.Visible = true;
                ClearForm();
                textBox_username.Focus();
                break;

            case FormMode.Edit:
                _isEditMode = true;
                groupBox2.Enabled = true;
                iconButton3.Enabled = true;
                iconButton4.Enabled = _selectedUserID != 1;
                iconButton6.Enabled = true;
                iconButton2.Enabled = false;
                textBox_password2.Visible = false;
                label_Confirm.Visible = false;
                break;
        }
    }

    private void ClearForm()
    {
        textBox_username.Clear();
        textBox_password.Clear();
        textBox_password2.Clear();
        textBox_userNotes.Clear();
        checkBox_userActive.Checked = true;

        checkBox_permProfile.Checked = false;
        checkBox_permGamePlay.Checked = false;
        checkBox_permMaps.Checked = false;
        checkBox_permPlayers.Checked = false;
        checkBox_permChat.Checked = false;
        checkBox_permBans.Checked = false;
        checkBox_permStats.Checked = false;
        checkBox_permUsers.Checked = false;
        checkBox_permAudit.Checked = false;
    }

    private void LoadUserToForm(UserDTO user)
    {
        _selectedUserID = user.UserID;

        textBox_username.Text = user.Username;
        textBox_password.Clear();
        textBox_password2.Clear();
        textBox_userNotes.Text = user.Notes;
        checkBox_userActive.Checked = user.IsActive;

        checkBox_permProfile.Checked = user.Permissions.Contains("profile");
        checkBox_permGamePlay.Checked = user.Permissions.Contains("gameplay");
        checkBox_permMaps.Checked = user.Permissions.Contains("maps");
        checkBox_permPlayers.Checked = user.Permissions.Contains("players");
        checkBox_permChat.Checked = user.Permissions.Contains("chat");
        checkBox_permBans.Checked = user.Permissions.Contains("bans");
        checkBox_permStats.Checked = user.Permissions.Contains("stats");
        checkBox_permUsers.Checked = user.Permissions.Contains("users");
        checkBox_permAudit.Checked = user.Permissions.Contains("audit");
    }

    private List<string> GetSelectedPermissions()
    {
        var permissions = new List<string>();

        if (checkBox_permProfile.Checked) permissions.Add("profile");
        if (checkBox_permGamePlay.Checked) permissions.Add("gameplay");
        if (checkBox_permMaps.Checked) permissions.Add("maps");
        if (checkBox_permPlayers.Checked) permissions.Add("players");
        if (checkBox_permChat.Checked) permissions.Add("chat");
        if (checkBox_permBans.Checked) permissions.Add("bans");
        if (checkBox_permStats.Checked) permissions.Add("stats");
        if (checkBox_permUsers.Checked) permissions.Add("users");
        if (checkBox_permAudit.Checked) permissions.Add("audit");

        return permissions;
    }

    // ================================================================================
    // GRID EVENT HANDLERS
    // ================================================================================

    private void DgUserList_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || CommonCore.instanceAdmin == null)
            return;

        int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
        var user = CommonCore.instanceAdmin.Users.FirstOrDefault(u => u.UserID == userId);

        if (user != null)
        {
            LoadUserToForm(user);
            SetFormMode(FormMode.Edit);
        }
    }

    // ================================================================================
    // BUTTON EVENT HANDLERS
    // ================================================================================

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadUserList();
        SetFormMode(FormMode.View);
    }

    private void BtnAddUser_Click(object? sender, EventArgs e)
    {
        SetFormMode(FormMode.Add);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_isEditMode)
            UpdateUser();
        else
            _ = CreateUser();
    }

    private async void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_selectedUserID <= 0) return;

        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete user '{textBox_username.Text}'?\n\nThis action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        var result = await ApiCore.ApiClient!.Admin.DeleteUserAsync(_selectedUserID);

        if (result.Success)
        {
            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList();
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        SetFormMode(FormMode.View);
    }

    // ================================================================================
    // USER OPERATIONS
    // ================================================================================

    private async Task CreateUser()
    {
        if (textBox_password.Text != textBox_password2.Text)
        {
            MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var request = new CreateUserRequestDTO
        {
            Username = textBox_username.Text.Trim(),
            Password = textBox_password.Text,
            Permissions = GetSelectedPermissions(),
            IsActive = checkBox_userActive.Checked,
            Notes = textBox_userNotes.Text.Trim()
        };

        AdminCommandResult result = await ApiCore.ApiClient!.Admin.CreateUserAsync(request);

        if (result.Success)
        {
            MessageBox.Show("User created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList();
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void UpdateUser()
    {
        var request = new UpdateUserRequestDTO
        {
            UserID = _selectedUserID,
            Username = textBox_username.Text.Trim(),
            NewPassword = string.IsNullOrWhiteSpace(textBox_password.Text)
                ? null
                : textBox_password.Text,
            Permissions = GetSelectedPermissions(),
            IsActive = checkBox_userActive.Checked,
            Notes = textBox_userNotes.Text.Trim()
        };

        AdminCommandResult result = await ApiCore.ApiClient!.Admin.UpdateUserAsync(request);

        if (result.Success)
        {
            MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList();
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ================================================================================
    // OTHER EVENT HANDLERS
    // ================================================================================

    private void CheckBox_showPass_CheckedChanged(object? sender, EventArgs e)
    {
        textBox_password.UseSystemPasswordChar = !checkBox_showPass.Checked;
        textBox_password2.UseSystemPasswordChar = !checkBox_showPass.Checked;
    }
}