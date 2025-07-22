using BHD_SharedResources.Classes.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_RemoteClient.Classes.RemoteFunctions
{
    public class CommandPacket
    {
        public string AuthToken { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public object CommandData { get; set; } = null!;
    }
    public class CommandResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object? ResponseData { get; set; } = null;
    }

    public class AuthenticationPacket
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class InstanceUpdatePacket
    {
        public required theInstance theInstance { get; set; }
        public required mapInstance mapInstance { get; set; }
        public required banInstance banInstance { get; set; }
        public required chatInstance chatInstance { get; set; }
        public required statInstance statInstance { get; set; }
        public required adminInstance adminInstance { get; set; }
    }

}
