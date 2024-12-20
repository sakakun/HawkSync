using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using ServerManager.Classes.Objects;
using ServerManager.Properties;

namespace ServerManager.Classes.Enviroment
{
    
    public struct statusObject
    {
        public string profileName;
        public byte[] statusByte;
        public string serverPlayerCount;
        public string currentMap;
        public string currentMapGameType;
        public string currentTimer;
        public string serverStatStatus;
        public bool isRunning;
    };
    
    // At this point everything should be a static refernces to a "process" or internal call to a child object.
    public class ServerInstance
    {
        // Instance Process Object
        public InstanceProcess          instanceProcess;
        // Base Profile Information
        public ServerProfile            serverProfile;
        // Game Server - Server Settings
        public ServerSettings           serverSettings;
        // Game Server - Chat Management
        // Game Server - Map Management
        public ServerMapManagement      mapManagement;
        // Game Server - Player Management
        // Game Server - Stat Management

        public ServerInstance(ServerProfile profile)
        {
            serverProfile = profile;
            loadInstance();
            initInstance();
        }
        // This is called to initalize tickers and anychecks.
        public void initInstance()
        {
             
        }
        // Load Instance from configuration files.
        private void loadInstance()
        {
            serverSettings = new ServerSettings(this);
            mapManagement = new ServerMapManagement(this);

        }
        // This will call all save functions nested above and for the profile itself.
        public void saveInstance()
        {
            serverSettings.SaveSettings();
        }
        // Stop any active tasks/tickers, and destroy any subObjects from memory.
        // Server files will be destroyed by the serverProfile class.
        public bool destoryInstance()
        {
            return true;
        }

        public statusObject instance_Status()
        {
            statusObject thisVar = new statusObject();

            try
            {
                thisVar.profileName = serverProfile.ProfileName;
                thisVar.serverPlayerCount = "";
                thisVar.currentMap = "";
                thisVar.currentMapGameType = "";
                thisVar.currentTimer = "";
                thisVar.serverStatStatus = "";
                thisVar.isRunning = false;
                thisVar.statusByte = statusByte("notactive");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error in status_profileWindow: {ex.Message}");
            }

            return thisVar;
        }

        public static byte[] statusByte(string statusType)
        {
            byte[] statusByte = null;
            string resourceName = $"ServerManager.Resources.Images.States.{statusType}.gif"; // Adjust namespace
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (Bitmap bitmap = (Bitmap)Image.FromStream(stream))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                statusByte = memoryStream.ToArray();
            }
            return statusByte;
        }
    }
}