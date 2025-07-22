using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.ObjectClasses
{
    public class objectGameTypes
    {
        public int      DatabaseId      { get; set; }
        public required string  Name            { get; set; } = string.Empty;
        public required string  ShortName       { get; set; } = string.Empty;
        public int      Bitmap          { get; set; }

        // Static list of all game types
        public static readonly List<objectGameTypes> All = new List<objectGameTypes>
        {
            new objectGameTypes { DatabaseId = 0, Name = "Deathmatch",           ShortName = "DM",    Bitmap = 2   },
            new objectGameTypes { DatabaseId = 1, Name = "Team Deathmatch",      ShortName = "TDM",   Bitmap = 32  },
            new objectGameTypes { DatabaseId = 2, Name = "Cooperative",          ShortName = "CP",    Bitmap = 1   },
            new objectGameTypes { DatabaseId = 3, Name = "Team King of the Hill",ShortName = "TKOTH", Bitmap = 64  },
            new objectGameTypes { DatabaseId = 4, Name = "King of the Hill",     ShortName = "KOTH",  Bitmap = 4   },
            new objectGameTypes { DatabaseId = 5, Name = "Search and Destroy",   ShortName = "SD",    Bitmap = 128 },
            new objectGameTypes { DatabaseId = 6, Name = "Attack and Defend",    ShortName = "AD",    Bitmap = 0   },
            new objectGameTypes { DatabaseId = 7, Name = "Capture the Flag",     ShortName = "CTF",   Bitmap = 16  },
            new objectGameTypes { DatabaseId = 8, Name = "Flagball",             ShortName = "FB",    Bitmap = 8   }
        };
    }
}