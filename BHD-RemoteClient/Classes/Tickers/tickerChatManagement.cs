using BHD_RemoteClient.Classes.GameManagement;
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

        private static readonly object tickerLock = new();
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

                // Only process chat when server is online or in start delay
                if (thisInstance.instanceStatus != InstanceStatus.ONLINE &&
                    thisInstance.instanceStatus != InstanceStatus.STARTDELAY)
                {
                    // return;
                }

                // Ensure the chat tab is initialized before proceeding
                thisServer.ChatTab.ChatTickerHook();


                if (ClientMemory.ReadMemoryIsProcessAttached())
                {
                    // --- Incoming Chat Messages ---
                    // Process latest chat message and update UI (non-blocking)
                    Task.Run(ProcessChatMessages);

                    // Process auto messages (non-blocking)
                    Task.Run(ProcessAutoMessages);
                    // Process Queued Messages (non-blocking)
                    Task.Run(ProcessQueuedMessages);
                }
                else
                {
                    AppDebug.Log("Remote: tickerChatManagement", "Server process is not attached. Ticker Skipping.");
                }
            }
        }

        public static void ProcessChatMessages()
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

                // Send Console Message to Server for Processing

                AppDebug.Log("tickerChatManagement", $"Console Message: {lastMessage}");
            }

        }

        public static void ProcessAutoMessages()
        {
            
        }

        public static void ProcessQueuedMessages()
        {
            
        }

    }
}