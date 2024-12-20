using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServerManager.Properties;

public class CountryCode
{
    public int Id { get; set; }
    public string Iso { get; set; }
    public string Name { get; set; }
    public string Nicename { get; set; }
    public string Iso3 { get; set; }
    public int? Numcode { get; set; }
    public int? Phonecode { get; set; }
}

public class CountryCodes
{
    public List<CountryCode> CountryCodesList { get; private set; }

    public CountryCodes()
    {
        try
        {
            // Load JSON data from embedded resource
            string jsonData = Resources.countryCodes;

            // Deserialize JSON data into a list of CountryCode
            var data = JsonConvert.DeserializeObject<List<CountryCode>>(jsonData);

            CountryCodesList = data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }
}