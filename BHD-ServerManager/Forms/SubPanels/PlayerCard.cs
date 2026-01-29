using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Services;
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
        private playerObject Player { get; set; } = new playerObject();
        private int SlotNumber = 0;
        private new ContextMenuStrip ContextMenu;

        private playerObject? LastPlayerData = null;
        private bool LastVisible = true;

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

        // ================================================================================
        // CONTEXT MENU ITEM CREATORS (Refactored to use playerInstanceManager)
        // ================================================================================

        private ToolStripMenuItem CreateArmMenuItem()
        {
            var command = new ToolStripMenuItem("Arm Player");
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.ArmPlayer(Player.PlayerSlot, Player.PlayerName);
                
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
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.DisarmPlayer(Player.PlayerSlot, Player.PlayerName);
                
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
                slapItem.Click += (sender, e) =>
                {
                    var result = playerInstanceManager.WarnPlayer(
                        Player.PlayerSlot, 
                        Player.PlayerName, 
                        slapMessage.SlapMessageText
                    );
                    
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
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.KickPlayer(Player.PlayerSlot, Player.PlayerName);
                
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
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.KillPlayer(Player.PlayerSlot, Player.PlayerName);
                
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
            banByName.Click += (sender, e) =>
            {
                var result = playerInstanceManager.BanPlayerByName(Player.PlayerName, Player.PlayerSlot);
                
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

                    var result = await playerInstanceManager.BanPlayerByIPAsync(
                        ipAddress, 
                        Player.PlayerName, 
                        Player.PlayerSlot
                    );
                    
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

                    var result = await playerInstanceManager.BanPlayerByBothAsync(
                        Player.PlayerName, 
                        ipAddress, 
                        Player.PlayerSlot
                    );
                    
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
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.ToggleGodMode(
                    Player.PlayerSlot, 
                    Player.PlayerName, 
                    !IsGod
                );

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
            command.Click += (sender, e) =>
            {
                var result = playerInstanceManager.SwitchPlayerTeam(
                    Player.PlayerSlot, 
                    Player.PlayerName, 
                    Player.PlayerTeam
                );
                
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
        /// Check if player's IP is flagged as proxy/VPN/TOR
        /// </summary>
        private async Task<bool> IsProxyDetectedAsync(string ipAddress)
        {
            try
            {
                // Check if proxy detection is enabled
                if (!TheInstance.proxyCheckEnabled || !ProxyCheckManager.IsInitialized)
                    return false;

                if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
                    return false;

                var result = await ProxyCheckManager.CheckIPAsync(ip);
                
                if (result.Success)
                {
                    // Check if any proxy/VPN/TOR flag is set
                    return result.IsProxy || result.IsVpn || result.IsTor;
                }

                return false;
            }
            catch (Exception ex)
            {
                AppDebug.Log("PlayerCard", $"Error checking proxy status: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Update player icon based on team and proxy status
        /// </summary>
        private async Task UpdatePlayerIconAsync(string ipAddress, int team)
        {
            bool isProxyDetected = await IsProxyDetectedAsync(ipAddress);

            if (isProxyDetected)
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

        // ================================================================================
        // PLAYER CARD UI UPDATE METHODS
        // ================================================================================

        public async void UpdateStatus(playerObject playerInfo)
        {
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
            
            ContextMenu.Items[0].Text = decodedPlayerName;
            ContextMenu.Items[1].Text = $"Ping: {Player.PlayerPing} ms";

            player_Tooltip.SetToolTip(this, $"Ping: {Player.PlayerPing} ms");
            playerContextMenuIcon.Visible = true;
        }

        public void ResetStatus()
        {
            Player = new playerObject
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
            
            playerContextMenuIcon.Visible = false;
        }

        public void ToggleSlot(bool visible = true)
        {
            if (Visible == visible)
                return;

            ResetStatus();
            Visible = visible;
        }

        public void UpdateCard(playerObject? playerInfo, bool visible)
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

        private static bool IsSamePlayer(playerObject? a, playerObject? b)
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