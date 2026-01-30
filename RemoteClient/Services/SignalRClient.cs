using Microsoft.AspNetCore.SignalR.Client;
using HawkSyncShared.DTOs;

namespace RemoteClient.Services;

/// <summary>
/// SignalR client for real-time server updates
/// </summary>
public class SignalRClient : IDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;

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
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            OnConnectionStateChanged?.Invoke("🟢 Connected");
            _ = SubscribeToUpdatesAsync();
            return Task.CompletedTask;
        };

        _connection.Closed += error =>
        {
            OnConnectionStateChanged?.Invoke(error != null 
                ? $"🔴 Disconnected: {error.Message}" 
                : "🔴 Disconnected");
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