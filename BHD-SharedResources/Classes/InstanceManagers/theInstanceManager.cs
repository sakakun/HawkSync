using BHD_SharedResources.Classes.InstanceInterfaces;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_SharedResources.Classes.InstanceManagers
{
    public static class theInstanceManager
    {
        public static theInstanceInterface Implementation { get; set; }

        public static void CheckSettings() => Implementation.CheckSettings();
        public static void LoadSettings(bool external = false, string path = null) => Implementation?.LoadSettings(external, path);
        public static void SaveSettings(bool external = false, string path = null) => Implementation?.SaveSettings(external, path);
        public static void ExportSettings() => Implementation?.ExportSettings();
        public static void ImportSettings() => Implementation?.ImportSettings();
        public static bool ValidateGameServerPath() => (bool)(Implementation?.ValidateGameServerPath());
        public static void GetServerVariables(bool import = false, theInstance updatedInstance = null!) => Implementation?.GetServerVariables(import, updatedInstance);
        public static void SetServerVariables() => Implementation?.SetServerVariables();
        public static void ValidateGameServerType(string serverPath) => Implementation?.ValidateGameServerType(serverPath);
        public static void UpdateGameServer() => Implementation?.UpdateGameServer();
        public static void InitializeTickers() => Implementation?.InitializeTickers();
        public static void changeTeamGameMode(int currentMapType, int nextMapType) => Implementation?.changeTeamGameMode(currentMapType, nextMapType);
        public static void HighlightDifferences() => Implementation?.HighlightDifferences();
        public static void HighlightTextBox(TextBox tb, string value) => Implementation?.HighlightTextBox(tb, value);
        public static void HighlightComboBox(ComboBox cb, string value) => Implementation?.HighlightComboBox(cb, value);
        public static void HighlightComboBoxIndex(ComboBox cb, int value) => Implementation?.HighlightComboBoxIndex(cb, value);
        public static void HighlightNumericUpDown(NumericUpDown num, int value) => Implementation?.HighlightNumericUpDown(num, value);
        public static void HighlightCheckBox(CheckBox cb, bool value) => Implementation?.HighlightCheckBox(cb, value);
    }
}
