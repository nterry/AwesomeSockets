using System;
using System.Data;
using System.Linq;
using System.Text;

namespace AwesomeSockets.Domain
{
    public class Convert
    {
        public static byte[] ToBytes(bool value)
        {
            return new[] { (value) ? (byte) 1 : (byte) 0 };
        }

        public static byte[] ToBytes(sbyte value)
        {
            return new[] { (byte) value };
        }

        public static byte[] ToBytes(byte value)
        {
            return new[] {  value };
        }

        public static byte[] ToBytes(char value)
        {
            var bytes = new byte[sizeof(char)];
            for (var i = sizeof(char) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        //TODO: This will bew converted to an exact string for serialization and char-wise compressed
        public static byte[] ToBytes(float value)
        {
            var floatArray = new[] { value };
            var byteArray = new byte[sizeof(float)];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }

        //TODO: This will bew converted to an exact string for serialization and char-wise compressed
        public static byte[] ToBytes(double value)
        {
            var doubleArray = new[] { value };
            var byteArray = new byte[sizeof(double)];
            Buffer.BlockCopy(doubleArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static byte[] ToBytes(short value)
        {
            var bytes = new byte[sizeof(short)];
            for (var i = sizeof(short) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(ushort value)
        {
            var bytes = new byte[sizeof(ushort)];
            for (var i = sizeof(ushort) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(int value)
        {
            var bytes = new byte[sizeof(int)];
            for (var i = sizeof(int) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(uint value)
        {
            var bytes = new byte[sizeof(uint)];
            for (var i = sizeof(uint) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(long value)
        {
            var bytes = new byte[sizeof(long)];
            for (var i = sizeof(long) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }

            return bytes;
        }

        public static byte[] ToBytes(ulong value)
        {
            var bytes = new byte[sizeof(ulong)];
            for (var i = sizeof(ulong) - 1; i >= 0; i--)
            {
                bytes[Math.Abs((~i) + 1)] = (byte)(value >> 8 * i);
            }
            return bytes;
        }

        public static T Get<T>(byte[] value)
        {
            if (typeof (T) == typeof (bool)) return (T) (object) GetBool(value);
            if (typeof(T) == typeof(byte)) return (T)(object) GetByte(value);
            if (typeof(T) == typeof(sbyte)) return (T)(object) GetSByte(value);
            if (typeof(T) == typeof(char)) return (T)(object) GetChar(value);
            if (typeof(T) == typeof(float)) return (T)(object) GetFloat(value);
            if (typeof(T) == typeof(double)) return (T)(object) GetDouble(value);    
            if (typeof(T) == typeof(int)) return (T)(object) GetInt(value);
            if (typeof(T) == typeof(uint)) return (T)(object) GetUInt(value);
            if (typeof(T) == typeof(long)) return (T)(object) GetLong(value);
            if (typeof(T) == typeof(ulong)) return (T)(object) GetULong(value);
            if (typeof(T) == typeof(short)) return (T)(object) GetShort(value);
            if (typeof(T) == typeof(ushort)) return (T)(object) GetUShort(value);
            if (typeof(T) == typeof(string)) return (T)(object) GetString(value);
            throw new DataException("Provided type ({0}) cannot be deserialized from given value. You must provide a valuetype (except struct and enum) or a string");
        }

        public static bool IsLittleEndian()
        {
            var test = ToBytes('a');
            var result = System.Convert.ToChar((test[0] | test[1] << 8));
            return (result == 'a');
        }

        //TODO: Need to handle endianness. All information is encoded in little-endian style
        #region private getters

        private static bool GetBool(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(bool)) throw new DataException("Provided data does not appear to be of type bool");
            return (value.First() != 0);
        }

        private static byte GetByte(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(byte)) throw new DataException("Provided data does not appear to be of type byte");
            return value.First();
        }

        private static sbyte GetSByte(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(sbyte)) throw new DataException("Provided data does not appear to be of type sbyte");
            return (sbyte) value.First();
        }

        private static char GetChar(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(char)) throw new DataException("Provided data does not appear to be of type char");
            return System.Convert.ToChar((value[0] | value[1] << 8));
        }

        //TODO: This is broken... Serialize works good, but de-serialize fails to return original value
        private static float GetFloat(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(float)) throw new DataException("Provided data does not appear to be of type float");
            return (float)System.Convert.ToDouble((value[0]) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24));
        }

        //TODO: This is broken... Serialize works good, but de-serialize fails to return original value
        private static double GetDouble(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(double)) throw new DataException("Provided data does not appear to be of type double");
            return System.Convert.ToDouble((value[0]) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24));
        }

        private static int GetInt(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(int)) throw new DataException("Provided data does not appear to be of type int");
            return (value[0] << 0) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24);
        }

        private static uint GetUInt(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(uint)) throw new DataException("Provided data does not appear to be of type uint");
            return System.Convert.ToUInt32((value[0] << 0) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24));
        }

        private static long GetLong(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(long)) throw new DataException("Provided data does not appear to be of type long");
            return System.Convert.ToInt64((value[0] << 0) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24 | value[4] << 32) | (value[5] << 40) | (value[6] << 48) | (value[7] << 56));
        }

        private static ulong GetULong(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(ulong)) throw new DataException("Provided data does not appear to be of type ulong");
            return System.Convert.ToUInt64((value[0] << 0) | (value[1] << 8) | (value[2] << 16) | (value[3] << 24 | value[4] << 32) | (value[5] << 40) | (value[6] << 48) | (value[7] << 56));
        }

        private static short GetShort(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(ulong)) throw new DataException("Provided data does not appear to be of type ulong");
            return System.Convert.ToInt16((value[0] << 0) | (value[1] << 8));
        }

        private static ushort GetUShort(byte[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != sizeof(ulong)) throw new DataException("Provided data does not appear to be of type ulong");
            return System.Convert.ToUInt16((value[0] << 0) | (value[1] << 8));
        }

        private static string GetString(byte[] value)
        {
            var sb = new StringBuilder();
            //var charArray = value.Select(x => (char) x).Aggregate((x, y) => sb.Append((char)x));
            foreach (var @char in value)
            {
                sb.Append(@char);
            }
            return sb.ToString();
        }
        #endregion
    }
}