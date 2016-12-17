namespace AwesomeSockets.Domain
{
    public enum SockType
    {
        Tcp,
        Udp
    }

    public enum SocketCommunicationTypes
    {
        Blocking,
        NonBlocking
    }

    public enum KeySizes
    {
        OneTwenyEight = 128,
        OneNinetyTwo = 192,
        TwoFiftySix = 256
    }
}
