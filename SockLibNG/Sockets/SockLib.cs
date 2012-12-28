using System;
using System.Collections.Generic;
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
            IPAddress ip = new IPAddress(new byte[] { 0, 0, 0, 0 });
            IPEndPoint localEndPoint = new IPEndPoint(ip, port);
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
            IPAddress ip = new IPAddress(ParseIpAddress(ipAddress));
            IPEndPoint remoteEndpoint = new IPEndPoint(ip, port);
            if (type == SocketCommunicationTypes.Blocking)
            {
                connectSocket.Connect(remoteEndpoint);
                return connectSocket;
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => TcpConnectThread(connectSocket, remoteEndpoint, callback)).Start();
            return null;
        }

        public static int SendMessage(Socket socket, Buffer buffer)
        {
            return socket.Send(Buffer.GetBuffer(buffer));
        }

        public static int ReceiveMessage(Socket socket, Buffer buffer, SocketCommunicationTypes type = SocketCommunicationTypes.Blocking, MessageThreadCallback callback = null)
        {
            if (type == SocketCommunicationTypes.Blocking)
            {
                return socket.Receive(Buffer.GetBufferRef(buffer));
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => MessageReceiveThread(socket, buffer, callback));
            return -1;  //Return negative 1 as 0 bytes received is valid and we want an invalid value 
        }

        public static int BytesAvailable(Socket socket)
        {
            return socket.Available;
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
