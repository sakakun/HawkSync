using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabPlayers : UserControl
    {
        private theInstance theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;

        // --- Class Variables ---
        private new string Name = "PlayerTab";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        private Dictionary<int, PlayerCard> playerCards = new Dictionary<int, PlayerCard>();            // Array to hold player cards for easy access.
        // --- Generate Init. Cards ---
        private void functionEvent_GeneratePlayerCards()
        {

            TableLayoutPanel PlayerCards = playerLayout;
            PlayerCards.Controls.Clear();

            // Ensure the TableLayoutPanel has 5 columns and 10 rows
            PlayerCards.ColumnCount = 5;
            PlayerCards.RowCount = 10;

            for (int i = 0; i < 50; i++)
            {
                int slotNum = i + 1;
                PlayerCard card = new PlayerCard();
                card.Name = $"PlayerCard_{slotNum}";
                card.Dock = DockStyle.Fill;
                card.ToggleSlot(slotNum, (i) < theInstance.gameMaxSlots ? true : false);
                playerCards[slotNum] = card;

                // Column by column: column = i / 10, row = i % 10
                PlayerCards.Controls.Add(card, i / 10, i % 10);
            }

        }

        public tabPlayers()
        {
            InitializeComponent();
        }

        public void tickerPlayerHook()
        {
            // Check if the first load is complete
            if (!_firstLoadComplete)
            {
                // Set the first load complete flag to true
                _firstLoadComplete = true;
                // Get the server settings on first load
                functionEvent_GeneratePlayerCards();
            }

            if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
            {
                for(int i = 0; i < 50; i++)
                {
                    playerCards[i + 1].ToggleSlot(i + 1, false);
                }
            } else
            {
                for (int i = 0; i < theInstance.gameMaxSlots; i++)
                {
                    int slotNum = i + 1;
                    if (theInstance.playerList.ContainsKey(slotNum))
                    {
                        playerCards[slotNum].ToggleSlot(slotNum, true);
                        playerCards[slotNum].UpdateStatus(theInstance.playerList[slotNum]);
                    }
                    else
                    {
                        playerCards[slotNum].ToggleSlot(slotNum, true);
                    }
                }
            }
        }
    }
}
