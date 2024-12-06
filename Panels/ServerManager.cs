using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using ServerManager.Classes.Enviroment;
using ServerManager.Classes.Modules;

namespace ServerManager.Panels
{
    public partial class ServerManager : Form
    {
        protected ServerEnvironment Env;
    
        public ServerManager()
        {
            Load += ServerManager_Load;
            InitializeComponent();
            
        }
        
        private void ServerManager_Load(object sender, EventArgs e)
        {
            Env = ServerEnvironment.Instance;

        }
        
        
    }
}