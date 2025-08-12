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

namespace BHD_ServerManager.Forms.Panels
{
    public partial class tabProfile : UserControl
    {
        private new string Name = "ProfileTab";                     // Name of the tab for logging purposes.
        public tabProfile()
        {
            InitializeComponent();
            Load += getProfile;                                     // Once loaded, get the profile data.
        }
        // --- Form Functions ---
        // --- Get Profile --- Allow to be triggered externally
        private void getProfile(object? sender, EventArgs e)
        {
            // Get the data from "theInterface" object and set it to the form.
            AppDebug.Log(this.Name, "Not Implemented Exception");
        }
        // --- Save Profile --- Allow to be triggered externally
        public void saveProfile()
        {
            // Set the data from the form to the "theInterface" object.
            // Trigger the save function in "theInstanceManager".
            AppDebug.Log(this.Name, "Not Implemented Exception");
        }
        // --- Toggle Profile Lock --- Allow to be triggered externally
        public void toggleProfileLock()
        {
            // Toggle the controls to be read-only or editable based on the theInstance server status.
            // If the server is running, make read-only. If stopped, make editable.
            AppDebug.Log(this.Name, "Not Implemented Exception");
        }

        // --- Action Click Events ---
        // --- Save Profile Button Clicked ---
        private void actionClick_SaveProfile(object sender, EventArgs e)
        {

        }
        // --- Reset Profile Button Clicked ---
        private void actionClick_ResetProfile(object sender, EventArgs e)
        {

        }
    }
}
