using HawkSyncShared;
using HawkSyncShared.SupportClasses;
using ServerManager.Classes.InstanceManagers;
using HawkSyncShared.Instances;
using ServerManager.Classes.SupportClasses;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.DTOs.tabPlayers;

namespace ServerManager.Classes.GameManagement
{
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static class ServerMemory
    {
        // Global Variables
        private static theInstance thisInstance => CommonCore.theInstance!;
        private static playerInstance playerInstance => CommonCore.instancePlayers!;
        private  static mapInstance mapInstance => CommonCore.instanceMaps!;

        // START: Process Memory Variables
        // Import of Dynamic Link Libraries
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("user32.dll")]
        static extern bool PostMessage(nint hWnd, uint Msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const int VK_ENTER = 0x0D;
        const int cmdConsole = 0xC0;
        const int chatConsole = 0x54;
        // END: Process Memory Variables

        // Corrected declaration of baseAddr
        public readonly static int baseAddr = 0x400000;

        // Process Related Variables
        public static Process? gameProcess { get; set; }
        public static nint windowHandle { get; set; } = nint.Zero;
        public static nint processHandle { get; set; } = nint.Zero;

        //
        // Server Memory Functions
        // Description: Functions to read and write memory and other supporting functions.
        //

        // Function: attachProcess, Attach to the Game Process, used by all memory functions.
        public static void AttachToGameProcess()
        {
            if (gameProcess == null || gameProcess.HasExited)
            {
                gameProcess = Process.GetProcessById((int)thisInstance.instanceAttachedPID!);
                windowHandle = gameProcess.MainWindowHandle;
                processHandle = OpenProcess(PROCESS_WM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION | PROCESS_QUERY_INFORMATION, false, gameProcess.Id);
            }
        }

        // Call this ONCE when server is shutting down
        public static void DetachFromGameProcess()
        {
            gameProcess?.Dispose();
            gameProcess = null;
            windowHandle = nint.Zero;
            processHandle = nint.Zero;
        }
        // Function: GetGameTypeID, Converts the MapType ShortName to and INT value.
        public static int getGameTypeID(string gameType)
        {
            switch (gameType)
            {
                case "DM":
                    return 0;
                case "TDM":
                    return 1;
                case "CP":
                    return 2;
                case "TKOTH":
                    return 3;
                case "KOTH":
                    return 4;
                case "SD":
                    return 5;
                case "AD":
                    return 6;
                case "CTF":
                    return 7;
                case "FB":
                    return 8;
                default:
                    return -1;
            }
        }

        //
        // Read & Write Memory
        // Description: Reads and writes memory to the game process.
        // 

        // Function: UpdatePlayerHostName, Set the player name of the "Host" for the Game Server.
        public static void UpdatePlayerHostName()
        {


            int buffer = 0;
            byte[] PointerAddr = new byte[4];
            var Pointer = baseAddr + 0x005ED600;
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr, PointerAddr.Length, ref buffer);
            int buffer2 = 0;
            byte[] Hostname = Encoding.GetEncoding(1252).GetBytes(thisInstance.gameHostName + "\0");
            var Address2HostName = BitConverter.ToInt32(PointerAddr, 0);
            WriteProcessMemory((int)processHandle, Address2HostName + 0x3C, Hostname, Hostname.Length, ref buffer2);



        }
        // Function: UpdateMapListCount, Set the Map Count for the Game Server, for looping purposes.
        public static void UpdateMapListCount()
        {


            int MapListMoveGarbageAddress = baseAddr + 0x5EA7B8;
            byte[] CurrentAddressBytes = new byte[4];
            int CurrentAddressRead = 0;
            ReadProcessMemory((int)processHandle, MapListMoveGarbageAddress, CurrentAddressBytes, CurrentAddressBytes.Length, ref CurrentAddressRead);
            int CurrentAddress = BitConverter.ToInt32(CurrentAddressBytes, 0);
            int NewAddress = CurrentAddress + 0x350;

            byte[] NewAddressBytes = BitConverter.GetBytes(NewAddress);
            int NewAddressWritten = 0;
            WriteProcessMemory((int)processHandle, MapListMoveGarbageAddress, NewAddressBytes, NewAddressBytes.Length, ref NewAddressWritten);

            int mapListLocationPtr = baseAddr + 0x005ED5F8;
            byte[] mapListLocationPtrBytes = new byte[4];
            int mapListLocationBytesPtrRead = 0;
            ReadProcessMemory((int)processHandle, mapListLocationPtr, mapListLocationPtrBytes, mapListLocationPtrBytes.Length, ref mapListLocationBytesPtrRead);

            int mapListNumberOfMaps = BitConverter.ToInt32(mapListLocationPtrBytes, 0) + 0x4;
            byte[] numberOfMaps = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int numberofMapsWritten = 0;
            WriteProcessMemory((int)processHandle, mapListNumberOfMaps, numberOfMaps, numberOfMaps.Length, ref numberofMapsWritten);

            mapListNumberOfMaps += 0x4;
            byte[] TotalnumberOfMaps = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int TotalnumberofMapsWritten = 0;
            WriteProcessMemory((int)processHandle, mapListNumberOfMaps, TotalnumberOfMaps, TotalnumberOfMaps.Length, ref TotalnumberofMapsWritten);


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

        // Function: UpdateMapCycle1
        // Clears the current map cycle and fills it with empty maps
        public static void UpdateMapCycle1()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #88");
            }



            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int mapCycleServerAddress = BitConverter.ToInt32(ServerMapCyclePtr, 0);

