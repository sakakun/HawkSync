using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace ServerManager.Classes.Enviroment;

public class ServerProfile
{
    public string ProfileName { get; set; }
    public string ProfilePath { get; set; }
    public string ServerPath { get; set; }
}

public class ServerProfiles
{
    public List<ServerProfile> ServerProfileList { get; private set; } = new List<ServerProfile>();
    private readonly string _dataPath;
    private string _profilePath;
    
    ServerEnvironment Env;
    
    public ServerProfiles(ServerEnvironment serverEnvironment)
    {
        Env = serverEnvironment;
    
        // Profile Settings
        _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
        _profilePath = Path.Combine(_dataPath, "profiles.json");
        LoadProfiles();
    }
    
    public ServerInstance AddProfile(string profileName, string profilePath, string serverPath)
    {
        var backupProfiles = new List<ServerProfile>(ServerProfileList);
        try
        {
           // Check for duplicates
            if (ServerProfileList.Exists(p => p.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase) || 
                                               p.ProfilePath.Equals(profilePath, StringComparison.OrdinalIgnoreCase)))
            {
                Program.DebugLog("Duplicate Profile Name or Path detected.");
                return null; 
            }
            
            // Create Profile
            ServerProfile newProfile = new ServerProfile { ProfileName = profileName, ProfilePath = profilePath, ServerPath = serverPath };
            ServerProfileList.Add(newProfile);
            
            // Build Instance
            ServerInstance newInstance = new ServerInstance(newProfile);
            
            // Add to active instances
            Env._serverInstances.Add(newProfile, newInstance);
            
            // Save the Profile Files
            SaveProfiles();
            
            return newInstance;    
        } 
        catch(Exception ex)
        {
            Program.DebugLog("Failure (Adding & Saving Profile Record): " + ex.Message);
            ServerProfileList = backupProfiles;
            return null;    
        }
    }
    
    public bool RemoveProfile(string profileName)
    {
        var backupProfiles = new List<ServerProfile>(ServerProfileList);
        
        try
        {
            var profileToRemove = ServerProfileList.Find(p => p.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase));
            if (profileToRemove != null)
            {
                // Remove Server Instance if it Exists
                if (Env._serverInstances.TryGetValue(profileToRemove, out ServerInstance instanceToRemove))
                {
                    if (instanceToRemove.destoryInstance())
                    {
                        Env._serverInstances.Remove(profileToRemove);    
                        
                        // Remove Profile Directories if it Exists
                        if (Directory.Exists(profileToRemove.ProfilePath))
                        {
                            Directory.Delete(profileToRemove.ProfilePath, true);
                        }
                        
                        // Remove Profile Record
                        ServerProfileList.Remove(profileToRemove);
                    }
                }
                
                // Save Profile List
                SaveProfiles();
                return true;    
            }
            return false;
        } 
        catch(Exception ex)
        {
            Program.DebugLog("Failure (Removing Profile Record): " + ex.Message);
            ServerProfileList = backupProfiles;
            return false;    
        }
    }
    
    private void SaveProfiles()
    {
        var json = JsonSerializer.Serialize(ServerProfileList);
        Directory.CreateDirectory(Path.GetDirectoryName(_profilePath)); 
        File.WriteAllText(_profilePath, json);
    }

    private void LoadProfiles()
    {
        if (File.Exists(_profilePath))
        {
            var json = File.ReadAllText(_profilePath);
            ServerProfileList = JsonSerializer.Deserialize<List<ServerProfile>>(json) ?? new List<ServerProfile>();
        }
    }
}
