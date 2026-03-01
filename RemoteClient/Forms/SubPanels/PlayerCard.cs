using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using RemoteClient.Classes.Helpers;
using RemoteClient.Core;
using RemoteClient.Services;
using System.Diagnostics;
using System.Net;
using System.Text;
using UserControl = System.Windows.Forms.UserControl;

namespace BHD_ServerManager.Classes.PlayerManagementClasses
{
    public partial class PlayerCard : UserControl
    {
        private const int GodModeHealth = 9999;
        private const int NormalHealth = 100;
        private const string EncodingName = "Windows-1252";

        private static chatInstance ChatInstance = CommonCore.instanceChat!;
        private static theInstance TheInstance = CommonCore.theInstance!;
        private PlayerObject Player { get; set; } = new PlayerObject();
        private int SlotNumber = 0;
        private new ContextMenuStrip ContextMenu;

        private PlayerObject? LastPlayerData = null;
        private bool LastVisible = true;
        private string? _lastCountryCode = null;

        private bool IsGod = false;

        public PlayerCard(int slotNum)
        {
            InitializeComponent();

            SlotNumber = slotNum;
            label_dataSlotNum.Text = slotNum.ToString();

            ContextMenu = new ContextMenuStrip();
            ContextMenu.Opening += ContextMenu_Opening;
            playerContextMenuIcon.ContextMenuStrip = ContextMenu;
            playerContextMenuIcon.Click -= PlayerContextMenuIcon_Click;
            playerContextMenuIcon.Click += PlayerContextMenuIcon_Click;

            label_dataIPinfo.Cursor = Cursors.Hand;
            label_dataIPinfo.Click += Label_dataIPinfo_Click;

            BuildContextMenu();

            this.ResetStatus();
        }

        private void ContextMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Refresh dynamic content
            ContextMenu.Items[0].Text = Player.PlayerName ?? "Unknown";
            ContextMenu.Items[1].Text = $"Ping: {Player.PlayerPing} ms";

            // Refresh slap messages
            if (ContextMenu.Items[5] is ToolStripMenuItem warningItem)
            {
                AddSlapsToWarningSubMenu(warningItem);
            }

            // Update god mode text
            if (ContextMenu.Items[11] is ToolStripMenuItem godModeItem)
            {
                godModeItem.Text = IsGod ? "Disable God Mode" : "Enable God Mode";
            }
        }

        private void BuildContextMenu()
        {
            var playerName = new ToolStripMenuItem(Player.PlayerName ?? "Unknown");
            var playerPing = new ToolStripMenuItem($"Ping: {Player.PlayerPing} ms");
            var armCommand = CreateArmMenuItem();
            var disarmCommand = CreateDisarmMenuItem();
            var warningCommand = CreateWarningMenuItem();
            var kickCommand = CreateKickMenuItem();
            var killCommand = CreateKillMenuItem();
            var banCommand = CreateBanMenuItem();
            var godModeCommand = CreateGodModeMenuItem();
            var switchTeamCommand = CreateSwitchTeamMenuItem();

            ContextMenu.Items.Add(playerName);
            ContextMenu.Items.Add(playerPing);
            ContextMenu.Items.Add(new ToolStripSeparator());
            ContextMenu.Items.Add(armCommand);
            ContextMenu.Items.Add(disarmCommand);
            ContextMenu.Items.Add(warningCommand);
            ContextMenu.Items.Add(kickCommand);
            ContextMenu.Items.Add(new ToolStripSeparator());
            ContextMenu.Items.Add(killCommand);
            ContextMenu.Items.Add(banCommand);
            ContextMenu.Items.Add(new ToolStripSeparator());
            ContextMenu.Items.Add(godModeCommand);
            ContextMenu.Items.Add(switchTeamCommand);
        }

        private void PlayerContextMenuIcon_Click(object? sender, EventArgs e)
        {
            ContextMenu.Show(this, new Point(playerContextMenuIcon.Location.X, playerContextMenuIcon.Location.Y));
        }

