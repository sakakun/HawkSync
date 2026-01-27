using BHD_ServerManager.Classes.GameManagement;
using BHD_ServerManager.Forms;
using BHD_ServerManager.Classes.CoreObjects;
using BHD_ServerManager.Classes.Instances;
using BHD_ServerManager.Classes.ObjectClasses;
using BHD_ServerManager.Classes.SupportClasses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BHD_ServerManager.Classes.StatsManagement
{
    public static class statsInstanceManager
    {
        private static theInstance theInstance = CommonCore.theInstance!;
        private static statInstance instanceStats = CommonCore.instanceStats!;
        private static ServerManagerUI thisServer => Program.ServerManagerUI!;

        // Throttle updates to once every 15 seconds
        private static DateTime _lastPlayerStatsUpdate = DateTime.MinValue;
        // Throttle updates to once every 15 seconds
        private static DateTime _lastWeaponStatsUpdate = DateTime.MinValue;

        public static void RunPlayerStatsUpdate()
        {
            foreach (var playerRecord in theInstance.playerList)
            {
                var playerObj = playerRecord.Value;
                if (playerObj != null && playerObj.PlayerNameBase64 != null)
                {
                    UpdatePlayerStats(playerObj);
                }
            }
        }

        public static void UpdatePlayerStats(playerObject CurrentPlayerObject)
        {
            if (CurrentPlayerObject == null || string.IsNullOrEmpty(CurrentPlayerObject.PlayerNameBase64))
                return;

            if (!instanceStats.playerStatsList.TryGetValue(CurrentPlayerObject.PlayerNameBase64, out var statObj))
            {
                statObj = new PlayerStatObject
                {
                    PlayerStatsCurrent = ClonePlayerObject(CurrentPlayerObject),
                    PlayerStatsPrevious = ClonePlayerObject(CurrentPlayerObject),
                    PlayerWeaponStats = new List<WeaponStatObject>()
                };
                instanceStats.playerStatsList[CurrentPlayerObject.PlayerNameBase64] = statObj;
                UpdateWeaponStats(statObj, CurrentPlayerObject, isNew: true);
            }
            else
            {
                statObj.PlayerStatsPrevious = ClonePlayerObject(statObj.PlayerStatsCurrent);
                statObj.PlayerStatsCurrent = ClonePlayerObject(CurrentPlayerObject);
                UpdateWeaponStats(statObj, CurrentPlayerObject, isNew: false);
            }
        }

        private static void UpdateWeaponStats(PlayerStatObject statObj, playerObject player, bool isNew)
        {
            if (isNew)
            {
                statObj.PlayerWeaponStats.Add(new WeaponStatObject
                {
                    WeaponID = player.SelectedWeaponID,
                    TimeUsed = player.PlayerTimePlayed,
                    Kills = player.stat_Kills,
                    Shots = player.stat_TotalShotsFired
                });
            }
            else
            {
                var diff = ComparePlayerStats(statObj.PlayerStatsCurrent, statObj.PlayerStatsPrevious);
                var weaponStat = statObj.PlayerWeaponStats
                    .FirstOrDefault(w => w.WeaponID == player.SelectedWeaponID);

                if (weaponStat == null)
                {
                    weaponStat = new WeaponStatObject
                    {
                        WeaponID = player.SelectedWeaponID
                    };
                    statObj.PlayerWeaponStats.Add(weaponStat);
                }

                weaponStat.TimeUsed += diff.PlayerTimePlayed;
                weaponStat.Kills += diff.stat_Kills;
                weaponStat.Shots += diff.stat_TotalShotsFired;
            }
        }

        public static playerObject ComparePlayerStats(playerObject CurrentPlayerObject, playerObject PreviousPlayerObject)
        {
            if (CurrentPlayerObject == null || PreviousPlayerObject == null)
                return new playerObject();

            return new playerObject
            {
                PlayerSlot = CurrentPlayerObject.PlayerSlot,
                PlayerName = CurrentPlayerObject.PlayerName,
                PlayerNameBase64 = CurrentPlayerObject.PlayerNameBase64,
                PlayerIPAddress = CurrentPlayerObject.PlayerIPAddress,
                PlayerTeam = CurrentPlayerObject.PlayerTeam,
                PlayerPing = CurrentPlayerObject.PlayerPing,
                PlayerTimePlayed = CurrentPlayerObject.PlayerTimePlayed - PreviousPlayerObject.PlayerTimePlayed,
                RoleID = CurrentPlayerObject.RoleID,
                RoleName = CurrentPlayerObject.RoleName,
                ActiveZoneTime = CurrentPlayerObject.ActiveZoneTime - PreviousPlayerObject.ActiveZoneTime,
                stat_Kills = CurrentPlayerObject.stat_Kills - PreviousPlayerObject.stat_Kills,
                stat_Deaths = CurrentPlayerObject.stat_Deaths - PreviousPlayerObject.stat_Deaths,
                stat_ZoneTime = CurrentPlayerObject.stat_ZoneTime - PreviousPlayerObject.stat_ZoneTime,
                stat_ZoneKills = CurrentPlayerObject.stat_ZoneKills - PreviousPlayerObject.stat_ZoneKills,
                stat_ZoneDefendKills = CurrentPlayerObject.stat_ZoneDefendKills - PreviousPlayerObject.stat_ZoneDefendKills,
                stat_RevivesGiven = CurrentPlayerObject.stat_RevivesGiven - PreviousPlayerObject.stat_RevivesGiven,
                stat_FBCaptures = CurrentPlayerObject.stat_FBCaptures - PreviousPlayerObject.stat_FBCaptures,
                stat_Suicides = CurrentPlayerObject.stat_Suicides - PreviousPlayerObject.stat_Suicides,
                stat_Murders = CurrentPlayerObject.stat_Murders - PreviousPlayerObject.stat_Murders,
                stat_Headshots = CurrentPlayerObject.stat_Headshots - PreviousPlayerObject.stat_Headshots,
                stat_KnifeKills = CurrentPlayerObject.stat_KnifeKills - PreviousPlayerObject.stat_KnifeKills,
                stat_RevivesReceived = CurrentPlayerObject.stat_RevivesReceived - PreviousPlayerObject.stat_RevivesReceived,
                stat_PSPAttempts = CurrentPlayerObject.stat_PSPAttempts - PreviousPlayerObject.stat_PSPAttempts,
                stat_PSPTakeovers = CurrentPlayerObject.stat_PSPTakeovers - PreviousPlayerObject.stat_PSPTakeovers,
                stat_DoubleKills = CurrentPlayerObject.stat_DoubleKills - PreviousPlayerObject.stat_DoubleKills,
                stat_FBCarrierKills = CurrentPlayerObject.stat_FBCarrierKills - PreviousPlayerObject.stat_FBCarrierKills,
                stat_FBCarrierDeaths = CurrentPlayerObject.stat_FBCarrierDeaths - PreviousPlayerObject.stat_FBCarrierDeaths,
                stat_ExperiencePoints = CurrentPlayerObject.stat_ExperiencePoints - PreviousPlayerObject.stat_ExperiencePoints,
                stat_SDADTargetsDestroyed = CurrentPlayerObject.stat_SDADTargetsDestroyed - PreviousPlayerObject.stat_SDADTargetsDestroyed,
                stat_SDADDefenseKills = CurrentPlayerObject.stat_SDADDefenseKills - PreviousPlayerObject.stat_SDADDefenseKills,
                stat_SDADAttackKills = CurrentPlayerObject.stat_SDADAttackKills - PreviousPlayerObject.stat_SDADAttackKills,
                stat_FlagSaves = CurrentPlayerObject.stat_FlagSaves - PreviousPlayerObject.stat_FlagSaves,
                stat_SniperKills = CurrentPlayerObject.stat_SniperKills - PreviousPlayerObject.stat_SniperKills,
                stat_TKOTHDefenseKills = CurrentPlayerObject.stat_TKOTHDefenseKills - PreviousPlayerObject.stat_TKOTHDefenseKills,
                stat_TKOTHAttackKills = CurrentPlayerObject.stat_TKOTHAttackKills - PreviousPlayerObject.stat_TKOTHAttackKills,
                stat_TotalShotsFired = CurrentPlayerObject.stat_TotalShotsFired - PreviousPlayerObject.stat_TotalShotsFired,
                PlayerWeapons = new List<string>(CurrentPlayerObject.PlayerWeapons),
                SelectedWeaponID = CurrentPlayerObject.SelectedWeaponID,
                SelectedWeaponName = CurrentPlayerObject.SelectedWeaponName
            };
        }

        private static playerObject ClonePlayerObject(playerObject obj)
        {
            if (obj == null) return new playerObject();
            return new playerObject
            {
                PlayerSlot = obj.PlayerSlot,
                PlayerName = obj.PlayerName,
                PlayerNameBase64 = obj.PlayerNameBase64,
                PlayerIPAddress = obj.PlayerIPAddress,
                PlayerTeam = obj.PlayerTeam,
                PlayerPing = obj.PlayerPing,
                PlayerTimePlayed = obj.PlayerTimePlayed,
                RoleID = obj.RoleID,
                RoleName = obj.RoleName,
                stat_Kills = obj.stat_Kills,
                stat_Deaths = obj.stat_Deaths,
                stat_ZoneTime = obj.stat_ZoneTime,
                stat_ZoneKills = obj.stat_ZoneKills,
                stat_ZoneDefendKills = obj.stat_ZoneDefendKills,
                stat_RevivesGiven = obj.stat_RevivesGiven,
                stat_FBCaptures = obj.stat_FBCaptures,
                stat_Suicides = obj.stat_Suicides,
                stat_Murders = obj.stat_Murders,
                stat_Headshots = obj.stat_Headshots,
                stat_KnifeKills = obj.stat_KnifeKills,
                stat_RevivesReceived = obj.stat_RevivesReceived,
                stat_PSPAttempts = obj.stat_PSPAttempts,
                stat_PSPTakeovers = obj.stat_PSPTakeovers,
                stat_DoubleKills = obj.stat_DoubleKills,
                stat_FBCarrierKills = obj.stat_FBCarrierKills,
                stat_FBCarrierDeaths = obj.stat_FBCarrierDeaths,
                stat_ExperiencePoints = obj.stat_ExperiencePoints,
                stat_SDADTargetsDestroyed = obj.stat_SDADTargetsDestroyed,
                stat_FlagSaves = obj.stat_FlagSaves,
                stat_SniperKills = obj.stat_SniperKills,
                stat_TKOTHDefenseKills = obj.stat_TKOTHDefenseKills,
                stat_TKOTHAttackKills = obj.stat_TKOTHAttackKills,
                stat_TotalShotsFired = obj.stat_TotalShotsFired,
                PlayerWeapons = new List<string>(obj.PlayerWeapons),
                SelectedWeaponID = obj.SelectedWeaponID,
                SelectedWeaponName = obj.SelectedWeaponName
            };
        }

        public static string GenerateImportData()
        {
            string importData = string.Empty;
            importData += "ServerID " + theInstance.WebStatsProfileID + "\n";
            importData += GenerateGameLine() + "\n";
            foreach (var playerStat in instanceStats.playerStatsList.Values)
            {
                importData += GeneratePlayerLines(playerStat);
            }
            importData += "\n\n\n\n";
            return importData;
        }

        public static string GenerateGameLine()
        {
            if (theInstance.instanceStatus == InstanceStatus.SCORING)
                ServerMemory.ReadMemoryWinningTeam();

            int totalSeconds = theInstance.gameTimeLimit * 60;
            int timeRemainingSeconds = (int)theInstance.gameInfoTimeRemaining.TotalSeconds;
            int timerSeconds = Math.Max(0, totalSeconds - timeRemainingSeconds);

            string timer = timerSeconds.ToString();
            string date = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
            string gametype = objectGameTypes.GetShortName(theInstance.gameInfoGameType) ?? "0";
            string dedicated = theInstance.gameDedicated ? "1" : "0";
            string servername = theInstance.gameServerName;
            string mapname = theInstance.gameInfoMapName;
            string maxplayers = theInstance.gameMaxSlots.ToString() ?? "0";
            string numplayers = theInstance.playerList?.Count.ToString() ?? "0";
            string winner = theInstance.gameMatchWinner.ToString();
            string mod = (theInstance.profileServerType == 0) ? "7" : "8";

            string gameLine = string.Empty;
            if (theInstance.instanceStatus == InstanceStatus.SCORING)
            {
                gameLine = $" Game {timer}__&__{date}__&__{gametype}__&__{dedicated}__&__{servername}__&__{mapname}__&__{maxplayers}__&__{numplayers}__&__{winner}__&__{mod}";
            }
            else
            {
                gameLine = $" Game {timer}__&__{date}__&__{gametype}__&__{dedicated}__&__{servername}__&__{mapname}__&__{maxplayers}__&__{numplayers}__&__{mod}";
            }
            AppDebug.Log("StatsManager", $"Generated Game Line: {gameLine}");
            return gameLine;
        }

        public static string GeneratePlayerLines(PlayerStatObject playerStats)
        {
            string PlayerLines = string.Empty;
            playerObject player = playerStats.PlayerStatsCurrent;
            PlayerLines = "  Player " + player.PlayerName + "__&__" + player.PlayerIPAddress + "\n";
            PlayerLines += GeneratePlayerStatLine(player);
            PlayerLines += GenerateWeaponStatLines(playerStats.PlayerWeaponStats);
            return PlayerLines;
        }

        public static string GeneratePlayerStatLine(playerObject player)
        {
            string PlayerStatLine = string.Empty;
            PlayerStatLine = "   PlayerStats ";
            PlayerStatLine += player.stat_Suicides + " ";
            PlayerStatLine += player.stat_Murders + " ";
            PlayerStatLine += player.stat_Kills + " ";
            PlayerStatLine += player.stat_Deaths + " ";
            PlayerStatLine += player.stat_ZoneTime + " ";
            PlayerStatLine += player.stat_FBCaptures + " ";
            PlayerStatLine += player.stat_FlagSaves + " ";
            PlayerStatLine += player.stat_SDADTargetsDestroyed + " ";
            PlayerStatLine += player.stat_RevivesReceived + " ";
            PlayerStatLine += player.stat_RevivesGiven + " ";
            PlayerStatLine += player.stat_PSPAttempts + " ";
            PlayerStatLine += player.stat_PSPTakeovers + " ";
            PlayerStatLine += player.stat_FBCarrierKills + " ";
            PlayerStatLine += player.stat_DoubleKills + " ";
            PlayerStatLine += player.stat_Headshots + " ";
            PlayerStatLine += player.stat_KnifeKills + " ";
            PlayerStatLine += player.stat_SniperKills + " ";
            PlayerStatLine += player.stat_TKOTHDefenseKills + " ";
            PlayerStatLine += player.stat_TKOTHAttackKills + " ";
            PlayerStatLine += player.stat_SDADDefenseKills + " ";
            PlayerStatLine += "0 "; // sdadpolicekills
            PlayerStatLine += player.stat_SDADAttackKills + " ";
            PlayerStatLine += "0 "; // sdadsecurekills
            PlayerStatLine += player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
            PlayerStatLine += " "; // spacer for the SPK ratio
            PlayerStatLine += player.stat_ExperiencePoints + " ";
            PlayerStatLine += player.PlayerTeam + " ";
            bool playerActive = theInstance.playerList.Values.Any(p => p.PlayerName == player.PlayerName);
            PlayerStatLine += playerActive ? "1 " : "0 ";
            PlayerStatLine += player.PlayerTimePlayed;
            return PlayerStatLine + "\n";
        }

        public static string GenerateWeaponStatLines(List<WeaponStatObject> weaponStatList)
        {
            string WeaponStatLines = string.Empty;
            foreach (var weaponStat in weaponStatList)
            {
                WeaponStatLines += "   Weapon " + weaponStat.WeaponID + " " + weaponStat.TimeUsed + " " + weaponStat.Kills + " " + weaponStat.Shots + "\n";
            }
            return WeaponStatLines;
        }

        public static string GenerateUpdateData()
        {
            string updateData = string.Empty;
            updateData += "ServerID " + theInstance.WebStatsProfileID + "\n";
            updateData += GenerateGameLine() + "\n";
            foreach (var playerStat in instanceStats.playerStatsList.Values)
            {
                playerObject Player = playerStat.PlayerStatsCurrent;
                updateData += "  Player " + Player.PlayerName + "\n";
                updateData += "   PlayerStats " + Player.PlayerTimePlayed + " " + Player.PlayerTeam + " " + Player.SelectedWeaponID + "\n";
                updateData += "\n";
            }
            updateData += "End\n\n\n\n";
            return updateData;
        }

        public static string GenerateReportData()
        {
            string reportData = string.Empty;
            reportData += "1\n";
            reportData += "DFBHD";
            foreach (var playerStat in instanceStats.playerStatsList.Values)
            {
                playerObject Player = playerStat.PlayerStatsCurrent;
                reportData += GeneratePlayerLines(playerStat);
                reportData += "\n";
            }
            reportData += "End\n\n\n\n";
            return reportData;
        }

        public static void ResetPlayerStats()
        {
            instanceStats.playerStatsList.Clear();
            instanceStats.lastPlayerStatsUpdate = DateTime.MinValue;
            instanceStats.lastPlayerStatsReport = DateTime.MinValue;
        }

        // --- ASYNC NETWORK METHODS WITH UI INVOKE FIXES ---

        public static async Task SendImportData(ServerManagerUI thisServer)

        {
            if (!theInstance.WebStatsEnabled || string.IsNullOrEmpty(theInstance.WebStatsProfileID) || string.IsNullOrEmpty(theInstance.WebStatsServerPath))
            {
                AppDebug.Log("StatsManager", "WebStats is not enabled or profile ID/server path is not set.");
                return;
            }

            string POST_URL = theInstance.WebStatsServerPath + "stats_import.php";
            Dictionary<string, string> DATA = new Dictionary<string, string>
            {
                ["serverid"] = theInstance.WebStatsProfileID,
                ["data"] = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(GenerateImportData())),
                ["BMT"] = "1"
            };

            try
            {
                var response = await SendBabstatsData(POST_URL, DATA);
                if (!string.IsNullOrEmpty(response))
                {
                    string responseData = response.Replace("\r", "").Replace("\n", "").Trim();
                    AppDebug.Log("StatsManager", $"Babstats Import Response: {responseData}");
                    AddStatsLogRowSafe(thisServer, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), responseData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending import data: {ex.Message}");
            }
        }

        public static async Task SendUpdateData(ServerManagerUI thisServer)
        {
            string POST_URL = theInstance.WebStatsServerPath + "status_update.php";
            Dictionary<string, string> DATA = new Dictionary<string, string>
            {
                ["serverid"] = theInstance.WebStatsProfileID,
                ["data"] = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(GenerateUpdateData())),
                ["BMT"] = "1"
            };

            try
            {
                var response = await SendBabstatsData(POST_URL, DATA);
                if (!string.IsNullOrEmpty(response))
                {
                    string responseData = response.Replace("\r", "").Replace("\n", "").Trim();
                    AppDebug.Log("StatsManager", $"Babstats Update Response: {responseData}");
                    AddStatsLogRowSafe(thisServer, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), responseData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending update data: {ex.Message}");
            }
        }

        public static async Task<string> SendReportData(ServerManagerUI thisServer)
        {
            string POST_URL = theInstance.WebStatsServerPath + "status_report.php";
            Dictionary<string, string> DATA = new Dictionary<string, string>
            {
                ["serverid"] = theInstance.WebStatsProfileID,
                ["data"] = Convert.ToBase64String(Encoding.GetEncoding("Windows-1252").GetBytes(GenerateReportData())),
                ["BMT"] = "1"
            };

            try
            {
                var response = await SendBabstatsData(POST_URL, DATA);
                if (!string.IsNullOrEmpty(response))
                {
                    AppDebug.Log("StatsManager", $"Babstats Report Response: {response}");
                    string[] messages = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var message in messages)
                    {
                        AddStatsLogRowSafe(thisServer, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending report data: {ex.Message}");
            }
            return string.Empty;
        }

        public static async Task<bool> TestBabstatsConnectionAsync(string WebURL)
        {
            var endpoints = new[]
            {
                "status_update.php",
                "status_report.php",
                "stats_import.php"
            };

            var baseUrl = WebURL;
            var data = new Dictionary<string, string>();
            int responseGood = 0;

            foreach (var endpoint in endpoints)
            {
                string url = baseUrl + endpoint;
                string response = await SendBabstatsData(url, data);

                if (response.IndexOf("no data sent", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    responseGood++;
                    AppDebug.Log("StatsManager", $"Babstats Test: {endpoint} - No data sent.");
                }
                else
                {
                    AppDebug.Log("StatsManager", $"Babstats Test: {url} - Response: {response}");
                }
            }
            if (responseGood == 3)
            {
                return true;
            }

            return false;
        }
        public static async Task<string> SendBabstatsData(string url, Dictionary<string, string> postData)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var httpClient = new HttpClient(handler);
            using var content = new FormUrlEncodedContent(postData);

            string postDataJson = JsonSerializer.Serialize(postData);
            AppDebug.Log("StatsManager", $"Babstats POST Data {url} JSON: {postDataJson}");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync(url, content);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                return $"Error sending data to Babstats server: {e.Message}";
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            AppDebug.Log("StatsManager", $"Babstats Response: {responseContent}");

            return responseContent;
        }

        // --- UI THREAD-SAFE HELPERS ---

        private static void AddStatsLogRowSafe(ServerManagerUI thisServer, string dateTime, string message)
        {
            if (thisServer.StatsTab.dg_statsLog.InvokeRequired)
            {
                thisServer.StatsTab.dg_statsLog.Invoke(new Action(() =>
                    thisServer.StatsTab.dg_statsLog.Rows.Add(dateTime, message)
                ));
            }
            else
            {
                thisServer.StatsTab.dg_statsLog.Rows.Add(dateTime, message);
            }
        }

        public static void PopulatePlayerStatsGrid()
        {
            if ((DateTime.UtcNow - _lastPlayerStatsUpdate).TotalSeconds < 15)
                return;
            _lastPlayerStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewPlayerStats = thisServer.StatsTab.dataGridViewPlayerStats;

            if (dataGridViewPlayerStats.InvokeRequired)
            {
                dataGridViewPlayerStats.Invoke(new Action(() => PopulatePlayerStatsGrid()));
                return;
            }

            dataGridViewPlayerStats.Rows.Clear();

            foreach (var statObj in instanceStats.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                var shotsPerKill = player.stat_Kills > 0 ? (player.stat_TotalShotsFired / player.stat_Kills).ToString() : "0";
                bool playerActive = theInstance.playerList.Values.Any(p => p.PlayerName == player.PlayerName);

                dataGridViewPlayerStats.Rows.Add(
                    player.PlayerName,
                    player.stat_Suicides,
                    player.stat_Murders,
                    player.stat_Kills,
                    player.stat_Deaths,
                    player.stat_ZoneTime,
                    player.stat_FBCaptures,
                    player.stat_FlagSaves,
                    player.stat_SDADTargetsDestroyed,
                    player.stat_RevivesReceived,
                    player.stat_RevivesGiven,
                    player.stat_PSPAttempts,
                    player.stat_PSPTakeovers,
                    player.stat_FBCarrierKills,
                    player.stat_DoubleKills,
                    player.stat_Headshots,
                    player.stat_KnifeKills,
                    player.stat_SniperKills,
                    player.stat_TKOTHDefenseKills,
                    player.stat_TKOTHAttackKills,
                    shotsPerKill,
                    player.stat_ExperiencePoints,
                    player.PlayerTeam,
                    playerActive ? "1" : "0",
                    player.PlayerTimePlayed
                );
            }

        }

        public static void PopulateWeaponStatsGrid()
        {
            if ((DateTime.UtcNow - _lastWeaponStatsUpdate).TotalSeconds < 15)
                return;
            _lastWeaponStatsUpdate = DateTime.UtcNow;

            DataGridView dataGridViewWeaponStats = thisServer.StatsTab.dataGridViewWeaponStats;

            if (dataGridViewWeaponStats.InvokeRequired)
            {
                dataGridViewWeaponStats.Invoke(new Action(() => PopulateWeaponStatsGrid()));
                return;
            }

            dataGridViewWeaponStats.Rows.Clear();

            foreach (var statObj in instanceStats.playerStatsList.Values)
            {
                var player = statObj.PlayerStatsCurrent;
                foreach (var weaponStat in statObj.PlayerWeaponStats)
                {
                    dataGridViewWeaponStats.Rows.Add(
                        player.PlayerName,
                        ((WeaponStack)weaponStat.WeaponID).ToString(),
                        weaponStat.TimeUsed,
                        weaponStat.Kills,
                        weaponStat.Shots
                    );
                }
            }
        }
    }
}