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
        // Function: UpdateMapCycleCounter
        public static void ReadMapCycleCounter()
        {

            byte[] currentMapCycleCountBytes = new byte[4];
            int currentMapCycleCountRead = 0;

            int mapCycleCounterPtr = baseAddr + 0x5ED644;

            ReadProcessMemory((int)processHandle, mapCycleCounterPtr, currentMapCycleCountBytes, currentMapCycleCountBytes.Length, ref currentMapCycleCountRead);

            int currentMapCycleCount = BitConverter.ToInt32(currentMapCycleCountBytes, 0);

            thisInstance.gameInfoMapCycleIndex = currentMapCycleCount;


        }
        // Function: Update Start Delay Timer
        public static void ReadStartDelayTimer()
        {
            byte[] currentStartDelayCountBytes = new byte[4];
            int currentStartDelayCountRead = 0;

            int StartDelayCounterPtr = baseAddr + 0x5DAE04;

            ReadProcessMemory((int)processHandle, StartDelayCounterPtr, currentStartDelayCountBytes, currentStartDelayCountBytes.Length, ref currentStartDelayCountRead);

            int currentStartDelayCount = BitConverter.ToInt32(currentStartDelayCountBytes, 0);

            thisInstance.gameInfoStartDelayTimer = currentStartDelayCount;

        }
        public static void ReadMemoryServerStatus()
        {

            var startingPointer = baseAddr + 0x00098334;
            byte[] startingPointerBuffer = new byte[4];
            int startingPointerReadBytes = 0;
            ReadProcessMemory((int)processHandle, startingPointer, startingPointerBuffer, startingPointerBuffer.Length, ref startingPointerReadBytes);

            int statusLocationPointer = BitConverter.ToInt32(startingPointerBuffer, 0);
            byte[] statusLocation = new byte[4];
            int statusLocationReadBytes = 0;
            ReadProcessMemory((int)processHandle, statusLocationPointer, statusLocation, statusLocation.Length, ref statusLocationReadBytes);
            int instanceStatus = BitConverter.ToInt32(statusLocation, 0);

            thisInstance.instanceStatus = (InstanceStatus)instanceStatus;


        }
        // Function: ReadMemoryGameTimeLeft
        public static void ReadMemoryGameTimeLeft()
        {
            if (thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                thisInstance.gameInfoTimeRemaining = TimeSpan.FromMinutes(thisInstance.gameStartDelay + thisInstance.gameTimeLimit);
                return;
            }

            // Read pointer to map time
            byte[] ptr = new byte[4];
            int readPtr = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x00061098, ptr, ptr.Length, ref readPtr);
            int mapTimeAddr = BitConverter.ToInt32(ptr, 0);

            // Read elapsed map time in game ticks (assumed to be milliseconds)
            byte[] mapTimeMs = new byte[4];
            int mapTimeRead = 0;
            ReadProcessMemory((int)processHandle, mapTimeAddr, mapTimeMs, mapTimeMs.Length, ref mapTimeRead);
            int mapTime = BitConverter.ToInt32(mapTimeMs, 0);

            // Convert to seconds (if value is in milliseconds, otherwise adjust as needed)
            int mapTimeInSeconds = mapTime / 60;

            // Calculate total time in seconds
            int totalTimeInSeconds = (thisInstance.gameStartDelay + thisInstance.gameTimeLimit) * 60;

            // Calculate time remaining
            int timeRemainingSeconds = totalTimeInSeconds - mapTimeInSeconds;
            if (timeRemainingSeconds < 0) timeRemainingSeconds = 0;

            thisInstance.gameInfoTimeRemaining = TimeSpan.FromSeconds(timeRemainingSeconds);
        }
        // Function: ReadMemoryCurrentMissionName
        public static void ReadMemoryCurrentMissionName()
        {


            // memory polling
            int bytesRead = 0;
            byte[] buffer = new byte[26];
            ReadProcessMemory((int)processHandle, 0x0071569C, buffer, buffer.Length, ref bytesRead);
            string MissionName = Encoding.GetEncoding(1252).GetString(buffer);
            mapInstance.CurrentMapName = MissionName.Replace("\0", "");


        }
        // Fuction: ReadMemoryCurrentGameType
        public static void ReadMemoryCurrentGameType()
        {


            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, 0x009F21A4, buffer, buffer.Length, ref bytesRead);
            int GameType = BitConverter.ToInt32(buffer, 0);
            CommonCore.instanceMaps!.CurrentGameType = GameType;

        }
        // FunctionL ReadMemoryCurrentMapIndex
        public static void ReadMemoryCurrentMapIndex()
        {
            if (thisInstance.instanceStatus != InstanceStatus.STARTDELAY &&
                thisInstance.instanceStatus != InstanceStatus.ONLINE)
            {
                return;
            }

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int MapCycleIndex = BitConverter.ToInt32(ServerMapCyclePtr, 0) + 0xC;
            byte[] mapIndexBytes = new byte[4];
            int mapIndexRead = 0;
            ReadProcessMemory((int)processHandle, MapCycleIndex, mapIndexBytes, mapIndexBytes.Length, ref mapIndexRead);
            mapInstance.CurrentMapIndex = BitConverter.ToInt32(mapIndexBytes, 0);


        }
        // Function: ReadMemoryWinningTeam
        public static void ReadMemoryWinningTeam()
        {

            int bytesRead = 8;
            byte[] buffer = new byte[8];
            ReadProcessMemory((int)processHandle, 0x009F370C, buffer, buffer.Length, ref bytesRead);
            int gameMatchWinner = BitConverter.ToInt32(buffer, 0);
            thisInstance.gameMatchWinner = gameMatchWinner;


        }
        // Function: ReadMemoryCurrentNumPlayers
        public static void ReadMemoryCurrentNumPlayers()
        {


            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, 0x0065DCBC, buffer, buffer.Length, ref bytesRead);
            int CurrentPlayers = BitConverter.ToInt32(buffer, 0);
            thisInstance.gameInfoNumPlayers = CurrentPlayers;


        }
        // Function: Read NumTeam State
        public static bool ReadNumTeams()
        {
            byte[] NumTeamsBytes = new byte[4];
            int NumTeamsRead = 0;

            int NumTeamsPtr = 0x00A344C4;

            ReadProcessMemory((int)processHandle, NumTeamsPtr, NumTeamsBytes, NumTeamsBytes.Length, ref NumTeamsRead);

            int NumTeamsCount = BitConverter.ToInt32(NumTeamsBytes, 0);

            return (NumTeamsCount == 4);         

        }
        public static void ReadMemoryCurrentGameWinConditions()
        {
            int scoreAddress1 = 0;
            int scoreAddress2 = 0;
            int gameTypeId = CommonCore.instanceMaps!.CurrentGameType;

            // CommonCore.instanceMaps!.CurrentGameType
            switch (gameTypeId)
            {
                // KOTH/TKOTH
                case 3:
                case 4:
                    scoreAddress1 = baseAddr + 0x5F21B8;
                    scoreAddress2 = baseAddr + 0x6344B4;
                    break;
                // SD & AD
                case 5:
                case 6:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // SD = Both Teams have a "Win Condition" (Targets Destroyed)
                    // AD = Only the Attacking Team has a "Win Condition" (Targets Destroyed), Defending Team has to last the timer.
                    scoreAddress1 = baseAddr + 0x5DDCAC; // Blue
                    scoreAddress2 = baseAddr + 0x5DDCB0; // Red
                    break;
                // CTF
                case 7:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // Based on the number of red and blue flags in map, ideally both addresses should be the same.
                    scoreAddress1 = baseAddr + 0x5DDCA4; // Blue
                    scoreAddress2 = baseAddr + 0x5DDCA8; // Red
                    break;
                // flag ball
                case 8:
                    scoreAddress1 = baseAddr + 0x5F21AC;
                    scoreAddress2 = baseAddr + 0x6034B8;
                    break;
                // all other game types...
                default:
                    scoreAddress1 = baseAddr + 0x5F21AC;
                    scoreAddress2 = baseAddr + 0x6034B8;
                    break;
            }

            // Blue Win Condition
            byte[] scoreBytes = new byte[4];
            int bytesRead = 0;
            bool success = ReadProcessMemory((int)processHandle, scoreAddress1, scoreBytes, scoreBytes.Length, ref bytesRead);
            int winConditionBlue = (success ? BitConverter.ToInt32(scoreBytes, 0) : 0);

            // Red Win Condition
            byte[] score2Bytes = new byte[4];
            int bytes2Read = 0;
            bool success2 = ReadProcessMemory((int)processHandle, scoreAddress2, score2Bytes, score2Bytes.Length, ref bytes2Read);
            int winConditionRed = (success2 ? BitConverter.ToInt32(score2Bytes, 0) : 0);

            if (winConditionBlue == winConditionRed || winConditionBlue > winConditionRed)
            {
                if (gameTypeId == 6) { thisInstance.gameInfoIsBlueDefending = true; }
                thisInstance.gameInfoWinCond = winConditionBlue;
            }
            else if (winConditionBlue < winConditionRed)
            {
                if (gameTypeId == 6) { thisInstance.gameInfoIsBlueDefending = false; }
                thisInstance.gameInfoWinCond = winConditionRed;
            }
            else
            {
                // Should never hit this, but just in case...
                thisInstance.gameInfoWinCond = winConditionRed;
            }
            
        }

        public static void ReadMemoryCurrentGameScores()
        {
            // DM = First Player to Reach Win Condition
            //    Return 0
            // KOTH = First Team to Reach Win Condition
            //    Return 0
            // COOP = NA
            //    Return 0
            // TDM = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E07EC
            //     RED OFFSET  = 0x5E08D4
            // TKOTH = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0884
            //     RED OFFSET  = 0x5E096C
            // FB/CTF = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0800
            //     RED OFFSET  = 0x5E08E8
            // SD/AD = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0808
            //     RED OFFSET  = 0x5E08F0

            int scoreAddress1 = 0;
            int scoreAddress2 = 0;
            int gameTypeId = CommonCore.instanceMaps!.CurrentGameType;

            switch (gameTypeId)
            {
                // KOTH/TKOTH
                case 3:
                    scoreAddress1 = baseAddr + 0x5E0884; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E096C; // Red Score
                    break;
                // SD & AD
                case 5:
                case 6:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // SD = Both Teams have a "Win Condition" (Targets Destroyed)
                    // AD = Only the Attacking Team has a "Win Condition" (Targets Destroyed), Defending Team has to last the timer.
                    scoreAddress1 = baseAddr + 0x5E0808; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E08F0; // Red Score
                    break;
                // CTF
                case 7:
                case 8:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // Based on the number of red and blue flags in map, ideally both addresses should be the same.
                    scoreAddress1 = baseAddr + 0x5DDCA4; // Blue Score
                    scoreAddress2 = baseAddr + 0x5DDCA8; // Red Score
                    break;
                // all other game types...
                default:
                    scoreAddress1 = baseAddr + 0x5E07EC; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E08D4; // Red Score
                    break;
            }

            if (gameTypeId == 0 || gameTypeId == 2 || gameTypeId == 4)
            {
                thisInstance.gameInfoBlueScore = 0;
                thisInstance.gameInfoRedScore = 0;
                return;
            }

            // Blue Win Condition
            byte[] scoreBytes = new byte[4];
            int bytesRead = 0;
            bool success = ReadProcessMemory((int)processHandle, scoreAddress1, scoreBytes, scoreBytes.Length, ref bytesRead);
            int blueTeamScore = (success ? BitConverter.ToInt32(scoreBytes, 0) : 0);

            // Red Win Condition
            byte[] score2Bytes = new byte[4];
            int bytes2Read = 0;
            bool success2 = ReadProcessMemory((int)processHandle, scoreAddress2, score2Bytes, score2Bytes.Length, ref bytes2Read);
            int redTeamScore = (success2 ? BitConverter.ToInt32(score2Bytes, 0) : 0);

            thisInstance.gameInfoBlueScore = blueTeamScore;
            thisInstance.gameInfoRedScore = redTeamScore;
            return;

        }
        // Function: ReadMemoryPlayerLeaningStatus
        // Returns: 0 = upright, 2 = left leaning, 4 = right leaning
        public static int ReadMemoryPlayerLeaningStatus(int playerSlot)
        {
            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Calculate the player's address
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            // Read prone/rolling status (2 bytes)
            byte[] proneStatusBytes = new byte[2];
            int proneStatusRead = 0;
            ReadProcessMemory((int)processHandle, playerObjectLocation + 0x164, proneStatusBytes, proneStatusBytes.Length, ref proneStatusRead);
            short proneStatus = BitConverter.ToInt16(proneStatusBytes, 0);

            // Check if player is rolling (95 or 96) - ignore leaning status when rolling
            if (proneStatus == 95 || proneStatus == 96)
            {
                AppDebug.Log("ReadMemoryPlayerLeaningStatus", $"Slot {playerSlot} - Rolling detected (prone: {proneStatus}), ignoring lean");
                return 0; // Return 0 (upright) when rolling
            }

            // Read leaning status (2 bytes)
            byte[] leaningStatusBytes = new byte[2];
            int leaningStatusRead = 0;
            ReadProcessMemory((int)processHandle, playerObjectLocation + 0x102, leaningStatusBytes, leaningStatusBytes.Length, ref leaningStatusRead);
            short leaningStatus = BitConverter.ToInt16(leaningStatusBytes, 0);

            // Normalize the status (remove the +8 for moving)
            // 0 = upright, 2 = left lean, 4 = right lean
            // 8 = moving upright, 10 = moving left lean, 12 = moving right lean
            int normalizedStatus = leaningStatus % 8;

            AppDebug.Log("ReadMemoryPlayerLeaningStatus", 
                $"Slot {playerSlot} - Object: 0x{playerObjectLocation:X8}, " +
                $"Lean Addr: 0x{(playerObjectLocation + 0x102):X8}, " +
                $"Prone Addr: 0x{(playerObjectLocation + 0x164):X8}, " +
                $"Raw: {leaningStatus}, Normalized: {normalizedStatus}");

            return normalizedStatus;
        }

	}
}
