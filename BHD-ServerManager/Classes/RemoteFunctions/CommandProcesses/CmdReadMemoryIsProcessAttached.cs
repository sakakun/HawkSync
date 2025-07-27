using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdReadMemoryIsProcessAttached")]
    public static class CmdReadMemoryIsProcessAttached
    {

        public static CommandResponse ProcessCommand(object data)
        {
            bool isAttached = ServerMemory.ReadMemoryIsProcessAttached();

            return new CommandResponse
            {
                Success = isAttached,
                Message = $"Game Server Attached? ({isAttached.ToString()})",
                ResponseData = isAttached.ToString()
            };
        }

    }
}
