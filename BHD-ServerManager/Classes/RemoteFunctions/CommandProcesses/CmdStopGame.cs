using BHD_ServerManager.Classes.GameManagement;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdStopGame")]
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
