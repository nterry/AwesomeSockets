using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AwesomeSockets.Domain.Sockets
{
    public class AwesomeSocket : ISocket
    {
        private readonly Socket internalSocket;

        private AwesomeSocket(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
        {
            internalSocket = new Socket(addressFamily, socketType, ProtocolType.IP);
        }

        public static AwesomeSocket New(SockType sockType = SockType.Tcp)
        {
            switch (sockType)
            {
                case SockType.Udp:
                    return new AwesomeSocket(AddressFamily.InterNetwork, SocketType.Dgram);
                default:
                    return new AwesomeSocket();
            }
        }
    }
}
