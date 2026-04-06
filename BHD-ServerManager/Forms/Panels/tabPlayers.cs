using ServerManager.Forms.SubPanels;
using System.ComponentModel;

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
            
            // Generate Cards
            generateCardData();
            PopulateCards();
            
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
        
        // --- Generate Init. Cards ---
        private void PopulateCards()
        {
            TableLayoutPanel PlayerTable = playerTable;
            
            PlayerTable.Controls.Clear();

            // Configure PlayerCards1: 5 columns x 10 rows = 50 cards
            PlayerTable.ColumnCount = 5;
            PlayerTable.RowCount = 10;
            PlayerTable.Padding = new Padding(0, 0, 0, 0);

            for (int i = 0; i < 50; i++)
            {
                PlayerTable.Controls.Add(PlayerCards[i + 1], i / 10, i % 10);
                
            }
            
            for (int i = 1; i < 51; i++)
            {
                PlayerCards[i].StartTicker();
            }
            
        }
        
    }
}
