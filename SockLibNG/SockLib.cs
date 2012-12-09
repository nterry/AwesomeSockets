using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SockLibNG.Domain.Sockets;

namespace SockLibNG
{
    //Callback for NonBlocking TcpAccept thread
    public delegate void TcpAcceptThreadCallback(Socket clientSocket);

    class SockLib
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

        public static Socket TcpAccept(Socket listenSocket, SocketCommunicationTypes type = SocketCommunicationTypes.Blocking, TcpAcceptThreadCallback callback = null)
        {
            if (type == SocketCommunicationTypes.Blocking)
            {
                return listenSocket.Accept();
            }
            if (callback == null) throw new ArgumentNullException("You must provide a valid callback when using the NonBlocking type");
            new Thread(() => TcpAcceptThread(listenSocket, callback)).Start();
            return null;
        }

        private static void TcpAcceptThread(Socket listenSocket, TcpAcceptThreadCallback callback)
        {
            listenSocket.Accept();
            callback(listenSocket);
        }

        public static Socket TcpConnect(string ipAddress, int port)
        {
            var connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = new IPAddress(ParseIpAddress(ipAddress));
            IPEndPoint remoteEndpoint = new IPEndPoint(ip, port);
            connectSocket.Connect(remoteEndpoint);
            return connectSocket;
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
