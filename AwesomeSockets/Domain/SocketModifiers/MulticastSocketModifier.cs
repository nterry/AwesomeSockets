using AwesomeSockets.Domain.Exceptions;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public class MulticastSocketModifier : AbstractSocketModifier, ISocketModifier
    {
        private string defaultMulticastAddress = "";
        private int defaultMulticastPort = 0;
        private int defaultTtl = 1;

        private static List<dynamic> conflicts = new List<dynamic> { typeof(MulticastV6SocketModifier) };

        public MulticastSocketModifier() : base(conflicts) { }

        public ISocket Apply(ISocket socket, params string[] args)
        {
            Socket actualSocket = socket.GetSocket();
            IPAddress multicastIPAddress = IPAddress.Parse((args[0] == null) ? defaultMulticastAddress : args[0]);
            int multicastPort = (args[1] == null) ? defaultMulticastPort : int.Parse(args[1]);
            int ttl = (args[2] == null) ? defaultTtl : int.Parse(args[2]);

            if (!actualSocket.CanMulticast()) throw new CannotMulticastException(string.Format(
                "IP address {0} cannot be multicasted! Please check the ip is in the 224.0.0.0/4 block and is a udp socket",
                multicastIPAddress));

            actualSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastIPAddress));
            actualSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);
            
            return socket;
        }
    }
}
