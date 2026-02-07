using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms.Panels;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;
using HawkSyncShared.Instances;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using BHD_ServerManager.Classes.Services.NetLimiter;
using HawkSyncShared.DTOs.tabPlayers;

namespace BHD_ServerManager.Forms
{
    public partial class ServerManagerUI : Form
    {

        private static theInstance thisInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;

        // Server Manager Tabs
        public tabProfile       ProfileTab = null!;                   // The Profile Tab User Control
        public tabGamePlay      GamePlayTab  = null!;                   // The Server Tab User Control
        public tabMaps          MapsTab    = null!;                   // The Maps Tab User Control
        public tabPlayers       PlayersTab = null!;                   // The Players Tab User Control
        public tabChat          ChatTab    = null!;                   // The Chat Tab User Control
        public tabBans          BanTab     = null!;                   // The Bans Tab User Control
        public tabStats         StatsTab   = null!;                   // The Stats Tab User Control
        public tabAdmin         AdminTab   = null!;                   // The Admin Tab User Control

        public ServerManagerUI()
        {
            InitializeComponent();
            Load += PostServerManagerInitalization;
        }

        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            // Load the User Controls into the TabPages
            tabProfile.Controls.Add(ProfileTab = new tabProfile());
            tabGamePlay.Controls.Add(GamePlayTab = new tabGamePlay());
            tabMaps.Controls.Add(MapsTab = new tabMaps());
            tabPlayers.Controls.Add(PlayersTab = new tabPlayers());
            tabChat.Controls.Add(ChatTab = new tabChat());
            tabBans.Controls.Add(BanTab = new tabBans());
            tabStats.Controls.Add(StatsTab = new tabStats());
            tabAdmin.Controls.Add(AdminTab = new tabAdmin());

            // Initialize "Global" Tickers for the Server Manager
            theInstanceManager.InitializeTickers();

            // Initialize Ticker for the Server Manager UI
            CommonCore.Ticker?.Start("ServerManager", 1000, Ticker_ServerManagerUI);

            // Old Server Settings Initialization - each item should be responsible for its own loading. 
            // adminInstanceManager.InitializeDefaultAdmin();

        }

