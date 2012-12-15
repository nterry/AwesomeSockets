using System;
using System.Data;
using System.Text;

namespace SockLibNG.Buffers
{
    public class Buffer
    {
        private const int BUFFER_SIZE = 1024;
        private readonly byte[] bytes;
        private int position;
        private bool finalized;

        private Buffer()
        {
            bytes = new byte[BUFFER_SIZE];
            position = 0;
            finalized = false;
        }

        public static Buffer New()
        {
            return new Buffer();
        }

        public static void ClearBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("Buffer provided cannot be null");
            buffer.ClearBuffer();
        }

        public static void FinalizeBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("Buffer provided cannot be null");
            buffer.FinalizeBuffer();
        }

        public static void Add(Buffer buffer, object value)
        {
            if (buffer == null) throw new ArgumentNullException("Buffer provided cannot be null");
            buffer.Add(value);
        }

        public static T Get<T>(Buffer buffer)
        {
            if (typeof(T) == typeof(bool)) return (T) (object) buffer.GetBoolean();
            if (typeof(T) == typeof(byte)) return (T) (object) buffer.GetByte();
            if (typeof(T) == typeof(sbyte)) return (T) (object) buffer.GetSByte();
            if (typeof(T) == typeof(char)) return (T) (object) buffer.GetChar();
            if (typeof(T) == typeof(double)) return (T) (object) buffer.GetDouble();
            if (typeof(T) == typeof(float)) return (T) (object) buffer.GetFloat();
            if (typeof(T) == typeof(int)) return (T) (object) buffer.GetInt();
            if (typeof(T) == typeof(uint)) return (T) (object) buffer.GetUInt();
            if (typeof(T) == typeof(long)) return (T) (object) buffer.GetLong();
            if (typeof(T) == typeof(ulong)) return (T) (object) buffer.GetULong();
            if (typeof(T) == typeof(short)) return (T) (object) buffer.GetShort();
            if (typeof(T) == typeof(ushort)) return (T) (object) buffer.GetUShort();
            if (typeof(T) == typeof(string)) return (T) (object) buffer.GetString();  //TODO: Need to automatically append null terminator to string ('\0')
            throw new DataException("Provided type cannot be serialized for transmission. You must provide a primitive or a string");
        }

        private void ClearBuffer()
        {
            for (var i = 0; i < BUFFER_SIZE; i ++)
            {
                bytes[i] = 0;
            }
            position = 0;
            finalized = false;
        }

        private void FinalizeBuffer()
        {
            finalized = true;   //TODO: Need to make all commands check for this flag before doing anything!!!!!
            position = 0;
        }

        private void Add(object primitive)
        {
            if (primitive == null) throw new ArgumentNullException("Value provided cannot be null");
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
            throw new DataException("Provided type cannot be serialized for transmission. You must provide a primitive or a string");
        }

        #region private getters
        private bool GetBoolean()
        {
            if (!CheckBufferBoundaries(sizeof(bool))) throw new ConstraintException("Failed to get bool, reached end of buffer.");
            var value =  BitConverter.ToBoolean(bytes, position);
            position += sizeof(bool);
            return value;
        }

        private byte GetByte()
        {
            if (!CheckBufferBoundaries(sizeof(byte))) throw new ConstraintException("Failed to get byte, reached end of buffer.");
            var value = bytes[position];
            position += sizeof(byte);
            return value;
        }

        private sbyte GetSByte()
        {
            if (!CheckBufferBoundaries(sizeof(sbyte))) throw new ConstraintException("Failed to get sbyte, reached end of buffer.");
            var value = bytes[position];
            position += sizeof(sbyte);
            return (sbyte)value;
        }

        private char GetChar()
        {
            if (!CheckBufferBoundaries(sizeof(char))) throw new ConstraintException("Failed to get char, reached end of buffer.");
            var value = BitConverter.ToChar(bytes, position);
            position += sizeof(char);
            return value;
        }

        private double GetDouble()
        {
            if (!CheckBufferBoundaries(sizeof(double))) throw new ConstraintException("Failed to get double, reached end of buffer.");
            var value = BitConverter.ToDouble(bytes, position);
            position += sizeof(double);
            return value;
        }

        private float GetFloat()
        {
            if (!CheckBufferBoundaries(sizeof(float))) throw new ConstraintException("Failed to get float, reached end of buffer.");
            var value = BitConverter.ToSingle(bytes, position);
            position += sizeof(float);
            return value;
        }

        private int GetInt()
        {
            if (!CheckBufferBoundaries(sizeof(int))) throw new ConstraintException("Failed to get int, reached end of buffer.");
            var value = BitConverter.ToInt32(bytes, position);
            position += sizeof(int);
            return value;
        }

        private uint GetUInt()
        {
            if (!CheckBufferBoundaries(sizeof(uint))) throw new ConstraintException("Failed to get uint, reached end of buffer.");
            var value = BitConverter.ToUInt32(bytes, position);
            position += sizeof(uint);
            return value;
        }

        private long GetLong()
        {
            if (!CheckBufferBoundaries(sizeof(long))) throw new ConstraintException("Failed to get long, reached end of buffer.");
            var value = BitConverter.ToInt64(bytes, position);
            position += sizeof(long);
            return value;
        }

        private ulong GetULong()
        {
            if (!CheckBufferBoundaries(sizeof(ulong))) throw new ConstraintException("Failed to get ulong, reached end of buffer.");
            var value = BitConverter.ToUInt64(bytes, position);
            position += sizeof(ulong);
            return value;
        }

        private short GetShort()
        {
            if (!CheckBufferBoundaries(sizeof(short))) throw new ConstraintException("Failed to get short, reached end of buffer.");
            var value = BitConverter.ToInt16(bytes, position);
            position += sizeof(short);
            return value;
        }

        private ushort GetUShort()
        {
            if (!CheckBufferBoundaries(sizeof(ushort))) throw new ConstraintException("Failed to get short, reached end of buffer.");
            var value = BitConverter.ToUInt16(bytes, position);
            position += sizeof(ushort);
            return value;
        }

        private string GetString()
        {
            var localPosition = -1;
            for (var i = position; i <= BUFFER_SIZE; i++)
            {
                if (bytes[i] == '\0')
                {
                    localPosition = i;
                    break;
                }
            }

            if (localPosition != -1)
            {
                var str =  new ASCIIEncoding().GetString(bytes, position, localPosition - position);
                position = localPosition + 1;
                return str;
            }
            throw new ConstraintException("Failed to get string, reached end of buffer.");
        }
        #endregion


        private bool CheckBufferBoundaries(byte[] bytesToCheck)
        {
            var roomLeft = bytes.Length - position;
            return roomLeft >= bytesToCheck.Length;
        }

        private bool CheckBufferBoundaries(int numberOfBytes)
        {
            var roomLeft = bytes.Length - position;
            return roomLeft >= numberOfBytes;
        }
    }
}
