using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SockLibNG.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "server")
            {
                new TestServer();
            }
            else if (args[0] == "client")
            {
                new TestClient();
            }
        }
    }
}
