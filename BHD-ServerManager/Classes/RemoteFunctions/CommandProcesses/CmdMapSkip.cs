using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdMapSkip")]
    public static class CmdMapSkip
    {

        public static CommandResponse ProcessCommand(object data)
        {
            chatInstanceManagers.SendMessageToQueue(true, 0, "resetgames");
            // GameManager.WriteMemorySendConsoleCommand("resetgames");

            return new CommandResponse
            {
                Success = true,
                Message = $"The command to skip the map was recieve and run.",
                ResponseData = true.ToString()
            };
        }

    }
}
