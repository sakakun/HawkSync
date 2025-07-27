using BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace BHD_ServerManager.Classes.RemoteFunctions
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHandlerAttribute : Attribute
    {
        public string CommandName { get; }
        public CommandHandlerAttribute(string commandName) => CommandName = commandName;
    }

    public class CommandProcessor
    {
        private static readonly adminInstance adminInstance = CommonCore.instanceAdmin!;

        // Dictionary maps command to (Type, handler)
        private readonly Dictionary<string, (Type type, Func<object, CommandResponse> handler)> _handlers;

        public CommandProcessor()
        {
            _handlers = new Dictionary<string, (Type, Func<object, CommandResponse>)>(StringComparer.OrdinalIgnoreCase);

            // Register static/manual handlers (if any)
            RegisterStaticHandlers();

            // Auto-register handlers with CommandHandlerAttribute
            RegisterAttributedHandlers();

        }

        private void RegisterStaticHandlers()
        {
            // If you have legacy handlers or want to keep some manual registrations, do it here.
            _handlers["Ping"] = (typeof(object), CmdPing.ProcessCommand);
            _handlers["ValidateUser"] = (typeof(AuthenticationPacket), data => CmdValidateUser.ProcessCommand((AuthenticationPacket)data));
            // ...add any other static/manual registrations here if needed...
            /*
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
            */
        }
        private void RegisterAttributedHandlers()
        {
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<CommandHandlerAttribute>() != null);

            foreach (var type in handlerTypes)
            {
                var attr = type.GetCustomAttribute<CommandHandlerAttribute>();
                // Look for a static method named "ProcessCommand" with correct signature
                var method = type.GetMethod("ProcessCommand", BindingFlags.Public | BindingFlags.Static);
                if (method != null && method.ReturnType == typeof(CommandResponse))
                {
                    // Determine parameter type for deserialization
                    var parameters = method.GetParameters();
                    var paramType = parameters.Length == 1 ? parameters[0].ParameterType : typeof(object);

                    // Create a delegate to invoke the static method
                    Func<object, CommandResponse> handler = arg =>
                    {
                        // If needed, convert arg to the expected parameter type
                        if (arg != null && paramType != typeof(object) && !paramType.IsInstanceOfType(arg))
                        {
                            if (arg is JsonElement jsonElement)
                            {
                                arg = JsonSerializer.Deserialize(jsonElement.GetRawText(), paramType)!;
                            }
                            else
                            {
                                arg = Convert.ChangeType(arg, paramType);
                            }
                        }
                        return (CommandResponse)method.Invoke(null, new[] { arg });
                    };

                    _handlers[attr.CommandName] = (paramType, handler);
                }
            }
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

            int userId = RemoteServer.AuthorizedClients
                .Where(u => !string.IsNullOrEmpty(u.AuthorizationToken) &&
                            u.AuthorizationToken.Equals(packet.AuthToken, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.ClientId)
                .FirstOrDefault();

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

                var response = entry.handler(arg);
                adminInstanceManager.AddLogEntry(userId, $"RCE - {packet.Command}: {response.Message}");
                return response;
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Unknown command: {packet.Command}"
            };
        }
    }
}