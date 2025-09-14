using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class RemoteClientCommands
    {
        private static consoleInstance instanceConsole => CommonCore.instanceConsole!;

        public static void ProcessCommand(string command)
        {
            // remove leading string "!rc client "
            string trimmedCommand = command.Substring(11).Trim();
            // Split command into parts
            string[] commandParts = trimmedCommand.Split(' ');
            AppDebug.Log("ProcessCommand", $"Latest Console Message {commandParts[0]}");
            switch (commandParts[0])
            {
                case "showconsole":
                    instanceConsole.ConsoleActive = commandParts[1] == "false" ? false : true;
                    AppDebug.Log("showconsole", commandParts.ToString());
                    break;
                default:
                    break;
            }

        }

    }
}
