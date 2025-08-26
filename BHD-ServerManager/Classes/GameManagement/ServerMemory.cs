using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace BHD_ServerManager.Classes.GameManagement
{
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static class ServerMemory
    {
        // Global Variables
        private readonly static theInstance thisInstance = CommonCore.theInstance!;
        private readonly static mapInstance mapInstance = CommonCore.instanceMaps!;

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
            byte[] Hostname = Encoding.Default.GetBytes(thisInstance.gameHostName + "\0");
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
            byte[] numberOfMaps = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
            int numberofMapsWritten = 0;
            WriteProcessMemory((int)processHandle, mapListNumberOfMaps, numberOfMaps, numberOfMaps.Length, ref numberofMapsWritten);

            mapListNumberOfMaps += 0x4;
            byte[] TotalnumberOfMaps = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
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
            byte[] FlagReturnTimeWrite = BitConverter.GetBytes(thisInstance.gameFlagReturnTime * 60);
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
        // Function: UpdateMaxTeamLives
        public static void UpdateMaxTeamLives()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x000D8554, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);
            byte[] MaxTeamLivesBytes = new byte[4];
            int MaxTeamLivesRead = 0;
            ReadProcessMemory((int)processHandle, Ptr1Addr, MaxTeamLivesBytes, MaxTeamLivesBytes.Length, ref MaxTeamLivesRead);
            int MaxTeamLives = BitConverter.ToInt32(MaxTeamLivesBytes, 0);

            int MaxTeamLivesWritten = 0;
            byte[] MaxTeamLivesWrite = BitConverter.GetBytes(thisInstance.gameMaxTeamLives);
            WriteProcessMemory((int)processHandle, Ptr1Addr, MaxTeamLivesWrite, MaxTeamLivesWrite.Length, ref MaxTeamLivesWritten);


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
            string ServerNameQuery = Encoding.Default.GetString(ServerName).Replace("\0", "");
            // end Server Query Name

            // Server Name Display
            byte[] Ptr3 = new byte[4];
            int Ptr3Read = 0;
            ReadProcessMemory((int)processHandle, Ptr2 + 0x000A7088, Ptr3, Ptr3.Length, ref Ptr3Read);
            int ServerDisplayerName = BitConverter.ToInt32(Ptr3, 0);

            byte[] ServerNameDisplay = new byte[31];
            int ServerNameRead = 0;
            ReadProcessMemory((int)processHandle, ServerDisplayerName + 0x30, ServerNameDisplay, ServerNameDisplay.Length, ref ServerNameRead);
            string ServerDisplayName = Encoding.Default.GetString(ServerNameDisplay).Replace("\0", "");
            // end Server Name Display

            // since either one or the other isn't what it should be.. just update them both. Call it a day.
            byte[] ServerNameBytes = Encoding.Default.GetBytes(thisInstance.gameServerName);
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, ServerDisplayerName, ServerNameBytes, ServerNameBytes.Length, ref bytesWritten);
            WriteProcessMemory((int)processHandle, Ptr2, ServerNameBytes, ServerNameBytes.Length, ref bytesWritten);


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
            string MOTD = Encoding.Default.GetString(MOTDBytes).Replace("\0", "");

            int MOTDWritten = 0;
            byte[] MOTDWrite = Encoding.Default.GetBytes(thisInstance.gameMOTD);
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


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005F204A, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);

            // Always use 16 bytes for the password buffer
            byte[] BluePasswordWrite = new byte[16];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordBlue))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordBlue);
                Array.Copy(pwBytes, BluePasswordWrite, Math.Min(pwBytes.Length, BluePasswordWrite.Length));
            }
            // If password is null or empty, BluePasswordWrite remains all zeros

            int BluePasswordWritten = 0;
            WriteProcessMemory((int)processHandle, Ptr1Addr, BluePasswordWrite, BluePasswordWrite.Length, ref BluePasswordWritten);


        }
        // Function: UpdateRedPassword
        public static void UpdateRedPassword()
        {


            byte[] Ptr1 = new byte[4];
            int Ptr1Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x006343D3, Ptr1, Ptr1.Length, ref Ptr1Read);

            int Ptr1Addr = BitConverter.ToInt32(Ptr1, 0);

            // Always use 16 bytes for the password buffer
            byte[] RedPasswordWrite = new byte[16];
            if (!string.IsNullOrEmpty(thisInstance.gamePasswordRed))
            {
                byte[] pwBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordRed);
                Array.Copy(pwBytes, RedPasswordWrite, Math.Min(pwBytes.Length, RedPasswordWrite.Length));
            }
            // If password is null or empty, RedPasswordWrite remains all zeros

            int RedPasswordWritten = 0;
            WriteProcessMemory((int)processHandle, Ptr1Addr + 0x33, RedPasswordWrite, RedPasswordWrite.Length, ref RedPasswordWritten);


        }
        // Function: UpdateMapCycle1
        // Clears the current map cycle and fills it with empty maps
        public static void UpdateMapCycle1()
        {
            if (mapInstance.currentMapPlaylist.Count > 128)
            {
                throw new Exception("Someway, somehow, someone bypassed the maplist checks. You are NOT allowed to have more than 128 maps. #88");
            }



            byte[] ServerMapCyclePtr = new byte[4];
            int Pointer2Read = 0;
            ReadProcessMemory((int)processHandle, baseAddr + 0x005ED5F8, ServerMapCyclePtr, ServerMapCyclePtr.Length, ref Pointer2Read);
            int mapCycleServerAddress = BitConverter.ToInt32(ServerMapCyclePtr, 0);

            int mapCycleTotalAddress = mapCycleServerAddress + 0x4;
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);

            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);

            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);

            foreach (mapFileInfo entry in mapInstance.previousMapPlaylist)
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
            if (mapInstance.currentMapPlaylist.Count > 128)
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
            var firstMap = mapInstance.currentMapPlaylist[0];
            WriteFixedString(firstMap.MapFile!, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            string mapName = firstMap.MapName!;
            if (mapName.Length > 31)
                mapName = mapName.Substring(0, 31);
            WriteFixedString(mapName, 28);
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
            bw.Write(new byte[256]); // adjust this padding as needed

            bw.Write(BitConverter.GetBytes(firstMap.CustomMap ? 1 : 0));
            bw.Write(new byte[24]); // adjust this padding as needed

            // Write additional maps
            for (int i = 1; i < mapInstance.currentMapPlaylist.Count; i++)
            {
                var map = mapInstance.currentMapPlaylist[i];
                WriteFixedString(map.MapFile!, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                string name = map.MapName!;
                if (name.Length > 31)
                    name = name.Substring(0, 31);
                WriteFixedString(name, 28);
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(thisInstance.gameScoreKills));
                bw.Write(new byte[256]); // adjust this padding as needed

                bw.Write(BitConverter.GetBytes(map.CustomMap ? 1 : 0));
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
            byte[] mapTotal = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
            int mapTotalWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleTotalAddress, mapTotal, mapTotal.Length, ref mapTotalWritten);


            int mapCycleCurrentIndex = mapCycleServerAddress + 0xC;
            byte[] resetMapIndex = BitConverter.GetBytes(mapInstance.currentMapPlaylist.Count);
            int resetMapIndexWritten = 0;
            WriteProcessMemory((int)processHandle, mapCycleCurrentIndex, resetMapIndex, resetMapIndex.Length, ref resetMapIndexWritten);


            byte[] mapCycleListAddress = new byte[4];
            int mapCycleListAddressRead = 0;
            ReadProcessMemory((int)processHandle, mapCycleServerAddress, mapCycleListAddress, mapCycleListAddress.Length, ref mapCycleListAddressRead);
            int mapCycleList = BitConverter.ToInt32(mapCycleListAddress, 0);


            for (int i = 0; i < mapInstance.currentMapPlaylist.Count; i++)
            {
                int mapFileIndexLocation = mapCycleList;
                byte[] mapFileBytes = new byte[0x20]; // 32 bytes
                byte[] nameBytes = Encoding.ASCII.GetBytes(mapInstance.currentMapPlaylist[i].MapFile!);
                Array.Copy(nameBytes, mapFileBytes, Math.Min(nameBytes.Length, mapFileBytes.Length));
                int mapFileBytesWritten = 0;
                WriteProcessMemory((int)processHandle, mapFileIndexLocation, mapFileBytes, mapFileBytes.Length, ref mapFileBytesWritten);
                mapFileIndexLocation += 0x20;

                byte[] customMapFlag = BitConverter.GetBytes(Convert.ToInt32(mapInstance.currentMapPlaylist[i].CustomMap));
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

            // to prevent locking of this PlayerIPAddress simply look at each PlayerIPAddress before writing to the PlayerIPAddress...
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
        // Function: UpdateMapCycleCounter
        public static void ReadMapCycleCounter()
        {

            byte[] currentMapCycleCountBytes = new byte[4];
            int currentMapCycleCountRead = 0;

            int mapCycleCounterPtr = baseAddr + 0x5ED644;

            ReadProcessMemory((int)processHandle, mapCycleCounterPtr, currentMapCycleCountBytes, currentMapCycleCountBytes.Length, ref currentMapCycleCountRead);

            int currentMapCycleCount = BitConverter.ToInt32(currentMapCycleCountBytes, 0);

            thisInstance.gameInfoMapCycleIndex = currentMapCycleCount;


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
        // Function: UpdateNextMapGameType
        public static void GetNextMapType()
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
            AppDebug.Log("ServerMemory", "Number of Maps: " + mapInstance.currentMapPlaylist.Count + " Pre-Check Current Map Index: " + currentMapIndex);

            if (currentMapIndex + 1 >= mapInstance.currentMapPlaylist.Count || mapInstance.currentMapPlaylist[currentMapIndex + 1] == null)
            {
                currentMapIndex = 0;
            }
            else
            {
                currentMapIndex++;
            }

            AppDebug.Log("ServerMemory", "Number of Maps: " + mapInstance.currentMapPlaylist.Count + " Current Map Index: " + currentMapIndex);
            int currentMapType = getGameTypeID(thisInstance.gameInfoGameType!);
            int nextMapType = getGameTypeID(mapInstance.currentMapPlaylist[currentMapIndex].MapType!);

            AppDebug.Log("ServerMemory", "Current Map Type: " + thisInstance.gameInfoMapName + " " + thisInstance.gameInfoGameType + " " + currentMapType);
            AppDebug.Log("ServerMemory", "Next Map Type: " + mapInstance.currentMapPlaylist[currentMapIndex].MapName + " " + mapInstance.currentMapPlaylist[currentMapIndex].MapType + " - " + nextMapType);

            thisInstance.gameInfoNextMapGameType = nextMapType;

        }
        public static void SetNextMapType()
        {
            AppDebug.Log("SetNextMapType", "Updated the Map Type for the Next Map");

            try
            {
                // Deal with the Players
                theInstanceManager.changeTeamGameMode(getGameTypeID(thisInstance.gameInfoGameType!), thisInstance.gameInfoNextMapGameType);

                // Change the MapType for the next map
                var CurrentGameTypeAddr = baseAddr + 0x5F21A4;
                byte[] nextMaptypeBytes = BitConverter.GetBytes(thisInstance.gameInfoNextMapGameType);
                int nextMaptypeBytesWrite = 0;
                WriteProcessMemory((int)processHandle, CurrentGameTypeAddr, nextMaptypeBytes, nextMaptypeBytes.Length, ref nextMaptypeBytesWrite);

            }
            catch (Exception ex)
            {
                AppDebug.Log("ServerMemory", "Something went wrong with ScoringProcessHandler: " + ex);
            }
        }
        // Update the Game Score for the next map
        public static void UpdateGameScores()
        {

            AppDebug.Log("UpdateGameScores", "Updated Game Scores");

            // This changes the score needed to win on the next map played.
            int nextGameScore = 0;
            var startingPtr1 = 0;
            var startingPtr2 = 0;

            switch (thisInstance.gameInfoNextMapGameType)
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

            AppDebug.Log("UpdateGameScores", "Game Score Updated: " + nextGameScore);

        }
        // Function UpdatePlayerTeam
        public static void UpdatePlayerTeam()
        {
            if (thisInstance.playerChangeTeamList.Count == 0)
            {
                return;
            }
            else
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

                for (int ii = 0; ii < thisInstance.playerChangeTeamList.Count; ii++)
                {
                    int playerLocationOffset = (thisInstance.playerChangeTeamList[ii].slotNum - 1) * 0xAF33C;

                    int playerLocation = playerlistStartingLocation + playerLocationOffset;
                    int playerTeamLocation = playerLocation + 0x90;
                    byte[] teamBytes = BitConverter.GetBytes(thisInstance.playerChangeTeamList[ii].Team);
                    int bytesWritten = 0;
                    WriteProcessMemory((int)processHandle, playerTeamLocation, teamBytes, teamBytes.Length, ref bytesWritten);
                    thisInstance.playerChangeTeamList.RemoveAt(ii);
                }


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
                NextMapIndex = mapInstance.currentMapPlaylist.Count;
            }
            else if (mapInstance.currentMapPlaylist[NextMapIndex - 1] != null)
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



            switch (MsgLocation)
            {
                case 1: // Yellow Message
                    colorcode = Functions.ToByteArray("6A 03".Replace(" ", ""));
                    WriteProcessMemory((int)processHandle, 0x00462ABA, colorcode, colorcode.Length, ref colorbuffer_written);
                    break;
                case 2: // Team Chat: Red
                    colorcode = Functions.ToByteArray("6A 04".Replace(" ", ""));
                    WriteProcessMemory((int)processHandle, 0x00462ABA, colorcode, colorcode.Length, ref colorbuffer_written);
                    break;
                case 3: // Team Chat: Blue
                    colorcode = Functions.ToByteArray("6A 05".Replace(" ", ""));
                    WriteProcessMemory((int)processHandle, 0x00462ABA, colorcode, colorcode.Length, ref colorbuffer_written);
                    break;
                default: // White
                    colorcode = Functions.ToByteArray("6A 01".Replace(" ", ""));
                    WriteProcessMemory((int)processHandle, 0x00462ABA, colorcode, colorcode.Length, ref colorbuffer_written);
                    break;
            }
            // post message
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, chatConsole, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, chatConsole, 0);
            Thread.Sleep(50);
            int bytesWritten = 0;
            byte[] buffer;
            buffer = Encoding.Default.GetBytes($"{Msg}\0"); // '\0' marks the end of string
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
            byte[] buffer = Encoding.Default.GetBytes($"{Command}\0"); // '\0' marks the end of string

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
            catch
            {
                // Process does not exist or access denied
                thisInstance.instanceAttachedPID = null;
                processHandle = nint.Zero; // Replace 'null' with 'IntPtr.Zero' for nint type
                AppDebug.Log("ServerMemory", "Process not found or access denied.");
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
            }



            byte[] Ptr = new byte[4];
            int ReadPtr = 0;

            ReadProcessMemory((int)processHandle, baseAddr + 0x00061098, Ptr, Ptr.Length, ref ReadPtr);
            int MapTimeAddr = BitConverter.ToInt32(Ptr, 0);

            Stopwatch stopwatchProcessingTime = new Stopwatch();
            stopwatchProcessingTime.Start();

            byte[] MapTimeMs = new byte[4];
            int MapTimeRead = 0;
            ReadProcessMemory((int)processHandle, MapTimeAddr, MapTimeMs, MapTimeMs.Length, ref MapTimeRead);
            int MapTime = BitConverter.ToInt32(MapTimeMs, 0);
            int MapTimeInSeconds = MapTime / 60;

            DateTime MapStartTime = DateTime.Now - TimeSpan.FromSeconds(MapTimeInSeconds);
            DateTime MapEndTime = MapStartTime + TimeSpan.FromMinutes(thisInstance.gameStartDelay + thisInstance.gameTimeLimit);

            byte[] TimeOffset = new byte[4];
            int TimeOffsetRead = 0;
            ReadProcessMemory((int)processHandle, MapTimeAddr, TimeOffset, TimeOffset.Length, ref TimeOffsetRead);
            int intTimeOffset = BitConverter.ToInt32(TimeOffset, 0);

            TimeSpan TimeRemaining = MapEndTime - (DateTime.Now + TimeSpan.FromMilliseconds(stopwatchProcessingTime.ElapsedMilliseconds) - TimeSpan.FromMilliseconds(intTimeOffset));
            stopwatchProcessingTime.Stop();

            thisInstance.gameInfoTimeRemaining = TimeRemaining;


        }
        // Function: ReadMemoryCurrentMissionName
        public static void ReadMemoryCurrentMissionName()
        {


            // memory polling
            int bytesRead = 0;
            byte[] buffer = new byte[26];
            ReadProcessMemory((int)processHandle, 0x0071569C, buffer, buffer.Length, ref bytesRead);
            string MissionName = Encoding.Default.GetString(buffer);
            thisInstance.gameInfoMapName = MissionName.Replace("\0", "");


        }
        // Fuction: ReadMemoryCurrentGameType
        public static void ReadMemoryCurrentGameType()
        {


            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, 0x009F21A4, buffer, buffer.Length, ref bytesRead);
            int GameType = BitConverter.ToInt32(buffer, 0);

            foreach (var gameType in objectGameTypes.All)
            {
                if (GameType.Equals(gameType.DatabaseId))
                {
                    thisInstance.gameInfoGameType = gameType.ShortName!;
                    break;
                }
            }


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
            thisInstance.gameInfoCurrentMapIndex = BitConverter.ToInt32(mapIndexBytes, 0);


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
            thisInstance.gameInfoCurrentNumPlayers = CurrentPlayers;


        }
        // Function: ReadMemoryGeneratePlayerList
        public static void ReadMemoryGeneratePlayerList()
        {
            if (thisInstance.instanceStatus == InstanceStatus.LOADINGMAP)
            {
                return;
            }

            Dictionary<int, playerObject> currentPlayerList = new Dictionary<int, playerObject>();
            int NumPlayers = thisInstance.gameInfoCurrentNumPlayers;

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

                    playerObject PlayerStats = ReadMemoryPlayerStats(playerSlot);
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
                            // Try to preserve PlayerJoined if player already exists in the persistent list
                            if (thisInstance.playerList.TryGetValue(playerSlot, out var existingPlayer))
                            {
                                PlayerStats.PlayerJoined = existingPlayer.PlayerJoined;
                            }
                            // Final Touches
                            PlayerStats.PlayerLastSeen = DateTime.Now;
                            PlayerStats.PlayerTimePlayed = (int)(PlayerStats.PlayerLastSeen - PlayerStats.PlayerJoined).TotalSeconds;
                            PlayerStats.PlayerSlot = playerSlot;
                            byte[] trimmedPlayerNameBytes = playerNameBytes.Where(b => b != 0).ToArray();
                            PlayerStats.PlayerNameBase64 = Convert.ToBase64String(trimmedPlayerNameBytes);
                            PlayerStats.PlayerTeam = playerTeam;
                            PlayerStats.PlayerIPAddress = playerIP;
                            PlayerStats.PlayerPing = PlayerStats.PlayerPing;
                            PlayerStats.RoleName = PlayerCharacterClass.ToString();
                            PlayerStats.SelectedWeaponName = PlayerSelectedWeapon.ToString();

                            // Push to List
                            currentPlayerList.Add(playerSlot, PlayerStats);

                            // Setup for Next Player
                            playerlistStartingLocation += 0xAF33C;

                            if (currentPlayerList.Count >= NumPlayers)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            AppDebug.Log("ServerMemory", "Detected an error!\n\n" + "Player Name: " + playerSlot + "\n\n" + formattedPlayerName + "\n\n" + e.ToString());
                        }

                    }

                }


            }
            thisInstance.playerList.Clear();
            foreach (var kvp in currentPlayerList)
            {
                thisInstance.playerList[kvp.Key] = kvp.Value;
            }
            // CoreManager.DebugLog("PlayerList Updated");
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
        public static playerObject ReadMemoryPlayerStats(int reqslot)
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

            byte[] read_name = new byte[15];
            int bytesread = 0;

            ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
            var PlayerName = Encoding.Default.GetString(read_name).Replace("\0", "");

            if (string.IsNullOrEmpty(PlayerName))
            {
                beginaddr += 0xAF33C;
                // Retry read if player name is empty
                ReadProcessMemory((int)processHandle, beginaddr + 0x1C, read_name, read_name.Length, ref bytesread);
                PlayerName = Encoding.Default.GetString(read_name).Replace("\0", "");
            }

            // Handle failure if still no player name found
            if (string.IsNullOrEmpty(PlayerName))
            {

                AppDebug.Log("ServerMemory", "Something went wrong here. We can't find any player names.");
                return new playerObject();
            }

            byte[] read_ping = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB40, read_ping, read_ping.Length, ref bytesread);

            int[] offsets = {
                0x000ADAB4, 0x000ADA94, 0x000ADA98, 0x000ADA8C, 0x000ADAD0,
                0x000ADA90, 0x000ADAD4, 0x000ADAF4, 0x000ADABC, 0x000ADAC0,
                0x000ADAC4, 0x000ADACC, 0x000ADACC, 0x000ADAA8, 0x000ADAC8,
                0x000ADAD4, 0x000ADAA4, 0x000ADADC, 0x000ADA94, 0x000ADAB0,
                0x000ADAAC, 0x000ADAD8, 0x000ADADC, 0x000ADAE0
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
                0x000ADA90, 0x000ADA94, 0x000ADA98, 0x000ADA9C,
                0x000ADAA8, 0x000ADAAC, 0x000ADAB0, 0x000ADAB4,
                0x000ADAB8, 0x000ADABC, 0x000ADAC0, 0x000ADAC4,
                0x000ADAC8, 0x000ADACC, 0x000ADAD0, 0x000ADAD4,
                0x000ADAE4, 0x000ADAE8, 0x000ADAEC
            };
            var stats2 = new int[offsets2.Length];
            for (int i = 0; i < offsets2.Length; i++)
            {
                byte[] read_data = new byte[4];
                ReadProcessMemory((int)processHandle, beginaddr + offsets2[i], read_data, read_data.Length, ref bytesread);
                stats2[i] = BitConverter.ToInt32(read_data, 0);
            }
            //Console.WriteLine(PlayerName);
            //Console.WriteLine(JsonConvert.SerializeObject(stats));
            //Console.WriteLine(JsonConvert.SerializeObject(stats2));

            byte[] read_playerObjectLocation = new byte[4];
            ReadProcessMemory((int)processHandle, beginaddr + 0x5E7C, read_playerObjectLocation, read_playerObjectLocation.Length, ref bytesread);
            int read_playerObject = BitConverter.ToInt32(read_playerObjectLocation, 0);

            byte[] read_selectedWeapon = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x178, read_selectedWeapon, read_selectedWeapon.Length, ref bytesread);
            int SelectedWeapon = BitConverter.ToInt32(read_selectedWeapon, 0);

            byte[] read_selectedCharacterClass = new byte[4];
            ReadProcessMemory((int)processHandle, read_playerObject + 0x244, read_selectedCharacterClass, read_selectedCharacterClass.Length, ref bytesread);
            int SelectedCharacterClass = BitConverter.ToInt32(read_selectedCharacterClass, 0);

            byte[] read_weapons = new byte[250];
            ReadProcessMemory((int)processHandle, beginaddr + 0x000ADB70, read_weapons, read_weapons.Length, ref bytesread);
            var MemoryWeapons = Encoding.Default.GetString(read_weapons).Replace("\0", "|");
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


            return new playerObject
            {
                PlayerName = Encoding.GetEncoding("Windows-1252").GetString(read_name),
                PlayerPing = BitConverter.ToInt32(read_ping, 0),
                RoleID = SelectedCharacterClass,
                SelectedWeaponID = SelectedWeapon,
                PlayerWeapons = WeaponList,
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
                stat_ADTargetsDestroyed = stats[19],
                stat_FlagSaves = stats[20],
                stat_SniperKills = stats[21],
                stat_TKOTHDefenseKills = stats[22],
                stat_TKOTHAttackKills = stats[23]
            };
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
            string LastMessage = Encoding.Default.GetString(Message).Replace("\0", "");

            int msgTypeAddr = ChatLogAddr + 0x78;
            byte[] msgType = new byte[4];
            int msgTypeRead = 0;
            ReadProcessMemory((int)processHandle, msgTypeAddr, msgType, msgType.Length, ref msgTypeRead);
            string msgTypeBytes = BitConverter.ToString(msgType).Replace("-", "");



            return new string[] { ChatLogAddr.ToString(), LastMessage, msgTypeBytes };
        }

        public static void ReadMemoryCurrentGameScore()
        {
            int scoreAddress1 = 0;
            int scoreAddress2 = 0;
            int currentGameScore = 0;
            
            // thisInstance.gameInfoGameType
            switch (objectGameTypes.All.FirstOrDefault(gt => gt.ShortName == thisInstance.gameInfoGameType)!.DatabaseId)
            {
                // KOTH/TKOTH
                case 3:
                case 4:
                    scoreAddress1 = baseAddr + 0x5F21B8;
                    scoreAddress2 = baseAddr + 0x6344B4;
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

            // Try reading from the first address
            byte[] scoreBytes = new byte[4];
            int bytesRead = 0;
            bool success = ReadProcessMemory((int)processHandle, scoreAddress1, scoreBytes, scoreBytes.Length, ref bytesRead);

            if (success && bytesRead == 4)
            {
                currentGameScore = BitConverter.ToInt32(scoreBytes, 0);
            }
            else
            {
                // Fallback: try the second address
                bytesRead = 0;
                success = ReadProcessMemory((int)processHandle, scoreAddress2, scoreBytes, scoreBytes.Length, ref bytesRead);
                if (success && bytesRead == 4)
                {
                    currentGameScore = BitConverter.ToInt32(scoreBytes, 0);
                }
                else
                {
                    AppDebug.Log("ReadMemoryCurrentGameScore", "Failed to read game score from memory.");
                    currentGameScore = -1;
                }
            }

            thisInstance.gameInfoCurrentGameScore = currentGameScore;
        }

    }

}
