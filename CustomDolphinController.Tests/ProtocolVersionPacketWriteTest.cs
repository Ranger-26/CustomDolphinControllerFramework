using CustomDolphinController.Enums;
using CustomDolphinController.Structs;
using NUnit.Framework;

namespace ControllerServer.Tests
{
    public class ProtocolVersionPacketWriteTest
    {
        private PacketHeader _header;
        
        [SetUp]
        public void Setup()
        {
            _header = new PacketHeader();
            _header.MagicString = "";
            _header.ProtocolVersion = 1010;
            _header.MessageType = MessageType.ProtocolVersionInfo;
            
        }

        [Test]
        public void HeaderTest()
        {
            
            Assert.Pass();
        }
    }
}