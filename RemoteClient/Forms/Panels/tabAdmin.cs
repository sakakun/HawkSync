using FontAwesome.Sharp;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.SupportClasses;
using RemoteClient.Core;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RemoteClient.Forms.Panels;

public partial class tabAdmin : UserControl
{
    // ================================================================================
    // FIELDS
    // ================================================================================

    private int _selectedUserID = -1;
    private bool _isEditMode = false;

    // Add these fields to your class
    private DateTime _lastUserListRefresh = DateTime.MinValue;
    private const int UserListRefreshIntervalSeconds = 10;
    // Add this field to your class to persist the last selected user ID
    private int? _lastSelectedUserId = null;
    private int _lastScrollIndex = 0;

    private enum FormMode
    {
        View,
        Add,
        Edit
    }

    // ================================================================================
    // CONSTRUCTOR & INITIALIZATION
    // ================================================================================

    public tabAdmin()
    {
        InitializeComponent();
        InitializeEvents();
        LoadUserList();
        SetFormMode(FormMode.View);
        dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

        // Subscribe to server updates
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;

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
        if(InvokeRequired)
        {
            Invoke(new Action(() => OnSnapshotReceived(snapshot)));
            return;
        }

        LoadUserList();

    }

    private void InitializeEvents()
    {
        // Grid events
        dataGridView1.CellDoubleClick += DgUserList_CellDoubleClick;

        // Button events
        iconButton1.Click += BtnRefresh_Click;      // Refresh
        iconButton2.Click += BtnAddUser_Click;      // Add User
        iconButton3.Click += BtnSave_Click;         // Save
        iconButton4.Click += BtnDelete_Click;       // Delete
        iconButton6.Click += BtnCancel_Click;       // Cancel

        // Checkbox events
        checkBox_showPass.CheckedChanged += CheckBox_showPass_CheckedChanged;
    }

    // ================================================================================
    // DATA LOADING
    // ================================================================================

    /// <summary>
    /// Load all users into the DataGridView from cache
    /// </summary>
    private void LoadUserList()
    {
        // Store scroll position before updating
        if (dataGridView1.Rows.Count > 0)
            _lastScrollIndex = dataGridView1.FirstDisplayedScrollingRowIndex;

        var users = CommonCore.instanceAdmin!.Users.OrderBy(u => u.UserID).ToList();

        // Build a lookup for quick access
        var userDict = users.ToDictionary(u => u.UserID);

        // Track which user IDs are present in the grid
        var gridUserIds = new HashSet<int>();
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;
            int userId = Convert.ToInt32(row.Cells[0].Value);
            gridUserIds.Add(userId);

            if (userDict.TryGetValue(userId, out var user))
            {
                // Update row if any data has changed
                bool needsUpdate =
                    row.Cells[1].Value?.ToString() != user.Username ||
                    row.Cells[2].Value?.ToString() != (user.IsActive ? "Active" : "Disabled") ||
                    row.Cells[3].Value?.ToString() != user.Created.ToString("yyyy-MM-dd") ||
                    row.Cells[4].Value?.ToString() != (user.LastLogin?.ToString("yyyy-MM-dd HH:mm") ?? "Never");

                if (needsUpdate)
                {
                    row.Cells[1].Value = user.Username;
                    row.Cells[2].Value = user.IsActive ? "Active" : "Disabled";
                    row.Cells[3].Value = user.Created.ToString("yyyy-MM-dd");
                    row.Cells[4].Value = user.LastLogin?.ToString("yyyy-MM-dd HH:mm") ?? "Never";
                }
            }
            else
            {
                // User no longer exists, remove row
                dataGridView1.Rows.Remove(row);
            }
        }

        // Add new users not present in the grid
        foreach (var user in users)
        {
            if (!gridUserIds.Contains(user.UserID))
            {
                dataGridView1.Rows.Add(
                    user.UserID,
                    user.Username,
                    user.IsActive ? "Active" : "Disabled",
                    user.Created.ToString("yyyy-MM-dd"),
                    user.LastLogin?.ToString("yyyy-MM-dd HH:mm") ?? "Never"
                );
            }
        }

        // Restore selection if possible
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

        // Restore scroll position if possible
        if (dataGridView1.RowCount > 0 && _lastScrollIndex >= 0 && _lastScrollIndex < dataGridView1.RowCount)
        {
            dataGridView1.FirstDisplayedScrollingRowIndex = _lastScrollIndex;
        }

