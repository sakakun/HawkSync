using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerManager.Classes.Enviroment;

public class ObjectServerSettings
{
    public string           PresetName              { get; set; } = "Default";
    public string           ServerName              { get; set; } = "ChangeMe Server Name";
    public string           Motd                    { get; set; } = "Welcome to the Server";
    public string           CountryCode             { get; set; } = "US";
    public string           ServerPassword          { get; set; } = String.Empty;
    public int              SessionType             { get; set; } = 0;
    public int              MaxSlots                { get; set; } = 50;
    public int              StartDelay              { get; set; } = 1;
    public int              LoopMaps                { get; set; } = 1;
    public int              MaxKills                { get; set; } = 250;
    public int              GameScore               { get; set; } = 10;
    public int              ZoneTimer               { get; set; } = 10;
    public int              RespawnTime             { get; set; } = 22;
    public int              TimeLimit               { get; set; } = 22;
    public bool             RequireNovaLogic        { get; set; } = false;
    public bool             RunWindowed             { get; set; } = true;
    public bool             AllowCustomSkins        { get; set; } = true;
    public int              GameMod                 { get; set; } = 0;
    public string           BlueTeamPassword        { get; set; } = String.Empty;
    public string           RedTeamPassword         { get; set; } = String.Empty;
    public bool             FriendlyFire            { get; set; } = false;
    public bool             FriendlyFireWarning     { get; set; } = true;
    public bool             FriendlyTags            { get; set; } = true;
    public bool             AutoBalance             { get; set; } = true;
    public bool             ShowTracers             { get; set; } = false;
    public bool             ShowTeamClays           { get; set; } = true;
    public bool             AllowAutoRange          { get; set; } = false;
    public bool             EnableMinPing           { get; set; } = false;
    public int              MinPing                 { get; set; } = 0;
    public bool             EnableMaxPing           { get; set; } = false;
    public int              MaxPing                 { get; set; } = 999; 
    
}

public class SettingsData
{
    public ObjectServerSettings CurrentSettings { get; set; }
    public Dictionary<string, ObjectServerSettings> ArchievedSettings { get; set; }
}

public class ServerSettings
{
    [JsonIgnore]
    private ServerInstance                                  _instance;
    [JsonIgnore]
    private string                                          _settingsPath;
    
    // Current Server Settings
    public ObjectServerSettings                             CurrentSettings         { get; private set; }
    public Dictionary<string, ObjectServerSettings>         ArchievedSettings       { get; private set; }
    

    public ServerSettings(ServerInstance serverInstance)
    {
        _instance = serverInstance;
        _settingsPath = Path.Combine(_instance.serverProfile.ProfilePath, "serverSettings.json");
        LoadSettings();
    }
    
    public void AddArchievedSettings(ObjectServerSettings settings)
    {
        ArchievedSettings.Add(settings.PresetName, settings);
        SaveSettings();
    }
    
    public void RemoveArchievedSettings(ObjectServerSettings settings)
    {
        ArchievedSettings.Remove(settings.PresetName);
        SaveSettings();
    }
    
    public object Get(string setting, string archievedKey = null)
    {
        if (archievedKey == null)
        {
            var propertyInfo = CurrentSettings.GetType().GetProperty(setting);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{setting}' does not exist on current settings.", nameof(setting));
            }
            return propertyInfo.GetValue(CurrentSettings, null);
        }

        if (ArchievedSettings.TryGetValue(archievedKey, out ObjectServerSettings archivedSettings))
        {
            var propertyInfo = archivedSettings.GetType().GetProperty(setting);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{setting}' does not exist on archived settings with key '{archievedKey}'.", nameof(setting));
            }
            return propertyInfo.GetValue(archivedSettings, null);
        }
        throw new KeyNotFoundException($"Archived key '{archievedKey}' not found.");
    }
    
    public void Set(string setting, object value, string archievedKey = null)
    {
        if (archievedKey == null)
        {
            var propertyInfo = CurrentSettings.GetType().GetProperty(setting);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{setting}' does not exist on current settings.", nameof(setting));
            }
            propertyInfo.SetValue(CurrentSettings, value);
            SaveSettings();
            return;
        }

        if (ArchievedSettings.TryGetValue(archievedKey, out ObjectServerSettings archivedSettings))
        {
            var propertyInfo = archivedSettings.GetType().GetProperty(setting);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{setting}' does not exist on archived settings with key '{archievedKey}'.", nameof(setting));
            }
            propertyInfo.SetValue(archivedSettings, value);
            SaveSettings();
            return;
        }
        throw new KeyNotFoundException($"Archived key '{archievedKey}' not found.");

    }
    
    public void SaveSettings()
    {
        var settingsData = new SettingsData
        {
            CurrentSettings = CurrentSettings,
            ArchievedSettings = ArchievedSettings
        };
        
        string json = JsonSerializer.Serialize(settingsData);
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)); 
        File.WriteAllText(_settingsPath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(_settingsPath))
        {
            var json = File.ReadAllText(_settingsPath);
            var settingsData = JsonSerializer.Deserialize<SettingsData>(json);
            
            CurrentSettings = settingsData?.CurrentSettings;
            ArchievedSettings = settingsData?.ArchievedSettings ?? new Dictionary<string, ObjectServerSettings>();
        } else
        {
            CurrentSettings = new ObjectServerSettings();
            ArchievedSettings = new Dictionary<string, ObjectServerSettings>();
            ArchievedSettings.Add("Default", CurrentSettings);
            SaveSettings();
        }
    }

}