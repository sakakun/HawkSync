using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using HawkSyncShared;
using RemoteClient.Core;
using RemoteClient.Forms.Panels;
using HawkSyncShared.SupportClasses;

namespace RemoteClient.Forms;

public partial class ServerManagerUI : Form
{
     
    // The Instances (Data)
    private static theInstance thisInstance => CommonCore.theInstance!;


    private static playerInstance playerInstance => CommonCore.instancePlayers!;

    // Server Manager Tabs
    public tabProfile       ProfileTab = null!;     // The Profile Tab User Control
    public tabGamePlay      ServerTab  = null!;     // The Server Tab User Control
    public tabMaps          MapsTab    = null!;     // The Maps Tab User Control
    public tabPlayers       PlayersTab = null!;     // The Players Tab User Control
    public tabChat          ChatTab    = null!;     // The Chat Tab User Control
    public tabBans          BanTab     = null!;     // The Bans Tab User Control
    public tabStats         StatsTab   = null!;     // The Stats Tab User Control
    public tabAdmin         AdminTab   = null!;     // The Admin Tab User Control

    public ServerManagerUI()
    {
        InitializeComponent();
        
        // SUBSCRIBE TO CONNECTION STATE CHANGES
        ApiCore.OnConnectionStateChanged += OnConnectionStateChanged;
        
        // OPTIONALLY: Subscribe to snapshots for server info panel
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;
        
        Load += PostServerManagerInitalization;

        // Initial update
        UpdateServerInfo();
    }

    private void PostServerManagerInitalization(object? sender, EventArgs e)
    {
        // Load the Panels
        functionEvent_loadPanels();   
    }

    private void functionEvent_loadPanels()
    {
        // Load the User Controls into the TabPages
        tabProfile.Controls.Add(ProfileTab = new tabProfile());
        tabGamePlay.Controls.Add(ServerTab = new tabGamePlay());
        tabMaps.Controls.Add(MapsTab = new tabMaps());
        tabPlayers.Controls.Add(PlayersTab = new tabPlayers());
        tabChat.Controls.Add(ChatTab = new tabChat());
        tabBans.Controls.Add(BanTab = new tabBans());
        tabStats.Controls.Add(StatsTab = new tabStats());
        tabAdmin.Controls.Add(AdminTab = new tabAdmin());
    }


