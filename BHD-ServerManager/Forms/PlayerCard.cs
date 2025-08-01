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
        public playerObject Player { get; set; } = new playerObject();
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        private bool isArmed = true;
        private bool isGod = false;

        // Constructor: Initializes the PlayerCard control and builds the context menu.
        public PlayerCard()
        {
            InitializeComponent();
            BuildContextMenu();
        }

        // Function: BuildContextMenu, constructs the context menu for the PlayerCard control.
        private void BuildContextMenu()
        {
            ToolStripMenuItem playerName = new ToolStripMenuItem();
            ToolStripMenuItem playerPing = new ToolStripMenuItem();
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

            playerContextMenuIcon.ContextMenuStrip = contextMenu;
            playerContextMenuIcon.Click += (sender, e) =>
            {
                contextMenu.Show(this, new Point(playerContextMenuIcon.Location.X, playerContextMenuIcon.Location.Y));
            };
        }
        // Function: command_ArmDisarm_Click, creates a menu item for arming or disarming the player.
        private ToolStripMenuItem command_ArmDisarm_Click()
        {

            ToolStripMenuItem command = new ToolStripMenuItem(isArmed ? "Disarm Player" : "Arm Player");
            command.Click += (sender, e) =>
            {
                // Armed = True/Disarmed False
                command.Text = isArmed ? "Disarm Player" : "Arm Player";
                // Placeholder for actual command logic
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
        // Function: command_Warning_Click, creates a menu item for warning the player with slap messages.
        private ToolStripMenuItem command_Warning_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Warn Player");
            command = AddSlaps2WarningSubMenu(command);
            return command;
        }
        // Function: AddSlaps2WarningSubMenu, adds slap messages to the warning command's sub-menu.
        private ToolStripMenuItem AddSlaps2WarningSubMenu(ToolStripMenuItem thisCommand)
        {
            // Clear existing items to avoid duplicates
            thisCommand.DropDownItems.Clear();
            foreach (var slapMessage in chatInstance.SlapMessages)
            {
                ToolStripMenuItem slapItem = new ToolStripMenuItem(slapMessage.SlapMessageText);
                slapItem.Click += (sender, e) =>
                {
                    // Placeholder for actual slap logic
                    GameManager.WriteMemorySendChatMessage(1, Player.PlayerName + ", " + slapMessage.SlapMessageText);
                    MessageBox.Show($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"Player {Player.PlayerName} has been slapped with message: {slapMessage.SlapMessageText}");
                };
                thisCommand.DropDownItems.Add(slapItem);
            }

            return thisCommand; // Placeholder for actual slap logic, if needed
        }
        // Function: command_Kick_Click, creates a menu item for kicking the player from the server.
        private ToolStripMenuItem command_Kick_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Kick Player");
            command.Click += (sender, e) =>
            {
                // Placeholder for actual command logic
                GameManager.WriteMemorySendConsoleCommand("punt " + Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been kicked from the server.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been kicked from the server.");
            };
            return command;
        }
        // Function: command_GodMode_Click, creates a menu item for toggling God Mode for the player.
        private ToolStripMenuItem command_GodMode_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem(isGod ? "Disable God Mode" : "Enable God Mode");
            command.Click += (sender, e) =>
            {
                if (!isGod)
                {
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, 9999); // Enable God Mode
                }
                else
                {
                    GameManager.WriteMemoryTogglePlayerGodMode(Player.PlayerSlot, 100); // Disable God Mode
                }
                isGod = !isGod;
                command.Text = isGod ? "Disable God Mode" : "Enable God Mode";
                MessageBox.Show($"Player {Player.PlayerName} has been {(isGod ? "enabled God Mode" : "disabled God Mode")}.", "Player Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} is now {(isGod ? "in God Mode" : "not in God Mode")}");
            };
            return command;
        }
        // Function: command_Kill_Click, creates a menu item for killing the player.
        private ToolStripMenuItem command_Kill_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Kill Player");
            command.Click += (sender, e) =>
            {
                // Placeholder for actual command logic
                GameManager.WriteMemoryKillPlayer(Player.PlayerSlot);
                MessageBox.Show($"Player {Player.PlayerName} has been killed.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Player {Player.PlayerName} has been killed.");
            }
            ;
            return command;
        }
        // Function: command_Ban_Click, creates a menu item for banning the player with options for name, IP, or both.
        private ToolStripMenuItem command_Ban_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Ban Player");
            ToolStripMenuItem banByName = new ToolStripMenuItem("Ban Name");
            ToolStripMenuItem banByIP = new ToolStripMenuItem("Ban IP Address");
            ToolStripMenuItem banByNameAndIP = new ToolStripMenuItem("Ban Both");

            // Add sub-menu items for banning
            command.DropDownItems.Add(banByName);
            command.DropDownItems.Add(banByIP);
            command.DropDownItems.Add(banByNameAndIP);

            banByName.Click += (sender, e) =>
            {
                // Ban by name logic
                banInstanceManager.AddBannedPlayer(Player.PlayerNameBase64!);
                MessageBox.Show($"Player {Player.PlayerName} has been banned by name.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban by name command clicked for player {Player.PlayerName}.");
            };
            banByIP.Click += (sender, e) =>
            {
                // Ban by name logic
                IPAddress iPAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(null!, iPAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned by name.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban by name command clicked for player {Player.PlayerName}.");
            };
            banByNameAndIP.Click += (sender, e) =>
            {
                // Ban Both
                IPAddress playerAddress = IPAddress.Parse(Player.PlayerIPAddress!);
                banInstanceManager.AddBannedPlayer(Player.PlayerNameBase64!, playerAddress, 32);
                MessageBox.Show($"Player {Player.PlayerName} has been banned.", "Player Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Debug.WriteLine($"Ban command clicked for player {Player.PlayerName}, but functionality is not implemented.");
            };

            return command;
        }
        // Function: command_SwitchTeam_Click, creates a menu item for switching the player's team.
        private ToolStripMenuItem command_SwitchTeam_Click()
        {
            ToolStripMenuItem command = new ToolStripMenuItem("Switch Team");
            command.Click += (sender, e) =>
            {
                // Check if player is already in the change PlayerTeam list
                var existing = thisInstance.playerChangeTeamList
                    .FirstOrDefault(p => p.slotNum == Player.PlayerSlot);

                if (existing != null)
                {
                    // Undo: remove from list
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
        // Function: UpdateStatus, updates the PlayerCard with the current player information.
        public void UpdateStatus(playerObject playerInfo)
        {
            Player = playerInfo;

            // Decode Base64 and interpret as Windows-1252
            byte[] decodedBytes = Convert.FromBase64String(Player.PlayerNameBase64);
            string decodedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(decodedBytes);

            // Update UI
            label_dataPlayerNameRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label_dataPlayerNameRole.UseCompatibleTextRendering = true;
            label_dataPlayerNameRole.Text = decodedPlayerName;

            // Other updates...
            label_dataIPinfo.Text = Player.PlayerIPAddress;
            label_dataSlotNum.Text = Player.PlayerSlot.ToString();
            playerTeamIcon.IconColor = Player.PlayerTeam switch
            {
                1 => Color.Blue, // Blue Team
                2 => Color.Red,  // Red Team
                _ => Color.Black // Default color for unassigned or unknown teams
            };
            player_Tooltip.SetToolTip(this, $"Ping: {Player.PlayerPing} ms");

            // Conext Updates
            playerContextMenuIcon.Visible = true; // Show context menu icon when player info is updated
            contextMenu.Items[0].Text = Player.PlayerName; // Update player name in context menu
            contextMenu.Items[1].Text = $"Ping: {Player.PlayerPing} ms"; // Update PlayerPing in context menu

            ToolStripMenuItem? WarnMenuUpdate = contextMenu.Items[4] as ToolStripMenuItem;
            AddSlaps2WarningSubMenu(WarnMenuUpdate!);

        }
        // Function: ResetStatus, resets the PlayerCard to default values for an empty slot.
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
            playerTeamIcon.IconColor = Color.Black; // Reset to default color
            playerContextMenuIcon.Visible = false; // Hide context menu icon by default
        }

    }
}
