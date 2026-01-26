using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
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

        private static theInstance ThisInstance = CommonCore.theInstance!;
        private static chatInstance ChatInstance = CommonCore.instanceChat!;
        private static banInstance BanInstance = CommonCore.instanceBans!;
        private playerObject Player { get; set; } = new playerObject();
        private int SlotNumber = 0;
        private ContextMenuStrip ContextMenu;

        private playerObject? LastPlayerData = null;
        private bool LastVisible = true;

        private bool IsGod = false;

        public PlayerCard(int slotNum)
        {
            InitializeComponent();

            SlotNumber = slotNum;
            label_dataSlotNum.Text = slotNum.ToString();

            ContextMenu = new ContextMenuStrip();
            playerContextMenuIcon.ContextMenuStrip = ContextMenu;
            playerContextMenuIcon.Click -= PlayerContextMenuIcon_Click;
            playerContextMenuIcon.Click += PlayerContextMenuIcon_Click;

            BuildContextMenu();

            this.ResetStatus();

        }

        private void BuildContextMenu()
        {
            var playerName = new ToolStripMenuItem(Player.PlayerName ?? "Unknown");
            var playerPing = new ToolStripMenuItem($"Ping: {Player.PlayerPing} ms");
            var ArmCommand = CreateArmMenuItem();
            var DisarmCommand = CreateDisarmMenuItem();
            var warningCommand = CreateWarningMenuItem();
            var kickCommand = CreateKickMenuItem();
            var killCommand = CreateKillMenuItem();
            var banCommand = CreateBanMenuItem();
            var godModeCommand = CreateGodModeMenuItem();
            var switchTeamCommand = CreateSwitchTeamMenuItem();

            ContextMenu.Items.Add(playerName);
            ContextMenu.Items.Add(playerPing);
            ContextMenu.Items.Add(new ToolStripSeparator());
            ContextMenu.Items.Add(ArmCommand);
            ContextMenu.Items.Add(DisarmCommand);
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

        private ToolStripMenuItem CreateArmMenuItem()
        {
            var command = new ToolStripMenuItem("Arm Player");
            command.Click += (sender, e) =>
            {
                command.Text = "Arm Player";
                ServerMemory.WriteMemoryArmPlayer(Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been Armed.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now Armed.");
            };
            return command;
        }
        private ToolStripMenuItem CreateDisarmMenuItem()
        {
            var command = new ToolStripMenuItem("Disarm Player");
            command.Click += (sender, e) =>
            {
                command.Text = "Disarm Player";
                ServerMemory.WriteMemoryDisarmPlayer(Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been Disarmed.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now Disarmed.");
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
                    ServerMemory.WriteMemorySendChatMessage(1, $"{Player.PlayerName}, {slapMessage.SlapMessageText}");
                    MessageBox.Show($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}");
                };
                command.DropDownItems.Add(slapItem);
            }
        }

        private ToolStripMenuItem CreateKickMenuItem()
        {
            var command = new ToolStripMenuItem("Kick Player");
            command.Click += (sender, e) =>
            {
                ServerMemory.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been kicked from the server.");
            };
            return command;
        }

        private ToolStripMenuItem CreateKillMenuItem()
        {
            var command = new ToolStripMenuItem("Kill Player");
            command.Click += (sender, e) =>
            {
                ServerMemory.WriteMemoryKillPlayer(Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been killed.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been killed.");
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

            banByName.Click += async (sender, e) =>
            {
                try
                {
                    // Create name ban record
                    var nameRecord = new banInstancePlayerName
                    {
                        RecordID = 0,
                        MatchID = 0,
                        PlayerName = Player.PlayerNameBase64!,
                        Date = DateTime.Now,
                        ExpireDate = null,
                        AssociatedIP = 0,
                        RecordType = banInstanceRecordType.Permanent,
                        RecordCategory = (int)RecordCategory.Ban,
                        Notes = $"Banned from PlayerCard context menu"
                    };

                    // Add to database
                    int recordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                    nameRecord.RecordID = recordID;

                    // Add to in-memory list
                    BanInstance.BannedPlayerNames.Add(nameRecord);

                    // Kick the player
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);

                    MessageBox.Show($"Player {Player.PlayerName} has been banned by name and kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Ban by name command executed for player {Player.PlayerName} (RecordID: {recordID})");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error banning player by name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppDebug.Log("PlayerCard", $"Error banning player by name: {ex}");
                }
            };

            banByIP.Click += async (sender, e) =>
            {
                try
                {
                    var ipAddress = IPAddress.Parse(Player.PlayerIPAddress!);

                    // Create IP ban record
                    var ipRecord = new banInstancePlayerIP
                    {
                        RecordID = 0,
                        MatchID = 0,
                        PlayerIP = ipAddress,
                        SubnetMask = 32,
                        Date = DateTime.Now,
                        ExpireDate = null,
                        AssociatedName = 0,
                        RecordType = banInstanceRecordType.Permanent,
                        RecordCategory = (int)RecordCategory.Ban,
                        Notes = $"Banned from PlayerCard context menu"
                    };

                    // Add to database
                    int recordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                    ipRecord.RecordID = recordID;

                    // Add to in-memory list
                    BanInstance.BannedPlayerIPs.Add(ipRecord);

                    // Add to NetLimiter filter if enabled
                    if (ThisInstance.netLimiterEnabled && !string.IsNullOrEmpty(ThisInstance.netLimiterFilterName))
                    {
                        await NetLimiterClient.AddIpToFilterAsync(ThisInstance.netLimiterFilterName, ipAddress.ToString(), 32);
                        Debug.WriteLine($"Added IP {ipAddress} to NetLimiter filter '{ThisInstance.netLimiterFilterName}'");
                    }

                    // Kick the player
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);

                    MessageBox.Show($"Player {Player.PlayerName} has been banned by IP and kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Ban by IP command executed for player {Player.PlayerName} (RecordID: {recordID})");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error banning player by IP: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppDebug.Log("PlayerCard", $"Error banning player by IP: {ex}");
                }
            };

            banByNameAndIP.Click += async (sender, e) =>
            {
                try
                {
                    var ipAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                    int nameRecordID = 0;
                    int ipRecordID = 0;

                    // Create name ban record
                    var nameRecord = new banInstancePlayerName
                    {
                        RecordID = 0,
                        MatchID = 0,
                        PlayerName = Player.PlayerNameBase64!,
                        Date = DateTime.Now,
                        ExpireDate = null,
                        AssociatedIP = 0,
                        RecordType = banInstanceRecordType.Permanent,
                        RecordCategory = (int)RecordCategory.Ban,
                        Notes = $"Banned from PlayerCard context menu"
                    };

                    // Add name to database
                    nameRecordID = DatabaseManager.AddPlayerNameRecord(nameRecord);
                    nameRecord.RecordID = nameRecordID;

                    // Create IP ban record
                    var ipRecord = new banInstancePlayerIP
                    {
                        RecordID = 0,
                        MatchID = 0,
                        PlayerIP = ipAddress,
                        SubnetMask = 32,
                        Date = DateTime.Now,
                        ExpireDate = null,
                        AssociatedName = nameRecordID,
                        RecordType = banInstanceRecordType.Permanent,
                        RecordCategory = (int)RecordCategory.Ban,
                        Notes = $"Banned from PlayerCard context menu"
                    };

                    // Add IP to database
                    ipRecordID = DatabaseManager.AddPlayerIPRecord(ipRecord);
                    ipRecord.RecordID = ipRecordID;

                    // Update name record with associated IP
                    nameRecord.AssociatedIP = ipRecordID;
                    DatabaseManager.UpdatePlayerNameRecord(nameRecord);

                    // Add to in-memory lists
                    BanInstance.BannedPlayerNames.Add(nameRecord);
                    BanInstance.BannedPlayerIPs.Add(ipRecord);

                    // Add to NetLimiter filter if enabled
                    if (ThisInstance.netLimiterEnabled && !string.IsNullOrEmpty(ThisInstance.netLimiterFilterName))
                    {
                        await NetLimiterClient.AddIpToFilterAsync(ThisInstance.netLimiterFilterName, ipAddress.ToString(), 32);
                        Debug.WriteLine($"Added IP {ipAddress} to NetLimiter filter '{ThisInstance.netLimiterFilterName}'");
                    }

                    // Kick the player
                    ServerMemory.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);

                    MessageBox.Show($"Player {Player.PlayerName} has been banned by name and IP, then kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Ban by name and IP command executed for player {Player.PlayerName} (Name RecordID: {nameRecordID}, IP RecordID: {ipRecordID})");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error banning player: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppDebug.Log("PlayerCard", $"Error banning player by name and IP: {ex}");
                }
            };

            return command;
        }

        private ToolStripMenuItem CreateGodModeMenuItem()
        {
            var command = new ToolStripMenuItem(IsGod ? "Disable God Mode" : "Enable God Mode");
            command.Click += (sender, e) =>
            {
                if (!IsGod)
                {
                    ServerMemory.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, GodModeHealth);
                }
                else
                {
                    ServerMemory.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, NormalHealth);
                }
                IsGod = !IsGod;
                command.Text = IsGod ? "Disable God Mode" : "Enable God Mode";
                MessageBox.Show($"Player {Player.PlayerName} has been {(IsGod ? "enabled God Mode" : "disabled God Mode")}.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now {(IsGod ? "in God Mode" : "not in God Mode")}");
            };
            return command;
        }

        private ToolStripMenuItem CreateSwitchTeamMenuItem()
        {
            var command = new ToolStripMenuItem("Switch Team");
            command.Click += (sender, e) =>
            {
                var existing = ThisInstance.playerChangeTeamList
                    .FirstOrDefault(p => p.slotNum == Player.PlayerSlot);

                if (existing != null)
                {
                    ThisInstance.playerChangeTeamList.Remove(existing);
                    MessageBox.Show($"Team switch for {Player.PlayerName} has been undone.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Team switch for {Player.PlayerName} (PlayerSlot {Player.PlayerSlot}) has been undone.");
                }
                else
                {
                    int newTeam = Player.PlayerTeam == 1 ? 2 : Player.PlayerTeam == 2 ? 1 : Player.PlayerTeam;
                    if (newTeam != Player.PlayerTeam)
                    {
                        ThisInstance.playerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = Player.PlayerSlot,
                            Team = newTeam
                        });
                        MessageBox.Show($"Player {Player.PlayerName} has been switched to PlayerTeam {(newTeam == 1 ? "Blue" : "Red")} for the next map.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Debug.WriteLine($"Player {Player.PlayerName} has been switched to PlayerTeam {newTeam}.");
                    }
                }
            };
            return command;
        }

        public void UpdateStatus(playerObject playerInfo)
        {
            Player = playerInfo;

            // Decode Base64 and interpret as Windows-1252
            byte[] decodedBytes = Convert.FromBase64String(Player.PlayerNameBase64 ?? "");
            string decodedPlayerName = Encoding.GetEncoding(EncodingName).GetString(decodedBytes);

            label_dataPlayerNameRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label_dataPlayerNameRole.UseCompatibleTextRendering = true;
            label_dataPlayerNameRole.Text = decodedPlayerName;

            label_dataIPinfo.Text = Player.PlayerIPAddress;
            playerTeamIcon.IconColor = Player.PlayerTeam switch
            {
                1 => Color.Blue,
                2 => Color.Red,
                _ => Color.Black
            };
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
            playerTeamIcon.IconColor = Color.Black;
            playerContextMenuIcon.Visible = false;
        }

        public void ToggleSlot(bool visible = true)
        {
            if (Visible == visible)
            {
                // If the visibility is already set, do nothing.
                return;
            }

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
            else if (LastPlayerData == playerInfo)
            {
                // Do nothing...
                return;
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