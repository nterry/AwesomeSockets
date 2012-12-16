using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffer = SockLibNG.Buffers.Buffer;

namespace SockLibNG.Domain.Exceptions
{
    class BufferFinalizedException : Exception
    {
        public BufferFinalizedException(string message) : base(message)
        {
            //here just for chained constructor
        }

    }
}
