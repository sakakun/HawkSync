using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabBans : UserControl
    {

        // --- Instance Objects ---
        private banInstance? instanceBans => CommonCore.instanceBans;
        private bool _initialized = false;

        public tabBans()
        {
            InitializeComponent();
            
            // Use VisibleChanged which fires more reliably
            this.VisibleChanged += TabBans_VisibleChanged;
        }

        private void TabBans_VisibleChanged(object? sender, EventArgs e)
        {
            // Only initialize once when first becoming visible
            if (this.Visible && !_initialized)
            {
                _initialized = true;
                // NetLimiterService.InitializeNetLimiter();
			}
        }

        
    }
}