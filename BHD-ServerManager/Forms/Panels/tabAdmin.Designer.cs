using System.ComponentModel;

namespace ServerManager.Forms.Panels;

partial class tabAdmin
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        tabAdminControls = new System.Windows.Forms.TabControl();
        pageAccounts = new System.Windows.Forms.TabPage();
        tabAccounts1 = new ServerManager.Forms.SubPanels.tabAdmin.tabAccounts();
        pageAuditLogs = new System.Windows.Forms.TabPage();
        tabAdminControls.SuspendLayout();
        pageAccounts.SuspendLayout();
        SuspendLayout();
        // 
        // tabAdminControls
        // 
        tabAdminControls.Controls.Add(pageAccounts);
        tabAdminControls.Controls.Add(pageAuditLogs);
        tabAdminControls.Dock = System.Windows.Forms.DockStyle.None;
        tabAdminControls.Location = new System.Drawing.Point(0, 0);
        tabAdminControls.Margin = new System.Windows.Forms.Padding(0);
        tabAdminControls.Name = "tabAdminControls";
        tabAdminControls.SelectedIndex = 0;
        tabAdminControls.Size = new System.Drawing.Size(966, 422);
        tabAdminControls.TabIndex = 0;
        // 
        // pageAccounts
        // 
        pageAccounts.Location = new System.Drawing.Point(4, 24);
        pageAccounts.Name = "pageAccounts";
        pageAccounts.Padding = new System.Windows.Forms.Padding(3);
        pageAccounts.Size = new System.Drawing.Size(958, 394);
        pageAccounts.TabIndex = 0;
        pageAccounts.Text = "Accounts";
        pageAccounts.UseVisualStyleBackColor = true;
        // 
        // pageAuditLogs
        // 
        pageAuditLogs.Location = new System.Drawing.Point(4, 24);
        pageAuditLogs.Name = "pageAuditLogs";
        pageAuditLogs.Padding = new System.Windows.Forms.Padding(3);
        pageAuditLogs.Size = new System.Drawing.Size(958, 394);
        pageAuditLogs.TabIndex = 1;
        pageAuditLogs.Text = "Audit Logs";
        pageAuditLogs.UseVisualStyleBackColor = true;
        // 
        // tabAdmin
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(tabAdminControls);
        Margin = new System.Windows.Forms.Padding(0);
        MaximumSize = new System.Drawing.Size(966, 422);
        MinimumSize = new System.Drawing.Size(966, 422);
        Size = new System.Drawing.Size(966, 422);
        tabAdminControls.ResumeLayout(false);
        pageAccounts.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ServerManager.Forms.SubPanels.tabAdmin.tabAccounts tabAccounts1;

    private System.Windows.Forms.TabControl tabAdminControls;
    private System.Windows.Forms.TabPage pageAccounts;
    private System.Windows.Forms.TabPage pageAuditLogs;

    #endregion
}
