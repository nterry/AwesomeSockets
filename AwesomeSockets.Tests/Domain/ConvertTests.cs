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
            var expectedFloat = new byte[] { 0, 64, 58, 71 };
            var expectedDouble = new byte[] { 103, 3, 166, 185, 64, 31, 18, 60 };

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
            var actualFloat = Convert.ToBytes((float)4.768e4);
            var actualDouble = Convert.ToBytes(2456e-22);
            
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
            Assert.AreEqual(expectedFloat, actualFloat);
            Assert.AreEqual(expectedDouble, actualDouble);
        }

        [Test]
        public void GetFloat_ReturnsCorrectFloat_WhenPowerIsHigh()
        {
            const float expectedFloat = (float) 4e-32;
            const float epsilon = (float) 1e-36;

            var actualFloat = Convert.Get<float>(Convert.ToBytes(expectedFloat));

            Assert.LessOrEqual(expectedFloat - actualFloat, epsilon); 
        }

        [Test]
        public void GetFloat_ReturnsCorrectFloat_WhenPowerIsLow()
        {
            const float expectedFloat = (float)4e4;
            const float epsilon = 1.0F;

            var actualFloat = Convert.Get<float>(Convert.ToBytes(expectedFloat));

            Assert.LessOrEqual(expectedFloat - actualFloat, epsilon);
        }

        [Test]
        public void GetDouble_ReturnsCorrectDouble_WhenPowerIsHigh()
        {
            const double expectedDouble = 4e64;
            const double epsilon = 1e-36;

            var actualDouble = Convert.Get<double>(Convert.ToBytes(expectedDouble));

            Assert.LessOrEqual(expectedDouble - actualDouble, epsilon);
        }

        [Test]
        public void GetDouble_ReturnsCorrectDouble_WhenPowerIsLow()
        {
            const double expectedDouble = 4e-2;
            const double epsilon = 1e-6;

            var actualDouble = Convert.Get<double>(Convert.ToBytes(expectedDouble));

            Assert.LessOrEqual(expectedDouble - actualDouble, epsilon);
        }
    }
}
