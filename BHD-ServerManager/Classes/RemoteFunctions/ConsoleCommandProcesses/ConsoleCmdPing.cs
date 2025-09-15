using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using Windows.Media.AppBroadcasting;

namespace BHD_ServerManager.Classes.RemoteFunctions.ConsoleCommandProcesses
{
    [ConsoleCommandHandler("ping")]
    public static class ConsoleCmdPing
    {

        private static consoleInstance intanceConsole => CommonCore.instanceConsole!;

        public static CommandResponse ProcessCommand(string AuthToken, string[] data)
        {
            // !rc ping
            // Send a message to the Admin client to verify connectivity.
            intanceConsole.AdminDirectMessages[AuthToken].Add(intanceConsole.AdminDirectMessages[AuthToken].Count, "Server Console: Pong");

            AppDebug.Log("ConsoleCmdPing", "Ping Command Ran");

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = null
            };
        }

    }
}
