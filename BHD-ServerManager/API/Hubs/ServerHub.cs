using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.DTOs.Audit;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared;

namespace BHD_ServerManager.API.Hubs;

[Authorize]
public class ServerHub : Hub
{
    private static readonly HashSet<string> _connectedClients = new();
    private static readonly Dictionary<string, string> _connectionToUsername = new();

    public async Task SubscribeToUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.Add(Context.ConnectionId);

        var username = Context.User?.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            _connectionToUsername[Context.ConnectionId] = username;
            adminInstanceManager.TrackSession(username);
            CommonCore.instanceAdmin!.ForceUIUpdate = true;
            AppDebug.Log("ServerHub", $"Client subscribed: {username} (Total: {_connectedClients.Count})");
        }
        else
        {
            AppDebug.Log("ServerHub", $"Client subscribed without username. Total: {_connectedClients.Count}");
        }
    }

    public async Task UnsubscribeFromUpdates()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.Remove(Context.ConnectionId);

        if (_connectionToUsername.TryGetValue(Context.ConnectionId, out var username))
        {
            _connectionToUsername.Remove(Context.ConnectionId);
            adminInstanceManager.RemoveSession(username);
            CommonCore.instanceAdmin!.ForceUIUpdate = true;
            AppDebug.Log("ServerHub", $"Client unsubscribed: {username}");
        }
    }

    public Task Heartbeat()
    {
        var username = Context.User?.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            adminInstanceManager.TrackSession(username);
        }
        return Task.CompletedTask;
    }

    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.FindFirst("username")?.Value ?? "Unknown";
        AppDebug.Log("ServerHub", $"Client connected: {username} ({Context.ConnectionId})");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedClients.Remove(Context.ConnectionId);

        if (_connectionToUsername.TryGetValue(Context.ConnectionId, out var username))
        {
            _connectionToUsername.Remove(Context.ConnectionId);
            adminInstanceManager.RemoveSession(username);
            CommonCore.instanceAdmin!.ForceUIUpdate = true;
            
            // Log disconnection as logout event
            var userId = Context.User?.FindFirst("userId")?.Value;
            var ipAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString();
            var disconnectReason = exception != null ? $"Abnormal disconnect: {exception.Message}" : "Client disconnected";
            
            DatabaseManager.LogAuditAction(
                userId: userId != null ? int.Parse(userId) : null,
                username: username,
                category: AuditCategory.System,
                actionType: AuditAction.Logout,
                description: disconnectReason,
                targetType: "Connection",
                targetId: Context.ConnectionId,
                targetName: "SignalR",
                ipAddress: ipAddress,
                success: exception == null,
                errorMessage: exception?.Message
            );
            
            AppDebug.Log("ServerHub", $"Client disconnected: {username} ({Context.ConnectionId})");
        }
        else
        {
            AppDebug.Log("ServerHub", $"Client disconnected: {Context.ConnectionId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}