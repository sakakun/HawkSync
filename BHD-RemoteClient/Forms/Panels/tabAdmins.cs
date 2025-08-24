using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_RemoteClient.Forms.Panels
{
    public partial class tabAdmins : UserControl
    {
        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private adminInstance instanceAdmin => CommonCore.instanceAdmin;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;

        // --- Class Variables ---
        private new string Name = "AdminsTab";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.
        private List<CheckBox> _adminRoleCheckBoxes = new ();       // List to hold checkboxes for admin roles.
        private List<AdminAccount> _adminAccounts = new ();         // List to hold admin accounts.
        private DateTime _lastAdminLogRefresh = DateTime.MinValue;  // Timestamp of the last admin log refresh.
        private void function_InitializeRoleCheckBoxes()
        {
            cb_roleAdmin.Tag = (int)AdminRoles.Admin;           // 2
            cb_roleModerator.Tag = (int)AdminRoles.Moderator;   // 1
            cb_roleDisabled.Tag = (int)AdminRoles.None;         // 0

            _adminRoleCheckBoxes = new List<CheckBox>
            {
                cb_roleAdmin,
                cb_roleModerator,
                cb_roleDisabled
            };

            foreach (var checkBox in _adminRoleCheckBoxes)
            {
                checkBox.Checked = false;
            }
        }

        private void function_RefreshAdminDataGrid()
        {
            dg_AdminUsers.SuspendLayout();
            dg_AdminUsers.Rows.Clear();

            foreach (var admin in instanceAdmin.Admins)
            {
                dg_AdminUsers.Rows.Add(admin.UserId, admin.Username, admin.Role.ToString());
            }

            dg_AdminUsers.ResumeLayout();
        }
        private void function_RefreshAdminLog()
        {
            if ((DateTime.UtcNow - _lastAdminLogRefresh).TotalSeconds < 15)
                return;

            dg_adminLog.SuspendLayout();
            dg_adminLog.Rows.Clear();

            var adminDict = instanceAdmin.Admins.ToDictionary(a => a.UserId, a => a.Username);

            foreach (var log in instanceAdmin.Logs.OrderByDescending(l => l.Timestamp).Take(50))
            {
                string username = adminDict.TryGetValue(log.UserId, out var name) ? name : $"UserId:{log.UserId}";
                dg_adminLog.Rows.Add(log.Timestamp, username, log.Action);
            }

            dg_adminLog.ResumeLayout();
            _lastAdminLogRefresh = DateTime.UtcNow;
        }

        private bool AdminListsAreEqual(List<AdminAccount> a, List<AdminAccount> b)
        {
            var props = typeof(AdminAccount).GetProperties();

            // Sort both lists by UserId for consistent order
            var aSorted = a.OrderBy(x => x.UserId).ToList();
            var bSorted = b.OrderBy(x => x.UserId).ToList();

            for (int i = 0; i < aSorted.Count; i++)
            {
                foreach (var prop in props)
                {
                    var aVal = prop.GetValue(aSorted[i]);
                    var bVal = prop.GetValue(bSorted[i]);
                    if (aVal == null && bVal == null) continue;
                    if (aVal == null || bVal == null) return false;
                    if (!aVal.Equals(bVal)) return false;
                }
            }
            return true;
        }

        private void functionEvent_SetRoleCheckbox(AdminRoles selectedRole)
        {
            cb_roleAdmin.Checked = selectedRole == AdminRoles.Admin;
            cb_roleModerator.Checked = selectedRole == AdminRoles.Moderator;
            cb_roleDisabled.Checked = selectedRole == AdminRoles.None;
        }


        public tabAdmins()
        {
            // Constructor for the Admins tab, initializes components and sets up the UI.
            InitializeComponent();
            // Set the initial state of the UI components.
            ActionClick_AdminNewUser(null!, null!);
            function_InitializeRoleCheckBoxes();
        }

        // Usage in AdminsTickerHook
        public void AdminsTickerHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                _adminAccounts = instanceAdmin.Admins.ToList();
                function_RefreshAdminDataGrid();
            }

            if (!AdminListsAreEqual(_adminAccounts, instanceAdmin.Admins))
            {
                _adminAccounts = instanceAdmin.Admins.ToList();
                function_RefreshAdminDataGrid();
            }

            function_RefreshAdminLog();
        }

        private void ActionClick_AdminNewUser(object sender, EventArgs e)
        {
            btn_adminAdd.Enabled = true;
            btn_adminSave.Enabled = false;
            btn_adminDelete.Enabled = false;
            tb_adminUser.Text = string.Empty;
            tb_adminPass.Text = string.Empty;
            tb_adminPass.PlaceholderText = "Password";
            function_InitializeRoleCheckBoxes();

        }

        private void ActionClick_AdminAddNew(object sender, EventArgs e)
        {
            // Return if no checkbox is selected, from _adminRoleCheckBoxes list.
            if (_adminRoleCheckBoxes.All(cb => !cb.Checked))
            {
                MessageBox.Show("Please select a role for the new admin account.", "No Role Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Get the selected role from the checkboxes.
            AdminRoles selectedRole = _adminRoleCheckBoxes.Find(cb => cb.Checked)?.Tag is int roleIndex
                ? (AdminRoles)roleIndex
                : AdminRoles.None;

            if (instanceAdmin.Admins.Any(a => a.Username.Equals(tb_adminUser.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("An admin account with this username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (adminInstanceManager.addAdminAccount(tb_adminUser.Text, tb_adminPass.Text, selectedRole))
            {
                MessageBox.Show("Admin account has been added successfully.", "Admin Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActionClick_AdminNewUser(sender, e);
            }
            else
            {
                MessageBox.Show("Failed to add admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            function_InitializeRoleCheckBoxes();

        }

        private void ActionClick_AdminEditUser(object sender, EventArgs e)
        {
            // Get the user ID from the currently selected row in the DataGridView.
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[dg_AdminUsers.CurrentCell.RowIndex].Cells[0].Value);

            // Get the selected role from the checkboxes.
            AdminRoles selectedRole = _adminRoleCheckBoxes.Find(cb => cb.Checked)?.Tag is int roleIndex
                ? (AdminRoles)roleIndex
                : AdminRoles.None;

            // Validate the username
            if (instanceAdmin.Admins.Any(a => a.Username.Equals(tb_adminUser.Text, StringComparison.OrdinalIgnoreCase) && a.UserId != userID))
            {
                MessageBox.Show("An admin account with this username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  Validate the password
            string? password = string.IsNullOrWhiteSpace(tb_adminPass.Text) ? null : tb_adminPass.Text;

            if (adminInstanceManager.updateAdminAccount(userID, tb_adminUser.Text, password, selectedRole))
            {
                MessageBox.Show("Admin account has been updated successfully.", "Admin Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActionClick_AdminNewUser(sender, e);

                // Immediately update the local admin list and refresh the grid
                _adminAccounts = instanceAdmin.Admins.ToList();
                function_RefreshAdminDataGrid();
            }
            else
            {
                MessageBox.Show("Failed to update admin account. Please check the user details and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            function_InitializeRoleCheckBoxes();
        }

        private void ActionClick_SelectAdmin(object sender, DataGridViewCellEventArgs e)
        {
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[e.RowIndex].Cells[0].Value);
            tb_adminUser.Text = instanceAdmin.Admins.Where(a => a.UserId == userID).Select(a => a.Username).FirstOrDefault() ?? string.Empty;
            tb_adminPass.Text = string.Empty;
            tb_adminPass.PlaceholderText = "Leave Empty to Keep Current";

            // Set the role checkboxes based on the selected admin account.
            foreach (var checkBox in _adminRoleCheckBoxes)
            {
                if (checkBox.Tag is int roleIndex &&
                    instanceAdmin.Admins.Any(a => a.UserId == userID && (int)a.Role == roleIndex))
                {
                    checkBox.Checked = true; // Check the checkbox that matches the admin's role.
                }
                else
                {
                    checkBox.Checked = false; // Uncheck if the role does not match.
                }
            }

            btn_adminNew.Enabled = true;
            btn_adminAdd.Enabled = false;
            btn_adminSave.Enabled = true;
            btn_adminDelete.Enabled = true;
        }

        private void ActionClick_AdminDelete(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(dg_AdminUsers.Rows[dg_AdminUsers.CurrentCell.RowIndex].Cells[0].Value);
            var confirmResult = MessageBox.Show("Are you sure you want to delete this admin account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                if (adminInstanceManager.removeAdminAccount(userID))
                {
                    MessageBox.Show("Admin account has been deleted successfully.", "Admin Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActionClick_AdminNewUser(sender, e);
                }
                else
                {
                    MessageBox.Show("Failed to delete admin account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            function_InitializeRoleCheckBoxes();

        }

        private void actionClick_roleAdmin(object sender, EventArgs e)
        {
            functionEvent_SetRoleCheckbox(AdminRoles.Admin);
        }
        private void actionClick_roleModerator(object sender, EventArgs e)
        {
            functionEvent_SetRoleCheckbox(AdminRoles.Moderator);
        }
        private void actionClick_roleDisabled(object sender, EventArgs e)
        {
            functionEvent_SetRoleCheckbox(AdminRoles.None);
        }

    }
}
