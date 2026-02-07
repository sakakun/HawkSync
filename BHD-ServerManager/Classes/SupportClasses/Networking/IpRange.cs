using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Windows.Graphics.Display;

namespace BHD_ServerManager.Classes.SupportClasses.Networking
{
        public class IpRange
        {
            public IPAddress Start { get; }
            public IPAddress End { get; }

            public IpRange(IPAddress start, IPAddress end)
            {
                Start = start;
                End = end;
            }

            public static IpRange FromCidr(IPAddress baseAddress, int subnetMask)
            {
                uint ip = IpToUint(baseAddress);
                uint mask = subnetMask == 0 ? 0 : 0xFFFFFFFF << (32 - subnetMask);
                uint start = ip & mask;
                uint end = start | ~mask;
                return new IpRange(UintToIp(start), UintToIp(end));
            }

            public List<IpRange> Subtract(IpRange other)
            {
                var result = new List<IpRange>();
                uint s1 = IpToUint(Start), e1 = IpToUint(End);
                uint s2 = IpToUint(other.Start), e2 = IpToUint(other.End);

                if (e2 < s1 || s2 > e1)
                {
                    // No overlap
                    result.Add(this);
                    return result;
                }
                if (s2 > s1)
                    result.Add(new IpRange(UintToIp(s1), UintToIp(s2 - 1)));
                if (e2 < e1)
                    result.Add(new IpRange(UintToIp(e2 + 1), UintToIp(e1)));
                return result;
            }

            public static uint IpToUint(IPAddress ip)
            {
                var bytes = ip.GetAddressBytes();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return BitConverter.ToUInt32(bytes, 0);
            }

            public static IPAddress UintToIp(uint ip)
            {
                var bytes = BitConverter.GetBytes(ip);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return new IPAddress(bytes);
            }

            public override string ToString()
            {
                return Start.Equals(End) ? Start.ToString() : $"{Start}-{End}";
            }
        }
}
