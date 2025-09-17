using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using Windows.Media.AppBroadcasting;
using Windows.UI.Notifications;

namespace BHD_ServerManager.Classes.RemoteFunctions.ConsoleCommandProcesses
{
    [ConsoleCommandHandler("show")]
    public static class ConsoleCmdShow
    {

        private static consoleInstance intanceConsole => CommonCore.instanceConsole!;

        public static CommandResponse ProcessCommand(string AuthToken, string[] data)
        {

            //          0   1    2
            // Example: !rc show "online-admins"

            switch (data[2])
            {
                case "admin-chat":
                default:
                    {
                        // Function to trigger the console window to show online admins in the console.
                        ShowAdminChat(AuthToken);
                        break;
                    }
            }

            return new CommandResponse
            {
                Success = true,
                Message = string.Empty,
                ResponseData = null
            };
        }


        private static void ShowAdminChat(string AuthToken)
        {
            var NotificationLines = new Dictionary<int, string>();
            string padding2 = new string('_', 22); // Always safe in Windows-1252

            // Header
            NotificationLines[15] = new string('\u00A0', 50) + "########################";
            NotificationLines[14] = new string('\u00A0', 50) + "# Online Admins";
            NotificationLines[13] = new string('\u00A0', 50) + "#";

            // Get online admins from AuthorizedClients
            var onlineAdmins = RemoteServer.AuthorizedClients
                .Where(c => c.User.IsOnline)
                .Select(c => c.User)
                .ToList();

            int lineIdx = 12;
            foreach (var admin in onlineAdmins)
            {
                // Format: "#_(XX)_UserName__________#"
                string userId = admin.UserId.ToString().PadLeft(2, '0');
                string userName = admin.Username.Replace(' ', '_');
                string formatted = $"# {userId} - {userName}";

                NotificationLines[lineIdx] = new string('\u00A0', 50) + formatted;
                lineIdx--;
                if (lineIdx < 3) break; // Only show up to 11 admins (lines 13..3)
            }

            // Fill remaining lines with padding
            for (; lineIdx >= 3; lineIdx--)
                NotificationLines[lineIdx] = new string('\u00A0', 50) + "#";

            NotificationLines[2] = new string('\u00A0', 50) + "#";
            NotificationLines[1] = new string('\u00A0', 50) + "#";
            NotificationLines[0] = new string('\u00A0', 50) + "########################";

            // Update the console for this admin
            if (intanceConsole.AdminConsoles.TryGetValue(AuthToken, out var console))
            {
                foreach (var kvp in NotificationLines)
                    console.NotificationLines[kvp.Key] = kvp.Value;
            }

            AppDebug.Log("ShowOnlineAdmins", "Server Side Console Updated.");
        }

    }
}
