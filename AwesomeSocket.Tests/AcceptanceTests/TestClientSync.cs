using System;
using System.Net;
using System.Net.Sockets;
using AwesomeSocket.Sockets;
using Buffer = AwesomeSocket.Buffers.Buffer;

namespace AwesomeSocket.Tests.AcceptanceTests
{
    class TestClientSync
    {
        private Socket _server;

        private readonly Buffer _sendBuffer;
        private readonly Buffer _receiveBuffer;

        public TestClientSync()
        {
            _sendBuffer = Buffer.New();
            _receiveBuffer = Buffer.New();
            Console.WriteLine("Connecting to server... Please Wait");
            _server = SockLib.TcpConnect("127.0.0.1", 14804);
            ReceiveTestMessage();
            SendTestMessage();
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

            var bytesSent = SockLib.SendMessage(_server, _sendBuffer);
            Console.WriteLine(string.Format("Sent payload. {0} bytes written.", bytesSent));
        }

        private void ReceiveTestMessage()
        {
            Tuple<int, EndPoint> bytesReceived = SockLib.ReceiveMessage(_server, _receiveBuffer);
            MessageReceived(bytesReceived);
        }

        private void TcpConnected(Socket socket)
        {
            Console.WriteLine("Connected to server. Waiting for server to send message...");
            _server = socket;
            ReceiveTestMessage();
        }

        private void MessageReceived(Tuple<int, EndPoint> bytesReceived)
        {
            Console.WriteLine(string.Format("Received message from server. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,
                                                                                                                                                                                Buffer.Get<int>(_receiveBuffer), 
                                                                                                                                                                                Buffer.Get<float>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<double>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<char>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }
    }
}
