﻿using BHD_RemoteClient.Classes.GameManagement;
using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Windows.Forms;

namespace BHD_RemoteClient.Classes.Tickers
{
    public static class tickerChatManagement
    {
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static ServerManager thisServer => Program.ServerManagerUI!;
        private static consoleInstance instanceConsole => CommonCore.instanceConsole!;

        private static readonly object tickerLock = new();
        private static readonly object tickerLock2 = new();
        private static readonly object tickerLock3 = new();
        private static bool _autoMessageRecoveryDone = false;

        // For deduplication of chat messages
        private static string? _lastProcessedPlayerName = null;
        private static string? _lastProcessedMessageText = null;

        // Helper for UI thread safety

        public static void runTicker()
        {
            // Always ensure UI thread safety for UI-bound operations
            if (thisServer.InvokeRequired)
            {
                try
                {
                    thisServer.BeginInvoke(new Action(runTicker));
                }
                catch (Exception ex)
                {
                    AppDebug.Log("Remote: tickerChatManagement", $"Error invoking runTicker: {ex.Message}");
                }
                return;
            }

            lock (tickerLock)
            {

                // Ensure the chat tab is initialized before proceeding
                thisServer.ChatTab.ChatTickerHook();


                if (ClientMemory.ReadMemoryIsProcessAttached())
                {
                    // --- Incoming Chat Messages ---
                    // Process latest chat message and update UI (non-blocking)
                    Task.Run(ProcessConsoleMessages);
                    // Process auto messages (non-blocking)
                    Task.Run(ProcessConsoleInjection);
                    // Process Queued Messages (non-blocking)
                    Task.Run(ProcessQueuedMessages);
                }
                else
                {
                    AppDebug.Log("Remote: tickerChatManagement", "Server process is not attached. Ticker Skipping.");
                }
            }
        }

        public static void ProcessConsoleMessages()
        {

            lock (tickerLock)
            {

                //var latestMessage = ClientMemory.ReadMemoryLastChatMessage();
                var latestMessage = ClientMemory.ReadMemoryConsoleMessage();

                string lastMessage = latestMessage;

                if (_lastProcessedMessageText != null || lastMessage == _lastProcessedMessageText)
                {
                    // Duplicate message, skip processing
                    return;
                }

                _lastProcessedMessageText = lastMessage;

                AppDebug.Log("tickerChatManagement", $"Console Message: {lastMessage}");

                if (lastMessage.StartsWith("!rc ") && !lastMessage.StartsWith("!rc client "))
                {
                    // Remote Server Command
                    CmdSendConsoleCommand.ProcessCommand(lastMessage);
                    return;
                } else {                     // Only add to chat if it's not a command
                    if (!lastMessage.StartsWith("!rc client "))
                    {
                        // Handle Console Activation Commands
                        instanceConsole.ConsoleActive = lastMessage switch
                        {
                            "!rc client console on" => true,
                            "!rc client console off" => false,
                            _ => instanceConsole.ConsoleActive
                        };

                        // Client Side Command
                        return;
                    }
                }
                
            }

        }

        public static void ProcessConsoleInjection()
        {
            lock (tickerLock2)
            {

                if (instanceConsole.ConsoleActive)
                {
                    for (int line = 0; line < 16; line++)
                    {
                        ClientMemory.InjectMessage(1, instanceConsole.ClientConsole.NotificationLines[line], 90, 0, line);
                        ClientMemory.InjectMessage(0, instanceConsole.ClientConsole.ChatLines[line], 90, 0, line);
                    }
                }

            }
        }

        public static void ProcessQueuedMessages()
        {
            lock (tickerLock3)
            {
                string AuthToken = Program.theRemoteClient!.AuthToken;

                Dictionary<int, string> messagesToProcess = instanceConsole.AdminDirectMessages[AuthToken];
                // instanceConsole.ClientDirectMessages
                if (AuthToken == null || AuthToken == "")
                {
                    return;
                }

                if (messagesToProcess.Count > instanceConsole.ClientDirectMessages.Count)
                {
                    foreach (var kvp in messagesToProcess)
                    {
                        if (!instanceConsole.ClientDirectMessages.ContainsKey(kvp.Key))
                        {

                            string decoded = System.Text.Encoding.GetEncoding("Windows-1252").GetString(Convert.FromBase64String(kvp.Value));

                            // Echo the new message
                            AppDebug.Log("ProcessQueuedMessages", $"New Direct Message [{kvp.Key}]: {decoded}");

                            // Add to ClientDirectMessages
                            instanceConsole.ClientDirectMessages[kvp.Key] = kvp.Value;

                            ClientMemory.PushMessage(0, kvp.Value, 909, 2000);
                        }
                    }
                }
            }
        }
    }
}