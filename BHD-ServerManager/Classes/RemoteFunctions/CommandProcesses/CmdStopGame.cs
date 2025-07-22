using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdStopGame
    {

        public static CommandResponse ProcessCommand(object data)
        {

            bool isStopped = StartServer.stopGame();

            return new CommandResponse
            {
                Success = isStopped,
                Message = $"The command to stop the server was recieve and run.",
                ResponseData = isStopped.ToString()
            };
        }

    }
}
