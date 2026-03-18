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

    public static partial class ServerMemory {

		// Function: WriteMemorySendChatMessage, writes a message to the chat console in-game, with a specified color.
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
            WriteString(0x00879A14, Msg, nullTerminated: true);
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
        public static void WriteMemoryChatCountDownKiller(int ChatLogAddr) => WriteInt(ChatLogAddr + 0x7C, 0);

                // Function: WriteMemorySendConsoleCommand
        public static void WriteMemorySendConsoleCommand(string Command)
        {

            // open cmdConsole
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, cmdConsole, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, cmdConsole, 0);
            Thread.Sleep(100);

            // Write to cmdConsole
            WriteString(0x00879A14, Command, nullTerminated: true);

            Thread.Sleep(100);
            PostMessage(windowHandle, (ushort)WM_KEYDOWN, VK_ENTER, 0);
            PostMessage(windowHandle, (ushort)WM_KEYUP, VK_ENTER, 0);

        }

        // Function: ReadMemoryLastChatMessage
        public static string[] ReadMemoryLastChatMessage()
        {
            // get last message sent...
            int ChatLogAddr = ReadInt(baseAddr + 0x00062D10);

            byte[] Message = new byte[74];
            int MessageRead = 0;
            ReadProcessMemory((int)processHandle, ChatLogAddr, Message, Message.Length, ref MessageRead);
            string LastMessage = Encoding.GetEncoding(1252).GetString(Message).Replace("\0", "");

            byte[] msgType = BitConverter.GetBytes(ReadInt(ChatLogAddr + 0x78));
            string msgTypeBytes = BitConverter.ToString(msgType).Replace("-", "");

            return new string[] { ChatLogAddr.ToString(), LastMessage, msgTypeBytes };
        }

	}

}
