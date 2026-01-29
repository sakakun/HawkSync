using BHD_ServerManager.Classes.ObjectClasses;
using System.Collections.Generic;

namespace BHD_ServerManager.Classes.Instances;

/// <summary>
/// Contains all player-related data that changes frequently during gameplay
/// </summary>
public class playerInstance
{
    // Active Players
    public Dictionary<int, playerObject> PlayerList { get; set; } = new();
    
    // Team Management
    public List<playerTeamObject> PlayerChangeTeamList { get; set; } = new();
    public List<playerTeamObject> PlayerPreviousTeamList { get; set; } = new();
    
}