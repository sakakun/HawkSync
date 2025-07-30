using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_ServerManager.Forms;
using BHD_SharedResources.Classes.InstanceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email.DataProvider;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdResetAvailableMaps")]
    public static class CmdResetAvailableMaps
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {

            thisServer.functionEvent_RefreshAvailableMaps();

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = string.Empty
            };
        }

    }
}
