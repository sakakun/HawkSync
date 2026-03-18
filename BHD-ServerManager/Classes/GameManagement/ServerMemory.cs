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
    // This class is a placeholder for server memory management.
    // Should be a static class to manage server memory operations.

    public static partial class ServerMemory
    {
        // Global Variables
        private static      theInstance         theInstance         => CommonCore.theInstance!;
        private static      playerInstance      playerInstance      => CommonCore.instancePlayers!;
        private static      mapInstance         mapInstance         => CommonCore.instanceMaps!;

        // Import of Dynamic Link Libraries
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("user32.dll")]
        static extern bool PostMessage(nint hWnd, uint Msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        private const int PROCESS_WM_READ = 0x0010;
        private const int PROCESS_VM_WRITE = 0x0020;
        private const int PROCESS_VM_OPERATION = 0x0008;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const int PROCESS_QUERY_INFORMATION = 0x0400;
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const int VK_ENTER = 0x0D;
        private const int cmdConsole = 0xC0;
        private const int chatConsole = 0x54;

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
        private const int NOVA_ID_ADDR          = 0x009DDA44;
        private const int PLAYER_LIST_PTR      = 0x009ED600;
        private const int PLAYER_STRIDE        = 0xaf33c;
        private const int PLAYER_OFF_ENTITY    = 0x18;  // ptr to data_715900 entity
        private const int PLAYER_OFF_NAME      = 0x1c;  // null-terminated name string


        // END: Process Memory Variables

        // Corrected declaration of baseAddr
        public readonly static int baseAddr = 0x400000;

        // Process Related Variables
        public static Process?      gameProcess         { get; set; }
        public static nint          windowHandle        { get; set; } = nint.Zero;
        public static nint          processHandle       { get; set; } = nint.Zero;

        // Function: attachProcess, Attach to the Game Process, used by all memory functions.
        public static void AttachToGameProcess()
        {
            if (gameProcess == null || gameProcess.HasExited)
            {
                gameProcess = Process.GetProcessById((int)theInstance.instanceAttachedPID!);
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

        // Function: ReadMemoryIsProcessAttached
        public static bool ReadMemoryIsProcessAttached()
        {
            // Check if PID and process handle are set
            if (theInstance.instanceAttachedPID == null || theInstance.instanceAttachedPID == 0 || processHandle == nint.Zero)
            {
                return false;
            }

            try
            {
                // Try to get the process by PID
                var process = Process.GetProcessById(theInstance.instanceAttachedPID.Value);

                // Check if the process path matches profileServerPath (if needed)
                if (!string.IsNullOrEmpty(theInstance.profileServerPath))
                {
                    // May throw if process has exited or access denied
                    string processPath = process.MainModule?.FileName ?? string.Empty;
                    if (!processPath.Equals(theInstance.profileServerPath + "\\dfbhd.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        theInstance.instanceAttachedPID = null;
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
                theInstance.instanceAttachedPID = null;
                processHandle = nint.Zero; // Replace 'null' with 'IntPtr.Zero' for nint type
                AppDebug.Log("ServerMemory", "Process not found or access denied.");
                return false;
            }
        }

        // Function: ReadMemoryServerStatus
        public static void ReadMemoryServerStatus() => theInstance.instanceStatus = (InstanceStatus)ReadInt(ReadInt(baseAddr + 0x00098334));

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

        private static void WriteInt(int address, int value)
        {
            byte[] buf = BitConverter.GetBytes(value);
            int written = 0;
            WriteProcessMemory((int)processHandle, address, buf, buf.Length, ref written);
        }

        // Encodes a string as Windows-1252 and writes it to process memory.
        private static void WriteString(int address, string value, bool nullTerminated = false)
        {
            byte[] buf = Encoding.GetEncoding(1252).GetBytes(nullTerminated ? value + "\0" : value);
            int written = 0;
            WriteProcessMemory((int)processHandle, address, buf, buf.Length, ref written);
        }

        // Writes a null-padded fixed-length Windows-1252 string into a game memory buffer.
        private static void WriteFixedString(int address, string? value, int bufferSize)
        {
            byte[] buf = new byte[bufferSize];
            if (!string.IsNullOrEmpty(value))
            {
                byte[] strBytes = Encoding.GetEncoding(1252).GetBytes(value);
                Array.Copy(strBytes, buf, Math.Min(strBytes.Length, bufferSize - 1));
            }
            int written = 0;
            WriteProcessMemory((int)processHandle, address, buf, buf.Length, ref written);
        }

    }

}
