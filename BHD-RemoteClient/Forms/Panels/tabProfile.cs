using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;

namespace BHD_RemoteClient.Forms.Panels
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
                    MessageBox.Show("Server path is not set on the server.  Please have an admin ", "Server Path Not Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    theServer.tabControl.SelectedTab = theServer.tabControl.TabPages[1];
                }

            }

            bool currentState = (theInstance.instanceStatus == InstanceStatus.OFFLINE);
            bool isModChecked = cb_profileModifierList1.CheckedItems.Contains("/mod");

            // Highlight differences
            functionEvent_HighlightDiffFields();
        }

        // --- Action Click Events ---
        // --- Reset Profile Button Clicked ---
        private void actionClick_ResetProfile(object sender, EventArgs e)
        {
            functionEvent_GetProfileSettings(sender, e); // Re-fetch the profile data to reset the form.
        }

    }
}
