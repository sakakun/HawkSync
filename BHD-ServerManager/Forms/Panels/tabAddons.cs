using BHD_ServerManager.Forms.Panels.Addons;
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
    public partial class tabAddons : UserControl
    {
        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;

        // --- Class Variables ---
        private new string Name = "AddonsTab";                      // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.

        public tabChatCommands ChatCommandsTab;
        
        public tabAddons()
        {
            InitializeComponent();

            // Initialize Addon Tabs
            panelChatCommands.Controls.Add(ChatCommandsTab = new tabChatCommands() { Dock = DockStyle.Fill });

        }

        public void TickerAddonsHook()
        {
            if (!_firstLoadComplete)
            {
                _firstLoadComplete = true;
            }
        }
    }
}
