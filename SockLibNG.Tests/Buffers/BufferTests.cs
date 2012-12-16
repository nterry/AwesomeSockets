using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Buffer = SockLibNG.Buffers.Buffer;

namespace SockLibNG.Tests.Buffers
{
    [TestFixture]
    class BufferTests
    {
        private Buffer testBuffer;

        [TestFixtureSetUp]
        public void Setup()
        {
            testBuffer = Buffer.New();
            Buffer.Add(testBuffer, 12);
            Buffer.Add(testBuffer, 32.0);
            Buffer.Add(testBuffer, "Foo is super fun and stuff");
        }
    }
}
