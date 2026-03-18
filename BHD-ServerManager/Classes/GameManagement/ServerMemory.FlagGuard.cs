using System;
using System.Collections.Generic;
using HawkSyncShared.SupportClasses;

namespace BHD_ServerManager.Classes.GameManagement
{
    public static partial class ServerMemory
    {
        private const int FLAG_RETURN_TIMER_RUNTIME_ADDR = 0x009F21E4;
        private const int FLAG_RETURN_TIMER_CONFIG_ADDR = 0x00A344A4;
        private const int FLAG_TIMER_OFFSET = 0x168;
        private const int FLAG_HOLD_FOREVER_VALUE = 0x3FFFFFFF;
        private const int PLAYER_TEAM_OFFSET = 0x90;

        private static readonly HashSet<int> _flagTimerGuardSeen = new();

        public static void TickFlagTimerGuard()
        {
			// Ignore if not in Hide and Seek or if Hide and Seek is disabled, since the flag timer only applies to that mode
			if (!mapInstance.IsCurrentMapHideAndSeek || !theInstance.gameEnableHideandSeek)
                return;

            try
            {
                if (ReadInt(FLAG_RETURN_TIMER_RUNTIME_ADDR) != FLAG_HOLD_FOREVER_VALUE)
                    WriteInt(FLAG_RETURN_TIMER_RUNTIME_ADDR, FLAG_HOLD_FOREVER_VALUE);

                if (ReadInt(FLAG_RETURN_TIMER_CONFIG_ADDR) != FLAG_HOLD_FOREVER_VALUE)
                    WriteInt(FLAG_RETURN_TIMER_CONFIG_ADDR, FLAG_HOLD_FOREVER_VALUE);

                int container = ReadInt(PLAYER_LIST_PTR);
                if (container == 0)
                    return;

                int numPlayers = ReadInt(container);
                int sessBase = ReadInt(container + 0x04);
                if (sessBase == 0 || numPlayers <= 0)
                    return;

                var activeFlagPtrs = new HashSet<int>();

                for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
                {
                    int session = sessBase + playerIndex * PLAYER_STRIDE;
                    int team = ReadInt(session + PLAYER_TEAM_OFFSET);
                    if (team != 3 && team != 4)
                        continue;

                    int playerEntPtr = ReadInt(session + PLAYER_OFF_ENTITY);
                    if (playerEntPtr == 0)
                        continue;

                    int liveFlags = ReadInt(playerEntPtr + ENTITY_FLAGS_OFF);
                    if ((liveFlags & 1) == 0)
                        continue;

                    int flagItemPtr = ReadInt(playerEntPtr + ENTITY_CARRY_SLOT);
                    if (flagItemPtr == 0)
                        continue;

                    int flagTypePtr = ReadInt(flagItemPtr + ENTITY_TYPEPTR_OFF);
                    int flagTypeId = flagTypePtr != 0 ? ReadInt(flagTypePtr + TYPEDEF_TYPEID_OFF) : 0;
                    if (flagTypeId != 0xFFF)
                        continue;

                    activeFlagPtrs.Add(flagItemPtr);

                    int timerAddr = flagItemPtr + FLAG_TIMER_OFFSET;
                    int timerValue = ReadInt(timerAddr);
                    if (timerValue <= 0 || timerValue < 0x10000000)
                    {
                        WriteInt(timerAddr, FLAG_HOLD_FOREVER_VALUE);

                        if (_flagTimerGuardSeen.Add(flagItemPtr))
                        {
                            AppDebug.Log(
                                "FlagTimerGuard",
                                $"Pinned flag timer: player=0x{playerEntPtr:X8} flag=0x{flagItemPtr:X8} old=0x{timerValue:X8} new=0x{FLAG_HOLD_FOREVER_VALUE:X8}");
                        }
                    }
                }

                if (_flagTimerGuardSeen.Count == 0)
                    return;

                var staleFlagPtrs = new List<int>();
                foreach (int ptr in _flagTimerGuardSeen)
                {
                    if (!activeFlagPtrs.Contains(ptr))
                        staleFlagPtrs.Add(ptr);
                }

                foreach (int ptr in staleFlagPtrs)
                    _flagTimerGuardSeen.Remove(ptr);
            }
            catch (Exception ex)
            {
                AppDebug.Log("TickFlagTimerGuard", "Error: " + ex);
            }
        }
    }
}