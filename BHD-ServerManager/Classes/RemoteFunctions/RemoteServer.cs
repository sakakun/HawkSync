using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Buffers.Binary;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace BHD_ServerManager.Classes.RemoteFunctions
{
    public static class RemoteServer
    {
        private static theInstance theInstance = CommonCore.theInstance!;
        private static mapInstance instanceMaps = CommonCore.instanceMaps!;
        private static banInstance instanceBans = CommonCore.instanceBans!;
        private static chatInstance instanceChat = CommonCore.instanceChat!;
        private static statInstance instanceStats = CommonCore.instanceStats!;
        private static adminInstance instanceAdmin = CommonCore.instanceAdmin!;

        private static readonly List<AuthorizedClient> _authorizedClients = new();
        private static TcpListener? _commListener;
        private static TcpListener? _updateListener;
        private static bool _isRunning;
        private static Thread? _serverThread;
        private static X509Certificate? _serverCertificate;
        public static bool IsRunning => _isRunning;

        private const int MaxMessageSize = 10 * 1024 * 1024; // 1 MB

        public static IReadOnlyList<AuthorizedClient> AuthorizedClients => _authorizedClients.AsReadOnly();

        public static void AddOrUpdateAuthorizedClient(AuthorizedClient client)
        {
            lock (_authorizedClients)
            {
                var existing = _authorizedClients.Find(c => c.User.Username.Equals(client.User.Username, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    existing.AuthorizationToken = client.AuthorizationToken;
                    existing.AuthorizationTime = client.AuthorizationTime;
                    existing.ClientId = client.ClientId;
                    existing.ClientStream = client.ClientStream;
                    existing.UpdateStream = client.UpdateStream;
                }
                else
                {
                    _authorizedClients.Add(client);
                }
            }
        }

        public static AuthorizedClient? GetAuthorizedClient(string username)
        {
            lock (_authorizedClients)
            {
                return _authorizedClients.Find(c => c.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static void Start(int commPort, int updatePort)
        {
            if (_isRunning) return;

            string certPath = Path.Combine(CommonCore.AppDataPath, "ServerCert.pfx");
            string passPath = Path.Combine(CommonCore.AppDataPath, "ServerCertPass.txt");
            string certPassword = File.Exists(passPath) ? File.ReadAllText(passPath).Trim() : "Babstat4Sucks";

            if (!File.Exists(certPath))
            {
                var distinguishedName = new X500DistinguishedName("CN=RemoteServer");
                using var rsa = RSA.Create(2048);
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                var cert = request.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(5));
                var pfxBytes = cert.Export(X509ContentType.Pfx, certPassword);
                File.WriteAllBytes(certPath, pfxBytes);
            }

            _serverCertificate = new X509Certificate2(certPath, certPassword);
            _commListener = new TcpListener(IPAddress.Any, commPort);
            _updateListener = new TcpListener(IPAddress.Any, updatePort);

            _isRunning = true;
            _serverThread = new Thread(RunServer) { IsBackground = true };
            _serverThread.Start();
        }

        public static bool IsPortAvailable(int port)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public static void Stop()
        {
            _isRunning = false;
            _commListener?.Stop();
            _updateListener?.Stop();
            _serverThread?.Join();
            AppDebug.Log("RemoteServer", "Server Stopped...");
        }

        private static void RunServer()
        {
            _commListener!.Start();
            _updateListener!.Start();
            
            AppDebug.Log("RemoteServer", "Server Started...");

            DateTime lastCleanup = DateTime.UtcNow;

            while (_isRunning)
            {
                try
                {
                    if (_commListener.Pending())
                    {
                        var commClient = _commListener.AcceptTcpClient();
                        AppDebug.Log("RemoteServer", $"Client connected on comm port {((IPEndPoint)commClient.Client.LocalEndPoint!).Port}");
                        commClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        Task.Run(() => HandleCommClient(commClient));
                    }

                    if (_updateListener.Pending())
                    {
                        var updateClient = _updateListener.AcceptTcpClient();
                        AppDebug.Log("RemoteServer", $"Client connected on update port {((IPEndPoint)updateClient.Client.LocalEndPoint!).Port}");
                        updateClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        Task.Run(() => HandleUpdateClient(updateClient));
                    }

                    // Cleanup every 10 seconds
                    if ((DateTime.UtcNow - lastCleanup).TotalSeconds > 10)
                    {
                        CleanupStaleAuthorizedClients();
                        lastCleanup = DateTime.UtcNow;
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("RemoteServer", "Error in RunServer: " + ex.Message);
                }
                Thread.Sleep(10);
            }
        }

        private static void HandleCommClient(TcpClient client)
        {
            try
            {
                using var sslStream = new SslStream(client.GetStream(), false);
                sslStream.AuthenticateAsServer(_serverCertificate!, false, false);

                var processor = new CommandProcessor();
                while (client.Connected)
                {
                    var packet = ReadMessage<CommandPacket>(sslStream);
                    if (packet == null) break;

                    var response = processor.ProcessCommand(packet);
                    WriteMessage(sslStream, response);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Exception in HandleCommClient: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private static void HandleUpdateClient(TcpClient client)
        {
            AuthorizedClient? authorizedClient = null;
            try
            {
                using var sslStream = new SslStream(client.GetStream(), false);
                sslStream.AuthenticateAsServer(_serverCertificate!, false, false);

                int attempts = 0;
                var handshakeStart = DateTime.UtcNow;

                // Auth handshake loop with timeout
                while (_isRunning && client.Connected && authorizedClient == null && attempts < 10)
                {
                    if ((DateTime.UtcNow - handshakeStart).TotalSeconds > 10)
                    {
                        AppDebug.Log("RemoteServer", "Handshake timeout.");
                        break;
                    }
                    var requestPacket = new CommandPacket
                    {
                        Command = "RequestAuthToken",
                        CommandData = "Please provide your AuthToken."
                    };
                    WriteMessage(sslStream, requestPacket);

                    var responsePacket = ReadMessage<CommandPacket>(sslStream, 5000);
                    if (responsePacket == null || string.IsNullOrWhiteSpace(responsePacket.AuthToken))
                    {
                        WriteMessage(sslStream, new CommandResponse
                        {
                            Success = false,
                            Message = "No AuthToken provided."
                        });
                        attempts++;
                        Thread.Sleep(500);
                        continue;
                    }

                    authorizedClient = _authorizedClients
                        .Find(c => c.AuthorizationToken.Equals(responsePacket.AuthToken, StringComparison.OrdinalIgnoreCase));

                    if (authorizedClient == null)
                    {
                        WriteMessage(sslStream, new CommandResponse
                        {
                            Success = false,
                            Message = "Invalid AuthToken."
                        });
                        attempts++;
                        Thread.Sleep(500);
                    }
                }

                if (authorizedClient == null)
                {
                    WriteMessage(sslStream, new CommandResponse
                    {
                        Success = false,
                        Message = "Authentication failed. Closing connection."
                    });
                    return;
                }

                // After successful authentication
                authorizedClient.User.IsOnline = true;
                authorizedClient.User.LastSeen = DateTime.UtcNow;

                WriteMessage(sslStream, new CommandResponse
                {
                    Success = true,
                    Message = "Authenticated. Receiving updates."
                });

                while (_isRunning && client.Connected)
                {
                    var updatePacket = new CommandPacket
                    {
                        AuthToken = authorizedClient.AuthorizationToken,
                        Command = "UpdateData",
                        CommandData = GetUpdateDataForClient()
                    };
                    WriteMessage(sslStream, updatePacket);

                    // Inside the update loop, on each update/command
                    authorizedClient.User.LastSeen = DateTime.UtcNow;

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Exception in HandleUpdateClient: " + ex.Message);
            }
            finally
            {
                client.Close();

                // In finally block
                if (authorizedClient != null)
                {
                    authorizedClient.User.IsOnline = false;
                }
            }
        }

        // Condensed generic read method
        private static T? ReadMessage<T>(SslStream sslStream, int readTimeout = Timeout.Infinite)
        {
            try
            {
                if (readTimeout != Timeout.Infinite)
                    sslStream.ReadTimeout = readTimeout;

                Span<byte> lengthBytes = stackalloc byte[4];
                int read = sslStream.Read(lengthBytes);
                if (read < 4) return default;
                int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);
                if (length <= 0 || length > MaxMessageSize)
                {
                    AppDebug.Log("RemoteServer", $"Invalid message length: {length}");
                    return default;
                }

                byte[] buffer = new byte[length];
                int offset = 0;
                while (offset < length)
                {
                    int bytesRead = sslStream.Read(buffer, offset, length - offset);
                    if (bytesRead == 0) return default;
                    offset += bytesRead;
                }

                return JsonSerializer.Deserialize<T>(buffer);
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", $"Error reading message: {ex.Message}");
                return default;
            }
        }

        // Condensed generic write method
        private static void WriteMessage<T>(SslStream sslStream, T message)
        {
            try
            {
                byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message!);
                if (jsonBytes.Length > MaxMessageSize)
                    throw new InvalidOperationException("Message too large to send.");

                Span<byte> lengthBytes = stackalloc byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(lengthBytes, jsonBytes.Length);
                sslStream.Write(lengthBytes);
                sslStream.Write(jsonBytes);
                sslStream.Flush();
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", $"Error writing message: {ex.Message}");
            }
        }

        private static object GetUpdateDataForClient()
        {
            var updateData = new InstanceUpdatePacket
            {
                theInstance = theInstance,
                mapInstance = instanceMaps,
                banInstance = instanceBans,
                chatInstance = instanceChat,
                statInstance = instanceStats,
                adminInstance = instanceAdmin
            };

            string json = JsonSerializer.Serialize(updateData);
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

            // Compress the data
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionLevel.Optimal))
            {
                gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
            }
            byte[] compressedBytes = memoryStream.ToArray();

            string base64 = Convert.ToBase64String(compressedBytes);
            AppDebug.Log("RemoteServer", $"Compressed message size: {compressedBytes.Length} bytes, Base64 size: {base64.Length} bytes");

            return base64;
        }

        private static void CleanupStaleAuthorizedClients()
        {
            lock (_authorizedClients)
            {
                var now = DateTime.UtcNow;
                // Find all stale clients
                var staleClients = _authorizedClients
                    .Where(c => (now - c.User.LastSeen).TotalSeconds > 60)
                    .ToList();

                // Set IsOnline to false for each stale client
                foreach (var client in staleClients)
                {
                    client.User.IsOnline = false;
                }

                // Remove them from the authorized clients list
                _authorizedClients.RemoveAll(c => staleClients.Contains(c));
            }
        }

    }

    public class AuthorizedClient
    {
        public int ClientId { get; set; }
        public Stream? ClientStream { get; set; }
        public Stream? UpdateStream { get; set; }
        public required AdminAccount User { get; set; }
        public string AuthorizationToken { get; set; } = string.Empty;
        public DateTime AuthorizationTime { get; set; } = DateTime.MinValue;
    }
}