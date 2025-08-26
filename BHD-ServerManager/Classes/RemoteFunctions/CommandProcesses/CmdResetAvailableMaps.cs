using BHD_ServerManager.Forms;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdResetAvailableMaps")]
    public static class CmdResetAvailableMaps
    {
        private static ServerManager thisServer => Program.ServerManagerUI!;
        public static CommandResponse ProcessCommand(object data)
        {
            // UI updates that should always run
            SafeInvoke(thisServer, () =>
            {
                thisServer.MapsTab.functionEvent_ResetAvailableMaps();
            });

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = string.Empty
            };
        }

        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

    }
}
