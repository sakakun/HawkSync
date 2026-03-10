using BHD_ServerManager.Classes.PlayerManagementClasses;
using HawkSyncShared;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabPlayers : UserControl
    {
        private theInstance? theInstance => CommonCore.theInstance;
        private playerInstance playerInstance => CommonCore.instancePlayers!;

        // --- Class Variables ---
        private bool _firstLoadComplete = false;

        private PlayerCard[] playerCards = new PlayerCard[80];

        // --- Generate Init. Cards ---
        private void functionEvent_GeneratePlayerCards()
        {
            TableLayoutPanel PlayerCards1 = playerTable1; // 50 cards (1-50)
            
            PlayerCards1.Controls.Clear();

            // Configure PlayerCards1: 5 columns x 8 rows = 40 cards
            PlayerCards1.ColumnCount = 5;
            PlayerCards1.RowCount = 10;
            PlayerCards1.Padding = new Padding(0, 0, 0, 0);

            for (int i = 0; i < 50; i++)
            {
                int slotNum = i + 1;
                PlayerCard card = new PlayerCard(slotNum);
                card.Name = $"PlayerCard_{slotNum}";
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(0);
                card.Padding = new Padding(0);
                card.ToggleSlot((i) < theInstance!.gameMaxSlots ? true : false);
                playerCards[i] = card;

                // Cards 1-40: column = i / 8, row = i % 8
                PlayerCards1.Controls.Add(card, i / 10, i % 10);
            }
        }

        public tabPlayers()
        {
            InitializeComponent();

            CommonCore.Ticker?.Start("tabPlayersTicker", 1000, tickerPlayerHook);
        }

        public void tickerPlayerHook()
        {

            if (InvokeRequired)
            {
                Invoke(new Action(tickerPlayerHook));
                return;
            }

            playerTable1.SuspendLayout();

            try
            {
                if (!_firstLoadComplete)
                {
                    _firstLoadComplete = true;
                    functionEvent_GeneratePlayerCards();
                }

                if (theInstance!.instanceStatus == InstanceStatus.OFFLINE)
                {
                    for (int i = 0; i < 50; i++)
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
            catch (Exception ex)
            {
                AppDebug.Log("tickerPlayerHook", $"Error in tickerPlayerHook: {ex.Message}");
            }
            finally
            {
                playerTable1.ResumeLayout();
            }
        }
    }
}
