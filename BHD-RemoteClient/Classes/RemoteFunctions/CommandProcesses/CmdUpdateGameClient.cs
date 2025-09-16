using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;

namespace BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses
{
    public static class CmdUpdateGameClient
    {

        private static theInstance theInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;
        private static statInstance instanceStats => CommonCore.instanceStats!;
        private static consoleInstance instanceConsole => CommonCore.instanceConsole!;

        public static void ProcessUpdateData(InstanceUpdatePacket UpdateData)
        {
            try
            {
                // Ideally there shouldn't be any null values, but let's handle it gracefully

                ProcessingTheInstance(UpdateData.theInstance);
                ProcessingChatInstance(UpdateData.chatInstance);
                ProcessingBanInstance(UpdateData.banInstance);
                ProcessingMapInstance(UpdateData.mapInstance);
                ProcessingAdminInstance(UpdateData.adminInstance);
                ProcessingStatInstance(UpdateData.statInstance);
                ProcessingConsoleInstance(UpdateData.consoleInstance);
                
                AppDebug.Log("RemoteClient", $"Client-side Mailboxes: {instanceConsole.AdminDirectMessages.Count}, Consoles: {instanceConsole.AdminConsoles.Count}");

            }
            catch (Exception ex)
            {
                AppDebug.Log("UpdateGameClient", "Error processing: " + ex.Message);
                return;
            }
            AppDebug.Log("UpdateGameClient", "Processing completed successfully");

        }

        public static void ProcessingTheInstance(theInstance updatedInstance)
        {
            if (updatedInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing theInstance update");

            var type = typeof(theInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                // Skip properties with [JsonIgnore]
                if (prop.GetCustomAttributes(typeof(System.Text.Json.Serialization.JsonIgnoreAttribute), true).Length > 0)
                    continue;

                try
                {
                    if (prop.Name == "instanceAttachedPID") { 
                        AppDebug.Log("UpdateGameClient", $"'{prop.Name}': {prop.GetValue(updatedInstance)}");
                    }
                    var updatedValue = prop.GetValue(updatedInstance);
                    prop.SetValue(theInstance, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }

        }
        public static void ProcessingChatInstance(chatInstance updatedChatInstance)
        {
            if (updatedChatInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing updatedChatInstance update");

            var type = typeof(chatInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    var updatedValue = prop.GetValue(updatedChatInstance);
                    prop.SetValue(instanceChat, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }
        }
        public static void ProcessingBanInstance(banInstance updatedBanInstance)
        {
            if (updatedBanInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing updatedBanInstance update");

            var type = typeof(banInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    var updatedValue = prop.GetValue(updatedBanInstance);
                    prop.SetValue(instanceBans, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }
        }
        public static void ProcessingMapInstance(mapInstance updatedMapInstance)
        {
            if (updatedMapInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing updatedMapInstance update");

            var type = typeof(mapInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    var updatedValue = prop.GetValue(updatedMapInstance);
                    prop.SetValue(instanceMaps, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }
        }
        public static void ProcessingAdminInstance(adminInstance updatedAdminInstance)
        {
            if (updatedAdminInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing updatedAdminInstance update");

            var type = typeof(adminInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    var updatedValue = prop.GetValue(updatedAdminInstance);
                    prop.SetValue(instanceAdmin, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }
        }
        public static void ProcessingStatInstance(statInstance updatedStatInstance)
        {
            if (updatedStatInstance == null) return;
            AppDebug.Log("UpdateGameClient", "Processing updatedStatInstance update");

            var type = typeof(statInstance);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

                try
                {
                    var updatedValue = prop.GetValue(updatedStatInstance);
                    prop.SetValue(instanceStats, updatedValue);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("UpdateGameClient", $"Failed to set property '{prop.Name}': {ex.Message}");
                }
            }
        }
        public static void ProcessingConsoleInstance(consoleInstance updatedConsoleInstance)
        {
            if (updatedConsoleInstance == null) return;
            AppDebug.Log("updatedConsoleInstance", "Processing updatedConsoleInstance update");

            // Ensure all keys from the update exist locally, and merge entries
            try
            {
                foreach (var kvp in updatedConsoleInstance.AdminDirectMessages)
                {
                    if (!instanceConsole.AdminDirectMessages.ContainsKey(kvp.Key))
                    {
                        AppDebug.Log("updatedConsoleInstance", $"Adding Key {kvp.Key}");
                        instanceConsole.AdminDirectMessages[kvp.Key] = new Dictionary<int, string>();
                    }

                    var currentDict = instanceConsole.AdminDirectMessages[kvp.Key];

                    // FIX: Use kvp.Value (the updated data) instead of tempDict
                    foreach (var innerKvp in kvp.Value)
                    {
                        AppDebug.Log("updatedConsoleInstance", $"Adding Entry for {kvp.Key} {innerKvp.Key} - {innerKvp.Value}");
                        currentDict[innerKvp.Key] = innerKvp.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("updatedConsoleInstance", $"Error updating AdminDirectMessages: {ex.Message}");
            }

            try
            {
                foreach (var kvp in updatedConsoleInstance.AdminConsoles)
                {
                    if (!instanceConsole.AdminConsoles.ContainsKey(kvp.Key))
                    {
                        instanceConsole.AdminConsoles[kvp.Key] = new consoleWindow();
                    }
                    instanceConsole.AdminConsoles[kvp.Key] = kvp.Value;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("updatedConsoleInstance", $"Error updating AdminConsoles: {ex.Message}");
            }

            //consoleInstanceManager.updateConsoleWindow(Program.theRemoteClient!.AuthToken);
        }
    }
}
