using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.DTOs;
using BHD_ServerManager.API.Hubs;
using BHD_ServerManager.Classes.SupportClasses;

namespace BHD_ServerManager.API.Services;

public class InstanceBroadcastService : BackgroundService
{
    private readonly IHubContext<ServerHub> _hubContext;

    public InstanceBroadcastService(IHubContext<ServerHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        AppDebug.Log("InstanceBroadcastService", "Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (CommonCore.theInstance == null)
                {
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                var snapshot = InstanceMapper.CreateSnapshot(
                    CommonCore.theInstance,
                    CommonCore.instancePlayers ?? new(),
                    CommonCore.instanceChat ?? new(),
                    CommonCore.instanceBans ?? new(),
                    CommonCore.instanceMaps ?? new(),
                    CommonCore.instanceAdmin ?? new(),
                    CommonCore.instanceStats ?? new()
                );

                await _hubContext.Clients.Group("ServerUpdates")
                    .SendAsync("InstanceUpdate", snapshot, stoppingToken);

                await Task.Delay(1000, stoppingToken); // 1 second intervals
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                AppDebug.Log("InstanceBroadcastService", $"Error: {ex.Message}");
                await Task.Delay(5000, stoppingToken);
            }
        }

        AppDebug.Log("InstanceBroadcastService", "Stopped");
    }
}