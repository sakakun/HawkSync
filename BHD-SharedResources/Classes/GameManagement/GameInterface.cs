using BHD_SharedResources.Classes.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_SharedResources.Classes.GameManagement
{
    public interface GameInterface
    {
        bool startGame();
        bool stopGame();

        // Game Server Memory Management
        int getGameTypeID(string gameType);
        void UpdatePlayerHostName();
        void UpdateMapListCount();
        void UpdateAllowCustomSkins();
        void UpdateDestroyBuildings();
        void UpdateFatBullets();
        void UpdateFlagReturnTime();
        void UpdateMinPing();
        void UpdateMinPingValue();
        void UpdateMaxPing();
        void UpdateMaxPingValue();
        void UpdateMaxTeamLives();
        void UpdateOneShotKills();
        void UpdatePSPTakeOverTime();
        void UpdateRequireNovaLogin();
        void UpdateRespawnTime();
        void UpdateWeaponRestrictions();
        void UpdateGamePlayOptions();
        void UpdateServerName();
        void UpdateMOTD();
        void UpdateTimeLimit();
        void UpdateLoopMaps();
        void UpdateStartDelay();
        void UpdateMaxSlots();
        void UpdateFriendlyFireKills();
        void UpdateBluePassword();
        void UpdateRedPassword();
        void UpdateMapCycle1();
        void UpdateMapCycle2();
        void UpdateSecondaryMapList();
        void UpdateNovaID();
        void UpdateGlobalGameType();
        void UpdateMapCycleCounter();
        void UpdateScoreBoardTimer();
        void UpdateNextMapGameType();
        void UpdateGameScores();
        void UpdatePlayerTeam();
        void UpdateNextMap(int NextMapIndex);
        void WriteMemoryScoreMap();
        void WriteMemorySendChatMessage(int MsgLocation, string Msg);
        void WriteMemoryChatCountDownKiller(int ChatLogAddr);
        void WriteMemoryDisarmPlayer(int playerSlot);
        void WriteMemoryArmPlayer(int playerSlot);
        void WriteMemoryKillPlayer(int playerSlot);
        void WriteMemoryTogglePlayerGodMode(int playerSlot, int health);
        void WriteMemorySendConsoleCommand(string Command);

        // Read memory methods
        bool ReadMemoryIsProcessAttached();
        void ReadMemoryServerStatus();
        void ReadMemoryGameTimeLeft();
        void ReadMemoryCurrentMissionName();
        void ReadMemoryCurrentGameType();
        void ReadMemoryCurrentMapIndex();
        void ReadMemoryWinningTeam();
        void ReadMemoryCurrentNumPlayers();
        void ReadMemoryGeneratePlayerList();
        string ReadMemoryGrabPlayerIPAddress(string playername);
        playerObject ReadMemoryPlayerStats(int reqslot);
        string[] ReadMemoryLastChatMessage();
    }
}
