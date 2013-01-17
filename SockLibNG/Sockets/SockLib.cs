using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SockLibNG.Domain.Sockets;
using Buffer = SockLibNG.Buffers.Buffer;

namespace SockLibNG.Sockets
{
    //Callback for NonBlocking TcpAccept thread
    public delegate void SocketThreadCallback(Socket socket);

    public delegate void MessageThreadCallback(int bytes);

    public class SockLib
    {
        public static Socket TcpListen(int port, int backlog = 10)
        {
            var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPAddress(new byte[] { 0, 0, 0, 0 });
            var localEndPoint = new IPEndPoint(ip, port);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(backlog);
            return listenSocket;
        }

        public static Socket TcpAccept(Socket listenSocket, SocketCommunicationTypes type = SocketCommunicationTypes.Blocking, SocketThreadCallback callback = null)
        {
            if (type == SocketCommunicationTypes.Blocking)
            {
                return listenSocket.Accept();
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => TcpAcceptThread(listenSocket, callback)).Start();
            return null;
        }

        public static Socket TcpConnect(string ipAddress, int port, SocketCommunicationTypes type = SocketCommunicationTypes.Blocking, SocketThreadCallback callback = null)
        {
            var connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPAddress(ParseIpAddress(ipAddress));
            var remoteEndpoint = new IPEndPoint(ip, port);
            if (type == SocketCommunicationTypes.Blocking)
            {
                connectSocket.Connect(remoteEndpoint);
                return connectSocket;
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => TcpConnectThread(connectSocket, remoteEndpoint, callback)).Start();
            return null;
        }

        public static Socket UdpConnect(int port)
        {
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var localEndpoint = new IPEndPoint(IPAddress.Any, port);
            udpSocket.Bind(localEndpoint);
            return udpSocket;
        }

        public static int SendMessage(Socket socket, Buffer buffer)
        {
            if (socket.ProtocolType == ProtocolType.Udp) throw new ConstraintException("Cannot call this method with a UDP socket");
            return socket.Send(Buffer.GetBuffer(buffer));
        }

        public static int SendMessage(Socket socket, string ip, int port, Buffer buffer)
        {
            if (socket.ProtocolType == ProtocolType.Tcp) throw new ConstraintException("Cannot call this method with a TCP socket");
            var ipAddress = new IPAddress(ParseIpAddress(ip));
            var remoteEndpoint = new IPEndPoint(ipAddress, port);
            return socket.SendTo(Buffer.GetBuffer(buffer), remoteEndpoint);
        }

        public static int ReceiveMessage(Socket socket, Buffer buffer, SocketCommunicationTypes type = SocketCommunicationTypes.Blocking, MessageThreadCallback callback = null)
        {
            if (type == SocketCommunicationTypes.Blocking)
            {
                return socket.Receive(Buffer.GetBufferRef(buffer));
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => MessageReceiveThread(socket, buffer, callback)).Start();
            return -1;  //Return negative 1 as 0 bytes received is valid and we want an invalid value 
        }

        public static IPAddress GetRemoteIpAddress(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Udp) throw new ConstraintException("Cannot get remote IP Address of a UDP socket.");
            var socketEndPoint = (IPEndPoint) socket.RemoteEndPoint;
            return socketEndPoint.Address;
        }

        public static int GetRemotePort(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Udp) throw new ConstraintException("Cannot get remote port of a UDP socket.");
            var socketEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            return socketEndPoint.Port;
        }

        public static int BytesAvailable(Socket socket)
        {
            return socket.Available;
        }

        public static void CloseSock(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Tcp) socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public static void CloseSock(Socket socket, int timeout)
        {
            if (socket.ProtocolType == ProtocolType.Tcp) socket.Shutdown(SocketShutdown.Both);
            socket.Close(timeout);
        }

        private static void TcpAcceptThread(Socket listenSocket, SocketThreadCallback callback)
        {
            var clientSocket = listenSocket.Accept();
            callback(clientSocket);
        }

        private static void TcpConnectThread(Socket connectSocket, EndPoint remoteEndpont, SocketThreadCallback callback)
        {
            connectSocket.Connect(remoteEndpont);
            callback(connectSocket);
        }

        private static void MessageReceiveThread(Socket socket, Buffer buffer, MessageThreadCallback callback)
        {
            var bytes = socket.Receive(Buffer.GetBufferRef(buffer));
            callback(bytes);
        }

        private static byte[] ParseIpAddress(string ipAddress)
        {
            var octetList = new List<byte>();
            foreach (var octet in ipAddress.Split(new[] { '.' }))
            {
                byte tmp;
                if (byte.TryParse(octet, out tmp))
                {
                    octetList.Add(tmp);
                }
                else
                {
                    throw new InvalidDataException(string.Format("Received a bad IP address for parsing. Received {0}.", octet));
                }
            }
            return octetList.ToArray();
        }
    }
}
