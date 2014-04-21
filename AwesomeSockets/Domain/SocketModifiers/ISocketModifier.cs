using AwesomeSockets.Domain.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public interface ISocketModifier
    {
        ISocket Apply(ISocket socket, params string[] args);
    }
}
