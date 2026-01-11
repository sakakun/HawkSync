using BHD_ServerManager.Classes.Instances;
using System.Windows.Forms;

namespace BHD_ServerManager.Classes.InstanceInterfaces
{
    public interface theInstanceInterface
    {
        void CheckSettings();
        void LoadSettings(bool external = false, string path = null);
        void SaveSettings(bool external = false, string path = null);
        void ExportSettings();
        void ImportSettings();
        bool ValidateGameServerPath();
        void GetServerVariables(bool import = false, theInstance updatedInstance = null!);
        void SetServerVariables();
        void ValidateGameServerType(string serverPath);
        void UpdateGameServer();
        void InitializeTickers();
        void changeTeamGameMode(int currentMapType, int nextMapType);
        void HighlightDifferences();
        void HighlightTextBox(TextBox tb, string value);
        void HighlightComboBox(ComboBox cb, string value);
        void HighlightComboBoxIndex(ComboBox cb, int value);
        void HighlightNumericUpDown(NumericUpDown num, int value);
        void HighlightCheckBox(CheckBox cb, bool value);
    }
}
