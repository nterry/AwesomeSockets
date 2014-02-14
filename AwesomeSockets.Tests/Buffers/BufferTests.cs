using System;
using NUnit.Framework;
using AwesomeSockets.Domain.Exceptions;
using Buffer = AwesomeSockets.Buffers.Buffer;
using Type2Byte.BaseConverters;

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
            var expected = T2B.ToBytes(4);
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

            Buffer.FinalizeBuffer(otherTestBuffer);

            var expected = Buffer.GetBuffer(testBuffer);
            var actual = Buffer.GetBuffer(otherTestBuffer);

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

        [Test]
        public void EncryptBuffer_CorrectlyEncryptsBuffer_WithTheGivenStringAsTheKey()
        {
            var testBuffer = CreateValidBuffer();

            Buffer.EncryptBuffer(testBuffer, "Foo", "Bar");
            var cipherBuffer = Buffer.Duplicate(testBuffer);
            Buffer.DecryptBuffer(testBuffer, "Foo", "Bar");

            Assert.IsTrue(testBuffer.Equals(cipherBuffer));
        }

        [Test]
        public void EncryptBuffer_IncorrectlyEncryptsBuffer_WithTheIncorrectGivenStringAsTheKey()
        {
            var testBuffer = CreateValidBuffer();
            var originalBuffer = Buffer.Duplicate(testBuffer);

            Buffer.EncryptBuffer(testBuffer, "Foo", "Bar");
            var cipherBuffer = Buffer.Duplicate(testBuffer);
            Buffer.DecryptBuffer(cipherBuffer, "Awesome", "Bar");
            var resultBuffer = Buffer.Duplicate(testBuffer);

            Assert.IsFalse(originalBuffer.Equals(resultBuffer));
        }

        [Test]
        public void EncryptBuffer_CorrectlyEncryptsBuffer_WithTheGivenStringAsTheInitVector()
        {
            var testBuffer = CreateValidBuffer();

            Buffer.EncryptBuffer(testBuffer, "Foo", "Bar");
            var cipherBuffer = Buffer.Duplicate(testBuffer);
            Buffer.DecryptBuffer(testBuffer, "Foo", "Bar");

            Assert.IsTrue(testBuffer.Equals(cipherBuffer));
        }

        //THIS TEST IS BAD. THE IV IS DIFFERENT BUT THE BUFFER'S BYTE ARRAY IS THE SAME. THE REASON IT'S 'PASSING' IS THAT THE NULLSTARTPOSITION IS DIFFERENT FOR SOME REASON
        //[Test]
        //public void EncryptBuffer_IncorrectlyEncryptsBuffer_WithTheIncorrectGivenStringAsTheInitVector()
        //{
        //    var testBuffer = CreateValidBuffer();
        //    var originalBuffer = Buffer.Duplicate(testBuffer);
        //
        //    Buffer.EncryptBuffer(testBuffer, "Foo", "Bar");
        //    var cipherBuffer = Buffer.Duplicate(testBuffer);
        //    Buffer.DecryptBuffer(cipherBuffer, "Foo", "Sauce");
        //    var resultBuffer = Buffer.Duplicate(testBuffer);
        //
        //    Assert.IsFalse(originalBuffer.Equals(resultBuffer));
        //}

        [Test]
        public void Duplicate_ReturnsADifferentButCorrectlyDuplicatedBuffer()
        {
            var testBuffer = CreateValidBuffer();
            var duplicateBuffer = Buffer.Duplicate(testBuffer);

            Assert.AreNotSame(testBuffer, duplicateBuffer);     //This checks to see that there isnt any REFERENCE equality
            Assert.IsTrue(testBuffer.Equals(duplicateBuffer));  //This check to see if there is VALUE equality
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
            var tempBuffer = Buffer.New(16);
            Buffer.Add(tempBuffer, 12);
            Buffer.Add(tempBuffer, 32.0);
            Buffer.Add(tempBuffer, 'c');
            return tempBuffer;
        }
    }
}