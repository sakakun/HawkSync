using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
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

        // Global Variables
        private readonly static theInstance thisInstance = CommonCore.theInstance!;

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
        public static Process? gameProcess { get; set; } = null;
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
                AppDebug.Log("AttachToGameProcess", $"Process Attached.");
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

        public static bool ReadMemoryIsProcessAttached()
        {
            // Check if PID and process handle are set
            if (thisInstance.instanceAttachedPID == null || thisInstance.instanceAttachedPID == 0 || processHandle == nint.Zero)
            {
                AppDebug.Log("ClientMemory", "No PID/Process Handle");
                return false;
            }

            try
            {
                // Try to get the process by PID
                var process = Process.GetProcessById(thisInstance.instanceAttachedPID.Value);

                // Check if the process executable is named "dfbhd.exe"
                string processPath = process.MainModule?.FileName ?? string.Empty;
                if (!Path.GetFileName(processPath).Equals("dfbhd.exe", StringComparison.OrdinalIgnoreCase))
                {
                    thisInstance.instanceAttachedPID = null;
                    processHandle = nint.Zero;
                    return false;
                }

                // If we got here, process is running and matches
                return true;
            }
            catch
            {
                // Process does not exist or access denied
                thisInstance.instanceAttachedPID = null;
                processHandle = nint.Zero; // Replace 'null' with 'IntPtr.Zero' for nint type
                AppDebug.Log("ClientMemory", "Process not found or access denied.");
                return false;
            }
        }
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
            string LastMessage = Encoding.GetEncoding("Windows-1252").GetString(Message).Replace("\0", "");

            int msgTypeAddr = ChatLogAddr + 0x78;
            byte[] msgType = new byte[4];
            int msgTypeRead = 0;
            ReadProcessMemory((int)processHandle, msgTypeAddr, msgType, msgType.Length, ref msgTypeRead);
            string msgTypeBytes = BitConverter.ToString(msgType).Replace("-", "");

            return new string[] { ChatLogAddr.ToString(), LastMessage, msgTypeBytes };
        }

        public static string ReadMemoryConsoleMessage()
        {
            // This is the absolute address of "!rc message goo"
            // Replace 'baseAddr' with your module base (the one you get from Process.MainModule.BaseAddress)
            var rcMessageAddr = baseAddr + 0x479ADC;

            // Allocate enough buffer for the string (100 bytes: 33 byte)
            byte[] buffer = new byte[33];
            int bytesRead = 0;

            ReadProcessMemory((int)processHandle, rcMessageAddr, buffer, buffer.Length, ref bytesRead);

            // Decode string and trim after first null character
            string message = Encoding.GetEncoding("Windows-1252").GetString(buffer);
            int nullIndex = message.IndexOf('\0');
            if (nullIndex >= 0)
                message = message.Substring(0, nullIndex);

            return message;
        }

        public static void InjectChatMessage(string message, int rgbColor, ushort timer, int slot = 0)
        {
            // Each chat slot = 128 bytes
            var chatBufferBase = 0x00876008;
            var targetAddr = chatBufferBase + (slot * 0x80);
            uint argbColor = ChatColorRgbToArgb(rgbColor);
            byte[] buffer = new byte[0x80]; // 128 bytes

            // 1. Message (120 bytes)
            byte[] msgBytes = Encoding.GetEncoding("Windows-1252").GetBytes(message);
            int msgLen = Math.Min(msgBytes.Length, 120);
            Array.Copy(msgBytes, buffer, msgLen);
            // If message is shorter than 120, buffer is already zero-padded

            // 2. Color (4 bytes at offset 120)
            Array.Copy(BitConverter.GetBytes(argbColor), 0, buffer, 120, 4);

            // 3. Timer (2 bytes at offset 124)
            Array.Copy(BitConverter.GetBytes(timer), 0, buffer, 124, 2);

            // 4. Spacer (2 bytes at offset 126)
            buffer[126] = 0; buffer[127] = 0; // Already zero by default

            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, targetAddr, buffer, buffer.Length, ref bytesWritten);
        }

        public static void PushChatMessage(string message, int rgbColor, ushort timer)
        {
            int maxSlots = 18; // number of chat messages visible
            int slotSize = 0x80;
            uint argbColor = ChatColorRgbToArgb(rgbColor);
            var chatBufferBase = 0x00876008;

            // Shift down: slot[n-1] -> slot[n], starting from bottom
            for (int i = maxSlots - 1; i > 0; i--)
            {
                var srcAddr = chatBufferBase + ((i - 1) * slotSize);
                var dstAddr = chatBufferBase + (i * slotSize);

                byte[] slotData = new byte[slotSize];
                int bytesRead = 0;
                ReadProcessMemory((int)processHandle, srcAddr, slotData, slotSize, ref bytesRead);

                int bytesWritten = 0;
                WriteProcessMemory((int)processHandle, dstAddr, slotData, slotData.Length, ref bytesWritten);
            }

            // Build new entry for slot 0
            byte[] buffer = new byte[slotSize];

            // 1. Message (120 bytes)
            byte[] msgBytes = Encoding.GetEncoding("Windows-1252").GetBytes(message);
            int msgLen = Math.Min(msgBytes.Length, 120);
            Array.Copy(msgBytes, buffer, msgLen);
            // If message is shorter than 120, buffer is already zero-padded

            // 2. Color (4 bytes at offset 120)
            Array.Copy(BitConverter.GetBytes(argbColor), 0, buffer, 120, 4);

            // 3. Timer (2 bytes at offset 124)
            Array.Copy(BitConverter.GetBytes(timer), 0, buffer, 124, 2);

            // 4. Spacer (2 bytes at offset 126)
            buffer[126] = 0; buffer[127] = 0; // Already zero by default

            int written = 0;
            WriteProcessMemory((int)processHandle, chatBufferBase, buffer, buffer.Length, ref written);
        }

        public static uint ChatColorRgbToArgb(int rgb)
        {
            // Extract digits: hundreds = R, tens = G, ones = B
            int r = (rgb / 100) % 10;
            int g = (rgb / 10) % 10;
            int b = rgb % 10;

            // Scale up to 0-255 range (optional, here just multiply by 28 for demonstration)
            r *= 28; // 0-252
            g *= 28;
            b *= 28;

            // Alpha is set to 255 (0xFF)
            return (0xFFu << 24) | ((uint)r << 16) | ((uint)g << 8) | (uint)b;
        }
    }
}
