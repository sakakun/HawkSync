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
        Dictionary<int, playerObject> playerList,
        chatInstance chat,
        banInstance bans,
        mapInstance maps)
    {
        return new ServerSnapshot
        {
            ServerData = server,
            Players = MapPlayerInstance(playerList),
            Chat = chat,
            Bans = MapBanInstance(bans),
            Maps = maps,
            SnapshotTime = DateTime.UtcNow
        };
    }

    private static PlayerInstanceDTO MapPlayerInstance(Dictionary<int, playerObject> playerList)
    {
        var players = playerList.ToDictionary(
            kvp => kvp.Key,
            kvp => new PlayerDTO
            {
                Slot = kvp.Value.PlayerSlot,
                Name = DecodePlayerName(kvp.Value.PlayerNameBase64),
                IPAddress = kvp.Value.PlayerIPAddress,
                Team = kvp.Value.PlayerTeam,
                Ping = kvp.Value.PlayerPing,
                Kills = kvp.Value.stat_Kills,
                Deaths = kvp.Value.stat_Deaths
            }
        );

        return new PlayerInstanceDTO { Players = players };
    }

    private static ChatInstanceDTO MapChatInstance(chatInstance chat)
    {
        return new ChatInstanceDTO
        {
            RecentMessages = chat.ChatLog.TakeLast(100).Select(log => new ChatMessageDTO
            {
                Timestamp = log.MessageTimeStamp,
                PlayerName = log.PlayerName,
                Message = log.MessageText,
                Team = GetTeamName(log.MessageType2)
            }).ToList(),
            SlapMessages = chat.SlapMessages.Select(s =>
                new SlapMessageDTO(s.SlapMessageId, s.SlapMessageText)).ToList()
        };
    }

    private static BanInstanceDTO MapBanInstance(banInstance bans)
    {
        return new BanInstanceDTO
        {
            BannedNames = bans.BannedPlayerNames.Select(b =>
                new BannedNameDTO(b.RecordID, b.PlayerName, b.Date, b.RecordType.ToString())).ToList(),
            BannedIPs = bans.BannedPlayerIPs.Select(b =>
                new BannedIPDTO(b.RecordID, b.PlayerIP.ToString(), b.SubnetMask, b.Date, b.RecordType.ToString())).ToList(),
            WhitelistedNames = bans.WhitelistedNames.Select(w =>
                new WhitelistedNameDTO(w.RecordID, w.PlayerName)).ToList()
        };
    }

    private static string DecodePlayerName(string? base64)
    {
        if (string.IsNullOrEmpty(base64)) return "Unknown";
        try
        {
            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.GetEncoding("Windows-1252").GetString(bytes);
        }
        catch
        {
            return "Unknown";
        }
    }

    private static string GetTeamName(int messageType2)
    {
        return messageType2 switch
        {
            1 => "Blue",
            2 => "Red",
            3 => "Host",
            _ => "All"
        };
    }
}