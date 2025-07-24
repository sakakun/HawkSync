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

        public void SendCommandPacket(SslStream sslStream, CommandPacket packet)
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
                AppDebug.Log("RemoteClient", "Error sending command packet: " + ex.Message);
            }
        }

        public CommandResponse? ReceiveCommandResponse(SslStream sslStream, int timeoutMs = 5000)
        {
            try
            {
                sslStream.ReadTimeout = timeoutMs;
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
                return JsonSerializer.Deserialize<CommandResponse>(buffer);
            }
            catch (Exception ex)
            {
                AppDebug.Log("RemoteClient", "Error receiving command response: " + ex.Message);
                return null;
            }
        }

        private void ListenForUpdates(CancellationToken token)
        {
            var stream = _updateStream!;
            bool authenticated = false;
            var handshakeStart = DateTime.UtcNow;

            // Allocate buffer once outside the loop
            byte[] lengthBytes = new byte[4];

            // Authentication handshake loop with timeout
            while (!token.IsCancellationRequested && stream.CanRead && !authenticated)
            {
                try
                {
                    stream.ReadTimeout = 5000; // 5 seconds
                    Array.Clear(lengthBytes, 0, lengthBytes.Length);
                    int read = stream.Read(lengthBytes, 0, lengthBytes.Length);
                    if (read < 4) break;

                    int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);
                    byte[] buffer = new byte[length];
                    int offset = 0;
                    while (offset < length)
                    {
                        int bytesRead = stream.Read(buffer, offset, length - offset);
                        if (bytesRead == 0) return;
                        offset += bytesRead;
                    }

                    using var doc = JsonDocument.Parse(buffer);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("Command", out var commandProp))
                    {
                        var packet = JsonSerializer.Deserialize<CommandPacket>(buffer);
                        if (packet != null && packet.Command == "RequestAuthToken")
                        {
                            var authPacket = new CommandPacket
                            {
                                AuthToken = AuthToken,
                                Command = "AuthToken",
                                CommandData = AuthToken
                            };
                            SendCommandPacket(stream, authPacket);
                        }
                    }
                    else if (root.TryGetProperty("Success", out var successProp))
                    {
                        var response = JsonSerializer.Deserialize<CommandResponse>(buffer);
                        if (response != null && response.Success)
                        {
                            authenticated = true;
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

                if ((DateTime.UtcNow - handshakeStart).TotalSeconds > 10)
                {
                    AppDebug.Log("RemoteClient", "Handshake timeout.");
                    break;
                }
            }

            // Update receiving loop
            while (!token.IsCancellationRequested && stream.CanRead && authenticated)
            {
                try
                {
                    stream.ReadTimeout = 10000; // 10 seconds
                    Array.Clear(lengthBytes, 0, lengthBytes.Length);
                    int read = stream.Read(lengthBytes, 0, lengthBytes.Length);
                    if (read < 4)
                    {
                        AppDebug.Log("RemoteClient", "Stream closed or insufficient data, breaking update loop.");
                        break;
                    }

                    int length = BinaryPrimitives.ReadInt32LittleEndian(lengthBytes);
                    byte[] buffer = new byte[length];
                    int offset = 0;
                    while (offset < length)
                    {
                        int bytesRead = stream.Read(buffer, offset, length - offset);
                        if (bytesRead == 0)
                        {
                            AppDebug.Log("RemoteClient", "No bytes read, returning from update loop.");
                            return;
                        }
                        offset += bytesRead;
                    }

                    using var doc = JsonDocument.Parse(buffer);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("Command", out var commandProp))
                    {
                        var updatePacket = JsonSerializer.Deserialize<CommandPacket>(buffer);
                        if (updatePacket != null && updatePacket.Command == "UpdateData")
                        {
                            ProcessUpdatePacket(updatePacket!);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Log("RemoteClient", "Error in update loop: " + ex.Message);
                    break;
                }
            }
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