        private void Label_dataIPinfo_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(label_dataIPinfo.Text))
            {
                Clipboard.SetText(label_dataIPinfo.Text);
            }
        }

        // ================================================================================
        // CONTEXT MENU ITEM CREATORS (Refactored to use playerInstanceManager)
        // ================================================================================

        private ToolStripMenuItem CreateArmMenuItem()
        {
            var command = new ToolStripMenuItem("Arm Player");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.ArmPlayerAsync(Player.PlayerSlot, Player.PlayerName);
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        private ToolStripMenuItem CreateDisarmMenuItem()
        {
            var command = new ToolStripMenuItem("Disarm Player");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.DisarmPlayerAsync(Player.PlayerSlot, Player.PlayerName);
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        private ToolStripMenuItem CreateWarningMenuItem()
        {
            var command = new ToolStripMenuItem("Warn Player");
            AddSlapsToWarningSubMenu(command);
            return command;
        }

        private void AddSlapsToWarningSubMenu(ToolStripMenuItem command)
        {
            command.DropDownItems.Clear();
            
            foreach (var slapMessage in ChatInstance.SlapMessages)
            {
                var slapItem = new ToolStripMenuItem(slapMessage.SlapMessageText);
                slapItem.Click += async (sender, e) =>
                {

                    var result = await ApiCore.ApiClient!.Player.WarnPlayerAsync(Player.PlayerSlot, Player.PlayerName, slapMessage.SlapMessageText);
                   
                    MessageBox.Show(
                        result.Success 
                            ? $"Player {Player.PlayerName} has been warned: {slapMessage.SlapMessageText}"
                            : result.Message,
                        result.Success ? "Player Action" : "Error",
                        MessageBoxButtons.OK,
                        result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                    );
                };
                command.DropDownItems.Add(slapItem);
            }
        }

        private ToolStripMenuItem CreateKickMenuItem()
        {
            var command = new ToolStripMenuItem("Kick Player");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.KickPlayerAsync(Player.PlayerSlot, Player.PlayerName);
                               
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        private ToolStripMenuItem CreateKillMenuItem()
        {
            var command = new ToolStripMenuItem("Kill Player");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.KillPlayerAsync(Player.PlayerSlot, Player.PlayerName);
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        private ToolStripMenuItem CreateBanMenuItem()
        {
            var command = new ToolStripMenuItem("Ban Player");
            var banByName = new ToolStripMenuItem("Ban Name");
            var banByIP = new ToolStripMenuItem("Ban IP Address");
            var banByNameAndIP = new ToolStripMenuItem("Ban Both");

            command.DropDownItems.Add(banByName);
            command.DropDownItems.Add(banByIP);
            command.DropDownItems.Add(banByNameAndIP);

            // Ban by Name
            banByName.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(Player.PlayerSlot, Player.PlayerName, string.Empty, false);
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };

            // Ban by IP (Async)
            banByIP.Click += async (sender, e) =>
            {
                try
                {
                    if (!IPAddress.TryParse(Player.PlayerIPAddress, out IPAddress? ipAddress))
                    {
                        MessageBox.Show(
                            $"Invalid IP address: {Player.PlayerIPAddress}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }
                    
                    var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(Player.PlayerSlot, string.Empty, Player.PlayerIPAddress, true);
                    
                    MessageBox.Show(
                        result.Message,
                        result.Success ? "Player Action" : "Error",
                        MessageBoxButtons.OK,
                        result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Unexpected error: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    AppDebug.Log("PlayerCard", $"Error in banByIP: {ex}");
                }
            };

            // Ban by Both (Async)
            banByNameAndIP.Click += async (sender, e) =>
            {
                try
                {
                    if (!IPAddress.TryParse(Player.PlayerIPAddress, out IPAddress? ipAddress))
                    {
                        MessageBox.Show(
                            $"Invalid IP address: {Player.PlayerIPAddress}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }
                    var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(Player.PlayerSlot, Player.PlayerName, Player.PlayerIPAddress, true);
                    
                    MessageBox.Show(
                        result.Message,
                        result.Success ? "Player Action" : "Error",
                        MessageBoxButtons.OK,
                        result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Unexpected error: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    AppDebug.Log("PlayerCard", $"Error in banByNameAndIP: {ex}");
                }
            };

            return command;
        }

        private ToolStripMenuItem CreateGodModeMenuItem()
        {
            var command = new ToolStripMenuItem(IsGod ? "Disable God Mode" : "Enable God Mode");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.ToggleGodPlayerAsync(Player.PlayerSlot, Player.PlayerName);

                if (result.Success)
                {
                    IsGod = !IsGod;
                    command.Text = IsGod ? "Disable God Mode" : "Enable God Mode";
                }
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        private ToolStripMenuItem CreateSwitchTeamMenuItem()
        {
            var command = new ToolStripMenuItem("Switch Team");
            command.Click += async (sender, e) =>
            {
                var result = await ApiCore.ApiClient!.Player.SwitchTeamPlayerAsync(Player.PlayerSlot, Player.PlayerName, Player.PlayerTeam);
                
                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return command;
        }

        // ================================================================================
        // PROXY/VPN DETECTION METHODS
        // ================================================================================

        /// <summary>
        /// Update player icon based on team and proxy status
        /// </summary>
        private async Task UpdatePlayerIconAsync(string ipAddress, int team)
        {

            if (Player.IsProxyDetected)
            {
                // Show warning icon for proxy/VPN/TOR
                playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonCircleExclamation;
                playerTeamIcon.IconColor = Color.Yellow;
            }
            else
            {
                // Show default team icon
                playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
                playerTeamIcon.IconColor = team switch
                {
                    1 => Color.Blue,
                    2 => Color.Red,
                    _ => Color.Black
                };
            }
        }

        /// <summary>
        /// Fetch and update country code and flag for new player
        /// </summary>
        private async Task UpdateCountryDataAsync(string ipAddress, string? countryCode)
        {
            // Only update flag if country code changed
            if (countryCode != _lastCountryCode)
            {
                _lastCountryCode = countryCode;
                await UpdateCountryFlagAsync(countryCode);
            }
        }

        /// <summary>
        /// Update country flag based on ISO code
        /// </summary>
        private async Task UpdateCountryFlagAsync(string? countryIsoCode)
        {
            try
            {
                if (string.IsNullOrEmpty(countryIsoCode))
                {
                    if (playerFlagIcon.Image != null)
                    {
                        playerFlagIcon.Image.Dispose();
                        playerFlagIcon.Image = null;
                    }
                    playerFlagIcon.Visible = false;
                    return;
                }

                var flag = await FlagHelper.GetFlagAsync(countryIsoCode);

                // Only assign if flag is valid (not null)
                if (flag != null)
                {
                    if (playerFlagIcon.Image != null)
                    {
                        playerFlagIcon.Image.Dispose();
                        playerFlagIcon.Image = null;
                    }
                    playerFlagIcon.Image = flag;
                    playerFlagIcon.Visible = true;
                    player_Tooltip.SetToolTip(playerFlagIcon, $"{countryIsoCode.ToUpper()}");
                }
                else
                {
                    if (playerFlagIcon.Image != null)
                    {
                        playerFlagIcon.Image.Dispose();
                        playerFlagIcon.Image = null;
                    }
                    playerFlagIcon.Visible = false;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("PlayerCard", $"Error loading flag for {countryIsoCode}: {ex.Message}");
                if (playerFlagIcon.Image != null)
                {
                    playerFlagIcon.Image.Dispose();
                    playerFlagIcon.Image = null;
                }
                playerFlagIcon.Visible = false;
            }
        }

        // ================================================================================
        // PLAYER CARD UI UPDATE METHODS
        // ================================================================================

        public async void UpdateStatus(PlayerObject playerInfo)
        {
            bool isNewPlayer = LastPlayerData == null || 
                               LastPlayerData.PlayerName != playerInfo.PlayerName || 
                               LastPlayerData.PlayerIPAddress != playerInfo.PlayerIPAddress;

            Player = playerInfo;

            // Decode Base64 and interpret as Windows-1252
            byte[] decodedBytes = Convert.FromBase64String(Player.PlayerNameBase64 ?? "");
            string decodedPlayerName = Encoding.GetEncoding(EncodingName).GetString(decodedBytes);

            label_dataPlayerNameRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label_dataPlayerNameRole.UseCompatibleTextRendering = true;
            label_dataPlayerNameRole.Text = decodedPlayerName;

            label_dataIPinfo.Text = Player.PlayerIPAddress;
            
            // Update icon based on proxy detection and team
            await UpdatePlayerIconAsync(Player.PlayerIPAddress, Player.PlayerTeam);

            // Only fetch country data for new players
            if (isNewPlayer)
            {
                await UpdateCountryDataAsync(Player.PlayerIPAddress, Player.CountryCode);
            }
            
            ContextMenu.Items[0].Text = decodedPlayerName;
            ContextMenu.Items[1].Text = $"Ping: {Player.PlayerPing} ms";

            player_Tooltip.SetToolTip(this, $"Ping: {Player.PlayerPing} ms");
            playerContextMenuIcon.Visible = true;
        }

        public void ResetStatus()
        {
            Player = new PlayerObject
            {
                PlayerSlot = SlotNumber,
                PlayerName = "Slot Empty",
                PlayerIPAddress = ""
            };
            LastPlayerData = Player;

            label_dataIPinfo.Text = Player.PlayerIPAddress;
            label_dataPlayerNameRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label_dataPlayerNameRole.UseCompatibleTextRendering = true;
            label_dataPlayerNameRole.Text = Player.PlayerName;
            label_dataSlotNum.Text = Player.PlayerSlot.ToString();
            
            // Reset to default icon
            playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
            playerTeamIcon.IconColor = Color.Black;
            
            // Reset flag and country code
            playerFlagIcon.Image?.Dispose();
            playerFlagIcon.Image = null;
            playerFlagIcon.Visible = false;
            _lastCountryCode = null;
            
            playerContextMenuIcon.Visible = false;
        }

        public void ToggleSlot(bool visible = true)
        {
            if (Visible == visible)
                return;

            ResetStatus();
            Visible = visible;
        }

        public void UpdateCard(PlayerObject? playerInfo, bool visible)
        {
            // Only update visibility if it changes
            if (Visible != visible)
            {
                Visible = visible;
                LastVisible = visible;
            }

            // Only update player data if the reference or key properties have changed
            if (playerInfo != null)
            {
                if (!ReferenceEquals(playerInfo, LastPlayerData) || !IsSamePlayer(playerInfo, LastPlayerData))
                {
                    UpdateStatus(playerInfo);
                    LastPlayerData = playerInfo;
                }
            }
            else if (LastPlayerData != null && playerInfo == null)
            {
                ResetStatus();
            }
        }

        private static bool IsSamePlayer(PlayerObject? a, PlayerObject? b)
        {
            if (a == null || b == null) return false;
            return a.PlayerSlot == b.PlayerSlot &&
                   a.PlayerName == b.PlayerName &&
                   a.PlayerIPAddress == b.PlayerIPAddress &&
                   a.PlayerTeam == b.PlayerTeam &&
                   a.PlayerPing == b.PlayerPing;
        }
    }
}