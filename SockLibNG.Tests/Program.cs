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
                    new TestServerAsync();
                    break;
                case "client":
                    new TestClientAsync();
                    break;
            }
        }
    }
}
