using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.Services;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.Tickers;
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
        private theInstance? theInstance => CommonCore.theInstance;
		private bool _initialized = false;

        public tabBans()
        {
            InitializeComponent();
            
            // Use VisibleChanged which fires more reliably
            this.VisibleChanged += TabBans_VisibleChanged;
        }

        private async void TabBans_VisibleChanged(object? sender, EventArgs e)
        {
            // Only initialize once when first becoming visible
            if (this.Visible && !_initialized)
            {
                _initialized = true;

                // NetLimiterClient.StartBridgeProcess();

                // await tickerNetLimiterMonitor.InitializeAsync(Path.Combine(theInstance.profileServerPath,"dfbhd.exe"));

			}
        }

        
    }
}