            int mapCycleTotalAddress = mapCycleServerAddress + 0x4;
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);

            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);

            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);

            foreach (MapObject entry in mapInstance.Playlists[0])
            {
                int mapFileIndexLocation = mapCycleList;

                byte[] mapFileBytes = new byte[0x20]; // 32 bytes, all initialized to 0
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);

                byte[] customMapFlag = BitConverter.GetBytes(Convert.ToInt32(false));
                int customMapFlagWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, customMapFlag, customMapFlag.Length, ref customMapFlagWritten);
                mapCycleList += 0x24;
            }


        }
        // Function: UpdateMapCycle2
        // Actually updates the memory of the game server with the current map list
        public static void UpdateMapCycle2()
        {
            if (mapInstance.Playlists[mapInstance.ActivePlaylist].Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #89");
            }



            byte[] Pointer1Bytes = new byte[4];
            int Pointer1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000DC6D8, Pointer1Bytes, Pointer1Bytes.Length, ref Pointer1Read);
            int mapCycleClientAddress = BitConverter.ToInt32(Pointer1Bytes, 0);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms, Encoding.Default);

            // Helper to write a fixed-length string with null padding
            void WriteFixedString(string value, int length)
            {
                var bytes = Encoding.Default.GetBytes(value);
                bw.Write(bytes, 0, Math.Min(bytes.Length, length));
                for (int i = bytes.Length; i < length; i++)
                    bw.Write((byte)0x00);
            }

            // Write the first map
            var firstMap = mapInstance.Playlists[mapInstance.ActivePlaylist][0];
            WriteFixedString(firstMap.MapFile!, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            string mapName = firstMap.MapName!;
            if (mapName.Length > 31)
                mapName = mapName.Substring(0, 31);
            WriteFixedString(mapName, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(firstMap.ModType==9 ? 1 : 0)); // Custom map flag
			bw.Write(new byte[24]); // adjust this padding as needed

            // Write additional maps
            for (int i = 1; i < mapInstance.Playlists[mapInstance.ActivePlaylist].Count; i++)
            {
                var map = mapInstance.Playlists[mapInstance.ActivePlaylist][i];
                WriteFixedString(map.MapFile!, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                string name = map.MapName!;
                if (name.Length > 31)
                    name = name.Substring(0, 31);
                WriteFixedString(name, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(map.ModType==9 ? 1 : 0));  // Custom map flag
                bw.Write(new byte[28]); // adjust this padding as needed
            }

            // Write to memory
            byte[] mapCycleClientBytes = ms.ToArray();
            int mapCycleClientWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleClientAddress, mapCycleClientBytes, mapCycleClientBytes.Length, ref mapCycleClientWritten);



            UpdateSecondaryMapList();
        }
        // Function: UpdateSecondaryMapList
        // Updates the secondary map list in the server memory
        public static void UpdateSecondaryMapList()
        {

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int mapCycleServerAddress = BitConverter.ToInt32(ServerMapCyclePtr, 0);

            int mapCycleTotalAddress = mapCycleServerAddress + 0x4;
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);


            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist].Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);


            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);


            for (int i = 0; i < mapInstance.Playlists[mapInstance.ActivePlaylist].Count; i++)
            {
                int mapFileIndexLocation = mapCycleList;
                byte[] mapFileBytes = new byte[0x20]; // 32 bytes
                byte[] nameBytes = Encoding.ASCII.GetBytes(mapInstance.Playlists[mapInstance.ActivePlaylist][i].MapFile!);
                Array.Copy(nameBytes, mapFileBytes, Math.Min(nameBytes.Length, mapFileBytes.Length));
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);
                mapFileIndexLocation += 0x20;

                byte[] customMapFlag = BitConverter.GetBytes(Convert.ToInt32(mapInstance.Playlists[mapInstance.ActivePlaylist][i].ModType==9?1:0));
                int customMapFlagWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, customMapFlag, customMapFlag.Length, ref customMapFlagWritten);
                mapCycleList += 0x24;
            }

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
        // Function: Update Start Delay Timer
        public static void ReadStartDelayTimer()
        {
            byte[] currentStartDelayCountBytes = new byte[4];
            int currentStartDelayCountRead = 0;

            int StartDelayCounterPtr = baseAddr + 0x5DAE04;

            ReadProcessMemory((int)processHandle, StartDelayCounterPtr, currentStartDelayCountBytes, currentStartDelayCountBytes.Length, ref currentStartDelayCountRead);

            int currentStartDelayCount = BitConverter.ToInt32(currentStartDelayCountBytes, 0);

            thisInstance.gameInfoStartDelayTimer = currentStartDelayCount;

        }

        // Function: Read NumTeam State
        public static bool ReadNumTeams()
        {
            byte[] NumTeamsBytes = new byte[4];
            int NumTeamsRead = 0;

            int NumTeamsPtr = 0x00A344C4;

            ReadProcessMemory((int)processHandle, NumTeamsPtr, NumTeamsBytes, NumTeamsBytes.Length, ref NumTeamsRead);

            int NumTeamsCount = BitConverter.ToInt32(NumTeamsBytes, 0);

            return (NumTeamsCount == 4);         

        }

        // Function: Update NumTeam State
        public static void UpdateNumTeams(bool isEnabled)
        {
            AppDebug.Log("Updating 4-Team Mode to: " + isEnabled, AppDebug.LogLevel.Info);

			var numTeams = 0x00A344C4;
            byte[] endTimerBytes = BitConverter.GetBytes(isEnabled ? 4 : 0);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, numTeams, endTimerBytes, endTimerBytes.Length, ref bytesWritten);

            AppDebug.Log($"4-Team Mode update complete. ({ReadNumTeams().ToString()})", AppDebug.LogLevel.Info);
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
		// Function: GetMapData, gets current and next map data from the server memory and updates the mapInstance accordingly
		public static void GetMapData()
        {

            // This will grab the current map index.
            var startingPtr = baseAddr + 0x005ED5F8;

            byte[] read_Ptr2Bytes = new byte[4];
            int read_Ptr2BytesRead = 0;
            ReadProcessMemory((int)processHandle, startingPtr, read_Ptr2Bytes, read_Ptr2Bytes.Length, ref read_Ptr2BytesRead);
            int Ptr2 = BitConverter.ToInt32(read_Ptr2Bytes, 0) + 0xC;

            byte[] CurrentMapIndexBytes = new byte[4];
            int CurrentMapIndexBytesRead = 0;
            ReadProcessMemory((int)processHandle, Ptr2, CurrentMapIndexBytes, CurrentMapIndexBytes.Length, ref CurrentMapIndexBytesRead);
            int currentMapIndex = BitConverter.ToInt32(CurrentMapIndexBytes, 0);
            
            if (currentMapIndex + 1 >= mapInstance.Playlists[mapInstance.ActivePlaylist].Count || mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex + 1] == null)
            {
                currentMapIndex = 0;
            }
            else
            {
                currentMapIndex++;
            }

            int currentMapType = CommonCore.instanceMaps!.CurrentGameType;
            int nextMapType = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapType;
            
            mapInstance.NextMapGameType = nextMapType;
            mapInstance.NextMapName = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapName!;
            mapInstance.NextMapFile = mapInstance.Playlists[mapInstance.ActivePlaylist][currentMapIndex].MapFile!;
            mapInstance.IsNextMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.NextMapFile);

            mapInstance.CurrentMapFile = mapInstance.Playlists[mapInstance.ActivePlaylist][mapInstance.ActualPlayingMapIndex].MapFile;
            mapInstance.IsCurrentMap4Team = mapInstanceManager.Is4TeamMap(mapInstance.CurrentMapFile);

		}
        public static void SetNextMapType()
        {
            try
            {
                // Change the MapType for the next map
                var CurrentGameTypeAddr = baseAddr + 0x5F21A4;
                byte[] nextMaptypeBytes = BitConverter.GetBytes(mapInstance.NextMapGameType);
                int nextMaptypeBytesWrite = 0;
                WriteProcessMemory((int)processHandle, CurrentGameTypeAddr, nextMaptypeBytes, nextMaptypeBytes.Length, ref nextMaptypeBytesWrite);

                // Determine if we need to enable/disable 4-team mode
                bool shouldEnable4Teams = thisInstance.gameEnableFourTeams && 
                                         mapInstance.IsNextMap4Team &&
                                         (mapInstance.NextMapGameType == 1 || mapInstance.NextMapGameType == 3 || mapInstance.NextMapGameType == 8); // TDM or TKOTH or FBL
                
				// Update the 4-team state in game memory
				UpdateNumTeams(shouldEnable4Teams);
                
				// Deal with the Players
				bool isCurrentMap4Team = thisInstance.gameEnableFourTeams && 
                                        mapInstance.IsCurrentMap4Team &&
                                        (CommonCore.instanceMaps!.CurrentGameType == 1 || CommonCore.instanceMaps!.CurrentGameType == 3 || CommonCore.instanceMaps!.CurrentGameType == 8);
                
                theInstanceManager.changeTeamGameMode(
                    CommonCore.instanceMaps!.CurrentGameType, 
                    mapInstance.NextMapGameType,
                    isCurrentMap4Team,
                    shouldEnable4Teams
                );

                // FILTER INVALID TEAM SWITCHES BEFORE APPLYING
                ValidateAndNormalizeTeamSwitches(shouldEnable4Teams);

                UpdatePlayerTeam();

            }
            catch (Exception ex)
            {
                AppDebug.Log("Something went wrong with ScoringProcessHandler", AppDebug.LogLevel.Error, ex);
            }
        }
        // Update the Game Score for the next map
        public static void UpdateGameScores()
        {

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
                    nextGameScore = thisInstance.gameScoreZoneTime;
                    break;
                // flag ball
                case 8:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = thisInstance.gameScoreFlags;
                    break;
                // all other game types...
                default:
                    startingPtr1 = baseAddr + 0x5F21AC;
                    startingPtr2 = baseAddr + 0x6034B8;
                    nextGameScore = thisInstance.gameScoreKills;
                    break;
            }
            byte[] nextGameScoreBytes = BitConverter.GetBytes(nextGameScore);
            int nextGameScoreWritten1 = 0;
            int nextGameScoreWritten2 = 0;
            WriteProcessMemory((int)processHandle, startingPtr1, nextGameScoreBytes, nextGameScoreBytes.Length, ref nextGameScoreWritten1);
            WriteProcessMemory((int)processHandle, startingPtr2, nextGameScoreBytes, nextGameScoreBytes.Length, ref nextGameScoreWritten2);

        }
        /// <summary>
        /// Validate and normalize team switches before applying them to game memory.
        /// Filters out invalid team numbers and normalizes teams that don't exist on the next map.
        /// </summary>
        private static void ValidateAndNormalizeTeamSwitches(bool nextMapIs4Team)
        {
            if (playerInstance.PlayerChangeTeamList.Count == 0)
                return;

            var toRemove = new List<playerTeamObject>();

            foreach (var teamSwitch in playerInstance.PlayerChangeTeamList)
            {
                // If next map is 2-team only, normalize team 3/4 to team 1/2
                if (!nextMapIs4Team && (teamSwitch.Team < 1 || teamSwitch.Team > 2))
                {
                    if (teamSwitch.Team == 3 || teamSwitch.Team == 4)
                    {
                        int normalizedTeam = ((teamSwitch.Team - 1) % 2) + 1; // Team 3→1, Team 4→2
                        teamSwitch.Team = normalizedTeam;
                    }
                    else
                    {
                        AppDebug.Log($"Invalid team number detected: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}. Removing from queue.", AppDebug.LogLevel.Warning);
                        toRemove.Add(teamSwitch);
                    }
                }
                // If next map is 4-team, validate team is 1-4
                else if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    AppDebug.Log($"Invalid team number detected: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}. Removing from queue.", AppDebug.LogLevel.Warning);
                    toRemove.Add(teamSwitch);
                }
            }

            // Remove invalid switches
            foreach (var item in toRemove)
            {
                playerInstance.PlayerChangeTeamList.Remove(item);
            }

            if (toRemove.Count > 0)
            {
                AppDebug.Log($"Filtered {toRemove.Count} invalid team switch(es). {playerInstance.PlayerChangeTeamList.Count} valid switch(es) remain.", AppDebug.LogLevel.Warning);
            }
        }

        // Function UpdatePlayerTeam
        public static void UpdatePlayerTeam()
        {
            if (playerInstance.PlayerChangeTeamList.Count == 0)
            {
                return;
            }

            // Determine if next map supports 4 teams (safety check)
            bool nextMapIs4Team = thisInstance.gameEnableFourTeams && 
                                 mapInstance.IsNextMap4Team &&
                                 (mapInstance.NextMapGameType == 1 || mapInstance.NextMapGameType == 3 || mapInstance.NextMapGameType == 8);

            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;
            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Iterate backwards to safely remove items during iteration
            for (int ii = playerInstance.PlayerChangeTeamList.Count - 1; ii >= 0; ii--)
            {
                var teamSwitch = playerInstance.PlayerChangeTeamList[ii];
                
                // SAFETY NET: Final validation before writing to memory
                if (!nextMapIs4Team && (teamSwitch.Team < 1 || teamSwitch.Team > 2))
                {
                    AppDebug.Log($"SAFETY: Skipping invalid team switch - Slot {teamSwitch.slotNum} → Team {teamSwitch.Team} " +
                                 $"(Next map '{mapInstance.NextMapName}' is 2-team only)", AppDebug.LogLevel.Warning);
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                if (teamSwitch.Team < 1 || teamSwitch.Team > 4)
                {
                    AppDebug.Log($"SAFETY: Skipping invalid team number - Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}", AppDebug.LogLevel.Warning);
                    playerInstance.PlayerChangeTeamList.RemoveAt(ii);
                    continue;
                }

                // Apply team switch to game memory
                int playerLocationOffset = (teamSwitch.slotNum - 1) * 0xAF33C;
                int playerLocation = playerlistStartingLocation + playerLocationOffset;
                int playerTeamLocation = playerLocation + 0x90;
                byte[] teamBytes = BitConverter.GetBytes(teamSwitch.Team);
                int bytesWritten = 0;
                WriteProcessMemory((int)processHandle, playerTeamLocation, teamBytes, teamBytes.Length, ref bytesWritten);
                
                AppDebug.Log($"Applied team switch: Slot {teamSwitch.slotNum} → Team {teamSwitch.Team}", AppDebug.LogLevel.Info);
                
                playerInstance.PlayerChangeTeamList.RemoveAt(ii);
            }
        }
        // Function: UpdatePlayMapNext
        public static void UpdateNextMap(int NextMapIndex)
        {

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int MapCycleIndex = BitConverter.ToInt32(ServerMapCyclePtr, 0) + 0xC;

            if (NextMapIndex - 1 == -1)
            {
                NextMapIndex = mapInstance.Playlists[mapInstance.ActivePlaylist].Count;
            }
            else if (mapInstance.Playlists[mapInstance.ActivePlaylist][NextMapIndex - 1] != null)
            {
                NextMapIndex--;
            }

            byte[] newMapIndexBytes = BitConverter.GetBytes(NextMapIndex);
            int newMapIndexBytesWritten = 0;
            WriteProcessMemory((int)processHandle, MapCycleIndex, newMapIndexBytes, newMapIndexBytes.Length, ref newMapIndexBytesWritten);


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
        // Function: WriteMemorySendChatMessage
        public static void WriteMemorySendChatMessage(int MsgLocation, string Msg)
        {
            int colorbuffer_written = 0;
            byte[] colorcode;

			/*
             * 6A 01 - White
			 * 6A 03 - Yellow
             * 6A 04 - Red
             * 6A 05 - Blue
             * 6A 08 - Server Orange (55)
             * 6A 09 - Server White
             */

            colorcode = Functions.ToByteArray($"6A 0{MsgLocation}".Replace(" ", ""));
            WriteProcessMemory((int)processHandle, 0x00462ABA, colorcode, colorcode.Length, ref colorbuffer_written);

            // post message
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, chatConsole, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, chatConsole, 0);
            Thread.Sleep(50);
            int bytesWritten = 0;
            byte[] buffer;
            buffer = Encoding.GetEncoding(1252).GetBytes($"{Msg}\0"); // '\0' marks the end of string
            WriteProcessMemory((int)processHandle, 0x00879A14, buffer, buffer.Length, ref bytesWritten);
            Thread.Sleep(50);
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, VK_ENTER, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, VK_ENTER, 0);

            // change color to normal
            Thread.Sleep(50);
            int revert_colorbuffer = 0;
            byte[] revert_colorcode = Functions.ToByteArray("6A 01".Replace(" ", ""));
            WriteProcessMemory((int)processHandle, 0x00462ABA, revert_colorcode, revert_colorcode.Length, ref revert_colorbuffer);
   
        }
        // Function: WriteMemoryChatCountDownKiller
        public static void WriteMemoryChatCountDownKiller(int ChatLogAddr)
        {


            byte[] countDownKiller = BitConverter.GetBytes(0);
            int countDownKillerWrite = 0;
            WriteProcessMemory((int)processHandle, ChatLogAddr + 0x7C, countDownKiller, countDownKiller.Length, ref countDownKillerWrite);


        }
        // Function: WriteMemoryDisarmPlayer
        public static void WriteMemoryDisarmPlayer(int playerSlot)
        {


            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] disablePlayerWeapon = BitConverter.GetBytes(0);
            int disablePlayerWeaponWrite = 0;
            WriteProcessMemory((int)processHandle, playerNewLocationAddress + 0xADE08, disablePlayerWeapon, disablePlayerWeapon.Length, ref disablePlayerWeaponWrite);


        }
        // Function: WriteMemoryArmPlayer
        public static void WriteMemoryArmPlayer(int playerSlot)
        {

            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] disablePlayerWeapon = BitConverter.GetBytes(1);
            int disablePlayerWeaponWrite = 0;
            WriteProcessMemory((int)processHandle, playerNewLocationAddress + 0xADE08, disablePlayerWeapon, disablePlayerWeapon.Length, ref disablePlayerWeaponWrite);


        }
        // Function: WriteMemoryKillPlayer
        public static void WriteMemoryKillPlayer(int playerSlot)
        {
            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;
            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            byte[] setPlayerHealth = BitConverter.GetBytes(0);
            int setPlayerHealthWrite = 0;

            WriteProcessMemory((int)processHandle, playerObjectLocation + 0x138, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);
            WriteProcessMemory((int)processHandle, playerObjectLocation + 0xE2, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);


        }
        // Function: WriteMemoryTogglePlayerGodMode
        public static void WriteMemoryTogglePlayerGodMode(int playerSlot, int health)
        {


            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory PlayerIPAddress from the game...
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;
            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Directly calculate the player's PlayerIPAddress
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            byte[] setPlayerHealth = BitConverter.GetBytes(health); //set god mode health
            int setPlayerHealthWrite = 0;

            byte[] setDamageBy = BitConverter.GetBytes(0);
            int setDamageByWrite = 0;
            
            WriteProcessMemory((int)processHandle, playerObjectLocation + 0x138, setDamageBy, setDamageBy.Length, ref setDamageByWrite);
            WriteProcessMemory((int)processHandle, playerObjectLocation + 0xE2, setPlayerHealth, setPlayerHealth.Length, ref setPlayerHealthWrite);


        }
        // Function: WriteMemorySendConsoleCommand
        public static void WriteMemorySendConsoleCommand(string Command)
        {


            // open cmdConsole
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, cmdConsole, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, cmdConsole, 0);
            Thread.Sleep(100);

            // Write to cmdConsole
            int bytesWritten = 0;
            byte[] buffer = Encoding.GetEncoding(1252).GetBytes($"{Command}\0"); // '\0' marks the end of string

            WriteProcessMemory((int)processHandle, 0x00879A14, buffer, buffer.Length, ref bytesWritten);
            Thread.Sleep(100);
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, VK_ENTER, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, VK_ENTER, 0);


        }


        //
        // Read Memory
        // Description: Reads memory from the game process only.
        //

        // Function: ReadMemoryIsProcessAttached
        public static bool ReadMemoryIsProcessAttached()
        {
            // Check if PID and process handle are set
            if (thisInstance.instanceAttachedPID == null || thisInstance.instanceAttachedPID == 0 || processHandle == nint.Zero)
            {
                return false;
            }

            try
            {
                // Try to get the process by PID
                var process = Process.GetProcessById(thisInstance.instanceAttachedPID.Value);

                // Check if the process path matches profileServerPath (if needed)
                if (!string.IsNullOrEmpty(thisInstance.profileServerPath))
                {
                    // May throw if process has exited or access denied
                    string processPath = process.MainModule?.FileName ?? string.Empty;
                    if (!processPath.Equals(thisInstance.profileServerPath + "\\dfbhd.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        thisInstance.instanceAttachedPID = null;
                        processHandle = nint.Zero; // Replace 'null' with 'IntPtr.Zero' for nint type
                        return false;
                    }
                }

                // If we got here, process is running and matches
                return true;
            }
            catch (Exception ex)
            {
                // Process does not exist or access denied
                thisInstance.instanceAttachedPID = null;
                processHandle = nint.Zero; // Replace 'null' with 'IntPtr.Zero' for nint type
                AppDebug.Log("Process not found or access denied.", AppDebug.LogLevel.Error, ex);
                return false;
            }
        }
        // Function: ReadMemoryServerStatus
        public static void ReadMemoryServerStatus()
        {

            var startingPointer = baseAddr + 0x00098334;
            byte[] startingPointerBuffer = new byte[4];
            int startingPointerReadBytes = 0;
            ReadProcessMemory((int)processHandle, startingPointer, startingPointerBuffer, startingPointerBuffer.Length, ref startingPointerReadBytes);

            int statusLocationPointer = BitConverter.ToInt32(startingPointerBuffer, 0);
            byte[] statusLocation = new byte[4];
            int statusLocationReadBytes = 0;
            ReadProcessMemory((int)processHandle, statusLocationPointer, statusLocation, statusLocation.Length, ref statusLocationReadBytes);
            int instanceStatus = BitConverter.ToInt32(statusLocation, 0);

            thisInstance.instanceStatus = (InstanceStatus)instanceStatus;


        }
        // Function: ReadMemoryGameTimeLeft
        public static void ReadMemoryGameTimeLeft()
        {
            if (thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                thisInstance.gameInfoTimeRemaining = TimeSpan.FromMinutes(thisInstance.gameStartDelay + thisInstance.gameTimeLimit);
                return;
            }

            // Read pointer to map time
            byte[] ptr = new byte[4];
            int readPtr = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x00061098, ptr, ptr.Length, ref readPtr);
            int mapTimeAddr = BitConverter.ToInt32(ptr, 0);

            // Read elapsed map time in game ticks (assumed to be milliseconds)
            byte[] mapTimeMs = new byte[4];
            int mapTimeRead = 0;
            ReadProcessMemory((int)processHandle, mapTimeAddr, mapTimeMs, mapTimeMs.Length, ref mapTimeRead);
            int mapTime = BitConverter.ToInt32(mapTimeMs, 0);

            // Convert to seconds (if value is in milliseconds, otherwise adjust as needed)
            int mapTimeInSeconds = mapTime / 60;

            // Calculate total time in seconds
            int totalTimeInSeconds = (thisInstance.gameStartDelay + thisInstance.gameTimeLimit) * 60;

            // Calculate time remaining
            int timeRemainingSeconds = totalTimeInSeconds - mapTimeInSeconds;
            if (timeRemainingSeconds < 0) timeRemainingSeconds = 0;

            thisInstance.gameInfoTimeRemaining = TimeSpan.FromSeconds(timeRemainingSeconds);
        }

        // Function: ReadMemoryCurrentMissionName
        public static void ReadMemoryCurrentMissionName()
        {


            // memory polling
            int bytesRead = 0;
            byte[] buffer = new byte[26];
            ReadProcessMemory((int)processHandle, 0x0071569C, buffer, buffer.Length, ref bytesRead);
            string MissionName = Encoding.GetEncoding(1252).GetString(buffer);
            mapInstance.CurrentMapName = MissionName.Replace("\0", "");


        }
        // Fuction: ReadMemoryCurrentGameType
        public static void ReadMemoryCurrentGameType()
        {


            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, 0x009F21A4, buffer, buffer.Length, ref bytesRead);
            int GameType = BitConverter.ToInt32(buffer, 0);
            CommonCore.instanceMaps!.CurrentGameType = GameType;

        }
        // FunctionL ReadMemoryCurrentMapIndex
        public static void ReadMemoryCurrentMapIndex()
        {
            if (thisInstance.instanceStatus != InstanceStatus.STARTDELAY &&
                thisInstance.instanceStatus != InstanceStatus.ONLINE)
            {
                return;
            }

            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int MapCycleIndex = BitConverter.ToInt32(ServerMapCyclePtr, 0) + 0xC;
            byte[] mapIndexBytes = new byte[4];
            int mapIndexRead = 0;
            ReadProcessMemory((int)processHandle, MapCycleIndex, mapIndexBytes, mapIndexBytes.Length, ref mapIndexRead);
            mapInstance.CurrentMapIndex = BitConverter.ToInt32(mapIndexBytes, 0);


        }
        // Function: ReadMemoryWinningTeam
        public static void ReadMemoryWinningTeam()
        {

            int bytesRead = 8;
            byte[] buffer = new byte[8];
            ReadProcessMemory((int)processHandle, 0x009F370C, buffer, buffer.Length, ref bytesRead);
            int gameMatchWinner = BitConverter.ToInt32(buffer, 0);
            thisInstance.gameMatchWinner = gameMatchWinner;


        }
        // Function: ReadMemoryCurrentNumPlayers
        public static void ReadMemoryCurrentNumPlayers()
        {


            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, 0x0065DCBC, buffer, buffer.Length, ref bytesRead);
            int CurrentPlayers = BitConverter.ToInt32(buffer, 0);
            thisInstance.gameInfoNumPlayers = CurrentPlayers;


        }
        // Function: ReadMemoryGeneratePlayerList
        public static void ReadMemoryGeneratePlayerList()
        {
            if (thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            Dictionary<int, PlayerObject> currentPlayerList = new Dictionary<int, PlayerObject>();
            int NumPlayers = thisInstance.gameInfoNumPlayers;

            if (NumPlayers > 0)
            {

                int buffer = 0;
                var Pointer = baseAddr + 0x005ED600;

                byte[] PointerAddr9 = new byte[4];
                ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
                var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;

                byte[] playerListStartingLocationByteArray = new byte[4];
                ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref buffer);

                int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);
                int failureCount = 0;

                for (int i = 0; i < NumPlayers; i++)
                {
                    if (failureCount == thisInstance.gameMaxSlots)
                    {
                        break;
                    }

                    byte[] slotNumberValue = new byte[4];
                    int slotNumberLocation = playerlistStartingLocation + 0xC;
                    ReadProcessMemory((int)processHandle, slotNumberLocation, slotNumberValue, slotNumberValue.Length, ref buffer);
                    int playerSlot = BitConverter.ToInt32(slotNumberValue, 0);

                    byte[] playerNameBytes = new byte[15];
                    int playerNameLocation = playerlistStartingLocation + 0x1C;
                    ReadProcessMemory((int)processHandle, playerNameLocation, playerNameBytes, playerNameBytes.Length, ref buffer);
                    string formattedPlayerName = Encoding.GetEncoding("Windows-1252").GetString(playerNameBytes).Replace("\0", "");

                    if (string.IsNullOrEmpty(formattedPlayerName) || string.IsNullOrWhiteSpace(formattedPlayerName))
                    {
                        playerlistStartingLocation += 0xAF33C;
                        i--;
                        failureCount++;
                        continue;
                    }

                    byte[] playerTeamBytes = new byte[4];
                    int playerTeamLocation = playerlistStartingLocation + 0x90;
                    ReadProcessMemory((int)processHandle, playerTeamLocation, playerTeamBytes, playerTeamBytes.Length, ref buffer);
                    int playerTeam = BitConverter.ToInt32(playerTeamBytes, 0);
                    string playerIP = ReadMemoryGrabPlayerIPAddress(formattedPlayerName).ToString();

                    PlayerObject PlayerStats = ReadMemoryPlayerStats(playerSlot);
                    CharacterClass PlayerCharacterClass = (CharacterClass)PlayerStats.RoleID;
                    WeaponStack PlayerSelectedWeapon = (WeaponStack)PlayerStats.SelectedWeaponID;

                    Dictionary<int, List<WeaponStack>> PlayerWeapons = new Dictionary<int, List<WeaponStack>>();

                    if (string.IsNullOrEmpty(formattedPlayerName) || string.IsNullOrWhiteSpace(formattedPlayerName))
                    {
                        if (currentPlayerList.Count >= NumPlayers)
                        {
                            break;
                        }
                        else
                        {
                            playerlistStartingLocation += 0xAF33C;
                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            // Try to preserve PlayerJoined and CountryCode if player already exists in the persistent list
                            if (playerInstance.PlayerList.TryGetValue(playerSlot, out var existingPlayer))
                            {
                                PlayerStats.PlayerJoined = existingPlayer.PlayerJoined;
                                PlayerStats.CountryCode = existingPlayer.CountryCode;
                            }
                            // Final Touches
                            PlayerStats.PlayerLastSeen = DateTime.Now;
                            PlayerStats.PlayerTimePlayed = (int)(PlayerStats.PlayerLastSeen - PlayerStats.PlayerJoined).TotalSeconds;
                            PlayerStats.PlayerSlot = playerSlot;
                            byte[] trimmedPlayerNameBytes = playerNameBytes.Where(b => b != 0).ToArray();
                            PlayerStats.PlayerNameBase64 = Convert.ToBase64String(trimmedPlayerNameBytes);
                            PlayerStats.PlayerName = formattedPlayerName;
                            PlayerStats.PlayerTeam = playerTeam;
                            PlayerStats.PlayerIPAddress = playerIP;
                            PlayerStats.PlayerPing = PlayerStats.PlayerPing;
                            PlayerStats.RoleName = PlayerCharacterClass.ToString();
                            PlayerStats.SelectedWeaponName = PlayerSelectedWeapon.ToString();

                            PlayerStats = PlayerObjectSanitizer.Sanitize(PlayerStats);

                            // Push to List
                            currentPlayerList.Add(playerSlot, PlayerStats);

                            // Setup for Next Player
                            playerlistStartingLocation += 0xAF33C;

                            if (currentPlayerList.Count >= NumPlayers)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            AppDebug.Log("Detected an error!\n\n" + "Player Name: " + playerSlot + "\n\n" + formattedPlayerName + "\n\n", AppDebug.LogLevel.Error, ex);
                        }

                    }

                }


            }
            playerInstance.PlayerList.Clear();
            foreach (var kvp in currentPlayerList)
            {
                playerInstance.PlayerList[kvp.Key] = kvp.Value;
            }
        }
        // Function: ReadMemoryGrabPlayerIPAddress
        public static string ReadMemoryGrabPlayerIPAddress(string playername)
        {


            const int playerIpAddressPointerOffset = 0x00ACE248;
            const int playernameOffset = 0xBC;

            int playerIpAddressPointerBuffer = 0;
            byte[] PointerAddr_2 = new byte[4];
            ReadProcessMemory((int)processHandle, baseAddr + playerIpAddressPointerOffset, PointerAddr_2, PointerAddr_2.Length, ref playerIpAddressPointerBuffer);

            int IPList = BitConverter.ToInt32(PointerAddr_2, 0) + playernameOffset;

            int failureCounter = 0;
            while (failureCounter <= thisInstance.gameMaxSlots)
            {
                byte[] playername_bytes = new byte[15];
                int playername_buffer = 0;
                ReadProcessMemory((int)processHandle, IPList, playername_bytes, playername_bytes.Length, ref playername_buffer);
                var currentPlayerName = Encoding.GetEncoding("Windows-1252").GetString(playername_bytes).Replace("\0", "");

                if (currentPlayerName == playername)
                {
                    failureCounter = 0;
                    break;
                }

                IPList += playernameOffset;
                failureCounter++;
            }

            if (failureCounter > thisInstance.gameMaxSlots)
            {

                return null!;
            }

            byte[] playerIPBytesPtr = new byte[4];
            int playerIPBufferPtr = 0;
            ReadProcessMemory((int)processHandle, IPList + 0xA4, playerIPBytesPtr, playerIPBytesPtr.Length, ref playerIPBufferPtr);

            int PlayerIPLocation = BitConverter.ToInt32(playerIPBytesPtr, 0) + 4;
            byte[] playerIPAddressBytes = new byte[4];
            int playerIPAddressBuffer = 0;
            ReadProcessMemory((int)processHandle, PlayerIPLocation, playerIPAddressBytes, playerIPAddressBytes.Length, ref playerIPAddressBuffer);

            IPAddress playerIp = new IPAddress(playerIPAddressBytes);

            return playerIp.ToString();
        }
        // Function: ReadMemoryPlayerStats
        public static PlayerObject ReadMemoryPlayerStats(int reqslot)
        {


            var baseaddr = 0x400000;
            var startList = baseaddr + 0x005ED600;

            byte[] startaddr = new byte[4];
            int startaddr_read = 0;
            ReadProcessMemory((int)processHandle, startList, startaddr, startaddr.Length, ref startaddr_read);
            var firstplayer = BitConverter.ToInt32(startaddr, 0) + 0x28;

            byte[] scanbeginaddr = new byte[4];
            ReadProcessMemory((int)processHandle, firstplayer, scanbeginaddr, scanbeginaddr.Length, ref startaddr_read);
            int beginaddr = BitConverter.ToInt32(scanbeginaddr, 0);

            if (reqslot != 1)
            {
                for (int i = 1; i < reqslot; i++)
                {
                    beginaddr += 0xAF33C;
                }
            }

            int playerMemStart = beginaddr;

            byte[] read_name = new byte[15];
            int bytesread = 0;

            ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
            var PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name).Replace("\0", "");

            if (string.IsNullOrEmpty(PlayerName))
            {
                beginaddr += 0xAF33C;
                // Retry read if player name is empty
                ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
                PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name).Replace("\0", "");
            }

            // Handle failure if still no player name found
            if (string.IsNullOrEmpty(PlayerName))
            {

                AppDebug.Log("Something went wrong here. We can't find any player names.", AppDebug.LogLevel.Info);
                return new PlayerObject();
            }

            byte[] read_ping = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB40, read_ping, read_ping.Length, ref bytesread);

            int[] offsets = {
                0x000ADAB4, // stat_TotalShotsFired
                0x000ADA94, // stat_Kills
                0x000ADA98, // stat_Deaths
                0x000ADA8C, // stat_Suicides
                0x000ADAD0, // stat_Headshots
                0x000ADA90, // stat_Murders
                0x000ADAD4, // stat_KnifeKills
                0x000ADAF4, // stat_ExperiencePoints
                0x000ADABC, // stat_RevivesReceived
                0x000ADAC0, // stat_PSPAttempts
                0x000ADAC4, // stat_PSPTakeovers
                0x000ADACC, // stat_DoubleKills
                0x000ADACC, // stat_RevivesGiven
                0x000ADAA8, // stat_FBCaptures
                0x000ADAC8, // stat_FBCarrierKills
                0x000ADAD4, // stat_FBCarrierDeaths
                0x000ADAA4, // stat_ZoneTime
                0x000ADADC, // stat_ZoneKills
                0x000ADA94, // stat_ZoneDefendKills
                0x000ADAB0, // stat_SDADTargetsDestroyed
                0x000ADAAC, // stat_FlagSaves
                0x000ADAD8, // stat_SniperKills
                0x000ADADC, // stat_TKOTHDefenseKills
                0x000ADAE0, // stat_TKOTHAttackKills
                0x000ADAE4, // stat_SDADDefendKill
                0x000ADAEC, // stat_SDADAttackKill
            };

            var stats = new int[offsets.Length];

            for (int i = 0; i < offsets.Length; i++)
            {
                byte[] read_data = new byte[4];
                ReadProcessMemory((int)processHandle, beginaddr + offsets[i], read_data, read_data.Length, ref bytesread);
                stats[i] = BitConverter.ToInt32(read_data, 0);
            }

            // Trail Checks
            int[] offsets2 =
            {
                // 20 fields before the first offset
                0x000ADA8C - 80, 0x000ADA8C - 76, 0x000ADA8C - 72, 0x000ADA8C - 68, 0x000ADA8C - 64,
                0x000ADA8C - 60, 0x000ADA8C - 56, 0x000ADA8C - 52, 0x000ADA8C - 48, 0x000ADA8C - 44,
                0x000ADA8C - 40, 0x000ADA8C - 36, 0x000ADA8C - 32, 0x000ADA8C - 28, 0x000ADA8C - 24,
                0x000ADA8C - 20, 0x000ADA8C - 16, 0x000ADA8C - 12, 0x000ADA8C - 8,  0x000ADA8C - 4,

                // All offsets between the lowest and highest, in 4-byte increments
                // From 0x000ADA8C to 0x000ADAF4
                0x000ADA8C, 0x000ADA90, 0x000ADA94, 0x000ADA98, 0x000ADA9C,
                0x000ADAA0, 0x000ADAA4, 0x000ADAA8, 0x000ADAAC, 0x000ADAB0,
                0x000ADAB4, 0x000ADAB8, 0x000ADABC, 0x000ADAC0, 0x000ADAC4,
                0x000ADAC8, 0x000ADACC, 0x000ADAD0, 0x000ADAD4, 0x000ADAD8,
                0x000ADADC, 0x000ADAE0, 0x000ADAE4, 0x000ADAE8, 0x000ADAEC,
                0x000ADAF0, 0x000ADAF4,

                // 20 fields after the last offset
                0x000ADAF4 + 4,  0x000ADAF4 + 8,  0x000ADAF4 + 12, 0x000ADAF4 + 16, 0x000ADAF4 + 20,
                0x000ADAF4 + 24, 0x000ADAF4 + 28, 0x000ADAF4 + 32, 0x000ADAF4 + 36, 0x000ADAF4 + 40,
                0x000ADAF4 + 44, 0x000ADAF4 + 48, 0x000ADAF4 + 52, 0x000ADAF4 + 56, 0x000ADAF4 + 60,
                0x000ADAF4 + 64, 0x000ADAF4 + 68, 0x000ADAF4 + 72, 0x000ADAF4 + 76, 0x000ADAF4 + 80
            };

            // Remove offsets in offsets2 that are present in offsets
            int[] filteredOffsets2 = offsets2.Except(offsets).ToArray();

            var stats2 = new int[filteredOffsets2.Length];
            for (int i = 0; i < filteredOffsets2.Length; i++)
            {
                byte[] read_data = new byte[4];
                ReadProcessMemory((int)processHandle, beginaddr + filteredOffsets2[i], read_data, read_data.Length, ref bytesread);
                stats2[i] = BitConverter.ToInt32(read_data, 0);
            }

            // For offsets2, skip entries where stats2[i] == 0
            string offsets2Log = string.Join(", ",
                filteredOffsets2
                    .Select((offset, i) => (offset, value: stats2[i]))
                    .Where(pair => pair.value != 0)
                    .Select(pair => $"{beginaddr + pair.offset:X8}:{pair.offset:X8} => {pair.value}\n")
            );
            // AppDebug.Log("PlayerStats", $"{PlayerName} :" + offsets2Log);
            
            // Score Offsets
            string offsetsLog = string.Join(", ", offsets.Select((offset, i) => $"{beginaddr + offset:X8}:{offset:X8} => {stats[i]}\n"));
            // AppDebug.Log("PlayerStats", $"{PlayerName} :" + offsetsLog);

            // Read Player Flag Time
            byte[] read_playerActiveZoneTimeByte = new byte[4];
            int activeZoneTimerLoc = playerMemStart + 0xADB2C;
            ReadProcessMemory((int)processHandle, activeZoneTimerLoc, read_playerActiveZoneTimeByte, read_playerActiveZoneTimeByte.Length, ref bytesread);
            int read_playerActiveZoneTimeInt = BitConverter.ToInt32(read_playerActiveZoneTimeByte, 0);

            // Read Player Object
            byte[] read_playerObjectLocation = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x5E7C, read_playerObjectLocation, read_playerObjectLocation.Length, ref bytesread);
            int read_playerObject = BitConverter.ToInt32(read_playerObjectLocation, 0);

            // Position X, Y, Z
            byte[] posBytes = new byte[12];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x08, posBytes, posBytes.Length, ref bytesread);
            float posX = BitConverter.ToSingle(posBytes, 0);
            float posY = BitConverter.ToSingle(posBytes, 4);
            float posZ = BitConverter.ToSingle(posBytes, 8);

            // Facing Yaw and Facing Pitch
            byte[] angBytes = new byte[8];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x14, angBytes, angBytes.Length, ref bytesread);
            int angA = BitConverter.ToInt32(angBytes, 0);
            int angB = BitConverter.ToInt32(angBytes, 4);

            // Convert engine-encoded ints to usable angles if needed (implement conversion).
            (float yaw, float pitch) = ConvertEngineAngles(angA, angB);

            // Player Health (read from player object; writer uses +0xE2)
            byte[] read_health = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0xE2, read_health, read_health.Length, ref bytesread);
            int PlayerHealth = BitConverter.ToInt32(read_health, 0);

            // Selected Weapon & Character Class
            byte[] read_selectedWeapon = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x178, read_selectedWeapon, read_selectedWeapon.Length, ref bytesread);
            int SelectedWeapon = BitConverter.ToInt32(read_selectedWeapon, 0);

            byte[] read_selectedCharacterClass = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x244, read_selectedCharacterClass, read_selectedCharacterClass.Length, ref bytesread);
            int SelectedCharacterClass = BitConverter.ToInt32(read_selectedCharacterClass, 0);
            
            // Weapons
            byte[] read_weapons = new byte[250];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB70, read_weapons, read_weapons.Length, ref bytesread);
            var MemoryWeapons = Encoding.GetEncoding(1252).GetString(read_weapons).Replace("\0", "|");
            string[] weapons = MemoryWeapons.Split('|');
            List<string> WeaponList = new List<string>();

            int failureCount = 0;
            foreach (var item in weapons)
            {
                if (!string.IsNullOrEmpty(item) && failureCount != 3)
                {
                    WeaponList.Add(item);
                }
                else
                {
                    if (failureCount == 3)
                    {
                        break;
                    }
                    else
                    {
                        failureCount++;
                    }
                }
            }

            return new PlayerObject
            {
                PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name),
                PlayerPing = BitConverter.ToInt32(read_ping, 0),
                RoleID = SelectedCharacterClass,
                SelectedWeaponID = SelectedWeapon,
                PlayerWeapons = WeaponList,
                ActiveZoneTime = read_playerActiveZoneTimeInt,
                PlayerHealth = PlayerHealth,
                PosX = posX,
                PosY = posY,
                PosZ = posZ,
                FacingYaw = yaw,
                FacingPitch = pitch,
                stat_TotalShotsFired = stats[0],
                stat_Kills = stats[1],
                stat_Deaths = stats[2],
                stat_Suicides = stats[3],
                stat_Headshots = stats[4],
                stat_Murders = stats[5],
                stat_KnifeKills = stats[6],
                stat_ExperiencePoints = stats[7],
                stat_RevivesReceived = stats[8],
                stat_PSPAttempts = stats[9],
                stat_PSPTakeovers = stats[10],
                stat_DoubleKills = stats[11],
                stat_RevivesGiven = stats[12],
                stat_FBCaptures = stats[13],
                stat_FBCarrierKills = stats[14],
                stat_FBCarrierDeaths = stats[15],
                stat_ZoneTime = stats[16],
                stat_ZoneKills = stats[17],
                stat_ZoneDefendKills = stats[18],
                stat_SDADTargetsDestroyed = stats[19],
                stat_FlagSaves = stats[20],
                stat_SniperKills = stats[21],
                stat_TKOTHDefenseKills = stats[22],
                stat_TKOTHAttackKills = stats[23],
                stat_SDADDefenseKills = stats[24],
                stat_SDADAttackKills = stats[25]
            };
        }

        public static (int rawA, int rawB, float degA, float degB) ConvertEngineAngleSingle(int encoded)
        {
            // Table addresses discovered in HLIL
            const int TABLE1_ADDR = 0x00E920E8; // data_e920e8
            const int TABLE2_ADDR = 0x00E920EC; // data_e920ec
            const int DATA_654CF4_PTR = 0x00654CF4; // contains pointer to table used for second output

            int bytesread = 0;

            int masked = encoded & 0x3FFFFF;
            uint idx = (uint)encoded >> 0x16;

            byte[] buf = new byte[4];

            // read baseA
            ReadProcessMemory((int)processHandle, TABLE1_ADDR + (int)(idx * 4), buf, buf.Length, ref bytesread);
            uint baseA = BitConverter.ToUInt32(buf, 0);

            // read deltaA
            ReadProcessMemory((int)processHandle, TABLE2_ADDR + (int)(idx * 4), buf, buf.Length, ref bytesread);
            uint deltaA = BitConverter.ToUInt32(buf, 0);

            long prod = (long)masked * (long)((int)deltaA - (int)baseA);
            uint low = (uint)prod;
            uint high = (uint)((ulong)prod >> 32);
            uint valA = ((low >> 0x16) | (high << 0x0a)) + baseA;

            // read pointer to table3 from data_654cf4 variable
            ReadProcessMemory((int)processHandle, DATA_654CF4_PTR, buf, buf.Length, ref bytesread);
            uint table3base = BitConverter.ToUInt32(buf, 0);

            // read baseB and deltaB from table3
            ReadProcessMemory((int)processHandle, (int)table3base + (int)(idx * 4), buf, buf.Length, ref bytesread);
            uint baseB = BitConverter.ToUInt32(buf, 0);
            ReadProcessMemory((int)processHandle, (int)table3base + (int)(idx * 4) + 4, buf, buf.Length, ref bytesread);
            uint deltaB = BitConverter.ToUInt32(buf, 0);

            long prod2 = (long)masked * (long)((int)deltaB - (int)baseB);
            uint low2 = (uint)prod2;
            uint high2 = (uint)((ulong)prod2 >> 32);
            uint valB = ((low2 >> 0x16) | (high2 << 0x0a)) + baseB;

            // Heuristic conversion to degrees (engine appears to use fixed-point scaling)
            const float DEG_SCALE = 360.0f / 65536.0f;
            float degA = (int)valA * DEG_SCALE;
            float degB = (int)valB * DEG_SCALE;

            return ((int)valA, (int)valB, degA, degB);
        }

        // Overload used by ReadMemoryPlayerStats which provides two encoded ints (yaw, pitch)
        public static (float yaw, float pitch) ConvertEngineAngles(int angA, int angB)
        {
            var a = ConvertEngineAngleSingle(angA);
            var b = ConvertEngineAngleSingle(angB);

            // Use the first degree component from each conversion as the primary angle
            return (a.degA, b.degA);
        }

        // Function: ReadMemoryLastChatMessage
        public static string[] ReadMemoryLastChatMessage()
        {

            var starterPtr = baseAddr + 0x00062D10;
            byte[] ChatLogPtr = new byte[4];
            int ChatLogPtrRead = 0;
            ReadProcessMemory((int)processHandle, starterPtr, ChatLogPtr, ChatLogPtr.Length, ref ChatLogPtrRead);

            // get last message sent...
            int ChatLogAddr = BitConverter.ToInt32(ChatLogPtr, 0);

            byte[] Message = new byte[74];
            int MessageRead = 0;
            ReadProcessMemory((int)processHandle, ChatLogAddr, Message, Message.Length, ref MessageRead);
            string LastMessage = Encoding.GetEncoding(1252).GetString(Message).Replace("\0", "");

            int msgTypeAddr = ChatLogAddr + 0x78;
            byte[] msgType = new byte[4];
            int msgTypeRead = 0;
            ReadProcessMemory((int)processHandle, msgTypeAddr, msgType, msgType.Length, ref msgTypeRead);
            string msgTypeBytes = BitConverter.ToString(msgType).Replace("-", "");

            return new string[] { ChatLogAddr.ToString(), LastMessage, msgTypeBytes };
        }

        public static void ReadMemoryCurrentGameWinConditions()
        {
            int scoreAddress1 = 0;
            int scoreAddress2 = 0;
            int gameTypeId = CommonCore.instanceMaps!.CurrentGameType;

            // CommonCore.instanceMaps!.CurrentGameType
            switch (gameTypeId)
            {
                // KOTH/TKOTH
                case 3:
                case 4:
                    scoreAddress1 = baseAddr + 0x5F21B8;
                    scoreAddress2 = baseAddr + 0x6344B4;
                    break;
                // SD & AD
                case 5:
                case 6:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // SD = Both Teams have a "Win Condition" (Targets Destroyed)
                    // AD = Only the Attacking Team has a "Win Condition" (Targets Destroyed), Defending Team has to last the timer.
                    scoreAddress1 = baseAddr + 0x5DDCAC; // Blue
                    scoreAddress2 = baseAddr + 0x5DDCB0; // Red
                    break;
                // CTF
                case 7:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // Based on the number of red and blue flags in map, ideally both addresses should be the same.
                    scoreAddress1 = baseAddr + 0x5DDCA4; // Blue
                    scoreAddress2 = baseAddr + 0x5DDCA8; // Red
                    break;
                // flag ball
                case 8:
                    scoreAddress1 = baseAddr + 0x5F21AC;
                    scoreAddress2 = baseAddr + 0x6034B8;
                    break;
                // all other game types...
                default:
                    scoreAddress1 = baseAddr + 0x5F21AC;
                    scoreAddress2 = baseAddr + 0x6034B8;
                    break;
            }

            // Blue Win Condition
            byte[] scoreBytes = new byte[4];
            int bytesRead = 0;
            bool success = ReadProcessMemory((int)processHandle, scoreAddress1, scoreBytes, scoreBytes.Length, ref bytesRead);
            int winConditionBlue = (success ? BitConverter.ToInt32(scoreBytes, 0) : 0);

            // Red Win Condition
            byte[] score2Bytes = new byte[4];
            int bytes2Read = 0;
            bool success2 = ReadProcessMemory((int)processHandle, scoreAddress2, score2Bytes, score2Bytes.Length, ref bytes2Read);
            int winConditionRed = (success2 ? BitConverter.ToInt32(score2Bytes, 0) : 0);

            if (winConditionBlue == winConditionRed || winConditionBlue > winConditionRed)
            {
                if (gameTypeId == 6) { thisInstance.gameInfoIsBlueDefending = true; }
                thisInstance.gameInfoWinCond = winConditionBlue;
            }
            else if (winConditionBlue < winConditionRed)
            {
                if (gameTypeId == 6) { thisInstance.gameInfoIsBlueDefending = false; }
                thisInstance.gameInfoWinCond = winConditionRed;
            }
            else
            {
                // Should never hit this, but just in case...
                thisInstance.gameInfoWinCond = winConditionRed;
            }
            
        }

        public static void ReadMemoryCurrentGameScores()
        {
            // DM = First Player to Reach Win Condition
            //    Return 0
            // KOTH = First Team to Reach Win Condition
            //    Return 0
            // COOP = NA
            //    Return 0
            // TDM = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E07EC
            //     RED OFFSET  = 0x5E08D4
            // TKOTH = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0884
            //     RED OFFSET  = 0x5E096C
            // FB/CTF = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0800
            //     RED OFFSET  = 0x5E08E8
            // SD/AD = First Team to Reach Win Condition
            //     BLUE OFFSET = 0x5E0808
            //     RED OFFSET  = 0x5E08F0

            int scoreAddress1 = 0;
            int scoreAddress2 = 0;
            int gameTypeId = CommonCore.instanceMaps!.CurrentGameType;

            switch (gameTypeId)
            {
                // KOTH/TKOTH
                case 3:
                    scoreAddress1 = baseAddr + 0x5E0884; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E096C; // Red Score
                    break;
                // SD & AD
                case 5:
                case 6:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // SD = Both Teams have a "Win Condition" (Targets Destroyed)
                    // AD = Only the Attacking Team has a "Win Condition" (Targets Destroyed), Defending Team has to last the timer.
                    scoreAddress1 = baseAddr + 0x5E0808; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E08F0; // Red Score
                    break;
                // CTF
                case 7:
                case 8:
                    // Blue and Red Win Conditions are stored in different memory locations
                    // Based on the number of red and blue flags in map, ideally both addresses should be the same.
                    scoreAddress1 = baseAddr + 0x5DDCA4; // Blue Score
                    scoreAddress2 = baseAddr + 0x5DDCA8; // Red Score
                    break;
                // all other game types...
                default:
                    scoreAddress1 = baseAddr + 0x5E07EC; // Blue Score
                    scoreAddress2 = baseAddr + 0x5E08D4; // Red Score
                    break;
            }

            if (gameTypeId == 0 || gameTypeId == 2 || gameTypeId == 4)
            {
                thisInstance.gameInfoBlueScore = 0;
                thisInstance.gameInfoRedScore = 0;
                return;
            }

            // Blue Win Condition
            byte[] scoreBytes = new byte[4];
            int bytesRead = 0;
            bool success = ReadProcessMemory((int)processHandle, scoreAddress1, scoreBytes, scoreBytes.Length, ref bytesRead);
            int blueTeamScore = (success ? BitConverter.ToInt32(scoreBytes, 0) : 0);

            // Red Win Condition
            byte[] score2Bytes = new byte[4];
            int bytes2Read = 0;
            bool success2 = ReadProcessMemory((int)processHandle, scoreAddress2, score2Bytes, score2Bytes.Length, ref bytes2Read);
            int redTeamScore = (success2 ? BitConverter.ToInt32(score2Bytes, 0) : 0);

            thisInstance.gameInfoBlueScore = blueTeamScore;
            thisInstance.gameInfoRedScore = redTeamScore;
            return;

        }

        
        // Function: WriteMemoryScoreMap
        public static void ScoreMap()
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

        // Function: ReadMemoryPlayerLeaningStatus
        // Returns: 0 = upright, 2 = left leaning, 4 = right leaning
        public static int ReadMemoryPlayerLeaningStatus(int playerSlot)
        {
            int buffer = 0;
            byte[] PointerAddr9 = new byte[4];
            var Pointer = baseAddr + 0x005ED600;

            // read the playerlist memory
            ReadProcessMemory((int)processHandle, Pointer, PointerAddr9, PointerAddr9.Length, ref buffer);
            var playerlistStartingLocationPointer = BitConverter.ToInt32(PointerAddr9, 0) + 0x28;
            byte[] playerListStartingLocationByteArray = new byte[4];
            int playerListStartingLocationBuffer = 0;
            ReadProcessMemory((int)processHandle, playerlistStartingLocationPointer, playerListStartingLocationByteArray, playerListStartingLocationByteArray.Length, ref playerListStartingLocationBuffer);

            int playerlistStartingLocation = BitConverter.ToInt32(playerListStartingLocationByteArray, 0);

            // Calculate the player's address
            int playerNewLocationAddress = playerlistStartingLocation + (playerSlot - 1) * 0xAF33C;

            byte[] playerObjectLocationBytes = new byte[4];
            int playerObjectLocationRead = 0;
            ReadProcessMemory((int)processHandle, playerNewLocationAddress + 0x11C, playerObjectLocationBytes, playerObjectLocationBytes.Length, ref playerObjectLocationRead);
            int playerObjectLocation = BitConverter.ToInt32(playerObjectLocationBytes, 0);

            // Read prone/rolling status (2 bytes)
            byte[] proneStatusBytes = new byte[2];
            int proneStatusRead = 0;
            ReadProcessMemory((int)processHandle, playerObjectLocation + 0x164, proneStatusBytes, proneStatusBytes.Length, ref proneStatusRead);
            short proneStatus = BitConverter.ToInt16(proneStatusBytes, 0);

            // Check if player is rolling (95 or 96) - ignore leaning status when rolling
            if (proneStatus == 95 || proneStatus == 96)
            {
                return 0;
            }

            // Read leaning status (2 bytes)
            byte[] leaningStatusBytes = new byte[2];
            int leaningStatusRead = 0;
            ReadProcessMemory((int)processHandle, playerObjectLocation + 0x102, leaningStatusBytes, leaningStatusBytes.Length, ref leaningStatusRead);
            short leaningStatus = BitConverter.ToInt16(leaningStatusBytes, 0);

            // Normalize the status (remove the +8 for moving)
            // 0 = upright, 2 = left lean, 4 = right lean
            // 8 = moving upright, 10 = moving left lean, 12 = moving right lean
            int normalizedStatus = leaningStatus % 8;

            return normalizedStatus;
        }

        private static int ReadInt(int address)
        {
            byte[] buf = new byte[4];
            int bytesRead = 0;
            ReadProcessMemory((int)processHandle, address, buf, buf.Length, ref bytesRead);
            return BitConverter.ToInt32(buf, 0);
        }

        private static string ReadString(int address, int maxLen = 32)
        {
            byte[] buf = new byte[maxLen];
            int bytesRead = 0;
            ReadProcessMemory((int)processHandle, address, buf, maxLen, ref bytesRead);
            int len = Array.IndexOf(buf, (byte)0);
            return System.Text.Encoding.ASCII.GetString(buf, 0, len < 0 ? bytesRead : len);
        }

        private const int ENTITY_ARRAY_BASE    = 0x00715900;
        private const int ENTITY_STRIDE        = 0x29c;

        // data_9e4728 timer list (second list: ptr at +0x18, count at +0x1c)
        private const int PSP_TIMER_LIST_PTR   = 0x009E4740; // value = heap ptr to array
        private const int PSP_TIMER_COUNT_ADDR = 0x009E4744; // count of active entries
        private const int PSP_ENTRY_STRIDE     = 0x14;
        private const int PSP_ENTRY_SPAWN      = 0x00;  // s[0] = PSP spawn entity ptr (index → letter A, B, …)
        private const int PSP_ENTRY_TEAM       = 0x04;  // s[1] = capturing player's team (1–4)
        private const int PSP_ENTRY_PLAYER     = 0x10;  // s[4] = player entity ptr in data_715900

        // data_9ed600 session struct (for player name lookup)
        private const int PLAYER_LIST_PTR      = 0x009ED600;
        private const int PLAYER_STRIDE        = 0xaf33c;
        private const int PLAYER_OFF_ENTITY    = 0x18;  // ptr to data_715900 entity
        private const int PLAYER_OFF_NAME      = 0x1c;  // null-terminated name string

        private static string GetPlayerName(int playerEntityPtr)
        {
            int container = ReadInt(PLAYER_LIST_PTR);
            if (container == 0) return "Unknown";
            int count     = ReadInt(container);
            int baseAddr  = ReadInt(container + 4);
            if (baseAddr == 0 || count <= 0) return "Unknown";

            for (int i = 0; i < count; i++)
            {
                int session = baseAddr + i * PLAYER_STRIDE;
                if (ReadInt(session + PLAYER_OFF_ENTITY) == playerEntityPtr)
                    return ReadString(session + PLAYER_OFF_NAME);
            }
            return "Unknown";
        }

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
                        continue;

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
                                WriteMemorySendChatMessage(8, $"** {carrierInfo} dropped the flag (killed)! **");
                            }
                        }
                        continue;
                    }

                    if (!isAlive)
                        continue;

                    int flagTypePtr = ReadInt(flagItemPtr + ENTITY_TYPEPTR_OFF);
                    int flagTypeId  = (flagTypePtr != 0) ? ReadInt(flagTypePtr + TYPEDEF_TYPEID_OFF) : 0;
                    if (flagTypeId != 0xfff)
                        continue;

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

                    if (distSq > FB_SCORE_RADIUS_SQ) continue;

                    int scoreAddr = baseAddr + TEAM_SCORE_OFFSETS[team - 1];
                    int oldScore  = ReadInt(scoreAddr);
                    WriteInt(scoreAddr, oldScore + 1);
                    _flagScorerLastScored[playerEntPtr] = DateTime.UtcNow;

                    string scorerName = thisInstance._flagCarriers[key];

                    WriteMemorySendChatMessage(8, $"** {scorerName} ({teamName}) scored! Score: {oldScore + 1} **");

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
                AppDebug.Log("Error", AppDebug.LogLevel.Error, ex);
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
            
        }

        private static void WriteInt(int address, int value)
        {
            byte[] buf = BitConverter.GetBytes(value);
            int written = 0;
            WriteProcessMemory((int)processHandle, address, buf, buf.Length, ref written);
        }

    }

}
