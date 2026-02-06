namespace HawkSyncShared.DTOs.tabPlayers;

public record ArmPlayerCommand(int PlayerSlot, string PlayerName);
public record DisarmPlayerCommand(int PlayerSlot, string PlayerName);
public record KickPlayerCommand(int PlayerSlot, string PlayerName);
public record GodModePlayerCommand(int PlayerSlot, string PlayerName);
public record SwitchTeamPlayerCommand(int PlayerSlot, string PlayerName, int TeamNum);
public record BanPlayerCommand(int PlayerSlot, string PlayerName, string PlayerIP, bool BanIP);
public record WarnPlayerCommand(int PlayerSlot, string PlayerName, string Message);
public record KillPlayerCommand(int PlayerSlot, string PlayerName);
public record SendChatCommand(string Message, int Channel = 1);