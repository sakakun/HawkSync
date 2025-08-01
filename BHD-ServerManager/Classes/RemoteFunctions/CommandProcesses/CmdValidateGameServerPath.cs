using BHD_SharedResources.Classes.InstanceManagers;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdValidateGameServerPath")]
    public static class CmdValidateGameServerPath
    {
        public static CommandResponse ProcessCommand(object data)
        {
            bool isValid = theInstanceManager.ValidateGameServerPath();

            return new CommandResponse
            {
                Success = isValid,
                Message = string.Empty,
                ResponseData = isValid
            };
        }

    }
}
