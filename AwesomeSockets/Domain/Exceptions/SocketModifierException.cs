using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeSockets.Domain.Exceptions
{
    public class SocketModifierException : Exception
    {
        public SocketModifierException(string message) : base(message)
        {
            //here just for chained constructor
        }
    }
}
