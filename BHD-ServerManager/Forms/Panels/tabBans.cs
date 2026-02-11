using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Tickers;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabBans;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Services.Maps;
using RecordDeleteAction = BHD_ServerManager.Classes.InstanceManagers.RecordDeleteAction;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabBans : UserControl
    {
        // --- Instance Objects ---
        private banInstance? instanceBans => CommonCore.instanceBans;
        private theInstance? theInstance => CommonCore.theInstance;

        public tabBans()
        {
            InitializeComponent();

            dgPlayerNamesBlacklist.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dgPlayerNamesWhitelist.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            banInstanceManager.LoadSettings();

            // Load Data into DataGridViews
            LoadBlacklistGrids();
            LoadWhitelistGrids();
            LoadProxyBlockedCountries();

            // Load Settings
            ProxyCheck_LoadSettings(null!, null!);
            NetLimiter_LoadSettings(null!, null!);

            // Initialize Form Controls
            BlacklistForm_Initialize();
            WhitelistForm_Initialize();

            // Wire up tab change event
            banControls.SelectedIndexChanged += BanControls_SelectedIndexChanged;

            CommonCore.Ticker?.Start("tabBansTicker", 1000, () => tabBansTicker());

        }

        public void tabBansTicker()
        {
            AppDebug.Log("tabBans", "Ticker update - checking NetLimiter settings lockdown");          

            NetLimiter_RefreshConnections();

            // NetLimiter Settings Lockdown
            bool netLimiterProcessAttached = (NetLimiterClient._bridgeProcess != null);
            bool shouldBeEnabled = !netLimiterProcessAttached;
    
            // Only update if the state has changed
            if (textBox_NetLimiterHost.Enabled != shouldBeEnabled)
            {
                textBox_NetLimiterHost.Enabled = shouldBeEnabled;
                num_NetLimiterPort.Enabled = shouldBeEnabled;
                textBox_NetLimiterUsername.Enabled = shouldBeEnabled;
                textBox_NetLimiterPassword.Enabled = shouldBeEnabled;
            }

            // Initialize Proxy Service if needed
            if (theInstance!.proxyCheckEnabled && !ProxyCheckManager.IsInitialized)
            {
                // Initialize Proxy Check
                banInstanceManager.InitializeProxyService();
            }
            
            if (theInstance.netLimiterEnabled && NetLimiterClient._bridgeProcess == null)
            {
                // This should reload the filters from NetLimiter and init the bridge process
                NetLimiter_RefreshFilters(null!, null!);

                // If this doesn't work, disable NetLimiter integration
                if (NetLimiterClient._bridgeProcess == null)
                {
                    theInstance.netLimiterEnabled = false;
                    checkBox_EnableNetLimiter.Checked = false;
                    ServerSettings.Set("NetLimiterEnabled", false);
                    MessageBox.Show("Failed to start NetLimiter bridge process. NetLimiter integration has been disabled.", "NetLimiter Integration Disabled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AppDebug.Log("tickerBanManagement", "NetLimiter integration disabled due to failure to start bridge process.");
                }
            } 

            if (instanceBans!.ForceUIUpdates)
            {
                instanceBans!.ForceUIUpdates = false;

                Blacklist_Refresh_Click(null!, null!);
                Whitelist_Refresh_Click(null!, null!);
                ProxyCheck_LoadSettings(null!, null!);
                NetLimiter_LoadSettings(null!, null!);
            }

        }

        private void NetLimiter_RefreshConnections()
        {
            
            if (InvokeRequired)
            {
                Invoke(new Action(NetLimiter_RefreshConnections));
                return;
            }

            dg_NetlimiterConnectionLog.Rows.Clear();

            var Connections = instanceBans!.NetLimiterConnectionLogs.ToList();

            AppDebug.Log("tabBans", $"Refreshing NetLimiter connections log with {Connections.Count} entries.");
            try
            {
                
                foreach (var Conn in Connections)
                {
                    var ip = Conn?.NL_ipAddress ?? string.Empty;
                    var vpnStatus = Conn?.NL_vpnStatus ?? string.Empty;
                    var notes = Conn?.NL_notes ?? string.Empty;
                    var rowId = Conn?.NL_rowID ?? 0;
                    var numCons = Conn?.NL_numCons ?? 0;
                    
                    dg_NetlimiterConnectionLog.Rows.Add(
                        rowId,
                        ip,
                        numCons,
                        vpnStatus,
                        notes
                    );
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("tabBans", $"Error refreshing NetLimiter connections log: {ex}");
            }
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
        private int _blacklistSelectedRecordIDName = -1;
        private int _blacklistSelectedRecordIDIP = -1;

        /// <summary>
        /// Load blacklist data from banInstance into DataGridViews
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

            // Reload from database via manager
            banInstanceManager.LoadBlacklistRecords();

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
            blacklist_DateStart.MinDate = DateTimePicker.MinimumDateTime;
            blacklist_DateStart.MaxDate = DateTimePicker.MaximumDateTime;
            blacklist_DateStart.Value = DateTime.Now;
            blacklist_DateEnd.MinDate = DateTimePicker.MinimumDateTime;
            blacklist_DateEnd.MaxDate = DateTimePicker.MaximumDateTime;
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
        /// Handle click event to save the current blacklist record (create new or update existing).
        /// Creates bidirectional associations when both name and IP records are added together.
        /// </summary>  
        private void Blacklist_Save_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
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

            try
            {
                // Determine if we're creating or updating
                bool isCreatingName = _blacklistSelectedRecordIDName == -1;
                bool isCreatingIP = _blacklistSelectedRecordIDIP == -1;

                // Handle different scenarios
                if (isNameVisible && isIPVisible)
                {
                    // Both name and IP
                    if (isCreatingName && isCreatingIP)
                    {
                        // Create both with bidirectional association
                        var result = banInstanceManager.AddBlacklistBothRecords(
                            blacklist_PlayerNameTxt.Text.Trim(),
                            ipAddress!,
                            subnetMask,
                            banDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Add to grids
                        var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == result.NameRecordID);
                        var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == result.IPRecordID);

                        if (nameRecord != null)
                        {
                            dgPlayerNamesBlacklist.Rows.Add(
                                nameRecord.RecordID,
                                nameRecord.PlayerName,
                                nameRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }

                        if (ipRecord != null)
                        {
                            dgPlayerAddressBlacklist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        // Update existing records
                        if (isNameVisible && !isCreatingName)
                        {
                            var nameResult = banInstanceManager.UpdateBlacklistNameRecord(
                                _blacklistSelectedRecordIDName,
                                blacklist_PlayerNameTxt.Text.Trim(),
                                banDate,
                                expireDate,
                                recordType,
                                notes,
                                isCreatingIP ? null : _blacklistSelectedRecordIDIP
                            );

                            if (!nameResult.Success)
                            {
                                MessageBox.Show(nameResult.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Update grid
                            var row = dgPlayerNamesBlacklist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value! == _blacklistSelectedRecordIDName);
                            if (row != null)
                            {
                                var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDName);
                                if (nameRecord != null)
                                {
                                    row.Cells[1].Value = nameRecord.PlayerName;
                                    row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }

                        if (isIPVisible && !isCreatingIP)
                        {
                            var ipResult = banInstanceManager.UpdateBlacklistIPRecord(
                                _blacklistSelectedRecordIDIP,
                                ipAddress!,
                                subnetMask,
                                banDate,
                                expireDate,
                                recordType,
                                notes,
                                isCreatingName ? null : _blacklistSelectedRecordIDName
                            );

                            if (!ipResult.Success)
                            {
                                MessageBox.Show(ipResult.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Update grid
                            var row = dgPlayerAddressBlacklist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value! == _blacklistSelectedRecordIDIP);
                            if (row != null)
                            {
                                var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDIP);
                                if (ipRecord != null)
                                {
                                    row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                    row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                }
                else if (isNameVisible)
                {
                    // Name only
                    if (isCreatingName)
                    {
                        var result = banInstanceManager.AddBlacklistNameRecord(
                            blacklist_PlayerNameTxt.Text.Trim(),
                            banDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == result.RecordID);
                        if (nameRecord != null)
                        {
                            dgPlayerNamesBlacklist.Rows.Add(
                                nameRecord.RecordID,
                                nameRecord.PlayerName,
                                nameRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        var result = banInstanceManager.UpdateBlacklistNameRecord(
                            _blacklistSelectedRecordIDName,
                            blacklist_PlayerNameTxt.Text.Trim(),
                            banDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Update grid
                        var row = dgPlayerNamesBlacklist.Rows.Cast<DataGridViewRow>()
                            .FirstOrDefault(r => (int)r.Cells[0].Value! == _blacklistSelectedRecordIDName);
                        if (row != null)
                        {
                            var nameRecord = instanceBans.BannedPlayerNames.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDName);
                            if (nameRecord != null)
                            {
                                row.Cells[1].Value = nameRecord.PlayerName;
                                row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
                else if (isIPVisible)
                {
                    // IP only
                    if (isCreatingIP)
                    {
                        var result = banInstanceManager.AddBlacklistIPRecord(
                            ipAddress!,
                            subnetMask,
                            banDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == result.RecordID);
                        if (ipRecord != null)
                        {
                            dgPlayerAddressBlacklist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        var result = banInstanceManager.UpdateBlacklistIPRecord(
                            _blacklistSelectedRecordIDIP,
                            ipAddress!,
                            subnetMask,
                            banDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Update grid
                        var row = dgPlayerAddressBlacklist.Rows.Cast<DataGridViewRow>()
                            .FirstOrDefault(r => (int)r.Cells[0].Value! == _blacklistSelectedRecordIDIP);
                        if (row != null)
                        {
                            var ipRecord = instanceBans.BannedPlayerIPs.FirstOrDefault(x => x.RecordID == _blacklistSelectedRecordIDIP);
                            if (ipRecord != null)
                            {
                                row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                // Reset selection IDs and form
                _blacklistSelectedRecordIDName = -1;
                _blacklistSelectedRecordIDIP = -1;
                blacklistForm.Visible = false;

                blacklist_btnDelete.Visible = false;
                blacklist_btnSave.Visible = false;
                blacklist_btnClose.Visible = false;

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

            // Control Buttons
            blacklist_btnClose.Visible = false;
            blacklist_btnSave.Visible = false;
            blacklist_btnDelete.Visible = false;

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
                OperationResult opResult;

                // Perform deletion based on action via manager
                switch (deleteAction)
                {
                    case RecordDeleteAction.Both:
                        opResult = banInstanceManager.DeleteBlacklistBothRecords(
                            _blacklistSelectedRecordIDName,
                            _blacklistSelectedRecordIDIP
                        );
                        break;

                    case RecordDeleteAction.NameOnly:
                        opResult = banInstanceManager.DeleteBlacklistNameRecord(_blacklistSelectedRecordIDName);
                        break;

                    case RecordDeleteAction.IPOnly:
                        opResult = banInstanceManager.DeleteBlacklistIPRecord(_blacklistSelectedRecordIDIP);
                        break;

                    default:
                        return;
                }

                if (!opResult.Success)
                {
                    MessageBox.Show(opResult.Message, "Delete Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Reset form
                _blacklistSelectedRecordIDName = -1;
                _blacklistSelectedRecordIDIP = -1;
                blacklistForm.Visible = false;

                // Refresh the data
                Blacklist_Refresh_Click(sender, e);

                // Control Buttons
                blacklist_btnClose.Visible = false;
                blacklist_btnSave.Visible = false;
                blacklist_btnDelete.Visible = false;

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
        /// Handle double-click on player name blacklist to edit
        /// </summary>
        private void Blacklist_NameGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
                return;

            var recordID = (int)dgPlayerNamesBlacklist.Rows[e.RowIndex].Cells[0].Value!;
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

            var recordID = (int)dgPlayerAddressBlacklist.Rows[e.RowIndex].Cells[0].Value!;
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
                blacklist_DateStart.MinDate = DateTimePicker.MinimumDateTime;
                blacklist_DateStart.MaxDate = DateTimePicker.MaximumDateTime;
                blacklist_DateStart.Value = nameRecord.Date;

                if (nameRecord.RecordType == banInstanceRecordType.Temporary && nameRecord.ExpireDate.HasValue)
                {
                    blacklist_TempBan.Checked = true;
                    blacklist_PermBan.Checked = false;
                    blacklist_DateEnd.Enabled = true;
                    blacklist_DateEnd.MinDate = DateTimePicker.MinimumDateTime;
                    blacklist_DateEnd.MaxDate = DateTimePicker.MaximumDateTime;
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
                blacklist_DateStart.MinDate = DateTimePicker.MinimumDateTime;
                blacklist_DateStart.MaxDate = DateTimePicker.MaximumDateTime;
                blacklist_DateStart.Value = ipRecord.Date;

                if (ipRecord.RecordType == banInstanceRecordType.Temporary && ipRecord.ExpireDate.HasValue)
                {
                    blacklist_TempBan.Checked = true;
                    blacklist_PermBan.Checked = false;
                    blacklist_DateEnd.Enabled = true;
                    blacklist_DateEnd.MinDate = DateTimePicker.MinimumDateTime;
                    blacklist_DateEnd.MaxDate = DateTimePicker.MaximumDateTime;
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

        private void btnExportBlacklist_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Export Blacklist to JSON";
                saveDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                saveDialog.FileName = "BlacklistExport.json";
                saveDialog.OverwritePrompt = true;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportBlacklistToJson(saveDialog.FileName);
                }
            }
        }

        // Add this method to your tabBans class
        private void ExportBlacklistToJson(string filePath)
        {
            if (instanceBans == null)
            {
                MessageBox.Show("No ban data available to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var exportData = new BlacklistImportModel();
            exportData.BannedPlayerIPs = instanceBans.BannedPlayerIPs;
            exportData.BannedPlayerNames = instanceBans.BannedPlayerNames;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                File.WriteAllText(filePath, JsonSerializer.Serialize(exportData, options));
                MessageBox.Show("Blacklist exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting blacklist: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportBlacklist_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Import Blacklist";
                openDialog.Filter = "JSON/INI Files (*.json;*.ini)|*.json;*.ini|JSON Files (*.json)|*.json|INI Files (*.ini)|*.ini|All Files (*.*)|*.*";
                openDialog.Multiselect = false;

                if (openDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    string filePath = openDialog.FileName;
                    string firstLine = File.ReadLines(filePath).FirstOrDefault()?.Trim() ?? "";

                    if (firstLine.Equals("[IpAddresses]", StringComparison.OrdinalIgnoreCase))
                    {
                        ImportLegacyBannedIps(filePath, sender, e);
                        return;
                    }
                    else if (firstLine.Equals("[Players]", StringComparison.OrdinalIgnoreCase))
                    {
                        ImportLegacyBannedNames(filePath, sender, e);
                        return;
                    }
                    else
                    {
                        string json = File.ReadAllText(openDialog.FileName);
                        var importData = JsonSerializer.Deserialize<BlacklistImportModel>(json);

                        if (importData == null)
                        {
                            MessageBox.Show("No data found in the selected file.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Track processed associations to avoid duplicates
                        var processedNameIds = new HashSet<int>();
                        var processedIpIds = new HashSet<int>();

                        // Process name records (handle both and name-only)
                        if (importData.BannedPlayerNames != null)
                        {
                            foreach (var nameRecord in importData.BannedPlayerNames)
                            {
                                if (nameRecord.AssociatedIP.HasValue && nameRecord.AssociatedIP.Value > 0)
                                {
                                    // Only process if not already handled
                                    if (!processedNameIds.Contains(nameRecord.RecordID) && !processedIpIds.Contains(nameRecord.AssociatedIP.Value))
                                    {
                                        var ipRecord = importData.BannedPlayerIPs?.FirstOrDefault(ip => ip.RecordID == nameRecord.AssociatedIP.Value);
                                        if (ipRecord != null)
                                        {
                                            var result = banInstanceManager.AddBlacklistBothRecords(
                                                nameRecord.PlayerName,
                                                ipRecord.PlayerIP,
                                                ipRecord.SubnetMask,
                                                nameRecord.Date,
                                                nameRecord.ExpireDate,
                                                nameRecord.RecordType,
                                                nameRecord.Notes,
                                                true
                                            );
                                            if (!result.Success)
                                            {
                                                MessageBox.Show($"Error importing name/IP pair '{nameRecord.PlayerName}/{ipRecord.PlayerIP}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            processedNameIds.Add(nameRecord.RecordID);
                                            processedIpIds.Add(ipRecord.RecordID);
                                            continue;
                                        }
                                    }
                                }
                                // Name only
                                if (!processedNameIds.Contains(nameRecord.RecordID))
                                {
                                    var result = banInstanceManager.AddBlacklistNameRecord(
                                        nameRecord.PlayerName,
                                        nameRecord.Date,
                                        nameRecord.ExpireDate,
                                        nameRecord.RecordType,
                                        nameRecord.Notes,
                                        0,
                                        true
                                    );
                                    if (!result.Success)
                                    {
                                        MessageBox.Show($"Error importing name record '{nameRecord.PlayerName}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    processedNameIds.Add(nameRecord.RecordID);
                                }
                            }
                        }

                        // Process IP-only records
                        if (importData.BannedPlayerIPs != null)
                        {
                            foreach (var ipRecord in importData.BannedPlayerIPs)
                            {
                                if (ipRecord.AssociatedName.HasValue && ipRecord.AssociatedName.Value > 0)
                                {
                                    // Already processed as part of a name/IP pair
                                    if (processedIpIds.Contains(ipRecord.RecordID))
                                        continue;
                                }
                                // IP only
                                if (!processedIpIds.Contains(ipRecord.RecordID))
                                {
                                    var result = banInstanceManager.AddBlacklistIPRecord(
                                        ipRecord.PlayerIP,
                                        ipRecord.SubnetMask,
                                        ipRecord.Date,
                                        ipRecord.ExpireDate,
                                        ipRecord.RecordType,
                                        ipRecord.Notes,
                                        0,
                                        true
                                    );
                                    if (!result.Success)
                                    {
                                        MessageBox.Show($"Error importing IP record '{ipRecord.PlayerIP}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    processedIpIds.Add(ipRecord.RecordID);
                                }
                            }
                        }

                        // Refresh UI
                        Blacklist_Refresh_Click(sender, e);

                        MessageBox.Show($"Blacklist imported successfully. {CommonCore.instanceBans!.BannedPlayerNames.Count} Records", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing blacklist: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportLegacyBannedIps(string filePath, object sender, EventArgs e)
        {
            var lines = File.ReadAllLines(filePath);
            int importedCount = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("["))
                    continue;

                var parts = line.Split('=');
                if (parts.Length != 2)
                    continue;

                string ipPattern = parts[0].Trim();
                string dateStr = parts[1].Trim();

                // Determine subnet mask based on wildcard position
                int subnetMask = 32;
                string ipBase = ipPattern;

                if (ipPattern.EndsWith(".*"))
                {
                    int periodCount = ipPattern.Count(c => c == '.');
                    switch (periodCount)
                    {
                        case 1: // e.g., 10.*
                            subnetMask = 8;
                            ipBase = ipPattern.Replace(".*", ".0.0.0");
                            break;
                        case 2: // e.g., 10.0.*
                            subnetMask = 16;
                            ipBase = ipPattern.Replace(".*", ".0.0");
                            break;
                        case 3: // e.g., 10.0.0.*
                            subnetMask = 24;
                            ipBase = ipPattern.Replace(".*", ".0");
                            break;
                    }
                }
                else
                {
                    ipBase = ipPattern.Replace("*", "0");
                }

                if (!IPAddress.TryParse(ipBase, out var ipAddress))
                    continue;

                if (!DateTime.TryParse(dateStr, out var banDate))
                    banDate = DateTime.Now;

                var result = banInstanceManager.AddBlacklistIPRecord(
                    ipAddress,
                    subnetMask,
                    banDate,
                    null,
                    banInstanceRecordType.Permanent,
                    "Imported from legacy INI",
                    0,
                    true
                );
                if (result.Success)
                    importedCount++;
            }

            Blacklist_Refresh_Click(sender, e);
            MessageBox.Show($"Imported {importedCount} legacy IP bans.", "Legacy Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportLegacyBannedNames(string filePath, object sender, EventArgs e)
        {
            var lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));
            int importedCount = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("["))
                    continue;

                // Format: {PlayerName}[#]={DateTime}[-]{Notes}
                var eqSplit = line.Split('=');
                if (eqSplit.Length != 2)
                    continue;

                string namePart = eqSplit[0].Trim();
                string rightPart = eqSplit[1].Trim();

                // Extract player name (remove [#])
                int bracketIdx = namePart.IndexOf('[');
                string playerName = bracketIdx > 0 ? namePart.Substring(0, bracketIdx) : namePart;

                // Extract date and notes
                var dashSplit = rightPart.Split(new[] { "[-]" }, StringSplitOptions.None);
                string dateStr = dashSplit.Length > 0 ? dashSplit[0].Trim() : "";
                string notes = dashSplit.Length > 1 ? dashSplit[1].Trim() : "";

                if (!DateTime.TryParse(dateStr, out var banDate))
                    banDate = DateTime.Now;

                var result = banInstanceManager.AddBlacklistNameRecord(
                    playerName,
                    banDate,
                    null,
                    banInstanceRecordType.Permanent,
                    string.IsNullOrEmpty(notes) ? "Imported from legacy INI" : notes,
                    0,
                    true
                );
                if (result.Success)
                    importedCount++;
            }

            Blacklist_Refresh_Click(sender, e);
            MessageBox.Show($"Imported {importedCount} legacy name bans.", "Legacy Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ================================================================================
        // WHITELIST FUNCTIONALITY
        // ================================================================================

        private int _whitelistSelectedRecordIDName = -1;
        private int _whitelistSelectedRecordIDIP = -1;

        /// <summary>
        /// Load whitelist data from banInstance into DataGridViews
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

            // Reload from database via manager
            banInstanceManager.LoadWhitelistRecords();

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
            dateTimePicker_WLstart.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker_WLstart.MaxDate = DateTimePicker.MaximumDateTime;
            dateTimePicker_WLstart.Value = DateTime.Now;
            dateTimePicker_WLend.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker_WLend.MaxDate = DateTimePicker.MaximumDateTime;
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
            dateTimePicker_WLstart.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker_WLstart.MaxDate = DateTimePicker.MaximumDateTime;
            dateTimePicker_WLstart.Value = DateTime.Now;
            dateTimePicker_WLend.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker_WLend.MaxDate = DateTimePicker.MaximumDateTime;
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

            // Gather form data
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

            // Parse IP if needed
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

            try
            {
                // Determine if we're creating or updating
                bool isCreatingName = _whitelistSelectedRecordIDName == -1;
                bool isCreatingIP = _whitelistSelectedRecordIDIP == -1;

                // Handle different scenarios
                if (isNameVisible && isIPVisible)
                {
                    // Both name and IP
                    if (isCreatingName && isCreatingIP)
                    {
                        // Create both with bidirectional association
                        var result = banInstanceManager.AddWhitelistBothRecords(
                            textBox_playerNameWL.Text.Trim(),
                            ipAddress!,
                            subnetMask,
                            exemptDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Add to grids
                        var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == result.NameRecordID);
                        var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == result.IPRecordID);

                        if (nameRecord != null)
                        {
                            dgPlayerNamesWhitelist.Rows.Add(
                                nameRecord.RecordID,
                                nameRecord.PlayerName,
                                nameRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }

                        if (ipRecord != null)
                        {
                            dgPlayerAddressWhitelist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        // Update existing records
                        if (isNameVisible && !isCreatingName)
                        {
                            var nameResult = banInstanceManager.UpdateWhitelistNameRecord(
                                _whitelistSelectedRecordIDName,
                                textBox_playerNameWL.Text.Trim(),
                                exemptDate,
                                expireDate,
                                recordType,
                                notes,
                                isCreatingIP ? null : _whitelistSelectedRecordIDIP
                            );

                            if (!nameResult.Success)
                            {
                                MessageBox.Show(nameResult.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Update grid
                            var row = dgPlayerNamesWhitelist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value! == _whitelistSelectedRecordIDName);
                            if (row != null)
                            {
                                var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDName);
                                if (nameRecord != null)
                                {
                                    row.Cells[1].Value = nameRecord.PlayerName;
                                    row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }

                        if (isIPVisible && !isCreatingIP)
                        {
                            var ipResult = banInstanceManager.UpdateWhitelistIPRecord(
                                _whitelistSelectedRecordIDIP,
                                ipAddress!,
                                subnetMask,
                                exemptDate,
                                expireDate,
                                recordType,
                                notes,
                                isCreatingName ? null : _whitelistSelectedRecordIDName
                            );

                            if (!ipResult.Success)
                            {
                                MessageBox.Show(ipResult.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Update grid
                            var row = dgPlayerAddressWhitelist.Rows.Cast<DataGridViewRow>()
                                .FirstOrDefault(r => (int)r.Cells[0].Value! == _whitelistSelectedRecordIDIP);
                            if (row != null)
                            {
                                var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDIP);
                                if (ipRecord != null)
                                {
                                    row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                    row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                }
                else if (isNameVisible)
                {
                    // Name only
                    if (isCreatingName)
                    {
                        var result = banInstanceManager.AddWhitelistNameRecord(
                            textBox_playerNameWL.Text.Trim(),
                            exemptDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == result.RecordID);
                        if (nameRecord != null)
                        {
                            dgPlayerNamesWhitelist.Rows.Add(
                                nameRecord.RecordID,
                                nameRecord.PlayerName,
                                nameRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        var result = banInstanceManager.UpdateWhitelistNameRecord(
                            _whitelistSelectedRecordIDName,
                            textBox_playerNameWL.Text.Trim(),
                            exemptDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Update grid
                        var row = dgPlayerNamesWhitelist.Rows.Cast<DataGridViewRow>()
                            .FirstOrDefault(r => (int)r.Cells[0].Value! == _whitelistSelectedRecordIDName);
                        if (row != null)
                        {
                            var nameRecord = instanceBans.WhitelistedNames.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDName);
                            if (nameRecord != null)
                            {
                                row.Cells[1].Value = nameRecord.PlayerName;
                                row.Cells[2].Value = nameRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
                else if (isIPVisible)
                {
                    // IP only
                    if (isCreatingIP)
                    {
                        var result = banInstanceManager.AddWhitelistIPRecord(
                            ipAddress!,
                            subnetMask,
                            exemptDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == result.RecordID);
                        if (ipRecord != null)
                        {
                            dgPlayerAddressWhitelist.Rows.Add(
                                ipRecord.RecordID,
                                $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}",
                                ipRecord.Date.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        var result = banInstanceManager.UpdateWhitelistIPRecord(
                            _whitelistSelectedRecordIDIP,
                            ipAddress!,
                            subnetMask,
                            exemptDate,
                            expireDate,
                            recordType,
                            notes
                        );

                        if (!result.Success)
                        {
                            MessageBox.Show(result.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Update grid
                        var row = dgPlayerAddressWhitelist.Rows.Cast<DataGridViewRow>()
                            .FirstOrDefault(r => (int)r.Cells[0].Value! == _whitelistSelectedRecordIDIP);
                        if (row != null)
                        {
                            var ipRecord = instanceBans.WhitelistedIPs.FirstOrDefault(x => x.RecordID == _whitelistSelectedRecordIDIP);
                            if (ipRecord != null)
                            {
                                row.Cells[1].Value = $"{ipRecord.PlayerIP}/{ipRecord.SubnetMask}";
                                row.Cells[2].Value = ipRecord.Date.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }

                // Reset selection IDs and form
                _whitelistSelectedRecordIDName = -1;
                _whitelistSelectedRecordIDIP = -1;
                Whitelist_Reset_Click(sender, e);
                panel2.Visible = false;

                // Control Buttons
                wlControlClose.Visible = false;
                wlControlSave.Visible = false;
                wlControlDelete.Visible = false;

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
                OperationResult opResult;

                // Perform deletion based on action via manager
                switch (deleteAction)
                {
                    case RecordDeleteAction.Both:
                        opResult = banInstanceManager.DeleteWhitelistBothRecords(
                            _whitelistSelectedRecordIDName,
                            _whitelistSelectedRecordIDIP
                        );
                        break;

                    case RecordDeleteAction.NameOnly:
                        opResult = banInstanceManager.DeleteWhitelistNameRecord(_whitelistSelectedRecordIDName);
                        break;

                    case RecordDeleteAction.IPOnly:
                        opResult = banInstanceManager.DeleteWhitelistIPRecord(_whitelistSelectedRecordIDIP);
                        break;

                    default:
                        return;
                }

                if (!opResult.Success)
                {
                    MessageBox.Show(opResult.Message, "Delete Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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

            var recordID = (int)dgPlayerNamesWhitelist.Rows[e.RowIndex].Cells[0].Value!;
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

            var recordID = (int)dgPlayerAddressWhitelist.Rows[e.RowIndex].Cells[0].Value!;
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
                dateTimePicker_WLstart.MinDate = DateTimePicker.MinimumDateTime;
                dateTimePicker_WLstart.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimePicker_WLstart.Value = nameRecord.Date;

                if (nameRecord.RecordType == banInstanceRecordType.Temporary && nameRecord.ExpireDate.HasValue)
                {
                    checkBox_tempWL.Checked = true;
                    checkBox_permWL.Checked = false;
                    dateTimePicker_WLend.Enabled = true;
                    dateTimePicker_WLend.MinDate = DateTimePicker.MinimumDateTime;
                    dateTimePicker_WLend.MaxDate = DateTimePicker.MaximumDateTime;
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
                dateTimePicker_WLstart.MinDate = DateTimePicker.MinimumDateTime;
                dateTimePicker_WLstart.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimePicker_WLstart.Value = ipRecord.Date;

                if (ipRecord.RecordType == banInstanceRecordType.Temporary && ipRecord.ExpireDate.HasValue)
                {
                    checkBox_tempWL.Checked = true;
                    checkBox_permWL.Checked = false;
                    dateTimePicker_WLend.Enabled = true;
                    dateTimePicker_WLend.MinDate = DateTimePicker.MinimumDateTime;
                    dateTimePicker_WLend.MaxDate = DateTimePicker.MaximumDateTime;
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

        private void btnExportWhitelist_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Export Whitelist to JSON";
                saveDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                saveDialog.FileName = "WhitelistExport.json";
                saveDialog.OverwritePrompt = true;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportWhitelistToJson(saveDialog.FileName);
                }
            }
        }

        private void ExportWhitelistToJson(string filePath)
        {
            if (instanceBans == null)
            {
                MessageBox.Show("No whitelist data available to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var exportData = new BlacklistImportModel
            {
                BannedPlayerNames = instanceBans.WhitelistedNames,
                BannedPlayerIPs = instanceBans.WhitelistedIPs
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                File.WriteAllText(filePath, JsonSerializer.Serialize(exportData, options));
                MessageBox.Show("Whitelist exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting whitelist: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportWhitelist_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Import Whitelist from JSON";
                openDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                openDialog.Multiselect = false;

                if (openDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    string json = File.ReadAllText(openDialog.FileName);
                    var importData = JsonSerializer.Deserialize<BlacklistImportModel>(json);

                    if (importData == null)
                    {
                        MessageBox.Show("No data found in the selected file.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Track processed associations to avoid duplicates
                    var processedNameIds = new HashSet<int>();
                    var processedIpIds = new HashSet<int>();

                    // Process name records (handle both and name-only)
                    if (importData.BannedPlayerNames != null)
                    {
                        foreach (var nameRecord in importData.BannedPlayerNames)
                        {
                            if (nameRecord.AssociatedIP.HasValue && nameRecord.AssociatedIP.Value > 0)
                            {
                                // Only process if not already handled
                                if (!processedNameIds.Contains(nameRecord.RecordID) && !processedIpIds.Contains(nameRecord.AssociatedIP.Value))
                                {
                                    var ipRecord = importData.BannedPlayerIPs?.FirstOrDefault(ip => ip.RecordID == nameRecord.AssociatedIP.Value);
                                    if (ipRecord != null)
                                    {
                                        var result = banInstanceManager.AddWhitelistBothRecords(
                                            nameRecord.PlayerName,
                                            ipRecord.PlayerIP,
                                            ipRecord.SubnetMask,
                                            nameRecord.Date,
                                            nameRecord.ExpireDate,
                                            nameRecord.RecordType,
                                            nameRecord.Notes,
                                            true
                                        );
                                        if (!result.Success)
                                        {
                                            MessageBox.Show($"Error importing name/IP pair '{nameRecord.PlayerName}/{ipRecord.PlayerIP}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        processedNameIds.Add(nameRecord.RecordID);
                                        processedIpIds.Add(ipRecord.RecordID);
                                        continue;
                                    }
                                }
                            }
                            // Name only
                            if (!processedNameIds.Contains(nameRecord.RecordID))
                            {
                                var result = banInstanceManager.AddWhitelistNameRecord(
                                    nameRecord.PlayerName,
                                    nameRecord.Date,
                                    nameRecord.ExpireDate,
                                    nameRecord.RecordType,
                                    nameRecord.Notes,
                                    0,
                                    true
                                );
                                if (!result.Success)
                                {
                                    MessageBox.Show($"Error importing name record '{nameRecord.PlayerName}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                processedNameIds.Add(nameRecord.RecordID);
                            }
                        }
                    }

                    // Process IP-only records
                    if (importData.BannedPlayerIPs != null)
                    {
                        foreach (var ipRecord in importData.BannedPlayerIPs)
                        {
                            if (ipRecord.AssociatedName.HasValue && ipRecord.AssociatedName.Value > 0)
                            {
                                // Already processed as part of a name/IP pair
                                if (processedIpIds.Contains(ipRecord.RecordID))
                                    continue;
                            }
                            // IP only
                            if (!processedIpIds.Contains(ipRecord.RecordID))
                            {
                                var result = banInstanceManager.AddWhitelistIPRecord(
                                    ipRecord.PlayerIP,
                                    ipRecord.SubnetMask,
                                    ipRecord.Date,
                                    ipRecord.ExpireDate,
                                    ipRecord.RecordType,
                                    ipRecord.Notes,
                                    0,
                                    true
                                );
                                if (!result.Success)
                                {
                                    MessageBox.Show($"Error importing IP record '{ipRecord.PlayerIP}': {result.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                processedIpIds.Add(ipRecord.RecordID);
                            }
                        }
                    }

                    // Refresh UI
                    Whitelist_Refresh_Click(sender, e);

                    MessageBox.Show($"Blacklist imported successfully. {CommonCore.instanceBans!.BannedPlayerNames.Count} Records", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing blacklist: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ================================================================================
        // PROXY CHECKING FUNCTIONALITY
        // ================================================================================

        /// <summary>
        /// Load and display proxy check settings from instance
        /// </summary>
        private void ProxyCheck_LoadSettings(object sender, EventArgs e)
        {
            if (theInstance == null)
                return;

            // Load settings via manager
            var settings = banInstanceManager.LoadProxyCheckSettings();

            // General API Information and Overall Enablement
            cb_enableProxyCheck.Checked = settings.Enabled;
            textBox_ProxyAPIKey.Text = settings.ApiKey;
            num_proxyCacheDays.Value = settings.CacheTime;

            // Proxy Checkboxes
            checkBox_proxyNone.Checked = (settings.ProxyAction == 0);
            checkBox_proxyKick.Checked = (settings.ProxyAction == 1);
            checkBox_proxyBlock.Checked = (settings.ProxyAction == 2);

            // VPN Checkboxes
            checkBox_vpnNone.Checked = (settings.VpnAction == 0);
            checkBox_vpnKick.Checked = (settings.VpnAction == 1);
            checkBox_vpnBlock.Checked = (settings.VpnAction == 2);

            // TOR Checkboxes
            checkBox_torNone.Checked = (settings.TorAction == 0);
            checkBox_torKick.Checked = (settings.TorAction == 1);
            checkBox_torBlock.Checked = (settings.TorAction == 2);

            // GEO Checkboxes
            checkBox_GeoOff.Checked = (settings.GeoMode == 0);
            checkBox_GeoBlock.Checked = (settings.GeoMode == 1);
            checkBox_GeoAllow.Checked = (settings.GeoMode == 2);

            // Proxy Checking Service Providers
            cb_serviceProxyCheckIO.Checked = (settings.ServiceProvider == 1);
            cb_serviceIP2LocationIO.Checked = (settings.ServiceProvider == 2);
        }

        private void ProxyCheck_SaveSettings(object sender, EventArgs e)
        {
            if (theInstance == null)
                return;

            // Gather settings from UI
            var settings = new ProxyCheckSettings(
                Enabled: cb_enableProxyCheck.Checked,
                ApiKey: textBox_ProxyAPIKey.Text,
                CacheTime: num_proxyCacheDays.Value,
                ProxyAction: checkBox_proxyBlock.Checked ? 2 : (checkBox_proxyKick.Checked ? 1 : 0),
                VpnAction: checkBox_vpnBlock.Checked ? 2 : (checkBox_vpnKick.Checked ? 1 : 0),
                TorAction: checkBox_torBlock.Checked ? 2 : (checkBox_torKick.Checked ? 1 : 0),
                GeoMode: checkBox_GeoBlock.Checked ? 1 : (checkBox_GeoAllow.Checked ? 2 : 0),
                ServiceProvider: cb_serviceProxyCheckIO.Checked ? 1 : (cb_serviceIP2LocationIO.Checked ? 2 : 0)
            );

            // Save via manager
            var result = banInstanceManager.SaveProxyCheckSettings(settings);

            if (result.Success)
            {
                MessageBox.Show("Proxy check settings saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error saving proxy check settings: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Handle adding a blocked country
        /// </summary>
        private void ProxyCheck_AddCountry_Click(object sender, EventArgs e)
        {
            if (instanceBans == null)
                return;

            string countryCode = textBox_countryCode.Text.Trim().ToUpper();
            string countryName = textBox_countryName.Text.Trim();

            // Add via manager
            var result = banInstanceManager.AddBlockedCountry(countryCode, countryName);

            if (result.Success)
            {
                // Add to DataGridView
                dgProxyCountryBlockList.Rows.Add(result.RecordID, countryCode, countryName);

                // Clear input fields
                textBox_countryCode.Text = string.Empty;
                textBox_countryName.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(result.Message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handle double-click on blocked countries grid to delete
        /// </summary>
        private void ProxyCheck_CountryGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || instanceBans == null)
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

            // Remove via manager
            var result = banInstanceManager.RemoveBlockedCountry(recordId);

            if (result.Success)
            {
                // Remove from DataGridView
                dgProxyCountryBlockList.Rows.RemoveAt(e.RowIndex);
            }
            else
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load blocked countries into DataGridView
        /// </summary>
        private void LoadProxyBlockedCountries()
        {
            if (instanceBans == null)
                return;

            dgProxyCountryBlockList.Rows.Clear();

            var countries = banInstanceManager.LoadBlockedCountries();

            foreach (var country in countries.OrderBy(c => c.CountryName))
            {
                // Add RecordID as first column (typically hidden in designer)
                dgProxyCountryBlockList.Rows.Add(country.RecordID, country.CountryCode, country.CountryName);
            }

            AppDebug.Log("tabBans", $"Loaded {countries.Count} blocked countries");
        }

        /// <summary>
        /// Test the selected proxy check service with a sample IP address
        /// </summary>
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
                string serviceName = cb_serviceProxyCheckIO.Checked ? "ProxyCheck.io" : "IP2Location.io";

                // Test with a known IP address (Google DNS - should not be a proxy/VPN)
                var testIP = IPAddress.Parse("8.8.8.8");

                AppDebug.Log("tabBans", $"Testing {serviceName} with IP: {testIP}");

                // Test via manager
                var (success, result, errorMessage) = await banInstanceManager.TestProxyService(apiKey, serviceProvider, testIP);

                // Display results
                if (success && result != null)
                {
                    var resultMessage = new StringBuilder();
                    resultMessage.AppendLine($"Service: {serviceName}");
                    resultMessage.AppendLine($"Test IP: {testIP}");
                    resultMessage.AppendLine($"");
                    resultMessage.AppendLine($"Results:");
                    resultMessage.AppendLine($"  • VPN: {(result.IsVpn ? "Yes" : "No")}");
                    resultMessage.AppendLine($"  • Proxy: {(result.IsProxy ? "Yes" : "No")}");
                    resultMessage.AppendLine($"  • Tor: {(result.IsTor ? "Yes" : "No")}");
                    resultMessage.AppendLine($"  • Risk Score: {result.RiskScore}/100");

                    if (!string.IsNullOrEmpty(result.CountryName))
                    {
                        resultMessage.AppendLine($"");
                        resultMessage.AppendLine($"Location:");
                        resultMessage.AppendLine($"  • Country: {result.CountryName} ({result.CountryCode})");
                        if (!string.IsNullOrEmpty(result.Region))
                            resultMessage.AppendLine($"  • Region: {result.Region}");
                        if (!string.IsNullOrEmpty(result.City))
                            resultMessage.AppendLine($"  • City: {result.City}");
                    }

                    if (!string.IsNullOrEmpty(result.Provider))
                    {
                        resultMessage.AppendLine($"");
                        resultMessage.AppendLine($"Provider: {result.Provider}");
                    }

                    MessageBox.Show(resultMessage.ToString(),
                        "Service Test Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Service: {serviceName}\nError: {errorMessage}",
                        "Service Test Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the test:\n\n{ex.Message}",
                    "Test Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                AppDebug.Log("tabBans", $"Proxy service test error: {ex}");
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

        // ================================================================================
        // NETLIMITER CHECKING FUNCTIONALITY
        // ================================================================================

        /// <summary>
        /// Load and display NetLimiter settings from instance
        /// </summary>
        private void NetLimiter_LoadSettings(object sender, EventArgs e)
        {
            if (theInstance == null)
                return;

            // Load settings via manager
            var settings = banInstanceManager.LoadNetLimiterSettings();

            // NetLimiter Integration Settings
            checkBox_EnableNetLimiter.Checked = settings.Enabled;
            textBox_NetLimiterHost.Text = settings.Host;
            num_NetLimiterPort.Value = settings.Port;
            textBox_NetLimiterUsername.Text = settings.Username;
            textBox_NetLimiterPassword.Text = settings.Password;
            comboBox_NetLimiterFilterName.Text = settings.FilterName;
            checkBox_NetLimiterEnableConLimit.Checked = settings.EnableConLimit;
            num_NetLimiterConThreshold.Value = settings.ConThreshold;
        }

        /// <summary>
        /// Save NetLimiter settings to instance and database
        /// </summary>
        private void NetLimiter_SaveSettings(object sender, EventArgs e)
        {
            if (theInstance == null)
                return;

            // Gather settings from UI
            var settings = new NetLimiterSettings(
                Enabled: checkBox_EnableNetLimiter.Checked,
                Host: textBox_NetLimiterHost.Text.Trim(),
                Port: (int)num_NetLimiterPort.Value,
                Username: textBox_NetLimiterUsername.Text.Trim(),
                Password: textBox_NetLimiterPassword.Text,
                FilterName: comboBox_NetLimiterFilterName.Text.Trim(),
                EnableConLimit: checkBox_NetLimiterEnableConLimit.Checked,
                ConThreshold: num_NetLimiterConThreshold.Value
            );

            // Save via manager
            var result = banInstanceManager.SaveNetLimiterSettings(settings);

            if (result.Success)
            {
                MessageBox.Show("NetLimiter settings saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error saving NetLimiter settings: {result.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async void NetLimiter_RefreshFilters(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => NetLimiter_RefreshFilters(sender, e)));
                return;
            }

            // Start Process via manager
            var startResult = NetLimiter_StartBridge();
            if (!startResult)
            {
                MessageBox.Show("Failed to start/access the NetLimiter Bridge process. Please ensure NetLimiter is installed and try again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Get filters via manager
            var (success, filters, errorMessage) = await banInstanceManager.GetNetLimiterFilters();

            if (!success || filters.Count == 0)
            {
                MessageBox.Show("No filters were retrieved from NetLimiter. Please ensure filters are configured in NetLimiter.",
                    "No Filters Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            comboBox_NetLimiterFilterName.Items.Clear();
            foreach (var filter in filters)
            {
                comboBox_NetLimiterFilterName.Items.Add(filter);
            }
        }

        public bool NetLimiter_StartBridge()
        {
            if (theInstance == null)
                return false;

            // Start via manager
            var result = banInstanceManager.StartNetLimiterBridge(
                theInstance.netLimiterHost,
                theInstance.netLimiterPort,
                theInstance.netLimiterUsername,
                theInstance.netLimiterPassword
            );

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}