namespace BHD_ServerManager.Forms.Panels
{
    partial class tabAddons
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            addonTabs = new TabControl();
            panelChatCommands = new TabPage();
            addonTabs.SuspendLayout();
            SuspendLayout();
            // 
            // addonTabs
            // 
            addonTabs.Controls.Add(panelChatCommands);
            addonTabs.Dock = DockStyle.Fill;
            addonTabs.Location = new Point(0, 0);
            addonTabs.Name = "addonTabs";
            addonTabs.SelectedIndex = 0;
            addonTabs.Size = new Size(902, 362);
            addonTabs.TabIndex = 0;
            // 
            // panelChatCommands
            // 
            panelChatCommands.Location = new Point(4, 24);
            panelChatCommands.Margin = new Padding(0);
            panelChatCommands.Name = "panelChatCommands";
            panelChatCommands.Padding = new Padding(3);
            panelChatCommands.Size = new Size(894, 334);
            panelChatCommands.TabIndex = 0;
            panelChatCommands.Text = "Chat Commands";
            panelChatCommands.UseVisualStyleBackColor = true;
            // 
            // tabAddons
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(addonTabs);
            Margin = new Padding(0);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabAddons";
            Size = new Size(902, 362);
            addonTabs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl addonTabs;
        private TabPage panelChatCommands;
    }
}
