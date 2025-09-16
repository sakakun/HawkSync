using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.Instances
{
    public class consoleInstance
    {
        // Broadcasted Variables
        [JsonInclude]
        public Dictionary<string, consoleWindow> AdminConsoles = new();

        [JsonInclude]
        public Dictionary<string, Dictionary<int, string>> AdminDirectMessages = new();
        

        // New: Per-client, per-type cancellation tokens
        [JsonIgnore]
        public Dictionary<string, CancellationTokenSource> AdminChatLineTasks = new(); // AuthToken -> CTS for ChatLines
        [JsonIgnore]
        public Dictionary<string, CancellationTokenSource> AdminNotificationLineTasks = new(); // AuthToken -> CTS for NotificationLines

        // Local Variables For Client
        [JsonIgnore]
        public consoleWindow            ClientConsole { get; set; } = new consoleWindow();                              // The Client's Console Window (Handled Locally)
        [JsonIgnore]
        public Dictionary<int, string>  ClientDirectMessages { get; set; } = new Dictionary<int, string>();                       // The Client's Direct Messages (Handled Locally), once processed will be placed here.
        [JsonIgnore]
        public bool                     ConsoleActive { get; set; } = false;                              // Show or Hide the Console (Handled Locally)
    }

    public class consoleWindow
    {

        // Notification Max Message Length = 53 Characters to stay on Screen - Starts half way on screen.

        // Extended Chat Message Lengths (Console and Notifications/Chat)
        // Chat Max Message Length = 80 Characters to stay on Screen
        // Notification Mac Message Lenth = 74 (50 Spaces + 24 Characters)

        // int = line number 0 - 17, string = text
        // Chat lines, 0 - 3 show on the screen, 0-17 appear in the chat window.
        // Notification lines, 0 - 5 show on the screen, 0-17 appear in the chat window.
        public Dictionary<int, string> ChatLines;
        public Dictionary<int, string> NotificationLines;
    
        public consoleWindow()
        {
            string padding = new string('_', 78); // Always safe in Windows-1252
            ChatLines = new Dictionary<int, string>();
            ChatLines.Add(15, "################################################################################");
            ChatLines.Add(14, "#" + padding + "#");
            ChatLines.Add(13, "#" + padding + "#");
            ChatLines.Add(12, "#" + padding + "#");
            ChatLines.Add(11, "#" + padding + "#");
            ChatLines.Add(10, "#" + padding + "#");
            ChatLines.Add( 9, "#" + padding + "#");
            ChatLines.Add( 8, "#" + padding + "#");
            ChatLines.Add( 7, "#" + padding + "#");
            ChatLines.Add( 6, "#" + padding + "#");
            ChatLines.Add( 5, "#" + padding + "#");
            ChatLines.Add( 4, "#" + padding + "#");
            ChatLines.Add( 3, "#" + padding + "#");
            ChatLines.Add( 2, "#" + padding + "#");
            ChatLines.Add( 1, "#" + padding + "#");
            ChatLines.Add( 0, "################################################################################");

            string padding2 = new string('_', 22); // Always safe in Windows-1252
            NotificationLines = new Dictionary<int, string>();
            NotificationLines.Add(15, new string('\u00A0', 50) + "########################");
            NotificationLines.Add(14, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add(13, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add(12, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add(11, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add(10, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 9, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 8, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 7, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 6, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 5, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 4, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 3, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 2, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 1, new string('\u00A0', 50) + "#" + padding2 + "#");
            NotificationLines.Add( 0, new string('\u00A0', 50) + "########################");
        }

    }



}
