using BHD_ServerManager.Forms;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdResetAvailableMaps")]
    public static class CmdResetAvailableMaps
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {

            thisServer.MapsTab.functionEvent_ResetAvailableMaps();

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = string.Empty
            };
        }

    }
}
