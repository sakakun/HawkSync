using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdMapScore")]
    public static class CmdMapScore
    {

        public static CommandResponse ProcessCommand(object data)
        {

            GameManager.WriteMemoryScoreMap();

            return new CommandResponse
            {
                Success = true,
                Message = $"The command toscore the map was recieve and run.",
                ResponseData = true.ToString()
            };
        }

    }
}
