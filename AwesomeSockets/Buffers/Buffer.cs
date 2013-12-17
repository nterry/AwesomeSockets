using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AwesomeSockets.Domain;
using AwesomeSockets.Domain.Exceptions;
using Convert = AwesomeSockets.Domain.Convert;

namespace AwesomeSockets.Buffers
{
    public class Buffer
    {
        private const int DEFAULT_BUFFER_SIZE = 1024;
        private readonly int bufferSize;
        private readonly byte?[] bytes;
        private int position;
        private bool finalized;

        private Buffer(int bufferSize)
        {
            this.bufferSize = bufferSize;
            bytes = new byte?[bufferSize];
            position = 0;
            finalized = false;
        }

        public static Buffer New()
        {
            return New(DEFAULT_BUFFER_SIZE);
        }

        public static Buffer New(int bufferSize)
        {
            return new Buffer(bufferSize);
        }


        public static void ClearBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.ClearBuffer();
        }

        public static void FinalizeBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.FinalizeBuffer();
        }

        public static void Add(Buffer buffer, Buffer bufferToWrite)
        {
            if (buffer == null || bufferToWrite == null) throw new ArgumentNullException("buffer");
            buffer.ClearBuffer();
            buffer = New();
            buffer.Add(bufferToWrite.bytes.DeNullify());
        }

        public static void Add(Buffer buffer, byte[] byteArray)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (byteArray.Length == 0) throw new DataException("Cannot provide a zero-length array");
            buffer.ClearBuffer();
            buffer.Add(byteArray);
            buffer.FinalizeBuffer();
        }

        public static void Add(Buffer buffer, object value)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (buffer.finalized) throw new BufferFinalizedException("Buffer provided is in 'finalized' state. You must call 'ClearBuffer()' to reset it.");
            buffer.Add(value);
        }

        public static T Get<T>(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
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
            if (typeof(T) == typeof(string)) return (T) (object) buffer.GetString();
            throw new DataException(string.Format("Provided type ({0}) cannot be deserialized from buffer. You must provide a (except struct and enum) or a string", typeof(T)));
        }

        public static byte[] GetBuffer(Buffer buffer)
        {
           if (buffer == null) throw new ArgumentNullException("buffer");
           if (!buffer.finalized) throw new BufferFinalizedException("Buffer provided is not in 'finalized' state. You must call 'FinalizeBuffer()' in order to get the full buffer");
           return buffer.GetBuffer();
        }

        public static byte[] GetBufferRef(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.ClearBuffer();
            return buffer.GetBuffer();
        }

        public static void EncryptBuffer(Buffer buffer, string encryptionKey)
        {
            
        }

        //Here for converting doubles and floats...
        public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
        {
            System.Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        }

        private byte[] GetBuffer()
        { 
            var tempList = new List<byte?>();
            for (var i = 0; i < bufferSize; i++)
            {
                if (bytes[i] != null)
                    tempList.Add(bytes[i]);
                else
                    return tempList.ToArray().DeNullify();
            }

            return bytes.DeNullify();
        }

        private void ClearBuffer()
        {
            for (var i = 0; i < bufferSize; i ++)
            {
                bytes[i] = 0;
            }
            position = 0;
            finalized = false;
        }

        private void FinalizeBuffer()
        {
            finalized = true;
            position = 0;
        }

        private void Add(byte[] byteArray)
        {
            for (var i = 0; i < bufferSize; i++)
            {
                if (i < byteArray.Count())
                    bytes[i] = byteArray[i];
                else
                    bytes[i] = null;
            }
        }

        private void Add(object primitive)
        {
            if (primitive == null) throw new ArgumentNullException("primitive");
            var array = ConvertToByteArray(primitive);
            //TODO: Need to increase buffer size in this case, rather than throwing an exception
            if (!CheckBufferBoundaries(array)) throw new ConstraintException("Failed to add primitive to buffer. There is no additional room for it.");
            foreach (var b in array)
            {
                bytes[position] = b;
                position += 1;
            }
        }

        private static byte[] ConvertToByteArray(object primitive)
        {
            if (primitive is bool) return Convert.ToBytes((bool) primitive);
            if (primitive is byte) return Convert.ToBytes((byte) primitive);
            if (primitive is sbyte) return Convert.ToBytes((sbyte) primitive);
            if (primitive is char) return Convert.ToBytes((char) primitive);
            if (primitive is double) return Convert.ToBytes((double) primitive);
            if (primitive is float) return Convert.ToBytes((float) primitive);
            if (primitive is int) return Convert.ToBytes((int) primitive);
            if (primitive is uint) return Convert.ToBytes((uint) primitive);
            if (primitive is long) return Convert.ToBytes((long) primitive);
            if (primitive is ulong) return Convert.ToBytes((ulong) primitive);
            if (primitive is short) return Convert.ToBytes((short) primitive);
            if (primitive is ushort) return Convert.ToBytes((ushort) primitive);
            if (primitive is string)
            {
                var str = primitive as string;
                if (str.Contains("\0")) throw new DataException("String cannot contain null character '\\0'");
                return new ASCIIEncoding().GetBytes(string.Format("{0}{1}", str, "\0"));    //The '\0' is to null-terminate the string so we can deserialize it on the other end as strings aren't fixed in size
            }
            throw new DataException("Provided type cannot be serialized for transmission. You must provide a value type (except enum and struct) or a string");
        }


        #region private getters
        private bool GetBoolean()
        {
            if (!CheckBufferBoundaries(sizeof(bool))) throw new ConstraintException("Failed to get bool, reached end of buffer.");
            var value = Convert.Get<bool>(bytes.DeNullify());
            position += sizeof(bool);
            return value;
        }

        private byte GetByte()
        {
            if (!CheckBufferBoundaries(sizeof(byte))) throw new ConstraintException("Failed to get byte, reached end of buffer.");
            var value = bytes[position];
            position += sizeof(byte);
            return value.ToByte();
        }

        private sbyte GetSByte()
        {
            if (!CheckBufferBoundaries(sizeof(sbyte))) throw new ConstraintException("Failed to get sbyte, reached end of buffer.");
            var value = bytes[position];
            position += sizeof(sbyte);
            return (sbyte)value.ToByte();
        }

        private char GetChar()
        {
            if (!CheckBufferBoundaries(sizeof(char))) throw new ConstraintException("Failed to get char, reached end of buffer.");
            var value = Convert.Get<char>(bytes.DeNullify());
            position += sizeof(char);
            return value;
        }

        private double GetDouble()
        {
            if (!CheckBufferBoundaries(sizeof(double))) throw new ConstraintException("Failed to get double, reached end of buffer.");
            var value = Convert.Get<Double>(bytes.DeNullify());
            position += sizeof(double);
            return value;
        }

        private float GetFloat()
        {
            if (!CheckBufferBoundaries(sizeof(float))) throw new ConstraintException("Failed to get float, reached end of buffer.");
            var value = Convert.Get<float>(bytes.DeNullify());
            position += sizeof(float);
            return value;
        }

        private int GetInt()
        {
            if (!CheckBufferBoundaries(sizeof(int))) throw new ConstraintException("Failed to get int, reached end of buffer.");
            var value = Convert.Get<int>(bytes.DeNullify());
            position += sizeof(int);
            return value;
        }

        private uint GetUInt()
        {
            if (!CheckBufferBoundaries(sizeof(uint))) throw new ConstraintException("Failed to get uint, reached end of buffer.");
            var value = Convert.Get<uint>(bytes.DeNullify());
            position += sizeof(uint);
            return value;
        }

        private long GetLong()
        {
            if (!CheckBufferBoundaries(sizeof(long))) throw new ConstraintException("Failed to get long, reached end of buffer.");
            var value = Convert.Get<long>(bytes.DeNullify());
            position += sizeof(long);
            return value;
        }

        private ulong GetULong()
        {
            if (!CheckBufferBoundaries(sizeof(ulong))) throw new ConstraintException("Failed to get ulong, reached end of buffer.");
            var value = Convert.Get<ulong>(bytes.DeNullify());
            position += sizeof(ulong);
            return value;
        }

        private short GetShort()
        {
            if (!CheckBufferBoundaries(sizeof(short))) throw new ConstraintException("Failed to get short, reached end of buffer.");
            var value = Convert.Get<short>(bytes.DeNullify());
            position += sizeof(short);
            return value;
        }

        private ushort GetUShort()
        {
            if (!CheckBufferBoundaries(sizeof(ushort))) throw new ConstraintException("Failed to get short, reached end of buffer.");
            var value = Convert.Get<ushort>(bytes.DeNullify());
            position += sizeof(ushort);
            return value;
        }

        private string GetString()
        {
            var localPosition = -1;
            for (var i = position; i <= bufferSize; i++)
            {
                if (bytes[i] == '\0')
                {
                    localPosition = i;
                    break;
                }
            }

            if (localPosition != -1)
            {
                var str = new ASCIIEncoding().GetString(bytes.DeNullify(), position, localPosition - position);
                position = localPosition + 1;
                return str;
            }
            throw new ConstraintException("Failed to get string, reached end of buffer.");
        }
        #endregion

        #region private boundary checks
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
        #endregion

        #region private misc methods

        private byte[] ConvertStringToByteArray(string stringToConvert)
        {
            var initialConvert = new ASCIIEncoding().GetBytes(stringToConvert);
            var sanitizedConvert = new byte[256];

            if (initialConvert.Length > 256)    //Truncate if greater than 256
            {
                sanitizedConvert = initialConvert.Take(256).ToArray();
            }
            else    //fill with repeats until 256
            {
                var rotatePosition = initialConvert.Length;
                for (var i = 0; i < initialConvert.Length; i++)
                    sanitizedConvert[i] = initialConvert[i];
                do
                {
                    //TODO: Need to rotate around initialConvert, appending to sanitizedConvert along the way
                } while (rotatePosition < 256);
                sanitizedConvert = null;
            }

            throw new NotImplementedException();
        }
        #endregion
    }
}
