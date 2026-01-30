using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using System;
using System.IO;

namespace BHD_ServerManager.Classes.CoreObjects
{
    public static class CommonCore
    {
        // Static Variables
        // Variable: AppDataPath, Path to the application data directory
        public static string AppDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings") + Path.DirectorySeparatorChar;

        // Static Instance Objects
        // Object: thisInstance, Memory and Variable storage for the current instance of the application
        public static theInstance? theInstance { get; set; }
        public static chatInstance? instanceChat { get; set; }
        public static statInstance? instanceStats { get; set; }
        public static playerInstance? instancePlayers { get; set; }
        public static adminInstance? instanceAdmin { get; set; } 
        public static mapInstance? instanceMaps { get; set; }

        public static banInstance? instanceBans { get; set; }
		// Object: Ticker, Timer Constructor for periodic tasks
		public static Ticker? Ticker { get; set; }

        public static void InitializeCore()
        {
            // Instances
            theInstance = new theInstance();
            instanceChat = new chatInstance();
            instanceStats = new statInstance();
            instancePlayers = new playerInstance();
            instanceAdmin = new adminInstance();
            instanceMaps = new mapInstance();
            instanceBans = DatabaseManager.LoadBanInstance();

            // Initialize the Ticker
            Ticker = new Ticker();
        }

    }
}
