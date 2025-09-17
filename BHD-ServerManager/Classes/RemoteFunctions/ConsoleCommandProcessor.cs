using BHD_ServerManager.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.RemoteFunctions
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleCommandHandlerAttribute : Attribute
    {
        public string CommandName { get; }
        public ConsoleCommandHandlerAttribute(string commandName) => CommandName = commandName;
    }

    public class ConsoleCommandProcessor
    {
        // Dictionary maps command to (Type, handler)
        private readonly Dictionary<string, (Type type, Delegate handler)> _handlers;

        public ConsoleCommandProcessor()
        {
            _handlers = new Dictionary<string, (Type, Delegate)>(StringComparer.OrdinalIgnoreCase);

            // Register static/manual handlers (if any)
            RegisterStaticHandlers();

            // Auto-register handlers with CommandHandlerAttribute
            RegisterAttributedHandlers();
        }

        private void RegisterStaticHandlers()
        {
            // If you have legacy handlers or want to keep some manual registrations, do it here.
            // _handlers["Ping"] = (typeof(object), CmdPing.ProcessCommand);
        }

        private void RegisterAttributedHandlers()
        {
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<ConsoleCommandHandlerAttribute>() != null);

            foreach (var type in handlerTypes)
            {
                var attr = type.GetCustomAttribute<ConsoleCommandHandlerAttribute>();
                var method = type.GetMethod("ProcessCommand", BindingFlags.Public | BindingFlags.Static);
                if (method != null && method.ReturnType == typeof(CommandResponse))
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 2 &&
                        parameters[0].ParameterType == typeof(string) &&
                        parameters[1].ParameterType == typeof(string[]))
                    {
                        // Handler expects (string AuthToken, string[] data)
                        Func<string, string[], CommandResponse> handler = (authToken, args) =>
                            (CommandResponse)method.Invoke(null, new object[] { authToken, args })!;

                        _handlers[attr!.CommandName] = (typeof(void), handler);
                    }
                    else if (parameters.Length == 1)
                    {
                        // Fallback: single parameter handler
                        var paramType = parameters[0].ParameterType;
                        Func<object, CommandResponse> handler = arg =>
                        {
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
        }

        public CommandResponse ProcessCommand(string AuthToken, string[] commandArgs)
        {
            if (commandArgs.Length < 2)
            {
                return new CommandResponse
                {
                    Success = false,
                    Message = "No command specified."
                };
            }

            // Example: !rc "ping"
            var command = commandArgs[1];

            if (_handlers.TryGetValue(command, out var entry))
            {
                if (entry.handler is Func<string, string[], CommandResponse> dualHandler)
                {
                    return dualHandler(AuthToken, commandArgs);
                }
                else if (entry.handler is Func<object, CommandResponse> singleHandler)
                {
                    return singleHandler(commandArgs);
                }
            }

            return new CommandResponse
            {
                Success = false,
                Message = $"Unknown command: {command}"
            };
        }
    }

}
