using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.IO;

namespace BHD_SharedResources.Classes.CoreObjects
{
    public static class CommonCore
    {
        // Static Variables
        // Variable: AppDataPath, Path to the application data directory
        public static string AppDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings") + Path.DirectorySeparatorChar;

        // Static Instance Objects
        // Object: thisInstance, Memory and Variable storage for the current instance of the application
        public static theInstance theInstance { get; set; } = null;
        public static mapInstance instanceMaps { get; set; } = null;
        public static chatInstance instanceChat { get; set; } = null;
        public static banInstance instanceBans { get; set; } = null;
        public static statInstance instanceStats { get; set; } = null;
        public static adminInstance instanceAdmin { get; set; } = null;


        // Object: Ticker, Timer Constructor for periodic tasks
        public static Ticker Ticker { get; set; }

        public static void InitializeCore()
        {
            // Instances
            theInstance = new theInstance();
            instanceMaps = new mapInstance();
            instanceChat = new chatInstance();
            instanceBans = new banInstance();
            instanceStats = new statInstance();
            instanceAdmin = new adminInstance();

            // Initialize the Ticker
            Ticker = new Ticker();
        }

    }
}
