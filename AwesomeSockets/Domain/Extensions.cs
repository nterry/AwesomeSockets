using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AwesomeSockets.Domain
{
    internal static class Extensions
    {
        private static IPAddress lowerMulticastAddress = IPAddress.Parse("224.0.0.0");
        private static IPAddress upperMulticastAddress = IPAddress.Parse("239.255.255.255");

        internal static bool CanMulticast(this Socket socket)
        {
            //IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;

            return ((socket.AddressFamily == AddressFamily.InterNetwork || socket.AddressFamily == AddressFamily.InterNetworkV6) &&
            (socket.ProtocolType == ProtocolType.Udp) &&
            (socket.SocketType == SocketType.Dgram));// &&
            //(IPAddressRange.CheckRange(lowerMulticastAddress, upperMulticastAddress, endpoint.Address)));
        }
    }
}
