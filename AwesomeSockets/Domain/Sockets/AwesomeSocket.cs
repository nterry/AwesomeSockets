using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace AwesomeSockets.Domain.Sockets
{
    public class AwesomeSocket : ISocket
    {
        internal readonly Socket InternalSocket;

        internal AwesomeSocket(AddressFamily addressFamily = AddressFamily.InterNetwork, SocketType socketType = SocketType.Stream)
        {
            InternalSocket = new Socket(addressFamily, socketType, ProtocolType.IP);
            InternalSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        internal AwesomeSocket(Socket socket)
        {
            InternalSocket = socket;
        }

        internal static AwesomeSocket New(SockType sockType = SockType.Tcp)
        {
            switch (sockType)
            {
                case SockType.Udp:
                    return new AwesomeSocket(AddressFamily.InterNetwork, SocketType.Dgram);
                default:
                    return new AwesomeSocket();
            }
        }

        internal static AwesomeSocket New(Socket socket)
        {
            return new AwesomeSocket(socket);
        }

        public Socket GetInternalSocket()
        {
            return InternalSocket;
        }

        public void SetGlobalConfiguration(Dictionary<SocketOptionName, object> opts)
        {
            foreach (var opt in opts)
            {
                InternalSocket.SetSocketOption(SocketOptionLevel.Socket, opt.Key, opt.Value);
            }
        }
    }
}
