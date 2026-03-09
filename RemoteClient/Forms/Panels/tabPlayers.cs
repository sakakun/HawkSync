using BHD_ServerManager.Classes.PlayerManagementClasses;
using HawkSyncShared;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.Instances;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels;

public partial class tabPlayers : UserControl
{

    private theInstance? theInstance => CommonCore.theInstance;
    private playerInstance playerInstance => CommonCore.instancePlayers!;

    // --- Class Variables ---
    private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

    private PlayerCard[] playerCards = new PlayerCard[80];

    // --- Generate Init. Cards ---
    private void functionEvent_GeneratePlayerCards()
    {
        TableLayoutPanel PlayerCards1 = playerTable1; // 40 cards (1-40)
        TableLayoutPanel PlayerCards2 = playerTable2; // 40 cards (41-80)
        
        PlayerCards1.Controls.Clear();
        PlayerCards2.Controls.Clear();

        // Configure PlayerCards1: 4 columns x 10 rows = 40 cards
        PlayerCards1.ColumnCount = 4;
        PlayerCards1.RowCount = 10;
        PlayerCards1.Padding = new Padding(0, 0, 0, 0);

        // Configure PlayerCards2: 4 columns x 10 rows = 40 cards
        PlayerCards2.ColumnCount = 4;
        PlayerCards2.RowCount = 10;
        PlayerCards2.Padding = new Padding(0, 0, 0, 0);

        for (int i = 0; i < 80; i++)
        {
            int slotNum = i + 1;
            PlayerCard card = new PlayerCard(slotNum);
            card.Name = $"PlayerCard_{slotNum}";
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0);
            card.Padding = new Padding(0);
            card.ToggleSlot((i) < theInstance!.gameMaxSlots ? true : false);
            playerCards[i] = card;

            // Distribute cards: 0-39 to PlayerCards1, 40-79 to PlayerCards2
            if (i < 40)
            {
                // Cards 1-40: column = i / 10, row = i % 10
                PlayerCards1.Controls.Add(card, i / 10, i % 10);
            }
            else
            {
                // Cards 41-80: adjust index and add to second table
                int adjustedIndex = i - 40;
                PlayerCards2.Controls.Add(card, adjustedIndex / 10, adjustedIndex % 10);
            }
        }
    }

    public tabPlayers()
    {
        InitializeComponent();
        
        // Subscribe to snapshot updates
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;
    }

    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }
        
        // Update Player Cards
        tickerPlayerHook();
    }

    public void tickerPlayerHook()
    {
        playerTable1.SuspendLayout();
        playerTable2.SuspendLayout();
        try
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                functionEvent_GeneratePlayerCards();
            }

            if (theInstance!.instanceStatus == InstanceStatus.OFFLINE)
            {
                for (int i = 0; i < 80; i++)
                {
                    playerCards[i].UpdateCard(null, false);
                }
            }
            else
            {
                for (int i = 0; i < theInstance.gameMaxSlots; i++)
                {
                    // Block update for players over 80 (max capacity across both tables)
                    if (i >= 80)
                    {
                        break;
                    }

                    int slotNum = i + 1;
                    if (playerInstance.PlayerList.TryGetValue(slotNum, out var playerInfo))
                    {
                        playerCards[i].UpdateCard(playerInfo, true);
                    }
                    else
                    {
                        playerCards[i].UpdateCard(null, true);
                    }
                }
            }
        }
        finally
        {
            playerTable1.ResumeLayout();
            playerTable2.ResumeLayout();
        }
    }
}