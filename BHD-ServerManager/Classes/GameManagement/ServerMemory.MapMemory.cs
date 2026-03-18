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

		// Function: UpdateMapCycleData, gets current and next map data from the server memory and updates the mapInstance accordingly
		public static void UpdateMapCycleData()
        {
            List<MapObject> currentPlaylist = mapInstance.Playlists[mapInstance.ActivePlaylist];

            // Guard: nothing to do with an empty playlist
            if (currentPlaylist.Count == 0) return;

            // Read Game Server Map Cycle Index
            int currentMapIndex = ReadInt(0x00A348D0);

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
                mapInstance.IsCurrentMapHideAndSeek = mapInstanceManager.IsHideSeekMap(mapInstance.CurrentMapFile);
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

            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #87");
            }

            const int mapCycleCountAddress = 0x00A3483C;
            const int mapCycleCountMirrorAddress = 0x00A03394;
            const int mapCycleSelectedIndexAddress = 0x00A348D0;

            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            int selectedIndex = ReadInt(mapCycleSelectedIndexAddress);

            if (selectedIndex < 0 || selectedIndex >= count)
                selectedIndex = 0;

            WriteInt(mapCycleCountAddress, count);
            WriteInt(mapCycleCountMirrorAddress, count);
            WriteInt(mapCycleSelectedIndexAddress, selectedIndex);

        }

        // Function: UpdateMapCycle1
        // Clears the current map cycle and fills it with empty maps
        public static void UpdateMapCycle1()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #88");
            }

            const int mapCycleTableAddress = 0x00A3F688;
            const int mapCycleTableSize = 0x39800;
            const int mapCycleCountAddress = 0x00A3483C;
            const int mapCycleCountMirrorAddress = 0x00A03394;
            const int mapCycleSelectedIndexAddress = 0x00A348D0;

            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            int selectedIndex = ReadInt(mapCycleSelectedIndexAddress);
            byte[] emptyTable = new byte[mapCycleTableSize];
            int emptyTableBytesWritten = 0;

            WriteProcessMemory((int)processHandle, mapCycleTableAddress, emptyTable, emptyTable.Length, ref emptyTableBytesWritten);

            if (selectedIndex < 0 || selectedIndex >= count)
                selectedIndex = 0;

            WriteInt(mapCycleCountAddress, count);
            WriteInt(mapCycleCountMirrorAddress, count);
            WriteInt(mapCycleSelectedIndexAddress, selectedIndex);

        }
        // Function: UpdateMapCycle2
        // Actually updates the memory of the game server with the current map list
        public static void UpdateMapCycle2()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #89");
            }

            const int mapCycleTableAddress = 0x00A3F688;
            const int mapCycleEntryStride = 0x730;
            const int mapCycleFileOffset = 0x000;
            const int mapCycleFileSize = 0x104;
            const int mapCycleNameOffset = 0x20F;
            const int mapCycleNameSize = 0x0FF;
            const int mapCycleModTypeOffset = 0x710;
            const int mapCycleCustomFlagOffset = 0x714;

            byte[] emptyEntry = new byte[mapCycleEntryStride];
            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;

            for (int i = 0; i < count; i++)
            {
                MapObject map = mapInstance.Playlists[mapInstance.ActivePlaylist][i];
                int entryBase = mapCycleTableAddress + (i * mapCycleEntryStride);
                int emptyEntryBytesWritten = 0;

                WriteProcessMemory((int)processHandle, entryBase, emptyEntry, emptyEntry.Length, ref emptyEntryBytesWritten);

                int customFlag = map.ModType == 9 ? 1 : 0;
                WriteFixedString(entryBase + mapCycleFileOffset, map.MapFile, mapCycleFileSize);
                WriteFixedString(entryBase + mapCycleNameOffset, map.MapName, mapCycleNameSize);
                WriteInt(entryBase + mapCycleModTypeOffset, customFlag);
                WriteInt(entryBase + mapCycleCustomFlagOffset, customFlag);
            }

            UpdateSecondaryMapList();
        }

        // Function: UpdateSecondaryMapList
        // Refreshes the derived 0x24 secondary list cache used by some live paths.
        public static void UpdateSecondaryMapList()
        {
            const int secondaryEntryStride = 0x24;
            const int secondaryEntryNameSize = 0x20;
            const int secondaryEntryFlagOffset = 0x20;
            const int secondaryListCountOffset = 0x04;
            const int secondaryListCapacityOffset = 0x08;
            const int secondaryListSelectedIndexOffset = 0x0C;
            const int mapCycleSelectedIndexAddress = 0x00A348D0;

            int mapCycleServerAddress = GetMapCycleServerAddress();
            if (mapCycleServerAddress == 0)
                return;

            int mapCycleList = ReadInt(mapCycleServerAddress);
            int capacity = ReadInt(mapCycleServerAddress + secondaryListCapacityOffset);
            int count = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;

            if (mapCycleList == 0 || capacity <= 0)
                return;

            if (count > capacity)
            {
                throw new Exception($"Secondary map list capacity ({capacity}) is smaller than the active playlist count ({count}). #90");
            }

            int selectedIndex = ReadInt(mapCycleSelectedIndexAddress);
            if (count <= 0)
                selectedIndex = -1;
            else if (selectedIndex < 0 || selectedIndex >= count)
                selectedIndex = 0;

            byte[] emptyList = new byte[capacity * secondaryEntryStride];
            int emptyListBytesWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleList, emptyList, emptyList.Length, ref emptyListBytesWritten);

            WriteInt(mapCycleServerAddress + secondaryListCountOffset, count);
            WriteInt(mapCycleServerAddress + secondaryListSelectedIndexOffset, selectedIndex);

            for (int i = 0; i < count; i++)
            {
                MapObject map = mapInstance.Playlists[mapInstance.ActivePlaylist][i];
                int entryBase = mapCycleList + (i * secondaryEntryStride);
                int customFlag = map.ModType == 9 ? 1 : 0;

                WriteFixedString(entryBase, map.MapFile, secondaryEntryNameSize);
                WriteInt(entryBase + secondaryEntryFlagOffset, customFlag);
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
            if (playlist.Count == 0)
                return;

            const int mapCycleSelectedIndexAddress = 0x00A348D0;
            int selectedIndex;

            if (NextMapIndex <= 0)
                selectedIndex = 0;
            else
                selectedIndex = NextMapIndex - 1;

            if (selectedIndex >= playlist.Count)
                selectedIndex = playlist.Count - 1;

            WriteInt(mapCycleSelectedIndexAddress, selectedIndex);
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
