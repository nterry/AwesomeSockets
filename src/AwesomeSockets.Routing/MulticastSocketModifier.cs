using System;
using System.Net;
using System.Net.Sockets;
using AwesomeSockets.Model;

namespace AwesomeSockets.Routing
{
    public class MulticastSocketModifier : ISocketModifier
    {
        private const string DefaultIpAddress = "224.168.100.2";

        private readonly IPAddress _ipAddress;

        public MulticastSocketModifier() : this(DefaultIpAddress) { }

        public MulticastSocketModifier(string ipAddress) : this(IPAddress.Parse(ipAddress)) { }

        public MulticastSocketModifier(IPAddress ipAddress)
        {
            _ipAddress = ipAddress;
        }

        public ISocket Apply(ISocket socket)
        {
            if (!socket.GetNativeSocket().SocketType.Equals(SocketType.Dgram))
            {
                throw new InvalidOperationException(
                    "Cannot apply Multicast modifier to a non-datagram-type socket!");
            }

            socket.GetNativeSocket().SetSocketOption(SocketOptionLevel.Socket,
                SocketOptionName.AddMembership, new MulticastOption(_ipAddress,
                    IPAddress.Any));

            return socket;
        }

        public ISocket UnApply(ISocket socket)
        {
            throw new NotImplementedException();
        }
    }
}
