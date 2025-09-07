using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms.Panels;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace BHD_RemoteClient.Forms
{
    public partial class ServerManager : Form
    {
        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;

        // Server Manager Tabs
        public tabProfile ProfileTab = null!;                   // The Profile Tab User Control
        public tabServer ServerTab = null!;                     // The Server Tab User Control
        public tabMaps MapsTab = null!;                         // The Maps Tab User Control
        public tabPlayers PlayersTab = null!;                   // The Players Tab User Control
        public tabChat ChatTab = null!;                         // The Chat Tab User Control
        public tabBans BanTab = null!;                          // The Bans Tab User Control
        public tabStats StatsTab = null!;                       // The Stats Tab User Control
        public tabAdmins AdminTab = null!;                      // The Admins Tab User Control
        public tabAddons AddonsTab = null!;                   // The Addons Tab User Control

        // Admin Selections
        private int adminSelectedId = -1; // Selected Admin ID for actions

        // Main constructor for the ServerManager
        public ServerManager()
        {
            InitializeComponent();

            // Instance Initialization
            theInstanceManager.CheckSettings();
            banInstanceManager.LoadSettings();
            chatInstanceManagers.LoadSettings();
            adminInstanceManager.LoadSettings();
            this.Load += PostServerManagerInitalization;
        }

        // Init. Form of the ServerManager
        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            functionEvent_loadPanels();                                         // Load the User Control Tabs

            // Start All Tickers
            theInstanceManager.InitializeTickers();
            theInstanceManager.GetServerVariables();
            mapInstanceManager.ResetAvailableMaps();

            // Admins Tab
            adminInstanceManager.UpdateAdminLogDialog();

        }
        private void functionEvent_loadPanels()
        {
            // Load the User Controls into the TabPages
            tabProfile.Controls.Add(ProfileTab = new tabProfile());
            tabServer.Controls.Add(ServerTab = new tabServer());
            tabMaps.Controls.Add(MapsTab = new tabMaps());
            tabPlayers.Controls.Add(PlayersTab = new tabPlayers());
            tabChat.Controls.Add(ChatTab = new tabChat());
            tabBans.Controls.Add(BanTab = new tabBans());
            tabStats.Controls.Add(StatsTab = new tabStats());
            tabAdmin.Controls.Add(AdminTab = new tabAdmins());
            tabAddons.Controls.Add(AddonsTab = new tabAddons());
        }
        //
        // Event Handlers for ServerManager
        // Description: Function calls not made by a Click or KeyPress event.
        //
        // Function: functionEvent_tickerServerGUI, Updates the status label based on the server's current status.
        internal void functionEvent_tickerServerGUI()
        {
            // Update the Server Status Text
            toolStripStatus.Text = theInstance.instanceStatus switch
            {
                InstanceStatus.OFFLINE => "Server is not running.",
                InstanceStatus.ONLINE => "Server is running. Game in progress.",
                InstanceStatus.SCORING => "Server is running. Game has ended, currently scoring.",
                InstanceStatus.LOADINGMAP => "Server is running. Game reset in progress.",
                InstanceStatus.STARTDELAY => "Server is running. Game ready, waiting for start.",
                _ => toolStripStatus.Text
            };

            functionEvent_UpdateStatusLabels();
        }

        private void functionEvent_UpdateStatusLabels()
        {
            // Offline Labels
            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                label_PlayersOnline.Text = "[Players Online]";
                label_BlueScore.Text = "[Blue Score]";
                label_RedScore.Text = "[Red Score]";
                label_WinCondition.Text = "[Win Condition]";
                label_TimeLeft.Text = "[Time Left]";
                return;
            }
            // Variables
            int scoreTotal = 0;
            string blueScore = string.Empty;
            string redScore = string.Empty;
            string playerName = string.Empty;
            playerObject? topPlayer = null;
            string winConditions = string.Empty;

            // Player Online Label
            label_PlayersOnline.Text = $"[{theInstance.gameInfoCurrentNumPlayers}/{theInstance.gameMaxSlots}]";

            if (theInstance.playerList.Count > 0)
            {
                winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Kills ({theInstance.gameInfoGameType})]";

                if (theInstance.gameInfoGameType == "DM")
                {
                    topPlayer = theInstance.playerList.Values.OrderByDescending(p => p.stat_Kills).FirstOrDefault();
                    scoreTotal = topPlayer!.stat_Kills;
                    playerName = (scoreTotal == 0 ? "Draw" : topPlayer!.PlayerName);
                    blueScore = $"{scoreTotal}";
                    redScore = $"{playerName}";
                }
                else if (theInstance.gameInfoGameType == "KOTH")
                {
                    topPlayer = theInstance.playerList.Values.OrderByDescending(p => p.ActiveZoneTime).FirstOrDefault();
                    scoreTotal = topPlayer!.ActiveZoneTime;
                    playerName = (scoreTotal == 0 ? "Draw" : topPlayer!.PlayerName);
                    blueScore = $"{TimeSpan.FromSeconds(scoreTotal):hh\\:mm\\:ss}";
                    redScore = $"{playerName}";
                    winConditions = $"[Time of {TimeSpan.FromSeconds(theInstance.gameInfoCurrentGameWinCond * 60):hh\\:mm\\:ss} ({theInstance.gameInfoGameType})]";
                }
                else if (theInstance.gameInfoGameType == "SD")
                {
                    blueScore = $"{theInstance.gameInfoCurrentBlueScore}";
                    redScore = $"{theInstance.gameInfoCurrentRedScore}";
                    winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Targets ({theInstance.gameInfoGameType})]";
                }
                else if (theInstance.gameInfoGameType == "AD")
                {
                    blueScore = $"{(theInstance.gameInfoCurrentGameDefendingTeamBlue == false ? "Red Attacking" : theInstance.gameInfoCurrentBlueScore)}";
                    redScore = $"{(theInstance.gameInfoCurrentGameDefendingTeamBlue ? "Blue Attacking" : theInstance.gameInfoCurrentRedScore)}";
                    winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Targets ({theInstance.gameInfoGameType})]";
                }
                else if (theInstance.gameInfoGameType == "TKOTH")
                {
                    blueScore = $"{TimeSpan.FromSeconds(theInstance.gameInfoCurrentBlueScore * 60):hh\\:mm\\:ss}";
                    redScore = $"{TimeSpan.FromSeconds(theInstance.gameInfoCurrentRedScore * 60):hh\\:mm\\:ss}";
                    winConditions = $"[Time of {TimeSpan.FromSeconds(theInstance.gameInfoCurrentGameWinCond * 60):hh\\:mm\\:ss} ({theInstance.gameInfoGameType})]";
                }
                else if (theInstance.gameInfoGameType == "CTF" || theInstance.gameInfoGameType == "FB")
                {
                    blueScore = $"{theInstance.gameInfoCurrentBlueScore}";
                    redScore = $"{theInstance.gameInfoCurrentRedScore}";
                    winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Captures ({theInstance.gameInfoGameType})]";
                }
                else if (theInstance.gameInfoGameType == "TDM")
                {
                    blueScore = $"{theInstance.gameInfoCurrentBlueScore}";
                    redScore = $"{theInstance.gameInfoCurrentRedScore}";
                    winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Kills ({theInstance.gameInfoGameType})]";
                }
                else
                {
                    blueScore = $"{theInstance.gameInfoCurrentBlueScore}";
                    redScore = $"{theInstance.gameInfoCurrentRedScore}";
                    winConditions = $"[{theInstance.gameInfoCurrentGameWinCond} Kills ({theInstance.gameInfoGameType})]";
                }


                label_BlueScore.Text = $"[{blueScore}]";
                label_RedScore.Text = $"[{redScore}]";

            }
            else
            {
                label_BlueScore.Text = "[NO]";
                label_RedScore.Text = "[PLAYERS]";
            }

            label_WinCondition.Text = winConditions;
            label_TimeLeft.Text = "[ " + theInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss") + " ]";
        }

        // Action Click Handlers
        // Scope: Program, Function: OnFormClosing override, Handles the form closing event to prompt the user and perform cleanup.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Example: Prompt user before closing
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Prevent the form from closing
                return;
            }

            // Place any cleanup logic here (e.g., save settings, stop tickers, etc.)
            CommonCore.Ticker?.Stop("ServerManager");
            CommonCore.Ticker?.Stop("ChatManager");
            CommonCore.Ticker?.Stop("PlayerManager");
            CommonCore.Ticker?.Stop("BanManager");

            // Save settings before closing
            theInstanceManager.SaveSettings();
            base.OnFormClosing(e);

        }

        // About Window Start
        // Win32 API constants and methods
        private const int WM_SYSCOMMAND = 0x112;
        private const int ABOUT_SYSMENU_ID = 0x1FFF; // Custom ID for AboutWindow
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, int wPosition, int wFlags, int wIDNewItem, string lpNewItem);
        private const int MF_STRING = 0x00000000;
        private const int MF_SEPARATOR = 0x00000800;
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get the system menu handle
            IntPtr systemMenuHandle = GetSystemMenu(this.Handle, false);

            // Add a separator and the AboutWindow item
            InsertMenu(systemMenuHandle, -1, MF_SEPARATOR, 0, string.Empty);
            InsertMenu(systemMenuHandle, -1, MF_STRING, ABOUT_SYSMENU_ID, "About");
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == ABOUT_SYSMENU_ID)
            {
                var about = new AboutWindow();
                about.ShowDialog();
                return;
            }
            base.WndProc(ref m);
        }
        // About Window End

    }
}

