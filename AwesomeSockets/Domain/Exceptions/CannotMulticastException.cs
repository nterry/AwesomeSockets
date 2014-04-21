using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AwesomeSockets.Domain.Exceptions
{
    public class CannotMulticastException : Exception
    {
        public CannotMulticastException(string message) : base(message)
        {
            //here just for chained constructor
        }
    }
}
