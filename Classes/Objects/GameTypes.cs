using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ServerManager.Classes.Objects;

public class GameType
{
    public int DatabaseId { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public int Bitmap { get; set; }
}

public class GameTypes
{
    public List<GameType> GameTypeList { get; private set; }
    
    public GameTypes()
    {
        // Load JSON data from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ServerManager.Resources.Database.gameTypes.json"; 

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string jsonData = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic[]>(jsonData);

            var result = new List<GameType>();

            foreach (var GameType in data)
            {
                result.Add(new GameType
                {
                    DatabaseId = GameType.id,
                    Name = GameType.name,
                    ShortName = GameType.shortname,
                    Bitmap = GameType.bitmap
                });
            }
            GameTypeList = result;
        }
    }
}