using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ServerManager.Classes.Objects;

public class ObjectMod
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Game { get; set; }
    public string Args { get; set; }
    public string Pff { get; set; }
    public byte[] ModIcon { get; set; }
}
public class Mods
{
    public List<ObjectMod> DefaultModList { get; private set; }

    public Mods()
    {
        // Load JSON data from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ServerManager.Resources.Database.mods.json"; 

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string jsonData = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic[]>(jsonData);

            var result = new List<ObjectMod>();

            foreach (var mod in data)
            {
                // Construct the resource name for the mod icon
                string iconResourceName = $"ServerManager.Resources.Images.Mods.{mod.id}.gif";
            
                byte[] ModIcon;
                using (Stream iconStream = assembly.GetManifestResourceStream(iconResourceName))
                {
                    if (iconStream != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            iconStream.CopyTo(ms);
                            ModIcon = ms.ToArray();
                        }
                    }
                    else
                    {
                        ModIcon = null; // Handle case where icon is not found
                    }
                }

                result.Add(new ObjectMod
                {
                    Id = mod.id,
                    Name = mod.name,
                    Game = mod.game,
                    Args = mod.args,
                    Pff = mod.pff,
                    ModIcon = ModIcon
                }); 
            }
        
            DefaultModList = result;
        }
    }
    
    
}
