using System.Collections.Generic;

namespace NetLimiterBridge
{
	public class Command
	{
		public string Action { get; set; }
		public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

	}

	public class Response
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}

	public class ConnectionInfo
	{
		public string RemoteAddress { get; set; }
		public int RemotePort { get; set; }
		public string LocalAddress { get; set; }
		public int LocalPort { get; set; }
		public string Protocol { get; set; }
	}
}