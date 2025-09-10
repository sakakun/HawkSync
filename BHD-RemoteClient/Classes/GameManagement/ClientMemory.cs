using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.GameManagement
{
    public class ClientMemory
    {

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
        public static void AttachToGameProcess(int ProcessID)
        {
            if (gameProcess == null || gameProcess.HasExited)
            {
                gameProcess = Process.GetProcessById(ProcessID);
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

    }
}
