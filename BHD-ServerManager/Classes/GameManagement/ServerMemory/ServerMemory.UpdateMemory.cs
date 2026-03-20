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

		// Constants for functions called in this file.
        

        // Function: UpdatePlayerHostName, Set the player name of the "Host" for the Game Server.
		public static void UpdatePlayerHostName()
        {
            int buffer = 0;
            byte[] Hostname = Encoding.GetEncoding(1252).GetBytes(thisInstance.gameHostName + "\0");
            WriteProcessMemory((int)processHandle, GetPlayerAddress(1), Hostname, Hostname.Length, ref buffer);
        }
        // Function: UpdateAllowCustomSkins
        public static void UpdateAllowCustomSkins()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000AD760, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] AllowCustomSkinsBytes = new byte[4];
            int AllowCustomSkinsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, AllowCustomSkinsBytes, AllowCustomSkinsBytes.Length, ref AllowCustomSkinsRead);
            int AllowCustomSkins = BitConverter.ToInt32(AllowCustomSkinsBytes, 0);

            int AllowCustomSkinsWritten = 0;
            byte[] AllowCustomSkinsWrite = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameCustomSkins));
            WriteProcessMemory((int)processHandle, Ptr1Addr, AllowCustomSkinsWrite, AllowCustomSkinsWrite.Length, ref AllowCustomSkinsWritten);


        }
        // Function: UpdateDestroyBuildings
        public static void UpdateDestroyBuildings()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D99B8, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] DestroyBuildingsBytes = new byte[4];
            int DestroyBuildingsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, DestroyBuildingsBytes, DestroyBuildingsBytes.Length, ref DestroyBuildingsRead);
            int DestroyBuildings = BitConverter.ToInt32(DestroyBuildingsBytes, 0);

            int DestroyBuildingsWritten = 0;
            byte[] DestroyBuildingsWrite = new byte[4];
            DestroyBuildingsWrite = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameDestroyBuildings));
            WriteProcessMemory((int)processHandle, Ptr1Addr, DestroyBuildingsWrite, DestroyBuildingsWrite.Length, ref DestroyBuildingsWritten);


        }
        // Function: UpdateFatBullets
        public static void UpdateFatBullets()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D7F14, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] FatBulletsBytes = new byte[4];
            int FatBulletsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, FatBulletsBytes, FatBulletsBytes.Length, ref FatBulletsRead);
            int FatBullets = BitConverter.ToInt32(FatBulletsBytes, 0);

            int FatBulletsWritten = 0;
            byte[] FatBulletsWrite = new byte[4];
            FatBulletsWrite = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameFatBullets));

            WriteProcessMemory((int)processHandle, Ptr1Addr, FatBulletsWrite, FatBulletsWrite.Length, ref FatBulletsWritten);


        }
        // Function: UpdateFlagReturnTime
        public static void UpdateFlagReturnTime()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB6AC, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] FlagReturnTimeBytes = new byte[4];
            int FlagReturnTimeRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, FlagReturnTimeBytes, FlagReturnTimeBytes.Length, ref FlagReturnTimeRead);
            int FlagReturnTime = BitConverter.ToInt32(FlagReturnTimeBytes, 0);

            int FlagReturnTimeWritten = 0;
            byte[] FlagReturnTimeWrite = BitConverter.GetBytes(thisInstance.gameFlagReturnTime);
            WriteProcessMemory((int)processHandle, Ptr1Addr, FlagReturnTimeWrite, FlagReturnTimeWrite.Length, ref FlagReturnTimeWritten);


        }
        // Function: UpdateMaxPing
        public static void UpdateMinPing()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D9A60, Ptr1, Ptr1.Length, ref Ptr1Read);
            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MinPingBytes = new byte[4];
            int MaxPingRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MinPingBytes, MinPingBytes.Length, ref MaxPingRead);
            int MinPing = BitConverter.ToInt32(MinPingBytes, 0);
            int MinPingWritten = 0;
            byte[] MinPingWrite = BitConverter.GetBytes(thisInstance.gameMinPing);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MinPingWrite, MinPingWrite.Length, ref MinPingWritten);


        }
        // Function: UpdateMinPingValue
        public static void UpdateMinPingValue()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D7FB8, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MinPingValueBytes = new byte[4];
            int MinPingValueRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MinPingValueBytes, MinPingValueBytes.Length, ref MinPingValueRead);
            int MinPingValue = BitConverter.ToInt32(MinPingValueBytes, 0);

            int MinPingValueWritten = 0;
            byte[] MinPingValueWrite = BitConverter.GetBytes(thisInstance.gameMinPingValue);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MinPingValueWrite, MinPingValueWrite.Length, ref MinPingValueWritten);


        }
        // Function: UpdateMaxPing
        public static void UpdateMaxPing()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB634, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MaxPingBytes = new byte[4];
            int MaxPingRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr + 0x4, MaxPingBytes, MaxPingBytes.Length, ref MaxPingRead);
            int MaxPing = BitConverter.ToInt32(MaxPingBytes, 0);

            byte[] MaxPingWrite = new byte[4];
            MaxPingWrite = BitConverter.GetBytes(thisInstance.gameMaxPing);

            int MaxPingWritten = 0;
            WriteProcessMemory((int)processHandle, Ptr1Addr + 0x4, MaxPingWrite, MaxPingWrite.Length, ref MaxPingWritten);


        }
        // Function: UpdateMaxPingValue
        public static void UpdateMaxPingValue()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB634, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MaxPingValueBytes = new byte[4];
            int MaxPingValueRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MaxPingValueBytes, MaxPingValueBytes.Length, ref MaxPingValueRead);
            int MaxPingValue = BitConverter.ToInt32(MaxPingValueBytes, 0);

            int MaxPingValueWritten = 0;
            byte[] MaxPingValueWrite = BitConverter.GetBytes(thisInstance.gameMaxPingValue);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MaxPingValueWrite, MaxPingValueWrite.Length, ref MaxPingValueWritten);


        }
        // Function: UpdateOneShotKills
        public static void UpdateOneShotKills()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D8580, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] OneShotKillsBytes = new byte[4];
            int OneShotKillsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, OneShotKillsBytes, OneShotKillsBytes.Length, ref OneShotKillsRead);
            int OneShotKills = BitConverter.ToInt32(OneShotKillsBytes, 0);

            int OneShotKillsWritten = 0;
            byte[] OneShotKillsWrite = new byte[4];
            OneShotKillsWrite = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameOneShotKills));

            WriteProcessMemory((int)processHandle, Ptr1Addr, OneShotKillsWrite, OneShotKillsWrite.Length, ref OneShotKillsWritten);


        }
        // Function: UpdatePSPTakeOverTime
        public static void UpdatePSPTakeOverTime()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB6FC, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] PSPTakeOverTimeBytes = new byte[4];
            int PSPTakeOverTimeRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr + 0x4, PSPTakeOverTimeBytes, PSPTakeOverTimeBytes.Length, ref PSPTakeOverTimeRead);
            int PSPTakeOverTimeValue = BitConverter.ToInt32(PSPTakeOverTimeBytes, 0);

            int PSPTakeOverTimeWritten = 0;
            byte[] PSPTakeOverTimeWrite = BitConverter.GetBytes(thisInstance.gamePSPTOTimer);
            WriteProcessMemory((int)processHandle, Ptr1Addr + 0x4, PSPTakeOverTimeWrite, PSPTakeOverTimeWrite.Length, ref PSPTakeOverTimeWritten);


        }
        // Function: UpdateRequireNovaLogin
        public static void UpdateRequireNovaLogin()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D9960, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] RequireNovaBytes = new byte[4];
            int RequireNovaRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, RequireNovaBytes, RequireNovaBytes.Length, ref RequireNovaRead);
            int RequireNova = BitConverter.ToInt32(RequireNovaBytes, 0);

            int RequireNovaWritten = 0;
            byte[] RequireNovaWrite = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameRequireNova));
            WriteProcessMemory((int)processHandle, Ptr1Addr, RequireNovaWrite, RequireNovaWrite.Length, ref RequireNovaWritten);


        }
        // Function: UpdateRespawnTime
        public static void UpdateRespawnTime()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DD4E8, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] RespawnTimeBytes = new byte[4];
            int RespawnTimeRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, RespawnTimeBytes, RespawnTimeBytes.Length, ref RespawnTimeRead);
            int RespawnTime = BitConverter.ToInt32(RespawnTimeBytes, 0);

            int RespawnTimeWritten = 0;
            byte[] RespawnTimeWrite = BitConverter.GetBytes(thisInstance.gameRespawnTime);
            WriteProcessMemory((int)processHandle, Ptr1Addr, RespawnTimeWrite, RespawnTimeWrite.Length, ref RespawnTimeWritten);


        }
        // Function: UpdateWeapon Restrictions
        public static void UpdateWeaponRestrictions()
        {


            byte[] Ptr1Bytes = new byte[4];
            int Ptr1Read = 0;
            int Ptr1Location = baseAddr + 0x0015C4B0;
            ReadProcessMemory((int)processHandle, Ptr1Location, Ptr1Bytes, Ptr1Bytes.Length, ref Ptr1Read);

            int Ptr1 = BitConverter.ToInt32(Ptr1Bytes, 0);
            int WeaponEntry = Ptr1 + 0x268;

            byte[] WPN_COLT45Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponColt45));
            int WPN_COLT45Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry, WPN_COLT45Bytes, WPN_COLT45Bytes.Length, ref WPN_COLT45Written);

            int WeaponEntry_WPN_M9BERETTA = WeaponEntry + 0x4;
            byte[] WPN_M9BERETTABytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM9Beretta));
            int WPN_M9BERETTAWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M9BERETTA, WPN_M9BERETTABytes, WPN_M9BERETTABytes.Length, ref WPN_M9BERETTAWritten);

            int WeaponEntry_WPN_REMMINGTONSG = WeaponEntry + 0x8;
            byte[] WPN_REMMINGTONSGBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponShotgun));
            int WPN_REMMINGTONSGWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_REMMINGTONSG, WPN_REMMINGTONSGBytes, WPN_REMMINGTONSGBytes.Length, ref WPN_REMMINGTONSGWritten);

            int WeaponEntry_WPN_CAR15 = WeaponEntry + 0xC;
            byte[] WPN_CAR15Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponCar15));
            int WPN_CAR15Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_CAR15, WPN_CAR15Bytes, WPN_CAR15Bytes.Length, ref WPN_CAR15Written);

            int WeaponEntry_WPN_CAR15_203 = WeaponEntry + 0x14;
            byte[] WPN_CAR15_203Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponCar15203));
            int WPN_CAR15_203Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_CAR15_203, WPN_CAR15_203Bytes, WPN_CAR15_203Bytes.Length, ref WPN_CAR15_203Written);

            int WeaponEntry_WPN_M16_BURST = WeaponEntry + 0x20;
            byte[] WPN_M16_BURSTBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM16));
            int WPN_M16_BURSTWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M16_BURST, WPN_M16_BURSTBytes, WPN_M16_BURSTBytes.Length, ref WPN_M16_BURSTWritten);

            int WeaponEntry_WPN_M16_BURST_203 = WeaponEntry + 0x28;
            byte[] WPN_M16_BURST_203Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM16203));
            int WPN_M16_BURST_203Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M16_BURST_203, WPN_M16_BURST_203Bytes, WPN_M16_BURST_203Bytes.Length, ref WPN_M16_BURST_203Written);

            int WeaponEntry_WPN_M21 = WeaponEntry + 0x34;
            byte[] WPN_M21Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM21));
            int WPN_M21Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M21, WPN_M21Bytes, WPN_M21Bytes.Length, ref WPN_M21Written);

            int WeaponEntry_WPN_M24 = WeaponEntry + 0x38;
            byte[] WPN_M24Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM24));
            int WPN_M24Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M24, WPN_M24Bytes, WPN_M24Bytes.Length, ref WPN_M24Written);

            int WeaponEntry_WPN_MCRT_300_TACTICAL = WeaponEntry + 0x3C;
            byte[] WPN_MCRT_300_TACTICALBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponMCRT300));
            int WPN_MCRT_300_TACTICALWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_MCRT_300_TACTICAL, WPN_MCRT_300_TACTICALBytes, WPN_MCRT_300_TACTICALBytes.Length, ref WPN_MCRT_300_TACTICALWritten);

            int WeaponEntry_WPN_BARRETT = WeaponEntry + 0x40;
            byte[] WPN_BARRETTBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponBarrett));
            int WPN_BARRETTWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_BARRETT, WPN_BARRETTBytes, WPN_BARRETTBytes.Length, ref WPN_BARRETTWritten);

            int WeaponEntry_WPN_SAW = WeaponEntry + 0x44;
            byte[] WPN_SAWBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponSAW));
            int WPN_SAWWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_SAW, WPN_SAWBytes, WPN_SAWBytes.Length, ref WPN_SAWWritten);

            int WeaponEntry_WPN_M60 = WeaponEntry + 0x48;
            byte[] WPN_M60Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM60));
            int WPN_M60Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M60, WPN_M60Bytes, WPN_M60Bytes.Length, ref WPN_M60Written);

            int WeaponEntry_WPN_M240 = WeaponEntry + 0x4C;
            byte[] WPN_M240Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponM240));
            int WPN_M240Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M240, WPN_M240Bytes, WPN_M240Bytes.Length, ref WPN_M240Written);

            int WeaponEntry_WPN_MP5 = WeaponEntry + 0x50;
            byte[] WPN_MP5Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponMP5));
            int WPN_MP5Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_MP5, WPN_MP5Bytes, WPN_MP5Bytes.Length, ref WPN_MP5Written);

            int WeaponEntry_WPN_G3 = WeaponEntry + 0x54;
            byte[] WPN_G3Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponG3));
            int WPN_G3Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_G3, WPN_G3Bytes, WPN_G3Bytes.Length, ref WPN_G3Written);

            int WeaponEntry_WPN_G36 = WeaponEntry + 0x5C;
            byte[] WPN_G36Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponG36));
            int WPN_G36Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_G36, WPN_G36Bytes, WPN_G36Bytes.Length, ref WPN_G36Written);

            int WeaponEntry_WPN_PSG1 = WeaponEntry + 0x64;
            byte[] WPN_PSG1Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponPSG1));
            int WPN_PSG1Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_PSG1, WPN_PSG1Bytes, WPN_PSG1Bytes.Length, ref WPN_PSG1Written);

            int WeaponEntry_WPN_XM84_STUN = WeaponEntry + 0x68;
            byte[] WPN_XM84_STUNBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponFlashGrenade));
            int WPN_XM84_STUNWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_XM84_STUN, WPN_XM84_STUNBytes, WPN_XM84_STUNBytes.Length, ref WPN_XM84_STUNWritten);

            int WeaponEntry_WPN_M67_FRAG = WeaponEntry + 0x6C;
            byte[] WPN_M67_FRAGBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponFragGrenade));
            int WPN_M67_FRAGWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_M67_FRAG, WPN_M67_FRAGBytes, WPN_M67_FRAGBytes.Length, ref WPN_M67_FRAGWritten);

            int WeaponEntry_WPN_AN_M8_SMOKE = WeaponEntry + 0x70;
            byte[] WPN_AN_M8_SMOKEBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponSmokeGrenade));
            int WPN_AN_M8_SMOKEWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_AN_M8_SMOKE, WPN_AN_M8_SMOKEBytes, WPN_AN_M8_SMOKEBytes.Length, ref WPN_AN_M8_SMOKEWritten);

            int WeaponEntry_WPN_SATCHEL_CHARGE = WeaponEntry + 0x78;
            byte[] WPN_SATCHEL_CHARGEBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponSatchelCharges));
            int WPN_SATCHEL_CHARGEWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_SATCHEL_CHARGE, WPN_SATCHEL_CHARGEBytes, WPN_SATCHEL_CHARGEBytes.Length, ref WPN_SATCHEL_CHARGEWritten);

            int WeaponEntry_WPN_CLAYMORE = WeaponEntry + 0x80;
            byte[] WPN_CLAYMOREBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponClaymore));
            int WPN_CLAYMOREWritten = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_CLAYMORE, WPN_CLAYMOREBytes, WPN_CLAYMOREBytes.Length, ref WPN_CLAYMOREWritten);

            int WeaponEntry_WPN_AT4 = WeaponEntry + 0x84;
            byte[] WPN_AT4Bytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.weaponAT4));
            int WPN_AT4Written = 0;
            WriteProcessMemory((int)processHandle, WeaponEntry_WPN_AT4, WPN_AT4Bytes, WPN_AT4Bytes.Length, ref WPN_AT4Written);


        }
        // Function: UpdateGamePlayOptions
        public static void UpdateGamePlayOptions()
        {

            int gameOptions = 0;
            try
            {
                gameOptions = Functions.CalulateGameOptions(thisInstance.gameOptionAutoBalance, thisInstance.gameOptionFF, thisInstance.gameOptionFriendlyTags, thisInstance.gameOptionFFWarn, thisInstance.gameOptionShowTracers, thisInstance.gameShowTeamClays, thisInstance.gameOptionAutoRange);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating game options: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] gameOptionsBytes = BitConverter.GetBytes(gameOptions);

            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D7D6C, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] GamePlayOptionsOneBytes = new byte[4];
            int GamePlayOptionsOneRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, GamePlayOptionsOneBytes, GamePlayOptionsOneBytes.Length, ref GamePlayOptionsOneRead);
            int GamePlayOptionsOne = BitConverter.ToInt32(GamePlayOptionsOneBytes, 0);

            int GamePlayOptionsOneWritten = 0;
            WriteProcessMemory((int)processHandle, Ptr1Addr, gameOptionsBytes, gameOptionsBytes.Length, ref GamePlayOptionsOneWritten);


        }
        // Function: UpdateServerName
        public static void UpdateServerName()
        {


            // Server Query Name
            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x001BF400, Ptr1, Ptr1.Length, ref Ptr1Read);
            int Ptr2 = BitConverter.ToInt32(Ptr1, 0);

            byte[] ServerName = new byte[31];
            int ServerNamePtrRead = 0;
            ReadProcessMemory((int)processHandle, Ptr2 + 0x4, ServerName, ServerName.Length, ref ServerNamePtrRead);
            string ServerNameQuery = Encoding.GetEncoding(1252).GetString(ServerName).Replace("\0", "");
            // end Server Query Name

            // Server Name Display
            byte[] Ptr3 = new byte[4];
            int Ptr3Read = 0;
            ReadProcessMemory((int)processHandle, Ptr2 + 0x000A7088, Ptr3, Ptr3.Length, ref Ptr3Read);
            int ServerDisplayerName = BitConverter.ToInt32(Ptr3, 0);

            byte[] ServerNameDisplay = new byte[31];
            int ServerNameRead = 0;
            ReadProcessMemory((int)processHandle, ServerDisplayerName + 0x30, ServerNameDisplay, ServerNameDisplay.Length, ref ServerNameRead);
            string ServerDisplayName = Encoding.GetEncoding(1252).GetString(ServerNameDisplay).Replace("\0", "");
            // end Server Name Display

            // since either one or the other isn't what it should be.. just update them both. Call it a day.
            byte[] ServerNameBytes = Encoding.GetEncoding(1252).GetBytes(thisInstance.gameServerName);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, ServerDisplayerName, ServerNameBytes, ServerNameBytes.Length, ref bytesWritten);
            WriteProcessMemory((int)processHandle, Ptr2 + 0x4, ServerNameBytes, ServerNameBytes.Length, ref bytesWritten);


        }
        // Function: UpdateMOTD
        public static void UpdateMOTD()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D9AAC, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MOTDBytes = new byte[85];
            int MOTDRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MOTDBytes, MOTDBytes.Length, ref MOTDRead);
            string MOTD = Encoding.GetEncoding(1252).GetString(MOTDBytes).Replace("\0", "");

            int MOTDWritten = 0;
            byte[] MOTDWrite = Encoding.GetEncoding(1252).GetBytes(thisInstance.gameMOTD);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MOTDWrite, MOTDWrite.Length, ref MOTDWritten);


        }
        // Function: UpdateTimeLimit
        public static void UpdateTimeLimit()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DD1DC, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] TimeLimitBytes = new byte[4];
            int TimeLimitRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, TimeLimitBytes, TimeLimitBytes.Length, ref TimeLimitRead);
            int TimeLimit = BitConverter.ToInt32(TimeLimitBytes, 0);

            int TimeLimitWritten = 0;
            byte[] TimeLimitWrite = BitConverter.GetBytes(thisInstance.gameTimeLimit);
            WriteProcessMemory((int)processHandle, Ptr1Addr, TimeLimitWrite, TimeLimitWrite.Length, ref TimeLimitWritten);


        }
        // Function: UpdateLoopMaps
        public static void UpdateLoopMaps()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB6A0, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] LoopMapsBytes = new byte[4];
            int LoopMapsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, LoopMapsBytes, LoopMapsBytes.Length, ref LoopMapsRead);
            int LoopMaps = BitConverter.ToInt32(LoopMapsBytes, 0);

            int LoopMapsWritten = 0;
            byte[] LoopMapsWrite = BitConverter.GetBytes(thisInstance.gameLoopMaps);
            WriteProcessMemory((int)processHandle, Ptr1Addr, LoopMapsWrite, LoopMapsWrite.Length, ref LoopMapsWritten);


        }
        // Function: UpdateStartDelay
        public static void UpdateStartDelay()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D7F00, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] StartDelayBytes = new byte[4];
            int StartDelayRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, StartDelayBytes, StartDelayBytes.Length, ref StartDelayRead);
            int StartDelay = BitConverter.ToInt32(StartDelayBytes, 0);

            int StartDelayWritten = 0;
            byte[] StartDelayWrite = BitConverter.GetBytes(thisInstance.gameStartDelay);
            WriteProcessMemory((int)processHandle, Ptr1Addr, StartDelayWrite, StartDelayWrite.Length, ref StartDelayWritten);


        }
        // Function: UpdateMaxSlots
        public static void UpdateMaxSlots()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D97A0, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MaxSlotsBytes = new byte[4];
            int MaxSlotsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MaxSlotsBytes, MaxSlotsBytes.Length, ref MaxSlotsRead);
            int MaxSlots = BitConverter.ToInt32(MaxSlotsBytes, 0);

            int MaxSlotsWritten = 0;
            byte[] MaxSlotsWrite = BitConverter.GetBytes(thisInstance.gameMaxSlots);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MaxSlotsWrite, MaxSlotsWrite.Length, ref MaxSlotsWritten);


        }
        // Function: UpdateFriendlyFireKills
        public static void UpdateFriendlyFireKills()
        {


            var baseAddr = 0x400000;

            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DB684, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] FriendlyFireKillsBytes = new byte[4];
            int FriendlyFireKillsRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, FriendlyFireKillsBytes, FriendlyFireKillsBytes.Length, ref FriendlyFireKillsRead);
            int FriendlyFireKills = BitConverter.ToInt32(FriendlyFireKillsBytes, 0);

            int FriendlyFireKillsWritten = 0;
            byte[] FriendlyFireKillsWrite = BitConverter.GetBytes(thisInstance.gameFriendlyFireKills);
            WriteProcessMemory((int)processHandle, Ptr1Addr, FriendlyFireKillsWrite, FriendlyFireKillsWrite.Length, ref FriendlyFireKillsWritten);


        }
        // Function: UpdateBluePassword
        public static void UpdateBluePassword()
        {
            // byte_9F204A: char[17] — direct buffer, no pointer indirection
            byte[] BluePasswordWrite = new byte[17];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordBlue))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordBlue);
                Array.Copy(pwBytes, BluePasswordWrite, Math.Min(pwBytes.Length, 16));
            }

            int BluePasswordWritten = 0;
            WriteProcessMemory((int)processHandle, baseAddr + 0x005F204A, BluePasswordWrite, BluePasswordWrite.Length, ref BluePasswordWritten);
        }
        // Function: UpdateRedPassword
        public static void UpdateRedPassword()
        {
            // byte_9F2039: char[17] — direct buffer, no pointer indirection
            byte[] RedPasswordWrite = new byte[17];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordRed))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordRed);
                Array.Copy(pwBytes, RedPasswordWrite, Math.Min(pwBytes.Length, 16));
            }

            int RedPasswordWritten = 0;
            WriteProcessMemory((int)processHandle, baseAddr + 0x005F2039, RedPasswordWrite, RedPasswordWrite.Length, ref RedPasswordWritten);
        }
        // Function: UpdateYellowPassword
        public static void UpdateYellowPassword()
        {
            // byte_9F205B: char[17] — direct buffer, no pointer indirection
            byte[] YellowPasswordWrite = new byte[17];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordYellow))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordYellow);
                Array.Copy(pwBytes, YellowPasswordWrite, Math.Min(pwBytes.Length, 16));
            }

            int YellowPasswordWritten = 0;
            WriteProcessMemory((int)processHandle, baseAddr + 0x005F205B, YellowPasswordWrite, YellowPasswordWrite.Length, ref YellowPasswordWritten);
        }
        // Function: UpdateVioletPassword
        public static void UpdateVioletPassword()
        {
            // byte_9F206C: char[17] — direct buffer, no pointer indirection
            byte[] VioletPasswordWrite = new byte[17];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordViolet))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordViolet);
                Array.Copy(pwBytes, VioletPasswordWrite, Math.Min(pwBytes.Length, 16));
            }

            int VioletPasswordWritten = 0;
            WriteProcessMemory((int)processHandle, baseAddr + 0x005F206C, VioletPasswordWrite, VioletPasswordWrite.Length, ref VioletPasswordWritten);
        }
        // Function: UpdateNovaID
        public static void UpdateNovaID()
        {
            if (thisInstance.gameRequireNova == true)
            {
                return; // since we are requiring nova login, just return.
            }

            byte[] CurrentAppIDBytes = new byte[4];
            int currentAppIDRead = 0;
            ReadProcessMemory((int)processHandle, 0x009DDA44, CurrentAppIDBytes, CurrentAppIDBytes.Length, ref currentAppIDRead);
            int CurrentAppID = BitConverter.ToInt32(CurrentAppIDBytes, 0);

            if (CurrentAppID != 0)
            {
                byte[] WriteAppIDBytes = BitConverter.GetBytes(0);
                int WriteAppIDWritten = 0;
                WriteProcessMemory((int)processHandle, 0x009DDA44, WriteAppIDBytes, WriteAppIDBytes.Length, ref WriteAppIDWritten);
            }


        }
        // Function: UpdateGlobalGameType
        public static void UpdateGlobalGameType()
        {
            // this function is responsible for adjusting the Pinger Queries to the current game type
            var startingPtr = baseAddr + 0xACE0E8; // pinger query
            byte[] read_pingergametype = new byte[4];
            int read_pingergametypeBytesRead = 0;
            ReadProcessMemory((int)processHandle, startingPtr, read_pingergametype, read_pingergametype.Length, ref read_pingergametypeBytesRead);
            int PingerGameType = BitConverter.ToInt32(read_pingergametype, 0);

            // get set gametype
            var CurrentGameTypeAddr = baseAddr + 0x5F21A4;
            byte[] read_currentgametype = new byte[4];
            int read_currentgametypeBytesRead = 0;
            ReadProcessMemory((int)processHandle, CurrentGameTypeAddr, read_currentgametype, read_currentgametype.Length, ref read_currentgametypeBytesRead);
            int CurrentGameType = BitConverter.ToInt32(read_currentgametype, 0);

            if (PingerGameType != CurrentGameType)
            {
                int UpdatePingerQuery = 0;
                WriteProcessMemory((int)processHandle, startingPtr, read_currentgametype, read_currentgametype.Length, ref UpdatePingerQuery);

                return;
            }
            else
            {

                // no update required... Exit the function.
                return;
            }
        }
        // Function: Update NumTeam State
        public static void UpdateNumTeams(bool isEnabled)
        {
            AppDebug.Log("UpdateNumTeams", "Updating 4-Team Mode to: " + isEnabled);

			var numTeams = 0x00A344C4;
            byte[] endTimerBytes = BitConverter.GetBytes(isEnabled ? 4 : 0);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, numTeams, endTimerBytes, endTimerBytes.Length, ref bytesWritten);

            AppDebug.Log("UpdateNumTeams", $"4-Team Mode update complete. ({ReadNumTeams().ToString()})");
		}
        // Function: Update Start Delay Timer
        public static void UpdateStartDelayTimer(int value)
        {
            var instanceTimer = baseAddr + 0x5DAE04;
            byte[] endTimerBytes = BitConverter.GetBytes(value);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, instanceTimer, endTimerBytes, endTimerBytes.Length, ref bytesWritten);
        }
		// Function: UpdateScoreBoardTimer
		public static void UpdateScoreBoardTimer()
        {


            // This function updates the scoreboard timer in the server memory
            var instanceTimer = baseAddr + 0x5DAE00;
            byte[] endTimerBytes = BitConverter.GetBytes(1);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, instanceTimer, endTimerBytes, endTimerBytes.Length, ref bytesWritten);


        }
        // Function: WriteMemoryScoreMap
        public static void WriteMemoryScoreMap()
        {
            if (thisInstance.instanceStatus == InstanceStatus.SCORING || thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            var startingPtr1 = 0;
            startingPtr1 = baseAddr + 0x5F3740;
            byte[] timerBytes = BitConverter.GetBytes(10);
            int timerWritten1 = 0;
            WriteProcessMemory((int)processHandle, startingPtr1, timerBytes, timerBytes.Length, ref timerWritten1);

        }
	}
}
