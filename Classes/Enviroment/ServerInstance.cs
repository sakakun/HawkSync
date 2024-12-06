using ServerManager.Classes.Objects;

namespace ServerManager.Classes.Enviroment
{
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
        
            
        }
        // This will call all save functions nested above and for the profile itself.
        public void saveInstance()
        {
            
            
        }
        // Stop any active tasks/tickers, and destroy any subObjects from memory.
        // Server files will be destroyed by the serverProfile class.
        public bool destoryInstance()
        {
            return true;
        }
        
    }
}