using RemoteClient.Forms.SubPanels.tabAdmin;

namespace RemoteClient.Forms.Panels;

public partial class tabAdmin : UserControl
{
    public tabAdmin()
    {
        InitializeComponent();

        tabAccountsHost.Controls.Clear();
        tabAccountsHost.Controls.Add(new tabAccounts { Dock = DockStyle.Fill });

        tabAuditHost.Controls.Clear();
        tabAuditHost.Controls.Add(new tabAudit { Dock = DockStyle.Fill });
    }
}