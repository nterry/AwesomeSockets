using System;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeSockets.Domain
{
    public static class Extensions
    {
        public static byte ToByte(this byte? nullableByte)
        {
            return nullableByte == null ? (byte)0 : nullableByte.Value;
        }

        public static byte[] DeNullify(this byte?[] bytes)
        {
            return bytes.Select(ToByte).ToArray();
        }


        //TODO: This is broken..... Need to fix
        public static byte?[] Nullify(this byte[] bytes, int minCheck)
        {
            var zeroCount = 0;
            var nullMarker = 0;
            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                    zeroCount++;
                else
                    zeroCount = 0;

                if (zeroCount != minCheck) 
                    continue;

                nullMarker = i;
                break;
            }
            return nullMarker > 0 ? RunNullification(bytes, nullMarker) : null;
        }

        private static byte?[] RunNullification(byte[] bytes, int start)
        {
            var tempList = new List<byte?>();
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < start)
                    tempList.Add(bytes[i]);
                else
                    tempList.Add(null); 
            }
            return tempList.ToArray();
        }

        #region ByteConverters


        public static byte[] ToBytes(this char value)
        {
            const int size = sizeof(char);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this float value)
        {
            var floatArray = new[] { value };
            var byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }

        public static byte[] ToBytes(this double value)
        {
            var doubleArray = new[] { value };
            var byteArray = new byte[doubleArray.Length * 8];
            Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }

        public static byte[] ToBytes(this short value)
        {
            const int size = sizeof(short);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this ushort value)
        {
            const int size = sizeof(ushort);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this int value)
        {
            const int size = sizeof(int);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this uint value)
        {
            const int size = sizeof(uint);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this long value)
        {
            const int size = sizeof(long);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(this ulong value)
        {
            const int size = sizeof(ulong);
            var bytes = new byte[size];
            for (var i = size - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

//        if (primitive is bool) return BitConverter.GetBytes((bool)primitive);
//            if (primitive is byte) return new[] { (byte)primitive }; //GetBytes casts a byte to a short resulting in a 2-byte array. We can just return the array with the byte in it.
//            if (primitive is sbyte) return BitConverter.GetBytes((sbyte)primitive);
//            if (primitive is char) return BitConverter.GetBytes((char)primitive);
//            if (primitive is double) return BitConverter.GetBytes((double)primitive);
//            if (primitive is float) return BitConverter.GetBytes((float)primitive);
//            if (primitive is int) return BitConverter.GetBytes((int)primitive);
//            if (primitive is uint) return BitConverter.GetBytes((uint)primitive);
//            if (primitive is long) return BitConverter.GetBytes((long)primitive);
//            if (primitive is ulong) return BitConverter.GetBytes((ulong)primitive);
//            if (primitive is short) return BitConverter.GetBytes((short)primitive);
//            if (primitive is ushort) return BitConverter.GetBytes((ushort)primitive);
//            if (primitive is string)
//            {
//                var str = primitive as string;
//                if (str.Contains("\0")) throw new DataException("String cannot contain null character '\\0'");
//                return new ASCIIEncoding().GetBytes(string.Format("{0}{1}", str, "\0"));    //The '\0' is to null-terminate the string so we can deserialize it on the other end as strings aren't fixed in size
//            }
//            throw new DataException("Provided type cannot be serialized for transmission. You must provide a primitive or a string");

        #endregion
    }
}
