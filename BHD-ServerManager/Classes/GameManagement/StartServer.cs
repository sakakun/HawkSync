using BHD_ServerManager.Classes.GameManagement.Patcher;
using BHD_ServerManager.Classes.InstanceManagers;
using BHD_ServerManager.Classes.SupportClasses;
using BHD_ServerManager.Forms.Panels;
using HawkSyncShared;
using HawkSyncShared.Instances;
using HawkSyncShared.SupportClasses;
using Salaros.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BHD_ServerManager.Classes.GameManagement
{
    // Ideally this should be a static class to manage server start operations.

    public static class StartServer
    {
        // External Imports for Windows API
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);


        // Internal Variables to Program
        private static theInstance theInstance => CommonCore.theInstance!;
        private static mapInstance mapInstance => CommonCore.instanceMaps!;
        private static tabProfile  tabProfile  => Program.ServerManagerUI!.ProfileTab;

        public static bool createAutoRes()
        {
            try
            {
                if (mapInstance.Playlists[mapInstance.ActivePlaylist] == null ||
                    mapInstance.Playlists[mapInstance.ActivePlaylist].Count == 0)
                {
                    AppDebug.Log("StartServer", "currentMapPlaylist is empty. Cannot create autores.bin.");
                    return false;
                }

                string autoResPath = Path.Combine(theInstance.profileServerPath!, "autores.bin");
                string dfvCFGPath = Path.Combine(theInstance.profileServerPath!, "dfv.cfg");

                string text = File.ReadAllText(dfvCFGPath);
                text = text.Replace("// DISPLAY", "[Display]");
                text = text.Replace("// CONTROLS", "[Controls]");
                text = text.Replace("// MULTIPLAYER", "[Multiplayer]");
                text = text.Replace("// MAP", "[infoCurrentMapName]");
                text = text.Replace("// SYSTEM", "[System]");

                var configFileFromString = new ConfigParser(
                    text,
                    new ConfigParserSettings
                    {
                        MultiLineValues = MultiLineValues.Simple
                            | MultiLineValues.AllowValuelessKeys
                            | MultiLineValues.QuoteDelimitedValues
                    });

                string hw3d_name = configFileFromString.GetValue("Display", "hw3d_name");
                string hw3d_guid = configFileFromString.GetValue("Display", "hw3d_guid");

                if (File.Exists(autoResPath))
                    File.Delete(autoResPath);

                var activePlaylist = mapInstance.Playlists[mapInstance.ActivePlaylist];
                var firstMap = activePlaylist[0];

                int dedicatedSlots = theInstance.gameMaxSlots + Convert.ToInt32(theInstance.gameDedicated);
                int loopMaps = theInstance.gameLoopMaps;

                int gamePlayOptions;
                try
                {
                    gamePlayOptions = Functions.CalulateGameOptions(
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
                    AppDebug.Log("StartServer", "Error calculating game options: " + ex.Message);
                    return false;
                }

                // -----------------------------------------------------------------
                // Layout constants
                // -----------------------------------------------------------------
                const int MAX_MAPS = 128;

                const int OFFSET_HEADER = 0x0000;
                const int OFFSET_PLAYLIST_META = 0x1347;
                const int OFFSET_CURRENT_MAP = 0x187F;
                const int OFFSET_MAP_TABLE = 0x1883;

                // Server rules offsets inside unk_9F1F28
                const int RULE_UNKNOWN_160B = 0x160B;
                const int RULE_MAX_SLOTS = 0x160F;          // 0x000D97A0
                const int RULE_DEDICATED = 0x1613;          
                const int RULE_MAX_TEAM_LIVES = 0x161F;     // 0x000D8554
                const int RULE_FLAG_SCORE = 0x1623;         // 0x5F21AC / 0x6034B8 
                const int RULE_START_DELAY = 0x1627;        // 0x000D7F00
                const int RULE_UNKNOWN_162F = 0x162F;       
                const int RULE_FLAG_RETURN_TIME = 0x1633;   // 0x000DB6AC
                const int RULE_UNKNOWN_1637 = 0x1637;
                const int RULE_UNKNOWN_163B = 0x163B;
                const int RULE_TIME_LIMIT = 0x163F;         // 0x000DD1DC
                const int RULE_ZONE_TIMER = 0x1643;         // 0x5F21B8 / 0x6344B4
                const int RULE_RESPAWN_TIME = 0x1647;       // 0x000DD4E8 
                const int RULE_DESTROY_BUILDINGS = 0x164B;  // 0x000D99B8
                const int RULE_UNKNOWN_1693 = 0x1693;
                const int RULE_UNKNOWN_1697 = 0x1697;
                const int RULE_UNKNOWN_169B = 0x169B;
                const int RULE_UNKNOWN_16B7 = 0x16B7;
                const int RULE_UNKNOWN_16BB = 0x16BB;
                const int RULE_UNKNOWN_16C7 = 0x16C7;
                const int RULE_UNKNOWN_16CB = 0x16CB;
                const int RULE_GAME_PORT = 0x16CF;
                const int RULE_ALLOW_CUSTOM_SKINS = 0x16D7; // 0x000AD760
                const int RULE_REQUIRE_NOVA_LOGIN = 0x16DB; // 0x000D9960
                const int RULE_LAN_MODE = 0x16EF;
                const int RULE_MIN_PING_VALUE = 0x16F3;     // 0x000D7FB8
                const int RULE_ENABLE_MIN_PING = 0x16F7;    // 0x000D9A60
                const int RULE_MAX_PING_VALUE = 0x16FB;     // 0x000DB634
                const int RULE_ENABLE_MAX_PING = 0x16FF;    // 0x000DB634
                const int RULE_UNKNOWN_1703 = 0x1703;
                const int RULE_UNKNOWN_1707 = 0x1707;
                const int RULE_MOTD = 0x170B;               // 0x000D9AAC

                // -----------------------------------------------------------------
                // Original blobs - unchanged
                // -----------------------------------------------------------------
                string _miscGraphicSettings = "00 0E 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 CD CC 4C 3F 06 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 10 00 00 00 10 00 00 00 10 00 00 08 00 00 00 01 00 00 00 00 10 00 00 00 00 D0 1E 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 1E 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 03 00 00 00 02 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 03 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 03 00 00 00 02 00 00 00 04 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 C0 00 00 00 C0 00 00 00 C0 00 00 00 C0 00 00 00 02 00 00 00 01 00 00 00";
                string applicationSettings = "01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00";

                // -----------------------------------------------------------------
                // Byte arrays
                // -----------------------------------------------------------------
                byte[] autoRestart = Encoding.Default.GetBytes("autorestartV0.0");

                // Keep original working behavior: 128
                byte[] numberOfMapsBytes = BitConverter.GetBytes(128);

                byte[] graphicsSetup_Name = Encoding.Default.GetBytes(hw3d_name);
                byte[] graphicsSetup_GUID = Encoding.Default.GetBytes(hw3d_guid);
                byte[] graphicsSetupMisc_Settings = Functions.ToByteArray(_miscGraphicSettings.Replace(" ", ""));
                byte[] applicationSettingBytes = Functions.ToByteArray(applicationSettings.Replace(" ", ""));
                byte[] windowedModeBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameWindowedMode));
                byte[] serverNameBytes = Encoding.Default.GetBytes(theInstance.gameServerName);
                byte[] countryCodeBytes = Encoding.Default.GetBytes(theInstance.gameCountryCode);
                byte[] bindAddressBytes = Encoding.Default.GetBytes(theInstance.profileBindIP!);
                byte[] firstMapFileBytes = Encoding.Default.GetBytes(firstMap.MapFile!);
                byte[] maxSlotsBytes = BitConverter.GetBytes(theInstance.gameMaxSlots);
                byte[] dedicatedBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameDedicated));
                byte[] gameScoreBytes = BitConverter.GetBytes(theInstance.gameScoreKills);
                byte[] serverPasswordBytes = Encoding.Default.GetBytes(theInstance.gamePasswordLobby!);
                byte[] redTeamPasswordBytes = Encoding.Default.GetBytes(theInstance.gamePasswordRed!);
                byte[] blueTeamPasswordBytes = Encoding.Default.GetBytes(theInstance.gamePasswordBlue!);
                byte[] gamePlayOptionsBytes = BitConverter.GetBytes(gamePlayOptions);
                byte[] loopMapsBytes = BitConverter.GetBytes(loopMaps > 0 ? 1 : 0); // preserved for parity
                byte[] gameTypeBytes = BitConverter.GetBytes(firstMap.MapType);
                byte[] timeLimitBytes = BitConverter.GetBytes(theInstance.gameTimeLimit);
                byte[] respawnTimeBytes = BitConverter.GetBytes(theInstance.gameRespawnTime);
                byte[] allowCustomSkinsBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameCustomSkins));
                byte[] requireNovaLoginBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameRequireNova));
                byte[] motdBytes = Encoding.Default.GetBytes(theInstance.gameMOTD);
                byte[] sessionTypeBytes = BitConverter.GetBytes(false);
                byte[] dedicatedSlotsBytes = BitConverter.GetBytes(dedicatedSlots);
                byte[] graphicsHeaderSettings = BitConverter.GetBytes(-1);
                byte[] graphicsSetting_1 = BitConverter.GetBytes(8);
                byte[] startDelayBytes = BitConverter.GetBytes(theInstance.gameStartDelay);
                byte[] minPingValueBytes = BitConverter.GetBytes(theInstance.gameMinPingValue);
                byte[] enableMinPingBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameMinPing));
                byte[] maxPingValueBytes = BitConverter.GetBytes(theInstance.gameMaxPingValue);
                byte[] enableMaxPingBytes = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameMaxPing));
                byte[] gamePortBytes = BitConverter.GetBytes(theInstance.profileBindPort);
                byte[] flagBallScoreBytes = BitConverter.GetBytes(theInstance.gameScoreFlags);
                byte[] zoneTimerBytes = BitConverter.GetBytes(theInstance.gameScoreZoneTime);
                byte[] customMapFlagBytes = BitConverter.GetBytes(Convert.ToInt32(firstMap.ModType == 9 ? 1 : 0));
                byte[] allowDestroyingBuildings = BitConverter.GetBytes(Convert.ToInt32(theInstance.gameDestroyBuildings));
                byte[] flagreturntime = BitConverter.GetBytes(theInstance.gameFlagReturnTime);

				// Keep original working behavior
				byte[] mapListPrehandle = BitConverter.GetBytes(10621344);

                byte[] finalAppSetup = Functions.ToByteArray("00 00 00 00 00 00 00 00 05 00 00 00 00".Replace(" ", ""));
                byte[] resolutionSetup = Functions.ToByteArray("02 00 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] graphicsPrehandle = Functions.ToByteArray("02 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] defaultWeaponSetup = Functions.ToByteArray("05 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] endOfMap = Functions.ToByteArray("20 B5 B6 01 00 00 00 00 53 01 00 00 00 13 00 00 00 13 00 00 00 04 00 00 00".Replace(" ", ""));

                // -----------------------------------------------------------------
                // Keep original MemoryStream sparse write behavior
                // -----------------------------------------------------------------
                MemoryStream ms = new MemoryStream();

                void WriteAt(int offset, byte[] bytes)
                {
                    ms.Seek(offset, SeekOrigin.Begin);
                    ms.Write(bytes, 0, bytes.Length);
                }

                // Header + count
                ms.Seek(OFFSET_HEADER, SeekOrigin.Begin);
                ms.Write(autoRestart, 0, autoRestart.Length);
                ms.Write(numberOfMapsBytes, 0, numberOfMapsBytes.Length);

                // Base values / application area
                WriteAt(0x004D, firstMapFileBytes);
                WriteAt(0x00AF, customMapFlagBytes);
                WriteAt(0x068F, resolutionSetup);
                WriteAt(0x0277, sessionTypeBytes);
                WriteAt(0x01C7, applicationSettingBytes);
                WriteAt(0x0283, dedicatedSlotsBytes);
                WriteAt(0x028F, gameTypeBytes);
                WriteAt(0x0293, finalAppSetup);

                // Graphics / playlist meta area
                WriteAt(OFFSET_PLAYLIST_META + 0x0000, graphicsPrehandle);
                WriteAt(OFFSET_PLAYLIST_META + 0x0008, graphicsHeaderSettings);
                WriteAt(OFFSET_PLAYLIST_META + 0x000C, graphicsSetting_1);
                WriteAt(OFFSET_PLAYLIST_META + 0x0010, windowedModeBytes);
                WriteAt(0x135F, graphicsSetup_Name);

                ms.Seek(0x137F, SeekOrigin.Begin);
                ms.Write(graphicsSetup_GUID, 0, graphicsSetup_GUID.Length);
                ms.Write(graphicsSetupMisc_Settings, 0, graphicsSetupMisc_Settings.Length);

                // Strings / passwords / server identity
                WriteAt(0x151F, gamePlayOptionsBytes);
                WriteAt(0x152F, serverPasswordBytes);
                WriteAt(0x1562, redTeamPasswordBytes);
                WriteAt(0x1573, blueTeamPasswordBytes);
                WriteAt(0x15A6, serverNameBytes);
                WriteAt(0x15C6, countryCodeBytes);
                WriteAt(0x15EA, bindAddressBytes);

                // Server rules block
                WriteAt(RULE_UNKNOWN_160B, BitConverter.GetBytes(1));
                WriteAt(RULE_MAX_SLOTS, BitConverter.GetBytes(80));             // Must be set 80 for the memory allocation to work correctly
				WriteAt(RULE_DEDICATED, dedicatedBytes);
                WriteAt(RULE_MAX_TEAM_LIVES, BitConverter.GetBytes(100));
                WriteAt(RULE_FLAG_SCORE, flagBallScoreBytes);
                WriteAt(RULE_START_DELAY, startDelayBytes);
                WriteAt(RULE_UNKNOWN_162F, BitConverter.GetBytes(1));
                WriteAt(RULE_FLAG_RETURN_TIME, flagreturntime);
                WriteAt(RULE_UNKNOWN_1637, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_163B, BitConverter.GetBytes(2));
                WriteAt(RULE_TIME_LIMIT, timeLimitBytes);
                WriteAt(RULE_ZONE_TIMER, zoneTimerBytes);
                WriteAt(RULE_RESPAWN_TIME, respawnTimeBytes);
                WriteAt(RULE_DESTROY_BUILDINGS, allowDestroyingBuildings);     // A3449C = destroybuild
                WriteAt(RULE_UNKNOWN_1693, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_1697, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_169B, BitConverter.GetBytes(15));
                WriteAt(RULE_UNKNOWN_16B7, BitConverter.GetBytes(2));
                WriteAt(RULE_UNKNOWN_16BB, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_16C7, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_16CB, BitConverter.GetBytes(2));
                WriteAt(RULE_GAME_PORT, gamePortBytes);
                WriteAt(RULE_ALLOW_CUSTOM_SKINS, allowCustomSkinsBytes);
                WriteAt(RULE_REQUIRE_NOVA_LOGIN, requireNovaLoginBytes);
                WriteAt(RULE_LAN_MODE, BitConverter.GetBytes(4));               // A34560 = lanmode (1, 2, 4)
                WriteAt(RULE_MIN_PING_VALUE, minPingValueBytes);
                WriteAt(RULE_ENABLE_MIN_PING, enableMinPingBytes);
                WriteAt(RULE_MAX_PING_VALUE, maxPingValueBytes);
                WriteAt(RULE_ENABLE_MAX_PING, enableMaxPingBytes);
                WriteAt(RULE_UNKNOWN_1703, BitConverter.GetBytes(1));
                WriteAt(RULE_UNKNOWN_1707, BitConverter.GetBytes(1));
                WriteAt(RULE_MOTD, motdBytes);

                // Keep original working writes that may overlap intentionally
                WriteAt(0x16F4, BitConverter.GetBytes(1));
                WriteAt(0x16FC, BitConverter.GetBytes(1));

                WriteAt(0x1DA4, gameScoreBytes);
                WriteAt(0x178B, defaultWeaponSetup);

                // Keep original working current-map prehandle
                WriteAt(OFFSET_CURRENT_MAP, mapListPrehandle);

                // -----------------------------------------------------------------
                // Map table
                // -----------------------------------------------------------------
                void WriteMapEntryBytes(string mapFile, string mapName, bool isCustom)
                {
                    byte[] mapFileBytes = Encoding.GetEncoding("Windows-1252").GetBytes(mapFile);
                    ms.Write(mapFileBytes, 0, mapFileBytes.Length);

                    ms.Seek(ms.Position + (0x20F - mapFileBytes.Length), SeekOrigin.Begin);

                    byte[] mapNameBytes = Encoding.GetEncoding("Windows-1252").GetBytes(mapName);
                    ms.Write(mapNameBytes, 0, mapNameBytes.Length);

                    ms.Seek(ms.Position + (0x305 - mapNameBytes.Length), SeekOrigin.Begin);
                    ms.Write(endOfMap, 0, endOfMap.Length);

                    ms.Seek(ms.Position + 0x1E3, SeekOrigin.Begin);
                    byte[] customMap = BitConverter.GetBytes(Convert.ToInt32(isCustom));
                    ms.Write(customMap, 0, customMap.Length);

                    ms.Seek(ms.Position + 0x1C, SeekOrigin.Begin);
                }

                ms.Seek(OFFSET_MAP_TABLE, SeekOrigin.Begin);

                foreach (var map in activePlaylist)
                {
                    WriteMapEntryBytes(
                        map.MapFile ?? "NA.bms",
                        map.MapName ?? "NA",
                        map.ModType == 9);
                }

                for (int i = activePlaylist.Count; i < MAX_MAPS; i++)
                {
                    WriteMapEntryBytes("NA.bms", "NA", false);
                }

                using (BinaryWriter writer = new BinaryWriter(File.Open(autoResPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write(ms.ToArray());
                }

                Thread.Sleep(1000);
                return true;
            }
            catch (Exception e)
            {
                AppDebug.Log("StartServer", "Error creating autores.bin file: " + e);
                return false;
            }
        }

        public static bool readAutoRes()
        {
            try
            {
                string autoResPath = Path.Combine(theInstance.profileServerPath!, "autores.bin");
                string dumpPath = Path.Combine(theInstance.profileServerPath!, "autores_dump.txt");

                if (!File.Exists(autoResPath))
                {
                    AppDebug.Log("ReadAutoRes", "autores.bin file does not exist.");
                    return false;
                }

                byte[] fileData = File.ReadAllBytes(autoResPath);

                using (StreamWriter writer = new StreamWriter(dumpPath, false, Encoding.UTF8))
                {
                    writer.WriteLine("==================== AUTORES.BIN DUMP ====================");
                    writer.WriteLine($"File Size: {fileData.Length} bytes");
                    writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("==========================================================");
                    writer.WriteLine();

                    // Header and map count (matches createAutoRes order)
                    writer.WriteLine($"0x0000: {Encoding.Default.GetString(fileData, 0x0000, 15)}");
                    writer.WriteLine($"0x000F: {BitConverter.ToInt32(fileData, 0x000F)}");

                    // Base values / application area
                    writer.WriteLine($"0x004D: {ReadNullTerminatedString(fileData, 0x004D)}");
                    writer.WriteLine($"0x00AF: {BitConverter.ToInt32(fileData, 0x00AF)}");
                    writer.WriteLine($"0x068F: {BitConverter.ToInt32(fileData, 0x068F)}");
                    writer.WriteLine($"0x0277: {BitConverter.ToInt32(fileData, 0x0277)}");
                    writer.WriteLine($"0x01C7: {FormatByteArray(fileData, 0x01C7, 188)}");
                    writer.WriteLine($"0x0283: {BitConverter.ToInt32(fileData, 0x0283)}");
                    writer.WriteLine($"0x028F: {BitConverter.ToInt32(fileData, 0x028F)}");
                    writer.WriteLine($"0x0293: {FormatByteArray(fileData, 0x0293, 13)}");

                    // Graphics / playlist meta area
                    writer.WriteLine($"0x1347: {FormatByteArray(fileData, 0x1347, 8)}");
                    writer.WriteLine($"0x134F: {BitConverter.ToInt32(fileData, 0x134F)}");
                    writer.WriteLine($"0x1353: {BitConverter.ToInt32(fileData, 0x1353)}");
                    writer.WriteLine($"0x1357: {BitConverter.ToInt32(fileData, 0x1357)}");
                    writer.WriteLine($"0x135F: {ReadNullTerminatedString(fileData, 0x135F)}");
                    writer.WriteLine($"0x137F: {ReadNullTerminatedString(fileData, 0x137F)}");

                    // Strings / passwords / server identity
                    writer.WriteLine($"0x151F: {BitConverter.ToInt32(fileData, 0x151F)}");
                    writer.WriteLine($"0x152F: {ReadNullTerminatedString(fileData, 0x152F)}");
                    writer.WriteLine($"0x1562: {ReadNullTerminatedString(fileData, 0x1562)}");
                    writer.WriteLine($"0x1573: {ReadNullTerminatedString(fileData, 0x1573)}");
                    writer.WriteLine($"0x15A6: {ReadNullTerminatedString(fileData, 0x15A6)}");
                    writer.WriteLine($"0x15C6: {ReadNullTerminatedString(fileData, 0x15C6)}");
                    writer.WriteLine($"0x15EA: {ReadNullTerminatedString(fileData, 0x15EA)}");

                    // Server rules block (in exact write order from createAutoRes)
                    writer.WriteLine($"0x160B: {BitConverter.ToInt32(fileData, 0x160B)}");
                    writer.WriteLine($"0x160F: {BitConverter.ToInt32(fileData, 0x160F)}");
                    writer.WriteLine($"0x1613: {BitConverter.ToInt32(fileData, 0x1613)}");
                    writer.WriteLine($"0x161F: {BitConverter.ToInt32(fileData, 0x161F)}");
                    writer.WriteLine($"0x1623: {BitConverter.ToInt32(fileData, 0x1623)}");
                    writer.WriteLine($"0x1627: {BitConverter.ToInt32(fileData, 0x1627)}");
                    writer.WriteLine($"0x162F: {BitConverter.ToInt32(fileData, 0x162F)}");
                    writer.WriteLine($"0x1633: {BitConverter.ToInt32(fileData, 0x1633)}");
                    writer.WriteLine($"0x1637: {BitConverter.ToInt32(fileData, 0x1637)}");
                    writer.WriteLine($"0x163B: {BitConverter.ToInt32(fileData, 0x163B)}");
                    writer.WriteLine($"0x163F: {BitConverter.ToInt32(fileData, 0x163F)}");
                    writer.WriteLine($"0x1643: {BitConverter.ToInt32(fileData, 0x1643)}");
                    writer.WriteLine($"0x1647: {BitConverter.ToInt32(fileData, 0x1647)}");
                    writer.WriteLine($"0x164B: {BitConverter.ToInt32(fileData, 0x164B)}");
                    writer.WriteLine($"0x1693: {BitConverter.ToInt32(fileData, 0x1693)}");
                    writer.WriteLine($"0x1697: {BitConverter.ToInt32(fileData, 0x1697)}");
                    writer.WriteLine($"0x169B: {BitConverter.ToInt32(fileData, 0x169B)}");
                    writer.WriteLine($"0x16B7: {BitConverter.ToInt32(fileData, 0x16B7)}");
                    writer.WriteLine($"0x16BB: {BitConverter.ToInt32(fileData, 0x16BB)}");
                    writer.WriteLine($"0x16C7: {BitConverter.ToInt32(fileData, 0x16C7)}");
                    writer.WriteLine($"0x16CB: {BitConverter.ToInt32(fileData, 0x16CB)}");
                    writer.WriteLine($"0x16CF: {BitConverter.ToInt32(fileData, 0x16CF)}");
                    writer.WriteLine($"0x16D7: {BitConverter.ToInt32(fileData, 0x16D7)}");
                    writer.WriteLine($"0x16DB: {BitConverter.ToInt32(fileData, 0x16DB)}");
                    writer.WriteLine($"0x16EF: {BitConverter.ToInt32(fileData, 0x16EF)}");
                    writer.WriteLine($"0x16F3: {BitConverter.ToInt32(fileData, 0x16F3)}");
                    writer.WriteLine($"0x16F7: {BitConverter.ToInt32(fileData, 0x16F7)}");
                    writer.WriteLine($"0x16FB: {BitConverter.ToInt32(fileData, 0x16FB)}");
                    writer.WriteLine($"0x16FF: {BitConverter.ToInt32(fileData, 0x16FF)}");
                    writer.WriteLine($"0x1703: {BitConverter.ToInt32(fileData, 0x1703)}");
                    writer.WriteLine($"0x1707: {BitConverter.ToInt32(fileData, 0x1707)}");
                    writer.WriteLine($"0x170B: {ReadNullTerminatedString(fileData, 0x170B)}");

                    // Additional writes from createAutoRes
                    writer.WriteLine($"0x16F4: {BitConverter.ToInt32(fileData, 0x16F4)}");
                    writer.WriteLine($"0x16FC: {BitConverter.ToInt32(fileData, 0x16FC)}");
                    writer.WriteLine($"0x1DA4: {BitConverter.ToInt32(fileData, 0x1DA4)}");
                    writer.WriteLine($"0x178B: {FormatByteArray(fileData, 0x178B, 188)}");
                    writer.WriteLine($"0x187F: {BitConverter.ToInt32(fileData, 0x187F)}");

                    writer.WriteLine();
                    writer.WriteLine("==================== MAP TABLE (0x1883) ====================");
                    
                    // Map table entries (128 maps, each entry is 0x730 bytes)
                    int mapTableOffset = 0x1883;
                    int mapEntrySize = 0x730;
                    for (int i = 0; i < 128; i++)
                    {
                        int offset = mapTableOffset + (i * mapEntrySize);
                        if (offset + 0x20F < fileData.Length)
                        {
                            string mapFile = ReadNullTerminatedString(fileData, offset);
                            string mapName = ReadNullTerminatedString(fileData, offset + 0x20F);
                            int customFlag = BitConverter.ToInt32(fileData, offset + 0x514);
                            
                            if (mapFile != "NA.bms" || i == 0)
                            {
                                writer.WriteLine($"Map {i:D3} @ 0x{offset:X}: File=\"{mapFile}\", Name=\"{mapName}\", Custom={customFlag}");
                            }
                        }
                    }

                    writer.WriteLine();
                    writer.WriteLine("==========================================================");
                }

                AppDebug.Log("ReadAutoRes", $"Dump written to: {dumpPath}");
                return true;
            }
            catch (Exception e)
            {
                AppDebug.Log("ReadAutoRes", "Error reading autores.bin file: " + e);
                return false;
            }
        }

        private static string FormatByteArray(byte[] data, int offset, int length)
        {
            if (offset + length > data.Length)
                length = data.Length - offset;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                if (i > 0) sb.Append(" ");
                sb.Append(data[offset + i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string ReadNullTerminatedString(byte[] data, int offset)
        {
            int length = 0;
            while (offset + length < data.Length && data[offset + length] != 0)
            {
                length++;
            }
            return Encoding.Default.GetString(data, offset, length);
        }

        // Function: CheckForExistingProcess
        private static readonly object processCheckLock = new();

        public static bool CheckForExistingProcess()
        {
            string file_name = "dfbhd.exe";
            string FullFileName = Path.Combine(theInstance.profileServerPath!, file_name);
            string windowTitle = $"BHD Server - {theInstance.gameServerName}";

            // Defensive: run process enumeration in a lock to prevent race conditions
            lock (processCheckLock)
            {
                // Enumerate processes by name (fast, but MainModule access can block)
                foreach (var searchProcess in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(file_name)))
                {
                    bool fileMatch = false;
                    bool titleMatch = false;

                    // Access MainModule in a try-catch to avoid blocking the main thread
                    try
                    {
                        fileMatch = string.Equals(searchProcess.MainModule?.FileName, FullFileName, StringComparison.OrdinalIgnoreCase);
                    }
                    catch (Win32Exception)
                    {
                        // Skip processes we can't access
                        continue;
                    }
                    catch (InvalidOperationException)
                    {
                        // Process may have exited
                        continue;
                    }
                    catch (SystemException)
                    {
                        // Defensive: skip if process is in a bad state
                        continue;
                    }

                    // Access MainWindowTitle in a try-catch
                    try
                    {
                        titleMatch = string.Equals(searchProcess.MainWindowTitle, windowTitle, StringComparison.Ordinal);
                    }
                    catch
                    {
                        // Defensive: skip if window title can't be read
                        continue;
                    }

                    if (fileMatch && titleMatch)
                    {
                        AppDebug.Log("StartServer", $"Found existing game process: {searchProcess.ProcessName} (PID: {searchProcess.Id})");
                        theInstance.instanceAttachedPID = searchProcess.Id;
                        theInstance.instanceProcessHandle = searchProcess.Handle;

                        SetProcessWindowTitle(searchProcess, windowTitle);

                        ServerMemory.AttachToGameProcess();
                        // New MatchID just incase...
                        theInstanceManager.GenerateMatchID();
                        return true;
                    }
                }
            }

            return false;
        }

        // Function: CmdStartGame
        public static bool startGame()
        {
            string file_name = "dfbhd.exe";
            string FullFileName = Path.Combine(theInstance.profileServerPath!, file_name);
            string windowTitle = $"BHD Server - {theInstance.gameServerName}";

            try
            {
                
                if (CheckForExistingProcess()) { return true; }

                bool wasPatched = DFBHDPatcher.Patch(FullFileName);
                if (!wasPatched)
                    AppDebug.Log("startGame", "Already patched, starting as-is.");

                AppDebug.Log("startGame", "No existing game process found, starting a new instance...");

                // Create the AutoRes File
                createAutoRes();
                
                // Commandline Switches
                string ProgramArguments = string.Empty;
                if (theInstance.profileServerAttribute01) { ProgramArguments += tabProfile.profileServerAttribute01.Text + " "; }
                if (theInstance.profileServerAttribute02) { ProgramArguments += tabProfile.profileServerAttribute02.Text + " "; }
                if (theInstance.profileServerAttribute03) { ProgramArguments += tabProfile.profileServerAttribute03.Text + " "; }
                if (theInstance.profileServerAttribute05) { ProgramArguments += tabProfile.profileServerAttribute05.Text + " "; }
                if (theInstance.profileServerAttribute06) { ProgramArguments += tabProfile.profileServerAttribute06.Text + " "; }
                if (theInstance.profileServerAttribute07) { ProgramArguments += tabProfile.profileServerAttribute07.Text + " "; }
                if (theInstance.profileServerAttribute08) { ProgramArguments += tabProfile.profileServerAttribute08.Text + " "; }
                if (theInstance.profileServerAttribute09) { ProgramArguments += tabProfile.profileServerAttribute09.Text + " "; }
                if (theInstance.profileServerAttribute10) { ProgramArguments += tabProfile.profileServerAttribute10.Text + " "; }
                if (theInstance.profileServerAttribute11) { ProgramArguments += tabProfile.profileServerAttribute11.Text + " "; }
                if (theInstance.profileServerAttribute12) { ProgramArguments += tabProfile.profileServerAttribute12.Text + " "; }
                if (theInstance.profileServerAttribute13) { ProgramArguments += tabProfile.profileServerAttribute13.Text + " "; }
                if (theInstance.profileServerAttribute14) { ProgramArguments += tabProfile.profileServerAttribute14.Text + " "; }
                if (theInstance.profileServerAttribute15) { ProgramArguments += tabProfile.profileServerAttribute15.Text + " "; }
                if (theInstance.profileServerAttribute16) { ProgramArguments += tabProfile.profileServerAttribute16.Text + " "; }
                if (theInstance.profileServerAttribute17) { ProgramArguments += tabProfile.profileServerAttribute17.Text + " "; }
                if (theInstance.profileServerAttribute18) { ProgramArguments += tabProfile.profileServerAttribute18.Text + " "; }
                if (theInstance.profileServerAttribute19) { ProgramArguments += tabProfile.profileServerAttribute19.Text + " "; }
                if (theInstance.profileServerAttribute20) { ProgramArguments += tabProfile.profileServerAttribute20.Text + " "; }
                if (theInstance.profileServerAttribute21) { ProgramArguments += tabProfile.profileServerAttribute21.Text + " "; }
                if (theInstance.profileServerAttribute04) { ProgramArguments += tabProfile.profileServerAttribute04.Text + " " + theInstance.profileModFileName + " "; }

                // Start Game
                var startInfo = new ProcessStartInfo
                {
                    FileName = FullFileName,
                    WorkingDirectory = theInstance.profileServerPath,
                    Arguments = ProgramArguments,
                    // WindowStyle = ProcessWindowStyle.Minimized
                };
                Process? process = Process.Start(startInfo);

                if (process == null)
                    throw new InvalidOperationException("Failed to start the game process.");

                // Set MaxWorkingSet and instanceAttachedPID directly from the started process
                process.MaxWorkingSet = new nint(0x7fffffff);
                theInstance.instanceAttachedPID = process.Id;
                theInstance.instanceProcessHandle = process.Handle;

                SetProcessWindowTitle(process, windowTitle);

                ServerMemory.AttachToGameProcess();
            }
            catch (Exception e)
            {
                AppDebug.Log("StartServer", "Error starting game: " + e.ToString());
                MessageBox.Show("Error starting game: " + e.Message, "Start Game Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
            }

            // Game Didn't Crash Yay!  Dump Data to the Instance
            theInstance.instanceCrashCounter = 0;

            // Wait for the Game to Start, 15 seconds to prevent memory corruption.
            Thread.Sleep(5000);

            // Set the Player Name of the Host of the Game Server
            ServerMemory.UpdatePlayerHostName();
            // Map Count Fix... Don't Underand why this is needed, but it is.
            ServerMemory.UpdateMapListCount();
            // Additional Game Settings to Set
            theInstanceManager.UpdateGameServer();

            return true;
        }
        // Function: stopGame
        public static bool stopGame()
        {
            string file_name = "dfbhd.exe";
            string FullFileName = Path.Combine(theInstance.profileServerPath!, file_name);

            try
            {
                if (theInstance.instanceAttachedPID.HasValue)
                {
                    try
                    {
                        ServerMemory.DetachFromGameProcess();

                        var process = Process.GetProcessById(theInstance.instanceAttachedPID.Value);
                        if (!process.HasExited)
                        {
                            process.Kill(true); // true for entire process tree (.NET 5+)
                            process.WaitForExit(5000); // Wait up to 5 seconds for exit
                        }

                        DFBHDPatcher.Unpatch(FullFileName);

                    }
                    catch (ArgumentException)
                    {
                        // Process already exited or does not exist
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("StopServer", "Error: " + ex.ToString());
                        return false;
                    }
                    finally
                    {
                        theInstance.instanceAttachedPID = null;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                AppDebug.Log("StartServer", "Error stopping game: " + e.ToString());
                return false;
            }
        }

        private static void SetProcessWindowTitle(Process process, string title)
        {
            // Wait for the main window handle to be created if needed
            for (int i = 0; i < 20; i++)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                    break;
                Thread.Sleep(250);
                process.Refresh();
            }
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                SetWindowText(process.MainWindowHandle, title);
            }
        }

    }


}
