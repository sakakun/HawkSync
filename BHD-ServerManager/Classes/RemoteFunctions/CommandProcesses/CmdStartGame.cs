using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdStartGame")]
    public static class CmdStartGame
    {
        public static CommandResponse ProcessCommand(object data)
        {
            bool isStarted = StartServer.startGame();

            // Wait for the server to be fully running (max 10 seconds)
            int waitMs = 0;
            while (!ServerMemory.ReadMemoryIsProcessAttached() && waitMs < 10000)
            {
                Thread.Sleep(500);
                waitMs += 500;
            }

            bool isRunning = ServerMemory.ReadMemoryIsProcessAttached();

            return new CommandResponse
            {
                Success = isStarted && isRunning,
                Message = isRunning
                    ? "The server was started successfully."
                    : "The server start command was issued, but the server did not start in time.",
                ResponseData = isRunning.ToString()
            };
        }

    }
}
