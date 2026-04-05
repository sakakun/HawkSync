using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.DTOs;
using ServerManager.API.Hubs;
using ServerManager.Classes.SupportClasses;

namespace ServerManager.API.Services;

public class InstanceBroadcastService : BackgroundService
{
    private readonly IHubContext<ServerHub> _hubContext;

    public InstanceBroadcastService(IHubContext<ServerHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        AppDebug.Log("InstanceBroadcastService Started", AppDebug.LogLevel.Info);

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
                AppDebug.Log($"Broadcast Error", AppDebug.LogLevel.Error, ex);
                await Task.Delay(5000, stoppingToken);
            }
        }

        AppDebug.Log("InstanceBroadcastService Stopped", AppDebug.LogLevel.Info);
    }
}