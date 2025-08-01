using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.ObjectClasses;

namespace BHD_ServerManager.Classes.GameManagement
{
    public class serverGameManager : GameInterface
    {
        public bool startGame() => GameManagement.StartServer.startGame();
        public bool stopGame() => GameManagement.StartServer.stopGame();


        // Game Server Memory Management
        public int getGameTypeID(string gameType) => ServerMemory.getGameTypeID(gameType);
        public void UpdatePlayerHostName() => ServerMemory.UpdatePlayerHostName();
        public void UpdateMapListCount() => ServerMemory.UpdateMapListCount();
        public void UpdateAllowCustomSkins() => ServerMemory.UpdateAllowCustomSkins();
        public void UpdateDestroyBuildings() => ServerMemory.UpdateDestroyBuildings();
        public void UpdateFatBullets() => ServerMemory.UpdateFatBullets();
        public void UpdateFlagReturnTime() => ServerMemory.UpdateFlagReturnTime();
        public void UpdateMinPing() => ServerMemory.UpdateMinPing();
        public void UpdateMinPingValue() => ServerMemory.UpdateMinPingValue();
        public void UpdateMaxPing() => ServerMemory.UpdateMaxPing();
        public void UpdateMaxPingValue() => ServerMemory.UpdateMaxPingValue();
        public void UpdateMaxTeamLives() => ServerMemory.UpdateMaxTeamLives();
        public void UpdateOneShotKills() => ServerMemory.UpdateOneShotKills();
        public void UpdatePSPTakeOverTime() => ServerMemory.UpdatePSPTakeOverTime();
        public void UpdateRequireNovaLogin() => ServerMemory.UpdateRequireNovaLogin();
        public void UpdateRespawnTime() => ServerMemory.UpdateRespawnTime();
        public void UpdateWeaponRestrictions() => ServerMemory.UpdateWeaponRestrictions();
        public void UpdateGamePlayOptions() => ServerMemory.UpdateGamePlayOptions();
        public void UpdateServerName() => ServerMemory.UpdateServerName();
        public void UpdateMOTD() => ServerMemory.UpdateMOTD();
        public void UpdateTimeLimit() => ServerMemory.UpdateTimeLimit();
        public void UpdateLoopMaps() => ServerMemory.UpdateLoopMaps();
        public void UpdateStartDelay() => ServerMemory.UpdateStartDelay();
        public void UpdateMaxSlots() => ServerMemory.UpdateMaxSlots();
        public void UpdateFriendlyFireKills() => ServerMemory.UpdateFriendlyFireKills();
        public void UpdateBluePassword() => ServerMemory.UpdateBluePassword();
        public void UpdateRedPassword() => ServerMemory.UpdateRedPassword();
        public void UpdateMapCycle1() => ServerMemory.UpdateMapCycle1();
        public void UpdateMapCycle2() => ServerMemory.UpdateMapCycle2();
        public void UpdateSecondaryMapList() => ServerMemory.UpdateSecondaryMapList();
        public void UpdateNovaID() => ServerMemory.UpdateNovaID();
        public void UpdateGlobalGameType() => ServerMemory.UpdateGlobalGameType();
        public void UpdateMapCycleCounter() => ServerMemory.UpdateMapCycleCounter();
        public void UpdateScoreBoardTimer() => ServerMemory.UpdateScoreBoardTimer();
        public void UpdateNextMapGameType() => ServerMemory.UpdateNextMapGameType();
        public void UpdateGameScores() => ServerMemory.UpdateGameScores();
        public void UpdatePlayerTeam() => ServerMemory.UpdatePlayerTeam();
        public void UpdateNextMap(int NextMapIndex) => ServerMemory.UpdateNextMap(NextMapIndex);
        public void WriteMemoryScoreMap() => ServerMemory.WriteMemoryScoreMap();
        public void WriteMemorySendChatMessage(int MsgLocation, string Msg) => ServerMemory.WriteMemorySendChatMessage(MsgLocation, Msg);
        public void WriteMemoryChatCountDownKiller(int ChatLogAddr) => ServerMemory.WriteMemoryChatCountDownKiller(ChatLogAddr);
        public void WriteMemoryDisarmPlayer(int playerSlot) => ServerMemory.WriteMemoryDisarmPlayer(playerSlot);
        public void WriteMemoryArmPlayer(int playerSlot) => ServerMemory.WriteMemoryArmPlayer(playerSlot);
        public void WriteMemoryKillPlayer(int playerSlot) => ServerMemory.WriteMemoryKillPlayer(playerSlot);
        public void WriteMemoryTogglePlayerGodMode(int playerSlot, int health) => ServerMemory.WriteMemoryTogglePlayerGodMode(playerSlot, health);
        public void WriteMemorySendConsoleCommand(string Command) => ServerMemory.WriteMemorySendConsoleCommand(Command);

        // Read memory methods
        public bool ReadMemoryIsProcessAttached() => ServerMemory.ReadMemoryIsProcessAttached();
        public void ReadMemoryServerStatus() => ServerMemory.ReadMemoryServerStatus();
        public void ReadMemoryGameTimeLeft() => ServerMemory.ReadMemoryGameTimeLeft();
        public void ReadMemoryCurrentMissionName() => ServerMemory.ReadMemoryCurrentMissionName();
        public void ReadMemoryCurrentGameType() => ServerMemory.ReadMemoryCurrentGameType();
        public void ReadMemoryCurrentMapIndex() => ServerMemory.ReadMemoryCurrentMapIndex();
        public void ReadMemoryWinningTeam() => ServerMemory.ReadMemoryWinningTeam();
        public void ReadMemoryCurrentNumPlayers() => ServerMemory.ReadMemoryCurrentNumPlayers();
        public void ReadMemoryGeneratePlayerList() => ServerMemory.ReadMemoryGeneratePlayerList();
        public string ReadMemoryGrabPlayerIPAddress(string playername) => ServerMemory.ReadMemoryGrabPlayerIPAddress(playername);
        public playerObject ReadMemoryPlayerStats(int reqslot) => ServerMemory.ReadMemoryPlayerStats(reqslot);
        public string[] ReadMemoryLastChatMessage() => ServerMemory.ReadMemoryLastChatMessage();

    }
}
