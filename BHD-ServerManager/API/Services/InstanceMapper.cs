using HawkSyncShared;
using HawkSyncShared.DTOs;
using HawkSyncShared.Instances;
using HawkSyncShared.ObjectClasses;
using HawkSyncShared.SupportClasses;
using System.Text;

namespace BHD_ServerManager.API.Services;

public static class InstanceMapper
{

    public static ServerSnapshot CreateSnapshot(
        theInstance server,
        playerInstance playerList,
        chatInstance chat,
        banInstance bans,
        mapInstance maps,
        adminInstance admin,
        statInstance stats)
    {
        return new ServerSnapshot
        {
            ServerData = server,
            Players = playerList,
            Chat = chat,
            Bans = bans,
            Maps = maps,
            Admins = admin,
            Stats = stats,
            SnapshotTime = DateTime.UtcNow
        };
    }

}