using NetLimiter.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoreLib.Net;
using IPAddress = CoreLib.Net.IPAddress;

namespace NetLimiterBridge
{
    public class NetLimiterBridge
    {
        private const string PipeName = "NetLimiterPipe";
        private NLClient _client;
        private bool _isRunning;
        private bool _connectionLimitEnabled;
        private int _connectionLimit;
        private int _monitoredAppId;

        public async Task RunAsync(string hostname, ushort port, string username = "", string password = "")
        {
            SecureString securePassword = CreateSecurePassword(password);
            _client = new NLClient();

            try
            {
                // Initialize connection
                if (hostname == "localhost" || hostname == "127.0.0.1")
                {
                    _client.Connect();
                }
                else
                {
                    await Task.Run(() => _client.Connect(hostname, port, username, securePassword));
                }

                Console.WriteLine("Connected to NetLimiter");
                _isRunning = true;

                // Start Named Pipe server
                await ListenForCommandsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize: {ex.Message}");
            }
            finally
            {
                _client?.Dispose();
            }
        }

        private async Task ListenForCommandsAsync()
		{
		    while (_isRunning)
		    {
		        try
		        {
		            using (var pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
		            {
		                Console.WriteLine("Waiting for client connection...");
		                await Task.Factory.FromAsync(pipeServer.BeginWaitForConnection, pipeServer.EndWaitForConnection, null);
		                Console.WriteLine("Client connected");

		                // Process multiple commands on the same connection
		                while (pipeServer.IsConnected && _isRunning)
		                {
		                    try
		                    {
		                        // Receive command
		                        Command command;
		                        using (var reader = new StreamReader(pipeServer, System.Text.Encoding.UTF8, false, 1024, true))
		                        {
		                            string commandJson = await reader.ReadLineAsync();
		                            
		                            // Client disconnected or sent empty line
		                            if (string.IsNullOrEmpty(commandJson))
		                            {
		                                Console.WriteLine("Client disconnected or sent empty command");
		                                break;
		                            }
		                            
		                            Console.WriteLine($"Received command: {commandJson}");
		                            command = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(commandJson);
		                        }

		                        Response response = ProcessCommand(command);

		                        // Send response
		                        using (var writer = new StreamWriter(pipeServer, System.Text.Encoding.UTF8, 1024, true))
		                        {
		                            string responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(response);
		                            Console.WriteLine($"Sending response: {responseJson}");
		                            await writer.WriteLineAsync(responseJson);
		                            await writer.FlushAsync();
		                        }

		                        // If client requested to end program, exit
		                        if (command.Action.ToLower() == "endprogram")
		                        {
		                            break;
		                        }
		                    }
		                    catch (Exception ex)
		                    {
		                        Console.WriteLine($"Command processing error: {ex.Message}");
		                        break; // Exit inner loop, wait for new connection
		                    }
		                }
		                
		                Console.WriteLine("Connection closed");
		                // Exit program when connection closes
		                _isRunning = false;
		                break;
		            }
		        }
		        catch (Exception ex)
		        {
		            Console.WriteLine($"Pipe error: {ex.Message}");
		        }
		    }
		}

		private Response ProcessCommand(Command command)
		{
			Console.WriteLine($"Processing command: {command.Action}");
			try
			{
				switch (command.Action.ToLower())
				{
					case "getappid":
						return GetAppId(command.Parameters["appPath"]);
					case "getfilternames":
						return GetFilterNames();
					case "getconnections":
						return GetConnections(command.Parameters["appId"]);
					case "getfilterobject":
						return GetFilterObject(command.Parameters["filterName"]);
					case "getfilterips":
						return GetFilterIpAddresses(command.Parameters["filterName"]);
                    case "addiptofilter":
                        int subnet = command.Parameters.ContainsKey("subnet") 
                            ? int.Parse(command.Parameters["subnet"]) 
                            : 32;
                        return AddIpToFilter(command.Parameters["filterName"], command.Parameters["ipAddress"], subnet);
                    case "removeipfromfilter":
                        int removeSubnet = command.Parameters.ContainsKey("subnet") 
                            ? int.Parse(command.Parameters["subnet"]) 
                            : 32;
                        return RemoveIpFromFilter(command.Parameters["filterName"], command.Parameters["ipAddress"], removeSubnet);

					case "setconnectionlimit":
						return SetConnectionLimit(int.Parse(command.Parameters["limit"]));

					case "enableconnectionlimit":
						return EnableConnectionLimit(bool.Parse(command.Parameters["enabled"]));

					case "endprogram":
						return EndProgram();

					default:
						return new Response { Success = false, Message = "Unknown command" };
				}
			}
			catch (Exception ex)
			{
				return new Response { Success = false, Message = ex.Message };
			}
		}

		private Response GetFilterNames()
        {
	        try
	        {
		        var filters = _client.Filters;
		        var filterNames = filters.Select(f => f.Name).ToList();

		        return new Response
		        {
			        Success = true,
			        Data = filterNames,
			        Message = $"Found {filterNames.Count} filter(s)"
		        };
	        }
	        catch (Exception ex)
	        {
		        return new Response { Success = false, Message = ex.Message };
	        }
        }

        private Response GetFilterIpAddresses(string filterName)
        {
	        try
	        {
		        var filters = _client.Filters;
		        var filter = _client.Filters.FirstOrDefault(f => f.Name == filterName);

		        if (filter != null)
		        {
			        var remoteAddressFilter = filter.Functions
				        .OfType<FFRemoteAddressInRange>()
				        .FirstOrDefault();

			        if (remoteAddressFilter == null)
			        {
				        return new Response { Success = false, Message = "No existing 'Remote address in range' function found." };
			        }

			        var ipAddresses = remoteAddressFilter.Values
				        .Select(range => new
				        {
					        Start = range.Range.Start.ToString(),
					        End = range.Range.End.ToString()
				        })
				        .ToList();

			        return new Response
			        {
				        Success = true,
				        Data = ipAddresses,
				        Message = $"Found {ipAddresses.Count} IP address range(s)"
			        };
		        }

		        return new Response { Success = false, Message = "Filter not found" };
	        }
	        catch (Exception ex)
	        {
		        return new Response { Success = false, Message = ex.Message };
	        }
        }
        private Response GetAppId(string appPath)
        {
            try
            {
                var nodeLoader = _client.CreateNodeLoader();
                nodeLoader.SelectAllNodes();
                nodeLoader.Load();

                var app = nodeLoader.Applications.Nodes
                    .FirstOrDefault(a => a.AppId?.Path != null && 
                                        a.AppId.Path.EndsWith(appPath, StringComparison.OrdinalIgnoreCase));

                if (app != null)
                {
                    return new Response
                    {
                        Success = true,
                        Data = app.AppId.GetHashCode(),
                        Message = "Application found"
                    };
                }

                return new Response { Success = false, Message = "Application not found" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }

        private Response GetConnections(string appId)
        {
            Debug.WriteLine($"Getting connections for AppId: {appId}");
            try
            {
                _monitoredAppId = int.Parse(appId);
                var nodeLoader = _client.CreateNodeLoader();
                nodeLoader.SelectAllNodes();
                nodeLoader.Load();

                var connections = nodeLoader.Connections.Nodes
                    .Where(node => node.Parent?.Parent?.AppId?.Path != null &&
                                   node.Parent.Parent.AppId.GetHashCode() == int.Parse(appId))
                    .Select(c => new ConnectionInfo
                    {
                        RemoteAddress = c.RemoteAddress.ToIPAddress4().ToString(),
                        RemotePort = c.RemotePort,
                        LocalAddress = c.LocalAddress.ToIPAddress4().ToString(),
                        LocalPort = c.LocalPort,
                        Protocol = c.Protocol.ToString()
                    })
                    .ToList();

                // Check connection limit if enabled
                if (_connectionLimitEnabled && connections.Count > _connectionLimit)
                {
                    Console.WriteLine($"Connection limit exceeded: {connections.Count}/{_connectionLimit}");
                    // Trigger filter addition for excess connections
                    var excessConnections = connections.Skip(_connectionLimit);
                    foreach (var conn in excessConnections)
                    {
                        Console.WriteLine($"Excess connection detected: {conn.RemoteAddress}");
                    }
                }

                return new Response
                {
                    Success = true,
                    Data = connections,
                    Message = $"Found {connections.Count} connections"
                };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }

        private Response GetFilterObject(string filterName)
        {
            try
            {
                var filters = _client.Filters;
                var filter = _client.Filters.FirstOrDefault(f => f.Name == filterName);

                if (filter != null)
                {
                    return new Response
                    {
                        Success = true,
                        Data = filter.Id.ToString(),
                        Message = "Filter found"
                    };
                }

                return new Response { Success = false, Message = "Filter not found" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }

        private Response AddIpToFilter(string filterName, string ipAddress, int subnet)
        {
            try
            {
                var filters = _client.Filters;
                var filter = _client.Filters.FirstOrDefault(f => f.Name == filterName);

                if (filter != null)
                {
                    var remoteAddressFilter = filter.Functions
                        .OfType<FFRemoteAddressInRange>()
                        .FirstOrDefault();

                    if (remoteAddressFilter == null)
                    {
                        return new Response { Success = false, Message = "No existing 'Remote address in range' function found." };
                    }

                    // Parse and validate IP address
                    IPAddress ip = IPAddress.Parse(ipAddress);
                    var ipString = ip.ToIPAddress4().ToString();

                    // Validate subnet mask
                    if (subnet < 0 || subnet > 32)
                    {
                        return new Response { Success = false, Message = "Subnet mask must be between 0 and 32." };
                    }

                    // Calculate range start and end based on subnet mask
                    string rangeStart;
                    string rangeEnd;

                    if (subnet == 32)
                    {
                        // Single IP
                        rangeStart = ipString;
                        rangeEnd = ipString;
                    }
                    else
                    {
                        // Calculate subnet range
                        CalculateSubnetRange(ipString, subnet, out rangeStart, out rangeEnd);
                    }

                    // Check if range already exists or overlaps
                    if (remoteAddressFilter.Values.Any(range =>
                        IsInRange(range.Range.Start.ToString(), range.Range.End.ToString(), rangeStart) ||
                        IsInRange(range.Range.Start.ToString(), range.Range.End.ToString(), rangeEnd)))
                    {
                        return new Response { Success = false, Message = $"IP range {rangeStart}-{rangeEnd} already exists or overlaps with existing range." };
                    }

                    remoteAddressFilter.Values.Add(new IPRangeFilterValue(rangeStart, rangeEnd));

                    _client.UpdateFilter(filter);

                    if (rangeStart == rangeEnd)
                    {
                        return new Response { Success = true, Message = $"IP {rangeStart} added to the filter." };
                    }
                    else
                    {
                        return new Response { Success = true, Message = $"IP range {rangeStart}-{rangeEnd} (/{subnet}) added to the filter." };
                    }
                }

                return new Response { Success = false, Message = "Filter not found" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }

        private static void CalculateSubnetRange(string ipAddress, int subnetMask, out string rangeStart, out string rangeEnd)
        {
            var ip = System.Net.IPAddress.Parse(ipAddress);
            byte[] ipBytes = ip.GetAddressBytes();

            // Calculate the number of host bits
            int hostBits = 32 - subnetMask;

            // Create subnet mask bytes
            uint maskValue = subnetMask == 0 ? 0 : 0xFFFFFFFF << hostBits;
            byte[] maskBytes = new byte[4];
            maskBytes[0] = (byte)((maskValue >> 24) & 0xFF);
            maskBytes[1] = (byte)((maskValue >> 16) & 0xFF);
            maskBytes[2] = (byte)((maskValue >> 8) & 0xFF);
            maskBytes[3] = (byte)(maskValue & 0xFF);

            // Calculate network address (start of range)
            byte[] networkBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            // Calculate broadcast address (end of range)
            byte[] broadcastBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                broadcastBytes[i] = (byte)(networkBytes[i] | ~maskBytes[i]);
            }

            rangeStart = new System.Net.IPAddress(networkBytes).ToString();
            rangeEnd = new System.Net.IPAddress(broadcastBytes).ToString();
        }

        private static bool IsInRange(string startAddress, string endAddress, string ipAddress)
        {
	        var start = System.Net.IPAddress.Parse(startAddress);
	        var end = System.Net.IPAddress.Parse(endAddress);
	        var ip = System.Net.IPAddress.Parse(ipAddress);

	        var startBytes = start.GetAddressBytes();
	        var endBytes = end.GetAddressBytes();
	        var ipBytes = ip.GetAddressBytes();

	        for (int i = 0; i < startBytes.Length; i++)
	        {
		        if (ipBytes[i] < startBytes[i] || ipBytes[i] > endBytes[i])
		        {
			        return false;
		        }
	        }
	        return true;
        }

        private Response RemoveIpFromFilter(string filterName, string ipAddress, int subnet)
        {
            try
            {
                var filters = _client.Filters;
                var filter = _client.Filters.FirstOrDefault(f => f.Name == filterName);

                if (filter != null)
                {
                    var remoteAddressFilter = filter.Functions
                        .OfType<FFRemoteAddressInRange>()
                        .FirstOrDefault();

                    if (remoteAddressFilter == null)
                    {
                        return new Response { Success = false, Message = "No existing 'Remote address in range' function found." };
                    }

                    // Parse and validate IP address
                    IPAddress ip = IPAddress.Parse(ipAddress);
                    var ipString = ip.ToIPAddress4().ToString();

                    // Validate subnet mask
                    if (subnet < 0 || subnet > 32)
                    {
                        return new Response { Success = false, Message = "Subnet mask must be between 0 and 32." };
                    }

                    // Calculate range start and end based on subnet mask
                    string rangeStart;
                    string rangeEnd;

                    if (subnet == 32)
                    {
                        // Single IP
                        rangeStart = ipString;
                        rangeEnd = ipString;
                    }
                    else
                    {
                        // Calculate subnet range
                        CalculateSubnetRange(ipString, subnet, out rangeStart, out rangeEnd);
                    }

                    // Find the actual range object in the collection that matches this IP range
                    var rangeToRemove = remoteAddressFilter.Values.FirstOrDefault(range =>
                        range.Range.Start.ToString() == rangeStart && 
                        range.Range.End.ToString() == rangeEnd);

                    if (rangeToRemove != null)
                    {
                        // Remove the actual object reference from the collection
                        remoteAddressFilter.Values.Remove(rangeToRemove);
                        _client.UpdateFilter(filter);
                
                        if (rangeStart == rangeEnd)
                        {
                            return new Response { Success = true, Message = $"IP {rangeStart} removed from the filter." };
                        }
                        else
                        {
                            return new Response { Success = true, Message = $"IP range {rangeStart}-{rangeEnd} (/{subnet}) removed from the filter." };
                        }
                    }

                    return new Response
                    {
                        Success = false,
                        Message = $"IP {ipAddress} was not found in the filter {filterName}"
                    };
                }

                return new Response { Success = false, Message = "Filter not found" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };
            }
        }

        private Response SetConnectionLimit(int limit)
        {
            _connectionLimit = limit;
            return new Response
            {
                Success = true,
                Message = $"Connection limit set to {limit}"
            };
        }

        private Response EnableConnectionLimit(bool enabled)
        {
            _connectionLimitEnabled = enabled;
            return new Response
            {
                Success = true,
                Message = $"Connection limit monitoring {(enabled ? "enabled" : "disabled")}"
            };
        }

        private Response EndProgram()
        {
            _isRunning = false;
            return new Response
            {
                Success = true,
                Message = "Program terminating"
            };
        }

        private static SecureString CreateSecurePassword(string password)
        {
            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}