using System.Collections.Generic;

namespace BHD_ServerManager.Classes.ObjectClasses
{
    public class objectGameTypes
    {
        public int DatabaseId { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string ShortName { get; set; } = string.Empty;
        public int Bitmap { get; set; }
        public int BitmapBytes { get; set; }

        // Static list of all game types
        public static readonly List<objectGameTypes> All = new List<objectGameTypes>
        {
            new objectGameTypes { DatabaseId = 0, Name = "Deathmatch",           ShortName = "DM",    Bitmap = 2   , BitmapBytes = 512 },
            new objectGameTypes { DatabaseId = 1, Name = "Team Deathmatch",      ShortName = "TDM",   Bitmap = 32  , BitmapBytes = 8192 },
            new objectGameTypes { DatabaseId = 2, Name = "Cooperative",          ShortName = "CP",    Bitmap = 1   , BitmapBytes = 256 },
            new objectGameTypes { DatabaseId = 3, Name = "Team King of the Hill",ShortName = "TKOTH", Bitmap = 64  , BitmapBytes = 16384 },
            new objectGameTypes { DatabaseId = 4, Name = "King of the Hill",     ShortName = "KOTH",  Bitmap = 4   , BitmapBytes = 1024 },
            new objectGameTypes { DatabaseId = 5, Name = "Search and Destroy",   ShortName = "SD",    Bitmap = 128 , BitmapBytes = 32768 },
            new objectGameTypes { DatabaseId = 6, Name = "Attack and Defend",    ShortName = "AD",    Bitmap = 0   , BitmapBytes = 128 },
            new objectGameTypes { DatabaseId = 7, Name = "Capture the Flag",     ShortName = "CTF",   Bitmap = 16  , BitmapBytes = 4068 },
            new objectGameTypes { DatabaseId = 8, Name = "Flagball",             ShortName = "FB",    Bitmap = 8   , BitmapBytes = 2048 }
        };

        public static string GetShortName(int databaseId)
        {
            foreach (var gameType in All)
            {
                if (gameType.DatabaseId == databaseId)
                {
                    return gameType.ShortName;
                }
            }
            return string.Empty; // Return empty string if not found
        }

    }
}