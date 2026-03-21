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

namespace BHD_ServerManager.Classes.GameManagement.Memory
{
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static partial class ServerMemory
    {

		// Constants for functions called in this file.
        // Function: UpdatePlayMapNext
        public static void UpdateNextMap(int NextMapIndex)
        {

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int MapCycleIndex = BitConverter.ToInt32(ServerMapCyclePtr, 0) + 0xC;

            if (NextMapIndex - 1 == -1)
            {
                NextMapIndex = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            }
            else if (mapInstance.Playlists[mapInstance.ActivePlaylist][NextMapIndex - 1] != null)
            {
                NextMapIndex--;
            }

            byte[] newMapIndexBytes = BitConverter.GetBytes(NextMapIndex);
            int newMapIndexBytesWritten = 0;
            WriteProcessMemory((int)processHandle, MapCycleIndex, newMapIndexBytes, newMapIndexBytes.Length, ref newMapIndexBytesWritten);


        }
        // Function: UpdateMapListCount, Set the Map Count for the Game Server, for looping purposes.
        public static void UpdateMapListCount()
        {
            int MapListMoveGarbageAddress = baseAddr + 0x5EA7B8;
            byte[] CurrentAddressBytes = new byte[4];
            int CurrentAddressRead = 0;
            ReadProcessMemory((int)processHandle, MapListMoveGarbageAddress, CurrentAddressBytes, CurrentAddressBytes.Length, ref CurrentAddressRead);
            int CurrentAddress = BitConverter.ToInt32(CurrentAddressBytes, 0);
            int NewAddress = CurrentAddress + 0x350;

            byte[] NewAddressBytes = BitConverter.GetBytes(NewAddress);
            int NewAddressWritten = 0;
            WriteProcessMemory((int)processHandle, MapListMoveGarbageAddress, NewAddressBytes, NewAddressBytes.Length, ref NewAddressWritten);

            int mapListLocationPtr = baseAddr + 0x005ED5F8;
            byte[] mapListLocationPtrBytes = new byte[4];
            int mapListLocationBytesPtrRead = 0;
            ReadProcessMemory((int)processHandle, mapListLocationPtr, mapListLocationPtrBytes, mapListLocationPtrBytes.Length, ref mapListLocationBytesPtrRead);

            int mapListNumberOfMaps = BitConverter.ToInt32(mapListLocationPtrBytes, 0) + 0x4;
            byte[] numberOfMaps = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int numberofMapsWritten = 0;
            WriteProcessMemory((int)processHandle, mapListNumberOfMaps, numberOfMaps, numberOfMaps.Length, ref numberofMapsWritten);

            mapListNumberOfMaps += 0x4;
            byte[] TotalnumberOfMaps = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int TotalnumberofMapsWritten = 0;
            WriteProcessMemory((int)processHandle, mapListNumberOfMaps, TotalnumberOfMaps, TotalnumberOfMaps.Length, ref TotalnumberofMapsWritten);
        }
        // Function: UpdateMapCycle1
        // Clears the current map cycle and fills it with empty maps
        public static void UpdateMapCycle1()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #88");
            }

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int mapCycleServerAddress = BitConverter.ToInt32(ServerMapCyclePtr, 0);

