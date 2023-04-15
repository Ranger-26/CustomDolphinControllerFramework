using System;
using CustomDolphinController.Enums;

namespace CustomDolphinController.Structs
{
    public struct PacketHeader : IEquatable<PacketHeader>
    {
        public string MagicString;//4 bytes
        public ushort ProtocolVersion;//2 bytes
        public ushort PacketLength;//2 bytes //THIS WILL BE WRITTEN AFTER THE PACKET IS PROCCESSED
        public uint CRC32;//4 bytes //THIS WILL BE WRITTEN AFTER THE PACKET IS PROCCESSED
        public uint ClientId;//4 bytes
        public MessageType MessageType;//4 bytes

        
        public override string ToString()
        {
            return $"MagicString: {MagicString}, ProtocolVersion: {ProtocolVersion}, PacketLength: {PacketLength}, CRC32: {CRC32}, ClientId: {ClientId}, MessageType: {MessageType}";
        }

        public bool Equals(PacketHeader other)
        {
            return MagicString == other.MagicString && ProtocolVersion == other.ProtocolVersion && PacketLength == other.PacketLength && CRC32 == other.CRC32 && ClientId == other.ClientId && MessageType == other.MessageType;
        }

        public override bool Equals(object obj)
        {
            return obj is PacketHeader other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MagicString, ProtocolVersion, PacketLength, CRC32, ClientId, (int) MessageType);
        }
    }
}