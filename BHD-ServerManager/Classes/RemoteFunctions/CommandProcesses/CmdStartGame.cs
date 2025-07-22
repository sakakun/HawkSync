using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdStartGame
    {

        public static CommandResponse ProcessCommand(object data)
        {

            bool isStarted = StartServer.startGame();

            return new CommandResponse
            {
                Success = isStarted,
                Message = $"The command to start the server was recieve and run.",
                ResponseData = isStarted.ToString()
            };
        }

    }
}
