using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using AwesomeSockets.Model;

namespace AwesomeSockets.Core
{
    public abstract class AwesomeSocket : ISocket
    {
        protected Socket _internalSocket;

        protected AwesomeSocket(Socket internalSocket)
        {
            _internalSocket = internalSocket;
        }

        public abstract void Dispose();
        public abstract void SetSocketOption(IDictionary<SocketOptionName, object> opts);
        public abstract Socket GetNativeSocket();
        public abstract int Push(Stream stream);
        public abstract int Pop(Stream stream);
        public abstract void Close(int timeout = 0);
        public abstract ISocket WithModifier<T>() where T : ISocketModifier, new();
    }
}
