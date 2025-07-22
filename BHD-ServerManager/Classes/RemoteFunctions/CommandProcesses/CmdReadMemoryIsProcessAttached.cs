using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdReadMemoryIsProcessAttached
    {

        public static CommandResponse ProcessCommand(object data)
        {
            return new CommandResponse
            {
                Success = true,
                Message = $"Pong",
                ResponseData = ServerMemory.ReadMemoryIsProcessAttached().ToString()
            };
        }

    }
}
