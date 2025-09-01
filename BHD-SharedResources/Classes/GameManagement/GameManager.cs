using BHD_SharedResources.Classes.ObjectClasses;

namespace BHD_SharedResources.Classes.GameManagement
{
    public class GameManager
    {
        public static GameInterface Implementation { get; set; }
        public static bool startGame() => Implementation.startGame();
        public static bool stopGame() => Implementation.stopGame();

        // Game Server Memory Management
        public static int getGameTypeID(string gameType) => Implementation.getGameTypeID(gameType);
        public static void UpdatePlayerHostName() => Implementation.UpdatePlayerHostName();
        public static void UpdateMapListCount() => Implementation.UpdateMapListCount();
        public static void UpdateAllowCustomSkins() => Implementation.UpdateAllowCustomSkins();
        public static void UpdateDestroyBuildings() => Implementation.UpdateDestroyBuildings();
        public static void UpdateFatBullets() => Implementation.UpdateFatBullets();
        public static void UpdateFlagReturnTime() => Implementation.UpdateFlagReturnTime();
        public static void UpdateMinPing() => Implementation.UpdateMinPing();
        public static void UpdateMinPingValue() => Implementation.UpdateMinPingValue();
        public static void UpdateMaxPing() => Implementation.UpdateMaxPing();
        public static void UpdateMaxPingValue() => Implementation.UpdateMaxPingValue();
        public static void UpdateMaxTeamLives() => Implementation.UpdateMaxTeamLives();
        public static void UpdateOneShotKills() => Implementation.UpdateOneShotKills();
        public static void UpdatePSPTakeOverTime() => Implementation.UpdatePSPTakeOverTime();
        public static void UpdateRequireNovaLogin() => Implementation.UpdateRequireNovaLogin();
        public static void UpdateRespawnTime() => Implementation.UpdateRespawnTime();
        public static void UpdateWeaponRestrictions() => Implementation.UpdateWeaponRestrictions();
        public static void UpdateGamePlayOptions() => Implementation.UpdateGamePlayOptions();
        public static void UpdateServerName() => Implementation.UpdateServerName();
        public static void UpdateMOTD() => Implementation.UpdateMOTD();
        public static void UpdateTimeLimit() => Implementation.UpdateTimeLimit();
        public static void UpdateLoopMaps() => Implementation.UpdateLoopMaps();
        public static void UpdateStartDelay() => Implementation.UpdateStartDelay();
        public static void UpdateMaxSlots() => Implementation.UpdateMaxSlots();
        public static void UpdateFriendlyFireKills() => Implementation.UpdateFriendlyFireKills();
        public static void UpdateBluePassword() => Implementation.UpdateBluePassword();
        public static void UpdateRedPassword() => Implementation.UpdateRedPassword();
        public static void UpdateMapCycle1() => Implementation.UpdateMapCycle1();
        public static void UpdateMapCycle2() => Implementation.UpdateMapCycle2();
        public static void UpdateSecondaryMapList() => Implementation.UpdateSecondaryMapList();
        public static void UpdateNovaID() => Implementation.UpdateNovaID();
        public static void UpdateGlobalGameType() => Implementation.UpdateGlobalGameType();
        public static void UpdateMapCycleCounter() => Implementation.UpdateMapCycleCounter();
        public static void UpdateScoreBoardTimer() => Implementation.UpdateScoreBoardTimer();
        public static void UpdateNextMapGameType() => Implementation.UpdateNextMapGameType();
        public static void UpdateGameScores() => Implementation.UpdateGameScores();
        public static void UpdatePlayerTeam() => Implementation.UpdatePlayerTeam();
        public static void UpdateNextMap(int NextMapIndex) => Implementation.UpdateNextMap(NextMapIndex);
        public static void WriteMemoryScoreMap() => Implementation.WriteMemoryScoreMap();
        public static void WriteMemorySendChatMessage(int MsgLocation, string Msg) => Implementation.WriteMemorySendChatMessage(MsgLocation, Msg);
        public static void WriteMemoryChatCountDownKiller(int ChatLogAddr) => Implementation.WriteMemoryChatCountDownKiller(ChatLogAddr);
        public static void WriteMemoryDisarmPlayer(int playerSlot) => Implementation.WriteMemoryDisarmPlayer(playerSlot);
        public static void WriteMemoryArmPlayer(int playerSlot) => Implementation.WriteMemoryArmPlayer(playerSlot);
        public static void WriteMemoryKillPlayer(int playerSlot) => Implementation.WriteMemoryKillPlayer(playerSlot);
        public static void WriteMemoryTogglePlayerGodMode(int playerSlot, int health) => Implementation.WriteMemoryTogglePlayerGodMode(playerSlot, health);
        public static void WriteMemorySendConsoleCommand(string Command) => Implementation.WriteMemorySendConsoleCommand(Command);

        // Read memory methods
        public static bool ReadMemoryIsProcessAttached() => Implementation.ReadMemoryIsProcessAttached();
        public static void ReadMemoryServerStatus() => Implementation.ReadMemoryServerStatus();
        public static void ReadMemoryGameTimeLeft() => Implementation.ReadMemoryGameTimeLeft();
        public static void ReadMemoryCurrentMissionName() => Implementation.ReadMemoryCurrentMissionName();
        public static void ReadMemoryCurrentGameType() => Implementation.ReadMemoryCurrentGameType();
        public static void ReadMemoryCurrentMapIndex() => Implementation.ReadMemoryCurrentMapIndex();
        public static void ReadMemoryWinningTeam() => Implementation.ReadMemoryWinningTeam();
        public static void ReadMemoryCurrentNumPlayers() => Implementation.ReadMemoryCurrentNumPlayers();
        public static void ReadMemoryGeneratePlayerList() => Implementation.ReadMemoryGeneratePlayerList();
        public static string ReadMemoryGrabPlayerIPAddress(string playername) => Implementation.ReadMemoryGrabPlayerIPAddress(playername);
        public static playerObject ReadMemoryPlayerStats(int reqslot) => Implementation.ReadMemoryPlayerStats(reqslot);
        public static string[] ReadMemoryLastChatMessage() => Implementation.ReadMemoryLastChatMessage();

    }
}
