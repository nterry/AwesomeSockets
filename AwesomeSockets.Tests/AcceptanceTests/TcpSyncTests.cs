using System;
using System.Net.Sockets;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using NUnit.Framework;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Tests.AcceptanceTests
{
    [TestFixture]
    class TcpSyncTests
    {
        bool SERVER_EXIT_FLAG;

        [Test]
        public void TcpSynchronousAcceptanceTest()
        {
            SERVER_EXIT_FLAG = false;
        }

        private void ServerThread()
        {
            var sendBuffer = Buffer.New();
            var recvBuffer = Buffer.New();
            var listenSocket = AweSock.TcpListen(14804);
            var client = AweSock.TcpAccept(listenSocket);

            SendServerTestMessage(client, sendBuffer);
            
            do
            {
            } while (!SERVER_EXIT_FLAG);
        }

        private void SendServerTestMessage(ISocket client, Buffer sendBuffer)
        {
            Buffer.ClearBuffer(sendBuffer);
            Buffer.Add(sendBuffer, 10);
            Buffer.Add(sendBuffer, 20.0F);
            Buffer.Add(sendBuffer, 40.0);
            Buffer.Add(sendBuffer, 'A');
            Buffer.Add(sendBuffer, "The quick brown fox jumped over the lazy dog");
            Buffer.Add(sendBuffer, (byte)255);
            Buffer.FinalizeBuffer(sendBuffer);

            var bytesSent = AweSock.SendMessage(client, sendBuffer);
            Console.WriteLine("Sent payload. {0} bytes written.", bytesSent);
        }

        private void ReceiveServerTestResponse(ISocket client, Buffer recvBuffer)
        {
            do
            {
                var bytesReceived = AweSock.ReceiveMessage(client, recvBuffer);
                if (bytesReceived.Item1 > 0)
                    SERVER_EXIT_FLAG = ValidateServerTestResponse(recvBuffer);
                else if (bytesReceived.Item1 == 0)
                    throw new ApplicationException("Client thread has died prematurely");

            } while (!SERVER_EXIT_FLAG);           
        }

        private bool ValidateServerTestResponse(Buffer receiveBuffer)
        {
            throw new NotImplementedException();
        }


        private void TcpAccepted(ISocket socket)
        {
            _client = socket;
            SendTestMessage();
        }

        private void MessageReceived(int bytesReceived)
        {
            Console.WriteLine(string.Format("Received message from client. Size is {0}. Details are as follows: {1} (int)\n{2} (float)\n{3} (double)\n{4} (char)\n{5} (string)\n{6} (byte)", bytesReceived,                                                                                                                                                                      Buffer.Get<string>(_receiveBuffer),
                                                                                                                                                                                Buffer.Get<byte>(_receiveBuffer)));
        }
    }
}
