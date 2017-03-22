using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using AwesomeSockets.Model;

namespace AwesomeSockets.Core
{
    public class UdpSocket : DatagramSocket
    {
        public UdpSocket(Socket internalSocket) : base(internalSocket)
        {
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public override void SetSocketOption(IDictionary<SocketOptionName, object> opts)
        {
            throw new System.NotImplementedException();
        }

        public override Socket GetNativeSocket()
        {
            throw new System.NotImplementedException();
        }

        public override int Push(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public override int Pop(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public override void Close(int timeout = 0)
        {
            throw new System.NotImplementedException();
        }

        public override ISocket WithModifier<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
