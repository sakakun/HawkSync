using BHD_ServerManager.API.Services;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.SupportClasses;
using HawkSyncShared;
using System;
using System.IO;
using System.Threading.Tasks;

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
                AppDebug.Log("APICore", $"API host already running on port {port}, skipping start");
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("APICore", "API host is null, creating new instance");
                ApiHost = new EmbeddedApiHost();
            }

            try
            {
                ApiHost.Start(port);
                IsApiRunning = true;
                AppDebug.Log("APICore", $"✅ API host started successfully on port {port}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("APICore", $"❌ Failed to start API host: {ex.Message}");
                IsApiRunning = false;
                throw; // Re-throw so caller knows it failed
            }
        }

        /// <summary>
        /// Stop the embedded API host
        /// </summary>
        public static async Task StopApiHost()
        {
            if (!IsApiRunning)
            {
                AppDebug.Log("APICore", "API host not running, skipping stop");
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("APICore", "API host is null but IsApiRunning was true, resetting flag");
                IsApiRunning = false;
                return;
            }

            try
            {
                await ApiHost.StopAsync();
                IsApiRunning = false;
                AppDebug.Log("APICore", "✅ API host stopped successfully");
            }
            catch (Exception ex)
            {
                AppDebug.Log("CommonCore", $"❌ Error stopping API host: {ex.Message}");
                // Still mark as not running even if stop failed
                IsApiRunning = false;
            }
        }

        /// <summary>
        /// Restart the API host on a new port
        /// </summary>
        public static async Task RestartApiHost(int newPort)
        {
            AppDebug.Log("APICore", $"Restarting API host on port {newPort}");
            
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
            AppDebug.Log("APICore", "Shutting down core infrastructure...");

            // Stop tickers
            CommonCore.Ticker?.StopAll();

            // Stop API if running
            if (IsApiRunning)
            {
                await StopApiHost();
            }

            // Cleanup database connections
            DatabaseManager.Shutdown();

            AppDebug.Log("APICore", "Core shutdown complete");
        }
    }
}