using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BHD_ServerManager.Classes.ObjectClasses
{

    public class mapFileInfo
    {
        /// <summary>
        /// Map Identifier (ID)
        /// </summary>
        public int          MapID { get; set; }
        /// <summary>
        /// Map File Name
        /// </summary>
        public string       MapFile { get; set; } = string.Empty;
        /// <summary>
        /// Map Name
        /// </summary>
        public string       MapName { get; set; } = string.Empty;
        /// <summary>
        /// Custom Map Flag - Indicates if the map is a custom map.
        /// </summary>
        public int          ModType { get; set; }
        /// <summary>
        /// Map Type (e.g., "Deathmatch", "Team Deathmatch") - Seen in the UI, multiple types can be associated with a map.
        /// </summary>\
        /// 
        public int          MapType { get; set; }

    }
}