using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.SupportClasses;
using System;
using System.Collections.Generic;
using System.Text;


namespace RemoteClient.Services.Commands;

public class PlayerService
{
    private readonly ApiClient _apiClient;

    public PlayerService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // PLAYER COMMANDS
    // ================================================================================

    public async Task<CommandResult> KickPlayerAsync(int playerSlot, string playerName)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new KickPlayerCommand(playerSlot, PlayerName);
        return await _apiClient.SendCommandAsync("/api/player/kick", command);
    }

    public async Task<CommandResult> BanPlayerAsync(int playerSlot, string playerName, string playerIP, bool banIP)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new BanPlayerCommand(playerSlot, PlayerName, playerIP, banIP);
        return await _apiClient.SendCommandAsync("/api/player/ban", command);
    }

    public async Task<CommandResult> WarnPlayerAsync(int playerSlot, string playerName, string message)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new WarnPlayerCommand(playerSlot, PlayerName, message);
        return await _apiClient.SendCommandAsync("/api/player/warn", command);
    }

    public async Task<CommandResult> KillPlayerAsync(int playerSlot, string playerName)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new KillPlayerCommand(playerSlot, PlayerName);
        return await _apiClient.SendCommandAsync("/api/player/kill", command);
    }

    public async Task<CommandResult> ArmPlayerAsync(int playerSlot, string playerName)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new ArmPlayerCommand(playerSlot, PlayerName);
        return await _apiClient.SendCommandAsync("/api/player/arm", command);
    }

    public async Task<CommandResult> DisarmPlayerAsync(int playerSlot, string playerName)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new DisarmPlayerCommand(playerSlot, PlayerName);
        return await _apiClient.SendCommandAsync("/api/player/disarm", command);
    }
    public async Task<CommandResult> ToggleGodPlayerAsync(int playerSlot, string playerName)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new GodModePlayerCommand(playerSlot, PlayerName);
        return await _apiClient.SendCommandAsync("/api/player/togglegodmode", command);
    }
    public async Task<CommandResult> SwitchTeamPlayerAsync(int playerSlot, string playerName, int teamNum)
    {
        var PlayerName = AppFunc.TB64(playerName);
        var command = new SwitchTeamPlayerCommand(playerSlot, PlayerName, teamNum);
        return await _apiClient.SendCommandAsync("/api/player/switchteam", command);
    }

}
