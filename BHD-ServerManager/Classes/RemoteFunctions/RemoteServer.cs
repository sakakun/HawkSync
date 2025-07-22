using BHD_ServerManager.Classes.StatsManagement;
using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.SupportClasses;
using BHD_SharedResources.Classes.Instances;

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

        private static readonly List<AuthorizedClient> _authorizedClients = new List<AuthorizedClient>();
        private static TcpListener? _commListener;
        private static TcpListener? _updateListener;
        private static bool _isRunning;
        private static Thread? _serverThread;
        private static X509Certificate? _serverCertificate;
        public static bool IsRunning => _isRunning;

        // Read-only access if needed
        public static IReadOnlyList<AuthorizedClient> AuthorizedClients => _authorizedClients.AsReadOnly();

        // Add or update an authorized client
        public static void AddOrUpdateAuthorizedClient(AuthorizedClient client)
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

        // Get authorized client by username
        public static AuthorizedClient? GetAuthorizedClient(string username)
        {
            return _authorizedClients.Find(c => c.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public static void Start(int commPort, int updatePort)
        {
            if (_isRunning) return;

            string certPath = Path.Combine(CommonCore.AppDataPath, "ServerCert.pfx");
            string passPath = Path.Combine(CommonCore.AppDataPath, "ServerCertPass.txt");
            string certPassword;

            // Read password from file if exists, else use default
            if (File.Exists(passPath))
            {
                certPassword = File.ReadAllText(passPath).Trim();
            }
            else
            {
                certPassword = "Babstat4Sucks";
            }

            // Create certificate if not found
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

            while (_isRunning)
            {
                if (_commListener.Pending())
                {
                    var commClient = _commListener.AcceptTcpClient();
                    Task.Run(() => HandleCommClient(commClient));
                }

                if (_updateListener.Pending())
                {
                    var updateClient = _updateListener.AcceptTcpClient();
                    Task.Run(() => HandleUpdateClient(updateClient));
                }

                Thread.Sleep(10);
            }
        }

        private static void HandleCommClient(TcpClient client)
        {
            AppDebug.Log("RemoteServer", "New communication client connected.");
            using (var sslStream = new SslStream(client.GetStream(), false))
            {
                sslStream.AuthenticateAsServer(_serverCertificate!, false, false);

                var processor = new CommandProcessor();
                while (client.Connected)
                {
                    // Read packet (implement your own protocol framing)
                    CommandPacket? packet = ReadPacket(sslStream);
                    if (packet == null) break;

                    CommandResponse response = processor.ProcessCommand(packet);
                    WriteResponse(sslStream, response);
                }
            }
            client.Close();
        }

        private static void HandleUpdateClient(TcpClient client)
        {
            AppDebug.Log("RemoteServer", "New update client connected.");
            using (var sslStream = new SslStream(client.GetStream(), false))
            {
                sslStream.AuthenticateAsServer(_serverCertificate!, false, false);

                AuthorizedClient? authorizedClient = null;
                int attempts = 0;

                // Auth handshake loop
                while (_isRunning && client.Connected && authorizedClient == null && attempts < 10)
                {
                    AppDebug.Log("RemoteServer", "Update Attempt: " + attempts);
                    // Step 1: Ask for AuthToken
                    var requestPacket = new CommandPacket
                    {
                        Command = "RequestAuthToken",
                        CommandData = "Please provide your AuthToken."
                    };
                    WriteCommandPacket(sslStream, requestPacket);

                    // Step 2: Receive AuthToken from client
                    CommandPacket? responsePacket = ReadCommandPacket(sslStream);
                    AppDebug.Log("RemoteServer", "Received AuthToken: " + responsePacket?.AuthToken);
                    if (responsePacket == null || string.IsNullOrWhiteSpace(responsePacket.AuthToken))
                    {
                        WriteResponse(sslStream, new CommandResponse
                        {
                            Success = false,
                            Message = "No AuthToken provided."
                        });
                        attempts++;
                        Thread.Sleep(500); // Wait before retry
                        continue;
                    }

                    // Step 3: Validate AuthToken
                    authorizedClient = _authorizedClients
                        .Find(c => c.AuthorizationToken.Equals(responsePacket.AuthToken, StringComparison.OrdinalIgnoreCase));
                    
                    AppDebug.Log("RemoteServer", "Authenticated: " + (authorizedClient != null));
                    if (authorizedClient == null)
                    {
                        WriteResponse(sslStream, new CommandResponse
                        {
                            Success = false,
                            Message = "Invalid AuthToken."
                        });
                        attempts++;
                        Thread.Sleep(500); // Wait before retry
                    }
                }

                if (authorizedClient == null)
                {
                    WriteResponse(sslStream, new CommandResponse
                    {
                        Success = false,
                        Message = "Authentication failed. Closing connection."
                    });
                    return;
                }

                // Step 4: Send constant updates
                WriteResponse(sslStream, new CommandResponse
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
                    AppDebug.Log("RemoteServer", "Sending update packet...");
                    WriteCommandPacket(sslStream, updatePacket);
                    Thread.Sleep(1000); // Send updates every second
                }
            }
            client.Close();
        }

        // Helper methods for CommandPacket
        private static void WriteCommandPacket(SslStream sslStream, CommandPacket packet)
        {

            try
            {
                byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(packet);
                Span<byte> lengthBytes = stackalloc byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(lengthBytes, jsonBytes.Length);
                sslStream.Write(lengthBytes);
                sslStream.Write(jsonBytes);
                sslStream.Flush();
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Error writing command packet: " + ex.Message);
            }
        }

        private static CommandPacket? ReadCommandPacket(SslStream sslStream)
        {

            try
            {
                Span<byte> lengthBytes = stackalloc byte[4];
                int read = sslStream.Read(lengthBytes);
                if (read < 4) return null;
                int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);

                byte[] buffer = new byte[length];
                int offset = 0;
                while (offset < length)
                {
                    int bytesRead = sslStream.Read(buffer, offset, length - offset);
                    if (bytesRead == 0) return null;
                    offset += bytesRead;
                }

                return JsonSerializer.Deserialize<CommandPacket>(buffer);
            } catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Error reading command packet: " + ex.Message);
                return null;
            }

        }

        private static object GetUpdateDataForClient()
        {
            InstanceUpdatePacket UpdateData = new InstanceUpdatePacket
            {
                theInstance = theInstance,
                mapInstance = instanceMaps,
                banInstance = instanceBans,
                chatInstance = instanceChat,
                statInstance = instanceStats,
                adminInstance = instanceAdmin
            };

            string json = JsonSerializer.Serialize(UpdateData);
            string dataEncoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
            return dataEncoded;
        }

        private static CommandPacket? ReadPacket(SslStream sslStream)
        {
            try
            {
                // Read 4 bytes for length prefix
                Span<byte> lengthBytes = stackalloc byte[4];
                int read = sslStream.Read(lengthBytes);
                if (read < 4) return null; // Connection closed or protocol error

                int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);

                // Read the JSON payload
                byte[] buffer = new byte[length];
                int offset = 0;
                while (offset < length)
                {
                    int bytesRead = sslStream.Read(buffer, offset, length - offset);
                    if (bytesRead == 0) return null; // Connection closed
                    offset += bytesRead;
                }

                // Deserialize to CommandPacket
                return JsonSerializer.Deserialize<CommandPacket>(buffer);
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Error reading packet: " + ex.Message);
                return null; // Handle error appropriately
            }
        }

        private static void WriteResponse(SslStream sslStream, CommandResponse response)
        {
            try
            {
                // Serialize response to JSON
                byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(response);

                // Write length prefix
                Span<byte> lengthBytes = stackalloc byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(lengthBytes, jsonBytes.Length);
                sslStream.Write(lengthBytes);

                // Write JSON payload
                sslStream.Write(jsonBytes);
                sslStream.Flush();
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteServer", "Error writing response: " + ex.Message);

            }
        }

    }

    public class AuthorizedClient
    { 
        public int ClientId                 { get; set; }
        public Stream? ClientStream         { get; set; }
        public Stream? UpdateStream         { get; set; }
        public required AdminAccount User                   { get; set; }
        public string AuthorizationToken    { get; set; } = string.Empty;
        public DateTime AuthorizationTime   { get; set; } = DateTime.MinValue;
    }

}
