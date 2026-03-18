using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using BHD_ServerManager.Classes.SupportClasses;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Storage;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabPlayers;

namespace BHD_ServerManager.Classes.GameManagement
{
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static partial class ServerMemory {

		// Function: ReadMapCycleCounter, Reads the Map Cycle Counter from Memory and stores it in the gameInfoMapCycleIndex variable.
		public static void ReadMapCycleCounter() => theInstance.gameInfoMapCycleIndex = ReadInt(baseAddr + 0x5ED644);

		// Function: UpdateMapCycleData, gets current and next map data from the server memory and updates the mapInstance accordingly
		public static void UpdateMapCycleData()
        {
            List<MapObject> currentPlaylist = mapInstance.Playlists[mapInstance.ActivePlaylist];

            // Guard: nothing to do with an empty playlist
            if (currentPlaylist.Count == 0) return;

            // Read Game Server Map Cycle Index
            int currentMapIndex = ReadInt(GetMapCycleServerAddress() + 0xC);

            // Clamp to valid range — stale memory can return an out-of-bounds index
            if (currentMapIndex < 0 || currentMapIndex >= currentPlaylist.Count)
                currentMapIndex = 0;

			// Update the "Current Map Index" in the mapInstance based on the server memory value
            mapInstance.CurrentMapIndex = currentMapIndex;

            // Update the "Currently Playing Map"
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP) { 
                mapInstance.ActualPlayingMapIndex = currentMapIndex; 
                mapInstance.CurrentGameType = currentPlaylist[currentMapIndex].MapType;
                mapInstance.CurrentMapName = currentPlaylist[currentMapIndex].MapName;
                mapInstance.CurrentMapFile = currentPlaylist[currentMapIndex].MapFile;
                mapInstance.IsCurrentMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.CurrentMapFile);
			}

            // Calculate the next map index (wraps to 0 at end of playlist)
            int nextMapIndex = (currentMapIndex + 1 < currentPlaylist.Count && currentPlaylist[currentMapIndex + 1] != null)
                ? currentMapIndex + 1
                : 0;

