using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.DTOs;
using static BHD_ServerManager.Classes.InstanceManagers.adminInstanceManager;

namespace BHD_ServerManager.Forms.Panels;

public partial class tabAdmin : UserControl
{
    // ================================================================================
    // FIELDS
    // ================================================================================

    private int _selectedUserID = -1;
    private bool _isEditMode = false;

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
        dataGridView1.Rows.Clear();

        // Get users from cache (adminInstance.Users)
        var users = CommonCore.instanceAdmin!.Users;

        foreach (var user in users.OrderBy(u => u.UserID))
        {
            dataGridView1.Rows.Add(
                user.UserID,
                user.Username,
                user.IsActive ? "Active" : "Disabled",
                user.Created.ToString("yyyy-MM-dd"),
                user.LastLogin?.ToString("yyyy-MM-dd HH:mm") ?? "Never"
            );
        }

        AppDebug.Log("tabAdmin", $"Loaded {users.Count} users from cache");
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
        adminInstanceManager.LoadUsersCache();
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
            CreateUser();
        }
    }

    /// <summary>
    /// Delete button clicked
    /// </summary>
    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_selectedUserID <= 0) return;

        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete user '{textBox_username.Text}'?\n\nThis action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        var result = adminInstanceManager.DeleteUser(_selectedUserID);

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
    private void CreateUser()
    {
        // Validate passwords match
        if (textBox_password.Text != textBox_password2.Text)
        {
            MessageBox.Show("Passwords do not match.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Build request
        var request = new CreateUserRequest(
            Username: textBox_username.Text.Trim(),
            Password: textBox_password.Text,
            Permissions: GetSelectedPermissions(),
            IsActive: checkBox_userActive.Checked,
            Notes: textBox_userNotes.Text.Trim()
        );

        // Call manager (cache will be auto-updated)
        var result = adminInstanceManager.CreateUser(request);

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
    private void UpdateUser()
    {
        // Build request
        var request = new UpdateUserRequest(
            UserID: _selectedUserID,
            Username: textBox_username.Text.Trim(),
            NewPassword: string.IsNullOrWhiteSpace(textBox_password.Text)
                ? null
                : textBox_password.Text,
            Permissions: GetSelectedPermissions(),
            IsActive: checkBox_userActive.Checked,
            Notes: textBox_userNotes.Text.Trim()
        );

        // Call manager (cache will be auto-updated)
        var result = adminInstanceManager.UpdateUser(request);

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