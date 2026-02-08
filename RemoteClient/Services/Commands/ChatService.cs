using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.SupportClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteClient.Services.Commands;

public class ChatService
{
    private readonly ApiClient _apiClient;

    public ChatService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }

    // ================================================================================
    // CHAT COMMANDS
    // ================================================================================

    public async Task<CommandResult> SendChatAsync(string message, int channel = 1)
    {
        string chatMessage = AppFunc.TB64(message);

        var command = new SendChatCommand(chatMessage, channel);
        return await _apiClient.SendCommandAsync("/api/chat/send", command);
    }

    // Example generic command sender for auto/slap messages
    public async Task<CommandResult> SendAutoMessageAsync(string message, int interval)
    {
        var command = new { Message = message, Interval = interval };
        return await _apiClient.SendCommandAsync("/api/chat/auto/add", command);
    }

    public async Task<CommandResult> RemoveAutoMessageAsync(string id)
    {
        var command = new { Id = id };
        return await _apiClient.SendCommandAsync("/api/chat/auto/remove", command);
    }

    public async Task<CommandResult> SendSlapMessageAsync(string message)
    {
        var command = new { Message = message };
        return await _apiClient.SendCommandAsync("/api/chat/slap/add", command);
    }

    public async Task<CommandResult> RemoveSlapMessageAsync(string id)
    {
        var command = new { Id = id };
        return await _apiClient.SendCommandAsync("/api/chat/slap/remove", command);
    }
}

