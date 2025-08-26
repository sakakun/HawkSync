using BHD_ServerManager.Classes.GameManagement;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text.Json;

namespace BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses
{
    [CommandHandler("CmdStartGame")]
    public static class CmdStartGame
    {
        public static CommandResponse ProcessCommand(object data)
        {
            theInstanceManager.SetServerVariables();
            bool isStarted = StartServer.startGame();

            // Wait for the server to be fully running (max 10 seconds)
            int waitMs = 0;
            while (!ServerMemory.ReadMemoryIsProcessAttached() && waitMs < 10000)
            {
                Thread.Sleep(500);
                waitMs += 500;
            }

            bool isRunning = ServerMemory.ReadMemoryIsProcessAttached();

            CommandResponse response = new CommandResponse
            {
                Success = isStarted && isRunning,
                Message = isRunning
                    ? "The server was started successfully."
                    : "The server start command was issued, but the server did not start in time.",
                ResponseData = isRunning.ToString()
            };

            // After creating CommandResponse
            AppDebug.Log("CmdStartGame", $"Sending response: {JsonSerializer.Serialize(response)}");
            // Log any network exceptions

            return response;
        }

    }
}
