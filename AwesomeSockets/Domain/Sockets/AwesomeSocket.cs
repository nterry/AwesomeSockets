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
        private readonly Socket _internalSocket;

        private AwesomeSocket(Socket socket)
        {
            _internalSocket = socket;
        }

        internal static AwesomeSocket New(Socket socket)
        {
            return new AwesomeSocket(socket);
        }

        public Socket GetSocket()
        {
            return _internalSocket;
        }

        public ISocket Accept()
        {
            return New(_internalSocket.Accept());
        }

        public void Connect(EndPoint remoteEndPoint)
        {
            _internalSocket.Connect(remoteEndPoint);
        }

        public int SendMessage(Buffer buffer)
        {
            return _internalSocket.Send(Buffer.GetBuffer(buffer));
        }

        public int SendMessage(string ip, int port, Buffer buffer)
        {
            var ipAddress = IPAddress.Parse(ip);
            var remoteEndpoint = new IPEndPoint(ipAddress, port);
            return _internalSocket.SendTo(Buffer.GetBuffer(buffer), remoteEndpoint);
        }

        public Tuple<int, EndPoint> ReceiveMessage(Buffer buffer)
        {
            return Tuple.Create(_internalSocket.Receive(Buffer.GetBufferRef(buffer)), _internalSocket.RemoteEndPoint);
        }

        public Tuple<int, EndPoint> ReceiveMessage(string ip, int port, Buffer buffer)
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            return Tuple.Create(_internalSocket.ReceiveFrom(Buffer.GetBufferRef(buffer), ref remoteEndPoint), remoteEndPoint);
        }

        public EndPoint GetRemoteEndPoint()
        {
            return _internalSocket.RemoteEndPoint;
        }

        public ProtocolType GetProtocolType()
        {
            return _internalSocket.ProtocolType;
        }

        public int GetBytesAvailable()
        {
            return _internalSocket.Available;
        }

        public void Close(int timeout = 0)
        {
            if (timeout == 0)
                _internalSocket.Close();
            else 
                _internalSocket.Close(timeout);
        }

        public ISocket WithModifier<T>() where T : ISocketModifier, new()
        {
            return new WithModifierWrapper<T>().ApplyModifier(this);
        }

        public void SetGlobalConfiguration(Dictionary<SocketOptionName, object> opts)
        {
            foreach (var opt in opts)
            {
                _internalSocket.SetSocketOption(SocketOptionLevel.Socket, opt.Key, opt.Value);
            }
        }
    }
}
