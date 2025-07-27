using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.RemoteFunctions;
using BHD_SharedResources.Classes.InstanceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Message = $"Returned response of '{isValid.ToString()}'",
                ResponseData = isValid
            };
        }

    }
}
