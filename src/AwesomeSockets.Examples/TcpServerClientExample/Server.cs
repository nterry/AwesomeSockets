using System.Collections.Generic;
using System.Threading;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Examples.TcpServerClientExample
{
    public class Server
    {
        private const int Port = 8080;

        private readonly ISocket _listenSocket;

        private Thread _acceptThread;
        private readonly List<Thread> _clientThreadList;

        public Server()
        {
            // Listen on the accpet port...
            _listenSocket = AweSock.TcpListen(Port);

             // Initialize the client thread list and accpet thread
            _acceptThread = new Thread(Accept);
            _clientThreadList = new List<Thread>();
        }


        private void Accept()
        {
            while (true)
            {
                var client = AweSock.TcpAccept(_listenSocket);
                _clientThreadList.Add(new Thread(() => Reply(client)));
            }
        }

        private void Reply(ISocket socket)
        {
            var inBuf = Buffer.New();
            var outBuf = Buffer.New();

            // Receive message blocks until we recieve something...
            AweSock.ReceiveMessage(socket, inBuf);
            var name = Buffer.Get<string>(inBuf);

            Buffer.ClearBuffer(outBuf);
            Buffer.Add(outBuf, $"Hello {name}!");
            Buffer.FinalizeBuffer(outBuf);

            AweSock.SendMessage(socket, outBuf);
            AweSock.CloseSock(socket);
        }
    }
}
