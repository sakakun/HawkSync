using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Classes.Tickers;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BHD_RemoteClient.Classes.InstanceManagers
{
    public class remoteTheInstanceManager : theInstanceInterface
    {
        public static string lastKnownSettingsPath = Path.Combine(CommonCore.AppDataPath, "lastKnownSettings.json");

        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;
        private static ServerManager thisServer => Program.ServerManagerUI!;

        public void changeTeamGameMode(int currentMapType, int nextMapType)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void CheckSettings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void ExportSettings()
        {
            // Export the settings to a JSON file using a ShowFileDialog from CoreManager
            string savePath = CommonCore.AppDataPath;
            string exportPath = Functions.ShowFileDialog(true, "Server Settings (*.svset)|*.svset|All files (*.*)|*.*", "Export Server Settings", savePath, "exportedServerSettings.svset")!;
            if (!string.IsNullOrEmpty(exportPath))
            {
                theInstanceManager.SaveSettings(true, exportPath); // Save the settings to the specified path
            }
        }

        public void GetServerVariables(bool import = false, theInstance updatedInstance = null!)
        {

            var newInstance = import && updatedInstance != null ? updatedInstance : theInstance;

            // Trigger "Gets" for the Tabs
            thisServer.ProfileTab.functionEvent_GetProfileSettings(null, null!);
            thisServer.ServerTab.functionEvent_GetServerSettings((updatedInstance != null ? updatedInstance : null!));
            thisServer.StatsTab.functionEvent_GetStatSettings((updatedInstance != null ? updatedInstance : null!));

        }

        public void ImportSettings()
        {
            string savePath = CommonCore.AppDataPath;
            string importPath = Functions.ShowFileDialog(true, "Server Settings (*.svset)|*.svset|All files (*.*)|*.*", "Export Server Settings", savePath, "importServerSettings.svset")!;
            if (!string.IsNullOrEmpty(importPath))
            {
                theInstanceManager.LoadSettings(true, importPath);
            }
        }

        public void InitializeTickers()
        {
            CommonCore.Ticker.Start("ServerManager", 1000, () => tickerServerManager.runTicker());
            CommonCore.Ticker.Start("ChatManager", 500, () => tickerChatManagement.runTicker());
        }

        public void LoadSettings(bool external, string path)
        {
            string settingsPath = external && !string.IsNullOrWhiteSpace(path) ? path : lastKnownSettingsPath;
            AppDebug.Log("InstanceManager", $"Loading settings from {(external ? "external path" : "default path")}: {settingsPath}");

            if (!File.Exists(settingsPath))
            {
                AppDebug.Log("InstanceManager", $"Settings file not found at {settingsPath}. Using default settings.");
                return;
            }

            try
            {
                var tempInstance = JsonSerializer.Deserialize<theInstance>(File.ReadAllText(settingsPath));
                if (tempInstance == null) return;

                theInstanceManager.GetServerVariables(true, tempInstance); // Get the variables from the tempInstance

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("InstanceManager", $"Failed to load settings: {ex.Message}");
            }
        }

        public void SaveSettings(bool external, string? path)
        {
            string settingsPath = external && !string.IsNullOrWhiteSpace(path) ? path : lastKnownSettingsPath;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                theInstance tempInstance = new theInstance();
                foreach (var prop in typeof(theInstance).GetProperties())
                {
                    if (prop.CanRead && prop.CanWrite && prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length == 0)
                    {
                        var value = prop.GetValue(theInstance);
                        if (value != null)
                        {
                            prop.SetValue(tempInstance, value);
                        }
                    }
                }
                // Only encode if not null or empty
                if (!string.IsNullOrEmpty(tempInstance.profileServerPath))
                {
                    tempInstance.profileServerPath = Convert.ToBase64String(Encoding.UTF8.GetBytes(tempInstance.profileServerPath));
                }
                string json = JsonSerializer.Serialize(tempInstance, options);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppDebug.Log("InstanceManager", $"Failed to save settings: {ex.Message}");
            }
        }

        public void SetServerVariables()
        {

            // TO DO: Remove the need for SetServerVariables in theInstanceManager.

        }

        public void UpdateGameServer()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public bool ValidateGameServerPath() => CmdValidateGameServerPath.ProcessCommand();

        public void ValidateGameServerType(string serverPath)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void HighlightDifferences()
        {
            // TO DO: Remove the need for HighlightDifferences in theInstanceManager.
        }

        // Helper methods for highlighting
        public void HighlightTextBox(TextBox tb, string value)
        {
            tb.BackColor = tb.Text == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightComboBox(ComboBox cb, string value)
        {
            cb.BackColor = cb.Text == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightComboBoxIndex(ComboBox cb, int value)
        {
            cb.BackColor = cb.SelectedIndex == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightNumericUpDown(NumericUpDown num, int value)
        {
            num.BackColor = (int)num.Value == value ? SystemColors.Window : Color.LightYellow;
        }

        public void HighlightCheckBox(CheckBox cb, bool value)
        {
            cb.BackColor = cb.Checked == value ? SystemColors.Control : Color.LightYellow;
        }
    }
}
