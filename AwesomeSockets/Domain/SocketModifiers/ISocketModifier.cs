using AwesomeSockets.Domain.Sockets;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public interface ISocketModifier
    {
        ISocket Apply(ISocket socket);
    }
}
