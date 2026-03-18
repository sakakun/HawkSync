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

        public static void ChangeTeamGameMode()
        {
            var maps = CommonCore.instanceMaps!;

            // Map's game type (e.g. DM, TDM, KOTH)
            int currentMapType = maps.CurrentGameType;
            int nextMapType    = maps.NextMapGameType;
            
            // Is map team-based (TDM/KOTH/FB) vs non-team (DM/FFA)
            bool isCurrentMapTeamMap = Functions.IsMapTeamBased(currentMapType);
            bool isNextMapTeamMap    = Functions.IsMapTeamBased(nextMapType);

			// Is map designed as a 4 Team Map (has 4T3AM in the file name)
			bool isCurrentMap4Team = maps.IsCurrentMap4Team;
            bool isNextMap4Team    = maps.IsNextMap4Team;

            // SCENARIO 1: Team-based → Non-team (2-team or 4-team → FFA/DM)
            if (isNextMapTeamMap == false && isCurrentMapTeamMap == true)
            {
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject playerObj = playerRecord.Value;
                    playerInstance.PlayerPreviousTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerTeam
                    });

                    // Assign all players to team 0 (Deathmatch/FFA)
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = (int)Teams.TEAM_GREEN // 0 = FFA
                    });
                }
            }

            // SCENARIO 2: Non-team → Team-based (FFA/DM → 2-team or 4-team)
            else if (isNextMapTeamMap == true && isCurrentMapTeamMap == false)
            {
                // Restore players who were on teams before FFA
                foreach (playerTeamObject playerObj in playerInstance.PlayerPreviousTeamList)
                {
                    if (playerInstance.PlayerList.ContainsKey(playerObj.slotNum))
                    {
                        playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = playerObj.slotNum,
                            Team = playerObj.Team
                        });
                    }
                }

                // Balance new players who joined during FFA
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject player = playerRecord.Value;
                    bool found = playerInstance.PlayerPreviousTeamList.Any(p => p.slotNum == player.PlayerSlot);

                    if (!found)
                    {
                        int assignedTeam = BalanceNewPlayer(isNextMap4Team);
                        
                        playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = player.PlayerSlot,
                            Team = assignedTeam
                        });
                        playerInstance.PlayerPreviousTeamList.Add(new playerTeamObject
                        {
                            slotNum = player.PlayerSlot,
                            Team = assignedTeam
                        });
                    }
                }
                playerInstance.PlayerPreviousTeamList.Clear();
            }

            // SCENARIO 3: 2-team → 4-team
            else if (isCurrentMapTeamMap && isNextMapTeamMap && !isCurrentMap4Team && isNextMap4Team)
            {
                // Split existing 2 teams into 4 teams
                var blueTeamPlayers = playerInstance.PlayerList.Values.Where(p => p.PlayerTeam == (int)Teams.TEAM_BLUE).ToList();
                var redTeamPlayers = playerInstance.PlayerList.Values.Where(p => p.PlayerTeam == (int)Teams.TEAM_RED).ToList();

                // Split Blue team → Blue (1) and Yellow (3)
                for (int i = 0; i < blueTeamPlayers.Count; i++)
                {
                    int newTeam = (i % 2 == 0) ? (int)Teams.TEAM_BLUE : (int)Teams.TEAM_YELLOW;
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = blueTeamPlayers[i].PlayerSlot,
                        Team = newTeam
                    });
                }

                // Split Red team → Red (2) and Purple (4)
                for (int i = 0; i < redTeamPlayers.Count; i++)
                {
                    int newTeam = (i % 2 == 0) ? (int)Teams.TEAM_RED : (int)Teams.TEAM_PURPLE;
                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = redTeamPlayers[i].PlayerSlot,
                        Team = newTeam
                    });
                }


            }

            // SCENARIO 4: 4-team → 2-team
            else if (isCurrentMapTeamMap && isNextMapTeamMap && isCurrentMap4Team && !isNextMap4Team)
            {
                // Merge 4 teams into 2 teams
                foreach (var playerRecord in playerInstance.PlayerList)
                {
                    PlayerObject player = playerRecord.Value;
                    int newTeam;

                    // Blue (1) and Yellow (3) → Blue (1)
                    // Red (2) and Purple (4) → Red (2)
                    if (player.PlayerTeam == (int)Teams.TEAM_BLUE || player.PlayerTeam == (int)Teams.TEAM_YELLOW)
                    {
                        newTeam = (int)Teams.TEAM_BLUE;
                    }
                    else // Red or Purple
                    {
                        newTeam = (int)Teams.TEAM_RED;
                    }

                    playerInstance.PlayerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = player.PlayerSlot,
                        Team = newTeam
                    });
                }

            }
            // SCENARIO 5: Same team structure (2→2 or 4→4) - no changes needed
            else if (isCurrentMapTeamMap && isNextMapTeamMap)
            {
                return;
            }
        }

        private static int BalanceNewPlayer(bool is4TeamMode)
        {
            if (is4TeamMode)
            {
                // Count players in each of the 4 teams (1, 2, 3, 4)
                int blueCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_BLUE);
                int redCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_RED);
                int yellowCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_YELLOW);
                int purpleCount = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_PURPLE);

                // Find minimum count
                int minCount = new[] { blueCount, redCount, yellowCount, purpleCount }.Min();

                // Assign to smallest team (with random tiebreaker)
                var smallestTeams = new List<int>();
                if (blueCount == minCount) smallestTeams.Add((int)Teams.TEAM_BLUE);
                if (redCount == minCount) smallestTeams.Add((int)Teams.TEAM_RED);
                if (yellowCount == minCount) smallestTeams.Add((int)Teams.TEAM_YELLOW);
                if (purpleCount == minCount) smallestTeams.Add((int)Teams.TEAM_PURPLE);

                Random rand = new Random();
                return smallestTeams[rand.Next(smallestTeams.Count)];
            }
            else
            {
                // 2-team balancing (Blue=1, Red=2)
                int blueteam = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_BLUE);
                int redteam = playerInstance.PlayerPreviousTeamList.Count(p => p.Team == (int)Teams.TEAM_RED);

                if (blueteam > redteam)
                {
                    return (int)Teams.TEAM_RED;
                }
                else if (blueteam < redteam)
                {
                    return (int)Teams.TEAM_BLUE;
                }
                else
                {
                    Random rand = new Random();
                    return rand.Next(1, 3) == 1 ? (int)Teams.TEAM_BLUE : (int)Teams.TEAM_RED;
                }
            }
        }

        private static void ValidateAndNormalizeTeamSwitches(bool nextMapIs4Team)
        {
            if (playerInstance.PlayerChangeTeamList.Count == 0)
                return;

            int removedCount = 0;

            for (int i = playerInstance.PlayerChangeTeamList.Count - 1; i >= 0; i--)
            {
                var teamSwitch = playerInstance.PlayerChangeTeamList[i];

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
                        playerInstance.PlayerChangeTeamList.RemoveAt(i);
                        removedCount++;
                    }
                }
                // If next map is 4-team, validate team is 1-4
                else if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    AppDebug.Log("ValidateTeamSwitches", 
                        $"Invalid team number detected: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}. Removing from queue.");
                    playerInstance.PlayerChangeTeamList.RemoveAt(i);
                    removedCount++;
                }
            }

            if (removedCount > 0)
            {
                AppDebug.Log("ValidateTeamSwitches", 
                    $"Filtered {removedCount} invalid team switch(es). {playerInstance.PlayerChangeTeamList.Count} valid switch(es) remain.");
            }
        }

        // Function UpdatePlayerTeam
        public static void UpdatePlayerTeam()
        {
            HashSet<int> s_fourTeamGameTypes = [1, 3, 8];    
            
            // No Teams Swaps Needed
            if (playerInstance.PlayerChangeTeamList.Count == 0) return;

            // Determine if next map supports 4 teams (safety check)
            bool nextMapIs4Team = theInstance.gameEnableFourTeams && mapInstance.IsNextMap4Team && s_fourTeamGameTypes.Contains(mapInstance.NextMapGameType);

            int playerlistStartingLocation = GetPlayerListBase();

            // Iterate backwards to safely remove items during iteration
            for (int ii = playerInstance.PlayerChangeTeamList.Count - 1; ii >= 0; ii--)
            {
                var teamSwitch = playerInstance.PlayerChangeTeamList[ii];
                
                // SAFETY NET: Final validation before writing to memory
                if (!nextMapIs4Team && (teamSwitch.Team < 1 || teamSwitch.Team > 2))
                {
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                // Apply team switch to game memory
                WriteInt(GetPlayerAddress(teamSwitch.slotNum) + 0x90, teamSwitch.Team);
                
                playerInstance.PlayerChangeTeamList.RemoveAt(ii);
            }
        }

        // Function: WriteMemoryDisarmPlayer
        public static void WriteMemoryDisarmPlayer(int playerSlot) => WriteInt(GetPlayerAddress(playerSlot) + 0xADE08, 0);
        
        // Function: WriteMemoryArmPlayer
        public static void WriteMemoryArmPlayer(int playerSlot) => WriteInt(GetPlayerAddress(playerSlot) + 0xADE08, 1);
        
        // Function: WriteMemoryKillPlayer
        public static void WriteMemoryKillPlayer(int playerSlot)
        {
            int playerObjectLocation = ReadInt(GetPlayerAddress(playerSlot) + 0x11C);
            WriteInt(playerObjectLocation + 0x138, 0);          // Host Killed Identifier
            WriteInt(playerObjectLocation + 0xE2, 0);           // Set Health to 0
        }

        // Function: WriteMemoryTogglePlayerGodMode
        public static void WriteMemoryTogglePlayerGodMode(int playerSlot, int health)
        {
            int playerObjectLocation = ReadInt(GetPlayerAddress(playerSlot) + 0x11C);
            WriteInt(playerObjectLocation + 0x138, 0);          // Host Identifier...
            WriteInt(playerObjectLocation + 0xE2, health);      // Set Health to specified value (e.g. 999 for god mode, 0 for normal)
		}

        // Function: ReadMemoryGeneratePlayerList
        public static void ReadMemoryGeneratePlayerList()
        {
            if (theInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            Dictionary<int, PlayerObject> currentPlayerList = new Dictionary<int, PlayerObject>();
            int NumPlayers = theInstance.gameInfoNumPlayers;

            if (NumPlayers > 0)
            {

                int playerlistStartingLocation = GetPlayerListBase();
                int buffer = 0;
                int failureCount = 0;

                for (int i = 0; i < NumPlayers; i++)
                {
                    if (failureCount == theInstance.gameMaxSlots)
                    {
                        break;
                    }

                    int playerSlot = ReadInt(playerlistStartingLocation + 0xC);

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

                    int playerTeam = ReadInt(playerlistStartingLocation + 0x90);
                    string playerIP = ReadMemoryGrabPlayerIPAddress(formattedPlayerName).ToString();

                    PlayerObject PlayerStats = ReadMemoryPlayerStats(playerSlot);
                    CharacterClass PlayerCharacterClass = (CharacterClass)PlayerStats.RoleID;
                    WeaponStack PlayerSelectedWeapon = (WeaponStack)PlayerStats.SelectedWeaponID;

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
        }

        // Function: ReadMemoryGrabPlayerIPAddress
        public static string ReadMemoryGrabPlayerIPAddress(string playername)
        {


            const int playerIpAddressPointerOffset = 0x00ACE248;
            const int playernameOffset = 0xBC;

            int IPList = ReadInt(baseAddr + playerIpAddressPointerOffset) + playernameOffset;

            int failureCounter = 0;
            while (failureCounter <= theInstance.gameMaxSlots)
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

            if (failureCounter > theInstance.gameMaxSlots)
            {

                return string.Empty;
            }

            int PlayerIPLocation = ReadInt(IPList + 0xA4) + 4;
            byte[] playerIPAddressBytes = new byte[4];
            int playerIPAddressBuffer = 0;
            ReadProcessMemory((int)processHandle, PlayerIPLocation, playerIPAddressBytes, playerIPAddressBytes.Length, ref playerIPAddressBuffer);

            IPAddress playerIp = new IPAddress(playerIPAddressBytes);

            return playerIp.ToString();
        }

        // Function: ReadMemoryPlayerStats
        public static PlayerObject ReadMemoryPlayerStats(int reqslot)
        {


            int beginaddr = GetPlayerAddress(reqslot);

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

            int read_ping_val = ReadInt(beginaddr + 0x000ADB40);

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
                stats[i] = ReadInt(beginaddr + offsets[i]);
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
                stats2[i] = ReadInt(beginaddr + filteredOffsets2[i]);
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

            int read_playerActiveZoneTimeInt = ReadInt(playerMemStart + 0xADB2C);

            int read_playerObject = ReadInt(beginaddr + 0x5E7C);

            int SelectedWeapon = ReadInt(read_playerObject + 0x178);

            int SelectedCharacterClass = ReadInt(read_playerObject + 0x244);
            
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
                PlayerPing = read_ping_val,
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

        // Function: ReadMemoryPlayerLeaningStatus
        // Returns: 0 = upright, 2 = left leaning, 4 = right leaning
        public static int ReadMemoryPlayerLeaningStatus(int playerSlot)
        {
            int playerObjectLocation = ReadInt(GetPlayerAddress(playerSlot) + 0x11C);

            // Read prone/rolling status (2 bytes)
            byte[] proneStatusBytes = new byte[2];
            int proneStatusRead = 0;
            ReadProcessMemory((int)processHandle, playerObjectLocation + 0x164, proneStatusBytes, proneStatusBytes.Length, ref proneStatusRead);
            short proneStatus = BitConverter.ToInt16(proneStatusBytes, 0);

            // Check if player is rolling (95 or 96) - ignore leaning status when rolling
            if (proneStatus == 95 || proneStatus == 96)
            {
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

            return normalizedStatus;
        }

        private static string GetPlayerName(int playerEntityPtr)
        {
            int container = ReadInt(PLAYER_LIST_PTR);
            if (container == 0) return "Unknown";
            int count     = ReadInt(container);
            int playerBaseAddr  = ReadInt(container + 4);
            if (playerBaseAddr == 0 || count <= 0) return "Unknown";

            for (int i = 0; i < count; i++)
            {
                int session = playerBaseAddr + i * PLAYER_STRIDE;
                if (ReadInt(session + PLAYER_OFF_ENTITY) == playerEntityPtr)
                    return ReadString(session + PLAYER_OFF_NAME);
            }
            return "Unknown";
        }

        // Returns the base address of the player session array (header field at container + 0x04).
        private static int GetPlayerListBase()
        {
            int container = ReadInt(PLAYER_LIST_PTR);
            if (container == 0) return 0;
            return ReadInt(container + 4);
        }

        // Returns the start address of the session struct for the given 1-based slot number.
        // Returns 0 when slot/container/base is invalid.
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

	}
}
