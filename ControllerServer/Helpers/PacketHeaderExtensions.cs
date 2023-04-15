using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ControllerServer.Helpers
{
    public static class PacketHeaderExtensions
    {
        public static PacketHeader FromByteArray(byte[] buffer)
        {
            PacketHeader header = new PacketHeader();

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    header.MagicString = new string(reader.ReadChars(4));
                    header.ProtocolVersion = reader.ReadUInt16();
                    header.PacketLength = reader.ReadUInt16();
                    header.TotalPacketLength = reader.ReadUInt32();
                    header.ClientId = reader.ReadUInt32();
                    header.MessageType = (MessageType)reader.ReadUInt32();
                }
            }

            // Reverse byte order if necessary
            if (BitConverter.IsLittleEndian)
            {
                header.ProtocolVersion = SwapEndian(header.ProtocolVersion);
                header.PacketLength = SwapEndian(header.PacketLength);
                header.TotalPacketLength = SwapEndian(header.TotalPacketLength);
                header.ClientId = SwapEndian(header.ClientId);
                header.MessageType = (MessageType)SwapEndian((uint)header.MessageType);
            }

            return header;
        }

        public static byte[] ToByteArray(this PacketHeader header)
        {
            int size = Marshal.SizeOf(header);
            byte[] buffer = new byte[size];

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(header.MagicString.ToCharArray());
                    writer.Write(header.ProtocolVersion);
                    writer.Write(header.PacketLength);
                    writer.Write(header.TotalPacketLength);
                    writer.Write(header.ClientId);
                    writer.Write((uint)header.MessageType);
                }
            }

            // Reverse byte order if necessary
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer, 4, 2);  // ProtocolVersion
                Array.Reverse(buffer, 6, 2);  // PacketLength
                Array.Reverse(buffer, 10, 4); // TotalPacketLength
                Array.Reverse(buffer, 14, 4); // ClientId
                Array.Reverse(buffer, 18, 4); // MessageType
            }

            return buffer;
        }

        
        private static uint SwapEndian(uint value)
        {
            return (value << 24) |
                   ((value << 8) & 0x00FF0000) |
                   ((value >> 8) & 0x0000FF00) |
                   (value >> 24);
        }

        private static ushort SwapEndian(ushort value)
        {
            return (ushort)((value << 8) | (value >> 8));
        }
    }

}