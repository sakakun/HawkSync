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
using System.Data;
using System.Net;
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
            functionEvent_UpdateScoreLabels();
        }

        private void functionEvent_UpdateScoreLabels()
        {
            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                label_WinCondition.Text = "[Win Condition]";
                return;
            }
            label_WinCondition.Text = $"{thisInstance.gameInfoCurrentGameScore} ({thisInstance.gameInfoGameType})";
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

    }
}