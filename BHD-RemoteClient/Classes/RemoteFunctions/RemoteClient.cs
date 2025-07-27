using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
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

        private readonly string _serverAddress;
        private readonly int _commPort;
        private readonly int _updatePort;
        public TcpClient? _commClient;
        private TcpClient? _updateClient;
        public SslStream? _commStream;
        private SslStream? _updateStream;
        private CancellationTokenSource? _cts;
        public string AuthToken = string.Empty;

        private const int MaxMessageSize = 1024 * 1024; // 1 MB

        public RemoteClient(string serverAddress, int commPort, int updatePort)
        {
            _serverAddress = serverAddress;
            _commPort = commPort;
            _updatePort = updatePort;
            Connect();
        }

        public void Connect()
        {
            try
            {
                Disconnect();
            }
            catch { }

            try
            {
                _cts = new CancellationTokenSource();

                // Connect communication channel
                _commClient = new TcpClient(_serverAddress, _commPort);
                _commStream = new SslStream(_commClient.GetStream(), false, ValidateServerCertificate!, null);
                _commStream.AuthenticateAsClient(_serverAddress);

                // Connect update channel
                _updateClient = new TcpClient(_serverAddress, _updatePort);
                _updateStream = new SslStream(_updateClient.GetStream(), false, ValidateServerCertificate!, null);
                _updateStream.AuthenticateAsClient(_serverAddress);

                // Start listening for updates
                Task.Run(() => ListenForUpdates(_cts.Token));
            }
            catch (Exception e)
            {
                AppDebug.Log("RemoteClient", "Error connecting: " + e.Message);
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
            catch { }
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
                            CommandData = AuthToken
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
            string base64 = thePacket.CommandData is JsonElement je ? je.GetString()! : (string)thePacket.CommandData;
            string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            InstanceUpdatePacket updateData = JsonSerializer.Deserialize<InstanceUpdatePacket>(json)!;
            CmdUpdateGameClient.ProcessUpdateData(updateData);
            Debug.WriteLine("Processing Complete.  Server Status: " + theInstance.instanceStatus);
        }

        private static bool ValidateServerCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // For demo purposes, accept any certificate. In production, validate properly!
            return true;
        }
    }
}