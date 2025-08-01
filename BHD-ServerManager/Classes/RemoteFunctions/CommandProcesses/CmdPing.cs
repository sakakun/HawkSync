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
                Message = string.Empty,
                ResponseData = null
            };
        }

    }
}
