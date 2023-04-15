using System;
using System.Runtime.InteropServices;
using System.Text;
using CustomDolphinController.Enums;
using CustomDolphinController.Structs;

namespace CustomDolphinController.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            PacketHeader _header = new PacketHeader();
            _header.MagicString = "DSUS";
            _header.ProtocolVersion = 1010;
            _header.ClientId = 2299867765;
            _header.MessageType = MessageType.ProtocolVersionInfo;
            Console.WriteLine($"Magic String size: {Encoding.UTF8.GetByteCount(_header.MagicString)}");
            Console.WriteLine($"Protocol Version size: {Marshal.SizeOf(_header.ProtocolVersion)}");
            Console.WriteLine($"Packet Length: {Marshal.SizeOf(_header.PacketLength)}");
            Console.WriteLine($"CRC32 size: {Marshal.SizeOf(_header.CRC32)}");
            Console.WriteLine($"Client id size: {Marshal.SizeOf(_header.ClientId)}");
            Console.WriteLine($"Message typee size: {Marshal.SizeOf((uint)_header.MessageType)}");
        }
    }
}