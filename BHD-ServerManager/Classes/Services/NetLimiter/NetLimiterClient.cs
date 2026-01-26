using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.Services.NetLimiter
{
    public static class NetLimiterClient
    {
        private const string PipeName = "NetLimiterPipe";
        private const int Timeout = 5000;

        private static NamedPipeClientStream? _pipeClient;
        private static SemaphoreSlim _pipeLock = new SemaphoreSlim(1, 1);

        public static Process? _bridgeProcess;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public static async Task<Response> SendCommandAsync(Command command)
        {
	        await _pipeLock.WaitAsync();
	        try
	        {
		        // Create connection if not exists or disconnected
		        if (_pipeClient == null || !_pipeClient.IsConnected)
		        {
			        _pipeClient?.Dispose();
			        _pipeClient =
				        new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
			        await _pipeClient.ConnectAsync(Timeout);
		        }

		        // Send command
		        string commandJson = JsonSerializer.Serialize(command, JsonOptions);
		        using (var writer = new StreamWriter(_pipeClient, Encoding.UTF8, 1024, true))
		        {
			        await writer.WriteLineAsync(commandJson);
			        await writer.FlushAsync();
		        }

		        // Receive response
		        using (var reader = new StreamReader(_pipeClient, Encoding.UTF8, false, 1024, true))
		        {
			        string? responseJson = await reader.ReadLineAsync();
			        if (string.IsNullOrEmpty(responseJson))
			        {
				        throw new InvalidOperationException("No response received from server");
			        }

			        Response response = JsonSerializer.Deserialize<Response>(responseJson, JsonOptions)!;
			        return response!;
		        }
	        }
	        finally
	        {
		        _pipeLock.Release();
	        }
        }


        public static async Task<int> GetAppId(string appPath)
        {
	        AppDebug.Log("GetAppIdAsync", "Grabbing Application ID");

	        var command = new Command
	        {
		        Action = "getappid",
		        Parameters = new Dictionary<string, string> { { "appPath", appPath } }
	        };

	        var response = await SendCommandAsync(command);
	        if (!response.Success || response.Data == null)
	        {
		        return 0;
	        }

	        // Convert JsonElement to int
	        if (response.Data is JsonElement jsonElement)
	        {
		        return jsonElement.GetInt32();
	        }

	        return Convert.ToInt32(response.Data);
        }

        public static async Task<List<ConnectionInfo>> GetConnectionsAsync(int appId)
        {
            var command = new Command
            {
                Action = "getconnections",
                Parameters = new Dictionary<string, string> { { "appId", appId.ToString() } }
            };

            var response = await SendCommandAsync(command);
            if (response.Success && response.Data != null)
            {
                // Deserialize JsonElement to List<ConnectionInfo>
                var jsonElement = (JsonElement)response.Data;
                return JsonSerializer.Deserialize<List<ConnectionInfo>>(jsonElement.GetRawText(), JsonOptions)!;
            }
            return new List<ConnectionInfo>();
        }

        public static async Task<bool> AddIpToFilterAsync(string filterName, string ipAddress, int subnet)
        {
            var command = new Command
            {
                Action = "addIpToFilter",
                Parameters = new Dictionary<string, string>
                {
                    { "filterName", filterName },
                    { "ipAddress", ipAddress },
					{ "subnet", subnet.ToString() }
                }
            };

            var response = await SendCommandAsync(command);
            return response.Success;
        }

        public static async Task<bool> RemoveIpFromFilterAsync(string filterName, string ipAddress, int subnet)
        {
            var command = new Command
            {
                Action = "removeIpFromFilter",
                Parameters = new Dictionary<string, string>
                {
                    { "filterName", filterName },
                    { "ipAddress", ipAddress },
					{ "subnet", subnet.ToString() }
                }
            };

            var response = await SendCommandAsync(command);
            return response.Success;
        }

        public static async Task<List<string>> GetFilterIpAddressesAsync(string filterName)
        {
	        var command = new Command
	        {
		        Action = "getfilterips",
		        Parameters = new Dictionary<string, string> { { "filterName", filterName } }
	        };

	        var response = await SendCommandAsync(command);
	        if (response.Success && response.Data != null)
	        {
		        // Deserialize JsonElement to List of IP range objects
		        var jsonElement = (JsonElement)response.Data;
		        var ipRanges = JsonSerializer.Deserialize<List<IpRange>>(jsonElement.GetRawText(), JsonOptions);
        
		        // Convert to list of IP strings (for single IPs, Start == End)
		        var ipList = new List<string>();
		        foreach (var range in ipRanges!)
		        {
			        if (range.Start == range.End)
			        {
				        ipList.Add(range.Start!);
			        }
			        else
			        {
				        ipList.Add($"{range.Start}-{range.End}");
			        }
		        }
        
		        return ipList;
	        }
    
	        return new List<string>();
        }

        private class IpRange
        {
	        public string? Start { get; set; }
	        public string? End { get; set; }
        }

        public static void StartBridgeProcess(string hostname = "localhost", ushort port = 11111, string username = "", string password = "")
		{

		    if (_bridgeProcess != null && !_bridgeProcess.HasExited)
		    {
		        AppDebug.Log("NetLimiterClient", "Bridge process already running");
		        return;
		    }

		    try
		    {
		        var bridgePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetLimiterBridge", "NetLimiterBridge.exe");
		        
		        if (!File.Exists(bridgePath))
		        {
		            throw new FileNotFoundException($"NetLimiterBridge.exe not found at {bridgePath}");
		        }

				string varArguments = string.Empty;
                if (hostname.Contains("localhost") || hostname.Contains("127.0.0.1")) {
					varArguments = $"{hostname} {port} {username} {password}";
				}

		        var startInfo = new ProcessStartInfo
		        {
		            FileName = bridgePath,
					Arguments = varArguments,
		            UseShellExecute = false,
		            CreateNoWindow = true, // Hide console window
		            RedirectStandardOutput = true,
		            RedirectStandardError = true,
		            WindowStyle = ProcessWindowStyle.Hidden
		        };

		        _bridgeProcess = Process.Start(startInfo);
		        
		        // Optional: Capture output for debugging
		        _bridgeProcess!.OutputDataReceived += (sender, e) => 
		        {
		            if (!string.IsNullOrEmpty(e.Data))
		                AppDebug.Log("NetLimiterBridge", e.Data);
		        };
		        _bridgeProcess.ErrorDataReceived += (sender, e) => 
		        {
		            if (!string.IsNullOrEmpty(e.Data))
		                AppDebug.Log("NetLimiterBridge [ERROR]", e.Data);
		        };
		        
		        _bridgeProcess.BeginOutputReadLine();
		        _bridgeProcess.BeginErrorReadLine();
		        
		        AppDebug.Log("NetLimiterClient", "Bridge process started");
		        
		        // Give it time to initialize
		        Task.Delay(1000).Wait();
		    }
		    catch (Exception ex)
		    {
		        AppDebug.Log("NetLimiterClient", $"Failed to start bridge process: {ex.Message}");
		        throw;
		    }
		}
		public static async Task<List<string>> GetFilterNamesAsync()
		{
			var command = new Command
			{
				Action = "getfilternames",
				Parameters = new Dictionary<string, string>()
			};

			var response = await SendCommandAsync(command);
			if (response.Success && response.Data != null)
			{
				// Deserialize JsonElement to List<string>
				var jsonElement = (JsonElement)response.Data;
				return JsonSerializer.Deserialize<List<string>>(jsonElement.GetRawText(), JsonOptions)!;
			}
    
			return new List<string>();
		}
		public static void StopBridgeProcess()
		{
		    if (_bridgeProcess != null && !_bridgeProcess.HasExited)
		    {
		        try
		        {
		            _bridgeProcess.Kill();
		            _bridgeProcess.Dispose();
		            _bridgeProcess = null;
		            AppDebug.Log("NetLimiterClient", "Bridge process stopped");
		        }
		        catch (Exception ex)
		        {
		            AppDebug.Log("NetLimiterClient", $"Error stopping bridge process: {ex.Message}");
		        }
		    }
		}

    }
}