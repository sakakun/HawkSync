using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Tickers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabBans : UserControl
    {
        // --- Instance Objects ---
        private banInstance? instanceBans => CommonCore.instanceBans;

        public tabBans()
        {
            InitializeComponent();
            
            // Load Data into DataGridViews
            LoadBlacklistGrids();
            LoadWhitelistGrids();

            // Initialize Form Controls
            BlacklistForm_Initialize();
            WhitelistForm_Initialize();

            // Wire up tab change event
            banControls.SelectedIndexChanged += BanControls_SelectedIndexChanged;
        }

        /// <summary>
        /// Handle tab control selection changes to refresh data when switching tabs
        /// </summary>
        private void BanControls_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (banControls.SelectedTab == tabBlacklist)
            {
                // Refresh blacklist data when blacklist tab is selected
                Blacklist_Refresh_Click(sender!, e);
            }
            else if (banControls.SelectedTab == tabWhitelist)
            {
                // Refresh whitelist data when whitelist tab is selected
                Whitelist_Refresh_Click(sender!, e);
            }
        }

        // ================================================================================
        // BLACKLIST FUNCTIONALITY
        // ================================================================================
        private int  _blacklistSelectedRecordIDName = -1;
        private int  _blacklistSelectedRecordIDIP   = -1;

        /// <summary>
        /// Load blacklist data from instanceBans into DataGridViews
        /// </summary>
        private void LoadBlacklistGrids()
        {
            if (instanceBans == null)
                return;

            // Clear existing rows
            dgPlayerNamesBlacklist.Rows.Clear();
            dgPlayerAddressBlacklist.Rows.Clear();

            // Load player name bans
            foreach (var record in instanceBans.BannedPlayerNames)
            {
                dgPlayerNamesBlacklist.Rows.Add(
                    record.RecordID,
                    record.PlayerName,
                    record.Date.ToString("yyyy-MM-dd")
                );
            }

            // Load player IP bans
            foreach (var record in instanceBans.BannedPlayerIPs)
            {
                dgPlayerAddressBlacklist.Rows.Add(
                    record.RecordID,
                    $"{record.PlayerIP}/{record.SubnetMask}",
                    record.Date.ToString("yyyy-MM-dd")
                );
            }

            AppDebug.Log("tabBans", 
                $"Loaded {instanceBans.BannedPlayerNames.Count} name bans and " +
                $"{instanceBans.BannedPlayerIPs.Count} IP bans");
        }

        /// <summary>
        /// Reload blacklist data from database
        /// </summary>
        public void Blacklist_Refresh_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Reload from database
            instanceBans.BannedPlayerNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Ban);
            instanceBans.BannedPlayerIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Ban);

            // Refresh DataGridViews
            LoadBlacklistGrids();
        }

        /// <summary>
        /// Initialize blacklist form to default hidden state with cleared fields.
        /// </summary>
        private void BlacklistForm_Initialize()
        {
            // The Form
            blacklistForm.Visible = false;
            // Player Name
            blacklist_PlayerName.Visible = false;
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddress.Visible = false;
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.SelectedText = "32";
            // Ban Dates
            blacklist_DateEnd.Enabled = false;
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;
            // Control Buttons
            blacklist_btnClose.Visible = false;
            blacklist_btnDelete.Visible = false;
            blacklist_btnSave.Visible = false;
            blacklist_btnReset.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new player name ban only.
        /// </summary>
        private void Blacklist_AddNameOnly_Click(object sender, EventArgs e)
        {
            ShowBlacklistForm(showPlayerName: true, showIPAddress: false);
            blacklist_btnDelete.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new IP address ban only.
        /// </summary>
        private void Blacklist_AddIPOnly_Click(object sender, EventArgs e)
        {
            ShowBlacklistForm(showPlayerName: false, showIPAddress: true);
            blacklist_btnDelete.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new ban with both player name and IP address.
        /// </summary>
        private void Blacklist_AddBoth_Click(object sender, EventArgs e)
        {
            ShowBlacklistForm(showPlayerName: true, showIPAddress: true);
            blacklist_btnDelete.Visible = false;
        }

        /// <summary>
        /// Display and configure the blacklist form for creating a new ban record.
        /// </summary>
        /// <param name="showPlayerName">Show player name input field</param>
        /// <param name="showIPAddress">Show IP address input fields</param>
        private void ShowBlacklistForm(bool showPlayerName, bool showIPAddress)
        {
            // Reset selection IDs for new record
            _blacklistSelectedRecordIDName = -1;
            _blacklistSelectedRecordIDIP = -1;
            // Show the Form & Reset Fields
            blacklistForm.Visible = true;
            // Player Name
            blacklist_PlayerName.Visible = showPlayerName;
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddress.Visible = showIPAddress;
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.Text = "32";
            // Ban Dates
            blacklist_DateStart.MinDate = DateTime.Today.AddYears(-1);
            blacklist_DateStart.MaxDate = DateTime.Today.AddYears(1);
            blacklist_DateStart.Value = DateTime.Now;
            blacklist_DateEnd.MinDate = DateTime.Today.AddYears(-1);
            blacklist_DateEnd.MaxDate = DateTime.Today.AddYears(1);
            blacklist_DateEnd.Value = DateTime.Now;
            blacklist_DateEnd.Enabled = false;
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;
            // Control Buttons
            blacklist_btnClose.Visible = true;
            blacklist_btnDelete.Visible = true;
            blacklist_btnSave.Visible = true;
            blacklist_btnReset.Visible = true;
        }

        /// <summary>
        /// Handle click event to reset all blacklist form fields to default values.
        /// </summary>
        private void Blacklist_Reset_Click(object sender, EventArgs e)
        {
            // Player Name
            blacklist_PlayerNameTxt.Text = String.Empty;
            // IP Address
            blacklist_IPAddressTxt.Text = String.Empty;
            blacklist_IPSubnetTxt.Text = "32";
            // Ban Dates
            blacklist_DateStart.MinDate = DateTime.Today.AddYears(-1);
            blacklist_DateStart.MaxDate = DateTime.Today.AddYears(1);
            blacklist_DateStart.Value = DateTime.Now;
            blacklist_DateEnd.MinDate = DateTime.Today.AddYears(-1);
            blacklist_DateEnd.MaxDate = DateTime.Today.AddYears(1);
            blacklist_DateEnd.Value = DateTime.Now;
            blacklist_DateEnd.Enabled = false;
            // Ban Type
            blacklist_TempBan.Checked = false;
            blacklist_PermBan.Checked = true;
            // Notes
            blacklist_notes.Text = String.Empty;
        }

        /// <summary>
        /// Handle click event to save the current blacklist record (create new or update existing).
        /// Creates bidirectional associations when both name and IP records are added together.
        /// </summary>  
        private void Blacklist_Save_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Determine ban type
            var recordType = blacklist_PermBan.Checked 
                ? banInstanceRecordType.Permanent 
                : banInstanceRecordType.Temporary;

            // Temp Ban Expiration Date
            DateTime? expireDate = recordType == banInstanceRecordType.Temporary 
                ? blacklist_DateEnd.Value 
                : null;

            // Validate temporary ban has future end date
            if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
            {
                if (expireDate.Value <= DateTime.Now)
                {
                    MessageBox.Show("Temporary ban end date must be greater than the current date and time.", 
                        "Validation Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            DateTime banDate = blacklist_DateStart.Value;
            string notes = blacklist_notes.Text.Trim();

            int nameRecordID = _blacklistSelectedRecordIDName;
            int ipRecordID = _blacklistSelectedRecordIDIP;

            // Track if we need to update associations after both records are created
            banInstancePlayerName? createdNameRecord = null;
            banInstancePlayerIP? createdIPRecord = null;
            bool needsAssociationUpdate = false;

            try
            {
                // Save Player Name Ban (if visible)
                if (blacklist_PlayerName.Visible && !string.IsNullOrWhiteSpace(blacklist_PlayerNameTxt.Text))
                {
                    string playerName = blacklist_PlayerNameTxt.Text.Trim();
    
                    if (_blacklistSelectedRecordIDName == -1)
                    {
                        // Create new record
                        var nameRecord = new banInstancePlayerName
                        {
                            RecordID = 0, // Will be assigned by database
                            MatchID = 0,
                            PlayerName = playerName,
                            Date = banDate,
                            ExpireDate = expireDate,
                            AssociatedIP = ipRecordID > 0 ? ipRecordID : 0,
                            RecordType = recordType,
                            RecordCategory = (int)RecordCategory.Ban,
                            Notes = notes
                        };

                        // Add to database
                        nameRecordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                        nameRecord.RecordID = nameRecordID;

                        // Add to in-memory list
                        instanceBans.BannedPlayerNames.Add(nameRecord);
        
                        // Add to DataGridView
                        dgPlayerNamesBlacklist.Rows.Add(
                            nameRecord.RecordID,
                            nameRecord.PlayerName,
                            nameRecord.Date.ToString("yyyy-MM-dd")
                        );

                        // Track for potential association update
                        createdNameRecord = nameRecord;
                        if (blacklist_IPAddress.Visible && _blacklistSelectedRecordIDIP == -1)
                        {
                            needsAssociationUpdate = true;
                        }
                    }
                    else
                    {
                        // Update existing record
                        var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDName);
                        if (nameRecord != null)
                        {
                            nameRecord.PlayerName = playerName;
                            nameRecord.Date = banDate;
                            nameRecord.ExpireDate = expireDate;
                            nameRecord.RecordType = recordType;
                            nameRecord.Notes = notes;
                            nameRecord.AssociatedIP = ipRecordID > 0 ? ipRecordID : nameRecord.AssociatedIP;

                            // Update database
                            DatabaseManager.UpdatePlayerNameRecord(nameRecord);

                            // Update DataGridView
                            var row = dgPlayerNamesBlacklist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value == _blacklistSelectedRecordIDName);
                            if (row != null)
                            {
                                row.Cells[1].Value = nameRecord.PlayerName;
                                row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                // Save IP Address Ban (if visible)
                if (blacklist_IPAddress.Visible && !string.IsNullOrWhiteSpace(blacklist_IPAddressTxt.Text))
                {
                    if (IPAddress.TryParse(blacklist_IPAddressTxt.Text.Trim(), out IPAddress? ipAddress))
                    {
                        int subnetMask = int.TryParse(blacklist_IPSubnetTxt.Text, out int subnet) 
                            ? subnet 
                            : 32;

                        if (_blacklistSelectedRecordIDIP == -1)
                        {
                            // Create new record
                            var ipRecord = new banInstancePlayerIP
                            {
                                RecordID = 0, // Will be assigned by database
                                MatchID = 0,
                                PlayerIP = ipAddress,
                                SubnetMask = subnetMask,
                                Date = banDate,
                                ExpireDate = expireDate,
                                AssociatedName = nameRecordID > 0 ? nameRecordID : 0,
                                RecordType = recordType,
                                RecordCategory = (int)RecordCategory.Ban,
                                Notes = notes
                            };

                            // Add to database
                            ipRecordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                            ipRecord.RecordID = ipRecordID;

                            // Add to in-memory list
                            instanceBans.BannedPlayerIPs.Add(ipRecord);

                            // Add to DataGridView
                            dgPlayerAddressBlacklist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );

                            // Track for potential association update
                            createdIPRecord = ipRecord;
                        }
                        else
                        {
                            // Update existing record
                            var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDIP);
                            if (ipRecord != null)
                            {
                                ipRecord.PlayerIP = ipAddress;
                                ipRecord.SubnetMask = subnetMask;
                                ipRecord.Date = banDate;
                                ipRecord.ExpireDate = expireDate;
                                ipRecord.RecordType = recordType;
                                ipRecord.Notes = notes;
                                ipRecord.AssociatedName = nameRecordID > 0 ? nameRecordID : ipRecord.AssociatedName;

                                // Update database
                                DatabaseManager.UpdatePlayerIPRecord(ipRecord);

                                // Update DataGridView
                                var row = dgPlayerAddressBlacklist.Rows.Cast<DataGridViewRow>()
                                    .FirstOrDefault(r => (int)r.Cells[0].Value == _blacklistSelectedRecordIDIP);
                                if (row != null)
                                {
                                    row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                    row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid IP Address format.", "Validation Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Update bidirectional association if both records were created together
                if (needsAssociationUpdate && createdNameRecord != null && createdIPRecord != null)
                {
                    createdNameRecord.AssociatedIP = createdIPRecord.RecordID;
                    DatabaseManager.UpdatePlayerNameRecord(createdNameRecord);
                    AppDebug.Log("tabBans", $"Updated bidirectional association: Name {createdNameRecord.RecordID} <-> IP {createdIPRecord.RecordID}");
                }

                // Reset selection IDs and form
                _blacklistSelectedRecordIDName = -1;
                _blacklistSelectedRecordIDIP = -1;
                Blacklist_Reset_Click(sender, e);
                blacklistForm.Visible = false;

                MessageBox.Show("Ban record saved successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving ban record: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("tabBans", $"Error saving ban: {ex}");
            }
        }

        /// <summary>
        /// Handle click event to toggle between temporary and permanent ban types.
        /// Enables/disables the expiration date picker based on selection.
        /// </summary>
        private void Blacklist_BanTypeToggle_Click(object sender, EventArgs e)
        {
            // Toggle between Temp and Perm on click
            CheckBox? checkBox = sender as CheckBox;
            if (checkBox?.Name == "blacklist_TempBan")
            {
                blacklist_PermBan.Checked = !blacklist_TempBan.Checked;
            }
            else
            {
                blacklist_TempBan.Checked = !blacklist_PermBan.Checked;
            }

            blacklist_DateEnd.Enabled = blacklist_TempBan.Checked;
        }

        /// <summary>
        /// Handle click event to close the blacklist form and reset all fields.
        /// Clears selected record IDs and hides control buttons.
        /// </summary>
        private void Blacklist_Close_Click(object sender, EventArgs e)
        {
            // Reset selection IDs
            _blacklistSelectedRecordIDName = -1;
            _blacklistSelectedRecordIDIP = -1;
            // Reset and Hide
            Blacklist_Reset_Click(sender, e);
            
            // Control Buttons
            blacklist_btnClose.Visible = false;
            blacklist_btnSave.Visible = false;
            blacklist_btnDelete.Visible = false;
            blacklist_btnReset.Visible = false;
            
            blacklistForm.Visible = false;

        }

        /// <summary>
        /// Handle delete button click for blacklist records
        /// </summary>
        private void Blacklist_Delete_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Check what records are selected
            bool hasNameRecord = _blacklistSelectedRecordIDName != -1;
            bool hasIPRecord = _blacklistSelectedRecordIDIP != -1;

            if (!hasNameRecord && !hasIPRecord)
            {
                MessageBox.Show("No record selected for deletion.", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the records
            banInstancePlayerName? nameRecord = hasNameRecord 
                ? instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDName)
                : null;
    
            banInstancePlayerIP? ipRecord = hasIPRecord 
                ? instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDIP)
                : null;

            // Check for associations
            int associatedIPID = nameRecord?.AssociatedIP ?? 0;
            int associatedNameID = ipRecord?.AssociatedName ?? 0;

            bool hasAssociation = (associatedIPID > 0 && hasNameRecord) || 
                                 (associatedNameID > 0 && hasIPRecord);

            RecordDeleteAction deleteAction;

            if (hasAssociation)
            {
                // Show dialog with options
                deleteAction = Blacklist_ShowDeleteConfirmationDialog(hasNameRecord, hasIPRecord, 
                    nameRecord?.PlayerName ?? "", 
                    ipRecord != null ? $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}" : "");
            }
            else
            {
                // Simple confirmation
                string recordType = hasNameRecord ? "player name" : "IP address";
                string recordValue = hasNameRecord ? nameRecord?.PlayerName ?? "" : $"{ipRecord?.PlayerIP}/{ipRecord?.SubnetMask}";
        
                var result = MessageBox.Show(
                    $"Are you sure you want to delete this {recordType} ban?\n\n{recordValue}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                deleteAction = hasNameRecord ? RecordDeleteAction.NameOnly : RecordDeleteAction.IPOnly;
            }

            if (deleteAction == RecordDeleteAction.None)
                return;

            try
            {
                // Perform deletion based on action
                switch (deleteAction)
                {
                    case RecordDeleteAction.Both:
                        Blacklist_DeleteBothRecords(nameRecord!, ipRecord!);
                        break;

                    case RecordDeleteAction.NameOnly:
                        Blacklist_DeleteNameRecord(nameRecord!, associatedIPID);
                        break;

                    case RecordDeleteAction.IPOnly:
                        Blacklist_DeleteIPRecord(ipRecord!, associatedNameID);
                        break;
                }

                // Reset form
                _blacklistSelectedRecordIDName = -1;
                _blacklistSelectedRecordIDIP = -1;
                Blacklist_Reset_Click(sender, e);
                blacklistForm.Visible = false;

                // Refresh the data
                Blacklist_Refresh_Click(sender, e);

                MessageBox.Show("Record(s) deleted successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("tabBans", $"Error deleting ban: {ex}");
            }
        }

        /// <summary>
        /// Delete both name and IP records
        /// </summary>
        private void Blacklist_DeleteBothRecords(banInstancePlayerName nameRecord, banInstancePlayerIP ipRecord)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerNameRecord(nameRecord.RecordID);
            DatabaseManager.RemovePlayerIPRecord(ipRecord.RecordID);

            // Remove from in-memory lists
            instanceBans.BannedPlayerNames.Remove(nameRecord);
            instanceBans.BannedPlayerIPs.Remove(ipRecord);

            AppDebug.Log("tabBans", $"Deleted both records: Name ID {nameRecord.RecordID}, IP ID {ipRecord.RecordID}");
        }

        /// <summary>
        /// Delete name record only, update associated IP if exists
        /// </summary>
        private void Blacklist_DeleteNameRecord(banInstancePlayerName nameRecord, int associatedIPID)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerNameRecord(nameRecord.RecordID);

            // Remove from in-memory list
            instanceBans.BannedPlayerNames.Remove(nameRecord);

            // Update associated IP record to clear the association
            if (associatedIPID > 0)
            {
                var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == associatedIPID);
                if (ipRecord != null)
                {
                    ipRecord.AssociatedName = 0;
                    DatabaseManager.UpdatePlayerIPRecord(ipRecord);
                    AppDebug.Log("tabBans", $"Cleared association on IP record {ipRecord.RecordID}");
                }
            }

            AppDebug.Log("tabBans", $"Deleted name record ID {nameRecord.RecordID}");
        }

        /// <summary>
        /// Delete IP record only, update associated name if exists
        /// </summary>
        private void Blacklist_DeleteIPRecord(banInstancePlayerIP ipRecord, int associatedNameID)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerIPRecord(ipRecord.RecordID);

            // Remove from in-memory list
            instanceBans.BannedPlayerIPs.Remove(ipRecord);

            // Update associated name record to clear the association
            if (associatedNameID > 0)
            {
                var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == associatedNameID);
                if (nameRecord != null)
                {
                    nameRecord.AssociatedIP = 0;
                    DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    AppDebug.Log("tabBans", $"Cleared association on name record {nameRecord.RecordID}");
                }
            }

            AppDebug.Log("tabBans", $"Deleted IP record ID {ipRecord.RecordID}");
        }

        /// <summary>
        /// Show dialog to confirm deletion when records are associated
        /// </summary>
        private RecordDeleteAction Blacklist_ShowDeleteConfirmationDialog(bool hasName, bool hasIP, string playerName, string ipAddress)
        {
            string message = "This record has an associated ";
    
            if (hasName && hasIP)
            {
                message = $"These records are linked:\n\nPlayer Name: {playerName}\nIP Address: {ipAddress}\n\nWhat would you like to delete?";
            }
            else if (hasName)
            {
                message = $"This player name ban has an associated IP address ban:\n\nPlayer Name: {playerName}\nAssociated IP: {ipAddress}\n\nWhat would you like to delete?";
            }
            else
            {
                message = $"This IP address ban has an associated player name ban:\n\nIP Address: {ipAddress}\nAssociated Name: {playerName}\n\nWhat would you like to delete?";
            }

            using (var dialog = new Form())
            {
                dialog.Text = "Confirm Delete - Associated Records";
                dialog.Size = new Size(375, 270);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                var label = new Label
                {
                    Text = message,
                    Location = new Point(20, 20),
                    Size = new Size(400, 95),
                    AutoSize = false
                };

                var btnBoth = new Button
                {
                    Text = "Delete Both",
                    Location = new Point(20, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Yes
                };

                var btnNameOnly = new Button
                {
                    Text = "Name Only",
                    Location = new Point(130, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Retry,
                    Enabled = hasName
                };

                var btnIPOnly = new Button
                {
                    Text = "IP Only",
                    Location = new Point(240, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Ignore,
                    Enabled = hasIP
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(20, 175),
                    Size = new Size(320, 30),
                    DialogResult = DialogResult.Cancel
                };

                dialog.Controls.Add(label);
                dialog.Controls.Add(btnBoth);
                dialog.Controls.Add(btnNameOnly);
                dialog.Controls.Add(btnIPOnly);
                dialog.Controls.Add(btnCancel);

                dialog.AcceptButton = btnCancel;
                dialog.CancelButton = btnCancel;

                var result = dialog.ShowDialog();

                return result switch
                {
                    DialogResult.Yes => RecordDeleteAction.Both,
                    DialogResult.Retry => RecordDeleteAction.NameOnly,
                    DialogResult.Ignore => RecordDeleteAction.IPOnly,
                    _ => RecordDeleteAction.None
                };
            }
        }

        /// <summary>
        /// Enum for delete action choices
        /// </summary>
        private enum RecordDeleteAction
        {
            None,
            Both,
            NameOnly,
            IPOnly
        }

        /// <summary>
        /// Handle double-click on player name blacklist to edit
        /// </summary>
        private void Blacklist_NameGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
                return;

            var recordID = (int)dgPlayerNamesBlacklist.Rows[e.RowIndex].Cells[0].Value;
            var record = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadBlacklistRecordForEditing(record, null);
        }

        /// <summary>
        /// Handle double-click on IP address blacklist to edit
        /// </summary>
        private void Blacklist_IPGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
                return;

            var recordID = (int)dgPlayerAddressBlacklist.Rows[e.RowIndex].Cells[0].Value;
            var record = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadBlacklistRecordForEditing(null, record);
        }

        /// <summary>
        /// Load a record into the form for editing
        /// </summary>
        private void LoadBlacklistRecordForEditing(banInstancePlayerName? nameRecord, banInstancePlayerIP? ipRecord)
        {
            if (instanceBans == null)
                return;

            // Set selected record IDs
            _blacklistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            _blacklistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;

            // Check for associations and load both if they exist
            if (nameRecord != null && nameRecord.AssociatedIP > 0)
            {
                ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == nameRecord.AssociatedIP);
                _blacklistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;
            }
            else if (ipRecord != null && ipRecord.AssociatedName > 0)
            {
                nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == ipRecord.AssociatedName);
                _blacklistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            }

            // Show the form
            blacklistForm.Visible = true;

            // Load player name data
            if (nameRecord != null)
            {
                blacklist_PlayerName.Visible = true;
                blacklist_PlayerNameTxt.Text = nameRecord.PlayerName;
            }
            else
            {
                blacklist_PlayerName.Visible = false;
                blacklist_PlayerNameTxt.Text = string.Empty;
            }

            // Load IP address data
            if (ipRecord != null)
            {
                blacklist_IPAddress.Visible = true;
                blacklist_IPAddressTxt.Text = ipRecord.PlayerIP.ToString();
                blacklist_IPSubnetTxt.Text = ipRecord.SubnetMask.ToString();
            }
            else
            {
                blacklist_IPAddress.Visible = false;
                blacklist_IPAddressTxt.Text = string.Empty;
                blacklist_IPSubnetTxt.Text = "32";
            }

            // Load common data (use nameRecord first, fallback to ipRecord)
            var dataRecord = nameRecord ?? (object?)ipRecord;
    
            if (nameRecord != null)
            {
                blacklist_DateStart.MinDate = DateTime.Today.AddYears(-1);
                blacklist_DateStart.MaxDate = DateTime.Today.AddYears(1);
                blacklist_DateStart.Value = nameRecord.Date;

                if (nameRecord.RecordType == banInstanceRecordType.Temporary && nameRecord.ExpireDate.HasValue)
                {
                    blacklist_TempBan.Checked = true;
                    blacklist_PermBan.Checked = false;
                    blacklist_DateEnd.Enabled = true;
                    blacklist_DateEnd.MinDate = DateTime.Today.AddYears(-1);
                    blacklist_DateEnd.MaxDate = DateTime.Today.AddYears(1);
                    blacklist_DateEnd.Value = nameRecord.ExpireDate.Value;
                }
                else
                {
                    blacklist_TempBan.Checked = false;
                    blacklist_PermBan.Checked = true;
                    blacklist_DateEnd.Enabled = false;
                }

                blacklist_notes.Text = nameRecord.Notes;
            }
            else if (ipRecord != null)
            {
                blacklist_DateStart.MinDate = DateTime.Today.AddYears(-1);
                blacklist_DateStart.MaxDate = DateTime.Today.AddYears(1);
                blacklist_DateStart.Value = ipRecord.Date;

                if (ipRecord.RecordType == banInstanceRecordType.Temporary && ipRecord.ExpireDate.HasValue)
                {
                    blacklist_TempBan.Checked = true;
                    blacklist_PermBan.Checked = false;
                    blacklist_DateEnd.Enabled = true;
                    blacklist_DateEnd.MinDate = DateTime.Today.AddYears(-1);
                    blacklist_DateEnd.MaxDate = DateTime.Today.AddYears(1);
                    blacklist_DateEnd.Value = ipRecord.ExpireDate.Value;
                }
                else
                {
                    blacklist_TempBan.Checked = false;
                    blacklist_PermBan.Checked = true;
                    blacklist_DateEnd.Enabled = false;
                }

                blacklist_notes.Text = ipRecord.Notes;
            }

            // Show control buttons
            blacklist_btnDelete.Visible = true;
            blacklist_btnSave.Visible = true;
            blacklist_btnReset.Visible = true;
        }

        // ================================================================================
        // WHITELIST FUNCTIONALITY
        // ================================================================================

        private int _whitelistSelectedRecordIDName = -1;
        private int _whitelistSelectedRecordIDIP = -1;

        /// <summary>
        /// Load whitelist data from instanceBans into DataGridViews
        /// </summary>
        private void LoadWhitelistGrids()
        {
            if (instanceBans == null)
                return;

            // Clear existing rows
            dgPlayerNamesWhitelist.Rows.Clear();
            dgPlayerAddressWhitelist.Rows.Clear();

            // Load player name whitelists
            foreach (var record in instanceBans.WhitelistedNames)
            {
                dgPlayerNamesWhitelist.Rows.Add(
                    record.RecordID,
                    record.PlayerName,
                    record.Date.ToString("yyyy-MM-dd")
                );
            }

            // Load player IP whitelists
            foreach (var record in instanceBans.WhitelistedIPs)
            {
                dgPlayerAddressWhitelist.Rows.Add(
                    record.RecordID,
                    $"{record.PlayerIP}/{record.SubnetMask}",
                    record.Date.ToString("yyyy-MM-dd")
                );
            }

            AppDebug.Log("tabBans",
                $"Loaded {instanceBans.WhitelistedNames.Count} name whitelists and " +
                $"{instanceBans.WhitelistedIPs.Count} IP whitelists");
        }

        /// <summary>
        /// Reload whitelist data from database
        /// </summary>
        public void Whitelist_Refresh_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Reload from database
            instanceBans.WhitelistedNames = DatabaseManager.GetPlayerNameRecords(RecordCategory.Whitelist);
            instanceBans.WhitelistedIPs = DatabaseManager.GetPlayerIPRecords(RecordCategory.Whitelist);

            // Refresh DataGridViews
            LoadWhitelistGrids();
        }

        /// <summary>
        /// Initialize whitelist form to default hidden state with cleared fields.
        /// </summary>
        private void WhitelistForm_Initialize()
        {
            // The Form
            panel2.Visible = false;
            // Player Name
            groupBox10.Visible = false;
            textBox_playerNameWL.Text = String.Empty;
            // IP Address
            groupBox9.Visible = false;
            textBox_addressWL.Text = String.Empty;
            cb_subnetWL.SelectedText = "32";
            // Exempt Dates
            dateTimePicker_WLend.Enabled = false;
            // Exempt Type
            checkBox_tempWL.Checked = false;
            checkBox_permWL.Checked = true;
            // Notes
            textBox_notesWL.Text = String.Empty;
            // Control Buttons
            wlControlClose.Visible = false;
            wlControlDelete.Visible = false;
            wlControlSave.Visible = false;
            wlControlReset.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new player name whitelist only.
        /// </summary>
        private void Whitelist_AddNameOnly_Click(object sender, EventArgs e)
        {
            ShowWhitelistForm(showPlayerName: true, showIPAddress: false);
            wlControlDelete.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new IP address whitelist only.
        /// </summary>
        private void Whitelist_AddIPOnly_Click(object sender, EventArgs e)
        {
            ShowWhitelistForm(showPlayerName: false, showIPAddress: true);
            wlControlDelete.Visible = false;
        }

        /// <summary>
        /// Handle click event to add a new whitelist with both player name and IP address.
        /// </summary>
        private void Whitelist_AddBoth_Click(object sender, EventArgs e)
        {
            ShowWhitelistForm(showPlayerName: true, showIPAddress: true);
            wlControlDelete.Visible = false;
        }

        /// <summary>
        /// Display and configure the whitelist form for creating a new whitelist record.
        /// </summary>
        /// <param name="showPlayerName">Show player name input field</param>
        /// <param name="showIPAddress">Show IP address input fields</param>
        private void ShowWhitelistForm(bool showPlayerName, bool showIPAddress)
        {
            // Reset selection IDs for new record
            _whitelistSelectedRecordIDName = -1;
            _whitelistSelectedRecordIDIP = -1;
            // Show the Form & Reset Fields
            panel2.Visible = true;
            // Player Name
            groupBox10.Visible = showPlayerName;
            textBox_playerNameWL.Text = String.Empty;
            // IP Address
            groupBox9.Visible = showIPAddress;
            textBox_addressWL.Text = String.Empty;
            cb_subnetWL.Text = "32";
            // Exempt Dates
            dateTimePicker_WLstart.MinDate = DateTime.Today.AddYears(-1);
            dateTimePicker_WLstart.MaxDate = DateTime.Today.AddYears(1);
            dateTimePicker_WLstart.Value = DateTime.Now;
            dateTimePicker_WLend.MinDate = DateTime.Today.AddYears(-1);
            dateTimePicker_WLend.MaxDate = DateTime.Today.AddYears(1);
            dateTimePicker_WLend.Value = DateTime.Now;
            dateTimePicker_WLend.Enabled = false;
            // Exempt Type
            checkBox_tempWL.Checked = false;
            checkBox_permWL.Checked = true;
            // Notes
            textBox_notesWL.Text = String.Empty;
            // Control Buttons
            wlControlClose.Visible = true;
            wlControlDelete.Visible = true;
            wlControlSave.Visible = true;
            wlControlReset.Visible = true;
        }

        /// <summary>
        /// Handle click event to reset all whitelist form fields to default values.
        /// </summary>
        private void Whitelist_Reset_Click(object sender, EventArgs e)
        {
            // Player Name
            textBox_playerNameWL.Text = String.Empty;
            // IP Address
            textBox_addressWL.Text = String.Empty;
            cb_subnetWL.Text = "32";
            // Exempt Dates
            dateTimePicker_WLstart.MinDate = DateTime.Today.AddYears(-1);
            dateTimePicker_WLstart.MaxDate = DateTime.Today.AddYears(1);
            dateTimePicker_WLstart.Value = DateTime.Now;
            dateTimePicker_WLend.MinDate = DateTime.Today.AddYears(-1);
            dateTimePicker_WLend.MaxDate = DateTime.Today.AddYears(1);
            dateTimePicker_WLend.Value = DateTime.Now;
            dateTimePicker_WLend.Enabled = false;
            // Exempt Type
            checkBox_tempWL.Checked = false;
            checkBox_permWL.Checked = true;
            // Notes
            textBox_notesWL.Text = String.Empty;
        }

        /// <summary>
        /// Handle click event to save the current whitelist record (create new or update existing).
        /// Creates bidirectional associations when both name and IP records are added together.
        /// </summary>
        private void Whitelist_Save_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Determine exempt type
            var recordType = checkBox_permWL.Checked
                ? banInstanceRecordType.Permanent
                : banInstanceRecordType.Temporary;

            // Temp Exempt Expiration Date
            DateTime? expireDate = recordType == banInstanceRecordType.Temporary
                ? dateTimePicker_WLend.Value
                : null;

            // Validate temporary whitelist has future end date
            if (recordType == banInstanceRecordType.Temporary && expireDate.HasValue)
            {
                if (expireDate.Value <= DateTime.Now)
                {
                    MessageBox.Show("Temporary whitelist end date must be greater than the current date and time.", 
                        "Validation Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            DateTime exemptDate = dateTimePicker_WLstart.Value;
            string notes = textBox_notesWL.Text.Trim();

            int nameRecordID = _whitelistSelectedRecordIDName;
            int ipRecordID = _whitelistSelectedRecordIDIP;

            // Track if we need to update associations after both records are created
            banInstancePlayerName? createdNameRecord = null;
            banInstancePlayerIP? createdIPRecord = null;
            bool needsAssociationUpdate = false;

            try
            {
                // Save Player Name Whitelist (if visible)
                if (groupBox10.Visible && !string.IsNullOrWhiteSpace(textBox_playerNameWL.Text))
                {
                    string playerName = textBox_playerNameWL.Text.Trim();

                    if (_whitelistSelectedRecordIDName == -1)
                    {
                        // Create new record
                        var nameRecord = new banInstancePlayerName
                        {
                            RecordID = 0, // Will be assigned by database
                            MatchID = 0,
                            PlayerName = playerName,
                            Date = exemptDate,
                            ExpireDate = expireDate,
                            AssociatedIP = ipRecordID > 0 ? ipRecordID : 0,
                            RecordType = recordType,
                            RecordCategory = (int)RecordCategory.Whitelist,
                            Notes = notes
                        };

                        // Add to database
                        nameRecordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                        nameRecord.RecordID = nameRecordID;

                        // Add to in-memory list
                        instanceBans.WhitelistedNames.Add(nameRecord);

                        // Add to DataGridView
                        dgPlayerNamesWhitelist.Rows.Add(
                            nameRecord.RecordID,
                            nameRecord.PlayerName,
                            nameRecord.Date.ToString("yyyy-MM-dd")
                        );

                        // Track for potential association update
                        createdNameRecord = nameRecord;
                        if (groupBox9.Visible && _whitelistSelectedRecordIDIP == -1)
                        {
                            needsAssociationUpdate = true;
                        }
                    }
                    else
                    {
                        // Update existing record
                        var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDName);
                        if (nameRecord != null)
                        {
                            nameRecord.PlayerName = playerName;
                            nameRecord.Date = exemptDate;
                            nameRecord.ExpireDate = expireDate;
                            nameRecord.RecordType = recordType;
                            nameRecord.Notes = notes;
                            nameRecord.AssociatedIP = ipRecordID > 0 ? ipRecordID : nameRecord.AssociatedIP;

                            // Update database
                            DatabaseManager.UpdatePlayerNameRecord(nameRecord);

                            // Update DataGridView
                            var row = dgPlayerNamesWhitelist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value == _whitelistSelectedRecordIDName);
                            if (row != null)
                            {
                                row.Cells[1].Value = nameRecord.PlayerName;
                                row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                // Save IP Address Whitelist (if visible)
                if (groupBox9.Visible && !string.IsNullOrWhiteSpace(textBox_addressWL.Text))
                {
                    if (IPAddress.TryParse(textBox_addressWL.Text.Trim(), out IPAddress? ipAddress))
                    {
                        int subnetMask = int.TryParse(cb_subnetWL.Text, out int subnet)
                            ? subnet
                            : 32;

                        if (_whitelistSelectedRecordIDIP == -1)
                        {
                            // Create new record
                            var ipRecord = new banInstancePlayerIP
                            {
                                RecordID = 0, // Will be assigned by database
                                MatchID = 0,
                                PlayerIP = ipAddress,
                                SubnetMask = subnetMask,
                                Date = exemptDate,
                                ExpireDate = expireDate,
                                AssociatedName = nameRecordID > 0 ? nameRecordID : 0,
                                RecordType = recordType,
                                RecordCategory = (int)RecordCategory.Whitelist,
                                Notes = notes
                            };

                            // Add to database
                            ipRecordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                            ipRecord.RecordID = ipRecordID;

                            // Add to in-memory list
                            instanceBans.WhitelistedIPs.Add(ipRecord);

                            // Add to DataGridView
                            dgPlayerAddressWhitelist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );

                            // Track for potential association update
                            createdIPRecord = ipRecord;
                        }
                        else
                        {
                            // Update existing record
                            var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDIP);
                            if (ipRecord != null)
                            {
                                ipRecord.PlayerIP = ipAddress;
                                ipRecord.SubnetMask = subnetMask;
                                ipRecord.Date = exemptDate;
                                ipRecord.ExpireDate = expireDate;
                                ipRecord.RecordType = recordType;
                                ipRecord.Notes = notes;
                                ipRecord.AssociatedName = nameRecordID > 0 ? nameRecordID : ipRecord.AssociatedName;

                                // Update database
                                DatabaseManager.UpdatePlayerIPRecord(ipRecord);

                                // Update DataGridView
                                var row = dgPlayerAddressWhitelist.Rows.Cast<DataGridViewRow>()
                                    .FirstOrDefault(r => (int)r.Cells[0].Value == _whitelistSelectedRecordIDIP);
                                if (row != null)
                                {
                                    row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                    row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid IP Address format.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Update bidirectional association if both records were created together
                if (needsAssociationUpdate && createdNameRecord != null && createdIPRecord != null)
                {
                    createdNameRecord.AssociatedIP = createdIPRecord.RecordID;
                    DatabaseManager.UpdatePlayerNameRecord(createdNameRecord);
                    AppDebug.Log("tabBans", $"Updated bidirectional association: Name {createdNameRecord.RecordID} <-> IP {createdIPRecord.RecordID}");
                }

                // Reset selection IDs and form
                _whitelistSelectedRecordIDName = -1;
                _whitelistSelectedRecordIDIP = -1;
                Whitelist_Reset_Click(sender, e);
                panel2.Visible = false;

                MessageBox.Show("Whitelist record saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving whitelist record: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("tabBans", $"Error saving whitelist: {ex}");
            }
        }

        /// <summary>
        /// Handle click event to toggle between temporary and permanent exempt types.
        /// Enables/disables the expiration date picker based on selection.
        /// </summary>
        private void Whitelist_ExemptTypeToggle_Click(object sender, EventArgs e)
        {
            // Toggle between Temp and Perm on click
            CheckBox? checkBox = sender as CheckBox;
            if (checkBox?.Name == "checkBox_tempWL")
            {
                checkBox_permWL.Checked = !checkBox_tempWL.Checked;
            }
            else
            {
                checkBox_tempWL.Checked = !checkBox_permWL.Checked;
            }

            dateTimePicker_WLend.Enabled = checkBox_tempWL.Checked;
        }

        /// <summary>
        /// Handle click event to close the whitelist form and reset all fields.
        /// Clears selected record IDs and hides control buttons.
        /// </summary>
        private void Whitelist_Close_Click(object sender, EventArgs e)
        {
            // Reset selection IDs
            _whitelistSelectedRecordIDName = -1;
            _whitelistSelectedRecordIDIP = -1;
            // Reset and Hide
            Whitelist_Reset_Click(sender, e);

            // Control Buttons
            wlControlClose.Visible = false;
            wlControlSave.Visible = false;
            wlControlDelete.Visible = false;
            wlControlReset.Visible = false;

            panel2.Visible = false;
        }

        /// <summary>
        /// Handle delete button click for whitelist records
        /// </summary>
        private void Whitelist_Delete_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            // Check what records are selected
            bool hasNameRecord = _whitelistSelectedRecordIDName != -1;
            bool hasIPRecord = _whitelistSelectedRecordIDIP != -1;

            if (!hasNameRecord && !hasIPRecord)
            {
                MessageBox.Show("No record selected for deletion.", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the records
            banInstancePlayerName? nameRecord = hasNameRecord
                ? instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDName)
                : null;

            banInstancePlayerIP? ipRecord = hasIPRecord
                ? instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDIP)
                : null;

            // Check for associations
            int associatedIPID = nameRecord?.AssociatedIP ?? 0;
            int associatedNameID = ipRecord?.AssociatedName ?? 0;

            bool hasAssociation = (associatedIPID > 0 && hasNameRecord) ||
                                 (associatedNameID > 0 && hasIPRecord);

            RecordDeleteAction deleteAction;

            if (hasAssociation)
            {
                // Show dialog with options
                deleteAction = Whitelist_ShowDeleteConfirmationDialog(hasNameRecord, hasIPRecord,
                    nameRecord?.PlayerName ?? "",
                    ipRecord != null ? $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}" : "");
            }
            else
            {
                // Simple confirmation
                string recordType = hasNameRecord ? "player name" : "IP address";
                string recordValue = hasNameRecord ? nameRecord?.PlayerName ?? "" : $"{ipRecord?.PlayerIP}/{ipRecord?.SubnetMask}";

                var result = MessageBox.Show(
                    $"Are you sure you want to delete this {recordType} whitelist?\n\n{recordValue}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                deleteAction = hasNameRecord ? RecordDeleteAction.NameOnly : RecordDeleteAction.IPOnly;
            }

            if (deleteAction == RecordDeleteAction.None)
                return;

            try
            {
                // Perform deletion based on action
                switch (deleteAction)
                {
                    case RecordDeleteAction.Both:
                        Whitelist_DeleteBothRecords(nameRecord!, ipRecord!);
                        break;

                    case RecordDeleteAction.NameOnly:
                        Whitelist_DeleteNameRecord(nameRecord!, associatedIPID);
                        break;

                    case RecordDeleteAction.IPOnly:
                        Whitelist_DeleteIPRecord(ipRecord!, associatedNameID);
                        break;
                }

                // Reset form
                _whitelistSelectedRecordIDName = -1;
                _whitelistSelectedRecordIDIP = -1;
                Whitelist_Reset_Click(sender, e);
                panel2.Visible = false;

                // Refresh the data
                Whitelist_Refresh_Click(sender, e);

                MessageBox.Show("Record(s) deleted successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("tabBans", $"Error deleting whitelist: {ex}");
            }
        }

        /// <summary>
        /// Delete both name and IP whitelist records
        /// </summary>
        private void Whitelist_DeleteBothRecords(banInstancePlayerName nameRecord, banInstancePlayerIP ipRecord)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerNameRecord(nameRecord.RecordID);
            DatabaseManager.RemovePlayerIPRecord(ipRecord.RecordID);

            // Remove from in-memory lists
            instanceBans.WhitelistedNames.Remove(nameRecord);
            instanceBans.WhitelistedIPs.Remove(ipRecord);

            AppDebug.Log("tabBans", $"Deleted both whitelist records: Name ID {nameRecord.RecordID}, IP ID {ipRecord.RecordID}");
        }

        /// <summary>
        /// Delete name whitelist record only, update associated IP if exists
        /// </summary>
        private void Whitelist_DeleteNameRecord(banInstancePlayerName nameRecord, int associatedIPID)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerNameRecord(nameRecord.RecordID);

            // Remove from in-memory list
            instanceBans.WhitelistedNames.Remove(nameRecord);

            // Update associated IP record to clear the association
            if (associatedIPID > 0)
            {
                var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == associatedIPID);
                if (ipRecord != null)
                {
                    ipRecord.AssociatedName = 0;
                    DatabaseManager.UpdatePlayerIPRecord(ipRecord);
                    AppDebug.Log("tabBans", $"Cleared association on IP whitelist record {ipRecord.RecordID}");
                }
            }

            AppDebug.Log("tabBans", $"Deleted name whitelist record ID {nameRecord.RecordID}");
        }

        /// <summary>
        /// Delete IP whitelist record only, update associated name if exists
        /// </summary>
        private void Whitelist_DeleteIPRecord(banInstancePlayerIP ipRecord, int associatedNameID)
        {
            if (instanceBans == null)
                return;

            // Delete from database
            DatabaseManager.RemovePlayerIPRecord(ipRecord.RecordID);

            // Remove from in-memory list
            instanceBans.WhitelistedIPs.Remove(ipRecord);

            // Update associated name record to clear the association
            if (associatedNameID > 0)
            {
                var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == associatedNameID);
                if (nameRecord != null)
                {
                    nameRecord.AssociatedIP = 0;
                    DatabaseManager.UpdatePlayerNameRecord(nameRecord);
                    AppDebug.Log("tabBans", $"Cleared association on name whitelist record {nameRecord.RecordID}");
                }
            }

            AppDebug.Log("tabBans", $"Deleted IP whitelist record ID {ipRecord.RecordID}");
        }

        /// <summary>
        /// Show dialog to confirm deletion when whitelist records are associated
        /// </summary>
        private RecordDeleteAction Whitelist_ShowDeleteConfirmationDialog(bool hasName, bool hasIP, string playerName, string ipAddress)
        {
            string message = "This record has an associated ";

            if (hasName && hasIP)
            {
                message = $"These records are linked:\n\nPlayer Name: {playerName}\nIP Address: {ipAddress}\n\nWhat would you like to delete?";
            }
            else if (hasName)
            {
                message = $"This player name whitelist has an associated IP address whitelist:\n\nPlayer Name: {playerName}\nAssociated IP: {ipAddress}\n\nWhat would you like to delete?";
            }
            else
            {
                message = $"This IP address whitelist has an associated player name whitelist:\n\nIP Address: {ipAddress}\nAssociated Name: {playerName}\n\nWhat would you like to delete?";
            }

            using (var dialog = new Form())
            {
                dialog.Text = "Confirm Delete - Associated Records";
                dialog.Size = new Size(375, 270);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                var label = new Label
                {
                    Text = message,
                    Location = new Point(20, 20),
                    Size = new Size(400, 95),
                    AutoSize = false
                };

                var btnBoth = new Button
                {
                    Text = "Delete Both",
                    Location = new Point(20, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Yes
                };

                var btnNameOnly = new Button
                {
                    Text = "Name Only",
                    Location = new Point(130, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Retry,
                    Enabled = hasName
                };

                var btnIPOnly = new Button
                {
                    Text = "IP Only",
                    Location = new Point(240, 135),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.Ignore,
                    Enabled = hasIP
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(20, 175),
                    Size = new Size(320, 30),
                    DialogResult = DialogResult.Cancel
                };

                dialog.Controls.Add(label);
                dialog.Controls.Add(btnBoth);
                dialog.Controls.Add(btnNameOnly);
                dialog.Controls.Add(btnIPOnly);
                dialog.Controls.Add(btnCancel);

                dialog.AcceptButton = btnCancel;
                dialog.CancelButton = btnCancel;

                var result = dialog.ShowDialog();

                return result switch
                {
                    DialogResult.Yes => RecordDeleteAction.Both,
                    DialogResult.Retry => RecordDeleteAction.NameOnly,
                    DialogResult.Ignore => RecordDeleteAction.IPOnly,
                    _ => RecordDeleteAction.None
                };
            }
        }

        /// <summary>
        /// Handle double-click on player name whitelist to edit
        /// </summary>
        private void Whitelist_NameGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
                return;

            var recordID = (int)dgPlayerNamesWhitelist.Rows[e.RowIndex].Cells[0].Value;
            var record = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadWhitelistRecordForEditing(record, null);
        }

        /// <summary>
        /// Handle double-click on IP address whitelist to edit
        /// </summary>
        private void Whitelist_IPGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
                return;

            var recordID = (int)dgPlayerAddressWhitelist.Rows[e.RowIndex].Cells[0].Value;
            var record = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadWhitelistRecordForEditing(null, record);
        }

        /// <summary>
        /// Load a whitelist record into the form for editing
        /// </summary>
        private void LoadWhitelistRecordForEditing(banInstancePlayerName? nameRecord, banInstancePlayerIP? ipRecord)
        {
            if (instanceBans == null)
                return;

            // Set selected record IDs
            _whitelistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            _whitelistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;

            // Check for associations and load both if they exist
            if (nameRecord != null && nameRecord.AssociatedIP > 0)
            {
                ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == nameRecord.AssociatedIP);
                _whitelistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;
            }
            else if (ipRecord != null && ipRecord.AssociatedName > 0)
            {
                nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == ipRecord.AssociatedName);
                _whitelistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            }

            // Show the form
            panel2.Visible = true;

            // Load player name data
            if (nameRecord != null)
            {
                groupBox10.Visible = true;
                textBox_playerNameWL.Text = nameRecord.PlayerName;
            }
            else
            {
                groupBox10.Visible = false;
                textBox_playerNameWL.Text = string.Empty;
            }

            // Load IP address data
            if (ipRecord != null)
            {
                groupBox9.Visible = true;
                textBox_addressWL.Text = ipRecord.PlayerIP.ToString();
                cb_subnetWL.Text = ipRecord.SubnetMask.ToString();
            }
            else
            {
                groupBox9.Visible = false;
                textBox_addressWL.Text = string.Empty;
                cb_subnetWL.Text = "32";
            }

            // Load common data (use nameRecord first, fallback to ipRecord)
            var dataRecord = nameRecord ?? (object?)ipRecord;

            if (nameRecord != null)
            {
                dateTimePicker_WLstart.MinDate = DateTime.Today.AddYears(-1);
                dateTimePicker_WLstart.MaxDate = DateTime.Today.AddYears(1);
                dateTimePicker_WLstart.Value = nameRecord.Date;

                if (nameRecord.RecordType == banInstanceRecordType.Temporary && nameRecord.ExpireDate.HasValue)
                {
                    checkBox_tempWL.Checked = true;
                    checkBox_permWL.Checked = false;
                    dateTimePicker_WLend.Enabled = true;
                    dateTimePicker_WLend.MinDate = DateTime.Today.AddYears(-1);
                    dateTimePicker_WLend.MaxDate = DateTime.Today.AddYears(1);
                    dateTimePicker_WLend.Value = nameRecord.ExpireDate.Value;
                }
                else
                {
                    checkBox_tempWL.Checked = false;
                    checkBox_permWL.Checked = true;
                    dateTimePicker_WLend.Enabled = false;
                }

                textBox_notesWL.Text = nameRecord.Notes;
            }
            else if (ipRecord != null)
            {
                dateTimePicker_WLstart.MinDate = DateTime.Today.AddYears(-1);
                dateTimePicker_WLstart.MaxDate = DateTime.Today.AddYears(1);
                dateTimePicker_WLstart.Value = ipRecord.Date;

                if (ipRecord.RecordType == banInstanceRecordType.Temporary && ipRecord.ExpireDate.HasValue)
                {
                    checkBox_tempWL.Checked = true;
                    checkBox_permWL.Checked = false;
                    dateTimePicker_WLend.Enabled = true;
                    dateTimePicker_WLend.MinDate = DateTime.Today.AddYears(-1);
                    dateTimePicker_WLend.MaxDate = DateTime.Today.AddYears(1);
                    dateTimePicker_WLend.Value = ipRecord.ExpireDate.Value;
                }
                else
                {
                    checkBox_tempWL.Checked = false;
                    checkBox_permWL.Checked = true;
                    dateTimePicker_WLend.Enabled = false;
                }

                textBox_notesWL.Text = ipRecord.Notes;
            }

            // Show control buttons
            wlControlDelete.Visible = true;
            wlControlSave.Visible = true;
            wlControlReset.Visible = true;
        }

    }
}