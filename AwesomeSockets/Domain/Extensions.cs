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
    }
}
