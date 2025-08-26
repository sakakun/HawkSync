﻿using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
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
        private playerObject Player { get; set; } = new playerObject();
        private int SlotNumber = 0;
        private ContextMenuStrip ContextMenu;

        private playerObject? LastPlayerData = null;
        private bool LastVisible = true;

        private bool IsArmed = true;
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
                GameManager.WriteMemoryArmPlayer(Player.PlayerSlot);
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
                GameManager.WriteMemoryDisarmPlayer(Player.PlayerSlot);
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
                    GameManager.WriteMemorySendChatMessage(1, $"{Player.PlayerName}, {slapMessage.SlapMessageText}");
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
                GameManager.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);
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
                GameManager.WriteMemoryKillPlayer(Player.PlayerSlot);
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

            banByName.Click += (sender, e) =>
            {
                banInstanceManager.AddBannedPlayer(Player.PlayerNameBase64!);
                MessageBox.Show($"Player {Player.PlayerName} has been banned by name.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban by name command clicked for player {Player.PlayerName}.");
            };
            banByIP.Click += (sender, e) =>
            {
                var ipAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(null!, ipAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned by IP.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban by IP command clicked for player {Player.PlayerName}.");
            };
            banByNameAndIP.Click += (sender, e) =>
            {
                var playerAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(Player.PlayerNameBase64!, playerAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban command clicked for player {Player.PlayerName}.");
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
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, GodModeHealth);
                }
                else
                {
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, NormalHealth);
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