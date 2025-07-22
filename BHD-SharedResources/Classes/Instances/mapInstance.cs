using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace BHD_SharedResources.Classes.Instances
{
    public class mapInstance
    {
        // Available Maps to Play
        public List<mapFileInfo> availableMaps          { get; set; } = new List<mapFileInfo>();
        // Used to Load the Custom Maps
        public List<mapFileInfo> customMaps             { get; set; } = new List<mapFileInfo>();
        // Current Map Playlist
        public List<mapFileInfo> currentMapPlaylist     { get; set; } = new List<mapFileInfo>();
        // Previous Map Playlist (for undo functionality)
        public List<mapFileInfo> previousMapPlaylist    { get; set; } = new List<mapFileInfo>();
        // Remote Map Playlist (for remote control)
        [JsonIgnore]
        public List<mapFileInfo> remoteMapPlaylist      { get; set; } = new List<mapFileInfo>();

    }

    public class mapFileInfo
    {
        public int          MapID           { get; set; }
        public string       MapFile         { get; set; } = string.Empty;
        public string       MapName         { get; set; } = string.Empty;
        public string       GameType        { get; set; } = string.Empty;
        public bool         CustomMap       { get; set; }
        public List<int>    GameTypes       { get; set; } = new List<int>();
        public int          ProfileServerType { get; set; }
        public List<int>    gameTypeBits    { get; set; } = new List<int>();
    }
}