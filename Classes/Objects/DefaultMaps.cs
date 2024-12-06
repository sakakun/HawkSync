using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ServerManager.Classes.Objects;

public class ObjectMap
{
    public string MapFile { get; set; }
    public string MapName { get; set; }
    public string GameType { get; set; }
    public bool CustomMap { get; set; }
    public List<int> GameTypes { get; set; }
}

public class DefaultMaps
{
    public List<ObjectMap> DefaultMapList { get; private set; }
    
    public DefaultMaps(List<GameType> gameTypes, int modId)
    {
        // Load JSON data from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ServerManager.Resources.Database.defaultMaps.json"; 

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string jsonData = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic[]>(jsonData);

            var result = new List<ObjectMap>();

            foreach (var map in data)
            {
                if (map.game == modId)
                {
                   List<int> types = new List<int>();
            
                    types.Add( gameTypes.FirstOrDefault(gt => gt.ShortName == map.gametype.ToString())!.DatabaseId );
                
                    result.Add(new ObjectMap
                    {
                        MapFile = map.mission_file,
                        MapName = map.mission_name,
                        CustomMap = false,
                        GameTypes = types
                    }); 
                }
            }
            
            DefaultMapList = result;
        }
    }
}