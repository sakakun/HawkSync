using RemoteClient.Services;
using HawkSyncShared.DTOs;

namespace RemoteClient.Core;

/// <summary>
/// Core infrastructure for remote client (like CommonCore on server)
/// </summary>
public static class ApiCore
{
    // ================================================================================
    // CLIENT INFRASTRUCTURE
    // ================================================================================
    
    public static ApiClient? ApiClient { get; private set; }
    public static SignalRClient? SignalRClient { get; private set; }
    public static UserInfoDTO? CurrentUser { get; private set; }
    public static string ServerUrl { get; private set; } = string.Empty;
    
    // ================================================================================
    // CURRENT STATE (Latest Snapshot)
    // ================================================================================
    
    public static ServerSnapshot? CurrentSnapshot { get; private set; }
    public static DateTime LastUpdate { get; private set; } = DateTime.MinValue;
    
    // ================================================================================
    // EVENTS
    // ================================================================================
    
    /// <summary>
    /// Fired when a new snapshot is received (every 1 second)
    /// </summary>
    public static event Action<ServerSnapshot>? OnSnapshotReceived;
    
    /// <summary>
    /// Fired when connection state changes
    /// </summary>
    public static event Action<string>? OnConnectionStateChanged;
    
    // ================================================================================
    // INITIALIZATION
    // ================================================================================
    
    /// <summary>
    /// Initialize and connect to server after successful login
    /// </summary>
    public static async Task InitializeAsync(string serverUrl, string jwtToken, UserInfoDTO user)
    {
        ServerUrl = serverUrl;
        CurrentUser = user;
        
        // Create clients
        ApiClient = new ApiClient(serverUrl) { JwtToken = jwtToken };
        SignalRClient = new SignalRClient(serverUrl);
        
        // Wire up events
        SignalRClient.OnSnapshotReceived += HandleSnapshotReceived;
        SignalRClient.OnConnectionStateChanged += HandleConnectionStateChanged;
        
        // Get initial snapshot
        CurrentSnapshot = await ApiClient.GetSnapshotAsync();
        
        // Connect to SignalR for real-time updates
        await SignalRClient.ConnectAsync(jwtToken);
    }
    
    private static void HandleSnapshotReceived(ServerSnapshot snapshot)
    {
        CurrentSnapshot = snapshot;
        LastUpdate = DateTime.Now;
        
        // Broadcast to all subscribers (tabs)
        OnSnapshotReceived?.Invoke(snapshot);
    }
    
    private static void HandleConnectionStateChanged(string state)
    {
        OnConnectionStateChanged?.Invoke(state);
    }
    
    // ================================================================================
    // SHUTDOWN
    // ================================================================================
    
    public static async Task ShutdownAsync()
    {
        if (SignalRClient != null)
        {
            await SignalRClient.DisconnectAsync();
            SignalRClient.Dispose();
        }
        
        ApiClient?.Dispose();
    }
}