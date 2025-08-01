using BHD_SharedResources.Classes.GameManagement;

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
