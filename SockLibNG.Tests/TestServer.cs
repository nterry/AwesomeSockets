using System;
using SockLibNG.Domain.Sockets;
using Buffer = SockLibNG.Buffers.Buffer;
using SockLibNG.Sockets;
using System.Net.Sockets;

namespace SockLibNG.Tests
{
    class TestServer
    {
        private readonly Socket _listenSocket;
        private Socket _client;

        private readonly Buffer _sendBuffer;
        private readonly Buffer _receiveBuffer;

        public TestServer()
        {
            _listenSocket = SockLib.TcpListen(14804);
            _sendBuffer = Buffer.New();
            _receiveBuffer = Buffer.New();
            SockLib.TcpAccept(_listenSocket, SocketCommunicationTypes.NonBlocking, TcpAccepted);
            ReceiveTestMessage();
            while(true)
            {
                //Here so we run continuously...
            }
        }

        private void SendTestMessage()
        {
            Buffer.ClearBuffer(_sendBuffer);
            Buffer.Add(_sendBuffer, 10);
            Buffer.Add(_sendBuffer, 20.0F);
            Buffer.Add(_sendBuffer, 40.0);
            Buffer.Add(_sendBuffer, 'A');
            Buffer.Add(_sendBuffer, "The quick brown fox jumped over the lazy dog");
            Buffer.Add(_sendBuffer, (byte)255);
            Buffer.FinalizeBuffer(_sendBuffer);

            var bytesSent = SockLib.SendMessage(_client, _sendBuffer);
            Console.WriteLine(string.Format("Sent payload. {0} bytes written.", bytesSent));
        }

        private void ReceiveTestMessage()
        {
            SockLib.ReceiveMessage(_client, _receiveBuffer, SocketCommunicationTypes.NonBlocking, MessageReceived);
        }

        private void TcpAccepted(Socket socket)
        {
            _client = socket;
            SendTestMessage();
        }

        private void MessageReceived(int bytesReceived)
        {
            Console.WriteLine(string.Format("Received message from client. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,
                                                                                                                                                                                Buffer.Get<int>(_receiveBuffer), 
                                                                                                                                                                                Buffer.Get<float>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<double>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<char>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }
    }
}