            int mapCycleTotalAddress = mapCycleServerAddress + 0x4;
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);

            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);

            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);

            foreach (MapObject entry in mapInstance.Playlists[0])
            {
                int mapFileIndexLocation = mapCycleList;

                byte[] mapFileBytes = new byte[0x20]; // 32 bytes, all initialized to 0
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);

                byte[] customMapFlag = BitConverter.GetBytes(Convert.ToInt32(false));
                int customMapFlagWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, customMapFlag, customMapFlag.Length, ref customMapFlagWritten);
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



            byte[] Pointer1Bytes = new byte[4];
            int Pointer1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DC6D8, Pointer1Bytes, Pointer1Bytes.Length, ref Pointer1Read);
            int mapCycleClientAddress = BitConverter.ToInt32(Pointer1Bytes, 0);

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

            bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
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

                bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
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

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int mapCycleServerAddress = BitConverter.ToInt32(ServerMapCyclePtr, 0);

            int mapCycleTotalAddress = mapCycleServerAddress + 0x4;
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);


            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);


            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);


            for (int i = 0; i < mapInstance.Playlists[mapInstance.ActivePlaylist].Count; i++)
            {
                int mapFileIndexLocation = mapCycleList;
                byte[] mapFileBytes = new byte[0x20]; // 32 bytes
                byte[] nameBytes = Encoding.ASCII.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist][i].MapFile!);
                Array.Copy(nameBytes, mapFileBytes, Math.Min(nameBytes.Length, mapFileBytes.Length));
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);
                mapFileIndexLocation += 0x20;

                byte[] customMapFlag = BitConverter.GetBytes(Convert.ToInt32(mapInstance.Playlists[mapInstance.ActivePlaylist][i].ModType==9?1:0));
                int customMapFlagWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, customMapFlag, customMapFlag.Length, ref customMapFlagWritten);
                mapCycleList += 0x24;
            }

        }   
		// Function: GetMapData, gets current and next map data from the server memory and updates the mapInstance accordingly
		public static void GetMapData()
        {

            // This will grab the current map index.
            var startingPtr = baseAddr + 0x005ED5F8;

            byte[] read_Ptr2Bytes = new byte[4];
            int read_Ptr2BytesRead = 0;
            ReadProcessMemory((int)processHandle, startingPtr, read_Ptr2Bytes, read_Ptr2Bytes.Length, ref read_Ptr2BytesRead);
            int Ptr2 = BitConverter.ToInt32(read_Ptr2Bytes, 0) + 0xC;

            byte[] CurrentMapIndexBytes = new byte[4];
            int CurrentMapIndexBytesRead = 0;
            ReadProcessMemory((int)processHandle, Ptr2, CurrentMapIndexBytes, CurrentMapIndexBytes.Length, ref CurrentMapIndexBytesRead);
            int currentMapIndex = BitConverter.ToInt32(CurrentMapIndexBytes, 0);

            AppDebug.Log("ServerMemory", "Number of Maps: " + mapInstance.Playlists[mapInstance.ActivePlaylist].Count + " Pre-Check Current Map Index: " + currentMapIndex);

            if (currentMapIndex + 1 >= mapInstance.Playlists[mapInstance.ActivePlaylist].Count || mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex + 1] == null)
            {
                currentMapIndex = 0;
            }
            else
            {
                currentMapIndex++;
            }

            AppDebug.Log("ServerMemory", "Number of Maps: " + mapInstance.Playlists[mapInstance.ActivePlaylist].Count + " Current Map Index: " + currentMapIndex);
            int currentMapType = CommonCore.instanceMaps!.CurrentGameType;
            int nextMapType = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapType;

            AppDebug.Log("ServerMemory", "Current Map Type: " + mapInstance.CurrentMapName + " " + CommonCore.instanceMaps!.CurrentGameType + " " + currentMapType);
            AppDebug.Log("ServerMemory", "Next Map Type: " + mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapName + " " + mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapType + " - " + nextMapType);

            mapInstance.NextMapGameType = nextMapType;
            mapInstance.NextMapName = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapName!;
            mapInstance.NextMapFile = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapFile!;
            mapInstance.IsNextMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.NextMapFile);

            mapInstance.CurrentMapFile = mapInstance.Playlists[mapInstance.ActivePlaylist][mapInstance.ActualPlayingMapIndex].MapFile;
            mapInstance.IsCurrentMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.CurrentMapFile);

		}
        public static void SetNextMapType()
        {
            AppDebug.Log("SetNextMapType", "Updated the Map Type for the Next Map");

            try
            {
                // Change the MapType for the next map
                var CurrentGameTypeAddr = baseAddr + 0x5F21A4;
                byte[] nextMaptypeBytes = BitConverter.GetBytes(mapInstance.NextMapGameType);
                int nextMaptypeBytesWrite = 0;
                WriteProcessMemory((int)processHandle, CurrentGameTypeAddr, nextMaptypeBytes, nextMaptypeBytes.Length, ref nextMaptypeBytesWrite);

                // Determine if we need to enable/disable 4-team mode
                bool shouldEnable4Teams = thisInstance.gameEnableFourTeams && 
                                         mapInstance.IsNextMap4Team &&
                                         (mapInstance.NextMapGameType == 1 || mapInstance.NextMapGameType == 3 || mapInstance.NextMapGameType == 8); // TDM or TKOTH or FBL

				AppDebug.Log("SetNextMapType", $"Next Map: {mapInstance.NextMapName} | Next Map Type: {mapInstance.NextMapGameType} | Is 4-Team Map: {mapInstance.IsNextMap4Team} | Should Enable 4-Team Mode: {shouldEnable4Teams}");

				// Update the 4-team state in game memory
				UpdateNumTeams(shouldEnable4Teams);
                
                AppDebug.Log("SetNextMapType", $"4-Team Mode: {(shouldEnable4Teams ? "Enabled" : "Disabled")} for next map");

                AppDebug.Log("SetNextMapType", $"4-Team Mode Enabled? {ServerMemory.ReadNumTeams().ToString()}");

				// Deal with the Players
				bool isCurrentMap4Team = thisInstance.gameEnableFourTeams && 
                                        mapInstance.IsCurrentMap4Team &&
                                        (CommonCore.instanceMaps!.CurrentGameType == 1 || CommonCore.instanceMaps!.CurrentGameType == 3 || CommonCore.instanceMaps!.CurrentGameType == 8);
                
                theInstanceManager.changeTeamGameMode(
                    CommonCore.instanceMaps!.CurrentGameType, 
                    mapInstance.NextMapGameType,
                    isCurrentMap4Team,
                    shouldEnable4Teams
                );

                // FILTER INVALID TEAM SWITCHES BEFORE APPLYING
                ValidateAndNormalizeTeamSwitches(shouldEnable4Teams);

                UpdatePlayerTeam();

            }
            catch (Exception ex)
            {
                AppDebug.Log("ServerMemory", "Something went wrong with ScoringProcessHandler: " + ex);
            }
        }
        // Update the Game Score for the next map
        public static void UpdateGameScores()
        {

            AppDebug.Log("UpdateGameScores", "Updated Game Scores");

            // This changes the score needed to win on the next map played.
            int nextGameScore = 0;
            var startingPtr1 = 0;
            var startingPtr2 = 0;

            switch (mapInstance.NextMapGameType)
            {
                // KOTH/TKOTH
                case 3:
                case 4:
                    startingPtr1 = baseAddr + 0x5F21B8;
                    startingPtr2 = baseAddr + 0x6344B4;
                    nextGameScore = thisInstance.gameScoreZoneTime;
                    break;
                // flag ball
                case 8:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = thisInstance.gameScoreFlags;
                    break;
                // all other game types...
                default:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = thisInstance.gameScoreKills;
                    break;
            }
            byte[] nextGameScoreBytes = BitConverter.GetBytes(nextGameScore);
            int nextGameScoreWritten1 = 0;
            int nextGameScoreWritten2 = 0;
            WriteProcessMemory((int)processHandle, startingPtr1, nextGameScoreBytes, nextGameScoreBytes.Length, ref nextGameScoreWritten1);
            WriteProcessMemory((int)processHandle, startingPtr2, nextGameScoreBytes, nextGameScoreBytes.Length, ref nextGameScoreWritten2);

            AppDebug.Log("UpdateGameScores", "Game Score Updated: " + nextGameScore);

        }

    }
}
