using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
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
    public partial class tabBans : UserControl
    {

        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        private banInstance instanceBans => CommonCore.instanceBans;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "BanTab";                        // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        public tabBans()
        {
            InitializeComponent();
        }

        public void BanTickerHook()
        {
            // Ensure the first load is complete before proceeding with updates.
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
                return;
            }
        }
}
