namespace HawkSyncShared.DTOs;

public record CommandResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
public record ArmPlayerCommand(int PlayerSlot, string PlayerName);
public record DisarmPlayerCommand(int PlayerSlot, string PlayerName);
public record KickPlayerCommand(int PlayerSlot, string PlayerName);
public record GodModePlayerCommand(int PlayerSlot, string PlayerName);
public record SwitchTeamPlayerCommand(int PlayerSlot, string PlayerName, int TeamNum);
public record BanPlayerCommand(int PlayerSlot, string PlayerName, string PlayerIP, bool BanIP);
public record WarnPlayerCommand(int PlayerSlot, string PlayerName, string Message);
public record KillPlayerCommand(int PlayerSlot, string PlayerName);
/// <summary>
/// Send a chat message to a specific channel
/// Channels: 0 = Server, 1 = Global, 2 = Blue Team, 3 = Red Team
/// </summary>
public record SendChatCommand(string Message, int Channel = 1); // Default to Global (1)