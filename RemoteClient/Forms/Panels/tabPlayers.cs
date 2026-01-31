using HawkSyncShared.DTOs;
using RemoteClient.Core;

namespace RemoteClient.Forms.Panels;

public partial class tabPlayers : UserControl
{
    public tabPlayers()
    {
        InitializeComponent();
        
        // Subscribe to snapshot updates
        ApiCore.OnSnapshotReceived += OnSnapshotReceived;
    }

    private void OnSnapshotReceived(ServerSnapshot snapshot)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnSnapshotReceived(snapshot));
            return;
        }

    }




}