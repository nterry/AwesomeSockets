using System.Collections.Generic;
using System.Linq;

namespace AwesomeSockets.Domain
{
    public static class Extensions
    {
        internal static byte ToByte(this byte? nullableByte)
        {
            return nullableByte == null ? (byte)0 : nullableByte.Value;
        }

        internal static byte[] DeNullify(this byte?[] bytes)
        {
            return bytes.Select(ToByte).ToArray();
        }


        //TODO: This is broken..... Need to fix
        internal static byte?[] Nullify(this byte[] bytes, int minCheck)
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
    }
}
