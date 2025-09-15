namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdPing")]
    public static class ConsoleCmdPing
    {

        public static CommandResponse ProcessCommand(object data)
        {
            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = null
            };
        }

    }
}
