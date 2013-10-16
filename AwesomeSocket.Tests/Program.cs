using AwesomeSocket.Tests.AcceptanceTests;

namespace AwesomeSocket.Tests
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
