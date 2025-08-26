using BHD_ServerManager.Classes.PlayerManagementClasses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

        private PlayerCard[] playerCards = new PlayerCard[50];

        // --- Generate Init. Cards ---
        private void functionEvent_GeneratePlayerCards()
        {

            TableLayoutPanel PlayerCards = playerLayout;
            PlayerCards.Controls.Clear();

            // Ensure the TableLayoutPanel has 5 columns and 10 rows
            PlayerCards.ColumnCount = 5;
            PlayerCards.RowCount = 10;
            PlayerCards.Padding = new Padding(0,3,0,0);

            for (int i = 0; i < 50; i++)
            {
                int slotNum = i + 1;
                PlayerCard card = new PlayerCard(slotNum);
                card.Name = $"PlayerCard_{slotNum}";
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(0);
                card.Padding = new Padding(0);
                card.ToggleSlot((i) < theInstance.gameMaxSlots ? true : false);
                playerCards[i] = card;

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
            playerLayout.SuspendLayout();
            try
            {
                if (!_firstLoadComplete)
                {
                    _firstLoadComplete = true;
                    functionEvent_GeneratePlayerCards();
                }

                if (theInstance.instanceStatus == InstanceStatus.OFFLINE)
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
                        int slotNum = i + 1;
                        if (theInstance.playerList.TryGetValue(slotNum, out var playerInfo))
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
                playerLayout.ResumeLayout();
            }
        }
    }
}
