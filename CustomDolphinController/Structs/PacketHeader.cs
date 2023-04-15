using CustomDolphinController.Enums;

namespace CustomDolphinController.Structs
{
    public struct PacketHeader
    {
        public string MagicString;//4 bytes
        public uint ProtocolVersion;//2 bytes
        public uint PacketLength;//2 bytes //THIS WILL BE WRITTEN AFTER THE PACKET IS PROCCESSED
        public uint CRC32;//4 bytes //THIS WILL BE WRITTEN AFTER THE PACKET IS PROCCESSED
        public uint ClientId;//4 bytes
        public MessageType MessageType;//4 bytes


  
        
        public override string ToString()
        {
            return $"MagicString: {MagicString}, ProtocolVersion: {ProtocolVersion}, PacketLength: {PacketLength}, CRC32: {CRC32}, ClientId: {ClientId}, MessageType: {MessageType}";
        }
    }
}