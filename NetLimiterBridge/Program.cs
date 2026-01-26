using System.Threading.Tasks;

namespace NetLimiterBridge
{
    internal class Program
    {

	    public static async Task Main(string[] args)
	    {
            // Create an instance of the NetLimiterBridge class
            var bridge = new NetLimiterBridge();

            // Default values for localhost connection
            string hostname = "localhost";
            ushort port = 9098;
            string username = "";
            string password = "";

            // Parse arguments if provided
            if (args.Length >= 1)
            {
                hostname = args[0];
            }
            if (args.Length >= 2)
            {
                port = ushort.Parse(args[1]);
            }
            if (args.Length >= 3)
            {
                username = args[2];
            }
            if (args.Length >= 4)
            {
                password = args[3];
            }

            // Run the bridge service
            await bridge.RunAsync(hostname, port, username, password);
	    }

    }
}