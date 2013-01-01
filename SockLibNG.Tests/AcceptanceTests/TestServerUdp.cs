using System;
using System.Net;
using System.Net.Sockets;
using SockLibNG.Sockets;
using Buffer = SockLibNG.Buffers.Buffer;

namespace SockLibNG.Tests.AcceptanceTests
{
    class TestServerUdp
    {
        private readonly Socket _localSocket;

        private const int LocalUdpPort = 14805;
        private const int RemoteUdpPort = 14806;

        private readonly Buffer _receiveBuffer;
        private readonly Buffer _sendBuffer;

        public TestServerUdp()
        {
            _receiveBuffer = Buffer.New();
            _sendBuffer = Buffer.New();

            var listenSocket = SockLib.TcpListen(14804);
            var client = SockLib.TcpAccept(listenSocket);
            var clientIp = SockLib.GetRemoteIpAddress(client);

            _localSocket = SockLib.UdpConnect(LocalUdpPort);

            SendTestMessage(clientIp);

            ReceiveTestResponse();



            while (true)
            {
                //Here so the main thread runs continuously.
            }
        }

        private void ReceiveTestResponse()
        {
            var bytesReceived = SockLib.ReceiveMessage(_localSocket, _receiveBuffer);
            Console.WriteLine(string.Format("Received message from client. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,
                                                                                                                                                                                Buffer.Get<int>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<float>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<double>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<char>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }

        private void SendTestMessage(IPAddress clientIp)
        {
            Console.WriteLine("Sending test message");
            Buffer.ClearBuffer(_sendBuffer);
            Buffer.Add(_sendBuffer, 20);
            Buffer.Add(_sendBuffer, 40.0F);
            Buffer.Add(_sendBuffer, 80.0);
            Buffer.Add(_sendBuffer, 'B');
            Buffer.Add(_sendBuffer, "Giggity gigity goo!!!");
            Buffer.Add(_sendBuffer, (byte)127);
            Buffer.FinalizeBuffer(_sendBuffer);

            SockLib.SendMessage(_localSocket, clientIp.ToString(), RemoteUdpPort, _sendBuffer);
        }
    }
}
