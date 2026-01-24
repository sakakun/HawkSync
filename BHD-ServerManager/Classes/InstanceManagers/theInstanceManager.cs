using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Classes.Tickers;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Storage;

namespace BHD_ServerManager.Classes.InstanceManagers
{
    public static class theInstanceManager
    {
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;
        private static theInstance thisInstance => CommonCore.theInstance!;

        // The Instances (Data)
        private static theInstance theInstance => CommonCore.theInstance!;

        public static bool ValidateGameServerPath()
        {
            // Validate the profile server path
            if (string.IsNullOrWhiteSpace(theInstance.profileServerPath) || !Directory.Exists(theInstance.profileServerPath))
            {
                AppDebug.Log("InstanceManager", "Profile server path is invalid or does not exist.");
                MessageBox.Show("The profile server path is invalid or does not exist.  Please 'set' your server path and refresh your map list before starting the server.", "Invalid Profile Server Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // thisServer.ServerTab.btn_UpdatePath.Visible = true; // Enable the button to allow user to set the path
                return false;
            }
            // thisServer.ServerTab.btn_UpdatePath.Visible = false;
            return true;
        }
        public static void GetServerVariables(bool import = false, theInstance updatedInstance = null!)
        {

            var newInstance = import && updatedInstance != null ? updatedInstance : theInstance;
            
            // Trigger "Gets" for the Tabs
            thisServer.StatsTab.functionEvent_GetStatSettings((updatedInstance != null ? updatedInstance : null!));

        }
        public static void UpdateGameServer()
        {
            // the following code will replace line by line until all variables have a "update" function in ServerMemory

            if (thisInstance.instanceStatus == InstanceStatus.OFFLINE)
                return;

            // Host Information
            ServerMemory.UpdateServerName();            // gameServerName
            ServerMemory.UpdatePlayerHostName();        // gameHostname
            ServerMemory.UpdateMOTD();                  // gameMOTD

            // Server Details
            ServerMemory.UpdateRequireNovaLogin();      // gameRequireNova

            // Server Options
            ServerMemory.UpdateTimeLimit();             // gameTimeLimit
            ServerMemory.UpdateLoopMaps();              // gameLoopMaps
            ServerMemory.UpdateStartDelay();            // gameStartDelay
            ServerMemory.UpdateRespawnTime();           // gameRespawnTime   
            ServerMemory.UpdateMaxSlots();              // gameMaxSlots

            // Team Options
            ServerMemory.UpdateBluePassword();          // gamePasswordBlue
            ServerMemory.UpdateRedPassword();           // gamePasswordRed

            // Game Play Settings
            ServerMemory.UpdateGamePlayOptions();       //gameOptionShowTracers, gameShowTeamClays, gameOptionAutoRange, gameOptionFF, gameOptionFFWarn, gameOptionFriendlyTags, gameOptionAutoBalance
            ServerMemory.UpdatePSPTakeOverTime();       //gamePSPTOTimer
            ServerMemory.UpdateFlagReturnTime();        //gameFlagReturnTime
            ServerMemory.UpdateMaxTeamLives();          //gameMaxTeamLives

            // Friendly Fire
            ServerMemory.UpdateFriendlyFireKills();     //gameFriendlyFireKills

            // Ping Checking
            ServerMemory.UpdateMinPing();               //gameMinPing
            ServerMemory.UpdateMaxPing();               //gameMaxPing
            ServerMemory.UpdateMinPingValue();          //gameMinPingValue
            ServerMemory.UpdateMaxPingValue();          //gameMaxPingValue

            // Misc
            ServerMemory.UpdateAllowCustomSkins();      //gameCustomSkins
            ServerMemory.UpdateDestroyBuildings();      //gameDestroyBuildings
            ServerMemory.UpdateFatBullets();            //gameFatBullets
            ServerMemory.UpdateOneShotKills();          //gameOneShotKills

            // Restrictions Weapons
            ServerMemory.UpdateWeaponRestrictions();

            // Role Restrictions
            //thisInstance.roleCQB = thisServer.ServerTab.cb_roleCQB.Checked;
            //thisInstance.roleGunner = thisServer.ServerTab.cb_roleGunner.Checked;
            //thisInstance.roleSniper = thisServer.ServerTab.cb_roleSniper.Checked;
            //thisInstance.roleMedic = thisServer.ServerTab.cb_roleMedic.Checked;

            // Update Game Scores for the next game.
            ServerMemory.UpdateGameScores();

            //
            // To Be Moved to Ticker Event
            //

            // Player State Check
            //thisInstance.gameAllowLeftLeaning = thisServer.ServerTab.cb_enableLeftLean.Checked;

        }
        public static void InitializeTickers()
        {
            // TODO: Remove the need for the thisServer variable in the ticker methods.
            CommonCore.Ticker?.Start("ServerManager", 500, () => tickerServerManager.runTicker());
            CommonCore.Ticker?.Start("ChatManager", 500, () => tickerChatManagement.runTicker());
            CommonCore.Ticker?.Start("PlayerManager", 1000, () => tickerPlayerManagement.runTicker());
            CommonCore.Ticker?.Start("BanManager", 1000, () => tickerBanManagement.runTicker());

        }
        public static void changeTeamGameMode(int currentMapType, int nextMapType)
        {

            bool isCurrentMapTeamMap = Functions.IsMapTeamBased(currentMapType);
            bool isNextMapTeamMap = Functions.IsMapTeamBased(nextMapType);

            if (isNextMapTeamMap == false && isCurrentMapTeamMap == true)
            {
                // TDM -> DM
                // Going to get every current player and add them to the previous PlayerTeam list
                // Then change their PlayerTeam number to PlayerSlot + 5 (DM Team) 6 - 56 aka everyone is on thier own PlayerTeam.
                foreach (var playerRecord in theInstance.playerList)
                {
                    playerObject playerObj = playerRecord.Value;
                    theInstance.playerPreviousTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerTeam
                    });

                    theInstance.playerChangeTeamList.Add(new playerTeamObject
                    {
                        slotNum = playerObj.PlayerSlot,
                        Team = playerObj.PlayerSlot + 5
                    });
                }
            }
            else if (isNextMapTeamMap == true && isCurrentMapTeamMap == false)
            {
                // DM -> TDM
                // Going to attempt to put them back in their own PlayerSlot or randomly assign them to a PlayerTeam.
                foreach (playerTeamObject playerObj in theInstance.playerPreviousTeamList)
                {
                    if (theInstance.playerList[playerObj.slotNum] != null)
                    {
                        theInstance.playerChangeTeamList.Add(new playerTeamObject
                        {
                            slotNum = playerObj.slotNum,
                            Team = playerObj.Team
                        });
                    }
                }
                foreach (var playerRecord in theInstance.playerList)
                {
                    playerObject player = playerRecord.Value;
                    bool found = false;
                    foreach (playerTeamObject previousPlayer in theInstance.playerPreviousTeamList)
                    {
                        if (player.PlayerSlot == previousPlayer.slotNum)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        int blueteam = 0;
                        int redteam = 0;

                        foreach (playerTeamObject playerTeam in theInstance.playerPreviousTeamList)
                        {
                            if (playerTeam.Team == (int)Teams.TEAM_BLUE)
                            {
                                blueteam++;
                            }
                            else if (playerTeam.Team == (int)Teams.TEAM_RED)
                            {
                                redteam++;
                            }
                        }

                        if (blueteam > redteam)
                        {
                            theInstance.playerChangeTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_RED
                            });
                            theInstance.playerPreviousTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_RED
                            });
                        }
                        else if (blueteam < redteam)
                        {
                            theInstance.playerChangeTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_BLUE
                            });
                            theInstance.playerPreviousTeamList.Add(new playerTeamObject
                            {
                                slotNum = player.PlayerSlot,
                                Team = (int)Teams.TEAM_BLUE
                            });
                        }
                        else if (blueteam == redteam)
                        {
                            Random rand = new Random();
                            int rnd = rand.Next(1, 2);
                            if (rnd == 1)
                            {
                                theInstance.playerChangeTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_BLUE
                                });
                                theInstance.playerPreviousTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_BLUE
                                });
                            }
                            else if (rnd == 2)
                            {
                                theInstance.playerChangeTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_RED
                                });
                                theInstance.playerPreviousTeamList.Add(new playerTeamObject
                                {
                                    slotNum = player.PlayerSlot,
                                    Team = (int)Teams.TEAM_RED
                                });
                            }
                        }
                    }
                }
                theInstance.playerPreviousTeamList.Clear();
            }

        }

    }
}
