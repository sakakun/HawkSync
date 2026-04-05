namespace HawkSyncShared.DTOs.API;

public record CommandResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
