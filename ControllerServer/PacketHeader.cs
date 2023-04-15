using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerServer
{
    public struct PacketHeader
    {
        public string MagicString;//4 bytes
        public uint ProtocolVersion;//2 bytes
        public uint PacketLength;//2 bytes
        public uint TotalPacketLength;//4 bytes
        public uint ClientId;//4 bytes
        public MessageType MessageType;//4 bytes
        
        public PacketHeader(byte[] headerBytes)
        {
            //magic string
            byte[] magicStringBytes = new byte[4];
            Array.Copy(headerBytes, 0, magicStringBytes, 0, 4);
            MagicString = Convert.ToString(BitConverter.ToString(magicStringBytes));
            Console.Write($"Magic String: {MagicString} |");
            //version
            byte[] versionBytes = new byte[2];
            Array.Copy(headerBytes, 4, versionBytes, 0, 2);
            ProtocolVersion = BitConverter.ToUInt16(versionBytes);
            Console.Write($"Version: {ProtocolVersion} |");
            //Packet Length
            byte[] packetLengthBytes = new byte[2];
            Array.Copy(headerBytes, 6, packetLengthBytes, 0, 2);
            PacketLength = BitConverter.ToUInt16(packetLengthBytes);
            Console.Write($"Packet Length: {PacketLength} |");
            //total Length
            byte[] totalPacketLengthBytes = new byte[4];
            Array.Copy(headerBytes, 8, totalPacketLengthBytes, 0, 4);
            TotalPacketLength = BitConverter.ToUInt32(totalPacketLengthBytes);
            Console.Write($"Total Packet Length: {TotalPacketLength} |");
            //client id
            byte[] clientIdBytes = new byte[4];
            Array.Copy(headerBytes, 12, clientIdBytes, 0, 4);
            ClientId = BitConverter.ToUInt32(clientIdBytes);
            Console.Write($"ClientId: {ClientId} |");
            //message type
            byte[] messageTypeBytes = new byte[4];
            Array.Copy(headerBytes, 16, messageTypeBytes, 0, 4);
            MessageType = (MessageType)BitConverter.ToUInt32(messageTypeBytes);
            Console.Write($"MessageType: {MessageType} |");
            Console.WriteLine();
        }

        public override string ToString()
        {
            return $"MagicString: {MagicString}, ProtocolVersion: {ProtocolVersion}, PacketLength: {PacketLength}, TotalPacketLength: {TotalPacketLength}, ClientId: {ClientId}, MessageType: {MessageType}";
        }
    }
}