using BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BHD_ServerManager.Classes.RemoteFunctions
{
    public class CommandProcessor
    {
        private static readonly adminInstance adminInstance = CommonCore.instanceAdmin!;

        // Dictionary maps command to (Type, handler)
        private readonly Dictionary<string, (Type type, Func<object, CommandResponse> handler)> _handlers;

        public CommandProcessor()
        {
            _handlers = new Dictionary<string, (Type, Func<object, CommandResponse>)>(StringComparer.OrdinalIgnoreCase)
            {
                { "Ping", (typeof(object), CmdPing.ProcessCommand) },
                { "ValidateUser", (typeof(AuthenticationPacket), data => CmdValidateUser.ProcessCommand((AuthenticationPacket)data)) },
                { "ReadMemoryIsProcessAttached", (typeof(object), CmdReadMemoryIsProcessAttached.ProcessCommand) },
                { "startGame", (typeof(object), CmdStartGame.ProcessCommand) },
                { "stopGame", (typeof(object), CmdStopGame.ProcessCommand) },
                { "ValidateGameServerPath", (typeof(object), CmdValidateGameServerPath.ProcessCommand) },
                { "CmdSetServerVariables", (typeof(object), CmdSetServerVariables.ProcessCommand) },
                { "CmdDisarmplayer", (typeof(object), CmdDisarmplayer.ProcessCommand) },
                { "CmdRearmplayer", (typeof(object), CmdRearmplayer.ProcessCommand) },
                { "CmdResetAvailableMaps", (typeof(object), CmdResetAvailableMaps.ProcessCommand) },
                { "CmdUpdateMapCycle", (typeof(object), CmdUpdateMapCycle.ProcessCommand) },
                { "CmdUpdateNextMap", (typeof(object), CmdUpdateNextMap.ProcessCommand) },
                { "CmdMapScore", (typeof(object), CmdMapScore.ProcessCommand) },
                { "CmdMapSkip", (typeof(object), CmdMapSkip.ProcessCommand) },
                { "CmdSendChatMessage", (typeof(object), CmdSendChatMessage.ProcessCommand) },
                { "CmdSendConsoleMessage", (typeof(object), CmdSendConsoleMessage.ProcessCommand) },
                { "CmdTogglePlayerGodMode", (typeof(object), CmdTogglePlayerGodMode.ProcessCommand) },
                { "CmdKillPlayer", (typeof(object), CmdKillPlayer.ProcessCommand) },
                { "CmdSwitchPlayerTeam", (typeof(object), CmdSwitchPlayerTeam.ProcessCommand) },
                { "CmdAddBannedPlayer", (typeof(object), CmdAddBannedPlayer.ProcessCommand) },
                { "CmdRemoveBannedPlayerAddress", (typeof(object), CmdRemoveBannedPlayerAddress.ProcessCommand) },
                { "CmdRemoveBannedPlayerBoth", (typeof(object), CmdRemoveBannedPlayerBoth.ProcessCommand) },
                { "CmdRemoveBannedPlayerName", (typeof(object), CmdRemoveBannedPlayerName.ProcessCommand) },
                { "CmdAddAutoMessage", (typeof(object), CmdAddAutoMessage.ProcessCommand) },
                { "CmdAddSlapMessage", (typeof(object), CmdAddSlapMessage.ProcessCommand) },
                { "CmdRemoveSlapMessage", (typeof(object), CmdRemoveSlapMessage.ProcessCommand) },
                { "CmdRemoveAutoMessage", (typeof(object), CmdRemoveAutoMessage.ProcessCommand) },
                { "CmdaddAdminAccount", (typeof(object), CmdaddAdminAccount.ProcessCommand) },
                { "CmdeditAdminAccount", (typeof(object), CmdeditAdminAccount.ProcessCommand) },
                { "CmddeleteAdminAccount", (typeof(object), CmddeleteAdminAccount.ProcessCommand) },
                { "CmdTestBabstatsConnection", (typeof(object), CmdTestBabstatsConnection.ProcessCommand) },
            };
        }

        public CommandResponse ProcessCommand(CommandPacket packet)
        {
            if (packet == null)
                return new CommandResponse { Success = false, Message = "Packet is null." };

            if (string.IsNullOrWhiteSpace(packet.Command))
                return new CommandResponse { Success = false, Message = "Command is missing." };

            // Authorization checks (unchanged)
            if (string.IsNullOrWhiteSpace(packet.AuthToken) && packet.Command != "ValidateUser")
                return new CommandResponse
                {
                    Success = false,
                    Message = "Authorization token is missing or invalid."
                };

            bool isAuthorized = RemoteServer.AuthorizedClients
                .Any(u => !string.IsNullOrEmpty(u.AuthorizationToken) &&
                          u.AuthorizationToken.Equals(packet.AuthToken, StringComparison.OrdinalIgnoreCase));

            if (!isAuthorized && packet.Command != "ValidateUser")
            {
                return new CommandResponse
                {
                    Success = false,
                    Message = "Unauthorized access."
                };
            }

            if (_handlers.TryGetValue(packet.Command, out var entry))
            {
                object arg = packet.CommandData;
                if (arg is JsonElement jsonElement && entry.type != typeof(object))
                {
                    arg = JsonSerializer.Deserialize(jsonElement.GetRawText(), entry.type)!;
                }
                return entry.handler(arg);
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Unknown command: {packet.Command}"
            };
        }
    }
}