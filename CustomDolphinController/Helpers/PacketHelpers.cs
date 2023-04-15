using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Helpers
{
    public static class PacketHelpers
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
                    header.CRC32 = reader.ReadUInt32();
                    header.ClientId = reader.ReadUInt32();
                    header.MessageType = (MessageType)reader.ReadUInt32();
                }
            }

            // Reverse byte order if necessary
            
            if (!BitConverter.IsLittleEndian)
            {
                header.ProtocolVersion = OperationsHelper.SwapEndian(header.ProtocolVersion);
                header.PacketLength = OperationsHelper.SwapEndian(header.PacketLength);
                header.CRC32 = OperationsHelper.SwapEndian(header.CRC32);
                header.ClientId = OperationsHelper.SwapEndian(header.ClientId);
                header.MessageType = (MessageType)OperationsHelper.SwapEndian((uint)header.MessageType);
            }
            
            return header;
        }
        
        public static byte[] ToByteArray(this PacketHeader header)
        {
            byte[] buffer = new byte[20];

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(header.MagicString.ToCharArray());//4 bytes
                    writer.Write(header.ProtocolVersion);//2 bytes
                    writer.Write((uint)0);//packet length will be manually added //4 bytes
                    writer.Write((uint)0);//crc32 will be manually added //4 bytes
                    writer.Write(header.ClientId);
                    writer.Write((uint)header.MessageType);
                }
            }

            // Reverse byte order if necessary
            // check this 
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer, 4, 2);  // ProtocolVersion
                Array.Reverse(buffer, 6, 2);  // PacketLength
                Array.Reverse(buffer, 10, 4); // TotalPacketLength
                Array.Reverse(buffer, 14, 4); // ClientId
                Array.Reverse(buffer, 18, 4); // MessageType
            }

            return buffer;
        }

        public static byte[] CreatePacket(PacketHeader header, byte[] extraData)
        {
            byte[] buffer = new byte[20+extraData.Length];
            //copy the header into the buffer
            Array.Copy(header.ToByteArray(), 0, buffer, 0, 20);
            //copy the additional data into the buffer
            Array.Copy(extraData, 0, buffer, 20, extraData.Length);
            //compute the crc32 checksum and append it to the buffer
            uint crc32 = buffer.ComputeCrc32();
            Array.Copy(BitConverter.GetBytes(crc32), 0, buffer, 8, 4);
            return buffer;
        }
    }

}