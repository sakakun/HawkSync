using System;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using HawkSyncShared;
using HawkSyncShared.DTOs.tabMaps;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.GameManagement;

namespace BHD_ServerManager.Classes.Helpers
{
    public static class LobbyReportHelper
    {
        // Reused static HttpClient for connection pooling and to avoid socket exhaustion.
        // We use per-call CancellationTokenSource with a 10s CancelAfter to enforce the 10s kill time.
        private static readonly HttpClient s_httpClient = new HttpClient
        {
            // Use an infinite timeout on the client itself and enforce per-request timeout via CancellationToken.
            Timeout = System.Threading.Timeout.InfiniteTimeSpan
        };

        // Cache HttpClient instances keyed by local IP (string). "default" key reuses s_httpClient.
        private static readonly ConcurrentDictionary<string, HttpClient> s_httpClientsByLocalIp = new ConcurrentDictionary<string, HttpClient>(StringComparer.OrdinalIgnoreCase);

        // Returns an HttpClient bound to the given local IP. If localIp is null/empty, returns the default shared client.
        private static HttpClient GetOrCreateHttpClientForLocalIp(string? localIp)
        {
            var key = string.IsNullOrWhiteSpace(localIp) ? "default" : localIp!.Trim();

            // Treat explicit "0.0.0.0" (IPv4 Any) as the default behaviour:
            // the script passes "selected" IP; when it passes "0.0.0.0" we should not bind the outgoing socket
            // to the unspecified address — instead reuse the shared client so the system chooses the appropriate local IP.
            if (key == "default" || key == "0.0.0.0")
                return s_httpClient;

            return s_httpClientsByLocalIp.GetOrAdd(key, _ =>
            {
                if (!IPAddress.TryParse(localIp, out var bindAddress))
                    throw new ArgumentException("Invalid local IP address", nameof(localIp));

                // Defensively treat parsed IPAddress.Any (0.0.0.0) as default too
                if (bindAddress.Equals(IPAddress.Any))
                    return s_httpClient;

                var handler = new SocketsHttpHandler
                {
                    // Keep client-level timeout infinite; per-request cancellation enforces 10s.
                    ConnectCallback = async (context, ct) =>
                    {
                        // Create socket for address family matching the bind address.
                        var socket = new Socket(bindAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            // Bind to the specified local IP with an ephemeral port (0).
                            socket.Bind(new IPEndPoint(bindAddress, 0));

                            // Connect to remote endpoint (host/port). Let the system resolve DNS as usual.
                            await socket.ConnectAsync(context.DnsEndPoint.Host, context.DnsEndPoint.Port, ct).ConfigureAwait(false);
                            return new NetworkStream(socket, ownsSocket: true);
                        }
                        catch
                        {
                            socket.Dispose();
                            throw;
                        }
                    }
                };

                return new HttpClient(handler, disposeHandler: true) { Timeout = System.Threading.Timeout.InfiniteTimeSpan };
            });
        }

