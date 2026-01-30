using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.Services.NetLimiter;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.Tickers
{
    public static class tickerNetLimiterMonitor
    {
        // Global Variables
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static int _appId;
        public static bool IsInitialized = false;

        // Helper for UI thread safety
        private static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        // Initialize the NetLimiter client and get the app ID
        public static async Task<bool> InitializeAsync(string appPath)
        {
	        AppDebug.Log("tickerNetLimiterMonitor", $"Attempting to initialize NetLimiter with AppPath: {appPath}");
	        
            try
            {
	            // Normalize path: replace double backslashes with single backslash
	            string normalizedPath = appPath.Replace("\\\\", "\\");
	            _appId = await NetLimiterClient.GetAppId(normalizedPath);
                AppDebug.Log("tickerNetLimiterMonitor", $"App ID: {_appId}");
                IsInitialized = _appId != 0 ? true:false;
                
                if (IsInitialized)
                {
                    AppDebug.Log("tickerNetLimiterMonitor", $"Initialized with AppId: {_appId}");
                }
                else
                {
                    AppDebug.Log("tickerNetLimiterMonitor", "Failed to get AppId from NetLimiter");
                }
                
                return IsInitialized;
            }
            catch (Exception ex)
            {
                AppDebug.Log("tickerNetLimiterMonitor", $"Error initializing NetLimiter: {ex.Message}");
                return false;
            }
        }

        // This method runs the ticker for NetLimiter connection monitoring.
        public static void runTicker()
        {
	        AppDebug.Log($"tickerNetLimiterMonitor", "Running Ticker");
            // Always marshal to UI thread for UI updates
            SafeInvoke(thisServer, () =>
            {
                if (!IsInitialized)
                {
                    AppDebug.Log("tickerNetLimiterMonitor", "NetLimiter not initialized. Ticker skipping.");
                    return;
                }

                Task.Run(async () =>
                {
	                try
	                {
		                List<ConnectionInfo> connections = await NetLimiterClient.GetConnectionsAsync(_appId);
                                
		                // Process connections on UI thread
		                SafeInvoke(thisServer, () =>
		                {
			                ProcessConnections(connections);
		                });
	                }
	                catch (Exception ex)
	                {
		                AppDebug.Log("tickerNetLimiterMonitor", $"Error getting connections: {ex.Message}");
		                IsInitialized = false;
	                }
                });
                
            });
        }

        // Process the retrieved connections
        private static void ProcessConnections(List<ConnectionInfo> connections)
        {
            if (connections == null || connections.Count == 0)
            {
                AppDebug.Log("tickerNetLimiterMonitor", "No active connections found.");
                return;
            }
            AppDebug.Log("tickerNetLimiterMonitor", $"Found {connections.Count} active connection(s)");
            
            // TODO: Add your connection processing logic here
            // For example: monitor specific IPs, track connection counts, etc.
            foreach (var connection in connections)
            {
                // Process each connection as needed
                AppDebug.Log("tickerNetLimiterMonitor", $"Connection: {connection.RemoteAddress}");
            }
        }

    }
}