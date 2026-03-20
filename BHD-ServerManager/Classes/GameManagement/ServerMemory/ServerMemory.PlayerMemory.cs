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
        private const int PLAYER_LIST_PTR   = 0x009ED600;
        private const int PLAYER_STRIDE     = 0xAF33C;
        private const int PLAYER_OFF_ENTITY = 0x18;
        private const int PLAYER_OFF_NAME   = 0x1C;

        // Returns the start address of the session struct for the given 1-based slot number.
        // Returns 0 when slot/container/base is invalid.
        private static string GetPlayerName(int playerEntityPtr)
        {
            int container = ReadInt(PLAYER_LIST_PTR);
            if (container == 0) return "Unknown";
            int count     = ReadInt(container);
            int baseAddr  = ReadInt(container + 4);
            if (baseAddr == 0 || count <= 0) return "Unknown";

            for (int i = 0; i < count; i++)
            {
                int session = baseAddr + i * PLAYER_STRIDE;
                if (ReadInt(session + PLAYER_OFF_ENTITY) == playerEntityPtr)
                    return ReadString(session + PLAYER_OFF_NAME);
            }
            return "Unknown";
        }
        private static int GetPlayerAddress(int slot)
        {
            if (slot <= 0) return 0;

            int container = ReadInt(PLAYER_LIST_PTR);
            if (container == 0) return 0;

            int count = ReadInt(container);
            int playerBaseAddr = ReadInt(container + 4);
            if (playerBaseAddr == 0 || count <= 0 || slot > count) return 0;

            return playerBaseAddr + (slot - 1) * PLAYER_STRIDE;
        }
        // Function: WriteMemoryDisarmPlayer
        public static void WriteMemoryDisarmPlayer(int playerSlot)
        {


            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] disablePlayerWeapon = BitConverter.GetBytes(0);
            int disablePlayerWeaponWrite = 0;
            WriteProcessMemory((int)processHandle, playerNewLocationAddress + 0xADE08, disablePlayerWeapon, disablePlayerWeapon.Length, ref disablePlayerWeaponWrite);


        }
        // Function: WriteMemoryArmPlayer
        public static void WriteMemoryArmPlayer(int playerSlot)
        {

            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] disablePlayerWeapon = BitConverter.GetBytes(1);
            int disablePlayerWeaponWrite = 0;
            WriteProcessMemory((int)processHandle, playerNewLocationAddress + 0xADE08, disablePlayerWeapon, disablePlayerWeapon.Length, ref disablePlayerWeaponWrite);


        }
        // Function: WriteMemoryKillPlayer
        public static void WriteMemoryKillPlayer(int playerSlot)
        {
            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;
            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            byte[] setPlayerHealth = BitConverter.GetBytes(0);
            int setPlayerHealthWrite = 0;

            WriteProcessMemory((int)processHandle, playerObjectLocation + 0x138, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);
            WriteProcessMemory((int)processHandle, playerObjectLocation + 0xE2, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);


        }
        // Function: WriteMemoryTogglePlayerGodMode
        public static void WriteMemoryTogglePlayerGodMode(int playerSlot, int health)
        {


            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            byte[] setPlayerHealth = BitConverter.GetBytes(health); //set god mode health
            int setPlayerHealthWrite = 0;

            byte[] setDamageBy = BitConverter.GetBytes(0);
            int setDamageByWrite = 0;

            AppDebug.Log("WriteMemoryTogglePlayerGodMode", $"Player Health Data - Base: 0x{playerNewLocationAddress:X8}, Object: 0x{playerObjectLocation:X8}, Damage Addr: 0x{(playerObjectLocation + 0x138):X8}, Health Addr: 0x{(playerObjectLocation + 0xE2):X8}");

            WriteProcessMemory((int)processHandle, playerObjectLocation + 0x138, setDamageBy, setDamageBy.Length, ref setDamageByWrite);
            WriteProcessMemory((int)processHandle, playerObjectLocation + 0xE2, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);


        }
        // Function UpdatePlayerTeam
        public static void UpdatePlayerTeam()
        {
            if (playerInstance.PlayerChangeTeamList.Count == 0)
            {
                return;
            }

            // Determine if next map supports 4 teams (safety check)
            bool nextMapIs4Team = thisInstance.gameEnableFourTeams && 
                                 mapInstance.IsNextMap4Team &&
                                 (mapInstance.NextMapGameType == 1 || mapInstance.NextMapGameType == 3 || mapInstance.NextMapGameType == 8);

            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Iterate backwards to safely remove items during iteration
            for (int ii = playerInstance.PlayerChangeTeamList.Count - 1; ii >= 0; ii--)
            {
                var teamSwitch = playerInstance.PlayerChangeTeamList[ii];
                
                // SAFETY NET: Final validation before writing to memory
                if (!nextMapIs4Team && (teamSwitch.Team < 1 || teamSwitch.Team > 2))
                {
                    AppDebug.Log("UpdatePlayerTeam", 
                        $"SAFETY: Skipping invalid team switch - Slot {teamSwitch.slotNum} → Team {teamSwitch.Team} " +
                        $"(Next map '{mapInstance.NextMapName}' is 2-team only)");
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    AppDebug.Log("UpdatePlayerTeam", 
                        $"SAFETY: Skipping invalid team number - Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}");
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                // Apply team switch to game memory
                int playerLocationOffset = (teamSwitch.slotNum - 1) * 0xAF33C;
                int playerLocation = playerlistStartingLocation + playerLocationOffset;
                int playerTeamLocation = playerLocation + 0x90;
                byte[] teamBytes = BitConverter.GetBytes(teamSwitch.Team);
                int bytesWritten = 0;
                WriteProcessMemory((int)processHandle, playerTeamLocation, teamBytes, teamBytes.Length, ref bytesWritten);
                
                AppDebug.Log("UpdatePlayerTeam", 
                    $"Applied team switch: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}");
                
                playerInstance.PlayerChangeTeamList.RemoveAt(ii);
            }
        }
        /// <summary>
        /// Validate and normalize team switches before applying them to game memory.
        /// Filters out invalid team numbers and normalizes teams that don't exist on the next map.
        /// </summary>
        private static void ValidateAndNormalizeTeamSwitches(bool nextMapIs4Team)
        {
            if (playerInstance.PlayerChangeTeamList.Count == 0)
                return;

            var toRemove = new List<playerTeamObject>();

            foreach (var teamSwitch in playerInstance.PlayerChangeTeamList)
            {
                // If next map is 2-team only, normalize team 3/4 to team 1/2
                if (!nextMapIs4Team && (teamSwitch.Team < 1 || teamSwitch.Team > 2))
                {
                    if (teamSwitch.Team == 3 || teamSwitch.Team == 4)
                    {
                        int normalizedTeam = ((teamSwitch.Team - 1) % 2) + 1; // Team 3→1, Team 4→2
                        AppDebug.Log("ValidateTeamSwitches", 
                            $"Normalizing team switch: Slot {teamSwitch.slotNum} from Team {teamSwitch.Team} → Team {normalizedTeam}. " +
                            $"Next map '{mapInstance.NextMapName}' only supports 2 teams.");
                        teamSwitch.Team = normalizedTeam;
                    }
                    else
                    {
                        AppDebug.Log("ValidateTeamSwitches", 
                            $"Invalid team number detected: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}. Removing from queue.");
                        toRemove.Add(teamSwitch);
                    }
                }
                // If next map is 4-team, validate team is 1-4
                else if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    AppDebug.Log("ValidateTeamSwitches", 
                        $"Invalid team number detected: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}. Removing from queue.");
                    toRemove.Add(teamSwitch);
                }
            }

            // Remove invalid switches
            foreach (var item in toRemove)
            {
                playerInstance.PlayerChangeTeamList.Remove(item);
            }

            if (toRemove.Count > 0)
            {
                AppDebug.Log("ValidateTeamSwitches", 
                    $"Filtered {toRemove.Count} invalid team switch(es). {playerInstance.PlayerChangeTeamList.Count} valid switch(es) remain.");
            }
        }
        // Function: ReadMemoryGeneratePlayerList
        public static void ReadMemoryGeneratePlayerList()
        {
            if (thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            Dictionary<int, PlayerObject> currentPlayerList = new Dictionary<int, PlayerObject>();
            int NumPlayers = thisInstance.gameInfoNumPlayers;

            if (NumPlayers > 0)
            {

                int buffer = 0;
                var Pointer = baseAddr + 0x005ED600;

                byte[] PointerAddr9 = new byte[4];
                ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
                var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

                byte[] playerListStartingLocationByteArray = new byte[4];
                ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref buffer);

                int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);
                int failureCount = 0;

                for (int i = 0; i < NumPlayers; i++)
                {
                    if (failureCount == thisInstance.gameMaxSlots)
                    {
                        break;
                    }

                    byte[] slotNumberValue = new byte[4];
                    int slotNumberLocation = playerlistStartingLocation + 0xC;
                    ReadProcessMemory((int)processHandle, slotNumberLocation, slotNumberValue, slotNumberValue.Length, ref buffer);
                    int playerSlot = BitConverter.ToInt32(slotNumberValue, 0);

                    byte[] playerNameBytes = new byte[15];
                    int playerNameLocation = playerlistStartingLocation + 0x1C;
                    ReadProcessMemory((int)processHandle, playerNameLocation, playerNameBytes, playerNameBytes.Length, ref buffer);
                    string formattedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(playerNameBytes).Replace("\0", "");

                    if (string.IsNullOrEmpty(formattedPlayerName) || string.IsNullOrWhiteSpace(formattedPlayerName))
                    {
                        playerlistStartingLocation += 0xAF33C;
                        i--;
                        failureCount++;
                        continue;
                    }

                    byte[] playerTeamBytes = new byte[4];
                    int playerTeamLocation = playerlistStartingLocation + 0x90;
                    ReadProcessMemory((int)processHandle, playerTeamLocation, playerTeamBytes, playerTeamBytes.Length, ref buffer);
                    int playerTeam = BitConverter.ToInt32(playerTeamBytes, 0);
                    string playerIP = ReadMemoryGrabPlayerIPAddress(formattedPlayerName).ToString();

                    PlayerObject PlayerStats = ReadMemoryPlayerStats(playerSlot);
                    CharacterClass PlayerCharacterClass = (CharacterClass)PlayerStats.RoleID;
                    WeaponStack PlayerSelectedWeapon = (WeaponStack)PlayerStats.SelectedWeaponID;

                    Dictionary<int, List<WeaponStack>> PlayerWeapons = new Dictionary<int, List<WeaponStack>>();

                    if (string.IsNullOrEmpty(formattedPlayerName) || string.IsNullOrWhiteSpace(formattedPlayerName))
                    {
                        if (currentPlayerList.Count >= NumPlayers)
                        {
                            break;
                        }
                        else
                        {
                            playerlistStartingLocation += 0xAF33C;
                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            // Try to preserve PlayerJoined and CountryCode if player already exists in the persistent list
                            if (playerInstance.PlayerList.TryGetValue(playerSlot, out var existingPlayer))
                            {
                                PlayerStats.PlayerJoined = existingPlayer.PlayerJoined;
                                PlayerStats.CountryCode = existingPlayer.CountryCode;
                            }
                            // Final Touches
                            PlayerStats.PlayerLastSeen = DateTime.Now;
                            PlayerStats.PlayerTimePlayed = (int)(PlayerStats.PlayerLastSeen - PlayerStats.PlayerJoined).TotalSeconds;
                            PlayerStats.PlayerSlot = playerSlot;
                            byte[] trimmedPlayerNameBytes = playerNameBytes.Where(b => b != 0).ToArray();
                            PlayerStats.PlayerNameBase64 = Convert.ToBase64String(trimmedPlayerNameBytes);
                            PlayerStats.PlayerName = formattedPlayerName;
                            PlayerStats.PlayerTeam = playerTeam;
                            PlayerStats.PlayerIPAddress = playerIP;
                            PlayerStats.PlayerPing = PlayerStats.PlayerPing;
                            PlayerStats.RoleName = PlayerCharacterClass.ToString();
                            PlayerStats.SelectedWeaponName = PlayerSelectedWeapon.ToString();

                            // Push to List
                            currentPlayerList.Add(playerSlot, PlayerStats);

                            // Setup for Next Player
                            playerlistStartingLocation += 0xAF33C;

                            if (currentPlayerList.Count >= NumPlayers)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            AppDebug.Log("ServerMemory", "Detected an error!\n\n" + "Player Name: " + playerSlot + "\n\n" + formattedPlayerName + "\n\n" + e.ToString());
                        }

                    }

                }


            }
            playerInstance.PlayerList.Clear();
            foreach (var kvp in currentPlayerList)
            {
                playerInstance.PlayerList[kvp.Key] = kvp.Value;
            }
            // CoreManager.DebugLog("PlayerList Updated");
        }
        // Function: ReadMemoryGrabPlayerIPAddress
        public static string ReadMemoryGrabPlayerIPAddress(string playername)
        {


            const int playerIpAddressPointerOffset = 0x00ACE248;
            const int playernameOffset = 0xBC;

            int playerIpAddressPointerBuffer = 0;
            byte[] PointerAddr_2 = new byte[4];
            ReadProcessMemory((int)processHandle, baseAddr + playerIpAddressPointerOffset, PointerAddr_2, PointerAddr_2.Length, ref playerIpAddressPointerBuffer);

            int IPList = BitConverter.ToInt32(PointerAddr_2, 0) + playernameOffset;

            int failureCounter = 0;
            while (failureCounter <= thisInstance.gameMaxSlots)
            {
                byte[] playername_bytes = new byte[15];
                int playername_buffer = 0;
                ReadProcessMemory((int)processHandle, IPList, playername_bytes, playername_bytes.Length, ref playername_buffer);
                var currentPlayerName = Encoding.GetEncoding("Windows-1252").GetString(playername_bytes).Replace("\0", "");

                if (currentPlayerName == playername)
                {
                    failureCounter = 0;
                    break;
                }

                IPList += playernameOffset;
                failureCounter++;
            }

            if (failureCounter > thisInstance.gameMaxSlots)
            {

                return null!;
            }

            byte[] playerIPBytesPtr = new byte[4];
            int playerIPBufferPtr = 0;
            ReadProcessMemory((int)processHandle, IPList + 0xA4, playerIPBytesPtr, playerIPBytesPtr.Length, ref playerIPBufferPtr);

            int PlayerIPLocation = BitConverter.ToInt32(playerIPBytesPtr, 0) + 4;
            byte[] playerIPAddressBytes = new byte[4];
            int playerIPAddressBuffer = 0;
            ReadProcessMemory((int)processHandle, PlayerIPLocation, playerIPAddressBytes, playerIPAddressBytes.Length, ref playerIPAddressBuffer);

            IPAddress playerIp = new IPAddress(playerIPAddressBytes);

            return playerIp.ToString();
        }
        // Function: ReadMemoryPlayerStats
        public static PlayerObject ReadMemoryPlayerStats(int reqslot)
        {


            var baseaddr = 0x400000;
            var startList = baseaddr + 0x005ED600;

            byte[] startaddr = new byte[4];
            int startaddr_read = 0;
            ReadProcessMemory((int)processHandle, startList, startaddr, startaddr.Length, ref startaddr_read);
            var firstplayer = BitConverter.ToInt32(startaddr, 0) + 0x28;

            byte[] scanbeginaddr = new byte[4];
            ReadProcessMemory((int)processHandle, firstplayer, scanbeginaddr, scanbeginaddr.Length, ref startaddr_read);
            int beginaddr = BitConverter.ToInt32(scanbeginaddr, 0);

            if (reqslot != 1)
            {
                for (int i = 1; i < reqslot; i++)
                {
                    beginaddr += 0xAF33C;
                }
            }

            int playerMemStart = beginaddr;

            byte[] read_name = new byte[15];
            int bytesread = 0;

            ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
            var PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name).Replace("\0", "");

            if (string.IsNullOrEmpty(PlayerName))
            {
                beginaddr += 0xAF33C;
                // Retry read if player name is empty
                ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
                PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name).Replace("\0", "");
            }

            // Handle failure if still no player name found
            if (string.IsNullOrEmpty(PlayerName))
            {

                AppDebug.Log("ServerMemory", "Something went wrong here. We can't find any player names.");
                return new PlayerObject();
            }

            byte[] read_ping = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB40, read_ping, read_ping.Length, ref bytesread);

            int[] offsets = {
                0x000ADAB4, // stat_TotalShotsFired
                0x000ADA94, // stat_Kills
                0x000ADA98, // stat_Deaths
                0x000ADA8C, // stat_Suicides
                0x000ADAD0, // stat_Headshots
                0x000ADA90, // stat_Murders
                0x000ADAD4, // stat_KnifeKills
                0x000ADAF4, // stat_ExperiencePoints
                0x000ADABC, // stat_RevivesReceived
                0x000ADAC0, // stat_PSPAttempts
                0x000ADAC4, // stat_PSPTakeovers
                0x000ADACC, // stat_DoubleKills
                0x000ADACC, // stat_RevivesGiven
                0x000ADAA8, // stat_FBCaptures
                0x000ADAC8, // stat_FBCarrierKills
                0x000ADAD4, // stat_FBCarrierDeaths
                0x000ADAA4, // stat_ZoneTime
                0x000ADADC, // stat_ZoneKills
                0x000ADA94, // stat_ZoneDefendKills
                0x000ADAB0, // stat_SDADTargetsDestroyed
                0x000ADAAC, // stat_FlagSaves
                0x000ADAD8, // stat_SniperKills
                0x000ADADC, // stat_TKOTHDefenseKills
                0x000ADAE0, // stat_TKOTHAttackKills
                0x000ADAE4, // stat_SDADDefendKill
                0x000ADAEC, // stat_SDADAttackKill
            };

            var stats = new int[offsets.Length];

            for (int i = 0; i < offsets.Length; i++)
            {
                byte[] read_data = new byte[4];
                ReadProcessMemory((int)processHandle, beginaddr + offsets[i], read_data, read_data.Length, ref bytesread);
                stats[i] = BitConverter.ToInt32(read_data, 0);
            }

            // Trail Checks
            int[] offsets2 =
            {
                // 20 fields before the first offset
                0x000ADA8C - 80, 0x000ADA8C - 76, 0x000ADA8C - 72, 0x000ADA8C - 68, 0x000ADA8C - 64,
                0x000ADA8C - 60, 0x000ADA8C - 56, 0x000ADA8C - 52, 0x000ADA8C - 48, 0x000ADA8C - 44,
                0x000ADA8C - 40, 0x000ADA8C - 36, 0x000ADA8C - 32, 0x000ADA8C - 28, 0x000ADA8C - 24,
                0x000ADA8C - 20, 0x000ADA8C - 16, 0x000ADA8C - 12, 0x000ADA8C - 8,  0x000ADA8C - 4,

                // All offsets between the lowest and highest, in 4-byte increments
                // From 0x000ADA8C to 0x000ADAF4
                0x000ADA8C, 0x000ADA90, 0x000ADA94, 0x000ADA98, 0x000ADA9C,
                0x000ADAA0, 0x000ADAA4, 0x000ADAA8, 0x000ADAAC, 0x000ADAB0,
                0x000ADAB4, 0x000ADAB8, 0x000ADABC, 0x000ADAC0, 0x000ADAC4,
                0x000ADAC8, 0x000ADACC, 0x000ADAD0, 0x000ADAD4, 0x000ADAD8,
                0x000ADADC, 0x000ADAE0, 0x000ADAE4, 0x000ADAE8, 0x000ADAEC,
                0x000ADAF0, 0x000ADAF4,

                // 20 fields after the last offset
                0x000ADAF4 + 4,  0x000ADAF4 + 8,  0x000ADAF4 + 12, 0x000ADAF4 + 16, 0x000ADAF4 + 20,
                0x000ADAF4 + 24, 0x000ADAF4 + 28, 0x000ADAF4 + 32, 0x000ADAF4 + 36, 0x000ADAF4 + 40,
                0x000ADAF4 + 44, 0x000ADAF4 + 48, 0x000ADAF4 + 52, 0x000ADAF4 + 56, 0x000ADAF4 + 60,
                0x000ADAF4 + 64, 0x000ADAF4 + 68, 0x000ADAF4 + 72, 0x000ADAF4 + 76, 0x000ADAF4 + 80
            };

            // Remove offsets in offsets2 that are present in offsets
            int[] filteredOffsets2 = offsets2.Except(offsets).ToArray();

            var stats2 = new int[filteredOffsets2.Length];
            for (int i = 0; i < filteredOffsets2.Length; i++)
            {
                byte[] read_data = new byte[4];
                ReadProcessMemory((int)processHandle, beginaddr + filteredOffsets2[i], read_data, read_data.Length, ref bytesread);
                stats2[i] = BitConverter.ToInt32(read_data, 0);
            }

            // For offsets2, skip entries where stats2[i] == 0
            string offsets2Log = string.Join(", ",
                filteredOffsets2
                    .Select((offset, i) => (offset, value: stats2[i]))
                    .Where(pair => pair.value != 0)
                    .Select(pair => $"{beginaddr + pair.offset:X8}:{pair.offset:X8} => {pair.value}\n")
            );
            // AppDebug.Log("PlayerStats", $"{PlayerName} :" + offsets2Log);
            
            // Score Offsets
            string offsetsLog = string.Join(", ", offsets.Select((offset, i) => $"{beginaddr + offset:X8}:{offset:X8} => {stats[i]}\n"));
            // AppDebug.Log("PlayerStats", $"{PlayerName} :" + offsetsLog);

            // Read Player Flag Time
            byte[] read_playerActiveZoneTimeByte = new byte[4];
            int activeZoneTimerLoc = playerMemStart + 0xADB2C;
            ReadProcessMemory((int)processHandle, activeZoneTimerLoc, read_playerActiveZoneTimeByte, read_playerActiveZoneTimeByte.Length, ref bytesread);
            int read_playerActiveZoneTimeInt = BitConverter.ToInt32(read_playerActiveZoneTimeByte, 0);

            // Read Player Object
            byte[] read_playerObjectLocation = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x5E7C, read_playerObjectLocation, read_playerObjectLocation.Length, ref bytesread);
            int read_playerObject = BitConverter.ToInt32(read_playerObjectLocation, 0);

            // Selected Weapon & Character Class
            byte[] read_selectedWeapon = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x178, read_selectedWeapon, read_selectedWeapon.Length, ref bytesread);
            int SelectedWeapon = BitConverter.ToInt32(read_selectedWeapon, 0);

            byte[] read_selectedCharacterClass = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x244, read_selectedCharacterClass, read_selectedCharacterClass.Length, ref bytesread);
            int SelectedCharacterClass = BitConverter.ToInt32(read_selectedCharacterClass, 0);
            
            // Weapons
            byte[] read_weapons = new byte[250];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB70, read_weapons, read_weapons.Length, ref bytesread);
            var MemoryWeapons = Encoding.GetEncoding(1252).GetString(read_weapons).Replace("\0", "|");
            string[] weapons = MemoryWeapons.Split('|');
            List<string> WeaponList = new List<string>();

            int failureCount = 0;
            foreach (var item in weapons)
            {
                if (!string.IsNullOrEmpty(item) && failureCount != 3)
                {
                    WeaponList.Add(item);
                }
                else
                {
                    if (failureCount == 3)
                    {
                        break;
                    }
                    else
                    {
                        failureCount++;
                    }
                }
            }

            return new PlayerObject
            {
                PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name),
                PlayerPing = BitConverter.ToInt32(read_ping, 0),
                RoleID = SelectedCharacterClass,
                SelectedWeaponID = SelectedWeapon,
                PlayerWeapons = WeaponList,
                ActiveZoneTime = read_playerActiveZoneTimeInt,
                stat_TotalShotsFired = stats[0],
                stat_Kills = stats[1],
                stat_Deaths = stats[2],
                stat_Suicides = stats[3],
                stat_Headshots = stats[4],
                stat_Murders = stats[5],
                stat_KnifeKills = stats[6],
                stat_ExperiencePoints = stats[7],
                stat_RevivesReceived = stats[8],
                stat_PSPAttempts = stats[9],
                stat_PSPTakeovers = stats[10],
                stat_DoubleKills = stats[11],
                stat_RevivesGiven = stats[12],
                stat_FBCaptures = stats[13],
                stat_FBCarrierKills = stats[14],
                stat_FBCarrierDeaths = stats[15],
                stat_ZoneTime = stats[16],
                stat_ZoneKills = stats[17],
                stat_ZoneDefendKills = stats[18],
                stat_SDADTargetsDestroyed = stats[19],
                stat_FlagSaves = stats[20],
                stat_SniperKills = stats[21],
                stat_TKOTHDefenseKills = stats[22],
                stat_TKOTHAttackKills = stats[23],
                stat_SDADDefenseKills = stats[24],
                stat_SDADAttackKills = stats[25]
            };
        }

	}
}
