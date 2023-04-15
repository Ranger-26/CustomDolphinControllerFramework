using System;
using CustomDolphinController.Enums;
using CustomDolphinController.Helpers;
using CustomDolphinController.Structs;
using NUnit.Framework;

namespace CustomDolphinController.Tests
{
    public class WriteVersionTest
    {
        private byte[] _buffer;
        
        [SetUp]
        public void Setup()
        {
            PacketHeader _header = new PacketHeader();
            _header.MagicString = "DSUS";
            _header.ProtocolVersion = 1010;
            _header.ClientId = 2299867765;
            _header.MessageType = MessageType.ProtocolVersionInfo;

            ushort version = 1010;
            _buffer = PacketHelpers.CreatePacket(_header, BitConverter.GetBytes(version));
        }

        [Test]
        public void PacketLengthTest()
        {
            PacketHeader _newHeader = PacketHelpers.FromByteArray(_buffer);
            Assert.AreEqual(6, _newHeader.PacketLength);
        }

        [Test]
        public void PacketCrc32Test()
        {
            PacketHeader _newHeader = PacketHelpers.FromByteArray(_buffer);
            uint crc32 = _newHeader.CRC32;
            _newHeader.CRC32 = 0;

            byte[] newBuffer = new byte[22];
            Array.Copy(_newHeader.ToByteArray(), 0, newBuffer, 0, 20);
            Array.Copy(BitConverter.GetBytes((ushort)1010), 0, newBuffer, 20, 2);
            
            Assert.AreEqual(newBuffer.ComputeCrc32(), crc32);
        }
    }
}