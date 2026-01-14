using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
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

        public tabBans()
        {
            InitializeComponent();
        }
    }
}