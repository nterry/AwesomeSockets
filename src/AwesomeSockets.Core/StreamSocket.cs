using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using AwesomeSockets.Model;

namespace AwesomeSockets.Core
{
    public abstract class StreamSocket : AwesomeSocket
    {
        protected StreamSocket(Socket internalSocket) : base(internalSocket)
        {
        }
    }
}
