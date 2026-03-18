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
    // This class is a placeholder for GamePlay Settings

    public static partial class ServerMemory
    {

        //
        // Update (Set) Functions
        //

		// Function: UpdateServerName, Updates the Server Name in Memory to match the value in the instance.
        // This is used to update the Server Name that is displayed in the Server Browser and when the server sends a message.
		public static void UpdateServerName()
        {
            // Server Query Name
            int Ptr2 = ReadInt(baseAddr + 0x001BF400);
            WriteString(Ptr2 + 0x4, theInstance.gameServerName);

            // Server Name Display
            int ServerDisplayerName = ReadInt(Ptr2 + 0x000A7088);
            WriteString(ServerDisplayerName, theInstance.gameServerName);
        }

		// Function: UpdatePlayerHostName, Updates the Host Name in the Player List Structure in Memory.
		// This is used to update the Host Name that is displayed when the server sends a message.
		public static void UpdatePlayerHostName() => WriteString(ReadInt(PLAYER_LIST_PTR) + 0x3C, theInstance.gameHostName, nullTerminated: true);

		// Function: UpdateMOTD, Updates the Message of the Day in Memory to match the value in the instance.
		public static void UpdateMOTD() => WriteString(ReadInt(baseAddr + 0x000D9AAC), theInstance.gameMOTD);
        
		// Function: UpdateAllowCustomSkins, Updates the Allow Custom Skins setting in Memory to match the value in the instance.
		public static void UpdateAllowCustomSkins() => WriteInt(ReadInt(baseAddr + 0x000AD760), Convert.ToInt32(theInstance.gameCustomSkins));

		// Function: UpdateDestroyBuildings, Updates the Destroy Buildings setting in Memory to match the value in the instance.
		public static void UpdateDestroyBuildings() => WriteInt(ReadInt(baseAddr + 0x000D99B8), Convert.ToInt32(theInstance.gameDestroyBuildings));

		// Function: UpdateFatBullets, Updates the Fat Bullets setting in Memory to match the value in the instance.
		public static void UpdateFatBullets() => WriteInt(ReadInt(baseAddr + 0x000D7F14), Convert.ToInt32(theInstance.gameFatBullets));

		// Function: UpdateFlagReturnTime, Updates the Flag Return Time setting in Memory to match the value in the instance.
		public static void UpdateFlagReturnTime() => WriteInt(ReadInt(baseAddr + 0x000DB6AC), theInstance.gameFlagReturnTime);

		// Function: UpdateMinPing, Updates the Min Ping setting in Memory to match the value in the instance.
		public static void UpdateMinPing() => WriteInt(ReadInt(baseAddr + 0x000D9A60), Convert.ToInt32(theInstance.gameMinPing));

		// Function: UpdateMinPingValue, Updates the Min Ping Value setting in Memory to match the value in the instance.
		public static void UpdateMinPingValue() => WriteInt(ReadInt(baseAddr + 0x000D7FB8), theInstance.gameMinPingValue);

		// Function: UpdateMaxPing, Updates the Max Ping setting in Memory to match the value in the instance.
		public static void UpdateMaxPing() => WriteInt(ReadInt(baseAddr + 0x000DB634) + 0x4, Convert.ToInt32(theInstance.gameMaxPing));

		// Function: UpdateMaxPingValue, Updates the Max Ping Value setting in Memory to match the value in the instance.
		public static void UpdateMaxPingValue() => WriteInt(ReadInt(baseAddr + 0x000DB634), theInstance.gameMaxPingValue);

		// Function: UpdateOneShotKills, Updates the One Shot Kills setting in Memory to match the value in the instance.
		public static void UpdateOneShotKills() => WriteInt(ReadInt(baseAddr + 0x000D8580), Convert.ToInt32(theInstance.gameOneShotKills));

		// Function: UpdatePSPTakeOverTime, Updates the PSP Take Over Time setting in Memory to match the value in the instance.
		public static void UpdatePSPTakeOverTime() => WriteInt(ReadInt(baseAddr + 0x000DB6FC) + 0x4, theInstance.gamePSPTOTimer);

		// Function: UpdateRequireNovaLogin, Updates the Require Nova Login setting in Memory to match the value in the instance.
		public static void UpdateRequireNovaLogin() => WriteInt(ReadInt(baseAddr + 0x000D9960), Convert.ToInt32(theInstance.gameRequireNova));

		// Function: UpdateRespawnTime, Updates the Respawn Time setting in Memory to match the value in the instance.
		public static void UpdateRespawnTime() => WriteInt(ReadInt(baseAddr + 0x000DD4E8), theInstance.gameRespawnTime);

		// Function: UpdateTimeLimit, Updates the Time Limit setting in Memory to match the value in the instance.
		public static void UpdateTimeLimit() => WriteInt(ReadInt(baseAddr + 0x000DD1DC), theInstance.gameTimeLimit);

		// Function: UpdateLoopMaps, Updates the Loop Maps setting in Memory to match the value in the instance.
		public static void UpdateLoopMaps() => WriteInt(ReadInt(baseAddr + 0x000DB6A0), theInstance.gameLoopMaps);
		
        // Function: UpdateStartDelay, Updates the Start Delay setting in Memory to match the value in the instance.
		public static void UpdateStartDelay() => WriteInt(ReadInt(baseAddr + 0x000D7F00), theInstance.gameStartDelay);

		// Function: UpdateMaxSlots, Updates the Max Slots setting in Memory to match the value in the instance.
		public static void UpdateMaxSlots() => WriteInt(ReadInt(baseAddr + 0x000D97A0), theInstance.gameMaxSlots);

		// Function: UpdateFriendlyFireKills, Updates the Friendly Fire Kills setting in Memory to match the value in the instance.
		public static void UpdateFriendlyFireKills() => WriteInt(ReadInt(baseAddr + 0x000DB684), theInstance.gameFriendlyFireKills);

		// Function: UpdateBluePassword, byte_9F204A: char[17] — direct buffer, no pointer indirection
        // Updates the Blue Team Password in Memory to match the value in the instance.
		public static void UpdateBluePassword() => WriteFixedString(baseAddr + 0x005F204A, theInstance.gamePasswordBlue, 17);

		// Function: UpdateRedPassword, byte_9F2039: char[17] — direct buffer, no pointer indirection
		// Updates the Red Team Password in Memory to match the value in the instance.
		public static void UpdateRedPassword() => WriteFixedString(baseAddr + 0x005F2039, theInstance.gamePasswordRed, 17);

		// Function: UpdateYellowPassword, byte_9F205B: char[17] — direct buffer, no pointer indirection
		// Updates the Yellow Team Password in Memory to match the value in the instance.
		public static void UpdateYellowPassword() => WriteFixedString(baseAddr + 0x005F205B, theInstance.gamePasswordYellow, 17);

		// Function: UpdateVioletPassword, byte_9F206C: char[17] — direct buffer, no pointer indirection
		// Updates the Violet Team Password in Memory to match the value in the instance.
		public static void UpdateVioletPassword() => WriteFixedString(baseAddr + 0x005F206C, theInstance.gamePasswordViolet, 17);

		// Function: Update Start Delay Timer, Updates the Start Delay Timer in Memory to match the value in the instance.
		public static void UpdateStartDelayTimer(int value) => WriteInt(baseAddr + 0x5DAE04, value);
		
		// Function: UpdateScoreBoardTimer
		public static void UpdateScoreBoardTimer() => WriteInt(baseAddr + 0x5DAE00, 1);


		// Function: UpdateNovaID, Updates the Nova ID in Memory to match the value in the instance.
		public static void UpdateNovaID()
        {
            if (theInstance.gameRequireNova)
                return; // game enforces Nova login natively; don't zero the NovaID

            if (ReadInt(NOVA_ID_ADDR) != 0)
                WriteInt(NOVA_ID_ADDR, 0);
        }

		// Function: UpdateGamePlayOptions, Updates the Game Play Options in Memory to match the values in the instance.
		public static void UpdateGamePlayOptions()
        {
            int MP_ATTRIB_ADDR = 0x00A34390;

            // Start from the current in-memory value so unrelated bits are preserved.
            int currentOptions = ReadInt(MP_ATTRIB_ADDR);
            int gameOptions;

            try
            {
                // Variables are: Auto Balance, Friendly Fire, Friendly Tags,
                // Friendly Fire Warning, Show Tracers, Show Team Clays, Allow Auto Range.
                gameOptions = Functions.CalulateGameOptions(
                    currentOptions,
                    theInstance.gameOptionAutoBalance,
                    theInstance.gameOptionFF,
                    theInstance.gameOptionFriendlyTags,
                    theInstance.gameOptionFFWarn,
                    theInstance.gameOptionShowTracers,
                    theInstance.gameShowTeamClays,
                    theInstance.gameOptionAutoRange);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error calculating game options: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            WriteInt(MP_ATTRIB_ADDR, gameOptions);
        }

		// Function: UpdateWeapon Restrictions, Updates the Weapon Restrictions in Memory to match the values in the instance.
		public static void UpdateWeaponRestrictions()
        {
            int WeaponEntry = ReadInt(baseAddr + 0x0015C4B0) + 0x268;

            WriteInt(WeaponEntry,          Convert.ToInt32(theInstance.weaponColt45));
            WriteInt(WeaponEntry + 0x04,   Convert.ToInt32(theInstance.weaponM9Beretta));
            WriteInt(WeaponEntry + 0x08,   Convert.ToInt32(theInstance.weaponShotgun));
            WriteInt(WeaponEntry + 0x0C,   Convert.ToInt32(theInstance.weaponCar15));
            WriteInt(WeaponEntry + 0x14,   Convert.ToInt32(theInstance.weaponCar15203));
            WriteInt(WeaponEntry + 0x20,   Convert.ToInt32(theInstance.weaponM16));
            WriteInt(WeaponEntry + 0x28,   Convert.ToInt32(theInstance.weaponM16203));
            WriteInt(WeaponEntry + 0x34,   Convert.ToInt32(theInstance.weaponM21));
            WriteInt(WeaponEntry + 0x38,   Convert.ToInt32(theInstance.weaponM24));
            WriteInt(WeaponEntry + 0x3C,   Convert.ToInt32(theInstance.weaponMCRT300));
            WriteInt(WeaponEntry + 0x40,   Convert.ToInt32(theInstance.weaponBarrett));
            WriteInt(WeaponEntry + 0x44,   Convert.ToInt32(theInstance.weaponSAW));
            WriteInt(WeaponEntry + 0x48,   Convert.ToInt32(theInstance.weaponM60));
            WriteInt(WeaponEntry + 0x4C,   Convert.ToInt32(theInstance.weaponM240));
            WriteInt(WeaponEntry + 0x50,   Convert.ToInt32(theInstance.weaponMP5));
            WriteInt(WeaponEntry + 0x54,   Convert.ToInt32(theInstance.weaponG3));
            WriteInt(WeaponEntry + 0x5C,   Convert.ToInt32(theInstance.weaponG36));
            WriteInt(WeaponEntry + 0x64,   Convert.ToInt32(theInstance.weaponPSG1));
            WriteInt(WeaponEntry + 0x68,   Convert.ToInt32(theInstance.weaponFlashGrenade));
            WriteInt(WeaponEntry + 0x6C,   Convert.ToInt32(theInstance.weaponFragGrenade));
            WriteInt(WeaponEntry + 0x70,   Convert.ToInt32(theInstance.weaponSmokeGrenade));
            WriteInt(WeaponEntry + 0x78,   Convert.ToInt32(theInstance.weaponSatchelCharges));
            WriteInt(WeaponEntry + 0x80,   Convert.ToInt32(theInstance.weaponClaymore));
            WriteInt(WeaponEntry + 0x84,   Convert.ToInt32(theInstance.weaponAT4));
        }

		// Function: UpdateGlobalGameType, Updates the Global Game Type in Memory to match the current game type.
		public static void UpdateGlobalGameType()
        {
            // this function is responsible for adjusting the Pinger Queries to the current game type
            int startingPtr = baseAddr + 0xACE0E8; // pinger query
            int PingerGameType = ReadInt(startingPtr);

            // get set gametype
            int CurrentGameType = ReadInt(baseAddr + 0x5F21A4);

            if (PingerGameType != CurrentGameType)
            {
                WriteInt(startingPtr, CurrentGameType);
            }
        }

		// Function: Update NumTeam State, Updates the Num Teams in Memory to match the value in the instance.
		public static void UpdateNumTeams(bool enableTeams) => WriteInt(0x00A344C4, enableTeams ? 4 : 0);

		// Function: UpdateGameScores, Updates the Game Scores in Memory to match the values in the instance.
		public static void UpdateGameScores()
        {

            AppDebug.Log("UpdateGameScores", "Updated Game Scores");

            // This changes the score needed to win on the next map played.
            int nextGameScore = 0;
            var startingPtr1 = 0;
            var startingPtr2 = 0;

            switch (mapInstance.NextMapGameType)
            {
                // KOTH/TKOTH
                case 3:
                case 4:
                    startingPtr1 = baseAddr + 0x5F21B8;
                    startingPtr2 = baseAddr + 0x6344B4;
                    nextGameScore = theInstance.gameScoreZoneTime;
                    break;
                // flag ball
                case 8:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = theInstance.gameScoreFlags;
                    break;
                // all other game types...
                default:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = theInstance.gameScoreKills;
                    break;
            }
            WriteInt(startingPtr1, nextGameScore);
            WriteInt(startingPtr2, nextGameScore);

        }

	}
}
