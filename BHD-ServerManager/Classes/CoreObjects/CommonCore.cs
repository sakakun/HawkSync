using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.SupportClasses;
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
        public static banInstance? instanceBans { get; set; }
        public static statInstance? instanceStats { get; set; }


        // Object: Ticker, Timer Constructor for periodic tasks
        public static Ticker? Ticker { get; set; }

        public static void InitializeCore()
        {
            // Instances
            theInstance = new theInstance();
            instanceChat = new chatInstance();
            instanceBans = new banInstance();
            instanceStats = new statInstance();

            // Initialize the Ticker
            Ticker = new Ticker();
        }

    }
}
