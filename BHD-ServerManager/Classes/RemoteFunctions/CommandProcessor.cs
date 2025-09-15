using BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
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
            _handlers["Ping"] = (typeof(object), ConsoleCmdPing.ProcessCommand);
            _handlers["ValidateUser"] = (typeof(AuthenticationPacket), data => CmdValidateUser.ProcessCommand((AuthenticationPacket)data));

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
                        return (CommandResponse)method.Invoke(null, new[] { arg })!;
                    };

                    _handlers[attr!.CommandName] = (paramType, handler);
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
                if (response.Message == string.Empty)
                {
                    // If no message is set, use a default success message no need to log this.
                    return response;
                }
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