using HawkSyncShared;
using ServerManager.Forms.SubPanels;
using System.ComponentModel;
using ServerManager.Forms.SubPanels.tabPlayers;

namespace ServerManager.Forms.Panels
{
    public partial class tabPlayers : UserControl
    {
        private readonly Dictionary<int, PlayerCardV2> PlayerCards = new Dictionary<int, PlayerCardV2>();
        
        private static bool IsDesignTime =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime || System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");

        public tabPlayers()
        {
            InitializeComponent();
            
            if (IsDesignTime)
                return;
            
            generateCardData();
            PopulateCards();
            StartSingleTicker();
        }

        private void generateCardData()
        {
            for (int i = 1; i < 51; i++)
            {
                PlayerCardV2 playerCard = new PlayerCardV2(i);
                playerCard.Name = $"PlayerCard_{i}";
                playerCard.Dock = DockStyle.Fill;
                playerCard.Margin = new Padding(0);
                playerCard.Padding = new Padding(0);
                playerCard.Visible = false;

                PlayerCards[i] = playerCard;
            }
        }
        
        private void PopulateCards()
        {
            TableLayoutPanel PlayerTable = playerTable;
            
            PlayerTable.Controls.Clear();

            PlayerTable.ColumnCount = 5;
            PlayerTable.RowCount = 10;
            PlayerTable.Padding = new Padding(0, 0, 0, 0);

            for (int i = 0; i < 50; i++)
            {
                PlayerTable.Controls.Add(PlayerCards[i + 1], i / 10, i % 10);
            }
            // No per-card StartTicker() here anymore
        }

        private void StartSingleTicker()
        {
            CommonCore.Ticker!.Start("TabPlayers_Cards", 1000, TickAllCards);
        }

        /// <summary>
        /// Called from a background timer thread once per second.
        /// Posts a single BeginInvoke to the UI thread to update all cards.
        /// </summary>
        private void TickAllCards()
        {
            if (IsDisposed || !IsHandleCreated) return;

            BeginInvoke(UpdateAllCards);
        }

        /// <summary>
        /// Runs on the UI thread. Iterates all cards and updates each one synchronously.
        /// </summary>
        private void UpdateAllCards()
        {
            
            if(!Visible) return; // Skip updates if the tab isn't visible
            
            foreach (var card in PlayerCards.Values)
            {
                card.UpdateCard();
            }
        }
    }
}
