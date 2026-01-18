using System.Threading.Tasks;

namespace NetLimiterBridge
{
    internal class Program
    {

	    public static async Task Main(string[] args)
	    {
		    // Create an instance of the NetLimiterBridge class
		    var bridge = new NetLimiterBridge();

		    string hostname = args[1];
		    ushort port = ushort.Parse(args[2]);
		    string username = args[3];
		    string password = args[4];

		    // Run the bridge service
		    await bridge.RunAsync(hostname, port, username, password);
	    }

    }
}