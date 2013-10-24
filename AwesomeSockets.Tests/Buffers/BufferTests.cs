using System;
using NUnit.Framework;
using AwesomeSockets.Domain.Exceptions;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.Tests.Buffers
{
    [TestFixture]
    class BufferTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetBuffer_ReturnsAnAppropriatelySizedBuffer()
        {
            var testBuffer = CreateValidBuffer();
            Buffer.FinalizeBuffer(testBuffer);
            const int expected = sizeof (int) + sizeof (double) + sizeof (char);
            var actual = Buffer.GetBuffer(testBuffer).Length;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetBuffer_BufferFinalizedException_WhenBufferIsntFinalized()
        {
            var invalidBuffer = CreateInvalidBuffer();
            Assert.Throws<BufferFinalizedException>(() => Buffer.GetBuffer(invalidBuffer));
        }

        [Test]
        public void GetBuffer_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Buffer nullBuffer = null;
            Assert.Throws<ArgumentNullException>(() => Buffer.GetBuffer(nullBuffer));
        }

        [Test]
        public void Add_ConcatenatesAppropriateByteToBuffer()
        {
            var testBuffer = Buffer.New();
            Buffer.Add(testBuffer, 4);
            Buffer.FinalizeBuffer(testBuffer);
            var expected = BitConverter.GetBytes(4);
            var actual = Buffer.GetBuffer(testBuffer);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Add_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Buffer nullBuffer = null;
            Assert.Throws<ArgumentNullException>(() => Buffer.Add(nullBuffer, 4));
        }



        private Buffer CreateValidBuffer()
        {
            var buff = CreateBuffer();
            Buffer.FinalizeBuffer(buff);
            return buff;
        }

        private Buffer CreateInvalidBuffer()
        {
            return CreateBuffer();
        }

        private Buffer CreateBuffer()
        {
            var tempBuffer = Buffer.New();
            Buffer.Add(tempBuffer, 12);
            Buffer.Add(tempBuffer, 32.0);
            Buffer.Add(tempBuffer, 'c');
            return tempBuffer;
        }
    }
}
