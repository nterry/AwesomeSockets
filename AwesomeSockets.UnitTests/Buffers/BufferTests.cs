using System;
using Xunit;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace AwesomeSockets.UnitTests.Buffers
{
    public class BufferTests
    {
        [Fact]
        public void GetBuffer_ReturnsAppropriatelySizedBuffer()
        {
            var testBuffer = CreateValidBuffer();
            const int expected = sizeof (int) + sizeof (double) + sizeof (char);
            var actual = Buffer.GetBuffer(testBuffer).Length;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetBuffer_ThrowsBufferFinalizedException_WhenBufferIsntFinalized()
        {
            var invalidBuffer = CreateInvalidBuffer();
            Assert.Throws<InvalidOperationException>(() => Buffer.GetBuffer(invalidBuffer));
        }
        
        [Fact]
        public void GetBuffer_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.GetBuffer(null));
        }
        
        [Fact]
        public void Add_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.Add(null, 4));
        }
        
        [Fact]
        public void Add_ThrowsBufferFinalizedException_WhenBufferIsFinalized()
        {
            var testBuffer = CreateValidBuffer();
            Assert.Throws<InvalidOperationException>(() => Buffer.Add(testBuffer, 1));
        }
        
        [Fact]
        public void ClearBuffer_ThrowsArgumentNullException_WhenBufferIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Buffer.ClearBuffer(null));
        }
        
        [Fact]
        public void FinalizeBuffer_DoesntThrowBufferFinalizedException_IfBufferIsAlreadyFinalized()
        {
            var testBuffer = CreateValidBuffer();
            Buffer.FinalizeBuffer(testBuffer);
        }
        
        [Fact]
        public void Duplicate_ReturnsADifferentButCorrectlyDuplicatedBuffer()
        {
            var testBuffer = CreateValidBuffer();
            var duplicateBuffer = Buffer.Duplicate(testBuffer);
        
            Assert.NotSame(testBuffer, duplicateBuffer);     //This checks to see that there isn't any REFERENCE equality
            Assert.True(testBuffer.Equals(duplicateBuffer));  //This check to see if there is VALUE equality
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
            var tempBuffer = Buffer.New(14);
            Buffer.Add(tempBuffer, 12);
            Buffer.Add(tempBuffer, 32.0);
            Buffer.Add(tempBuffer, 'c');
            return tempBuffer;
        }
    }
}