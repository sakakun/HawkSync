using BHD_ServerManager.API.Services;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.SupportClasses;
using HawkSyncShared;

namespace BHD_ServerManager.API
{
    public static class APICore
    {
        /// <summary>
        /// Embedded API host for remote client connections
        /// </summary>
        public static EmbeddedApiHost? ApiHost { get; private set; }
        
        /// <summary>
        /// Indicates if the API is currently running
        /// </summary>
        public static bool IsApiRunning { get; private set; }

        // ================================================================================
        // API MANAGEMENT
        // ================================================================================
        
        /// <summary>
        /// Start the embedded API host on the specified port
        /// </summary>
        /// <param name="port">Port number to bind to (default: 5000)</param>
        public static void StartApiHost(int port = 5000)
        {
            if (IsApiRunning)
            {
                AppDebug.Log($"API host already running on port {port}, skipping start", AppDebug.LogLevel.Info);
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("API host is null, creating new instance", AppDebug.LogLevel.Info);
                ApiHost = new EmbeddedApiHost();
            }

            try
            {
                ApiHost.Start(port);
                IsApiRunning = true;
                AppDebug.Log($"API host started successfully on port {port}", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Failed to start API host", AppDebug.LogLevel.Error, ex);
                IsApiRunning = false;
                throw;
            }
        }

        /// <summary>
        /// Stop the embedded API host
        /// </summary>
        public static async Task StopApiHost()
        {
            if (!IsApiRunning)
            {
                AppDebug.Log("API host not running, skipping stop", AppDebug.LogLevel.Info);
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("API host is null but IsApiRunning was true, resetting flag", AppDebug.LogLevel.Warning);
                IsApiRunning = false;
                return;
            }

            try
            {
                await ApiHost.StopAsync();
                IsApiRunning = false;
                AppDebug.Log("API host stopped successfully", AppDebug.LogLevel.Info);
            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error stopping API host", AppDebug.LogLevel.Error, ex);
                // Still mark as not running even if stop failed
                IsApiRunning = false;
            }
        }

        /// <summary>
        /// Restart the API host on a new port
        /// </summary>
        public static async Task RestartApiHost(int newPort)
        {
            AppDebug.Log($"Restarting API host on port {newPort}", AppDebug.LogLevel.Info);
            
            await StopApiHost();
            
            // Small delay to ensure port is released
            await Task.Delay(500);
            
            StartApiHost(newPort);
        }

        // ================================================================================
        // SHUTDOWN
        // ================================================================================
        
        /// <summary>
        /// Cleanup all core infrastructure on application shutdown
        /// </summary>
        public static async Task Shutdown()
        {
            AppDebug.Log("Shutting down core infrastructure...", AppDebug.LogLevel.Info);

            // Stop tickers
            CommonCore.Ticker?.StopAll();

            // Stop API if running
            if (IsApiRunning)
            {
                await StopApiHost();
            }

            // Cleanup database connections
            DatabaseManager.Shutdown();

            AppDebug.Log("Core shutdown complete", AppDebug.LogLevel.Info);
        }
    }
}