			// Update the "Next Playing Map" information in the mapInstance based on the server memory value
            mapInstance.NextMapGameType = currentPlaylist[nextMapIndex].MapType;
            mapInstance.NextMapName = currentPlaylist[nextMapIndex].MapName;
            mapInstance.NextMapFile = currentPlaylist[nextMapIndex].MapFile;
            mapInstance.IsNextMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.NextMapFile);

		}

		// Function: UpdateMapListCount, Updates the Map List Count in Memory to match the number of maps in the active playlist.
		public static void UpdateMapListCount()
        {

            int MapListMoveGarbageAddress = baseAddr + 0x5EA7B8;
            WriteInt(MapListMoveGarbageAddress, ReadInt(MapListMoveGarbageAddress) + 0x350);

            int mapListNumberOfMaps = GetMapCycleServerAddress() + 0x4;
            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            WriteInt(mapListNumberOfMaps, count);
            WriteInt(mapListNumberOfMaps + 0x4, count);

        }

        // Function: UpdateMapCycle1
        // Clears the current map cycle and fills it with empty maps
        public static void UpdateMapCycle1()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #88");
            }

            int mapCycleServerAddress = GetMapCycleServerAddress();
            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            WriteInt(mapCycleServerAddress + 0x4, count);
            WriteInt(mapCycleServerAddress + 0xC, count);
            int mapCycleList = ReadInt(mapCycleServerAddress);

            foreach (MapObject entry in mapInstance.Playlists[mapInstance.ActivePlaylist])
            {
                int mapFileIndexLocation = mapCycleList;

                byte[] mapFileBytes = new byte[0x20]; // 32 bytes, all initialized to 0
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);

                WriteInt(mapFileIndexLocation, 0);
                mapCycleList += 0x24;
            }

        }
        // Function: UpdateMapCycle2
        // Actually updates the memory of the game server with the current map list
        public static void UpdateMapCycle2()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #89");
            }

            int mapCycleClientAddress = ReadInt(baseAddr + 0x000DC6D8);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms, Encoding.Default);

            // Helper to write a fixed-length string with null padding
            void WriteFixedString(string value, int length)
            {
                var bytes = Encoding.Default.GetBytes(value);
                bw.Write(bytes, 0, Math.Min(bytes.Length, length));
                for (int i = bytes.Length; i < length; i++)
                    bw.Write((byte)0x00);
            }

            // Write the first map
            var firstMap = mapInstance.Playlists[mapInstance.ActivePlaylist][0];
            WriteFixedString(firstMap.MapFile!, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            string mapName = firstMap.MapName!;
            if (mapName.Length > 31)
                mapName = mapName.Substring(0, 31);
            WriteFixedString(mapName, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(theInstance.gameScoreKills));
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(firstMap.ModType==9 ? 1 : 0)); // Custom map flag
			bw.Write(new byte[24]); // adjust this padding as needed

            // Write additional maps
            for (int i = 1; i < mapInstance.Playlists[mapInstance.ActivePlaylist].Count; i++)
            {
                var map = mapInstance.Playlists[mapInstance.ActivePlaylist][i];
                WriteFixedString(map.MapFile!, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                string name = map.MapName!;
                if (name.Length > 31)
                    name = name.Substring(0, 31);
                WriteFixedString(name, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(theInstance.gameScoreKills));
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(map.ModType==9 ? 1 : 0));  // Custom map flag
                bw.Write(new byte[28]); // adjust this padding as needed
            }

            // Write to memory
            byte[] mapCycleClientBytes = ms.ToArray();
            int mapCycleClientWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleClientAddress, mapCycleClientBytes, mapCycleClientBytes.Length, ref mapCycleClientWritten);

            UpdateSecondaryMapList();
        }

        // Function: UpdateSecondaryMapList
        // Updates the secondary map list in the server memory
        public static void UpdateSecondaryMapList()
        {

            int mapCycleServerAddress = GetMapCycleServerAddress();
            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            WriteInt(mapCycleServerAddress + 0x4, count);
            WriteInt(mapCycleServerAddress + 0xC, count);
            int mapCycleList = ReadInt(mapCycleServerAddress);


            for (int i = 0; i < mapInstance.Playlists[mapInstance.ActivePlaylist].Count; i++)
            {
                int mapFileIndexLocation = mapCycleList;
                byte[] mapFileBytes = new byte[0x20]; // 32 bytes
                byte[] nameBytes = Encoding.ASCII.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist][i].MapFile!);
                Array.Copy(nameBytes, mapFileBytes, Math.Min(nameBytes.Length, mapFileBytes.Length));
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);
                mapFileIndexLocation += 0x20;

                WriteInt(mapFileIndexLocation, mapInstance.Playlists[mapInstance.ActivePlaylist][i].ModType == 9 ? 1 : 0);
                mapCycleList += 0x24;
            }

        }

		// Function: SetupNextMap, Sets the Map Type for the next map in memory and updates the 4-team mode if necessary.
		public static void SetupNextMap()
        {
            HashSet<int> s_fourTeamGameTypes = [1, 3, 8];

            // Change the MapType for the next map
            WriteInt(baseAddr + 0x5F21A4, mapInstance.NextMapGameType);

            // Determine if we need to enable/disable 4-team mode
            bool shouldEnable4Teams = theInstance.gameEnableFourTeams && mapInstance.IsNextMap4Team && s_fourTeamGameTypes.Contains(mapInstance.NextMapGameType);

			// Update the 4-team state in game memory
			UpdateNumTeams(shouldEnable4Teams);

			// Deal with the Players
            bool isCurrentMap4Team  = theInstance.gameEnableFourTeams && mapInstance.IsCurrentMap4Team && s_fourTeamGameTypes.Contains(mapInstance.CurrentGameType);

			// If we're switching between 4-team and non-4-team modes, we need to change the game mode in memory to trigger the appropriate team balancing logic
			ChangeTeamGameMode();

            // FILTER INVALID TEAM SWITCHES BEFORE APPLYING
            ValidateAndNormalizeTeamSwitches(shouldEnable4Teams);

			// Apply the team switches to memory
			UpdatePlayerTeam();

        }

		// Function: UpdatePlayMapNext, Updates the next map index in memory to match the current map index in the mapInstance.
		public static void UpdateNextMap(int NextMapIndex)
        {
            var playlist = mapInstance.Playlists[mapInstance.ActivePlaylist];

            if (NextMapIndex == 0)
                NextMapIndex = playlist.Count;
            else if (playlist[NextMapIndex - 1] != null)
                NextMapIndex--;

            WriteInt(GetMapCycleServerAddress() + 0xC, NextMapIndex);
        }

        // Function: WriteMemoryScoreMap
        public static void ScoreMap()
        {
            if (theInstance.instanceStatus is InstanceStatus.SCORING or InstanceStatus.LOADINGMAP)
                return;           

            WriteInt(baseAddr + 0x5F3740, 10);
        }

        // Returns the dereferenced map-cycle server struct pointer
        private static int GetMapCycleServerAddress() => ReadInt(baseAddr + 0x005ED5F8);

	}
}
