using System;

namespace AwesomeSockets.Domain.Exceptions
{
    public class BufferFinalizedException : Exception
    {
        public BufferFinalizedException(string message) : base(message)
        {
            //here just for chained constructor
        }

    }
}
