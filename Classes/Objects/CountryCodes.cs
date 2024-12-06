using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ServerManager.Classes.Objects;

public class CountryCode
{
    public string Iso { get; set; }
    public string Nicename { get; set; }
}

public class CountryCodes
{
    public List<CountryCode> CountryCodesList { get; private set; }
    
    public CountryCodes()
    {
        // Load JSON data from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "ServerManager.Resources.Database.countryCodes.json"; 

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            string jsonData = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<dynamic[]>(jsonData);

            var result = new List<CountryCode>();

            foreach (var country in data)
            {
                result.Add(new CountryCode
                {
                    Iso = country.iso,
                    Nicename = country.nicename
                });
            }
            CountryCodesList = result;
        }
    }
}