using AwesomeSockets.Domain.Exceptions;
using AwesomeSockets.Domain.SocketModifiers;
using AwesomeSockets.Domain.Sockets;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AwesomeSockets.Tests.Domain.SocketModifiers
{
    [TestFixture]
    public class MulticastSocketModifierTests
    {
        private MulticastSocketModifier CreateTestModel()
        {
            return new MulticastSocketModifier();
        }

        //[Test]
        //public void Apply_CorrectlyAppliesTheMulticastOptions_WhenSocketDetailsAreCorrect()
        //{
            //var testModel = CreateTestModel();
            //Mock<ISocket> mockSocket = new Mock<ISocket>();
            //Socket actualSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //mockSocket.Setup(x => x.GetSocket()).Returns(actualSocket);

            //testModel.Apply(mockSocket.Object, "224.0.0.1", "14567", "2");

            //var actual = actualSocket.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface);
        //}

        [Test]
        [ExpectedException(typeof(CannotMulticastException))]
        public void Apply_ThrowsAnException_WhenSocketIsNotUdp()
        {
            var testModel = CreateTestModel();
            Mock<ISocket> mockSocket = new Mock<ISocket>();
            Socket actualSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mockSocket.Setup(x => x.GetSocket()).Returns(actualSocket);

            testModel.Apply(mockSocket.Object, "224.0.0.1", "14567", "2");
        }

        [Test]
        [ExpectedException(typeof(CannotMulticastException))]
        public void Apply_ThrowsAnException_WhenPortParamIsInvalid()
        {
            var testModel = CreateTestModel();
            Mock<ISocket> mockSocket = new Mock<ISocket>();
            Socket actualSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mockSocket.Setup(x => x.GetSocket()).Returns(actualSocket);

            testModel.Apply(mockSocket.Object, "224.0.0.1", "invalid", "2");
        }

        [Test]
        [ExpectedException(typeof(CannotMulticastException))]
        public void Apply_ThrowsAnException_WhenTtlParamIsInvalid()
        {
            var testModel = CreateTestModel();
            Mock<ISocket> mockSocket = new Mock<ISocket>();
            Socket actualSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mockSocket.Setup(x => x.GetSocket()).Returns(actualSocket);

            testModel.Apply(mockSocket.Object, "224.0.0.1", "14567", "invalid");
        }
    }
}
