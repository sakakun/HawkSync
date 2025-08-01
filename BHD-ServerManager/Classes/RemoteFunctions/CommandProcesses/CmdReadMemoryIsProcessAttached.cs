using BHD_ServerManager.Classes.GameManagement;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdReadMemoryIsProcessAttached")]
    public static class CmdReadMemoryIsProcessAttached
    {

        public static CommandResponse ProcessCommand(object data)
        {
            bool isAttached = ServerMemory.ReadMemoryIsProcessAttached();

            return new CommandResponse
            {
                Success = isAttached,
                Message = string.Empty,
                ResponseData = isAttached.ToString()
            };
        }

    }
}
