using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.DTOs.Audit;
using ServerManager.Classes.SupportClasses;
using ServerManager.Classes.InstanceManagers;
using HawkSyncShared;

namespace ServerManager.API.Hubs;

[Authorize]
public class ServerHub : Hub
{
    private static readonly ConcurrentDictionary<string, byte> _connectedClients = new();
    private static readonly ConcurrentDictionary<string, string> _connectionToUsername = new();
    private static readonly ConcurrentDictionary<string, int> _userConnectionCounts = new(StringComparer.OrdinalIgnoreCase);

    public async Task SubscribeToUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.TryAdd(Context.ConnectionId, 0);

        var username = Context.User?.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            _connectionToUsername[Context.ConnectionId] = username;
            _userConnectionCounts.AddOrUpdate(username, 1, (_, count) => count + 1);
            adminInstanceManager.TrackSession(username);
            CommonCore.instanceAdmin!.ForceUIUpdate = true;
        }
        else
        {
            AppDebug.Log($"Client subscribed without username. Total: {_connectedClients.Count}", AppDebug.LogLevel.Warning);
        }
    }

    public async Task UnsubscribeFromUpdates()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ServerUpdates");
        _connectedClients.TryRemove(Context.ConnectionId, out _);

        if (_connectionToUsername.TryRemove(Context.ConnectionId, out var username))
        {
            DecrementAndMaybeRemoveSession(username);
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
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedClients.TryRemove(Context.ConnectionId, out _);

        if (_connectionToUsername.TryRemove(Context.ConnectionId, out var username))
        {
            DecrementAndMaybeRemoveSession(username);
            
            // Log disconnection as logout event
            var userId = Context.User?.FindFirst("userId")?.Value;
            var ipAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString();
            var disconnectReason = exception != null ? $"Abnormal disconnect: {exception.Message}" : "Client disconnected";
            int? parsedUserId = int.TryParse(userId, out var uid) ? uid : null;
            
            DatabaseManager.LogAuditAction(
                userId: parsedUserId,
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
            
        }
        else
        {
            AppDebug.Log($"Client disconnected: {Context.ConnectionId}", AppDebug.LogLevel.Warning);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private static void DecrementAndMaybeRemoveSession(string username)
    {
        var newCount = _userConnectionCounts.AddOrUpdate(username, 0, (_, count) => Math.Max(0, count - 1));
        if (newCount > 0)
        {
            return;
        }

        _userConnectionCounts.TryRemove(username, out _);
        adminInstanceManager.RemoveSession(username);
        CommonCore.instanceAdmin!.ForceUIUpdate = true;
    }
}