using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ServerManager.Properties;

namespace ServerManager.Classes.Objects;

public class ObjectMap
{
    public int id { get; set; }
    public string mission_name { get; set; }
    public string mission_file { get; set; }
    public string gametype  { get; set; }
    public int game { get; set; }
    public bool CustomMap  { get; set; }
    public List<int> GameTypes { get; set; }

    public ObjectMap()
    {
        GameTypes = new List<int>();
    }
}

public class DefaultMaps
{
    public List<ObjectMap> DefaultMapList { get; private set; }

    public DefaultMaps(List<GameType> gameTypes, int modId)
    {
        string jsonData = Resources.defaultMaps;
        var data = JsonConvert.DeserializeObject<List<ObjectMap>>(jsonData); // Deserialize to List<ObjectMap>
        
        DefaultMapList = data
            .Where(map => map.game == modId) // Filter maps by GameType
            .Select(map => new ObjectMap
            {
                mission_file = map.mission_file,
                mission_name = map.mission_name,
                CustomMap = false,
                gametype = map.gametype,
                GameTypes = new List<int>
                {
                    gameTypes.FirstOrDefault(gt => gt.ShortName == map.gametype)?.Bitmap ?? 0 // Populate GameTypes
                }
            })
            .ToList(); // Convert to List<ObjectMap>
        
    }
    
    public static void sum_up_recursive(List<int> numbers, int target, List<int> partial, ref ObjectMap map)
    {
        int s = 0;
        foreach (int x in partial) s += x;

        if (s == target)
        {
            map.GameTypes = partial; // Update GameTypes with the found combination
        }

        if (s >= target)
            return;

        for (int i = 0; i < numbers.Count; i++)
        {
            List<int> remaining = new List<int>();
            int n = numbers[i];
            for (int j = i + 1; j < numbers.Count; j++) remaining.Add(numbers[j]);

            List<int> partial_rec = new List<int>(partial);
            partial_rec.Add(n);
            sum_up_recursive(remaining, target, partial_rec, ref map);
        }
    }
}