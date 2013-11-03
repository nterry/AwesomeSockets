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
        public void GetBuffer_ReturnsAppropriatelySizedBuffer()
        {
            var testBuffer = CreateValidBuffer();
            const int expected = sizeof (int) + sizeof (double) + sizeof (char);
            var actual = Buffer.GetBuffer(testBuffer).Length;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetBuffer_ThrowsBufferFinalizedException_WhenBufferIsntFinalized()
        {
            var invalidBuffer = CreateInvalidBuffer();
            Assert.Throws<BufferFinalizedException>(() => Buffer.GetBuffer(invalidBuffer));
        }

        [Test]
        public void GetBuffer_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.GetBuffer(null));
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
            Assert.Throws<ArgumentNullException>(() => Buffer.Add(null, 4));
        }

        [Test]
        public void Add_ThrowsBufferFinalizedException_WhenBufferIsFinalized()
        {
            var testBuffer = CreateValidBuffer();
            Assert.Throws<BufferFinalizedException>(() => Buffer.Add(testBuffer, 1));
        }

        [Test]
        public void Add_ClearsCurrentBuffer_WhenNewValueIsProvided_AndBufferIsntFinalized()
        {
            var testBuffer = CreateBuffer();
            Buffer.Add(testBuffer, new[] {(byte) 1});

            var otherTestBuffer = Buffer.New();
            Buffer.Add(otherTestBuffer, (byte) 1);
//
//            Buffer.Add(otherTestBuffer, testBuffer);
            Buffer.FinalizeBuffer(otherTestBuffer);

            var expected = Buffer.GetBuffer(testBuffer);
            var actual = Buffer.GetBuffer(otherTestBuffer);
//            var actual = Buffer.GetBuffer(otherTestBuffer);
//
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ClearBuffer_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.ClearBuffer(null));
        }

        [Test]
        public void FinalizeBuffer_ArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.FinalizeBuffer(null));
        }

        [Test]
        public void FinalizeBuffer_DoesntThrowBufferFinalizedException_IfBufferIsAlreadyFinalized()
        {
            var testBuffer = CreateValidBuffer();
            Assert.DoesNotThrow(() => Buffer.FinalizeBuffer(testBuffer));
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
