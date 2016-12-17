using AwesomeSockets.Domain.SocketModifiers;
using AwesomeSockets.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

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

        public Socket GetSocket()
        {
            return InternalSocket;
        }

        public ISocket Accept()
        {
            return New(InternalSocket.Accept());
        }

        public void Connect(EndPoint remoteEndPoint)
        {
            InternalSocket.Connect(remoteEndPoint);
        }

        public int SendMessage(Buffer buffer)
        {
            return InternalSocket.Send(Buffer.GetBuffer(buffer));
        }

        public int SendMessage(string ip, int port, Buffer buffer)
        {
            var ipAddress = IPAddress.Parse(ip);
            var remoteEndpoint = new IPEndPoint(ipAddress, port);
            return InternalSocket.SendTo(Buffer.GetBuffer(buffer), remoteEndpoint);
        }

        public Tuple<int, EndPoint> ReceiveMessage(Buffer buffer)
        {
            return Tuple.Create(InternalSocket.Receive(Buffer.GetBufferRef(buffer)), InternalSocket.RemoteEndPoint);
        }

        public Tuple<int, EndPoint> ReceiveMessage(string ip, int port, Buffer buffer)
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            return Tuple.Create(InternalSocket.ReceiveFrom(Buffer.GetBufferRef(buffer), ref remoteEndPoint), remoteEndPoint);
        }

        public EndPoint GetRemoteEndPoint()
        {
            return InternalSocket.RemoteEndPoint;
        }

        public ProtocolType GetProtocolType()
        {
            return InternalSocket.ProtocolType;
        }

        public int GetBytesAvailable()
        {
            return InternalSocket.Available;
        }

        public void Close(int timeout = 0)
        {
            //TODO: The socket.close() method has been re-added to corefx and will be available in 1.2... Until then, we can only use it in 4.0+
            //TODO: See https://github.com/dotnet/corefx/issues/12060 for more details...
#if NET40
            if (timeout == 0)
                InternalSocket.Close();
            else 
                InternalSocket.Close(timeout);
#endif
        }

        public ISocket WithModifier<T>() where T : ISocketModifier, new()
        {
            return new WithModifierWrapper<T>().ApplyModifier(this);
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
