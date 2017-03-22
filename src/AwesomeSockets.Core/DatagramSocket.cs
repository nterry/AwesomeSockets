using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using AwesomeSockets.Model;

namespace AwesomeSockets.Core
{
    public abstract class DatagramSocket : AwesomeSocket
    {
        protected DatagramSocket(Socket internalSocket) : base(internalSocket)
        {
        }
    }
}
