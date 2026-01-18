using NetLimiter.Service;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace NetLimiterBridge
{
    public class MainClient
    {
        private static string _hostname = "38.54.101.108";
        private static ushort _port = 9298;
        private static string _username = "rssaka";
        private static string _password = "Dk75Rn43s!";
        private static string _processName = "dfbhd.exe";

        public async Task RunAsync()
        {
            SecureString securePassword = CreateSecurePassword(_password);

            using (NLClient client = new NLClient())
            {
                string host = _hostname;
                if (host == "localhost" || host == "127.0.0.1")
                {
                    try
                    {
                        client.Connect();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to connect to localhost: {ex.Message}");
                        return;
                    }
                }
                else
                {
                    if (!await TryConnectAsync(client, host, _port, _username, securePassword))
                    {
                        Console.WriteLine("Failed to connect after multiple attempts.");
                        return;
                    }
                }

                while (true)
                {


                    NodeLoader nodeLoader = client.CreateNodeLoader();
                    nodeLoader.SelectAllNodes();
                    nodeLoader.Load();

                    var cnnLogEvents = nodeLoader.Connections.Nodes
                        .Where(node => node.Parent?.Parent?.AppId?.Path != null &&
                                       node.Parent.Parent.AppId.Path.EndsWith(_processName, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (ConnectionNode logEvent in cnnLogEvents)
                    {
                        string ipString = logEvent.RemoteAddress.ToIPAddress4().ToString();
                        Console.WriteLine(ipString);
                    }

                }
            }
        }

        private static SecureString CreateSecurePassword(string password)
        {
            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            return securePassword;
        }

        private static async Task<bool> TryConnectAsync(NLClient client, string host, ushort port, string username, SecureString password)
        {
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    await Task.Run(() => client.Connect(host, port, username, password));
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection attempt {attempt} failed: {ex.Message}");
                    if (attempt == 3) return false;
                }
            }
            return false;
        }

    }
}