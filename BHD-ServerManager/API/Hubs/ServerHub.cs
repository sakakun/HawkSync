using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.SupportClasses;

namespace BHD_ServerManager.API.Hubs;

[Authorize]
public class ServerHub : Hub
{
    private static readonly HashSet<string> _connectedClients = new();

    public async Task SubscribeToUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.Add(Context.ConnectionId);
        AppDebug.Log("ServerHub", $"Client subscribed. Total: {_connectedClients.Count}");
    }

    public async Task UnsubscribeFromUpdates()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.Remove(Context.ConnectionId);
    }

    public override async Task OnConnectedAsync()
    {
        AppDebug.Log("ServerHub", $"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedClients.Remove(Context.ConnectionId);
        AppDebug.Log("ServerHub", $"Client disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}