    // EVENT HANDLER - Connection state changed
    private void OnConnectionStateChanged(string state)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnConnectionStateChanged(state));
            return;
        }

        if (state.Contains("Reconnecting"))
        {
            MessageBox.Show("Connection to the server has been lost. The remote is attempting to reconnect.", 
                "Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // If the connection state indicates a disconnection, close the UI
        if (state.Contains("Disconnected"))
        {
            MessageBox.Show("Connection to the server has been lost. The Server Manager will now close.", 
                "Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
        }
    }

    // EVENT HANDLER - Snapshot received
    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }
        
        // Update the Core Instances
        CommonCore.theInstance = snapshot.ServerData;
        CommonCore.instanceChat = snapshot.Chat;
        CommonCore.instanceMaps = snapshot.Maps;
        CommonCore.instanceBans = snapshot.Bans;
        CommonCore.instancePlayers = snapshot.Players;

        AppDebug.Log("ServerManagerUI", "Snapshot received, updating server info panel. Active Playlist: " + CommonCore.instanceMaps.ActivePlaylist);

        UpdateServerInfo();
    }

    private void UpdateServerInfo()
    {
        if (ApiCore.CurrentSnapshot == null) return;
        
        var s = ApiCore.CurrentSnapshot;
        
        functionEvent_UpdateStatusLabels();
    }

    private void functionEvent_UpdateStatusLabels()
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

        // Offline Labels
        if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
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
        label_PlayersOnline.Text = $"[{thisInstance.gameInfoNumPlayers}/{thisInstance.gameMaxSlots}]";

        if (playerInstance.PlayerList.Count > 0)
        {
            winConditions = $"[{thisInstance.gameInfoWinCond} Kills ({thisInstance.gameInfoGameType})]";

            if (thisInstance.gameInfoGameType == 0)
            {
                topPlayer = playerInstance.PlayerList.Values.OrderByDescending(p => p.stat_Kills).FirstOrDefault();
                scoreTotal = topPlayer!.stat_Kills;
                playerName = (scoreTotal == 0 ? "Draw" : topPlayer!.PlayerName);
                blueScore = $"{scoreTotal}";
                redScore = $"{playerName}";
            } else if (thisInstance.gameInfoGameType == 4)
            {
                topPlayer = playerInstance.PlayerList.Values.OrderByDescending(p => p.ActiveZoneTime).FirstOrDefault();
                scoreTotal = topPlayer!.ActiveZoneTime;
                playerName = ( scoreTotal == 0 ? "Draw" : topPlayer!.PlayerName);
                blueScore = $"{TimeSpan.FromSeconds(scoreTotal):hh\\:mm\\:ss}";
                redScore = $"{playerName}";
                winConditions = $"[Time of {TimeSpan.FromSeconds(thisInstance.gameInfoWinCond*60):hh\\:mm\\:ss} ({thisInstance.gameInfoGameType})]";
            } else if (thisInstance.gameInfoGameType == 5)
            {
                blueScore = $"{thisInstance.gameInfoBlueScore}";
                redScore = $"{thisInstance.gameInfoRedScore}";
                winConditions = $"[{thisInstance.gameInfoWinCond} Targets ({thisInstance.gameInfoGameType})]";
            } else if (thisInstance.gameInfoGameType == 6)
            {
                blueScore = $"{(thisInstance.gameInfoIsBlueDefending == false ? "Red Attacking" : thisInstance.gameInfoBlueScore) }";
                redScore = $"{(thisInstance.gameInfoIsBlueDefending ? "Blue Attacking" : thisInstance.gameInfoRedScore )}";
                winConditions = $"[{thisInstance.gameInfoWinCond} Targets ({thisInstance.gameInfoGameType})]";
            } else if (thisInstance.gameInfoGameType == 3)
            {
                blueScore = $"{TimeSpan.FromSeconds(thisInstance.gameInfoBlueScore * 60):hh\\:mm\\:ss}";
                redScore = $"{TimeSpan.FromSeconds(thisInstance.gameInfoRedScore * 60):hh\\:mm\\:ss}";
                winConditions = $"[Time of {TimeSpan.FromSeconds(thisInstance.gameInfoWinCond * 60):hh\\:mm\\:ss} ({thisInstance.gameInfoGameType})]";
            } else if (thisInstance.gameInfoGameType == 7 || thisInstance.gameInfoGameType == 8)
            {
                blueScore = $"{thisInstance.gameInfoBlueScore}";
                redScore = $"{thisInstance.gameInfoRedScore}";
                winConditions = $"[{thisInstance.gameInfoWinCond} Captures ({thisInstance.gameInfoGameType})]";
            } else if (thisInstance.gameInfoGameType == 1)
            {
                blueScore = $"{thisInstance.gameInfoBlueScore}";
                redScore = $"{thisInstance.gameInfoRedScore}";
                winConditions = $"[{thisInstance.gameInfoWinCond} Kills ({thisInstance.gameInfoGameType})]";
            } else
            {
                blueScore = $"{thisInstance.gameInfoBlueScore}";
                redScore = $"{thisInstance.gameInfoRedScore}";
                winConditions = $"[{thisInstance.gameInfoWinCond} Kills ({thisInstance.gameInfoGameType})]";
            }


            label_BlueScore.Text = $"[{blueScore}]";
            label_RedScore.Text = $"[{redScore}]";

        } else
        {
            label_BlueScore.Text = "[NO]";
            label_RedScore.Text = "[PLAYERS]";
        }

        label_WinCondition.Text = winConditions;
        label_TimeLeft.Text = "[ "+ thisInstance.gameInfoTimeRemaining.ToString(@"hh\:mm\:ss") + " ]";
    }


}