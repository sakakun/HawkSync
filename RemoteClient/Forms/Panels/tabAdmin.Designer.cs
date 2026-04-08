namespace RemoteClient.Forms.Panels
{
    partial class tabAdmin
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
			tabControlAdmin = new TabControl();
			tabPageAccounts = new TabPage();
			tabAccountsHost = new Panel();
			tabPageAudit = new TabPage();
			tabAuditHost = new Panel();
			tabControlAdmin.SuspendLayout();
			tabPageAccounts.SuspendLayout();
			tabPageAudit.SuspendLayout();
			SuspendLayout();
			// 
			// tabControlAdmin
			// 
			tabControlAdmin.Controls.Add(tabPageAccounts);
			tabControlAdmin.Controls.Add(tabPageAudit);
			tabControlAdmin.Dock = DockStyle.Fill;
			tabControlAdmin.Location = new Point(0, 0);
			tabControlAdmin.Name = "tabControlAdmin";
			tabControlAdmin.SelectedIndex = 0;
			tabControlAdmin.Size = new Size(966, 422);
			tabControlAdmin.TabIndex = 0;
			// 
			// tabPageAccounts
			// 
			tabPageAccounts.Controls.Add(tabAccountsHost);
			tabPageAccounts.Location = new Point(4, 24);
			tabPageAccounts.Name = "tabPageAccounts";
			tabPageAccounts.Padding = new Padding(0);
			tabPageAccounts.Size = new Size(958, 394);
			tabPageAccounts.TabIndex = 0;
			tabPageAccounts.Text = "Accounts";
			tabPageAccounts.UseVisualStyleBackColor = true;
			// 
			// tabAccountsHost
			// 
			tabAccountsHost.Dock = DockStyle.Fill;
			tabAccountsHost.Location = new Point(0, 0);
			tabAccountsHost.Name = "tabAccountsHost";
			tabAccountsHost.Size = new Size(958, 394);
			tabAccountsHost.TabIndex = 0;
			// 
			// tabPageAudit
			// 
			tabPageAudit.Controls.Add(tabAuditHost);
			tabPageAudit.Location = new Point(4, 24);
			tabPageAudit.Name = "tabPageAudit";
			tabPageAudit.Padding = new Padding(0);
			tabPageAudit.Size = new Size(958, 394);
			tabPageAudit.TabIndex = 1;
			tabPageAudit.Text = "Audit";
			tabPageAudit.UseVisualStyleBackColor = true;
			// 
			// tabAuditHost
			// 
			tabAuditHost.Dock = DockStyle.Fill;
			tabAuditHost.Location = new Point(0, 0);
			tabAuditHost.Name = "tabAuditHost";
			tabAuditHost.Size = new Size(958, 394);
			tabAuditHost.TabIndex = 0;
			// 
			// tabAdmin
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(tabControlAdmin);
			MaximumSize = new Size(966, 422);
			MinimumSize = new Size(966, 422);
			Name = "tabAdmin";
			Size = new Size(966, 422);
			tabControlAdmin.ResumeLayout(false);
			tabPageAccounts.ResumeLayout(false);
			tabPageAudit.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

														private TabControl tabControlAdmin;
														private TabPage tabPageAccounts;
														private Panel tabAccountsHost;
														private TabPage tabPageAudit;
														private Panel tabAuditHost;
	}
}
