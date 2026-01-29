namespace HawkSyncShared.DTOs;

public record LoginRequest(string Username, string Password);

public record LoginResponse
{
    public bool Success { get; init; }
    public string Token { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public UserInfoDTO? User { get; init; }
}

public record UserInfoDTO
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public List<string> Permissions { get; init; } = new();
}