        AppDebug.Log("tabAdmin", $"Loaded {users.Count} users from cache (differential update)");
    }

    // ================================================================================
    // FORM MODE MANAGEMENT
    // ================================================================================

    /// <summary>
    /// Set the form to a specific mode (View/Add/Edit)
    /// </summary>
    private void SetFormMode(FormMode mode)
    {
        switch (mode)
        {
            case FormMode.View:
                _isEditMode = false;
                _selectedUserID = -1;
                groupBox2.Enabled = false;
                iconButton3.Enabled = false;  // Save
                iconButton4.Enabled = false;  // Delete
                iconButton6.Enabled = false;  // Cancel
                iconButton2.Enabled = true;   // Add User
                textBox_password2.Visible = false;
                label_Confirm.Visible = false;
                ClearForm();
                break;

            case FormMode.Add:
                _isEditMode = false;
                _selectedUserID = -1;
                groupBox2.Enabled = true;
                iconButton3.Enabled = true;   // Save
                iconButton4.Enabled = false;  // Delete
                iconButton6.Enabled = true;   // Cancel
                iconButton2.Enabled = false;  // Add User
                textBox_password2.Visible = true;
                label_Confirm.Visible = true;
                ClearForm();
                textBox_username.Focus();
                break;

            case FormMode.Edit:
                _isEditMode = true;
                groupBox2.Enabled = true;
                iconButton3.Enabled = true;   // Save
                iconButton4.Enabled = (_selectedUserID != 1); // Can't delete admin
                iconButton6.Enabled = true;   // Cancel
                iconButton2.Enabled = false;  // Add User
                textBox_password2.Visible = false;
                label_Confirm.Visible = false;
                break;
        }
    }

    /// <summary>
    /// Clear all form fields
    /// </summary>
    private void ClearForm()
    {
        textBox_username.Clear();
        textBox_password.Clear();
        textBox_password2.Clear();
        textBox_userNotes.Clear();
        checkBox_userActive.Checked = true;

        // Clear all permissions
        checkBox_permProfile.Checked = false;
        checkBox_permGamePlay.Checked = false;
        checkBox_permMaps.Checked = false;
        checkBox_permPlayers.Checked = false;
        checkBox_permChat.Checked = false;
        checkBox_permBans.Checked = false;
        checkBox_permStats.Checked = false;
        checkBox_permUsers.Checked = false;
    }

    /// <summary>
    /// Load a user's data into the form for editing
    /// </summary>
    private void LoadUserToForm(UserDTO user)
    {
        _selectedUserID = user.UserID;

        textBox_username.Text = user.Username;
        textBox_password.Clear(); // Don't show password
        textBox_password2.Clear();
        textBox_userNotes.Text = user.Notes;
        checkBox_userActive.Checked = user.IsActive;

        // Load permissions
        checkBox_permProfile.Checked = user.Permissions.Contains("profile");
        checkBox_permGamePlay.Checked = user.Permissions.Contains("gameplay");
        checkBox_permMaps.Checked = user.Permissions.Contains("maps");
        checkBox_permPlayers.Checked = user.Permissions.Contains("players");
        checkBox_permChat.Checked = user.Permissions.Contains("chat");
        checkBox_permBans.Checked = user.Permissions.Contains("bans");
        checkBox_permStats.Checked = user.Permissions.Contains("stats");
        checkBox_permUsers.Checked = user.Permissions.Contains("users");
    }

    /// <summary>
    /// Build a list of selected permissions from checkboxes
    /// </summary>
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

        return permissions;
    }

    // ================================================================================
    // GRID EVENT HANDLERS
    // ================================================================================

    /// <summary>
    /// Double-click on a user row to edit
    /// </summary>
    private void DgUserList_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
        
        // Get from cache
        var user = CommonCore.instanceAdmin!.Users.FirstOrDefault(u => u.UserID == userId);

        if (user != null)
        {
            LoadUserToForm(user);
            SetFormMode(FormMode.Edit);
        }
    }

    // ================================================================================
    // BUTTON EVENT HANDLERS
    // ================================================================================

    /// <summary>
    /// Refresh button clicked - reload cache from database
    /// </summary>
    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadUserList();
        LoadUserList();
        SetFormMode(FormMode.View);
    }

    /// <summary>
    /// Add User button clicked
    /// </summary>
    private void BtnAddUser_Click(object? sender, EventArgs e)
    {
        SetFormMode(FormMode.Add);
    }

    /// <summary>
    /// Save button clicked
    /// </summary>
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_isEditMode)
        {
            UpdateUser();
        }
        else
        {
            _ = CreateUser();
        }
    }

    /// <summary>
    /// Delete button clicked
    /// </summary>
    private async void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_selectedUserID <= 0) return;

        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete user '{textBox_username.Text}'?\n\nThis action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        var result = await ApiCore.ApiClient!.DeleteUserAsync(_selectedUserID);

        if (result.Success)
        {
            MessageBox.Show("User deleted successfully.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList(); // Cache already updated by manager
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Cancel button clicked
    /// </summary>
    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        SetFormMode(FormMode.View);
    }

    // ================================================================================
    // USER OPERATIONS
    // ================================================================================

    /// <summary>
    /// Create a new user
    /// </summary>
    private async Task CreateUser()
    {

        // Validate passwords match
        if (textBox_password.Text != textBox_password2.Text)
        {
            MessageBox.Show("Passwords do not match.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // Call manager (cache will be auto-updated)
        AdminCommandResult result = await ApiCore.ApiClient!.CreateUserAsync(request);

        if (result.Success)
        {
            MessageBox.Show("User created successfully.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList(); // Refresh grid from updated cache
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
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

        // Call manager (cache will be auto-updated)
        AdminCommandResult result = await ApiCore.ApiClient!.UpdateUserAsync(request);

        if (result.Success)
        {
            MessageBox.Show("User updated successfully.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUserList(); // Refresh grid from updated cache
            SetFormMode(FormMode.View);
        }
        else
        {
            MessageBox.Show(result.Message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ================================================================================
    // OTHER EVENT HANDLERS
    // ================================================================================

    /// <summary>
    /// Show/hide password checkbox changed
    /// </summary>
    private void CheckBox_showPass_CheckedChanged(object? sender, EventArgs e)
    {
        textBox_password.UseSystemPasswordChar = !checkBox_showPass.Checked;
        textBox_password2.UseSystemPasswordChar = !checkBox_showPass.Checked;
    }
}