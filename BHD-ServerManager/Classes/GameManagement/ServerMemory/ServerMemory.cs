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
        // Global Variables
        private static theInstance          thisInstance            => CommonCore.theInstance!;
        private static playerInstance       playerInstance          => CommonCore.instancePlayers!;
        private static mapInstance          mapInstance             => CommonCore.instanceMaps!;

        // Import of Dynamic Link Libraries
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint       OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool       ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("user32.dll")]
        static extern bool              PostMessage(nint hWnd, uint Msg, int wParam, int lParam);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool              WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        // Process Memory CONST
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

        private static void WriteInt(int address, int value)
        {
            byte[] buf = BitConverter.GetBytes(value);
            int written = 0;
            WriteProcessMemory((int)processHandle, address, buf, buf.Length, ref written);
        }

    }

}
