using SockLibNG.Tests.AcceptanceTests;

namespace SockLibNG.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "server":
                    //new TestServerTcpAsync();
                    new TestServerUdp();
                    break;
                case "client":
                    //new TestClientTcpAsync();
                    new TestClientUdp();
                    break;
            }
        }
    }
}
