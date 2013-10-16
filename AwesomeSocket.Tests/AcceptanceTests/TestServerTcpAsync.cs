using System;
using System.Net;
using AwesomeSocket.Domain.Sockets;
using AwesomeSocket.Sockets;
using System.Net.Sockets;
using Buffer = AwesomeSocket.Buffers.Buffer;

namespace AwesomeSocket.Tests.AcceptanceTests
{
    class TestServerTcpAsync
    {
        private readonly Socket _listenSocket;
        private Socket _client;

        private readonly Buffer _receiveBuffer;
        private readonly Buffer _sendBuffer;

        public TestServerTcpAsync()
        {
            _receiveBuffer = Buffer.New();
            _sendBuffer = Buffer.New();
            _listenSocket = SockLib.TcpListen(14804);
            Console.WriteLine("Server now listening on TCP port 14804");
            SockLib.TcpAccept(_listenSocket, SocketCommunicationTypes.NonBlocking, ClientConnected);
            while (true)
            {
                //Here so the main thread runs continuously
            }
        }

        private void ClientConnected(Socket clientSocket)
        {
            Console.WriteLine("Client has connected.");
            _client = clientSocket;
            SockLib.ReceiveMessage(_client, _receiveBuffer, SocketCommunicationTypes.NonBlocking, MessageReceived);
            SendTestMessage();
        }

        private void MessageReceived(int bytesReceived, EndPoint remoteEndpoint)
        {
            Console.WriteLine(string.Format("Received message from client. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,
                                                                                                                                                                                Buffer.Get<int>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<float>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<double>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<char>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }

        private void SendTestMessage()
        {
            Console.WriteLine("Sending message to client");
            Buffer.ClearBuffer(_sendBuffer);
            Buffer.Add(_sendBuffer, 10);
            Buffer.Add(_sendBuffer, 20.0F);
            Buffer.Add(_sendBuffer, 40.0);
            Buffer.Add(_sendBuffer, 'A');
            Buffer.Add(_sendBuffer, "The quick brown fox jumped over the lazy dog");
            Buffer.Add(_sendBuffer, (byte) 255);
            Buffer.FinalizeBuffer(_sendBuffer);

            SockLib.SendMessage(_client, _sendBuffer);
        }
    }
}
