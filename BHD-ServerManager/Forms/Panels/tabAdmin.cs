using System.ComponentModel;
using ServerManager.Forms.SubPanels.tabAdmin;

namespace ServerManager.Forms.Panels;

public partial class tabAdmin : UserControl
{
    
    private tabAccounts         tabAccounts         = null!;
    private tabAudit            tabAudit            = null!;
    
    private static bool IsDesignTime =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime || System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains("devenv");
    
    public tabAdmin()
    {
        InitializeComponent();
        
        if (IsDesignTime)
            return;
        
        // Add sub-panel to tabControls
        pageAccounts.Controls.Add(tabAccounts = new tabAccounts());
        pageAuditLogs.Controls.Add(tabAudit = new tabAudit());
        
    }
}