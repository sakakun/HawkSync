using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.Storage;
using Button = System.Windows.Controls.Button;
using UserControl = System.Windows.Forms.UserControl;

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabProfile : UserControl
    {
        // --- Instance Objects ---
        private theInstance theInstance => CommonCore.theInstance;
        // --- UI Objects ---
        private ServerManager theServer => Program.ServerManagerUI!;
        // --- Class Variables ---
        private new string Name = "ProfileTab";                     // Name of the tab for logging purposes.
        private bool _firstLoadComplete = false;                    // First load flag to prevent certain actions on initial load.
        
        public tabProfile()
        {
            InitializeComponent();
        }
        // --- Form Functions ---
        // --- Get Profile --- Allow to be triggered externally
        public void functionEvent_GetProfileSettings(object? sender, EventArgs e)
        {
            // Get the data from "theInterface" object and set it to the form.
            tb_profileServerPath.Text = theInstance.profileServerPath;
            tb_modFile.Text = theInstance.profileModFileName;
            // Set checked items for cb_profileModifierList1
            for (int i = 0; i < cb_profileModifierList1.Items.Count; i++)
            {
                string item = cb_profileModifierList1.Items[i].ToString()!;
                cb_profileModifierList1.SetItemChecked(i, theInstance.profileModifierList1 != null && theInstance.profileModifierList1.Contains(item));
            }

            // Set checked items for cb_profileModifierList2
            for (int i = 0; i < cb_profileModifierList2.Items.Count; i++)
            {
                string item = cb_profileModifierList2.Items[i].ToString()!;
                cb_profileModifierList2.SetItemChecked(i, theInstance.profileModifierList2 != null && theInstance.profileModifierList2.Contains(item));
            }
            // if sender type is a button, open a message box to confirm the profile data has been loaded.
            if (sender is Button)
            {
                MessageBox.Show("Profile data loaded successfully.", "Profile Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // --- Save Profile --- Allow to be triggered externally
        public void funtionEvent_SetProfileSettings()
        {
            // Set the data from the form to the "theInterface" object.
            // Trigger the save function in "theInstanceManager".
            theInstance.profileServerPath = tb_profileServerPath.Text;
            theInstance.profileModFileName = tb_modFile.Text;
            theInstance.profileModifierList1 = cb_profileModifierList1.CheckedItems.Cast<string>().ToList();
            theInstance.profileModifierList2 = cb_profileModifierList2.CheckedItems.Cast<string>().ToList();
            // Save the instance data
            try
            {
                theInstanceManager.SaveSettings();
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, $"Failed to save profile settings: {ex.Message}");
                MessageBox.Show("Failed to save profile settings. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Show a message box to confirm the save.
            MessageBox.Show("Profile settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // --- Highlight Differences --- Allow to be triggered
        private void functionEvent_HighlightDiffFields()
        {
            // Profile Server Path
            tb_profileServerPath.BackColor = tb_profileServerPath.Text != theInstance.profileServerPath
                ? Color.LightYellow : SystemColors.Window;

            // Mod File Name
            tb_modFile.BackColor = tb_modFile.Text != theInstance.profileModFileName
                ? Color.LightYellow : SystemColors.Window;

            // Modifier List 1
            var checkedItems1 = cb_profileModifierList1.CheckedItems.Cast<string>().ToList();
            bool list1Diff = !(theInstance.profileModifierList1?.Count == checkedItems1.Count &&
                               theInstance.profileModifierList1.All(checkedItems1.Contains) &&
                               checkedItems1.All(theInstance.profileModifierList1.Contains));
            cb_profileModifierList1.BackColor = list1Diff ? Color.LightYellow : SystemColors.Window;

            // Modifier List 2
            var checkedItems2 = cb_profileModifierList2.CheckedItems.Cast<string>().ToList();
            bool list2Diff = !(theInstance.profileModifierList2?.Count == checkedItems2.Count &&
                               theInstance.profileModifierList2.All(checkedItems2.Contains) &&
                               checkedItems2.All(theInstance.profileModifierList2.Contains));
            cb_profileModifierList2.BackColor = list2Diff ? Color.LightYellow : SystemColors.Window;
        }
        // --- Ticker Profile Hook --- Allow to be triggered externally by the Server Manager Ticker
        public void tickerProfileTabHook()
        {
            if (!_firstLoadComplete)
            {
                functionEvent_GetProfileSettings(null, null!); // Initial load of profile data.
                _firstLoadComplete = true;

                // If the profileServerPath is not set or does not exist, switch to the Profile tab and show a message box.
                if (theInstance.profileServerPath == string.Empty || !Directory.Exists(theInstance.profileServerPath))
                {
                    theServer.tabControl.SelectedTab = theServer.tabControl.TabPages[0];
                    MessageBox.Show("Please set the Server Path in the Profile tab.", "Server Path Not Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    theServer.tabControl.SelectedTab = theServer.tabControl.TabPages[1];
                }

            }

            bool currentState = (theInstance.instanceStatus == InstanceStatus.OFFLINE);
            bool isModChecked = cb_profileModifierList1.CheckedItems.Contains("/mod");

            // Enable/disable all profile controls (except mod fields)
            tb_profileServerPath.Enabled = currentState;
            cb_profileModifierList1.Enabled = currentState;
            cb_profileModifierList2.Enabled = currentState;
            btn_profileBrowse1.Enabled = currentState;
            btn_resetProfile.Enabled = currentState;
            btn_saveProfile.Enabled = currentState;

            // Enable/disable mod fields based on /mod checked and OFFLINE status
            tb_modFile.Enabled = currentState && isModChecked;
            btn_profileBrowse2.Enabled = currentState && isModChecked;

            // Highlight differences
            functionEvent_HighlightDiffFields();
        }

        // --- Action Click Events ---
        // --- Save Profile Button Clicked ---
        private void actionClick_SaveProfile(object sender, EventArgs e)
        {
            funtionEvent_SetProfileSettings();
        }
        // --- Reset Profile Button Clicked ---
        private void actionClick_ResetProfile(object sender, EventArgs e)
        {
            functionEvent_GetProfileSettings(sender, e); // Re-fetch the profile data to reset the form.
        }
        // --- Open Profile Folder Button Clicked ---
        private void actionClick_profileOpenFolderDialog(object sender, EventArgs e)
        {
            // This will open the folder dialog and allow the user to select a folder, once selected , it will place the folder path in tb.profileServerPath.
            try
            {
                // Create a new FolderBrowserDialog instance.
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    // Set the description for the dialog.
                    folderBrowserDialog.Description = "Select the Mod Profile Folder";
                    // Show the dialog and check if the user selected a folder.
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Set the text of the tb.profileServerPath to the selected folder path.
                        tb_profileServerPath.Text = folderBrowserDialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, $"Failed to open profile folder dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile folder dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // --- Open Profile File Button Clicked ---
        private void actionClick_profileOpenFileDialog(object sender, EventArgs e)
        {
            // This will open the file dialog and allow the user to select a *.pff file.  Once selected, it will place the file name (no extension) int tb.modFile.
            try
            {
                // Create a new OpenFileDialog instance.
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Set the filter for the file dialog to only show *.pff files.
                    openFileDialog.Filter = "Mod Files (*.pff)|*.pff";
                    openFileDialog.Title = "Select a Mod File";
                    // Show the dialog and check if the user selected a file.
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected file name without the extension.
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        // Set the text of the tb_modFile to the selected file name.
                        tb_modFile.Text = fileNameWithoutExtension;
                    }
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log(this.Name, $"Failed to open profile file dialog: {ex.Message}");
                MessageBox.Show("Failed to open profile file dialog. Please check the logs for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
