using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SockLibNG.Buffer
{
    class Buffer
    {
        private readonly byte[] bytes;
        private int position;

        public Buffer()
        {
            bytes = new byte[1024];
            position = 0;
        }

        public void Add(object primitive)
        {
            var array = ConvertToByteArray(primitive);
            if (!CheckBufferBoundaries(array)) throw new ConstraintException("Failed to add primitive to buffer. There is no additional room for it.");
            foreach (var b in array)
            {
                bytes[position] = b;
                position += 1;
            }
        }

        //NOTE: BitConverter class is .NET ONLY AFAIK. In order to be mono compliant, we need to use DataConvert located at http://www.mono-project.com/Mono_DataConvert
        private byte[] ConvertToByteArray(object primitive)
        {
            if (primitive is int) return BitConverter.GetBytes((int)primitive);
            if (primitive is short) return BitConverter.GetBytes((short)primitive);
            //TODO: Finish list according to list at http://msdn.microsoft.com/en-us/library/ya5y69ds.aspx
        }

        private bool CheckBufferBoundaries(byte[] bytesToCheck)
        {
            var roomLeft = bytes.Length - position;
            return roomLeft >= bytesToCheck.Length;
        }
    }
}
