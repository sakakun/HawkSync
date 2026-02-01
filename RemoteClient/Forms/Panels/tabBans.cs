using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using RemoteClient.Core;
using RemoteClient.Services;
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

namespace RemoteClient.Forms.Panels
{
    public partial class tabBans : UserControl
    {
        private theInstance? theInstance => CommonCore.theInstance;
        private banInstance? banInstance => CommonCore.instanceBans;

        public tabBans()
        {
            InitializeComponent();
            // Initialize Forms
            InitializeBlacklistForm();
            InitializeWhitelistForm();

            // Timers
            SetupProxyCheckInactivityTimer();
            SetupNetLimiterInactivityTimer();
            // Change Trackers
            WireUpProxyCheckChangeTracking();
            WireUpNetLimiterChangeTracking();

            // OPTIONALLY: Subscribe to snapshots for server info panel
            ApiCore.OnSnapshotReceived += OnSnapshotReceived;

        }

        private void OnSnapshotReceived(ServerSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ServerSnapshot>(OnSnapshotReceived), snapshot);
                return;
            }

            UpdateUIElements();
        }

        private void UpdateUIElements()
        {
            // Update Blacklist and Whitelist UI
            UpdateNameListUI(dgPlayerNamesBlacklist, banInstance!.BannedPlayerNames, tabBlacklist);
            UpdateNameListUI(dgPlayerNamesWhitelist, banInstance!.WhitelistedNames, tabWhitelist);
            UpdateIPListUI(dgPlayerAddressBlacklist, banInstance!.BannedPlayerIPs, tabBlacklist);
            UpdateIPListUI(dgPlayerAddressWhitelist, banInstance!.WhitelistedIPs, tabWhitelist);

            // Update Country Proxy Block List UI
            UpdateProxyCountryBlockListUI();
            // Netlimiter Connection Log UI
            UpdateNetLimiterConnectionLogUI();
            // Proxy Settings UI
            ProxyCheck_LoadSettings(null!, null!);
            LoadProxyBlockedCountries();
            // Netlimiter Settings UI
            NetLimiter_LoadSettings();

        }

        private void UpdateNameListUI(DataGridView grid, List<banInstancePlayerName> nameList, TabPage tab)
        {
            if (banControls.SelectedTab != tab || !tab.Visible || nameList == null)
                return;

            if (banInstance!.ForceUIUpdates) { grid.Rows.Clear(); }

            var recordDict = nameList.ToDictionary(n => n.RecordID);

            for (int i = grid.Rows.Count - 1; i >= 0; i--)
            {
                var row = grid.Rows[i];
                if (row.Cells[0].Value is int recordID && !recordDict.ContainsKey(recordID))
                {
                    grid.Rows.RemoveAt(i);
                }
            }

            var presentRecordIDs = new HashSet<int>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[0].Value is int recordID && recordDict.TryGetValue(recordID, out var record))
                {
                    presentRecordIDs.Add(recordID);

                    if (!Equals(row.Cells[1].Value, record.PlayerName))
                        row.Cells[1].Value = record.PlayerName;
                    var dateStr = record.Date.ToString("yyyy-MM-dd");
                    if (!Equals(row.Cells[2].Value, dateStr))
                        row.Cells[2].Value = dateStr;
                    // Removed checks for columns 3 and 4
                }
            }

            foreach (var record in nameList)
            {
                if (!presentRecordIDs.Contains(record.RecordID))
                {
                    grid.Rows.Add(
                        record.RecordID,
                        record.PlayerName,
                        record.Date.ToString("yyyy-MM-dd")
                    );
                }
            }
        }

        private void UpdateIPListUI(DataGridView grid, List<banInstancePlayerIP> ipList, TabPage tab)
        {
            if (banControls.SelectedTab != tab || !tab.Visible || ipList == null)
                return;

            if (banInstance!.ForceUIUpdates) { grid.Rows.Clear(); }

            var recordDict = ipList.ToDictionary(ip => ip.RecordID);

            for (int i = grid.Rows.Count - 1; i >= 0; i--)
            {
                var row = grid.Rows[i];
                if (row.Cells[0].Value is int recordID && !recordDict.ContainsKey(recordID))
                {
                    grid.Rows.RemoveAt(i);
                }
            }

            var presentRecordIDs = new HashSet<int>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[0].Value is int recordID && recordDict.TryGetValue(recordID, out var record))
                {
                    presentRecordIDs.Add(recordID);

                    if (!Equals(row.Cells[1].Value, record.PlayerIP.ToString()))
                        row.Cells[1].Value = record.PlayerIP.ToString() + "/" + record.SubnetMask;
                    var dateStr = record.Date.ToString("yyyy-MM-dd");
                    if (!Equals(row.Cells[2].Value, dateStr))
                        row.Cells[2].Value = dateStr;
                    // Removed checks for columns 3 and 4 (ExpireDate, Notes)
                }
            }

            foreach (var record in ipList)
            {
                if (!presentRecordIDs.Contains(record.RecordID))
                {
                    grid.Rows.Add(
                        record.RecordID,
                        record.PlayerIP.ToString() + "/" + record.SubnetMask,
                        record.Date.ToString("yyyy-MM-dd")
                    );
                }
            }
        }

        private void UpdateProxyCountryBlockListUI()
        {
            // Only update UI if the Proxy Checking tab is currently selected and visible
            if (banControls.SelectedTab != tabProxyChecking || !tabProxyChecking.Visible)
                return;

            if (banInstance == null || banInstance.ProxyBlockedCountries == null) return;

            // Build a HashSet of current RecordIDs in the ProxyBlockedCountries list
            HashSet<int> currentRecordIDs = banInstance.ProxyBlockedCountries.Select(c => c.RecordID).ToHashSet();

            // Remove rows from the DataGridView that are no longer in the ProxyBlockedCountries list
            for (int i = dgProxyCountryBlockList.Rows.Count - 1; i >= 0; i--)
            {
                var row = dgProxyCountryBlockList.Rows[i];
                if (row.Cells[0].Value is int recordID && !currentRecordIDs.Contains(recordID))
                {
                    dgProxyCountryBlockList.Rows.RemoveAt(i);
                }
            }

            // Build a HashSet of existing RecordIDs in the DataGridView after removals
            HashSet<int> existingRecordIDs = new HashSet<int>();
            foreach (DataGridViewRow row in dgProxyCountryBlockList.Rows)
            {
                if (row.Cells[0].Value is int recordID)
                {
                    existingRecordIDs.Add(recordID);
                }
            }

            // Add new proxy country records that are not already in the DataGridView
            foreach (var country in banInstance.ProxyBlockedCountries)
            {
                if (!existingRecordIDs.Contains(country.RecordID))
                {
                    dgProxyCountryBlockList.Rows.Add(
                        country.RecordID,
                        country.CountryCode,
                        country.CountryName
                    );
                }
            }
        }

        private void UpdateNetLimiterConnectionLogUI()
        {
            // Only update UI if the Netlimiter tab is currently selected and visible
            if (banControls.SelectedTab != tabNetlimiter || !tabNetlimiter.Visible)
                return;

            if (banInstance == null || banInstance.NetLimiterConnectionLogs == null)
                return;

            dg_NetlimiterConnectionLog.Rows.Clear();


            // Add new connection log entries that are not already in the DataGridView
            foreach (var log in banInstance.NetLimiterConnectionLogs)
            {
                dg_NetlimiterConnectionLog.Rows.Add(
                    log.NL_rowID,
                    log.NL_ipAddress,
                    log.NL_numCons,
                    log.NL_vpnStatus,
                    log.NL_notes
                );
            }
        }

        private void RefreshBlacklistWhitelist(object sender, EventArgs e)
        {
            banInstance!.ForceUIUpdates = true;
            UpdateUIElements();
        }
        /// ========================================== ///
        /// Black List Functions
        /// ========================================== ///

        // ================================================================================
        // BLACKLIST FUNCTIONALITY
        // ================================================================================

        private int _blacklistSelectedRecordIDName = -1;
        private int _blacklistSelectedRecordIDIP = -1;

        private void InitializeBlacklistForm()
        {
            // Initially hide the blacklist form
            blacklistForm.Visible = false;
            methodFunction_HideBlacklistButtons();
        }

        private void methodFunction_HideBlacklistButtons()
        {
            blacklist_btnClose.Visible = false;
            blacklist_btnDelete.Visible = false;
            blacklist_btnSave.Visible = false;
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
        /// Handle click event to close the blacklist form.
        /// </summary>
        private void blacklist_btnClose_Click(object sender, EventArgs e)
        {
            // Hide the blacklist form
            blacklistForm.Visible = false;
            methodFunction_HideBlacklistButtons();
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
        /// Handle double-click on player name blacklist to edit
        /// </summary>
        private void Blacklist_NameGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || banInstance == null)
                return;

            var recordID = (int)dgPlayerNamesBlacklist.Rows[e.RowIndex].Cells[0].Value!;
            var record = banInstance.BannedPlayerNames.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadBlacklistRecordForEditing(record, null);
        }
        /// <summary>
        /// Handle double-click on IP address blacklist to edit
        /// </summary>
        private void Blacklist_IPGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || banInstance == null)
                return;

            var recordID = (int)dgPlayerAddressBlacklist.Rows[e.RowIndex].Cells[0].Value!;
            var record = banInstance.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadBlacklistRecordForEditing(null, record);
        }
        /// <summary>
        /// Load a record into the form for editing
        /// </summary>
        private void LoadBlacklistRecordForEditing(banInstancePlayerName? nameRecord, banInstancePlayerIP? ipRecord)
        {
            if (banInstance == null)
                return;

            // Set selected record IDs
            _blacklistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            _blacklistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;

            // Check for associations and load both if they exist
            if (nameRecord != null && nameRecord.AssociatedIP > 0)
            {
                ipRecord = banInstance.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == nameRecord.AssociatedIP);
                _blacklistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;
            }
            else if (ipRecord != null && ipRecord.AssociatedName > 0)
            {
                nameRecord = banInstance.BannedPlayerNames.FirstOrDefault(x => x.RecordID == ipRecord.AssociatedName);
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
            blacklist_btnClose.Visible = true;
        }
        private async void Blacklist_Save_Click(object sender, EventArgs e)
        {
            if (banInstance == null)
                return;

            // Gather form data
            var recordType = blacklist_PermBan.Checked
                ? banInstanceRecordType.Permanent
                : banInstanceRecordType.Temporary;

            DateTime? expireDate = recordType == banInstanceRecordType.Temporary
                ? blacklist_DateEnd.Value
                : null;

            DateTime banDate = blacklist_DateStart.Value;
            string notes = blacklist_notes.Text.Trim();

            bool isNameVisible = blacklist_PlayerName.Visible && !string.IsNullOrWhiteSpace(blacklist_PlayerNameTxt.Text);
            bool isIPVisible = blacklist_IPAddress.Visible && !string.IsNullOrWhiteSpace(blacklist_IPAddressTxt.Text);

            // Parse IP if needed
            IPAddress? ipAddress = null;
            int subnetMask = 32;

            if (isIPVisible)
            {
                if (!IPAddress.TryParse(blacklist_IPAddressTxt.Text.Trim(), out ipAddress))
                {
                    MessageBox.Show("Invalid IP Address format.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(blacklist_IPSubnetTxt.Text, out subnetMask))
                    subnetMask = 32;
            }

            var req = new BanRecordSaveRequest
            {
                NameRecordID = _blacklistSelectedRecordIDName == -1 ? null : _blacklistSelectedRecordIDName,
                IPRecordID = _blacklistSelectedRecordIDIP == -1 ? null : _blacklistSelectedRecordIDIP,
                PlayerName = isNameVisible ? blacklist_PlayerNameTxt.Text.Trim() : null,
                IPAddress = isIPVisible ? blacklist_IPAddressTxt.Text.Trim() : null,
                SubnetMask = subnetMask,
                BanDate = banDate,
                ExpireDate = expireDate,
                RecordType = recordType,
                Notes = notes,
                IsName = isNameVisible,
                IsIP = isIPVisible
            };

            try
            {
                var result = await ApiCore.ApiClient!.SaveBlacklistRecordAsync(req);
                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Ban record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hide the blacklist form
                blacklistForm.Visible = false;
                methodFunction_HideBlacklistButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving ban record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private RecordDeleteAction Blacklist_ShowDeleteConfirmationDialog(bool hasName, bool hasIP, string playerName, string ipAddress)
        {
            string message;
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
        private async void blacklist_btnDelete_Click(object sender, EventArgs e)
        {
            if (banInstance == null)
                return;

            bool hasName = _blacklistSelectedRecordIDName != -1;
            bool hasIP = _blacklistSelectedRecordIDIP != -1;

            if (!hasName && !hasIP)
            {
                MessageBox.Show("No record selected for deletion.", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the records (for display)
            var nameRecord = hasName
                ? banInstance.BannedPlayerNames.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDName)
                : null;
            var ipRecord = hasIP
                ? banInstance.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDIP)
                : null;

            int associatedIPID = nameRecord?.AssociatedIP ?? 0;
            int associatedNameID = ipRecord?.AssociatedName ?? 0;
            bool hasAssociation = (associatedIPID > 0 && hasName) || (associatedNameID > 0 && hasIP);

            RecordDeleteAction deleteAction;

            if (hasAssociation)
            {
                deleteAction = Blacklist_ShowDeleteConfirmationDialog(
                    hasName, hasIP,
                    nameRecord?.PlayerName ?? "",
                    ipRecord != null ? $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}" : "");
            }
            else
            {
                string recordType = hasName ? "player name" : "IP address";
                string recordValue = hasName ? nameRecord?.PlayerName ?? "" : $"{ipRecord?.PlayerIP}/{ipRecord?.SubnetMask}";

                var result = MessageBox.Show(
                    $"Are you sure you want to delete this {recordType} ban?\n\n{recordValue}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                deleteAction = hasName ? RecordDeleteAction.NameOnly : RecordDeleteAction.IPOnly;
            }

            if (deleteAction == RecordDeleteAction.None)
                return;

            bool anySuccess = false;
            StringBuilder errorMessages = new StringBuilder();

            // Send delete commands to server
            if (deleteAction == RecordDeleteAction.Both || deleteAction == RecordDeleteAction.NameOnly)
            {
                var response = await ApiCore.ApiClient!.DeleteBlacklistRecordAsync(_blacklistSelectedRecordIDName, isName: true);
                if (response.Success)
                    anySuccess = true;
                else
                    errorMessages.AppendLine(response.Message);
            }
            if (deleteAction == RecordDeleteAction.Both || deleteAction == RecordDeleteAction.IPOnly)
            {
                var response = await ApiCore.ApiClient!.DeleteBlacklistRecordAsync(_blacklistSelectedRecordIDIP, isName: false);
                if (response.Success)
                    anySuccess = true;
                else
                    errorMessages.AppendLine(response.Message);
            }

            if (anySuccess)
            {
                MessageBox.Show("Record(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                blacklistForm.Visible = false;
                methodFunction_HideBlacklistButtons();
                RefreshBlacklistWhitelist(sender, e);
            }
            else
            {
                MessageBox.Show(errorMessages.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================================
        // WHITELIST FUNCTIONALITY
        // ================================================================================

        private int _whitelistSelectedRecordIDName = -1;
        private int _whitelistSelectedRecordIDIP = -1;

        private void InitializeWhitelistForm()
        {
            // Initially hide the blacklist form
            WhitelistForm.Visible = false;
            methodFunction_HideWhitelistButtons();
        }
        private void methodFunction_HideWhitelistButtons()
        {
            wlControlClose.Visible = false;
            wlControlDelete.Visible = false;
            wlControlSave.Visible = false;
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
            WhitelistForm.Visible = true;
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
        /// Handle click event to close the blacklist form.
        /// </summary>
        private void Whitelist_btnClose_Click(object sender, EventArgs e)
        {
            // Hide the blacklist form
            WhitelistForm.Visible = false;
            methodFunction_HideWhitelistButtons();
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
        /// Handle double-click on player name whitelist to edit
        /// </summary>
        private void Whitelist_NameGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || banInstance == null)
                return;

            var recordID = (int)dgPlayerNamesWhitelist.Rows[e.RowIndex].Cells[0].Value!;
            var record = banInstance.WhitelistedNames.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadWhitelistRecordForEditing(record, null);
        }
        /// <summary>
        /// Handle double-click on IP address whitelist to edit
        /// </summary>
        private void Whitelist_IPGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || banInstance == null)
                return;

            var recordID = (int)dgPlayerAddressWhitelist.Rows[e.RowIndex].Cells[0].Value!;
            var record = banInstance.WhitelistedIPs.FirstOrDefault(x => x.RecordID == recordID);

            if (record == null)
                return;

            LoadWhitelistRecordForEditing(null, record);
        }
        /// <summary>
        /// Load a whitelist record into the form for editing
        /// </summary>
        private void LoadWhitelistRecordForEditing(banInstancePlayerName? nameRecord, banInstancePlayerIP? ipRecord)
        {
            if (banInstance == null)
                return;

            // Set selected record IDs
            _whitelistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            _whitelistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;

            // Check for associations and load both if they exist
            if (nameRecord != null && nameRecord.AssociatedIP > 0)
            {
                ipRecord = banInstance.WhitelistedIPs.FirstOrDefault(x => x.RecordID == nameRecord.AssociatedIP);
                _whitelistSelectedRecordIDIP = ipRecord?.RecordID ?? -1;
            }
            else if (ipRecord != null && ipRecord.AssociatedName > 0)
            {
                nameRecord = banInstance.WhitelistedNames.FirstOrDefault(x => x.RecordID == ipRecord.AssociatedName);
                _whitelistSelectedRecordIDName = nameRecord?.RecordID ?? -1;
            }

            // Show the form
            WhitelistForm.Visible = true;

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
            wlControlClose.Visible = true;
        }
        private async void Whitelist_Save_Click(object sender, EventArgs e)
        {
            if (banInstance == null)
                return;

            var recordType = checkBox_permWL.Checked
                ? banInstanceRecordType.Permanent
                : banInstanceRecordType.Temporary;

            DateTime? expireDate = recordType == banInstanceRecordType.Temporary
                ? dateTimePicker_WLend.Value
                : null;

            DateTime exemptDate = dateTimePicker_WLstart.Value;
            string notes = textBox_notesWL.Text.Trim();

            bool isNameVisible = groupBox10.Visible && !string.IsNullOrWhiteSpace(textBox_playerNameWL.Text);
            bool isIPVisible = groupBox9.Visible && !string.IsNullOrWhiteSpace(textBox_addressWL.Text);

            IPAddress? ipAddress = null;
            int subnetMask = 32;

            if (isIPVisible)
            {
                if (!IPAddress.TryParse(textBox_addressWL.Text.Trim(), out ipAddress))
                {
                    MessageBox.Show("Invalid IP Address format.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(cb_subnetWL.Text, out subnetMask))
                    subnetMask = 32;
            }

            var req = new BanRecordSaveRequest
            {
                NameRecordID = _whitelistSelectedRecordIDName == -1 ? null : _whitelistSelectedRecordIDName,
                IPRecordID = _whitelistSelectedRecordIDIP == -1 ? null : _whitelistSelectedRecordIDIP,
                PlayerName = isNameVisible ? textBox_playerNameWL.Text.Trim() : null,
                IPAddress = isIPVisible ? textBox_addressWL.Text.Trim() : null,
                SubnetMask = subnetMask,
                BanDate = exemptDate,
                ExpireDate = expireDate,
                RecordType = recordType,
                Notes = notes,
                IsName = isNameVisible,
                IsIP = isIPVisible
            };

            try
            {
                var result = await ApiCore.ApiClient!.SaveWhitelistRecordAsync(req);
                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Whitelist record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hide the whitelist form
                WhitelistForm.Visible = false;
                methodFunction_HideBlacklistButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving whitelist record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private RecordDeleteAction Whitelist_ShowDeleteConfirmationDialog(bool hasName, bool hasIP, string playerName, string ipAddress)
        {
            string message;
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
        private async void wlControlDelete_Click(object sender, EventArgs e)
        {
            if (banInstance == null)
                return;

            bool hasName = _whitelistSelectedRecordIDName != -1;
            bool hasIP = _whitelistSelectedRecordIDIP != -1;

            if (!hasName && !hasIP)
            {
                MessageBox.Show("No record selected for deletion.", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nameRecord = hasName
                ? banInstance.WhitelistedNames.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDName)
                : null;
            var ipRecord = hasIP
                ? banInstance.WhitelistedIPs.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDIP)
                : null;

            int associatedIPID = nameRecord?.AssociatedIP ?? 0;
            int associatedNameID = ipRecord?.AssociatedName ?? 0;
            bool hasAssociation = (associatedIPID > 0 && hasName) || (associatedNameID > 0 && hasIP);

            RecordDeleteAction deleteAction;

            if (hasAssociation)
            {
                deleteAction = Whitelist_ShowDeleteConfirmationDialog(
                    hasName, hasIP,
                    nameRecord?.PlayerName ?? "",
                    ipRecord != null ? $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}" : "");
            }
            else
            {
                string recordType = hasName ? "player name" : "IP address";
                string recordValue = hasName ? nameRecord?.PlayerName ?? "" : $"{ipRecord?.PlayerIP}/{ipRecord?.SubnetMask}";

                var result = MessageBox.Show(
                    $"Are you sure you want to delete this {recordType} whitelist?\n\n{recordValue}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                deleteAction = hasName ? RecordDeleteAction.NameOnly : RecordDeleteAction.IPOnly;
            }

            if (deleteAction == RecordDeleteAction.None)
                return;

            bool anySuccess = false;
            StringBuilder errorMessages = new StringBuilder();

            // Send delete commands to server
            if (deleteAction == RecordDeleteAction.Both || deleteAction == RecordDeleteAction.NameOnly)
            {
                var response = await ApiCore.ApiClient!.DeleteWhitelistRecordAsync(_whitelistSelectedRecordIDName, isName: true);
                if (response.Success)
                    anySuccess = true;
                else
                    errorMessages.AppendLine(response.Message);
            }
            if (deleteAction == RecordDeleteAction.Both || deleteAction == RecordDeleteAction.IPOnly)
            {
                var response = await ApiCore.ApiClient!.DeleteWhitelistRecordAsync(_whitelistSelectedRecordIDIP, isName: false);
                if (response.Success)
                    anySuccess = true;
                else
                    errorMessages.AppendLine(response.Message);
            }

            if (anySuccess)
            {
                MessageBox.Show("Record(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hide the whitelist form
                WhitelistForm.Visible = false;
                methodFunction_HideBlacklistButtons();
            }
            else
            {
                MessageBox.Show(errorMessages.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================================
        // PROXY CHECKING FUNCTIONALITY
        // ================================================================================

        // Add these fields to your class
        private bool _isEditingProxyCheck = false;
        private bool _suppressProxyCheckChangeTracking = false;
        private DateTime _lastProxyCheckEditTime = DateTime.MinValue;
        private System.Windows.Forms.Timer? _proxyCheckInactivityTimer;
        private const int PROXYCHECK_INACTIVITY_TIMEOUT_SECONDS = 120;

        // Call this in your constructor after InitializeComponent()
        private void SetupProxyCheckInactivityTimer()
        {
            _proxyCheckInactivityTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _proxyCheckInactivityTimer.Tick += ProxyCheckInactivityTimer_Tick;
            _proxyCheckInactivityTimer.Start();
        }

        // Timer tick handler
        private void ProxyCheckInactivityTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isEditingProxyCheck)
                return;

            if ((DateTime.Now - _lastProxyCheckEditTime).TotalSeconds >= PROXYCHECK_INACTIVITY_TIMEOUT_SECONDS)
            {
                _isEditingProxyCheck = false;
                ProxyCheck_LoadSettings(null!, null!);
                // Optionally, show a message to the user
                MessageBox.Show(
                    "ProxyCheck settings refreshed due to inactivity.\nAny unsaved changes were discarded.",
                    "Auto-Refresh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // Wire up change tracking for all ProxyCheck controls
        private void WireUpProxyCheckChangeTracking()
        {
            cb_enableProxyCheck.CheckedChanged += ProxyCheck_ControlValueChanged;
            textBox_ProxyAPIKey.TextChanged += ProxyCheck_ControlValueChanged;
            num_proxyCacheDays.ValueChanged += ProxyCheck_ControlValueChanged;

            checkBox_proxyNone.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_proxyKick.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_proxyBlock.CheckedChanged += ProxyCheck_ControlValueChanged;

            checkBox_vpnNone.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_vpnKick.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_vpnBlock.CheckedChanged += ProxyCheck_ControlValueChanged;

            checkBox_torNone.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_torKick.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_torBlock.CheckedChanged += ProxyCheck_ControlValueChanged;

            checkBox_GeoOff.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_GeoBlock.CheckedChanged += ProxyCheck_ControlValueChanged;
            checkBox_GeoAllow.CheckedChanged += ProxyCheck_ControlValueChanged;

            cb_serviceProxyCheckIO.CheckedChanged += ProxyCheck_ControlValueChanged;
            cb_serviceIP2LocationIO.CheckedChanged += ProxyCheck_ControlValueChanged;
        }

        private void ProxyCheck_ControlValueChanged(object? sender, EventArgs e)
        {
            if (_suppressProxyCheckChangeTracking)
                return;

            if (!_isEditingProxyCheck)
            {
                _isEditingProxyCheck = true;
            }
            _lastProxyCheckEditTime = DateTime.Now;
        }

        // Update ProxyCheck_LoadSettings to respect editing state
        private void ProxyCheck_LoadSettings(object sender, EventArgs e)
        {
            if (_isEditingProxyCheck)
                return;

            _suppressProxyCheckChangeTracking = true;
            try
            {
                if (theInstance == null)
                    return;

                cb_enableProxyCheck.Checked = theInstance.proxyCheckEnabled;
                textBox_ProxyAPIKey.Text = theInstance.proxyCheckAPIKey;
                num_proxyCacheDays.Value = theInstance.proxyCheckCacheTime;

                checkBox_proxyNone.Checked = (theInstance.proxyCheckProxyAction == 0);
                checkBox_proxyKick.Checked = (theInstance.proxyCheckProxyAction == 1);
                checkBox_proxyBlock.Checked = (theInstance.proxyCheckProxyAction == 2);

                checkBox_vpnNone.Checked = (theInstance.proxyCheckVPNAction == 0);
                checkBox_vpnKick.Checked = (theInstance.proxyCheckVPNAction == 1);
                checkBox_vpnBlock.Checked = (theInstance.proxyCheckVPNAction == 2);

                checkBox_torNone.Checked = (theInstance.proxyCheckTORAction == 0);
                checkBox_torKick.Checked = (theInstance.proxyCheckTORAction == 1);
                checkBox_torBlock.Checked = (theInstance.proxyCheckTORAction == 2);

                checkBox_GeoOff.Checked = (theInstance.proxyCheckGeoMode == 0);
                checkBox_GeoBlock.Checked = (theInstance.proxyCheckGeoMode == 1);
                checkBox_GeoAllow.Checked = (theInstance.proxyCheckGeoMode == 2);

                cb_serviceProxyCheckIO.Checked = (theInstance.proxyCheckServiceProvider == 1);
                cb_serviceIP2LocationIO.Checked = (theInstance.proxyCheckServiceProvider == 2);
            }
            finally
            {
                _suppressProxyCheckChangeTracking = false;
            }
        }

        private void ProxyCheck_CBServicesChanged(object sender, EventArgs e)
        {
            var clicked = (CheckBox)sender;
            cb_serviceProxyCheckIO.Checked = clicked == cb_serviceProxyCheckIO;
            cb_serviceIP2LocationIO.Checked = clicked == cb_serviceIP2LocationIO;
        }

        private void ProxyCheck_CBProxyChange(object sender, EventArgs e)
        {
            var clicked = (CheckBox)sender;
            checkBox_proxyNone.Checked = clicked == checkBox_proxyNone;
            checkBox_proxyKick.Checked = clicked == checkBox_proxyKick;
            checkBox_proxyBlock.Checked = clicked == checkBox_proxyBlock;
        }

        private void ProxyCheck_CBVPNChange(object sender, EventArgs e)
        {
            var clicked = (CheckBox)sender;
            checkBox_vpnNone.Checked = clicked == checkBox_vpnNone;
            checkBox_vpnKick.Checked = clicked == checkBox_vpnKick;
            checkBox_vpnBlock.Checked = clicked == checkBox_vpnBlock;
        }

        private void ProxyCheck_CBTORChange(object sender, EventArgs e)
        {
            var clicked = (CheckBox)sender;
            checkBox_torNone.Checked = clicked == checkBox_torNone;
            checkBox_torKick.Checked = clicked == checkBox_torKick;
            checkBox_torBlock.Checked = clicked == checkBox_torBlock;
        }

        private void ProxyCheck_CBGEOChange(object sender, EventArgs e)
        {
            var clicked = (CheckBox)sender;
            checkBox_GeoAllow.Checked = clicked == checkBox_GeoAllow;
            checkBox_GeoBlock.Checked = clicked == checkBox_GeoBlock;
            checkBox_GeoOff.Checked = clicked == checkBox_GeoOff;
        }

        private async void ProxyCheck_SaveSettings(object sender, EventArgs e)
        {
            // Gather settings from UI
            var settings = new ProxyCheckSettingsRequest
            {
                ProxyCheckEnabled = cb_enableProxyCheck.Checked,
                ProxyAPIKey = textBox_ProxyAPIKey.Text.Trim(),
                ProxyCacheDays = (int)num_proxyCacheDays.Value,
                ProxyAction = checkBox_proxyBlock.Checked ? 2 : checkBox_proxyKick.Checked ? 1 : 0,
                VPNAction = checkBox_vpnBlock.Checked ? 2 : checkBox_vpnKick.Checked ? 1 : 0,
                TORAction = checkBox_torBlock.Checked ? 2 : checkBox_torKick.Checked ? 1 : 0,
                GeoMode = checkBox_GeoBlock.Checked ? 1 : checkBox_GeoAllow.Checked ? 2 : 0,
                ServiceProvider = cb_serviceProxyCheckIO.Checked ? 1 : cb_serviceIP2LocationIO.Checked ? 2 : 0
            };

            try
            {
                var result = await ApiCore.ApiClient!.SaveProxyCheckSettingsAsync(settings);

                if (result.Success)
                {
                    _isEditingProxyCheck = false;
                    MessageBox.Show("ProxyCheck settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optionally reload settings/UI here
                    ProxyCheck_LoadSettings(sender, e);
                }
                else
                {
                    MessageBox.Show($"Failed to save settings:\n\n{result.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProxyCheck_ResetChanges(object sender, EventArgs e)
        {
            // Exit edit mode and reset change tracking
            _isEditingProxyCheck = false;
            _lastProxyCheckEditTime = DateTime.MinValue;

            // Reload settings from the server instance
            ProxyCheck_LoadSettings(sender, e);

            // Optionally, notify the user
            MessageBox.Show(
                "All unsaved changes to ProxyCheck settings have been discarded and the latest settings have been reloaded from the server.",
                "ProxyCheck Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Load blocked countries into DataGridView
        /// </summary>
        private void LoadProxyBlockedCountries()
        {
            if (banInstance == null)
                return;

            var countries = banInstance.ProxyBlockedCountries;
            var countryDict = countries.ToDictionary(c => c.RecordID);

            // Remove rows that are no longer present
            for (int i = dgProxyCountryBlockList.Rows.Count - 1; i >= 0; i--)
            {
                var row = dgProxyCountryBlockList.Rows[i];
                if (row.Cells[0].Value is int recordID && !countryDict.ContainsKey(recordID))
                {
                    dgProxyCountryBlockList.Rows.RemoveAt(i);
                }
            }

            // Track which record IDs are already present
            var presentRecordIDs = new HashSet<int>();
            foreach (DataGridViewRow row in dgProxyCountryBlockList.Rows)
            {
                if (row.Cells[0].Value is int recordID && countryDict.TryGetValue(recordID, out var country))
                {
                    presentRecordIDs.Add(recordID);

                    // Update if changed
                    if (!Equals(row.Cells[1].Value, country.CountryCode))
                        row.Cells[1].Value = country.CountryCode;
                    if (!Equals(row.Cells[2].Value, country.CountryName))
                        row.Cells[2].Value = country.CountryName;
                }
            }

            // Add new countries not present in the grid
            foreach (var country in countries)
            {
                if (!presentRecordIDs.Contains(country.RecordID))
                {
                    dgProxyCountryBlockList.Rows.Add(
                        country.RecordID,
                        country.CountryCode,
                        country.CountryName
                    );
                }
            }
        }

        private async void ProxyCheck_TestService_Click(object sender, EventArgs e)
        {
            // Validate service selection
            if (!cb_serviceProxyCheckIO.Checked && !cb_serviceIP2LocationIO.Checked)
            {
                MessageBox.Show("Please select a proxy check service provider.",
                    "Service Not Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Validate API key
            string apiKey = textBox_ProxyAPIKey.Text.Trim();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Please enter an API key.",
                    "API Key Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Disable button during test
            var button = sender as Button;
            if (button != null)
            {
                button.Enabled = false;
                button.Text = "Testing...";
            }

            try
            {
                int serviceProvider = cb_serviceProxyCheckIO.Checked ? 1 : 2;
                string testIP = "8.8.8.8"; // Or allow user input

                var result = await ApiCore.ApiClient!.TestProxyCheckServiceAsync(apiKey, serviceProvider, testIP);

                if (result == null || !result.Success)
                {
                    MessageBox.Show($"Service test failed: {result?.ErrorMessage ?? "Unknown error"}",
                        "Service Test Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var resultMessage = new StringBuilder();
                resultMessage.AppendLine($"Service: {(serviceProvider == 1 ? "ProxyCheck.io" : "IP2Location.io")}");
                resultMessage.AppendLine($"Test IP: {testIP}");
                resultMessage.AppendLine();
                resultMessage.AppendLine($"Results:");
                resultMessage.AppendLine($"  • VPN: {(result.IsVpn ? "Yes" : "No")}");
                resultMessage.AppendLine($"  • Proxy: {(result.IsProxy ? "Yes" : "No")}");
                resultMessage.AppendLine($"  • Tor: {(result.IsTor ? "Yes" : "No")}");
                resultMessage.AppendLine($"  • Risk Score: {result.RiskScore}/100");

                if (!string.IsNullOrEmpty(result.CountryName))
                {
                    resultMessage.AppendLine();
                    resultMessage.AppendLine($"Location:");
                    resultMessage.AppendLine($"  • Country: {result.CountryName} ({result.CountryCode})");
                    if (!string.IsNullOrEmpty(result.Region))
                        resultMessage.AppendLine($"  • Region: {result.Region}");
                    if (!string.IsNullOrEmpty(result.City))
                        resultMessage.AppendLine($"  • City: {result.City}");
                }

                if (!string.IsNullOrEmpty(result.Provider))
                {
                    resultMessage.AppendLine();
                    resultMessage.AppendLine($"Provider: {result.Provider}");
                }

                MessageBox.Show(resultMessage.ToString(),
                    "Service Test Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            finally
            {
                // Re-enable button
                if (button != null)
                {
                    button.Enabled = true;
                    button.Text = "Test Service";
                }
            }
        }

        private async void btn_proxyAddCountry_Click(object sender, EventArgs e)
        {
            string countryCode = textBox_countryCode.Text.Trim().ToUpper();
            string countryName = textBox_countryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(countryName))
            {
                MessageBox.Show("Please enter both country code and country name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = await ApiCore.ApiClient!.AddBlockedCountryAsync(countryCode, countryName);

            if (result.Success)
            {
                textBox_countryCode.Text = string.Empty;
                textBox_countryName.Text = string.Empty;
                // Optionally refresh the grid
                LoadProxyBlockedCountries();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dgProxyCountryBlockList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int recordId = (int)dgProxyCountryBlockList.Rows[e.RowIndex].Cells[0].Value!;
            string countryCode = dgProxyCountryBlockList.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
            string countryName = dgProxyCountryBlockList.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";

            var dialogResult = MessageBox.Show(
                $"Are you sure you want to remove '{countryName}' ({countryCode}) from the blocked countries list?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
                return;

            var result = await ApiCore.ApiClient!.RemoveBlockedCountryAsync(recordId);

            if (result.Success)
            {
                // Optionally refresh the grid
                LoadProxyBlockedCountries();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================================
        // NETLIMITER CHECKING FUNCTIONALITY
        // ================================================================================

        // Add these fields to your class
        private bool _isEditingNetLimiter = false;
        private bool _suppressNetLimiterChangeTracking = false;
        private DateTime _lastNetLimiterEditTime = DateTime.MinValue;
        private System.Windows.Forms.Timer? _netLimiterInactivityTimer;
        private const int NETLIMITER_INACTIVITY_TIMEOUT_SECONDS = 120;

        // Call this in your constructor after InitializeComponent()
        private void SetupNetLimiterInactivityTimer()
        {
            _netLimiterInactivityTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _netLimiterInactivityTimer.Tick += NetLimiterInactivityTimer_Tick;
            _netLimiterInactivityTimer.Start();
        }

        // Timer tick handler
        private void NetLimiterInactivityTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isEditingNetLimiter)
                return;

            if ((DateTime.Now - _lastNetLimiterEditTime).TotalSeconds >= NETLIMITER_INACTIVITY_TIMEOUT_SECONDS)
            {
                _isEditingNetLimiter = false;
                NetLimiter_LoadSettings();
                MessageBox.Show(
                    "NetLimiter settings refreshed due to inactivity.\nAny unsaved changes were discarded.",
                    "Auto-Refresh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // Wire up change tracking for all NetLimiter controls
        private void WireUpNetLimiterChangeTracking()
        {
            checkBox_EnableNetLimiter.CheckedChanged += NetLimiter_ControlValueChanged;
            textBox_NetLimiterHost.TextChanged += NetLimiter_ControlValueChanged;
            num_NetLimiterPort.ValueChanged += NetLimiter_ControlValueChanged;
            textBox_NetLimiterUsername.TextChanged += NetLimiter_ControlValueChanged;
            textBox_NetLimiterPassword.TextChanged += NetLimiter_ControlValueChanged;
            comboBox_NetLimiterFilterName.TextChanged += NetLimiter_ControlValueChanged;
            checkBox_NetLimiterEnableConLimit.CheckedChanged += NetLimiter_ControlValueChanged;
            num_NetLimiterConThreshold.ValueChanged += NetLimiter_ControlValueChanged;
        }

        private void NetLimiter_ControlValueChanged(object? sender, EventArgs e)
        {
            if (_suppressNetLimiterChangeTracking)
                return;

            if (!_isEditingNetLimiter)
            {
                _isEditingNetLimiter = true;
            }
            _lastNetLimiterEditTime = DateTime.Now;
        }

        // Update NetLimiter_LoadSettings to respect editing state
        private void NetLimiter_LoadSettings()
        {
            if (_isEditingNetLimiter)
                return;

            _suppressNetLimiterChangeTracking = true;
            try
            {
                if (theInstance == null)
                    return;

                checkBox_EnableNetLimiter.Checked = theInstance.netLimiterEnabled;
                textBox_NetLimiterHost.Text = theInstance.netLimiterHost;
                num_NetLimiterPort.Value = theInstance.netLimiterPort;
                textBox_NetLimiterUsername.Text = theInstance.netLimiterUsername;
                textBox_NetLimiterPassword.Text = theInstance.netLimiterPassword;
                comboBox_NetLimiterFilterName.Text = theInstance.netLimiterFilterName;
                checkBox_NetLimiterEnableConLimit.Checked = theInstance.netLimiterEnableConLimit;
                num_NetLimiterConThreshold.Value = theInstance.netLimiterConThreshold;
            }
            finally
            {
                _suppressNetLimiterChangeTracking = false;
            }
        }

        private void NetLimiter_SaveSettings(object sender, EventArgs e)
        {
            if (theInstance == null)
                return;
            // Gather settings from UI
            var settings = new NetLimiterSettingsRequest
            {
                NetLimiterEnabled = checkBox_EnableNetLimiter.Checked,
                NetLimiterHost = textBox_NetLimiterHost.Text.Trim(),
                NetLimiterPort = (int)num_NetLimiterPort.Value,
                NetLimiterUsername = textBox_NetLimiterUsername.Text.Trim(),
                NetLimiterPassword = textBox_NetLimiterPassword.Text,
                NetLimiterFilterName = comboBox_NetLimiterFilterName.Text.Trim(),
                NetLimiterEnableConLimit = checkBox_NetLimiterEnableConLimit.Checked,
                NetLimiterConThreshold = (int)num_NetLimiterConThreshold.Value
            };
            try
            {
                var result = ApiCore.ApiClient!.SaveNetLimiterSettingsAsync(settings).Result;
                if (result.Success)
                {
                    MessageBox.Show("NetLimiter settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optionally reload settings/UI here
                    NetLimiter_LoadSettings();
                }
                else
                {
                    MessageBox.Show($"Failed to save settings:\n\n{result.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void NetLimiter_ResetForm(object sender, EventArgs e)
        {
            // Exit edit mode and reset change tracking
            _isEditingNetLimiter = false;
            _lastNetLimiterEditTime = DateTime.MinValue;

            // Reload settings from the server instance
            NetLimiter_LoadSettings();

            // Optionally, notify the user
            MessageBox.Show(
                "All unsaved changes to NetLimiter settings have been discarded and the latest settings have been reloaded from the server.",
                "NetLimiter Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public async void NetLimiter_RefreshFilters(object sender, EventArgs e)
        {
            var (success, filters, errorMessage) = await ApiCore.ApiClient!.GetNetLimiterFiltersAsync();

            if (!success || filters == null || filters.Count == 0)
            {
                MessageBox.Show(errorMessage ?? "No filters were retrieved from NetLimiter.",
                    "No Filters Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            comboBox_NetLimiterFilterName.Items.Clear();
            foreach (var filter in filters)
                comboBox_NetLimiterFilterName.Items.Add(filter);
        }

    }
}