        /// <summary>
        /// Independent ticker method for updating the Server Manager UI elements.
        /// </summary>
        /// <remarks>If called from a thread other than the UI thread, the method automatically marshals
        /// the update to the UI thread. This ensures thread-safe updates to UI elements.</remarks>
        public void Ticker_ServerManagerUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(Ticker_ServerManagerUI));
                return;
            }

            // Update Label for Win Condition
            UpdateStatusLabels();
        }

        /// <summary>
        /// Updates the status labels in the user interface to reflect the current server state, player counts, scores,
        /// win conditions, and remaining game time.
        /// </summary>
        /// <remarks>This method should be called whenever the game state changes to ensure that the
        /// displayed information remains accurate. If the server is offline, only offline-related labels are updated.
        /// For active games, the labels are updated based on the current game type and relevant statistics. This method
        /// is not thread-safe and should be invoked from the UI thread.</remarks>
        private void UpdateStatusLabels()
        {

            // Update the Server Status Text
            toolStripStatus.Text = thisInstance.instanceStatus switch
            {
                InstanceStatus.OFFLINE => "Server is not running.",
                InstanceStatus.ONLINE => "Server is running. Game in progress.",
                InstanceStatus.SCORING => "Server is running. Game has ended, currently scoring.",
                InstanceStatus.LOADINGMAP => "Server is running. Game reset in progress.",
                InstanceStatus.STARTDELAY => "Server is running. Game ready, waiting for start.",
                _ => toolStripStatus.Text
            };

            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                SetOfflineLabels();
                return;
            }

            label_PlayersOnline.Text = $"[{thisInstance.gameInfoNumPlayers}/{thisInstance.gameMaxSlots}]";

            string blueScore = "[NO]";
            string redScore = "[PLAYERS]";
            string winConditions = string.Empty;

            if (playerInstance.PlayerList.Count > 0)
            {
                int gameType = thisInstance.gameInfoGameType;
                int winCond = thisInstance.gameInfoWinCond;
                int blue = thisInstance.gameInfoBlueScore;
                int red = thisInstance.gameInfoRedScore;
                bool isBlueDefending = thisInstance.gameInfoIsBlueDefending;

                switch (gameType)
                {
                    case 0:
                        var topPlayerKills = playerInstance.PlayerList.Values.OrderByDescending(p => p.stat_Kills).FirstOrDefault();
                        int scoreTotal = topPlayerKills?.stat_Kills ?? 0;
                        string playerName = (scoreTotal == 0 ? "Draw" : topPlayerKills?.PlayerName ?? "Unknown");
                        blueScore = $"{scoreTotal}";
                        redScore = $"{playerName}";
                        winConditions = $"[{winCond} Kills ({gameType})]";
                        break;
                    case 4:
                        var topPlayerZone = playerInstance.PlayerList.Values.OrderByDescending(p => p.ActiveZoneTime).FirstOrDefault();
                        int zoneTime = topPlayerZone?.ActiveZoneTime ?? 0;
                        string zonePlayer = (zoneTime == 0 ? "Draw" : topPlayerZone?.PlayerName ?? "Unknown");
                        blueScore = $"{TimeSpan.FromSeconds(zoneTime):hh\\:mm\\:ss}";
                        redScore = $"{zonePlayer}";
                        winConditions = $"[Time of {TimeSpan.FromSeconds(winCond * 60):hh\\:mm\\:ss} ({gameType})]";
                        break;
                    case 5:
                        blueScore = $"{blue}";
                        redScore = $"{red}";
                        winConditions = $"[{winCond} Targets ({gameType})]";
                        break;
                    case 6:
                        blueScore = $"{(isBlueDefending == false ? "Red Attacking" : blue)}";
                        redScore = $"{(isBlueDefending ? "Blue Attacking" : red)}";
                        winConditions = $"[{winCond} Targets ({gameType})]";
                        break;
                    case 3:
                        blueScore = $"{TimeSpan.FromSeconds(blue * 60):hh\\:mm\\:ss}";
                        redScore = $"{TimeSpan.FromSeconds(red * 60):hh\\:mm\\:ss}";
                        winConditions = $"[Time of {TimeSpan.FromSeconds(winCond * 60):hh\\:mm\\:ss} ({gameType})]";
                        break;
                    case 7:
                    case 8:
                        blueScore = $"{blue}";
                        redScore = $"{red}";
                        winConditions = $"[{winCond} Captures ({gameType})]";
                        break;
                    case 1:
                        blueScore = $"{blue}";
                        redScore = $"{red}";
                        winConditions = $"[{winCond} Kills ({gameType})]";
                        break;
                    default:
                        blueScore = $"{blue}";
                        redScore = $"{red}";
                        winConditions = $"[{winCond} Kills ({gameType})]";
                        break;
                }
            }

            label_BlueScore.Text = $"[{blueScore}]";
            label_RedScore.Text = $"[{redScore}]";
            label_WinCondition.Text = winConditions;
            label_TimeLeft.Text = "[ " + thisInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss") + " ]";
        }

        private void SetOfflineLabels()
        {
            label_PlayersOnline.Text = "[Players Online]";
            label_BlueScore.Text = "[Blue Score]";
            label_RedScore.Text = "[Red Score]";
            label_WinCondition.Text = "[Win Condition]";
            label_TimeLeft.Text = "[Time Left]";
        }

        // --- Form Closing ---
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Confirm Exit
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Nevermind and return to the Server Manager
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            // Stop all Tickers
            CommonCore.Ticker?.StopAll();
            // Stop the NetLimiter Bridge Process if it's running
            NetLimiterClient.StopBridgeProcess();
            // Close Application
            base.OnFormClosing(e);
        }

    }
}