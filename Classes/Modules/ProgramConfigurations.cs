using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace ServerManager.Classes.Modules
{
    public class ProgramConfigurations
    {
        private Dictionary<string, object> settings;
        public string dataPath;
        private string configPath;
        
        public ProgramConfigurations()
        {
            settings = new Dictionary<string, object>();
            dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            configPath = Path.Combine(dataPath, "config.json");
            LoadSettings();
        }

        public void SetVar(string var, object value)
        {
            settings[var] = value;
            SaveSettings();
        }

        public object GetVar(string var)
        {
            settings.TryGetValue(var, out var value);
            return value;
        }

        private void SaveSettings()
        {
            var json = JsonSerializer.Serialize(settings);
            Directory.CreateDirectory(Path.GetDirectoryName(configPath)); // Ensure directory exists
            File.WriteAllText(configPath, json);
        }

        private void LoadSettings()
        {
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            }
        }
    }
}
