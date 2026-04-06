namespace RemoteClient.Forms.SubPanels;

using Core;
using Classes.Helpers;
using HawkSyncShared.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.Instances;
using System.Net;
using System.Text;
using System.ComponentModel;

public partial class PlayerCardV2 : UserControl
{
    private int _playerSlot;

    private PlayerObject                playerData      { get; set; } = new PlayerObject();
    private string                      CountryCode     { get; set; } = string.Empty;

    private chatInstance?               chatInstance    => CommonCore.instanceChat;
    private playerInstance?             playerInstance  => CommonCore.instancePlayers;
    private theInstance?                theInstance     => CommonCore.theInstance;
    
    private static bool IsDesignTime =>
                LicenseManager.UsageMode == LicenseUsageMode.Designtime || System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");
    
    public PlayerCardV2(int slotNum)
    {
        _playerSlot = slotNum;
        
        InitializeComponent();
        
        if (IsDesignTime)
            return;
        
        BuildContextMenu(contextMenu);
        
        contextMenu.Opening += ContextMenu_Opening;
        playerContextMenuIcon.IconSize = 32;
        playerContextMenuIcon.ContextMenuStrip = contextMenu;
        playerContextMenuIcon.Click -= ContextMenu_IconClick;
        playerContextMenuIcon.Click += ContextMenu_IconClick;
        
        label_dataIPinfo.Cursor = Cursors.Hand;
        label_dataIPinfo.Click += CardPlayerIP_Click;
        
        ResetCard();
        
    }

    public void StartTicker()
    {
        CommonCore.Ticker!.Start($"Ticker_{Name}", 1000, PlayerCardTicker);
    }
    
    private void RunSafe(Func<Task> action)
    {
        _ = RunSafeInternal(action);
    }

