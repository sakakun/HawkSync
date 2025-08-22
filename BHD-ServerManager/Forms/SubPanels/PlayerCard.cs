using BHD_SharedResources.Classes.CoreObjects;
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
        private static theInstance thisInstance = CommonCore.theInstance!;
        private static chatInstance chatInstance = CommonCore.instanceChat!;
        private playerObject Player { get; set; } = new playerObject();
        // Change the visibility of the 'contextMenu' field from 'private' to 'public' to match the containing type's visibility.
        // This resolves CS9032 by ensuring the required member is not less visible than the containing type.
        private ContextMenuStrip contextMenu;

        private bool isArmed = true;
        private bool isGod = false;

        public PlayerCard()
        {
            InitializeComponent();

            contextMenu = new ContextMenuStrip();
            playerContextMenuIcon.ContextMenuStrip = contextMenu;
            playerContextMenuIcon.Click -= PlayerContextMenuIcon_Click;
            playerContextMenuIcon.Click += PlayerContextMenuIcon_Click;

            BuildContextMenu();
        }

        private void BuildContextMenu()
        {

            ToolStripMenuItem playerName = new ToolStripMenuItem(Player.PlayerName ?? "Unknown");
            ToolStripMenuItem playerPing = new ToolStripMenuItem($"Ping: {Player.PlayerPing} ms");
            ToolStripMenuItem command_ArmDisarm = command_ArmDisarm_Click();
            ToolStripMenuItem command_Warning = command_Warning_Click();
            ToolStripMenuItem command_Kick = command_Kick_Click();
            ToolStripMenuItem command_Kill = command_Kill_Click();
            ToolStripMenuItem command_Ban = command_Ban_Click();

            contextMenu.Items.Add(playerName);
            contextMenu.Items.Add(playerPing);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(command_ArmDisarm);
            contextMenu.Items.Add(command_Warning);
            contextMenu.Items.Add(command_Kick);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(command_Kill);
            contextMenu.Items.Add(command_Ban);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(command_GodMode_Click());
            contextMenu.Items.Add(command_SwitchTeam_Click());

        }

        private void PlayerContextMenuIcon_Click(object? sender, EventArgs e)
        {
            contextMenu.Show(this, new Point(playerContextMenuIcon.Location.X, playerContextMenuIcon.Location.Y));
        }

        private ToolStripMenuItem command_ArmDisarm_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem(isArmed ? "Disarm Player" : "Arm Player");
            command.Click += (sender, e) =>
            {
                command.Text = isArmed ? "Disarm Player" : "Arm Player";
                if (isArmed)
                {
                    GameManager.WriteMemoryDisarmPlayer(Player.PlayerSlot);
                }
                else
                {
                    GameManager.WriteMemoryArmPlayer(Player.PlayerSlot);
                }
                isArmed = !isArmed;
                MessageBox.Show($"Player {Player.PlayerName} has been {(isArmed ? "armed" : "disarmed")}.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now {(isArmed ? "armed" : "disarmed")}");
            };
            return command;
        }

        private ToolStripMenuItem command_Warning_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Warn Player");
            command = AddSlaps2WarningSubMenu(command);
            return command;
        }

        private ToolStripMenuItem AddSlaps2WarningSubMenu(ToolStripMenuItem thisCommand)
        {
            thisCommand.DropDownItems.Clear();
            foreach (var slapMessage in chatInstance.SlapMessages)
            {
                ToolStripMenuItem slapItem = new ToolStripMenuItem(slapMessage.SlapMessageText);
                slapItem.Click += (sender, e) =>
                {
                    GameManager.WriteMemorySendChatMessage(1, Player.PlayerName + ", " + slapMessage.SlapMessageText);
                    MessageBox.Show($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}");
                };
                thisCommand.DropDownItems.Add(slapItem);
            }
            return thisCommand;
        }

        private ToolStripMenuItem command_Kick_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Kick Player");
            command.Click += (sender, e) =>
            {
                GameManager.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been kicked from the server.");
            };
            return command;
        }

        private ToolStripMenuItem command_GodMode_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem(isGod ? "Disable God Mode" : "Enable God Mode");
            command.Click += (sender, e) =>
            {
                if (!isGod)
                {
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, 9999);
                }
                else
                {
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, 100);
                }
                isGod = !isGod;
                command.Text = isGod ? "Disable God Mode" : "Enable God Mode";
                MessageBox.Show($"Player {Player.PlayerName} has been {(isGod ? "enabled God Mode" : "disabled God Mode")}.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now {(isGod ? "in God Mode" : "not in God Mode")}");
            };
            return command;
        }

        private ToolStripMenuItem command_Kill_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Kill Player");
            command.Click += (sender, e) =>
            {
                GameManager.WriteMemoryKillPlayer(Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been killed.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been killed.");
            };
            return command;
        }

        private ToolStripMenuItem command_Ban_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Ban Player");
            ToolStripMenuItem banByName = new ToolStripMenuItem("Ban Name");
            ToolStripMenuItem banByIP = new ToolStripMenuItem("Ban IP Address");
            ToolStripMenuItem banByNameAndIP = new ToolStripMenuItem("Ban Both");

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
                IPAddress iPAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(null!, iPAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned by IP.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban by IP command clicked for player {Player.PlayerName}.");
            };
            banByNameAndIP.Click += (sender, e) =>
            {
                IPAddress playerAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(Player.PlayerNameBase64!, playerAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban command clicked for player {Player.PlayerName}.");
            };

            return command;
        }

        private ToolStripMenuItem command_SwitchTeam_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Switch Team");
            command.Click += (sender, e) =>
            {
                var existing = thisInstance.playerChangeTeamList
                    .FirstOrDefault(p => p.slotNum == Player.PlayerSlot);

                if (existing != null)
                {
                    thisInstance.playerChangeTeamList.Remove(existing);
                    MessageBox.Show($"Team switch for {Player.PlayerName} has been undone.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Team switch for {Player.PlayerName} (PlayerSlot {Player.PlayerSlot}) has been undone.");
                }
                else
                {
                    int newTeam = Player.PlayerTeam == 1 ? 2 : Player.PlayerTeam == 2 ? 1 : Player.PlayerTeam;
                    if (newTeam != Player.PlayerTeam)
                    {
                        thisInstance.playerChangeTeamList.Add(new playerTeamObject
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
            string decodedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(decodedBytes);

            label_dataPlayerNameRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label_dataPlayerNameRole.UseCompatibleTextRendering = true;
            label_dataPlayerNameRole.Text = decodedPlayerName;

            label_dataIPinfo.Text = Player.PlayerIPAddress;
            label_dataSlotNum.Text = Player.PlayerSlot.ToString();
            playerTeamIcon.IconColor = Player.PlayerTeam switch
            {
                1 => Color.Blue,
                2 => Color.Red,
                _ => Color.Black
            };
            this.contextMenu.Items[0].Text = decodedPlayerName;
            this.contextMenu.Items[1].Text = $"Ping: {Player.PlayerPing} ms";

            player_Tooltip.SetToolTip(this, $"Ping: {Player.PlayerPing} ms");
            playerContextMenuIcon.Visible = true;
        }

        public void ResetStatus(int slotNum)
        {
            Player = new playerObject
            {
                PlayerSlot = slotNum,
                PlayerName = "Slot Empty",
                PlayerIPAddress = ""
            };
            label_dataIPinfo.Text = Player.PlayerIPAddress;
            label_dataPlayerNameRole.Text = $"{Player.PlayerName}";
            label_dataSlotNum.Text = Player.PlayerSlot.ToString();
            playerTeamIcon.IconColor = Color.Black;
            playerContextMenuIcon.Visible = false;

        }

        public void ToggleSlot(int slotNum, bool visible = true)
        {
            if (this.Visible == visible)
            {
                // If the visibility is already set, do nothing.
                return;
            }

            this.ResetStatus(slotNum);
            this.Visible = visible;
        }
    }
}