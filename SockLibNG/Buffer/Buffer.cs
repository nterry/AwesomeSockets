using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SockLibNG.Buffer
{
    class Buffer
    {
        private const int BufferSize = 1024;
        private readonly byte[] bytes;
        private int position; 

        public Buffer()
        {
            bytes = new byte[BufferSize];
            position = 0;
        }

        public static Buffer New()
        {
            return new Buffer();
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

        public byte[] Get()
        {
            return bytes;
        }

        //NOTE: BitConverter class is .NET ONLY AFAIK. In order to be mono compliant, we need to use DataConvert located at http://www.mono-project.com/Mono_DataConvert
        private byte[] ConvertToByteArray(object primitive)
        {
            if (primitive is bool) return BitConverter.GetBytes((bool)primitive);
            if (primitive is byte) return BitConverter.GetBytes((byte)primitive);
            if (primitive is sbyte) return BitConverter.GetBytes((sbyte)primitive);
            if (primitive is char) return BitConverter.GetBytes((char)primitive);
            if (primitive is double) return BitConverter.GetBytes((double)primitive);
            if (primitive is float) return BitConverter.GetBytes((float)primitive);
            if (primitive is int) return BitConverter.GetBytes((int)primitive);
            if (primitive is uint) return BitConverter.GetBytes((uint)primitive);
            if (primitive is long) return BitConverter.GetBytes((long)primitive);
            if (primitive is ulong) return BitConverter.GetBytes((ulong)primitive);
            if (primitive is short) return BitConverter.GetBytes((short)primitive);
            if (primitive is ushort) return BitConverter.GetBytes((ushort)primitive);
            if (primitive is string) return new ASCIIEncoding().GetBytes((string) primitive);
            throw new DataException("Provided type cannot be serialized for transmission. You must provide a primitive.");
        }

        private bool CheckBufferBoundaries(byte[] bytesToCheck)
        {
            var roomLeft = bytes.Length - position;
            return roomLeft >= bytesToCheck.Length;
        }
    }
}
