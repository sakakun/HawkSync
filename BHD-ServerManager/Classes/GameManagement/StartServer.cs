using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.ObjectClasses;
using BHD_SharedResources.Classes.SupportClasses;
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
        private readonly static theInstance thisInstance = CommonCore.theInstance!;
        private readonly static mapInstance mapInstance = CommonCore.instanceMaps!;

        // Function: createAutoRes
        public static bool createAutoRes()
        {

            try
            {
                if (mapInstance.currentMapPlaylist == null || mapInstance.currentMapPlaylist.Count == 0)
                {
                    AppDebug.Log("StartServer", "currentMapPlaylist is empty. Cannot create autores.bin.");
                    return false;
                }

                string autoResPath = Path.Combine(thisInstance.profileServerPath!, "autores.bin");
                string dfvCFGPath = Path.Combine(thisInstance.profileServerPath!, "dfv.cfg");

                string text = File.ReadAllText(dfvCFGPath);
                text = text.Replace("// DISPLAY", "[Display]");
                text = text.Replace("// CONTROLS", "[Controls]");
                text = text.Replace("// MULTIPLAYER", "[Multiplayer]");
                text = text.Replace("// MAP", "[infoCurrentMapName]");
                text = text.Replace("// SYSTEM", "[System]");

                var configFileFromString = new ConfigParser(text,
                  new ConfigParserSettings
                  {
                      MultiLineValues = MultiLineValues.Simple | MultiLineValues.AllowValuelessKeys | MultiLineValues.QuoteDelimitedValues
                  });
                // get string vars
                string hw3d_name = configFileFromString.GetValue("Display", "hw3d_name");
                string hw3d_guid = configFileFromString.GetValue("Display", "hw3d_guid");

                // delete existing autores file if it exists
                if (File.Exists(autoResPath))
                {
                    File.Delete(autoResPath);
                }

                MemoryStream ms = new MemoryStream();
                int dedicatedSlots = thisInstance.gameMaxSlots + Convert.ToInt32(thisInstance.gameDedicated);
                int loopMaps = thisInstance.gameLoopMaps;
                int gamePlayOptions = 0;
                try
                {
                    gamePlayOptions = Functions.CalulateGameOptions(thisInstance.gameOptionAutoBalance, thisInstance.gameOptionFF, thisInstance.gameOptionFriendlyTags, thisInstance.gameOptionFFWarn, thisInstance.gameOptionShowTracers, thisInstance.gameShowTeamClays, thisInstance.gameOptionAutoRange);
                }
                catch (Exception ex)
                {
                    AppDebug.Log("StartServer", "Error calculating game options: " + ex.Message);
                    return false;
                }

                string _miscGraphicSettings = "00 0E 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 CD CC 4C 3F 06 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 10 00 00 00 10 00 00 00 10 00 00 08 00 00 00 01 00 00 00 00 10 00 00 00 00 D0 1E 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 1E 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 03 00 00 00 02 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 03 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 03 00 00 00 02 00 00 00 04 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 C0 00 00 00 C0 00 00 00 C0 00 00 00 C0 00 00 00 02 00 00 00 01 00 00 00";
                string applicationSettings = "01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00";

                byte[] autoRestart = Encoding.Default.GetBytes("autorestartV0.0");
                byte[] numberOfMapsBytes = BitConverter.GetBytes(128);

                byte[] graphicsSetup_Name = Encoding.Default.GetBytes(hw3d_name);
                byte[] graphicsSetup_GUID = Encoding.Default.GetBytes(hw3d_guid);
                byte[] graphicsSetupMisc_Settings = Functions.ToByteArray(_miscGraphicSettings.Replace(" ", ""));
                byte[] applicationSettingBytes = Functions.ToByteArray(applicationSettings.Replace(" ", ""));
                byte[] windowedModeBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameWindowedMode));
                byte[] ServerNameBytes = Encoding.Default.GetBytes(thisInstance.gameServerName);
                byte[] countryCodeBytes = Encoding.Default.GetBytes(thisInstance.gameCountryCode);
                byte[] BindAddress = Encoding.Default.GetBytes(thisInstance.profileBindIP!);
                byte[] firstMapFile = Encoding.Default.GetBytes(mapInstance.currentMapPlaylist[0].MapFile!);
                byte[] maxSlotsBytes = BitConverter.GetBytes(thisInstance.gameMaxSlots);
                byte[] dedicatedBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameDedicated));
                byte[] GameScoreBytes = BitConverter.GetBytes(thisInstance.gameScoreKills);
                byte[] StartDelayBytes = BitConverter.GetBytes(thisInstance.gameStartDelay);
                byte[] serverPasswordBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordLobby!);
                byte[] redTeamPasswordBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordRed!);
                byte[] blueTeamPasswordBytes = Encoding.Default.GetBytes(thisInstance.gamePasswordBlue!);
                byte[] gamePlayOptionsBytes = BitConverter.GetBytes(gamePlayOptions);
                byte[] loopMapsBytes = BitConverter.GetBytes(loopMaps > 0 ? 1 : 0);

                byte[] gameTypeBytes = BitConverter.GetBytes(
                    objectGameTypes.All.FirstOrDefault(gt => gt.ShortName == mapInstance.currentMapPlaylist[0].GameType)?.DatabaseId ?? 0);
                byte[] timeLimitBytes = BitConverter.GetBytes(thisInstance.gameTimeLimit);
                byte[] respawnTimeBytes = BitConverter.GetBytes(thisInstance.gameRespawnTime);
                byte[] allowCustomSkinsBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameCustomSkins));
                byte[] requireNovaLoginBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameRequireNova));
                byte[] MOTDBytes = Encoding.Default.GetBytes(thisInstance.gameMOTD);
                byte[] sessionTypeBytes = BitConverter.GetBytes(thisInstance.gameSessionType);
                byte[] dedicatedSlotsBytes = BitConverter.GetBytes(dedicatedSlots);
                byte[] graphicsHeaderSettings = BitConverter.GetBytes(-1);
                byte[] graphicsSetting_1 = BitConverter.GetBytes(8);
                byte[] startDelayBytes = BitConverter.GetBytes(thisInstance.gameStartDelay);
                byte[] minPingValueBytes = BitConverter.GetBytes(thisInstance.gameMinPingValue);
                byte[] enableMinPingBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameMinPing));
                byte[] maxPingValueBytes = BitConverter.GetBytes(thisInstance.gameMaxPingValue);
                byte[] enableMaxPingBytes = BitConverter.GetBytes(Convert.ToInt32(thisInstance.gameMaxPing));
                byte[] gamePortBytes = BitConverter.GetBytes(thisInstance.profileBindPort);
                byte[] flagBallScoreBytes = BitConverter.GetBytes(thisInstance.gameScoreFlags);
                byte[] zoneTimerBytes = BitConverter.GetBytes(thisInstance.gameScoreZoneTime);
                byte[] customMapFlagBytes = BitConverter.GetBytes(Convert.ToInt32(mapInstance.currentMapPlaylist[0].CustomMap));

                byte[] mapListPrehandle = BitConverter.GetBytes(10621344);
                byte[] finalAppSetup = Functions.ToByteArray("00 00 00 00 00 00 00 00 05 00 00 00 00".Replace(" ", ""));
                byte[] resolutionSetup = Functions.ToByteArray("02 00 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] graphicsPrehandle = Functions.ToByteArray("02 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] defaultWeaponSetup = Functions.ToByteArray("05 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00".Replace(" ", ""));
                byte[] endOfMapCfg = Functions.ToByteArray("20 B5 B6 01".Replace(" ", ""));
                byte[] endOfMapCfg2 = Functions.ToByteArray("53 01 00 00 00 13 00 00 00 13 00 00 00 04 00 00 00".Replace(" ", ""));


                ms.Seek(0, SeekOrigin.Begin);
                // autorestart header + Number of Total Maps
                ms.Write(autoRestart, 0, autoRestart.Length);
                ms.Write(numberOfMapsBytes, 0, numberOfMapsBytes.Length);

                ms.Seek(0x4D, SeekOrigin.Begin);
                ms.Write(firstMapFile, 0, firstMapFile.Length);

                ms.Seek(0xAF, SeekOrigin.Begin);
                ms.Write(customMapFlagBytes, 0, customMapFlagBytes.Length);

                ms.Seek(0x68F, SeekOrigin.Begin);
                ms.Write(resolutionSetup, 0, resolutionSetup.Length);

                ms.Seek(0x277, SeekOrigin.Begin);
                ms.Write(sessionTypeBytes, 0, sessionTypeBytes.Length);

                ms.Seek(0x1C7, SeekOrigin.Begin);
                ms.Write(applicationSettingBytes, 0, applicationSettingBytes.Length);

                ms.Seek(0x283, SeekOrigin.Begin);
                ms.Write(dedicatedSlotsBytes, 0, dedicatedSlotsBytes.Length);

                ms.Seek(0x28F, SeekOrigin.Begin);
                ms.Write(gameTypeBytes, 0, gameTypeBytes.Length);

                ms.Seek(0x293, SeekOrigin.Begin);
                ms.Write(finalAppSetup, 0, finalAppSetup.Length);

                ms.Seek(0x1347, SeekOrigin.Begin);
                ms.Write(graphicsPrehandle, 0, graphicsPrehandle.Length);

                ms.Seek(0x134F, SeekOrigin.Begin);
                ms.Write(graphicsHeaderSettings, 0, graphicsHeaderSettings.Length);

                ms.Seek(0x1353, SeekOrigin.Begin);
                ms.Write(graphicsSetting_1, 0, graphicsSetting_1.Length);

                ms.Seek(0x1357, SeekOrigin.Begin);
                ms.Write(windowedModeBytes, 0, windowedModeBytes.Length);

                ms.Seek(0x135F, SeekOrigin.Begin);
                ms.Write(graphicsSetup_Name, 0, graphicsSetup_Name.Length);

                ms.Seek(0x137F, SeekOrigin.Begin);
                ms.Write(graphicsSetup_GUID, 0, graphicsSetup_GUID.Length);
                ms.Write(graphicsSetupMisc_Settings, 0, graphicsSetupMisc_Settings.Length);

                ms.Seek(0x152F, SeekOrigin.Begin);
                ms.Write(serverPasswordBytes, 0, serverPasswordBytes.Length);

                ms.Seek(0x1562, SeekOrigin.Begin);
                ms.Write(redTeamPasswordBytes, 0, redTeamPasswordBytes.Length);

                ms.Seek(0x1573, SeekOrigin.Begin);
                ms.Write(blueTeamPasswordBytes, 0, blueTeamPasswordBytes.Length);

                ms.Seek(0x151F, SeekOrigin.Begin);
                ms.Write(gamePlayOptionsBytes, 0, gamePlayOptionsBytes.Length);

                ms.Seek(0x15A6, SeekOrigin.Begin);
                ms.Write(ServerNameBytes, 0, ServerNameBytes.Length);

                ms.Seek(0x15C6, SeekOrigin.Begin);
                ms.Write(countryCodeBytes, 0, countryCodeBytes.Length);

                ms.Seek(0x1613, SeekOrigin.Begin);
                ms.Write(dedicatedBytes, 0, dedicatedBytes.Length);

                ms.Seek(0x15EA, SeekOrigin.Begin);
                ms.Write(BindAddress, 0, BindAddress.Length);

                ms.Seek(0x160B, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x161F, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(100), 0, BitConverter.GetBytes(100).Length);

                ms.Seek(0x162F, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1633, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(210), 0, BitConverter.GetBytes(210).Length);

                ms.Seek(0x1637, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x163B, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(2), 0, BitConverter.GetBytes(2).Length);

                ms.Seek(0x164B, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1693, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1697, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x169B, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(15), 0, BitConverter.GetBytes(15).Length);

                ms.Seek(0x16B7, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(2), 0, BitConverter.GetBytes(2).Length);

                ms.Seek(0x16BB, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x16C7, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x16CB, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(2), 0, BitConverter.GetBytes(2).Length);

                ms.Seek(0x16EF, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(4), 0, BitConverter.GetBytes(4).Length);

                ms.Seek(0x16F4, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x16FC, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1703, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1707, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);

                ms.Seek(0x1627, SeekOrigin.Begin);
                ms.Write(startDelayBytes, 0, startDelayBytes.Length);

                ms.Seek(0x16F3, SeekOrigin.Begin);
                ms.Write(minPingValueBytes, 0, minPingValueBytes.Length);

                ms.Seek(0x16F7, SeekOrigin.Begin);
                ms.Write(enableMinPingBytes, 0, enableMinPingBytes.Length);

                ms.Seek(0x16FB, SeekOrigin.Begin);
                ms.Write(maxPingValueBytes, 0, maxPingValueBytes.Length);

                ms.Seek(0x16FF, SeekOrigin.Begin);
                ms.Write(enableMaxPingBytes, 0, enableMaxPingBytes.Length);

                ms.Seek(0x160F, SeekOrigin.Begin);
                ms.Write(maxSlotsBytes, 0, maxSlotsBytes.Length);

                ms.Seek(0x16CF, SeekOrigin.Begin);
                ms.Write(gamePortBytes, 0, gamePortBytes.Length);

                ms.Seek(0x16DB, SeekOrigin.Begin);
                ms.Write(requireNovaLoginBytes, 0, requireNovaLoginBytes.Length);

                ms.Seek(0x16D7, SeekOrigin.Begin);
                ms.Write(allowCustomSkinsBytes, 0, allowCustomSkinsBytes.Length);

                ms.Seek(0x170B, SeekOrigin.Begin);
                ms.Write(MOTDBytes, 0, MOTDBytes.Length);

                ms.Seek(0x1623, SeekOrigin.Begin);
                ms.Write(flagBallScoreBytes, 0, flagBallScoreBytes.Length);

                ms.Seek(0x1643, SeekOrigin.Begin);
                ms.Write(zoneTimerBytes, 0, zoneTimerBytes.Length);

                ms.Seek(0x1647, SeekOrigin.Begin);
                ms.Write(respawnTimeBytes, 0, respawnTimeBytes.Length);

                ms.Seek(0x163F, SeekOrigin.Begin);
                ms.Write(timeLimitBytes, 0, timeLimitBytes.Length);

                ms.Seek(0x1DA4, SeekOrigin.Begin);
                ms.Write(GameScoreBytes, 0, GameScoreBytes.Length);

                ms.Seek(0x178B, SeekOrigin.Begin);
                ms.Write(defaultWeaponSetup, 0, defaultWeaponSetup.Length);

                ms.Seek(0x187F, SeekOrigin.Begin);
                ms.Write(mapListPrehandle, 0, mapListPrehandle.Length);

                byte[] endOfMap = Functions.ToByteArray("20 B5 B6 01 00 00 00 00 53 01 00 00 00 13 00 00 00 13 00 00 00 04 00 00 00".Replace(" ", ""));

                foreach (var map in mapInstance.currentMapPlaylist)
                {
                    byte[] mapFile = Encoding.Default.GetBytes(map.MapFile!);
                    ms.Write(mapFile, 0, mapFile.Length);

                    ms.Seek(ms.Position + (0x20F - mapFile.Length), SeekOrigin.Begin);
                    byte[] mapName = Encoding.Default.GetBytes(map.MapName!);
                    ms.Write(mapName, 0, mapName.Length);

                    ms.Seek(ms.Position + (0x305 - mapName.Length), SeekOrigin.Begin);
                    ms.Write(endOfMap, 0, endOfMap.Length);

                    ms.Seek(ms.Position + 0x1E3, SeekOrigin.Begin);
                    byte[] customMap = BitConverter.GetBytes(Convert.ToInt32(map.CustomMap!));
                    ms.Write(customMap, 0, customMap.Length);

                    // prepare for next entry
                    ms.Seek(ms.Position + 0x1C, SeekOrigin.Begin);
                }

                for (int i = mapInstance.currentMapPlaylist.Count; i < 128; i++)
                {
                    byte[] mapFile = Encoding.Default.GetBytes("NA.bms");
                    ms.Write(mapFile, 0, mapFile.Length);

                    ms.Seek(ms.Position + (0x20F - mapFile.Length), SeekOrigin.Begin);
                    byte[] mapName = Encoding.Default.GetBytes("NA");
                    ms.Write(mapName, 0, mapName.Length);

                    ms.Seek(ms.Position + (0x305 - mapName.Length), SeekOrigin.Begin);
                    ms.Write(endOfMap, 0, endOfMap.Length);

                    ms.Seek(ms.Position + 0x1E3, SeekOrigin.Begin);
                    byte[] customMap = BitConverter.GetBytes(Convert.ToInt32(false));
                    ms.Write(customMap, 0, customMap.Length);

                    // prepare for next entry
                    ms.Seek(ms.Position + 0x1C, SeekOrigin.Begin);
                }

                BinaryWriter writer = new BinaryWriter(File.Open(autoResPath, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                writer.Seek(0, SeekOrigin.Begin);
                writer.Write(ms.ToArray());
                writer.Close();

                Thread.Sleep(1000); // sleep 100ms to allow flushing the file to complete

                return true;

            }
            catch (Exception e)
            {
                AppDebug.Log("StartServer", "Error creating autores.bin file: " + e.ToString());
                return false;
            }

        }
        // Function: CmdStartGame
        public static bool startGame()
        {
            string file_name = "dfbhd.exe";
            string FullFileName = Path.Combine(thisInstance.profileServerPath!, file_name);
            string windowTitle = $"BHD Server - {thisInstance.gameServerName}";

            try
            {
                // Is there an instance already running? Locate by the profileServerPath
                foreach (var searchProcess in Process.GetProcesses())
                {
                    try
                    {
                        if (string.Equals(searchProcess.MainModule?.FileName, FullFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            AppDebug.Log("StartServer", "Found existing game process: " + searchProcess.ProcessName + " (PID: " + searchProcess.Id + ")");
                            thisInstance.instanceAttachedPID = searchProcess.Id;
                            thisInstance.instanceProcessHandle = searchProcess.Handle;

                            SetProcessWindowTitle(searchProcess, windowTitle);

                            ServerMemory.AttachToGameProcess();

                            return true;
                        }
                    }
                    catch (Win32Exception)
                    {
                        // Skip processes we can't access
                        continue;
                    }
                }

                Debug.WriteLine("No existing game process found, starting a new instance...");

                // Create the AutoRes File
                createAutoRes();

                // Start Game
                var startInfo = new ProcessStartInfo
                {
                    FileName = FullFileName,
                    WorkingDirectory = thisInstance.profileServerPath,
                    Arguments = "/w /LOADBAR /NOSYSDUMP /serveonly /autorestart",
                    // WindowStyle = ProcessWindowStyle.Minimized
                };
                Process? process = Process.Start(startInfo);

                if (process == null)
                    throw new InvalidOperationException("Failed to start the game process.");

                // Set MaxWorkingSet and instanceAttachedPID directly from the started process
                process.MaxWorkingSet = new nint(0x7fffffff);
                thisInstance.instanceAttachedPID = process.Id;
                thisInstance.instanceProcessHandle = process.Handle;

                SetProcessWindowTitle(process, windowTitle);

                ServerMemory.AttachToGameProcess();
            }
            catch (Exception e)
            {
                AppDebug.Log("StartServer", "Error starting game: " + e.ToString());
                return false;
            }

            // Game Didn't Crash Yay!  Dump Data to the Instance
            thisInstance.instanceCrashCounter = 0;

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
            try
            {
                if (thisInstance.instanceAttachedPID.HasValue)
                {
                    try
                    {
                        ServerMemory.DetachFromGameProcess();

                        var process = Process.GetProcessById(thisInstance.instanceAttachedPID.Value);
                        if (!process.HasExited)
                        {
                            process.Kill(true); // true for entire process tree (.NET 5+)
                            process.WaitForExit(5000); // Wait up to 5 seconds for exit
                        }

                    }
                    catch (ArgumentException)
                    {
                        // Process already exited or does not exist
                    }
                    catch (Exception ex)
                    {
                        AppDebug.Log("StartServer", "Error killing game process: " + ex.ToString());
                        return false;
                    }
                    finally
                    {
                        thisInstance.instanceAttachedPID = null;
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
