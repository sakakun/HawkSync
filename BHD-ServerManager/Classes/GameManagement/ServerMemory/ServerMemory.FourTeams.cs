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

        // --- Flag scorer integration -------------------------------------------------
        // Bay XY positions taken directly from the .mis file (static map objects — never move):
        //   item 9:  type_id 1481 (Violet crate, team 4) position (-51386276, 15723288, -65536)
        //   item 10: type_id 2147 (Yellow crate, team 3) position (-49278408, 17839658, -65536)
        // Indexed by team number (indices 3 and 4 used; 0–2 unused).
        private static readonly (long X, long Y)[] FB_BAY_POS =
        {
            (0L, 0L),                       // [0] unused
            (0L, 0L),                       // [1] team 1 (handled natively)
            (0L, 0L),                       // [2] team 2 (handled natively)
            (-49278408L, 17839658L),        // [3] Yellow  – item 10, type_id 2147
            (-51386276L, 15723288L),        // [4] Violet  – item 9,  type_id 1481
        };

        // data_9e4728 timer list (second list: ptr at +0x18, count at +0x1c)
        private const int PSP_TIMER_LIST_PTR   = 0x009E4740; // value = heap ptr to array
        private const int PSP_TIMER_COUNT_ADDR = 0x009E4744; // count of active entries
        private const int PSP_ENTRY_STRIDE     = 0x14;
        private const int PSP_ENTRY_SPAWN      = 0x00;  // s[0] = PSP spawn entity ptr (index → letter A, B, …)
        private const int PSP_ENTRY_TEAM       = 0x04;  // s[1] = capturing player's team (1–4)
        private const int PSP_ENTRY_PLAYER     = 0x10;  // s[4] = player entity ptr in data_715900

        private const int ENTITY_ARRAY_BASE    = 0x00715900;
        private const int ENTITY_STRIDE        = 0x29c;

        // Entity world-position offsets (player heap entity — verified via sub_472650 arg1 layout)
        private const int ENTITY_POS_X         = 0x08;  // entity world-space X
        private const int ENTITY_POS_Y         = 0x0c;  // entity world-space Y

        // Squared proximity threshold (game units).  200 000-unit radius ≈ 20 m.
        // The two bays are ~3 000 000 units apart — no cross-team false positives.
        private const long FB_SCORE_RADIUS_SQ  = 40_000_000_000L;
        private static readonly int[] TEAM_SCORE_OFFSETS = new[] { 0x5E0800, 0x5E08E8, 0x5E09D0, 0x5E0AB8 };
        private const int ENTITY_TYPEPTR_OFF   = 0x24;
        private const int TYPEDEF_TYPEID_OFF   = 0x30;
        private const int SCORE_COOLDOWN_MS    = 5000;
        // Flag-carry entity field offsets (confirmed from sub_43d310/sub_43d420/sub_43e3d0)
        private const int ENTITY_CARRY_SLOT    = 0x174; // playerEntity+0x174 = ptr to carried flag entity
        private const int FLAG_CARRIER_LINK    = 0x134; // flagEntity+0x134 = ptr to carrying player (= 0 when free)
        private const int ENTITY_FLAGS_OFF     = 0x20;  // bitfield; bit 0 = live-player, bit 1 = "being carried"
        private const int FLAG_SPAWNBAY_LINK   = 0x38;  // flagEntity+0x38 = permanent ptr to spawn-bay entity
        // Spawn-bay entity stores the flag's original position at these offsets
        private const int SPAWNBAY_POS_X       = 0x6c;
        private const int SPAWNBAY_POS_Y       = 0x70;
        private const int SPAWNBAY_POS_Z       = 0x74;
        private const int SPAWNBAY_ORIENT      = 0x78;

        // Per-player score cooldown (keyed on player entity ptr)
        private static readonly Dictionary<int, DateTime> _flagScorerLastScored = new();

        // PSP label cache: maps the contesting-player key → PSP letter captured during that contest
        private static readonly Dictionary<string, string> _pspLabels = new();


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
                int numPlayers = thisInstance.gameInfoNumPlayers;
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
                        if (thisInstance._flagCarriers.TryGetValue(key, out var carrierInfo))
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
                    if (!thisInstance._flagCarriers.ContainsKey(key))
                    {
                        string name = GetPlayerName(playerEntPtr);
                        thisInstance._flagCarriers[key] = name;
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

                    string scorerName = thisInstance._flagCarriers[key];
                    AppDebug.Log("FlagScorer", $"SCORED: slot={pi} team={team} score {oldScore}\u2192{oldScore + 1}");
                    WriteMemorySendChatMessage(8,
                        $"** {scorerName} ({teamName}) scored! Score: {oldScore + 1} **");

                    ReturnCarriedFlag(playerEntPtr);
                }

                // Clean up carriers who are no longer carrying (natural drop, not from death)
                foreach (var key in thisInstance._flagCarriers.Keys.Where(k => !currentCarriers.Contains(k)).ToList())
                {
                    thisInstance._flagCarriers.Remove(key);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("TickFlagScorer", "Error: " + ex);
            }
        }
        /// <summary>
        ///  Polls the current state of all Player Spawn Points (PSPs) and updates contesting and capture notifications
        /// accordingly.
        /// </summary>
        /// <remarks>This method should be called periodically to ensure that player actions related to
        /// PSPs are detected and appropriate chat messages are sent. It manages the internal state for players
        /// contesting or capturing PSPs and triggers notifications when a PSP is being taken or has been captured. This
        /// method is not thread-safe and should be called from the main game loop or a synchronized context.</remarks>
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
                    int team         = ReadInt(entryAddr + PSP_ENTRY_TEAM);   // s[1] = capturing player's team
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

                    if (!thisInstance._contesting.ContainsKey(key))
                    {
                        string name = GetPlayerName(playerEntPtr);
                        thisInstance._contesting[key] = name;
                        _pspLabels[key] = spawnLabel;
                        // Max ~40 chars: "** <15> taking PSP A! (YLW) **"
                        WriteMemorySendChatMessage(8,
                            $"** {name} taking PSP {spawnLabel}! ({teamName}) **");
                    }
                }
            }

            foreach (var key in thisInstance._contesting.Keys.Where(k => !currentKeys.Contains(k)).ToList())
            {
                string name  = thisInstance._contesting[key];
                string label = _pspLabels.TryGetValue(key, out var l) ? l : "?";
                // Max ~38 chars: "** <15> captured PSP A! **"
                WriteMemorySendChatMessage(8, $"** {name} captured PSP {label}! **");
                thisInstance._contesting.Remove(key);
                _pspLabels.Remove(key);
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
