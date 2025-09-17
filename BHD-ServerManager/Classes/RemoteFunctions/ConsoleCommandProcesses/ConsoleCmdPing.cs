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
            string message = "Server Console: Pong";

            string encodedMessage = Convert.ToBase64String(System.Text.Encoding.GetEncoding("Windows-1252").GetBytes(message));

            intanceConsole.AdminDirectMessages[AuthToken].Add(intanceConsole.AdminDirectMessages[AuthToken].Count + 1, encodedMessage);
            
            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = null
            };
        }

    }
}
