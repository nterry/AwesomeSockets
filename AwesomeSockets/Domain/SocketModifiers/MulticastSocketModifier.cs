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
    public class MulticastSocketModifier : ISocketModifier
    {   
        public ISocket Apply(ISocket socket, params string[] args)
        {
            IPAddress multicastIPAddress = IPAddress.Parse(args[0]);
            int multicastPort = int.Parse(args[1]);
            //TODO: Need to check if socket is UDP as Multicast only works with UDP...
            //TODO: Additionally, the invocation of the AweSock static class method breaks encapsulation. Need an internal method...
            AweSock.SetSockOpt(socket, new Dictionary<SocketOptionName, object>
                {
                    { SocketOptionName.AddMembership, new MulticastOption(multicastIPAddress) }
                });
            return socket;
        }
    }
}
