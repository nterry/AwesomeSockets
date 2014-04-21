using AwesomeSockets.Domain.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public class MulticastV6SocketModifier : AbstractSocketModifier, ISocketModifier
    {
        private static List<dynamic> conflicts = new List<dynamic> { typeof(MulticastSocketModifier) };

        public MulticastV6SocketModifier() : base(conflicts) { }

        public ISocket Apply(ISocket socket, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
