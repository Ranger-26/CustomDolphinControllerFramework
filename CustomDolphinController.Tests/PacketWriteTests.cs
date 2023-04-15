using System;
using CustomDolphinController.Enums;
using CustomDolphinController.Helpers;
using CustomDolphinController.Structs;
using NUnit.Framework;

namespace CustomDolphinController.Tests
{
    public class PacketWriteTests
    {
        private PacketHeader _header;
        private byte[] _buffer;
        
        [SetUp]
        public void Setup()
        {
            _header = new PacketHeader();
            _header.MagicString = "DSUS";
            _header.ProtocolVersion = 1010;
            _header.ClientId = 2299867765;
            _header.MessageType = MessageType.ProtocolVersionInfo;

            _buffer = _header.ToByteArray();
        }

        [Test]
        public void HeaderTest()
        {
            PacketHeader newHeader = PacketHelpers.FromByteArray(_buffer);
            Assert.AreEqual(newHeader, _header);
        }

    }
}