using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ServerManager.Classes.Enviroment;

namespace ServerManager.Panels;

public partial class ServerProfileEditor : Form
{
    private ServerEnvironment Env;
    private string profilePath;
    private bool editMode;

    public ServerProfileEditor(bool editMode, ServerProfile serverProfile = null)
    {
        this.editMode = editMode;

        InitializeComponent();
        loadProfile(editMode, serverProfile);
    }

    private void loadProfile(bool editMode, ServerProfile serverProfile = null)
    {
        if (editMode && serverProfile != null)
        {
            textBox_profileName.Text = serverProfile.ProfileName;
            textBox_serverPath.Text = serverProfile.ServerPath;
            check_modInfo();
        }
        label_message.Text = String.Empty;
    }

    private void click_browserExe(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Executable files (*.exe)|*.exe";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetFileName(openFileDialog.FileName) == "dfbhd.exe")
                {
                    string directoryPath = Path.GetDirectoryName(openFileDialog.FileName);
                    textBox_serverPath.Text = directoryPath;
                    check_modInfo();
                }
                else
                {
                    MessageBox.Show("Please select the file named 'dfbhd.exe'.", "Invalid Server Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    private void profileName_validateChars(object sender, KeyPressEventArgs e)
    {
        // Define the regex pattern to match allowed characters
        string pattern = @"^[a-zA-Z0-9 ]$";
        // Check if the character is valid
        if (!Regex.IsMatch(e.KeyChar.ToString(), pattern) && !char.IsControl(e.KeyChar))
        {
            // Suppress the input
            e.Handled = true;
        }
    }

    private void check_modInfo()
    {
        bool modFound = false;
        foreach (var mod in Env._modList)
        {
            if (mod.Id != 1)
            {
                if (File.Exists(Path.Combine(textBox_serverPath.Text, mod.Pff)))
                {
                    modFound = true;
                    label_modInfo.Text = mod.Name;
                }
            }
        }
        if (!modFound)
        {
            label_modInfo.Text = "No mods detected.";
        }
    }

    private void click_accept(object sender, EventArgs e)
    {
        profilePath = Path.Combine(Env._programConfig.dataPath, "Profiles", textBox_profileName.Text.Replace(" ", "_"));
        
        try
        {
            /*if (editMode && serverProfile != null)
            {
                if (Env._serverProfiles.EditProfile(serverProfile, textBox_profileName.Text, profilePath, textBox_serverPath.Text))
                    Close();
                
                return;
            }

            if (Env._serverProfiles.AddProfile(textBox_profileName.Text, profilePath, textBox_serverPath.Text))
                Close();
            */
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Program.DebugLog($"Profile Editor: {ex.Message}");
        }
        
    }
}
