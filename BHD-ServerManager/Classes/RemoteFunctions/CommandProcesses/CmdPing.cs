using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdPing")]
    public static class CmdPing
    {

        public static CommandResponse ProcessCommand(object data)
        {
            return new CommandResponse
            {
                Success = true,
                Message = $"Pong",
                ResponseData = null
            };
        }

    }
}
