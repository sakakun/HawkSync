using BHD_SharedResources.Classes.GameManagement;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdMapSkip")]
    public static class CmdMapSkip
    {

        public static CommandResponse ProcessCommand(object data)
        {

            GameManager.WriteMemorySendConsoleCommand("resetgames");

            return new CommandResponse
            {
                Success = true,
                Message = $"The command to skip the map was recieve and run.",
                ResponseData = true.ToString()
            };
        }

    }
}
