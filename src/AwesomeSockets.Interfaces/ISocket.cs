using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace AwesomeSockets.Model
{
    public interface ISocket : IDisposable
    {
        void SetSocketOption(IDictionary<SocketOptionName, object> opts);

        Socket GetNativeSocket();

        int Push(Stream stream);

        int Pop(Stream stream);

        void Close(int timeout = 0);

        ISocket WithModifier<T>() where T : ISocketModifier, new();
    }
}
