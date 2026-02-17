using Microsoft.AspNetCore.SignalR.Client;
using HawkSyncShared.DTOs.API;

namespace RemoteClient.Services;

/// <summary>
/// SignalR client for real-time server updates
/// </summary>
public class SignalRClient : IDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;
    private System.Timers.Timer? _heartbeatTimer;

    public event Action<ServerSnapshot>? OnSnapshotReceived;
    public event Action<string>? OnConnectionStateChanged;

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;
    public HubConnectionState State => _connection?.State ?? HubConnectionState.Disconnected;

    public SignalRClient(string serverUrl)
    {
        _hubUrl = $"{serverUrl.TrimEnd('/')}/hubs/server";
    }

    public async Task ConnectAsync(string jwtToken)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(jwtToken);
            })
            .WithAutomaticReconnect(new[]
            {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            })
            .Build();

        // Connection events
        _connection.Reconnecting += error =>
        {
            OnConnectionStateChanged?.Invoke("🟡 Reconnecting...");
            StopHeartbeat();
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            OnConnectionStateChanged?.Invoke("🟢 Connected");
            _ = SubscribeToUpdatesAsync();
            StartHeartbeat();
            return Task.CompletedTask;
        };

        _connection.Closed += error =>
        {
            OnConnectionStateChanged?.Invoke(error != null 
                ? $"🔴 Disconnected: {error.Message}" 
                : "🔴 Disconnected");
            StopHeartbeat();
            return Task.CompletedTask;
        };

        // Register message handlers
        _connection.On<ServerSnapshot>("InstanceUpdate", snapshot =>
        {
            OnSnapshotReceived?.Invoke(snapshot);
        });

        // Connect
        await _connection.StartAsync();
        OnConnectionStateChanged?.Invoke("🟢 Connected");

        // Subscribe to updates
        await SubscribeToUpdatesAsync();

        // Start heartbeat
        StartHeartbeat();
    }

    private void StartHeartbeat()
    {
        _heartbeatTimer = new System.Timers.Timer(30000); // 30 seconds
        _heartbeatTimer.Elapsed += async (sender, e) =>
        {
            if (_connection?.State == HubConnectionState.Connected)
            {
                try
                {
                    await _connection.InvokeAsync("Heartbeat");
                }
                catch
                {
                    // Ignore heartbeat errors
                }
            }
        };
        _heartbeatTimer.Start();
    }

    private void StopHeartbeat()
    {
        if (_heartbeatTimer != null)
        {
            _heartbeatTimer.Stop();
            _heartbeatTimer.Dispose();
            _heartbeatTimer = null;
        }
    }

    private async Task SubscribeToUpdatesAsync()
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("SubscribeToUpdates");
        }
    }

    public async Task DisconnectAsync()
    {
        StopHeartbeat();

        if (_connection != null)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("UnsubscribeFromUpdates");
                }

                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
            catch { }
            finally
            {
                _connection = null;
            }
        }
    }

    public void Dispose()
    {
        DisconnectAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }
}