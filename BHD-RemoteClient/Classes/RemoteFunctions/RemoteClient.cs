using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_RemoteClient.Forms;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BHD_RemoteClient.Classes.RemoteFunctions
{
    public class RemoteClient
    {
        private static theInstance theInstance => CommonCore.theInstance!;
        private static chatInstance instanceChat => CommonCore.instanceChat!;
        private static mapInstance instanceMaps => CommonCore.instanceMaps!;
        private static banInstance instanceBans => CommonCore.instanceBans!;
        private static adminInstance instanceAdmin => CommonCore.instanceAdmin!;
        private static ServerManager? ServerManagerUI => Program.ServerManagerUI;
        private static LoginWindow? LoginWindowUI => Program.LoginWindowUI;

        private readonly string _serverAddress;
        private readonly int _commPort;
        private readonly int _updatePort;
        public TcpClient? _commClient;
        private TcpClient? _updateClient;
        public SslStream? _commStream;
        private SslStream? _updateStream;
        private CancellationTokenSource? _cts;
        public string AuthToken = string.Empty;

        private const int MaxMessageSize = 10 * 1024 * 1024; // 1 MB

        public RemoteClient(string serverAddress, int commPort, int updatePort)
        {
            _serverAddress = serverAddress;
            _commPort = commPort;
            _updatePort = updatePort;
        }

        public bool Connect(int timeoutMs = 5000)
        {
            Disconnect();

            try
            {
                _cts = new CancellationTokenSource();

                // Connect communication channel with timeout
                AppDebug.Log("RemoteClient", $"Connecting to comm port {_commPort}...");
                var commClient = new TcpClient();
                var commConnectTask = commClient.ConnectAsync(_serverAddress, _commPort);
                if (!commConnectTask.Wait(timeoutMs))
                {
                    AppDebug.Log("RemoteClient", $"Timeout connecting to comm port {_commPort}.");
                    return false;
                }
                AppDebug.Log("RemoteClient", $"Connected to comm port {_commPort}.");

                _commClient = commClient;
                _commStream = new SslStream(_commClient.GetStream(), false, ValidateServerCertificate!, null);
                _commClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                var commAuthTask = _commStream.AuthenticateAsClientAsync(_serverAddress);
                if (!commAuthTask.Wait(timeoutMs))
                {
                    AppDebug.Log("RemoteClient", "Timeout during SSL authentication (comm channel).");
                    return false;
                }
                AppDebug.Log("RemoteClient", $"SSL authenticated on comm port {_commPort}.");

                // Connect update channel with timeout
                AppDebug.Log("RemoteClient", $"Connecting to update port {_updatePort}...");
                var updateClient = new TcpClient();
                var updateConnectTask = updateClient.ConnectAsync(_serverAddress, _updatePort);
                if (!updateConnectTask.Wait(timeoutMs))
                {
                    AppDebug.Log("RemoteClient", $"Timeout connecting to update port {_updatePort}.");
                    return false;
                }
                AppDebug.Log("RemoteClient", $"Connected to update port {_updatePort}.");

                _updateClient = updateClient;
                _updateStream = new SslStream(_updateClient.GetStream(), false, ValidateServerCertificate!, null);
                _updateClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                var updateAuthTask = _updateStream.AuthenticateAsClientAsync(_serverAddress);
                if (!updateAuthTask.Wait(timeoutMs))
                {
                    AppDebug.Log("RemoteClient", "Timeout during SSL authentication (update channel).");
                    return false;
                }

                // Start listening for updates
                Task.Run(() => ListenForUpdates(_cts.Token));

                return true;
            }
            catch (Exception e)
            {
                AppDebug.Log("RemoteClient", "Error connecting: " + e.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _cts?.Cancel();
                _updateStream?.Dispose();
                _commStream?.Dispose();
                _updateClient?.Close();
                _commClient?.Close();
            }
            catch (Exception e)
            {
                AppDebug.Log("RemoteClient", "Disconnecting Error: " + e.Message);
            }
        }

        // Generic write method
        public static void WriteMessage<T>(SslStream sslStream, T message)
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
                AppDebug.Log("RemoteClient", $"Error writing message: {ex.Message}");
            }
        }

        // Generic read method
        public static T? ReadMessage<T>(SslStream sslStream, int readTimeout = 5000)
        {
            try
            {
                sslStream.ReadTimeout = readTimeout;
                Span<byte> lengthBytes = stackalloc byte[4];
                int read = sslStream.Read(lengthBytes);
                if (read < 4) return default;
                int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);
                if (length <= 0 || length > MaxMessageSize)
                {
                    AppDebug.Log("RemoteClient", $"Invalid message length: {length}");
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
                AppDebug.Log("RemoteClient", $"Error reading message: {ex.Message}");
                return default;
            }
        }

        private void ListenForUpdates(CancellationToken token)
        {
            var stream = _updateStream!;
            bool authenticated = false;
            var handshakeStart = DateTime.UtcNow;

            // Authentication handshake loop with timeout
            while (!token.IsCancellationRequested && stream.CanRead && !authenticated)
            {
                try
                {
                    var packet = ReadMessage<CommandPacket>(stream, 5000);
                    if (packet != null && packet.Command == "RequestAuthToken")
                    {
                        var authPacket = new CommandPacket
                        {
                            AuthToken = AuthToken,
                            Command = "AuthToken",
                            CommandData = new
                            {
                                AuthToken = AuthToken,
                                ClientVersion = Program.ApplicationVersion
                            }
                        };
                        WriteMessage(stream, authPacket);

                        // Immediately expect a CommandResponse
                        var response = ReadMessage<CommandResponse>(stream, 5000);
                        if (response != null && response.Success)
                        {
                            authenticated = true;
                            AppDebug.Log("RemoteClient", "Authenticated for updates.");
                            break;
                        }
                        else
                        {
                            AppDebug.Log("RemoteClient", $"Auth failed: {response?.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("RemoteClient", "Error during authentication handshake: " + ex.Message);
                    break;
                }

                if ((DateTime.UtcNow - handshakeStart).TotalSeconds > 60)
                {
                    AppDebug.Log("RemoteClient", "Update Token Handshake timeout.");
                    KillServerManagerUI();
                    break;
                }
            }

            // Update receiving loop
            while (!token.IsCancellationRequested && stream.CanRead && authenticated)
            {
                try
                {
                    var updatePacket = ReadMessage<CommandPacket>(stream, 5000);
                    if (updatePacket != null && updatePacket.Command == "UpdateData")
                    {
                        ProcessUpdatePacket(updatePacket);
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("RemoteClient", "Error in update loop: " + ex.Message);
                    KillServerManagerUI();
                    break;
                }
            }
        }

        public CommandResponse? SendCommandAndGetResponse(CommandPacket packet, int timeoutMs = 5000)
        {
            if (_commStream == null)
                return null;

            WriteMessage(_commStream, packet);
            return ReadMessage<CommandResponse>(_commStream, timeoutMs);
        }

        private static void ProcessUpdatePacket(CommandPacket thePacket)
        {
            try
            {
                // Decode Base64
                string base64 = thePacket.CommandData is JsonElement je ? je.GetString()! : (string)thePacket.CommandData;
                byte[] compressedBytes = Convert.FromBase64String(base64);

                // Decompress GZip
                using var memoryStream = new MemoryStream(compressedBytes);
                using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Decompress);
                using var decompressedStream = new MemoryStream();
                gzipStream.CopyTo(decompressedStream);

                // Deserialize the decompressed JSON
                decompressedStream.Position = 0;
                string json = System.Text.Encoding.UTF8.GetString(decompressedStream.ToArray());
                InstanceUpdatePacket updateData = JsonSerializer.Deserialize<InstanceUpdatePacket>(json)!;

                // Process the update data
                CmdUpdateGameClient.ProcessUpdateData(updateData);
                Debug.WriteLine("Processing Complete. Server Status: " + theInstance.instanceStatus);
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteClient", $"Error processing update packet: {ex.Message}");
            }
        }

        private static bool ValidateServerCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // For demo purposes, accept any certificate. In production, validate properly!
            return true;
        }

        public void KillServerManagerUI()
        {
            try
            {
                Disconnect();
                ServerManagerUI?.Invoke((MethodInvoker)delegate
                {
                    ServerManagerUI.Close();
                });
                LoginWindowUI?.Invoke((MethodInvoker)delegate
                {
                    LoginWindowUI.ServerManagerUI_FormClosed(null, null!);
                });
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteClient", "Error closing ServerManagerUI: " + ex.Message);
            }
        }


    }
}