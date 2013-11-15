using System.Collections.Generic;
using System.Net.Sockets;

namespace AwesomeSockets.Domain.Sockets
{
    public interface ISocket
    {
        void SetGlobalConfiguration(Dictionary<SocketOptionName, object> opts);
        Socket GetInternalSocket();
    }
}