        /// <summary>
        /// Compatible function to be able to report to the import.php script used by NovaHQ for server listing and stats tracking.
        /// This is a POST request based on the observed behaviour of the NovaHQ php "import" and variable seen in a Wireshark capture of the traffic.
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="SKey"></param>
        /// <param name="reportPort"></param>
        /// <param name="modString"></param>
        /// <param name="cancellationToken">Optional token. Method enforces a 10 second per-request timeout in addition to this token.</param>
        /// <returns></returns>
        public static async Task<HeartBeatResponse> SendHeartbeat(string uriString = "http://nw.novahq.net/server/heartbeat-dll", string SKey = "SECRET_KEY", string reportPort = null!, string modString = null!, string? localIp = null, CancellationToken cancellationToken = default)
        {
			// uriString default is never used as in practice the script always passes the URI, but it's there for safety and testing purposes.
			// Per-request cancellation source that is linked to the caller's token and enforces a 10s timeout.
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(TimeSpan.FromSeconds(10));
            var ct = linkedCts.Token;

            try
            {
                var uri = new Uri(uriString);
                var encoding = Encoding.GetEncoding(1252); // Windows-1252

                var theInstance = CommonCore.theInstance;
                var maps = CommonCore.instanceMaps;
                if (theInstance is null || maps is null)
                {
                    // This should not happen in normal operation, but return a clear response if it does.
                    return new HeartBeatResponse { StatusCode = 0, ReasonPhrase = "Missing CommonCore instances" };
                }

                var serverName = theInstance.gameServerName ?? string.Empty;
                var serverPort = reportPort ?? theInstance.profileBindPort.ToString();
                var serverMessage = theInstance.gameMOTD ?? string.Empty;
                var countryCode = theInstance.gameCountryCode ?? string.Empty;
                var isDedicated = theInstance.gameDedicated ? "Dedicated" : "Non-Dedicated";
                var numPlayers = theInstance.gameInfoNumPlayers.ToString();
                var maxPlayers = theInstance.gameMaxSlots.ToString();
                var mapType = GameTypeObject.GetShortName(maps.CurrentGameType) ?? string.Empty;
                var mapName = maps.CurrentMapName ?? string.Empty;
                var mapFile = maps.CurrentMapFile ?? string.Empty;
                var modText = IsTeamSaber() + (modString ?? string.Empty);

                var isPassworded = "HiddenPassword";
                if (string.IsNullOrEmpty(theInstance.gamePasswordLobby) &&
                    string.IsNullOrEmpty(theInstance.gamePasswordRed) &&
                    string.IsNullOrEmpty(theInstance.gamePasswordBlue) &&
                    string.IsNullOrEmpty(theInstance.gamePasswordYellow) &&
                    string.IsNullOrEmpty(theInstance.gamePasswordViolet))
                {
                    isPassworded = string.Empty;
                }

                var fields = new Dictionary<string, string>
                {
                    ["Encoding"] = "Windows-1252",
                    ["PKey"] = "eYkJaPPR-3WNbgPN93,(ZwxBCnEW", // consider moving secrets out of source
                    ["PVer"] = "1.0.9",
                    ["SKey"] = SKey,
                    ["DataType"] = "0x100",
                    ["GameID"] = "dfbhd",
                    ["Name"] = serverName,
                    ["Port"] = serverPort,
                    ["CK"] = "0",
                    ["Country"] = countryCode,
                    ["Type"] = isDedicated,
                    ["GameType"] = mapType,
                    ["CurrentPlayers"] = numPlayers,
                    ["MaxPlayers"] = maxPlayers,
                    ["MissionName"] = mapName,
                    ["MissionFile"] = mapFile,
                    ["TimeRemaining"] = (theInstance.gameInfoTimeRemaining.TotalSeconds*60).ToString(),
                    ["Password"] = isPassworded,
                    ["Message"] = serverMessage,
                    ["Mod"] = modText,
                    ["PlayerList"] = BuildPlayerList()
                };

                // Build application/x-www-form-urlencoded body using specified encoding
                var parts = new List<string>(fields.Count);
                foreach (var kv in fields)
                {
                    parts.Add(UrlEncode(kv.Key, encoding) + "=" + UrlEncode(kv.Value, encoding));
                }
                var body = string.Join("&", parts);
                var bodyBytes = encoding.GetBytes(body);

                using var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.UserAgent.ParseAdd("NovaHQ Heartbeat DLL (1.0.9)");
                request.Headers.Connection.Add("Keep-Alive");
                request.Headers.Host = uri.Host;

                var content = new ByteArrayContent(bodyBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                request.Content = content;

                var client = GetOrCreateHttpClientForLocalIp(localIp);
                using var resp = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                
                AppDebug.Log("SendHeartbeat", $"Heartbeat Successful! ({uriString})");
                
                return new HeartBeatResponse
                {
                    StatusCode = (int)resp.StatusCode,
                    ReasonPhrase = resp.ReasonPhrase ?? string.Empty
                };
            }
            catch (OperationCanceledException)
            {
                AppDebug.Log("SendHeartbeat", $"Failed to send heartbeat to {uriString}");
                // Distinguish cancellation/timeout from other failures.
                return new HeartBeatResponse
                {
                    StatusCode = -2,
                    ReasonPhrase = "Request canceled or timed out (10s)"
				};
            }
            catch (Exception ex)
            {
                AppDebug.Log("SendHeartbeat", $"Failed to send heartbeat to {uriString}");
                return new HeartBeatResponse
                {
                    StatusCode = -1,
                    ReasonPhrase = ex.Message
                };
            }
        }

        private static string UrlEncode(string value, Encoding enc)
        {
            if (value == null) return string.Empty;
            var bytes = enc.GetBytes(value);
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                // Unreserved characters according to RFC 3986
                if ((b >= 0x30 && b <= 0x39) || (b >= 0x41 && b <= 0x5A) || (b >= 0x61 && b <= 0x7A) || b == 0x2D || b == 0x5F || b == 0x2E || b == 0x7E)
                {
                    sb.Append((char)b);
                }
                else if (b == 0x20)
                {
                    sb.Append('+');
                }
                else
                {
                    sb.Append('%').Append(b.ToString("X2"));
                }
            }
            return sb.ToString();
        }

        private static string IsTeamSaber()
        {
            // If file "EXP1.pff" exists in the game directoy.
            if (System.IO.File.Exists(System.IO.Path.Combine(CommonCore.theInstance!.profileServerPath, "EXP1.pff")))
            {
                return "TS:";
            }

            return "";
        }

        private static string BuildPlayerList()
        {
            var currentPlayerList = CommonCore.instancePlayers!.PlayerList.ToList();
            // Build the PlayerList JSON string based on the current players in the server.
            var playerList = new Dictionary<string, object>
            {
                ["0"] = new
                {
                    Name = CommonCore.theInstance!.gameHostName,
                    NameBase64Encoded = Convert.ToBase64String(Encoding.GetEncoding(1252).GetBytes(CommonCore.theInstance!.gameHostName)),
                    Kills = "0",
                    Deaths = "0",
                    WeaponId = "5",
                    WeaponText = "CAR-15",
                    TeamId = "5",
                    TeamText = "Unknown"
                }
            };

            foreach (var playerRecord in currentPlayerList)
            {
                var player = playerRecord.Value;

                playerList.Add(player.PlayerSlot.ToString(), new
                {
                    Name = player.PlayerName,
                    NameBase64Encoded = player.PlayerNameBase64,
                    Kills = player.stat_Kills.ToString(),
                    Deaths = player.stat_Deaths.ToString(),
                    WeaponId = player.SelectedWeaponID.ToString(),
                    WeaponText = player.SelectedWeaponName,
                    TeamId = player.PlayerTeam.ToString(),
                    TeamText = player.PlayerTeam switch
                    {
                        0 => "None",
                        1 => "Blue",
                        2 => "Red",
                        3 => "Yellow",
                        4 => "Violet",
                        _ => "Unknown"
                    }
                });
            }

            return Convert.ToBase64String(Encoding.GetEncoding(1252).GetBytes(System.Text.Json.JsonSerializer.Serialize(playerList)));
            
        }

    }

    public record HeartBeatResponse
    {
        public int StatusCode { get; init; }
        public string ReasonPhrase { get; init; } = string.Empty;
    }
}