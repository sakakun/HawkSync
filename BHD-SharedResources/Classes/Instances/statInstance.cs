using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHD_SharedResources.Classes.ObjectClasses;

namespace BHD_SharedResources.Classes.Instances
{
    public class statInstance
    {

        public DateTime         lastPlayerStatsUpdate { get; set; } = DateTime.MinValue;
        public DateTime         lastPlayerStatsReport { get; set; } = DateTime.MinValue;
        public Dictionary<string, PlayerStatObject> playerStatsList { get; set; } = new Dictionary<string, PlayerStatObject>();

    }

    public class PlayerStatObject
    {
        public required playerObject PlayerStatsCurrent { get; set; }
        public required playerObject PlayerStatsPrevious { get; set; }
        public required List<WeaponStatObject> PlayerWeaponStats { get; set; }
    }

    public class WeaponStatObject
    {
        public int WeaponID { get; set; }
        public int TimeUsed { get; set; }
        public int Kills { get; set; }
        public int Shots { get; set; }
    }
}