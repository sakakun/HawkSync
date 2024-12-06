using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServerManager.Classes.Modules;
using ServerManager.Classes.Objects;

namespace ServerManager.Classes.Enviroment
{
    public class ServerEnvironment
    {
        // Program Level
        public ProgramConfigurations                _programConfig;
        public Ticker                               _systemTicker;

        // Game Server Level
        public ServerProfiles                                 _serverProfiles;
        public Dictionary<ServerProfile, ServerInstance>      _serverInstances;

        // Reference Lists
        public List<CountryCode>                    _countryCodes;
        public List<GameType>                       _gameTypes;
        public Dictionary<int, List<ObjectMap>>     _defaultMaps;
        public List<ObjectMod>                      _modList;
        
        // Singleton instance
        private static ServerEnvironment            _instance;

        // Private constructor
        private ServerEnvironment()
        {
        
            // Initialize your properties here
            _programConfig = new ProgramConfigurations();
            _systemTicker = new Ticker();
            _serverInstances = new Dictionary<ServerProfile, ServerInstance>();
            _countryCodes = (new CountryCodes()).CountryCodesList;
            _gameTypes = (new GameTypes()).GameTypeList;
            
            // Default Maps
            _defaultMaps = new Dictionary<int, List<ObjectMap>>();
            _defaultMaps.Add(0, (new DefaultMaps(_gameTypes, 0)).DefaultMapList);
            
            // Default Mods
            _modList = (new Mods()).DefaultModList;
            
            // Load Profiles & Init. Instances
            _serverProfiles = new ServerProfiles(this);
            
        }

        // Static property to access the instance
        public static ServerEnvironment Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServerEnvironment();
                }
                return _instance;
            }
        }
    }
}
