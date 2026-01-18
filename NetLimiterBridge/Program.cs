using System.Threading.Tasks;

namespace NetLimiterBridge
{
    internal class Program
    {

	    public static async Task Main(string[] args)
	    {
		    // Create an instance of the NetLimiterBridge class
		    var bridge = new NetLimiterBridge();

		    // Connection parameters
		    string hostname = "38.54.101.108";
		    ushort port = 9298;
		    string username = "rssaka";
		    string password = "Dk75Rn43s!";

		    // Run the bridge service
		    await bridge.RunAsync(hostname, port, username, password);
	    }

    }
}