    private async Task RunSafeInternal(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            AppDebug.Log("Unhandled async UI action", AppDebug.LogLevel.Error, ex);
            MessageBox.Show(
                $"Unexpected error: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
    
    private void PlayerCardTicker()
    {
        try
        {
            // Read non-UI state on the ticker thread (safe)
            bool shouldBeVisible = CommonCore.theInstance!.instanceStatus != InstanceStatus.OFFLINE &&
                                   _playerSlot <= CommonCore.theInstance.gameMaxSlots;

            if (!playerInstance!.PlayerList.ContainsKey(_playerSlot))
            {
                BeginInvoke(() =>
                {
                    Visible = shouldBeVisible;
                    ResetCard();
                });
                return;
            }

            PlayerObject _playerData = playerInstance.PlayerList[_playerSlot];
            if (DateTime.Now - _playerData.PlayerLastSeen > TimeSpan.FromSeconds(5))
            {
                BeginInvoke(() =>
                {
                    Visible = shouldBeVisible;
                    ResetCard();
                });
                return;
            }

            // Update Player Data (plain object assignment, not UI — safe on ticker thread)
            playerData = _playerData;

            // Marshal all UI updates to the UI thread
            BeginInvoke(() =>
            {
                Visible = shouldBeVisible;
                RunSafe(UpdatePlayerCard);
            });
        }
        catch (Exception ex)
        {
            AppDebug.Log("Error in PlayerCardTicker", AppDebug.LogLevel.Error, ex);
        }
    }
    
    private async Task UpdatePlayerCard()
    {
        // Decode Base64 and interpret as Windows-1252
        byte[] decodedBytes = Convert.FromBase64String(playerData.PlayerNameBase64);
        string decodedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(decodedBytes);
        
        label_dataPlayerNameRole.UseCompatibleTextRendering = true;
        label_dataPlayerNameRole.Text = decodedPlayerName;

        label_dataIPinfo.Text = playerData.PlayerIPAddress;
            
        // Update icon based on proxy detection and team
        UpdatePlayerIconAsync(playerData.PlayerTeam);
        await UpdateCountryDataAsync();
            
        contextMenu.Items[0].Text = decodedPlayerName;
        contextMenu.Items[1].Text = $"Ping: {playerData.PlayerPing} ms";

        player_Tooltip.SetToolTip(this, $"Ping: {playerData.PlayerPing} ms");
        playerContextMenuIcon.Visible = true;
    }

    private void BuildContextMenu(ContextMenuStrip cardMenu)
    {
        cardMenu.Items.Clear();
        
        var playerName = new ToolStripMenuItem(playerData.PlayerName);
        var playerPing = new ToolStripMenuItem($"Ping: {playerData.PlayerPing} ms");
        var armCommand = CreateArmMenuItem();
        var disarmCommand = CreateDisarmMenuItem();
        var warningCommand = CreateWarningMenuItem();
        var kickCommand = CreateKickMenuItem();
        var killCommand = CreateKillMenuItem();
        var banCommand = CreateBanMenuItem();
        var godModeCommand = CreateGodModeMenuItem();
        var switchTeamCommand = CreateSwitchTeamMenuItem();

        cardMenu.Items.Add(playerName);
        cardMenu.Items.Add(playerPing);
        cardMenu.Items.Add(new ToolStripSeparator());
        cardMenu.Items.Add(armCommand);
        cardMenu.Items.Add(disarmCommand);
        cardMenu.Items.Add(warningCommand);
        cardMenu.Items.Add(kickCommand);
        cardMenu.Items.Add(new ToolStripSeparator());
        cardMenu.Items.Add(killCommand);
        cardMenu.Items.Add(banCommand);
        cardMenu.Items.Add(new ToolStripSeparator());
        cardMenu.Items.Add(godModeCommand);
        cardMenu.Items.Add(switchTeamCommand);
    }
    
    private void ResetCard()
    {
        playerData = new PlayerObject
        {
            PlayerSlot = _playerSlot,
            PlayerName = "Slot Empty",
            PlayerIPAddress = ""
        };

        label_dataPlayerNameRole.UseCompatibleTextRendering = true;
        label_dataPlayerNameRole.Text = playerData.PlayerName;
        label_dataIPinfo.Text = playerData.PlayerIPAddress;
        label_dataSlotNum.Text = _playerSlot.ToString();
            
        // Reset to default icon
        playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
        playerTeamIcon.IconColor = Color.Black;
            
        // Reset flag and country code
        try
        {
            if (!PlayerFlagIcon.IsDisposed)
            {
                PlayerFlagIcon.Visible = false; // Hide the Image
                var img = PlayerFlagIcon.Image; // Grab the Object
                PlayerFlagIcon.Image = null; // Set Image to Null
                img?.Dispose(); // Dispose the Object
            }
        }
        catch (Exception ex)
        {
            AppDebug.Log("Failed to Dispose of Player Flag", AppDebug.LogLevel.Error, ex);
        }
        CountryCode = string.Empty;
        
        BuildContextMenu(contextMenu);
        
        playerContextMenuIcon.Visible = false;        
    }

    public void ToggleVisibility(bool toggle)
    {
        if (Visible == toggle)
            return;

        ResetCard();
        Visible = toggle;
    }
    
    // ================================================================================
    // CONTEXT MENU ITEM CREATORS (Refactored to use playerInstanceManager)
    // ================================================================================

    private ToolStripMenuItem CreateArmMenuItem()
    {
        var command = new ToolStripMenuItem("Arm Player");
        command.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.ArmPlayerAsync(playerData.PlayerSlot, playerData.PlayerName);
                
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
        command.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.DisarmPlayerAsync(playerData.PlayerSlot, playerData.PlayerName);
                
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
            
        foreach (var slapMessage in chatInstance!.SlapMessages)
        {
            var slapItem = new ToolStripMenuItem(slapMessage.SlapMessageText);
            slapItem.Click += async (_, _) =>
            {

                var result = await ApiCore.ApiClient!.Player.WarnPlayerAsync(playerData.PlayerSlot, playerData.PlayerName, slapMessage.SlapMessageText);
                   
                MessageBox.Show(
                    result.Success 
                        ? $"Player {playerData.PlayerName} has been warned: {slapMessage.SlapMessageText}"
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
        command.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.KickPlayerAsync(playerData.PlayerSlot, playerData.PlayerName);
                               
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
        command.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.KillPlayerAsync(playerData.PlayerSlot, playerData.PlayerName);
                
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
        banByName.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(playerData.PlayerSlot, playerData.PlayerName, string.Empty, false);
            
            MessageBox.Show(
                result.Message,
                result.Success ? "Player Action" : "Error",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );
        };

        // Ban by IP (Async)
        banByIP.Click += async (_, _) =>
        {
            try
            {
                if (!IPAddress.TryParse(playerData.PlayerIPAddress, out _))
                {
                    MessageBox.Show(
                        $"Invalid IP address: {playerData.PlayerIPAddress}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                
                var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(playerData.PlayerSlot, string.Empty, playerData.PlayerIPAddress, true);
                
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
                AppDebug.Log($"Error in banByIP", AppDebug.LogLevel.Error, ex);
            }
        };

        // Ban by Both (Async)
        banByNameAndIP.Click += async (_, _) =>
        {
            try
            {
                if (!IPAddress.TryParse(playerData.PlayerIPAddress, out _))
                {
                    MessageBox.Show(
                        $"Invalid IP address: {playerData.PlayerIPAddress}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                var result = await ApiCore.ApiClient!.Player.BanPlayerAsync(playerData.PlayerSlot, playerData.PlayerName, playerData.PlayerIPAddress, true);
                
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
                AppDebug.Log($"Error in banByNameAndIP", AppDebug.LogLevel.Error, ex);
            }
        };

        return command;
    }

    private ToolStripMenuItem CreateGodModeMenuItem()
    {
        var command = new ToolStripMenuItem(playerData.IsGod ? "Disable God Mode" : "Enable God Mode");
        command.Click += async (_, _) =>
        {
            var result = await ApiCore.ApiClient!.Player.ToggleGodPlayerAsync(playerData.PlayerSlot, playerData.PlayerName);

            if (result.Success)
            {
                playerData.IsGod = !playerData.IsGod;
                command.Text = playerData.IsGod ? "Disable God Mode" : "Enable God Mode";
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
        var mapInstance = CommonCore.instanceMaps!;

        // Check if already queued for team switch
        var existingSwitch = playerInstance!.PlayerChangeTeamList.FirstOrDefault(p => p.slotNum == playerData.PlayerSlot);

        // Check game type - disable for DM (0) and KOTH (4)
        if (mapInstance.CurrentGameType == 0 || mapInstance.CurrentGameType == 4)
        {
            var command = new ToolStripMenuItem("Switch Team")
            {
                Enabled = false,
                ToolTipText = "Team switching not available in Deathmatch or King of the Hill"
            };
            return command;
        }

        // If 4-team support disabled or next map is not a 4-team map, use simple toggle
        if (!theInstance!.gameEnableFourTeams || !mapInstance.IsNextMap4Team)
        {
            var command = new ToolStripMenuItem(existingSwitch != null ? "Cancel Team Switch" : "Switch Team");
            command.Click += async (_, _) =>
            {
                var result = await ApiCore.ApiClient!.Player.SwitchTeamPlayerAsync(
                    playerData.PlayerSlot, 
                    playerData.PlayerName, 
                    playerData.PlayerTeam, 
                    playerData.PlayerTeam == 1 ? 2 : 1 // Toggle between Team 1 and Team 2
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

        // Next map is 4-team with 4-team support enabled - show submenu
        var switchTeamMenu = new ToolStripMenuItem(existingSwitch != null ? "Cancel Team Switch" : "Switch Team");

        // If already queued, clicking cancels the switch
        if (existingSwitch != null)
        {
            switchTeamMenu.Click += async (_, _) =>
            {
                var result = await ApiCore.ApiClient!.Player.SwitchTeamPlayerAsync(
                    playerData.PlayerSlot, 
                    playerData.PlayerName, 
                    playerData.PlayerTeam, 
                    existingSwitch.Team
                );

                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };
            return switchTeamMenu;
        }

        // Show options for all teams except current team
        var teamOptions = new[]
        {
            (Team: 1, Name: "Blue", Color: Color.Blue),
            (Team: 2, Name: "Red", Color: Color.Red),
            (Team: 3, Name: "Yellow", Color: Color.Gold),
            (Team: 4, Name: "Violet", Color: Color.Violet)
        };

        foreach (var teamOption in teamOptions)
        {
            if (teamOption.Team == playerData.PlayerTeam)
                continue; // Skip current team

            var teamItem = new ToolStripMenuItem($"Switch to {teamOption.Name}")
            {
                ForeColor = teamOption.Color
            };

            int targetTeam = teamOption.Team; // Capture for lambda

            teamItem.Click += async (_, _) =>
            {
                var result = await ApiCore.ApiClient!.Player.SwitchTeamPlayerAsync(playerData.PlayerSlot, playerData.PlayerName, playerData.PlayerTeam, targetTeam);

                MessageBox.Show(
                    result.Message,
                    result.Success ? "Player Action" : "Error",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
                );
            };

            switchTeamMenu.DropDownItems.Add(teamItem);
        }

        return switchTeamMenu;
    }
    
    // ================================================================================
    // CONTEXT MENU ACTIONS
    // ================================================================================
    
    private void ContextMenu_Opening(object? sender, CancelEventArgs e)
    {
        // Refresh dynamic content
        contextMenu.Items[0].Text = playerData.PlayerName;
        contextMenu.Items[1].Text = $"Ping: {playerData.PlayerPing} ms";

        // Refresh slap messages
        if (contextMenu.Items[5] is ToolStripMenuItem warningItem)
        {
            AddSlapsToWarningSubMenu(warningItem);
        }

        // Update god mode text
        if (contextMenu.Items[11] is ToolStripMenuItem godModeItem)
        {
            godModeItem.Text = playerData.IsGod ? "Disable God Mode" : "Enable God Mode";
        }

        // Update team switch menu (rebuild to reflect current state)
        if (contextMenu.Items.Count > 12)
        {
            contextMenu.Items.RemoveAt(12);
            contextMenu.Items.Insert(12, CreateSwitchTeamMenuItem());
        }
    }
    private void ContextMenu_IconClick(object? sender, EventArgs e)
    {
        contextMenu.Show(this, new Point(playerContextMenuIcon.Location.X, playerContextMenuIcon.Location.Y));
    }
    private void CardPlayerIP_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(label_dataIPinfo.Text))
        {
            Clipboard.SetText(label_dataIPinfo.Text);
        }
    }
    
    // ================================================================================
    // PROXY/VPN DETECTION METHODS
    // ================================================================================

    /// <summary>
    /// Update player icon based on team and proxy status
    /// </summary>
    private void UpdatePlayerIconAsync(int team)
    {
        if (playerData.IsProxyDetected)
        {
            // Show warning icon for proxy/VPN/TOR
            playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonCircleExclamation;
            playerTeamIcon.IconColor = Color.Orange;
        }
        else
        {
            // Show default team icon
            playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
            playerTeamIcon.IconColor = team switch
            {
                1 => Color.Blue,
                2 => Color.Red,
                3 => Color.Gold,
                4 => Color.Violet,
                _ => Color.Black
            };
        }
    }

    /// <summary>
    /// Fetch and update country code and flag for new player
    /// </summary>
    private async Task UpdateCountryDataAsync()
    {
        string? code = playerData.CountryCode;

        // Only update flag if country code changed
        if (code != CountryCode && code != null)
        {
            CountryCode = code;
            await UpdateCountryFlagAsync(code);
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
                if (PlayerFlagIcon.Image != null)
                {
                    PlayerFlagIcon.Image.Dispose();
                    PlayerFlagIcon.Image = null;
                }
                PlayerFlagIcon.Visible = false;
                return;
            }

            var flag = await FlagHelper.GetFlagAsync(countryIsoCode);

            // Only assign if flag is valid (not null)
            if (flag != null)
            {
                if (PlayerFlagIcon.Image != null)
                {
                    PlayerFlagIcon.Image.Dispose();
                    PlayerFlagIcon.Image = null;
                }
                PlayerFlagIcon.Image = flag;
                PlayerFlagIcon.Visible = true;
                player_Tooltip.SetToolTip(PlayerFlagIcon, $"{countryIsoCode.ToUpper()}");
            }
            else
            {
                if (PlayerFlagIcon.Image != null)
                {
                    PlayerFlagIcon.Image.Dispose();
                    PlayerFlagIcon.Image = null;
                }
                PlayerFlagIcon.Visible = false;
            }
        }
        catch (Exception ex)
        {
            AppDebug.Log($"Error loading flag for {countryIsoCode}", AppDebug.LogLevel.Error, ex);
            if (PlayerFlagIcon.Image != null)
            {
                PlayerFlagIcon.Image.Dispose();
                PlayerFlagIcon.Image = null;
            }
            PlayerFlagIcon.Visible = false;
        }
    }
    
}