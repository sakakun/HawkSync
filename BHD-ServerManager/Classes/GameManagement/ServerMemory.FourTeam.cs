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

        public static void PollPspState()
        {
            int count   = ReadInt(PSP_TIMER_COUNT_ADDR);
            int listPtr = ReadInt(PSP_TIMER_LIST_PTR);

            var currentKeys = new HashSet<string>();

            if (count > 0 && listPtr != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    int entryAddr    = listPtr + i * PSP_ENTRY_STRIDE;
                    int spawnPtr     = ReadInt(entryAddr + PSP_ENTRY_SPAWN);  // s[0] = PSP entity ptr
                    int team         = ReadInt(entryAddr + PSP_ENTRY_TEAM);   // s[1] = player's team
                    int playerEntPtr = ReadInt(entryAddr + PSP_ENTRY_PLAYER); // s[4] = player entity ptr

                    if (team != 3 && team != 4) continue;
                    if (playerEntPtr == 0) continue;

                    // Derive PSP letter from entity array index (A, B, C, …)
                    string spawnLabel = "?";
                    if (spawnPtr != 0 && spawnPtr >= ENTITY_ARRAY_BASE)
                    {
                        int idx = (spawnPtr - ENTITY_ARRAY_BASE) / ENTITY_STRIDE;
                        spawnLabel = ((char)('A' + (idx % 26))).ToString();
                    }

                    string key      = $"e{playerEntPtr:X8}";
                    string teamName = team == 3 ? "YLW" : "VLT";

                    currentKeys.Add(key);

                    if (!theInstance._contesting.ContainsKey(key))
                    {
                        string name = GetPlayerName(playerEntPtr);
                        theInstance._contesting[key] = name;
                        _pspLabels[key] = spawnLabel;
                        // Max ~40 chars: "** <15> taking PSP A! (YLW) **"
                        WriteMemorySendChatMessage(8,
                            $"** {name} taking PSP {spawnLabel}! ({teamName}) **");
                    }
                }
            }

            foreach (var key in theInstance._contesting.Keys.Where(k => !currentKeys.Contains(k)).ToList())
            {
                string name  = theInstance._contesting[key];
                string label = _pspLabels.TryGetValue(key, out var l) ? l : "?";
                // Max ~38 chars: "** <15> captured PSP A! **"
                WriteMemorySendChatMessage(8, $"** {name} captured PSP {label}! **");
                theInstance._contesting.Remove(key);
                _pspLabels.Remove(key);
            }
        }

        // Called every ticker loop.  Detects when a team-3/4 player carrying the flagball
        // is within scoring range of their team's return bay and manually triggers the score.
        public static void TickFlagScorer()
        {
            try
            {
                if (CommonCore.instanceMaps!.CurrentGameType != 8) return;

                // Exact same two-level pointer chain as ReadMemoryGeneratePlayerList:
                //   *(0x9ED600) = outer;  *(outer + 0x28) = sessions base
                int outer    = ReadInt(PLAYER_LIST_PTR);
                if (outer == 0) return;
                int sessBase = ReadInt(outer + 0x28);
                if (sessBase == 0) return;
                int numPlayers = theInstance.gameInfoNumPlayers;
                if (numPlayers <= 0) return;

                var currentCarriers = new HashSet<string>();

                for (int pi = 0; pi < numPlayers; pi++)
                {
                    int session = sessBase + pi * PLAYER_STRIDE;

                    // Team at session+0x90 — verified by ReadMemoryGeneratePlayerList / UpdatePlayerTeam
                    int team = ReadInt(session + 0x90);
                    if (team != 3 && team != 4) continue;

                    // Entity ptr at session+0x18 (PLAYER_OFF_ENTITY) — points into data_715900
                    // (verified: GetPlayerName matches these against PSP entry entity ptrs)
                    int playerEntPtr = ReadInt(session + PLAYER_OFF_ENTITY);
                    if (playerEntPtr == 0)
                    {
                        AppDebug.Log("FlagScorer", $"slot={pi} team={team}: entPtr=0, skip");
                        continue;
                    }

                    // entity+0x20 bit 0 = live player (sub_472650)
                    int liveFlags = ReadInt(playerEntPtr + ENTITY_FLAGS_OFF);
                    bool isAlive = (liveFlags & 1) != 0;

                    // entity+0x174 = carried item ptr
                    int flagItemPtr = ReadInt(playerEntPtr + ENTITY_CARRY_SLOT);

                    string key = $"e{playerEntPtr:X8}";
                    string teamName = team == 3 ? "Yellow" : "Violet";

                    if (flagItemPtr == 0)
                    {
                        // Check if player was carrying flag and died (dropped it)
                        if (theInstance._flagCarriers.TryGetValue(key, out var carrierInfo))
                        {
                            if (!isAlive)
                            {
                                WriteMemorySendChatMessage(8,
                                    $"** {carrierInfo} dropped the flag (killed)! **");
                            }
                        }
                        AppDebug.Log("FlagScorer", $"slot={pi} team={team} ent=0x{playerEntPtr:X8}: carry=0 (not holding flag)");
                        continue;
                    }

                    if (!isAlive)
                    {
                        AppDebug.Log("FlagScorer", $"slot={pi} team={team} ent=0x{playerEntPtr:X8}: not alive (flags=0x{liveFlags:X})");
                        continue;
                    }

                    int flagTypePtr = ReadInt(flagItemPtr + ENTITY_TYPEPTR_OFF);
                    int flagTypeId  = (flagTypePtr != 0) ? ReadInt(flagTypePtr + TYPEDEF_TYPEID_OFF) : 0;
                    if (flagTypeId != 0xfff)
                    {
                        AppDebug.Log("FlagScorer", $"slot={pi} team={team}: carrying typeId={flagTypeId} (need 4095)");
                        continue;
                    }

                    // Player is carrying the flag
                    currentCarriers.Add(key);

                    // Check if this is a new pickup
                    if (!theInstance._flagCarriers.ContainsKey(key))
                    {
                        string name = GetPlayerName(playerEntPtr);
                        theInstance._flagCarriers[key] = name;
                        WriteMemorySendChatMessage(8,
                            $"** {name} ({teamName}) picked up flag! **");
                    }

                    if (_flagScorerLastScored.TryGetValue(playerEntPtr, out var last) &&
                        (DateTime.UtcNow - last).TotalMilliseconds < SCORE_COOLDOWN_MS)
                        continue;

                    // Look up this team's bay position (hardcoded from the .mis file)
                    if (team < 3 || team > 4) continue;
                    long bx = FB_BAY_POS[team].X;
                    long by = FB_BAY_POS[team].Y;

                    long px = ReadInt(playerEntPtr + ENTITY_POS_X);
                    long py = ReadInt(playerEntPtr + ENTITY_POS_Y);
                    long distSq = (px - bx) * (px - bx) + (py - by) * (py - by);

                    AppDebug.Log("FlagScorer",
                        $"slot={pi} team={team} player=({px},{py}) bay=({bx},{by}) distSq={distSq} threshold={FB_SCORE_RADIUS_SQ}");

                    if (distSq > FB_SCORE_RADIUS_SQ) continue;

                    int scoreAddr = baseAddr + TEAM_SCORE_OFFSETS[team - 1];
                    int oldScore  = ReadInt(scoreAddr);
                    WriteInt(scoreAddr, oldScore + 1);
                    _flagScorerLastScored[playerEntPtr] = DateTime.UtcNow;

                    string scorerName = theInstance._flagCarriers[key];
                    AppDebug.Log("FlagScorer", $"SCORED: slot={pi} team={team} score {oldScore}\u2192{oldScore + 1}");
                    WriteMemorySendChatMessage(8,
                        $"** {scorerName} ({teamName}) scored! Score: {oldScore + 1} **");

                    ReturnCarriedFlag(playerEntPtr);
                }

                // Clean up carriers who are no longer carrying (natural drop, not from death)
                foreach (var key in theInstance._flagCarriers.Keys.Where(k => !currentCarriers.Contains(k)).ToList())
                {
                    theInstance._flagCarriers.Remove(key);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("TickFlagScorer", "Error: " + ex);
            }
        }

        // Replicates sub_43d420(playerEntity) + sub_43e3d0(flagItem):
        //   - Unlinks the carried flag from the player
        //   - Teleports the flag entity back to its spawn-bay position
        private static void ReturnCarriedFlag(int playerEntPtr)
        {
            // Read the flag item the player is carrying (playerEntity+0x174)
            int flagItemPtr = ReadInt(playerEntPtr + ENTITY_CARRY_SLOT);
            if (flagItemPtr == 0)
                return; // player is not carrying a flag

            // sub_43d420: unlink flag from player, clear carry-state bits
            WriteInt(playerEntPtr + ENTITY_CARRY_SLOT, 0);     // clear player's carry slot
            WriteInt(flagItemPtr + FLAG_CARRIER_LINK, 0);       // clear flag's "carrier" backlink
            int flags = ReadInt(flagItemPtr + ENTITY_FLAGS_OFF);
            WriteInt(flagItemPtr + ENTITY_FLAGS_OFF, flags & ~0x12); // clear "carried" bits (2 | 0x10)

            // sub_43e3d0: teleport flag to its spawn-bay position
            int spawnBay = ReadInt(flagItemPtr + FLAG_SPAWNBAY_LINK);
            if (spawnBay != 0)
            {
                WriteInt(flagItemPtr + 0x08, ReadInt(spawnBay + SPAWNBAY_POS_X));
                WriteInt(flagItemPtr + 0x0c, ReadInt(spawnBay + SPAWNBAY_POS_Y));
                WriteInt(flagItemPtr + 0x10, ReadInt(spawnBay + SPAWNBAY_POS_Z));
                WriteInt(flagItemPtr + 0x14, ReadInt(spawnBay + SPAWNBAY_ORIENT));
            }

            AppDebug.Log("FlagScorer",
                $"FlagReturn: player=0x{playerEntPtr:X8} flag=0x{flagItemPtr:X8} spawnBay=0x{spawnBay:X8}");
        }

	}
}
