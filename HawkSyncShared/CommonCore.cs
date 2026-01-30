using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HawkSyncShared
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
            instanceBans = new banInstance();

            // Initialize ticker
            Ticker = new Ticker();

            AppDebug.Log("CommonCore", "Core initialization complete");
        }

    }
}