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
    
    public bool AddProfile(string profileName, string profilePath, string serverPath)
    {
        var backupProfiles = new List<ServerProfile>(ServerProfileList);
        try
        {
           // Check for duplicates
            if (ServerProfileList.Exists(p => p.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase) || 
                                               p.ProfilePath.Equals(profilePath, StringComparison.OrdinalIgnoreCase)))
            {
                Program.DebugLog("Duplicate Profile Name or Path detected.",true);
                return false;
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

            MessageBox.Show("Profile Added.  Next list refresh will reflect the changes.");
            
            return true;    
        } 
        catch(Exception ex)
        {
            Program.DebugLog("Add Profile Error: " + ex.Message, true);
            ServerProfileList = backupProfiles;
            return false;    
        }
    }

    public bool EditProfile(ServerProfile profile, string profileName, string profilePath, string serverPath)
    {
        if (profile.ProfilePath != profilePath && profile.ProfileName != profilePath)
        {
            try
            {
                Directory.Move(profile.ProfilePath, profilePath);
            }
            catch (Exception ex)
            {
                Program.DebugLog("Edit Profile Error: " + ex.Message, true);
                return false;
            }

            profile.ProfilePath = profilePath;
            profile.ProfileName = profileName;
        }

        if (profile.ServerPath != serverPath) 
        {
            profile.ServerPath = serverPath;
        }
        
        SaveProfiles();
        
        MessageBox.Show("Profile Changed.  Next list refresh will reflect the changes.");
        
        return true;
    }
    
    public bool RemoveProfile(ServerProfile profileToRemove)
    {
        var backupProfiles = new List<ServerProfile>(ServerProfileList);
        
        try
        {
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
            Program.DebugLog("Remove Profile Error: " + ex.Message, true);
            ServerProfileList = backupProfiles;
            return false;    
        }
    }
    
    private void SaveProfiles()
    {
        var json = JsonSerializer.Serialize(ServerProfileList);
        Directory.CreateDirectory(Path.GetDirectoryName(_profilePath)!); 
        File.WriteAllText(_profilePath, json);
    }

    private void LoadProfiles()
    {
        if (File.Exists(_profilePath))
        {
            var json = File.ReadAllText(_profilePath);
            ServerProfileList = JsonSerializer.Deserialize<List<ServerProfile>>(json) ?? new List<ServerProfile>();
            foreach (var profile in ServerProfileList)
            {
                // Build Instance
                ServerInstance newInstance = new ServerInstance(profile);
            
                // Add to active instances
                Env._serverInstances.Add(profile, newInstance);
            }
        }
    }
}
