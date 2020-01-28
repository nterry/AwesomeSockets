using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public class MulticastSocketModifier : ISocketModifier
    {
        //These are RFC included values
        private readonly IPAddress _multicastIpAddress = IPAddress.Parse("224.168.100.2");
        private readonly int _multicastPort = 11000;
        
        public ISocket Apply(ISocket socket)
        {
            //TODO: Need to check if socket is UDP as Multicast only works with UDP...
            AweSock.SetSockOpt(socket, new Dictionary<SocketOptionName, object>
            {
                { SocketOptionName.AddMembership, new MulticastOption(_multicastIpAddress, IPAddress.Parse("127.0.0.1")) }
            });
            return socket;
        }
    }
}
