using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Forms.Panels;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.StatsManagement;
using BHD_SharedResources.Classes.SupportClasses;
using Microsoft.VisualBasic.Logging;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace BHD_ServerManager.Forms
{
    public partial class ServerManager : Form
    {
        // The Instances (Data)
        private static theInstance thisInstance => CommonCore.theInstance!;

        // Server Manager Tabs
        public tabProfile ProfileTab = null!;                   // The Profile Tab User Control
        public tabServer  ServerTab  = null!;                   // The Server Tab User Control
        public tabMaps    MapsTab    = null!;                   // The Maps Tab User Control
        public tabPlayers PlayersTab = null!;                   // The Players Tab User Control
        public tabChat    ChatTab    = null!;                   // The Chat Tab User Control
        public tabBans    BanTab     = null!;                   // The Bans Tab User Control
        public tabStats   StatsTab   = null!;                   // The Stats Tab User Control
        public tabAdmins  AdminTab   = null!;                   // The Admins Tab User Control

        public ServerManager()
        {
            InitializeComponent();
            Load += PostServerManagerInitalization;
        }

        private void PostServerManagerInitalization(object? sender, EventArgs e)
        {
            functionEvent_loadPanels();                                         // Load the User Control Tabs

            theInstanceManager.CheckSettings();
            banInstanceManager.LoadSettings();
            chatInstanceManagers.LoadSettings();
            adminInstanceManager.LoadSettings();
            theInstanceManager.InitializeTickers();
            theInstanceManager.GetServerVariables();
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
        }

        // --- Server Status and Controls ---
        public void functionEvent_tickerServerGUI()
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
            // Update Label for Win Condition
            functionEvent_UpdateStatusLabels();
        }

        private void functionEvent_UpdateStatusLabels()
        {
            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                label_PlayersOnline.Text = "[Players Online]";
                label_WinCondition.Text = "[Win Condition]";
                label_TimeLeft.Text = "[Time Left]";
                return;
            }

            int scoreTotal = 0;
            string playerName = string.Empty;
            playerObject? topPlayer = null;
            
            if (thisInstance.playerList.Count > 0)
            {
                if (thisInstance.gameInfoGameType == "DM")
                {
                    topPlayer = thisInstance.playerList.Values.OrderByDescending(p => p.stat_Kills).FirstOrDefault();
                    scoreTotal = topPlayer!.stat_Kills;
                    playerName = topPlayer!.PlayerName;
                    label_BlueScore.Text = $"[{scoreTotal}]";
                    label_RedScore.Text = $"[{playerName}]";
                } else if (thisInstance.gameInfoGameType == "KOTH")
                {
                    topPlayer = thisInstance.playerList.Values.OrderByDescending(p => p.ActiveZoneTime).FirstOrDefault();
                    scoreTotal = topPlayer!.ActiveZoneTime;
                    playerName = topPlayer!.PlayerName.Trim();
                    label_BlueScore.Text = $"[{TimeSpan.FromSeconds(scoreTotal):hh\\:mm\\:ss}]";
                    label_RedScore.Text = $"[{playerName}]";
                }
            } else
            {
                label_BlueScore.Text = "[N/A]";
                label_RedScore.Text = "[N/A]";
            }


            // Red Team Label


            label_PlayersOnline.Text = $"[{thisInstance.gameInfoCurrentNumPlayers}/{thisInstance.gameMaxSlots}]";
            label_WinCondition.Text = $"{thisInstance.gameInfoCurrentGameScore} ({thisInstance.gameInfoGameType})";
            label_TimeLeft.Text = "[ "+ thisInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss") + " ]";
        }

        // --- Form Closing ---
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            CommonCore.Ticker?.Stop("ServerManager");
            CommonCore.Ticker?.Stop("ChatManager");
            CommonCore.Ticker?.Stop("PlayerManager");
            CommonCore.Ticker?.Stop("BanManager");
            RemoteServer.Stop();
            theInstanceManager.SaveSettings();
            base.OnFormClosing(e);
        }

        // --- About Window Start---
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
        // --- About Window End ---
    }
}