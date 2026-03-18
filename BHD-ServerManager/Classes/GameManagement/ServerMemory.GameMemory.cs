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

		// Function: Read Start Delay Timer, Reads the Start Delay Timer from Memory and stores it in the instance variable.
		public static void ReadStartDelayCounter() => theInstance.gameInfoStartDelayTimer = ReadInt(baseAddr + 0x5DAE04);

        // Function: ReadMemoryGameTimeLeft
        public static void ReadMemoryGameTimeLeft()
        {
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                theInstance.gameInfoTimeRemaining = TimeSpan.FromMinutes(theInstance.gameStartDelay + theInstance.gameTimeLimit);
                return;
            }

            // Read elapsed map time in game ticks (assumed to be milliseconds)
            int mapTime = ReadInt(ReadInt(baseAddr + 0x00061098));

            // Convert to seconds (if value is in milliseconds, otherwise adjust as needed)
            int mapTimeInSeconds = mapTime / 60;

            // Calculate total time in seconds
            int totalTimeInSeconds = (theInstance.gameStartDelay + theInstance.gameTimeLimit) * 60;

            // Calculate time remaining
            int timeRemainingSeconds = totalTimeInSeconds - mapTimeInSeconds;
            if (timeRemainingSeconds < 0) timeRemainingSeconds = 0;

            theInstance.gameInfoTimeRemaining = TimeSpan.FromSeconds(timeRemainingSeconds);
        }

		// Function: ReadMemoryCurrentMissionName
		// Active Poll of the current mission name in memory.
        // Review Removal
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
		// Active Poll of the current game type in memory, sometimes the game type is not properly updated in memory on loading of a map.
		public static void ReadMemoryCurrentGameType() => mapInstance.CurrentGameType = ReadInt(0x009F21A4);

        // Function: ReadMemoryCurrentMapIndex
        public static void ReadMemoryCurrentMapIndex()
        {
            if (theInstance.instanceStatus is InstanceStatus.STARTDELAY or InstanceStatus.ONLINE)
                mapInstance.CurrentMapIndex = ReadInt(GetMapCycleServerAddress() + 0xC);
        }

        // Function: ReadMemoryWinningTeam
        public static void ReadMemoryWinningTeam() => theInstance.gameMatchWinner = ReadInt(0x009F370C);

        // Function: ReadMemoryCurrentNumPlayers
        public static void ReadMemoryCurrentNumPlayers() => theInstance.gameInfoNumPlayers = ReadInt(0x0065DCBC);

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
            int winConditionBlue = ReadInt(scoreAddress1);

            // Red Win Condition
            int winConditionRed = ReadInt(scoreAddress2);

            if (winConditionBlue == winConditionRed || winConditionBlue > winConditionRed)
            {
                if (gameTypeId == 6) { theInstance.gameInfoIsBlueDefending = true; }
                theInstance.gameInfoWinCond = winConditionBlue;
            }
            else if (winConditionBlue < winConditionRed)
            {
                if (gameTypeId == 6) { theInstance.gameInfoIsBlueDefending = false; }
                theInstance.gameInfoWinCond = winConditionRed;
            }
            else
            {
                // Should never hit this, but just in case...
                theInstance.gameInfoWinCond = winConditionRed;
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
                theInstance.gameInfoBlueScore = 0;
                theInstance.gameInfoRedScore = 0;
                return;
            }

            // Blue Score
            int blueTeamScore = ReadInt(scoreAddress1);

            // Red Score
            int redTeamScore = ReadInt(scoreAddress2);

            theInstance.gameInfoBlueScore = blueTeamScore;
            theInstance.gameInfoRedScore = redTeamScore;
            return;

        }


	}
}
