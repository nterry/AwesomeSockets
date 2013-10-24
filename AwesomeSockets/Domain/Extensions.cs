using System.Linq;

namespace AwesomeSockets.Domain
{
    static class Extensions
    {
        public static byte ToByte(this byte? nullableByte)
        {
            return nullableByte == null ? (byte)0 : nullableByte.Value;
        }

        public static byte[] DeNullify(this byte?[] bytes)
        {
            return bytes.Select(ToByte).ToArray();
        }
    }
}
