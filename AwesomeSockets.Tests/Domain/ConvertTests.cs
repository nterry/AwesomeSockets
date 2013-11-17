using NUnit.Framework;
using Convert = AwesomeSockets.Domain.Convert;

namespace AwesomeSockets.Tests.Domain
{
    [TestFixture]
    public class ConvertTests
    {
        [Test]
        public void ToBytes_ReturrnsAppropriatelyConvertedValue()
        {
            var expectedSByte = new byte[] { 165 };
            var expectedByte = new byte[] { 45 };
            var expectedChar = new byte[] { 99, 0 };
            var expectedBool = new byte[] { 1 };
            var expectedInt = new byte[] { 89, 34, 255, 255 };
            var expectedUInt = new byte[] { 78, 97, 188, 0 };
            var expectedShort = new byte[] { 184, 17 };
            var expectedUShort = new byte[] { 145, 30 };
            var expectedLong = new byte[] { 184, 53, 185, 194, 219, 6, 0, 0 };
            var expectedULong = new byte[] { 15, 246, 250, 107, 91, 163, 32, 109 };

            var actualSByte = Convert.ToBytes((sbyte) -91);
            var actualByte = Convert.ToBytes((byte) 45);
            var actualChar = Convert.ToBytes('c');
            var actualBool = Convert.ToBytes(true);
            var actualInt = Convert.ToBytes(-56743);
            var actualUInt = Convert.ToBytes((uint) 12345678);
            var actualShort = Convert.ToBytes((short) 4536);
            var actualUShort = Convert.ToBytes((ushort) 7825);
            var actualLong = Convert.ToBytes(7540934522296);
            var actualULong = Convert.ToBytes((ulong) 7863464562437846543);

            Assert.AreEqual(expectedSByte, actualSByte);
            Assert.AreEqual(expectedByte, actualByte);
            Assert.AreEqual(expectedChar, actualChar);
            Assert.AreEqual(expectedBool,actualBool);
            Assert.AreEqual(expectedInt, actualInt);
            Assert.AreEqual(expectedUInt, actualUInt);
            Assert.AreEqual(expectedShort, actualShort);
            Assert.AreEqual(expectedUShort, actualUShort);
            Assert.AreEqual(expectedLong, actualLong);
            Assert.AreEqual(expectedULong, actualULong);
        }
    }
}
