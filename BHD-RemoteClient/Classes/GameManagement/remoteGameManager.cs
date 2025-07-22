using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.GameManagement;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.GameManagement
{
    public class remoteGameManager : GameInterface
    {
        void GameInterface.attachProcess()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        int GameInterface.getGameTypeID(string gameType)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return 0;
        }

        void GameInterface.ReadMemoryCurrentGameType()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryCurrentMapIndex()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryCurrentMissionName()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryCurrentNumPlayers()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryGameTimeLeft()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryGeneratePlayerList()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        string GameInterface.ReadMemoryGrabPlayerIPAddress(string playername)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return string.Empty;
        }

        bool GameInterface.ReadMemoryIsProcessAttached() => CmdReadMemoryIsProcessAttached.ProcessCommand();

        string[] GameInterface.ReadMemoryLastChatMessage()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return new string[] { };
        }

        playerObject GameInterface.ReadMemoryPlayerStats(int reqslot)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
            return new playerObject();
        }

        void GameInterface.ReadMemoryServerStatus()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.ReadMemoryWinningTeam()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        bool GameInterface.startGame() => CmdStartGame.ProcessCommand();
        bool GameInterface.stopGame() => CmdStopGame.ProcessCommand();

        void GameInterface.UpdateAllowCustomSkins()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateBluePassword()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateDestroyBuildings()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateFatBullets()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateFlagReturnTime()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateFriendlyFireKills()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateGamePlayOptions()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateGameScores()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateGlobalGameType()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateLoopMaps()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMapCycle1()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMapCycle2()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMapCycleCounter()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMapListCount()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMaxPing()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMaxPingValue()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMaxSlots()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMaxTeamLives()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMinPing()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMinPingValue()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateMOTD()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateNextMap(int NextMapIndex) => CmdUpdateNextMap.ProcessCommand(NextMapIndex);

        void GameInterface.UpdateNextMapGameType()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateNovaID()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateOneShotKills()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdatePlayerHostName()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdatePlayerTeam()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdatePSPTakeOverTime()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateRedPassword()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateRequireNovaLogin()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateRespawnTime()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateScoreBoardTimer()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateSecondaryMapList()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateServerName()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateStartDelay()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateTimeLimit()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        void GameInterface.UpdateWeaponRestrictions()
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void WriteMemoryArmPlayer(int playerSlot) => CmdRearmplayer.ProcessCommand(playerSlot);

        void GameInterface.WriteMemoryChatCountDownKiller(int ChatLogAddr)
        {
            AppDebug.Log("Not Implemented", "Not Implemented");
        }

        public void WriteMemoryDisarmPlayer(int playerSlot) => CmdDisarmplayer.ProcessCommand(playerSlot);

        public void WriteMemoryKillPlayer(int playerSlot) => CmdKillPlayer.ProcessCommand(playerSlot);

        public void WriteMemoryScoreMap() => CmdMapScore.ProcessCommand();

        public void WriteMemorySendChatMessage(int MsgLocation, string Msg) => CmdSendChatMessage.ProcessCommand(MsgLocation, Msg);

        public void WriteMemorySendConsoleCommand(string Command) => CmdSendConsoleMessage.ProcessCommand(Command);

        public void WriteMemoryTogglePlayerGodMode(int playerSlot, int health) => CmdTogglePlayerGodMode.ProcessCommand(playerSlot, health);
    }
}
