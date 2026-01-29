using BHD_ServerManager.API.Services;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.CoreObjects
{
    public static class CommonCore
    {
        // ================================================================================
        // PATHS
        // ================================================================================
        
        /// <summary>
        /// Path to the application data directory
        /// </summary>
        public static string AppDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings") + Path.DirectorySeparatorChar;

        // ================================================================================
        // INSTANCE OBJECTS
        // ================================================================================
        
        public static theInstance? theInstance { get; set; }
        public static chatInstance? instanceChat { get; set; }
        public static statInstance? instanceStats { get; set; }
        public static playerInstance? instancePlayers { get; set; }
        public static adminInstance? instanceAdmin { get; set; } 
        public static mapInstance? instanceMaps { get; set; }
        public static banInstance? instanceBans { get; set; }

        // ================================================================================
        // INFRASTRUCTURE
        // ================================================================================
        
        /// <summary>
        /// Ticker for periodic tasks
        /// </summary>
        public static Ticker? Ticker { get; set; }

        /// <summary>
        /// Embedded API host for remote client connections
        /// </summary>
        public static EmbeddedApiHost? ApiHost { get; private set; }
        
        /// <summary>
        /// Indicates if the API is currently running
        /// </summary>
        public static bool IsApiRunning { get; private set; }

        // ================================================================================
        // INITIALIZATION
        // ================================================================================
        
        /// <summary>
        /// Initialize all core instances and infrastructure
        /// </summary>
        public static void InitializeCore()
        {
            AppDebug.Log("CommonCore", "Initializing core instances...");

            // Initialize instances
            theInstance = new theInstance();
            instanceChat = new chatInstance();
            instanceStats = new statInstance();
            instancePlayers = new playerInstance();
            instanceAdmin = new adminInstance();
            instanceMaps = new mapInstance();
            instanceBans = DatabaseManager.LoadBanInstance();

            // Initialize ticker
            Ticker = new Ticker();

            // Create API host (but don't start - managed by ticker based on settings)
            ApiHost = new EmbeddedApiHost();

            AppDebug.Log("CommonCore", "Core initialization complete");
        }

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
                AppDebug.Log("CommonCore", $"API host already running on port {port}, skipping start");
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("CommonCore", "API host is null, creating new instance");
                ApiHost = new EmbeddedApiHost();
            }

            try
            {
                ApiHost.Start(port);
                IsApiRunning = true;
                AppDebug.Log("CommonCore", $"✅ API host started successfully on port {port}");
            }
            catch (Exception ex)
            {
                AppDebug.Log("CommonCore", $"❌ Failed to start API host: {ex.Message}");
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
                AppDebug.Log("CommonCore", "API host not running, skipping stop");
                return;
            }

            if (ApiHost == null)
            {
                AppDebug.Log("CommonCore", "API host is null but IsApiRunning was true, resetting flag");
                IsApiRunning = false;
                return;
            }

            try
            {
                await ApiHost.StopAsync();
                IsApiRunning = false;
                AppDebug.Log("CommonCore", "✅ API host stopped successfully");
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
            AppDebug.Log("CommonCore", $"Restarting API host on port {newPort}");
            
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
            AppDebug.Log("CommonCore", "Shutting down core infrastructure...");

            // Stop tickers
            Ticker?.StopAll();

            // Stop API if running
            if (IsApiRunning)
            {
                await StopApiHost();
            }

            // Cleanup database connections
            DatabaseManager.Shutdown();

            AppDebug.Log("CommonCore", "Core shutdown complete");
        }
    }
}