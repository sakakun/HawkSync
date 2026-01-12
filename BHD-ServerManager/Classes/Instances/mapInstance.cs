using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BHD_ServerManager.Classes.Instances
{
    public class mapInstance
    {
        // Available Maps to Play
        public List<mapFileInfo> availableMaps { get; set; } = new List<mapFileInfo>();
        // Used to Load the Custom Maps
        public List<mapFileInfo> customMaps { get; set; } = new List<mapFileInfo>();
        // Current Map Playlist
        public List<mapFileInfo> currentMapPlaylist { get; set; } = new List<mapFileInfo>();
        // Previous Map Playlist (for undo functionality)
        public List<mapFileInfo> previousMapPlaylist { get; set; } = new List<mapFileInfo>();
        // Remote Map Playlist (for remote control)
        [JsonIgnore]
        public List<mapFileInfo> remoteMapPlaylist { get; set; } = new List<mapFileInfo>();

    }

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