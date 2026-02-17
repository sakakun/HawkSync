using HawkSyncShared.DTOs.API;
using HawkSyncShared.DTOs.tabPlayers;
using HawkSyncShared.SupportClasses;
using HawkSyncShared.Instances;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
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

    // ================================================================================
    // CHAT HISTORY
    // ================================================================================

    public async Task<List<string>?> GetDistinctPlayerNamesAsync(int limit = 500)
    {
        try
        {
            return await _apiClient._httpClient.GetFromJsonAsync<List<string>>($"/api/chat/history/players?limit={limit}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting player names: {ex.Message}");
            return null;
        }
    }

    public async Task<ChatHistoryResponse?> GetChatHistoryAsync(ChatHistoryRequest request)
    {
        try
        {
            var response = await _apiClient._httpClient.PostAsJsonAsync("/api/chat/history/search", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error getting chat history: HTTP {response.StatusCode}: {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ChatHistoryResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting chat history: {ex.Message}");
            return null;
        }
    }
}

public class ChatHistoryRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PlayerFilter { get; set; }
    public int? TypeFilter { get; set; }
    public int? TeamFilter { get; set; }
    public string? SearchText { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}

public class ChatHistoryResponse
{
    public List<ChatLogObject> Logs { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

